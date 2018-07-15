using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace RunescapeBot.UITools
{
    public static class User32
    {
        #region constants

        public const int GWL_STYLE = -16;
        public const int ESCAPE_HOTKEY_ID = 1;
        public const int SW_MAXIMIZE = 3;
        public const int SW_MINIMIZE = 6;
        public const int SW_RESTORE = 9;
        public const UInt32 WS_MAXIMIZE = 0x1000000;

        const int KEYEVENTF_KEYUP = 0x0002;
        const int INPUT_KEYBOARD = 1;

        #endregion

        #region interops

        // DLL libraries used to manage hotkeys
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);
        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        /// <summary>
        /// Retrieves the cursor's position, in screen coordinates.
        /// </summary>
        /// <see>See MSDN documentation for further information.</see>
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT point);

        /// <summary>
        /// Sets the cursor's position, in screen coordinates.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
        [DllImport("User32.dll")]
        public static extern bool SetForegroundWindow(int hWnd);
        [DllImport("User32.dll")]
        public static extern int GetTopWindow(int hWnd);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        public static extern bool IsIconic(int handle);
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(int handle, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern IntPtr SetCapture(int hWnd);

        [DllImport("user32.dll")]
        public static extern long ReleaseCapture();

        #endregion

        #region custom structs

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

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        #endregion
    }
}
