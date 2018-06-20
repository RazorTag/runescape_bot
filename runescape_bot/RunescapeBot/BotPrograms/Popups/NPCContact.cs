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

namespace RunescapeBot.BotPrograms.Popups
{
    public class NPCContact
    {
        const int NPC_CONTACT_CAST_TIME = 8 * BotRegistry.GAME_TICK;    //minimum time between choosing an NPC to contact and the NPC dialog opening

        Process RSClient;
        public int Left { get; set; }
        public int Right { get; set; }
        public int Top { get; set; }
        public int Bottom { get; set; }

        public NPCContact(Process rsClient)
        {
            RSClient = rsClient;
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
            if (screenWidth < 765)
            {
                Left = 8;
            }
            else
            {
                double yIntercept = -375;
                double slope = 0.5;
                Left = (int)Math.Round(yIntercept + (slope * screenWidth));
            }
        }

        /// <summary>
        /// Determines the location of the right side of the bank pop-up
        /// </summary>
        private void SetRight(int screenWidth)
        {
            if (screenWidth < 765)
            {
                Right = Left + 499;
            }
            else
            {
                double yIntercept = 124;
                double slope = 0.5;
                Right = (int)Math.Round(yIntercept + (slope * screenWidth));
            }
        }

        /// <summary>
        /// Determines the location of the top side of the bank pop-up
        /// </summary>
        private void SetTop(int screenHeight)
        {
            if (screenHeight < 503)
            {
                Top = 23;
            }
            else
            {
                double yIntercept = -228.5;
                double slope = 0.5;
                Top = (int)Math.Round(yIntercept + (slope * screenHeight));
            }
        }

        /// <summary>
        /// Determines the location of the bottom side of the bank pop-up
        /// </summary>
        private void SetBottom(int screenHeight)
        {
            if (screenHeight < 503)
            {
                Bottom = Top + 499;
            }
            else
            {
                double yIntercept = 70.5;
                double slope = 0.5;
                Bottom = (int)Math.Round(yIntercept + (slope * screenHeight));
            }
        }

        /// <summary>
        /// Close the bank pop-up using the top-right X button
        /// </summary>
        public void Close()
        {
            int x = Left + 482;
            int y = Top + 17;
            Mouse.LeftClick(x, y, RSClient, 7);
        }

        /// <summary>
        /// Determines if the bank screen is currently visible
        /// </summary>
        /// <returns>true if the bank is open</returns>
        public bool NPCContactIsOpen()
        {
            int left = Left + 182;
            int right = Left + 316;
            int top = Top + 12;
            int bottom = Top + 25;

            Color[,] screen;
            screen = ScreenScraper.GetRGB(ScreenScraper.CaptureWindow(RSClient));
            screen = ImageProcessing.ScreenPiece(screen, left, right, top, bottom);
            double titleMatch = ImageProcessing.FractionalMatch(screen, RGBHSBRangeFactory.BankTitle());

            return titleMatch > 0.05;
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

                if (NPCContactIsOpen())
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Goes through the dialog with the dark mage to repair runecrafting pouches
        /// </summary>
        /// <returns>true if successful</returns>
        public bool RepairPouches(TextBoxTool textBox)
        {
            Mouse.LeftClick(Left + 240, Top + 102, RSClient, 10);
            BotProgram.SafeWait(NPC_CONTACT_CAST_TIME);
            return textBox.ClickThroughTextScreens(4, 5000);
        }
    }
}
