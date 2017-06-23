using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpForFinancialMarkets.Orders
{

    public class OrderItem
    {
        public OrderItem()
        {
            Order = null;
        }

        public OrderItem(string name)
        {
            Name = name;
            Order = null;
        }

        public OrderItem(OrderItem orderItem)
        {
            Name = orderItem.Name;
            Order = orderItem.Order;
        }

        public string Name { get; set; }

        public Order Order { get; set; }
    }
}
