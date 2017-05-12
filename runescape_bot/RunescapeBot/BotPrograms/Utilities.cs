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
    class Utilities
    {
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


        public static void EnterAmount(Process rsClient, int amount)
        {
            Keyboard keyboard = new Keyboard(rsClient);
            keyboard.WriteNumber(amount);
            Thread.Sleep(200);
            keyboard.Enter();
        }
    }
}
