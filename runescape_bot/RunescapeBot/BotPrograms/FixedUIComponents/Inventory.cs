using RunescapeBot.BotPrograms.Popups;
using RunescapeBot.Common;
using RunescapeBot.ImageTools;
using RunescapeBot.UITools;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;

namespace RunescapeBot.BotPrograms
{
    public class Inventory
    {
        #region properties

        private Color[,] Screen;
        private Keyboard Keyboard;
        private Random RNG;
        private Process RSClient;

        /// <summary>
        /// Inventory slots to be dropped when more space is needed
        /// </summary>
        protected bool[,] EmptySlots;

        /// <summary>
        /// Inventory slots to be dropped when more space is needed. Readonly.
        /// </summary>
        public bool[,] GetEmptySlots { get { return EmptySlots; } }

        #endregion

        #region constructors

        public Inventory(Process rsClient, Keyboard keyboard)
        {
            RNG = new Random();
            RSClient = rsClient;
            SelectedTab = TabSelect.Unknown;
            Keyboard = keyboard;
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
            Screen = screen;
        }

        /// <summary>
        /// Takes a screenshot of the game
        /// </summary>
        /// <param name="overwrite">set to true to take a screenshot even if one already exists</param>
        private void ManuallySetScreen(bool overwrite)
        {
            if (overwrite || Screen == null)
            {
                Screen = ScreenScraper.GetRGB(ScreenScraper.CaptureWindow(RSClient, true));
            }
        }

        #endregion

        #region tabs

        /// <summary>
        /// Opens a tab on the inventory UI if it is not already open
        /// </summary>
        /// <param name="tab">tab to open</param>
        /// <param name="x">x coordinate of the center of the tab</param>
        /// <param name="y">y coordinate of the center of the tab</param>
        /// <param name="safeTab">set to true to click on tab even if it is already believed to be open</param>
        /// <returns>true if the tab is clicked on</returns>
        private bool OpenTab(TabSelect tab, int offsetRight, int offsetBottom, bool safeTab = false)
        {
            if (!safeTab && SelectedTab == tab)
            {
                return false;
            }
            Point? screenSize = ScreenScraper.GetScreenSize(RSClient);
            if (screenSize == null)
            {
                return false;
            }

            int x = screenSize.Value.X - offsetRight;
            int y = screenSize.Value.Y - offsetBottom;
            Mouse.LeftClick(x, y, RSClient, 10);
            BotProgram.SafeWait(TAB_SWITCH_WAIT);
            SelectedTab = tab;
            return true;
        }

        /// <summary>
        /// Opens the inventory if it isn't open already
        /// </summary>
        /// <param name="safeTab">Set to false to rely on the last saved value for the selected tab</param>
        /// <returns>True if the inventory is not assumed to already be open</returns>
        public bool OpenInventory(bool safeTab = false)
        {
            return OpenTab(TabSelect.Inventory, INVENTORY_TAB_OFFSET_RIGHT, TAB_TOP_OFFSET_BOTTOM, safeTab);
        }

        /// <summary>
        /// Opens the inventory if it isn't open already
        /// </summary>
        /// <param name="safeTab">Set to false to rely on the last saved value for the selected tab</param>
        /// <returns>True if the inventory is not assumed to already be open</returns>
        public bool OpenEquipment(bool safeTab = false)
        {
            return OpenTab(TabSelect.Equipment, EQUIPMENT_TAB_OFFSET_RIGHT, TAB_TOP_OFFSET_BOTTOM, safeTab);
        }

        /// <summary>
        /// Opens the spellbook if it isn't open already
        /// </summary>
        /// <param name="safeTab">Set to false to rely on the last saved value for the selected tab</param>
        /// <returns>True if the spellbook is not assumed to already be open</returns>
        public bool OpenSpellbook(bool safeTab = false)
        {
            return OpenTab(TabSelect.Spellbook, SPELLBOOK_TAB_OFFSET_RIGHT, TAB_TOP_OFFSET_BOTTOM, safeTab);
        }

