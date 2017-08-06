using Library;
using Library.Binominal;

namespace CSharpForFinancialMarkets
{
    public abstract class MultiAssetPayoffStrategy : ITwoFactorPayoff
    {
        public abstract double Payoff(double S1, double S2);
    }
}