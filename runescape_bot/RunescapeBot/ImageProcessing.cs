using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public static class ImageProcessing
    {
        /// <summary>
        /// Creates a boolean array to represent that match 
        /// </summary>
        /// <param name="artifactColor"></param>
        /// <returns></returns>
        public static bool[,] ColorFilter(Color[,] rgbImage, ColorRange artifactColor)
        {
            Color pixelColor;
            int width = rgbImage.GetLength(0);
            int height = rgbImage.GetLength(1);
            bool[,] filterPixels = new bool[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    pixelColor = rgbImage[x, y];
                    if (artifactColor.ColorInRange(pixelColor))
                    {
                        filterPixels[x, y] = true;
                    }
                }
            }
            return filterPixels;
        }

        public static List<Blob> FindBlobs(bool[,] artifactImage)
        {
            return null;
        }
    }
}
