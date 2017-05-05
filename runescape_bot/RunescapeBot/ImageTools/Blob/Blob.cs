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
                return RightBound - LeftBound;
            }
        }

        /// <summary>
        /// Returns the vertical distance between top and bottom bounds
        /// </summary>
        public int Height
        {
            get
            {
                return BottomBound - TopBound;
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
        /// The left most x-value of any pixel in the blob
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
        /// The left most x-value of any pixel in the blob
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
        /// The left most x-value of any pixel in the blob
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
        /// Calculates the distance to another point
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public double DistanceTo(Point point)
        {
            int rise = Center.Y - point.Y;
            int run = Center.X - point.X;
            return Math.Sqrt(rise * rise + run * run);
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
    }
}
