using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms.Bots.Smithing
{
    public class BlastFurnace : BotProgram
    {
        public BlastFurnace(RunParams startParams) : base(startParams)
        {
            RunParams.Run = true;
        }
    }
}
