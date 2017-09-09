using RunescapeBot.BotPrograms.Popups;
using RunescapeBot.Common;
using RunescapeBot.ImageTools;
using RunescapeBot.UITools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;

namespace RunescapeBot.BotPrograms
{
    public class Inventory
    {
        private Color[,] Screen;
        private Keyboard Keyboard;
        private Random RNG;
        private Process RSClient;

        /// <summary>
        /// Inventory slots to be dropped when more space is needed
        /// </summary>
        protected bool[,] EmptySlots;


        public Inventory(Process rsClient, Color[,] screen, Keyboard keyboard)
        {
            RNG = new Random();
            this.RSClient = rsClient;
            SelectedTab = TabSelect.Unknown;
            this.Screen = screen;
            this.Keyboard = keyboard;
            EmptySlots = new bool[INVENTORY_COLUMNS, INVENTORY_ROWS];
        }

        /// <summary>
        /// Sets the client to use
        /// </summary>
        /// <param name="rsClient"></param>
        public void SetClient(Process rsClient)
        {
            RSClient = rsClient;
        }

        /// <summary>
        /// Sets the latest screenshot
        /// </summary>
        /// <param name="screen"></param>
        public void SetScreen(Color[,] screen)
        {
            this.Screen = screen;
        }

        /// <summary>
        /// Opens the inventory if it isn't open already
        /// </summary>
        /// <param name="safeTab">Set to false to rely on the last saved value for the selected tab</param>
        /// <returns>True if the inventory is not assumed to already be open</returns>
        public bool OpenInventory(bool safeTab = true)
        {
            if (!safeTab && SelectedTab == TabSelect.Inventory)
            {
                return false;
            }

            Point? screenSize = ScreenScraper.GetScreenSize(RSClient);
            
            if (screenSize == null )
            {
                return false;
            } 
            int x = screenSize.Value.X - INVENTORY_TAB_OFFSET_RIGHT;
            int y = screenSize.Value.Y - INVENTORY_TAB_OFFSET_BOTTOM;
            Mouse.LeftClick(x, y, RSClient, 6);
            Thread.Sleep((int)Probability.HalfGaussian(TAB_SWITCH_WAIT, 0.1 * TAB_SWITCH_WAIT, true));
            SelectedTab = TabSelect.Inventory;
            return true;
        }

        /// <summary>
        /// Opens the spellbook if it isn't open already
        /// </summary>
        /// <param name="safeTab">Set to false to rely on the last saved value for the selected tab</param>
        /// <returns>True if the spellbook is not assumed to already be open</returns>
        public bool OpenSpellbook(bool safeTab = true)
        {
            if (!safeTab && SelectedTab == TabSelect.Spellbook)
            {
                return false;
            }
            int x = Screen.GetLength(0) - SPELLBOOK_TAB_OFFSET_RIGHT;
            int y = Screen.GetLength(1) - SPELLBOOK_TAB_OFFSET_BOTTOM;
            Mouse.LeftClick(x, y, RSClient, 6);
            Thread.Sleep((int)Probability.HalfGaussian(TAB_SWITCH_WAIT, 0.1 * TAB_SWITCH_WAIT, true));
            SelectedTab = TabSelect.Spellbook;
            return true;
        }

        /// <summary>
        /// Opens the logout tab if it isn't already open
        /// </summary>
        /// <param name="safeTab">Set to false to rely on the last saved value for the selected tab</param>
        /// <returns>true if the logout tab is not assumes to already be open</returns>
        public bool OpenLogout(bool safeTab = true)
        {
            if (!safeTab && SelectedTab == TabSelect.Logout)
            {
                return false;
            }
            int x = Screen.GetLength(0) - LOGOUT_TAB_OFFSET_RIGHT;
            int y = Screen.GetLength(1) - LOGOUT_TAB_OFFSET_BOTTOM;
            Mouse.LeftClick(x, y, RSClient, 6);
            Thread.Sleep((int)Probability.HalfGaussian(TAB_SWITCH_WAIT, 0.1 * TAB_SWITCH_WAIT, true));
            SelectedTab = TabSelect.Logout;
            return true;
        }

