using RunescapeBot.FileIO;
using RunescapeBot.ImageTools;
using RunescapeBot.UITools;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace RunescapeBot.BotPrograms
{
    /// <summary>
    /// Used by the bot to inform that is has completed its task
    /// </summary>
    public delegate void BotResponse();

    /// <summary>
    /// Base class for bot programs that handles starting and stopping of bot programs
    /// Implement Run in a derived class to tell a bot program what to do
    /// </summary>
    public class BotProgram
    {
        /// <summary>
        /// Checksum for the RUNE SCAPE logo on the login page
        /// </summary>
        private const long LOGIN_LOGO_COLOR_SUM = 15456063;

        /// <summary>
        /// Milliseconds to wait after starting the client
        /// </summary>
        private const int CLIENT_STARTUP_WAIT_TIME = 20000;

        /// <summary>
        /// Error message to show the user for a start error
        /// </summary>
        private string LoadError;

        /// <summary>
        /// The number of consecutive previously failed login attempts.
        /// </summary>
        private int failedLoginAttempts;

        /// <summary>
        /// Specifies how the bot should be run
        /// </summary>
        public StartParams RunParams;

        /// <summary>
        /// Process to which this bot program is attached
        /// </summary>
        protected Process RSClient { get; set; }

        /// <summary>
        /// Keyboard controller
        /// </summary>
        protected Keyboard Keyboard { get; set; }

        /// <summary>
        /// Thread in which the run method is executed
        /// </summary>
        protected Thread RunThread { get; set; }

        /// <summary>
        /// Stores a bitmap of the client window
        /// </summary>
        protected Bitmap Bitmap { get; set; }

        /// <summary>
        /// Stores a Color array of the client window
        /// </summary>
        protected Color[,] ColorArray { get; set; }

        /// <summary>
        /// The sidebar including the inventory and spellbook
        /// </summary>
        protected Inventory Inventory { get; set; }

        /// <summary>
        /// Stock random number generator
        /// </summary>
        protected Random RNG { get; set; }

        /// <summary>
        /// Tells anyone listening to stop at their convenience
        /// </summary>
        protected bool StopFlag { get; set; }

        /// <summary>
        /// Center of the screen
        /// </summary>
        protected Point Center
        {
            get
            {
                if (ColorArray == null)
                {
                    return new Point(0, 0);
                }
                else
                {
                    return new Point(ColorArray.GetLength(0) / 2, ColorArray.GetLength(1) / 2);
                }
            }
        }


        /// <summary>
        /// Initializes a bot program with a client matching startParams
        /// </summary>
        /// <param name="startParams">specifies the username to search for</param>
        public BotProgram(StartParams startParams)
        {
            RSClient = ScreenScraper.GetOSBuddy(out LoadError);
            this.RunParams = startParams;
            RNG = new Random();
            Keyboard = new Keyboard(RSClient);
            Inventory = new Inventory(RSClient);
        }
       
        /// <summary>
        /// Begins execution of the bot program. Fails if a bot program is already running for the selected process.
        /// </summary>
        /// <param name="runningBots"></param>
        /// <param name="iterations"></param>
        public void Start()
        {
            if (!String.IsNullOrEmpty(LoadError))
            {
                Process client;

                if (ScreenScraper.StartOSBuddy(RunParams.ClientFilePath, out client))
                {
                    RSClient = client;
                    SafeWait(CLIENT_STARTUP_WAIT_TIME);
                }
                else
                {
                    MessageBox.Show(LoadError);
                    return;
                }
            }
            RunThread = new Thread(Process);
            RunThread.Start();
        }

        /// <summary>
        /// Handles the sequential calling of the methods used to do bot work
        /// </summary>
        private void Process()
        {
            Setup();
            Iterate();
            Done();
        }

        /// <summary>
        /// Standard setup before running a bot
        /// </summary>
        private void Setup()
        {
            ReadWindow();
            if (!IsLoggedIn())
            {
                LogIn();
            }
            if (StopFlag) { return; }
            Run();
        }

        /// <summary>
        /// Contains the logic that determines what an implemented bot program does
        /// Executes before beginning iteration
        /// <param name="timeout">length of time after which the bot program should quit</param>
        /// </summary>
        protected virtual void Run()
        {
            if (StopFlag) { return; }
        }

        /// <summary>
        /// Begins iterating after Run is called. Called for the number of iterations specified by the user.
        /// Is only called if both Iterations and FrameRate are specified.
        /// </summary>
        private void Iterate()
        {
            if (StopFlag) { return; }
            int randomFrameOffset, randomFrameTime;
            
            //don't limit by iterations unless the user has specified a positive number of iterations
            if (RunParams.Iterations == 0)
            {
                RunParams.Iterations = int.MaxValue;
            }
            
            //randomize the time between executions
            if (RunParams.RandomizeFrames)
            {
                randomFrameOffset = (int) (0.6 * RunParams.FrameTime);
            }
            else
            {
                randomFrameOffset = 0;
            }

            for (int i = 0; i < RunParams.Iterations; i++)
            {
                if (DateTime.Now > RunParams.RunUntil)
                {
                    return; //quit if we have gone over our time limit
                }

                Stopwatch watch = Stopwatch.StartNew();
                ReadWindow();   //Read the game window color values into Bitmap and ColorArray
                if (StopFlag) { return; }   //quit immediately if the stop flag has been raised or we can't log back in

                //Only do the actual botting if we are logged in
                if (CheckLogIn())
                {
                    if (Bitmap != null) //Make sure the read is successful before using the bitmap values
                    {
                        if (!Execute()) //quit by an override Execute method
                        {
                            return;
                        }
                        if (StopFlag) { return; }
                    }
                }
                else
                {
                    if (failedLoginAttempts > 10)
                    {
                        Process client = RSClient;
                        if (ScreenScraper.RestartOSBuddy(RunParams.ClientFilePath, ref client))
                        {
                            RSClient = client;
                            failedLoginAttempts = 0;
                            SafeWait(CLIENT_STARTUP_WAIT_TIME);
                        }
                        else
                        {
                            return; //The client didnt restart correctly, so we can't continue.
                        }
                    }
                }

                randomFrameTime = RunParams.FrameTime + RNG.Next(-randomFrameOffset, randomFrameOffset + 1);
                randomFrameTime = Math.Max(0, randomFrameTime);
                watch.Stop();
                if (watch.ElapsedMilliseconds < randomFrameTime)
                {
                    SafeWait(randomFrameTime - (int)watch.ElapsedMilliseconds);
                }
                if (StopFlag) { return; }
            }

            return;
        }

        /// <summary>
        /// A single iteration. Return false to stop execution.
        /// </summary>
        protected virtual bool Execute()
        {
            return false;
        }

        /// <summary>
        /// Clean up
        /// </summary>
        private void Done()
        {
            if (Bitmap != null)
            {
                Bitmap.Dispose();
            }
            RunParams.TaskComplete();
        }

        /// <summary>
        /// Forcefully stops execution of a bot program
        /// </summary>
        public void Stop()
        {
            if (StopFlag)
            {
                RunParams.TaskComplete();
            }
            else
            {
                StopFlag = true;
            }
        }

        /// <summary>
        /// Creates a boolean array to represent a color filter match
        /// </summary>
        /// <param name="artifactColor"></param>
        /// <returns></returns>
        protected bool[,] ColorFilter(ColorRange artifactColor)
        {
            return ColorFilter(ColorArray, artifactColor);
        }

        /// <summary>
        /// Creates a boolean array to represent a color filter match
        /// </summary>
        /// <param name="artifactColor"></param>
        /// <returns></returns>
        protected bool[,] ColorFilter(Color[,] image, ColorRange artifactColor)
        {
            if (ColorArray == null)
            {
                return null;
            }
            return ImageProcessing.ColorFilter(image, artifactColor);
        }

        /// <summary>
        /// Creates a boolean array of a portion of the screen to represent a color filter match
        /// </summary>
        /// <param name="artifactColor"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="top"></param>
        /// <param name="bottom"></param>
        /// <returns></returns>
        protected bool[,] ColorFilterPiece(ColorRange artifactColor, int left, int right, int top, int bottom, out Point trimOffset)
        {
            Color[,] colorArray = ScreenPiece(left, right, top, bottom, out trimOffset);
            if (ColorArray == null)
            {
                return null;
            }
            return ImageProcessing.ColorFilter(colorArray, artifactColor);
        }

        /// <summary>
        /// Creates a boolean array of a portion of the screen to represent a color filter match
        /// </summary>
        /// <param name="artifactColor"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="top"></param>
        /// <param name="bottom"></param>
        /// <returns></returns>
        protected bool[,] ColorFilterPiece(ColorRange artifactColor, int left, int right, int top, int bottom)
        {
            Point empty;
            return ColorFilterPiece(artifactColor, left, right, top, bottom, out empty);
        }

        /// <summary>
        /// Gets a rectangle from ColorArray
        /// </summary>
        /// <param name="topLeft"></param>
        /// <param name="bottomRight"></param>
        /// <returns></returns>
        protected Color[,] ScreenPiece(int left, int right, int top, int bottom, out Point trimOffset)
        {
            left = Math.Max(left, 0);
            right = Math.Min(right, ColorArray.GetLength(0) - 1);
            top = Math.Max(top, 0);
            bottom = Math.Min(bottom, ColorArray.GetLength(1) - 1);
            if ((left > right) || (top > bottom))
            {
                trimOffset = Point.Empty;
                return null;
            }
            Color[,] screenPiece = new Color[right - left + 1, bottom - top + 1];
            trimOffset = new Point(left, top);

            for (int x = left; x <= right; x++)
            {
                for (int y = top; y <= bottom; y++)
                {
                    screenPiece[x - left, y - top] = ColorArray[x, y];
                }
            }
            return screenPiece;
        }

        /// <summary>
        /// Gets a rectangle from ColorArray
        /// </summary>
        /// <param name="topLeft"></param>
        /// <param name="bottomRight"></param>
        /// <returns></returns>
        protected Color[,] ScreenPiece(int left, int right, int top, int bottom)
        {
            Point empty;
            return ScreenPiece(left, right, top, bottom, out empty);
        }

        /// <summary>
        /// Wrapper for ScreenScraper.CaptureWindow
        /// </summary>
        protected bool ReadWindow()
        {
            if (Bitmap != null)
            {
                Bitmap.Dispose();
            }
            if (StopFlag) { return false; }

            Bitmap = ScreenScraper.CaptureWindow(RSClient);
            ColorArray = ScreenScraper.GetRGB(Bitmap);

            return Bitmap != null;
        }

        /// <summary>
        /// Retrieve the color of a single pixel
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        protected Color GetPixel(int x, int y)
        {
            return ColorArray[x, y];
        }

        /// <summary>
        /// Determines the portion of the screen taken up by a blob
        /// </summary>
        /// <param name="artifact"></param>
        /// <returns></returns>
        protected double ArtifactSize(Blob artifact)
        {
            double screenSize = Bitmap.Size.Width * Bitmap.Size.Height;
            return artifact.Size / screenSize;
        }

        /// <summary>
        /// Wrapper for MouseActions.LeftMouseClick
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        protected void LeftClick(int x, int y, int hoverDelay = 100)
        {
            if (!StopFlag)  //don't click if the stop flag has been raised
            {
                Mouse.LeftClick(x, y, RSClient, hoverDelay);
            }
        }

        /// <summary>
        /// Wrapper for MouseActions.RightMouseClick
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        protected void RightClick(int x, int y, int hoverDelay = 100)
        {
            if (!StopFlag)  //don't click if the stop flag has been raised
            {
                Mouse.RightClick(x, y, RSClient, hoverDelay);
            }
        }

        /// <summary>
        /// Sets the pixels in client UI areas to false.
        /// This should only be used with untrimmed images.
        /// </summary>
        /// <param name="mask"></param>
        protected void EraseClientUIFromMask(ref bool[,] mask)
        {
            if (mask == null)
            {
                return;
            }

            int width = mask.GetLength(0);
            int height = mask.GetLength(1);

            EraseFromMask(ref mask, 0, 519, height - 159, height);              //erase chat box
            EraseFromMask(ref mask, width - 241, width, height - 336, height);  //erase inventory
            EraseFromMask(ref mask, width - 211, width, 0, 192);                //erase minimap
        }

        /// <summary>
        /// Clears a rectangle from a boolean mask
        /// </summary>
        /// <param name="mask"></param>
        /// <param name="xMin">Inclusive</param>
        /// <param name="xMax">Exclusive</param>
        /// <param name="yMin">Inclusive</param>
        /// <param name="yMax">Exclusive</param>
        protected void EraseFromMask(ref bool[,] mask, int xMin, int xMax, int yMin, int yMax)
        {
            for (int x = Math.Max(0, xMin); x < Math.Min(xMax, mask.GetLength(0) - 1); x++)
            {
                for (int y = Math.Max(0, yMin); y < Math.Min(yMax, mask.GetLength(1) - 1); y++)
                {
                    mask[x, y] = false;
                }
            }
        }

        /// <summary>
        /// Determines if the user is logged out and logs him back in if he is.
        /// If the bot does not have valid login information, then it will quit.
        /// </summary>
        /// <returns>true if we are already logged in or we are able to log in, false if we can't log in</returns>
        private bool CheckLogIn()
        { 
            if (IsLoggedIn())
            {
                return true;    //already logged in
            }
            SafeWait(5000);
            if (IsLoggedIn())
            {
                return true;
            }

            //see if we have login and password to log in
            if (string.IsNullOrEmpty(RunParams.Login) || string.IsNullOrEmpty(RunParams.Password))
            {
                MessageBox.Show("Cannot log in without login information");
                return false;
            }
            else
            {
                return LogIn();
            }
        }

        /// <summary>
        /// Tries to log in. Make sure the client window has focus before calling this method.
        /// </summary>
        /// <returns>true if login is successful, false if login fails</returns>
        private bool LogIn()
        {
            int center = ColorArray.GetLength(0) / 2;

            //log in at the login screen
            if (!IsWelcomeScreen())
            {
                //Click existing account. Clicks in a dead space if we are already on the login screen.
                LeftClick(center + 16, 288);

                //fill in login
                LeftClick(center + 137, 259);
                Keyboard.Backspace(350);
                Keyboard.WriteLine(RunParams.Login);

                //fill in password
                Keyboard.Tab();
                Keyboard.Backspace(20);
                Keyboard.WriteLine(RunParams.Password);

                //trigger the login button
                Keyboard.Enter();
                if (SafeWait(5000)) { return false; }
            }

            //click the "CLICK HERE TO PLAY" button on the welcome screen
            if (ConfirmWelcomeScreen())
            {
                failedLoginAttempts = 0;
                LeftClick(center, 337);
            }
            else
            {
                failedLoginAttempts++;
                return false;
            }
            if (SafeWait(5000)) { return false; }

            //verify the log in
            ReadWindow();
            return IsLoggedIn();
        }

        /// <summary>
        /// Determines if the last screenshot was of the welcome screen
        /// </summary>
        /// <returns>true if we are on the welcome screen, false otherwise</returns>
        private bool IsWelcomeScreen()
        {
            int centerX = Center.X;
            int centerY = 337;
            int offsetX = 110;
            int offsetY = 40;
            int totalSize = (2 * offsetX + 1) * (2 * offsetY + 1);
            int redBlobSize;

            ColorRange red = ColorFilters.WelcomeScreenClickHere();
            bool[,] clickHere = ColorFilter(red);
            clickHere = ColorFilterPiece(red, centerX - offsetX, centerX + offsetX, centerY - offsetY, centerY + offsetY);
            redBlobSize = ImageProcessing.BiggestBlob(clickHere).Size;

            return ((2 * redBlobSize) > totalSize);
        }

        /// <summary>
        /// Determines if the welcome screen has been reached
        /// </summary>
        /// <returns>true if the welcome screen has been reached, false if not or if the StopFlag is raised</returns>
        private bool ConfirmWelcomeScreen()
        {
            for (int i = 0; i < 10; i++)
            {
                if (StopFlag) { return false; }
                ReadWindow();
                if (IsWelcomeScreen())
                {
                    return true;
                }
                else
                {
                    if (StopFlag) { return false; }
                    SafeWait(1000);
                }
            }
            return false;
        }

        /// <summary>
        /// Determines if the client is logged in
        /// </summary>
        /// <returns>true if we are logged in, false if we are logged out</returns>
        private bool IsLoggedIn()
        {
            Color color;
            int height = ColorArray.GetLength(1);
            int centerX = Center.X;
            int checkRow = Math.Min(height, ScreenScraper.LOGIN_WINDOW_HEIGHT + 50);    //50 pixels below where the bottom of the login picture should be
            int xOffset = (ScreenScraper.LOGIN_WINDOW_WIDTH / 2) + 50;
            for (int x = centerX - xOffset; x < centerX + xOffset; x++)
            {
                //check bottom of login box
                color = ColorArray[x, checkRow];
                if (!ImageProcessing.ColorsAreEqual(color, Color.Black))
                {
                    return true;
                }
            }
            for (int y = 0; y < checkRow; y++)  //check sides
            {
                //check left of login box
                color = ColorArray[centerX - xOffset, y];
                if (!ImageProcessing.ColorsAreEqual(color, Color.Black))
                {
                    return true;
                }

                //check right of login box
                color = ColorArray[centerX + xOffset, y];
                if (!ImageProcessing.ColorsAreEqual(color, Color.Black))
                {
                    return true;
                }
            }

            //color-based hash of the RUNE SCAPE logo on the login screen to verify that it is there
            long colorSum = ImageProcessing.ColorSum(ScreenPiece(Center.X - 224, Center.X + 220, 0, 160));
            if ((colorSum < (LOGIN_LOGO_COLOR_SUM * 0.99)) || (colorSum > (LOGIN_LOGO_COLOR_SUM * 1.01)))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Waits for the specified time while periodically checking for the stop flag
        /// </summary>
        /// <param name="waitTime"></param>
        /// <returns>true if the StopFlag has been raised</returns>
        private bool SafeWait(int waitTime)
        {
            int nextWaitTime;
            int waitInterval = 1000;
            int numIntervals = (int) Math.Ceiling((double)waitTime / (double)waitInterval);
            Stopwatch watch = new Stopwatch();
            watch.Start();

            for (int i = 0; i < numIntervals; i++)
            {
                nextWaitTime = Math.Min(waitInterval, (waitTime - (int)watch.ElapsedMilliseconds));
                nextWaitTime = Math.Max(0, nextWaitTime);
                Thread.Sleep(nextWaitTime);
                if (StopFlag) { return true; }
            }
            return StopFlag;
        }

        /// <summary>
        /// Positions the camera facing north and as high as possible
        /// </summary>
        protected void DefaultCamera()
        {
            int compassX = ColorArray.GetLength(0) - 159;
            int compassY = 21;
            LeftClick(compassX, compassY);
            Keyboard.UpArrow(1000);
        }
    }
}
