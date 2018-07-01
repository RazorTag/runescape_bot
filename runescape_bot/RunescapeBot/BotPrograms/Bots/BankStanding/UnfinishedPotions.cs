using RunescapeBot.BotPrograms.Popups;
using System.Drawing;

namespace RunescapeBot.BotPrograms
{
    public class UnfinishedPotions : Herblore
    {
        /// <param name="startParams"></param>
        /// <param name="craftingTime">time needed to make the 14 items being crafted</param>
        public UnfinishedPotions(RunParams startParams) : base(startParams)
        {
            SingleMakeTime = MAKE_UNFINISHED_POTION_TIME;
        }

        protected override bool Execute()
        {
            if (!WithdrawHerbsAndVials()
                || !MakeUnfinishedPotions(false))
            {
                return false;
            }

            //Only continue if we have enough supplies for another full inventory
            return RunParams.Iterations >= HALF_INVENTORY;
        }
    }
}
