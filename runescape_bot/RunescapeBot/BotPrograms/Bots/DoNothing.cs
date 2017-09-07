using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms
{
    public class DoNothing : BotProgram
    {
        public DoNothing(RunParams startParams) : base(startParams) { }

        /// <summary>
        /// Don't actually do anything
        /// </summary>
        /// <returns>returns false to stop the bot program</returns>
        protected override bool Run()
        {
            RunParams.ActiveBot.BotState = BotState.Running;
            int timeToRun = (int)(RunParams.RunUntil - DateTime.Now).TotalMilliseconds;
            RunParams.SetNewState(timeToRun);
            RunParams.BotIdle = true;
            SafeWait(timeToRun);
            RunParams.BotIdle = false;

            return false;
        }
    }
}
