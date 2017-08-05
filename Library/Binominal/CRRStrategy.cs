using System;

namespace Library.Binominal
{
    public class CRRStrategy : BinomialLatticeStrategy
    {
        public CRRStrategy(double vol, double interest, double delta)
            : base(vol, interest, delta)
        {
            /* double e = Math.Exp((r)*k);
            double sr = Math.Sqrt(exp(vol*vol*k) - 1.0);
            u = e * (1.0 + sr);
            d = e * (1.0 - sr);*/

            var R1 = (r - 0.5 * s * s) * k;
            var R2 = s * Math.Sqrt(k);

            u = Math.Exp(R1 + R2);
            d = Math.Exp(R1 - R2);

            var discounting = Math.Exp(-r * k);

            p = 0.5;
        }
    }
}