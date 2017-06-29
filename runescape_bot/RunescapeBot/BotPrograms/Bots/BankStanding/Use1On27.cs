using RunescapeBot.BotPrograms.Popups;
using RunescapeBot.Common;
using RunescapeBot.ImageTools;
using System.Collections.Generic;
using System.Drawing;

namespace RunescapeBot.BotPrograms
{
    class Use1On27 : GenericBank
    {
        private const int WAIT_FOR_FLETCHING_WINDOW_TIMEOUT = 5000;
        private const int WAIT_FOR_MAKEX_POPUP_TIMEOUT = 5000;
        private const int CONSECUTIVE_FAILURES_ALLOWED = 3;

        protected Point UseWithInventorySlot;
        protected Point UseOnInventorySlot;
        protected Point UseWithBankSlot;
        protected Point UseOnBankSlot;

        public Use1On27(StartParams startParams, int makeTime) : base(startParams, makeTime) { }

        protected override void Run()
        {
            Inventory.OpenInventory(true);
            UseWithInventorySlot = new Point(0, 0);
            UseOnInventorySlot = new Point(1, 0);
            UseWithBankSlot = new Point(7, 0);
            UseOnBankSlot = new Point(6, 0);
        }

        protected override bool Execute()
        {
            //Open the bank
            Bank bankPopup;
            if (!OpenBank(out bankPopup))
            {
                if (!MoveToBank())
                {
                    FailedRuns++;
                    return false;
                }
                OpenBank(out bankPopup);
            }

            //Refresh inventory to a knife and 27 logs
            if (StopFlag) { return false; }

            if (!bankPopup.WaitForPopup(WAIT_FOR_BANK_WINDOW_TIMEOUT))
            {
                MoveToBank();
                FailedRuns++;
                return true;
            }
            bankPopup.DepositInventory();
            bankPopup.WithdrawOne(7, 0);
            bankPopup.WithdrawAll(6, 0);
            bankPopup.CloseBank();

            //Fletch the logs
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

            //Wait for the inventory to be fletched
            SafeWaitPlus(MakeTime, 1200);

            return true;
        }
    }
}
