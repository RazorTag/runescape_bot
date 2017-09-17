using RunescapeBot.Common;
using RunescapeBot.ImageTools;
using RunescapeBot.UITools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RunescapeBot.BotPrograms.Popups;
using System.Diagnostics;

namespace RunescapeBot.BotPrograms
{
    public class ButlerSawmill : CamelotHouse
    {
        protected Point InventoryCashSlot, InventoryLogSlot, FirstLogSlot;
        protected Point BankCashSlot, BankLogSlot, BankPlankSlot;

        RGBHSBRange YellowMouseOverText;

        int DemonHeadSize { get { return ArtifactSize(0.0001); } }

        public ButlerSawmill(RunParams startParams) : base(startParams)
        {
            InventoryCashSlot = new Point(2, 0);
            InventoryLawRuneSlot = new Point(1, 0);
            InventoryLogSlot = new Point(0, 0);
            FirstLogSlot = new Point(3, 0);
            BankCashSlot = new Point(7, 0);
            BankLawRuneSlot = new Point(6, 0);
            BankLogSlot = new Point(5, 0);
            BankPlankSlot = new Point(4, 0);
            YellowMouseOverText = RGBHSBRangeFactory.MouseoverTextNPC();
        }

        protected override bool Run()
        {
            base.Run();

            //ReadWindow();
            //ConvertPlanksDialog();
            //PayButler();
            //RefreshInventory();
            //InitiateDemonDialog();
            //Construct();

            return true;
        }

        /// <summary>
        /// Get the demon butler to take oak logs to the sawmill
        /// </summary>
        /// <returns>true if successful</returns>
        protected override bool Construct()
        {
            Inventory.OpenOptions(false);
            Point houseOptions = Probability.GaussianCircle(HouseOptionsLocation(), 5, 0, 360, 12);
            Mouse.MoveMouse(houseOptions.X, houseOptions.Y, RSClient);
            if (WaitForTeleport()) { return false; }
            if (!WaitForDialog(IsAtHouse))
            {
                return false;
            }

            if (StopFlag || !CallServant())
            {
                return false;
            }
            if (WaitForDialog(InventoryActive, 1000))
            {
                Inventory.OpenInventory();
            }

            if (StopFlag || !InitiateDemonDialog())
            {
                return false;
            }

            long lastBody = 0;
            long dialogBody = 0;
            Stopwatch watch = new Stopwatch();
            watch.Start();
            while (!StopFlag && (watch.ElapsedMilliseconds < 30000) && !Inventory.SlotIsEmpty(Inventory.INVENTORY_CAPACITY - 1))
            {
                dialogBody = DialogHash();

                if (dialogBody != lastBody)
                {
                    if (ContinueBar())
                    {
                        Keyboard.Space();
                    }
                    else
                    {
                        if (ProcessDialog(dialogBody))
                        {
                            lastBody = dialogBody;
                        }
                    }
                }
                SafeWait(200);
            }
            return Inventory.SlotIsEmpty(Inventory.INVENTORY_CAPACITY - 1, true);
        }

        /// <summary>
        /// Responds to a given dialog body hash
        /// </summary>
        /// <param name="bodyHash">total number of text pixels in a dialog box body</param>
        protected bool ProcessDialog(long bodyCount)
        {
            const double hashPrecision = 0.00001;

            if (Numerical.CloseEnough(16029632, bodyCount, hashPrecision))   //"Yes<br>No" - title:"Convert the planks for 2500 coins?"
            {
                Keyboard.WriteNumber(1);
            }
            else if (Numerical.CloseEnough(16029168, bodyCount, hashPrecision))   //"Yes<br>No" - title:"Convert the planks for 6250 coins?"
            {
                Keyboard.WriteNumber(1);
            }
            else if (Numerical.CloseEnough(16023580, bodyCount, hashPrecision))   //"Yes<br>No" - title:"Convert the planks for 12500 coins?"
            {
                Keyboard.WriteNumber(1);
            }
            else if (Numerical.CloseEnough(16021762, bodyCount, hashPrecision))   //"Yes<br>No" - title:"Convert the planks for 37500 coins?"
            {
                Keyboard.WriteNumber(1);
            }
            else if (Numerical.CloseEnough(15831076, bodyCount, hashPrecision))  //"Take to sawmill: 25 x logs<br>"Something else..."
            {
                Keyboard.WriteNumber(1);
            }
            else if (Numerical.CloseEnough(15801599, bodyCount, hashPrecision))   //"Take to sawmill: 25 x Oak logs<br>"Something else..."
            {
                Keyboard.WriteNumber(1);
            }
            else if (Numerical.CloseEnough(15790514, bodyCount, hashPrecision))   //"Take to sawmill: 25 x Teak logs<br>"Something else..."
            {
                Keyboard.WriteNumber(1);
            }
            else if (Numerical.CloseEnough(15741143, bodyCount, hashPrecision))   //"Take to sawmill: 25 x Mahogany logs<br>"Something else..."
            {
                Keyboard.WriteNumber(1);
            }
            else if (Numerical.CloseEnough(15774502, bodyCount, hashPrecision))   //"Serve...<br>Go to the bank...<br>Go to the sawmill<br>Greet guests<br>You're fired"
            {
                StartSawmillTask();
            }
            else if (Numerical.CloseEnough(15989089, bodyCount, hashPrecision))   //"Sawmill<br>Bank<br>Never mind"
            {
                Keyboard.WriteNumber(1);
            }
            else if (Numerical.CloseEnough(15959844, bodyCount, hashPrecision) || Numerical.CloseEnough(16167022, bodyCount, hashPrecision))  //"Enter amount:"
            {
                Keyboard.WriteNumber(25);
                SafeWaitPlus(50, 25);
                Keyboard.Enter();
            }
            else if (Numerical.CloseEnough(15768332, bodyCount, hashPrecision))  //"Okay, here's 10,000 coins.<br>I'll pay you later.<br>You're fired!"
            {
                Keyboard.WriteNumber(1);
            }
            else
            {
                return false;
            }                

            return true;    //one of the conditions was met
        }

