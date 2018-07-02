using RunescapeBot.Common;
using RunescapeBot.ImageTools;
using RunescapeBot.UITools;
using System.Diagnostics;

namespace RunescapeBot.BotPrograms
{
    /// <summary>
    /// Smiths cannonballs at the furnace in Port Phasmatys
    /// </summary>
    public class Cannonballs : FurnacePhasmatys
    {
        public const int SINGLE_SMITH_TIME = 6000;
        private const int CANNONBALL_COLOR_SUM = 13219;

        public Cannonballs(RunParams startParams) : base(startParams)
        {
            SingleMakeTime = SINGLE_SMITH_TIME;
            MakeQuantity = 27;
        }

        /// <summary>
        /// Make the inventory of cannonballs and wait for them to finish
        /// </summary>
        /// <returns>true if the actions succeed, false if they fail</returns>
        protected override bool FurnaceActions()
        {
            ChatBoxSingleOptionMakeAll(RSClient);
            SafeWait(300);
            WatchNetflix(-200);
            CountDownItems(true);
            SafeWaitPlus(0, 3500);
            return true;
        }
    }
}
