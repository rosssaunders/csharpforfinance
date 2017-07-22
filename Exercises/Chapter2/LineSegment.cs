using System;

namespace CSharpForFinancialMarkets.Chapter2
{
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
}