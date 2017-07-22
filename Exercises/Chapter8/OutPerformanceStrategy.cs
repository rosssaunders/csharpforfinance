using System;

namespace CSharpForFinancialMarkets
{
    public class OutPerformanceStrategy : MultiAssetPayoffStrategy
    {
        /// <summary>
        /// Values of underlyings at maturity
        /// </summary>
        public double I1 { get; set; } 

        /// <summary>
        /// Values of underlyings at maturity
        /// </summary>
        public double I2 { get; set; }

        /// <summary>
        /// Call +1 or put -1
        /// </summary>
        public int w { get; set; }  

        /// <summary>
        /// Strike rate of option
        /// </summary>
        public double K { get; set; }  

        public OutPerformanceStrategy()
        {

        }

        public OutPerformanceStrategy(double currentRate1, double currentRate2, int cp, double strikeRate)
        {
            I1 = currentRate1;
            I2 = currentRate2;
            w = cp;
            K = strikeRate;
        }

        public override double Payoff(double S1, double S2)
        {
            return Math.Max(w * ((I1 / S1) - (I2 / S2)) - w * K, 0.0);
        }
    }
}