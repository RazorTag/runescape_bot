using System.Drawing;

namespace RunescapeBot.BotPrograms
{
    public class Herblore : Use14On14
    {
        protected const int WAIT_FOR_HERBS_TO_CLEAN = 300;
        protected const int MAKE_POTION_TIME = 2 * BotRegistry.GAME_TICK;
        protected const int MAKE_UNFINISHED_POTION_TIME = BotRegistry.GAME_TICK;

        public Herblore(RunParams startParams, int makeTime) : base(startParams, makeTime)
        {
            UseOnInventorySlot = new Point(3, 3);
            UseWithInventorySlot = new Point(3, 2);
        }

        /// <summary>
        /// Cleans the herbs in the first 14 inventory slots
        /// </summary>
        /// <returns></returns>
        protected bool CleanHerbs()
        {
            //snake through the grimy herbs to clean them
            for (int x = 0; x < Inventory.INVENTORY_COLUMNS; x++)
            {
                Inventory.ClickInventory(x, 0, false);
            }
            for (int x = Inventory.INVENTORY_COLUMNS - 1; x >= 0; x--)
            {
                Inventory.ClickInventory(x, 1, false);
            }
            Inventory.ClickInventory(0, 2, false);
            Inventory.ClickInventory(0, 3, false);
            Inventory.ClickInventory(1, 3, false);
            Inventory.ClickInventory(1, 2, false);
            Inventory.ClickInventory(2, 2, false);
            Inventory.ClickInventory(3, 2, false);

            return !SafeWaitPlus(WAIT_FOR_HERBS_TO_CLEAN, 0.35 * WAIT_FOR_HERBS_TO_CLEAN);
        }
    }
}
