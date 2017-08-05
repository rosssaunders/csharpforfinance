using System;

namespace Library.Binominal
{
    public class EQPStrategy : BinomialLatticeStrategy
    {
        public EQPStrategy(double vol, double interest, double delta)
            : base(vol, interest, delta)
        {
            BinomialType = BinomialType.Additive;

            // Needed for additive method: page 19 Clewlow/Strickland formula 2.17 
            // "v" is "nu" here, for "v" see  page 19 formula 2.14
            var nu = r - 0.5 * s * s;

            var a = nu * k;
            var b = 0.5 * Math.Sqrt(4.0 * s * s * k - 3.0 * nu * nu * k * k);

            // EQP parameters: page 19 formula 2.17
            u = 0.5 * a + b;
            d = 1.5 * a - b;

            p = 0.5;
        }
    }
}