using RunescapeBot.BotPrograms.Popups;
using RunescapeBot.UITools;
using System.Drawing;

namespace RunescapeBot.BotPrograms
{
    /// <summary>
    /// Moves between the Port Phasmatys bank and the Port Phasmatys furnace
    /// </summary>
    public class FurnacePhasmatys : BankPhasmatys
    {
        private const int CONSECUTIVE_FAILURES_ALLOWED = 5;
        private int failedRuns;

        public FurnacePhasmatys(RunParams startParams) : base(startParams)
        {
            RunParams.Run = true;
            failedRuns = 0;
        }

        protected override bool Execute()
        {
            if (failedRuns > CONSECUTIVE_FAILURES_ALLOWED) { return false; }

            //Move to the bank and open it
            if (!MoveToBankPhasmatys() || !ClickBankBooth())
            {
                failedRuns++;
                return true;
            }

            //Refresh inventory to a bracelet mould and 27 gold bars
            if (StopFlag) { return false; }
            BankPopup = new Bank(RSClient, Inventory, Keyboard);
            if (!BankPopup.WaitForPopup(BotUtilities.WAIT_FOR_BANK_WINDOW_TIMEOUT))
            {
                failedRuns++;
                return true;
            }
            if (StopFlag) { return false; }
            BankPopup.DepositInventory();
            SafeWaitPlus(500, 200);
            BankPopup.WithdrawOne(7, 0);
            SafeWaitPlus(500, 150);
            BankPopup.WithdrawAll(6, 0);

            //Move to the furnace and use a gold bar on it
            if (StopFlag) { return false; }
            if (!MoveToFurnace(6000))
            {
                failedRuns++;
                return true;
            }
            Inventory.ClickInventory(1, 0, false);
            HandEye.ClickStationaryObject(Furnace, STATIONARY_OBJECT_TOLERANCE, 100, 12000, 1000);

            //Do the bot-specific actions at the furnace
            if (StopFlag) { return false; }
            if (!FurnaceActions())
            {
                failedRuns++;
                return true;
            }

            failedRuns = 0;
            return true;
        }

        /// <summary>
        /// Do furnace things
        /// </summary>
        /// <returns>Return true if the actions succeed, false if they fail</returns>
        protected virtual bool FurnaceActions()
        {
            return true;
        }
    }
}
