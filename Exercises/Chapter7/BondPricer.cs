using System;
using Library.Bonds;
using Library.Dates;

namespace CSharpForFinancialMarkets
{
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
}