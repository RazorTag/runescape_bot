using System;
using System.Drawing;

namespace RunescapeBot.Common
{
    public class Probability
    {
        /// <summary>
        /// Generates a random value from a Gaussian distribution
        /// </summary>
        /// <param name="mean">the average value for the distribution</param>
        /// <param name="stdDev">standard deviation. equal to the square root of variance.</param>
        /// <returns>a random number from a Gaussian distribution</returns>
        public static double RandomGaussian(double mean, double stdDev)
        {
            Random rng = new Random();
            double u1 = 1.0 - rng.NextDouble();
            double u2 = 1.0 - rng.NextDouble();
            double stdDevOffsets = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
            double randomValue = mean + stdDev * stdDevOffsets; //random normal(mean,stdDev^2)
            return randomValue;
        }

        /// <summary>
        /// Modifies a gaussian distribution to re-roll values that fall outside of the bounds.
        /// Be careful not to choose inputs such that the bounds enclose a very small portion of the probability density curve.
        /// This method could take a very long time to execute with improbable bounds.
        /// </summary>
        /// <param name="mean">the average value for the distribution</param>
        /// <param name="stdDev">standard deviation. equal to the square root of variance.</param>
        /// <param name="maxDeviation">random values are not allowed to deviate from the mean by more than the max deviation</param>
        /// <returns>a value from a Gaussian distribution within a maximum deviation</returns>
        public static double BoundedGaussian(double mean, double stdDev, double minValue, double maxValue)
        {
            double randomValue;
            do
            {
                randomValue = RandomGaussian(mean, stdDev);
            } while ((randomValue < minValue) || (randomValue > maxValue));

            return randomValue;
        }

        /// <summary>
        /// Modifies a gaussian distribution to re-roll values that deviate by more than the maximum.
        /// Generates the mean and stdDev to use based on the bounds.
        /// Mean is the midpoint of the bounds.
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <param name="stdDevToRangeRatio">the ratio of the standard deviation to the range of possible values</param>
        /// <returns></returns>
        public static double BoundedGaussian(double minValue, double maxValue)
        {
            const double stdDevToRangeRatio = 0.15;
            double mean = (maxValue + minValue) / 2.0;
            double stdDev = stdDevToRangeRatio * (maxValue - minValue);
            return BoundedGaussian(mean, stdDev, minValue, maxValue);
        }

        /// <summary>
        /// Folded Gaussian distribution where values are only possible on one side of what would be the mean for a standard normal distribution
        /// </summary>
        /// <param name="psuedoMean">the pseudo mean which serves as the lower limit by default or the upper limit if positive is set to false</param>
        /// <param name="stdDev">standard deviation</param>
        /// <param name="positive">set to true to skew positive with a lower bound. set to false to skew negative with an upper bound.</param>
        /// <param name="boundary">maximum variance for a a ppossible value</param>
        /// <returns>a random point from a one-sided Gaussian distribution</returns>
        public static double HalfGaussian(double psuedoMean, double stdDev, bool positive = true)
        {
            Random rng = new Random();
            double u1 = 1.0 - rng.NextDouble();
            double u2 = 1.0 - rng.NextDouble();
            double stdDevOffsets = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)

            double randomValue;
            if (positive)
            {
                randomValue = psuedoMean + stdDev * Math.Abs(stdDevOffsets); //upper half of random normal(mean,stdDev^2)
            }
            else
            {
                randomValue = psuedoMean + stdDev * -Math.Abs(stdDevOffsets); //lower half of random normal(mean,stdDev^2)
            }
            return randomValue;
        }

        /// <summary>
        /// Chooses a point from a rectangle based on a Gaussian distribution around the center along each axis
        /// </summary>
        /// <param name="left">left bound (inclusive)</param>
        /// <param name="right">right bound (inclusive)</param>
        /// <param name="top">top bound (inclusive)</param>
        /// <param name="bottom">bottom bound (inclusive)</param>
        /// /// <param name="stdDevToRangeRatio">the ratio of the standard deviation to the range of possible values</param>
        /// <returns></returns>
        public static Point GaussianRectangle(int left, int right, int top, int bottom, double stdDevToRangeRatio = 0.15)
        {
            double x = BoundedGaussian(left, right);
            double y = BoundedGaussian(top, bottom);
            Point point = new Point((int)Math.Round(x), (int)Math.Round(y));
            return point;
        }

        /// <summary>
        /// Chooses a point from a Gaussian distribution radiating outward from a central point
        /// </summary>
        /// <param name="center">the center point of the distribution</param>
        /// <param name="stdDev">standard deviation for the Gaussian distribution</param>
        /// <param name="arcStart">start of the arc from which values can be drawn</param>
        /// <param name="arcEnd">end of the arc from which values can be drawn</param>
        /// <param name="maxRadius">maximum permitted radius</param>
        /// <returns>a random point</returns>
        public static Point GaussianCircle(Point center, double stdDev, double arcStart = 0, double arcEnd = 360, double maxRadius = double.MaxValue)
        {
            Random rng = new Random();
            double radius = HalfGaussian(0, stdDev, true);
            radius %= maxRadius;

            arcStart = arcStart % 360;
            arcEnd = arcEnd % 360;
            if (arcStart > arcEnd)
            {
                arcEnd += 360;
            }
            double angle = arcStart + (rng.NextDouble() * (arcEnd - arcStart));
            angle = (angle % 360) * ((2 * Math.PI) / 360.0);    //normalize and convert to radians

            int x = center.X + ((int)Math.Round(Math.Cos(angle) * radius));
            int y = center.Y - ((int)Math.Round(Math.Sin(angle) * radius));

            return new Point(x, y);
        }
    }
}
