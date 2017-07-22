using System;
using System.Diagnostics;
using Library.Bonds;
using Library.ConvertibleBond;
using Library.Data;
using Library.Equities;
using Library.Forex;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static System.Math;

namespace CSharpForFinancialMarkets
{
    [TestClass]
    public class Chapter7
    {
        [TestMethod]
        public void C7_11_1_FutureValue()
        {
            var nPeriods = 6;
            var P = Pow(10.0, 7);
            var r = 0.092d;

            var interestEngine = new InterestRateCalculator(nPeriods, r);
            var fv = interestEngine.FutureValue(P);

            Debug.WriteLine(fv);
        }

        [TestMethod]
        public void C7_11_1_FutureValue2()
        {
            var nPeriods = 6;
            var P = Pow(10.0, 7);
            var r = 0.092d;

            var m = 2;

            var interestEngine = new InterestRateCalculator(nPeriods, r);
            var fv2 = interestEngine.FutureValue(P, m);

            Debug.WriteLine(fv2);
        }

        [TestMethod]
        public void C7_11_1_OrdinaryAnnuityTest()
        {
            var A = 2.0 * Pow(10.0, 6);
            var interestEngine = new InterestRateCalculator(15, 0.08);
            Debug.WriteLine(interestEngine.OrdinaryAnnuity(A));
        }

        [TestMethod]
        public void C7_11_1_PresentValue()
        {
            var Pn = 5.0 * Pow(10.0, 6);
            var interestEngine = new InterestRateCalculator(7, 0.10);

            Debug.WriteLine(interestEngine.PresentValue(Pn));
        }

        [TestMethod]
        public void C7_11_1_PresentValueOfSeries()
        {
            var interestEngine = new InterestRateCalculator(5, 0.0625);
            var nPeriods2 = interestEngine.Periods;
            var futureValues = new double[nPeriods2]; // For five years

            for (long j = 0; j < nPeriods2 - 1; j++)
                // The first 4 years
                futureValues[j] = 100.0;

            futureValues[nPeriods2 - 1] = 1100.0;

            Debug.WriteLine("**Present value, series: {0} ", interestEngine.PresentValue(futureValues));
        }

        [TestMethod]
        public void C7_11_1_PresentValueOfOrdinaryAnnuity()
        {
            // Present Value of an ordinary annuity
            var A = 100.0;

            var interest = 0.09;
            var numberOfPeriods = 8;
            var interestEngine = new InterestRateCalculator(numberOfPeriods, interest);

            Debug.WriteLine("**PV, ordinary annuity: {0}", interestEngine.PresentValueOrdinaryAnnuity(A));
        }

        [TestMethod]
        public void C7_11_1_FutureValueContinuous()
        {
            // Now test periodic testing with continuous compounding
            var P0 = Pow(10.0, 8);
            var r = 0.092;
            var nPeriods2 = 6;
            var interestEngine = new InterestRateCalculator(nPeriods2, r);

            for (var mm = 1; mm <= 100000000; mm *= 12)
                Debug.WriteLine("Periodic: {0}\t {1}", mm, interestEngine.FutureValue(P0, mm));

            Debug.WriteLine("Continuous Compounding: {0}", interestEngine.FutureValue(P0));
        }

        [TestMethod]
        public void C7_11_1_BondPricing()
        {
            // Bond pricing
            var coupon = 50; // Cash coupon, i.e. 10.0% rate semiannual on parValue
            var n = 40; // Number of payments
            var annualInterest = 0.11; // Interest rate annualized
            var paymentPerYear = 2; // Number of payment per year
            var parValue = 1000.0;
            var myBond = new Library.Bonds.Bond(n, annualInterest, coupon, paymentPerYear);

            var bondPrice = myBond.Price(parValue);

            Debug.WriteLine("Bond price: {0}", bondPrice);
        }


        // (ref Fixed-Income Securities Valuation, Risk Management and Portfolio Strategies page 3-4)
        // Element for build a bond

