using RunescapeBot.Common;
using RunescapeBot.ImageTools;
using RunescapeBot.UITools;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;

namespace RunescapeBot.BotPrograms.Popups
{
    public class Bank
    {
        Process RSClient;
        public int Left { get; set; }
        public int Right { get; set; }
        public int Top { get; set; }
        public int Bottom { get; set; }

        public Bank(Process RSClient)
        {
            this.RSClient = RSClient;
            Point screenSize = ScreenScraper.GetWindowSize(RSClient);
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
            double yIntercept = -368.5;
            double slope = 0.5;
            Left = (int) Math.Round(yIntercept + (slope * screenWidth));
        }

        /// <summary>
        /// Determines the location of the right side of the bank pop-up
        /// </summary>
        private void SetRight(int screenWidth)
        {
            double yIntercept = 118.5;
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
                double yIntercept = -481.5;
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
                double yIntercept = 317.5;
                double slope = 0.5;
                Bottom = (int)Math.Round(yIntercept + (slope * screenHeight));
            }
        }

        /// <summary>
        /// Determines if the bank screen is currently visible
        /// </summary>
        /// <returns>true if the bank is open</returns>
        public bool BankIsOpen()
        {
            const double minTitleMatch = 0.05;
            double titleMatch;
            int left = Left + 162;
            int right = Left + 325;
            int top = Top + 8;
            int bottom = Top + 25;
            
            Color[,] screen;
            screen = ScreenScraper.GetRGB(ScreenScraper.CaptureWindow(RSClient));
            screen = ImageProcessing.ScreenPiece(screen, left, right, top, bottom);
            titleMatch = ImageProcessing.FractionalMatch(screen, RGBHSBRanges.BankTitle());
            
            return titleMatch > minTitleMatch;
        }

        /// <summary>
        /// Wait for the bank pop-up to open
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns>true if the bank pop-up opens</returns>
        public bool WaitForPopup(int timeout = 3000)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            while (watch.ElapsedMilliseconds < timeout)
            {
                if (BotProgram.StopFlag) { return false; }

                if (BankIsOpen())
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Clicks on the "Deposit Inventory" button in the bank pop-up
        /// </summary>
        /// <param name="screenWidth">width of the game screen in pixels</param>
        /// <param name="screenHeight">height of the game screen in pixels</param>
        public void DepositInventory()
        {
            const int xOffset = 427;
            const int yOffset = 22; //offset from bottom
            Point click = Probability.GaussianCircle(new Point(Left + xOffset, Bottom - yOffset), 2, 0, 360, 5);
            Mouse.LeftClick(click.X, click.Y, RSClient);
            Thread.Sleep(200 + (int)Probability.HalfGaussian(0, 10, true));
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

            Point click = Probability.GaussianCircle(new Point(itemSlot.Value.X, itemSlot.Value.Y), 4, 0, 360, 11);
            Mouse.LeftClick(click.X, click.Y, RSClient);
        }

        /// <summary>
        /// Use the Withdraw N option to withdraw the specified number of items
        /// </summary>
        /// <param name="column">slots from the left</param>
        /// <param name="row">slots from the top</param>
        /// <param name="quantity">number of items to withdraw</param>
        public void WithdrawN(int column, int row)
        {
            const int yOffset = 70;
            WithdrawMenuClick(column, row, yOffset);
        }

        /// <summary>
        /// Use the Withdraw X option to withdraw the specified number of items
        /// </summary>
        /// <param name="column">slots from the left</param>
        /// <param name="row">slots from the top</param>
        /// <param name="quantity">number of items to withdraw</param>
        public void WithdrawX(int column, int row, int quantity)
        {
            const int yOffset = 85;
            WithdrawMenuClick(column, row, yOffset);
            if (WaitForEnterAmount(5000))
            {
                BotUtilities.EnterAmount(RSClient, quantity);
            }
        }

        /// <summary>
        /// Withdraw all items using Withdraw-All
        /// </summary>
        /// <param name="column">bank item slots from left slot (0-7)</param>
        /// <param name="row">bank item slots from top slot (0-n)</param>
        /// <param name="numberToWithdraw"></param>
        public void WithdrawAll(int column, int row)
        {
            const int yOffset = 100;
            WithdrawMenuClick(column, row, yOffset);
        }

        /// <summary>
        /// Opens the Withdraw-X menu for a specified bank slot and clicks on the specified withdraw option
        /// </summary>
        /// <param name="column"></param>
        /// <param name="row"></param>
        /// <param name="yOffset"></param>
        public void WithdrawMenuClick(int column, int row, int yOffset)
        {
            Point? itemSlot = ItemSlotLocation(column, row);
            if (itemSlot == null)
            {
                return;
            }

            //open the withdraw right-click menu
            Point click = Probability.GaussianCircle(new Point(itemSlot.Value.X, itemSlot.Value.Y), 4, 0, 360, 11);
            Mouse.RightClick(click.X, click.Y, RSClient);
            BotProgram.SafeWaitPlus(200, 100);
            RightClick menu = new RightClick(click.X, click.Y, RSClient, 9);
            menu.WaitForPopup();

            //click on Withdraw-All
            click = Probability.GaussianRectangle(click.X - 90, click.X + 90, click.Y + yOffset - 2, click.Y + yOffset + 2);
            Mouse.LeftClick(click.X, click.Y, RSClient);
            BotProgram.SafeWaitPlus(200, 100);
        }

        /// <summary>
        /// Close the bank pop-up using the top-right X button
        /// </summary>
        public void CloseBank()
        {
            const int xOffset = 469;
            const int yOffset = 17;
            Point click = Probability.GaussianCircle(new Point(Left + xOffset, Top + yOffset), 3, 0, 360, 7);
            Mouse.LeftClick(click.X, click.Y, RSClient);
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
    }
}
