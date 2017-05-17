using RunescapeBot.ImageTools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms
{
    public class Combat : BotProgram
    {
        const int TARGET_HP_LEFT = 6;
        const int TARGET_HP_RIGHT = 130;
        const int TARGET_HP_TOP = 24;
        const int TARGET_HP_BOTTOM = 55;

        public Combat(StartParams startParams) : base(startParams) { }

        /// <summary>
        /// Determines whether the player is in combat using OSBuddy's target hitpoints indicator
        /// </summary>
        /// <returns></returns>
        protected bool InCombat()
        {
            const int maxScreenshotAge = 500;
            const double minBackground = 0.1;

            ColorRange background = ColorFilters.OSBuddyEnemyHitpointsBackground();

            if (TimeSinceLastScreenShot() <= maxScreenshotAge)
            {
                ReadWindow();
            }

            bool[,] targetBackground = ColorFilterPiece(background, TARGET_HP_LEFT, TARGET_HP_RIGHT, TARGET_HP_TOP, TARGET_HP_BOTTOM);
            double backgroundMatch = ImageProcessing.FractionalMatch(targetBackground);
            return backgroundMatch >= minBackground;
        }

        /// <summary>
        /// Estimates the target's fraction of its maximum hitpoints using OSBuddy's target hitpoints indicator
        /// </summary>
        /// <returns></returns>
        protected double TargetHitpointFraction()
        {
            const int maxScreenshotAge = 500;

            if (TimeSinceLastScreenShot() <= maxScreenshotAge)
            {
                ReadWindow();
            }

            Color[,] targetHitpoints = ScreenPiece(TARGET_HP_LEFT, TARGET_HP_RIGHT, TARGET_HP_TOP, TARGET_HP_BOTTOM);
            ColorRange greenHPBar = ColorFilters.OSBuddyEnemyHitpointsGreen();
            bool[,] greenHP = ColorFilter(targetHitpoints, greenHPBar);
            double greenWidth = ImageProcessing.BiggestBlob(greenHP).Width;
            ColorRange redHPBar = ColorFilters.OSBuddyEnemyHitpointsRed();
            bool[,] redHP = ColorFilter(targetHitpoints, redHPBar);
            double redWidth = ImageProcessing.BiggestBlob(redHP).Width;

            return greenWidth / (greenWidth + redWidth);
        }
    }
}
