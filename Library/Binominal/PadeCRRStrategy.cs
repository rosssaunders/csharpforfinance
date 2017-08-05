using System;

namespace Library.Binominal
{
    public class PadeCRRStrategy : BinomialLatticeStrategy
    {
        public PadeCRRStrategy(double vol, double interest, double delta)
            : base(vol, interest, delta)
        {
            var R1 = (r - 0.5 * s * s) * k;
            var R2 = s * Math.Sqrt(k);

            // Cayley transform
            var z1 = R1 + R2;
            var z2 = R1 - R2;

            u = (2.0 + z1) / (2.0 - z1);
            d = (2.0 + z2) / (2.0 - z2);

            p = 0.5;
        }
    }
}