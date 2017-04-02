using RunescapeBot.ImageTools;
using System.Diagnostics;
using System.Windows.Forms;

namespace RunescapeBot.UITools
{
    public class Keyboard
    {
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
        /// Types a message in the active window
        /// </summary>
        /// <param name="line"></param>
        public void WriteLine(string line)
        {
            SendKeys.SendWait(line);
        }

        /// <summary>
        /// Spams the backspace key
        /// </summary>
        /// <param name="maxCharsToDelete">the number of times to spam the backspace key</param>
        public void Backspace(int maxCharsToDelete)
        {
            for (int i = 0; i < maxCharsToDelete; i++)
            {
                SendKeys.SendWait("{BACKSPACE}");
            }
        }

        /// <summary>
        /// Presses the tab key
        /// </summary>
        public void Tab()
        {
            SendKeys.SendWait("{TAB}");
        }

        /// <summary>
        /// Presses the enter key
        /// </summary>
        public void Enter()
        {
            SendKeys.SendWait("{ENTER}");
        }

        /// <summary>
        /// Presses the up arrow
        /// </summary>
        public void UpArrow(int iterations)
        {
            ScreenScraper.BringToForeGround(RSClient);
            for (int i = 0; i < iterations; i++)
            {
                SendKeys.SendWait("{UP}");
                //Thread.Sleep(100);
            }
        }

        /// <summary>
        /// Presses the right arrow
        /// </summary>
        public void RightArrow()
        {
            SendKeys.SendWait("{RIGHT}");
        }

        /// <summary>
        /// Presses the down arrow
        /// </summary>
        public void DownArrow()
        {
            SendKeys.SendWait("{DOWN}");
        }

        /// <summary>
        /// Presses the left arrow
        /// </summary>
        public void LeftArrow()
        {
            SendKeys.SendWait("{LEFT}");
        }
    }
}