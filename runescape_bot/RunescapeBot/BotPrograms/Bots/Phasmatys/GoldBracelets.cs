﻿using RunescapeBot.BotPrograms.Popups;
using RunescapeBot.Common;
using RunescapeBot.ImageTools;
using RunescapeBot.UITools;
using System.Diagnostics;
using System.Drawing;
using System.Threading;

namespace RunescapeBot.BotPrograms
{
    /// <summary>
    /// Smiths gold bracelets at the furnace in Port Phasmatys
    /// </summary>
    public class GoldBracelets : FurnacePhasmatys
    {
        public const int SINGLE_CRAFTING_TIME = 1800;
        private const int WAIT_FOR_CRAFTING_WINDOW_TIMEOUT = 10000;
        private const int WAIT_FOR_MAKEX_POPUP_TIMEOUT = 5000;

        public GoldBracelets(RunParams startParams) : base(startParams)
        {
            SingleMakeTime = SINGLE_CRAFTING_TIME;
            MakeQuantity = 27;
        }

        protected override bool Run()
        {
            //ReadWindow();
            //DebugUtilities.SaveImageToFile(Bitmap);

            //ReadWindow();
            //bool[,] furnaceIcon = ColorFilter(FurnaceIconOrange);
            //DebugUtilities.TestMask(Bitmap, ColorArray, FurnaceIconOrange, furnaceIcon, "C:\\Projects\\Roboport\\test_pictures\\mask_tests\\", "furnaceIcon");

            //ReadWindow();
            //MaskTest(BankIconDollar);

            //ReadWindow();
            //bool[,] furnace = ColorFilter(Furnace);
            //DebugUtilities.TestMask(Bitmap, ColorArray, Furnace, furnace, "C:\\Projects\\Roboport\\test_pictures\\mask_tests\\", "furnace");

            //MaskTest(RGBHSBRangeFactory.BankBoothPhasmatys());

            //ReadWindow();
            //bankPopup = new Bank(ScreenWidth, ScreenHeight);

            //ReadWindow();
            //MakeX makeX = new MakeX(0, 0, RSClient);
            //bool test = makeX.WaitForEnterAmount(60000);

            //ReadWindow();
            //FurnaceCrafting crafting = new FurnaceCrafting(RSClient);
            //crafting.MakeBracelets(FurnaceCrafting.Jewel.None, 27, 60000);

            //ReadWindow();
            //RGBHSBRange floor = RGBHSBRangeFactory.PhasmatysBuildingFloorLight();
            //bool[,] mask = ColorFilter(floor);
            //DebugUtilities.TestMask(Bitmap, ColorArray, floor, mask, "C:\\Projects\\Roboport\\test_pictures\\mask_tests\\", "floor");

            return true;
        }

        protected override bool FurnaceActions()
        {
            Screen.ReadWindow();

            //make 27 bars and wait
            if (StopFlag) { return false; }
            CraftPopup = new FurnaceCrafting(RSClient, Keyboard);
            if (!CraftPopup.WaitForPopup(WAIT_FOR_CRAFTING_WINDOW_TIMEOUT))
            {
                return false;
            }
            CraftPopup.MakeBracelets(FurnaceCrafting.Jewel.None, 27, WAIT_FOR_MAKEX_POPUP_TIMEOUT);
            SafeWait(200, 150);
            WatchNetflix(0);
            CountDownItems(true);
            SafeWaitPlus(0, 1000);
            return true;
        }
    }
}
