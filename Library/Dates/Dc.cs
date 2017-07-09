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
    public enum Dc
    {
        // many can be added
        _30_360 = 1,
        _Act_360 = 2,
        _Act_Act_ISMA,
        _ItalianBTP,
        _Act_365,
        _30_Act,
        _30_365
    }
}
