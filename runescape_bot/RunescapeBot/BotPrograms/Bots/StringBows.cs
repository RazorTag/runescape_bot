using RunescapeBot.BotPrograms.Popups;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms
{
    class StringBows : BankPhasmatys
    {
        private const int FLETCHING_TIME = 16800;
        private const int WAIT_FOR_BANK_WINDOW_TIMEOUT = 5000;
        private const int WAIT_FOR_FLETCHING_WINDOW_TIMEOUT = 5000;
        private const int WAIT_FOR_MAKEX_POPUP_TIMEOUT = 5000;
        private const int CONSECUTIVE_FAILURES_ALLOWED = 3;
        private int FailedRuns;
        private Point BowSlot;
        private Point StringSlot;
        private MakeAllSlim MakeAllSlim;

        public StringBows(StartParams startParams) : base(startParams)
        {

        }

        protected override void Run()
        {
            Inventory.OpenInventory(true);
            BowSlot = new Point(0, 4);
            StringSlot = new Point(0, 3);

            //Open the bank
            if (!ClickBankBooth())
            {
                if (!MoveToBank())
                {
                    return;
                }
                ClickBankBooth();
            }
            Bank bank = new Bank(RSClient);
            if (bank.WaitForPopup(WAIT_FOR_BANK_WINDOW_TIMEOUT))
            {
                bank.DepositInventory();
                bank.WithdrawX(7, 0, 14);
                bank.CloseBank();
            }
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
                FailedRuns++;
                return true;
            }
            BankPopup.DepositInventory();
            BankPopup.WithdrawN(7, 0);
            BankPopup.WithdrawN(6, 0);
            BankPopup.CloseBank();

            //String the bows
            Inventory.UseItemOnItem(StringSlot, BowSlot, false);
            int left = 230;
            int right = 282;
            int top = ScreenHeight - 110;
            int bottom = ScreenHeight - 70;
            Point rightClick = new Point(RNG.Next(left, right), RNG.Next(top, bottom));
            RightClick(rightClick.X, rightClick.Y, 500);
            MakeAllSlim = new MakeAllSlim(rightClick.X, rightClick.Y, RSClient);
            if (!MakeAllSlim.WaitForPopup(WAIT_FOR_MAKEX_POPUP_TIMEOUT))
            {
                FailedRuns++;
                return true;
            }
            MakeAllSlim.MakeAllItems();

            //Wait for the inventory to be fletched
            SafeWait(FLETCHING_TIME);

            return true;
        }
    }
}
