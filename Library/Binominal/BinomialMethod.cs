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
        private Lattice<double> _lattice;                           // Magic number == 2 means binomial
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

            double val = 0.0;
            _lattice = new Lattice<double>(N, 2, val);
        }

        public void ModifyLattice(double U)
        { // Forward induction; building the tree 

            double down = _strategy.DownValue;
            double up = _strategy.UpValue;

            int si = _lattice.MinIndex;
            _lattice[si, si] = U;

            // Loop from the min index to the end index
            for (int n = _lattice.MinIndex + 1; n <= _lattice.MaxIndex; n++)
            {
                for (int i = 0; i < _lattice.NumberColumns(n) - 1; i++)
                {
                    _lattice[n, i] = down * _lattice[n - 1, i];
                    _lattice[n, i + 1] = up * _lattice[n - 1, i];
                }
            }

            // Postcondition: we now have the complete lattice for the underlying asset
        }

        public double GetPrice(Vector<double> RHS)
        { 
            // Backward induction; calculate the price based on discete payoff function at t = T
            double pr = _strategy.ProbValue;

            // Initialise the vector at the expiry date/MaxIndex
            int ei = _lattice.MaxIndex;


            // Exception handling: sizes of RHS and base vector must be the same
            for (int i = 0; i < _lattice.NumberColumns(ei); i++)
            {
                _lattice[ei, i] = RHS[i];
            }

            double S;   // Value at node [n,i] before it gets overwritten
            // Loop from the max index to the start (min) index
            for (int n = _lattice.MaxIndex - 1; n >= _lattice.MinIndex; n--)
            {
                for (int i = 0; i < _lattice.NumberColumns(n); i++)
                {
                    S = _lattice[n, i];
                    _lattice[n, i] = _disc * (pr * _lattice[n + 1, i + 1] + (1.0 - pr) * _lattice[n + 1, i]);

                    // Now take early exercise into account
                    if (_constraintExists)
                    {
                        _lattice[n, i] = _con(_lattice[n, i], S);

                    }
                }
            }

            int si = _lattice.MinIndex;
            return _lattice[si, si];
        }

        public Vector<double> BasePyramidVector()
        {
            return _lattice.BasePyramidVector();
        }

        // Underlying lattice
        public Lattice<double> GetLattice => this._lattice;
    }
}
