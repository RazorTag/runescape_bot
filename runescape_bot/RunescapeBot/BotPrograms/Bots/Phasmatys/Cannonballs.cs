using RunescapeBot.ImageTools;
using System.Diagnostics;

namespace RunescapeBot.BotPrograms
{
    /// <summary>
    /// Smiths cannonballs at the furnace in Port Phasmatys
    /// </summary>
    public class Cannonballs : FurnacePhasmatys
    {
        private const int SINGLE_SMITH_TIME = 6000;
        private const int TOTAL_SMITH_TIME = 162000;
        private const int CANNONBALL_COLOR_SUM = 13219;
        Stopwatch watch;

        public Cannonballs(RunParams startParams) : base(startParams)
        {
            watch = new Stopwatch();
        }

        /// <summary>
        /// Make the inventory of cannonballs and wait for them to finish
        /// </summary>
        /// <returns>true if the actions succeed, false if they fail</returns>
        protected override bool FurnaceActions()
        {
            BotUtilities.ChatBoxSingleOptionMakeAll(RSClient);

            //verify that cannonballs are being smithed
            watch.Restart();
            SafeWait(3 * SINGLE_SMITH_TIME);
            watch.Stop();
            if (!Inventory.SlotIsEmpty(2, 0, true))   //steel bars aren't being processed
            {
                return false;
            }

            SafeWaitPlus(TOTAL_SMITH_TIME - (int)watch.ElapsedMilliseconds, 3500);
            RunParams.Iterations -= 27;
            return true;
        }
    }
}
