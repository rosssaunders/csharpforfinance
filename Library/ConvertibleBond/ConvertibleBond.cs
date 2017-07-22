using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpForFinancialMarkets;
using Library.Data;
using Library.Equities;
using static System.Math;

namespace Library.ConvertibleBond
{
    public class BasketComponent
    {
        internal BasketComponent()
        {
            
        }

        public Equity Equity { get; internal set; }

        public double Weight { get; internal set; }
    }

    public class Basket
    {
        internal Basket()
        {
            Components = new List<BasketComponent>();
        }

        public List<BasketComponent> Components { get; }

        public void Add(Equity equity, double weight)
        {
            Components.Add(new BasketComponent() { Equity = equity, Weight = weight });
        }
    }

    /// <summary>
    /// From Chapter 7: Exercise 2
    /// </summary>
    public class ConvertibleBond : Option
    {
        public Basket Basket { get; }

        public ConvertibleBond(OptionType optionType, double expiry, double strike, double costOfCarry, double interest, double volatility) : base(optionType, expiry, strike, costOfCarry, interest, volatility)
        {
            Basket = new Basket();
        }

        public double Price(double U, Matrix<double> correlations)
        {
            var M1 = Basket.Components.Sum(t => t.Weight * t.Equity.Price);

            var M2 = 0d;

            for (var i = 0; i < Basket.Components.Count; i++)
            {
                for (var j = 0; j < Basket.Components.Count; j++)
                {
                    var power = correlations[i + 1, j + 1] * Basket.Components[i].Equity.Volatility * Basket.Components[j].Equity.Volatility * T;

                    M2 += Basket.Components[i].Weight * Basket.Components[j].Weight * Basket.Components[i].Equity.Price * Basket.Components[j].Equity.Price * Exp(power);
                }
            }

            sig = 1 / T * Log(M2 / M1);

            ///Assume b = r
            return base.PutPrice(M2);
        }
    }
}
