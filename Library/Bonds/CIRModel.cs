using System;
using static System.Math;
using static CSharpForFinancialMarkets.SpecialFunctions;

namespace Library
{
    public class CIRModel : BondModel
    {
        private double phi1;
        private double phi2;
        private double phi3;

        /// <summary>
        /// Terms in A(t,s) * exp(-r*B(t,s))
        /// </summary>
        /// <param name="t"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public double A(double t, double s)
        {
            double tmp1 = Exp(phi1 * (s - t));
            double tmp2 = Exp(phi2 * (s - t));

            double val = phi1 * tmp2 / (phi2 * (tmp1 - 1.0) + phi1);

            return Pow(val, phi3);
        }

        public double B(double t, double s)
        {
            double tmp1 = Exp(phi1 * (s - t));

            return (tmp1 - 1.0) / (phi2 * (tmp1 - 1.0) + phi1);
        }

        public CIRModel() : base(0,0,0,0)
        {

        }

        public CIRModel(double kappa, double theta, double vol, double r)
            : base(kappa, theta, vol, r)
        {
            // Calculate the work variables
            phi1 = Sqrt(kappa * kappa + 2.0 * vol * vol);
            phi2 = 0.5 * (kappa + phi1);
            phi3 = 2.0 * kappa * theta / (vol * vol);
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

            // Non-central chi^2 or FDM
        }
    }
}
