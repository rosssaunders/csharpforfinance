using System;

namespace CSharpForFinancialMarkets
{
    public class ExchangeStrategy : MultiAssetPayoffStrategy
    {
        // No member data
        public ExchangeStrategy() { }

        public override double Payoff(double S1, double S2)
        {
            return Math.Max(S1 - S2, 0.0); // a1S1 - a2S2 in general
        }
    }
}