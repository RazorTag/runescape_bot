using RunescapeBot.ImageTools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms
{
    public class Woodcutting : BotProgram
    {
        protected int TreeTextChars;
        protected int TreeTextWidth;
        protected List<Blob> Trees;


        public Woodcutting(RunParams startParams) : base(startParams)
        {
            RunParams.Run = true;
        }

        /// <summary>
        /// Called once when the bot starts running
        /// </summary>
        protected override bool Run()
        {
            Inventory.SetEmptySlots();
            return true;
        }

        /// <summary>
        /// Called once for each iteration of the bot
        /// </summary>
        /// <returns></returns>
        protected override bool Execute()
        {

            return true;
        }

        /// <summary>
        /// Find all of the trees on the screen and sorts them by proximity to the player
        /// </summary>
        /// <returns>true if any trees are located</returns>
        protected bool LocateTrees()
        {
            return false;
        }
    }
}
