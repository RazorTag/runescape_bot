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
        public RunParams ActiveRunParams { get { return ParamsList[ActiveBot]; } }

        /// <summary>
        /// Index of the active bot
        /// </summary>
        public int ActiveBot { get; set; }


        public int Count { get { return ParamsList.Length; } }
    }
}
