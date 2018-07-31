using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms.Bots.MageTrainingArena
{
    public class EnchantingChamber : BotProgram
    {
        public EnchantingChamber(RunParams startParams) : base(startParams) { }

        protected override bool Run()
        {
            Inventory.SetEmptySlots();
            return true;
        }

        protected override bool Execute()
        {
            return true;
        }
    }
}
