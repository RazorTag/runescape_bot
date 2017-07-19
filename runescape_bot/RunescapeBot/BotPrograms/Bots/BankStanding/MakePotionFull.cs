using RunescapeBot.BotPrograms.Popups;
using System.Drawing;

namespace RunescapeBot.BotPrograms
{
    public class MakePotionFull : Herblore
    {
        protected Point SecondaryIngredientBankSlot;
        protected Point UnfPotionBankSlot;

        /// <summary>
        /// Sets the time required to make 14 unfinished potions
        /// </summary>
        /// <param name="startParams"></param>
        /// <param name="craftingTime">time needed to make the 14 items being crafted</param>
        public MakePotionFull(RunParams startParams) : base(startParams, MAKE_UNFINISHED_POTION_TIME)
        {
            SingleMakeTime = MAKE_POTION_TIME;
        }

        /// <summary>
        /// Clean the grimy herbs before making the unfinished potions
        /// </summary>
        /// <returns>true if successful</returns>
        protected override bool PreMake()
        {
            return CleanHerbs();
        }

        /// <summary>
        /// Use the first 14 items on the second 14 items
        /// </summary>
        /// <returns>true if successful</returns>
        protected override bool ProcessInventory()
        {
            if (PreMake() && MakeItems(false) && PostMake())
            {
                FailedRuns = 0;
                return true;
            }
            else
            {
                return ++FailedRuns > 2;
            }
        }

        /// <summary>
        /// Withdraw secondary ingredients and unfinished potions then makes finished potions
        /// </summary>
        /// <returns></returns>
        protected override bool PostMake()
        {
            Bank bank;
            OpenBank(out bank);
            WithdrawItems(bank);
            MakeItems(true);
            return true;
        }
    }
}
