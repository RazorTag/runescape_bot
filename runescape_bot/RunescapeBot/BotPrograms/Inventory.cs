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
        /// <summary>
        /// Milliseconds to wait after switching tabs
        /// </summary>
        private const int TAB_SWITCH_WAIT = 500;

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

        private bool[,] inventory;
        private Random rng;
        private Process rsClient;

        public Inventory(Process rsClient)
        {
            inventory = new bool[4, 7];
            rng = new Random();
            this.rsClient = rsClient;
        }

        /// <summary>
        /// Opens the inventory
        /// </summary>
        /// <param name="screen"></param>
        public void OpenInventory(Color[,] screen)
        {
            int x = screen.GetLength(0) - INVENTORY_TAB_OFFSET_LEFT;
            int y = screen.GetLength(1) - INVENTORY_TAB_OFFSET_TOP + rng.Next(-5, 6);
            Mouse.LeftClick(x, y, rsClient);

        }

        /// <summary>
        /// Opens the inventory and clicks on an inventory slot
        /// </summary>
        /// <param name="x">slots away from the leftmost column (0-3)</param>
        /// <param name="y">slots away from the topmost column (0-6)</param>
        public void ClickInventory(Color[,] screen, int x, int y)
        {
            int xOffset = screen.GetLength(0) - INVENTORY_OFFSET_LEFT + (x * INVENTORY_GAP_X) + rng.Next(-5, 6);
            int yOffset = screen.GetLength(1) - INVENTORY_OFFSET_TOP + (y * INVENTORY_GAP_Y) + rng.Next(-5, 6);
            OpenInventory(screen);
            Thread.Sleep(TAB_SWITCH_WAIT);
            Mouse.LeftClick(xOffset, yOffset, rsClient);
        }

        /// <summary>
        /// Opens the spellbook
        /// </summary>
        /// <param name="screen"></param>
        public void OpenSpellbook(Color[,] screen)
        {
            int x = screen.GetLength(0) - SPELLBOOK_TAB_OFFSET_LEFT + rng.Next(-5, 6);
            int y = screen.GetLength(1) - SPELLBOOK_TAB_OFFSET_TOP + rng.Next(-5, 6);
            Mouse.LeftClick(x, y, rsClient);
        }

        /// <summary>
        /// Clicks a slot in the 
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="x">slots away from the leftmost column (0-3)</param>
        /// <param name="y">slots away from the topmost column (0-6)</param>
        public void ClickSpellbook(Color[,] screen, int x, int y)
        {
            int xOffset = screen.GetLength(0) - SPELLBOOK_OFFSET_LEFT + (x * SPELLBOOK_GAP_X) + rng.Next(-5, 6);
            int yOffset = screen.GetLength(1) - SPELLBOOK_OFFSET_TOP + (y * SPELLBOOK_GAP_Y) + rng.Next(-5, 6);
            OpenSpellbook(screen);
            Thread.Sleep(TAB_SWITCH_WAIT);
            Mouse.LeftClick(xOffset, yOffset, rsClient);
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
            Thread.Sleep(TAB_SWITCH_WAIT);
            Mouse.LeftClick(x, y, rsClient);
        }

        /// <summary>
        /// Selects high alchemy from the standard spellbook and alchs the specified inventory item
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="x">slots away from the leftmost column (0-3)</param>
        /// <param name="y">slots away from the topmost column (0-6)</param>
        /// <returns></returns>
        public bool Alch(Color[,] screen, int x, int y)
        {
            //int alchCenterX = 
            OpenSpellbook(screen);
            ClickSpellbook(screen, 6, 4);
            ClickInventory(screen, x, y);

            return false;
        }
    }
}
