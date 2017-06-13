using RunescapeBot.BotPrograms.Popups;
using System.Drawing;

namespace RunescapeBot.BotPrograms
{
    class StringBows : BankPhasmatys
    {
        private const int FLETCHING_TIME = 16800;
        private const int WAIT_FOR_BANK_WINDOW_TIMEOUT = 5000;
        private const int CONSECUTIVE_FAILURES_ALLOWED = 3;
        private int FailedRuns;
        private Point BowSlot;
        private Point StringSlot;

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
            if (FailedRuns > 1)
            {
                return false;
            }

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
            if (!Utilities.ChatBoxSingleOptionMakeAll(RSClient))
            {
                FailedRuns++;
                return true;
            }

            //Wait for the inventory to be fletched
            SafeWait(FLETCHING_TIME + RNG.Next(-200, 201));

            FailedRuns = 0;
            return true;
        }
    }
}
