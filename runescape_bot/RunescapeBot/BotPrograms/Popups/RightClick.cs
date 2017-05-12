using RunescapeBot.Common;
using RunescapeBot.ImageTools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms.Popups
{
    public class RightClick
    {
        protected Process RSClient;
        protected int XClick;
        protected int YClick;
        protected int Height;
        protected int TitleHeight;
        protected int Width;
        protected int TitleHash;

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
            Height = 95;
            TitleHeight = 15;
            Width = 154;
            TitleHash = 50840;
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
        /// Waits for the Make-X popup to appear
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public virtual bool WaitForPopup(int timeout)
        {
            const int padding = 2;
            const int mouseOffset = 10;

            Color[,] screen = null;
            Stopwatch watch = new Stopwatch();
            watch.Start();
            long titleHash;

            int left = XClick - (Width / 2) + padding;
            int right = XClick - mouseOffset;
            int top = YClick + padding;
            int bottom = YClick + TitleHeight - padding;

            while (watch.ElapsedMilliseconds < timeout)
            {
                screen = ScreenScraper.GetRGB(ScreenScraper.CaptureWindow(RSClient));
                screen = ImageProcessing.ScreenPiece(screen, left, right, top, bottom);
                titleHash = ImageProcessing.ColorSum(screen);

                if (Numerical.CloseEnough(TitleHash, titleHash, 0.001))
                {
                    return true;
                }
            }
            DebugUtilities.SaveImageToFile(screen, "C:\\Projects\\Roboport\\test_pictures\\right-click.jpg");
            return false;
        }

        /// <summary>
        /// Waits for the "Enter amount:" prompt to appear over the chat box
        /// </summary>
        /// <param name="timeout">Gives up after the max wait time has elapsed</param>
        /// <returns>true if the prompt appears</returns>
        public bool WaitForEnterAmount(int timeout)
        {
            return Utilities.WaitForEnterAmount(RSClient, timeout);
        }
    }
}
