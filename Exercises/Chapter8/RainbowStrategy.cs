using System;

namespace CSharpForFinancialMarkets
{
    public class RainbowStrategy : MultiAssetPayoffStrategy
    {
        /// <summary>
        /// Strike
        /// </summary>
        public double K { get; set; }

        /// <summary>
        /// +1 call, -1 put
        /// </summary>
        public int w { get; set; }

        /// <summary>
        /// Comparisons.Max (1) or Comparisons.Min (!1) of 2 assets
        /// </summary>
        public int type { get; set; }

        public RainbowStrategy(int cp, double strike, int DMinDMax)
        {
            K = strike;
            w = cp;
            type = DMinDMax;
        }

        public override double Payoff(double S1, double S2)
        {
            if (type == 1)  // Comparisons.Max
                return Math.Max(w * Math.Max(S1, S2) - w * K, 0.0);

            return Math.Max(w * Math.Min(S1, S2) - w * K, 0.0);
        }
    }
}