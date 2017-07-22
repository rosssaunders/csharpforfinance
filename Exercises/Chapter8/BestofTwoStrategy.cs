using System;

namespace CSharpForFinancialMarkets
{
    /// <summary>
    /// Best of 2 options
    /// </summary>
    public class BestofTwoStrategy : MultiAssetPayoffStrategy
    {
        /// <summary>
        /// Strike
        /// </summary>
        public double K { get; set; }

        /// <summary>
        /// +1 call, -1 put
        /// </summary>
        public int w { get; set; }      

        public BestofTwoStrategy()
        {
            K = 95.0;
            w = +1;
        }

        public BestofTwoStrategy(double strike, int cp)
        {
            K = strike; w = cp;
        }

        public override double Payoff(double S1, double S2)
        {
            double max = Math.Max(S1, S2);
            return Math.Max(w * (max - K), 0.0);
        }
    }
}