        /// <summary>
        /// Opens the inventory and clicks on an inventory slot
        /// </summary>
        /// <param name="x">slots away from the leftmost column (0-3)</param>
        /// <param name="y">slots away from the topmost column (0-6)</param>
        public void ClickInventory(int x, int y, bool safeTab = true)
        {
            InventoryToScreen(ref x, ref y);
            OpenInventory(safeTab);
            Mouse.LeftClick(x, y, RSClient, 5);
        }

        /// <summary>
        /// Opens the inventory and clicks on an inventory slot
        /// </summary>
        /// <param name="index">sequential slot in the inventory (0-27)</param>
        public void ClickInventory(int index, bool safeTab = true)
        {
            Point inventorySlot = InventoryIndexToCoordinates(index);
            ClickInventory(inventorySlot.X, inventorySlot.Y, safeTab);
        }

        /// <summary>
        /// Drops an item at the specified inventory coordinates
        /// </summary>
        /// <param name="x">slots from the left column of the inventory (0-3)</param>
        /// <param name="y">slots from the top row of the inventory (0-6)</param>
        public void DropItem(int x, int y, bool safeTab = true, int[] extraOptions = null)
        {
            OpenInventory(safeTab);
            InventoryToScreen(ref x, ref y);

            Point click;
            const int timeout = 3000;
            Stopwatch watch = new Stopwatch();
            watch.Start();
            bool done = false;
            RightClickInventory dropPopup = null;

            while (!done && (watch.ElapsedMilliseconds < timeout))
            {
                if (BotProgram.StopFlag) { return; }

                click = Probability.GaussianCircle(new Point(x, y), 4.0, 0, 360, 10);
                Mouse.RightClick(click.X, click.Y, RSClient, 0);
                dropPopup = new RightClickInventory(click.X, click.Y, RSClient, extraOptions);
                if (dropPopup.WaitForPopup(1000))
                {
                    done = true;
                }
                else
                {
                    OpenInventory(true);
                }
            }

            dropPopup.DropItem();
        }

        /// <summary>
        /// Set the list of empty inventory slots
        /// </summary>
        public void SetEmptySlots()
        {
            OpenInventory();
            Screen = ScreenScraper.GetRGB(ScreenScraper.CaptureWindow(RSClient));
            EmptySlots = new bool[INVENTORY_COLUMNS, INVENTORY_ROWS];

            for (int x = 0; x < INVENTORY_COLUMNS; x++)
            {
                for (int y = 0; y < INVENTORY_ROWS; y++)
                {
                    EmptySlots[x, y] = SlotIsEmpty(x, y);
                }
            }
        }

        /// <summary>
        /// Drops all of the items in the inventory
        /// </summary>
        /// <param name="safeTab"></param>
        public void DropInventory(bool safeTab = true, bool onlyDropPreviouslyEmptySlots = false)
        {
            Screen = ScreenScraper.GetRGB(ScreenScraper.CaptureWindow(RSClient));
            OpenInventory(safeTab);
            for (int x = 0; x < INVENTORY_COLUMNS; x++)
            {
                for (int y = 0; y < INVENTORY_ROWS; y++)
                {
                    if ((!onlyDropPreviouslyEmptySlots || EmptySlots[x, y]) && !SlotIsEmpty(x, y, false, false))
                    {
                        if (BotProgram.StopFlag) { return; }
                        DropItem(x, y, false);
                    }
                }
            }
        }

        /// <summary>
        /// Converts an inventory index (0-27) to inventory coordinates X:(0-3), Y:(0-6)
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Point InventoryIndexToCoordinates(int index)
        {
            int x = index % 4;
            int y = index / 4;
            return new Point(x, y);
        }

