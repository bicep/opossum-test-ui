using System.Windows;
using System.Windows.Media;
using System;

namespace Shapes
{
    public class CustomLine
    {
        private double slope;
        private double c;
        private Point point1;
        private Point point2;
        private bool isVertical = false;

        public CustomLine() { }

        public CustomLine(Point p1, Point p2) : this()
        {
            this.point1 = p1;
            this.point2 = p2;
            this.slope = CustomLine.calculateSlope(p1, p2);
            if (slope == 1e+10) { this.isVertical = true; }
            if (!isVertical) { this.c = CustomLine.calculateC(p1, slope); };
        }

        public CustomLine(Point p1, double s) : this()
        {
            this.point1 = p1;
            this.slope = s;
            if (slope == 1e+10) { this.isVertical = true; }
            if (!isVertical) { this.c = CustomLine.calculateC(p1, slope); };
            this.point2 = this.calculatePoint2();
        }

        public double getSlope()
        {
            return this.slope;
        }

        public void setSlope(double newSlope)
        {
            this.slope = newSlope;
        }

        public Point calculatePoint2()
        {
            Point point = new Point();
            if (this.isVertical)
            {
                point.X = this.point1.X;
                // just some whatever y point
                point.Y = this.point1.Y + 1;
                return point;
            }
            point.X = this.point1.X + 1;
            point.Y = this.slope * point.X + this.c;
            return point;
            
        }

        public bool getIsVertical()
        {
            return this.isVertical;
        }

        public Point getPoint1()
        {
            return this.point1;
        }

        public Point getPoint2()
        {
            return this.point2;
        }

        public double getConstant()
        {
            return this.c;
        }

        private static double calculateSlope(Point p1, Point p2) {
            // if x = constant then just make gradient to be 0
            if ((p2.X - p1.X) != 0) { return (p2.Y - p1.Y) / (p2.X - p1.X); }
            else { return 1e+10; }
        }

        private static double calculateC(Point p1, double slope) {
            return (p1.Y - (slope * p1.X));      
        }

        public static double getPerpendicularSlope(double slope)
        {
            if (slope == 0) { return 1e+10; }
            if (slope == 1e+10) { return 0; }
            return -1 / slope;
        }

        public static Point? getIntersectionCoordinates(CustomLine l1, CustomLine l2) {
            Point intersectionPoint = new Point();
            // if parallel
            if (l1.getSlope() == l2.getSlope()) { return null; }

            // if either one is vertical (but not both because we would have returned null already)

            else if (l1.getIsVertical()) {
                intersectionPoint.X = l1.getPoint1().X;
                Point l2Point = l2.getPoint1();
                intersectionPoint.Y = (intersectionPoint.X * l2.getSlope()) + l2.getConstant();
            }

            else if (l2.getIsVertical())
            {
                intersectionPoint.X = l2.getPoint1().X;
                Point l1Point = l1.getPoint1();
                intersectionPoint.Y = (intersectionPoint.X * l1.getSlope()) + l1.getConstant();
            }

            else
            {
                Point l1Point = l1.getPoint1();
                Point l2Point = l2.getPoint1();
                double a1 = l1Point.Y - l1.getSlope() * l1Point.X;
                double a2 = l2Point.Y - l2.getSlope() * l2Point.X;
                intersectionPoint.X = (a1 - a2) / (l2.getSlope() - l1.getSlope());
                intersectionPoint.Y = a2 + l2.getSlope() * intersectionPoint.X;
            }

            return intersectionPoint;
        }

        public static bool withinLineSegment(Point point3, CustomLine l)
        {
            Point point1 = l.getPoint1();
            Point point2 = l.getPoint2();
            // we use sqrt here because getDistance does not actually sqrt
            double p1p2 = (Utilities.getDistance(point1, point2));
            double p1p3 = (Utilities.getDistance(point1, point3));
            double p2p3 = (Utilities.getDistance(point2, point3));
            if (p1p2 == (p1p3 + p2p3)) { return true; }
            else { return false; }
        }
    }
}