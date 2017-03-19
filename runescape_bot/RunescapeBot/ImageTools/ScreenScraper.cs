using RunescapeBot.UITools;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
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
        public const int LOGIN_WINDOW_HEIGHT = 503;    //y-cordinate of a pixe directly below the login window
        #endregion

        #region screenreader
        /// <summary>
        /// Brings the client window to the foreground and shows it
        /// </summary>
        /// <param name="rsHandle"></param>
        public static void BringToForeGround(Process rsClient)
        {
            int rsHandle = (int)rsClient.MainWindowHandle;
            if (User32.IsIconic(rsHandle))
            {
                User32.ShowWindow(rsHandle, SW_RESTORE);
            }
            User32.SetForegroundWindow(rsHandle);
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
                User32.ShowWindow((int)rsClient.MainWindowHandle, SW_MAXIMIZE);
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
            User32.GetWindowRect(rsClient.MainWindowHandle, ref windowRect);
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
        /// Makes an RGB 
        /// </summary>
        /// <param name="bitmap"></param>
        public static Color[,] GetRGB(this Bitmap bitmap)
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

        /// <summary>
        /// Saves a Bitmap object to file as an image
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="fileName"></param>
        /// <param name="format"></param>
        public static void SaveImageToFile(Bitmap bitmap, string fileName, ImageFormat format)
        {
            IntPtr hBitmap = bitmap.GetHbitmap();
            Image img = Image.FromHbitmap(hBitmap);
            img.Save(fileName, format);
        }

        /// <summary>
        /// Saves an RGB array to file as an image
        /// </summary>
        /// <param name="rgbArray"></param>
        /// <param name="fileName"></param>
        /// <param name="format"></param>
        public static void SaveImageToFile(Color[,] rgbArray, string fileName, ImageFormat format)
        {
            Bitmap bitmap = new Bitmap(rgbArray.GetLength(0), rgbArray.GetLength(1));
            for(int x = 0; x < rgbArray.GetLength(0); x++)
            {
                for(int y = 0; y < rgbArray.GetLength(1); y++)
                {
                    bitmap.SetPixel(x, y, rgbArray[x, y]);
                }
            }
            SaveImageToFile(bitmap, fileName, format);
        }
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

        /// <summary>
        /// Gets the width and height of a window
        /// </summary>
        /// <param name="OSBuddy"></param>
        /// <returns>Point(width, height)</returns>
        public static Point GetOSBuddyWindowSize(Process OSBuddy)
        {
            RECT windowRect = new RECT();
            User32.GetWindowRect(OSBuddy.MainWindowHandle, ref windowRect);
            ScreenScraper.TrimOSBuddy(ref windowRect);
            return new Point(windowRect.right - windowRect.left, windowRect.bottom - windowRect.top);
        }
        #endregion
    }
}
