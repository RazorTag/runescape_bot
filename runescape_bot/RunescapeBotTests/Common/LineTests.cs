using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;

namespace RunescapeBot.Common.Tests
{
    [TestClass()]
    public class LineTests
    {
        [TestMethod()]
        [DataRow(0, 0, 0, 0, 0)]
        [DataRow(4, 8, 9, 0, -8)]
        [DataRow(52, -57, 765, 82, 139)]
        public void RiseTest(int ax, int ay, int bx, int by, int expectedRise)
        {
            Line line = new Line(new Point(ax, ay), new Point(bx, by));
            double rise = line.Rise;
            Assert.AreEqual(expectedRise, rise, 0.001);
        }

        [TestMethod()]
        [DataRow(0, 0, 0, 0, 0)]
        [DataRow(4, 8, 9, 0, 5)]
        [DataRow(52, -57, 765, 82, 713)]
        public void RunTest(int ax, int ay, int bx, int by, int expectedRun)
        {
            Line line = new Line(new Point(ax, ay), new Point(bx, by));
            double run = line.Run;
            Assert.AreEqual(expectedRun, run, 0.001);
        }

        [TestMethod()]
        [DataRow(4, 8, 9, 0, -1.6)]
        [DataRow(52, -57, 765, 82, 0.194951)]
        public void SlopeTest(int ax, int ay, int bx, int by, double expectedSlope)
        {
            Line line = new Line(new Point(ax, ay), new Point(bx, by));
            double slope = line.Slope;
            Assert.AreEqual(expectedSlope, slope, 0.001);
        }

        [TestMethod()]
        [DataRow(0, 0, 0, 0, 0)]
        [DataRow(4, 8, 9, 0, 9.433981)]
        [DataRow(52, -57, 765, 82, 726.422742)]
        public void LengthTest(int ax, int ay, int bx, int by, double expectedLength)
        {
            Line line = new Line(new Point(ax, ay), new Point(bx, by));
            double length = line.Length;
            Assert.AreEqual(expectedLength, length, 0.001);
        }

        [TestMethod()]
        public void LineTest()
        {
            Point a = new Point(5, 5);
            Point b = new Point(10, 0);
            Line line = new Line(a, b);
            Assert.AreEqual(a, line.StartPoint);
            Assert.AreEqual(b, line.EndPoint);
        }

        [TestMethod()]
        [DataRow(0, 0, 0, 0, 0, 0, 0)]
        [DataRow(0, 0, 2, 0, 0.5, 1, 0)]
        [DataRow(524, 30, -34, 509, 0.1, 468, 78)]
        [DataRow(634743, 3462, 256253, 25457547, 0.8, 331951, 20366730)]
        public void LineSegmentFractionTest(int ax, int ay, int bx, int by, double fraction, int cx, int cy)
        {
            Point expected = new Point(cx, cy);
            Line line = new Line(new Point(ax, ay), new Point(bx, by));
            Point actual = line.LineSegmentFraction(fraction);
            Assert.IsTrue(expected.Equals(actual));
        }

        [TestMethod()]
        [DataRow(0, 0, 0, 0, 0, 0, 0)]
        [DataRow(0, 0, 2, 0, 1, 1, 0)]
        [DataRow(524, 30, -34, 509, 10, 516, 37)]
        public void OffsetFromStartTest(int ax, int ay, int bx, int by, double offset, int cx, int cy)
        {
            Point expected = new Point(cx, cy);
            Line line = new Line(new Point(ax, ay), new Point(bx, by));
            Point actual = line.OffsetFromStart(offset);
            Assert.IsTrue(expected.Equals(actual));
        }

        [TestMethod()]
        [DataRow(0, 0, 0, 0, 0, 0, 0)]
        [DataRow(0, 0, 2, 0, 1, 1, 0)]
        [DataRow(524, 30, -34, 509, 10, -26, 502)]
        public void OffsetFromEndTest(int ax, int ay, int bx, int by, double offset, int cx, int cy)
        {
            Point expected = new Point(cx, cy);
            Line line = new Line(new Point(ax, ay), new Point(bx, by));
            Point actual = line.OffsetFromEnd(offset);
            Assert.IsTrue(expected.Equals(actual));
        }

        [TestMethod()]
        [DataRow(0, 0, 0, 0, 0, 0, 0)]
        [DataRow(0, 0, 2, 0, 1, 1, 0)]
        [DataRow(524, 30, -34, 509, 10, 516, 37)]
        public void DirectionalOffsetTest(int ax, int ay, int bx, int by, double offset, int cx, int cy)
        {
            Point start = new Point(ax, ay);
            Point end = new Point(bx, by);
            Point expected = new Point(cx, cy);
            Point actual = Line.DirectionalOffset(offset, start, end);
            Assert.IsTrue(expected.Equals(actual));
        }

        [TestMethod()]
        [DataRow(0, 0, 1, 0, 1, 0, 0, true)]
        [DataRow(0, 0, 1, 0, 1, 1, 0, true)]
        [DataRow(0, 0, 0, 2, 1, 0, 1, true)]
        [DataRow(77, 66, 391, 255, 2, 276, 185, true)]
        [DataRow(77, 66, 391, 255, 2, 276, 190, false)]
        [DataRow(77, 66, 391, 255, 2, 411, 267, true)]
        [DataRow(77, 66, 391, 255, 2, 62, 57, true)]
        [DataRow(77, 66, 391, 255, 2, 295, 127, false)]
        [DataRow(77, 66, 391, 255, 2, 125, 203, false)]
        public void LineContainsTest(int ax, int ay, int bx, int by, double tolerance, int cx, int cy, bool contains)
        {
            Point start = new Point(ax, ay);
            Point end = new Point(bx, by);
            Line line = new Line(start, end);
            Point point = new Point(cx, cy);
            bool lineContains = line.LineContains(point, tolerance);
            Assert.AreEqual(contains, lineContains);
        }

        [TestMethod()]
        [DataRow(0, 0, 1, 0, 1, 0, 0, true)]
        [DataRow(0, 0, 1, 0, 1, 1, 0, true)]
        [DataRow(0, 0, 0, 2, 1, 0, 1, true)]
        [DataRow(77, 66, 391, 255, 2, 276, 185, true)]
        [DataRow(77, 66, 391, 255, 2, 276, 190, false)]
        [DataRow(77, 66, 391, 255, 2, 411, 267, false)]
        [DataRow(77, 66, 391, 255, 2, 62, 57, false)]
        [DataRow(77, 66, 391, 255, 2, 295, 127, false)]
        [DataRow(77, 66, 391, 255, 2, 125, 203, false)]
        public void LineSegmentContainsTest(int ax, int ay, int bx, int by, double tolerance, int cx, int cy, bool contains)
        {
            Point start = new Point(ax, ay);
            Point end = new Point(bx, by);
            Line line = new Line(start, end);
            Point point = new Point(cx, cy);
            bool lineContains = line.LineSegmentContains(point, tolerance);
            Assert.AreEqual(contains, lineContains);
        }
    }
}