using System;
using System.Drawing;

namespace RunescapeBot.Common
{
    /// <summary>
    /// Represents an infinite line and a line segment
    /// </summary>
    public class Line
    {
        private const double infiniteSlope = 1000000000;

        //two points that define a line and a line seggment
        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }

        public double Rise { get { return EndPoint.Y - StartPoint.Y; } }
        public double Run { get { return EndPoint.X - StartPoint.X; } }
        public double Slope { get { return (Run != 0) ? (Rise / Run) : infiniteSlope; } }  //use an arbitrarily large number for slope in the event of a vertical line
        public double Length { get { return Math.Sqrt(Math.Pow(Rise, 2) + Math.Pow(Run, 2)); } }

        /// <summary>
        /// The distinction between start and end points is only meaningful for some operations on the line segment
        /// </summary>
        /// <param name="startPoint">start of the line segment</param>
        /// <param name="endPoint">end of the line segment</param>
        public Line(Point startPoint, Point endPoint)
        {
            this.StartPoint = startPoint;
            this.EndPoint = endPoint;
        }

        /// <summary>
        /// Calculates a point on the line based on a fraction of the line segment
        /// </summary>
        /// <param name="fraction">the fraction of the line segment from the start point toward the end point</param>
        /// <returns>a point on the line that is a line segment fraction from the start point toward the end point</returns>
        public Point LineSegmentFraction(double fraction)
        {
            double run = EndPoint.X - StartPoint.X;
            double rise = EndPoint.Y - StartPoint.Y;
            double x = StartPoint.X + (fraction * run);
            double y = StartPoint.Y + (fraction * rise);
            return new Point((int) Math.Round(x), (int) Math.Round(y));
        }

        /// <summary>
        /// Calculates a point that is a distance specified by offset along the line from start toward end
        /// </summary>
        /// <param name="offset">The distance along the line from the start point</param>
        /// <returns>A point on the line an offset away from the start point</returns>
        public Point OffsetFromStart(double offset)
        {
            return DirectionalOffset(offset, StartPoint, EndPoint);
        }

        /// <summary>
        /// Calculates a point that is a distance specified by offset along the line from end toward start
        /// </summary>
        /// <param name="offset">The distance along the line from the end point toward the start point</param>
        /// <returns>A point on the line an offset away from the end point toward the start point</returns>
        public Point OffsetFromEnd(double offset)
        {
            return DirectionalOffset(offset, EndPoint, StartPoint);
        }

        /// <summary>
        /// Moves along a line from a start point a specified distance toward an end point
        /// </summary>
        /// <param name="offset">the distance to move along the line</param>
        /// <param name="startPoint">the point to begin moving from</param>
        /// <param name="endPoint">defines the direction to move in</param>
        /// <returns>the point we ended up at after moving a specified distance from the start point toward the end point</returns>
        public static Point DirectionalOffset(double offset, Point startPoint, Point endPoint)
        {
            if (startPoint.Equals(endPoint)) { return startPoint; }

            double run = endPoint.X - startPoint.X;
            double rise = endPoint.Y - startPoint.Y;
            double x = startPoint.X + (offset * run);
            double y = startPoint.Y + (offset * rise);
            return new Point((int)Math.Round(x), (int)Math.Round(y));
        }

        /// <summary>
        /// Determines if a point is close to the line
        /// </summary>
        /// <param name="point">point to check for closeness</param>
        /// <param name="tolerance">allowed distance from the line</param>
        /// <returns>true if the point is sufficiently close to the line</returns>
        public bool LineContains(Point point, double tolerance = 1.415)
        {
            double slope = Slope;
            double yIntercept = StartPoint.Y - (slope * StartPoint.X);
            double normalSlope = (slope == 0) ? infiniteSlope : -Math.Pow(slope, -1);
            double normalYIntercept = point.Y - (normalSlope * point.X);
            double intersectionX = (normalYIntercept - yIntercept) / (slope - normalSlope);
            double intersectionY = yIntercept + slope * intersectionX;
            Point intersection = new Point((int)Math.Round(intersectionX), (int)Math.Round(intersectionY));
            double distanceFromLine = Geometry.DistanceBetweenPoints(point, intersection);
            return distanceFromLine <= tolerance;
        }

        /// <summary>
        /// Determines if a point is close to the line segment bounded by the start and end points
        /// </summary>
        /// <param name="point">point to check for closeness</param>
        /// <param name="tolerance">allowed distance from the line</param>
        /// <returns>true if the point is sufficiently close to the line segment</returns>
        public bool LineSegmentContains(Point point, double tolerance = 1.415)
        {
            if (!LineContains(point, tolerance)) { return false; }
            bool horizontalFit = ((StartPoint.X - tolerance) <= point.X && point.X <= (EndPoint.X + tolerance))
                || ((EndPoint.X - tolerance) <= point.X && point.X <= (StartPoint.X + tolerance));
            bool verticalFit = ((StartPoint.Y - tolerance) <= point.Y && point.Y <= (EndPoint.Y + tolerance))
                || ((EndPoint.Y - tolerance) <= point.Y && point.Y <= (StartPoint.Y + tolerance));
            return horizontalFit && verticalFit;
        }
    }
}
