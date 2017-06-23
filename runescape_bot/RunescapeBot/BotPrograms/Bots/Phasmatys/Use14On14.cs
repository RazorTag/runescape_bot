using RunescapeBot.BotPrograms.Popups;
using System.Drawing;

namespace RunescapeBot.BotPrograms
{
    public class Use14On14 : BankPhasmatys
    {
        private const int WAIT_FOR_BANK_WINDOW_TIMEOUT = 5000;
        private const int CONSECUTIVE_FAILURES_ALLOWED = 3;
        private int FailedRuns;
        protected int CraftingTime;
        protected Point UseOnInventorySlot;
        protected Point UseWithInventorySlot;
        protected Point UseOnBankSlot;
        protected Point UseWithBankSlot;

        /// <summary>
        /// Sets the time required to make 14 items
        /// </summary>
        /// <param name="startParams"></param>
        /// <param name="craftingTime">time needed to make the 14 items being crafted</param>
        public Use14On14(StartParams startParams, int craftingTime) : base(startParams)
        {
            this.CraftingTime = craftingTime;
            UseOnInventorySlot = new Point(0, 4);
            UseWithInventorySlot = new Point(0, 3);
            UseOnBankSlot = new Point(6, 0);
            UseWithBankSlot = new Point(7, 0);
        }

        protected override void Run()
        {
            Inventory.OpenInventory(true);

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
            if (FailedRuns > 1) { return false; }

            if (StopFlag) { return false; }
            if(WithdrawItems(UseWithBankSlot, UseOnBankSlot)
                && PreMake()
                && MakeItems(CraftingTime)
                && PostMake())
            {
                FailedRuns = 0;
                RunParams.Iterations -= 14;
                return true;
            }
            
            return false;
        }

        /// <summary>
        /// Withdraw two sets of 14 items from the bank
        /// </summary>
        /// <returns>true if successful</returns>
        protected bool WithdrawItems(Point useWith, Point useOn)
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
            BankPopup = new Bank(RSClient);
            if (!BankPopup.WaitForPopup(WAIT_FOR_BANK_WINDOW_TIMEOUT))
            {
                FailedRuns++;
                return false;
            }

            //waithdraw items from the bank
            BankPopup.DepositInventory();
            BankPopup.WithdrawN(useWith.X, useWith.Y);
            BankPopup.WithdrawN(useOn.X, useOn.Y);
            BankPopup.CloseBank();
            return true;
        }

        /// <summary>
        /// Make the 14 items
        /// </summary>
        /// <param name="craftTime">time to wait for he items to be made</param>
        /// <returns>true if successful</returns>
        protected bool MakeItems(int craftTime)
        {
            Inventory.UseItemOnItem(UseWithInventorySlot, UseOnInventorySlot, false);
            if (!BotUtilities.ChatBoxSingleOptionMakeAll(RSClient))
            {
                FailedRuns++;
                return true;
            }
            return !SafeWait(craftTime, 300.0);
        }

        /// <summary>
        /// Called after withdrawing items and before using one on the other to make them
        /// </summary>
        /// <returns></returns>
        protected virtual bool PreMake()
        {
            return true;
        }

        /// <summary>
        /// Called after the 14 items have been made
        /// </summary>
        /// <returns></returns>
        protected virtual bool PostMake()
        {
            return true;
        }
    }
}
