﻿using RunescapeBot.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;

namespace RunescapeBot.ImageTools
{
    public static class ImageProcessing
    {
        /// <summary>
        /// Creates a boolean array to represent that match 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static bool[,] ColorFilter(Color[,] rgbImage, ColorRange filter)
        {
            if (rgbImage == null || filter == null) { return new bool[0, 0]; }

            int width = rgbImage.GetLength(0);
            int height = rgbImage.GetLength(1);
            bool[,] filterPixels = new bool[width, height];
            int numThreads = Math.Max(1, Environment.ProcessorCount - 1);
            if (numThreads > rgbImage.GetLength(0))
            {
                numThreads = 1; //not enough stuff to do to justify multithreading
            }
            int heavyThreads = width % numThreads;
            int lightWidth = (width - heavyThreads) / numThreads;
            Thread[] threadPool = new Thread[numThreads];
            int assignedColumns = 0;

            //Create the n+1 width threads (heavy)
            for (int i = 0; i < heavyThreads; i++)
            {
                int start = assignedColumns;
                int end = assignedColumns + lightWidth + 1;
                threadPool[i] = new Thread(() => ColorFilterPiece(rgbImage, filter, ref filterPixels, start, end));
                assignedColumns += lightWidth + 1;
            }

            //Create the n width threads (light)
            for (int i = heavyThreads; i < numThreads; i++)
            {
                int start = assignedColumns;
                int end = assignedColumns + lightWidth;
                threadPool[i] = new Thread(() => ColorFilterPiece(rgbImage, filter, ref filterPixels, start, end));
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
        /// <param name="filter">color range to use for filtering</param>
        /// <param name="filterPixels">filtered binary image</param>
        /// <param name="xMin">inclusive</param>
        /// <param name="xMax">exclusive</param>
        /// <param name="yMin">inclusive</param>
        /// <param name="yMax">exclusive</param>
        private static void ColorFilterPiece(Color[,] rgbImage, ColorRange filter, ref bool[,] filterPixels, int xMin, int xMax)
        {
            Color pixelColor;
            int height = rgbImage.GetLength(1);

            for (int x = xMin; x < xMax; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    pixelColor = rgbImage[x, y];
                    filterPixels[x, y] = filter.ColorInRange(pixelColor);
                }
            }
        }

        /// <summary>
        /// Finds  all of the blobs in a binary image. Only considers right, left, up, down adjacency (not diagonal)
        /// </summary>
        /// <param name="image"></param>
        /// <returns>a list of blobs found from biggest to smallest</returns>
        public static List<Blob> FindBlobs(bool[,] image, bool sort = false, int minSize = 1, int maxSize = int.MaxValue)
        {
            if (image == null) { return new List<Blob>(); }

            Blob blob;
            Point pixel;
            List<Blob> allBlobs = new List<Blob>();
            int width = image.GetLength(0);
            int height = image.GetLength(1);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (image[x, y])
                    {
                        blob = new Blob();
                        AddPixelToBlob(x, y, image, blob);

                        while (blob.HasPixelsToProcess())
                        {
                            pixel = blob.NextPixel();
                            CheckNeighbors(blob, image, pixel.X, pixel.Y, width, height);
                        }

                        if (blob.Size >= minSize && blob.Size <= maxSize)
                        {
                            allBlobs.Add(blob);
                        }
                    }
                }
            }

            if (sort)
            {
                allBlobs.Sort(new BlobSizeComparer());
                allBlobs.Reverse();
            }

            return allBlobs;
        }

        /// <summary>
        /// Adds any neighbors to the blob
        /// </summary>
        /// <param name="blob"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="image"></param>
        private static void CheckNeighbors(Blob blob, bool[,] image, int x, int y, int width, int height)
        {
            //top
            if (y > 0)
            {
                if (image[x, y - 1])
                {
                    AddPixelToBlob(x, y - 1, image, blob);
                }
            }

            //left
            if (x > 0)
            {
                if (image[x - 1, y])
                {
                    AddPixelToBlob(x - 1, y, image, blob);
                }
            }

            //right
            if (x < (width - 1))
            {
                if (image[x + 1, y])
                {
                    AddPixelToBlob(x + 1, y, image, blob);
                }
            }

            //bottom
            if (y < (height - 1))
            {
                if (image[x, y + 1])
                {
                    AddPixelToBlob(x, y + 1, image, blob);
                }
            }
        }

