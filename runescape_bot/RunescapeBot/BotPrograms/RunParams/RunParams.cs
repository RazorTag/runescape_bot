﻿using RunescapeBot.ImageTools;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace RunescapeBot.BotPrograms
{
    public class RunParams
    {
        public RunParams()
        {
            InitializeBaseParams();
            InitializePhasmatys();
        }

        /// <summary>
        /// Call this constructor to initialize a bot for a rotation
        /// </summary>
        /// <param name="baseParams">Set to false if the bot is part of a rotation and not the base RunParams</param>
        public RunParams(bool baseParams = true)
        {
            InitializeBaseParams();

            if (baseParams)
            {
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
        }

        /// <summary>
        /// Initializes the Phasmatys rotation bot list.
        /// This should only be called to initialize the base RunParams.
        /// </summary>
        private void InitializePhasmatys()
        {
            PhasmatysParams = new RunParamsList(SimpleRotation.NUMBER_OF_BOTS);
            for (int i = 0; i < PhasmatysParams.Count; i++)
            {
                PhasmatysParams.ParamsList[i] = new PhasmatysRunParams();
                PhasmatysParams.ParamsList[i].TaskComplete = new BotResponse(DoNothing);
            }
        }

        #region settings

        /// <summary>
        /// Username of the bot account
        /// </summary>
        public string BotName { get; set; }

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
        private DateTime runUntil;
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

        /// <summary>
        /// Number of iterations after which the bot program should cease execution
        /// </summary>
        public int Iterations {
            get { return iterations; }
            set
            {
                iterations = Math.Max(0, value);
                SetIterations(iterations);
            }
        }
        private int iterations;

        /// <summary>
        /// Called when a new value is set for Iterations.
        /// </summary>
        /// <param name="iterations">new value for iterations</param>
        protected virtual void SetIterations(int iterations) { }

        /// <summary>
        /// Set to true to run for infinitely many iterations
        /// </summary>
        public bool InfiniteIterations { get; set; }

        /// <summary>
        /// Bot program to run
        /// </summary>
        public BotRegistry.BotActions BotAction { get; set; }

        /// <summary>
        /// Stores the bot's current position in its work cycle
        /// </summary>
        public BotProgram.BotState BotState { get; set; }

        /// <summary>
        /// Time when the current bot state began
        /// </summary>
        public DateTime CurrentStateStart { get; set; }

        /// <summary>
        /// TIme when the current bot state will end
        /// </summary>
        public DateTime CurrentStateEnd { get; set; }

        /// <summary>
        /// Average number of milliseconds between frames
        /// </summary>
        public int FrameTime { get; set; }

        /// <summary>
        /// Set to true to slightly vary the time between frames
        /// </summary>
        public bool RandomizeFrames { get; set; }

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
        public ScreenScraper.Client ClientType { get; set; }

        /// <summary>
        /// Toggles run on when the player has run energy
        /// </summary>
        public bool Run { get; set; }

        /// <summary>
        /// Set to true when the bot is idle so that the start form knows it can show itself
        /// </summary>
        public bool BotIdle { get; set; }

        /// <summary>
        /// Set to true to run the bot without breaks until it fails or is stopped externally
        /// </summary>
        public bool SlaveDriver { get; set; }

        /// <summary>
        /// List of bots to run on a rotation
        /// </summary>
        public RunParamsList RotationParams { get; set; }

        /// <summary>
        /// List of bots to run on the Phasmatys rotation
        /// </summary>
        public RunParamsList PhasmatysParams { get; set; }

        #endregion

        /// <summary>
        /// Sets the start time and length of a new bot state
        /// </summary>
        /// <param name="stateLength">length of the new bot state in milliseconds</param>
        public void SetNewState(long stateLength)
        {
            CurrentStateStart = DateTime.Now;
            CurrentStateEnd = CurrentStateStart.AddMilliseconds(stateLength);
        }

        #region delegates
        /// <summary>
        /// Used by the bot to inform that is has completed its task
        /// </summary>
        public BotResponse TaskComplete;

        /// <summary>
        /// Method that does nothing at all.
        /// Used as filler for a BotDone callback when nothing should be done.
        /// </summary>
        private void DoNothing() { }
        #endregion
    }
}