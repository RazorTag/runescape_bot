using RunescapeBot.Common;
using RunescapeBot.ImageTools;
using RunescapeBot.UITools;
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
    public class FurnaceCrafting
    {
        private const int NO_JEWEL_X_OFFSET = 98;
        private const int ITEM_ICON_WIDTH = 50;
        private const int BRACELET_Y_OFFSET = 264;

        private Process RSClient;
        private int Left;
        private int Top;

        //enum value is the x-offset from the left side of the popup
        public enum Jewel : int
        {
            None = 0,
            Sapphire = 1,
            Emerald = 2,
            Ruby = 3,
            Diamond = 4,
            Dragonstone = 5,
            Onxy = 6,
            Zenyte = 7
        }

        public FurnaceCrafting(Process rsClient)
        {
            this.RSClient = rsClient;
            Point screenSize = ScreenScraper.GetWindowSize(RSClient);
            SetLeft(screenSize.X);
            SetTop(screenSize.Y);
        }

        /// <summary>
        /// Determines the location of the left side of the bank pop-up
        /// </summary>
        private void SetLeft(int screenWidth)
        {
            double yIntercept = -369;
            double slope = 0.5;
            Left = (int)Math.Round(yIntercept + (slope * screenWidth));
        }

        /// <summary>
        /// Determines the location of the top side of the bank pop-up
        /// </summary>
        private void SetTop(int screenHeight)
        {
            double yIntercept = -229;
            double slope = 0.5;
            Top = (int)Math.Round(yIntercept + (slope * screenHeight));
        }

        /// <summary>
        /// Waits for the furnace crafting popup to appear
        /// </summary>
        /// <returns>true if the popup is found</returns>
        public bool WaitForPopup(int timeout)
        {
            const int popupTitleHash = 842393;
            Color[,] screen;
            Stopwatch watch = new Stopwatch();
            watch.Start();
            long titleHash;

            int left = Left + 144;
            int right = Left + 344;
            int top = Top + 4;
            int bottom = Top + 23;

            while (watch.ElapsedMilliseconds < timeout)
            {
                if (BotProgram.StopFlag) { return false; }

                screen = ScreenScraper.GetRGB(ScreenScraper.CaptureWindow(RSClient));
                screen = ImageProcessing.ScreenPiece(screen, left, right, top, bottom);
                titleHash = ImageProcessing.ColorSum(screen);
                
                if (Numerical.CloseEnough(popupTitleHash, titleHash, 0.001))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Makes the specified number of bracelets
        /// </summary>
        /// <param name="jewel">The type of jewel to use (sapphire, emerald, ruby, etc.)</param>
        /// <param name="numberToMake">Number of bracelets to craft</param>
        public void MakeBracelets(Jewel jewel, int numberToMake, int timeout)
        {
            int left = Left + NO_JEWEL_X_OFFSET + (((int)jewel) * ITEM_ICON_WIDTH) - 4;
            int right = Left + NO_JEWEL_X_OFFSET + (((int)jewel) * ITEM_ICON_WIDTH) + 4;
            int top = Top + BRACELET_Y_OFFSET - 4;
            int bottom = Top + BRACELET_Y_OFFSET + 4;
            Point click = Probability.GaussianRectangle(left, right, top, bottom);
            Mouse.RightClick(click.X, click.Y, RSClient);
            MakeAll makeAll = new MakeAll(click.X, click.Y, RSClient);
            makeAll.WaitForPopup(timeout);
            makeAll.MakeAllItems();
        }

        /// <summary>
        /// Closes the pop-up using the top-right X button
        /// </summary>
        public void Close()
        {
            const int xOffset = 474;
            const int yOffset = 17;
            Point click = Probability.GaussianRectangle(Left + xOffset - 6, Left + xOffset + 6, Top + yOffset - 5, Top + yOffset + 5);
            Mouse.LeftClick(click.X, click.Y, RSClient);
        }
    }
}