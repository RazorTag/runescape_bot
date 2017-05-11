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
    public class Bank
    {
        Process RSClient;
        Random RNG;
        public int Left { get; set; }
        public int Right { get; set; }
        public int Top { get; set; }
        public int Bottom { get; set; }

        public Bank(Process RSClient)
        {
            this.RSClient = RSClient;
            RNG = new Random();
            Point screenSize = ScreenScraper.GetOSBuddyWindowSize(RSClient);
            SetLeft(screenSize.X);
            SetRight(screenSize.X);
            SetTop(screenSize.Y);
            SetBottom(screenSize.Y);
        }

        /// <summary>
        /// Determines the location of the left side of the bank pop-up
        /// </summary>
        private void SetLeft(int screenWidth)
        {
            double yIntercept = -369.5;
            double slope = 0.5;
            Left = (int) Math.Round(yIntercept + (slope * screenWidth));
        }

        /// <summary>
        /// Determines the location of the right side of the bank pop-up
        /// </summary>
        private void SetRight(int screenWidth)
        {
            double yIntercept = 117.5;
            double slope = 0.5;
            Right = (int) Math.Round(yIntercept + (slope * screenWidth));
        }

        /// <summary>
        /// Determines the location of the top side of the bank pop-up
        /// </summary>
        private void SetTop(int screenHeight)
        {
            if (screenHeight < 970)
            {
                Top = 3;
            }
            else
            {
                double yIntercept = -482;
                double slope = 0.5;
                Top = (int) Math.Round(yIntercept + (slope * screenHeight));
            }
        }

        /// <summary>
        /// Determines the location of the bottom side of the bank pop-up
        /// </summary>
        private void SetBottom(int screenHeight)
        {
            if (screenHeight < 970)
            {
                Bottom = screenHeight - 167;
            }
            else
            {
                double yIntercept = 317;
                double slope = 0.5;
                Bottom = (int)Math.Round(yIntercept + (slope * screenHeight));
            }
        }

        /// <summary>
        /// Wait for the bank pop-up to open
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns>true if the bank pop-up opens</returns>
        public bool WaitForPopup(int timeout)
        {
            const int popupTitleHash = 625098;
            Color[,] screen;
            Stopwatch watch = new Stopwatch();
            watch.Start();
            long titleHash;

            int left = Left + 162;
            int right = Left + 325;
            int top = Top + 8;
            int bottom = Top + 25;

            while (watch.ElapsedMilliseconds < timeout)
            {
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
        /// Clicks on the "Deposit Inventory" button in the bank pop-up
        /// </summary>
        /// <param name="screenWidth">width of the game scree in pixels</param>
        /// <param name="screenHeight">height of the game screen in pixels</param>
        public void DepositInventory()
        {
            const int xOffset = 427;
            const int yOffset = 22; //offset from bottom
            int x = Left + xOffset + RNG.Next(-5, 6);
            int y = Bottom - yOffset + RNG.Next(-5, 6);
            Mouse.LeftClick(x, y, RSClient);
            Thread.Sleep(200);
        }

        /// <summary>
        /// Determines the point on screen of a bank item slot.
        /// Assumes that the scroll is at the top of the current tab.
        /// Only works on the current tab.
        /// </summary>
        /// <param name="column">bank item slots from left slot (0-7)</param>
        /// <param name="row">bank item slots from top slot (0-n)</param>
        /// <returns></returns>
        private Point? ItemSlotLocation(int column, int row)
        {
            if (column < 0 || column > 7 || row < 0)
            {
                return null;
            }

            const int xOffset = 71;
            const int yOffset = 91;
            const int slotWidth = 48;
            const int slotHeight = 36;

            int xCoordinate = Left + xOffset + (column * slotWidth);
            int yCoordinate = Top + yOffset + (row * slotHeight);
            return new Point(xCoordinate, yCoordinate);
        }

        /// <summary>
        /// Withdraw a single item by left clicking
        /// </summary>
        /// <param name="column">bank item slots from left slot (0-7)</param>
        /// <param name="row">bank item slots from top slot (0-n)</param>
        public void WithdrawOne(int column, int row)
        {
            Point? itemSlot = ItemSlotLocation(column, row);
            if (itemSlot == null)
            {
                return;
            }

            int xClick = itemSlot.Value.X + RNG.Next(-10, 11);
            int yClick = itemSlot.Value.Y + RNG.Next(-10, 11);
            Mouse.LeftClick(xClick, yClick, RSClient);
        }

        /// <summary>
        /// Withdraw X items using Withdraw-X
        /// </summary>
        /// <param name="column">bank item slots from left slot (0-7)</param>
        /// <param name="row">bank item slots from top slot (0-n)</param>
        /// <param name="numberToWithdraw"></param>
        public void WithdrawAll(int column, int row)
        {
            Point? itemSlot = ItemSlotLocation(column, row);
            if (itemSlot == null)
            {
                return;
            }

            const int withdrawAllOffset = 101;

            //open the withdraw right-click menu
            int xClick = itemSlot.Value.X + RNG.Next(-10, 11);
            int yClick = itemSlot.Value.Y + RNG.Next(-10, 11);
            Mouse.RightClick(xClick, yClick, RSClient);
            Thread.Sleep(200);

            //click on Withdraw-All
            xClick += RNG.Next(-90, 91);
            yClick += withdrawAllOffset + RNG.Next(-2, 3);
            Mouse.LeftClick(xClick, yClick, RSClient);
            Thread.Sleep(200);
        }

        /// <summary>
        /// Close the bank pop-up using the top-right X button
        /// </summary>
        public void CloseBank()
        {
            const int xOffset = 469;
            const int yOffset = 17;
            int xClick = Left + xOffset + RNG.Next(-6, 7);
            int yClick = Top + yOffset + RNG.Next(-5, 6);
            Mouse.LeftClick(xClick, yClick, RSClient);
        }
    }
}
