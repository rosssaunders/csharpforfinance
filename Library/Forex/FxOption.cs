using System;
using System.Collections.Generic;
using System.Text;
using CSharpForFinancialMarkets;
using static System.Math;

namespace Library.Forex
{
    public class FxOption : Option
    {
        public double RiskFreeRate1 { get; set; }

        public double RiskFreeRate2 { get; set; }

        public double ExchangeRate1 { get; set; }

        public double ExchangeRate2 { get; set; }

        public double Currency1Volatility { get; set; }

        public double Currency2Volatility { get; set; }

        public double Correlation { get; set; }

        public FxOption(OptionType optionType, double expiry, double strike, double costOfCarry, double interest, double volatility) : base(optionType, expiry, strike, costOfCarry, interest, volatility)
        {
        }

        public double Price()
        {
            var sig = Sqrt(Pow(Currency1Volatility, 2) + Pow(Currency2Volatility, 2) - 2 * Correlation * Currency1Volatility * Currency2Volatility);

            var d1 = Log(ExchangeRate1, ExchangeRate2) + (RiskFreeRate2 - RiskFreeRate1 + (sig * sig) / 2) * T;

            var d2 = d1 - sig * Sqrt(T);

            var price = ExchangeRate1 * Exp(-RiskFreeRate1 * T) * SpecialFunctions.N(-d1) -
                        ExchangeRate2 * Exp(-RiskFreeRate2 * T) * SpecialFunctions.N(-d2);

            return price;
        }
    }
}
