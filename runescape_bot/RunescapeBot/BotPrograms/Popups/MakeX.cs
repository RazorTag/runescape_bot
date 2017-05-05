using RunescapeBot.Common;
using RunescapeBot.ImageTools;
using RunescapeBot.UITools;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;

namespace RunescapeBot.BotPrograms.Popups
{
    public class MakeX
    {
        private Process RSClient;
        private int XClick;
        private int YClick;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xClick">the x-coordinate of the click that opened the Make-X popup</param>
        /// <param name="yClick">the y-coordinate of the click that opened the Make-X popup</param>
        /// <param name="rsClient"></param>
        public MakeX(int xClick, int yClick, Process rsClient)
        {
            this.RSClient = rsClient;
            this.XClick = xClick;
            this.YClick = yClick;
        }

        /// <summary>
        /// Click the Make-1 option in a Make-X pop-up
        /// </summary>
        public void MakeOne()
        {
            const int yOffset = 25;

            Random rng = new Random();
            XClick += rng.Next(-50, 51);
            YClick += yOffset + rng.Next(-2, 3);
            Mouse.LeftClick(XClick, YClick, RSClient);
        }

        /// <summary>
        /// Click the Make-5 option in a Make-X pop-up
        /// </summary>
        public void MakeFive()
        {
            const int yOffset = 40;

            Random rng = new Random();
            XClick += rng.Next(-50, 51);
            YClick += yOffset + rng.Next(-2, 3);
            Mouse.LeftClick(XClick, YClick, RSClient);
        }

        /// <summary>
        /// Click the Make-10 option in a Make-X pop-up
        /// </summary>
        public void MakeTen()
        {
            const int yOffset = 55;

            Random rng = new Random();
            XClick += rng.Next(-50, 51);
            YClick += yOffset + rng.Next(-2, 3);
            Mouse.LeftClick(XClick, YClick, RSClient);
        }

        /// <summary>
        /// Use the Make-X option in a Make-X pop-up
        /// </summary>
        public void MakeXItems(int itemsToMake)
        {
            const int yOffset = 70;

            Random rng = new Random();
            XClick += rng.Next(-50, 51);
            YClick += yOffset + rng.Next(-2, 3);
            Mouse.LeftClick(XClick, YClick, RSClient);

            //Wait for the "Enter amount:" prompt to appear
            if (WaitForEnterAmount(5000))
            {
                Thread.Sleep(200);
                Keyboard keyboard = new Keyboard(RSClient);
                keyboard.WriteNumber(itemsToMake);
                Thread.Sleep(200);
                keyboard.Enter();
            }
        }

        /// <summary>
        /// Waits for the "Enter amount:" prompt to appear over the chat box
        /// </summary>
        /// <param name="timeout">Gives up after the max wait time has elapsed</param>
        /// <returns>true if the prompt appears</returns>
        public bool WaitForEnterAmount(int timeout)
        {
            Point screenSize = ScreenScraper.GetOSBuddyWindowSize(RSClient);
            const int asterisk = 91235;
            const int left = 252;
            const int right = 265;
            int top = screenSize.Y - 81;
            int bottom = screenSize.Y - 69;

            Color[,] screen;
            Stopwatch watch = new Stopwatch();
            watch.Start();
            long asteriskHash;

            while (watch.ElapsedMilliseconds < timeout)
            {
                screen = ScreenScraper.GetRGB(ScreenScraper.CaptureWindow(RSClient));
                screen = ImageProcessing.ScreenPiece(screen, left, right, top, bottom);
                asteriskHash = ImageProcessing.ColorSum(screen);
                if (Numerical.CloseEnough(asterisk, asteriskHash, 0.05))
                {
                    return true;
                }
            }

            return false;
        }
    }
}