        /// <summary>
        /// Clicks a slot in the standard spellbook.
        /// Does not handle waiting after casting a spell.
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="x">slots away from the leftmost column (0-3)</param>
        /// <param name="y">slots away from the topmost column (0-6)</param>
        private void ClickSpellbook(int x, int y, bool safeTab = true)
        {
            SpellbookToScreen(ref x, ref y);
            OpenSpellbook(safeTab);
            Mouse.LeftClick(x, y, RSClient, 5);
        }

        /// <summary>
        /// Casts telegrab at the specified location on the screen
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="x">x pixel location on the screen</param>
        /// <param name="y">y pixel location on the screen</param>
        public void Telegrab(int x, int y, bool safeTab = true, bool autoWait = true)
        {
            ClickSpellbook(5, 2, safeTab);
            Mouse.LeftClick(x, y, RSClient, 1);
            if (autoWait) { Thread.Sleep((int)Probability.HalfGaussian(5000, 300, true)); } //telegrab takes about 5 seconds
        }

        /// <summary>
        /// Casts a spell on an item in the inventory. Assumes that casting the spell reopens the spellbook.
        /// </summary>
        /// <param name="spellBookX"></param>
        /// <param name="spellBookY"></param>
        /// <param name="castTime">cooldown time of the spell</param>
        /// <param name="x">x inventory or screen coordinate within the inventory</param>
        /// <param name="y">y inventory or screen coordinate within the inventory</param>
        /// <param name="safeTab"></param>
        /// <returns>true if the spell is cast successfully</returns>
        private bool CastInventorySpell(int spellBookX, int spellBookY, int castTime, int x, int y, bool safeTab = true, bool autoWait = true)
        {
            if (ScreenToInventory(ref x, ref y))
            {
                OpenSpellbook(safeTab);
                ClickSpellbook(spellBookX, spellBookY, safeTab);
                SelectedTab = TabSelect.Inventory;
                ClickInventory(x, y, false);
                SelectedTab = TabSelect.Spellbook;
                if (autoWait) { Thread.Sleep((int)Probability.HalfGaussian(castTime, 100, true)); }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Selects high alchemy from the standard spellbook and alchs the specified inventory item
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="x">slots away from the leftmost column (0-3) or screen x coordinate</param>
        /// <param name="y">slots away from the topmost column (0-6) or screen y coordinate</param>
        /// <returns>true if the alch succeeds</returns>
        public bool Alch(int x, int y, bool safeTab = true, bool autoWait = true)
        {
            return CastInventorySpell(6, 4, 3000, x, y, safeTab, autoWait);
        }

        /// <summary>
        /// Telegrabs at a specified location and alchs the item that was grabbed.
        /// Assumes that the item will go into the first open inventory space (not necessarily true for stackable items).
        /// Does nothing if the inventory is already full.
        /// </summary>
        /// <param name="x">x location on the game screen (from left) where the item to telegrab is located</param>
        /// <param name="y">y location on the game screen (from top) where the item to telegrab is located</param>
        /// <returns>true if successful</returns>
        public bool GrabAndAlch(int x, int y)
        {
            Point? emptySlot = FirstEmptySlot();
            if (emptySlot == null)
            {
                return false;   //no empty inventory slots
            }

            Telegrab(x, y);
            if (BotProgram.StopFlag) { return false; }
            Alch(emptySlot.Value.X, emptySlot.Value.Y);

            return true;
        }

        /// <summary>
        /// Casts an enchant spell on jewelry within the inventory
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="safeTab"></param>
        /// <param name="autoWait"></param>
        /// <returns>true if successful</returns>
        public bool Enchant(int x, int y, int enchantLevel, bool safeTab = true, bool autoWait = true)
        {
            if (enchantLevel < 1 || enchantLevel > 7)
            {
                throw new Exception("Enchantment level " + enchantLevel + " is not supported.");
            }

            Point spell = new Point(0, 0);

            switch (enchantLevel)
            {
                case 1:
                    spell = new Point(5, 0);
                    break;
                case 2:
                    spell = new Point(2, 2);
                    break;
                case 3:
                    spell = new Point(0, 4);
                    break;
                case 4:
                    spell = new Point(1, 5);
                    break;
                case 5:
                    spell = new Point(1, 7);
                    break;
                case 6:
                    spell = new Point(0, 9);
                    break;
                case 7:
                    spell = new Point(2, 9);
                    break;
            }
            return CastInventorySpell(spell.X, spell.Y, 1800, x, y, safeTab, autoWait);
        }

        /// <summary>
        /// Uses an item in the inventory on another item in the inventory
        /// </summary>
        /// <param name="subjectItem">the item to use on another item</param>
        /// <param name="objectItem">the item to be used on</param>
        /// <returns>true if the operation seems to succeed</returns>
        public void UseItemOnItem(Point subjectItem, Point objectItem, bool safeTab = true)
        {
            ClickInventory(subjectItem.X, subjectItem.Y, safeTab);
            Thread.Sleep((int)Probability.HalfGaussian(200, 30, true));
            ClickInventory(objectItem.X, objectItem.Y, safeTab);
        }

        /// <summary>
        /// Finds the next slot that a new picked up item would go into.
        /// </summary>
        /// <returns>The first empty inventory slot scanning left to right then top to bottom. Returns null if inventory is full.</returns>
        public Point? FirstEmptySlot(bool safeTab = true)
        {
            if (OpenInventory(safeTab))
            {
                Screen = ScreenScraper.GetRGB(ScreenScraper.CaptureWindow(RSClient));
            }
            Point? inventorySlot;

            for (int slot = 0; slot < INVENTORY_CAPACITY; slot++)
            {
                inventorySlot = InventoryIndexToCoordinates(slot);
                if (SlotIsEmpty(inventorySlot.Value.X, inventorySlot.Value.Y))
                {
                    return inventorySlot;
                }

                if (BotProgram.StopFlag) { return null; }
            }

            return null;
        }

        /// <summary>
        /// Determines if the given inventory slot contains any item
        /// </summary>
        /// <param name="x">slots away from the leftmost column (0-3) or screen x coordinate</param>
        /// <param name="y">slots away from the topmost column (0-6) or screen y coordinate</param>
        /// <returns>true if a slot is empty</returns>
        public bool SlotIsEmpty(int xSlot, int ySlot, bool readScreen = false, bool safeTab = false)
        {
            OpenInventory(safeTab);
            int x = xSlot;
            int y = ySlot;
            InventoryToScreen(ref x, ref y);
            if (readScreen)
            {
                Screen = ScreenScraper.GetRGB(ScreenScraper.CaptureWindow(RSClient));
            }

            int xOffset = (INVENTORY_GAP_X / 2) - 1;
            int yOffset = (INVENTORY_GAP_Y / 2) - 1;
            int left = x - xOffset;
            int right = x + xOffset;
            int top = y - yOffset;
            int bottom = y + yOffset;

            Color[,] itemIcon = ImageProcessing.ScreenPiece(Screen, left, right, top, bottom);
            double emptyMatch = ImageProcessing.FractionalMatch(itemIcon, RGBHSBRangeFactory.EmptyInventorySlot());
            return (emptyMatch > 0.99) || Windows10WatermarkEmpty(itemIcon, xSlot, ySlot);
        }

        /// <summary>
        /// Checks an inventory slot against the expected value when the Windows 10 unregistered watermark is present
        /// </summary>
        /// <param name="itemIcon">image of the inventory slot</param>
        /// <param name="xSlot">0-3</param>
        /// <param name="ySlot">0-6</param>
        /// <returns>true if the slot matches the expected value for an empty inventory slot on the bottom row when the Windows 10 watermark is present</returns>
        private bool Windows10WatermarkEmpty(Color[,] itemIcon, int xSlot, int ySlot)
        {
            if (ySlot >= (INVENTORY_ROWS - 2))  //special handling for the bottom 2 rows Windows 10 watermark
            {
                double emptyMatch = ImageProcessing.FractionalMatch(itemIcon, RGBHSBRangeFactory.InventorySlotWindows10Watermark());
                return emptyMatch > 0.99;
            }
            return false;   //handling not implementing for rows 0-5
        }

        /// <summary>
        /// Converts coordinates from screen to inventory coordinates if the cordinates exceed the size of the inventory
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>true if translation succeeds</returns>
        private bool ScreenToInventory(ref int x, ref int y)
        {
            if (x >= INVENTORY_COLUMNS && y >= INVENTORY_ROWS)
            {
                //convert screen coordinates to inventory coordinates
                int inventoryLeft = Screen.GetLength(0) - INVENTORY_OFFSET_LEFT;
                int inventoryTop = Screen.GetLength(1) - INVENTORY_OFFSET_TOP;
                x = (int)Math.Round((x - inventoryLeft) / ((double)INVENTORY_GAP_X));
                y = (int)Math.Round((y - inventoryTop) / ((double)INVENTORY_GAP_Y));
            }

            //check if we found an actual inventory spot
            if ((x >= 0) && (x < INVENTORY_COLUMNS) && (y >= 0) && (y < INVENTORY_ROWS))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Converts coordinates from the standard spellbook to screen coordinates if the cordinates exceed the size of the spellbook
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>true if translation succeeds</returns>
        private bool SpellbookToScreen(ref int x, ref int y)
        {
            if (x < 0 || x > 6 || y < 0 || y > 9)
            {
                return false;
            }

            x = Screen.GetLength(0) - SPELLBOOK_OFFSET_LEFT + (x * SPELLBOOK_GAP_X);
            y = Screen.GetLength(1) - SPELLBOOK_OFFSET_TOP + (y * SPELLBOOK_GAP_Y);
            return true;
        }

        /// <summary>
        /// Converts coordinates from inventory to screen coordinates if the cordinates exceed the size of the inventory
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>true if translation succeeds</returns>
        public bool InventoryToScreen(ref int x, ref int y)
        {
            if (x < 0 || x >= INVENTORY_COLUMNS || y < 0 || y >= INVENTORY_ROWS)
            {
                return false;
            }

            x = Screen.GetLength(0) - INVENTORY_OFFSET_LEFT + (x * INVENTORY_GAP_X);
            y = Screen.GetLength(1) - INVENTORY_OFFSET_TOP + (y * INVENTORY_GAP_Y);
            return true;
        }

        #region constants
        /// <summary>
        /// Milliseconds to wait after switching tabs
        /// </summary>
        private const int TAB_SWITCH_WAIT = 200;

        private const int INVENTORY_TAB_OFFSET_RIGHT = 118;
        private const int INVENTORY_TAB_OFFSET_BOTTOM = 320;
        private const int INVENTORY_OFFSET_LEFT = 185;
        private const int INVENTORY_OFFSET_TOP = 275;
        private const int INVENTORY_GAP_X = 42;
        private const int INVENTORY_GAP_Y = 36;
        public const int INVENTORY_CAPACITY = 28;
        public const int INVENTORY_COLUMNS = 4;
        public const int INVENTORY_ROWS = 7;

        private const int SPELLBOOK_TAB_OFFSET_RIGHT = 19;
        private const int SPELLBOOK_TAB_OFFSET_BOTTOM = 320;
        private const int SPELLBOOK_OFFSET_LEFT = 191;
        private const int SPELLBOOK_OFFSET_TOP = 272;
        private const int SPELLBOOK_GAP_X = 24;
        private const int SPELLBOOK_GAP_Y = 24;
        public const int SPELLBOOK_COLUMNS = 7;
        public const int SPELLBOOK_ROWS = 10;

        private const int LOGOUT_TAB_OFFSET_RIGHT = 120;
        private const int LOGOUT_TAB_OFFSET_BOTTOM = 18;

        public TabSelect SelectedTab { get; set; }
        public enum TabSelect : int
        {
            Unknown,
            Inventory,
            Spellbook,
            Logout
        }

        public enum Item : int
        {
            Unknown = -1,
            Empty = 0
        }
        private const int EMPTY_ITEM_HASH = 3423623;
        #endregion
    }
}
