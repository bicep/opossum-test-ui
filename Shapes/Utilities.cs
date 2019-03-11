using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Shapes
{
    static class Utilities
    {
        // util methods (to be refactored)
        public static Double getDistance(Point point1, Point point2)
        {
            return Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));
        }

        public static bool floatsEqual(double n1, double n2, double offset)
        {
            if ((n2 <= n1 + offset) && (n2 >= n1 - offset))
            {
                return true;
            }
            return false;
        }

        public static bool containsNegative(List<Double> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] < 0)
                {
                    return true;
                }
            }
            return false;
        }

        public static Point getPolygonCentroid(Polygon polygon)
        {
            Point centroid =
                polygon.Points.Aggregate(
                new { xSum = 0.0, ySum = 0.0, n = 0 },
                (acc, p) => new
                {
                    xSum = acc.xSum + p.X,
                    ySum = acc.ySum + p.Y,
                    n = acc.n + 1
                },
                acc => new Point(acc.xSum / acc.n, acc.ySum / acc.n));

            return centroid;
        }
    }
}
