using System;

namespace CSharpForFinancialMarkets
{
    public class SpreadStrategy : MultiAssetPayoffStrategy
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
        /// a > 0, b < 0
        /// </summary>
        public double a { get; set; }

        /// <summary>
        /// a > 0, b < 0
        /// </summary>
        public double b { get; set; }

        public SpreadStrategy()
        {

        }

        public SpreadStrategy(int cp)
        {
            K = 0.0;
            w = cp;
            a = 1.0;
            b = -1.0;
        }

        public SpreadStrategy(int cp, double strike, double A, double B)
        {
            K = strike;
            w = cp;
            a = A;
            b = B;
        }

        public override double Payoff(double S1, double S2)
        {
            double sum = a * S1 + b * S2;
            return Math.Max(w * (sum - K), 0.0);
        }
    }
}