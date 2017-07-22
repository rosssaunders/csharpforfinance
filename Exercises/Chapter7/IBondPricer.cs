using System;

namespace CSharpForFinancialMarkets
{
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
}