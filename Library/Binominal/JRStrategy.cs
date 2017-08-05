using System;

namespace Library.Binominal
{
    public class JRStrategy : BinomialLatticeStrategy
    {
        public JRStrategy(double vol, double interest, double delta)
            : base(vol, interest, delta)
        {
            var k2 = Math.Sqrt(k);
            u = Math.Exp(s * k2);

            d = 1.0 / u;

            p = 0.5 + (r - 0.5 * s * s) * k2 * 0.5 / s;
        }
    }
}