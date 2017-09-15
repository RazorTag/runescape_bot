using RunescapeBot.ImageTools;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace RunescapeBot.Common
{
    /// <summary>
    /// Utilities for geometric calculations
    /// </summary>
    public static class Geometry
    {
        private static Random rng;
        private static Random RNG
        {
            get
            {
                if (rng == null) { rng = new Random(); }
                return rng;
            }
            set
            {
                rng = value;
            }
        }

        /// <summary>
        /// Calculates the distance between two points.
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
        /// Adds the x and y values for two points together
        /// </summary>
        /// <param name="a">first point</param>
        /// <param name="b">second point</param>
        /// <returns>a new point that is the sum of the given two points</returns>
        public static Point AddPoints(Point a, Point b)
        {
            int x = a.X + b.X;
            int y = a.Y + b.Y;
            return new Point(x, y);
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
        /// <returns>the radius of the circle</returns>
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
            int midX, midY;
            double bNess = RNG.NextDouble();
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

        /// <summary>
        /// Determines how rectangular a shape is
        /// </summary>
        /// <param name="shape">blob to check for rectangularity</param>
        /// <returns>rectangularity (0-1) indicating how rectangular a shape is</returns>
        public static double Rectangularity(Blob shape)
        {
            int width = shape.RightBound - shape.LeftBound + 1;
            int height = shape.BottomBound - shape.TopBound + 1;
            double interiorVolume = width * height;
            double rectangularity = shape.Size / interiorVolume;
            return rectangularity;
        }

        /// <summary>
        /// Returns the Blob in the supplied list that is the closest to the supplied point
        /// </summary>
        /// <param name="allMatches"></param>
        /// <param name="center"></param>
        /// <returns></returns>
        public static Blob ClosestBlobToPoint(List<Blob> allMatches, Point matchPoint)
        {
            if (allMatches == null || allMatches.Count == 0) {
                return null;
            }
            Blob foundBlob = allMatches[0];
            foreach (Blob currentBlob in allMatches)
            {
                if (DistanceBetweenPoints(matchPoint,foundBlob.Center) >= DistanceBetweenPoints(matchPoint,currentBlob.Center))
                {
                    foundBlob = currentBlob;
                }
            }
            return foundBlob;
        }

        /// <summary>
        /// Adds all of the points within a circle definition to an existing blob
        /// </summary>
        /// <param name="blob"></param>
        /// <param name="circleCenter"></param>
        /// <param name="circleRadius"></param>
        public static void AddMinimapIconToBlob(ref Blob blob, Point circleCenter)
        {
            const int circleRadius = 7;
            double maxRadius = circleRadius + Numerical.NonZero(0);
            Point point;

            for (int x = circleCenter.X - circleRadius; x <= circleCenter.X + circleCenter.X; x++)
            {
                for (int y = circleCenter.Y - circleRadius; y <= circleCenter.Y + circleRadius; y++)
                {
                    point = new Point(x, y);
                    if (DistanceBetweenPoints(point, circleCenter) <= maxRadius)
                    {
                        blob.AddPixel(point, true);
                    }
                }
            }
        }
    }
}
