using RunescapeBot.BotPrograms;
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
        public const int OSBUDDY_TOOLBAR_WIDTH = 33;
        public const int OSBUDDY_BORDER_WIDTH = 3;
        public const int JAGEX_TOOLBAR_WIDTH = 23;
        public const int JAGEX_BORDER_WIDTH = 0;
        public const int LOGIN_WINDOW_HEIGHT = 503;
        public const int LOGIN_WINDOW_WIDTH = 765;
        #endregion

        /// <summary>
        /// The client to look for 
        /// </summary>
        public enum Client
        {
            None = 0,
            Jagex,
            OSBuddy
        }

        /// <summary>
        /// THe client type most recently found
        /// </summary>
        public static Client ClientType { get; set; }

        #region screenreader
        /// <summary>
        /// Brings the client window to the foreground and shows it
        /// </summary>
        /// <param name="rsHandle"></param>
        public static void BringToForeGround(Process rsClient)
        {
            if (!ProcessExists(rsClient)) { return; }
            
            int rsHandle = (int)rsClient.MainWindowHandle;
            if (IsIconic(rsHandle))
            {
                ShowWindow(rsHandle, SW_RESTORE);
            }
            SetForegroundWindow(rsHandle);
            MaximizeWindow(rsClient);
        }

        /// <summary>
        /// Maximizes a window if it isn't already
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
        /// Gets the game screen size of the client
        /// </summary>
        /// <param name="rsClient"></param>
        /// <returns></returns>
        public static Point? GetScreenSize(Process rsClient)
        {
            if (!ProcessExists(rsClient)) { return null; }

            BringToForeGround(rsClient);
            RECT windowRect = new RECT();
            GetWindowRect(rsClient.MainWindowHandle, ref windowRect);
            if (!TrimClient(ref windowRect))
            {
                return null;
            }
            else
            {
                return new Point(windowRect.right - windowRect.left, windowRect.bottom - windowRect.top);
            }
        }

        /// <summary>
        /// Creates an Image object containing a screen shot of a specific window
        /// </summary>
        /// <param name="handle">The handle to the window. (In windows forms, this is obtained by the Handle property)</param>
        /// <returns></returns>
        public static Bitmap CaptureWindow(Process rsClient)
        {
            if (!ProcessExists(rsClient)) { return null; }

            BringToForeGround(rsClient);
            RECT windowRect = new RECT();
            GetWindowRect(rsClient.MainWindowHandle, ref windowRect);
            if (!TrimClient(ref windowRect))
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
            if (bitmap == null) { return new Color[0, 0]; }

            const int PixelWidth = 3;
            const PixelFormat PixelFormat = PixelFormat.Format24bppRgb;
            int width, height, pixelOffset;
            Color[,] rgbArray;

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

        #region attach to process

        /// <summary>
        /// Determines if a process is currently running
        /// </summary>
        /// <param name="processToFind"></param>
        /// <returns>true if the process is found, false otherwise</returns>
        public static bool ProcessExists(Process processToFind, Client clientType = Client.None)
        {
            if (processToFind == null) { return false; }

            try
            {
                Process process = Process.GetProcessById(processToFind.Id);
                if (process == null || process.HasExited) { return false; }

                switch (clientType)
                {
                    case Client.Jagex:
                        return IsJagex(process);
                    case Client.OSBuddy:
                        return IsOSBuddy(process);
                    default:
                        return true;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Trims out the borders of the client from the target rectangle
        /// </summary>
        /// <param name="windowRect"></param>
        private static bool TrimClient(ref RECT windowRect)
        {
            //correct for ghost padding issue seen with the Jagex client
            if (windowRect.top < 0)
            {
                int ghostPadding = -windowRect.top;
                windowRect.top += ghostPadding;
                windowRect.right -= ghostPadding;
                windowRect.bottom -= ghostPadding;
                windowRect.left += ghostPadding;
            }

            switch (ClientType)
            {
                case Client.Jagex:
                    windowRect.top += JAGEX_TOOLBAR_WIDTH;
                    windowRect.right -= JAGEX_BORDER_WIDTH;
                    windowRect.bottom -= JAGEX_BORDER_WIDTH;
                    windowRect.left += JAGEX_BORDER_WIDTH;
                    break;
                case Client.OSBuddy:
                    windowRect.top += OSBUDDY_TOOLBAR_WIDTH;
                    windowRect.right -= OSBUDDY_BORDER_WIDTH;
                    windowRect.bottom -= OSBUDDY_BORDER_WIDTH;
                    windowRect.left += OSBUDDY_BORDER_WIDTH;
                    break;
            }

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
        /// <param name="client"></param>
        /// <returns>Point(width, height)</returns>
        public static Point GetWindowSize(Process client)
        {
            if (!ProcessExists(client)) { return new Point(); }

            RECT windowRect = new RECT();
            GetWindowRect(client.MainWindowHandle, ref windowRect);
            TrimClient(ref windowRect);
            return new Point(windowRect.right - windowRect.left, windowRect.bottom - windowRect.top);
        }

        /// <summary>
        /// Closes an instance of a RuneScape client and opens a new one
        /// </summary>
        /// <param name="client"></param>
        /// <returns>true if successful</returns>
        public static bool RestartClient(ref Process client, string clientFilePath, string arguments)
        {
            return CloseClient(ref client) && StartClient(ref client, clientFilePath, arguments);
        }

        /// <summary>
        /// Attempts to close an RuneScape client window if one exists
        /// </summary>
        /// <param name="client">RuneScape client main window</param>
        /// <returns>true if successful</returns>
        public static bool CloseClient(ref Process client)
        {
            if (ProcessExists(client))
            {
                if (!client.CloseMainWindow())
                {
                    return false;   //unable to close the current instance of the RuneScape client
                }

                Stopwatch watch = new Stopwatch();
                watch.Start();
                while (ProcessExists(client) && (watch.ElapsedMilliseconds < 30000) && !BotProgram.StopFlag)
                {
                    Thread.Sleep(500);
                    client = GetClient();
                }
                if (ProcessExists(client))
                {
                    return false;
                }
            }

            client = null;
            ClientType = Client.None;
            return true;
        }

        /// <summary>
        /// Starts a new instance of the Runescape client
        /// </summary>
        /// <returns>true if successful</returns>
        public static bool StartClient(ref Process client, string clientFilePath, string arguments)
        {
            client = GetClient();
            if (ProcessExists(client))
            {
                return true;    //the Runescape client is already running
            }

            //start the Runescape client
            try
            {
                Process.Start(clientFilePath, arguments);

                //verify that the Runescape client has loaded
                Stopwatch watch = new Stopwatch();
                watch.Start();
                while (!ProcessExists(client) && (watch.ElapsedMilliseconds < 300000) && !BotProgram.StopFlag)
                {
                    Thread.Sleep(500);
                    client = GetClient();
                }
                return ProcessExists(client);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Returns a RuneScape client of the specified type
        /// </summary>
        /// <param name="loadError"></param>
        /// <param name="clientType"></param>
        /// <returns>null if no client is found</returns>
        public static Process GetClient()
        {
            Process[] processlist = Process.GetProcesses();

            foreach (Process process in processlist)
            {
                if (IsJagex(process)
                    || IsOSBuddy(process))
                {
                    return process;
                }
            }
            return null;    //no OSBuddy client found
        }

        /// <summary>
        /// Determines if a process is an OSBuddy client
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        public static bool IsOSBuddy(Process process)
        {
            if (process == null) { return false; }

            const string windowName = "OSBUDDY";
            string mainWindowTitle = process.MainWindowTitle.ToUpper();
            if (mainWindowTitle.Contains(windowName) && !mainWindowTitle.Contains("LOADER"))
            {
                ClientType = Client.OSBuddy;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Determines if a process is the Jagex client
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        public static bool IsJagex(Process process)
        {
            if (process == null) { return false; }

            string windowName = "OLD SCHOOL RUNESCAPE";
            string mainWindowTitle = process.MainWindowTitle.ToUpper();
            if (mainWindowTitle.Contains(windowName))
            {
                ClientType = Client.Jagex;
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        /// <summary>
        /// Gives the offset of the position of the login screen based on the client
        /// </summary>
        /// <returns></returns>
        public static Point LoginScreenOffset()
        {
            Point offset = new Point(0, 0);

            switch (ClientType)
            {
                case Client.Jagex:
                    offset = new Point(0, 20);
                    break;
                default:
                    offset = new Point(0, 0);
                    break;
            }
            return offset;
        }
    }
}
