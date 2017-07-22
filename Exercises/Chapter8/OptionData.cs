namespace CSharpForFinancialMarkets
{
    public class OptionData
    {
        public string ID;

        // Member data public for convenience
        public double r;        // Interest rate
        public double sig;      // Volatility
        public double K;        // Strike price
        public double T;        // Expiry date
        public double b;        // Cost of carry

        public string otyp;     // Option name (call, put)
    }
}