using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace RunescapeBot.BotPrograms
{
    public class RunParams
    {
        public RunParams()
        {
            if (FrameTime == 0)
            {
                FrameTime = 3000;
                RandomizeFrames = true;
                BotState = BotProgram.BotState.Running;
            }
        }

        #region settings
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
        public int Iterations { get; set; }

        /// <summary>
        /// Bot program to run
        /// </summary>
        public Start.BotActions BotAction { get; set; }

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
        /// File location of the client to run
        /// </summary>
        public string ClientFilePath { get; set; }

        /// <summary>
        /// Toggles run on when the player has run energy
        /// </summary>
        public bool Run { get; set; }

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
        #endregion
    }
}