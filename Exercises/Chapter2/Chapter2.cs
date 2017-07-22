using Microsoft.VisualStudio.TestTools.UnitTesting;
using static System.Math;
using CSharpForFinancialMarkets;

namespace CSharpForFinancialMarkets.Chapter2
{
    [TestClass]
    public class Chaper2Tests
    {
        [TestMethod]
        public void C2_ZeroCouponBond()
        {
            double r;		// Interest rate
            double sig;	    // Volatility
            double K;		// Strike price
            double T;		// Expiry date
            double b;		// Cost of carry

            OptionType type;	// Option name (call, put)

            K = 0.8;
            sig = 0.1;
            r = 0.05;
            b = 0.0;
            T = 1;
            type = OptionType.Call;

            Option opt = new Option(type, T, K, b, r, sig);

            // 3. Get the price 
            double S1 = Exp(-0.05 * 5);
            double S2 = Exp(-0.05 * 1);

            var price = opt.Price(S1 / S2);

            Assert.AreEqual(price, 0.0404, 0.001);
        }

        [TestMethod]
        public void C2_NormalDistributionTest()
        {
            Assert.AreEqual(SpecialFunctions.N(0), 0.5, 0.001);
        }
    }
}
