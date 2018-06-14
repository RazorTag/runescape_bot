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
        const double _hashPrecision = 0.00001;

        protected Point InventoryCashSlot, InventoryLogSlot, FirstLogSlot;
        protected Point BankCashSlot, BankLogSlot, BankPlankSlot;

        RGBHSBRange YellowMouseOverText;

        int DemonHeadSize { get { return ArtifactArea(0.0001); } }

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
            Mouse.Move(houseOptions.X, houseOptions.Y, RSClient);
            if (WaitForTeleport()) { return false; }
            if (!WaitFor(IsAtHouse) || !CallServant() || !InitiateDemonDialog())
            {
                return false;
            }

            long lastBody = 0;
            long dialogBody = 0;
            Stopwatch watch = new Stopwatch();
            watch.Start();
            while (!StopFlag && (watch.ElapsedMilliseconds < 30000) && AnyDialog(true))
            {
                dialogBody = DialogHash(false);

                if (!Numerical.CloseEnough(dialogBody, lastBody, _hashPrecision))
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
            return !AnyDialog(false);
        }

        /// <summary>
        /// Responds to a given dialog body hash
        /// </summary>
        /// <param name="bodyHash">total number of text pixels in a dialog box body</param>
        protected bool ProcessDialog(long bodyCount)
        {
            if (Numerical.CloseEnough(16029632, bodyCount, _hashPrecision))   //"Yes<br>No" - title:"Convert the planks for 2500 coins?"
            {
                Keyboard.WriteNumber(1);
            }
            else if (Numerical.CloseEnough(16029168, bodyCount, _hashPrecision))   //"Yes<br>No" - title:"Convert the planks for 6250 coins?"
            {
                Keyboard.WriteNumber(1);
            }
            else if (Numerical.CloseEnough(16023580, bodyCount, _hashPrecision))   //"Yes<br>No" - title:"Convert the planks for 12500 coins?"
            {
                Keyboard.WriteNumber(1);
            }
            else if (Numerical.CloseEnough(16021762, bodyCount, _hashPrecision))   //"Yes<br>No" - title:"Convert the planks for 37500 coins?"
            {
                Keyboard.WriteNumber(1);
            }
            else if (Numerical.CloseEnough(15831076, bodyCount, _hashPrecision))  //"Take to sawmill: 25 x logs<br>"Something else..."
            {
                Keyboard.WriteNumber(1);
            }
            else if (Numerical.CloseEnough(15801599, bodyCount, _hashPrecision))   //"Take to sawmill: 25 x Oak logs<br>"Something else..."
            {
                Keyboard.WriteNumber(1);
            }
            else if (Numerical.CloseEnough(15790514, bodyCount, _hashPrecision))   //"Take to sawmill: 25 x Teak logs<br>"Something else..."
            {
                Keyboard.WriteNumber(1);
            }
            else if (Numerical.CloseEnough(15741143, bodyCount, _hashPrecision))   //"Take to sawmill: 25 x Mahogany logs<br>"Something else..."
            {
                Keyboard.WriteNumber(1);
            }
            else if (Numerical.CloseEnough(15774502, bodyCount, _hashPrecision))   //"Serve...<br>Go to the bank...<br>Go to the sawmill<br>Greet guests<br>You're fired"
            {
                StartSawmillTask();
            }
            else if (Numerical.CloseEnough(15989089, bodyCount, _hashPrecision))   //"Sawmill<br>Bank<br>Never mind"
            {
                Keyboard.WriteNumber(1);
            }
            else if (Numerical.CloseEnough(15959844, bodyCount, _hashPrecision) || Numerical.CloseEnough(16167022, bodyCount, _hashPrecision))  //"Enter amount:"
            {
                Keyboard.WriteNumber(25);
                SafeWaitPlus(50, 25);
                Keyboard.Enter();
            }
            else if (Numerical.CloseEnough(15768332, bodyCount, _hashPrecision))  //"Okay, here's 10,000 coins.<br>I'll pay you later.<br>You're fired!"
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
            int y = Inventory.TAB_TOP_OFFSET_BOTTOM;
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
            while (!StopFlag && !WaitFor(AnyDialog, 1500))
            {
                Blob demon;
                if (LocateStationaryObject(DemonHead, out demon, ArtifactLength(0.015), 3000, ArtifactArea(0.00005), ArtifactArea(0.0005)))
                {
                    Mouse.Move(demon.Center.X, demon.Center.Y, RSClient);
                    if (WaitForMouseOverText(YellowMouseOverText))
                    {
                        LeftClick(demon.Center.X, demon.Center.Y);
                        if (SafeWait(500)) { return false; }
                    }
                }
                else
                {
                    if (++demonTries > 2)
                    {
                        return AnyDialog(true);
                    }
                }
            }
            return WaitFor(AnyDialog);
        }

        /// <summary>
        /// Sets up a sawmill task for the butler
        /// Assumes that the butler is already waiting on a command
        /// </summary>
        /// <returns>true if successful</returns>
        protected bool StartSawmillTask()
        {
            Inventory.ClickInventory(FirstLogSlot, true);
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
            SafeWait(500);
            ReadWindow();
            if (InventoryIsReady())
            {
                return true;
            }

            Inventory.ClickInventory(InventoryLogSlot, true);
            Mouse.Move(Center.X, Center.Y, RSClient);
            if (WaitForTeleport())
            {
                return false;
            }

            if (!ItemsAreReady())   //reset the inventory to its correct starting configuration
            {
                RefreshInventory();
                if (SafeWait(1000)) { return false; }
                if (!ItemsAreReady())
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
        /// Determines if the inventory looks like it is ready
        /// </summary>
        /// <returns>true if the inventory appears to be set up correctly</returns>
        protected override bool InventoryIsReady()
        {
            if (!ItemsAreReady() || Inventory.SlotIsEmpty(Inventory.INVENTORY_CAPACITY - 1))
            {
                return false;   //TODO restock at the GE
            }

            return true;
        }

        /// <summary>
        /// Determines if the logs, law runes, and coins are in te inventory
        /// </summary>
        /// <returns>true if the first 3 inventory slots are not empty</returns>
        protected bool ItemsAreReady()
        {
            return (!Inventory.SlotIsEmpty(InventoryLogSlot) && !Inventory.SlotIsEmpty(InventoryLawRuneSlot) && !Inventory.SlotIsEmpty(InventoryCashSlot));
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
            Bank bankPopup = new Bank(RSClient, Inventory);
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
            bankPopup.Close();

            return ItemsAreReady();
        }
    }
}
