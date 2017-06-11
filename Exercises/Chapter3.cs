using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static System.Math;

namespace CSharpForFinancialMarkets.Chapter3
{
    public class Point
    {
        private static int numPoints = 0;
        private static Point origin = new Point(0d, 0d);
        public static Point origin2;

        private double xc = 0;
        private double yc = 0;

        static Point()
        {
            double x = ((8.0 * 62.0) / (31.0 * 16.0)) - 1.0;
            double y = (88.0 / 2.0) - (11.0 * 4.0);
            origin2 = new Point(x, y);
        }

        ~Point()
        {
            numPoints--;
        }

        public static int GetPoints()
        {
            return numPoints;
        }

        public static Point Origin
        {
            get
            {
                return origin;
            }
        }

        public Point()
        {
            numPoints++;
        }

        public Point(double x, double y) : this()
        {
            X = x;
            Y = y;
        }

        public Point(Point point) : this(point.X, point.Y)
        {
        }

        public double X { get => xc; set => xc = value; }
        public double Y { get => yc; set => yc = value; }

        public double Distance(Point p2)
        {
            return Sqrt(Pow(Abs(p2.Y - this.Y), 2) + Pow(Abs(p2.X - this.X), 2));
        }

        public override string ToString()
        {
            return string.Format("Point ({0}, {1})", X, Y);
        }
    }

    [TestClass]
    public class Chapter3Tests
    {
        [TestMethod]
        public void E3_14_1()
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
        public void SwaptionCallPrice()
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
        public void SwaptionPutPrice()
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
        public void E3_14_3()
        {
            /*
                Squares from rectangles
                Stacks from Lists
             
                Taking a specialized class and attempting to restrict functionality.
            */
        }
    }
}
