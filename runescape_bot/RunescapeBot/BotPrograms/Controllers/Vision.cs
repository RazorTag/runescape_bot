using RunescapeBot.Common;
using RunescapeBot.ImageTools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms
{
    public class Vision
    {
        #region properties

        private GameScreen Screen;
        private RunParams RunParams;

        #endregion

        #region constructors

        public Vision(GameScreen screen, RunParams runParams)
        {
            Screen = screen;
            RunParams = runParams;
        }

        #endregion

        #region methods

        /// <summary>
        /// Creates a boolean array to represent a color filter match
        /// </summary>
        /// <param name="artifactColor"></param>
        /// <returns></returns>
        internal bool[,] ColorFilter(ColorFilter artifactColor)
        {
            return ColorFilter(Screen, artifactColor);
        }

        /// <summary>
        /// Creates a boolean array to represent a color filter match
        /// </summary>
        /// <param name="artifactColor"></param>
        /// <returns></returns>
        internal bool[,] ColorFilter(Color[,] image, ColorFilter artifactColor)
        {
            if (Screen == null)
            {
                return null;
            }
            return ImageProcessing.ColorFilter(image, artifactColor);
        }

        /// <summary>
        /// Creates a boolean array of a portion of the screen to represent a color filter match
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="top"></param>
        /// <param name="bottom"></param>
        /// <returns></returns>
        internal bool[,] ColorFilterPiece(ColorFilter filter, int left, int right, int top, int bottom, out Point trimOffset)
        {
            if (Screen == null)
            {
                trimOffset = new Point(0, 0);
                return null;
            }
            Color[,] colorArray = ScreenPiece(left, right, top, bottom, out trimOffset);
            return ImageProcessing.ColorFilter(colorArray, filter);
        }

        /// <summary>
        /// Creates a boolean array of a portion of the screen to represent a color filter match
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="top"></param>
        /// <param name="bottom"></param>
        /// <returns></returns>
        internal bool[,] ColorFilterPiece(ColorFilter filter, int left, int right, int top, int bottom)
        {
            Point empty;
            return ColorFilterPiece(filter, left, right, top, bottom, out empty);
        }

        /// <summary>
        /// Filters in a square within the screen shot
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <param name="offset"></param>
        /// <returns>The filtered screenshot cropped to the edges of the circle</returns>
        internal bool[,] ColorFilterPiece(ColorFilter filter, Point center, int radius, out Point offset)
        {
            int left = center.X - radius;
            int right = center.X + radius;
            int top = center.Y - radius;
            int bottom = center.Y + radius;
            return ColorFilterPiece(filter, left, right, top, bottom, out offset);
        }

        /// <summary>
        /// Filters in a square within the screen shot
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <returns>The filtered screenshot cropped to the edges of the circle</returns>
        internal bool[,] ColorFilterPiece(ColorFilter filter, Point center, int radius)
        {
            Point offset;
            return ColorFilterPiece(filter, center, radius, out offset);
        }

        /// <summary>
        /// Determines the fraction of piece of an RGB image that matches a color filter
        /// </summary>
        /// <param name="filter">filter to use for matching</param>
        /// <param name="left">left bound (inclusive)</param>
        /// <param name="right">right bound (inclusive)</param>
        /// <param name="top">top bound (inclusive)</param>
        /// <param name="bottom">bottom bound (inclusive)</param>
        /// <returns>The fraction (0-1) of the image that matches the filter</returns>
        internal double FractionalMatchPiece(ColorFilter filter, int left, int right, int top, int bottom)
        {
            bool[,] binaryImage = ColorFilterPiece(filter, left, right, top, bottom);
            return ImageProcessing.FractionalMatch(binaryImage);
        }

        /// <summary>
        /// Gets a rectangle from ColorArray
        /// </summary>
        /// <param name="topLeft"></param>
        /// <param name="bottomRight"></param>
        /// <returns></returns>
        internal Color[,] ScreenPiece(int left, int right, int top, int bottom, out Point trimOffset)
        {
            return ImageProcessing.ScreenPiece(Screen, left, right, top, bottom, out trimOffset);
        }

        /// <summary>
        /// Gets a rectangle from ColorArray
        /// </summary>
        /// <param name="topLeft"></param>
        /// <param name="bottomRight"></param>
        /// <returns></returns>
        internal Color[,] ScreenPiece(int left, int right, int top, int bottom)
        {
            Point empty;
            return ScreenPiece(left, right, top, bottom, out empty);
        }

        /// <summary>
        /// Sets the pixels in client UI areas to false.
        /// This should only be used with untrimmed images.
        /// </summary>
        /// <param name="mask"></param>
        internal void EraseClientUIFromMask(ref bool[,] mask)
        {
            if (mask == null) { return; }

            const int chatBoxWidth = 518;
            const int chatBoxHeight = 158;
            const int inventoryWidth = 240;
            const int inventoryHeight = 335;
            const int minimapWidth = 210;
            const int minimapHeight = 192;

            int width = mask.GetLength(0);
            int height = mask.GetLength(1);
            int requiredWidth = Math.Max(Math.Max(chatBoxWidth, inventoryWidth), minimapWidth);
            int requiredHeight = Math.Max(Math.Max(chatBoxHeight, inventoryHeight), minimapHeight);
            if ((width < requiredWidth) || (height < requiredHeight)) { return; }

            if (!RunParams.ClosedChatBox)   //do not erase chat box if the chat box is supposed to be closed
            {
                EraseFromMask(ref mask, 0, chatBoxWidth, height - chatBoxHeight, height);              //erase chat box
            }
            EraseFromMask(ref mask, width - inventoryWidth, width, height - inventoryHeight, height);  //erase inventory
            EraseFromMask(ref mask, width - minimapWidth, width, 0, minimapHeight);                //erase minimap
        }

        /// <summary>
        /// Clears a rectangle from a boolean mask
        /// </summary>
        /// <param name="mask"></param>
        /// <param name="xMin">Inclusive</param>
        /// <param name="xMax">Exclusive</param>
        /// <param name="yMin">Inclusive</param>
        /// <param name="yMax">Exclusive</param>
        internal void EraseFromMask(ref bool[,] mask, int xMin, int xMax, int yMin, int yMax)
        {
            for (int x = Math.Max(0, xMin); x < Math.Min(xMax, mask.GetLength(0) - 1); x++)
            {
                for (int y = Math.Max(0, yMin); y < Math.Min(yMax, mask.GetLength(1) - 1); y++)
                {
                    mask[x, y] = false;
                }
            }
        }

        /// <summary>
        /// Looks for an object that isn't moving (meaning the player isn't moving)
        /// </summary>
        /// <param name="stationaryObject">color filter used to locate the stationary object</param>
        /// <param name="foundObject">returns the Blob if it is found or null if not found</param>
        /// <param name="tolerance">maximum allowed distance in pixels between subsequent object locations</param>
        /// <param name="maxWaitTime">time to wait before gving up</param>
        /// <param name="minimumSize">minimum required size of the object in pixels</param>
        /// <param name="findObject">custom method to locate the object</param>
        /// <param name="verificationPasses">number of times to verify the position of the object after finding it</param>
        /// <returns>True if the object is found</returns>
        internal bool LocateStationaryObject(ColorFilter stationaryObject, out Blob foundObject, double tolerance, int maxWaitTime, int minimumSize = 1, int maximumSize = int.MaxValue, FindObject findObject = null, int verificationPasses = 1)
        {
            findObject = findObject ?? LocateObject;

            foundObject = null;
            Point? lastPosition = null;
            int effectivePasses = 0;
            int totalPasses = 0;
            Stopwatch giveUpWatch = new Stopwatch();
            giveUpWatch.Start();

            while (giveUpWatch.ElapsedMilliseconds < maxWaitTime || totalPasses <= verificationPasses)
            {
                if (BotProgram.StopFlag) { return false; }

                Blob objectBlob = null;
                findObject(stationaryObject, out objectBlob, minimumSize, maximumSize);

                if (objectBlob != null)
                {
                    if (Geometry.DistanceBetweenPoints(objectBlob.Center, lastPosition) <= tolerance)
                    {
                        effectivePasses++;
                    }
                    else
                    {
                        effectivePasses = 0;
                        lastPosition = objectBlob.Center;
                    }

                    if (effectivePasses >= verificationPasses)
                    {
                        foundObject = objectBlob;
                        return true;
                    }
                }
                else
                {
                    lastPosition = null;
                }
                totalPasses++;
            }

            return false;
        }
        internal delegate bool FindObject(ColorFilter stationaryObject, out Blob foundObject, int minimumSize = 1, int maximumSize = int.MaxValue);

        /// <summary>
        /// Locates all of the matching objects on the game screen (minus UI) that fit within the given size constraints
        /// </summary>
        /// <param name="objectFilter">color filter for the object type to search for</param>
        /// <param name="minSize">minimum required pixels</param>
        /// <param name="maxSize">maximum allowed pixels</param>
        /// <returns>List of blobs sorted from biggest to smallest</returns>
        internal List<Blob> LocateObjects(ColorFilter objectFilter, int minimumSize = 1, int maximumSize = int.MaxValue)
        {
            Screen.ReadWindow();
            bool[,] objectPixels = ColorFilter(objectFilter);
            EraseClientUIFromMask(ref objectPixels);
            List<Blob> objects = ImageProcessing.FindBlobs(objectPixels, true, minimumSize, maximumSize);
            return objects;
        }

        /// <summary>
        /// Locates all of the matching objects on the game screen (minus UI) that fit within the given size constraints
        /// </summary>
        /// <param name="objectFilter">color filter for the object type to search for</param>
        /// <param name="left">left bound of the search area</param>
        /// <param name="right">right bound of the search area</param>
        /// <param name="top">top bound of the search area</param>
        /// <param name="bottom">bottom bound of the search area</param>
        /// <param name="shiftBlobs">when true, moves all of the found blobs to their position on the original screen rather than the sub piece being scanned</param>
        /// <param name="minimumSize">minimum required pixels</param>
        /// <param name="maximumSize">maximum allowed pixels</param>
        /// <returns></returns>
        internal List<Blob> LocateObjects(ColorFilter objectFilter, int left, int right, int top, int bottom, bool shiftBlobs = true, int minimumSize = 1, int maximumSize = int.MaxValue)
        {
            Screen.ReadWindow();
            bool[,] objectPixels = ColorFilterPiece(objectFilter, left, right, top, bottom);
            EraseClientUIFromMask(ref objectPixels);
            List<Blob> foundObjects = ImageProcessing.FindBlobs(objectPixels, true, minimumSize, maximumSize);
            if (shiftBlobs)
            {
                foreach (Blob foundObject in foundObjects)
                {
                    foundObject.ShiftPixels(left, top);
                }
            }
            return foundObjects;
        }

        /// <summary>
        /// Finds the object closest to the center of the screen that matches the given criteria
        /// </summary>
        /// <param name="objectFilter">color filter for the object</param>
        /// <param name="minimumSize">minimum required size for the object in pixels</param>
        /// <param name="maximumSize">maximum allowed size for the object in pixels</param>
        /// <returns>the found object or null if none is found</returns>
        internal Blob LocateClosestObject(ColorFilter objectFilter, int minimumSize = 1, int maximumSize = int.MaxValue)
        {
            List<Blob> objects = LocateObjects(objectFilter, minimumSize, maximumSize);
            Blob closestObject = Geometry.ClosestBlobToPoint(objects, Screen.Center);
            return closestObject;
        }

        /// <summary>
        /// Finds the object closest to the center of the screen that matches the given criteria
        /// </summary>
        /// <param name="objectFilter">color filter for the object</param>
        /// <param name="minimumSize">minimum required size for the object in pixels</param>
        /// <param name="maximumSize">maximum allowed size for the object in pixels</param>
        /// <returns>the found object or null if none is found</returns>
        internal bool LocateClosestObject(ColorFilter objectFilter, out Blob closestObject, int minimumSize = 1, int maximumSize = int.MaxValue)
        {
            List<Blob> objects = LocateObjects(objectFilter, minimumSize, maximumSize);
            closestObject = Geometry.ClosestBlobToPoint(objects, Screen.Center);
            return closestObject != null;
        }

        /// <summary>
        /// Looks for an object that matches a filter
        /// </summary>
        /// <param name="stationaryObject"></param>
        /// <param name="foundObject"></param>
        /// <param name="minimumSize"></param>
        /// <returns></returns>
        internal bool LocateObject(ColorFilter stationaryObject, out Blob foundObject, int minimumSize = 1, int maximumSize = int.MaxValue)
        {
            Screen.ReadWindow();
            bool[,] objectPixels = ColorFilter(stationaryObject);
            EraseClientUIFromMask(ref objectPixels);
            return LocateObject(objectPixels, out foundObject, minimumSize, maximumSize);
        }

        /// <summary>
        /// Looks for an object that matches a filter in a particular region of the screen
        /// </summary>
        /// <param name="stationaryObject"></param>
        /// <param name="foundObject"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="top"></param>
        /// <param name="bottom"></param>
        /// <param name="minimumSize"></param>
        /// <returns></returns>
        internal bool LocateObject(ColorFilter stationaryObject, out Blob foundObject, int left, int right, int top, int bottom, int minimumSize = 1, int maximumSize = int.MaxValue)
        {
            Screen.ReadWindow();
            bool[,] objectPixels = ColorFilterPiece(stationaryObject, left, right, top, bottom);
            if (LocateObject(objectPixels, out foundObject, minimumSize, maximumSize))
            {
                foundObject.ShiftPixels(Math.Max(0, left), Math.Max(0, top));
                return true;
            }
            return false;
        }

        /// <summary>
        /// Looks for an object that matches a filter in a particular region of the screen
        /// </summary>
        /// <param name="stationaryObject"></param>
        /// <param name="foundObject"></param>
        /// <param name="center"></param>
        /// <param name="searchRadius"></param>
        /// <param name="minimumSize"></param>
        /// <param name="maximumSize"></param>
        /// <returns></returns>
        internal bool LocateObject(ColorFilter stationaryObject, out Blob foundObject, Point center, int searchRadius, int minimumSize = 1, int maximumSize = int.MaxValue)
        {
            int left = center.X - searchRadius;
            int right = center.X + searchRadius;
            int top = center.Y - searchRadius;
            int bottom = center.Y + searchRadius;
            return LocateObject(stationaryObject, out foundObject, left, right, top, bottom, minimumSize, maximumSize);
        }

        /// <summary>
        /// Finds the biggest blob in a binary image
        /// </summary>
        /// <param name="objectPixels"></param>
        /// <param name="foundObject"></param>
        /// <param name="minimumSize"></param>
        /// <returns></returns>
        internal bool LocateObject(bool[,] objectPixels, out Blob foundObject, int minimumSize = 1, int maximumSize = int.MaxValue)
        {
            foundObject = null;
            List<Blob> objectBlobs = ImageProcessing.FindBlobs(objectPixels, true, minimumSize, maximumSize);

            if (objectBlobs != null && objectBlobs.Count > 0)
            {
                foreach (Blob blob in objectBlobs)
                {
                    if (blob.Size < minimumSize)
                    {
                        return false;
                    }
                    if (blob.Size <= maximumSize)
                    {
                        foundObject = blob;
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Waits for a specified type of mouseover text to appear
        /// </summary>
        /// <param name="textColor">the color of the mousover text to wait for</param>
        /// <param name="timeout">time to wait before giving up</param>
        /// <returns>true if the specified type of mouseover text is found</returns>
        internal bool WaitForMouseOverText(ColorFilter textColor, int timeout = 5000)
        {
            const int left = 5;
            const int right = 500;
            const int top = 5;
            const int bottom = 18;

            Stopwatch watch = new Stopwatch();
            watch.Start();
            Blob mouseoverText = null;

            do
            {
                if (LocateObject(textColor, out mouseoverText, left, right, top, bottom, 10))
                {
                    return true;
                }
            }
            while ((watch.ElapsedMilliseconds < timeout) && !BotProgram.StopFlag);

            return false;
        }

        /// <summary>
        /// Waits until the player stops moving
        /// </summary>
        /// <param name="timeout">maximum time in milliseconds to wait</param>
        /// <returns>true if player stops moving, false if we give up</returns>
        internal bool WaitDuringPlayerAnimation(int timeout = 180000, double colorStrictness = 0.95, double locationStrictness = 0.99)
        {
            int xOffset = Screen.ArtifactLength(0.06);
            int yOffset = Screen.ArtifactLength(0.06);
            Color[,] pastImage = null;
            Color[,] presentImage = null;
            Color[,] futureImage = null;

            Stopwatch watch = new Stopwatch();
            watch.Start();

            while (!ImageProcessing.ImageMatch(pastImage, presentImage, colorStrictness, locationStrictness)
                || !ImageProcessing.ImageMatch(presentImage, futureImage, colorStrictness, locationStrictness))
            {
                if (BotProgram.StopFlag || watch.ElapsedMilliseconds >= timeout || BotProgram.SafeWait(100))
                {
                    return false;   //timeout
                }

                Screen.ReadWindow(true, true);
                pastImage = presentImage;
                presentImage = futureImage;
                futureImage = ScreenPiece(Screen.Center.X - xOffset, Screen.Center.X + xOffset, Screen.Center.Y - yOffset, Screen.Center.Y + yOffset);
            }

            return true;
        }

        #endregion
    }
}
