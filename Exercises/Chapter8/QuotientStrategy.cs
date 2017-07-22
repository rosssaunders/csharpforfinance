using System;

namespace CSharpForFinancialMarkets
{
    public class QuotientStrategy : MultiAssetPayoffStrategy
    {
        /// <summary>
        /// Strike
        /// </summary>
        public double K { get; set; }

        /// <summary>
        /// +1 call, -1 put
        /// </summary>
        public int w { get; set; }

        public QuotientStrategy()
        {

        }

        public QuotientStrategy(double strike, int cp)
        {
            K = strike;
            w = cp;
        }

        public override double Payoff(double S1, double S2)
        {
            return Math.Max(w * (S1 / S2) - w * K, 0.0);
        }
    }
}