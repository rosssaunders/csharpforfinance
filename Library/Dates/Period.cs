// ----------------------------------------------------------------------
// IMPORTANT DISCLAIMER:
// The code is for demonstration purposes only, it comes with NO WARRANTY AND GUARANTEE.
// No liability is accepted by the Author with respect any kind of damage caused by any use
// of the code under any circumstances.
// Any market parameters used are not real data but have been created to clarify the exercises 
// and should not be viewed as actual market data.
//
// Date.cs
// Author Andrea Germani
// Copyright (C) 2012. All right reserved
// ------------------------------------------------------------------------
// Web references
// http: // www.isda.org/c_and_a/pdf/mktc1198.pdf
// http: // en.wikipedia.org/wiki/Accrued_interest
// http: // en.wikipedia.org/wiki/Day_count_convention
// http: // www.swx.com/download/trading/products/bonds/accrued_interest_en.pdf
// http: // www.isda.org/c_and_a/docs/30-360-2006ISDADefs.xls
// http: // www.euronext.com/editorial/wide/editorial-4304-EN.html
// see also Quantlib.org for Target calendar

// Exercise:
// 1) add method to complete all convention you find to following address:
// http: // en.wikipedia.org/wiki/Day_count_convention
// 2) test the following spreadsheet:
// http: // www.isda.org/c_and_a/docs/30-360-2006ISDADefs.xls

namespace System
{
    public struct Period
    {
        // Data member 
        public int tenor;  // tenor as int (i.e. 1, 2....)
        public TenorType tenorType;  // tenor type

        // Constructors
        public Period(int tenor, TenorType tenorType)
        {
            this.tenor = tenor;
            this.tenorType = tenorType;
        }
        public Period(string period)
        {
            char maturity = period[period.Length - 1];
            int n_periods = int.Parse(period.Remove(period.Length - 1, 1));
            tenor = n_periods;
            // C# 3.0 Cookbook, par 20.10
            tenorType = (TenorType)Enum.Parse(typeof(TenorType), Convert.ToString(maturity).ToUpper());
        }

        // Method get string format
        public string GetPeriodStringFormat()
        {
            return tenor + (tenorType.ToString()).ToLower();
        }

        // Interval in time 1Y = 1, 6m = 0.5, ...18m =1.5
        public double TimeInterval()
        {
            return ((double)this.tenor / (double)this.tenorType);
        }
    }
}
