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
        private Random RNG;
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
            RNG = new Random();
            Point screenSize = ScreenScraper.GetOSBuddyWindowSize(RSClient);
            SetLeft(screenSize.X);
            SetTop(screenSize.Y);
        }

        /// <summary>
        /// Determines the location of the left side of the bank pop-up
        /// </summary>
        private void SetLeft(int screenWidth)
        {
            double yIntercept = -370;
            double slope = 0.5;
            Left = (int)Math.Round(yIntercept + (slope * screenWidth));
        }

        /// <summary>
        /// Determines the location of the top side of the bank pop-up
        /// </summary>
        private void SetTop(int screenHeight)
        {
            double yIntercept = -229.5;
            double slope = 0.5;
            Top = (int)Math.Round(yIntercept + (slope * screenHeight));
        }

        /// <summary>
        /// Waits for the furnace crafting popup to appear
        /// </summary>
        /// <returns>true if the popup is found</returns>
        public bool WaitForPopup(int timeout)
        {
            const int popupTitleHash = 768146;
            Color[,] screen;
            Stopwatch watch = new Stopwatch();
            watch.Start();
            long titleHash;

            int left = Left + 145;
            int right = Left + 345;
            int top = Top + 8;
            int bottom = Top + 25;

            while (watch.ElapsedMilliseconds < timeout)
            {
                screen = ScreenScraper.GetRGB(ScreenScraper.CaptureWindow(RSClient));
                screen = ImageProcessing.ScreenPiece(screen, left, right, top, bottom);
                titleHash = ImageProcessing.ColorSum(screen);
                if (Numerical.CloseEnough(popupTitleHash, titleHash, 0.05))
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
        public void MakeBracelets(Jewel jewel, int numberToMake)
        {
            int xClick = Left + NO_JEWEL_X_OFFSET + (((int)jewel) * ITEM_ICON_WIDTH) + RNG.Next(-4, 5);
            int yClick = Top + BRACELET_Y_OFFSET + RNG.Next(-4, 5);
            Mouse.RightClick(xClick, yClick, RSClient);
            MakeX makeX = new MakeX(xClick, yClick, RSClient);
            makeX.MakeXItems(numberToMake);
        }

        /// <summary>
        /// Closes the pop-up using the top-right X button
        /// </summary>
        public void Close()
        {
            const int xOffset = 474;
            const int yOffset = 17;
            int xClick = Left + xOffset + RNG.Next(-6, 7);
            int yClick = Top + yOffset + RNG.Next(-5, 6);
        }
    }
}