        /// <summary>
        /// Finds the biggest blob in the image
        /// </summary>
        /// <param name="image"></param>
        /// <returns>the blob with the greatest number of pixels</returns>
        public static Blob BiggestBlob(bool[,] image)
        {
            if (image == null) { return null; }

            Blob biggestBlob;
            List<Blob> blobs = FindBlobs(image);

            if (blobs.Count > 0)
            {
                biggestBlob = blobs[0];
            }
            else
            {
                return new Blob();
            }

            foreach (Blob blob in blobs)
            {
                biggestBlob = biggestBlob.MaxBlob(blob);
            }

            if (biggestBlob.Size == 0)
            {
                biggestBlob = null;
            }
            return biggestBlob;
        }

        /// <summary>
        /// FInds blobs that have a center within range of a search point
        /// </summary>
        /// <param name="image">image to search in</param>
        /// <param name="searchPoint">point to search from</param>
        /// <param name="range">range from the search point to consider</param>
        /// <returns>list of blobs within the specified search area</returns>
        public static List<Blob> BlobsWithinRange(bool[,] image, Point searchPoint, int range, bool sort = false)
        {
            List<Blob> blobs = FindBlobs(image);
            List<Blob> blobsWithinRange = new List<Blob>();
            double distance;

            for (int i = 0; i < blobs.Count; i++)
            {
                distance = Geometry.DistanceBetweenPoints(blobs[i].Center, searchPoint);
                if (distance <= range)
                {
                    blobsWithinRange.Add(blobs[i]);
                }
            }

            if (sort)
            {
                blobsWithinRange.Sort(new BlobSizeComparer());
                blobsWithinRange.Reverse();
            }

            return blobsWithinRange;
        }

        /// <summary>
        /// Finds the blobs that are within the specified distance of the search point
        /// </summary>
        /// <param name="image">image to search in</param>
        /// <param name="searchPoint">point to search from</param>
        /// <param name="range">range from the search point to consider</param>
        /// <returns></returns>
        public static Blob BiggestBlobWithinRange(bool[,] image, Point searchPoint, int range)
        {
            List<Blob> blobs = FindBlobs(image);
            Blob biggestBlob = null;
            double distance;

            for (int i = 0; i < blobs.Count; i++)
            {
                distance = Geometry.DistanceBetweenPoints(blobs[i].Center, searchPoint);
                if ((blobs[i].Size > biggestBlob.Size) && (distance < range))
                {
                    biggestBlob = blobs[i];
                }
            }

            return biggestBlob;
        }

        /// <summary>
        /// Finds the blob that is closest to the search point
        /// </summary>
        /// <param name="image">binary image to search</param>
        /// <param name="searchPoint">point to search from</param>
        /// <param name="minSize">the minimum number of pixels that must be in a blob for it to be considered a match</param>
        /// <returns>The blob meeting the minimum size that is closest to the search point. Returns null if no blobs meet the minimum size.</returns>
        public static Blob ClosestBlob(bool[,] image, Point searchPoint, int minSize = 1)
        {
            List<Blob> blobs = FindBlobs(image);
            Blob closestBlob = null;
            double closestDistance = double.MaxValue;
            double distance;

            for (int i = 0; i < blobs.Count; i++)
            {
                distance = Geometry.DistanceBetweenPoints(blobs[i].Center, searchPoint);
                if ((blobs[i].Size >= minSize) && (distance < closestDistance))
                {
                    closestDistance = distance;
                    closestBlob = blobs[i];
                }
            }

            return closestBlob;
        }

        /// <summary>
        /// Adds the pixel to the blob and removes the pixel from the image
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="image"></param>
        private static void AddPixelToBlob(int x, int y, bool[,] image, Blob blob)
        {
            blob.AddPixel(new Point(x, y));
            image[x, y] = false;
        }

        /// <summary>
        /// Determines if two colors have the same RGB values
        /// </summary>
        /// <param name="color1"></param>
        /// <param name="color2"></param>
        /// <returns>true if the colors match on RGB values</returns>
        public static bool ColorsAreEqual(Color color1, Color color2)
        {
            if (color1 == null || color2 == null) { return false; }
            return (color1.R == color2.R) && (color1.B == color2.B) && (color1.G == color2.G);
        }