        /// <summary>
        /// Opens the logout tab if it isn't already open
        /// </summary>
        /// <param name="safeTab">Set to false to rely on the last saved value for the selected tab</param>
        /// <returns>true if the logout tab is not assumes to already be open</returns>
        public bool OpenLogout(bool safeTab = false)
        {
            return OpenTab(TabSelect.Logout, LOGOUT_TAB_OFFSET_RIGHT, TAB_BOTTOM_OFFSET_BOTTOM, safeTab);
        }

        /// <summary>
        /// Opens the options tab if it isn't open already
        /// </summary>
        /// <param name="safeTab">Set to false to rely on the last saved value for the selected tab</param>
        /// <returns>True if the options tab is not assumed to already be open</returns>
        public bool OpenOptions(bool safeTab = false)
        {
            return OpenTab(TabSelect.Options, OPTIONS_TAB_OFFSET_RIGHT, OPTIONS_TAB_OFFSET_BOTTOM, safeTab);
        }

        #endregion

        #region imaging

        /// <summary>
        /// Wrapper for ScreenScraper.CaptureWindow
        /// </summary>
        public bool ReadWindow(bool fastCapture = false)
        {
            Bitmap screenshot;
            try
            {
                screenshot = ScreenScraper.CaptureWindow(RSClient, fastCapture);
                Screen = ScreenScraper.GetRGB(screenshot);
            }
            catch
            {
                return false;
            }

            bool success = (screenshot != null) && (Screen.GetLength(0) > 0) && (Screen.GetLength(1) > 0);
            screenshot.Dispose();
            return success;
        }

        /// <summary>
        /// Makes an image of an inventory slot
        /// </summary>
        /// <param name="column">column to look at (0-3, left to right)</param>
        /// <param name="row">row to look at (0-6, top to bottom)</param>
        /// <param name="readWindow">set to false to skip taking a new screenshot</param>
        /// <returns>an image of an inventory slot</returns>
        public Color[,] SlotPicture(int column, int row, bool readWindow = true)
        {
            OpenInventory(false);
            int x = 0, y = 0;
            InventoryToScreen(ref x, ref y);
            int left = x - INVENTORY_GAP_X / 2;
            int right = x + INVENTORY_GAP_X / 2;
            int top = y - INVENTORY_GAP_Y / 2;
            int bottom = y + INVENTORY_GAP_Y / 2;
            if (readWindow) { ReadWindow(); }

            return ImageProcessing.ScreenPiece(Screen, left, right, top, bottom);
        }

        #endregion

        #region inventory

        /// <summary>
        /// Opens the inventory and clicks on an inventory slot
        /// </summary>
        /// <param name="x">slots away from the leftmost column (0-3)</param>
        /// <param name="y">slots away from the topmost column (0-6)</param>
        public void ClickInventory(int x, int y, bool safeTab = false)
        {
            InventoryToScreen(ref x, ref y);
            OpenInventory(safeTab);
            Mouse.LeftClick(x, y, RSClient, 5);
        }

        /// <summary>
        /// Opens the inventory and clicks on an inventory slot
        /// </summary>
        /// <param name="slot">inventory slot</param>
        public void ClickInventory(Point slot, bool safeTab = false)
        {
            ClickInventory(slot.X, slot.Y, safeTab);
        }

        /// <summary>
        /// Opens the inventory and clicks on an inventory slot
        /// </summary>
        /// <param name="index">sequential slot in the inventory (0-27)</param>
        public void ClickInventory(int index, bool safeTab = false)
        {
            Point inventorySlot = InventoryIndexToCoordinates(index);
            ClickInventory(inventorySlot.X, inventorySlot.Y, safeTab);
        }

