using System;
using static System.Math;

namespace Library.Binominal
{
    public class LeisenReimerStrategy : BinomialLatticeStrategy
    {
        public LeisenReimerStrategy(double n, double S, double K, double T, double b, double vol)
            : base(0, 0, 0)
        {
            var tmp = vol * Sqrt(T);
            var d1 = (Log(S / K) + (b + vol * vol / 2) * T) / tmp;
            var d2 = d1 - tmp;

            var dT = T / n;

            p = H(d2, n); 
            u = Exp(b * dT) * (H(d1, n) / H(d2, n));
            d = (Exp(b * dT) - p * u) / (1 - p); 
        }

        /// <summary>
        /// Peizer-Pratt Inversion
        /// </summary>
        /// <param name="z"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        private static double H(double z, double n)
        {
            const double oneThird = 1d / 3d;
            const double oneSixth = 1d / 6d;

            return 0.5 + Sig(z) * 0.5 * Sqrt(1 - Exp(-Pow(z / (n + oneThird), 2) * (n + oneSixth)));
        }

        private static double Sig(double z)
        {
            return z >= 0 ? 1 : -1;
        }
    }
}