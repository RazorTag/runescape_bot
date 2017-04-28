using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms
{
    class NightmareZoneD : BotProgram
    {
        public NightmareZoneD(StartParams startParams) : base(startParams)
        {

        }

        protected override bool Execute()
        {
            Inventory.ClickInventory(0, 0);

            return true;
        }
    }
}
