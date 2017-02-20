using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;

namespace WindowsFormsApplication1
{
    /// <summary>
    /// Responsible for interacting with the RS client
    /// </summary>
    public static class ScreenScraper
    {
        #region interops
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        /// <summary>
        /// Struct representing a point.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public static implicit operator Point(POINT point)
            {
                return new Point(point.X, point.Y);
            }
        }

        /// <summary>
        /// Retrieves the cursor's position, in screen coordinates.
        /// </summary>
        /// <see>See MSDN documentation for further information.</see>
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT point);

        //This is a replacement for Cursor.Position in WinForms
        [DllImport("user32.dll")]
        static extern bool SetCursorPosition(int x, int y);

        [DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
        #endregion

        #region constants
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;
        #endregion

        #region click handlers
        /// <summary>
        /// Execute a left mouse click and return the mouse to its original position
        /// </summary>
        /// <param name="x">pixels from left of client</param>
        /// <param name="y">pixels from top of client</param>
        public static void LeftMouseClick(int x, int y)
        {
            POINT originalCursorPos;

            GetCursorPos(out originalCursorPos);
            SetCursorPosition(x, y);
            mouse_event(MOUSEEVENTF_LEFTDOWN, x, y, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, x, y, 0, 0);
            SetCursorPosition(originalCursorPos.X, originalCursorPos.Y);    //return the cursor to its original position
        }

        /// <summary>
        /// Execute a right mouse click and return the mouse to its original position
        /// </summary>
        /// <param name="x">pixels from left of client</param>
        /// <param name="y">pixels from top of client</param>
        public static void RightMouseClick(int x, int y)
        {
            POINT originalCursorPos;

            GetCursorPos(out originalCursorPos);
            SetCursorPosition(x, y);
            mouse_event(MOUSEEVENTF_RIGHTDOWN, x, y, 0, 0);
            mouse_event(MOUSEEVENTF_RIGHTUP, x, y, 0, 0);
            SetCursorPosition(originalCursorPos.X, originalCursorPos.Y);    //return the cursor to its original position
        }
        #endregion

        /// <summary>
        /// Finds the OSBuddy client process to attach to
        /// </summary>
        /// <param name="startParams">specifies the username to select from multiple OSBuddy processes</param>
        /// <param name="loadError">used to output an error in finding a process</param>
        /// <returns></returns>
        public static Process GetOSBuddy(StartParams startParams, ref string loadError)
        {
            string windowName = "OSBuddy";
            string username = startParams.username;
            Process[] processlist = Process.GetProcesses();

            foreach (Process process in processlist)
            {
                if (!String.IsNullOrEmpty(process.MainWindowTitle) && process.MainWindowTitle.Contains(windowName))
                {
                    if (String.IsNullOrEmpty(username))
                    {
                        return process;
                    }
                    else    //verify that the username matches that specified by the user
                    {
                        if (process.MainWindowTitle.Contains(username))
                        {
                            return process;
                        }
                    }
                }
            }
            if (String.IsNullOrEmpty(username))
            {
                loadError = "No OSBuddy client found";
            }
            else
            {
                loadError = "No OSBuddy client found with username " + startParams.username;
            }
            
            return null;    //no suitable OSBuddy client found
        }
    }
}
