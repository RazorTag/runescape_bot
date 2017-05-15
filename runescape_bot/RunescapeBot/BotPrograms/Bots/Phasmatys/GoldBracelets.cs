using RunescapeBot.BotPrograms.Popups;
using RunescapeBot.UITools;
using System.Drawing;

namespace RunescapeBot.BotPrograms
{
    /// <summary>
    /// Smiths gold bracelets at the furnace in Port Phasmatys
    /// </summary>
    public class GoldBracelets : FurnacePhasmatys
    {
        private const int CRAFTING_TIME = 50000;
        private const int WAIT_FOR_CRAFTING_WINDOW_TIMEOUT = 15000;
        private const int WAIT_FOR_MAKEX_POPUP_TIMEOUT = 5000;

        public GoldBracelets(StartParams startParams) : base(startParams) { }

        protected override void Run()
        {
            //Debug

            //ReadWindow();
            //bool[,] furnaceIcon = ColorFilter(FurnaceIconOrange);
            //DebugUtilities.TestMask(Bitmap, ColorArray, FurnaceIconOrange, furnaceIcon, "C:\\Projects\\Roboport\\test_pictures\\mask_tests\\", "furnaceIcon");

            //ReadWindow();
            //bool[,] bankIcon = ColorFilter(BankIconDollar);
            //DebugUtilities.TestMask(Bitmap, ColorArray, BankIconDollar, bankIcon, "C:\\Projects\\Roboport\\test_pictures\\mask_tests\\", "bankIcon");

            //ReadWindow();
            //bool[,] furnace = ColorFilter(Furnace);
            //DebugUtilities.TestMask(Bitmap, ColorArray, Furnace, furnace, "C:\\Projects\\Roboport\\test_pictures\\mask_tests\\", "furnace");

            //ReadWindow();
            //bool[,] bankBooth = ColorFilter(BankBooth);
            //DebugUtilities.TestMask(Bitmap, ColorArray, BankBooth, bankBooth, "C:\\Projects\\Roboport\\test_pictures\\mask_tests\\", "bankBooth");

            //ReadWindow();
            //bankPopup = new Bank(ScreenWidth, ScreenHeight);

            //ReadWindow();
            //MakeX makeX = new MakeX(0, 0, RSClient);
            //bool test = makeX.WaitForEnterAmount(60000);

            //ReadWindow();
            //FurnaceCrafting crafting = new FurnaceCrafting(RSClient);
            //crafting.MakeBracelets(FurnaceCrafting.Jewel.None, 27, 60000);
        }

        protected override bool FurnaceActions()
        {
            //make 27 bars and wait
            if (StopFlag) { return false; }
            CraftPopup = new FurnaceCrafting(RSClient);
            if (!CraftPopup.WaitForPopup(WAIT_FOR_CRAFTING_WINDOW_TIMEOUT))
            {
                return false;
            }
            CraftPopup.MakeBracelets(FurnaceCrafting.Jewel.None, 27, WAIT_FOR_MAKEX_POPUP_TIMEOUT);

            //TODO verify that gold bars are being crafted

            SafeWait(CRAFTING_TIME);

            return true;
        }
    }
}
