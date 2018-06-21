using RunescapeBot.Common;
using RunescapeBot.ImageTools;
using RunescapeBot.UITools;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;

namespace RunescapeBot.BotPrograms.Popups
{
    public class RightClick
    {
        protected const int MAX_ROWS = 20;
        protected const int TITLE_HEIGHT = 16;
        protected const int ROW_HEIGHT = 15;
        protected const int MIN_WIDTH = 103;
        protected Process RSClient;
        protected int XClick;
        protected int YClick;
        protected int Height;
        protected int Width;

        public int Top { get { return YClick; } }
        public int Bottom { get { return YClick + Height; } }

        public int GetHeight() { return Height; }
        public int GetWidth() { return Width; }

        /// <summary>
        /// Create a record of a basic popup
        /// </summary>
        /// <param name="xClick">the x-coordinate of the click that opened the Make-X popup</param>
        /// <param name="yClick">the y-coordinate of the click that opened the Make-X popup</param>
        /// <param name="rsClient"></param>
        public RightClick(int xClick, int yClick, Process rsClient)
        {
            this.RSClient = rsClient;
            this.XClick = xClick;
            this.YClick = yClick;
            SetSize();
            AdjustPosition();
        }

        /// <summary>
        /// Create a record of a basic popup
        /// </summary>
        /// <param name="xClick">the x-coordinate of the click that opened the Make-X popup</param>
        /// <param name="yClick">the y-coordinate of the click that opened the Make-X popup</param>
        /// <param name="rsClient"></param>
        /// <param name="rows">Expected number of lines in the popup. Include all rows expect for the title.</param>
        public RightClick(int xClick, int yClick, Process rsClient, int rows)
        {
            this.RSClient = rsClient;
            this.XClick = xClick;
            this.YClick = yClick;
            SetSize();
            this.Height = TITLE_HEIGHT + (rows * ROW_HEIGHT);
            AdjustPosition();
        }

        /// <summary>
        /// Create a record of a basic popup
        /// </summary>
        /// <param name="xClick">the x-coordinate of the click that opened the Make-X popup</param>
        /// <param name="yClick">the y-coordinate of the click that opened the Make-X popup</param>
        /// <param name="rsClient"></param>
        public RightClick(int xClick, int yClick, Process rsClient, int width, int height)
        {
            this.RSClient = rsClient;
            this.XClick = xClick;
            this.YClick = yClick;
            this.Width = width;
            this.Height = height;
            AdjustPosition();
        }

        /// <summary>
        /// Sets the dimensions of the popup
        /// </summary>
        protected virtual void SetSize()
        {
            Width = 100;
            Height = 50;
        }

        /// <summary>
        /// Adjusts the popup position in cases where the popup runs into the bottom, left, or right of the screen
        /// </summary>
        protected void AdjustPosition()
        {
            Point? screenSize = ScreenScraper.GetScreenSize(RSClient);
            if (screenSize != null)
            {
                //adjust for hitting the bottom of the screen
                YClick = Math.Min(YClick, screenSize.Value.Y - Height);

                //adjust for hitting the left of the screen
                XClick = Math.Max(XClick, Width / 2);

                //adjust for hitting the right of the screen
                XClick = Math.Min(XClick, screenSize.Value.X - (Width / 2));
            }
        }

        /// <summary>
        /// Waits for the right-click popup to appear
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public virtual bool WaitForPopup(int timeout = 3000, CheckHeight checkHeight = CheckHeight.None)
        {
            Color[,] screen = null;
            Stopwatch watch = new Stopwatch();
            watch.Start();
            while (watch.ElapsedMilliseconds < timeout)
            {
                if (BotProgram.SafeWait(200)) { return false; }
                screen = ScreenScraper.GetRGB(ScreenScraper.CaptureWindow(RSClient));
                if (PopupExists(screen))
                {
                    return PopupIsCorrectHeight(screen, checkHeight);
                }
            }
            
            return false;
        }

        /// <summary>
        /// Method for verifying the popup
        /// </summary>
        public enum CheckHeight : int
        {
            None,   //Do not check height at all
            Half,   //Verify that the popup is at least as tall as we expect.
            Full    //Verify that the popup is exactly as tall as we expect.
        }

