using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms
{
    public class Phasmatys : BotProgram
    {
        public const int NUMBER_OF_BOTS = 3;

        public Phasmatys(RunParams runParams) : base(runParams) { }

        /// <summary>
        /// Handles rotation cycles among the three bots
        /// </summary>
        protected override void ManageBot()
        {
            
        }
    }
}
