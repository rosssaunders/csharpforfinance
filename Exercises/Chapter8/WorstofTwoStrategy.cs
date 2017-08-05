using System;

namespace CSharpForFinancialMarkets
{
    /// <summary>
    /// Best of 2 options
    /// </summary>
    public class WorstOfTwoStrategy : MultiAssetPayoffStrategy
    {
        /// <summary>
        /// Strike
        /// </summary>
        public double K { get; set; }       

        /// <summary>
        /// +1 call, -1 put
        /// </summary>
        public int W { get; set; }         

        // All public classes need default ructor
        public WorstOfTwoStrategy()
        {
            K = 95.0;
            W = +1;
        }

        public WorstOfTwoStrategy(double strike, int cp)
        {
            K = strike;
            W = cp;
        }

        public override double Payoff(double S1, double S2)
        {
            var min = Math.Min(S1, S2);
            return Math.Max(W * (min - K), 0.0);
        }
    }
}