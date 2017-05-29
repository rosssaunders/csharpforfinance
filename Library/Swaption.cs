using static System.Math;

namespace CSharpForFinancialMarkets
{
    public class Swaption : Option
    {
        /// <summary>
        /// Swap Tenor
        /// </summary>
        public double t { get; set; }     // Volatility

        /// <summary>
        /// Number of compounding per year in the swap rate
        /// </summary>
        public double m { get; set; }       // Strike price

        public Swaption(OptionType optionType, double expiry, double strike, double interest, double volatility, double swapTenor, double numberOfCompoundsPerYear) : base(optionType, expiry, strike, 0, interest, volatility)
        {
            // Create Swaption instance
            t = swapTenor;
            m = numberOfCompoundsPerYear;
        }

        private double Alpha(double f, double m, double t)
        {
            return (1 - (1 / Pow(1 + (f / m), t * m))) / f;
        }

        protected override double CallPrice(double U)
        {
            var blackPrice = base.CallPrice(U);
            var alpha = Alpha(U, m, t);

            return alpha * blackPrice;
        }

        protected override double PutPrice(double U)
        {
            var blackPrice = base.PutPrice(U);
            var alpha = Alpha(U, m, t);

            return alpha * blackPrice;
        }
    }
}
