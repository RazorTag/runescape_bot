using RunescapeBot.BotPrograms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RunescapeBot.ImageTools
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
            int width = rgbImage.GetLength(0);
            int height = rgbImage.GetLength(1);
            bool[,] filterPixels = new bool[width, height];
            int numThreads = Math.Max(1, Environment.ProcessorCount - 1);
            int heavyThreads = width % numThreads;
            int lightWidth = (width - heavyThreads) / numThreads;
            Thread[] threadPool = new Thread[numThreads];
            int assignedColumns = 0;

            //Create the n+1 width threads
            for (int i = 0; i < heavyThreads; i++)
            {
                int start = assignedColumns;
                int end = assignedColumns + lightWidth + 1;
                threadPool[i] = new Thread(() => ColorFilterPiece(rgbImage, artifactColor, ref filterPixels, start, end));
                assignedColumns += lightWidth + 1;
            }
            //Create the n width threads
            for (int i = heavyThreads; i < numThreads; i++)
            {
                int start = assignedColumns;
                int end = assignedColumns + lightWidth;
                threadPool[i] = new Thread(() => ColorFilterPiece(rgbImage, artifactColor, ref filterPixels, start, end));
                assignedColumns += lightWidth;
            }

            //Sart the threads
            for(int i = 0; i < numThreads; i++)
            {
                threadPool[i].Start();
            }

            //Wait for all threads to finish
            for (int i = 0; i < numThreads; i++)
            {
                threadPool[i].Join();
            }

            return filterPixels;
        }

        /// <summary>
        /// Filters a portion of the rgbImage into a binary image
        /// </summary>
        /// <param name="rgbImage">unfiltered image</param>
        /// <param name="artifactColor">color range to use for filtering</param>
        /// <param name="filterPixels">filtered binary image</param>
        /// <param name="xMin">inclusive</param>
        /// <param name="xMax">exclusive</param>
        /// <param name="yMin">inclusive</param>
        /// <param name="yMax">exclusive</param>
        public static void ColorFilterPiece(Color[,] rgbImage, ColorRange artifactColor, ref bool[,] filterPixels, int xMin, int xMax)
        {
            Color pixelColor;
            int height = rgbImage.GetLength(1);

            for (int x = xMin; x < xMax; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    pixelColor = rgbImage[x, y];
                    filterPixels[x, y] = artifactColor.ColorInRange(pixelColor);
                }
            }
        }

        //public delegate void 

        /// <summary>
        /// Finds 
        /// </summary>
        /// <param name="artifactImage"></param>
        /// <returns></returns>
        public static List<Blob> FindBlobs(bool[,] artifactImage)
        {
            Blob blob;
            Point pixel;
            List<Blob> allBlobs = new List<Blob>();
            int width = artifactImage.GetLength(0);
            int height = artifactImage.GetLength(1);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (artifactImage[x, y])
                    {
                        blob = new Blob();
                        AddPixelToBlob(x, y, artifactImage, blob);

                        while (blob.HasPixelsToProcess())
                        {
                            pixel = blob.NextPixel();
                            CheckNeighbors(blob, artifactImage, pixel.X, pixel.Y, width, height);
                        }
                        
                        allBlobs.Add(blob);
                    }
                }
            }

            return allBlobs;
        }

        /// <summary>
        /// Adds any neighbors to the blob
        /// </summary>
        /// <param name="blob"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="artifactImage"></param>
        private static void CheckNeighbors(Blob blob, bool[,] artifactImage, int x, int y, int width, int height)
        {
            //top
            if (y > 0)
            {
                if (artifactImage[x, y - 1])
                {
                    AddPixelToBlob(x, y - 1, artifactImage, blob);
                }
            }

            //left
            if (x > 0)
            {
                if (artifactImage[x - 1, y])
                {
                    AddPixelToBlob(x - 1, y, artifactImage, blob);
                }
            }

            //right
            if (x < (width - 1))
            {
                if (artifactImage[x + 1, y])
                {
                    AddPixelToBlob(x + 1, y, artifactImage, blob);
                }
            }

            //bottom
            if (y < (height - 1))
            {
                if (artifactImage[x, y + 1])
                {
                    AddPixelToBlob(x, y + 1, artifactImage, blob);
                }
            }
        }

        /// <summary>
        /// Finds the biggest blob in the image
        /// </summary>
        /// <param name="artifactImage"></param>
        /// <returns></returns>
        public static Blob BiggestBlob(bool[,] artifactImage)
        {
            Blob biggestBlob;
            List<Blob> blobs = FindBlobs(artifactImage);

            if (blobs.Count > 0)
            {
                biggestBlob = blobs[0];
            }
            else
            {
                return null;
            }

            foreach (Blob blob in blobs)
            {
                biggestBlob = biggestBlob.MaxBlob(blob);
            }

            return biggestBlob;
        }

        /// <summary>
        /// Adds the pixel to the blob and removes the pixel from the image
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="artifactImage"></param>
        private static void AddPixelToBlob(int x, int y, bool[,] artifactImage, Blob blob)
        {
            blob.AddPixel(new Point(x, y));
            artifactImage[x, y] = false;
        }

        /// <summary>
        /// Determines if two colors have the same RGB values
        /// </summary>
        /// <param name="color1"></param>
        /// <param name="color2"></param>
        /// <returns>true if the colors match on RGB values</returns>
        public static bool ColorsAreEqual(Color color1, Color color2)
        {
            return (color1.R == color2.R) && (color1.B == color2.B) && (color1.G == color2.G);
        }
    }
}
