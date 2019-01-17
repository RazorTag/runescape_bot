using RunescapeBot.BotPrograms.Chat;
using RunescapeBot.Common;
using RunescapeBot.FileIO;
using RunescapeBot.ImageTools;
using RunescapeBot.UITools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

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
        protected RSClient RSClient;

        /// <summary>
        /// Stores a Color array of the client window
        /// </summary>
        protected GameScreen Screen;

        /// <summary>
        /// Image processing.
        /// </summary>
        protected Vision Vision;

        /// <summary>
        /// Handles actions requiring both sight and input (mouse/keyboard) in tight coordination.
        /// </summary>
        protected HandEye HandEye;

        /// <summary>
        /// Keyboard controller
        /// </summary>
        protected Keyboard Keyboard { get; set; }

        /// <summary>
        /// Thread in which the run method is executed
        /// </summary>
        protected Thread RunThread { get; set; }

        /// <summary>
        /// The last time that we checked if we are in a bot world
        /// </summary>
        protected DateTime LastBotWorldCheck { get; set; }

        /// <summary>
        /// Handles locating and using banks.
        /// </summary>
        protected Banking Banking;

        /// <summary>
        /// The sidebar including the inventory and spellbook.
        /// </summary>
        protected Inventory Inventory { get; set; }

        /// <summary>
        /// The minimap and associated gauges at the top-left of the game window.
        /// </summary>
        protected MinimapGauge Minimap { get; set; }

        /// <summary>
        /// The textbox at the bottom-left of the game screen.
        /// </summary>
        protected TextBoxTool Textbox { get; set; }

        /// <summary>
        /// Text chat between the player and other players via the chat textbox.
        /// </summary>
        protected Conversation Conversation { get; set; }

        /// <summary>
        /// Expected time to complete a single iteration.
        /// </summary>
        protected int SingleMakeTime;

        /// <summary>
        /// Number of iterations in a single execution.
        /// </summary>
        protected int MakeQuantity;

        /// <summary>
        /// Reusable random number generator.
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

        //Eating
        protected const int EAT_TIME = 3 * BotRegistry.GAME_TICK;
        protected Queue<int> FoodSlots;

        /// <summary>
        /// Gets the approximate width/height of a square tile near the center of the game screen.
        /// </summary>
        public double TileWidth { get { return Screen.ArtifactLength(0.06); } }

        #endregion

        #region core bot process
        /// <summary>
        /// Initializes a bot program with a client matching startParams
        /// </summary>
        /// <param name="startParams">specifies how to run the bot</param>
        protected BotProgram(RunParams startParams)
        {
            RunParams = startParams;
            RunParams.ClientType = ScreenScraper.Client.Jagex;
            RunParams.DefaultCameraPosition = RunParams.CameraPosition.NorthAerial;
            RunParams.LoginWorld = 0;

            RSClient = new RSClient(RunParams);
            Screen = new GameScreen(RSClient, RunParams);

            Vision = new Vision(Screen, RunParams);
            Keyboard = new Keyboard(RSClient);
            Mouse.RSClient = RSClient;
            HandEye = new HandEye(Vision, Screen);
            Inventory = new Inventory(RSClient, Keyboard, Screen);
            Minimap = new MinimapGauge(RSClient, Keyboard, Screen);
            Textbox = new TextBoxTool(Keyboard, Screen);
            Banking = new Banking(Screen, Vision, HandEye, RSClient, Keyboard, Inventory, Minimap);
            Conversation = new Conversation(Textbox, Screen, Keyboard, RunParams.Conversation);
            
            RNG = new Random();
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
        /// Handles the sequential calling of the methods used to do bot work.
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
                    LogError.SimpleLog(e);  //Log an error raised during a bot's execution.
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
            Screen.MakeSureWindowHasBeenRead();
            if ((RunParams.RunLoggedIn && !CheckLogIn(true)) || !Run())
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

            double avgWorkTime = 84.8;
            double avgRestTime = 25.1;
            double avgStatusTimeRemaining = (avgRestTime / (avgRestTime + avgWorkTime)) * (avgRestTime / 2) + (avgWorkTime / (avgRestTime + avgWorkTime)) * (avgWorkTime / 2);  //expected time left in minutes to finish working or resting when it is time for the bot to sleep
            int awakeTime = UnitConversions.HoursToMilliseconds(10 - (avgStatusTimeRemaining / 60.0));
            Stopwatch sleepWatch = new Stopwatch();
            bool done = false;
            sleepWatch.Start();

            //alternate between work periods (Iterate) and break periods
            //Works for an average of 7.716 hours during each 10 hour awake period
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
            ScreenScraper.BringToForeGround();
            SafeWait(1000); //give the client time to show up on screen

            Stopwatch iterationWatch = new Stopwatch();
            Stopwatch workIntervalWatch = new Stopwatch();
            workIntervalWatch.Start();

            while ((DateTime.Now < RunParams.RunUntil) && ((RunParams.Iterations > 0) || (RunParams.InfiniteIterations == true)))
            {
                if (StopFlag) { return true; }   //quit immediately if the stop flag has been raised or we can't log back in
                iterationWatch.Restart();
                if (!Screen.ReadWindow() || BotWorldCheck(false)) { continue; }   //We had to switch out of a bot world

                //Only do the actual botting if we are logged in
                if (CheckLogIn(false))
                {
                    if (Screen.LooksValid()) //Make sure the read is successful before using the bitmap values
                    {
                        if (RunParams.AutoEat)
                        {
                            ManageHitpoints(false);
                        }

                        if (RunParams.Run)
                        {
                            Minimap.RunCharacter(RunParams.RunAbove, false); //Turn on run if the player has run energy
                        }
                        
                        if (!Execute() && !StopFlag) //quit by a bot program
                        {
                            LogError.ScreenShot(Screen, "bot-quit");
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
        /// Clean up.
        /// </summary>
        private void Done()
        {
            Conversation.Stop();

            if (LogoutWhenDone)
            {
                Logout();
            }

            BotIsDone = true;
            Screen.Dispose();

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

            //average of 84.8 minutes
            if (workType < 0.3)   //30%
            {
                mean = 45;
                stdDev = 18;
            }
            else if (workType < 0.7) //40%
            {
                mean = 83; 
                stdDev = 35;
            }
            else //30%
            {
                mean = 127;
                stdDev = 21;
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
                randomize = (int)((Screen.Height / 1000.0) * randomize);
                Point moveLocation = Probability.GaussianCircle(new Point(x, y), stdRatio * randomize, 0, 360, randomize);
                Mouse.Move(moveLocation.X, moveLocation.Y);
            }
        }

        /// <summary>
        /// Wrapper for MouseActions.LeftMouseClick
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="hoverDelay"></param>
        /// <param name="randomize">maximum number of pixels in each direction by which to randomize the click location</param>
        protected Point LeftClick(int x, int y, int randomize = 5, int hoverDelay = Mouse.HOVER_DELAY)
        {
            randomize = (int)((Screen.Height / 1000.0) * randomize);
            return Mouse.LeftClick(x, y, randomize, hoverDelay);
        }

        /// <summary>
        /// Wrapper for MouseActions.RightMouseClick
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        protected Point RightClick(int x, int y, int randomize = 5, int hoverDelay = Mouse.HOVER_DELAY)
        {
            randomize = (int)((Screen.Height / 1000.0) * randomize);
            return Mouse.RightClick(x, y, randomize, hoverDelay);
        }

        /// <summary>
        /// Selects Make All for the single make option that shows up over the chat box
        /// </summary>
        /// <param name="rsClient"></param>
        /// <returns></returns>
        public bool ChatBoxSingleOptionMakeAll(Process rsClient)
        {
            Point screenSize = ScreenScraper.GetWindowSize(rsClient);
            int X = 256;
            int Y = screenSize.Y - 90;
            Random rng = new Random();

            Point leftClick = new Point(X, Y);
            Blob clickBlob = new Blob(leftClick);

            HandEye.MouseOverDroppedItem(clickBlob,true, 20, 5000);

            return true;
        }

        #endregion

        #region login/logout

        /// <summary>
        /// Respond to a failed attempt to log in
        /// </summary>
        /// <returns>true if the failed login is handled satisfactorily. false if the bot should stop</returns>
        private bool HandleFailedLogIn()
        {
            return RSClient.PrepareClient(Screen, true);
        }

        /// <summary>
        /// Determines if the user is logged out and logs him back in if he is.
        /// If the bot does not have valid login information, then it will quit.
        /// </summary>
        /// <returns>true if we are already logged in or we are able to log in, false if we can't log in</returns>
        protected virtual bool CheckLogIn(bool readWindow)
        {
            //Check several times over several seconds to make sure that we are logged out before trying to log in
            for (int i = 0; i < 6; i++)
            {
                if (!Screen.IsLoggedOut(readWindow))
                {
                    return true;    //already logged in
                }
                SafeWait(1000);
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
        /// Opens the world selector on the login screen
        /// </summary>
        /// <returns>true if successful</returns>
        protected bool OpenWorldSelector(Point loginOffset)
        {
            int x = loginOffset.X + (Screen.Center.X - 326);
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

            Point offset = (loginOffset ?? Screen.LoginScreenOffset());
            if (!OpenWorldSelector(offset)) { return false; }
            SafeWait(2000);
            int worldIndex = world - lowestWorld;

            //adjust for missing worlds
            if (world > 362) { worldIndex -= 2; }
            if (world > 370) { worldIndex -= 2; }
            if (world > 378) { worldIndex -= 2; }

            int column = worldIndex / rowCount;
            int row = worldIndex % rowCount;
            int x = offset.X + (Screen.Center.X - 182) + (column * columnWidth);
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
            Point loginOffset = Screen.LoginScreenOffset();

            //log in at the login screen
            if (!Screen.IsWelcomeScreen(out clickLocation))
            {
                if (!SelectLoginWorld(RunParams.LoginWorld, loginOffset))
                {
                    return false;
                }

                //Click existing account. Clicks in a dead space if we are already on the login screen.
                LeftClick(Screen.Center.X + 16 + loginOffset.X, 288 + loginOffset.Y);

                if (RunParams.LoginWorld > 0)
                {
                    LeftClick(Screen.Center.X - 78 + loginOffset.X, 320 + loginOffset.Y);
                    SafeWait(500);
                }

                //fill in login
                LeftClick(Screen.Center.X + 137 + loginOffset.X, 249 + loginOffset.Y);
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
                if (Screen.ConfirmWelcomeScreen(out clickLocation))
                {
                    LeftClick(clickLocation.Value.X, clickLocation.Value.Y);
                }
                else
                {
                    return false;
                }
            }

            //verify the log in
            if (Screen.ConfirmLogin())
            {
                DefaultCamera();
                ChatBox();
                return true;
            }
            else
            {
                return false;
            }
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
        /// Logs out of the game
        /// </summary>
        protected void Logout()
        {
            RunParams.LoggedIn = false; //Signal our intent to logout immediately to anything else that is checking the login state.
            const int maxLogoutAttempts = 10;
            int logoutAttempts = 0;

            while (!Screen.IsLoggedOut(true) && (logoutAttempts++ < maxLogoutAttempts) && !StopFlag)
            {
                Inventory.OpenLogout();
                SafeWait(800, 200);
                LeftClick(Screen.Width - 38, Screen.Height - 286);    //close out of world switcher
                SafeWait(2000, 400);
                LeftClick(Screen.Width - 120, Screen.Height - 71, 5); //click here to logout
                SafeWait(2000, 400);
            }
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
            if (Screen.IsLoggedOut(readWindow))
            {
                if (Screen.LoginSetForBotWorld(false))
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
            Screen.MakeSureWindowHasBeenRead(readWindow);
            Inventory.OpenLogout();
            SafeWaitPlus(1000, 150);
            Screen.ReadWindow();
            if (!Screen.WorldSwitcherIsOpen())
            {
                ClickWorldSwitcher();
                SafeWaitPlus(1500, 500);
                Screen.ReadWindow();
            }

            Stopwatch watch = new Stopwatch();
            watch.Start();
            while (!Screen.WorldSwitcherIsOpen() && (watch.ElapsedMilliseconds < 3000) && !StopFlag)
            {
                ClickWorldSwitcher();
                SafeWait(600, 200);
                Screen.ReadWindow();
            }

            int left = Screen.Width - 84;
            int right = left + 30;
            int top = Screen.Height - 297;
            int bottom = top + 20;
            long colorSum = ImageProcessing.ColorSum(Vision.ScreenPiece(left, right, top, bottom));
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
            int left = Screen.Width - 180;
            int right = left + 110;
            int top = Screen.Height - 123;
            int bottom = top + 15;
            Point click = Probability.GaussianRectangle(left, right, top, bottom);
            LeftClick(click.X, click.Y);
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
            if (readWindow) { Screen.ReadWindow(); }

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
                    Screen.ReadWindow();
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
        /// <returns>true if the stop flag is raised</returns>
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
        /// Positions the camera facing north and as high as possible
        /// </summary>
        protected void DefaultCamera()
        {
            int compassX = Screen.Width - 159;
            int compassY = 21;

            switch (RunParams.DefaultCameraPosition)
            {
                case RunParams.CameraPosition.AsIs:
                    break;
                case RunParams.CameraPosition.NorthAerial:
                    LeftClick(compassX, compassY);
                    Keyboard.HoldKey(Keys.Up, 3000);
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
                LeftClick(34, Screen.Height - 13, 5);
                if (waitToClose) { SafeWait(3000); }
            }
        }

        /// <summary>
        /// Determines if the chat box is currently open
        /// </summary>
        /// <returns></returns>
        protected bool ChatBoxIsOpen(bool readWindow = false)
        {
            if (readWindow) { Screen.ReadWindow(); }
            
            int left = 11;
            int right = left + 30;
            int top = Screen.Height - 43;
            int bottom = top + 13;

            Color[,] chatName = Vision.ScreenPiece(left, right, top, bottom);
            double textMatch = ImageProcessing.FractionalMatch(chatName, RGBHSBRangeFactory.Black());
            double backgroundMatch = ImageProcessing.FractionalMatch(chatName, RGBHSBRangeFactory.ChatBoxBackground());

            return textMatch > 0.05 && backgroundMatch > 0.5;
        }

        /// <summary>
        /// Decrements the Iterations counter as items are made.
        /// Assumes that items are being made in time. Does not visually check.
        /// </summary>
        /// <param name="decrement">Set to true to decrement Iterations during the countdown</param>
        protected void CountDownItems(bool decrement, bool watchNetflix = false)
        {
            if (watchNetflix)
            {
                WatchNetflix(0);
            }

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
            Mouse.MoveMouseAsynchronous(x, y);
        }

        /// <summary>
        /// Creates a series of test images to visual how a color filter works
        /// </summary>
        /// <param name="filter">color filter to test</param>
        /// <param name="directory">directory in which to save test images</param>
        /// <param name="name">base name for test images</param>
        /// <param name="readWindow">Set to true to always read the window</param>
        protected void MaskTest(ColorFilter filter, string name = "maskTest", string directory = "C:\\Projects\\Roboport\\test_pictures\\mask_tests\\", bool readWindow = false)
        {
            Screen.MakeSureWindowHasBeenRead();
            bool[,] thing = Vision.ColorFilter(filter);
            DebugUtilities.TestMask(Screen.Bitmap, Screen, filter, thing, directory, name);
        }

        /// <summary>
        /// Very rough time needed for the player to run to another point on the game screen.
        /// Assumes that there are no obstacles between the player and target that would force the player to change course.
        /// </summary>
        /// <param name="target">Target point to run to.</param>
        /// <returns>The estimated time to run to the target location.</returns>
        public double RunTime(Point target)
        {
            double tiles = Geometry.DistanceBetweenPoints(target, Screen.Center) / TileWidth;
            int effectiveTiles = (int)Math.Round(tiles);
            if (effectiveTiles % 2 == 1)
            {
                tiles++;
            }
            double travelTime = (tiles / 2.0) * BotRegistry.GAME_TICK;
            return travelTime + (2 * BotRegistry.GAME_TICK);
        }

        #endregion
    }
}