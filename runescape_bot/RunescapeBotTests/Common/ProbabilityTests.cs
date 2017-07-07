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
    public class ProbabilityTests
    {
        const int numSamples = 100;

        [TestMethod()]
        [DataRow(0, 0)]
        [DataRow(100, 15)]
        public void RandomGaussianTest(double mean, double stdDev)
        {
            double total = 0;
            double sample;
            for (int i = 0; i < numSamples; i++)
            {
                sample = Probability.RandomGaussian(mean, stdDev);
                total += sample;
            }
            double average = total / ((double)numSamples);
            double maxDeviation = 10 * (stdDev / Math.Sqrt(numSamples));

            Assert.AreEqual(mean, average, maxDeviation);
        }

        [TestMethod()]
        [DataRow(0, 1, -2, 2, 0)]
        [DataRow(100, 15, double.MinValue, double.MaxValue, 100)]
        public void BoundedGaussianTest(double mean, double stdDev, double minValue, double maxValue, double expectedMean)
        {
            double total = 0;
            double sample;
            for (int i = 0; i < numSamples; i++)
            {
                sample = Probability.BoundedGaussian(mean, stdDev, minValue, maxValue);
                Assert.IsFalse(sample < minValue);
                Assert.IsFalse(sample > maxValue);
                total += sample;
            }
            double average = total / ((double)numSamples);
            double maxDeviation = 10 * (stdDev / Math.Sqrt(numSamples));

            Assert.AreEqual(expectedMean, average, maxDeviation);
        }

        [TestMethod()]
        [DataRow(10, 20)]
        [DataRow(-1, 1)]
        public void BoundedGaussianRangeTest(double minValue, double maxValue)
        {
            double total = 0;
            double sample;
            for (int i = 0; i < numSamples; i++)
            {
                sample = Probability.BoundedGaussian(minValue, maxValue);
                Assert.IsFalse(sample < minValue);
                Assert.IsFalse(sample > maxValue);
                total += sample;
            }
            double average = total / ((double)numSamples);
            double maxDeviation = 2.5 * ((maxValue - minValue) / Math.Sqrt(numSamples));
            double expectedMean = Numerical.Average(minValue, maxValue);
        }

        [TestMethod()]
        [DataRow(0, 1, true)]
        [DataRow(100, 15, false)]
        public void HalfGaussianTest(double psuedoMean, double stdDev, bool positive)
        {
            double total = 0;
            double sample;
            for (int i = 0; i < numSamples; i++)
            {
                sample = Probability.HalfGaussian(psuedoMean, stdDev, positive);
                if (positive)
                {
                    Assert.IsTrue(sample >= psuedoMean);
                }
                else
                {
                    Assert.IsTrue(sample <= psuedoMean);
                }
                total += sample;
            }
            double average = total / ((double)numSamples);
            double expectedMean = Numerical.BooleanAdd(psuedoMean, stdDev * (Math.Sqrt(2) / Math.Sqrt(Math.PI)), positive);
            double expectedStdDev = stdDev * Math.Sqrt(1 - (2 / Math.PI));
            double maxDeviation = 10 * (expectedStdDev / Math.Sqrt(numSamples));

            Assert.AreEqual(expectedMean, average, maxDeviation);
        }

        [TestMethod()]
        [DataRow(0, 1, 0, 1)]
        [DataRow(0, 10, 0, 10)]
        [DataRow(52, 1070, -49, 101)]
        public void GaussianRectangleTest(int left, int right, int top, int bottom)
        {
            double totalX = 0;
            double totalY = 0;
            Point sample;
            for (int i = 0; i < numSamples; i++)
            {
                sample = Probability.GaussianRectangle(left, right, top, bottom);
                Assert.IsFalse(sample.X < left);
                Assert.IsFalse(sample.X > right);
                Assert.IsFalse(sample.Y < top);
                Assert.IsFalse(sample.Y > bottom);
                totalX += sample.X;
                totalY += sample.Y;
            }
            double meanX = totalX / numSamples;
            double meanY = totalY / numSamples;
            double expectedX = Numerical.Average(left, right);
            double expectedY = Numerical.Average(top, bottom);
            double maxHorizontalDispersion = 2.5 * ((right - left) / Math.Sqrt(numSamples));
            double maxVerticalDispersion = 2.5 * ((bottom - top) / Math.Sqrt(numSamples));
            Assert.AreEqual(expectedX, meanX, maxHorizontalDispersion);
            Assert.AreEqual(expectedY, meanY, maxVerticalDispersion);
        }

        [TestMethod()]
        [DataRow(0, 0, 100, 0, 360, 300)]
        [DataRow(0, 0, 200, -10, -50, 1000)]
        [DataRow(0, 0, 10, -10, 10, double.MaxValue)]
        public void GaussianCircleTest(int x, int y, double stdDev, double arcStart, double arcEnd, double maxRadius)
        {
            Point center = new Point(x, y);
            double totalX = 0;
            double totalY = 0;
            Point sample;
            double radius;

            for (int i = 0; i < numSamples; i++)
            {
                sample = Probability.GaussianCircle(center, stdDev, arcStart, arcEnd, maxRadius);
                radius = Geometry.DistanceBetweenPoints(center, sample);
                Assert.IsTrue(radius <= (maxRadius + 1));
                totalX += sample.X;
                totalY += sample.Y;
            }
            double maxDeviation = 10 * (stdDev / Math.Sqrt(numSamples));
            int meanX = (int)Math.Round(totalX / numSamples);
            int meanY = (int)Math.Round(totalY / numSamples);
            double meanRadius = Geometry.DistanceBetweenPoints(center, new Point(meanX, meanY));
            Assert.IsTrue(meanRadius < maxDeviation);
        }

        [TestMethod()]
        [DataRow(-10, -50)]
        [DataRow(-10, 10)]
        [DataRow(0, 360)]
        [DataRow(100, -290)]
        public void RandomAngleTest(double arcStart, double arcEnd)
        {
            double angle = Probability.RandomAngle(arcStart, arcEnd);
            arcStart = Numerical.Modulo(arcStart, 360);
            arcEnd = Numerical.Modulo(arcEnd, 360);
            if (arcStart >= arcEnd)
            {
                arcEnd += 360;
            }
            if (angle < arcStart)
            {
                angle += 360;
            }
            Assert.IsFalse(angle < arcStart);
            Assert.IsFalse(angle > arcEnd);
        }
    }
}