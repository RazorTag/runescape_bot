using RunescapeBot.ImageTools;
using System.Drawing;

namespace RunescapeBot.BotPrograms
{
    public static class RGBHSBRangeFactory
    {
        #region login
        /// <summary>
        /// Creates the color range to represent a lesser demon's red skin
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange WelcomeScreenClickHere()
        {
            Color dark = Color.FromArgb(25, 0, 0);
            Color light = Color.FromArgb(140, 70, 50);
            HSBRange hsbRange = new HSBRange(355, 20, 0.4f, 1f, 0.05f, 0.5f);
            return new RGBHSBRange(dark, light, hsbRange);
        }
        #endregion

        #region monsters
        /// <summary>
        /// Creates the color range to represent a lesser demon's red skin
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange LesserDemonSkin()
        {
            Color dark = Color.FromArgb(25, 5, 0);
            Color light = Color.FromArgb(140, 70, 50);
            HSBRange hsbRange = new HSBRange(355, 20, 0.4f, 1f, 0.05f, 0.5f);
            return new RGBHSBRange(dark, light, hsbRange);

        }

        /// <summary>
        /// Creates the color range to represent a lesser demon's horns, hoofs, and tail spike
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange LesserDemonHorn()
        {
            Color dark = Color.FromArgb(0, 0, 0);
            Color light = Color.FromArgb(30, 30, 30);
            HSBRange hsbRange = new HSBRange(355, 5, 0.030f, 0.400f, 0.020f, 0.120f);
            return new RGBHSBRange(dark, light, hsbRange);
        }
        #endregion

        #region stationary objcts

