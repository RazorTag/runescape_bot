using RunescapeBot.BotPrograms.Popups;
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

namespace RunescapeBot.BotPrograms
{
    public static class Utilities
    {
        private const int WAIT_FOR_MAKEALL_POPUP_TIMEOUT = 1000;
        private const int CHATBOX_OPTION_RIGHT_CLICK_MAX_TRIES = 3;
        private const int CHATBOX_OPTION_RIGHT_CLICK_HOVER_DELAY = 500;

        /// <summary>
        /// Waits for the "Enter amount:" prompt to appear over the chat box
        /// </summary>
        /// <param name="timeout">Gives up after the max wait time has elapsed</param>
        /// <returns>true if the prompt appears</returns>
        public static bool WaitForEnterAmount(Process rsClient, int timeout)
        {
            Point screenSize = ScreenScraper.GetOSBuddyWindowSize(rsClient);
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
                screen = ScreenScraper.GetRGB(ScreenScraper.CaptureWindow(rsClient));
                screen = ImageProcessing.ScreenPiece(screen, left, right, top, bottom);
                asteriskHash = ImageProcessing.ColorSum(screen);
                if (Numerical.CloseEnough(asterisk, asteriskHash, 0.05))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Enters a number into a chatbox Enter Amount: prompt
        /// </summary>
        /// <param name="rsClient"></param>
        /// <param name="amount"></param>
        public static void EnterAmount(Process rsClient, int amount)
        {
            Keyboard keyboard = new Keyboard(rsClient);
            keyboard.WriteNumber(amount);
            Thread.Sleep(200);
            keyboard.Enter();
        }


        public static bool ChatBoxSingleOptionMakeAll(Process rsClient)
        {
            Point screenSize = ScreenScraper.GetOSBuddyWindowSize(rsClient);
            int left = 230;
            int right = 282;
            int top = screenSize.Y - 110;
            int bottom = screenSize.Y - 70;
            Random rng = new Random();
            MakeAllSlim makeAllSlim = null;
            Point rightClick;

            //Try to right-click the chatbox option
            for (int i = 0; i < CHATBOX_OPTION_RIGHT_CLICK_MAX_TRIES; i++)
            {
                Thread.Sleep(CHATBOX_OPTION_RIGHT_CLICK_HOVER_DELAY);
                rightClick = new Point(rng.Next(left, right), rng.Next(top, bottom));
                Mouse.RightClick(rightClick.X, rightClick.Y, rsClient, CHATBOX_OPTION_RIGHT_CLICK_HOVER_DELAY);
                makeAllSlim = new MakeAllSlim(rightClick.X, rightClick.Y, rsClient);

                if (makeAllSlim.WaitForPopup(WAIT_FOR_MAKEALL_POPUP_TIMEOUT))
                {
                    //Select Make All
                    makeAllSlim.MakeAllItems();
                    return true;
                }
                else
                {
                    Mouse.Offset(0, -300, 100);
                }
            }

            return false;
        }
    }
}
