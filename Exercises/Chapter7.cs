using Library.Bonds;
using Library.Dates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using static System.Math;

namespace CSharpForFinancialMarkets
{
    struct Bond
    {
        // Member data calculated
        public Schedule schedule;
        public double[] cashFlows;
        public double couponRate;
        // Member data
        public double redemptionValue;
        public Dc dayCount;
        public int settlementDaysLag; // lag in business day between today (trade date of the bond) and the settlement date

        // constructor
        public Bond(Date StartDate, Date EndDate, string CouponTenor, Rule RuleGenerator,
                BusinessDayAdjustment RollAdj, BusinessDayAdjustment PayAdj, string LagPayFromRecordDate,
            double CouponRate, double RedemptionValue, Dc DayCount, int SettlementDaysLag)
        {
            // Passing value to data member
            this.redemptionValue = RedemptionValue;
            this.dayCount = DayCount;
            this.settlementDaysLag = SettlementDaysLag;
            this.couponRate = CouponRate;

            // Build schedule
            this.schedule = new Schedule(StartDate, EndDate, CouponTenor, RuleGenerator, RollAdj, LagPayFromRecordDate, PayAdj);

            // Build CashFlows
            this.cashFlows = Date.GetFullCouponArray(schedule.fromDates, schedule.toDates, CouponRate, RedemptionValue, DayCount);
        }
    }

    // Interface IBondPricer
    interface IBondPricer
    {
        // Calculate Yield according to customized convection (freq,DayCount and compounding), given a clean price
        double YieldFromCleanPrice(double cleanPrice, int freq, Dc DayCount, Compounding compounding);

        // Calculate Clean price from Yield according to customized convection (freq,DayCount and compounding), given a clean price
        double CleanPriceFromYield(double yield, int freq, Dc DayCount, Compounding compounding);

        // Calculate DirtyPrice, see page 27, Clean and Dirty Bond Price, see Choudhry, M. 2010. Fixed-income Securities and Derivatives Handbook : Analysis and Valuation. Bloomberg
        double DirtyPrice(double CleanPrice);

        double AccruedInterest();
    }

    // Bond Pricer
    class BondPricer : IBondPricer
    {
        // Data member
        private Bond bond;
        private Date settlementDate;

        Date lastCouponDate;  // Given settlement date, it is lastCouponDate
        Date nextCouponDate;  // Given settlement date, it is nextCouponDate
        double accruedInterest; // Given settlement date, it is accruedInterest

        // Constructor
        public BondPricer(Bond Bond, Date Today)
        {
            // Bond to data member
            this.bond = Bond;

            // Inizialize the pricer as of today
            CalculateSettlementDate(Today);
            CalculateCouponData();
            CalculateAccruedInterest();
        }

        #region Interface IBondPricer implementation
        // Calculate Yield according to customized convection (freq,DayCount and compounding), given a clean price
        public double YieldFromCleanPrice(double cleanPrice, int freq, Dc DayCount, Compounding compounding)
        {
            double fullPrice = DirtyPrice(cleanPrice); // calculate full price
            double guess = 0.05;  // initial guess: arbitrary value
            return Formula.YieldFromFullPrice(fullPrice, guess, bond.schedule.payDates, bond.cashFlows, freq, DayCount,
                settlementDate, compounding, bond.redemptionValue); // calc yield from full price
        }

        // Calculate Clean price from Yield according to customized convection (freq,DayCount and compounding), given a clean price
        public double CleanPriceFromYield(double yield, int freq, Dc DayCount, Compounding compounding)
        {
            double fullPrice = Formula.FullPriceFromYield(yield, bond.schedule.payDates, bond.cashFlows, freq, DayCount,
                settlementDate, compounding, bond.redemptionValue);
            return fullPrice - accruedInterest; // clean price
        }

        // Calculate DirtyPrice, see page 27, Clean and Dirty Bond Price, see Choudhry, M. 2010. Fixed-income Securities and Derivatives Handbook : Analysis and Valuation. Bloomberg
        public double DirtyPrice(double CleanPrice)
        {
            return CleanPrice + accruedInterest;  // dirty price = clean + accrued
        }

