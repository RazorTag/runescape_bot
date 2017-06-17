using RunescapeBot.Common;
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
        #region properties
        /// <summary>
        /// Checksum for the RUNE SCAPE logo on the login page
        /// </summary>
        private const long LOGIN_LOGO_COLOR_SUM = 15456063;
        private const long LOGIN_LOGO_SUM_INFERNAL = 10145709;

        /// <summary>
        /// Approximate time in milliseconds needed for OSBuddy to start
        /// </summary>
        private const int OSBUDDY_LOAD_TIME = 20000;

        /// <summary>
        /// Error message to show the user for a start error
        /// </summary>
        private string LoadError;

        /// <summary>
        /// Specifies how the bot should be run
        /// </summary>
        public StartParams RunParams;

        /// <summary>
        /// Process to which this bot program is attached
        /// </summary>
        private Process client;
        protected Process RSClient
        {
            get
            {
                return client;
            }
            set
            {
                client = value;
                if (Keyboard != null)
                {
                    Keyboard.SetClient(client);
                }
                if (Inventory != null)
                {
                    Inventory.SetClient(client);
                }
            }
        }

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
        private Color[,] colorArray;
        protected Color[,] ColorArray
        {
            get
            {
                return colorArray;
            }
            set
            {
                colorArray = value;
                if (Inventory != null)
                {
                    Inventory.SetScreen(colorArray);
                }
            }
        }

        /// <summary>
        /// The time that the last screenshot was taken
        /// </summary>
        protected DateTime LastScreenShot { get; set; }

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
        /// Set to true after the bot stops naturally
        /// </summary>
        protected bool BotIsDone { get; set; }

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
                    return new Point(ScreenWidth / 2, ScreenHeight / 2);
                }
            }
        }

        /// <summary>
        /// The width in pixels of the most recent image o the game screen
        /// </summary>
        protected int ScreenWidth
        {
            get
            {
                return ColorArray.GetLength(0);
            }
        }

        /// <summary>
        /// The width in pixels of the most recent image o the game screen
        /// </summary>
        protected int ScreenHeight
        {
            get
            {
                return ColorArray.GetLength(1);
            }
        }
        #endregion

        #region core bot process
        /// <summary>
        /// Initializes a bot program with a client matching startParams
        /// </summary>
        /// <param name="startParams">specifies the username to search for</param>
        protected BotProgram(StartParams startParams)
        {
            RSClient = ScreenScraper.GetOSBuddy(out LoadError);
            RunParams = startParams;
            RNG = new Random();
            Keyboard = new Keyboard(RSClient);
            Inventory = new Inventory(RSClient, ColorArray);
        }
       
        /// <summary>
        /// Begins execution of the bot program. Fails if a bot program is already running for the selected process.
        /// </summary>
        /// <param name="runningBots"></param>
        /// <param name="iterations"></param>
        public void Start()
        {
            BotIsDone = false;
            if (PrepareClient())
            {
                RunThread = new Thread(Process);
                RunThread.Start();
            }
            else
            {
                Done();
            }
        }

        /// <summary>
        /// Makes sure that OSBuddy is running and starts it if it isn't
        /// </summary>
        /// <param name="forceRestart">Set to true to force a client restart even if the client is already running</param>
        /// <returns></returns>
        private bool PrepareClient(bool forceRestart = false)
        {
            if (!forceRestart && ScreenScraper.ProcessExists(RSClient)) { return true; }

            if (ScreenScraper.RestartOSBuddy(RunParams.ClientFilePath, ref client))
            {
                for (int i = 0; i < 180; i++)
                {
                    if (SafeWait(OSBUDDY_LOAD_TIME)) { return false; }
                    RSClient = ScreenScraper.GetOSBuddy(out LoadError);

                    if (RSClient != null)
                    {
                        ReadWindow();
                        if (IsLoggedOut())
                        {
                            return true;
                        }
                    }
                }
            }

            //The client did not start correctly
            if (String.IsNullOrEmpty(LoadError))
            {
                MessageBox.Show("OSBuddy did not start correctly");
            }
            else
            {
                MessageBox.Show(LoadError);
            }

            return false;
        }

        /// <summary>
        /// Handles the sequential calling of the methods used to do bot work
        /// </summary>
        private void Process()
        {
            int awakeTime = UnitConversions.HoursToMilliseconds(12);
            Stopwatch sleepWatch = new Stopwatch();
            bool done = false;

            Setup();
            sleepWatch.Start();

            //alternate between work periods (Iterate) and break periods
            do
            {
                done = Iterate();
                if (!done)
                {
                    if (sleepWatch.ElapsedMilliseconds < awakeTime) //rest before another work cycle
                    {
                        if (RNG.NextDouble() > 0.2) //20% chance to stay logged in until automatically loggin out after 5 minutes of inactivity
                        {
                            Logout();    //80% chance to log out normally
                        }
                        done = SafeWait(RandomBreakTime());
                    }
                    else  //get a good night's sleep before resuming
                    {
                        done = SafeWait(RandomSleepTime());
                        sleepWatch.Restart();
                    }
                }
            } while (!done);
            
            Done();
        }

        /// <summary>
        /// Standard setup before running a bot
        /// </summary>
        private void Setup()
        {
            ReadWindow();
            bool test = IsLoggedIn();
            if (IsLoggedOut())
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
        /// <returns>true if execution is finished and the bot should stop. returns false if the bot should continue after a break.</returns>
        private bool Iterate()
        {
            if (StopFlag) { return true; }
            int randomFrameOffset, randomFrameTime;
            
            //don't limit by iterations unless the user has specified a positive number of iterations
            if (RunParams.Iterations == 0)
            {
                RunParams.Iterations = int.MaxValue;
            }

            //don't limit by run until time unless the user has specified a future date/time
            if ((RunParams.RunUntil - DateTime.Now).TotalMilliseconds <= 0)
            {
                RunParams.RunUntil = DateTime.MaxValue;
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

            Stopwatch iterationWatch = new Stopwatch();
            Stopwatch workIntervalWatch = new Stopwatch();
            workIntervalWatch.Start();
            int workInterval = RandomWorkTime();

            while ((DateTime.Now < RunParams.RunUntil) && (RunParams.Iterations-- > 0))
            {
                iterationWatch.Restart();
                if (!ReadWindow()) { continue; }   //Read the game window color values into Bitmap and ColorArray
                if (StopFlag) { return true; }   //quit immediately if the stop flag has been raised or we can't log back in

                //Turn on run if the player has run energy
                RunCharacter();

                //Only do the actual botting if we are logged in
                if (CheckLogIn())
                {
                    if (Bitmap != null) //Make sure the read is successful before using the bitmap values
                    {
                        if (!Execute()) //quit by an override Execute method
                        {
                            return true;
                        }
                        if (StopFlag) { return true; }
                    }
                }
                else
                {
                    if (!HandleFailedLogIn()) { return true; }   //stop if we are unable to recover
                }

                randomFrameTime = RunParams.FrameTime + RNG.Next(-randomFrameOffset, randomFrameOffset + 1);
                randomFrameTime = Math.Max(0, randomFrameTime);
                if (iterationWatch.ElapsedMilliseconds < randomFrameTime)
                {
                    SafeWait(randomFrameTime - (int)iterationWatch.ElapsedMilliseconds);
                }
                if (StopFlag) { return true; }
                if (workIntervalWatch.ElapsedMilliseconds > workInterval) { return false; } //stop execution so that the bot can take a break and resume later
            }

            return true;
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
            BotIsDone = true;
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
            StopFlag = true;
        }

        /// <summary>
        /// Generates a number of milliseconds for the bot to run before logging out and resting
        /// </summary>
        /// <returns>the next work time in milliseconds</returns>
        private int RandomWorkTime()
        {
            int workType = RNG.Next(0, 7);
            double mean, stdDev;   //measured in minutes


            if (workType < 1)   // 0
            {
                mean = 45;
                stdDev = 8;
            }
            else if (workType < 2) // 1
            {
                mean = 90; 
                stdDev = 5;
            }
            else if (workType < 5) // 2-4
            {
                mean = 115;
                stdDev = 30;
            }
            else  // 5-6
            {
                mean = 255;
                stdDev = 56;
            }

            double workTime = Probability.BoundedGaussian(mean, stdDev, 1.0, double.MaxValue);
            return (int) (workTime * 60 * 1000);
        }

        /// <summary>
        /// Generates a random number of milliseconds for the bot to take a break
        /// </summary>
        /// <returns>the number of milliseconds for the bot to rest</returns>
        private int RandomBreakTime()
        {
            const int avgBreakLength = 15 * 60 * 1000;  // 15 minutes = 900,000 ms
            const int minBreakTime = 2 * 60 * 1000;     // 2 minutes = 120,000 ms
            int breakLength = (int)Probability.BoundedGaussian(avgBreakLength, 0.35 * avgBreakLength, minBreakTime, double.MaxValue);
            return breakLength;
        }

        /// <summary>
        /// Generates a random number of milliseconds for the bot to sleep at night
        /// </summary>
        /// <returns>the number of milliseconds for the bot to sleep</returns>
        private int RandomSleepTime()
        {
            const double avgSleepLength = 7.8 * 60 * 60 * 1000;  //7.8 hours = 28,080,000 ms
            const double standardDeviation = 20 * 60 * 1000;   //20 minutes = 1,200,000 ms
            int breakLength = (int)Probability.RandomGaussian(avgSleepLength, standardDeviation);
            return breakLength;
        }

        #endregion

        #region user actions
        /// <summary>
        /// Wrapper for MouseActions.LeftMouseClick
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="hoverDelay"></param>
        /// <param name="randomize">maximum number of pixels in each direction by which to randomize the click location</param>
        protected void LeftClick(int x, int y, int randomize = 0, int hoverDelay = 200)
        {
            if (!StopFlag)  //don't click if the stop flag has been raised
            {
                Mouse.LeftClick(x, y, RSClient, randomize, hoverDelay);
            }
        }

        /// <summary>
        /// Wrapper for MouseActions.RightMouseClick
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        protected void RightClick(int x, int y, int randomize = 0, int hoverDelay = 200)
        {
            if (!StopFlag)  //don't click if the stop flag has been raised
            {
                Mouse.RightClick(x, y, RSClient, randomize, hoverDelay);
            }
        }

        /// <summary>
        /// Clicks on a stationary object
        /// </summary>
        /// <param name="stationaryObject">color filter for the stationary object</param>
        /// <param name="tolerance">maximum allowable distance between two subsequent checks to consider both objects the same object</param>
        /// <param name="afterClickWait">time to wait after clicking on the stationary object</param>
        /// <param name="maxWaitTime">maximum time to wait before giving up</param>
        /// <returns></returns>
        protected bool ClickStationaryObject(ColorRange stationaryObject, double tolerance, int afterClickWait, int maxWaitTime, int minimumSize)
        {
            Blob foundObject;

            if (LocateStationaryObject(stationaryObject, out foundObject, tolerance, maxWaitTime, minimumSize))
            {
                LeftClick(foundObject.Center.X, foundObject.Center.Y);
                SafeWaitPlus(afterClickWait, 0.2 * afterClickWait);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Looks for an object that isn't moving (meaning the player isn't moving)
        /// </summary>
        /// <param name="stationaryObject">color filter used to locate the stationary object</param>
        /// <param name="foundObject">returns the Blob if it is found</param>
        /// <param name="tolerance">maximum allowed distance in pixels between subsequent object locations</param>
        /// <param name="maxWaitTime">time to wait before gving up</param>
        /// <param name="minimumSize">minimum required size of the object in pixels</param>
        /// <returns>True if the object is found</returns>
        protected bool LocateStationaryObject(ColorRange stationaryObject, out Blob foundObject, double tolerance, int maxWaitTime, int minimumSize)
        {
            foundObject = null;
            Point? lastPosition = null;
            const int scanInterval = 100; //minimum time between checks in milliseconds
            Stopwatch intervalWatch = new Stopwatch();
            Stopwatch giveUpWatch = new Stopwatch();
            giveUpWatch.Start();

            while (giveUpWatch.ElapsedMilliseconds < maxWaitTime)
            {
                if (StopFlag) { return false; }

                intervalWatch.Restart();
                Blob objectBlob;
                LocateObject(stationaryObject, out objectBlob, minimumSize);

                if (objectBlob != null && objectBlob.Size >= minimumSize)
                {
                    if (Geometry.DistanceBetweenPoints(objectBlob.Center, lastPosition) <= tolerance)
                    {
                        foundObject = objectBlob;
                        return true;
                    }
                    else
                    {
                        lastPosition = objectBlob.Center;
                    }
                }
                else
                {
                    lastPosition = null;
                }

                if (StopFlag) { return false; }
                SafeWait(Math.Max(0, scanInterval - (int)intervalWatch.ElapsedMilliseconds));
            }

            return false;
        }

        /// <summary>
        /// Looks for an object that matches a filter
        /// </summary>
        /// <param name="stationaryObject"></param>
        /// <param name="foundObject"></param>
        /// <param name="maxWaitTime"></param>
        /// <param name="minimumSize"></param>
        /// <returns></returns>
        protected bool LocateObject(ColorRange stationaryObject, out Blob foundObject, int minimumSize)
        {
            ReadWindow();
            bool[,] objectPixels = ColorFilter(stationaryObject);
            EraseClientUIFromMask(ref objectPixels);
            Blob objectBlob = ImageProcessing.BiggestBlob(objectPixels);

            if (objectBlob != null && objectBlob.Size >= minimumSize)
            {
                foundObject = objectBlob;
                return true;
            }
            else
            {
                foundObject = null;
                return false;
            }
        }
        #endregion

        #region vision utilities
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

            if (!PrepareClient()) { return false; }
            LastScreenShot = DateTime.Now;
            Bitmap = ScreenScraper.CaptureWindow(RSClient);
            ColorArray = ScreenScraper.GetRGB(Bitmap);

            return Bitmap != null;
        }

        /// <summary>
        /// Gets the time since the last screenshot
        /// </summary>
        /// <returns>time elapsed since the most recent screenshot</returns>
        protected int TimeSinceLastScreenShot()
        {
            TimeSpan elapsedTime = DateTime.Now - LastScreenShot;
            return (int)elapsedTime.TotalMilliseconds;
        }

        /// <summary>
        /// Takes a new screenshot if the current one is too old
        /// </summary>
        /// <param name="maxScreenShotAge">the maximum usable age of an old screenshot in milliseconds</param>
        protected void UpdateScreenshot(int maxScreenshotAge = 500)
        {
            if (TimeSinceLastScreenShot() <= maxScreenshotAge)
            {
                ReadWindow();
            }
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
        /// <param name="filter"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="top"></param>
        /// <param name="bottom"></param>
        /// <returns></returns>
        protected bool[,] ColorFilterPiece(ColorRange filter, int left, int right, int top, int bottom, out Point trimOffset)
        {
            Color[,] colorArray = ScreenPiece(left, right, top, bottom, out trimOffset);
            if (ColorArray == null)
            {
                return null;
            }
            return ImageProcessing.ColorFilter(colorArray, filter);
        }

        /// <summary>
        /// Creates a boolean array of a portion of the screen to represent a color filter match
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="top"></param>
        /// <param name="bottom"></param>
        /// <returns></returns>
        protected bool[,] ColorFilterPiece(ColorRange filter, int left, int right, int top, int bottom)
        {
            Point empty;
            return ColorFilterPiece(filter, left, right, top, bottom, out empty);
        }

        /// <summary>
        /// Filters in a square within the screen shot
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <param name="offset"></param>
        /// <returns>The filtered screenshot cropped to the edges of the circle</returns>
        protected bool[,] ColorFilterPiece(ColorRange filter, Point center, int radius, out Point offset)
        {
            int left = center.X - radius;
            int right = center.X + radius;
            int top = center.Y - radius;
            int bottom = center.Y + radius;
            return ColorFilterPiece(filter, left, right, top, bottom, out offset);
        }

        /// <summary>
        /// Filters in a square within the screen shot
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <returns>The filtered screenshot cropped to the edges of the circle</returns>
        protected bool[,] ColorFilterPiece(ColorRange filter, Point center, int radius)
        {
            Point offset;
            return ColorFilterPiece(filter, center, radius, out offset);
        }

        /// <summary>
        /// Gets a rectangle from ColorArray
        /// </summary>
        /// <param name="topLeft"></param>
        /// <param name="bottomRight"></param>
        /// <returns></returns>
        protected Color[,] ScreenPiece(int left, int right, int top, int bottom, out Point trimOffset)
        {
            return ImageProcessing.ScreenPiece(ColorArray, left, right, top, bottom, out trimOffset);
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
        /// Gets a rectangle containing the minimap with the non-minimap corners set to false
        /// </summary>
        /// <param name="filter">the filter to use on the minimap</param>
        /// <param name="offset">gets set to the offset from the game screen to the minimap piece</param>
        /// <returns>true for the pixels on the minimap that match the filter</returns>
        protected bool[,] MinimapFilter(ColorRange filter, out Point offset)
        {
            int left = ScreenWidth - 157;
            int right = ScreenWidth - 8;
            int top = 8;
            int bottom = 159;
            Point center = new Point((left + right) / 2, (top + bottom) / 2);
            double radius = 70.0;
            double distance;
            offset = new Point(left, top);
            bool[,] minimapFilter = new bool[right - left + 1, bottom - top + 1];

            for (int x = left; x <= right; x++)
            {
                for (int y = top; y <= bottom; y++)
                {
                    distance = Math.Sqrt(Math.Pow(x - center.X, 2) + Math.Pow(y - center.Y, 2));
                    if (distance < radius)
                    {
                        minimapFilter[x - left, y - top] = filter.ColorInRange(ColorArray[x, y]);
                    }
                }
            }

            return minimapFilter;
        }

        /// <summary>
        /// Gets a rectangle containing the minimap with the non-minimap corners set to false
        /// </summary>
        /// <param name="filter">the filter to use on the minimap</param>
        /// <returns>true for the pixels on the minimap that match the filter</returns>
        protected bool[,] MinimapFilter(ColorRange filter)
        {
            Point offset;
            return MinimapFilter(filter, out offset);
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
        #endregion

        #region login/restart
        /// <summary>
        /// Respond to a failed attempt to log in
        /// </summary>
        /// <returns>true if the failed login is handled satisfactorily. false if the bot should stop</returns>
        private bool HandleFailedLogIn()
        {
            return PrepareClient(true);
        }

        /// <summary>
        /// Determines if the user is logged out and logs him back in if he is.
        /// If the bot does not have valid login information, then it will quit.
        /// </summary>
        /// <returns>true if we are already logged in or we are able to log in, false if we can't log in</returns>
        private bool CheckLogIn()
        { 
            if (!IsLoggedOut())
            {
                return true;    //already logged in
            }
            if (SafeWait(5000)) { return false; }
            if (!IsLoggedOut())
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
        /// Tries to log in.
        /// </summary>
        /// <returns>true if login is successful, false if login fails</returns>
        private bool LogIn()
        {
            Point? clickLocation;
            int center = ScreenWidth / 2;

            //log in at the login screen
            if (!IsWelcomeScreen(out clickLocation))
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
            if (ConfirmWelcomeScreen(out clickLocation))
            {
                LeftClick(clickLocation.Value.X, clickLocation.Value.Y);
            }
            else
            {
                return false;
            }

            //verify the log in
            if (ConfirmLogin())
            {
                DefaultCamera();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Determines if the last screenshot was of the welcome screen
        /// </summary>
        /// <returns>true if we are on the welcome screen, false otherwise</returns>
        private bool IsWelcomeScreen(out Point? clickLocation)
        {
            int centerX = Center.X;
            const int centerY = 337;
            const int width = 220;
            const int height = 80;
            int left = centerX - (width / 2);
            int right = centerX + (width / 2);
            int top = centerY - (height / 2);
            int bottom = centerY + (height / 2);
            int totalSize = width * height;
            int redBlobSize;

            ColorRange red = ColorFilters.WelcomeScreenClickHere();
            bool[,] clickHere = ColorFilterPiece(red, left, right, top, bottom);
            Blob enterGame = ImageProcessing.BiggestBlob(clickHere);
            redBlobSize = enterGame.Size;

            if (redBlobSize > (totalSize / 2))
            {
                clickLocation = enterGame.RandomBlobPixel();
                clickLocation = new Point(clickLocation.Value.X + left, clickLocation.Value.Y + top);
                return true;
            }
            else
            {
                clickLocation = null;
                return false;
            }
        }

        /// <summary>
        /// Determines if the welcome screen has been reached
        /// </summary>
        /// <returns>true if the welcome screen has been reached, false if not or if the StopFlag is raised</returns>
        private bool ConfirmWelcomeScreen(out Point? clickLocation)
        {
            clickLocation = null;

            //Wait up to 60 seconds
            for (int i = 0; i < 60; i++)
            {
                if (StopFlag) { return false; }
                ReadWindow();
                
                if (IsWelcomeScreen(out clickLocation))
                {
                    return true;
                }
                else
                {
                    if (StopFlag) { return false; }
                    SafeWait(1000);
                }
            }
            return false;   //We timed out waiting.
        }

        /// <summary>
        /// Determines if  the client is logged in
        /// </summary>
        /// <returns>true if we are verifiably logged in</returns>
        private bool IsLoggedIn()
        {
            MakeSureWindowHasBeenRead();

            //Get a piece of the column from the right of the inventory
            int right = ScreenWidth - 10;
            int left = right - 12;
            int bottom = ScreenHeight - 105;
            int top = bottom - 45;
            Color[,] inventoryColumn = ScreenPiece(left, right, top, bottom);

            //Compare the column against the expected value
            long columnSum = ImageProcessing.ColorSum(inventoryColumn);
            long expectedColumnSum = 122689;
            if (columnSum > (1.01 * expectedColumnSum) || columnSum < (0.99 * expectedColumnSum))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Waits on the client to log in after clicking through the welcome screen
        /// </summary>
        /// <returns>true if the log in is verified, false if we time out waiting</returns>
        private bool ConfirmLogin()
        {
            //Wait for up to 60 seconds
            for (int i = 0; i < 60; i++)
            {
                if (StopFlag) { return false; }
                ReadWindow();
                if (IsLoggedIn())
                {
                    return true;
                }
                else
                {
                    if (StopFlag) { return false; }
                    SafeWait(1000);
                }
            }

            return false;   //We timed out waiting.
        }

        /// <summary>
        /// Determines if the client is logged out
        /// </summary>
        /// <returns>true if we are verifiably logged out</returns>
        private bool IsLoggedOut()
        {
            MakeSureWindowHasBeenRead();

            Color color;
            int height = ScreenHeight;
            int centerX = Center.X;
            int checkRow = Math.Min(height - 1, ScreenScraper.LOGIN_WINDOW_HEIGHT + 1);    //1 pixel below where the bottom of the login window should be
            int xOffset = (ScreenScraper.LOGIN_WINDOW_WIDTH / 2) + 2;
            for (int x = centerX - xOffset; x < centerX + xOffset; x++)
            {
                //check bottom of login box
                color = ColorArray[x, checkRow];
                if (!ImageProcessing.ColorsAreEqual(color, Color.Black))
                {
                    return false;
                }
            }
            for (int y = 0; y < checkRow; y++)  //check sides
            {
                //check left of login box
                color = ColorArray[centerX - xOffset, y];
                if (!ImageProcessing.ColorsAreEqual(color, Color.Black))
                {
                    return false;
                }

                //check right of login box
                color = ColorArray[centerX + xOffset, y];
                if (!ImageProcessing.ColorsAreEqual(color, Color.Black))
                {
                    return false;
                }
            }

            //color-based hash of the RUNE SCAPE logo on the login screen to verify that it is there
            long colorSum = ImageProcessing.ColorSum(ScreenPiece(Center.X - 224, Center.X + 220, 0, 160));
            if (!Numerical.CloseEnough(LOGIN_LOGO_COLOR_SUM, colorSum, 0.01) && !Numerical.CloseEnough(LOGIN_LOGO_SUM_INFERNAL, colorSum, 0.01))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Logs out of the game
        /// </summary>
        protected void Logout()
        {
            LeftClick(ScreenWidth - 120, ScreenHeight - 18);
            LeftClick(ScreenWidth - 120, ScreenHeight - 86);
        }
        #endregion

        #region miscellaneous
        /// <summary>
        /// Waits for the specified time while periodically checking for the stop flag
        /// </summary>
        /// <param name="waitTime"></param>
        /// <returns>true if the StopFlag has been raised</returns>
        protected bool SafeWait(int waitTime)
        {
            int nextWaitTime;
            int waitInterval = 100;
            Stopwatch watch = new Stopwatch();
            watch.Start();

            while (watch.ElapsedMilliseconds < waitTime)
            {
                nextWaitTime = Math.Min(waitInterval, (int)(waitTime - watch.ElapsedMilliseconds));
                Thread.Sleep(nextWaitTime);
                if (StopFlag) { return true; }
            }
            return StopFlag;
        }

        /// <summary>
        /// Waits for a random time from a Gaussian distribution
        /// </summary>
        /// <param name="meanWaitTime">average wait time</param>
        /// <param name="standardDeviation">standard deviation froom the mean</param>
        /// <returns>true if the StopFlag has been raised</returns>
        protected bool SafeWait(int meanWaitTime, double standardDeviation)
        {
            if (meanWaitTime <= 0)
            {
                return StopFlag;
            }
            else
            {
                int waitTime = (int)Probability.BoundedGaussian(meanWaitTime, standardDeviation, 0.0, double.MaxValue);
                return SafeWait(waitTime);
            }
        }

        /// <summary>
        /// Waits for at least the specified wait time
        /// </summary>
        /// <param name="minWaitTime"></param>
        /// <param name="stdDev"></param>
        /// <returns></returns>
        protected bool SafeWaitPlus(int minWaitTime, double stdDev)
        {
            if (minWaitTime <= 0)
            {
                return StopFlag;
            }
            else
            {
                int waitTime = (int)Probability.HalfGaussian(minWaitTime, stdDev, true);
                return SafeWait(waitTime);
            }
        }

        /// <summary>
        /// Positions the camera facing north and as high as possible
        /// </summary>
        protected void DefaultCamera()
        {
            int compassX = ScreenWidth - 159;
            int compassY = 21;
            LeftClick(compassX, compassY);
            Keyboard.UpArrow(1000);
        }

        /// <summary>
        /// Calls ReadWindow if the current screen image is unsatisfactory
        /// </summary>
        /// <returns>true unless the window needs to be read but can't</returns>
        private bool MakeSureWindowHasBeenRead()
        {
            if ((Bitmap == null) || (ColorArray == null))
            {
                return ReadWindow();
            }
            if ((ScreenWidth == 0) || (ScreenHeight == 0))
            {
                return ReadWindow();
            }
            return true;
        }

        /// <summary>
        /// Sets the player to run (as opposed to walk) if Run is enabled and run energy is fairly high (~50%)
        /// </summary>
        protected void RunCharacter()
        {
            if (!RunParams.Run)
            {
                return;
            }

            if (!CharacterIsRunning() && RunEnergyIsHigh())
            {
                ToggleRun();
            }
        }

        /// <summary>
        /// Toggle the run/walk status using the run enery meter next to the minimap
        /// </summary>
        protected void ToggleRun()
        {
            int x = ScreenWidth - 145;
            int y = 144;
            LeftClick(x, y, 3, 200);
        }

        /// <summary>
        /// Determines if the character is currently running
        /// </summary>
        /// <returns>true for running, false for walking</returns>
        protected bool CharacterIsRunning()
        {
            Color runColor = GetPixel(ScreenWidth - 145, 147);
            ColorRange runEnergyFoot = ColorFilters.RunEnergyFoot();
            return runEnergyFoot.ColorInRange(runColor);
        }

        /// <summary>
        /// Determines if the character's run energy is above roughly 50%
        /// </summary>
        /// <returns></returns>
        protected bool RunEnergyIsHigh()
        {
            int left = ScreenWidth - 181;
            int right = ScreenWidth - 162;
            int top = 141;
            int bottom = 155;
            Color[,] runEnergyPercentage = ScreenPiece(left, right, top, bottom);
            return MinimapGaugeIsHigh(runEnergyPercentage, 0.05);
        }

        /// <summary>
        /// Determines if a minimap gauge is above roughly 50%
        /// </summary>
        /// <param name="gaugePercentage">array of Color pixels containing the gauge percentage number</param>
        /// <param name="threshold">minimum match needed to be considered high</param>
        /// <returns>true if the gauge is low, false otherwise</returns>
        protected bool MinimapGaugeIsHigh(Color[,] gaugePercentage, double threshold)
        {
            ColorRange highGauge = ColorFilters.MinimapGaugeYellowGreen();
            double highMatch = ImageProcessing.FractionalMatch(gaugePercentage, highGauge);
            return highMatch >= threshold;
        }
        #endregion
    }
}