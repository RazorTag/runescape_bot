using RunescapeBot.Common;
using RunescapeBot.ImageTools;
using RunescapeBot.UITools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms
{
    public class MinimapGauge
    {
        Color[,] Screen;
        private Process RSClient;
        private Keyboard Keyboard;

        private int ScreenWidth
        {
            get
            {
                if (Screen != null)
                {
                    return Screen.GetLength(0);
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Determines the center of the minimap viewing area
        /// </summary>
        /// <returns></returns>
        public Point Center
        {
            get
            {
                int x = ScreenWidth - OFFSET_LEFT + (WIDTH / 2);
                int y = OFFSET_TOP + (HEIGHT / 2);
                return new Point(x, y);
            }
        }

        public MinimapGauge(Process rsClient, Keyboard keyboard)
        {
            RSClient = rsClient;
            Keyboard = keyboard;
        }

        /// <summary>
        /// Called by BotProgram to update with the most current screenshot
        /// </summary>
        /// <param name="colorArray">the newest screenshot</param>
        public void SetScreen(Color[,] colorArray)
        {
            Screen = colorArray;
        }

        /// <summary>
        /// Converts radial coordinates from the center of the minimap into rectangular game screen coordinates
        /// </summary>
        /// <param name="angle">counterclockwise angle from the right direction in degrees</param>
        /// <param name="radius">fraction of the radius of the minimap to move</param>
        /// <returns>a point on the minimap in terms of game screen coordinates</returns>
        public Point RadialClickLocation(double angle, double radius)
        {
            angle *= ((2 * Math.PI) / 360.0);    //convert to radians
            radius *= CLICK_RADIUS;
            Point center = Center;
            int x = center.X + ((int)Math.Round(Math.Cos(angle) * radius));
            int y = center.Y - ((int)Math.Round(Math.Sin(angle) * radius));
            return new Point(x, y);
        }

        /// <summary>
        /// Looks for an object that matches a filter using the most recent screen read
        /// </summary>
        /// <param name="stationaryObject"></param>
        /// <param name="foundObject"></param>
        /// <param name="minimumSize"></param>
        /// <returns>the biggest object on the minimap that matches the search criteria</returns>
        public Blob LocateObject(RGBHSBRange stationaryObject, int minimumSize = 1, int maximumSize = int.MaxValue)
        {
            bool[,] objectPixels = MinimapFilter(stationaryObject);
            List<Blob> objectBlobs = ImageProcessing.FindBlobs(objectPixels, true, minimumSize, maximumSize);

            if (objectBlobs != null && objectBlobs.Count > 0)
            {
                foreach (Blob blob in objectBlobs)
                {
                    if (blob.Size < minimumSize)
                    {
                        return null;
                    }
                    if (blob.Size <= maximumSize)
                    {
                        return blob;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Gets a rectangle containing the minimap with the non-minimap corners set to false
        /// </summary>
        /// <param name="filter">the filter to use on the minimap</param>
        /// <param name="offset">gets set to the offset from the game screen to the minimap piece</param>
        /// <returns>true for the pixels on the minimap that match the filter</returns>
        public bool[,] MinimapFilter(RGBHSBRange filter, out Point offset)
        {
            if (Screen.GetLength(0) < 1)    //the screen has not been read yet
            {
                offset = new Point(0, 0);
                return null;
            }

            int left = ScreenWidth - OFFSET_LEFT;
            int right = left + WIDTH - 1;
            int top = OFFSET_TOP;
            int bottom = top + HEIGHT - 1;
            Point center = new Point((left + right) / 2, (top + bottom) / 2);
            double radius = CLICK_RADIUS;
            double distance;
            offset = new Point(left, top);
            bool[,] minimapFilter = new bool[right - left + 1, bottom - top + 1];

            for (int x = left; x <= right; x++)
            {
                for (int y = top; y <= bottom; y++)
                {
                    distance = Math.Sqrt(Math.Pow(x - center.X, 2) + Math.Pow(y - center.Y, 2));
                    if (distance <= radius)
                    {
                        minimapFilter[x - left, y - top] = filter.ColorInRange(Screen[x, y]);
                    }
                }
            }

            return minimapFilter;
        }

        /// <summary>
        /// Gets a rectangle containing the minimap with the non-minimap corners set to false
        /// </summary>
        /// <param name="filter">the filter to use on the minimap</param>
        /// <returns>true for the pixels on the minimap that match the filter</returns>
        public bool[,] MinimapFilter(RGBHSBRange filter)
        {
            Point offset;
            return MinimapFilter(filter, out offset);
        }

        /// <summary>
        /// Determines if the player has very green hitpoints using the most recent screen read
        /// </summary>
        /// <returns></returns>
        public bool HighHitpoints()
        {
            RectangleBounds hitpoints = HitpointsDigitsArea();
            double greenHitpointMatch = ImageProcessing.FractionalMatchPiece(Screen, RGBHSBRangeFactory.HitpointsGreen(), hitpoints.Left, hitpoints.Right, hitpoints.Top, hitpoints.Bottom);
            return greenHitpointMatch > 0.05;
        }

        /// <summary>
        /// Determines the player's hitpoints as a fraction of their maximum hitpoints (0-1)
        /// </summary>
        /// <returns>the player's remaining hitpoints as a fraction of total hitpoints (0-1)</returns>
        public double Hitpoints()
        {
            RectangleBounds hitpoints = HitpointsDigitsArea();
            Color[,] hitPoints = ImageProcessing.ScreenPiece(Screen, hitpoints.Left, hitpoints.Right, hitpoints.Top, hitpoints.Bottom);
            Color hitpointSample = FirstGaugeNumberPixel(hitPoints);
            double hue = hitpointSample.GetHue();
            double hitpointFraction = hue / 120.0;
            return hitpointFraction;
        }

        /// <summary>
        /// Finds a pixel in an image that matches the hitpoint number color spectrum (zero blue)
        /// </summary>
        /// <param name="gaugeNumber">image of the digits for a minimap gauge</param>
        /// <returns>the first pixel match for the digits in a gauge</returns>
        public Color FirstGaugeNumberPixel(Color[,] gaugeNumber)
        {
            for (int x = 0; x < gaugeNumber.GetLength(0); x++)
            {
                for (int y = 0; y < gaugeNumber.GetLength(1); y++)
                {
                    if (gaugeNumber[x, y].B == 0)
                    {
                        return gaugeNumber[x, y];
                    }
                }
            }
            return Color.Red;
        }

        /// <summary>
        /// Gets the bounds of the hitpoints digits area
        /// </summary>
        /// <param name="screenSize">dimensions of the game screen</param>
        /// <returns>left, right, top, and bottom bounds</returns>
        public RectangleBounds HitpointsDigitsArea()
        {
            int left = ScreenWidth - HITPOINTS_DIGITS_OFFSET_LEFT;
            int right = left + HITPOINTS_DIGITS_WIDTH;
            int top = HITPOINTS_DIGITS_OFFSET_TOP;
            int bottom = top + HITPOINTS_DIGITS_HEIGHT;
            return new RectangleBounds(left, right, top, bottom);
        }

        /// <summary>
        /// Converts coordinates with a screenshot of the minimap to coordinates for the entire game screen
        /// </summary>
        /// <param name="minimapCoordinates">a point withing the minimap</param>
        /// <returns>a point on the game screen within the minimap</returns>
        public Point MinimapToScreenCoordinates(Point minimapCoordinates)
        {
            int x = minimapCoordinates.X + ScreenWidth - OFFSET_LEFT;
            int y = minimapCoordinates.Y + OFFSET_TOP;
            return new Point(x, y);
        }

        #region constants

        public const int HITPOINTS_DIGITS_OFFSET_LEFT = 207; //pixels from left of hitpoints digits area to right of screen (using screen width as x index for right of screen)
        public const int HITPOINTS_DIGITS_OFFSET_TOP = 61; //pixels from top of screen to top of hitpoints digits area (using 0 as y index for top of screen)
        public const int HITPOINTS_DIGITS_WIDTH = 21;  //pixels from left to right of hitpoints digits area (1 less than horizontal number of pixels)
        public const int HITPOINTS_DIGITS_HEIGHT = 13;  //pixels from top to bottom of hitpoints digits area (1 less than vertical number of pixels)

        public const int CLICK_RADIUS = 72;
        public const int WIDTH = 150;
        public const int HEIGHT = 152;
        public const int OFFSET_LEFT = 156; //pixels from the left side of the minimap to the right edge of the game screen
        public const int OFFSET_TOP = 8;    //pixels from the top of the game screen to the top of the minimap

        public const int GRID_SQUARE_SIZE = 4;  //width and height of a grid square in pixels on the minimap

        #endregion
    }
}
