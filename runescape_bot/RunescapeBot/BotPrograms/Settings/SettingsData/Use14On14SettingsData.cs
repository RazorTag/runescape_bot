using RunescapeBot.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms.Settings
{
    public class Use14On14SettingsData
    {
        public Use14On14SettingsData()
        {
            MakeTime = 0;
        }

        /// <summary>
        /// Time in milliseconds needed to make a single item.
        /// </summary>
        public int MakeTime
        {
            get { return makeTime; }
            set
            {
                makeTime = (int) Numerical.LimitToRange(value, 0, 1000000);
            }
        }
        private int makeTime;
    }
}
