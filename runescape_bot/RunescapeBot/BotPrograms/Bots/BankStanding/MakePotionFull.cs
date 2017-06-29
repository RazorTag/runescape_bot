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
        public MakePotionFull(StartParams startParams) : base(startParams, MAKE_UNFINISHED_POTION_TIME) { }

        /// <summary>
        /// Clean the grimy herbs before making the unfinished potions
        /// </summary>
        /// <returns>true if successful</returns>
        protected override bool PreMake()
        {
            return CleanHerbs();
        }

        /// <summary>
        /// Withdraw secondary ingredients and unfinished potions then makes finished potions
        /// </summary>
        /// <returns></returns>
        protected override bool PostMake()
        {
            WithdrawItems(SecondaryIngredientBankSlot, UnfPotionBankSlot);
            MakeItems(MAKE_POTION_TIME);
            return true;
        }
    }
}
