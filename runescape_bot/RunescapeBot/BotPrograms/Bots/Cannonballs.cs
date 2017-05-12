using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms.Bots
{
    /// <summary>
    /// Smiths cannonballs at the furnace in Port Phasmatys
    /// </summary>
    class Cannonballs : BotProgram
    {

        public Cannonballs(StartParams startParams) : base(startParams)
        {
            RunParams.Run = true;
        }
    }
}
