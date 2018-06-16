using System;
using System.Collections.Generic;
using System.Drawing;

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
            return miss <= Math.Abs(target * tolerance);
        }

        /// <summary>
        /// Compares a value against a list of values to see if any of them are close enough
        /// </summary>
        /// <param name="targets">list of values for comparison</param>
        /// <param name="test">single value to compare against each target value</param>
        /// <param name="tolerance">fraction of the target value by which the test value is allowed to diviate from the target value</param>
        /// <returns>true if a match is found</returns>
        public static bool CloseEnough(List<long> targets, long test, double tolerance)
        {
            foreach (long target in targets)
            {
                if (CloseEnough(target, test, tolerance))
                {
                    return true;
                }
            }
            return false;
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
        /// Determines if a value is within specified bounds inclusive
        /// </summary>
        /// <param name="test">the value to check if it is within the upper and lower bounds</param>
        /// <param name="lowerBound">minimum allowed value (inclusive)</param>
        /// <param name="upperBound">maximum allowed value (inclusive)</param>
        /// <returns>true if the test value is within bounds</returns>
        public static bool WithinBounds(double test, double lowerBound, double upperBound)
        {
            return (test >= lowerBound) && (test <= upperBound);
        }

        /// <summary>
        /// Coerces a vlue to the nearest value within a specified range if the given value is outside of the range
        /// </summary>
        /// <param name="input">value to coerce</param>
        /// <param name="minimum">lowest accpetable value</param>
        /// <param name="maximum">highest acceptable value</param>
        /// <returns>The nearest value to input within the rang specified by minimum and maximum</returns>
        public static double LimitToRange(double input, double minimum, double maximum)
        {
            if (input < minimum) { return minimum; }
            if (input > maximum) { return maximum; }
            return input;
        }

        /// <summary>
        /// Takes the average of two numbers
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>The arithmetic mean of a and b</returns>
        public static double Average(double a, double b)
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
            int x = (int) Math.Round((a.X + b.X) / 2.0);
            int y = (int) Math.Round((a.Y + b.Y) / 2.0);
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
                return 0.000001;
            }
            else
            {
                return possibleZero;
            }
        }

        /// <summary>
        /// Adds or subtracts two numerical values based on a boolean value
        /// </summary>
        /// <param name="a">first number</param>
        /// <param name="b">number to add or subtract</param>
        /// <param name="add">true to add, false to subtract</param>
        /// <returns>sum or difference</returns>
        public static double BooleanAdd(double a, double b, bool add)
        {
            if (add)
            {
                return a + b;
            }
            else
            {
                return a - b;
            }
        }

        /// <summary>
        /// Takes the modulus of a value
        /// </summary>
        /// <param name="value">value to be modulated</param>
        /// <returns>modulation of a value</returns>
        public static double Modulo(double value, double modulus)
        {
            return value - modulus * Math.Floor(value / modulus);
        }
    }
}
