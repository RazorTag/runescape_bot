using RunescapeBot.BotPrograms;
using RunescapeBot.Common;
using RunescapeBot.ImageTools;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace RunescapeBot.UITools
{
    public class Keyboard
    {
        private const int KEY_SPAM_INTERVAL = 1;
        private Process RSClient;

        /// <summary>
        /// Creates a keyboard controller
        /// </summary>
        /// <param name="rsClient"></param>
        public Keyboard(Process rsClient)
        {
            RSClient = rsClient;
        }

        /// <summary>
        /// Sets the client for he keyboard to use
        /// </summary>
        /// <param name="rsClient"></param>
        public void SetClient(Process rsClient)
        {
            RSClient = rsClient;
        }

        /// <summary>
        /// Verifies that the client exists and that it is visible and maximized
        /// </summary>
        /// <returns></returns>
        private bool PrepareClientForInput()
        {
            if (BotProgram.StopFlag)
            {
                return false;
            }
            if (ScreenScraper.ProcessExists(RSClient))
            {
                ScreenScraper.BringToForeGround(RSClient);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Types a message in the active window
        /// </summary>
        /// <param name="line"></param>
        public void WriteLine(string line)
        {
            if (PrepareClientForInput())
            {
                SendKeys.SendWait(line);
            }
        }

        /// <summary>
        /// Types a number in the active window
        /// </summary>
        /// <param name="number">The number to type</param>
        public void WriteNumber(int number)
        {
            if (PrepareClientForInput())
            {
                string line = number.ToString();
                SendKeys.SendWait(line);
            }
        }

        /// <summary>
        /// Spams the backspace key
        /// </summary>
        /// <param name="maxCharsToDelete">the number of times to spam the backspace key</param>
        public void Backspace(int maxCharsToDelete)
        {
            if (PrepareClientForInput())
            {
                for (int i = 0; i < maxCharsToDelete; i++)
                {
                    if (BotProgram.StopFlag) { return; }
                    SendKeys.SendWait("{BACKSPACE}");
                    Thread.Sleep(KEY_SPAM_INTERVAL);
                }
            }
        }

        /// <summary>
        /// Presses the tab key
        /// </summary>
        public void Tab()
        {
            if (PrepareClientForInput())
            {
                SendKeys.SendWait("{TAB}");
            }
        }

        /// <summary>
        /// Presses the enter key
        /// </summary>
        public void Enter()
        {
            if (PrepareClientForInput())
            {
                SendKeys.SendWait("{ENTER}");
            }
        }

        /// <summary>
        /// Presses the up arrow
        /// </summary>
        public void UpArrow(int iterations)
        {
            if (PrepareClientForInput())
            {
                for (int i = 0; i < iterations; i++)
                {
                    if (BotProgram.StopFlag) { return; }
                    SendKeys.SendWait("{UP}");
                    Thread.Sleep(KEY_SPAM_INTERVAL);
                }
            }
        }

        /// <summary>
        /// Presses the right arrow
        /// </summary>
        public void RightArrow(int iterations)
        {
            if (PrepareClientForInput())
            {
                for (int i = 0; i < iterations; i++)
                {
                    if (BotProgram.StopFlag) { return; }
                    SendKeys.SendWait("{RIGHT}");
                    Thread.Sleep(KEY_SPAM_INTERVAL);
                }
            }
        }

        /// <summary>
        /// Presses the down arrow
        /// </summary>
        public void DownArrow(int iterations)
        {
            if (PrepareClientForInput())
            {
                for (int i = 0; i < iterations; i++)
                {
                    if (BotProgram.StopFlag) { return; }
                    SendKeys.SendWait("{DOWN}");
                    Thread.Sleep(KEY_SPAM_INTERVAL);
                }
            }
        }

        /// <summary>
        /// Presses the left arrow
        /// </summary>
        public void LeftArrow(int iterations)
        {
            if (PrepareClientForInput())
            {
                for (int i = 0; i < iterations; i++)
                {
                    if (BotProgram.StopFlag) { return; }
                    SendKeys.SendWait("{LEFT}");
                    Thread.Sleep(KEY_SPAM_INTERVAL);
                }
            }
        }

        /// <summary>
        /// Sends the specified F-key to the client
        /// </summary>
        /// <param name="fKey">number of F-key to send. 6 send F6, etc.</param>
        public void FKey(int fKey)
        {
            const int minF = 1;
            const int maxF = 16;
            if (fKey < minF || fKey > maxF) { throw new System.Exception("F" + fKey + " is not in the valid range of " + minF + "-" + maxF); }

            if (PrepareClientForInput())
            {
                string fKeyCode = "{F" + fKey + "}";
                SendKeys.SendWait(fKeyCode);
            }
        }

        /// <summary>
        /// Presses the escape key
        /// </summary>
        public void Space()
        {
            if (PrepareClientForInput())
            {
                SendKeys.SendWait(" ");
            }
        }
    }
}