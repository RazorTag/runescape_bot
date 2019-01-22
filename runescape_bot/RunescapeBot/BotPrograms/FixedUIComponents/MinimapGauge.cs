using RunescapeBot.BotPrograms.FixedUIComponents;
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
        #region properties

        private RSClient RSClient;
        private Keyboard Keyboard;
        private GameScreen Screen;

        private int ScreenWidth
        {
            get
            {
                if (Screen != null)
                {
                    return Screen.Width;
                }
                else
                {
                    return 0;
                }
            }
        }

        private int ScreenHeight
        {
            get
            {
                if (Screen != null)
                {
                    return Screen.Height;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Determines the center of the minimap viewing area in game screen coordinates
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

        /// <summary>
        /// Determines the center of the minimap viewing area in minimap coordinates
        /// </summary>
        /// <returns></returns>
        public Point MinimapCenter
        {
            get
            {
                int x = WIDTH / 2;
                int y = HEIGHT / 2;
                return new Point(x, y);
            }
        }

        #endregion

        #region constructors

        public MinimapGauge(RSClient rsClient, Keyboard keyboard, GameScreen screen)
        {
            RSClient = rsClient;
            Keyboard = keyboard;
            Screen = screen;
        }

        #endregion

        #region visual

        /// <summary>
        /// Wrapper for ScreenScraper.CaptureWindow
        /// </summary>
        public bool ReadWindow(bool fastCapture = false)
        {
            Bitmap screenshot;
            try
            {
                screenshot = ScreenScraper.CaptureWindow(fastCapture);
                Screen.Value = ScreenScraper.GetRGB(screenshot);
            }
            catch
            {
                return false;
            }

            bool success = (screenshot != null) && (Screen.Width > 0) && (Screen.Height > 0);
            screenshot.Dispose();
            return success;
        }

        /// <summary>
        /// Looks for an object that matches a filter using the most recent screen read
        /// </summary>
        /// <param name="stationaryObject"></param>
        /// <param name="foundObject"></param>
        /// <param name="minimumSize"></param>
        /// <param name="shiftObject">set to true to shift the position of the object from minimap coordinates to game screen coordinates</param>
        /// <returns>the biggest object on the minimap that matches the search criteria</returns>
        public Blob LocateObject(IColorFilter stationaryObject, int minimumSize = 1, int maximumSize = int.MaxValue, bool shiftObject = false)
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
                        if (shiftObject) { MinimapToScreenBlob(blob); }
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
        public bool[,] MinimapFilter(IColorFilter filter, out Point offset)
        {
            if (Screen.Width < 1)    //the screen has not been read yet
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
        public bool[,] MinimapFilter(IColorFilter filter)
        {
            Point offset;
            return MinimapFilter(filter, out offset);
        }

        #endregion

        #region gauge reading

        /// <summary>
        /// Determines the player's hitpoints as a fraction of their maximum hitpoints (0-1)
        /// </summary>
        /// <returns>the player's remaining hitpoints as a fraction of total hitpoints (0-1)</returns>
        public double Hitpoints(bool readWindow = false)
        {
            if (readWindow) { ReadWindow(); }
            RectangleBounds hitpoints = HitpointsDigitsArea();
            Color[,] hitPoints = ImageProcessing.ScreenPiece(Screen, hitpoints.Left, hitpoints.Right, hitpoints.Top, hitpoints.Bottom);
            return GaugeFraction(hitPoints);
        }

        /// <summary>
        /// Determines the fraction remaining of a gauge using the readout digits to the left of the bubble
        /// </summary>
        /// <param name="gaugePercentage">array of Color pixels containing the gauge percentage number</param>
        /// <param name="threshold">minimum match needed to be considered high</param>
        /// <returns>true if the gauge is low, false otherwise</returns>
        public double GaugeFraction(Color[,] gaugePercentage)
        {
            Color gaugeSample = FirstGaugeNumberPixel(gaugePercentage);
            double hue = gaugeSample.GetHue();
            double gaugeFraction = hue / 120.0;
            return gaugeFraction;
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
        /// Sets the player to run (as opposed to walk) if run energy is fairly high (~50%)
        /// </summary>
        /// <param name="minRunEnergy">minimum fraction of run energy required to to turn on run</param>
        /// <returns>true if successful</returns>
        public bool RunCharacter(double minRunEnergy = 0.0, bool readWindow = false)
        {
            if (readWindow && !ReadWindow())
            {
                return false;
            }

            if (!CharacterIsRunning() && (RunEnergy() >= minRunEnergy))
            {
                ToggleRun();
            }
            return true;
        }

        /// <summary>
        /// Toggle the run/walk status using the run enery meter next to the minimap
        /// </summary>
        /// <returns>true if successful</returns>
        public bool ToggleRun(bool readWindow = false)
        {
            if (readWindow && !ReadWindow())
            {
                return false;
            }

            Point runOrb = RunOrbSamplePoint();
            Mouse.LeftClick(runOrb.X, runOrb.Y, 5);
            return true;
        }

        /// <summary>
        /// Determines if the character is currently running.
        /// Assumes the character is running if a stamina potion is active because we cannot differentiate.
        /// </summary>
        /// <returns>true for running, false for walking</returns>
        public bool CharacterIsRunning(bool readWindow = false)
        {
            if (readWindow && !ReadWindow())
            {
                return false;
            }

            Point runOrb = RunOrbSamplePoint();
            Color runColor = Screen[runOrb.X, runOrb.Y];
            RGBHSBRange normalRunEnergyFoot = RGBHSBRangeFactory.RunEnergyFoot();
            RGBHSBRange staminaRunEnergyFoot = RGBHSBRangeFactory.RunEnergyFootStamina();
            bool normalRun = normalRunEnergyFoot.ColorInRange(runColor);
            bool staminaRun = staminaRunEnergyFoot.ColorInRange(runColor);
            return normalRun || staminaRun;
        }

        /// <summary>
        /// Returns the point to look at or click on for the run energy orb next to the minimap
        /// </summary>
        /// <returns>the middle of the run orb</returns>
        public Point RunOrbSamplePoint()
        {
            Point runOrb;
            switch (ScreenScraper.ClientType)
            {
                case ScreenScraper.Client.Jagex:
                    runOrb = new Point(ScreenWidth - 160, 132);
                    break;
                case ScreenScraper.Client.OSBuddy:
                    runOrb = new Point(ScreenWidth - 156, 137);
                    break;
                default:
                    return new Point(0, 0);
            }
            return runOrb;
        }

        /// <summary>
        /// Determines fraction remaining of the character's run energy
        /// </summary>
        /// <returns>fraction of remaining run energy</returns>
        public double RunEnergy(bool readWindow = false)
        {
            if (readWindow && !ReadWindow())
            {
                return 0.0;
            }

            int left, right, top, bottom;
            switch (ScreenScraper.ClientType)
            {
                case ScreenScraper.Client.Jagex:
                    left = ScreenWidth - 196;
                    right = left + 20;
                    top = 128;
                    bottom = top + 12;
                    break;
                case ScreenScraper.Client.OSBuddy:
                    left = ScreenWidth - 193;
                    right = left + 20;
                    top = 133;
                    bottom = top + 12;
                    break;
                default:
                    return 0.0;
            }

            Color[,] runEnergyPercentage = ImageProcessing.ScreenPiece(Screen, left, right, top, bottom);
            return GaugeFraction(runEnergyPercentage);
        }

        #endregion

        #region movement

        /// <summary>
        /// Waits for the disappearance of the red flag on the minimap which indicates the target to move to
        /// </summary>
        /// <param name="timeout">maximum time to wait for the red flag to disappear</param>
        /// <returns>true if the red flag disappears</returns>
        public bool WaitDuringMovement(int timeout = 30000, int startupWait = 0)
        {
            const int redFlagMinSize = 20;  //ex 28
            const int redFlagMaxSize = 43;
            Stopwatch watch = new Stopwatch();
            watch.Start();
            Blob redFlag;

            BotProgram.SafeWait(startupWait);
            while (watch.ElapsedMilliseconds < timeout)
            {
                if (BotProgram.StopFlag) { return false; }
                ReadWindow();
                redFlag = LocateObject(RGBHSBRangeFactory.GenericColor(Color.Red), redFlagMinSize, redFlagMaxSize);
                if (redFlag == null)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Clicks a point on the minimap specified by radial coordinates
        /// </summary>
        /// <param name="angle">counterclockwise angle from the right direction in degrees</param>
        /// <param name="radius">fraction of the radius of the minimap to move (0-1)</param>
        /// <param name="waitToArrive">set to true to wait until the player almost reaches the destination before returning</param>
        /// <param name="randomization">maximum allowed randomized deviation from the exact click point</param>
        /// <returns>true if successful</returns>
        public bool MoveToPosition(double angle, double radius, bool waitToArrive, int randomization, int minimumWaitTime = 0, Point? restMouse = null, int timeout = 30000)
        {
            radius = Numerical.LimitToRange(radius, 0, 1);
            Point click = RadialToRectangular(angle, radius);
            Mouse.LeftClick(click.X, click.Y, randomization);
            if (waitToArrive)
            {
                //safe minimum time before we expect the red flag to disappear
                //This is also necessary to ensure that the red flag appears on the minimap before we start looking for it.
                Stopwatch watch = new Stopwatch();
                watch.Start();
                int waitTime = Math.Max(minimumWaitTime, (int) (500 + radius * 2500 - watch.ElapsedMilliseconds));
                if (BotProgram.SafeWait(waitTime)) { return false; }
                Mouse.Move(restMouse);
                return WaitDuringMovement(timeout);
            }
            Mouse.Move(restMouse);
            return true;
        }

        /// <summary>
        /// Moves along a path described by a series of movement vectors.
        /// Tries to avoid being stationary between movement vectors.
        /// </summary>
        /// <param name="waypoints">list of movement vectors</param>
        /// <param name="randomization">maximum allowed randomized deviation from the exact click point</param>
        /// <returns>true if successful</returns>
        public bool MoveAlongPath(List<MinimapWaypoint> waypoints, int randomization, Point? restMouse)
        {
            for (int i = 0; i < waypoints.Count - 1; i++)
            {
                Point nextWaypoint = RadialToRectangular(waypoints[i + 1].Angle, waypoints[i + 1].Radius);
                MoveToPosition(waypoints[i].Angle, waypoints[i].Radius, true, randomization, waypoints[i].MinWaitTime, nextWaypoint);
            }
            MoveToPosition(waypoints[waypoints.Count - 1].Angle, waypoints[waypoints.Count - 1].Radius, true, randomization, waypoints[waypoints.Count - 1].MinWaitTime);
            return true;
        }

        #endregion

        #region conversions

        /// <summary>
        /// Converts radial coordinates from the center of the minimap into rectangular game screen coordinates
        /// </summary>
        /// <param name="angle">counterclockwise angle from the right direction in degrees</param>
        /// <param name="radius">fraction of the radius of the minimap to move</param>
        /// <returns>a point on the minimap in terms of game screen coordinates</returns>
        public Point RadialToRectangular(double angle, double radius)
        {
            angle *= ((2 * Math.PI) / 360.0);    //convert to radians
            radius *= CLICK_RADIUS;
            Point center = Center;
            int x = center.X + ((int)Math.Round(Math.Cos(angle) * radius));
            int y = center.Y - ((int)Math.Round(Math.Sin(angle) * radius));
            return new Point(x, y);
        }

        /// <summary>
        /// Converts coordinates of a point on the minimap to the corresponding position in game.
        /// This should be treated as a rough estimate, not a precise conversion.
        /// </summary>
        /// <param name="minimapCoordinates">a point withing the minimap</param>
        /// <returns>a point on the game screen within the minimap</returns>
        public Point ExtrapolateMinimapToGame(Point minimapCoordinates)
        {
            int minimapX = minimapCoordinates.X - MinimapCenter.X;
            int minimapY = minimapCoordinates.Y - MinimapCenter.Y;
            int gameX = (int) (minimapX * ARTIFACT_LENGTH_TO_MINIMAP_RATIO * ScreenHeight);
            int gameY = (int) (minimapY * ARTIFACT_LENGTH_TO_MINIMAP_RATIO * ScreenHeight);
            return new Point((ScreenWidth / 2) + gameX, (ScreenHeight / 2) + gameY);
        }

        /// <summary>
        /// Converts coordinates with a screenshot of the minimap to coordinates for the game screen
        /// </summary>
        /// <param name="minimapCoordinates">a point withing the minimap</param>
        /// <returns>a point on the game screen within the minimap</returns>
        public Point MinimapToScreenCoordinates(Point minimapCoordinates)
        {
            int x = minimapCoordinates.X + ScreenWidth - OFFSET_LEFT;
            int y = minimapCoordinates.Y + OFFSET_TOP;
            return new Point(x, y);
        }

        /// <summary>
        /// Converts the position of a blob in the minimap to its position on the game screen
        /// </summary>
        /// <param name="minimapBlob">a blob within the minimap</param>
        public void MinimapToScreenBlob(Blob minimapBlob)
        {
            minimapBlob.ShiftPixels(ScreenWidth - OFFSET_LEFT, OFFSET_TOP);
        }

        #endregion

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
        public const double ARTIFACT_LENGTH_TO_MINIMAP_RATIO = 0.015;   //ratio of the percentage of game screen height to pixels on the minimap

        #endregion
    }
}
