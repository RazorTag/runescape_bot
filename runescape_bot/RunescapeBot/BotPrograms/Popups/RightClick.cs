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
        protected Process RSClient;
        protected int XClick;
        protected int YClick;
        protected int Height;
        protected int Width;
        protected int TitleHeight;

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
        /// Sets the dimensions of the popup
        /// </summary>
        protected virtual void SetSize()
        {
            Height = 96;
            TitleHeight = 16;
            Width = 154;
        }

        /// <summary>
        /// Adjusts the popup position in cases where the popup runs into the bottom, left, or right of the screen
        /// </summary>
        private void AdjustPosition()
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
        public virtual bool WaitForPopup(int timeout = 3000)
        {
            Bitmap screen = null;
            Stopwatch watch = new Stopwatch();
            watch.Start();
            while (watch.ElapsedMilliseconds < timeout)
            {
                if (BotProgram.StopFlag) { return false; }

                screen = ScreenScraper.CaptureWindow(RSClient);

                if (PopupIsCorrectSize(screen))
                {
                    screen.Dispose();
                    return true;
                }
                screen.Dispose();
            }

            return false;
        }

        /// <summary>
        /// Verifies that the pop-up is roughly the expected size
        /// </summary>
        /// <returns></returns>
        private bool PopupIsCorrectSize(Bitmap screen)
        {
            const int xPadding = 20;
            const int yPadding = 6;
            int x = XClick + Width / 2 - xPadding;
            int y = YClick + Height - yPadding;
            Color bottomRight = screen.GetPixel(x, y);
            ColorRange rightClickColor = ColorFilters.RightClickPopup();
            return rightClickColor.ColorInRange(bottomRight);
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
        /// Selects an option from the right-click menu
        /// </summary>
        /// <param name="yOffset">y-offset of the middle of the option from the top of the popup</param>
        public void SelectOption(int yOffset, int maxXOffset = int.MaxValue)
        {
            int xRandomization = Math.Min(maxXOffset, Math.Max(10, (Width / 2) - 5));
            int yRandomization = 2;
            Point clickOffset = Probability.GaussianRectangle(-xRandomization, xRandomization, yOffset - yRandomization, yOffset + yRandomization);
            Point click = new Point(XClick + clickOffset.X, YClick + clickOffset.Y);
            Mouse.LeftClick(click.X, click.Y, RSClient);
        }
    }
}
