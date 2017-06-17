using System;

namespace RunescapeBot.Common
{
    public static class UnitConversions
    {
        /// <summary>
        /// Converts minutes to milliseconds
        /// </summary>
        /// <param name="minutes">number of minutes to convert</param>
        /// <returns>number of milliseconds in the given number of minutes rounded to the nearest millisecond</returns>
        public static int MinutesToMilliseconds(double minutes)
        {
            int milliseconds = (int) Math.Round(minutes * 60000);
            return milliseconds;
        }

        /// <summary>
        /// Converts hours to milliseconds
        /// </summary>
        /// <param name="hours">number of hours to convert</param>
        /// <returns>number of milliseconds in the given number of hours rounded to the nearest millisecond</returns>
        public static int HoursToMilliseconds(double hours)
        {
            int milliseconds = (int)Math.Round(hours * 3600000);
            return milliseconds;
        }
    }
}
