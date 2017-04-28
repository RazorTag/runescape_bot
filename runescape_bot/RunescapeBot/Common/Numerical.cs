using System;
using System.Collections.Generic;
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
        /// <returns>True if te values are close enough.</returns>
        public static bool CloseEnough(long target, long test, double tolerance)
        {
            long miss = Math.Abs(test - target);
            return miss < Math.Abs(target * tolerance);
        }
    }
}
