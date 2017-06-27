using RunescapeBot.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace RunescapeBot.ImageTools
{
    public class Blob
    {
        public Blob()
        {
            Pixels = new Dictionary<Point, Point>();
            FoundPixels = new Queue<Point>();
        }

        /// <summary>
        /// List of pixels in the blob.
        /// </summary>
        private Dictionary<Point, Point> Pixels { get; set; }

        /// <summary>
        /// Found and unprocessed pixels. Need to be checked for neighbors.
        /// </summary>
        private Queue<Point> FoundPixels { get; set; }

        /// <summary>
        /// Caches the center point of the blob
        /// </summary>
        private Point? center;
        public Point Center
        {
            get
            {
                if (center == null)
                {
                    int totalX = 0;
                    int totalY = 0;

                    foreach (KeyValuePair<Point, Point> pixel in Pixels)
                    {
                        totalX += pixel.Value.X;
                        totalY += pixel.Value.Y;
                    }
                    if (Size == 0)
                    {
                        center = new Point(0, 0);
                    }
                    else
                    {
                        center = new Point(totalX / Size, totalY / Size);
                    }
                }
                
                return (Point)center;
            }
        }

        /// <summary>
        /// Returns the horizontal distance between left and right bounds
        /// </summary>
        public int Width
        {
            get
            {
                return RightBound - LeftBound + 1;
            }
        }

        /// <summary>
        /// Returns the vertical distance between top and bottom bounds
        /// </summary>
        public int Height
        {
            get
            {
                return BottomBound - TopBound + 1;
            }
        }

        /// <summary>
        /// True if the current value of the bounds are correct
        /// </summary>
        private bool boundsCalculated;

        /// <summary>
        /// The left most x-value of any pixel in the blob
        /// </summary>
        private int leftBound;
        public int LeftBound
        {
            get
            {
                if (!boundsCalculated)
                {
                    FindBounds();
                }
                return leftBound;
            }
        }

        /// <summary>
        /// The right most x-value of any pixel in the blob
        /// </summary>
        private int rightBound;
        public int RightBound
        {
            get
            {
                if (!boundsCalculated)
                {
                    FindBounds();
                }
                return rightBound;
            }
        }

        /// <summary>
        /// The top most x-value of any pixel in the blob
        /// </summary>
        private int topBound;
        public int TopBound
        {
            get
            {
                if (!boundsCalculated)
                {
                    FindBounds();
                }
                return topBound;
            }
        }

        /// <summary>
        /// The bottom most x-value of any pixel in the blob
        /// </summary>
        private int bottomBound;
        public int BottomBound
        {
            get
            {
                if (!boundsCalculated)
                {
                    FindBounds();
                }
                return bottomBound;
            }
        }

        /// <summary>
        /// Number of pixels that make up this blob
        /// </summary>
        public int Size
        {
            get
            {
                return Pixels.Count;
            }
        }

        /// <summary>
        /// Determines the left, right, top, and bottom most pixels in the blob
        /// </summary>
        private void FindBounds()
        {
            if (Size == 0)
            {
                leftBound = 0;
                rightBound = 0;
                topBound = 0;
                bottomBound = 0;
            }
            else
            {
                leftBound = Int32.MaxValue;
                rightBound = Int32.MinValue;
                topBound = Int32.MaxValue;
                bottomBound = Int32.MinValue;

                foreach (KeyValuePair<Point, Point> pixel in Pixels)
                {
                    leftBound = Math.Min(leftBound, pixel.Value.X);
                    rightBound = Math.Max(rightBound, pixel.Value.X);
                    topBound = Math.Min(topBound, pixel.Value.Y);
                    bottomBound = Math.Max(bottomBound, pixel.Value.Y);
                }
            }
        }

        /// <summary>
        /// Finds the pixel at the top of the blob
        /// </summary>
        /// <returns>the average of the pixel(s) at the upper bound of the blob</returns>
        public Point GetTop()
        {
            int topBound = TopBound;
            List<Point> topPixels = new List<Point>();

            foreach (KeyValuePair<Point, Point> pixel in Pixels)
            {
                if (pixel.Value.Y == topBound)
                {
                    topPixels.Add(pixel.Value);
                }
            }
            return Geometry.CenterPoint(topPixels);
        }

        /// <summary>
        /// Finds the pixel at the bottom of the blob
        /// </summary>
        /// <returns>the average of the pixel(s) at the bottom bound of the blob</returns>
        public Point GetBottom()
        {
            int bottomBound = BottomBound;
            List<Point> bottomPixels = new List<Point>();

            foreach (KeyValuePair<Point, Point> pixel in Pixels)
            {
                if (pixel.Value.Y == bottomBound)
                {
                    bottomPixels.Add(pixel.Value);
                }
            }
            return Geometry.CenterPoint(bottomPixels);
        }

        /// <summary>
        /// Finds the pixel at the left of the blob
        /// </summary>
        /// <returns>the average of the pixel(s) at the left bound of the blob</returns>
        public Point GetLeft()
        {
            int leftBound = LeftBound;
            List<Point> leftPixels = new List<Point>();

            foreach (KeyValuePair<Point, Point> pixel in Pixels)
            {
                if (pixel.Value.X == leftBound)
                {
                    leftPixels.Add(pixel.Value);
                }
            }
            return Geometry.CenterPoint(leftPixels);
        }

        /// <summary>
        /// Finds the pixel at the right of the blob
        /// </summary>
        /// <returns>the average of the pixel(s) at the right bound of the blob</returns>
        public Point GetRight()
        {
            int rightBound = RightBound;
            List<Point> rightPixels = new List<Point>();

            foreach (KeyValuePair<Point, Point> pixel in Pixels)
            {
                if (pixel.Value.X == rightBound)
                {
                    rightPixels.Add(pixel.Value);
                }
            }
            return Geometry.CenterPoint(rightPixels);
        }

        /// <summary>
        /// Gets the top-left most pixel
        /// </summary>
        /// <returns>the point from the blob with the greatest combination of leftness and topness</returns>
        public Point GetTopLeft()
        {
            if (Pixels.Count == 0) {  return new Point();}

            int left = -1;
            int top = -1;

            foreach (KeyValuePair<Point, Point> pixel in Pixels)
            {
                if (left == -1)
                {
                    left = pixel.Value.X;
                    top = pixel.Value.Y;
                }
                else
                {
                    if ((left - pixel.Value.X) + (top - pixel.Value.Y) > 0)
                    {
                        left = pixel.Value.X;
                        top = pixel.Value.Y;
                    }
                }
            }
            return new Point(left, top);
        }

        /// <summary>
        /// Calculates the distance to another point
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public double DistanceTo(Point point)
        {
            int rise = Center.Y - point.Y;
            int run = Center.X - point.X;
            return Math.Sqrt((rise * rise) + (run * run));
        }

        /// <summary>
        /// Add a new pixel to the blob
        /// </summary>
        /// <param name="newPixel"></param>
        public bool AddPixel(Point newPixel)
        {
            if (!ContainsPixel(newPixel))
            {
                FoundPixels.Enqueue(newPixel);
                Pixels.Add(newPixel, newPixel);
                center = null;
                boundsCalculated = false;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Add a new pixel to the blob
        /// </summary>
        /// <param name="newPixel"></param>
        public bool AddPixel(int x, int y)
        {
            Point newPixel = new Point(x, y);
            return AddPixel(newPixel);
        }

        /// <summary>
        /// Add all of the pixels from another blob to this blob
        /// </summary>
        /// <param name="blob"></param>
        public void AddBlob(Blob blob)
        {
            foreach (KeyValuePair<Point, Point> pixel in blob.Pixels)
            {
                AddPixel(pixel.Value);
            }
        }

        /// <summary>
        /// The next pixel to be checked for neighbors
        /// </summary>
        /// <returns></returns>
        public Point NextPixel()
        {
            return FoundPixels.Dequeue();
        }

        /// <summary>
        /// Determines if there any pixels left to check for neighbors
        /// </summary>
        /// <returns></returns>
        public bool HasPixelsToProcess()
        {
            return FoundPixels.Count > 0;
        }

        /// <summary>
        /// Determines if a point exists in this blob
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool ContainsPixel(Point pixel)
        {
            return Pixels.ContainsKey(pixel);
        }

        /// <summary>
        /// Determines if a point exists in this blob
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool ContainsPixel(int x, int y)
        {
            Point pixel = new Point(x, y);
            return ContainsPixel(pixel);
        }

        /// <summary>
        /// Returns the larger of this and another blob
        /// </summary>
        /// <param name="otherBlob"></param>
        /// <returns></returns>
        public Blob MaxBlob(Blob otherBlob)
        {
            if (otherBlob == null)
            {
                return this;
            }
            if (otherBlob.Size > this.Size)
            {
                return otherBlob;
            }
            else
            {
                return this;
            }
        }

        /// <summary>
        /// Sorts a list of blobs in ascending order by Size
        /// </summary>
        /// <param name="blobs"></param>
        /// <returns></returns>
        public static List<Blob> SortBlobs(List<Blob> blobs)
        {
            return blobs.OrderByDescending(o => o.Size).ToList();
        }

        /// <summary>
        /// Picks a random pixel within the blob
        /// </summary>
        /// <returns>a random blob pixel's coordinates in terms of the game screen</returns>
        public Point? RandomBlobPixel()
        {
            if (Pixels.Count == 0) { return null; }

            Random rng = new Random();
            return Pixels.ElementAt(rng.Next(0, Pixels.Count)).Value;
        }

        /// <summary>
        /// Creates a new blob made up of all of the unique pixels from all of the blobs in the list
        /// </summary>
        /// <param name="blobs">list of blobs to combine</param>
        /// <returns>new combined blob</returns>
        public static Blob Combine(List<Blob> blobs)
        {
            if (blobs == null || blobs.Count == 0) { return null; }

            Blob megaBlob = new Blob();
            for (int i = 0; i < blobs.Count; i++)
            {
                megaBlob.AddBlob(blobs[i]);
            }
            return megaBlob;
        }

        /// <summary>
        /// Finds the closest blob in a list of blobs
        /// </summary>
        /// <param name="center">center point to search from</param>
        /// <param name="blobs">list of blobs to search</param>
        /// <returns>the blob from the blobs list that is closest to the center point</returns>
        public static Blob ClosestBlob(Point center, List<Blob> blobs)
        {
            double minDistance = double.MaxValue;
            double nextDistance;
            Blob closestBlob = null;

            for (int i = 0; i < blobs.Count; i++)
            {
                nextDistance = Geometry.DistanceBetweenPoints(center, blobs[i].Center);
                if (nextDistance < minDistance)
                {
                    minDistance = nextDistance;
                    closestBlob = blobs[i];
                }
            }
            return closestBlob;
        }

        #region feature matching

        /// <summary>
        /// Determines if the blob is apprximately fits a disk.
        /// i.e. hollow center surrounded by a ring of finite thickness or a solid disk
        /// </summary>
        /// <param name="minimumHollow">minimum required inner radius as a fraction of what the outer radius would be with an inner radius of 0</param>
        /// <returns></returns>
        public bool IsCircle(double minimumHollow = 0.0)
        {
            const double allowedRadiusFactor = 1.3;
            const double requiredInsidePortion = 0.9;

            //determine the expected outer radius of the circle
            Point closestPixel = ClosestPixel(Center);
            Point farthestPixel = FarthestPixel(Center);
            double innerRadius = Geometry.DistanceBetweenPoints(Center, closestPixel);
            double minInnerRadius = minimumHollow * Geometry.CircleRadius(Size);
            if (innerRadius < minInnerRadius)
            {
                return false;   //the blob is not hollow enough
            }
            double diskArea = Size + Geometry.CircleArea(innerRadius);
            double outerRadius = Geometry.CircleRadius(diskArea);

            //determine the portion of the blob that fits inside of the expected outer radius
            double allowedRadius = allowedRadiusFactor * outerRadius;
            int insidePixels = 0;
            foreach (KeyValuePair<Point, Point> pixel in Pixels)
            {
                if (Geometry.DistanceBetweenPoints(Center, pixel.Value) <= allowedRadius)
                {
                    insidePixels++;
                }
            }
            double insidePortion = insidePixels / ((double)Size);

            return insidePortion >= requiredInsidePortion;
        }

        /// <summary>
        /// Finds the pixel in the blob that is closest to the start point
        /// </summary>
        /// <param name="start"></param>
        /// <returns>the closest pixel in the blob</returns>
        private Point ClosestPixel(Point start)
        {
            double closestDistance = Double.MaxValue;
            double distance;
            Point closestPoint = new Point();

            foreach (KeyValuePair<Point, Point> pixel in Pixels)
            {
                distance = Geometry.DistanceBetweenPoints(start, pixel.Value);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPoint = pixel.Value;
                }
            }
            return closestPoint;
        }

        /// <summary>
        /// Finds the pixel in the blob that is farthest from the start point
        /// </summary>
        /// <param name="start"></param>
        /// <returns>the closest pixel in the blob</returns>
        private Point FarthestPixel(Point start)
        {
            double farthestDistance = Double.MinValue;
            double distance;
            Point farthestPoint = new Point();

            foreach (KeyValuePair<Point, Point> pixel in Pixels)
            {
                distance = Geometry.DistanceBetweenPoints(start, pixel.Value);
                if (distance > farthestDistance)
                {
                    farthestDistance = distance;
                    farthestPoint = pixel.Value;
                }
            }
            return farthestPoint;
        }

        #endregion
    }
}
