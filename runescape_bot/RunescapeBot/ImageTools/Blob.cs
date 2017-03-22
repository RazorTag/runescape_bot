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
        private bool _centerSet;
        private Point _center;
        public Point Center
        {
            get
            {
                if (!_centerSet)
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
                        _center = new Point(0, 0);
                    }
                    else
                    {
                        _center = new Point(totalX / Size, totalY / Size);
                    }
                    _centerSet = true;
                }
                
                return _center;
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
                _centerSet = false;
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
