﻿using RunescapeBot.BotPrograms.Settings;
using RunescapeBot.Common;
using RunescapeBot.ImageTools;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace RunescapeBot.BotPrograms
{
    [Serializable]
    public class RunParams
    {
        #region constructors

        public RunParams()
        {
            InitializeBaseParams();
            InitializeRotation();
            InitializePhasmatys();
        }

        /// <summary>
        /// Call this constructor to initialize a bot for a rotation
        /// </summary>
        /// <param name="baseParams">Set to false if the bot is part of a rotation and not the base RunParams</param>
        public RunParams(bool baseParams)
        {
            InitializeBaseParams();
            if (baseParams)
            {
                InitializeRotation();
                InitializePhasmatys();
            }
        }

        /// <summary>
        /// Sets the normal default values for commonly used run parameters
        /// </summary>
        private void InitializeBaseParams()
        {
            BotName = "New Bot";
            FrameTime = 3000;
            RandomizeFrames = true;
            BotState = BotProgram.BotState.Running;
            RunUntil = DateTime.Now;
            BotWorldCheckEnabled = false;
            BotWorldCheckInterval = UnitConversions.MinutesToMilliseconds(5);
            TaskComplete = new BotResponse(DoNothing);
            StartEatingBelow = 0.5;
            StopEatingAbove = 0.8;
            ClosedChatBox = false;
            RunLoggedIn = true;
            RunAbove = 0.2;
            Conversation = false;
            CustomSettingsData = new CustomSettingsData();
        }

        /// <summary>
        /// Initializes the rotation bot list.
        /// This should only be called to initialize the base RunParams.
        /// </summary>
        private void InitializeRotation()
        {
            RotationParams = new RunParamsList(SimpleRotation.NUMBER_OF_BOTS, RunUntil);
            for (int i = 0; i < RotationParams.Count; i++)
            {
                RotationParams.ParamsList[i] = new RotationRunParams();
            }
        }

        /// <summary>
        /// Initializes the Phasmatys rotation bot list.
        /// This should only be called to initialize the base RunParams.
        /// </summary>
        private void InitializePhasmatys()
        {
            PhasmatysParams = new RunParamsList(PhasmatysRotation.NUMBER_OF_BOTS, RunUntil);
            for (int i = 0; i < PhasmatysParams.Count; i++)
            {
                PhasmatysParams.ParamsList[i] = new PhasmatysRunParams();
            }
        }

        #endregion

        #region settings

        /// <summary>
        /// Username of the bot account
        /// </summary>
        public virtual string BotName { get; set; }

        /// <summary>
        /// Username to use when logging in
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Password to use when logging in
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Time when the bot program should cease execution
        /// </summary>
        [XmlIgnore]
        public DateTime RunUntil
        {
            get { return runUntil; }
            set
            {
                if (value > DateTime.Now)
                {
                    runUntil = value;
                }
                else
                {
                    runUntil = DateTime.MaxValue;
                }
            }
        }
        private DateTime runUntil;

        /// <summary>
        /// Number of iterations after which the bot program should cease execution
        /// </summary>
        public virtual int Iterations {
            get { return iterations; }
            set { iterations = Math.Max(0, value); }
        }
        private int iterations;

        /// <summary>
        /// Set to true to run for infinitely many iterations
        /// </summary>
        [XmlIgnore]
        public bool InfiniteIterations { get; set; }

        /// <summary>
        /// Bot program to run
        /// </summary>
        public virtual BotRegistry.BotActions BotAction { get; set; }

        /// <summary>
        /// Stores the bot's current position in its work cycle
        /// </summary>
        [XmlIgnore]
        public BotProgram.BotState BotState { get; set; }

        /// <summary>
        /// Time when the current bot state began
        /// </summary>
        [XmlIgnore]
        public DateTime CurrentStateStart { get; set; }

        /// <summary>
        /// TIme when the current bot state will end
        /// </summary>
        [XmlIgnore]
        public DateTime CurrentStateEnd { get; set; }

        /// <summary>
        /// Average number of milliseconds between frames
        /// </summary>
        [XmlIgnore]
        public int FrameTime { get; set; }

        /// <summary>
        /// Set to true to slightly vary the time between frames
        /// </summary>
        [XmlIgnore]
        public bool RandomizeFrames { get; set; }

        /// <summary>
        /// True by default to enable periodic bot world checks. Set to false to disable bot world checks.
        /// </summary>
        [XmlIgnore]
        public bool BotWorldCheckEnabled { get; set; }

        /// <summary>
        /// Minimum number of milliseconds between bot world checks
        /// </summary>
        [XmlIgnore]
        public int BotWorldCheckInterval { get; set; }

        /// <summary>
        /// File path of the standard Jagex OSRS client
        /// </summary>
        public string JagexClient { get; set; }

        /// <summary>
        /// File path of the OSBuddy client
        /// </summary>
        public string OSBuddyClient { get; set; }

        /// <summary>
        /// The file path of the RuneScape client to use
        /// </summary>
        public string RuneScapeClient
        {
            get
            {
                switch (ClientType)
                {
                    case ScreenScraper.Client.Jagex:
                        return JagexClient;
                    case ScreenScraper.Client.OSBuddy:
                        return OSBuddyClient;
                    default:
                        return JagexClient;
                }
            }
        }

        /// <summary>
        /// Arguments to be supplied with the file path when starting a client
        /// </summary>
        public string ClientFlags
        {
            get
            {
                switch (ClientType)
                {
                    case ScreenScraper.Client.Jagex:
                        return "oldschool";
                    case ScreenScraper.Client.OSBuddy:
                        return "";
                    default:
                        return "oldschool";
                }
            }
        }

        /// <summary>
        /// Specifies which client to use
        /// </summary>
        [XmlIgnore]
        public ScreenScraper.Client ClientType { get; set; }

        /// <summary>
        /// Toggles run on when the player has run energy
        /// </summary>
        [XmlIgnore]
        public bool Run { get; set; }

        /// <summary>
        /// Set to true when the bot is idle so that the start form knows it can show itself
        /// </summary>
        [XmlIgnore]
        public bool BotIdle { get; set; }

        /// <summary>
        /// The RunParams for the currently running bot.
        /// Defaults to the base RunParams if no bot is running.
        /// </summary>
        public RunParams ActiveBot
        {
            get
            {
                switch (BotManager)
                {
                    case BotRegistry.BotManager.Rotation:
                        return RotationParams.ActiveRunParams;
                    case BotRegistry.BotManager.Phasmatys:
                        return PhasmatysParams.ActiveRunParams;
                    default:
                        return this;
                }
            }
        }

        /// <summary>
        /// Set to true to run the bot without breaks until it fails or is stopped externally
        /// </summary>
        [XmlIgnore]
        public bool SlaveDriver { get; set; }

        /// <summary>
        /// Default world to use when logging in. Keep as 0 to avoid choosing a world.
        /// </summary>
        [XmlIgnore]
        public int LoginWorld { get; set; }

        /// <summary>
        /// Makes sure to log into the game before calling the Run method
        /// </summary>
        [XmlIgnore]
        public bool RunLoggedIn { get; set; }

        /// <summary>
        /// Set to true to automatically 
        /// </summary>
        [XmlIgnore]
        public bool AutoEat { get; set; }

        /// <summary>
        /// Fraction of total hitpoints below which the bot will automaticall eat the next food if AutoEat is set to true.
        /// </summary>
        [XmlIgnore]
        public double StartEatingBelow { get; set; }

        /// <summary>
        /// Fraction of total hitpoints above which the bot will stop automatically eating.
        /// </summary>
        [XmlIgnore]
        public double StopEatingAbove { get; set; }

        /// <summary>
        /// Run the bot with the chat box closed so that the bot can see that area of the screen.
        /// </summary>
        [XmlIgnore]
        public bool ClosedChatBox { get; set; }

        /// <summary>
        /// Do not turn on run below this threshold.
        /// </summary>
        [XmlIgnore]
        public double RunAbove { get; set; }

        /// <summary>
        /// How to set the camera after logging in.
        /// </summary>
        [XmlIgnore]
        public CameraPosition DefaultCameraPosition { get; set; }
        public enum CameraPosition : int
        {
            AsIs,
            NorthAerial
        }

        /// <summary>
        /// The result of the most recent check of the client's logged in status.
        /// Should default to false when the results of a login state check are uncertain.
        /// </summary>
        [XmlIgnore]
        public bool LoggedIn { get; set; }

        /// <summary>
        /// Set to true to enable text scanning and conversation with other players.
        /// </summary>
        [XmlIgnore]
        public bool Conversation { get; set; }

        /// <summary>
        /// Indicates which tab is currently running a bot
        /// Set to -1 if no bot is running
        /// </summary>
        public BotRegistry.BotManager BotManager { get; set; }

        /// <summary>
        /// List of bots to run on a rotation
        /// </summary>
        public RunParamsList RotationParams { get; set; }

        /// <summary>
        /// List of bots to run on the Phasmatys rotation
        /// </summary>
        public RunParamsList PhasmatysParams { get; set; }

        #endregion

        #region custom settings

        /// <summary>
        /// Used for saving custom settings choices
        /// </summary>
        public CustomSettingsData CustomSettingsData { get; set; }

        #endregion

        #region start form state info

        /// <summary>
        /// The tab selected on the start form
        /// </summary>
        public virtual int SelectedTab { get; set; }

        /// <summary>
        /// The Phasmatys bot selected from the dropdown
        /// </summary>
        public virtual int SelectedPhasmatysBot { get; set; }

        /// <summary>
        /// The simple rotation bot selected from the dropdown
        /// </summary>
        public virtual int SelectedRotationBot { get; set; }

        /// <summary>
        /// Sets the start time and length of a new bot state
        /// </summary>
        /// <param name="stateLength">length of the new bot state in milliseconds</param>
        public void SetNewState(long stateLength)
        {
            stateLength = (long) Numerical.LimitToRange(stateLength, 0, UnitConversions.HoursToMilliseconds(168));  //limit state length to 1 week as a sanity check
            CurrentStateStart = DateTime.Now;
            CurrentStateEnd = CurrentStateStart.AddMilliseconds(stateLength);
        }

        #endregion

        #region delegates
        /// <summary>
        /// Used by the bot to inform that is has completed its task
        /// </summary>
        [XmlIgnore]
        public BotResponse TaskComplete { get; set; }

        /// <summary>
        /// Method that does nothing at all.
        /// Used as filler for a BotDone callback when nothing should be done.
        /// </summary>
        protected void DoNothing() { }
        #endregion
    }
}