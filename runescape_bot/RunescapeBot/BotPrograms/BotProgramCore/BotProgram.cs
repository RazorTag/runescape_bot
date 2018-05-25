using RunescapeBot.Common;
using RunescapeBot.ImageTools;
using RunescapeBot.UITools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using RunescapeBot.FileIO;
using RunescapeBot.BotPrograms.Popups;

namespace RunescapeBot.BotPrograms
{
    /// <summary>
    /// Used by the bot to inform that is has completed its task before stopping the execution thread
    /// </summary>
    public delegate void BotResponse();

    /// <summary>
    /// Base class for bot programs that handles starting and stopping of bot programs
    /// Implement Run in a derived class to tell a bot program what to do
    /// </summary>
    public class BotProgram
    {
        #region properties

        private const long LOGIN_LOGO_COLOR_SUM = 15456063;

        /// <summary>
        /// Used to provide the current state of the bot for the start menu
        /// </summary>
        public enum BotState
        {
            Running,
            Break,
            Sleep
        }

        /// <summary>
        /// Specifies how the bot should be run
        /// </summary>
        public RunParams RunParams { get; set; }

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
                if (Minimap != null)
                {
                    Minimap.SetScreen(colorArray);
                }
            }
        }

        /// <summary>
        /// The time that the last screenshot was taken
        /// </summary>
        protected DateTime LastScreenShot { get; set; }

        /// <summary>
        /// The last time that we checked if we are in a bot world
        /// </summary>
        protected DateTime LastBotWorldCheck { get; set; }

        /// <summary>
        /// The sidebar including the inventory and spellbook
        /// </summary>
        protected Inventory Inventory { get; set; }


        protected MinimapGauge Minimap { get; set; }

        /// <summary>
        /// Expected time to complete a single iteration
        /// </summary>
        protected int SingleMakeTime;

        /// <summary>
        /// Number of iterations in a single execution
        /// </summary>
        protected int MakeQuantity;

        /// <summary>
        /// Stock random number generator
        /// </summary>
        protected Random RNG { get; set; }

        /// <summary>
        /// Tells anyone listening to stop at their earliest convenience
        /// </summary>
        public static bool StopFlag { get; set; }

        /// <summary>
        /// Set to true to logout out of the game before stopping the bot
        /// </summary>
        public bool LogoutWhenDone { get; set; }

        /// <summary>
        /// Set to true after the bot stops naturally
        /// </summary>
        public bool BotIsDone { get; set; }

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
        /// The width in pixels of the most recent image of the game screen
        /// </summary>
        protected int ScreenWidth
        {
            get
            {
                if (ColorArray != null)
                {
                    return ColorArray.GetLength(0);
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// The width in pixels of the most recent image of the game screen
        /// </summary>
        protected int ScreenHeight
        {
            get
            {
                if (ColorArray != null)
                {
                    return ColorArray.GetLength(1);
                }
                else
                {
                    return 0;
                }
            }
        }

        //Banking
        protected const int WAIT_FOR_BANK_WINDOW_TIMEOUT = 5000;
        protected const int WAIT_FOR_BANK_LOCATION = 8000;
        protected List<BankLocator> PossibleBankTypes;
        protected BankLocator BankBoothLocator;

        //Eating
        protected const int EAT_TIME = 3 * BotRegistry.GAME_TICK;
        protected Queue<int> FoodSlots;

        #endregion

        #region core bot process
        /// <summary>
        /// Initializes a bot program with a client matching startParams
        /// </summary>
        /// <param name="startParams">specifies how to run the bot</param>
        protected BotProgram(RunParams startParams)
        {
            RSClient = ScreenScraper.GetClient();
            RunParams = startParams;
            RNG = new Random();
            Keyboard = new Keyboard(RSClient);
            Inventory = new Inventory(RSClient, Keyboard);
            Minimap = new MinimapGauge(RSClient, Keyboard);
            RunParams.ClientType = ScreenScraper.Client.Jagex;
            RunParams.DefaultCameraPosition = RunParams.CameraPosition.NorthAerial;
            RunParams.LoginWorld = 0;

            PossibleBankTypes = new List<BankLocator>();
            PossibleBankTypes.Add(LocateBankBoothVarrock);
            PossibleBankTypes.Add(LocateBankBoothSeersVillage);
            PossibleBankTypes.Add(LocateBankBoothPhasmatys);
        }
       
        /// <summary>
        /// Begins execution of the bot program. Fails if a bot program is already running for the selected process.
        /// </summary>
        public void Start()
        {
            StopFlag = false;
            BotIsDone = false;
            LastBotWorldCheck = DateTime.Now;
            RunThread = new Thread(Process);
            RunThread.Start();
        }

        /// <summary>
        /// Handles the sequential calling of the methods used to do bot work
        /// </summary>
        private void Process()
        {
            if (Debugger.IsAttached)
            {
                BotProcess();
            }
            else
            {
                try
                {
                    BotProcess();
                }
                catch (Exception e)
                {
                    LogError.SimpleLog(e);  //log an error raised during a bot's execution
                    MessageBox.Show("See " + LogError.FilePath + " for details.", "Critical Error");
                    throw e;
                }
            }
        }

        /// <summary>
        /// Handles the start, running, and end of a bot
        /// </summary>
        private void BotProcess()
        {
            //don't limit by iterations unless the user has specified a positive number of iterations
            if (RunParams.Iterations <= 0)
            {
                RunParams.InfiniteIterations = true;
            }

            //don't limit by run until time unless the user has specified a future date/time
            if ((RunParams.RunUntil - DateTime.Now).TotalMilliseconds <= 0)
            {
                RunParams.RunUntil = DateTime.MaxValue;
            }

            ManageBot();
            Done();
        }

        /// <summary>
        /// Handles break, sleep, and rotation cycles
        /// </summary>
        protected virtual void ManageBot()
        {
            MakeSureWindowHasBeenRead();
            if ((RunParams.RunLoggedIn && !CheckLogIn()) || !Run())
            {
                return;
            }

            if (RunParams.SlaveDriver)
            {
                while (!StopFlag && !Iterate())
                {
                    continue;
                }
                return;
            }

            int awakeTime = UnitConversions.HoursToMilliseconds(10);
            Stopwatch sleepWatch = new Stopwatch();
            bool done = false;
            sleepWatch.Start();

            //alternate between work periods (Iterate) and break periods
            while (!(done = Iterate()))
            {
                if (sleepWatch.ElapsedMilliseconds < awakeTime) //rest before another work cycle
                {
                    Logout();
                    done = Break();
                }
                else  //get a good night's sleep before resuming
                {
                    Logout();
                    done = Sleep();
                    sleepWatch.Restart();
                }
            }
        }

        /// <summary>
        /// Takes a break from executing the bot
        /// </summary>
        /// <returns>true if execution should stop</returns>
        private bool Break()
        {
            RunParams.BotState = BotState.Break;
            long breakLength = RandomBreakTime();
            RunParams.SetNewState(breakLength);
            RunParams.BotIdle = true;
            bool quit = SafeWait(breakLength);
            RunParams.BotIdle = false;
            return quit || (DateTime.Now >= RunParams.RunUntil);
        }

        /// <summary>
        /// Pretends to sleep for the night
        /// </summary>
        /// <returns>true if execution should stop</returns>
        private bool Sleep()
        {
            RunParams.BotState = BotState.Sleep;
            long sleepLength = RandomSleepTime();
            RunParams.SetNewState(sleepLength);
            RunParams.BotIdle = true;
            bool quit = SafeWait(sleepLength);
            RunParams.BotIdle = false;
            return quit || (DateTime.Now >= RunParams.RunUntil);
        }

        /// <summary>
        /// Contains the logic that determines what an implemented bot program does
        /// Executes before beginning iteration
        /// <param name="timeout">length of time after which the bot program should quit</param>
        /// </summary>
        /// <returns>false if execution should stop</returns>
        protected virtual bool Run()
        {
            return !StopFlag;
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
            
            //randomize the time between executions
            if (RunParams.RandomizeFrames)
            {
                randomFrameOffset = (int) (0.6 * RunParams.FrameTime);
            }
            else
            {
                randomFrameOffset = 0;
            }

            RunParams.BotState = BotState.Running;
            long workInterval = RunParams.SlaveDriver ? (long)(RunParams.RunUntil - DateTime.Now).TotalMilliseconds : RandomWorkTime();
            RunParams.SetNewState(workInterval);
            ScreenScraper.BringToForeGround(RSClient);
            SafeWait(1000); //give the client time to show up on screen

            Stopwatch iterationWatch = new Stopwatch();
            Stopwatch workIntervalWatch = new Stopwatch();
            workIntervalWatch.Start();

            while ((DateTime.Now < RunParams.RunUntil) && ((RunParams.Iterations > 0) || (RunParams.InfiniteIterations == true)))
            {
                if (StopFlag) { return true; }   //quit immediately if the stop flag has been raised or we can't log back in
                iterationWatch.Restart();
                if (!ReadWindow() || BotWorldCheck()) { continue; }   //We had to switch out of a bot world

                //Only do the actual botting if we are logged in
                if (CheckLogIn())
                {
                    if (Bitmap != null) //Make sure the read is successful before using the bitmap values
                    {
                        ReadWindow();

                        if (RunParams.AutoEat)
                        {
                            ManageHitpoints(false);
                        }

                        if (RunParams.Run)
                        {
                            Minimap.RunCharacter(0.5, false); //Turn on run if the player has run energy
                        }
                        
                        if (!Execute() && !StopFlag) //quit by a bot program
                        {
                            LogError.ScreenShot(ColorArray, "bot-quit");
                            return true;
                        }
                        if (StopFlag) { return true; }
                    }
                }
                else
                {
                    if (StopFlag) { return true; }
                    if (!HandleFailedLogIn())
                    {
                        return true;    //stop if we are unable to recover
                    }
                }

                randomFrameTime = RunParams.FrameTime + RNG.Next(-randomFrameOffset, randomFrameOffset + 1);
                randomFrameTime = Math.Max(0, randomFrameTime);
                if (iterationWatch.ElapsedMilliseconds < randomFrameTime)
                {
                    SafeWait(randomFrameTime - (int)iterationWatch.ElapsedMilliseconds);
                }
                if (StopFlag) { return true; }
                if (workIntervalWatch.ElapsedMilliseconds > workInterval)
                {
                    return RunParams.SlaveDriver;
                } 
            }

            return true;
        }

        /// <summary>
        /// A single iteration. Return false to stop execution.
        /// </summary>
        /// <returns>false if execution should be stopped</returns>
        protected virtual bool Execute()
        {
            return false;
        }

        /// <summary>
        /// Clean up
        /// </summary>
        private void Done()
        {
            if (LogoutWhenDone)
            {
                Logout();
            }

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
        public void Stop(bool logoutWhenDone)
        {
            LogoutWhenDone = logoutWhenDone;
            StopFlag = true;
        }

        /// <summary>
        /// Generates a number of milliseconds for the bot to run before logging out and resting
        /// </summary>
        /// <returns>the next work time in milliseconds</returns>
        public int RandomWorkTime()
        {
            double workType = RNG.NextDouble();
            double mean, stdDev;   //measured in minutes

            //average of 92.65 minutes
            if (workType < 0.25)   //25%
            {
                mean = 45;
                stdDev = 8;
            }
            else if (workType < 0.65) //40%
            {
                mean = 92; 
                stdDev = 5;
            }
            else if (workType < 0.95) //30%
            {
                mean = 117;
                stdDev = 30;
            }
            else  //5%
            {
                mean = 190;
                stdDev = 56;
            }

            double workTime = Probability.BoundedGaussian(mean, stdDev, 1, 345);
            workTime = Math.Min(workTime, (RunParams.RunUntil - DateTime.Now).TotalMilliseconds);
            return UnitConversions.MinutesToMilliseconds(workTime);
        }

        /// <summary>
        /// Generates a random number of milliseconds for the bot to take a break
        /// </summary>
        /// <returns>the number of milliseconds for the bot to rest</returns>
        private int RandomBreakTime()
        {
            double breakType = RNG.NextDouble();
            double mean, stdDev;   //measured in minutes

            //average of 25.1 minutes
            if (breakType < 0.75)   //75%
            {
                mean = 15;
                stdDev = 5.5;
            }
            else if (breakType < 0.90)  //15%
            {
                mean = 33;
                stdDev = 8.7;
            }
            else  //10%
            {
                mean = 89;
                stdDev = 41;
            }

            int minBreakTime = 2;
            int breakLength = (int) Probability.BoundedGaussian(mean, stdDev, minBreakTime, double.MaxValue);
            return UnitConversions.MinutesToMilliseconds(breakLength);
        }

        /// <summary>
        /// Generates a random number of milliseconds for the bot to sleep at night
        /// </summary>
        /// <returns>the number of milliseconds for the bot to sleep</returns>
        private int RandomSleepTime()
        {
            double avgSleepLength = UnitConversions.HoursToMilliseconds(14);
            double standardDeviation = UnitConversions.MinutesToMilliseconds(30);
            int breakLength = (int) Probability.RandomGaussian(avgSleepLength, standardDeviation);
            return breakLength;
        }

        #endregion

        #region user actions

        /// <summary>
        /// Moves the mouse to another position
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="randomize">radius within which to pick a random position</param>
        protected void MoveMouse(int x, int y, int randomize = 0, double stdRatio = 0.35)
        {
            if (!StopFlag)
            {
                randomize = (int)((ScreenHeight / 1000.0) * randomize);
                Point moveLocation = Probability.GaussianCircle(new Point(x, y), stdRatio * randomize, 0, 360, randomize);
                Mouse.Move(moveLocation.X, moveLocation.Y, RSClient);
            }
        }

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
                randomize = (int)((ScreenHeight / 1000.0) * randomize);
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
                randomize = (int)((ScreenHeight / 1000.0) * randomize);
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
        protected bool ClickStationaryObject(RGBHSBRange stationaryObject, double tolerance, int afterClickWait, int maxWaitTime, int minimumSize)
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
        /// <param name="foundObject">returns the Blob if it is found or null if not found</param>
        /// <param name="tolerance">maximum allowed distance in pixels between subsequent object locations</param>
        /// <param name="maxWaitTime">time to wait before gving up</param>
        /// <param name="minimumSize">minimum required size of the object in pixels</param>
        /// <param name="findObject">custom method to locate the object</param>
        /// <param name="verificationPasses">number of times to verify the position of the object after finding it</param>
        /// <returns>True if the object is found</returns>
        protected bool LocateStationaryObject(RGBHSBRange stationaryObject, out Blob foundObject, double tolerance, int maxWaitTime, int minimumSize = 1, int maximumSize = int.MaxValue, FindObject findObject = null, int verificationPasses = 1)
        {
            findObject = findObject ?? LocateObject;

            foundObject = null;
            Point? lastPosition = null;
            int effectivePasses = 0;
            int totalPasses = 0;
            Stopwatch giveUpWatch = new Stopwatch();
            giveUpWatch.Start();

            while (giveUpWatch.ElapsedMilliseconds < maxWaitTime || totalPasses <= verificationPasses)
            {
                if (StopFlag) { return false; }

                Blob objectBlob = null;
                findObject(stationaryObject, out objectBlob, minimumSize, maximumSize);

                if (objectBlob != null)
                {
                    if (Geometry.DistanceBetweenPoints(objectBlob.Center, lastPosition) <= tolerance)
                    {
                        effectivePasses++;
                    }
                    else
                    {
                        effectivePasses = 0;
                        lastPosition = objectBlob.Center;
                    }

                    if (effectivePasses >= verificationPasses)
                    {
                        foundObject = objectBlob;
                        return true;
                    }
                }
                else
                {
                    lastPosition = null;
                }
                totalPasses++;
            }

            return false;
        }
        protected delegate bool FindObject(RGBHSBRange stationaryObject, out Blob foundObject, int minimumSize = 1, int maximumSize = int.MaxValue);

        /// <summary>
        /// Locates all of the matching objects on the game screen (minus UI) that fit within the given size constraints
        /// </summary>
        /// <param name="objectFilter">color filter for the object type to search for</param>
        /// <param name="minSize">minimum required pixels</param>
        /// <param name="maxSize">maximum allowed pixels</param>
        /// <returns></returns>
        protected List<Blob> LocateObjects(RGBHSBRange objectFilter, int minimumSize = 1, int maximumSize = int.MaxValue)
        {
            ReadWindow();
            bool[,] objectPixels = ColorFilter(objectFilter);
            EraseClientUIFromMask(ref objectPixels);
            List<Blob> objects = ImageProcessing.FindBlobs(objectPixels, true, minimumSize, maximumSize);
            return objects;
        }

        /// <summary>
        /// Locates all of the matching objects on the game screen (minus UI) that fit within the given size constraints
        /// </summary>
        /// <param name="objectFilter">color filter for the object type to search for</param>
        /// <param name="left">left bound of the search area</param>
        /// <param name="right">right bound of the search area</param>
        /// <param name="top">top bound of the search area</param>
        /// <param name="bottom">bottom bound of the search area</param>
        /// <param name="shiftBlobs">when true, moves all of the found blobs to their position on the original screen rather than the sub piece being scanned</param>
        /// <param name="minimumSize">minimum required pixels</param>
        /// <param name="maximumSize">maximum allowed pixels</param>
        /// <returns></returns>
        protected List<Blob> LocateObjects(RGBHSBRange objectFilter, int left, int right, int top, int bottom, bool shiftBlobs = true, int minimumSize = 1, int maximumSize = int.MaxValue)
        {
            ReadWindow();
            bool[,] objectPixels = ColorFilterPiece(objectFilter, left, right, top, bottom);
            EraseClientUIFromMask(ref objectPixels);
            List<Blob> foundObjects = ImageProcessing.FindBlobs(objectPixels, true, minimumSize, maximumSize);
            if (shiftBlobs)
            {
                foreach (Blob foundObject in foundObjects)
                {
                    foundObject.ShiftPixels(left, top);
                }
            }
            return foundObjects;
        }

        /// <summary>
        /// Finds the object closest to the center of the screen that matches the given criteria
        /// </summary>
        /// <param name="objectFilter">color filter for the object</param>
        /// <param name="minimumSize">minimum required size for the object in pixels</param>
        /// <param name="maximumSize">maximum allowed size for the object in pixels</param>
        /// <returns>the found object or null if none is found</returns>
        protected Blob LocateClosestObject(RGBHSBRange objectFilter, int minimumSize = 1, int maximumSize = int.MaxValue)
        {
            List<Blob> objects = LocateObjects(objectFilter, minimumSize, maximumSize);
            Blob closestObject = Geometry.ClosestBlobToPoint(objects, Center);
            return closestObject;
        }

        /// <summary>
        /// Finds the object closest to the center of the screen that matches the given criteria
        /// </summary>
        /// <param name="objectFilter">color filter for the object</param>
        /// <param name="minimumSize">minimum required size for the object in pixels</param>
        /// <param name="maximumSize">maximum allowed size for the object in pixels</param>
        /// <returns>the found object or null if none is found</returns>
        protected bool LocateClosestObject(RGBHSBRange objectFilter, out Blob closestObject, int minimumSize = 1, int maximumSize = int.MaxValue)
        {
            List<Blob> objects = LocateObjects(objectFilter, minimumSize, maximumSize);
            closestObject = Geometry.ClosestBlobToPoint(objects, Center);
            return closestObject != null;
        }

        /// <summary>
        /// Looks for an object that matches a filter
        /// </summary>
        /// <param name="stationaryObject"></param>
        /// <param name="foundObject"></param>
        /// <param name="minimumSize"></param>
        /// <returns></returns>
        protected bool LocateObject(RGBHSBRange stationaryObject, out Blob foundObject, int minimumSize = 1, int maximumSize = int.MaxValue)
        {
            ReadWindow();
            bool[,] objectPixels = ColorFilter(stationaryObject);
            EraseClientUIFromMask(ref objectPixels);
            return LocateObject(objectPixels, out foundObject, minimumSize, maximumSize);
        }

        /// <summary>
        /// Looks for an object that matches a filter in a particular region of the screen
        /// </summary>
        /// <param name="stationaryObject"></param>
        /// <param name="foundObject"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="top"></param>
        /// <param name="bottom"></param>
        /// <param name="minimumSize"></param>
        /// <returns></returns>
        protected bool LocateObject(RGBHSBRange stationaryObject, out Blob foundObject, int left, int right, int top, int bottom, int minimumSize = 1, int maximumSize = int.MaxValue)
        {
            ReadWindow();
            bool[,] objectPixels = ColorFilterPiece(stationaryObject, left, right, top, bottom);
            if (LocateObject(objectPixels, out foundObject, minimumSize, maximumSize))
            {
                foundObject.ShiftPixels(left, top);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Finds the biggest blob in a binary image
        /// </summary>
        /// <param name="objectPixels"></param>
        /// <param name="foundObject"></param>
        /// <param name="minimumSize"></param>
        /// <returns></returns>
        protected bool LocateObject(bool[,] objectPixels, out Blob foundObject, int minimumSize = 1, int maximumSize = int.MaxValue)
        {
            foundObject = null;
            List<Blob> objectBlobs = ImageProcessing.FindBlobs(objectPixels, true, minimumSize, maximumSize);

            if (objectBlobs != null && objectBlobs.Count > 0)
            {
                foreach (Blob blob in objectBlobs)
                {
                    if (blob.Size < minimumSize)
                    {
                        return false;
                    }
                    if (blob.Size <= maximumSize)
                    {
                        foundObject = blob;
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Waits for a specified type of mouseover text to appear
        /// </summary>
        /// <param name="textColor">the color of the mousover text to wait for</param>
        /// <param name="timeout">time to wait before giving up</param>
        /// <returns>true if the specified type of mouseover text is found</returns>
        protected bool WaitForMouseOverText(RGBHSBRange textColor, int timeout = 5000)
        {
            const int left = 5;
            const int right = 500;
            const int top = 5;
            const int bottom = 18;

            Stopwatch watch = new Stopwatch();
            watch.Start();
            Blob mouseoverText = null;

            do
            {
                if (LocateObject(textColor, out mouseoverText, left, right, top, bottom, 10))
                {
                    return true;
                }
            }
            while ((watch.ElapsedMilliseconds < timeout) && !StopFlag);

            return false;
        }

        /// <summary>
        /// Mouses over an alleged stationary object. Left-clicks if the stationary object text appears.
        /// </summary>
        /// <param name="stationaryObject">alleged stationary object</param>
        /// <param name="click">set to false to skip clicking on the stationary object</param>
        /// <param name="randomization">maximum number of pixels from the center of the blob that it is safe to click</param>
        /// <returns>true if a matching stationary object is found and clicked on</returns>
        protected bool MouseOverStationaryObject(Blob stationaryObject, bool click = true, int randomization = 5, int maxWait = 1000)
        {
            return MouseOver(stationaryObject.Center, RGBHSBRangeFactory.MouseoverTextStationaryObject(), click, randomization, maxWait);
        }

        /// <summary>
        /// Mouses over an alleged NPC. Left-clicks if the NPC text appears.
        /// </summary>
        /// <param name="ObjectsToCheck">alleged NPC</param
        /// <param name="click">set to false to skip clicking on the NPC</param>
        /// <param name="randomization">maximum number of pixels from the center of the blob that it is safe to click</param>
        /// <returns>true if a matching NPC is found and clicked on</returns>
        protected bool MouseOverNPC(Blob npc, bool click = true, int randomization = 5, int maxWait = 1000)
        {
            return MouseOver(npc.Center, RGBHSBRangeFactory.MouseoverTextNPC(), click, randomization, maxWait);
        }

        /// <summary>
        /// Mouses over an alleged NPC. Left-clicks if the NPC text appears.
        /// </summary>
        /// <param name="ObjectsToCheck">alleged NPC</param
        /// <param name="click">set to false to skip clicking on the NPC</param>
        /// <param name="randomization">maximum number of pixels from the center of the blob that it is safe to click</param>
        /// <returns>true if a matching NPC is found and clicked on</returns>
        protected bool MouseOverDroppedItem(Blob item, bool click = true, int randomization = 5, int maxWait = 1000)
        {
            return MouseOver(item.Center, RGBHSBRangeFactory.MouseoverTextDroppedItem(), click, randomization, maxWait);
        }

        /// <summary>
        /// Mouses over each object in a list of blobs. Left-clicks the first object with mouseover text matching textColor.
        /// </summary>
        /// <param name="ObjectsToCheck">list of objects to mouse over</param>
        /// <param name="textColor">color of text expected to be in the top-left on mouseover</param>
        /// <param name="randomization">maximum number of pixels from the center of the blob that it is safe to click</param>
        /// <returns>true if a matching object is found and clicked on</returns>
        protected bool MouseOver(List<Blob> ObjectsToCheck, RGBHSBRange textColor, bool click = true, int randomization = 5, int maxWait = 1000)
        {
            randomization = (int)((ScreenHeight / 1000.0) * randomization);
            Point clickLocation;

            foreach (Blob objectCheck in ObjectsToCheck)
            {
                if (StopFlag) { return false; }

                clickLocation = objectCheck.Center;

                if (MouseOver(clickLocation, textColor, click, randomization, maxWait))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Mouses over a single point and left-clicks it if it matches the specified mouseover text color.
        /// </summary>
        /// <param name="mouseover">point to mouse over</param>
        /// <param name="textColor">color of text expected to be in the top-left on mouseover</param>
        /// <param name="click">set to false to hover and check without clicking</param>
        /// <param name="randomization">maximum number of pixels from the center of the blob that it is safe to click</param>
        /// <param name="maxWait">max time to wait before concluding that the mouse over failed</param>
        /// <param name="lagTime">time to wait before checking and clicking after mousing over</param>
        /// <returns>true if a matching object is found and clicked on</returns>
        protected bool MouseOver(Point mouseover, RGBHSBRange textColor, bool click = true, int randomization = 5, int maxWait = 1000, int lagTime = 100)
        {
            randomization = (int)((ScreenHeight / 1000.0) * randomization);
            mouseover = Probability.GaussianCircle(mouseover, randomization);
            Mouse.Move(mouseover.X, mouseover.Y, RSClient);
            if (SafeWait(lagTime)) { return false; }

            if (WaitForMouseOverText(textColor, maxWait))
            {
                if (click) { LeftClick(mouseover.X, mouseover.Y, 0, 0); }
                return true;
            }
            return false;
        }

        #endregion

        #region vision utilities
        /// <summary>
        /// Wrapper for ScreenScraper.CaptureWindow
        /// </summary>
        protected bool ReadWindow(bool checkClient = true, bool fastCapture = false)
        {
            if (checkClient && !PrepareClient()) { return false; }

            if (Bitmap != null)
            {
                Bitmap.Dispose();
            }

            try
            {
                LastScreenShot = DateTime.Now;
                Bitmap = ScreenScraper.CaptureWindow(RSClient, fastCapture);
                ColorArray = ScreenScraper.GetRGB(Bitmap);
            }
            catch
            {
                return false;
            }   

            return (Bitmap != null) && (ScreenHeight > 0) && (ScreenWidth > 0);
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
            if (x < 0 || x >= ScreenWidth || y < 0 || y >= ScreenHeight)
            {
                return Color.Black;
            }
            return ColorArray[x, y];
        }

        /// <summary>
        /// Creates a boolean array to represent a color filter match
        /// </summary>
        /// <param name="artifactColor"></param>
        /// <returns></returns>
        protected bool[,] ColorFilter(RGBHSBRange artifactColor)
        {
            return ColorFilter(ColorArray, artifactColor);
        }

        /// <summary>
        /// Creates a boolean array to represent a color filter match
        /// </summary>
        /// <param name="artifactColor"></param>
        /// <returns></returns>
        protected bool[,] ColorFilter(Color[,] image, RGBHSBRange artifactColor)
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
        protected bool[,] ColorFilterPiece(RGBHSBRange filter, int left, int right, int top, int bottom, out Point trimOffset)
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
        protected bool[,] ColorFilterPiece(RGBHSBRange filter, int left, int right, int top, int bottom)
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
        protected bool[,] ColorFilterPiece(RGBHSBRange filter, Point center, int radius, out Point offset)
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
        protected bool[,] ColorFilterPiece(RGBHSBRange filter, Point center, int radius)
        {
            Point offset;
            return ColorFilterPiece(filter, center, radius, out offset);
        }

        /// <summary>
        /// Determines the fraction of piece of an RGB image that matches a color filter
        /// </summary>
        /// <param name="filter">filter to use for matching</param>
        /// <param name="left">left bound (inclusive)</param>
        /// <param name="right">right bound (inclusive)</param>
        /// <param name="top">top bound (inclusive)</param>
        /// <param name="bottom">bottom bound (inclusive)</param>
        /// <returns>The fraction (0-1) of the image that matches the filter</returns>
        protected double FractionalMatchPiece(RGBHSBRange filter, int left, int right, int top, int bottom)
        {
            bool[,] binaryImage = ColorFilterPiece(filter, left, right, top, bottom);
            return ImageProcessing.FractionalMatch(binaryImage);
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
        /// Determines the pixels on the screen taken up by an artifact of a known fraction of the screen
        /// </summary>
        /// <param name="artifactSize">the size of an artifact in terms of fraction of the square of the screen height</param>
        /// <returns>the number of pixels taken up by an artifact</returns>
        protected int ArtifactArea(double artifactSize)
        {
            if (!MakeSureWindowHasBeenRead()) { return 0; }
            double pixels = artifactSize * ScreenHeight * ScreenHeight;
            return (int) Math.Round(pixels);
        }

        /// <summary>
        /// Determines the pixel length of an artifact of a known fraction of the screen's height
        /// </summary>
        /// <param name="artifactLength">the fraction of the screen height taken up by the artifact</param>
        /// <returns>the pixel length of the artifact</returns>
        protected int ArtifactLength(double artifactLength)
        {
            if (!MakeSureWindowHasBeenRead()) { return 0; }
            double length = artifactLength * ScreenHeight;
            return (int)Math.Round(length);
        }

        /// <summary>
        /// Sets the pixels in client UI areas to false.
        /// This should only be used with untrimmed images.
        /// </summary>
        /// <param name="mask"></param>
        protected void EraseClientUIFromMask(ref bool[,] mask)
        {
            if (mask == null) { return; }

            const int chatBoxWidth = 518;
            const int chatBoxHeight = 158;
            const int inventoryWidth = 240;
            const int inventoryHeight = 335;
            const int minimapWidth = 210;
            const int minimapHeight = 192;

            int width = mask.GetLength(0);
            int height = mask.GetLength(1);
            int requiredWidth = Math.Max(Math.Max(chatBoxWidth, inventoryWidth), minimapWidth);
            int requiredHeight = Math.Max(Math.Max(chatBoxHeight, inventoryHeight), minimapHeight);
            if ((width < requiredWidth) || (height < requiredHeight)) { return; }

            if (!RunParams.ClosedChatBox)   //do not erase chat box if the chat box is supposed to be closed
            {
                EraseFromMask(ref mask, 0, chatBoxWidth, height - chatBoxHeight, height);              //erase chat box
            }
            EraseFromMask(ref mask, width - inventoryWidth, width, height - inventoryHeight, height);  //erase inventory
            EraseFromMask(ref mask, width - minimapWidth, width, 0, minimapHeight);                //erase minimap
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
        /// Makes sure that a client is running and starts it if it isn't
        /// </summary>
        /// <param name="forceRestart">Set to true to force a client restart even if the client is already running</param>
        /// <returns>true if client is successfully prepared</returns>
        protected bool PrepareClient(bool forceRestart = false)
        {
            if (!forceRestart && ScreenScraper.ProcessExists(RSClient)) { return true; }

            Stopwatch longWatch = new Stopwatch();
            longWatch.Start();
            while (longWatch.ElapsedMilliseconds < UnitConversions.HoursToMilliseconds(24) && !StopFlag)
            {
                if (!ScreenScraper.RestartClient(ref client, RunParams.RuneScapeClient, RunParams.ClientFlags))
                {
                    SafeWait(5000);
                    continue;
                }
                RSClient = client;

                Stopwatch watch = new Stopwatch();
                watch.Start();
                do
                {
                    SafeWait(UnitConversions.SecondsToMilliseconds(5));
                    if (ReadWindow(false) && (IsLoggedOut(false) || IsLoggedIn()))
                    {
                        BroadcastConnection();
                        return true;
                    }
                }
                while ((watch.ElapsedMilliseconds < UnitConversions.MinutesToMilliseconds(5)) && !StopFlag);

                BroadcastDisconnect();
            }

            if (!StopFlag)
            {
                BroadcastFailure();
                const string errorMessage = "Client did not start correctly";
                MessageBox.Show(errorMessage);
            }
            
            client = null;
            return false;
        }

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
        protected virtual bool CheckLogIn()
        {
            if (!IsLoggedOut())
            {
                return true;    //already logged in
            }
            if (SafeWait(2000)) { return false; }
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
        /// Finds the offset for the login screen
        /// </summary>
        /// <returns></returns>
        private Point LoginScreenOffset()
        {
            int yOffset = 0;
            while (ImageProcessing.ColorsAreEqual(GetPixel(Center.X, yOffset), Color.Black) && (yOffset < ScreenHeight))
            {
                yOffset++;
            }
            return new Point(0, yOffset);
        }

        /// <summary>
        /// Opens the world selector on the login screen
        /// </summary>
        /// <returns>true if successful</returns>
        protected bool OpenWorldSelector(Point loginOffset)
        {
            int x = loginOffset.X + (Center.X - 326);
            int y = loginOffset.Y + 481;
            LeftClick(x, y);
            SafeWait(500);
            return true;
        }

        /// <summary>
        /// Switch to a world on the login screen
        /// </summary>
        /// <returns>true if successful</returns>
        private bool SelectLoginWorld(int world, Point? loginOffset)
        {
            const int rowCount = 18;
            const int lowestWorld = 301;
            const int highestWorld = 394;
            const int rowHeight = 24;
            const int columnWidth = 94;

            if (world < lowestWorld || world > highestWorld)
            {
                RunParams.LoginWorld = 0;
                return true;
            }

            Point offset = (loginOffset ?? LoginScreenOffset());
            if (!OpenWorldSelector(offset)) { return false; }
            SafeWait(2000);
            int worldIndex = world - lowestWorld;

            //adjust for missing worlds
            if (world > 362) { worldIndex -= 2; }
            if (world > 370) { worldIndex -= 2; }
            if (world > 378) { worldIndex -= 2; }

            int column = worldIndex / rowCount;
            int row = worldIndex % rowCount;
            int x = offset.X + (Center.X - 182) + (column * columnWidth);
            int y = offset.Y + 46 + (row * rowHeight);
            LeftClick(x, y, 5);
            SafeWait(500);
            LastBotWorldCheck = DateTime.Now;
            return true;
        }

        /// <summary>
        /// Tries to log in.
        /// </summary>
        /// <returns>true if login is successful, false if login fails</returns>
        protected bool LogIn()
        {
            Point? clickLocation;
            Point loginOffset = LoginScreenOffset();

            //log in at the login screen
            if (!IsWelcomeScreen(out clickLocation))
            {
                if (!SelectLoginWorld(RunParams.LoginWorld, loginOffset))
                {
                    return false;
                }

                //Click existing account. Clicks in a dead space if we are already on the login screen.
                LeftClick(Center.X + 16 + loginOffset.X, 288 + loginOffset.Y);

                if (RunParams.LoginWorld > 0)
                {
                    LeftClick(Center.X - 78 + loginOffset.X, 320 + loginOffset.Y);
                    SafeWait(500);
                }

                //fill in login
                LeftClick(Center.X + 137 + loginOffset.X, 249 + loginOffset.Y);
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

            if (!PvPWorldSet())
            {
                //click the "CLICK HERE TO PLAY" button on the welcome screen
                if (ConfirmWelcomeScreen(out clickLocation))
                {
                    LeftClick(clickLocation.Value.X, clickLocation.Value.Y);
                }
                else
                {
                    return false;
                }
            }

            //verify the log in
            if (ConfirmLogin())
            {
                BroadcastLogin();
                DefaultCamera();
                ChatBox();
                return true;
            }
            else
            {
                BroadcastLogout();
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

            RGBHSBRange red = RGBHSBRangeFactory.WelcomeScreenClickHere();
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
        /// Determines if RunParams specifies a PvP world to log into
        /// </summary>
        /// <returns></returns>
        protected bool PvPWorldSet()
        {
            int world = RunParams.LoginWorld;
            return (world == 325) || (world == 337) || (world == 392);
        }

        /// <summary>
        /// Determines if  the client is logged in
        /// </summary>
        /// <returns>true if we are verifiably logged in</returns>
        private bool IsLoggedIn(bool readWindow = true)
        {
            if (readWindow) { ReadWindow(); }

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
        protected bool IsLoggedOut(bool readWindow = false)
        {
            if (readWindow && !ReadWindow()) { return false; }

            Color color;
            Point loginOffset = LoginScreenOffset();
            int height = ScreenHeight;
            int top = loginOffset.Y;
            int centerX = Center.X + loginOffset.X;
            int checkRow = Math.Min(Math.Max(0, height - 1), loginOffset.Y + ScreenScraper.LOGIN_WINDOW_HEIGHT + 1);    //1 pixel below where the bottom of the login window should be
            int xOffset = (ScreenScraper.LOGIN_WINDOW_WIDTH / 2) + 2;
            int blackPixels = 0;
            int totalPixels = 0;

            for (int x = centerX - xOffset; x < centerX + xOffset; x++)
            {
                //check bottom of login box
                color = ColorArray[x, checkRow];
                blackPixels += ImageProcessing.ColorsAreEqual(color, Color.Black) ? 1 : 0;
                totalPixels++;
            }
            for (int y = top; y < checkRow; y++)  //check sides
            {
                //check left of login box
                color = ColorArray[centerX - xOffset, y];
                blackPixels += ImageProcessing.ColorsAreEqual(color, Color.Black) ? 1 : 0;
                totalPixels++;

                //check right of login box
                color = ColorArray[centerX + xOffset, y];
                blackPixels += ImageProcessing.ColorsAreEqual(color, Color.Black) ? 1 : 0;
                totalPixels++;
            }
            //assume we are logged out if a majority off the border pixels are perfectly black
            if ((blackPixels / ((double)totalPixels)) < 0.25)
            {
                return false;
            }

            //check for "Welcome to RuneScape" yellow text
            int topWelcome = top + 241;
            int bottomWelcome = topWelcome + 13;
            int leftWelcome = Center.X - 75;
            int rightWelcome = leftWelcome + 146;
            bool[,] welcomeText = ColorFilterPiece(RGBHSBRangeFactory.Yellow(), leftWelcome, rightWelcome, topWelcome, bottomWelcome);
            double welcomeMatch = ImageProcessing.FractionalMatch(welcomeText);
            if (welcomeMatch > 0.2)
            {
                return true;
            }

            //color-based hash of the RUNE SCAPE logo on the login screen to verify that it is there
            int topLogo = top;
            int leftLogo = Center.X - 224 + loginOffset.X;
            int rightLogo = leftLogo + 444;
            int bottomLogo = topLogo + 160;
            long colorSum = ImageProcessing.ColorSum(ScreenPiece(leftLogo, rightLogo, topLogo, bottomLogo));
            return Numerical.CloseEnough(LOGIN_LOGO_COLOR_SUM, colorSum, 0.01) || Numerical.CloseEnough(LOGIN_LOGO_ALT, colorSum, 0.01);
        }
        const int LOGIN_LOGO_ALT = 15821488;

        /// <summary>
        /// Logs out of the game
        /// </summary>
        protected void Logout()
        {
            const int maxLogoutAttempts = 10;
            int logoutAttempts = 0;

            while (!IsLoggedOut(true) && (logoutAttempts++ < maxLogoutAttempts) && !StopFlag)
            {
                Inventory.OpenLogout();
                SafeWait(800, 200);
                LeftClick(ScreenWidth - 38, ScreenHeight - 286);    //close out of world switcher
                SafeWait(2000, 400);
                LeftClick(ScreenWidth - 120, ScreenHeight - 71, 5); //click here to logout
                SafeWait(2000, 400);
            }

            BroadcastLogout();
        }

        /// <summary>
        /// Changes world if logged into a bot world. Can be called while logged in or logged out.
        /// </summary>
        /// <returns>true if we detect a bot world and attempt to change worlds</returns>
        protected bool BotWorldCheck(bool readWindow = false)
        {
            if (!RunParams.BotWorldCheckEnabled || PvPWorldSet())
            {
                return false;
            }

            //restart client if set to a bot world
            int loginWorld = (RunParams.LoginWorld > 0) ? RunParams.LoginWorld : 340;
            if (IsLoggedOut(readWindow))
            {
                if (LoginSetForBotWorld(false))
                {
                    SelectLoginWorld(loginWorld, null);
                    return true;
                }
                LastBotWorldCheck = DateTime.Now;
            }
            //Only check for a bot world while logged in if we haven't checked for a while.
            else if ((long)(DateTime.Now - LastBotWorldCheck).TotalMilliseconds > RunParams.BotWorldCheckInterval)
            {
                if (LoggedIntoBotWorld(false))
                {
                    Logout();
                    SelectLoginWorld(loginWorld, null);
                    return true;
                }
                //update the last bot world check time only if we verify that we aren't on a bot world
                //This ensures that restarting the client to a bot world does not allow you to log into a bot world.
                LastBotWorldCheck = DateTime.Now;
            }

            return false;
        }

        /// <summary>
        /// Determines if the client is logged into world 385 (F2P) or world 386 (P2P).
        /// Also identifies worlds 358 and 368 as bot worlds.
        /// Assumes that the client is logged into the game.
        /// </summary>
        /// /// <param name="readWindow">Set to true to force a new screen read</param>
        /// <returns>true if the client is logged into world 385 or 386</returns>
        protected bool LoggedIntoBotWorld(bool readWindow = false)
        {
            MakeSureWindowHasBeenRead(readWindow);
            Inventory.OpenLogout();
            SafeWaitPlus(1000, 150);
            ReadWindow();
            if (!WorldSwitcherIsOpen())
            {
                ClickWorldSwitcher();
                SafeWaitPlus(1500, 500);
                ReadWindow();
            }

            Stopwatch watch = new Stopwatch();
            watch.Start();
            while (!WorldSwitcherIsOpen() && (watch.ElapsedMilliseconds < 3000) && !StopFlag)
            {
                ClickWorldSwitcher();
                SafeWait(600, 200);
                ReadWindow();
            }

            int left = ScreenWidth - 84;
            int right = left + 30;
            int top = ScreenHeight - 297;
            int bottom = top + 20;
            long colorSum = ImageProcessing.ColorSum(ScreenPiece(left, right, top, bottom));
            bool freeBotWorld = Numerical.CloseEnough(120452, colorSum, 0.00001);
            bool memberBotWorld = Numerical.CloseEnough(121998, colorSum, 0.00001);
            return memberBotWorld || freeBotWorld;
        }

        /// <summary>
        /// Left clicks on the world switcher option in the logout tab to open the world switcher.
        /// Assumes that the logout tab is already open.
        /// </summary>
        protected void ClickWorldSwitcher()
        {
            int left = ScreenWidth - 180;
            int right = left + 110;
            int top = ScreenHeight - 123;
            int bottom = top + 15;
            Point click = Probability.GaussianRectangle(left, right, top, bottom);
            LeftClick(click.X, click.Y);
        }

        /// <summary>
        /// Determines if the world switcher is open
        /// </summary>
        /// <returns>true if the world switcher is open</returns>
        protected bool WorldSwitcherIsOpen()
        {
            int left = ScreenWidth - 200;
            int right = left + 150;
            int top = ScreenHeight - 297;
            int bottom = top + 20;
            Color[,] currentWorldTitle = ScreenPiece(left, right, top, bottom);
            double worldTextMatch = ImageProcessing.FractionalMatch(currentWorldTitle, RGBHSBRangeFactory.BankTitle());
            const double worldTextMinimumMatch = 0.05;
            return worldTextMatch > worldTextMinimumMatch;
        }

        /// <summary>
        /// Determines if one of the two bot worlds is selected on the login screen.
        /// Assumes that the client is on the login screen.
        /// </summary>
        /// <param name="readWindow">Set to true to force a new screen read</param>
        /// <returns>true if the world is set to 385 or 386. May also hit on 358 and 368</returns>
        protected bool LoginSetForBotWorld(bool readWindow = false)
        {
            MakeSureWindowHasBeenRead(readWindow);
            Point loginOffset = LoginScreenOffset();
            int left = Center.X - 323 + loginOffset.X;
            int right = left + 30;
            int top = 466 + loginOffset.Y;
            int bottom = top + 14;
            long colorSum = ImageProcessing.ColorSum(ScreenPiece(left, right, top, bottom));
            bool freeBotWorld = Numerical.CloseEnough(LOGIN_BOT_WORLD_385, colorSum, 0.0005);
            bool memberBotWorld = Numerical.CloseEnough(LOGIN_BOT_WORLD_386, colorSum, 0.00005);
            return memberBotWorld || freeBotWorld;
        }
        private const int LOGIN_BOT_WORLD_385 = 130228;
        private const int LOGIN_BOT_WORLD_386 = 133468;

        #endregion

        #region banking

        /// <summary>
        /// Locates and opens an unknown bank type
        /// Refer to member PossibleBankBooths for a list of possible bank types
        /// </summary>
        /// <returns>true if the bank is opened</returns>
        protected bool OpenBank(out Bank bankPopup)
        {
            bankPopup = null;

            if (OpenKnownBank(out bankPopup))
            {
                return true;
            }
            return IdentifyBank() && OpenKnownBank(out bankPopup);
        }

        /// <summary>
        /// Locates and opens a nown bank type
        /// </summary>
        /// <param name="bankPopup"></param>
        /// <returns></returns>
        protected bool OpenKnownBank(out Bank bankPopup)
        {
            bankPopup = null;
            if (BankBoothLocator == null) { return false; }

            Blob bankBooth;
            if (BankBoothLocator(out bankBooth))
            {
                MouseOverStationaryObject(bankBooth, true, 10);
                bankPopup = new Bank(RSClient, Inventory);
                return bankPopup.WaitForPopup(WAIT_FOR_BANK_WINDOW_TIMEOUT);
            }
            return false;
        }

        /// <summary>
        /// Determines if any of the possible bank types appear on screen.
        /// Sets BankBoothCounter to the first match found.
        /// </summary>
        /// <returns>true if any of them do</returns>
        protected bool IdentifyBank()
        {
            Blob booth;
            ReadWindow();
            foreach (BankLocator bankLocator in PossibleBankTypes)
            {
                if (bankLocator(out booth))
                {
                    BankBoothLocator = bankLocator;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Finds the closest bank booth in the Varrock west bank
        /// </summary>
        /// <param name="bankBoothColor">not used</param>
        /// <param name="bankBooth">returns the found bank booth blob</param>
        /// <returns>true if a bank booth is found</returns>
        protected bool LocateBankBooth(RGBHSBRange bankBoothColor, out Blob bankBooth)
        {
            bankBooth = null;

            if (!ReadWindow()) { return false; }
            bool[,] bankBooths = ColorFilter(bankBoothColor);
            List<Blob> potentialBoothBlobs = ImageProcessing.FindBlobs(bankBooths, false, MinBankBoothSize, MaxBankBoothSize);
            List<Blob> booths = new List<Blob>();

            foreach(Blob potentialBooth in potentialBoothBlobs)
            {
                if (Geometry.Rectangularity(potentialBooth) > 0.8)
                {
                    booths.Add(potentialBooth);
                }
            }

            bankBooth = Blob.ClosestBlob(Center, booths);

            return bankBooth != null;
        }

        /// <summary>
        /// Delegate for custom bank locators
        /// </summary>
        /// <param name="bankBooth"></param>
        /// <returns></returns>
        protected delegate bool BankLocator(out Blob bankBooth);

        protected int MinBankBoothSize { get { return ArtifactArea(0.000582); } } //ex 0.000682
        protected int MaxBankBoothSize { get { return ArtifactArea(0.001145); } } //ex 0.001045

        /// <summary>
        /// Locates a bank booth with the counter color from the Varrock west bank
        /// </summary>
        /// <param name="bankBooth"></param>
        /// <returns>true if a bank booth is found</returns>
        protected bool LocateBankBoothVarrock(out Blob bankBooth)
        {
            return LocateBankBooth(RGBHSBRangeFactory.BankBoothVarrockWest(), out bankBooth);
        }

        /// <summary>
        /// Locates a bank booth in the Seers' Village bank
        /// </summary>
        /// <param name="bankBooth"></param>
        /// <returns>true if a bank booth is found</returns>
        protected bool LocateBankBoothSeersVillage(out Blob bankBooth)
        {
            bankBooth = null;
            List<Blob> possibleBankBooths = LocateObjects(RGBHSBRangeFactory.BankBoothSeersVillage(), MinBankBoothSize, MaxBankBoothSize);
            if (possibleBankBooths == null) { return false; }

            List<Blob> bankBooths = new List<Blob>();
            foreach (Blob booth in possibleBankBooths)
            {
                double widthToHeight = booth.Width / (double)booth.Height;
                if (Numerical.WithinBounds(widthToHeight, 2.3, 3.2))
                {
                    bankBooths.Add(booth);
                }
            }

            if (bankBooths.Count != 9)
            {
                return false;
            }
            bankBooths.Sort(new BlobHorizontalComparer());
            bankBooths.RemoveAt(5); //remove closed bank booths
            bankBooths.RemoveAt(4);
            bankBooths.RemoveAt(2);
            bankBooth = Blob.ClosestBlob(Center, bankBooths);
            return true;
        }

        /// <summary>
        /// Finds the closest bank booth in the Port Phasmatys bank
        /// </summary>
        /// <returns>True if the bank booths are found</returns>
        protected bool LocateBankBoothPhasmatys(out Blob bankBooth)
        {
            bankBooth = null;
            const int numberOfBankBooths = 6;
            const double minBoothWidthToHeightRatio = 2.3667;   //ex 2.6667
            const double maxBoothWidthToHeightRatio = 3.1333;   //ex 2.8333

            ReadWindow();
            bool[,] bankBooths = ColorFilter(RGBHSBRangeFactory.BankBoothPhasmatys());
            List<Blob> boothBlobs = new List<Blob>();
            List<Blob> possibleBoothBlobs = ImageProcessing.FindBlobs(bankBooths, false, MinBankBoothSize, MaxBankBoothSize);  //list of blobs from biggest to smallest
            possibleBoothBlobs.Sort(new BlobProximityComparer(Center));
            double widthToHeightRatio;

            //Remove blobs that aren't bank booths
            foreach(Blob possibleBooth in possibleBoothBlobs)
            {
                widthToHeightRatio = (possibleBooth.Width / (double)possibleBooth.Height);
                if (Numerical.WithinBounds(widthToHeightRatio, minBoothWidthToHeightRatio, maxBoothWidthToHeightRatio))
                {
                    boothBlobs.Add(possibleBooth);
                }
            }

            if (boothBlobs.Count != numberOfBankBooths)
            {
                return false;   //We either failed to locate all of the booths or identified something that was not actually a booth.
            }

            //Reduce the blob list to the bank booths
            boothBlobs.Sort(new BlobHorizontalComparer());  //sort from left to right
            boothBlobs.RemoveAt(3); //remove the unusable booths without tellers
            boothBlobs.RemoveAt(0);
            bankBooth = Blob.ClosestBlob(Center, boothBlobs);
            return true;
        }

        /// <summary>
        /// Finds the closest bank booth in the Port Phasmatys bank
        /// </summary>
        /// <returns>True if the bank booths are found</returns>
        protected bool LocateBankBoothPhasmatys(RGBHSBRange bankBoothColor, out Blob bankBooth, int minimumSize = 1, int maximumSize = int.MaxValue)
        {
            return LocateBankBoothPhasmatys(out bankBooth);
        }

        #endregion

        #region eating

        /// <summary>
        /// Makes a queue of inventory slots with food in them in the order that they should be eaten
        /// </summary>
        protected virtual void SetFoodSlots()
        {
            FoodSlots = new Queue<int>();
        }

        /// <summary>
        /// Consumes food if hitpoints are not high
        /// </summary>
        /// <returns>true if hitpoints are succesfully restored, false if hitpoints cannot be restored and bot should stop</returns>
        protected virtual bool ManageHitpoints(bool readWindow = false)
        {
            if (readWindow) { ReadWindow(); }

            double hitpoints = Minimap.Hitpoints();
            if (hitpoints < RunParams.StartEatingBelow)
            {
                while (hitpoints <= RunParams.StopEatingAbove)
                {
                    if (!EatNextFood())
                    {
                        return false;
                    }
                    if (SafeWait(EAT_TIME)) { return false; }
                    hitpoints = Minimap.Hitpoints();
                    ReadWindow();
                }
            }

            return true;
        }

        /// <summary>
        /// Eats the next food in the inventory
        /// </summary>
        /// <returns>true if successful, false if no more food exists</returns>
        protected virtual bool EatNextFood()
        {
            if (FoodSlots.Count == 0)
            {
                return false;
            }
            int nextFood = FoodSlots.Dequeue();
            Inventory.ClickInventory(nextFood);

            return true;
        }

        #endregion

        #region miscellaneous
        /// <summary>
        /// Waits for the specified time while periodically checking for the stop flag
        /// </summary>
        /// <param name="waitTime"></param>
        /// <returns>true if the StopFlag has been raised</returns>
        public static bool SafeWait(long waitTime)
        {
            int nextWaitTime;
            int waitInterval = 100;
            Stopwatch watch = new Stopwatch();
            watch.Start();

            while ((watch.ElapsedMilliseconds < waitTime))
            {
                if (StopFlag)
                {
                    return true;
                }
                nextWaitTime = Math.Min(waitInterval, (int)(waitTime - watch.ElapsedMilliseconds));
                Thread.Sleep(Math.Max(0, nextWaitTime));
            }
            return StopFlag;
        }

        /// <summary>
        /// Waits for a random time from a Gaussian distribution
        /// </summary>
        /// <param name="meanWaitTime">average wait time</param>
        /// <param name="stdDev">standard deviation froom the mean</param>
        /// <returns>true if the StopFlag has been raised</returns>
        public static bool SafeWait(long meanWaitTime, double stdDev)
        {
            if (meanWaitTime <= 0)
            {
                return StopFlag;
            }
            else
            {
                int waitTime = (int)Probability.BoundedGaussian(meanWaitTime, stdDev, 0.0, double.MaxValue);
                return SafeWait(waitTime);
            }
        }

        /// <summary>
        /// Waits for at least the specified wait time
        /// </summary>
        /// <param name="minWaitTime"></param>
        /// <param name="stdDev"></param>
        /// <returns></returns>
        public static bool SafeWaitPlus(long minWaitTime, double stdDev)
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
        /// Waits until the player stops moving
        /// </summary>
        /// <param name="timeout">maximum time in milliseconds to wait</param>
        /// <returns>true if player stops moving, false if we give up</returns>
        protected bool WaitDuringPlayerAnimation(int timeout = 180000, double colorStrictness = 0.95, double locationStrictness = 0.99)
        {
            int xOffset = ArtifactLength(0.06);
            int yOffset = ArtifactLength(0.06);
            Color[,] pastImage = null;
            Color[,] presentImage = null;
            Color[,] futureImage = null;

            Stopwatch watch = new Stopwatch();
            watch.Start();

            while (!ImageProcessing.ImageMatch(pastImage, presentImage, colorStrictness, locationStrictness)
                || !ImageProcessing.ImageMatch(presentImage, futureImage, colorStrictness, locationStrictness))
            {
                if (StopFlag || watch.ElapsedMilliseconds >= timeout)
                {
                    return false;   //timeout
                }

                ReadWindow();
                pastImage = presentImage;
                presentImage = futureImage;
                futureImage = ScreenPiece(Center.X - xOffset, Center.X + xOffset, Center.Y - yOffset, Center.Y + yOffset);
            }
            //DebugUtilities.SaveImageToFile(pastImage, "C:\\Projects\\Roboport\\debug_pictures\\player-before.png");
            //DebugUtilities.SaveImageToFile(presentImage, "C:\\Projects\\Roboport\\debug_pictures\\player-during.png");
            //DebugUtilities.SaveImageToFile(presentImage, "C:\\Projects\\Roboport\\debug_pictures\\player-after.png");
            return true;
        }

        /// <summary>
        /// Positions the camera facing north and as high as possible
        /// </summary>
        protected void DefaultCamera()
        {
            int compassX = ScreenWidth - 159;
            int compassY = 21;

            switch (RunParams.DefaultCameraPosition)
            {
                case RunParams.CameraPosition.AsIs:
                    break;
                case RunParams.CameraPosition.NorthAerial:
                    LeftClick(compassX, compassY);
                    Keyboard.UpArrow(1500);
                    break;
            }
        }

        /// <summary>
        /// Closes the chatbox if it is open and ClosedChatBox is set to true in RunParams
        /// </summary>
        /// <param name="ReadWindow">Set to true to read the screen</param>
        protected void ChatBox(bool readWindow = false, bool waitToClose = true)
        {
            if (RunParams.ClosedChatBox && ChatBoxIsOpen(readWindow))
            {
                LeftClick(34, ScreenHeight - 13, 5);
                if (waitToClose) { SafeWait(3000); }
            }
        }

        /// <summary>
        /// Determines if the chat box is currently open
        /// </summary>
        /// <returns></returns>
        protected bool ChatBoxIsOpen(bool readWindow = false)
        {
            if (readWindow) { ReadWindow(); }
            
            int left = 11;
            int right = left + 30;
            int top = ScreenHeight - 43;
            int bottom = top + 13;

            Color[,] chatName = ScreenPiece(left, right, top, bottom);
            double textMatch = ImageProcessing.FractionalMatch(chatName, RGBHSBRangeFactory.Black());
            double backgroundMatch = ImageProcessing.FractionalMatch(chatName, RGBHSBRangeFactory.ChatBoxBackground());

            return textMatch > 0.05 && backgroundMatch > 0.5;
        }

        /// <summary>
        /// Calls ReadWindow if the current screen image is unsatisfactory
        /// </summary>
        /// <returns>true unless the window needs to be read but can't</returns>
        private bool MakeSureWindowHasBeenRead(bool readWindow = false)
        {
            if (readWindow || (ScreenWidth == 0) || (ScreenHeight == 0))
            {
                return ReadWindow();
            }
            return true;
        }

        /// <summary>
        /// Moves the character to a bank icon on the minimap
        /// </summary>
        /// <returns>true if the bank icon is found</returns>
        protected virtual bool MoveToBank(int maxRunTimeToBank = 10000, bool readWindow = true, int minBankIconSize = 4)
        {
            if (readWindow) { ReadWindow(); }
            
            Point offset;
            bool[,] minimapBankIcon = Minimap.MinimapFilter(RGBHSBRangeFactory.BankIconDollar(), out offset);
            Blob bankBlob = ImageProcessing.BiggestBlob(minimapBankIcon);
            if (bankBlob == null || bankBlob.Size < minBankIconSize) { return false; }

            Point clickLocation = new Point(offset.X + bankBlob.Center.X, offset.Y + bankBlob.Center.Y);
            LeftClick(clickLocation.X, clickLocation.Y, 3);
            SafeWait(maxRunTimeToBank);

            return true;
        }

        /// <summary>
        /// Decrements the Iterations counter as items are made
        /// Assumes that items are being made in time. Does not visually check.
        /// </summary>
        /// <param name="decrement">Set to true to decrement Iterations during the countdown</param>
        protected void CountDownItems(bool decrement)
        {
            RunParams.BotIdle = true;
            for (int i = 0; i < MakeQuantity; i++)
            {
                if (SafeWait(SingleMakeTime))
                {
                    RunParams.BotIdle = false;
                    return;
                }
                if (decrement)
                {
                    RunParams.Iterations--;
                }
            }
            RunParams.BotIdle = false;
        }

        /// <summary>
        /// Moves the mouse to the left side of the window as if leaving to watch Netflix
        /// </summary>
        /// <param name="yOffset">vertcal offset for the average location of the position to move to</param>
        protected void WatchNetflix(int yOffset)
        {
            int x = -ScreenScraper.BorderWidth;
            int y = (int)Probability.RandomGaussian(Mouse.Y + yOffset, 100);
            Mouse.MoveMouseAsynchronous(x, y, RSClient);
        }

        /// <summary>
        /// Creates a series of test images to visual how a color filter works
        /// </summary>
        /// <param name="filter">color filter to test</param>
        /// <param name="directory">directory in which to save test images</param>
        /// <param name="name">base name for test images</param>
        protected void MaskTest(RGBHSBRange filter, string name = "maskTest", string directory = "C:\\Projects\\Roboport\\test_pictures\\mask_tests\\")
        {
            ReadWindow();
            bool[,] thing = ColorFilter(filter);
            DebugUtilities.TestMask(Bitmap, ColorArray, filter, thing, directory, name);
        }

        #endregion

        #region broadcasting

        private void BroadcastConnection()
        {

        }

        private void BroadcastDisconnect()
        {

        }

        private void BroadcastFailure()
        {

        }

        private void BroadcastLogin()
        {

        }

        private void BroadcastLogout()
        {

        }

        #endregion
    }
}