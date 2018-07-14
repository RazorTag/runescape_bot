using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms.Settings
{
    [Serializable]
    public class FalconrySettingsData
    {
        /// <summary>
        /// Determines whether to catch spotted kebbits.
        /// </summary>
        public bool CatchSpottedKebbits { get; set; }

        /// <summary>
        /// Determines whether to catch dark kebbits.
        /// </summary>
        public bool CatchDarkKebbits { get; set; }

        /// <summary>
        /// Determines whether to catch dashing kebbits.
        /// </summary>
        public bool CatchDashingKebbits { get; set; }


        public FalconrySettingsData()
        {
            CatchSpottedKebbits = true;
            CatchDarkKebbits = true;
            CatchDashingKebbits = true;
        }
    }
}