        /// <summary>
        /// Checks the area where the title bar should be for blackness
        /// </summary>
        /// <param name="screen">the full game screen</param>
        /// <returns>true if the black popup title bar is found</returns>
        protected bool PopupExists(Color[,] screen)
        {   
            int top = YClick + 1;
            int bottom = top + TITLE_HEIGHT - 1;
            int left = XClick - MIN_WIDTH / 2;
            int right = XClick + MIN_WIDTH / 2;
            double blackness = ImageProcessing.FractionalMatchPiece(screen, RGBHSBRangeFactory.Black(), left, right, top, bottom);
            return blackness > 0.25;
        }

        /// <summary>
        /// Checks the area where the center of the bottom row of the popup is expected to be
        /// </summary>
        /// <param name="screen">the full game screen</param>
        /// <returns>true if the expected bottom row is found</returns>
        protected bool PopupIsCorrectHeight(Color[,] screen, CheckHeight checkHeight)
        {
            if (checkHeight == CheckHeight.None)
            {
                return true;    //No height checking needed
            }

            int left = XClick - MIN_WIDTH / 2;
            int right = XClick + MIN_WIDTH / 2;
            int top = Bottom - ROW_HEIGHT;
            double lastRow = ImageProcessing.FractionalMatchPiece(screen, RGBHSBRangeFactory.RightClickPopup(), left, right, top, Bottom);

            switch (checkHeight)
            {
                case CheckHeight.Half:
                    return lastRow > 0.7;
                case CheckHeight.Full:
                    double belowPopup = ImageProcessing.FractionalMatchPiece(screen, RGBHSBRangeFactory.RightClickPopup(), left, right, top + ROW_HEIGHT, Bottom + ROW_HEIGHT);
                    return lastRow > 0.7 && belowPopup < 0.7;
            }

            return false;   //Should never reach here.
        }

        /// <summary>
        /// Verifies that the pop-up is roughly the expected size
        /// </summary>
        /// <returns></returns>
        protected virtual void GetPopupHeight(Bitmap screen)
        {
            int yOffset;
            for (int row = 0; row < MAX_ROWS; row++)
            {
                yOffset = RowOffset(row);
                Color background = screen.GetPixel(XClick, YClick + yOffset);
                RGBHSBRange rightClickColor = RGBHSBRangeFactory.RightClickPopup();
                if (rightClickColor.ColorInRange(background))
                {
                    Height = TITLE_HEIGHT + ((row + 1) * ROW_HEIGHT);
                }
            }
        }

        /// <summary>
        /// Waits for the "Enter amount:" prompt to appear over the chat box
        /// </summary>
        /// <param name="timeout">Gives up after the max wait time has elapsed</param>
        /// <returns>true if the prompt appears</returns>
        public bool WaitForEnterAmount(int timeout)
        {
            return BotUtilities.WaitForEnterAmount(RSClient, timeout);
        }

        /// <summary>
        /// Selects an option from the right-click menu by y-offset in terms pf pixels
        /// </summary>
        /// <param name="yOffset">y-offset of the middle of the option from the top of the popup</param>
        protected void SelectOption(int yOffset, int maxXOffset = int.MaxValue)
        {
            int xRandomization = Math.Min(maxXOffset, Math.Max(10, (Width / 2) - 5));
            int yRandomization = 2;
            Point clickOffset = Probability.GaussianRectangle(-xRandomization, xRandomization, yOffset - yRandomization, yOffset + yRandomization);
            Point click = new Point(XClick + clickOffset.X, YClick + clickOffset.Y);
            Mouse.LeftClick(click.X, click.Y, RSClient);
        }

        /// <summary>
        /// Select an option from the right-click menu in terms of order in the list of options
        /// </summary>
        /// <param name="whichOption">the row number (starting from 0) of the option to click</param>
        public void CustomOption(int rowNumber)
        {
            int yOffset = RowOffset(rowNumber);
            SelectOption(yOffset);
        }

        /// <summary>
        /// Gets the y-offset of the middle of a row indexed from top to bottom
        /// </summary>
        /// <param name="rowNumber">row numbering starts at 0</param>
        /// <returns></returns>
        protected int RowOffset(int rowNumber)
        {
            return 25 + (ROW_HEIGHT * rowNumber);
        }
    }
}