        /// <summary>
        /// Sums all of the R, G, and B values in the image
        /// </summary>
        /// <param name="rgbImage">color array to sum</param>
        /// <returns></returns>
        public static long ColorSum(Color[,] rgbImage)
        {
            if (rgbImage == null) { return 0; }

            Color pixel;
            long colorSum = 0;

            for (int x = 0; x < rgbImage.GetLength(0); x++)
            {
                for (int y = 0; y < rgbImage.GetLength(1); y++)
                {
                    pixel = rgbImage[x, y];
                    colorSum += pixel.R + pixel.G + pixel.B;
                }
            }
            return colorSum;
        }

        /// <summary>
        /// Checks a piece of the screen against an expected value for approximate equivalence
        /// </summary>
        /// <param name="image">full image of the game screen</param>
        /// <param name="left">left bound of the piece of the screen to check</param>
        /// <param name="right">right bound of the piece of the screen to check</param>
        /// <param name="top">top bound of the piece of the screen to check</param>
        /// <param name="bottom">bottom bound of the piece of the screen to check</param>
        /// <param name="expectedColorSum">the expected color sum of the piece of the screen</param>
        /// <param name="tolerance">allowed deviation of the actual value from the expected value</param>
        /// <returns></returns>
        public static bool ScreenPieceCheck(Color[,] image, int left, int right, int top, int bottom, long expectedColorSum, double tolerance)
        {
            Color[,] screenPiece = ScreenPiece(image, left, right, top, bottom);
            long colorSum = ColorSum(screenPiece);
            return Numerical.CloseEnough(expectedColorSum, colorSum, tolerance);
        }

        /// <summary>
        /// Determines the fraction of an RGB image that matches a color filter
        /// </summary>
        /// <param name="rgbImage">image to match on</param>
        /// <param name="filter">filter to use for matching</param>
        /// <returns>The fraction (0-1) of the image that matches the filter</returns>
        public static double FractionalMatch(Color[,] rgbImage, ColorRange filter)
        {
            bool[,] binaryImage = ColorFilter(rgbImage, filter);
            return FractionalMatch(binaryImage);
        }

        /// <summary>
        /// Determines the fraction of a binary image that is true
        /// </summary>
        /// <param name="image">binary image to check</param>
        /// <returns>The fraction (0-1) of the image that is true</returns>
        public static double FractionalMatch(bool[,] image)
        {
            if (image == null) { return 0.0; }

            int matches = 0;
            int width = image.GetLength(0);
            int height = image.GetLength(1);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (image[x, y])
                    {
                        matches++;
                    }
                }
            }

            return matches / ((double) (width * height));
        }

        /// <summary>
        /// Gets a rectangle from ColorArray
        /// </summary>
        /// <param name="image"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="top"></param>
        /// <param name="bottom"></param>
        /// <returns></returns>
        public static Color[,] ScreenPiece(Color[,] image, int left, int right, int top, int bottom, out Point trimOffset)
        {
            if (image == null)
            {
                trimOffset = new Point();
                return new Color[0, 0];
            }

            left = Math.Max(left, 0);
            right = Math.Min(right, image.GetLength(0) - 1);
            top = Math.Max(top, 0);
            bottom = Math.Min(bottom, image.GetLength(1) - 1);
            if ((left > right) || (top > bottom))
            {
                trimOffset = Point.Empty;
                return null;
            }
            Color[,] screenPiece = new Color[right - left + 1, bottom - top + 1];
            trimOffset = new Point(left, top);

            for (int x = left; x <= right; x++)
            {
                for (int y = top; y <= bottom; y++)
                {
                    screenPiece[x - left, y - top] = image[x, y];
                }
            }
            return screenPiece;
        }

        /// <summary>
        /// Gets a rectangle from ColorArray
        /// </summary>
        /// <param name="image"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="top"></param>
        /// <param name="bottom"></param>
        /// <returns></returns>
        public static Color[,] ScreenPiece(Color[,] image, int left, int right, int top, int bottom)
        {
            Point empty;
            return ScreenPiece(image, left, right, top, bottom, out empty);
        }
    }
}
