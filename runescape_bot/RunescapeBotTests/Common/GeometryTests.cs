using Microsoft.VisualStudio.TestTools.UnitTesting;
using RunescapeBot.Common;
using RunescapeBot.ImageTools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.Common.Tests
{
    [TestClass()]
    public class GeometryTests
    {
        [TestMethod()]
        [DataRow(true, 0, 0, true, 0, 0, double.MaxValue)]
        [DataRow(true, 0, 0, false, 5, 5, double.MaxValue)]
        [DataRow(false, 5, 5, true, 0, 0, double.MaxValue)]
        [DataRow(false, 307, 520, false, 101, 903, 434.88504228)]
        [DataRow(false, -307, -520, false, -101, -903, 434.88504228)]
        public void DistanceBetweenPointsTest(bool nullA, int ax, int ay, bool nullB, int bx, int by, double expected)
        {
            Point? a = nullA ? (Point?)null : new Point(ax, ay);
            Point? b = nullB ? (Point?)null : new Point(bx, by);
            double distance = Geometry.DistanceBetweenPoints(a, b);
            Assert.AreEqual(expected, distance, 0.001 * expected);
        }

        [TestMethod()]
        [DataRow(0, 0, 0, 0, 0, 0)]
        [DataRow(7, 4, 2, 6, 9, 10)]
        [DataRow(-6, 0, 2, -12, -4, -12)]
        public void AddPointsTest(int ax, int ay, int bx, int by, int sumX, int sumY)
        {
            Point a = new Point(ax, ay);
            Point b = new Point(bx, by);
            Point sum = new Point(sumX, sumY);
            Assert.IsTrue(sum.Equals(Geometry.AddPoints(a, b)));
        }

        [TestMethod()]
        [DataRow(0, 0)]
        [DataRow(1, 3.1416)]
        [DataRow(5.5, 95.033)]
        [DataRow(8.165, 209.441)]
        public void CircleAreaTest(double radius, double area)
        {
            Assert.AreEqual(area, Geometry.CircleArea(radius), 0.001 * area);
        }

        [TestMethod()]
        [DataRow(0, 0)]
        [DataRow(3.1416, 1)]
        [DataRow(95.033, 5.5)]
        [DataRow(209.441, 8.165)]
        public void CircleRadiusTest(double area, double radius)
        {
            Assert.AreEqual(radius, Geometry.CircleRadius(area), 0.001 * radius);
        }

        [TestMethod()]
        [DataRow(5, 0, 5, 10)]
        [DataRow(-10, 702, -10, -32)]
        [DataRow(5, 27, 500, 27)]
        [DataRow(385, -480, 35, -480)]
        [DataRow(5, 23, 504, 236)]
        [DataRow(543, 665, 52, 38)]
        [DataRow(523, 2, 74, 1009)]
        [DataRow(5, 2465, 749, 9)]
        public void RandomMidpointTest(int ax, int ay, int bx, int by)
        {
            Point a = new Point(ax, ay);
            Point b = new Point(bx, by);
            Line testLine = new Line(a, b);

            const int numTestPoints = 100;
            Point[] testPoints = new Point[numTestPoints];
            for (int i = 0; i < numTestPoints; i++)
            {
                testPoints[i] = Geometry.RandomMidpoint(a, b);
                Assert.IsTrue(testLine.LineSegmentContains(testPoints[i]));
            }

            double totalX = 0;
            double totalY = 0;
            for (int i = 0; i < numTestPoints; i++)
            {
                totalX += testPoints[i].X;
                totalY += testPoints[i].Y;
            }
            double meanX = totalX / numTestPoints;
            double meanY = totalY / numTestPoints;
            double expectedX = (ax + bx) / 2.0;
            double expectedY = (ay + by) / 2.0;
            double maxDeviation = testLine.Length / (2 * Math.Sqrt(numTestPoints));
            Assert.IsTrue(Math.Abs(expectedX - meanX) <= maxDeviation);
        }

        [TestMethod()]
        public void CenterPointTest()
        {
            List<Point> cluster = new List<Point>();
            cluster.Add(new Point(5, 0));
            cluster.Add(new Point(58, 403));
            cluster.Add(new Point(242, 38));
            cluster.Add(new Point(0, 0));
            cluster.Add(new Point(0, 857));
            Point expectedCenter = new Point(61, 260);
            Point actualCenter = Geometry.CenterPoint(cluster);
            Assert.IsTrue(expectedCenter.Equals(actualCenter));
        }

        [TestMethod()]
        public void RectangularityTest()
        {
            Blob blob = new Blob();
            blob.AddPixel(new Point(0, 0));
            blob.AddPixel(new Point(1, 0));
            blob.AddPixel(new Point(0, 1));
            blob.AddPixel(new Point(1, 1));
            Assert.AreEqual(1.0, Geometry.Rectangularity(blob), 0.01);
        }
    }
}