using System;
using System.Collections.Generic;
using System.Text;
using Library.Data;

namespace Library.Binominal
{
    public class BinomialMethod
    { 
        // Simple model for GBM

        // Underlying data structure
        private readonly BinomialLatticeStrategy _strategy;         // Reference to an algorithm

        // The possibility to define constraints on top of the European 
        // option solution, e.g. early exercise, barriers
        public delegate double ConstraintMethod(double price, double S);

        private readonly ConstraintMethod _con;
        private readonly bool _constraintExists;
        private readonly double _disc;

        public BinomialMethod(double discounting, BinomialLatticeStrategy strategy, int N)
        {
            _disc = discounting;
            _strategy = strategy;

            BuildLattice(N);

            _constraintExists = false;
        }

        public BinomialMethod(double discounting, BinomialLatticeStrategy strategy, int N, ConstraintMethod constraint)
        {
            _disc = discounting;
            _strategy = strategy;

            BuildLattice(N);

            _con = constraint;

            _constraintExists = true;
        }

        private void BuildLattice(int N)
        { 
            // Build a binomial lattice
            const double val = 0.0;
            GetOptionLattice = new Lattice<double>(N, 2, val);
            GetAssetLattice = new Lattice<double>(N, 2, val);
        }

        public void ModifyLattice(double U)
        { 
            // Forward induction; building the tree 
            var down = _strategy.DownValue;
            var up = _strategy.UpValue;

            var si = GetOptionLattice.MinIndex;

            GetOptionLattice[si, si] = U;
            GetAssetLattice[si, si] = U;

            // Loop from the min index to the end index
            for (var n = GetOptionLattice.MinIndex + 1; n <= GetOptionLattice.MaxIndex; n++)
            {
                for (var i = 0; i < GetOptionLattice.NumberColumns(n) - 1; i++)
                {
                    GetOptionLattice[n, i] = down * GetOptionLattice[n - 1, i];
                    GetOptionLattice[n, i + 1] = up * GetOptionLattice[n - 1, i];

                    GetAssetLattice[n, i] = down * GetAssetLattice[n - 1, i];
                    GetAssetLattice[n, i + 1] = up * GetAssetLattice[n - 1, i];
                }
            }

            // Postcondition: we now have the complete lattice for the underlying asset
        }

        public double GetPrice(Vector<double> RHS)
        { 
            // Backward induction; calculate the price based on discete payoff function at t = T
            var pr = _strategy.ProbValue;

            // Initialise the vector at the expiry date/MaxIndex
            var ei = GetOptionLattice.MaxIndex;

            // Exception handling: sizes of RHS and base vector must be the same
            for (var i = 0; i < GetOptionLattice.NumberColumns(ei); i++)
            {
                GetOptionLattice[ei, i] = RHS[i];
            }

            double S;   // Value at node [n,i] before it gets overwritten

            // Loop from the max index to the start (min) index
            for (var n = GetOptionLattice.MaxIndex - 1; n >= GetOptionLattice.MinIndex; n--)
            {
                for (var i = 0; i < GetOptionLattice.NumberColumns(n); i++)
                {
                    S = GetOptionLattice[n, i];
                    GetOptionLattice[n, i] = _disc * (pr * GetOptionLattice[n + 1, i + 1] + (1.0 - pr) * GetOptionLattice[n + 1, i]);

                    // Now take early exercise into account
                    if (_constraintExists)
                    {
                        GetOptionLattice[n, i] = _con(GetOptionLattice[n, i], S);
                    }
                }
            }

            var si = GetOptionLattice.MinIndex;
            return GetOptionLattice[si, si];
        }

        public Vector<double> BasePyramidVector()
        {
            return GetOptionLattice.BasePyramidVector();
        }

        // Underlying lattice
        public Lattice<double> GetOptionLattice { get; private set; }

        public Lattice<double> GetAssetLattice { get; private set; }
    }
}
