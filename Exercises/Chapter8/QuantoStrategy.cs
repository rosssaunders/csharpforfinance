using System;

namespace CSharpForFinancialMarkets
{
    public class QuantoStrategy : MultiAssetPayoffStrategy
    {
        /// <summary>
        /// Strike in foreign currency
        /// </summary>
        public double Kf { get; set; }

        /// <summary>
        /// Fixed exchange rate
        /// </summary>
        public double fer { get; set; }

        /// <summary>
        /// +1 call, -1 put
        /// </summary>
        public int w { get; set; }

        public QuantoStrategy()
        {

        }

        public QuantoStrategy(double foreignStrike, int cp, double forExchangeRate)
        {
            Kf = foreignStrike;
            w = cp;
            fer = forExchangeRate;
        }

        public override double Payoff(double S1, double S2)
        {
            return fer * Math.Max(w * S1 - w * Kf, 0.0);
        }
    }
}