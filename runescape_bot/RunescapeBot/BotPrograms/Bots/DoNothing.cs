using RunescapeBot.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms
{
    public class DoNothing : BotProgram
    {
        private int ArbitraryWaitTime;

        public DoNothing(RunParams runParams) : base(runParams)
        {
            RunParams.SlaveDriver = true;
            ArbitraryWaitTime = UnitConversions.MinutesToMilliseconds(15);
        }

        /// <summary>
        /// Wait until the bot is stopped
        /// </summary>
        /// <returns></returns>
        protected override bool Execute()
        {
            return !SafeWait(ArbitraryWaitTime);
        }
    }
}
