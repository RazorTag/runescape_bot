using Microsoft.VisualStudio.TestTools.UnitTesting;
using RunescapeBot.Common;
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
            Assert.AreEqual(expected, distance, 0.01);
        }
    }
}