using System;

namespace Library.Binominal
{
    public class TRGStrategy : BinomialLatticeStrategy
    {
        public TRGStrategy(double vol, double interest, double delta)
            : base(vol, interest, delta)
        {
            BinomialType = BinomialType.Additive;

            // Needed for additive method: page 19 formula 2.19 
            // "v" is "nu" here, for "v" see  page 17 formula 2.14
            var nu = r - 0.5 * s * s;

            var nudt = nu * k;

            // TRG parameters: page 19 formula 2.19
            u = Math.Sqrt(s * s * k + nudt * nudt);
            d = -u;

            p = 0.5 * (1.0 + nudt / u);
        }
    }
}