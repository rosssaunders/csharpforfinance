using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static System.Math;

namespace UnitTestProject1
{
    public struct Point
    {
        public double x;
        public double y;

        public Point(double xVal, double yVal)
        {
            x = xVal;
            y = yVal;
        }

        public double distance(Point p2)
        {
            return Sqrt(Pow(Abs(p2.y - this.y), 2) + Pow(Abs(p2.x - this.x), 2));
        }

        public override string ToString()
        {
            return string.Format("Point ({0}, {1})", x, y);
        }
    }

    public struct LineSegment
    {
        private Point startPoint;
        private Point endPoint;

        public LineSegment(Point p1, Point p2)
        {
            startPoint = p1;
            endPoint = p2;
        }

        public LineSegment(LineSegment l)
        {
            startPoint = l.start();
            endPoint = l.end();
        }

        Point start()
        {
            return startPoint;
        }

        Point end()
        {
            return endPoint;
        }

        void start(Point pt)
        {
            startPoint = pt;
        }

        void end(Point pt)
        {
            endPoint = pt;
        }

        double length()
        {
            var xLength = endPoint.x - startPoint.x;
            var yLength = endPoint.y - startPoint.y;
            var length = Math.Sqrt((xLength * xLength) + (yLength * yLength));
            return length;
        }

        Point MidPoint()
        {
            return new Point((endPoint.x - startPoint.x) / 2, (endPoint.y - startPoint.y) / 2);
        }
    }

    [TestClass]
    public class Chaper2
    {
        [TestMethod]
        public void ZeroCouponBond()
        {
            double r;		// Interest rate
            double sig;	    // Volatility
            double K;		// Strike price
            double T;		// Expiry date
            double b;		// Cost of carry

            OptionType type;	// Option name (call, put)

            K = 0.8;
            sig = 0.1;
            r = 0.05;
            b = 0.0;
            T = 1;
            type = OptionType.Call;

            Option opt = new Option(type, T, K, b, r, sig);

            // 3. Get the price 
            double S1 = Exp(-0.05 * 5);
            double S2 = Exp(-0.05 * 1);

            var price = opt.Price(S1 / S2);

            Assert.AreEqual(price, 0.0404, 0.001);
        }

        [TestMethod]
        public void TestMethod1()
        {
            Assert.AreEqual(SpecialFunctions.N(0), 0.5, 0.001);
        }
    }
}
