using System;

namespace CSharpForFinancialMarkets
{
    /// <summary>
    /// Best of 2 options
    /// </summary>
    public class WorstofTwoStrategy : MultiAssetPayoffStrategy
    {
        /// <summary>
        /// Strike
        /// </summary>
        public double K { get; set; }       

        /// <summary>
        /// +1 call, -1 put
        /// </summary>
        public int w { get; set; }         

        // All public classes need default ructor
        public WorstofTwoStrategy()
        {
            K = 95.0;
            w = +1;
        }

        public WorstofTwoStrategy(double strike, int cp)
        {
            K = strike;
            w = cp;
        }

        public override double Payoff(double S1, double S2)
        {
            double min = Math.Min(S1, S2);
            return Math.Max(w * (min - K), 0.0);
        }
    }
}