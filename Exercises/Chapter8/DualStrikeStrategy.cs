using System;

namespace CSharpForFinancialMarkets
{
    public class DualStrikeStrategy : MultiAssetPayoffStrategy
    {
        public double K1 { get; set; }

        public double K2 { get; set; }

        public int w1 { get; set; }

        /// <summary>
        /// calls or puts
        /// </summary>
        public int w2 { get; set; } 

        public DualStrikeStrategy()
        {

        }

        public DualStrikeStrategy(double strike1, double strike2, int cp1, int cp2)
        {
            K1 = strike1;
            K2 = strike2;
            w1 = cp1;
            w2 = cp2;
        }

        public override double Payoff(double S1, double S2)
        {
            return Math.Max(Math.Max(w1 * (S1 - K1), w2 * (S2 - K2)), 0.0);
        }
    }
}