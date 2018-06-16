using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms.FixedUIComponents
{
    public class MinimapWaypoint
    {
        /// <summary>
        /// Angle in degress starting at due right and going counterclockwise
        /// </summary>
        public double Angle { get; set; }

        /// <summary>
        /// Fraction of the distance from the center to the edge of the minimap. (0-1)
        /// </summary>
        public double Radius { get; set; }

        /// <summary>
        /// Minimum time in milliseconds to wait before checking for stopping.
        /// </summary>
        public int MinWaitTime { get; set; }


        public MinimapWaypoint(double angle, double radius, int minWaitTime)
        {
            Angle = angle;
            Radius = radius;
            MinWaitTime = minWaitTime;
        }
    }
}
