using CSharpForFinancialMarkets.Orders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpForFinancialMarkets.Chapter4
{
    [TestClass]
    class Chapter4Tests
    {
        [TestMethod]
        public void C4_14_1()
        {
            /*
             1a) 
                i) Add a new method to BondVisitor Visit that takes a FDMModel
                ii) Pricing methods put on OptionPricer class that uses the IVP classes to compute the price.
             
             1b) 
                Q. Other operations on Bonds using the Visitor pattern
                A. Theoretical value of Bond using different models

             1c)
                Additional subclasses of BondVisitor
            */
        }

        [TestMethod]
        public void C4_14_2()
        {
            var o = new Order();
            o.Name = "Ross";
            o.RegistrationDate = DateTime.Today.AddDays(-1);
        }

        [TestMethod]
        public void C4_14_3_4()
        {
           /*
            * Compile error on the Read-Only property
           */
        }

        [TestMethod]
        public void C4_14_5()
        {
            /*
             * See the Static constructor and property on the Order class in the Order Folder.
            */
        }

        [TestMethod]
        public void C4_14_6_10()
        {
            /*
             * See the code in the Order folder
            */
        }
    }
}
