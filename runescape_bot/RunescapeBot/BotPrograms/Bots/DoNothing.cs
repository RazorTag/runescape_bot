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
            const int arbitraryWaitTime = 5000; //5000 ms

            while (!StopFlag && (DateTime.Now < RunParams.RunUntil))
            {
                SafeWait(arbitraryWaitTime);
            }

            return false;
        }
    }
}
