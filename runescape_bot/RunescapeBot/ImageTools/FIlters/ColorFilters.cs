using RunescapeBot.ImageTools;
using System.Drawing;

namespace RunescapeBot.BotPrograms
{
    public static class ColorFilters
    {
        #region login
        /// <summary>
        /// Creates the color range to represent a lesser demon's red skin
        /// </summary>
        /// <returns></returns>
        public static ColorRange WelcomeScreenClickHere()
        {
            Color dark = Color.FromArgb(25, 0, 0);
            Color light = Color.FromArgb(140, 70, 50);
            HSBRange hsbRange = new HSBRange(355, 20, 0.4f, 1f, 0.05f, 0.5f);
            return new ColorRange(dark, light, hsbRange);
        }
        #endregion

        #region monsters
        /// <summary>
        /// Creates the color range to represent a lesser demon's red skin
        /// </summary>
        /// <returns></returns>
        public static ColorRange LesserDemonSkin()
        {
            Color dark = Color.FromArgb(25, 5, 0);
            Color light = Color.FromArgb(140, 70, 50);
            HSBRange hsbRange = new HSBRange(355, 20, 0.4f, 1f, 0.05f, 0.5f);
            return new ColorRange(dark, light, hsbRange);

        }

        /// <summary>
        /// Creates the color range to represent a lesser demon's horns, hoofs, and tail spike
        /// </summary>
        /// <returns></returns>
        public static ColorRange LesserDemonHorn()
        {
            Color dark = Color.FromArgb(0, 0, 0);
            Color light = Color.FromArgb(30, 30, 30);
            HSBRange hsbRange = new HSBRange(0, 0, 0.030f, 0.400f, 0.020f, 0.120f);
            return new ColorRange(dark, light, hsbRange);
        }
        #endregion

        #region stationary objcts

