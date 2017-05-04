using RunescapeBot.BotPrograms;
using RunescapeBot.FileIO;
using RunescapeBot.UITools;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using static RunescapeBot.UITools.User32;

namespace RunescapeBot.ImageTools
{
    /// <summary>
    /// Responsible for interacting with the RS client
    /// </summary>
    public static class ScreenScraper
    {
        #region constants
        public static int OSBUDDY_TOOLBAR_WIDTH = 31;  //does not include the border underneath the toolbar
        public static int OSBUDDY_BORDER_WIDTH = 2;
        public const int LOGIN_WINDOW_HEIGHT = 503;
        public const int LOGIN_WINDOW_WIDTH = 765;
        #endregion

        #region screenreader
        /// <summary>
        /// Brings the client window to the foreground and shows it
        /// </summary>
        /// <param name="rsHandle"></param>
        public static void BringToForeGround(Process rsClient)
        {
            int rsHandle = (int)rsClient.MainWindowHandle;
            if (IsIconic(rsHandle))
            {
                ShowWindow(rsHandle, SW_RESTORE);
            }
            SetForegroundWindow(rsHandle);
            MaximizeWindow(rsClient);
        }

        /// <summary>
        /// Maximizes a window if it isn;t already
        /// </summary>
        /// <param name="rsClient"></param>
        /// <returns></returns>
        private static void MaximizeWindow(Process rsClient)
        {
            int style = GetWindowLong(rsClient.MainWindowHandle, GWL_STYLE);
            if ((style & WS_MAXIMIZE) != WS_MAXIMIZE)
            {
                ShowWindow((int)rsClient.MainWindowHandle, SW_MAXIMIZE);
                Thread.Sleep(1000);  //wait for the window to maximize
            }
        }

        /// <summary>
        /// Creates an Image object containing a screen shot of a specific window
        /// </summary>
        /// <param name="handle">The handle to the window. (In windows forms, this is obtained by the Handle property)</param>
        /// <returns></returns>
        public static Bitmap CaptureWindow(Process rsClient)
        {
            BringToForeGround(rsClient);

            RECT windowRect = new RECT();
            GetWindowRect(rsClient.MainWindowHandle, ref windowRect);
            if (!TrimOSBuddy(ref windowRect))
            {
                return null;
            }

            int width = windowRect.right - windowRect.left;
            int height = windowRect.bottom - windowRect.top;
            Bitmap screenShot = new Bitmap(width, height, PixelFormat.Format32bppRgb);
            Graphics gScreen = Graphics.FromImage(screenShot);
            gScreen.CopyFromScreen(windowRect.left, windowRect.top, 0, 0, new Size(width, height), CopyPixelOperation.SourceCopy);

            return screenShot;
        }

        /// <summary>
        /// Makes an RGB array from a Bitmap
        /// </summary>
        /// <param name="bitmap"></param>
        public static Color[,] GetRGB(Bitmap bitmap)
        {
            const int PixelWidth = 3;
            const PixelFormat PixelFormat = PixelFormat.Format24bppRgb;
            int width, height, pixelOffset;
            Color[,] rgbArray;
            
            if (bitmap == null) throw new ArgumentNullException("image");

            width = bitmap.Width;
            height = bitmap.Height;
            rgbArray = new Color[width, height];

            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat);
            try
            {
                byte[] pixelData = new Byte[data.Stride];
                for (int y = 0; y < height; y++)
                {
                    Marshal.Copy(data.Scan0 + (y * data.Stride), pixelData, 0, data.Stride);
                    for (int x = 0; x < width; x++)
                    {
                        // PixelFormat.Format32bppRgb means the data is stored
                        // in memory as BGR. We want RGB, so we must do some 
                        // bit-shuffling.
                        pixelOffset = x * PixelWidth;
                        rgbArray[x, y] = Color.FromArgb(
                            pixelData[pixelOffset + 2], // R 
                            pixelData[pixelOffset + 1], // G
                            pixelData[pixelOffset]      // B
                        );
                    }
                }
            }
            finally
            {
                bitmap.UnlockBits(data);
            }
            return rgbArray;
        }
        #endregion

        #region OSBuddy
        /// <summary>
        /// Finds the OSBuddy client process to attach to
        /// </summary>
        /// <param name="loadError">used to output an error in finding a process</param>
        /// <returns></returns>
        public static Process GetOSBuddy(out string loadError)
        {
            loadError = "";
            string windowName = "OSBUDDY";
            string mainWindowTitle;
            Process[] processlist = Process.GetProcesses();

            foreach (Process process in processlist)
            {
                mainWindowTitle = process.MainWindowTitle.ToUpper();
                if (mainWindowTitle.Contains(windowName))
                {
                    return process;
                }
            }
            
            loadError = "No OSBuddy client found";
            return null;    //no OSBuddy client found
        }

        /// <summary>
        /// Determines if a process is currently running
        /// </summary>
        /// <param name="processToFind"></param>
        /// <returns>true if the process is found, false otherwise</returns>
        public static bool ProcessExists(Process processToFind)
        {
            IntPtr mainWindowHandle = processToFind.MainWindowHandle;
            Process[] processlist = Process.GetProcesses();

            foreach (Process process in processlist)
            {
                if (mainWindowHandle == process.MainWindowHandle)
                {
                    return true;
                }
            }
            return false;
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

            //Ignore any portion of the game screen that is not visible
            windowRect.top = Math.Max(0, windowRect.top);
            windowRect.right = Math.Max(0, windowRect.right);
            windowRect.bottom = Math.Max(0, windowRect.bottom);
            windowRect.left = Math.Max(0, windowRect.left);

            return ((windowRect.right > windowRect.left) && (windowRect.bottom > windowRect.top));
        }

        /// <summary>
        /// Gets the width and height of a window
        /// </summary>
        /// <param name="OSBuddy"></param>
        /// <returns>Point(width, height)</returns>
        public static Point GetOSBuddyWindowSize(Process OSBuddy)
        {
            RECT windowRect = new RECT();
            GetWindowRect(OSBuddy.MainWindowHandle, ref windowRect);
            TrimOSBuddy(ref windowRect);
            return new Point(windowRect.right - windowRect.left, windowRect.bottom - windowRect.top);
        }

        /// <summary>
        /// Closes an instance of OSBuddy ad opens a new one
        /// </summary>
        /// <param name="OSBuddy"></param>
        /// <returns></returns>
        public static bool RestartOSBuddy(string clientFilePath, ref Process OSBuddy)
        {
            if ((OSBuddy != null) && !OSBuddy.CloseMainWindow())
            {
                return false;   //unable to close the current instance of OSBuddy
            }
            if (!StartOSBuddy(clientFilePath))
            {
                return false;   //unable to start a new instance of OSBuddy
            }
            return true;
        }

        /// <summary>
        /// Starts a new instance of the OSBuddy client
        /// </summary>
        /// <returns></returns>
        public static bool StartOSBuddy(string clientFilePath)
        {
            string error;
            Process OSBuddy = GetOSBuddy(out error);
            if (!(OSBuddy == null))
            {
                return true;    //OSBuddy is already running
            }
            
            //start OSBuddy
            try
            {
                OSBuddy = new Process();
                OSBuddy.StartInfo.FileName = clientFilePath;
                return OSBuddy.Start();
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}
