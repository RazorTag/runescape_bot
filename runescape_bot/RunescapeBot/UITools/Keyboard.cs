using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RunescapeBot.UITools
{
    class Keyboard
    {
        /// <summary>
        /// Types a message in the active window
        /// </summary>
        /// <param name="line"></param>
        public static void WriteLine(string line)
        {
            SendKeys.SendWait(line);
        }

        /// <summary>
        /// Spams the backspace key
        /// </summary>
        /// <param name="maxCharsToDelete">the number of times to spam the backspace key</param>
        public static void Backspace(int maxCharsToDelete)
        {
            for (int i = 0; i < maxCharsToDelete; i++)
            {
                SendKeys.SendWait("{BACKSPACE}");
            }
        }

        /// <summary>
        /// Presses the tab key
        /// </summary>
        public static void Tab()
        {
            SendKeys.SendWait("{TAB}");
        }

        /// <summary>
        /// Presses the enter key
        /// </summary>
        public static void Enter()
        {
            SendKeys.SendWait("{ENTER}");
        }
    }
}