using RunescapeBot.ImageTools;
using System.Drawing;

namespace RunescapeBot.BotPrograms
{
    public static class ColorFilters
    {
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
            //Evan's conflict testing
            //mq test
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

        /// <summary>
        /// Creates the color range to represent the rune color
        /// </summary>
        /// <returns></returns>
        public static ColorRange RuneMedHelm()
        {
            Color dark = Color.FromArgb(52, 70, 77);
            Color light = Color.FromArgb(95, 131, 137);
            HSBRange hsbRange = new HSBRange(190, 200,  0.18f, 0.21f, 0.245f, 0.48f);
            return new ColorRange(dark, light, hsbRange);
        }

        /// <summary>
        /// Creates the color range to represent the mithril color
        /// </summary>
        /// <returns></returns>
        public static ColorRange MithrilArmor()
        {
            Color dark = Color.FromArgb(45, 45, 70);
            Color light = Color.FromArgb(100, 97, 125);
            HSBRange hsbRange = new HSBRange(239, 241, 0.1f, 0.2f, 0.23f, 0.44f);
            return new ColorRange(dark, light, hsbRange);
        }

        /// <summary>
        /// ===========>something else
        /// </summary>
        /// <returns></returns>
        public static ColorRange MerkleArmor()
        {
            Color dark = Color.FromArgb(45, 45, 70);
            Color light = Color.FromArgb(100, 97, 125);
            HSBRange hsbRange = new HSBRange(239, 241, 0.1f, 0.2f, 0.23f, 0.44f);
            return new ColorRange(dark, light, hsbRange);
        }
    }
}