        /// <summary>
        /// The gray top of a furnace
        /// </summary>
        /// <returns></returns>
        public static ColorRange Furnace()
        {
            Color dark = Color.FromArgb(46, 43, 43);
            Color light = Color.FromArgb(85, 80, 78);
            HSBRange hsbRange = new HSBRange(0, 0, 0.02f, 0.07f, 0.18f, 0.32f);
            return new ColorRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The teller side counter of a Port Phasmatys bank booth
        /// </summary>
        /// <returns></returns>
        public static ColorRange BankBoothPhasmatys()
        {
            Color dark = Color.FromArgb(62, 67, 27);
            Color light = Color.FromArgb(71, 76, 36);
            HSBRange hsbRange = new HSBRange(64, 72, 0.360f, 0.418f, 0.182f, 0.222f);
            return new ColorRange(dark, light, hsbRange);
        }

        #region gnome stronghold agility obstacles

        /// <summary>
        /// The teller side counter of a Port Phasmatys bank booth
        /// </summary>
        /// <returns></returns>
        public static ColorRange LogBalance()
        {
            Color dark = Color.FromArgb(68, 39, 3);
            Color light = Color.FromArgb(107, 64, 17);
            HSBRange hsbRange = new HSBRange(30, 35, 0.735f, 0.875f, 0.143f, 0.236f);
            return new ColorRange(dark, light, hsbRange);
        }

        #endregion

        #endregion

        #region items (ground)
        /// <summary>
        /// Creates the color range to represent the rune color
        /// </summary>
        /// <returns></returns>
        public static ColorRange RuneMedHelm()
        {
            Color dark = Color.FromArgb(52, 70, 77);
            Color light = Color.FromArgb(95, 131, 137);
            HSBRange hsbRange = new HSBRange(190, 200, 0.18f, 0.21f, 0.245f, 0.48f);
            return new ColorRange(dark, light, hsbRange);
        }

        /// <summary>
        /// Creates the color range to represent the mithril color
        /// we have to be careful with this filter because it also picks up water and similarly colored shades
        /// </summary>
        /// <returns></returns>
        public static ColorRange MithrilArmor()
        {
            Color dark = Color.FromArgb(45, 45, 70);
            Color light = Color.FromArgb(105, 105, 130);
            HSBRange hsbRange = new HSBRange(215, 315, 0.100f, 0.200f, 0.203f, 0.460f);
            return new ColorRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The orange outline on a chaos rune
        /// </summary>
        /// <returns></returns>
        public static ColorRange ChaosRuneOrange()
        {
            Color dark = Color.FromArgb(218, 161, 20);
            Color light = Color.FromArgb(229, 174, 33);
            HSBRange hsbRange = new HSBRange(40, 46, 0.728f, 0.848f, 0.436f, 0.544f);
            return new ColorRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The white skull and crossbones on a death rune
        /// </summary>
        /// <returns></returns>
        public static ColorRange DeathRuneWhite()
        {
            Color dark = Color.FromArgb(223, 219, 219);
            Color light = Color.FromArgb(234, 230, 230);
            HSBRange hsbRange = new HSBRange(357, 3, 0.021f, 0.121f, 0.840f, 0.940f);
            return new ColorRange(dark, light, hsbRange);
        }
        #endregion

        #region minimap

        /// <summary>
        /// The yellow version of the run energy foot icon
        /// </summary>
        /// <returns></returns>
        public static ColorRange RunEnergyFoot()
        {
            Color dark = Color.FromArgb(215, 192, 77);
            Color light = Color.FromArgb(255, 236, 130);
            HSBRange hsbRange = new HSBRange(39, 60, 0.668f, 0.948f, 0.557f, 0.778f);
            return new ColorRange(dark, light, hsbRange);
        }

        /// <summary>
        /// Middle yellow to green range for the gauges aroound the minimap. Roughly 50% - 100%
        /// (hitpoints, prayer points, run energy, special attack energy)
        /// </summary>
        /// <returns></returns>
        public static ColorRange MinimapGaugeYellowGreen()
        {
            Color dark = Color.FromArgb(0, 163, 0);
            Color light = Color.FromArgb(194, 255, 121);
            HSBRange hsbRange = new HSBRange(67, 130, 0.174f, 1.000f, 0.400f, 0.663f);
            return new ColorRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The orange part of a furnace minimap icon
        /// </summary>
        /// <returns></returns>
        public static ColorRange FurnaceIconOrange()
        {
            Color dark = Color.FromArgb(146, 83, 0);
            Color light = Color.FromArgb(255, 255, 136);
            HSBRange hsbRange = new HSBRange(15, 40, 0.330f, 1.000f, 0.380f, 0.770f);
            return new ColorRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The yellow part of a bank minimap icon
        /// </summary>
        /// <returns></returns>
        public static ColorRange BankIconDollar()
        {
            Color dark = Color.FromArgb(94, 80, 0);
            Color light = Color.FromArgb(233, 231, 136);
            HSBRange hsbRange = new HSBRange(43, 69, 0.433f, 1.000f, 0.164f, 0.709f);
            return new ColorRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The brown 
        /// </summary>
        /// <returns></returns>
        public static ColorRange PhasmatysBuildingFloor()
        {
            Color dark = Color.FromArgb(44, 50, 34);
            Color light = Color.FromArgb(92, 96, 79);
            HSBRange hsbRange = new HSBRange(334, 100, 0.003f, 0.254f, 0.104f, 0.404f);
            return new ColorRange(dark, light, hsbRange);
        }

        #endregion

        #region Inventory
        /// <summary>
        /// Catches most of the colors found in the opaque background of the inventory
        /// </summary>
        /// <returns></returns>
        public static ColorRange EmptyInventorySlot()
        {
            Color dark = Color.FromArgb(56, 48, 35);
            Color light = Color.FromArgb(66, 58, 45);
            HSBRange hsbRange = new HSBRange(28, 40, 0.15f, 0.25f, 0.15f, 0.25f);
            return new ColorRange(dark, light, hsbRange);
        }
        #endregion

        #region textBoxes

        /// <summary>
        /// The gray background color of a right-click menu
        /// </summary>
        /// <returns></returns>
        public static ColorRange RightClickPopup()
        {
            Color dark = Color.FromArgb(91, 82, 69);
            Color light = Color.FromArgb(95, 86, 73);
            HSBRange hsbRange = new HSBRange(33, 37, 0.126f, 0.143f, 0.309f, 0.334f);
            return new ColorRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The orange title text in the bank screen
        /// </summary>
        /// <returns></returns>
        public static ColorRange BankTitle()
        {
            Color dark = Color.FromArgb(253, 150, 29);
            Color light = Color.FromArgb(255, 154, 33);
            HSBRange hsbRange = new HSBRange(31, 33, 0.995f, 1.000f, 0.556f, 0.566f);
            return new ColorRange(dark, light, hsbRange);
        }

        #endregion

        #region OSBuddy
        /// <summary>
        /// The gray background on the OSBuddy popup in the top-left of the screen that shows the current target's hitpoints
        /// </summary>
        /// <returns></returns>
        public static ColorRange OSBuddyEnemyHitpointsBackground()
        {
            Color dark = Color.FromArgb(66, 60, 52);
            Color light = Color.FromArgb(68, 62, 54);
            HSBRange hsbRange = new HSBRange(33, 35, 0.116f, 0.118f, 0.234f, 0.236f);
            return new ColorRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The green portion of the health bar on the OSBuddy popup in the top-left of the screen that shows the current target's hitpoints
        /// </summary>
        /// <returns></returns>
        public static ColorRange OSBuddyEnemyHitpointsGreen()
        {
            Color dark = Color.FromArgb(45, 121, 35);
            Color light = Color.FromArgb(47, 123, 37);
            HSBRange hsbRange = new HSBRange(112, 114, 0.543f, 0.545f, 0.309f, 0.311f);
            return new ColorRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The red portion of the health bar on the OSBuddy popup in the top-left of the screen that shows the current target's hitpoints
        /// </summary>
        /// <returns></returns>
        public static ColorRange OSBuddyEnemyHitpointsRed()
        {
            Color dark = Color.FromArgb(125, 41, 35);
            Color light = Color.FromArgb(127, 43, 37);
            HSBRange hsbRange = new HSBRange(3, 5, 0.555f, 0.557f, 0.317f, 0.319f);
            return new ColorRange(dark, light, hsbRange);
        }
        #endregion
    }
}