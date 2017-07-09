using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Bonds
{
    public class InterestRateCalculator
    {
        private double r;  //Interest Rate
        private int nPeriods; //Numer of Periods per year

        public InterestRateCalculator(int numberOfPeriods, double interest)
        {
            r = interest;
            nPeriods = numberOfPeriods;
        }

        public double Interest { get => r; }

        public int Periods { get => nPeriods; }

        /// <summary>
        /// Future value of a sum of money invested today.
        /// </summary>
        /// <param name="P0"></param>
        /// <returns></returns>
        public double FutureValue(double P0)
        {
            double factor = 1.0 + Interest;
            return P0 * Math.Pow(factor, nPeriods);
        }

        /// <summary>
        /// Future value of a sum of money invested today, m periods per year. r is annual interest rate
        /// </summary>
        /// <param name="P0"></param>
        /// <returns></returns>
        public double FutureValue(double P0, int m)
        {
            double R = this.Interest / m;
            int newPeriods = m * nPeriods;

            var myBond = new InterestRateCalculator(newPeriods, R);
            
            return myBond.FutureValue(P0);
        }

        public double OrdinaryAnnuity(double A)
        {
            double factor = 1.0 + Interest;
            return A * ((Math.Pow(factor, nPeriods) - 1.0) / Interest);
        }

        public double PresentValue(double Pn)
        {
            double factor = 1.0 + Interest;
            return Pn * (1.0 / Math.Pow(factor, nPeriods));
        }

        public double PresentValue(double[] futureValues)
        {
            double factor = 1.0 + Interest;
            double PV = 0.0;

            for(int t = 0; t < nPeriods; t++)
            {
                PV += futureValues[t] / Math.Pow(factor, t + 1);
            }

            return PV;
        }

        public double PresentValueConstant(double Coupon)
        {
            double factor = 1.0 + Interest;
            double PV = 0.0;

            for(int t = 0; t < nPeriods; t++)
            {
                PV += 1.0 / Math.Pow(factor, t + 1);
            }

            return PV * Coupon;
        }

        public double PresentValueOrdinaryAnnuity(double A)
        {
            double factor = 1.0 + Interest;
            double numerator = 1.0 - (1.0 / Math.Pow(factor, nPeriods));
            return (A * numerator) / Interest;
        }

        public double FutureValueContinuous(double P0)
        {
            double growthFactor = Math.Exp(r * nPeriods);
            return P0 * growthFactor;
        }
    }

    public class Bond
    {
        InterestRateCalculator eng;

        /// <summary>
        /// Interest Rate
        /// </summary>
        private double r;

        /// <summary>
        /// Number of Periods
        /// </summary>
        private int nPeriods;

        /// <summary>
        /// Cash coupon payment
        /// </summary>
        private double c;

        public Bond(int numberPeriods, double interest, double Coupon, int paymentPerYear)
        {
            nPeriods = numberPeriods;
            r = interest / (double)paymentPerYear;
            c = Coupon;

            eng = new InterestRateCalculator(nPeriods, r);
        }

        public Bond(InterestRateCalculator irCalculator, double Coupon, int paymentPerYear)
        {
            eng = irCalculator;
            c = Coupon;
            nPeriods = eng.Periods;
            r = eng.Interest / (double)paymentPerYear;
        }

        public double Price(double redemptionValue)
        {
            double pvCoupon = eng.PresentValueConstant(c);
            double pvPar = eng.PresentValue(redemptionValue);
            return pvCoupon + pvPar;
        }
    }
}
