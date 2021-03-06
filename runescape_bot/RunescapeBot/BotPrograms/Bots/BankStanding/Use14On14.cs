﻿using RunescapeBot.BotPrograms.Popups;
using RunescapeBot.ImageTools;
using System.Drawing;
using System.Collections.Generic;
using RunescapeBot.BotPrograms.Settings;

namespace RunescapeBot.BotPrograms
{
    public class Use14On14 : BankStand
    {
        public const int STRING_BOW_TIME = 2 * BotRegistry.GAME_TICK;
        public const int FLETCH_BOW_TIME = 3 * BotRegistry.GAME_TICK;
        private const int CONSECUTIVE_FAILURES_ALLOWED = 3;

        protected Use14On14SettingsData UserSelections;

        protected Point UseOnInventorySlot;
        protected Point UseWithInventorySlot;
        protected Point UseOnBankSlot;
        protected Point UseWithBankSlot;

        /// <summary>
        /// Sets the time required to make 14 items
        /// </summary>
        /// <param name="startParams"></param>
        /// <param name="makeTime">time needed to make the 14 items being crafted</param>
        public Use14On14(RunParams startParams, int makeTime) : base(startParams)
        {
            UserSelections = startParams.CustomSettingsData.Use14On14;
            if (UserSelections.MakeTime > 0)
            {
                SingleMakeTime = UserSelections.MakeTime;
            }
            else
            {
                SingleMakeTime = makeTime;
            }

            MakeQuantity = Inventory.INVENTORY_CAPACITY / 2;
            UseOnInventorySlot = new Point(0, 4);
            UseWithInventorySlot = new Point(0, 3);
            UseOnBankSlot = new Point(6, 0);
            UseWithBankSlot = new Point(7, 0);
        }

        protected override bool Run()
        {
            //ReadWindow();
            //bool[,] bankBooth = ColorFilter(ColorFilters.BankBoothVarrockWest());
            //DebugUtilities.TestMask(Bitmap, ColorArray, ColorFilters.BankBoothVarrockWest(), bankBooth, "C:\\Projects\\Roboport\\test_pictures\\mask_tests\\", "bankBooth");

            return true;
        }

        /// <summary>
        /// Withdraw two sets of 14 items from the bank
        /// </summary>
        /// <returns>true if successful</returns>
        protected override bool WithdrawItems(Bank bank)
        {
            if (RunParams.Iterations < Inventory.INVENTORY_CAPACITY / 2) { return false; }

            bank.DepositInventory();
            bank.WithdrawX(UseWithBankSlot.X, UseWithBankSlot.Y, Inventory.INVENTORY_CAPACITY / 2);
            bank.WithdrawX(UseOnBankSlot.X, UseOnBankSlot.Y, Inventory.INVENTORY_CAPACITY / 2);
            return true;
        }

        /// <summary>
        /// Use the first 14 items on the second 14 items
        /// </summary>
        /// <returns>true if successful</returns>
        protected override bool ProcessInventory()
        {
            if (PreMake() && MakeItems(true) && PostMake())
            {
                FailedRuns = 0;
                return true;
            }
            else
            {
                return ++FailedRuns > 2;
            }
        }

        /// <summary>
        /// Make the 14 items
        /// </summary>
        /// <param name="craftTime">time to wait for he items to be made</param>
        /// <returns>true if successful</returns>
        protected bool MakeItems(bool finishedProduct)
        {
            Inventory.UseItemOnItem(UseWithInventorySlot, UseOnInventorySlot, false);
            SafeWaitPlus(500, 150);
            if (!ChatBoxSingleOptionMakeAll(RSClient))
            {
                return false;
            }

            //Wait for the inventory to be processed
            WatchNetflix(0);
            CountDownItems(finishedProduct);
            return !SafeWaitPlus(0, 300);
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
