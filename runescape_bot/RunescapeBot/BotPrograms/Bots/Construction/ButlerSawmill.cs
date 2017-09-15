using RunescapeBot.Common;
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
        protected Point InventoryCashSlot, InventoryLogSlot, FirstLogSlot;
        protected Point BankCashSlot, BankLogSlot, BankPlankSlot;

        int DemonHeadSize { get { return ArtifactSize(0.0001); } }

        public ButlerSawmill(RunParams startParams) : base(startParams)
        {
            InventoryCashSlot = new Point(2, 0);
            InventoryLawRuneSlot = new Point(1, 0);
            InventoryLogSlot = new Point(0, 0);
            FirstLogSlot = new Point(3, 0);
            BankCashSlot = new Point(0, 7);
            BankLawRuneSlot = new Point(0, 6);
            BankLogSlot = new Point(0, 5);
            BankPlankSlot = new Point(0, 4);
        }

        protected override bool Run()
        {
            base.Run();

            //ConvertPlanksDialog();
            //PayButler();

            return true;
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

            bool skipRepeatLastDialog;
            if (StopFlag || !InitiateDemonDialog(out skipRepeatLastDialog))
            {
                return false;
            }

            if (!skipRepeatLastDialog)
            {
                if (StopFlag || !WaitForDialog(RepeatLastTaskDialog))
                {
                    return false;
                }
                Keyboard.WriteNumber(1);
            }

            if (StopFlag || !WaitForDialog(DemonButlerDialog))   //detect sawmill cost foresight
            {
                return false;
            }
            Keyboard.Space();

            if (StopFlag || !WaitForDialog(ConvertPlanksDialog))
            {
                return false;
            }
            Keyboard.WriteNumber(1);

            if (StopFlag || !WaitForDialog(DemonButlerDialog))   //detect demon-aggrandizement
            {
                return false;
            }
            Keyboard.Space();

            return true;
        }

        /// <summary>
        /// After calling the demon, gets the point where you can choose to send 25 logs
        /// </summary>
        /// <returns></returns>
        protected bool InitiateDemonDialog(out bool skipRepeatLastDialog)
        {
            skipRepeatLastDialog = false;
            int demonTries = 0;
            while (!StopFlag && !WaitForDialog(AnyDialog, 1500))
            {
                Blob demon;
                if (LocateObject(DemonHead, out demon, 1))
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
                if (WaitingForCommand())
                {
                    if (StartSawmillTask())
                    {
                        skipRepeatLastDialog = true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (!PayButler())
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Sets up a sawmill task for the butler
        /// Assumes that the butler is already waiting on a command
        /// </summary>
        /// <returns>true if successful</returns>
        protected bool StartSawmillTask()
        {
            Inventory.ClickInventory(FirstLogSlot);
            Blob demon;
            if (!LocateObject(DemonHead, out demon, 100))
            {
                return false;
            }
            LeftClick(demon.Center.X, demon.Center.Y);

            if (!WaitForDialog(DemonButlerDialog))
            {
                return false;
            }
            Keyboard.Space();

            if (!WaitForDialog(SelectAnOptionDialog))
            {
                return false;
            }
            Keyboard.WriteNumber(1);

            BotUtilities.WaitForEnterAmount(RSClient, 5000);
            Keyboard.WriteNumber(25);

            return true;
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

        /// <summary>
        /// Determines if the dialog box is showing the "Repeat last task?" title
        /// </summary>
        /// <returns>true if the last task dialog is showing</returns>
        protected bool ConvertPlanksDialog()
        {
            long titleHash = DialogTitleHash();
            return Numerical.CloseEnough(4033845, titleHash, _titleHashPrecision);
        }
    }
}
