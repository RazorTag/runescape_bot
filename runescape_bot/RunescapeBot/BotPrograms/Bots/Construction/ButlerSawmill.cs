using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms
{
    public class ButlerSawmill : CamelotHouse
    {
        public ButlerSawmill(RunParams startParams) : base(startParams) { }


        protected override bool Construct()
        {
            CallServant();
            //TODO send oak logs to the lumber mill
            return true;
        }
    }
}
