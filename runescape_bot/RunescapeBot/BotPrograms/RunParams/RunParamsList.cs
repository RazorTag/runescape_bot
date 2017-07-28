using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms
{
    [Serializable]
    public class RunParamsList
    {
        public RunParamsList() { }

        public RunParamsList(int botCount)
        {
            ParamsList = new RunParams[botCount];
            ActiveBot = -1;
        }

        /// <summary>
        /// List of bots to run on the Phasmatys rotation
        /// </summary>
        public RunParams[] ParamsList { get; set; }

        /// <summary>
        /// Indexes RunParamslIst to ParamsList
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public RunParams this[int index]
        {
            get { return ParamsList[index]; }
            set { ParamsList[index] = value; }
        }

        /// <summary>
        /// Reference to the currently active RunParams
        /// </summary>
        public RunParams ActiveRunParams
        {
            get
            {
                if (ActiveBot >=0 && ActiveBot < ParamsList.Length)
                {
                    return ParamsList[ActiveBot];
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Index of the active bot
        /// </summary>
        public int ActiveBot { get; set; }

        /// <summary>
        /// The number of bots in this paramtere list
        /// </summary>
        public int Count { get { return ParamsList.Length; } }

        /// <summary>
        /// Determines if the active bot is currently idle.
        /// </summary>
        /// <returns>true if the current bot is idle, false if active or unknown</returns>
        public bool ActiveBotIsIdle()
        {
            RunParams activeRunParams = ActiveRunParams;
            if (activeRunParams == null)
            {
                return false;
            }
            return activeRunParams.BotIdle;
        }
    }
}