        /// <summary>
        /// The gray top of a furnace
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange Furnace()
        {
            Color dark = Color.FromArgb(46, 43, 43);
            Color light = Color.FromArgb(85, 80, 78);
            HSBRange hsbRange = new HSBRange(0, 360, 0.02f, 0.07f, 0.18f, 0.32f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The white tarp above a tea stall
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange TeaStallRoof()
        {
            Color dark = Color.FromArgb(173, 159, 135);
            Color light = Color.FromArgb(203, 189, 168);
            HSBRange hsbRange = new HSBRange(31, 44, 0.169f, 0.260f, 0.603f, 0.728f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The teller side counter of a Port Phasmatys bank booth
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange BankBoothPhasmatys()
        {
            Color dark = Color.FromArgb(62, 67, 27);
            Color light = Color.FromArgb(71, 76, 36);
            HSBRange hsbRange = new HSBRange(64, 72, 0.360f, 0.418f, 0.182f, 0.222f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The teller side counter of a Port Phasmatys bank booth
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange BankBoothVarrockWest()
        {
            Color dark = Color.FromArgb(78, 67, 34);
            Color light = Color.FromArgb(101, 87, 46);
            HSBRange hsbRange = new HSBRange(43, 47, 0.358f, 0.395f, 0.217f, 0.290f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// Neutral purple from a house portal.
        /// Does not cover all of the purple shades seen in the portal
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange HousePortalPurple()
        {
            Color dark = Color.FromArgb(59, 15, 78);
            Color light = Color.FromArgb(141, 78, 176);
            HSBRange hsbRange = new HSBRange(273.6f, 288.7f, 0.293f, 0.724f, 0.152f, 0.529f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// Brown wood from a bank chest (like at castle wars)
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange BankChest()
        {
            Color dark = Color.FromArgb(30, 21, 14);
            Color light = Color.FromArgb(156, 118, 87);
            HSBRange hsbRange = new HSBRange(23.0f, 29.2f, 0.270f, 0.322f, 0.050f, 0.507f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        #endregion

        #region agility obstacles

        /// <summary>
        /// The teller side counter of a Port Phasmatys bank booth
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange LogBalance()
        {
            Color dark = Color.FromArgb(61, 33, 0);
            Color light = Color.FromArgb(110, 68, 14);
            HSBRange hsbRange = new HSBRange(26, 40, 0.728f, 1.000f, 0.079f, 0.274f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The wooden frame for a cargo net
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange CargoNetFrameTop()
        {
            Color dark = Color.FromArgb(90, 54, 8);
            Color light = Color.FromArgb(92, 56, 10);
            HSBRange hsbRange = new HSBRange(33, 35, 0.815f, 0.825f, 0.191f, 0.201f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// Climbable tree branch from the Gnome Stronghold agility course
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange GnomeTreeBranch()
        {
            Color dark = Color.FromArgb(52, 39, 8);
            Color light = Color.FromArgb(147, 121, 71);
            HSBRange hsbRange = new HSBRange(37, 44, 0.355f, 0.668f, 0.127f, 0.418f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The cutout of the tree trunk for a climbable tree
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange GnomeTreeTrunk()
        {
            Color dark = Color.FromArgb(75, 59, 39);
            Color light = Color.FromArgb(148, 120, 86);
            HSBRange hsbRange = new HSBRange(30, 37, 0.263f, 0.302f, 0.233f, 0.449f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// Traversable tightrope
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange Tightrope()
        {
            Color dark = Color.FromArgb(119, 95, 65);
            Color light = Color.FromArgb(174, 142, 101);
            HSBRange hsbRange = new HSBRange(31, 36, 0.264f, 0.308f, 0.370f, 0.530f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// THe rim of the opening of a drain pipe
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange DrainPipe()
        {
            Color dark = Color.FromArgb(86, 80, 80);
            Color light = Color.FromArgb(129, 121, 121);
            HSBRange hsbRange = new HSBRange(355, 12, 0.027f, 0.044f, 0.335f, 0.483f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        #endregion

        #region items (ground)
        /// <summary>
        /// Creates the color range to represent the rune color
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange RuneMedHelm()
        {
            Color dark = Color.FromArgb(52, 70, 77);
            Color light = Color.FromArgb(95, 131, 137);
            HSBRange hsbRange = new HSBRange(190, 200, 0.18f, 0.21f, 0.245f, 0.48f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// Creates the color range to represent the mithril color
        /// we have to be careful with this filter because it also picks up water and similarly colored shades
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange MithrilArmor()
        {
            Color dark = Color.FromArgb(45, 45, 70);
            Color light = Color.FromArgb(105, 105, 130);
            HSBRange hsbRange = new HSBRange(215, 315, 0.100f, 0.200f, 0.203f, 0.460f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The orange outline on a chaos rune
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange ChaosRuneOrange()
        {
            Color dark = Color.FromArgb(218, 161, 20);
            Color light = Color.FromArgb(229, 174, 33);
            HSBRange hsbRange = new HSBRange(40, 46, 0.728f, 0.848f, 0.436f, 0.544f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The white skull and crossbones on a death rune
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange DeathRuneWhite()
        {
            Color dark = Color.FromArgb(223, 219, 219);
            Color light = Color.FromArgb(234, 230, 230);
            HSBRange hsbRange = new HSBRange(357, 3, 0.021f, 0.121f, 0.840f, 0.940f);
            return new RGBHSBRange(dark, light, hsbRange);
        }
        #endregion

        #region minimap

        /// <summary>
        /// The yellow version of the run energy foot icon
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange RunEnergyFoot()
        {
            Color dark = Color.FromArgb(215, 192, 77);
            Color light = Color.FromArgb(255, 236, 130);
            HSBRange hsbRange = new HSBRange(39, 60, 0.668f, 0.948f, 0.557f, 0.778f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// Middle yellow to green range for the gauges aroound the minimap. Roughly 50% - 100%
        /// (hitpoints, prayer points, run energy, special attack energy)
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange MinimapGaugeYellowGreen()
        {
            Color dark = Color.FromArgb(0, 163, 0);
            Color light = Color.FromArgb(194, 255, 121);
            HSBRange hsbRange = new HSBRange(67, 130, 0.174f, 1.000f, 0.400f, 0.663f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The orange part of a furnace minimap icon
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange FurnaceIconOrange()
        {
            Color dark = Color.FromArgb(146, 83, 0);
            Color light = Color.FromArgb(255, 255, 136);
            HSBRange hsbRange = new HSBRange(15, 40, 0.330f, 1.000f, 0.380f, 0.770f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The yellow part of a bank minimap icon
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange BankIconDollar()
        {
            Color dark = Color.FromArgb(94, 80, 0);
            Color light = Color.FromArgb(233, 231, 136);
            HSBRange hsbRange = new HSBRange(43, 69, 0.433f, 1.000f, 0.164f, 0.709f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The brown floor of a builidng on the minimap
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange PhasmatysBuildingFloorLight()
        {
            Color dark = Color.FromArgb(25, 31, 19);
            Color light = Color.FromArgb(99, 103, 82);
            HSBRange hsbRange = new HSBRange(334, 95, 0.083f, 0.191f, 0.117f, 0.363f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The brown floor of a builidng on the minimap
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange PhasmatysBuildingFloorDark()
        {
            Color dark = Color.FromArgb(25, 31, 19);
            Color light = Color.FromArgb(99, 103, 82);
            HSBRange hsbRange = new HSBRange(334, 80, 0.083f, 0.191f, 0.117f, 0.363f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        #endregion

        #region Inventory

        /// <summary>
        /// Catches most of the colors found in the opaque background of the inventory
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange EmptyInventorySlot()
        {
            Color dark = Color.FromArgb(56, 48, 35);
            Color light = Color.FromArgb(66, 58, 45);
            HSBRange hsbRange = new HSBRange(28, 40, 0.15f, 0.25f, 0.15f, 0.25f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// Catches most of the colors found in the opaque background of the inventory
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange InventorySlotWindows10Watermark()
        {
            Color dark = Color.FromArgb(56, 48, 35);
            Color light = Color.FromArgb(151, 149, 146);
            HSBRange hsbRange = new HSBRange(18, 42, 0.000f, 0.25f, 0.15f, 0.700f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        #endregion

        #region textBoxes

        /// <summary>
        /// The gray background color of a right-click menu
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange RightClickPopup()
        {
            Color dark = Color.FromArgb(91, 82, 69);
            Color light = Color.FromArgb(95, 86, 73);
            HSBRange hsbRange = new HSBRange(33, 37, 0.126f, 0.143f, 0.309f, 0.334f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The orange title text in the bank screen
        /// Also applies to lots of inventory text
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange BankTitle()
        {
            Color dark = Color.FromArgb(253, 150, 29);
            Color light = Color.FromArgb(255, 154, 33);
            HSBRange hsbRange = new HSBRange(31, 33, 0.995f, 1.000f, 0.556f, 0.566f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The orange text for an item in an inventory right click menu
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange InventoryRightClickItemName()
        {
            Color dark = Color.FromArgb(245, 139, 59);
            Color light = Color.FromArgb(255, 149, 69);
            HSBRange hsbRange = new HSBRange(23, 27, 0.990f, 1.000f, 0.615f, 0.635f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The red text for a dialog box title
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange DialogBoxTitle()
        {
            Color dark = Color.FromArgb(127, 0, 0);
            Color light = Color.FromArgb(129, 1, 0);
            HSBRange hsbRange = new HSBRange(359.0f, 1.0f, 0.995f, 1.000f, 0.246f, 0.256f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        #endregion

        #region OSBuddy
        /// <summary>
        /// The gray background on the OSBuddy popup in the top-left of the screen that shows the current target's hitpoints
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange OSBuddyEnemyHitpointsBackground()
        {
            Color dark = Color.FromArgb(66, 60, 52);
            Color light = Color.FromArgb(68, 62, 54);
            HSBRange hsbRange = new HSBRange(33, 35, 0.116f, 0.118f, 0.234f, 0.236f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The green portion of the health bar on the OSBuddy popup in the top-left of the screen that shows the current target's hitpoints
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange OSBuddyEnemyHitpointsGreen()
        {
            Color dark = Color.FromArgb(45, 121, 35);
            Color light = Color.FromArgb(47, 123, 37);
            HSBRange hsbRange = new HSBRange(112, 114, 0.543f, 0.545f, 0.309f, 0.311f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The red portion of the health bar on the OSBuddy popup in the top-left of the screen that shows the current target's hitpoints
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange OSBuddyEnemyHitpointsRed()
        {
            Color dark = Color.FromArgb(125, 41, 35);
            Color light = Color.FromArgb(127, 43, 37);
            HSBRange hsbRange = new HSBRange(3, 5, 0.555f, 0.557f, 0.317f, 0.319f);
            return new RGBHSBRange(dark, light, hsbRange);
        }
        #endregion

        #region generic colors

        /// <summary>
        /// perfect black
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange Black()
        {
            Color dark = Color.FromArgb(0, 0, 0);
            Color light = Color.FromArgb(0, 0, 0);
            return new RGBHSBRange(dark, light, null);
        }

        /// <summary>
        /// Perfect yellow
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange Yellow()
        {
            Color dark = Color.FromArgb(255, 255, 0);
            Color light = Color.FromArgb(255, 255, 0);
            return new RGBHSBRange(dark, light, null);
        }

        #endregion

        #region mouseover description

        /// <summary>
        /// The blue text in the top-left corner of the screen when mousing over a clickable stationary object
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange MouseoverTextStationaryObject()
        {
            Color dark = Color.FromArgb(0, 189, 188);
            Color light = Color.FromArgb(39, 246, 245);
            HSBRange hsbRange = new HSBRange(175.8f, 182.0f, 0.717f, 1.000f, 0.352f, 0.542f); ;
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The blue text in the top-left corner of the screen when mousing over a clickable stationary object
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange MouseoverTextNPC()
        {
            Color dark = Color.FromArgb(181, 181, 0);
            Color light = Color.FromArgb(225, 225, 16);
            HSBRange hsbRange = new HSBRange(58.0f, 62.0f, 0.970f, 1.000f, 0.336f, 0.462f); ;
            return new RGBHSBRange(dark, light, hsbRange);
        }

        #endregion

        #region mining rocks

        /// <summary>
        /// The blue text in the top-left corner of the screen when mousing over a clickable stationary object
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange IronRock()
        {
            Color dark = Color.FromArgb(40, 25, 20);
            Color light = Color.FromArgb(95, 60, 45);
            HSBRange hsbRange = new HSBRange(14f, 20f, 0.35f, 0.43f, 0.09f, 0.27f) ;
            return new RGBHSBRange(dark, light, hsbRange);
        }

        #endregion

        #region fishing
        /// <summary>
        /// classic blue color of fish sprinkles
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange FishingTile()
        {
            Color dark = Color.FromArgb(116, 156, 213);
            Color light = Color.FromArgb(163, 190, 232);
            HSBRange hsbRange = new HSBRange(205f, 216f, 0.48f, 0.62f, 0.64f, 0.77f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        public static RGBHSBRange FishingPole()
        {
            Color dark = Color.FromArgb(45, 31, 0);
            Color light = Color.FromArgb(61, 41, 0);
            HSBRange hsbRange = new HSBRange(40f, 43f, 0.98f, 1.02f, 0.089f, 0.119f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        #endregion

    }
}