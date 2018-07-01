using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms.Settings
{
    public class LesserDemonSettingsData
    {
        /// <summary>
        /// Set to true to telegrab valuable items as they are dropped
        /// </summary>
        public bool Telegrab { get; set; }

        /// <summary>
        /// Set to true to cast high alchemy on armor drops
        /// </summary>
        public bool HighAlch { get; set; }


        public LesserDemonSettingsData() { }
    }
}
