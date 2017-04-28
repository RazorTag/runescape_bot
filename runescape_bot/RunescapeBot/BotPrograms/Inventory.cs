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
        private bool[,] inventory;
        private Random RNG;
        private Process RSClient;

        public Inventory(Process rsClient, Color[,] screen)
        {
            inventory = new bool[4, 7];
            RNG = new Random();
            this.RSClient = rsClient;
            SelectedTab = TabSelect.Unknown;
            this.Screen = screen;
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

            int x = Screen.GetLength(0) - INVENTORY_TAB_OFFSET_LEFT + RNG.Next(-5, 6);
            int y = Screen.GetLength(1) - INVENTORY_TAB_OFFSET_TOP + RNG.Next(-5, 6);
            Mouse.LeftClick(x, y, RSClient);
            Thread.Sleep(TAB_SWITCH_WAIT);
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
            int x = Screen.GetLength(0) - SPELLBOOK_TAB_OFFSET_LEFT + RNG.Next(-5, 6);
            int y = Screen.GetLength(1) - SPELLBOOK_TAB_OFFSET_TOP + RNG.Next(-5, 6);
            Mouse.LeftClick(x, y, RSClient);
            Thread.Sleep(TAB_SWITCH_WAIT);
            SelectedTab = TabSelect.Spellbook;
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
            x += RNG.Next(-5, 6);
            y += RNG.Next(-5, 6);
            OpenInventory(safeTab);
            Mouse.LeftClick(x, y, RSClient, 200);
        }

        /// <summary>
        /// Opens the inventory and clicks on an inventory slot
        /// </summary>
        /// <param name="index">sequential slot in the inventory (1-28)</param>
        public void ClickInventory(int index, bool safeTab = true)
        {
            Point inventorySlot = InventoryIndexToCoordinates(index);
            ClickInventory(inventorySlot.X, inventorySlot.Y, safeTab);
        }

        /// <summary>
        /// COnverts an inventory index (0-27) to inventory coordinates X:(0-3), Y:(0-6)
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
            x += RNG.Next(-5, 6);   //randomize the click location so we don't repeat the same pixel
            y += RNG.Next(-5, 6);
            OpenSpellbook(safeTab);
            Mouse.LeftClick(x, y, RSClient, 200);
        }

        /// <summary>
        /// Casts telegrab at the specified location on the screen
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="x">x pixel location on the screen</param>
        /// <param name="y">y pixel location on the screen</param>
        public void Telegrab(int x, int y, bool safeTab = true)
        {
            ClickSpellbook(5, 2, safeTab);
            Mouse.LeftClick(x, y, RSClient, 200);
            Thread.Sleep(5000); //telegrab takes about 5 seconds
        }

        /// <summary>
        /// Selects high alchemy from the standard spellbook and alchs the specified inventory item
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="x">slots away from the leftmost column (0-3) or screen x coordinate</param>
        /// <param name="y">slots away from the topmost column (0-6) or screen y coordinate</param>
        /// <returns>true if the alch succeeds</returns>
        public bool Alch(int x, int y, bool safeTab = true)
        {
            if (ScreenToInventory(ref x, ref y))
            {
                OpenSpellbook();
                ClickSpellbook(6, 4);
                SelectedTab = TabSelect.Inventory;
                ClickInventory(x, y, false);
                SelectedTab = TabSelect.Spellbook;
                Thread.Sleep(2000);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Telegrabs at a specified location and alchs the item that was grabbed.
        /// Assumes that the item will go into the first open inventory space (not necessarily true for stackable items).
        /// Does nothing if the inventory is already full.
        /// </summary>
        /// <param name="x">x location on the game screen (from left) where the item to telegrab is located</param>
        /// <param name="y">y location on the game screen (from top) where the item to telegrab is located</param>
        /// <returns></returns>
        public bool GrabAndAlch(int x, int y)
        {
            Point? emptySlot = FirstEmptySlot();
            if (emptySlot == null)
            {
                return false;   //no empty inventory slots
            }

            Telegrab(x, y);
            Alch(emptySlot.Value.X, emptySlot.Value.Y);

            return true;
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
            }

            return null;
        }

        /// <summary>
        /// Determines if the given inventory slot contains any item
        /// </summary>
        /// <param name="x">slots away from the leftmost column (0-3) or screen x coordinate</param>
        /// <param name="y">slots away from the topmost column (0-6) or screen y coordinate</param>
        /// <returns>Item enum value</returns>
        public bool SlotIsEmpty(int x, int y, bool safeTab = false)
        {
            OpenInventory(safeTab);
            InventoryToScreen(ref x, ref y);

            int xOffset = (INVENTORY_GAP_X / 2) - 1;
            int yOffset = (INVENTORY_GAP_Y / 2) - 1;
            int left = x - xOffset;
            int right = x + xOffset;
            int top = y - yOffset;
            int bottom = y + yOffset;

            Color[,] itemIcon = ImageProcessing.ScreenPiece(Screen, left, right, top, bottom);
            double emptyMatch = ImageProcessing.FractionalMatch(itemIcon, ColorFilters.EmptyInventorySlot());
            return emptyMatch > 0.99; //99% match needed to pass
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
            if (x > 3 && y > 6)
            {
                //convert screen coordinates to inventory coordinates
                int inventoryLeft = Screen.GetLength(0) - INVENTORY_OFFSET_LEFT;
                int inventoryTop = Screen.GetLength(1) - INVENTORY_OFFSET_TOP;
                x = (int)Math.Round((x - inventoryLeft) / ((double)INVENTORY_GAP_X));
                y = (int)Math.Round((y - inventoryTop) / ((double)INVENTORY_GAP_Y));
            }

            //check if we found an actual inventory spot
            if ((x >= 0) && (x < 4) && (y >= 0) && (y < 7))
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
        private bool InventoryToScreen(ref int x, ref int y)
        {
            if (x < 0 || x > 3 || y < 0 || y > 6)
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

        private const int INVENTORY_TAB_OFFSET_LEFT = 119;
        private const int INVENTORY_TAB_OFFSET_TOP = 320;
        private const int INVENTORY_OFFSET_LEFT = 186;
        private const int INVENTORY_OFFSET_TOP = 275;
        private const int INVENTORY_GAP_X = 42;
        private const int INVENTORY_GAP_Y = 36;
        private const int INVENTORY_CAPACITY = 28;

        private const int SPELLBOOK_TAB_OFFSET_LEFT = 20;
        private const int SPELLBOOK_TAB_OFFSET_TOP = 320;
        private const int SPELLBOOK_OFFSET_LEFT = 192;
        private const int SPELLBOOK_OFFSET_TOP = 272;
        private const int SPELLBOOK_GAP_X = 24;
        private const int SPELLBOOK_GAP_Y = 24;        

        public TabSelect SelectedTab { get; set; }
        public enum TabSelect : int
        {
            Unknown,
            Inventory,
            Spellbook
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
