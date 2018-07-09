using RunescapeBot.BotPrograms.Popups;
using RunescapeBot.Common;
using RunescapeBot.ImageTools;
using System.Collections.Generic;
using System.Drawing;

namespace RunescapeBot.BotPrograms
{
    class Use1On27 : BankStand
    {
        public const int CUT_GEM_TIME = 2 * BotRegistry.GAME_TICK;

        private const int WAIT_FOR_FLETCHING_WINDOW_TIMEOUT = 5000;
        private const int WAIT_FOR_MAKEX_POPUP_TIMEOUT = 5000;
        private const int CONSECUTIVE_FAILURES_ALLOWED = 3;

        protected Point UseWithInventorySlot;
        protected Point UseOnInventorySlot;
        protected Point WithDrawBankSlot;

        public Use1On27(RunParams startParams, int makeTime) : base(startParams)
        {
            SingleMakeTime = makeTime;
            MakeQuantity = 28;
            UseWithInventorySlot = new Point(0, 0);
            // UseOnInventorySlot = new Point(1, 0);
            WithDrawBankSlot = new Point(7, 0);
            UseOnBankSlot = new Point(6, 0);
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
            Inventory.UseItemOnItem(UseWithInventorySlot, UseOnInventorySlot, false);
            SafeWaitPlus(500, 200);
            if (!ChatBoxSingleOptionMakeAll(RSClient))
            {
                return false;
            }

            //Wait for the inventory to be processed
            WatchNetflix(0);
            CountDownItems(true);
            SafeWaitPlus(0, 300);

            return true;
        }
    }
}
