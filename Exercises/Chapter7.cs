using Library.Bonds;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using static System.Math;

namespace CSharpForFinancialMarkets
{
    public class Chapter7
    {
        [TestMethod]
        public void FutureValue()
        {
            var nPeriods = 6;
            var P = Pow(10.0, 7);
            var r = 0.092d;

            var interestEngine = new InterestRateCalculator(nPeriods, r);
            var fv = interestEngine.FutureValue(P);

            Debug.WriteLine(fv); 
        }

        [TestMethod]
        public void FutureValue2()
        {
            int nPeriods = 6;
            var P = Pow(10.0, 7);
            var r = 0.092d;

            int m = 2;
            
            var interestEngine = new InterestRateCalculator(nPeriods, r);
            var fv2 = interestEngine.FutureValue(P, m);

            Debug.WriteLine(fv2);
        }

        [TestMethod]
        public void OrdinaryAnnuityTest()
        {
            var A = 2.0 * Pow(10.0, 6);
            var interestEngine = new InterestRateCalculator(15, 0.08);
            Debug.WriteLine(interestEngine.OrdinaryAnnuity(A));
        }

        [TestMethod]
        public void PresentValue()
        {
            var Pn = 5.0 * Pow(10.0, 6);
            var interestEngine = new InterestRateCalculator(7, 0.10);

            Debug.WriteLine(interestEngine.PresentValue(Pn));
        }

        [TestMethod]
        public void PresentValueOfSeries()
        {
            var interestEngine = new InterestRateCalculator(5, 0.0625);
            var nPeriods2 = interestEngine.Periods;
            var futureValues = new double[nPeriods2];  // For five years

            for (long j = 0; j < nPeriods2 - 1; j++)
            {  
                // The first 4 years
                futureValues[j] = 100.0;
            }

            futureValues[nPeriods2 - 1] = 1100.0;

            Debug.WriteLine("**Present value, series: {0} ", interestEngine.PresentValue(futureValues));
        }

        [TestMethod]
        public void PresentValueOfOrdinaryAnnuity()
        {
            // Present Value of an ordinary annuity
            var A = 100.0;

            var interest = 0.09;
            var numberOfPeriods = 8;
            var interestEngine = new InterestRateCalculator(numberOfPeriods, interest);

            Debug.WriteLine("**PV, ordinary annuity: {0}", interestEngine.PresentValueOrdinaryAnnuity(A));
        }

        [TestMethod]
        public void FutureValueContinuous()
        {
            // Now test periodic testing with continuous compounding
            var P0 = Pow(10.0, 8);
            var r = 0.092;
            var nPeriods2 = 6;
            var interestEngine = new InterestRateCalculator(nPeriods2, r);

            for (int mm = 1; mm <= 100000000; mm *= 12)
            {
                Debug.WriteLine("Periodic: {0}\t {1}", mm, interestEngine.FutureValue(P0, mm));
            }

            Debug.WriteLine("Continuous Compounding: {0}", interestEngine.FutureValue(P0));
        }

        [TestMethod]
        public void BondPricing()
        {
            // Bond pricing
            var coupon = 50;                        // Cash coupon, i.e. 10.0% rate semiannual on parValue
            var n = 40;                             // Number of payments
            var annualInterest = 0.11;              // Interest rate annualized
            var paymentPerYear = 2;                 // Number of payment per year
            var parValue = 1000.0;
            var myBond = new Bond(n, annualInterest, coupon, paymentPerYear);

            var bondPrice = myBond.Price(parValue);

            Debug.WriteLine("Bond price: {0}", bondPrice);
        }
    }
}
