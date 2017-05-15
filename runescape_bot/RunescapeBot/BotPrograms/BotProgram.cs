using RunescapeBot.Common;
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
        #region properties
        /// <summary>
        /// Checksum for the RUNE SCAPE logo on the login page
        /// </summary>
        private const long LOGIN_LOGO_COLOR_SUM = 15456063;

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

        /// <summary>
        /// Initializes a bot program with a client matching startParams
        /// </summary>
        /// <param name="startParams">specifies the username to search for</param>
        protected BotProgram(StartParams startParams)
        {
            RSClient = ScreenScraper.GetOSBuddy(out LoadError);
            this.RunParams = startParams;
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
            bool ready = false;

            if (!String.IsNullOrEmpty(LoadError))
            {
                if (ScreenScraper.StartOSBuddy(RunParams.ClientFilePath))
                {
                    for (int i = 0; i < 300; i++)
                    {
                        if (SafeWait(1000)) { return; }
                        RSClient = ScreenScraper.GetOSBuddy(out LoadError);

                        if(RSClient != null)
                        {
                            ReadWindow();
                            if (IsLoggedOut())
                            {
                                ready = true;
                                break;
                            }
                        }
                    }
                }
                if (!ready)
                {
                    if (!String.IsNullOrEmpty(LoadError))
                    {
                        MessageBox.Show(LoadError);
                    }
                    else
                    {
                        MessageBox.Show("OSBuddy did not start correctly");
                    }

                    Done();
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

                //Turn on run if the player has run energy
                RunCharacter();

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
                            if(SafeWait(1000)) { return; }
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

        #region user actions
        /// <summary>
        /// Wrapper for MouseActions.LeftMouseClick
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="hoverDelay"></param>
        /// <param name="randomize">maximum number of pixels in each direction by which to randomize the click location</param>
        protected void LeftClick(int x, int y, int hoverDelay = 200, int randomize = 0)
        {
            if (!StopFlag)  //don't click if the stop flag has been raised
            {
                Mouse.LeftClick(x, y, RSClient, hoverDelay, randomize);
            }
        }

        /// <summary>
        /// Wrapper for MouseActions.RightMouseClick
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        protected void RightClick(int x, int y, int hoverDelay = 200, int randomize = 0)
        {
            if (!StopFlag)  //don't click if the stop flag has been raised
            {
                Mouse.RightClick(x, y, RSClient, hoverDelay, randomize);
            }
        }

        /// <summary>
        /// Clicks on a stationary object
        /// </summary>
        /// <param name="stationaryObject">color filter for the stationary object</param>
        /// <param name="tolerance">maximum allowable distance between two subsequent checks to consider both objects the same object</param>
        /// <param name="afterClickWait">time to wait affter clicking on the stationary object</param>
        /// <param name="maxWaitTime">maximum time to wait before giving up</param>
        /// <returns></returns>
        protected bool ClickStationaryObject(ColorRange stationaryObject, double tolerance, int afterClickWait, int maxWaitTime)
        {
            Point? objectLocation;

            if (LocateStationaryObject(stationaryObject, out objectLocation, tolerance, maxWaitTime))
            {
                LeftClick(objectLocation.Value.X, objectLocation.Value.Y);
                SafeWait(afterClickWait);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Looks for an object that isn't moving (meaning the player isn't moving)
        /// </summary>
        /// <returns>True if the object is found</returns>
        protected bool LocateStationaryObject(ColorRange stationaryObject, out Point? objectLocation, double tolerance, int maxWaitTime)
        {
            objectLocation = null;
            Point? lastPosition = null;
            const int scanInterval = 200; //time between checks in milliseconds
            Stopwatch watch = new Stopwatch();

            for (int i = 0; i < (maxWaitTime / ((double)scanInterval)); i++)
            {
                if (StopFlag) { return false; }

                watch.Restart();
                ReadWindow();
                bool[,] objectPixels = ColorFilter(stationaryObject);
                Blob objectBlob = ImageProcessing.BiggestBlob(objectPixels);

                if (objectBlob != null && objectBlob.Size > 100)
                {
                    if (Geometry.DistanceBetweenPoints(objectBlob.Center, lastPosition) <= tolerance)
                    {
                        objectLocation = objectBlob.Center;
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

                watch.Stop();
                SafeWait(500);
            }

            return false;
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
        /// Tries to log in. Make sure the client window has focus before calling this method.
        /// </summary>
        /// <returns>true if login is successful, false if login fails</returns>
        private bool LogIn()
        {
            int center = ScreenWidth / 2;

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
                LeftClick(center, 337);
            }
            else
            {
                failedLoginAttempts++;
                return false;
            }

            //verify the log in
            if (ConfirmLogin())
            {
                failedLoginAttempts = 0;
                DefaultCamera();
                return true;
            }
            else
            {
                failedLoginAttempts++;
                return false;
            }
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
            bool[,] clickHere = ColorFilterPiece(red, centerX - offsetX, centerX + offsetX, centerY - offsetY, centerY + offsetY);
            redBlobSize = ImageProcessing.BiggestBlob(clickHere).Size;

            return ((2 * redBlobSize) > totalSize);
        }

        /// <summary>
        /// Determines if the welcome screen has been reached
        /// </summary>
        /// <returns>true if the welcome screen has been reached, false if not or if the StopFlag is raised</returns>
        private bool ConfirmWelcomeScreen()
        {
            //Wait up to 60 seconds
            for (int i = 0; i < 60; i++)
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
            long expectedColumnSum = 133405;
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
            int checkRow = Math.Min(height, ScreenScraper.LOGIN_WINDOW_HEIGHT + 50);    //50 pixels below where the bottom of the login picture should be
            int xOffset = (ScreenScraper.LOGIN_WINDOW_WIDTH / 2) + 50;
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
            if ((colorSum < (LOGIN_LOGO_COLOR_SUM * 0.99)) || (colorSum > (LOGIN_LOGO_COLOR_SUM * 1.01)))
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
            LeftClick(x, y, 200, 3);
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