﻿using RunescapeBot.BotPrograms.Popups;
using RunescapeBot.UITools;
using System.Drawing;

namespace RunescapeBot.BotPrograms
{
    /// <summary>
    /// Moves between the Port Phasmatys bank and the Port Phasmatys furnace
    /// </summary>
    public class FurnacePhasmatys : BankPhasmatys
    {
        private const int WAIT_FOR_BANK_WINDOW_TIMEOUT = 15000;
        private const int CONSECUTIVE_FAILURES_ALLOWED = 5;
        private int failedRuns;
        private Point guessBankLocation;

        public FurnacePhasmatys(StartParams startParams) : base(startParams)
        {
            RunParams.Run = true;
            failedRuns = 0;
            ReadWindow();
            guessBankLocation = new Point(Center.X + 100, Center.Y);
        }

        protected override bool Execute()
        {
            if (failedRuns > CONSECUTIVE_FAILURES_ALLOWED)
            {
                return false;
            }

            //Move to the bank and open it
            if (!MoveToBank())
            {
                failedRuns++;
                return true;
            }
            Mouse.MoveMouse(guessBankLocation.X + RNG.Next(-20, 26), guessBankLocation.Y + RNG.Next(-30, 41), RSClient);    //Move the mouse to the neighborhood of where we expect the bank booth to be
            ClickBankBooth();

            //Refresh inventory to a bracelet mould and 27 gold bars
            if (StopFlag) { return false; }
            BankPopup = new Bank(RSClient);
            if (!BankPopup.WaitForPopup(WAIT_FOR_BANK_WINDOW_TIMEOUT))
            {
                failedRuns++;
                return true;
            }
            if (StopFlag) { return false; }
            BankPopup.DepositInventory();
            SafeWait(500);
            BankPopup.WithdrawOne(7, 0);
            SafeWait(500);
            BankPopup.WithdrawAll(6, 0);

            //Move to the furnace and use a gold bar on it
            if (StopFlag) { return false; }
            if (!MoveToFurnace())
            {
                failedRuns++;
                return true;
            }
            Inventory.ClickInventory(1, 0);
            ClickStationaryObject(Furnace, STATIONARY_OBJECT_TOLERANCE, 100, 12000, 1000);

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