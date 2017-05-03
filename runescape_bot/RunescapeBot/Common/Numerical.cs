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
    }
}