        /// <summary>
        /// Looks for the brown packpack that normally appears on the inventory tab icon
        /// </summary>
        /// <returns>true if found</returns>
        protected bool InventoryActive()
        {
            Blob backpack;
            int radius = 15;
            int x = Inventory.INVENTORY_TAB_OFFSET_RIGHT;
            int y = Inventory.INVENTORY_TAB_OFFSET_BOTTOM;
            int left = ScreenWidth - x - radius;
            int right = left + 2 * radius;
            int top = ScreenHeight - y - radius;
            int bottom = top + 2 * radius;
            return LocateObject(BackpackBrown, out backpack, left, right, top, bottom, 5);
        }

        /// <summary>
        /// After calling the demon, makes sure that conversation with the demon has started
        /// </summary>
        /// <returns>true if successful</returns>
        protected bool InitiateDemonDialog()
        {
            int demonTries = 0;
            while (!StopFlag && !WaitForDialog(AnyDialog, 1000))
            {
                Blob demon;
                if (LocateStationaryObject(DemonHead, out demon, ArtifactLength(0.015), 3000, ArtifactSize(0.00005), ArtifactSize(0.0005)))
                {
                    Mouse.MoveMouse(demon.Center.X, demon.Center.Y, RSClient);
                    if (WaitForMouseOverText(YellowMouseOverText))
                    {
                        LeftClick(demon.Center.X, demon.Center.Y);
                        SafeWait(500);
                    }
                }
                else
                {
                    if (++demonTries > 2)
                    {
                        return false;
                    }
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
            return true;
        }

        /// <summary>
        /// Carefully un-notes logs using the bank chest
        /// </summary>
        /// <returns>true if successful</returns>
        protected override bool Bank()
        {
            Inventory.OpenInventory();
            Inventory.ClickInventory(InventoryLogSlot);
            Mouse.MoveMouse(Center.X, Center.Y, RSClient);
            if (WaitForTeleport()) { return false; }

            if (!InventoryIsReady())
            {
                RefreshInventory();
                if (SafeWait(1000)) { return false; }
                if (!InventoryIsReady())
                {
                    return false;   //TODO restock at the GE
                }
            }

            const int maxUnNoteTries = 5;
            for (int i = 0; i < maxUnNoteTries; i++)
            {
                if (StopFlag) { return false; }
                if (UnNoteBankChest(InventoryLogSlot, i == 0))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Determines if the inventory look like it is ready
        /// </summary>
        /// <returns>true if the inventory appears to be set up correctly</returns>
        protected override bool InventoryIsReady()
        {
            if (Inventory.SlotIsEmpty(InventoryLogSlot, true) || Inventory.SlotIsEmpty(InventoryLawRuneSlot) || Inventory.SlotIsEmpty(InventoryCashSlot))
            {
                return false;   //TODO restock at the GE
            }

            for (int i = 3; i < Inventory.INVENTORY_CAPACITY; i++)
            {
                if (!Inventory.SlotIsEmpty(i))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Deposits the entire inventory and withdraws all of the needed items
        /// </summary>
        /// <returns></returns>
        protected override bool RefreshInventory()
        {
            Point bankChest;
            if (!BankChestClickLocation(out bankChest))
            {
                return false;
            }

            LeftClick(bankChest.X, bankChest.Y);
            Bank bankPopup = new Bank(RSClient);
            if (!bankPopup.WaitForPopup(BotUtilities.WAIT_FOR_BANK_WINDOW_TIMEOUT))
            {
                return false;
            }

            bankPopup.DepositInventory();
            if (SafeWaitPlus(500, 200)) { return false; }
            bankPopup.WithdrawAsNotes();
            bankPopup.WithdrawAll(BankLogSlot.X, BankLogSlot.Y);
            if (SafeWaitPlus(500, 200)) { return false; }
            bankPopup.WithdrawAll(BankLawRuneSlot.X, BankLawRuneSlot.Y);
            if (SafeWaitPlus(500, 200)) { return false; }
            bankPopup.WithdrawAll(BankCashSlot.X, BankCashSlot.Y);
            bankPopup.CloseBank();

            return InventoryIsReady();
        }
    }
}
