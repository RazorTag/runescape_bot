using System;
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
            if (a == null || b == null) { return double.MaxValue; }

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

        /// <summary>
        /// Randomly chooses a point on the line segment connecting the two points
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>a random point between the two given points</returns>
        public static Point RandomMidpoint(Point a, Point b)
        {
            Random rng = new Random();
            int midX, midY;
            double bNess = rng.NextDouble();
            midX = (int)Math.Round((bNess * (b.X - a.X)) + a.X);
            midY = (int)Math.Round((bNess * (b.Y - a.Y)) + a.Y);
            return new Point(midX, midY);
        }

        /// <summary>
        /// Calculates the center of mass of a collection of points
        /// </summary>
        /// <param name="cluster"></param>
        /// <returns>the averae of the list of points</returns>
        public static Point CenterPoint(List<Point> cluster)
        {
            if (cluster == null) { return new Point(); }

            int xTotal = 0;
            int yTotal = 0;
            for (int i = 0; i < cluster.Count; i++)
            {
                xTotal += cluster[i].X;
                yTotal += cluster[i].Y;
            }
            int x = (int) Math.Round(xTotal / ((double)cluster.Count));
            int y = (int) Math.Round(yTotal / ((double)cluster.Count));

            return new Point(x, y);
        }
    }
}
