using RunescapeBot.BotPrograms.Popups;
using RunescapeBot.UITools;
using System.Drawing;

namespace RunescapeBot.BotPrograms
{
    /// <summary>
    /// Smiths gold bracelets at the furnace in Port Phasmatys
    /// </summary>
    public class GoldBracelets : BankPhasmatys
    {
        private const int CRAFTING_TIME = 50000;
        private const int WAIT_FOR_BANK_WINDOW_TIMEOUT = 15000;
        private const int WAIT_FOR_CRAFTING_WINDOW_TIMEOUT = 15000;
        private const int WAIT_FOR_MAKEX_POPUP_TIMEOUT = 5000;
        private const int CONSECUTIVE_FAILURES_ALLOWED = 5;
        private int failedRuns;
        private Point guessBankLocation;

        public GoldBracelets(StartParams startParams) : base(startParams)
        {
            RunParams.Run = true;
            failedRuns = 0;
            ReadWindow();
            guessBankLocation = new Point(Center.X + 100, Center.Y);
        }

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

        protected override bool Execute()
        {
            if (failedRuns > CONSECUTIVE_FAILURES_ALLOWED)
            {
                return false;
            }

            //Move to the bank and open it
            if (!MoveToBank())
            {
                failedRuns++;
                return true;
            }
            Mouse.MoveMouse(guessBankLocation.X + RNG.Next(-20, 26), guessBankLocation.Y + RNG.Next(-30, 41), RSClient);    //Move the mouse to the neighborhood of where we expect the bank booth to be
            ClickBankBooth();

            //Refresh inventory to a bracelet mould and 27 gold bars
            if (StopFlag) { return false; }
            BankPopup = new Bank(RSClient);
            if (!BankPopup.WaitForPopup(WAIT_FOR_BANK_WINDOW_TIMEOUT))
            {
                failedRuns++;
                return true;
            }
            BankPopup.DepositInventory();
            BankPopup.WithdrawOne(7, 0);
            BankPopup.WithdrawAll(6, 0);

            //Move to the furnace and use a gold bar on it
            if (StopFlag) { return false; }
            if (!MoveToFurnace())
            {
                failedRuns++;
                return true;
            }
            Inventory.ClickInventory(0, 1);
            ClickStationaryObject(Furnace, STATIONARY_OBJECT_TOLERANCE, 100, 12000);

            //make 27 bars and wait
            if (StopFlag) { return false; }
            CraftPopup = new FurnaceCrafting(RSClient);
            if (!CraftPopup.WaitForPopup(WAIT_FOR_CRAFTING_WINDOW_TIMEOUT))
            {   
                failedRuns++;
                return true;
            }
            CraftPopup.MakeBracelets(FurnaceCrafting.Jewel.None, 27, WAIT_FOR_MAKEX_POPUP_TIMEOUT);
            SafeWait(CRAFTING_TIME);
            //TODO verify that all gold bars have been crafted

            failedRuns = 0;
            return true;
        }
    }
}
