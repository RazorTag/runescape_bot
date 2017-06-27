using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms
{
    public class WillowTrees : BotProgram
    {
        public WillowTrees(StartParams startParams) : base(startParams)
        {
            RunParams.Run = true;
        }


        protected override void Run()
        {
            Inventory.DropInventory();
        }
    }
}
