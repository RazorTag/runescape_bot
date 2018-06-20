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
    /// <summary>
    /// Tools for the textbox in the bottom left corner of the screen
    /// </summary>
    public class TextBoxTool
    {
        protected Process RSClient;
        protected Color[,] Screen;
        protected Keyboard Keyboard;

        public int Left { get { return Screen == null ? 0 : 0; } }
        public int Right { get { return Screen == null ? 0 : Left + 518; } }
        public int Top { get { return Screen == null ? 0 : Screen.GetLength(1) - 165; } }
        public int Bottom { get { return Screen == null ? 0 : Top + 141; } }
        public int Area { get { return (Right - Left) * (Bottom - Top); } }
        private double PixelSize { get { return 1 / Area; } }

        public TextBoxTool(Process rsClient, Keyboard keyboard)
        {
            RSClient = rsClient;
            Keyboard = keyboard;
        }

        /// <summary>
        /// Updates the game screen image.
        /// </summary>
        /// <param name="screen">new image of the entire game screen</param>
        public void SetScreen(Color[,] screen)
        {
            Screen = screen;
        }

        /// <summary>
        /// Called by BotProgram to update with the most current client process.
        /// </summary>
        /// <param name="colorArray">the newest screenshot</param>
        public void SetClient(Process rsClient)
        {
            RSClient = rsClient;
        }

        /// <summary>
        /// Takes a hash of the textbox dialog area
        /// </summary>
        /// <param name="filter">filter for text colors to look for</param>
        /// <returns>the portion of the dialog text area filled with text color</returns>
        public double DialogBodyText()
        {
            Screen = ScreenScraper.ReadWindow(RSClient, true);
            ColorFilter filter = RGBHSBRangeGroupFactory.DialogText();
            double match = ImageProcessing.FractionalMatchPiece(Screen, filter, Left + 126, Right - 126, Top + 46, Bottom - 38);
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
                Screen = ScreenScraper.ReadWindow(RSClient, true);
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
    }
}
