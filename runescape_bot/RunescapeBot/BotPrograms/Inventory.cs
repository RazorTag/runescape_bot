using RunescapeBot.UITools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms
{
    public class Inventory
    {
        private bool[,] inventory;
        private Random rng;
        private Process rsClient;

        public Inventory(Process rsClient)
        {
            inventory = new bool[4, 7];
            rng = new Random();
            this.rsClient = rsClient;
            SelectedTab = TabSelect.Unknown;
        }

        /// <summary>
        /// Opens the inventory if it isn't open already
        /// </summary>
        /// <param name="screen"></param>
        public void OpenInventory(Color[,] screen, bool safeTab = true)
        {
            if (!safeTab && SelectedTab == TabSelect.Inventory) { return; }
            int x = screen.GetLength(0) - INVENTORY_TAB_OFFSET_LEFT;
            int y = screen.GetLength(1) - INVENTORY_TAB_OFFSET_TOP + rng.Next(-5, 6);
            Mouse.LeftClick(x, y, rsClient);
            Thread.Sleep(TAB_SWITCH_WAIT);
            SelectedTab = TabSelect.Inventory;
        }

        /// <summary>
        /// Opens the spellbook if it isn't open already
        /// </summary>
        /// <param name="screen"></param>
        public void OpenSpellbook(Color[,] screen, bool safeTab = true)
        {
            if (!safeTab && SelectedTab == TabSelect.Spellbook) { return; }
            int x = screen.GetLength(0) - SPELLBOOK_TAB_OFFSET_LEFT + rng.Next(-5, 6);
            int y = screen.GetLength(1) - SPELLBOOK_TAB_OFFSET_TOP + rng.Next(-5, 6);
            Mouse.LeftClick(x, y, rsClient);
            Thread.Sleep(TAB_SWITCH_WAIT);
            SelectedTab = TabSelect.Spellbook;
        }

        /// <summary>
        /// Opens the inventory and clicks on an inventory slot
        /// </summary>
        /// <param name="x">slots away from the leftmost column (0-3)</param>
        /// <param name="y">slots away from the topmost column (0-6)</param>
        public void ClickInventory(Color[,] screen, int x, int y, bool safeTab = true)
        {
            int xOffset = screen.GetLength(0) - INVENTORY_OFFSET_LEFT + (x * INVENTORY_GAP_X) + rng.Next(-5, 6);
            int yOffset = screen.GetLength(1) - INVENTORY_OFFSET_TOP + (y * INVENTORY_GAP_Y) + rng.Next(-5, 6);
            OpenInventory(screen, safeTab);
            Mouse.LeftClick(xOffset, yOffset, rsClient, 200);
        }

        /// <summary>
        /// Opens the inventory and clicks on an inventory slot
        /// </summary>
        /// <param name="index">sequential slot in the inventory (1-28)</param>
        public void ClickInventory(Color[,] screen, int index, bool safeTab = true)
        {
            index--;
            int x = index % 4;
            int y = index / 4;
            ClickInventory(screen, x, y, safeTab);
        }

        /// <summary>
        /// Clicks a slot in the standard spellbook.
        /// Does not handle waiting after casting a spell.
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="x">slots away from the leftmost column (0-3)</param>
        /// <param name="y">slots away from the topmost column (0-6)</param>
        private void ClickSpellbook(Color[,] screen, int x, int y, bool safeTab = true)
        {
            int xOffset = screen.GetLength(0) - SPELLBOOK_OFFSET_LEFT + (x * SPELLBOOK_GAP_X) + rng.Next(-5, 6);
            int yOffset = screen.GetLength(1) - SPELLBOOK_OFFSET_TOP + (y * SPELLBOOK_GAP_Y) + rng.Next(-5, 6);
            OpenSpellbook(screen, safeTab);
            Mouse.LeftClick(xOffset, yOffset, rsClient, 200);
        }

        /// <summary>
        /// Casts telegrab at the specified location on the screen
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="x">x pixel location on the screen</param>
        /// <param name="y">y pixel location on the screen</param>
        public void Telegrab(Color[,] screen, int x, int y)
        {
            ClickSpellbook(screen, 5, 2);
            Mouse.LeftClick(x, y, rsClient, 200);
            Thread.Sleep(5000); //telegrab takes about 5 seconds
        }

        /// <summary>
        /// Selects high alchemy from the standard spellbook and alchs the specified inventory item
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="x">slots away from the leftmost column (0-3) or screen x coordinate</param>
        /// <param name="y">slots away from the topmost column (0-6) or screen y coordinate</param>
        /// <returns>true if the alch succeeds</returns>
        public bool Alch(Color[,] screen, int x, int y)
        {
            if (ScreenToInventory(screen, ref x, ref y))
            {
                OpenSpellbook(screen);
                ClickSpellbook(screen, 6, 4);
                SelectedTab = TabSelect.Inventory;
                ClickInventory(screen, x, y, false);
                SelectedTab = TabSelect.Spellbook;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Converts coordinates from screen to inventory coordinates if the cordinates exceed the size of the inventory
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>true if translation succeeds</returns>
        private bool ScreenToInventory(Color[,] screen, ref int x, ref int y)
        {
            if (x > 3 && y > 6)
            {
                //convert screen coordinates to inventory coordinates
                int inventoryLeft = screen.GetLength(0) - INVENTORY_OFFSET_LEFT;
                int inventoryTop = screen.GetLength(1) - INVENTORY_OFFSET_TOP;
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
        #endregion
    }
}
