using RunescapeBot.ImageTools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms
{
    public class ButlerSawmill : CamelotHouse
    {
        protected Point InventoryCashSlot, InventoryLogSlot;
        protected Point BankCashSlot, BankLogSlot, BankPlankSlot;

        public ButlerSawmill(RunParams startParams) : base(startParams)
        {
            InventoryCashSlot = new Point(2, 0);
            InventoryLawRuneSlot = new Point(1, 0);
            InventoryLogSlot = new Point(0, 0);
            BankCashSlot = new Point(0, 7);
            BankLawRuneSlot = new Point(0, 6);
            BankLogSlot = new Point(0, 5);
            BankPlankSlot = new Point(0, 4);
        }

        /// <summary>
        /// Get the demon butler to take oak logs to the sawmill
        /// </summary>
        /// <returns>true if successful</returns>
        protected override bool Construct()
        {
            if (StopFlag || !CallServant())
            {
                return false;
            }

            if (StopFlag || !InitiateDemonDialog())
            {
                return false;
            }
            
            SafeWait(1000); //TODO detect repeat-last-task
            if (StopFlag) { return false; }
            Keyboard.WriteNumber(1);

            SafeWait(1000); //TODO detect sawmill-cost
            if (StopFlag) { return false; }
            Keyboard.Space();

            
            SafeWait(1000); //TODO detect convert-planks-accept
            if (StopFlag) { return false; }
            Keyboard.WriteNumber(1);

            SafeWait(1000); //TODO detect demon-aggrandizement
            if (StopFlag) { return false; }
            Keyboard.Space();

            return true;
        }


        protected bool InitiateDemonDialog()
        {
            int demonTries = 0;
            while (!StopFlag && !WaitForDialog(AnyDialog, 1500))
            {
                Blob demon;
                if (LocateObject(DemonHead, out demon, 100))
                {
                    LeftClick(demon.Center.X, demon.Center.Y);
                    SafeWaitPlus(1000, 100);
                }
                else
                {
                    if (++demonTries > 2)
                    {
                        return false;
                    }
                }
            }

            if (DemonButlerDialog())
            {
                if (!PayDemonButler())
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Goes through the dialog to pay the demon butler
        /// </summary>
        /// <returns>true if successful</returns>
        protected bool PayDemonButler()
        {
            return true;    //TODO pay the demon butler
        }

        /// <summary>
        /// Carefully un-notes logs using the bank chest
        /// </summary>
        /// <returns></returns>
        protected override bool Bank()
        {
            if (Inventory.SlotIsEmpty(InventoryLogSlot, true) || Inventory.SlotIsEmpty(InventoryLawRuneSlot) || Inventory.SlotIsEmpty(InventoryCashSlot))
            {
                return false;   //TODO restock at the GE
            }
            return UnNoteBankChest(InventoryLogSlot);
        }
    }
}
