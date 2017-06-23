using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CSharpForFinancialMarkets.Orders
{

    public class Order : IPricing
    {
        public static int OrderNumber { get; private set; }

        private List<OrderItem> m_items = new List<OrderItem>();

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
            Debug.Write($"{Name}, {RegistrationDate:dd-MMM-yyyy}");
        }

        public void AddItem(OrderItem item)
        {
            if (string.IsNullOrWhiteSpace(item.Name))
                throw new NoNameException("Missing name");

            item.Order = this;
            m_items.Add(item);

            ItemAdded?.Invoke(this, new OrderAddedEventArgs() { Order = this });
        }

        public IEnumerable<OrderItem> Items
        {
            get
            {
                return m_items;
            }
        }

        public static Order operator +(Order order1, Order order2)
        {
            var combinedOrder = new Order()
            {
                Name = $"{order1.Name}+{order2.Name}",
                Price = order1.Price + order2.Price
            };

            foreach (var item in order1.Items)
                combinedOrder.AddItem(new OrderItem(item));

            foreach (var item in order2.Items)
                combinedOrder.AddItem(new OrderItem(item));

            return combinedOrder;
        }

        public OrderItem this[int key]
        {
            get
            {
                return m_items[key];
            }
            set
            {
                m_items[key] = value;
            }
        }

        public int Count { get => m_items.Count; }

        public event EventHandler<OrderAddedEventArgs> ItemAdded;

        public static void PrintItems(Order o)
        {
            foreach(OrderItem item in o.Items)
            {
                Debug.Write(item.Name);
            }
        }
    }

    public class OrderAddedEventArgs : EventArgs
    {
        public Order Order { get; set; }
    }
}
