using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms
{
    public class MakePotionFull : Use14On14
    {
        protected const int MAKE_POTION_TIME = 16800;
        protected const int WAIT_FOR_HERBS_TO_CLEAN = 600;
        protected Point SecondaryIngredientBankSlot;
        protected Point UnfPotionBankSlot;

        /// <summary>
        /// Sets the time required to make 14 unfinished potions
        /// </summary>
        /// <param name="startParams"></param>
        /// <param name="craftingTime">time needed to make the 14 items being crafted</param>
        public MakePotionFull(StartParams startParams) : base(startParams, 8400)
        {
            SecondaryIngredientBankSlot = new Point(5, 0);
            UnfPotionBankSlot = new Point(4, 0);
        }

        /// <summary>
        /// Clean the grimy herbs before making the unfinished potions
        /// </summary>
        /// <returns>true if successful</returns>
        protected override bool PreMake()
        {
            for (int i = 0; i < 14; i++)
            {
                Inventory.ClickInventory(i, false);
            }
            return !SafeWaitPlus(WAIT_FOR_HERBS_TO_CLEAN, 200) ;
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
