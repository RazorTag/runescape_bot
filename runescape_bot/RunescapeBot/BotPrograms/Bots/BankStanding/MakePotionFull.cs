using RunescapeBot.BotPrograms.Popups;
using System.Drawing;

namespace RunescapeBot.BotPrograms
{
    public class MakePotionFull : Herblore
    {
        /// <summary>
        /// Sets the time required to make 14 unfinished potions
        /// </summary>
        /// <param name="startParams"></param>
        /// <param name="craftingTime">time needed to make the 14 items being crafted</param>
        public MakePotionFull(RunParams startParams) : base(startParams)
        {
            SingleMakeTime = MAKE_FINISHED_POTION_TIME;
        }


        protected override bool Execute()
        {
            if (!WithdrawHerbsAndVials()
                || !MakeUnfinishedPotions(false)
                || !WithdrawSecondaryIngredients()
                || !MakeFinishedPotions(true))
            {
                return false;
            }

            //Only continue if we have enough supplies for another full inventory
            return RunParams.Iterations >= HALF_INVENTORY;
        }
    }
}
