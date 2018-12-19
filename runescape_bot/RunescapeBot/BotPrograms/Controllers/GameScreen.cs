using RunescapeBot.Common;
using RunescapeBot.ImageTools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms
{
    public class GameScreen : IDisposable
    {
        #region properties

        /// <summary>
        /// Stores a Color array of the client window.
        /// </summary>
        public Color[,] Value { get; set; }

        /// <summary>
        /// Stores a bitmap of the client window
        /// </summary>
        public Bitmap Bitmap { get; set; }

        /// <summary>
        /// The time that the last screenshot was taken
        /// </summary>
        private DateTime LastScreenShot { get; set; }

        /// <summary>
        /// The game client.
        /// </summary>
        private RSClient RSClient;

        /// <summary>
        /// Defines how the bot should be run.
        /// </summary>
        private RunParams RunParams;

        /// <summary>
        /// Horizontal length of the game screen.
        /// </summary>
        public int Width
        {
            get {
                return Value != null ? Value.GetLength(0) : 0;
            }
        }

        /// <summary>
        /// Vertical length of the game screen.
        /// </summary>
        public int Height
        {
            get {
                return Value != null ? Value.GetLength(1) : 0;
            }
        }

        /// <summary>
        /// The center point of the screen.
        /// </summary>
        public Point Center
        {
            get
            {
                if (!LooksValid())
                {
                    return new Point(0, 0);
                }
                else
                {
                    return new Point(Width / 2, Height / 2);
                }
            }
        }

        #endregion

        #region constructors

        public GameScreen(RSClient client, RunParams runParams)
        {
            RSClient = client;
            RunParams = runParams;
        }

        #endregion

        #region vision

        /// <summary>
        /// Determines if the screen looks like a valid capture.
        /// </summary>
        /// <returns></returns>
        public bool LooksValid()
        {
            return Value != null && Width > 100 && Height > 100;
        }

        /// <summary>
        /// Calls ReadWindow if the current screen image is unsatisfactory
        /// </summary>
        /// <param name="readWindow">Set to true to always read the window</param>
        /// <returns>true unless the window needs to be read but can't</returns>
        public bool MakeSureWindowHasBeenRead(bool readWindow = false)
        {
            if (readWindow || !LooksValid())
            {
                return ReadWindow();
            }
            return true;
        }

        /// <summary>
        /// Determines the pixels on the screen taken up by an artifact of a known fraction of the screen
        /// </summary>
        /// <param name="artifactSize">the size of an artifact in terms of fraction of the square of the screen height</param>
        /// <returns>the number of pixels taken up by an artifact</returns>
        public int ArtifactArea(double artifactSize)
        {
            if (!MakeSureWindowHasBeenRead()) { return 0; }
            double pixels = artifactSize * Height * Height;
            return (int)Math.Round(pixels);
        }

        /// <summary>
        /// Determines the pixel length of an artifact of a known fraction of the screen's height
        /// </summary>
        /// <param name="artifactLength">the fraction of the screen height taken up by the artifact</param>
        /// <returns>the pixel length of the artifact</returns>
        public int ArtifactLength(double artifactLength)
        {
            if (!MakeSureWindowHasBeenRead()) { return 0; }
            double length = artifactLength * Height;
            return (int)Math.Round(length);
        }

        /// <summary>
        /// Wrapper for ScreenScraper.CaptureWindow
        /// </summary>
        internal bool ReadWindow(bool checkClient = true, bool fastCapture = false)
        {
            if (BotProgram.StopFlag || checkClient && !RSClient.PrepareClient()) { return false; }

            Value = null;

            try
            {
                LastScreenShot = DateTime.Now;
                Bitmap = ScreenScraper.CaptureWindow(fastCapture);
                Value = ScreenScraper.GetRGB(Bitmap);
            }
            catch
            {
                return false;
            }
            finally
            {
                if (Bitmap != null)
                {
                    Bitmap.Dispose();
                }
            }

            return (Value != null) && (Height > 0) && (Width > 0);
        }

        /// <summary>
        /// Gets the time since the last screenshot
        /// </summary>
        /// <returns>time elapsed since the most recent screenshot</returns>
        public int TimeSinceLastScreenShot()
        {
            TimeSpan elapsedTime = DateTime.Now - LastScreenShot;
            return (int)elapsedTime.TotalMilliseconds;
        }

        /// <summary>
        /// Takes a new screenshot if the current one is too old
        /// </summary>
        /// <param name="maxScreenShotAge">the maximum usable age of an old screenshot in milliseconds</param>
        public void UpdateScreenshot(int maxScreenshotAge = 500)
        {
            if (TimeSinceLastScreenShot() <= maxScreenshotAge)
            {
                ReadWindow();
            }
        }

        #endregion

        #region logged in/logged out checks

        /// <summary>
        /// Finds the offset for the login screen
        /// </summary>
        /// <returns></returns>
        public Point LoginScreenOffset()
        {
            int yOffset = 0;
            while (ImageProcessing.ColorsAreEqual(Value[Center.X, yOffset], Color.Black) && (yOffset < Height))
            {
                yOffset++;
            }
            return new Point(0, yOffset);
        }

        /// <summary>
        /// Determines if the last screenshot was of the welcome screen
        /// </summary>
        /// <returns>true if we are on the welcome screen, false otherwise</returns>
        public bool IsWelcomeScreen(out Point? clickLocation)
        {
            int centerX = Center.X;
            const int centerY = 337;
            const int width = 220;
            const int height = 80;
            int left = centerX - (width / 2);
            int right = centerX + (width / 2);
            int top = centerY - (height / 2);
            int bottom = centerY + (height / 2);
            int totalSize = width * height;
            int redBlobSize;

            RGBHSBRange red = RGBHSBRangeFactory.WelcomeScreenClickHere();
            bool[,] clickHere = ImageProcessing.ColorFilterPiece(Value, red, left, right, top, bottom);
            Blob enterGame = ImageProcessing.BiggestBlob(clickHere);
            redBlobSize = enterGame.Size;

            if (redBlobSize > (totalSize / 2))
            {
                clickLocation = enterGame.RandomBlobPixel();
                clickLocation = new Point(clickLocation.Value.X + left, clickLocation.Value.Y + top);
                return true;
            }
            else
            {
                clickLocation = null;
                return false;
            }
        }

        /// <summary>
        /// Determines if the welcome screen has been reached
        /// </summary>
        /// <returns>true if the welcome screen has been reached, false if not or if the StopFlag is raised</returns>
        public bool ConfirmWelcomeScreen(out Point? clickLocation)
        {
            clickLocation = null;

            //Wait up to 60 seconds
            for (int i = 0; i < 60; i++)
            {
                if (BotProgram.StopFlag) { return false; }
                ReadWindow();

                if (IsWelcomeScreen(out clickLocation))
                {
                    return true;
                }
                else
                {
                    if (BotProgram.StopFlag) { return false; }
                    BotProgram.SafeWait(1000);
                }
            }
            return false;   //We timed out waiting.
        }

        /// <summary>
        /// Determines if the client is logged in
        /// </summary>
        /// <returns>true if we are verifiably logged in</returns>
        public bool IsLoggedIn(bool readWindow = true)
        {
            if (readWindow) { ReadWindow(); }

            //Get a piece of the column from the right of the inventory
            int right = Width - 10;
            int left = right - 12;
            int bottom = Height - 105;
            int top = bottom - 45;
            Color[,] inventoryColumn = ImageProcessing.ScreenPiece(Value, left, right, top, bottom);

            //Compare the column against the expected value
            long columnSum = ImageProcessing.ColorSum(inventoryColumn);
            long expectedColumnSum = 133405;
            if (columnSum > (1.01 * expectedColumnSum) || columnSum < (0.99 * expectedColumnSum))
            {
                return RunParams.LoggedIn = false;
            }

            return RunParams.LoggedIn = true;
        }

        /// <summary>
        /// Waits on the client to log in after clicking through the welcome screen
        /// </summary>
        /// <returns>true if the log in is verified, false if we time out waiting</returns>
        public bool ConfirmLogin()
        {
            //Wait for up to 60 seconds
            for (int i = 0; i < 60; i++)
            {
                if (BotProgram.StopFlag) { return false; }
                ReadWindow();
                if (IsLoggedIn())
                {
                    return true;
                }
                else
                {
                    if (BotProgram.StopFlag) { return false; }
                    BotProgram.SafeWait(1000);
                }
            }

            return false;   //We timed out waiting.
        }

        /// <summary>
        /// Determines if the client is logged out
        /// </summary>
        /// <returns>true if we are verifiably logged out</returns>
        public bool IsLoggedOut(bool readWindow = false)
        {
            if (readWindow && !ReadWindow())
            {
                RunParams.LoggedIn = false;
                return false;
            }

            Color color;
            Point loginOffset = LoginScreenOffset();
            int height = Height;
            int top = loginOffset.Y;
            int centerX = Center.X + loginOffset.X;
            int checkRow = Math.Min(Math.Max(0, height - 1), loginOffset.Y + ScreenScraper.LOGIN_WINDOW_HEIGHT + 1);    //1 pixel below where the bottom of the login window should be
            int xOffset = (ScreenScraper.LOGIN_WINDOW_WIDTH / 2) + 2;
            int blackPixels = 0;
            int totalPixels = 0;

            for (int x = centerX - xOffset; x < centerX + xOffset; x++)
            {
                //check bottom of login box
                color = Value[x, checkRow];
                blackPixels += ImageProcessing.ColorsAreEqual(color, Color.Black) ? 1 : 0;
                totalPixels++;
            }
            for (int y = top; y < checkRow; y++)  //check sides
            {
                //check left of login box
                color = Value[centerX - xOffset, y];
                blackPixels += ImageProcessing.ColorsAreEqual(color, Color.Black) ? 1 : 0;
                totalPixels++;

                //check right of login box
                color = Value[centerX + xOffset, y];
                blackPixels += ImageProcessing.ColorsAreEqual(color, Color.Black) ? 1 : 0;
                totalPixels++;
            }
            //assume we are logged out if a majority off the border pixels are perfectly black
            if ((blackPixels / ((double)totalPixels)) < 0.25)
            {
                return false;
            }

            //Check for "Welcome to RuneScape" yellow text. We are probably logged out at this point.
            int topWelcome = top + 241;
            int bottomWelcome = topWelcome + 13;
            int leftWelcome = Center.X - 75;
            int rightWelcome = leftWelcome + 146;
            bool[,] welcomeText = ImageProcessing.ColorFilterPiece(Value, RGBHSBRangeFactory.Yellow(), leftWelcome, rightWelcome, topWelcome, bottomWelcome);
            double welcomeMatch = ImageProcessing.FractionalMatch(welcomeText);

            if (!Numerical.WithinRange(welcomeMatch, 0.23275, 0.01)) //ex 0.23275
            {
                //Check for the "Enter your username/email & password." text.
                leftWelcome = Center.X - 140;
                rightWelcome = leftWelcome + 280;
                topWelcome = top + 206;
                bottomWelcome = topWelcome + 10;
                welcomeText = ImageProcessing.ColorFilterPiece(Value, RGBHSBRangeFactory.Yellow(), leftWelcome, rightWelcome, topWelcome, bottomWelcome);
                welcomeMatch = ImageProcessing.FractionalMatch(welcomeText);

                if (!Numerical.WithinRange(welcomeMatch, 0.25234, 0.01)) //ex. 0.2523
                {
                    return false;   //Could not find the welcome text or the enter text.
                }
            }

            RunParams.LoggedIn = false;
            return true;
        }

        /// <summary>
        /// Determines if the world switcher is open
        /// </summary>
        /// <returns>true if the world switcher is open</returns>
        public bool WorldSwitcherIsOpen()
        {
            int left = Width - 200;
            int right = left + 150;
            int top = Height - 297;
            int bottom = top + 20;
            Color[,] currentWorldTitle = ImageProcessing.ScreenPiece(Value, left, right, top, bottom);
            double worldTextMatch = ImageProcessing.FractionalMatch(currentWorldTitle, RGBHSBRangeFactory.BankTitle());
            const double worldTextMinimumMatch = 0.05;
            return worldTextMatch > worldTextMinimumMatch;
        }

        /// <summary>
        /// Determines if one of the two bot worlds is selected on the login screen.
        /// Assumes that the client is on the login screen.
        /// </summary>
        /// <param name="readWindow">Set to true to force a new screen read</param>
        /// <returns>true if the world is set to 385 or 386. May also hit on 358 and 368</returns>
        public bool LoginSetForBotWorld(bool readWindow = false)
        {
            MakeSureWindowHasBeenRead(readWindow);
            Point loginOffset = LoginScreenOffset();
            int left = Center.X - 323 + loginOffset.X;
            int right = left + 30;
            int top = 466 + loginOffset.Y;
            int bottom = top + 14;
            long colorSum = ImageProcessing.ColorSum(ImageProcessing.ScreenPiece(Value, left, right, top, bottom));
            bool freeBotWorld = Numerical.CloseEnough(LOGIN_BOT_WORLD_385, colorSum, 0.0005);
            bool memberBotWorld = Numerical.CloseEnough(LOGIN_BOT_WORLD_386, colorSum, 0.00005);
            return memberBotWorld || freeBotWorld;
        }

        /// <summary>
        /// Clean up the memory hog bitmap.
        /// </summary>
        public void Dispose()
        {
            if (Bitmap != null)
            {
                Bitmap.Dispose();
            }
        }

        private const int LOGIN_BOT_WORLD_385 = 130228;
        private const int LOGIN_BOT_WORLD_386 = 133468;

        #endregion

        #region implicit conversions

        /// <summary>
        /// Implicitly use the Color[,] Value of GameScreen when needed.
        /// </summary>
        /// <param name="screen">this GameScreen</param>
        public static implicit operator Color[,](GameScreen screen)
        {
            return screen.Value;
        }

        /// <summary>
        /// Indexer for the Color array.
        /// </summary>
        /// <param name="x">Horizontal indices from left.</param>
        /// <param name="y">Vertical indices from top.</param>
        public Color this[int x, int y]
        {
            get { return Value[x, y]; }
            set { Value[x, y] = value; }
        }

        #endregion
    }
}
