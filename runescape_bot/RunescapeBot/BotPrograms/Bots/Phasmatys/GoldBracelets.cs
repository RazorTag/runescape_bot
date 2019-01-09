using RunescapeBot.BotPrograms.Popups;
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
