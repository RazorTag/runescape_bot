using RunescapeBot.Common;
using RunescapeBot.ImageTools;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace RunescapeBot.BotPrograms
{
    public class Combat : BotProgram
    {
        //the bounds of the entire target monster hitpoints popup
        const int TARGET_HP_LEFT = 6;
        const int TARGET_HP_RIGHT = 130;
        const int TARGET_HP_TOP = 24;
        const int TARGET_HP_BOTTOM = 55;

        //the hitpoint measurement from the last time that hitpoints were checked for a decrease
        //set to double.MaxValue when no hitpoint bar is found
        private KeyValuePair<DateTime, double> oldHitpoints;

        public Combat(RunParams startParams) : base(startParams)
        {
            RunParams.ClientType = ScreenScraper.Client.OSBuddy;
        }

        /// <summary>
        /// Determines whether the player is in combat using OSBuddy's target hitpoints indicator
        /// </summary>
        /// <returns></returns>
        protected bool InCombat()
        {
            const double minBackground = 0.1;

            RGBHSBRange background = RGBHSBRangeFactory.OSBuddyEnemyHitpointsBackground();
            Screen.UpdateScreenshot();

            bool[,] targetBackground = Vision.ColorFilterPiece(background, TARGET_HP_LEFT, TARGET_HP_RIGHT, TARGET_HP_TOP, TARGET_HP_BOTTOM);
            double backgroundMatch = ImageProcessing.FractionalMatch(targetBackground);
            if (backgroundMatch >= minBackground)
            {
                return true;
            }
            else
            {
                oldHitpoints = new KeyValuePair<DateTime, double>(DateTime.Now, double.MaxValue);
                return false;
            }
        }

        /// <summary>
        /// Estimates the target's fraction of its maximum hitpoints using OSBuddy's target hitpoints indicator
        /// </summary>
        /// <returns></returns>
        protected double TargetHitpointFraction()
        {
            if (!InCombat()) { return double.MaxValue; }

            Screen.UpdateScreenshot();
            Color[,] targetHitpoints = Vision.ScreenPiece(TARGET_HP_LEFT, TARGET_HP_RIGHT, TARGET_HP_TOP, TARGET_HP_BOTTOM);
            RGBHSBRange greenHPBar = RGBHSBRangeFactory.OSBuddyEnemyHitpointsGreen();
            bool[,] greenHP = Vision.ColorFilter(targetHitpoints, greenHPBar);
            double greenWidth = ImageProcessing.BiggestBlob(greenHP).Width;
            RGBHSBRange redHPBar = RGBHSBRangeFactory.OSBuddyEnemyHitpointsRed();
            bool[,] redHP = Vision.ColorFilter(targetHitpoints, redHPBar);
            double redWidth = ImageProcessing.BiggestBlob(redHP).Width;

            return greenWidth / Numerical.NonZero(greenWidth + redWidth);
        }

        /// <summary>
        /// Determines if a monsters hitpoint bar is lower than the last recorded instance
        /// </summary>
        /// <param name="checkInterval">minimum time between hitpoint checks in milliseconds</param>
        /// <returns>
        /// Always returns true if a full check interval hasn't elapsed since the last check. 
        /// Otherwise, returns true if the hitpoints are measured as less than the last check.
        /// </returns>
        protected bool HitpointsHaveDecreased(int checkInterval = 5000)
        {
            if ((DateTime.Now - oldHitpoints.Key).TotalMilliseconds > checkInterval)
            {
                double currentHitpoints = TargetHitpointFraction();
                double previousHitpoints = oldHitpoints.Value;
                oldHitpoints = new KeyValuePair<DateTime, double>(DateTime.Now, currentHitpoints);
                return currentHitpoints < previousHitpoints;
            }

            return true;
        }
    }
}
