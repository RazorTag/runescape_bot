using System;

namespace WindowsFormsApplication1
{
    public class StartParams
    {
        public StartParams()
        {
            if (FrameRate == 0)
            {
                FrameRate = 1.0;
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
        public double FrameRate { get; set; }

        /// <summary>
        /// Time at which to end execution if it hasn't ended already
        /// </summary>
        public DateTime EndTime { get; set; }
    }
}
