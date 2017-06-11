using System;
using static System.Math;
using static CSharpForFinancialMarkets.SpecialFunctions;

namespace Library
{
    public class VasicekModel : BondModel
    {
        private double longTermYield;
        
        /// <summary>
        /// Terms in A(t,s) * exp(-r*B(t,s))
        /// </summary>
        /// <param name="t"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        private double A(double t, double s)
        {
            double R = longTermYield;
            double smt = s - t;
            double exp = 1.0 - Exp(-kappa * smt);

            double result = R * exp / kappa - smt * R - 0.25 * vol * vol * exp * exp / (kappa * kappa * kappa);

            return Exp(result);
        }

        private double B(double t, double s)
        {
            return (1.0 - Exp(-kappa * (s - t))) / kappa;
        }

        public VasicekModel(double kappa, double theta, double vol, double r) : base(kappa, theta, vol, r)
        {
            longTermYield = theta - 0.5 * vol * vol / (kappa * kappa);
        }

        public override double P(double t, double s)
        {
            return A(t, s) * Exp(-r * B(t, s));
        }

        public override double R(double t, double s)
        {
            return (-Log(A(t, s)) + B(t, s) * r) / (s - t);
        }

        public override double YieldVolatility(double t, double s)
        {
            return vol * (1.0 - Exp(-kappa * (s - t))) / (kappa * (s - t));
        }

        // Accept visitor.
        public override void Accept(BondVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
