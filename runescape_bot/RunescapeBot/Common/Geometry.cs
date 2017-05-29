﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.Common
{
    /// <summary>
    /// Utilities for geometric calculations
    /// </summary>
    public static class Geometry
    {
        /// <summary>
        /// Calculates the distance between two point in a 2D plane.
        /// </summary>
        /// <param name="a">Point a</param>
        /// <param name="b">Point b</param>
        /// <returns>The distance between the two points. Returns the max double value if at least one point is null.</returns>
        public static double DistanceBetweenPoints(Point? a, Point? b)
        {
            if (a == null || b == null)
            {
                return double.MaxValue;
            }

            int xDiff = a.Value.X - b.Value.X;
            int yDiff = a.Value.Y - b.Value.Y;
            return Math.Sqrt(Math.Pow(xDiff, 2.0) + Math.Pow(yDiff, 2.0));
        }

        /// <summary>
        /// Calculates the area of a circle from its radius
        /// </summary>
        /// <param name="radius"></param>
        /// <returns>the area of the circle</returns>
        public static double CircleArea(double radius)
        {
            return Math.PI * radius * radius;
        }

        /// <summary>
        /// Calculates the radius of a circle from its area
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public static double CircleRadius(double area)
        {
            return Math.Sqrt(area / Math.PI);
        }
    }
}
