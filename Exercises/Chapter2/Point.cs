using System;

namespace CSharpForFinancialMarkets.Chapter2
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
            return Math.Sqrt(Math.Pow(Math.Abs((double) (p2.y - this.y)), 2) + Math.Pow(Math.Abs((double) (p2.x - this.x)), 2));
        }

        public override string ToString()
        {
            return string.Format("Point ({0}, {1})", x, y);
        }
    }
}