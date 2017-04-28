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
            HSBRange hsbRange = new HSBRange(0, 0, 0.05f, 0.3f, 0.02f, 0.12f);
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
            HSBRange hsbRange = new HSBRange(215, 315, 0.1f, 0.2f, 0.23f, 0.46f);
            return new ColorRange(dark, light, hsbRange);
        }
        #endregion

        #region minimap icons
        /// <summary>
        /// The orange part of a furnace minimap icon
        /// </summary>
        /// <returns></returns>
        public static ColorRange FurnaceIconOrange()
        {
            Color dark = Color.FromArgb(200, 70, 20);
            Color light = Color.FromArgb(255, 220, 200);
            HSBRange hsbRange = new HSBRange(20, 40, 0.52f, 1.0f, 0.50f, 0.75f);
            return new ColorRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The yellow part of a bank minimap icon
        /// </summary>
        /// <returns></returns>
        public static ColorRange BankIconDollar()
        {
            Color dark = Color.FromArgb(120, 100, 40);
            Color light = Color.FromArgb(250, 220, 140);
            HSBRange hsbRange = new HSBRange(40, 52, 0.35f, 0.9f, 0.30f, 0.76f);
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
    }
}
