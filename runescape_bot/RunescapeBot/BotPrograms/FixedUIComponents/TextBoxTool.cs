using RunescapeBot.Common;
using RunescapeBot.ImageTools;
using RunescapeBot.UITools;
using System.Diagnostics;
using System.Drawing;

namespace RunescapeBot.BotPrograms
{
    /// <summary>
    /// Tools for the textbox in the bottom left corner of the screen
    /// </summary>
    public class TextBoxTool
    {
        #region properties

        private RSClient RSClient;
        protected Keyboard Keyboard;
        private GameScreen Screen;

        public static RGBHSBRange PlayerChatText = RGBHSBRangeFactory.GenericColor(Color.Blue);

        public const int ROW_HEIGHT = 14;

        /// <summary>
        /// Bounds of the text box (inside the frame of the text box).
        /// Right boun is inside of the scroll bar.
        /// </summary>
        public int Left { get { return Screen.LooksValid() ? 7 : 0; } }
        public int Right { get { return Screen.LooksValid() ? Left + 488 : 0; } }
        public int Top { get { return Screen.LooksValid() ? Bottom - 128 : 0; } }
        public int Bottom { get { return Screen.LooksValid() ? Screen.Height - 30 : 0; } }
        public int Area { get { return (Right - Left) * (Bottom - Top); } }

        /// <summary>
        /// 
        /// </summary>
        public Color[,] ChatBody { get { return Screen.SubScreen(Left, Right, Top, Bottom); } }
        private double PixelSize { get { return 1.0 / Area; } }

        #endregion

        #region constructors

        public TextBoxTool(RSClient rsClient, Keyboard keyboard, GameScreen screen)
        {
            RSClient = rsClient;
            Keyboard = keyboard;
            Screen = screen;
        }

        #endregion

        #region NPC dialog

        /// <summary>
        /// Determines the area corresponding to a row of text.
        /// </summary>
        /// <param name="rowIndex">The index of the the row to locate (0-7, bottom to top).</param>
        /// <returns>The bounds of the area containing the text for the specified row.</returns>
        public RectangleBounds ChatRowLocation(int rowIndex)
        {
            RectangleBounds rowLocation = new RectangleBounds();
            //TODO
            return rowLocation;
        }

        /// <summary>
        /// Determines the area where the player's display name and typing text appear.
        /// </summary>
        /// <returns></returns>
        public RectangleBounds ChatEntryLocation()
        {
            RectangleBounds rowLocation = new RectangleBounds();
            rowLocation.Left = Left + 4;
            rowLocation.Right = Right;
            rowLocation.Top = Bottom - 13;
            rowLocation.Bottom = rowLocation.Top + ROW_HEIGHT;
            return rowLocation;
        }

        /// <summary>
        /// Takes a hash of the textbox dialog area
        /// </summary>
        /// <param name="filter">filter for text colors to look for</param>
        /// <returns>the portion of the dialog text area filled with text color</returns>
        public double DialogBodyText()
        {
            Screen.Value = ScreenScraper.ReadWindow(true);
            ColorFilter filter = RGBHSBRangeGroupFactory.DialogText();
            double match = ImageProcessing.FractionalMatchPiece(Screen, filter, Left + 119, Right - 103, Top + 39, Bottom - 32);
            return match;
        }

        /// <summary>
        /// Determines if the current 
        /// </summary>
        /// /// <param name="filter">color filter to match the expected text color</param>
        /// <param name="expectedText">hash of the expected text body</param>
        /// <param name="allowedPixelDifference">maximum allowed deviation from the expected hash value in pixels</param>
        /// <param name="filter">color filter to match the expected text color</param>
        /// <returns>true if the text matches the expected hash</returns>
        public bool DialogBodyTextMatch(double expectedText, int allowedPixelDifference)
        {
            double match = DialogBodyText();
            double tolerance = (allowedPixelDifference + 0.5) * PixelSize;  //use an extra half pixel to avoid rounding errors
            return Numerical.WithinRange(expectedText, match, tolerance);
        }

        /// <summary>
        /// Waits for an expected set of text to appear in the textbox
        /// </summary>
        /// <param name="timeout">time in milliseconds to keep trying before giving up</param>
        /// <param name="filter">color filter to match the expected text color</param>
        /// <param name="expectedText">hash of the expected text body</param>
        /// <param name="allowedPixelDifference">maximum allowed deviation from the expected hash value in pixels</param>
        /// <param name="filter">color filter to match the expected text color</param>
        /// <returns>true if matching text appears</returns>
        public bool WaitForExpectedText(double expectedText, int allowedPixelDifference, int timeout)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            while (watch.ElapsedMilliseconds < timeout && !BotProgram.StopFlag)
            {
                Screen.Value = ScreenScraper.ReadWindow(true);
                if (DialogBodyTextMatch(expectedText, allowedPixelDifference))
                {
                    return true;
                }
                BotProgram.SafeWait(100);
            }
            return false;
        }

        /// <summary>
        /// Clicks through a specified number of text screens with the space bar.
        /// </summary>
        /// <param name="screenCount">number of text screens to click through</param>
        /// <param name="timeout">max time allowed to wait for each text screen</param>
        /// <returns></returns>
        public bool ClickThroughTextScreens(int screenCount, int timeout)
        {
            Stopwatch watch = new Stopwatch();
            int screensClicked = 0;
            double textHash = 0, priorText = 0;

            watch.Start();
            while (screensClicked < screenCount && watch.ElapsedMilliseconds < timeout && !BotProgram.StopFlag)
            {
                textHash = DialogBodyText();
                if (textHash != priorText)
                {
                    priorText = textHash;
                    Keyboard.Space();
                    screensClicked++;
                    BotProgram.SafeWaitPlus(500, 100);
                    watch.Restart();
                }
                else
                {
                    BotProgram.SafeWait(100);
                }
            }

            return screensClicked == screenCount;
        }

        #endregion
    }
}
