using RunescapeBot.BotPrograms.Popups;
using System;
using System.Drawing;

namespace RunescapeBot.BotPrograms
{
    public class TanHides : BankStand
    {
        public const int TAN_HIDE_SPELL_TIME = 2 * BotRegistry.GAME_TICK;
        protected Point SpellSlot;
        protected Point HidesBankSlot;
        protected Point FirstHidesInventorySlot;
        protected bool spellbookClosed;

        public TanHides(RunParams runParams) : base(runParams)
        {
            SpellSlot = new Point(3, 3);
            HidesBankSlot = new Point(7, 0);
            FirstHidesInventorySlot = new Point(0, 0);
            RunParams.Iterations = (RunParams.Iterations / 5) * 5 ;
            spellbookClosed = true;
        }

        /// <summary>
        /// Deposits tanned 
        /// </summary>
        /// <param name="bank"></param>
        /// <returns>true if successful</returns>
        protected override bool WithdrawItems(Bank bank)
        {
            bank.DepositAll(FirstHidesInventorySlot);
            bank.WithdrawAll(HidesBankSlot.X, HidesBankSlot.Y);
            return true;
        }

        /// <summary>
        /// Use the Tan Leather spell 5 times to tan 25 hides
        /// </summary>
        /// <returns>true is successful</returns>
        protected override bool ProcessInventory()
        {
            if (spellbookClosed)
            {
                SafeWait(1500); //Make sure that the bank closes before trying to switch to the spellbook tab on the first run
                spellbookClosed = false;
            }

            int casts = Math.Min(5, RunParams.Iterations / 5);
            for (int i = 0; i < casts - 1; i++)
            {
                Inventory.ClickSpellbookLunar(SpellSlot.X, SpellSlot.Y);
                RunParams.Iterations -= 5;
                if (SafeWaitPlus(TAN_HIDE_SPELL_TIME + 150, 100)) { return false; }
            }
            //Do last cast with a shorter wait
            Inventory.ClickSpellbookLunar(SpellSlot.X, SpellSlot.Y);
            RunParams.Iterations -= 5;
            if (SafeWaitPlus(350, 150)) { return false; }

            return true;
        }
    }
}
