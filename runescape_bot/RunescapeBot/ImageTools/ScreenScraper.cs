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
        public const int SW_RESTORE = 9;
        #endregion

        #region screenreader
        /// <summary>
        /// Brings the client window to the foreground and shows it
        /// </summary>
        /// <param name="rsHandle"></param>
        public static void BringToForeGround(int rsHandle)
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
            BringToForeGround(rsClient.MainWindowHandle.ToInt32());

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
    }
}
