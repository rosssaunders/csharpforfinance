using System;

namespace CSharpForFinancialMarkets.Orders
{
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
