using System;
using System.Drawing;

namespace RunescapeBot.Common
{
    /// <summary>
    /// Represents an infinite line and a line segment
    /// </summary>
    public class Line
    {
        //two points that define a line and a line seggment
        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }

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
            double run = endPoint.X - startPoint.X;
            double rise = endPoint.Y - startPoint.Y;
            double length = Math.Sqrt(Math.Pow(run, 2.0) + Math.Pow(rise, 2.0));
            double runRate = run / length;
            double riseRate = rise / length;
            double x = startPoint.X + (offset * runRate);
            double y = startPoint.Y + (offset * riseRate);
            return new Point((int)Math.Round(x), (int)Math.Round(y));
        }
    }
}
