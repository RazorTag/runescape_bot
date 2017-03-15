using RunescapeBot.ImageTools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static RunescapeBot.UITools.User32;

namespace RunescapeBot.UITools
{
    public static class MouseActions
    {
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        #region click handlers
        /// <summary>
        /// Execute a left mouse click and return the mouse to its original position
        /// </summary>
        /// <param name="x">pixels from left of client</param>
        /// <param name="y">pixels from top of client</param>
        public static void LeftMouseClick(int x, int y, Process rsClient, bool preserveMousePosition)
        {
            MouseClick(x, y, rsClient, MOUSEEVENTF_LEFTDOWN, MOUSEEVENTF_LEFTUP, preserveMousePosition);
        }

        /// <summary>
        /// Execute a right mouse click and return the mouse to its original position
        /// </summary>
        /// <param name="x">pixels from left of client</param>
        /// <param name="y">pixels from top of client</param>
        public static void RightMouseClick(int x, int y, Process rsClient, bool preserveMousePosition)
        {
            MouseClick(x, y, rsClient, MOUSEEVENTF_RIGHTDOWN, MOUSEEVENTF_RIGHTUP, preserveMousePosition);
        }

        /// <summary>
        /// Click down and release a mouse button
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="rsClient"></param>
        /// <param name="clickTypeDown"></param>
        /// <param name="clickTypeUp"></param>
        private static void MouseClick(int x, int y, Process rsClient, int clickTypeDown, int clickTypeUp, bool preserveMousePosition)
        {
            POINT originalCursorPos;
            int hWnd = rsClient.MainWindowHandle.ToInt32();
            ScreenScraper.BringToForeGround(hWnd);

            TranslateClick(ref x, ref y, rsClient);
            ScreenScraper.BringToForeGround(hWnd);
            User32.GetCursorPos(out originalCursorPos);
            User32.SetCursorPos(x, y);
            Thread.Sleep(100);  //wait for RS client to recognize that the cursor is hovering over the demon
            User32.mouse_event(clickTypeDown, x, y, 0, 0);
            User32.mouse_event(clickTypeUp, x, y, 0, 0);
            User32.SetCursorPos(originalCursorPos.X, originalCursorPos.Y);    //return the cursor to its original position
        }

        /// <summary>
        /// Translate a click location from a position within the diplay portion of OSBuddy to a position on the screen.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private static void TranslateClick(ref int x, ref int y, Process rsClient)
        {
            //adjust for the position of the OSBuddy window
            RECT windowRect = new RECT();
            User32.GetWindowRect(rsClient.MainWindowHandle, ref windowRect);
            x += windowRect.left;
            y += windowRect.top;

            //adjust for the borders and toolbar
            x += ScreenScraper.OSBUDDY_BORDER_WIDTH;
            y += ScreenScraper.OSBUDDY_TOOLBAR_WIDTH + ScreenScraper.OSBUDDY_BORDER_WIDTH;
        }
        #endregion
    }
}
