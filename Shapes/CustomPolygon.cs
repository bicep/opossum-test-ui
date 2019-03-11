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

    public class CustomPolygon
    {
        private Polygon polygon;
        private List<CustomLine> polygonLines;

        public CustomPolygon(Polygon polygon)
        {
            this.polygon = polygon;
            this.PopulatePolygonLines();
        }

        private void PopulatePolygonLines()
        {
            this.polygonLines = new List<CustomLine>();
            for (int i = 0; i<this.polygon.Points.Count; i++)
            {
                Point firstPoint = this.polygon.Points[i];
                Point secondPoint;
                // if this is the last point
                if (i == this.polygon.Points.Count - 1) {
                    secondPoint = this.polygon.Points[0];
                }
                else
                {
                    secondPoint = this.polygon.Points[i + 1];
                }

                this.polygonLines.Add(new CustomLine(firstPoint, secondPoint));
            }
        }

        public List<CustomLine> GetPolygonLines()
        {
            return this.polygonLines;
        }

        public PointCollection GetPoints()
        {
            return this.polygon.Points;
        }

        public static Point ClosestPointFromPointToPolygon(CustomPolygon polygon, Point point)
        {
            // we keep track of point with minimum distance
            Point? minPoint = null;
            double minDist = double.MaxValue;

            List<CustomLine> polygonLines = polygon.GetPolygonLines();
            // for each line in polygon, find the perpendicular line from point 
            for (int i = 0; i < polygonLines.Count(); i++)
            {
                CustomLine lineSegment = polygonLines[i];
                double slope = lineSegment.getSlope();
                double perpSlope = CustomLine.getPerpendicularSlope(slope);
                CustomLine perpLine = new CustomLine(point, perpSlope);
                Point? intersectionPoint = CustomLine.getIntersectionCoordinates(lineSegment, perpLine);

                if (intersectionPoint.HasValue) {
                    double dist = Utilities.getDistance(intersectionPoint.Value, point);
                    // does this line intersect the polygon line segment?
                    // is the intersection point and the point a min distance
                    if (CustomLine.withinLineSegment(intersectionPoint.Value, lineSegment)
                        && dist < minDist)
                    {
                        minDist = dist;
                        minPoint = intersectionPoint;
                    }
                }
                else
                {
                    throw new Exception("Intersection Point is null even though it should have a coordinate");
                }
            }
            
            // If there is a minpoint we are good to go
            if (minPoint.HasValue)
            {
                return minPoint.Value;
            }
            
            // or else we calculate the closest distance from the point to the polygon corners
            else
            {
                minDist = double.MaxValue;
                PointCollection points = polygon.GetPoints();
                for (int i = 0; i<points.Count; i++)
                {
                    double dist = Utilities.getDistance(points[i], point);
                    if (dist < minDist) {
                        minDist = dist;
                        minPoint = points[i];
                    }
                }
            }

            // If there is a minpoint we are good to go
            if (minPoint.HasValue)
            {
                return minPoint.Value;
            }
            else
            {
                throw new Exception("Point with minimum distance is null even though it should have a coordinate");
            }
        }

        public static bool IsInPolygon(PointCollection poly, Point p)
        {
            Point p1, p2;


            bool inside = false;


            if (poly.Count < 3)
            {
                return inside;
            }


            var oldPoint = new Point(
                poly[poly.Count - 1].X, poly[poly.Count - 1].Y);


            for (int i = 0; i < poly.Count; i++)
            {
                var newPoint = new Point(poly[i].X, poly[i].Y);


                if (newPoint.X > oldPoint.X)
                {
                    p1 = oldPoint;

                    p2 = newPoint;
                }

                else
                {
                    p1 = newPoint;

                    p2 = oldPoint;
                }


                if ((newPoint.X < p.X) == (p.X <= oldPoint.X)
                    && (p.Y - (long)p1.Y) * (p2.X - p1.X)
                    < (p2.Y - (long)p1.Y) * (p.X - p1.X))
                {
                    inside = !inside;
                }


                oldPoint = newPoint;
            }


            return inside;
        }
    }
}
