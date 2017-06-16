using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.Common
{
    public class Numerical
    {
        /// <summary>
        /// Compares two values for approximate equality
        /// </summary>
        /// <param name="target">One of the numbers to compare. Used as the basis to determine the allowed range</param>
        /// <param name="test">One of the numbers to compare.</param>
        /// <param name="tolerance">The allowed deviation of the test value from the target value as a fraction of the target value. 0 means that the two values must be exactly equal.</param>
        /// <returns>True if the values are close enough.</returns>
        public static bool CloseEnough(long target, long test, double tolerance)
        {
            long miss = Math.Abs(test - target);
            return miss < Math.Abs(target * tolerance);
        }

        /// <summary>
        /// Determines if a value is within range of a target value
        /// </summary>
        /// <param name="test">the value to check if it is within a given range</param>
        /// <param name="target">center of the target range</param>
        /// <param name="offsetRange">positive and negative offset for the target range</param>
        /// <returns>true if the test value is within the target range</returns>
        public static bool WithinRange(double test, double target, double offsetRange)
        {
            return Math.Abs(test - target) <= offsetRange;
        }

        /// <summary>
        /// Determines if a value is within specified bounds
        /// </summary>
        /// <param name="test">the value to check if it is within the upper and lower bounds</param>
        /// <param name="lowerBound">minimum allowed value</param>
        /// <param name="upperBound">maximum allowed value</param>
        /// <returns>true if the test value is within bounds</returns>
        public static bool WithinBounds(double test, double lowerBound, double upperBound)
        {
            return (test >= lowerBound) && (test <= upperBound);
        }

        /// <summary>
        /// Takes the average of two numbers
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>The arithmetic mean of a and b</returns>
        public static double Average(int a, int b)
        {
            return (a + b) / 2.0;
        }

        /// <summary>
        /// Finds the midpoint of two points. Rounds left and up.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Point Average(Point a, Point b)
        {
            int x = (a.X + b.X) / 2;
            int y = (a.Y + b.Y) / 2;
            return new Point(x, y);
        }

        /// <summary>
        /// Adds a small positive value if the input value is exactly zero
        /// </summary>
        /// <param name="possibleZero"></param>
        /// <returns>something other than 0.0</returns>
        public static double NonZero(double possibleZero)
        {
            if (possibleZero == 0.0)
            {
                return 0.000000001;
            }
            else
            {
                return possibleZero;
            }
        }

        /// <summary>
        /// Generates a random value from a Gaussian distribution
        /// </summary>
        /// <param name="mean">the average value for the distribution</param>
        /// <param name="standardDeviation">standard deviation. equal to the square root of variance.</param>
        /// <returns>a random number from a Gaussian distribution</returns>
        public static double RandomGaussian(double mean, double standardDeviation)
        {
            Random rng = new Random();
            double u1 = 1.0 - rng.NextDouble();
            double u2 = 1.0 - rng.NextDouble();
            double stdDevOffsets = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
            double randomValue = mean + standardDeviation * stdDevOffsets; //random normal(mean,stdDev^2)
            return randomValue;
        }

        /// <summary>
        /// Modifies a gaussian distribution to re-roll values that deviate by more than the maximum 
        /// </summary>
        /// <param name="mean">the average value for the distribution</param>
        /// <param name="standardDeviation">standard deviation. equal to the square root of variance.</param>
        /// <param name="maxDeviation">random values are not allowed to deviate from the mean by more than the max deviation</param>
        /// <returns>a value from a Gaussian distribution within a maximum deviation</returns>
        public static double BoundedGaussian(double mean, double standardDeviation, double minValue, double maxValue)
        {
            double randomValue;
            do
            {
                randomValue = RandomGaussian(mean, standardDeviation);
            } while ((randomValue < minValue) || (randomValue > maxValue));

            return randomValue;
        }
    }
}
