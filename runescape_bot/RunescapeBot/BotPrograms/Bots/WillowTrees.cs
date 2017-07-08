using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms
{
    public class WillowTrees : BotProgram
    {
        public WillowTrees(RunParams startParams) : base(startParams)
        {
            RunParams.Run = true;
        }

        /// <summary>
        /// Called once when the bot starts running
        /// </summary>
        protected override void Run()
        {
            Inventory.DropInventory();
        }

        /// <summary>
        /// Called once for each iteration of the bot
        /// </summary>
        /// <returns></returns>
        protected override bool Execute()
        {
            return true;
        }
    }
}
