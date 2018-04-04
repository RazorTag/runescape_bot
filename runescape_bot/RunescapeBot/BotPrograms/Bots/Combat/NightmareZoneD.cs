using RunescapeBot.BotPrograms.FixedUIComponents;
using RunescapeBot.Common;
using RunescapeBot.ImageTools;
using RunescapeBot.UITools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms
{
    class NightmareZoneD : BotProgram
    {
        RGBHSBRange OverloadInventory = RGBHSBRangeFactory.OverloadInventory();
        RGBHSBRange OverloadTimer = RGBHSBRangeFactory.OverloadTimer();
        RGBHSBRange AbsorptionInventory = RGBHSBRangeFactory.Absorption();
        RGBHSBRange HitpointsRed = RGBHSBRangeFactory.HitpointsRed();
        RGBHSBRange HitpointsGreen = RGBHSBRangeFactory.HitpointsGreen();
        RGBHSBRange PotionTimerBackground = RGBHSBRangeFactory.PotionTimerBackground();

        protected Point rockCake;   //location of the rock cake in the inventory
        protected bool hasOverloads;
        protected bool hasAbsorptions;
        protected DateTime lastOverload;
        protected const int overloadDrainTime = 13000;  //time in milliseconds to wait for a dose of overload to take effect

        public NightmareZoneD(RunParams startParams) : base(startParams)
        {
            startParams.FrameTime = 5000;
            startParams.ClientType = ScreenScraper.Client.OSBuddy;
            startParams.BotWorldCheckEnabled = false;
            hasOverloads = true;
            hasAbsorptions = true;
            rockCake = new Point(0, 0);
            lastOverload = DateTime.MinValue;
        }

        protected override bool Run()
        {
            //ReadWindow();
            //DebugUtilities.SaveImageToFile(Bitmap, "C:\\Projects\\Roboport\\test_pictures\\nightmare_zone\\test.png");

            //ReadWindow();
            //bool[,] furnaceIcon = ColorFilter(OverloadInventory);
            //DebugUtilities.TestMask(Bitmap, ColorArray, OverloadInventory, furnaceIcon, "C:\\Projects\\Roboport\\test_pictures\\mask_tests\\", "potions");

            //ReadWindow();
            //AbsorptionShieldIsHigh();

            //ReadWindow();
            //Minimap.HighHitpoints();

            //ReadWindow();
            //Overload();

            //ReadWindow();
            //Absorption();

            //ReadWindow();
            //Hitpoints();

            return true;
        }

        protected override bool Execute()
        {
            ReadWindow();

            if (Minimap.HighHitpoints())
            {
                return false;   //Assume that we died in the Nightmare Zone if hitpoints suddenly become full
            }

            if (Overload() || Hitpoints() || Absorption())
            {
                if (StopFlag) { return false; }
                Point inventoryCorner = new Point(ScreenWidth - Inventory.INVENTORY_OFFSET_LEFT - Inventory.INVENTORY_GAP_X, ScreenHeight - Inventory.INVENTORY_OFFSET_TOP - Inventory.INVENTORY_GAP_Y);
                int acceptableAreaRadius = (int) Geometry.DistanceBetweenPoints(Center, inventoryCorner);
                MoveMouse(Center.X, Center.Y, acceptableAreaRadius);
                SafeWait(3000);
            }

            return true;
        }

        /// <summary>
        /// Eats a bite of rock cake if hitpoints are above 2
        /// </summary>
        /// <returns>true if a bite of rock cake is taken</returns>
        protected bool Hitpoints()
        {
            if ((DateTime.Now - lastOverload).TotalMilliseconds < overloadDrainTime)
            {
                return false;   //an overload might be taking effect
            }

            const double twoHitpointsMatch = 0.0487;
            RectangleBounds hitpoints = Minimap.HitpointsDigitsArea(ScreenWidth);
            double redHitpointsMatch = FractionalMatchPiece(HitpointsRed, hitpoints.Left, hitpoints.Right, hitpoints.Top, hitpoints.Bottom);

            if (Numerical.WithinRange(redHitpointsMatch, twoHitpointsMatch, 0.01*twoHitpointsMatch)) //something other than 2 hitpoints
            {
                return false;   //hitpoints are already at 2
            }

            Inventory.ClickInventory(0, 0, false);
            return true;
        }

        /// <summary>
        /// Drinks a dose of absorption potion if the absorption shield is low
        /// </summary>
        /// <returns>true if a dose of absorption is consumed</returns>
        protected bool Absorption()
        {
            if (!hasAbsorptions || AbsorptionShieldIsHigh())
            {
                return false; //The absorption shield is already high or we ran out of absorptions to drink
            }

            Point? firstAbsorption = Inventory.FirstColorMatchingSlot(AbsorptionInventory, 0.01, false);
            if (firstAbsorption == null || !firstAbsorption.HasValue)
            {
                hasAbsorptions = false;
                return false;
            }

            Inventory.ClickInventory(firstAbsorption.Value, false);
            return true;
        }

        /// <summary>
        /// Determines if the absorption shield is at least 900 and therefore close to maximum
        /// </summary>
        /// <returns>true is the absorption shield has a value of 900-999 (999 is max value but 1000+ would break this method if made possible)</returns>
        protected bool AbsorptionShieldIsHigh()
        {
            int left = 20;
            int right = 31;
            int top = 25;
            int bottom = 59;

            double hundredsPlaceWhite = FractionalMatchPiece(RGBHSBRangeFactory.White(), left, right, top, bottom);
            return Numerical.WithinRange(hundredsPlaceWhite, 0.1476, 0.0005);    //TODO determine good values for range
        }

        /// <summary>
        /// Drinks a dose of overload if the timer does not show up above the chat box
        /// </summary>
        /// <returns>true if a dose of overload is consumed</returns>
        protected bool Overload()
        {
            if (!hasOverloads || OverloadTimerExists() || (DateTime.Now - lastOverload).TotalMilliseconds < overloadDrainTime)
            {
                return false;   //An overload is active or we ran out of overloads to drink
            }

            Point? firstOverload = Inventory.FirstColorMatchingSlot(OverloadInventory, 0.01, false);
            if (firstOverload == null || !firstOverload.HasValue)
            {
                hasOverloads = false;
                return false;
            }

            Inventory.ClickInventory(firstOverload.Value, false);
            lastOverload = DateTime.Now;
            return true;
        }

        /// <summary>
        /// Determines if an overload potion is active by looking for the timer above the chat box
        /// </summary>
        /// <returns>true if the timer is found in the expected location above the right side of the chatbox</returns>
        protected bool OverloadTimerExists()
        {
            //bool[,] potionTimerSlot = potionTimerSlot = ColorFilterPiece(OverloadTimer, new Point(503, ScreenHeight - 185), 5);
            //double overloadTimerMatch = overloadTimerMatch = ImageProcessing.FractionalMatch(potionTimerSlot);
            //return overloadTimerMatch > 0.5;

            int left = 478;
            int right = left + 37;
            int top = ScreenHeight - 198;
            int bottom = top + 29;

            bool[,] potionTimerSlot = potionTimerSlot = ColorFilterPiece(PotionTimerBackground, left, right, top, bottom);
            double overloadTimerMatch = overloadTimerMatch = ImageProcessing.FractionalMatch(potionTimerSlot);
            return overloadTimerMatch > 0.5;
        }
    }
}
