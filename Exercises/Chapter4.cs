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
        public void E4_14_1()
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
        public void E4_14_2()
        {
            var o = new Order();
            o.Name = "Ross";
            o.RegistrationDate = DateTime.Today.AddDays(-1);
        }

        [TestMethod]
        public void E4_14_3_4()
        {
           /*
            * Compile error on the Read-Only property
           */
        }

        [TestMethod]
        public void E4_14_5()
        {
            /*
             * See the Static constructor and property on the Order class below.
            */
        }

        [TestMethod]
        public void E4_14_6_10()
        {
            /*
             * See the code below
            */
        }
    }

    public interface IPricing
    {
        double Price { get; set; }

        double Discount { get; set; }
    }

    public class Order : IPricing
    {
        public static int OrderNumber { get; private set; }

        static Order()
        {
            OrderNumber = 1;
        }

        public Order()
        {
            OrderNumber++;

            Name = string.Empty;
            RegistrationDate = DateTime.Today;
        }

        public Order(string name, DateTime registrationDate) : this()
        {
            Name = name;
            RegistrationDate = registrationDate;
        }

        public Order(Order order) : this()
        {
            Name = order.Name;
            RegistrationDate = order.RegistrationDate;
        }

        public string Name { get; set; }
       
        public DateTime RegistrationDate { get; set; }
        public virtual double Price { get; set; }
        public virtual double Discount { get => 0; set => throw new NotImplementedException(); }

        public virtual void Print()
        {
            Console.WriteLine($"{Name}, {RegistrationDate:dd-MMM-yyyy}");
        }
    }

    public class ExternalOrder : Order
    {
        private double m_discount;

        public string Company { get; set; }
        
        public override double Discount
        {
            get => Price - m_discount;
            set => m_discount = value;
        }

        public override void Print()
        {
            Console.WriteLine($"{Name}, {RegistrationDate:dd-MMM-yyyy}, {Company}");
        }
    }
}
