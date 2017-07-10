// ----------------------------------------------------------------------
// IMPORTANT DISCLAIMER:
// The code is for demonstration purposes only, it comes with NO WARRANTY AND GUARANTEE.
// No liability is accepted by the Author with respect any kind of damage caused by any use
// of the code under any circumstances.
// Any market parameters used are not real data but have been created to clarify the exercises 
// and should not be viewed as actual market data.
//
// FormulaUtility.cs
// Author Andrea Germani
// Copyright (C) 2012. All right reserved
// ------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Bonds
{
    public static class Formula
    {
        // Tuckman, B. 2002: page 60, formula 4.15
        public static double FullPriceFromSemiAnnualYield(double yield, double coupon, int numberOfFutureCoupons, double yfToNextCoupon)
        {
            int freq = 2;
            double fullPrice = 0.0;
            for (int i = 0; i <= numberOfFutureCoupons; i++)
            {
                fullPrice += (coupon / freq) * (1 / (Math.Pow(1 + (yield / freq), i + yfToNextCoupon)));
            }
            fullPrice += (1 / (Math.Pow(1 + (yield / freq), numberOfFutureCoupons + yfToNextCoupon)));
            return fullPrice;
        }

        // Haug, E. 2007: 11.2.5
        public static double FullPriceFromYield(double yield, double coupon, double yfToNextCoupon, int numberOfFutureCoupons,
            double FaceValue, int freq)
        {
            return
                Math.Pow(1 + yield, (1 / freq) - yfToNextCoupon) *
                    (
                    (coupon / freq) *
                    ((Math.Pow(1 + yield, -numberOfFutureCoupons / freq) - 1) / (1 - Math.Pow(1 + yield, 1 / freq))) +
                    (FaceValue / (Math.Pow(1 + yield, numberOfFutureCoupons / freq)))
                    );
        }

        // Calculate dirty price from yield, adapting 
        public static double FullPriceFromYield(double yield, Date[] CashFlowsDate, double[] CashFlows,
            int freq, Dc dc, Date SettlementDate, Compounding compounding, double FaceValue)
        {
            // initial discount
            double df = 1.0;  // discount
            double yf = 0.0;

            // find next payment date that is greater than settlement date, previous payments will no more need      
            int iStart = Array.FindIndex(CashFlowsDate, n => n > SettlementDate);  // index of next payment
            int iEnd = CashFlowsDate.GetLength(0);  // total payments (past+ future)
            int dim = iEnd - iStart;  // number of payments in the future

            // myCFs vector of future payments(after settlement date)
            // myCFsDates vector of date of each element of myCFs, starting from settlement date        
            Date[] myCFsDates = new Date[dim + 1]; // start from settlement date + future dates of payment
            double[] myCFs = new double[dim]; // future payments
            Array.ConstrainedCopy(CashFlowsDate, iStart, myCFsDates, 1, dim);  // populate myCFsDates 1 to dim index
            myCFsDates[0] = SettlementDate;  // first element of myCFsDates is settlement date
            Array.ConstrainedCopy(CashFlows, iStart, myCFs, 0, dim); // populate myCFs

            // Iterate to add each cash flow discounted, according to given convention
            double fullPrice = 0.0; // starting value
            for (int i = 0; i < dim; i++)
            {
                yf = myCFsDates[i].YF(myCFsDates[i + 1], dc);  // calculate new yf
                df *= CalCDF(yield, yf, freq, compounding); // calculate new df
                fullPrice += myCFs[i] * df;
            }

            // Adding discounted face amount
            fullPrice += FaceValue * df;

            // final result
            return fullPrice;
        }

        // Calculate Yield from dirty price
        public static double YieldFromFullPrice(double fullPrice, double yieldGuess, Date[] CashFlowsDate, double[] CashFlows,
            int freq, Dc dc, Date SettlementDate, Compounding compounding, double FaceValue)
        {
            int N = CashFlowsDate.GetLength(0);  // number of Cash flows dates
            int k = Array.FindIndex(CashFlowsDate, n => n > SettlementDate);  // get index of first cash flow after settlement date
            Date[] Dates = new Date[N - k];  // array with pay dates after settlement
            double[] Flows = new double[N - k];  // flows after settlement
            Array.Copy(CashFlowsDate, k, Dates, 0, N - k); // fill array with needed data
            Array.Copy(CashFlows, k, Flows, 0, N - k); // fill array with needed data

            // define my function: we should find yield that match starting price
            NumMethod.myMethodDelegate fname =
                       y_ => fullPrice - FullPriceFromYield(y_, Dates, Flows,
            freq, dc, SettlementDate, compounding, FaceValue);

            // return yield
            return NumMethod.NewRapNum(fname, yieldGuess);
        }

        public static double CalCDF(double r, double yf, int freq, Compounding c)
        {
            double n = (double)freq;
            switch (c)
            {
                case Compounding.Simple:
                    return 1 / (1.0 + r * yf);  // page 10, formula (1.8) Fixed Income Securities And Derivatives Handbook, Moorad Choudhry
                case Compounding.Compounded:
                    return 1 / (System.Math.Pow(1.0 + r / n, n * yf));  // page 11, formula (1.9) Fixed Income Securities And Derivatives Handbook, Moorad Choudhry
                case Compounding.Continuous:
                    return System.Math.Exp(-r * yf);  // page 9m, formula (1.6) Fixed Income Securities And Derivatives Handbook, Moorad Choudhry. Solve for PV, where FV=1
            }
            return 0;
        }
    }
}
