using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace RunescapeBot.BotPrograms
{
    public class StartParams
    {
        public StartParams()
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
        public DateTime RunUntil { get; set; }

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

        #region delegates
        /// <summary>
        /// Used by the bot to inform that is has completed its task
        /// </summary>
        public BotResponse TaskComplete;
        #endregion
    }
}