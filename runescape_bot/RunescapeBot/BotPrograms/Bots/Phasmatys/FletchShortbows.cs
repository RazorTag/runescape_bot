using RunescapeBot.BotPrograms.Popups;
using RunescapeBot.UITools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms
{
    class FletchShortbows : BankPhasmatys
    {
        private const int FLETCHING_TIME = 50000;
        private const int WAIT_FOR_BANK_WINDOW_TIMEOUT = 5000;
        private const int WAIT_FOR_FLETCHING_WINDOW_TIMEOUT = 5000;
        private const int WAIT_FOR_MAKEX_POPUP_TIMEOUT = 5000;
        private const int CONSECUTIVE_FAILURES_ALLOWED = 3;
        private int FailedRuns;
        private Point KnifeSlot;
        private Point LogSlot;
        private MakeXSlim MakeXSlim;

        public FletchShortbows(StartParams startParams) : base(startParams)
        {
            
        }

        protected override void Run()
        {
            Inventory.OpenInventory(true);
            KnifeSlot = new Point(0, 0);
            LogSlot = new Point(1, 0);
        }

        protected override bool Execute()
        {
            //Open the bank
            if (!ClickBankBooth())
            {
                if (!MoveToBank())
                {
                    FailedRuns++;
                    return true;
                }
                ClickBankBooth();
            }

            //Refresh inventory to a knife and 27 logs
            if (StopFlag) { return false; }
            BankPopup = new Bank(RSClient);
            if (!BankPopup.WaitForPopup(WAIT_FOR_BANK_WINDOW_TIMEOUT))
            {
                MoveToBank();
                FailedRuns++;
                return true;
            }
            BankPopup.DepositInventory();
            BankPopup.WithdrawOne(7, 0);
            BankPopup.WithdrawAll(6, 0);
            BankPopup.CloseBank();

            //Fletch the logs
            Inventory.UseItemOnItem(KnifeSlot, LogSlot, false);
            int left = 170;
            int right = 222;
            int top = ScreenHeight - 110;
            int bottom = ScreenHeight - 70;
            Point rightClick = new Point(RNG.Next(left, right), RNG.Next(top, bottom));
            RightClick(rightClick.X, rightClick.Y, 500);
            MakeXSlim = new MakeXSlim(rightClick.X, rightClick.Y, RSClient);
            if (!MakeXSlim.WaitForPopup(WAIT_FOR_MAKEX_POPUP_TIMEOUT))
            {
                FailedRuns++;
                return true;
            }
            MakeXSlim.MakeXItems(27);

            //Wait for the inventory to be fletched
            SafeWait(FLETCHING_TIME);

            return true;
        }
    }
}
