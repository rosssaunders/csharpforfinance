using CSharpForFinancialMarkets.Chapter4;
using CSharpForFinancialMarkets.Orders;
using Library.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Homework
{
    public interface IResettable
    {
        void Reset();
    }

    /// <summary>
    /// FIFO
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Queue<T> where T : IResettable
    {
        private List<T> items;

        public Queue()
        {
            items = new List<T>();
        }

        public void Enqueue(T item)
        {
            //Add to the end of the list
            items.Add(item);
        }

        public T Dequeue()
        {
            if (items.Count > 0)
            {
                var item = items[0];
                items.RemoveAt(0);
                return item;
            }

            throw new QueueEmptyException("No items are in the queue");
        }

        public void Reset()
        {
            foreach (var item in items)
                item.Reset();
        }
    }

    public class QueueEmptyException : Exception
    {
        public QueueEmptyException(string message) : base(message) { }
    }

    public class Account : IResettable
    {
        public void Reset() { }
    }


    [TestClass]
    class Chapter6Tests
    {
        [TestMethod]
        public void E6_12_1()
        {
            /*
                Generic Queue.
             */

            var queue = new Queue<Account>();

            var item = new Account();
            queue.Enqueue(item);
            Assert.AreEqual(item, queue.Dequeue());

            queue.Reset();
        }

        [TestMethod]
        public void E5_15_2()
        {
            /*
                Generic Methods

                See the Min & Max function below.

                I think there is a mistake in the book. I cannot think of (nor find) any way to use the < and > operators on a generic type without casting to type first.
                E.g. check if the type is int and then cast. Maybe thats what the author wants you to discover but it's not obvious.
             */
        }

        public static T Minimum<T>(T item1, T item2) where T : IComparable<T>
        {
            if(item1.CompareTo(item2) >= 0)
                return item1;

            return item2;
        }

        public static T Maximum<T>(T item1, T item2) where T : IComparable<T>
        {
            if (item1.CompareTo(item2) >= 0)
                return item2;

            return item1;
        }

        [TestMethod]
        public void E5_15_3()
        {
            /*
                The question is asking for pros and cons of the following implementations:

                1. Constraining the type at the class level
                2. Contraining the type to the function 

                Good question as I'd consider both to be ok. I'd normally lean towards 2 since it's easier to read the code.
             */
        }

        [TestMethod]
        public void E5_15_4_5()
        {
            /*
                Implementation of stats methods. I've combined Ex 4 & 5 and have implemented them straight as extension methods
            
                See below 
             */

                        
        }

        

        [TestMethod]
        public void E5_15_4_6()
        {
            /*
                Performance of the Matrix class vs raw arrays            
                
                Since the matrix class is a thin wrapper around a native .net array the performance will roughly be the same with a tiny overhead on each call.
             */
        }
    }

    public static class Stats
    {
        //Measures of Central Tendency

        public static T SumOfSquares<T>(this Vector<T> vector)
        {
            T accumulator = default(T);
            for(var i = vector.MinIndex; i < vector.MaxIndex; i++)
            {
                accumulator = Vector<T>.addTT(accumulator, Vector<T>.mulTT(vector[i], vector[i]));
            }

            return accumulator;
        }

        public static T Sum<T>(this Vector<T> vector)
        {
            T accumulator = default(T);
            for (var i = vector.MinIndex; i < vector.MaxIndex; i++)
            {
                accumulator = Vector<T>.addTT(accumulator, vector[i]);
            }

            return accumulator;
        }

        public static T SumOfReciprocals<T>(this Vector<T> vector)
        {
            var div = GenericOperatorFactory<int, T, T, T>.Divide;

            T accumulator = default(T);
            for (var i = vector.MinIndex; i < vector.MaxIndex; i++)
            {
                accumulator = Vector<T>.addTT(accumulator, div(1, vector[i]));
            }

            return accumulator;
        }

        public static T ArithmetricMean<T>(this Vector<T> vector)
        {
            var div = GenericOperatorFactory<T, int, T, T>.Divide;

            return div(Sum(vector), vector.Length);
        }

        public static T GeometricMean<T>(this Vector<T> vector)
        {
            return default(T);
        }

        public static T HarmonicMean<T>(this Vector<T> vector)
        {
            return default(T);
        }

        public static T RootMeanSquare<T>(this Vector<T> vector)
        {
            return default(T);
        }

        //Measures of Dispersion

        public static T MeanAverageDeviation<T>(this Vector<T> vector)
        {
            return default(T);
        }

        public static T StandardDeviation<T>(this Vector<T> vector)
        {
            return default(T);
        }

        public static T Variance<T>(this Vector<T> vector)
        {
            return default(T);
        }
    }
}
