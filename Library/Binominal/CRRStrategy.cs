using System;
using static System.Math;

namespace Library.Binominal
{
    public class CRRStrategy : BinomialLatticeStrategy
    {
        public CRRStrategy(double vol, double interest, double delta)
            : base(vol, interest, delta)
        {
            var R1 = (r - 0.5 * s * s) * k;
            var R2 = s * Sqrt(k);

            u = Exp(R1 + R2);
            d = Exp(R1 - R2);

            p = 0.5;
        }
    }
}