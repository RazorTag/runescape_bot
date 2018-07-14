using RunescapeBot.BotPrograms.Popups;
using RunescapeBot.Common;
using RunescapeBot.ImageTools;
using System.Collections.Generic;
using System.Drawing;

namespace RunescapeBot.BotPrograms
{
    class ClickAll28 : BankStand
    {
        public const int HERB_CLEAN_TIME = 2 * BotRegistry.GAME_TICK;

        private const int WAIT_FOR_FLETCHING_WINDOW_TIMEOUT = 5000;
        private const int WAIT_FOR_MAKEX_POPUP_TIMEOUT = 5000;
        private const int CONSECUTIVE_FAILURES_ALLOWED = 3;

        protected Point WithDrawBankSlot;

        public ClickAll28(RunParams startParams, int makeTime) : base(startParams)
        {
            SingleMakeTime = makeTime;
            MakeQuantity = 28;
            WithDrawBankSlot = new Point(7, 0);
        }

        protected override bool Run()
        {
            Inventory.OpenInventory(true);

            return true;
        }

        /// <summary>
        /// Withdraw one item and 27 of another item
        /// </summary>
        /// <returns>true if successful</returns>
        protected override bool WithdrawItems(Bank bank)
        {
            bank.DepositInventory();
            bank.WithdrawAll(WithDrawBankSlot.X, WithDrawBankSlot.Y);
            // bank.WithdrawAll(UseOnBankSlot.X, UseOnBankSlot.Y);
            return true;
        }

        /// <summary>
        /// Use the tool on the other 27 items
        /// </summary>
        /// <returns>true if successful</returns>
        protected override bool ProcessInventory()
        {
            for (int i = 0 ; i < 28 ; i++) {
                Inventory.ClickInventory(Inventory.InventoryIndexToCoordinates(i), false);
                SafeWait(500);
            }
            // SafeWaitPlus(500, 200);
            // if (!ChatBoxSingleOptionMakeAll(RSClient))
            // {
            //     return false;
            // }

            //Wait for the inventory to be processed
            WatchNetflix(0);
            CountDownItems(true);
            SafeWaitPlus(0, 1500);

            return true;
        }
    }
}
