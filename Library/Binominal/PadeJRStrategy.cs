using System;

namespace Library.Binominal
{
    public class PadeJRStrategy : BinomialLatticeStrategy
    {
        public PadeJRStrategy(double vol, double interest, double delta)
            : base(vol, interest, delta)
        {
            var k2 = Math.Sqrt(k);

            // Cayley transform
            var z = s * Math.Sqrt(k);

            var num = 12.0 - 6.0 * z + z * z;
            var denom = 12.0 + 6.0 * z + z * z;

            d = num / denom;
            u = denom / num;

            p = 0.5 + (r - 0.5 * s * s) * k2 * 0.5 / s;
        }
    }
}