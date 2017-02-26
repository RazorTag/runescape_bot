using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    /// <summary>
    /// Responsible for interacting with the RS client
    /// </summary>
    public static class ScreenScraper
    {
        private static int OSBUDDY_TOOLBAR_WIDTH = 31;  //does not include the border underneath the toolbar
        private static int OSBUDDY_BORDER_WIDTH = 2;
        public const int SW_RESTORE = 9;

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
        public static void LeftMouseClick(int x, int y, Process rsClient)
        {
            POINT originalCursorPos;

            User32.SetForegroundWindow(rsClient.MainWindowHandle.ToInt32());
            TranslateClick(ref x, ref y, rsClient);
            User32.GetCursorPos(out originalCursorPos);
            User32.SetCursorPos(x, y);
            User32.mouse_event(MOUSEEVENTF_LEFTDOWN, x, y, 0, 0);
            User32.mouse_event(MOUSEEVENTF_LEFTUP, x, y, 0, 0);
            User32.SetCursorPos(originalCursorPos.X, originalCursorPos.Y);    //return the cursor to its original position
        }

        /// <summary>
        /// Execute a right mouse click and return the mouse to its original position
        /// </summary>
        /// <param name="x">pixels from left of client</param>
        /// <param name="y">pixels from top of client</param>
        public static void RightMouseClick(int x, int y, Process rsClient)
        {
            POINT originalCursorPos;

            User32.SetForegroundWindow(rsClient.MainWindowHandle.ToInt32());
            TranslateClick(ref x, ref y, rsClient);
            User32.GetCursorPos(out originalCursorPos);
            User32.SetCursorPos(x, y);
            User32.mouse_event(MOUSEEVENTF_RIGHTDOWN, x, y, 0, 0);
            User32.mouse_event(MOUSEEVENTF_RIGHTUP, x, y, 0, 0);
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
            x -= OSBUDDY_BORDER_WIDTH;
            y -= OSBUDDY_TOOLBAR_WIDTH + OSBUDDY_BORDER_WIDTH;
        }
        #endregion

        #region screenreader
        /// <summary>
        /// Brings the client window to th fireground and shows it
        /// </summary>
        /// <param name="rsHandle"></param>
        private static void BringToForeGround(int rsHandle)
        {
            if (User32.IsIconic(rsHandle))
            {
                User32.ShowWindow(rsHandle, SW_RESTORE);
            }
            User32.SetForegroundWindow(rsHandle);
        }

        /// <summary>
        /// Creates an Image object containing a screen shot of a specific window
        /// </summary>
        /// <param name="handle">The handle to the window. (In windows forms, this is obtained by the Handle property)</param>
        /// <returns></returns>
        public static Bitmap CaptureWindow(Process rsClient)
        {
            int rsHandle = rsClient.MainWindowHandle.ToInt32();
            

            IntPtr handle = rsClient.MainWindowHandle;

            // get the hDC of the target window
            IntPtr hdcSrc = User32.GetWindowDC(handle);
            // get the size
            RECT windowRect = new RECT();
            User32.GetWindowRect(handle, ref windowRect);
            if (!TrimOSBuddy(ref windowRect))
            {
                return null;
            }

            int width = windowRect.right - windowRect.left;
            int height = windowRect.bottom - windowRect.top;
            // create a device context we can copy to
            IntPtr hdcDest = GDI32.CreateCompatibleDC(hdcSrc);
            // create a bitmap we can copy it to,
            // using GetDeviceCaps to get the width/height
            IntPtr hBitmap = GDI32.CreateCompatibleBitmap(hdcSrc, width, height);
            // select the bitmap object
            IntPtr hOld = GDI32.SelectObject(hdcDest, hBitmap);
            // bitblt over
            GDI32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, OSBUDDY_BORDER_WIDTH, OSBUDDY_TOOLBAR_WIDTH + OSBUDDY_BORDER_WIDTH, GDI32.SRCCOPY);
            // restore selection
            GDI32.SelectObject(hdcDest, hOld);
            // clean up 
            GDI32.DeleteDC(hdcDest);
            User32.ReleaseDC(handle, hdcSrc);

            return Image.FromHbitmap(hBitmap);
        }

        /// <summary>
        /// Saves a Bitmap object to file as an image
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="fileName"></param>
        /// <param name="format"></param>
        public static void WriteBitmapToFile(Bitmap bitmap, string fileName, ImageFormat format)
        {
            IntPtr hBitmap = bitmap.GetHbitmap();
            Image img = Image.FromHbitmap(hBitmap);
            img.Save(fileName, format);
        }

        /// <summary>
        /// Captures a screen shot of a specific window, and saves it to a file
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="filename"></param>
        /// <param name="format"></param>
        //public static void CaptureWindowToFile(Process rsClient, string filename, ImageFormat format)
        //{
        //    // get a .NET image object for it
        //    IntPtr bitmap = new IntPtr();

        //    try
        //    {
        //        bitmap = CaptureWindow(rsClient);
        //    }
        //    catch (HiddenWindowException e)
        //    {
        //        return;
        //    }
            
        //    Image img = Image.FromHbitmap(bitmap);
        //    img.Save(filename, format);
        //    // free up the Bitmap object
        //    GDI32.DeleteObject(bitmap);
        //}
        #endregion

        #region OSBuddy
        /// <summary>
        /// Finds the OSBuddy client process to attach to
        /// </summary>
        /// <param name="startParams">specifies the username to select from multiple OSBuddy processes</param>
        /// <param name="loadError">used to output an error in finding a process</param>
        /// <returns></returns>
        public static Process GetOSBuddy(StartParams startParams, out string loadError)
        {
            loadError = "";
            string windowName = "OSBUDDY";
            string username = startParams.username.ToUpper();
            string mainWindowTitle;
            Process[] processlist = Process.GetProcesses();

            foreach (Process process in processlist)
            {
                mainWindowTitle = process.MainWindowTitle.ToUpper();
                if (mainWindowTitle.Contains(windowName) && mainWindowTitle.Contains(username))
                {
                    return process;
                }
            }
            
            loadError = "No OSBuddy client found";
            if (!String.IsNullOrEmpty(username))
            {
                loadError += " with username " + startParams.username;
            }
            
            return null;    //no suitable OSBuddy client found
        }

        /// <summary>
        /// Trims out the borders of the OSBuddy client from the target rectangle
        /// </summary>
        /// <param name="windowRect"></param>
        private static bool TrimOSBuddy(ref RECT windowRect)
        {
            windowRect.top += OSBUDDY_TOOLBAR_WIDTH + OSBUDDY_BORDER_WIDTH;
            windowRect.right -= OSBUDDY_BORDER_WIDTH;
            windowRect.bottom -= OSBUDDY_BORDER_WIDTH;
            windowRect.left += OSBUDDY_BORDER_WIDTH;

            return (windowRect.top > 0) || (windowRect.right > 0) || (windowRect.bottom > 0) || (windowRect.left > 0);
        }
        #endregion

        #region private inner classes
        /// <summary>
        /// Helper class containing Gdi32 API functions
        /// </summary>
        private class GDI32
        {
            public const int SRCCOPY = 0x00CC0020; // BitBlt dwRop parameter

            [DllImport("gdi32.dll")]
            public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest,
                int nWidth, int nHeight, IntPtr hObjectSource,
                int nXSrc, int nYSrc, int dwRop);
            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth,
                int nHeight);
            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
            [DllImport("gdi32.dll")]
            public static extern bool DeleteDC(IntPtr hDC);
            [DllImport("gdi32.dll")]
            public static extern bool DeleteObject(IntPtr hObject);
            [DllImport("gdi32.dll")]
            public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
        }

        /// <summary>
        /// Helper class containing User32 API functions
        /// </summary>
        private class User32
        {
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
            [DllImport("user32.dll")]
            public static extern bool IsIconic(int handle);
            [DllImport("user32.dll")]
            public static extern bool ShowWindow(int handle, int nCmdShow);
        }
        #endregion
    }
}
