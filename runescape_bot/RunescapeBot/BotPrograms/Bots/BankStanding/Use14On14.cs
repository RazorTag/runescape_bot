using RunescapeBot.BotPrograms.Popups;
using RunescapeBot.ImageTools;
using System.Drawing;
using System.Collections.Generic;

namespace RunescapeBot.BotPrograms
{
    public class Use14On14 : GenericBank
    {
        private const int CONSECUTIVE_FAILURES_ALLOWED = 3;

        protected Point UseOnInventorySlot;
        protected Point UseWithInventorySlot;
        protected Point UseOnBankSlot;
        protected Point UseWithBankSlot;

        /// <summary>
        /// Sets the time required to make 14 items
        /// </summary>
        /// <param name="startParams"></param>
        /// <param name="makeTime">time needed to make the 14 items being crafted</param>
        public Use14On14(RunParams startParams, int makeTime) : base(startParams, makeTime)
        {
            UseOnInventorySlot = new Point(0, 4);
            UseWithInventorySlot = new Point(0, 3);
            UseOnBankSlot = new Point(6, 0);
            UseWithBankSlot = new Point(7, 0);
        }

        protected override void Run()
        {
            //ReadWindow();
            //bool[,] bankBooth = ColorFilter(ColorFilters.BankBoothVarrockWest());
            //DebugUtilities.TestMask(Bitmap, ColorArray, ColorFilters.BankBoothVarrockWest(), bankBooth, "C:\\Projects\\Roboport\\test_pictures\\mask_tests\\", "bankBooth");

            Inventory.OpenInventory(true);

            //Open the bank
            Bank bankPopup;
            if (!OpenBank(out bankPopup))
            {
                if (!MoveToBank())
                {
                    return;
                }
                if (!OpenBank(out bankPopup))
                {
                    return;
                }
            }
            bankPopup.DepositInventory();
            bankPopup.WithdrawX(UseWithBankSlot.X, UseWithBankSlot.Y, 14);
            bankPopup.CloseBank();
        }

        /// <summary>
        /// Makes potions from scratch
        /// </summary>
        /// <returns></returns>
        protected override bool Execute()
        {
            if (FailedRuns >= 2) { return false; }

            if (StopFlag) { return false; }
            if(WithdrawItems(UseWithBankSlot, UseOnBankSlot)
                && PreMake()
                && MakeItems(MakeTime)
                && PostMake())
            {
                FailedRuns = 0;
                RunParams.Iterations -= 14;
                return true;
            }

            FailedRuns++;
            return false;
        }

        /// <summary>
        /// Withdraw two sets of 14 items from the bank
        /// </summary>
        /// <returns>true if successful</returns>
        protected bool WithdrawItems(Point useWith, Point useOn)
        {
            //Open the bank
            Bank bankPopup;
            if (!OpenBank(out bankPopup))
            {
                if (!MoveToBank())
                {
                    return true;
                }
                OpenBank(out bankPopup);
            }
            Bank BankPopup = new Bank(RSClient);
            if (!BankPopup.WaitForPopup(WAIT_FOR_BANK_WINDOW_TIMEOUT))
            {
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
