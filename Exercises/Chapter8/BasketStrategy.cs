using System;

namespace CSharpForFinancialMarkets
{
    /// <summary>
    /// 2-asset basket option payoff
    /// </summary>
    public class BasketStrategy : MultiAssetPayoffStrategy
    {
        /// <summary>
        /// Strike
        /// </summary>
        public double K { get; set; }

        /// <summary>
        ///  +1 call, -1 put
        /// </summary>
        public int w { get; set; }

        /// <summary>
        /// w1 + w2 = 1
        /// </summary>
        public double w1 { get; set; }

        /// <summary>
        /// w1 + w2 = 1
        /// </summary>
        public double w2 { get; set; } 

        // All public classes need default ructor
        public BasketStrategy()
        {
            K = 95.0; w = +1; w1 = 0.5; w2 = 0.5;
        }

        public BasketStrategy(double strike, int cp, double weight1, double weight2)
        {
            K = strike;
            w = cp;
            w1 = weight1;
            w2 = weight2;
        }

        public override double Payoff(double S1, double S2)
        {
            double sum = w1 * S1 + w2 * S2;
            return Math.Max(w * (sum - K), 0.0);
        }
    }
}