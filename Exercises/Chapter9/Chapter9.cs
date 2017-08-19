using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Library.Binominal;
using Library.Data;
using Library.Excel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharpForFinancialMarkets.Chapter9
{
    [TestClass]
    public class Chapter9
    {
        private Library.Binominal.Option _testOption;
        private int numberOfSteps = 50;

        [TestInitialize]
        public void Initialize()
        {
            _testOption = new Library.Binominal.Option
            {
                K = 100,
                type = 2,
                T = 0.25,
                r = 0.1,
                sig = 0.2
            };
        }

        [TestMethod]
        public void C9_9_ExcelOutput()
        {
            var opt = _testOption;

            var steps = 50;
            var S = opt.K;

            double k = opt.T / steps;

            double discounting = Math.Exp(-opt.r * k);

            var binParams = new CRRStrategy(opt.sig, opt.r, k); ; // Factory
            var bn = new BinomialMethod(discounting, binParams, steps, opt.EarlyImpl);

            bn.ModifyLattice(opt.K);

            // Phase III: Backward Induction and compute option price
            Vector<double> RHS = new Vector<double>(bn.BasePyramidVector());
            if (binParams.BinomialType == BinomialType.Additive)
            {
                RHS[RHS.MinIndex] = S * Math.Exp(steps * binParams.DownValue);
                for (int j = RHS.MinIndex + 1; j <= RHS.MaxIndex; j++)
                {
                    RHS[j] = RHS[j - 1] * Math.Exp(binParams.UpValue - binParams.DownValue);
                }
            }

            var pay = opt.PayoffVector(RHS);

            var pr = bn.GetPrice(pay);

            // Display lattice in Excel
            var file = Path.GetTempFileName();
            file = Path.ChangeExtension(file, "xlsx");
            ExcelMechanisms exl = new ExcelMechanisms(file);

            try
            {
                // Display in Excel; first create array of asset mesh points
                int startIndex = 0;
                Vector<double> xarr = new Vector<double>(steps + 1, startIndex);
                xarr[xarr.MinIndex] = 0.0;
                for (int j = xarr.MinIndex + 1; j <= xarr.MaxIndex; j++)
                {
                    xarr[j] = xarr[j - 1] + k;
                }

                string sheetName = "Lattice";

                exl.printLatticeInExcel(bn.GetOptionLattice, xarr, sheetName);

                exl.Save();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Assert.AreEqual(pr, 3.0732, 0.01);
        }

        [TestMethod]
        public void C9_9_4_European()
        {
            var opt = _testOption;

            var k = opt.T / numberOfSteps;
            var discounting = Math.Exp(-opt.r * k);

            var binParams = new CRRStrategy(opt.sig, opt.r, k); ; // Factory
            var bn = new BinomialMethod(discounting, binParams, numberOfSteps);

            bn.ModifyLattice(opt.K);

            // Phase III: Backward Induction and compute option price
            Vector<double> RHS = new Vector<double>(bn.BasePyramidVector());
            if (binParams.BinomialType == BinomialType.Additive)
            {
                RHS[RHS.MinIndex] = opt.K * Math.Exp(numberOfSteps * binParams.DownValue);
                for (int j = RHS.MinIndex + 1; j <= RHS.MaxIndex; j++)
                {
                    RHS[j] = RHS[j - 1] * Math.Exp(binParams.UpValue - binParams.DownValue);
                }
            }

            var pay = opt.PayoffVector(RHS);

            var pr = bn.GetPrice(pay);

            Assert.AreEqual(pr, 2.8307, 0.01);
        }

        [TestMethod]
        public void C9_9_4_EarlyExercise()
        {
            var opt = _testOption;

            var steps = 50;
            var S = opt.K;

            double k = opt.T / steps;

            double discounting = Math.Exp(-opt.r * k);

            var binParams = new CRRStrategy(opt.sig, opt.r, k); ; // Factory
            var bn = new BinomialMethod(discounting, binParams, steps, opt.EarlyImpl);

            bn.ModifyLattice(opt.K);

            // Phase III: Backward Induction and compute option price
            Vector<double> RHS = new Vector<double>(bn.BasePyramidVector());
            if (binParams.BinomialType == BinomialType.Additive)
            {
                RHS[RHS.MinIndex] = S * Math.Exp(steps * binParams.DownValue);
                for (int j = RHS.MinIndex + 1; j <= RHS.MaxIndex; j++)
                {
                    RHS[j] = RHS[j - 1] * Math.Exp(binParams.UpValue - binParams.DownValue);
                }
            }

            var pay = opt.PayoffVector(RHS);

            var pr = bn.GetPrice(pay);

            Assert.AreEqual(pr, 3.0732, 0.01);
        }

        [TestMethod]
        public void C9_9_6_TwoFactor_Spread_American()
        {
            // Declare and initialise the parameters
            var myData = new TwoFactorBinomialParameters
            {
                sigma1 = 0.2,
                sigma2 = 0.3,
                T = 1.0,
                r = 0.06,
                K = 1.0,
                div1 = 0.03,
                div2 = 0.04,
                rho = 0.5,
                exercise = true
            };

            // Clewlow and Strickland p. 47 
            // false;
            var S1 = 100.0;
            var S2 = 100.0;
            var w1 = 1.0;
            var w2 = -1.0;
            var cp = 1;
            myData.pay = new SpreadStrategy(cp, myData.K, w1, w2);

            var myTree = new TwoFactorBinomial(myData, numberOfSteps, S1, S2);
            var price = myTree.Price();

            Assert.AreEqual(price, 10.15119, 0.001);
        }

        [TestMethod]
        public void C9_9_6_TwoFactor_Spread_European()
        {
            // Declare and initialise the parameters
            var myData = new TwoFactorBinomialParameters
            {
                sigma1 = 0.2,
                sigma2 = 0.3,
                T = 1.0,
                r = 0.06,
                K = 1.0,
                div1 = 0.03,
                div2 = 0.04,
                rho = 0.5,
                exercise = false
            };

            // Clewlow and Strickland p. 47 
            // false;
            var S1 = 100.0;
            var S2 = 100.0;
            var w1 = 1.0;
            var w2 = -1.0;
            var cp = 1;
            myData.pay = new SpreadStrategy(cp, myData.K, w1, w2);

            var myTree = new TwoFactorBinomial(myData, numberOfSteps, S1, S2);
            var price = myTree.Price();

            Assert.AreEqual(price, 10.13757, 0.001);
        }

        [TestMethod]
        public void C9_9_6_TwoFactor_Basket_European()
        {
            double Price(double T, double sig1, double sig2)
            {
                var myData = new TwoFactorBinomialParameters
                {
                    sigma1 = sig1,
                    sigma2 = sig2,
                    T = T,
                    r = 0.1,
                    K = 40.0,
                    div1 = 0.0,
                    div2 = 0.0,
                    rho = 0.5,
                    exercise = false
                };

                var S1 = 18.0;
                var S2 = 20.0;
                var w1 = 1.0;
                var w2 = 1.0;
                var cp = -1; // Weights; put option
                myData.pay = new BasketStrategy(myData.K, cp, w1, w2);

                var myTree = new TwoFactorBinomial(myData, 500, S1, S2);
                return myTree.Price();
            }
            
            Assert.AreEqual(Price(0.95, 0.1, 0.1), 0.6032, 0.001);
            Assert.AreEqual(Price(0.95, 0.1, 0.2), 1.2402, 0.001);
            Assert.AreEqual(Price(0.95, 0.1, 0.3), 1.9266, 0.001);
            Assert.AreEqual(Price(0.95, 0.2, 0.1), 1.1597, 0.001);
            Assert.AreEqual(Price(0.95, 0.2, 0.2), 1.7749, 0.001);
            Assert.AreEqual(Price(0.95, 0.2, 0.3), 2.4383, 0.001);
            Assert.AreEqual(Price(0.95, 0.3, 0.1), 1.7639, 0.001);
            Assert.AreEqual(Price(0.95, 0.3, 0.2), 2.355, 0.001);
            Assert.AreEqual(Price(0.95, 0.3, 0.3), 2.9976, 0.001);

            Assert.AreEqual(Price(0.05, 0.1, 0.1), 1.8025, 0.001);
            Assert.AreEqual(Price(0.05, 0.1, 0.2), 1.8332, 0.001);
            Assert.AreEqual(Price(0.05, 0.1, 0.3), 1.9117, 0.001);
            Assert.AreEqual(Price(0.05, 0.2, 0.1), 1.8270, 0.001);
            Assert.AreEqual(Price(0.05, 0.2, 0.2), 1.8859, 0.001);
            Assert.AreEqual(Price(0.05, 0.2, 0.3), 1.9817, 0.001);
            Assert.AreEqual(Price(0.05, 0.3, 0.1), 1.8906, 0.001);
            Assert.AreEqual(Price(0.05, 0.3, 0.2), 1.9682, 0.001);
            Assert.AreEqual(Price(0.05, 0.3, 0.3), 2.0737, 0.001);
        }

        [TestMethod]
        public void C9_9_7_PadeApproximation()
        {
            var opt = new Library.Binominal.Option
            {
                K = 65,
                type = 1,
                T = 0.25,
                r = 0.08,
                sig = 0.3
            };

            double PriceWithJR(int numSteps, int type)
            {
                opt.type = type;

                var steps = numSteps;
                var S = 60;
                double k = opt.T / steps;

                double discounting = Math.Exp(-opt.r * k);

                var binParams = new PadeJRStrategy(opt.sig, opt.r, k); // Factory
                var bn = new BinomialMethod(discounting, binParams, steps);

                bn.ModifyLattice(S);

                // Phase III: Backward Induction and compute option price
                var RHS = new Vector<double>(bn.BasePyramidVector());

                var pay = opt.PayoffVector(RHS);

                return bn.GetPrice(pay);
            }

            double PriceWithCRR(int numSteps, int type)
            {
                opt.type = type;

                var steps = numSteps;
                var S = 60;
                double k = opt.T / steps;

                double discounting = Math.Exp(-opt.r * k);

                var binParams = new PadeCRRStrategy(opt.sig, opt.r, k); // Factory
                var bn = new BinomialMethod(discounting, binParams, steps);

                bn.ModifyLattice(S);

                // Phase III: Backward Induction and compute option price
                var RHS = new Vector<double>(bn.BasePyramidVector());

                var pay = opt.PayoffVector(RHS);

                return bn.GetPrice(pay);
            }

            Assert.AreEqual(PriceWithCRR(100, 1), 2.1399, 0.01);
            Assert.AreEqual(PriceWithCRR(100, 2), 5.8527, 0.01);
            Assert.AreEqual(PriceWithCRR(200, 1), 2.1365, 0.01);
            Assert.AreEqual(PriceWithCRR(200, 2), 5.8494, 0.01);

            Assert.AreEqual(PriceWithJR(100, 1), 2.1386, 0.01);
            Assert.AreEqual(PriceWithJR(100, 2), 5.8516, 0.01);
            Assert.AreEqual(PriceWithJR(200, 1), 2.1344, 0.01);
            Assert.AreEqual(PriceWithJR(200, 2), 5.8473, 0.01);
        }

        [TestMethod]
        public void C9_9_9_2_TheLeisenReimerMethod()
        {
            var opt = _testOption;
            var numberOfSteps = 99;

            var k = opt.T / numberOfSteps;
            var discounting = Math.Exp(-opt.r * k);

            var S = opt.K;

            var binParams = new LeisenReimerStrategy(numberOfSteps, S, opt.K, opt.T, opt.r, opt.sig);
            var bn = new BinomialMethod(discounting, binParams, numberOfSteps);

            bn.ModifyLattice(S);

            // Phase III: Backward Induction and compute option price
            var RHS = new Vector<double>(bn.BasePyramidVector());
            var pay = opt.PayoffVector(RHS);
            var pr = bn.GetPrice(pay);

            Assert.AreEqual(pr, 2.8307, 0.01);
        }
    }
}
