using RunescapeBot.BotPrograms.Popups;
using RunescapeBot.Common;
using RunescapeBot.ImageTools;
using System.Collections.Generic;
using System.Drawing;

namespace RunescapeBot.BotPrograms
{
    class Use1On27 : BankStand
    {
        private const int WAIT_FOR_FLETCHING_WINDOW_TIMEOUT = 5000;
        private const int WAIT_FOR_MAKEX_POPUP_TIMEOUT = 5000;
        private const int CONSECUTIVE_FAILURES_ALLOWED = 3;

        protected Point UseWithInventorySlot;
        protected Point UseOnInventorySlot;
        protected Point UseWithBankSlot;
        protected Point UseOnBankSlot;

        public Use1On27(RunParams startParams, int makeTime) : base(startParams)
        {
            SingleMakeTime = makeTime;
            MakeQuantity = 27;
            UseWithInventorySlot = new Point(0, 0);
            UseOnInventorySlot = new Point(1, 0);
            UseWithBankSlot = new Point(7, 0);
            UseOnBankSlot = new Point(6, 0);
        }

        protected override bool Run()
        {
            Inventory.OpenInventory(true);

            return true;
        }

        /// <summary>
        /// Withdraw two sets of 14 items from the bank
        /// </summary>
        /// <returns>true if successful</returns>
        protected override bool WithdrawItems(Bank bank)
        {
            if (RunParams.Iterations < 14) { return false; }

            bank.DepositInventory();
            bank.WithdrawOne(7, 0);
            bank.WithdrawAll(6, 0);
            return true;
        }

        /// <summary>
        /// Use the tool on the other 27 items
        /// </summary>
        /// <returns>true if successful</returns>
        protected override bool ProcessInventory()
        {
            Inventory.UseItemOnItem(UseWithInventorySlot, UseOnInventorySlot, false);
            int left = 170;
            int right = 222;
            int top = ScreenHeight - 110;
            int bottom = ScreenHeight - 70;
            Point rightClick = Probability.GaussianRectangle(left, right, top, bottom);
            RightClick(rightClick.X, rightClick.Y);
            MakeXSlim MakeXSlim = new MakeXSlim(rightClick.X, rightClick.Y, RSClient);
            if (!MakeXSlim.WaitForPopup(WAIT_FOR_MAKEX_POPUP_TIMEOUT))
            {
                FailedRuns++;
                return true;
            }
            MakeXSlim.MakeXItems(27);

            //Wait for the inventory to be processed
            CountDownItems(true);
            SafeWaitPlus(0, 300);

            return true;
        }
    }
}
