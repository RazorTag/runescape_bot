using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms
{
    public class PhasmatysParams
    {
        public PhasmatysParams()
        {
            BotName = "New Bot";
        }

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
        /// Number of gold bars remaining
        /// </summary>
        public int GoldBars { get; set; }

        /// <summary>
        /// Number of steel bars remaining
        /// </summary>
        public int SteelBars { get; set; }

        /// <summary>
        /// Number of unstrung bows remaining
        /// </summary>
        public int Bows { get; set; }
    }
}
