using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms
{
    class DartTips : BotProgram
    {
        private const int CONSECUTIVE_FAILURES_ALLOWED = 3;
        private int failedRuns;

        public DartTips(StartParams startParams) : base(startParams)
        {
            RunParams.Run = true;
            failedRuns = 0;
        }

        protected override bool Execute()
        {
            if (failedRuns > CONSECUTIVE_FAILURES_ALLOWED) { return false; }



            failedRuns = 0;
            return true;
        }
    }
}