        // Get AccruedInterest
        public double AccruedInterest() { return accruedInterest; }

        #endregion

        #region Private method used in constructor
        // Set Settlement Date in Data member
        private void CalculateSettlementDate(Date td)
        {
            this.settlementDate = td.add_workdays(bond.settlementDaysLag);
        }

        // Calculate  lastCouponDate, nextCouponDate, currentCouponRate in data member
        private void CalculateCouponData()
        {
            // the bond schedule
            Schedule schdl = bond.schedule;

            // assign lastCopuponDate and nextCouponDate
            Date[] Roll = schdl.toDates;  // rolling of coupon
            int k = Array.FindIndex(Roll, n => n > this.settlementDate); // look for index for next coupon

            this.nextCouponDate = Roll[k];  // nextCouponDate

            if (k == 0)
            {
                this.lastCouponDate = schdl.fromDates[0]; // lastCouponDate
            }
            else
            {
                this.lastCouponDate = Roll[k - 1];  // lastCouponDate
            }
        }

        // Calculate Accrued Interest
        private void CalculateAccruedInterest()
        {
            accruedInterest = Date.AccruedInterest(settlementDate, lastCouponDate, nextCouponDate, bond.couponRate,
                bond.redemptionValue, bond.dayCount);
        }
        #endregion
    }

    [TestClass]
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
        public void BondPricing2()
        {
            // Create Bond
            Console.Write("Create a specific bond..");
            Bond myBond = CreateMyBond();
            Console.WriteLine("Bond created!");

            // Inizialize a bond price
            Console.Write("Create a specific bond pricer...");
            Date td = new Date(2011, 02, 8);
            BondPricer myPricer = new BondPricer(myBond, td);
            Console.WriteLine("BondPricer created!");
            Console.WriteLine();

            Console.WriteLine("Calculate Dirty Price from Clean Price");
            double cleanPrice = 99;
            Console.WriteLine("CleanPrice {0} DirtyPrice {1}", cleanPrice, myPricer.DirtyPrice(cleanPrice));
            Console.WriteLine();

            // Parameters to calculate yield, can be modified
            int freq = 1;
            Dc dc = Dc._30_360;
            Compounding c = Compounding.Compounded;

            Console.WriteLine("Parameters used for yield: freq {1}, DayCount {2}, Compounding", freq, dc.ToString(), c.ToString());
            Console.WriteLine("Calculate Clean Price From Yield");
            double yield = 0.05;
            Console.WriteLine("given a yield% of {0},  the clean price is: {1}", yield, myPricer.CleanPriceFromYield(yield, 1, dc, c));
            Console.WriteLine();

            Console.WriteLine("Calculate Yield From Clean Price");
            double cleanpx = 99.05;
            Console.WriteLine("given a clean price of {0},  the yield is: {1}", cleanpx, myPricer.YieldFromCleanPrice(cleanpx, 1, dc, c));

            Console.WriteLine();
            Console.WriteLine("Accrued Interest is");
            Console.WriteLine(myPricer.AccruedInterest());
        }

        // It Create a specific bond
        static Bond CreateMyBond()
        {
            // Data
            Date sd = new Date(2010, 9, 20);
            Date ed = new Date(2012, 9, 20);
            string couponTenor = "6m";
            Rule rule = Rule.Backward;
            BusinessDayAdjustment rollAdj = BusinessDayAdjustment.Unadjusted;
            BusinessDayAdjustment payAdj = BusinessDayAdjustment.Following;
            string lagFromRecordDate = "0d";
            double coupon = 0.0425;
            double FaceAmount = 100;
            Dc dc = Dc._ItalianBTP;
            int lagSettlement = 3;

            // Create a bond
            return new Bond(sd, ed, couponTenor, rule, rollAdj, payAdj, lagFromRecordDate, coupon, FaceAmount, dc, lagSettlement);
        }
    }
}
