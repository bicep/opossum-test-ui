using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shapes;
using System.Windows;

namespace ShapesTest
{
    [TestClass]
    public class CustomLineTests
    {
        [TestMethod]
        public void testNormalIntersection()
        {
            Point p1 = new Point(0,100);
            Point p2 = new Point(100, 0);
            Point p3 = new Point(0, 0);
            Point p4 = new Point(100, 100);
            Point p5 = new Point(50, 50);
            CustomLine l1 = new CustomLine(p1, p2);
            CustomLine l2 = new CustomLine(p3, p4);
            Point? intersectionPoint = CustomLine.getIntersectionCoordinates(l1, l2);
            Assert.IsTrue(intersectionPoint.HasValue);
            Assert.AreEqual(p5.X, intersectionPoint.Value.X);
            Assert.AreEqual(p5.Y, intersectionPoint.Value.Y);
        }
        [TestMethod]
        public void testOneVerticalIntersection()
        {
            // horizontal line
            Point p1 = new Point(0, 100);
            Point p2 = new Point(10, 100);
            // vertical lines
            Point p3 = new Point(5, 0);
            Point p4 = new Point(5, 100);
            CustomLine l1 = new CustomLine(p1, p2);
            CustomLine l2 = new CustomLine(p3, p4);
            Point? intersectionPoint = CustomLine.getIntersectionCoordinates(l1, l2);
            Assert.IsTrue(intersectionPoint.HasValue);
            Assert.AreEqual(p3.X, intersectionPoint.Value.X);
            Assert.AreEqual(p1.Y, intersectionPoint.Value.Y);
        }
    }
}
