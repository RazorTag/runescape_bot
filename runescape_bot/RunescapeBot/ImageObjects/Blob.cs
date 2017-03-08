using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public class Blob
    {
        public Blob()
        {

        }

        /// <summary>
        /// List of pixels in the blob.
        /// </summary>
        public Dictionary<int, Point> Pixels { get; set; }

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
        /// Determines the center of area of the blob
        /// </summary>
        public Point Center()
        {
            int totalX = 0;
            int totalY = 0;

            foreach (KeyValuePair<int, Point> pixel in Pixels)
            {
                totalX += pixel.Value.X;
                totalY += pixel.Value.Y;
            }
            return new Point(totalX / Size, totalY / Size);
        }

        /// <summary>
        /// Add a new pixel to the blob
        /// </summary>
        /// <param name="newPixel"></param>
        public bool AddPixel(Point newPixel)
        {
            if (!ContainsPixel(newPixel))
            {
                Pixels.Add(newPixel.GetHashCode(), newPixel);
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
        /// Determines if a point exists in this blob
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool ContainsPixel(Point pixel)
        {
            return Pixels.ContainsKey(pixel.GetHashCode());
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
    }
}
