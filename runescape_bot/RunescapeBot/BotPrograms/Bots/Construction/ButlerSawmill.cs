using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms.Bots.Construction
{
    public class ButlerSawmill : BotProgram
    {
        public ButlerSawmill(RunParams startParams) : base(startParams)
        {
            RunParams.Run = true;
        }


        protected override bool Execute()
        {
            return true;
        }
    }
}
