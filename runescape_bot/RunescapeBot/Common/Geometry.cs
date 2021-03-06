﻿using RunescapeBot.ImageTools;
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
            if (cluster == null || cluster.Count == 0) { return new Point(); }

            int xTotal = 0;
            int yTotal = 0;
            for (int i = 0; i < cluster.Count; i++)
            {
                xTotal += cluster[i].X;
                yTotal += cluster[i].Y;
            }
            int x = (int) Math.Round(xTotal / ((double)cluster.Count), 0);
            int y = (int) Math.Round(yTotal / ((double)cluster.Count), 0);

            return new Point(x, y);
        }

        /// <summary>
        /// Determines how rectangular a shape is. 0.5 is the minimum rectangularity for a rectangle tilted to a diamond position.
        /// </summary>
        /// <param name="shape">blob to check for rectangularity</param>
        /// <returns>rectangularity (0-1) indicating how rectangular a shape is</returns>
        public static double Rectangularity(Blob shape)
        {
            //TODO  Find the minimum area bounding rectangle instead of the vertical/horizontal bounding rectangle
            int width = shape.RightBound - shape.LeftBound + 1;
            int height = shape.BottomBound - shape.TopBound + 1;
            double interiorVolume = width * height;
            double rectangularity = shape.Size / interiorVolume;
            return rectangularity;
        }

        /// <summary>
        /// Returns the Blob in the supplied list that is the closest to the supplied point
        /// </summary>
        /// <param name="allMatches">list of blobs to check for proximity</param>
        /// <param name="center">comparison point</param>
        /// <returns>the blob that is closest to the specified point</returns>
        public static Blob ClosestBlobToPoint(List<Blob> allMatches, Point matchPoint)
        {
            if (allMatches == null || allMatches.Count == 0) {
                return null;
            }
            Blob foundBlob = allMatches[0];
            foreach (Blob currentBlob in allMatches)
            {
                if (DistanceBetweenPoints(matchPoint, currentBlob.Center) <= DistanceBetweenPoints(matchPoint,foundBlob.Center))
                {
                    foundBlob = currentBlob;
                }
            }
            return foundBlob;
        }

        /// <summary>
        /// Returns the Blob in the supplied list that is the farthest from the supplied point
        /// </summary>
        /// <param name="allMatches">list of blobs to check for proximity</param>
        /// <param name="center">comparison point</param>
        /// <returns>the blob that is farthest from the specified point</returns>
        public static Blob FarthestBlobFromPoint(List<Blob> allMatches, Point matchPoint)
        {
            if (allMatches == null || allMatches.Count == 0)
            {
                return null;
            }
            Blob foundBlob = allMatches[0];
            foreach (Blob currentBlob in allMatches)
            {
                if (DistanceBetweenPoints(matchPoint, currentBlob.Center) >= DistanceBetweenPoints(matchPoint, foundBlob.Center))
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

        /// <summary>
        /// Calculates the area of a rectangle based on its bounds.
        /// </summary>
        /// <param name="rectangle">Left, right, top, and bottom bounds of a rectangle.</param>
        /// <returns>Area of the rectange.</returns>
        public static int RectangleArea(RectangleBounds rectangle)
        {
            int width = rectangle.Right - rectangle.Left + 1;
            int height = rectangle.Bottom - rectangle.Top + 1;
            return width * height;
        }
    }
}
