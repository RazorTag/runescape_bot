using RunescapeBot.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using System.Collections.Generic;

namespace RunescapeBot.Common.Tests
{
    [TestClass()]
    public class NumericalTests
    {
        [TestMethod()]
        [DataRow(0, 0, 0, true)]
        [DataRow(50, 52, 2, true)]
        [DataRow(0, 100, 10, false)]
        [DataRow(100, 110, 115, true)]
        public void CloseEnoughTest(long target, long test, double tolerance, bool close)
        {
            Assert.AreEqual(Numerical.CloseEnough(target, test, tolerance), close);
        }

        [TestMethod()]
        [DataRow(new long[] { 0, 0 }, 0, 0, true)]
        [DataRow(new long[] { 224, 507 }, 500, 0.02, true)]
        [DataRow(new long[] { 224, 507 }, 500, 0.01, false)]
        public void CloseEnoughTest1(long[] targets, long test, double tolerance, bool close)
        {
            List<long> targetList = new List<long>();
            foreach (long target in targets)
            {
                targetList.Add(target);
            }
            Assert.AreEqual(Numerical.CloseEnough(targetList, test, tolerance), close);
        }

        [TestMethod()]
        [DataRow(0, 0, 0, true)]
        [DataRow(50, 52, 2, true)]
        [DataRow(0, 100, 10, false)]
        [DataRow(100, 110, 115, true)]
        [DataRow(107.54, 108.99, 2.5769, true)]
        public void WithinRangeTest(double test, double target, double offsetRange, bool close)
        {
            Assert.AreEqual(Numerical.WithinRange(target, test, offsetRange), close);
        }

        [TestMethod()]
        [DataRow(0, 0, 0, true)]
        [DataRow(5, 0, 10, true)]
        [DataRow(-235.245, -452.2153, 2234, true)]
        [DataRow(405, 500, 509, false)]
        [DataRow(450, 500, 400, false)]
        public void WithinBoundsTest(double test, double lowerBound, double upperBound, bool withinBounds)
        {
            Assert.AreEqual(Numerical.WithinBounds(test, lowerBound, upperBound), withinBounds);
        }

        [TestMethod()]
        [DataRow(0, 0, 0)]
        [DataRow(56, 130, 93)]
        [DataRow(-45.3, 82.9, 18.8)]
        public void AverageTest(double a, double b, double mean)
        {
            Assert.AreEqual(Numerical.Average(a, b), mean, 0.001);
        }

        [TestMethod()]
        [DataRow(0, 0, 0, 0, 0, 0)]
        [DataRow(25, 74, -254, -1540, -114, -733)]
        [DataRow(1, 1, 2, 2, 2, 2)]
        [DataRow(0, 0, -1, -1, 0, 0)]
        public void AveragePointsTest(int ax, int ay, int bx, int by, int cx, int cy)
        {
            Point a = new Point(ax, ay);
            Point b = new Point(bx, by);
            Point average = Numerical.Average(a, b);
            Point expected = new Point(cx, cy);
            Assert.IsTrue(expected.Equals(average));
        }

        [TestMethod()]
        [DataRow(0)]
        [DataRow(0.001)]
        [DataRow(0.01)]
        [DataRow(0.1)]
        [DataRow(1)]
        public void NonZeroTest(double possibleZero)
        {
            double nonZero = Numerical.NonZero(possibleZero);
            Assert.AreNotEqual(nonZero, 0.0);
            if (possibleZero == 0.0)
            {
                Assert.AreEqual(0.0, nonZero, 0.001);
            }
        }

        [TestMethod()]
        [DataRow(0, 0, true, 0)]
        [DataRow(2, 2, true, 4)]
        [DataRow(2, 2, false, 0)]
        [DataRow(5.634, -215.2, false, 220.834)]
        public void BooleanAddTest(double a, double b, bool add, double expectedResult)
        {
            double result = Numerical.BooleanAdd(a, b, add);
            Assert.AreEqual(expectedResult, result, 0.001);
        }

        [TestMethod()]
        [DataRow(0, 1, 0)]
        [DataRow(1, 1, 0)]
        [DataRow(5, 2, 1)]
        [DataRow(-5, 2, 1)]
        [DataRow(5, -2, -1)]
        [DataRow(-5, -2, -1)]
        public void ModuloTest(double value, double modulus, double expectedResult)
        {
            double result = Numerical.Modulo(value, modulus);
            Assert.AreEqual(expectedResult, result, 0.001);
        }

        [TestMethod()]
        [DataRow(-1, 0, 100, 0)]
        [DataRow(101, 0, 100, 100)]
        [DataRow(double.MaxValue, -55, 55, 55)]
        [DataRow(int.MinValue, 0, double.MaxValue, 0)]
        public void LimitToRangeTest(double input, double minimum, double maximum, double expectedResult)
        {
            double result = Numerical.LimitToRange(input, minimum, maximum);
            Assert.AreEqual(expectedResult, result);
        }
    }
}