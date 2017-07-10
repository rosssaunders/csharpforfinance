using CSharpForFinancialMarkets.Chapter4;
using CSharpForFinancialMarkets.Orders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Homework
{
    class Chapter5Tests
    {
        [TestMethod]
        public void E5_15_1()
        {
            dynamic x = 5;
            x = new Order();

            
            /*
                See the Order Item code in the Order folder
                
                Notes: instead of the ArrayList class, which is not strongly typed, I've used the List<T> type.
             */

            var order = new Order();
            order.Name = "Order 1";
            order.AddItem(new OrderItem("Item1"));

            foreach(OrderItem item in order.Items)
            {
                Debug.Write($"{item.Name}, {item.Order.Name}");
            }
        }

        [TestMethod]
        public void E5_15_2()
        {
            /*
                Operator Overloading.

                See the Order class for the implementation
             */

            var order1 = new Order();
            order1.Name = "Order 1";
            order1.AddItem(new OrderItem("Item 1"));

            var order2 = new Order();
            order2.Name = "Order 2";
            order2.AddItem(new OrderItem("Item 2"));

            var order3 = order1 + order2;

            Debug.Write(order3.Name);
            foreach (OrderItem item in order3.Items)
            {
                Debug.Write($"{item.Name}, {item.Order.Name}");
            }
        }

        [TestMethod]
        public void E5_15_3()
        {
            /*
                See the Order code in the Order folder for the Exception implementation

                Note: ApplicationException does NOT exist in .net core.
             */

            var order = new Order()
            {
                Name = "Order 1"
            };

            try
            {
                order.AddItem(new OrderItem());
            }
            catch(NoNameException)
            {
                Debug.Write("Correct exception type caught");
            }
            catch(Exception)
            {
                throw;
            }
        }

        [TestMethod]
        public void E5_15_4()
        {
            /*
                See the Order Item code in the Order folder for the implementation.

                Note: Instead of a delegate type exposed by the class, I'm using the standard EventHander<T> type to expose the 
                payload of the event which is the recommened Event pattern by Microsoft.
             */

            var order = new Order();
            order.Name = "Order 1";

            order.ItemAdded += Order_ItemAdded;

            order.AddItem(new OrderItem("Item1"));
            order.AddItem(new OrderItem("Item2"));
            order.AddItem(new OrderItem("Item3"));
            order.AddItem(new OrderItem("Item4"));
        }

        private void Order_ItemAdded(object sender, OrderAddedEventArgs e)
        {
            Order.PrintItems(e.Order);
        }
    }

}
