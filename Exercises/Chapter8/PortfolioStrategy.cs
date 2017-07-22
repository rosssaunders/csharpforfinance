using System;

namespace CSharpForFinancialMarkets
{
    /// <summary>
    /// Portfolio option
    /// </summary>
    public class PortfolioStrategy : MultiAssetPayoffStrategy
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
        /// quantities of each underlying
        /// </summary>
        public double n1 { get; set; }

        /// <summary>
        /// quantities of each underlying
        /// </summary>
        public double n2 { get; set; }              

        // All public classes need default ructor
        public PortfolioStrategy()
        {
            K = 95.0;
            w = +1;
            n1 = n2 = 1;
        }

        public PortfolioStrategy(int N1, int N2, double strike, int cp)
        {
            K = strike; w = cp;
            n1 = N1; n2 = N2;
        }

        public override double Payoff(double S1, double S2)
        {
            double min = Math.Min(S1, S2);
            return Math.Max(w * (n1 * S1 + n2 * S2 - K), 0.0);
        }
    }
}