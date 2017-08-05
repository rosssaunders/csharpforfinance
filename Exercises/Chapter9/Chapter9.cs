using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;
using Library.Binominal;
using Library.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharpForFinancialMarkets.Chapter9
{
    public interface IOptionFactory
    {
        Library.Binominal.Option create();
    }

    public class ConsoleEuropeanOptionFactory : IOptionFactory
    {
        public Library.Binominal.Option create()
        {
            Console.Write("\n*** Parameters for option object ***\n");

            var opt = new Library.Binominal.Option();

            Console.Write("Strike: ");
            opt.K = Convert.ToDouble(Console.ReadLine());

            Console.Write("Volatility: ");
            opt.sig = Convert.ToDouble(Console.ReadLine());

            Console.Write("Interest rate: ");
            opt.r = Convert.ToDouble(Console.ReadLine());


            Console.Write("Expiry date: ");
            opt.T = Convert.ToDouble(Console.ReadLine());

            Console.Write("1. Call, 2. Put: ");
            opt.type = Convert.ToInt32(Console.ReadLine());

            return opt;
        }
    }

    [TestClass]
    public class Chapter9
    {
        static IOptionFactory getFactory()
        {
            return new ConsoleEuropeanOptionFactory();

            // Later, other factory types
        }

        public static BinomialLatticeStrategy getStrategy(double sig, double r, double k,
            double S, double K, int N)
        {
            Console.WriteLine(
                "\n1. CRR, 2. JR, 3. TRG, 4. EQP, 5. Modified CRR:\n6. Cayley JR Transform: 7 Cayley CRR:");

            int choice;
            choice = Convert.ToInt32(Console.ReadLine());

            if (choice == 1)
                return new CRRStrategy(sig, r, k);

            if (choice == 2)
                return new JRStrategy(sig, r, k);

            if (choice == 3)
                return new TRGStrategy(sig, r, k);

            if (choice == 4)
                return new EQPStrategy(sig, r, k);

            if (choice == 5)
                return new ModCRRStrategy(sig, r, k, S, K, N);

            if (choice == 6)
                return new PadeJRStrategy(sig, r, k);

            if (choice == 7)
                return new PadeCRRStrategy(sig, r, k);


            return new CRRStrategy(sig, r, k);
        }

        public void TestStrategies()
        {
            // Phase I: Create and initialise the option
            IOptionFactory fac = getFactory();

            int N = 200;
            Console.Write("Number of time steps: ");
            N = Convert.ToInt32(Console.ReadLine());

            double S;
            Console.Write("Underlying price: ");
            S = Convert.ToDouble(Console.ReadLine());

            Library.Binominal.Option opt = fac.create();

            double k = opt.T / N;

            // Create basic lattice
            double discounting = Math.Exp(-opt.r * k);

            // Phase II: Create the binomial method and forward induction
            BinomialLatticeStrategy binParams = getStrategy(opt.sig, opt.r, k, S, opt.K, N); // Factory
            BinomialMethod bn = new BinomialMethod(discounting, binParams, N);

            bn.modifyLattice(S);

            // Phase III: Backward Induction and compute option price
            Vector<double> RHS = new Vector<double>(bn.BasePyramidVector());
            if (binParams.BinomialType == BinomialType.Additive)
            {
                RHS[RHS.MinIndex] = S * Math.Exp(N * binParams.DownValue);
                for (int j = RHS.MinIndex + 1; j <= RHS.MaxIndex; j++)
                {
                    RHS[j] = RHS[j - 1] * Math.Exp(binParams.UpValue - binParams.DownValue);
                }
            }

            Vector<double> Pay = opt.PayoffVector(RHS);

            double pr = bn.getPrice(Pay);
            Console.WriteLine("European {0}", pr);

            // Binomial method with early exercise
            BinomialMethod bnEarly = new BinomialMethod(discounting, binParams, N, opt.EarlyImpl);
            bnEarly.modifyLattice(S);
            Vector<double> RHS2 = new Vector<double>(bnEarly.BasePyramidVector());
            Vector<double> Pay2 = opt.PayoffVector(RHS2);
            double pr2 = bnEarly.getPrice(Pay2);
            Console.WriteLine("American {0}", pr2);


            // Display in Excel; first create array of asset mesh points
            int startIndex = 0;
            Vector<double> xarr = new Vector<double>(N + 1, startIndex);
            xarr[xarr.MinIndex] = 0.0;
            for (int j = xarr.MinIndex + 1; j <= xarr.MaxIndex; j++)
            {
                xarr[j] = xarr[j - 1] + k;
            }

            // Display lattice in Excel
            //ExcelMechanisms exl = new ExcelMechanisms();

            try
            {
                //public void printLatticeInExcel(Lattice<double> lattice, Vector<double> xarr, string SheetName)
                string sheetName = "Lattice";

                //convert lattice to DataSet

                //exl.printLatticeInExcel(bnEarly.getLattice(), xarr, sheetName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private Library.Binominal.Option GetTestOption()
        {
            return new Library.Binominal.Option
            {
                K = 100,
                type = 2,
                T = 0.25,
                r = 0.1,
                sig = 0.2
            };
        }

        private int numberOfSteps = 50;

        [TestMethod]
        public void C9_9_4_European()
        {
            var opt = GetTestOption();

            var k = opt.T / numberOfSteps;
            var discounting = Math.Exp(-opt.r * k);

            var binParams = new CRRStrategy(opt.sig, opt.r, k); ; // Factory
            var bn = new BinomialMethod(discounting, binParams, numberOfSteps);

            bn.modifyLattice(opt.K);

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

            var pr = bn.getPrice(pay);

            Assert.AreEqual(pr, 2.8307, 0.01);
        }

        [TestMethod]
        public void C9_9_4_EarlyExercise()
        {
            var opt = GetTestOption();

            var steps = 50;
            var S = opt.K;

            double k = opt.T / steps;

            double discounting = Math.Exp(-opt.r * k);

            var binParams = new CRRStrategy(opt.sig, opt.r, k); ; // Factory
            var bn = new BinomialMethod(discounting, binParams, steps, opt.EarlyImpl);

            bn.modifyLattice(opt.K);

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

            var pr = bn.getPrice(pay);

            Assert.AreEqual(pr, 3.0732, 0.01);
        }
    }
}
