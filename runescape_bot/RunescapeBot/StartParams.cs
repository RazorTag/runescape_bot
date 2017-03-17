using System;

namespace RunescapeBot
{
    public class StartParams
    {
        public StartParams()
        {
            if (FrameTime == 0)
            {
                FrameTime = 3000;
                RandomizeFrames = true;
            }
        }

        /// <summary>
        /// Username to search for when locating a RS client
        /// </summary>
        public string username { get; set; }

        /// <summary>
        /// Bot program to run
        /// </summary>
        public Start.BotActions BotAction { get; set; }

        /// <summary>
        /// Time when the bot program should cease execution
        /// </summary>
        public DateTime RunUntil { get; set; }

        /// <summary>
        /// Number of iterations after which the bot program should cease execution
        /// </summary>
        public int Iterations { get; set; }

        /// <summary>
        /// Rate at which to iterate in units of Hz
        /// </summary>
        public int FrameTime { get; set; }

        /// <summary>
        /// Set to true to slightly vary the time between frames
        /// </summary>
        public bool RandomizeFrames { get; set; }

        /// <summary>
        /// Used by the bot to inform that is has completed its task
        /// </summary>
        public BotResponse TaskComplete;
    }
}