        /// <summary>
        /// Drops an item at the specified inventory coordinates
        /// </summary>
        /// <param name="x">slots from the left column of the inventory (0-3)</param>
        /// <param name="y">slots from the top row of the inventory (0-6)</param>
        public bool DropItem(int x, int y, bool safeTab = true, int[] extraOptions = null)
        {
            InventoryToScreen(ref x, ref y);
            RightClickInventory popup;
            Point click;
            int dropOptions = RightClickInventory.OptionIndex(1, extraOptions);
            for (int i = 0; i < 3; i++)
            {
                if (BotProgram.StopFlag) { return false; }

                click = Probability.GaussianCircle(new Point(x, y), 5.0, 0, 360, 10);
                Mouse.RightClick(click.X, click.Y, RSClient, 0);
                popup = new RightClickInventory(click.X, click.Y, RSClient, null);
                if (popup.WaitForPopup(1500))
                {
                    popup.DropItem();
                    return true;
                }
                else
                {
                    OpenInventory(true);
                }
            }

            return false;
        }

        /// <summary>
        /// Selects a right-click option for an item in the player's inventory
        /// </summary>
        /// <param name="x">column (0-3)</param>
        /// <param name="y">row (0-6)</param>
        /// <param name="option">option index starting from 0</param>
        /// <param name="safeTab">set to false to skip making sure that the inventory tab is open</param>
        /// <param name="extraOptions">list indices of non-standard right-click options. Standard options include Use, Drop, Examine, and Cancel</param>
        public bool RightClickInventoryOption(int x, int y, int option, bool safeTab = false)
        {
            OpenInventory(safeTab);
            InventoryToScreen(ref x, ref y);
            return TryRightClickOption(new Point(x, y), option);
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
        /// Sets the empty of full value of an inventory slot
        /// </summary>
        /// <param name="x">column (0-3)</param>
        /// <param name="y">row (0-6)</param>
        /// <param name="slotIsEmpty">true for an empty slot, false for a full slot</param>
        public void SetEmptySlot(int x, int y, bool slotIsEmpty)
        {
            if (x >= 0 && x < INVENTORY_COLUMNS && y >= 0 && y < INVENTORY_ROWS)
            {
                EmptySlots[x, y] = slotIsEmpty;
            }
        }

        /// <summary>
        /// Drops all of the items in the inventory
        /// </summary>
        /// <param name="safeTab"></param>
        public void DropInventory(bool safeTab = true, bool onlyDropPreviouslyEmptySlots = true)
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
        /// Uses an item in the inventory on another item in the inventory
        /// </summary>
        /// <param name="subjectItem">the item to use on another item</param>
        /// <param name="objectItem">the item to be used on</param>
        /// <returns>true if the operation seems to succeed</returns>
        public void UseItemOnItem(Point subjectItem, Point objectItem, bool safeTab = false)
        {
            ClickInventory(subjectItem.X, subjectItem.Y, safeTab);
            BotProgram.SafeWaitPlus(200, 30);
            ClickInventory(objectItem.X, objectItem.Y, false);
        }

        /// <summary>
        /// Finds the next slot that matches a single color filter
        /// </summary>
        /// <returns>The first matching inventory slot scanning left to right then top to bottom. Returns null if no match is found.</returns>
        public Point? FirstColorMatchingSlot(ColorFilter colorFilter, double matchStrictness = 0.1, bool safeTab = true)
        {
            if (OpenInventory(safeTab))
            {
                Screen = ScreenScraper.GetRGB(ScreenScraper.CaptureWindow(RSClient));
            }
            Point? inventorySlot;

            for (int slot = 0; slot < INVENTORY_CAPACITY; slot++)
            {
                inventorySlot = InventoryIndexToCoordinates(slot);
                if (SlotMatchesColorFilter(inventorySlot.Value.X, inventorySlot.Value.Y, colorFilter, matchStrictness, false, false))
                {
                    return inventorySlot;
                }

                if (BotProgram.StopFlag) { return null; }
            }

            return null;
        }

        /// <summary>
        /// Finds the next slot that a new picked up item would go into.
        /// </summary>
        /// <param name="safeTab">set to false to rely on the Inventory's record of whether it is already on the inventory tab</param>
        /// <returns>The first empty inventory slot scanning left to right then top to bottom. Returns null if inventory is full.</returns>
        public Point? FirstEmptySlot(bool safeTab = true)
        {
            return FirstColorMatchingSlot(RGBHSBRangeFactory.EmptyInventorySlot(), 0.99, safeTab);
        }

        /// <summary>
        /// Determines if the given inventory slot matches a color filter
        /// </summary>
        /// <param name="x">slots away from the leftmost column (0-3) or screen x coordinate</param>
        /// <param name="y">slots away from the topmost column (0-6) or screen y coordinate</param>
        /// <param name="colorFilter">color range to match on</param>
        /// <param name="readScreen">set to true to reread the game screen before checking</param>
        /// <param name="safeTab">set to true to switch to the inventory tab even if it already thinks that it is selected</param>
        /// <returns>true if the slot matches a color filter</returns>
        public bool SlotMatchesColorFilter(int xSlot, int ySlot, ColorFilter colorFilter, double matchStrictness = 0.1, bool readScreen = false, bool safeTab = false)
        {
            int x = xSlot;
            int y = ySlot;
            InventoryToScreen(ref x, ref y);
            if (OpenInventory(safeTab) || readScreen)
            {
                Screen = ScreenScraper.GetRGB(ScreenScraper.CaptureWindow(RSClient, true));
            }

            int xOffset = (INVENTORY_GAP_X / 2) - 1;
            int yOffset = (INVENTORY_GAP_Y / 2) - 1;
            int left = x - xOffset;
            int right = x + xOffset;
            int top = y - yOffset;
            int bottom = y + yOffset;

            Color[,] itemIcon = ImageProcessing.ScreenPiece(Screen, left, right, top, bottom);
            double colorMatch = ImageProcessing.FractionalMatch(itemIcon, colorFilter);
            return (colorMatch > matchStrictness);
        }

        /// <summary>
        /// Determines if the given inventory slot contains any item
        /// </summary>
        /// <param name="x">slots away from the leftmost column (0-3) or screen x coordinate</param>
        /// <param name="y">slots away from the topmost column (0-6) or screen y coordinate</param>
        /// <param name="readScreen">set to true to reread the game screen before checking</param>
        /// <param name="safeTab">set to true to switch to the inventory tab even if it already thinks that it is selected</param>
        /// <returns>true if the slot is empty</returns>
        public bool SlotIsEmpty(int xSlot, int ySlot, bool readScreen = false, bool safeTab = false)
        {
            return SlotMatchesColorFilter(xSlot, ySlot, RGBHSBRangeFactory.EmptyInventorySlot(), 0.95, readScreen, safeTab);
        }

        /// <summary>
        /// Determines if the given inventory slot contains any item
        /// </summary>
        /// <param name="slot">inventor coordinates of the slot to check</param>
        /// <param name="readScreen">set to true to reread the game screen before checking</param>
        /// <param name="safeTab">set to true to switch to the inventory tab even if it already thinks that it is selected</param>
        /// <returns>true if the slot is empty</returns>
        public bool SlotIsEmpty(Point slot, bool readScreen = false, bool safeTab = false)
        {
            return SlotIsEmpty(slot.X, slot.Y, readScreen, safeTab);
        }

        /// <summary>
        /// Determines if the given inventory slot contains any item
        /// </summary>
        /// <param name="slot">index of the slot iin the inventory</param>
        /// <param name="readScreen">set to true to reread the game screen before checking</param>
        /// <param name="safeTab">set to true to switch to the inventory tab even if it already thinks that it is selected</param>
        /// <returns>true if the slot is empty</returns>
        public bool SlotIsEmpty(int slot, bool readScreen = false, bool safeTab = false)
        {
            Point slotCoordinates = InventoryIndexToCoordinates(slot);
            return SlotIsEmpty(slotCoordinates);
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

        /// <summary>
        /// Mirrors an inventory slot across the horzontal midline of the inventory.
        /// Slots in the middle row of an odd number of rows are mirrored horizontally.
        /// </summary>
        /// <param name="original"></param>
        /// <returns>A mirror of the input point original.</returns>
        public Point MirrorVertical(Point original)
        {
            int mirrorX = original.X;
            int mirrorY = (INVENTORY_ROWS - 1) - original.Y;

            //mirror horizontally for a middle row since it has no vertical mirror row
            if ((INVENTORY_ROWS % 2 == 1) && (original.Y == INVENTORY_ROWS / 2))
            {
                mirrorX = (INVENTORY_COLUMNS - 1) - original.X;
            }

            return new Point(mirrorX, mirrorY);
        }

        /// <summary>
        /// Mirrors an inventory slot across the vertical midline of the inventory.
        /// Slots in the middle row of an odd number of rows are mirrored horizontally.
        /// </summary>
        /// <param name="original"></param>
        /// <returns>A mirror of the input point original.</returns>
        public Point MirrorHorizontal(Point original)
        {
            int mirrorX = (INVENTORY_COLUMNS - 1) - original.X;
            int mirrorY = original.Y;

            //mirror vertically for a middle column since it has no horizontal mirror row
            if ((INVENTORY_ROWS % 2 == 1) && (original.Y == INVENTORY_ROWS / 2))
            {
                mirrorY = (INVENTORY_ROWS - 1) - original.Y;
            }

            return new Point(mirrorX, mirrorY);
        }

        #endregion

        #region equipment

        #region teleports

        /// <summary>
        /// Uses an amulet of glory to teleport the player
        /// </summary>
        /// <param name="teleportLocation">teleport location from the GloryTeleports enum</param>
        /// <returns>true if successful</returns>
        public bool GloryTeleport(GloryTeleports teleportLocation, bool waitForTeleport = true)
        {
            if (!EquipItemRightClick(1, 1, (int)teleportLocation))
            {
                return false;
            }
            if (waitForTeleport)
            {
                BotProgram.SafeWait(TELEPORT_DURATION);
            }
            return true;
        }
        public enum GloryTeleports : int
        {
            Edgeville = 1,
            Karamja = 2,
            Draynor = 3,
            AlKharid = 4
        }

        #endregion

        /// <summary>
        /// Selects one of the right-click options from an equipped item
        /// </summary>
        /// <param name="x">equipment column</param>
        /// <param name="y">equipment row</param>
        /// <param name="option">option index from right-click popup</param>
        /// <returns>true if successful</returns>
        public bool EquipItemRightClick(double x, double y, int option)
        {
            OpenEquipment();
            Point itemLocation = EquipmentLocation(x, y);
            return TryRightClickOption(itemLocation, option);
        }

        /// <summary>
        /// Repeatedly attempts to right-click an item and select one of the options from the popup
        /// </summary>
        /// <param name="itemLocation">location of the item in screen coordinates</param>
        /// <param name="option">index of the option from the right-click popup to select</param>
        /// <returns>true if successful</returns>
        private bool TryRightClickOption(Point itemLocation, int option)
        {
            RightClickInventory popup = null;
            Point click;

            for (int i = 0; i < 3; i++)
            {
                if (BotProgram.StopFlag) { return false; }

                click = Probability.GaussianCircle(new Point(itemLocation.X, itemLocation.Y), 4.0, 0, 360, 10);
                Mouse.RightClick(click.X, click.Y, RSClient, 0);
                popup = new RightClickInventory(click.X, click.Y, RSClient, null);
                if (popup.WaitForPopup(1500))
                {
                    popup.CustomOption(option);
                    return true;
                }
                else
                {
                    OpenInventory(true);
                }
            }
            return false;
        }

        /// <summary>
        /// Determines the location of an equipment slot in screen coordinates based on equipment tab coordinates
        /// </summary>
        /// <param name="x">horizontal equipment gaps from left equipment column (starting from 0)</param>
        /// <param name="y">vertical equipment gaps from top equipment row (starting from 0)</param>
        /// <returns>the screen coordinates in the middle of the equipmnt slot specified by the given equipment coordinates</returns>
        public Point EquipmentLocation(double column, double row)
        {
            int x = (int) (Screen.GetLength(0) - EQUIPMENT_LEFT_OFFSET_RIGHT + column * EQUIPMENT_HORIZONTAL_GAP);
            int y = (int)(Screen.GetLength(1) - EQUIPMENT_TOP_OFFSET_BOTTOM + row * EQUIPMENT_VERTICAL_GAP);
            return new Point(x, y);
        }

        #endregion

        #region spellbook

        #region teleports

        /// <summary>
        /// Attempts to teleport to the specified location
        /// </summary>
        /// <param name="location">location to teleport to</param>
        /// <param name="wait">whether to wait for expected teleport duration</param>
        /// <returns>False if the player doesn't have the runes to teleport</returns>
        public bool StandardTeleport(StandardTeleports location, bool wait = true, bool safeTab = false)
        {
            Point spellbookSlot = TeleportToSpellBookSlot(location);
            ClickSpellbookStandard(spellbookSlot.X, spellbookSlot.Y, safeTab);
            if (wait) { BotProgram.SafeWait(TELEPORT_DURATION); }
            return true;
        }

        /// <summary>
        /// Attempts to teleport to the specified location
        /// </summary>
        /// <param name="location">location to teleport to</param>
        /// <param name="wait">whether to wait for expected teleport duration</param>
        /// <returns>False if the player doesn't have the runes to teleport</returns>
        public bool HoverStandardTeleport(StandardTeleports location, bool wait = true, bool safeTab = false)
        {
            Point spellbookSlot = TeleportToSpellBookSlot(location);
            HoverSpellbookStandard(spellbookSlot.X, spellbookSlot.Y, safeTab);
            if (wait) { BotProgram.SafeWait(TELEPORT_DURATION); }
            return true;
        }

        /// <summary>
        /// Converts a standard spellbook teleport location to inventory slot coordinates
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public Point TeleportToSpellBookSlot(StandardTeleports location)
        {
            switch (location)
            {
                case StandardTeleports.Home:
                    return new Point(0, 0);
                case StandardTeleports.Varrock:
                    return new Point(1, 2);
                case StandardTeleports.Lumbridge:
                    return new Point(4, 2);
                case StandardTeleports.Falador:
                    return new Point(0, 3);
                case StandardTeleports.House:
                    return new Point(2, 3);
                case StandardTeleports.Camelot:
                    return new Point(5, 3);
                case StandardTeleports.Ardougne:
                    return new Point(4, 4);
                case StandardTeleports.Watchtower:
                    return new Point(2, 5);
                case StandardTeleports.Trollheim:
                    return new Point(2, 6);
                case StandardTeleports.ApeAtoll:
                    return new Point(5, 6);
                case StandardTeleports.Kourend:
                    return new Point(3, 7);
                default:
                    throw new Exception("Selected teleport not implemented");
            }
        }

        /// <summary>
        /// Determines if the player is carrying the required runes for the selected spell
        /// </summary>
        /// <param name="x">inventory column (0-6)</param>
        /// <param name="y">inventory row (0-9)</param>
        /// <returns></returns>
        public bool TeleportHasRunes(StandardTeleports location)
        {
            Point inventorySlot = TeleportToSpellBookSlot(location);
            int x = inventorySlot.X;
            int y = inventorySlot.Y;
            if (!SpellbookStandardToScreen(ref x, ref y))
            {
                return false;
            }
            OpenSpellbook(false);
            int radius = 5;
            Color[,] icon = ImageProcessing.ScreenPiece(Screen, x - radius, x + radius, y - radius, y + radius);
            Color colorAverage = ImageProcessing.ColorAverage(icon);
            float brightness = colorAverage.GetBrightness();
            return brightness > 0.2f;
        }

        public enum StandardTeleports : int
        {
            Home,
            Varrock,
            Lumbridge,
            Falador,
            House,
            Camelot,
            Ardougne,
            Watchtower,
            Trollheim,
            ApeAtoll,
            Kourend
        }

        #endregion

        /// <summary>
        /// Clicks a slot in the standard spellbook.
        /// Does not handle waiting after casting a spell.
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="x">slots away from the leftmost column (0-3)</param>
        /// <param name="y">slots away from the topmost column (0-6)</param>
        public void ClickSpellbookStandard(int x, int y, bool safeTab = false)
        {
            OpenSpellbook(safeTab);
            SpellbookStandardToScreen(ref x, ref y);
            Mouse.LeftClick(x, y, RSClient, 5);
        }
        /// <summary>
        /// Clicks a slot in the standard spellbook.
        /// Does not handle waiting after casting a spell.
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="x">slots away from the leftmost column (0-3)</param>
        /// <param name="y">slots away from the topmost column (0-6)</param>
        public void HoverSpellbookStandard(int x, int y, bool safeTab = false)
        {
            OpenSpellbook(safeTab);
            SpellbookStandardToScreen(ref x, ref y);
            Mouse.Move(x, y, RSClient);
        }

        /// <summary>
        /// Clicks a slot in the lunar spellbook.
        /// Does not handle waiting after casting a spell.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="safeTab"></param>
        public void ClickSpellbookLunar(int x, int y, bool safeTab = false)
        {
            OpenSpellbook(safeTab);
            SpellbookLunarToScreen(ref x, ref y);
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
            ClickSpellbookStandard(5, 2, safeTab);
            Mouse.LeftClick(x, y, RSClient, 1);
            if (autoWait)
            {
                BotProgram.SafeWait(5000, 300); //telegrab takes about 5 seconds
            }
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
                ClickSpellbookStandard(spellBookX, spellBookY, safeTab);
                SelectedTab = TabSelect.Inventory;
                ClickInventory(x, y, false);
                SelectedTab = TabSelect.Spellbook;
                if (autoWait)
                {
                    BotProgram.SafeWaitPlus(castTime, 100);
                }
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
        /// Converts coordinates from the standard spellbook to screen coordinates if the cordinates exceed the size of the spellbook
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>true if translation succeeds</returns>
        private bool SpellbookStandardToScreen(ref int x, ref int y)
        {
            if (x < 0 || x >= SPELLBOOK_STANDARD_COLUMNS || y < 0 || y >= SPELLBOOK_STANDARD_ROWS)
            {
                return false;
            }
            ManuallySetScreen(false);
            x = Screen.GetLength(0) - SPELLBOOK_STANDARD_OFFSET_LEFT + (x * SPELLBOOK_STANDARD_GAP_X);
            y = Screen.GetLength(1) - SPELLBOOK_STANDARD_OFFSET_TOP + (y * SPELLBOOK_STANDARD_GAP_Y);
            return true;
        }

        /// <summary>
        /// Converts coordinates from the standard spellbook to screen coordinates if the cordinates exceed the size of the spellbook
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>true if translation succeeds</returns>
        private bool SpellbookLunarToScreen(ref int x, ref int y)
        {
            if (x < 0 || x >= SPELLBOOK_LUNAR_COLUMNS || y < 0 || y >= SPELLBOOK_LUNAR_ROWS)
            {
                return false;
            }
            ManuallySetScreen(false);
            x = Screen.GetLength(0) - SPELLBOOK_LUNAR_OFFSET_LEFT + (x * SPELLBOOK_LUNAR_GAP_X);
            y = Screen.GetLength(1) - SPELLBOOK_LUNAR_OFFSET_TOP + (y * SPELLBOOK_LUNAR_GAP_Y);
            return true;
        }

        #endregion

        #region constants

        /// <summary>
        /// Milliseconds to wait after switching tabs
        /// </summary>
        private const int TAB_SWITCH_WAIT = BotRegistry.GAME_TICK;

        public const int TAB_HORIZONTAL_GAP = 33;
        public const int TAB_RIGHT_OFFSET_RIGHT = 22;
        public const int TAB_TOP_OFFSET_BOTTOM = 317;
        public const int TAB_BOTTOM_OFFSET_BOTTOM = 18;

        public const int INVENTORY_TAB_OFFSET_RIGHT = TAB_RIGHT_OFFSET_RIGHT + 3 * TAB_HORIZONTAL_GAP;
        public const int INVENTORY_OFFSET_LEFT = 185;
        public const int INVENTORY_OFFSET_TOP = 275;
        public const int INVENTORY_GAP_X = 42;
        public const int INVENTORY_GAP_Y = 36;
        public const int INVENTORY_CAPACITY = INVENTORY_ROWS * INVENTORY_COLUMNS;
        public const int INVENTORY_COLUMNS = 4;
        public const int INVENTORY_ROWS = 7;

        public const int EQUIPMENT_TAB_OFFSET_RIGHT = TAB_RIGHT_OFFSET_RIGHT + 2 * TAB_HORIZONTAL_GAP;
        public const int EQUIPMENT_LEFT_OFFSET_RIGHT = 177; //pixels from the left column of the equipment tab to the right edge of the screen
        public const int EQUIPMENT_TOP_OFFSET_BOTTOM = 276;
        public const int EQUIPMENT_HORIZONTAL_GAP = 56;
        public const int EQUIPMENT_VERTICAL_GAP = 40;
        public const int EQUIPMENT_PARTIAL_GAP = 16;    //the gap between the left column and the cape slot or the right column and the ammo slot

        public const int SPELLBOOK_TAB_OFFSET_RIGHT = 21;
        public const int SPELLBOOK_TAB_OFFSET_BOTTOM = 320;
        public const int SPELLBOOK_STANDARD_OFFSET_LEFT = 191;   //horizontal distance between middle of home teleport icon and right of screen
        public const int SPELLBOOK_STANDARD_OFFSET_TOP = 272;    //vertical distance between middle of home teleport icon and bottom of screen
        public const int SPELLBOOK_STANDARD_GAP_X = 24;
        public const int SPELLBOOK_STANDARD_GAP_Y = 24;
        public const int SPELLBOOK_STANDARD_COLUMNS = 7;
        public const int SPELLBOOK_STANDARD_ROWS = 10;

        public const int SPELLBOOK_LUNAR_OFFSET_LEFT = 197;   //horizontal distance between middle of home teleport icon and right of screen
        public const int SPELLBOOK_LUNAR_OFFSET_TOP = 278;    //vertical distance between middle of home teleport icon and bottom of screen
        public const int SPELLBOOK_LUNAR_GAP_X = 30;
        public const int SPELLBOOK_LUNAR_GAP_Y = 29;
        public const int SPELLBOOK_LUNAR_COLUMNS = 6;
        public const int SPELLBOOK_LUNAR_ROWS = 8;

        public const int OPTIONS_TAB_OFFSET_RIGHT = 89;
        public const int OPTIONS_TAB_OFFSET_BOTTOM = 19;

        public const int LOGOUT_TAB_OFFSET_RIGHT = TAB_RIGHT_OFFSET_RIGHT + 3 * TAB_HORIZONTAL_GAP;

        public const int TELEPORT_DURATION = 5 * BotRegistry.GAME_TICK;

        public TabSelect SelectedTab { get; set; }
        public enum TabSelect : int
        {
            Unknown,
            Inventory,
            Equipment,
            Spellbook,
            Logout,
            Options
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
