using System;
using Library.Dates;

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
}