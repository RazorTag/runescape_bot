using RunescapeBot.BotPrograms.Popups;
using RunescapeBot.Common;
using RunescapeBot.ImageTools;
using RunescapeBot.UITools;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;

namespace RunescapeBot.BotPrograms
{
    public static class BotUtilities
    {
        public const int WAIT_FOR_BANK_WINDOW_TIMEOUT = 8000;

        private const int WAIT_FOR_MAKEALL_POPUP_TIMEOUT = 1000;
        private const int CHATBOX_OPTION_RIGHT_CLICK_MAX_TRIES = 5;
        private const int CHATBOX_OPTION_RIGHT_CLICK_HOVER_DELAY = 500;

        /// <summary>
        /// Waits for the "Enter amount:" prompt to appear over the chat box
        /// </summary>
        /// <param name="timeout">Gives up after the max wait time has elapsed</param>
        /// <returns>true if the prompt appears</returns>
        public static bool WaitForEnterAmount(Process rsClient, int timeout)
        {
            Point screenSize = ScreenScraper.GetWindowSize(rsClient);
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
                if (BotProgram.StopFlag) { return false; }

                screen = ScreenScraper.GetRGB(ScreenScraper.CaptureWindow(rsClient));
                screen = ImageProcessing.ScreenPiece(screen, left, right, top, bottom);
                asteriskHash = ImageProcessing.ColorSum(screen);
                if (Numerical.CloseEnough(asterisk, asteriskHash, 0.01))
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
            Thread.Sleep(500);
            keyboard.Enter();
        }

    }
}
