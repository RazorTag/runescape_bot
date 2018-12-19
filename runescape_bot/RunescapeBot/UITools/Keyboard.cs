using RunescapeBot.BotPrograms;
using RunescapeBot.ImageTools;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using static RunescapeBot.UITools.User32;

namespace RunescapeBot.UITools
{
    public class Keyboard
    {
        #region interops

        const int PauseBetweenStrokes = 50;
        [DllImport("user32.dll", SetLastError = true)]
        static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        const int KEY_DOWN_EVENT = 0x0001; //Key down flag
        const int KEY_UP_EVENT = 0x0002; //Key up flag

        #endregion

        #region properties

        private const int KEY_SPAM_INTERVAL = 1;
        private RSClient RSClient;

        #endregion

        /// <summary>
        /// Creates a keyboard controller
        /// </summary>
        /// <param name="rsClient"></param>
        public Keyboard(RSClient rsClient)
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
                ScreenScraper.BringToForeGround();
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
        /// Holds a key down for a set time.
        /// </summary>
        /// <param name="key">System.Windows.Forms.Keys to specify a key to hold down.</param>
        /// <param name="milliseconds">Number of milliseconds to hold down the key.</param>
        public void HoldKey(Keys key, int milliseconds)
        {
            keybd_event((byte)key, 0, KEY_DOWN_EVENT, 0);
            BotProgram.SafeWait(milliseconds);
            keybd_event((byte)key, 0, KEY_UP_EVENT, 0);
        }

        /// <summary>
        /// Holds the shift key down until released.
        /// </summary>
        public void ShiftDown()
        {
            keybd_event((byte)Keys.ShiftKey, 0, KEY_DOWN_EVENT, 0);
        }

        /// <summary>
        /// Releases the shift key.
        /// </summary>
        public void ShiftUp()
        {
            keybd_event((byte)Keys.ShiftKey, 0, KEY_UP_EVENT, 0);
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