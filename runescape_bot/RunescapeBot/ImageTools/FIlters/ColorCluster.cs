using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.ImageTools.FIlters
{
    public class ColorCluster
    {
        private List<RGBHSBRange> cluster;

        public ColorCluster()
        {
            cluster = new List<RGBHSBRange>();
        }


        /// <summary>
        /// Adds a color range to the cluster
        /// </summary>
        /// <param name="color"></param>
        public void Add(RGBHSBRange color)
        {
            cluster.Add(color);
        }

        /// <summary>
        /// Determines if a pixel
        /// </summary>
        /// <param name="x">x-coordinate of the pixel to check</param>
        /// <param name="y">y-coordinate of the pixel to check</param>
        /// <param name="image"></param>
        /// <returns></returns>
        public bool PixelInCluster(int x, int y, Color[,] image)
        {
            return false;
        }
    }
}
