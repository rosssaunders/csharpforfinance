using System;

namespace CSharpForFinancialMarkets
{
    public class BestWorstStrategy : MultiAssetPayoffStrategy
    {
        /// <summary>
        /// Strike
        /// </summary>
        public double K { get; set; } 

        /// <summary>
        /// +1 Best, -1 Worst
        /// </summary>
        public double w { get; set; }      

        public BestWorstStrategy()
        {

        }

        public BestWorstStrategy(double cash, double BestWorst)
        {
            K = cash;
            w = BestWorst;
        }

        public override double Payoff(double S1, double S2)
        {
            if (w == 1) // Best
                return Math.Max(Math.Max(S1, S2), K);

            return Math.Min(Math.Min(S1, S2), K);
        }
    }
}