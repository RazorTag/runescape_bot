using RunescapeBot.BotPrograms.Popups;
using System.Drawing;

namespace RunescapeBot.BotPrograms
{
    public class Herblore : BotProgram
    {
        public const int MAKE_FINISHED_POTION_TIME = 2 * BotRegistry.GAME_TICK;
        public const int MAKE_UNFINISHED_POTION_TIME = BotRegistry.GAME_TICK;
        protected const int WAIT_FOR_HERBS_TO_CLEAN = 500;
        protected const int HALF_INVENTORY = Inventory.INVENTORY_CAPACITY / 2;

        protected Point HerbBankSlot;
        protected Point VialOfWaterBankSlot;
        protected Point SecondaryIngredientBankSlot;

        protected Point FirstHalfInventorySlot;
        protected Point SecondHalfInventorySlot;

        /// <summary>
        /// The first half of the inventory slots are set to true in the order that items are added to the inventory.
        /// </summary>
        protected bool[,] FirstHalfInventory
        {
            get
            {
                if (_firstHalfInventory != null)
                {
                    return _firstHalfInventory;
                }
                else
                {
                    _firstHalfInventory = new bool[Inventory.INVENTORY_COLUMNS, Inventory.INVENTORY_ROWS];
                    for (int i = 0; i < Inventory.INVENTORY_CAPACITY / 2; i++)
                    {
                        Point inventorySlot = Inventory.InventoryIndexToCoordinates(i);
                        _firstHalfInventory[inventorySlot.X, inventorySlot.Y] = true;
                    }
                    return _firstHalfInventory;
                }
            }
        }
        protected bool[,] _firstHalfInventory;


        public Herblore(RunParams startParams) : base(startParams)
        {
            SingleMakeTime = MAKE_FINISHED_POTION_TIME;
            MakeQuantity = HALF_INVENTORY;

            HerbBankSlot = new Point(7, 0);
            VialOfWaterBankSlot = new Point(6, 0);
            SecondaryIngredientBankSlot = new Point(5, 0);

            FirstHalfInventorySlot = Inventory.InventoryIndexToCoordinates(Inventory.INVENTORY_CAPACITY / 2 - 1);
            SecondHalfInventorySlot = Inventory.InventoryIndexToCoordinates(Inventory.INVENTORY_CAPACITY / 2);
        }

        /// <summary>
        /// Cleans the herbs in the first 14 inventory slots by default.
        /// </summary>
        /// <param name="fullInventory">set to true to clean all 28 inventory slots</param>
        /// <returns></returns>
        protected bool CleanHerbs(bool fullInventory = false)
        {
            //snake through the grimy herbs to clean them like a human would instead of going in index order
            for (int row = 0; row < Inventory.INVENTORY_ROWS; row++)
            {
                if (row % 2 == 0)
                {
                    for (int column = Inventory.INVENTORY_COLUMNS - 1; column >= 0; column--)
                    {
                        if (FirstHalfInventory[column, row])
                        {
                            Inventory.ClickInventory(column, row, false);
                        }
                    }
                }
                else
                {
                    for (int column = 0; column < Inventory.INVENTORY_COLUMNS; column++)
                    {
                        if (FirstHalfInventory[column, row])
                        {
                            Inventory.ClickInventory(column, row, false);
                        }
                    }
                }
            }

            return !SafeWaitPlus(WAIT_FOR_HERBS_TO_CLEAN, 0.35 * WAIT_FOR_HERBS_TO_CLEAN);
        }

        /// <summary>
        /// Waits for half an inventory of unfinished potions to be made from clean herbs and vials of water
        /// </summary>
        /// <returns>true if successful</returns>
        protected bool WaitToMakeUnfinishedPotions(bool countdown)
        {
            if (countdown)
            {
                CountDownItems(true, true);
                return true;
            }

            return !SafeWaitPlus(HALF_INVENTORY * MAKE_UNFINISHED_POTION_TIME, 200);
        }

        /// <summary>
        /// Waits for half an inventory of finished potions to be made from unfinished potions and secondary ingredients
        /// </summary>
        /// <returns>true if successful</returns>
        protected bool WaitToMakeFinishedPotions(bool countdown)
        {
            if (countdown)
            {
                CountDownItems(true, true);
                return true;
            }

            return !SafeWaitPlus(HALF_INVENTORY * MAKE_FINISHED_POTION_TIME, 200);
        }

        /// <summary>
        /// Opens the bank, withdraw 14 herbs + 14 vials of water, and closes the bank.
        /// </summary>
        /// <returns>true if successful</returns>
        protected bool WithdrawHerbsAndVials()
        {
            Bank bank;
            if (!Banking.OpenBank(out bank, 2)) { return false; }
            bank.DepositInventory();
            bank.WithdrawX(HerbBankSlot.X, HerbBankSlot.Y, HALF_INVENTORY);
            bank.WithdrawX(VialOfWaterBankSlot.X, VialOfWaterBankSlot.Y, HALF_INVENTORY);
            bank.Close();
            return true;
        }

        /// <summary>
        /// Cleans herbs if grimy and makes unfinished potions.
        /// </summary>
        /// <returns>true if successful</returns>
        protected bool MakeUnfinishedPotions(bool countdown)
        {
            if (true && !CleanHerbs()) { return false; }    //TODO Replace true with conditional based user settings
            Inventory.UseItemOnItem(SecondHalfInventorySlot, FirstHalfInventorySlot);
            if (SafeWaitPlus(750, 200)) { return false; }
            if (!ChatBoxSingleOptionMakeAll(RSClient)) { return false; }
            return WaitToMakeUnfinishedPotions(countdown);
        }

        /// <summary>
        /// Withdraw half an inventory of secondary ingredients.
        /// </summary>
        /// <returns>true if successful</returns>
        protected bool WithdrawSecondaryIngredients()
        {
            Bank bank;
            if (!Banking.OpenBank(out bank)) { return false; }
            bank.WithdrawX(SecondaryIngredientBankSlot.X, SecondaryIngredientBankSlot.Y, HALF_INVENTORY);
            bank.Close();
            return true;
        }

        /// <summary>
        /// Makes finished potions from unfinished potions and secondary ingredients.
        /// </summary>
        /// <returns>true if successful</returns>
        protected bool MakeFinishedPotions(bool countdown)
        {
            Inventory.UseItemOnItem(FirstHalfInventorySlot, SecondHalfInventorySlot);
            if (SafeWaitPlus(750, 200)) { return false; }
            if (!ChatBoxSingleOptionMakeAll(RSClient)) { return false; }
            return WaitToMakeFinishedPotions(countdown);
        }
    }
}
