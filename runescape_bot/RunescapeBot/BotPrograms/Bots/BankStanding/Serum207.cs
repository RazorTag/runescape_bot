using RunescapeBot.BotPrograms.Popups;
using System.Drawing;

namespace RunescapeBot.BotPrograms
{
    public class Serum207 : Herblore
    {
        protected Point AshesBankSlot;
        protected Point TarrominPotionBankSlot;

        public Serum207(RunParams startParams) : base(startParams)
        {
            AshesBankSlot = new Point(7, 0);
            TarrominPotionBankSlot = new Point(6, 0);
        }

        protected override bool Execute()
        {
            if (!WithdrawAshesAndPotions()
                || !MakeSerums())
            {
                return false;
            }


            //Only continue if we have enough supplies for another full inventory
            return RunParams.Iterations >= HALF_INVENTORY;
        }

        /// <summary>
        /// Opens the bank, withdraw 14 ashes + 14 tarromin potions, and closes the bank.
        /// </summary>
        /// <returns>true if successful</returns>
        protected bool WithdrawAshesAndPotions()
        {
            Bank bank;
            if (!OpenBank(out bank, 2)) { return false; }
            bank.DepositInventory();
            bank.WithdrawX(HerbBankSlot.X, HerbBankSlot.Y, HALF_INVENTORY);
            bank.WithdrawX(VialOfWaterBankSlot.X, VialOfWaterBankSlot.Y, HALF_INVENTORY);
            bank.Close();
            return true;
        }

        /// <summary>
        /// Manually uses each ashes on a tarromin potions to make serums.
        /// </summary>
        /// <returns>true if successful</returns>
        protected bool MakeSerums()
        {
            Point ashesInventorySlot;
            Point tarrominPotionInventorySlot;

            for (int i = 0; i < HALF_INVENTORY; i++)
            {
                if (StopFlag) { return false; }

                ashesInventorySlot = Inventory.InventoryIndexToCoordinates(i);
                if (ashesInventorySlot.Y % 2 == 1)  //Iterate through odd rows right to left instead of left to right like on even rows.
                {
                    ashesInventorySlot = Inventory.MirrorHorizontal(ashesInventorySlot);
                }
                tarrominPotionInventorySlot = Inventory.MirrorVertical(ashesInventorySlot);

                if (i % 2 == 0)
                {   //Use top half item on mirrored bottom half item.
                    Inventory.UseItemOnItem(ashesInventorySlot, tarrominPotionInventorySlot, false);
                }
                else
                {   //Use bottom half item on mirrored top half item.
                    Inventory.UseItemOnItem(tarrominPotionInventorySlot, ashesInventorySlot, false);
                }
                
                RunParams.Iterations--;
            }
            return true;
        }
    }
}
