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

        protected Keyboard Keyboard;
        private GameScreen Screen;

        public static RGBHSBRange PlayerChatText = RGBHSBRangeFactory.GenericColor(Color.Blue);

        public const int ROW_HEIGHT = 14;
        public const int INPUT_ROW_HEIGHT = ROW_HEIGHT + 1;
        public const int INPUT_DIVIDER_WIDTH = 1;
        public const int WIDTH = 489;
        public const int HEIGHT = 129;

        /// <summary>
        /// Number of rows visible in the public chat history.
        /// </summary>
        public const int CHAT_ROW_COUNT = 8;

        /// <summary>
        /// Offset from the bottom of the textbox tool to the bottom of the output area.
        /// </summary>
        public const int INPUT_OFFSET = INPUT_ROW_HEIGHT + INPUT_DIVIDER_WIDTH;

        /// <summary>
        /// Bounds of the text box (inside the frame of the text box).
        /// Right bound is inside of the scroll bar.
        /// </summary>
        public int Left { get { return Screen.LooksValid() ? 7 : 0; } }
        public int Right { get { return Screen.LooksValid() ? Left + (WIDTH - 1) : 0; } }
        public int Top { get { return Screen.LooksValid() ? Bottom - (HEIGHT - 1) : 0; } }
        public int Bottom { get { return Screen.LooksValid() ? Screen.Height - 30 : 0; } }

        /// <summary>
        /// The y-coordinate of the line that divides the input box from the chat history box.
        /// </summary>
        public int InputDivider { get { return Screen.LooksValid() ? Bottom - 15 : 0; } }

        /// <summary>
        /// Area of the full text box.
        /// </summary>
        public int Area { get { return (Right - Left + 1) * (Bottom - Top + 1); } }
        private double PixelSize { get { return 1.0 / Area; } }

        /// <summary>
        /// Image of the full text box
        /// </summary>
        public Color[,] TextBoxImage
        {
            get
            {
                Color[,] image = Screen.SubScreen(Left, Right, Top, Bottom);
                return (image.GetLength(0) == WIDTH && image.GetLength(1) == HEIGHT) ? image : null;
            }
        }

        /// <summary>
        /// The location of the chat row within the input box.
        /// </summary>
        public RectangleBounds InputLocation
        {
            get
            {
                int top = InputDivider + 2;
                int bottom = top + (ROW_HEIGHT - 1);
                return new RectangleBounds(Left, Right, top, bottom);
            }
        }

        /// <summary>
        /// Image of the implicit chat row within the input box.
        /// </summary>
        public Color[,] InputImage
        {
            get
            {
                return Screen.SubScreen(InputLocation);
            }
        }

        #endregion

        #region constructors

        public TextBoxTool( Keyboard keyboard, GameScreen screen)
        {
            Keyboard = keyboard;
            Screen = screen;
        }

        #endregion

        #region NPC dialog

        /// <summary>
        /// Determines the area corresponding to a row of text within the TextBoxTool area.
        /// </summary>
        /// <param name="rowIndex">The index of the the row to locate (0-7, bottom to top).</param>
        /// <returns>The bounds of the area containing the text for the specified row.</returns>
        public RectangleBounds ChatRowLocation(int rowIndex)
        {
            int bottom = InputDivider - 1;
            int top = bottom - (ROW_HEIGHT - 1);
            RectangleBounds rowLocation = new RectangleBounds(Left, Right, top, bottom);
            return rowLocation;
        }

        /// <summary>
        /// Gets an image of the specified chat row.
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        public Color[,] ChatRowImage(int rowIndex)
        {
            RectangleBounds bounds = ChatRowLocation(rowIndex);
            return Screen.SubScreen(bounds);
        }

        /// <summary>
        /// Gets an image of the specified chat row.
        /// </summary>
        /// <param name="rowIndex">Starts at 0. Goes from bottom to top. Does not include the input row.</param>
        /// <returns></returns>
        public static Color[,] ChatRowImage(Color[,] textBoxImage, int rowIndex)
        {
            int bottom = (textBoxImage.GetLength(1) - 1) - INPUT_OFFSET - (rowIndex * ROW_HEIGHT);
            int top = bottom - (ROW_HEIGHT - 1);
            Color[,] chatRowImage = ImageProcessing.ScreenPiece(textBoxImage, 0, textBoxImage.GetLength(0), top, bottom);
            return chatRowImage;
        }

        /// <summary>
        /// Determines the area where the player's display name and currently typing text appear.
        /// </summary>
        /// <returns></returns>
        public RectangleBounds ChatEntryLocation()
        {
            RectangleBounds rowLocation = new RectangleBounds();
            rowLocation.Left = Left + 4;
            rowLocation.Right = Right;
            rowLocation.Bottom = Bottom;
            rowLocation.Top = rowLocation.Bottom - (ROW_HEIGHT - 1);
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
