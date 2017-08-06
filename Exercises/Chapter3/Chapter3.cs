using System;
using Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharpForFinancialMarkets.Chapter3
{
    [TestClass]
    public class Chapter3Tests
    {
        [TestMethod]
        public void C3_14_1()
        {
           /*
            See the methods added to the Option class in the Library
            
            CallVega
            CallRho
            CallCharm
            CallVomma

           */
        }

        [TestMethod]
        public void C3_14_2_A_SwaptionCallPrice()
        {
            //var b = 0;
            var T = 2;
            var K = 0.075;
            var r = 0.06;
            var sig = 0.2;

            var F = 0.07;
            var m = 2;
            var t = 4;

            var option = new Swaption(OptionType.Call, T, K, r, sig, t, m);

            var swaptionPrice = option.Price(F);

            Assert.AreEqual(0.01796, swaptionPrice, 0.01);
        }

        [TestMethod]
        public void C3_14_2_B_SwaptionPutPrice()
        {
            //var b = 0;
            var T = 2;
            var K = 0.075;
            var r = 0.06;
            var sig = 0.2;

            var F = 0.07;
            var m = 2;
            var t = 4;

            var option = new Swaption(OptionType.Put, T, K, r, sig, t, m);

            var swaptionPrice = option.Price(F);

            Assert.AreEqual(0.033, swaptionPrice, 0.01);
        }

        [TestMethod]
        public void C3_14_3()
        {
            /*
                Squares from rectangles
                Stacks from Lists
             
                Taking a specialized class and attempting to restrict functionality.
            */
        }
    }
}