        // • The bond's currency denomination. An example is US$ for a US Treasury bond.
        // • The maturity date This is the date on which the principal amount is due.
        // • The coupon type. It can be fixed, floating or multi-coupon (a mix of fixed and floating or different
        // fixed). 
        // • The coupon rate It is expressed in percentage of the principal amount.
        // • The coupon frequency 
        // • The day-count type 
        // • The interest accrual date This is the date when interest begins to accrue.
        // • The settlement date This is the date on which payment is due in exchange for the bond. It is
        // generally equal to the trade date plus a number of working days.
        // • The first coupon date This is the date of the first interest payment.
        // • The par amount or nominal amount or principal amount This is the face value of the bond.
        // Note that the nominal amount is used to calculate the coupon bond. For example, consider
        // a bond with a fixed 5% coupon rate and a $1,000 nominal amount. The coupon is equal to
        // 5% × $1 ,000 = $50 . 
        // • The redemption value Expressed in percentage of the nominal amount, it is the price at which
        // the bond is redeemed on the maturity date. In most cases, the redemption value is equal to 100%
        // of the bond nominal amount. double

        // In this simple example we can deal only fixed single value coupon bond. 
        [TestMethod]
        public void C7_11_1_BondPricing2()
        {
            // Data
            var sd = new Date(2010, 9, 20);
            var ed = new Date(2012, 9, 20);
            var couponTenor = "6m";
            var rule = Rule.Backward;
            var rollAdj = BusinessDayAdjustment.Unadjusted;
            var payAdj = BusinessDayAdjustment.Following;
            var lagFromRecordDate = "0d";
            var coupon = 0.0425;
            double FaceAmount = 100;
            var dc = Dc._ItalianBTP;
            var lagSettlement = 3;

            // Create a bond
            var myBond = new Bond(sd, ed, couponTenor, rule, rollAdj, payAdj, lagFromRecordDate, coupon, FaceAmount, dc,
                lagSettlement);

            // Inizialize a bond price
            var td = new Date(2011, 02, 8);
            var myPricer = new BondPricer(myBond, td);

            double cleanPrice = 99;
            Console.WriteLine("CleanPrice {0} DirtyPrice {1}", cleanPrice, myPricer.DirtyPrice(cleanPrice));
            Console.WriteLine();

            // Parameters to calculate yield, can be modified
            var freq = 1;
            dc = Dc._30_360;
            var c = Compounding.Compounded;

            Console.WriteLine("Parameters used for yield: freq {1}, DayCount {2}, Compounding", freq, dc.ToString(),
                c.ToString());
            Console.WriteLine("Calculate Clean Price From Yield");

            var yield = 0.05;
            Console.WriteLine("given a yield% of {0},  the clean price is: {1}", yield,
                myPricer.CleanPriceFromYield(yield, 1, dc, c));
            Console.WriteLine();

            Console.WriteLine("Calculate Yield From Clean Price");
            var cleanpx = 99.05;
            Console.WriteLine("given a clean price of {0},  the yield is: {1}", cleanpx,
                myPricer.YieldFromCleanPrice(cleanpx, 1, dc, c));

            Console.WriteLine();
            Console.WriteLine("Accrued Interest is");
            Console.WriteLine(myPricer.AccruedInterest());
        }

        [TestMethod]
        public void C7_11_2_ReverseConvertibleBondOnEquityBasket()
        {
            var cb = new ConvertibleBond(OptionType.Put, 5, 100, 0, 0, 0.2);
            cb.Basket.Add(new Equity() { Price = 100, Volatility = 0.2}, 0.5);
            cb.Basket.Add(new Equity() { Price = 100, Volatility = 0.2}, 0.5);

            var correlations = new Matrix<double>(2,2);
            correlations[1, 1] = 0.8;
            correlations[1, 2] = 1;
            correlations[2, 1] = 1;
            correlations[2, 1] = 0.8;

            var price = cb.Price(100, correlations);

            Console.WriteLine(price);
        }

        [TestMethod]
        public void C7_11_3_FxOption()
        {
            var fxOption = new FxOption(OptionType.Put, 365, 100, 0, 0, 0.2);
            fxOption.Correlation = 0.8;
            fxOption.ExchangeRate1 = 1.1;
            fxOption.ExchangeRate2 = 1 / 1.1;
            fxOption.Currency1Volatility = 0.2;
            fxOption.Currency2Volatility = 0.1;
            fxOption.RiskFreeRate1 = 0;
            fxOption.RiskFreeRate2 = 0;

            Console.WriteLine(fxOption.Price(100));
        }
    }
}