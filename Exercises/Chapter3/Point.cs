using System;

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

        public Point(Point point) : this((double) point.X, (double) point.Y)
        {
        }

        public double X { get => xc; set => xc = value; }
        public double Y { get => yc; set => yc = value; }

        public double Distance(Point p2)
        {
            return Math.Sqrt(Math.Pow(Math.Abs((double) (p2.Y - this.Y)), 2) + Math.Pow(Math.Abs((double) (p2.X - this.X)), 2));
        }

        public override string ToString()
        {
            return string.Format("Point ({0}, {1})", X, Y);
        }
    }
}