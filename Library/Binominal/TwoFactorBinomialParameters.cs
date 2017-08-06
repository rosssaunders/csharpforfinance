using System;
using System.Collections.Generic;
using System.Text;
using CSharpForFinancialMarkets;

namespace Library.Binominal
{
    public class TwoFactorBinomialParameters
    {
        public ITwoFactorPayoff pay;      // Payoff function

        // Option data
        public double sigma1;
        public double sigma2;
        public double T;
        public double r;
        public double K;
        public double div1, div2;           // Dividends
        public double rho;                  // Correlation
        public bool exercise;               // false if European, true if American	

        // Default constuctor, prototype object
        public TwoFactorBinomialParameters()
        {

            sigma1 = 0.2;
            sigma2 = 0.3;
            T = 1.0;                        // One year
            r = 0.06;
            K = 1;
            div1 = 0.03;
            div2 = 0.04;
            rho = 0.5;
            exercise = true;
        }

        public double payoff(double S1, double S2)
        {
            return pay.Payoff(S1, S2);
        }
    }
}
