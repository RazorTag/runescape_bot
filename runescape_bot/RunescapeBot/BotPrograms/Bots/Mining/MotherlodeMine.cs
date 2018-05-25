using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms
{
    public class MotherlodeMine : BotProgram
    {
        public MotherlodeMine(RunParams startParams) : base(startParams)
        {
            RunParams.Run = true;
        }

        protected override bool Run()
        {
            //ReadWindow();
            //DebugUtilities.SaveImageToFile(Bitmap, "C:\\Projects\\Roboport\\test_pictures\\motherlode_mine\\clean_rocks2.png");

            return false;
        }

        /// <summary>
        /// Assumes that we are starting next to the bank chest.
        /// </summary>
        /// <returns>false if the bot cannot continue</returns>
        protected override bool Execute()
        {
            if (MoveToRocks()
                && MineRocks()
                && MoveToHopper()
                && CleanRocks()
                && BankRocks())
            {
                return true;
            }

            return Recover();
        }

        /// <summary>
        /// Moves the general vicinity of rocks to mine.
        /// Assumes that we are starting near the bank chest
        /// </summary>
        /// <returns>true if successful</returns>
        protected bool MoveToRocks()
        {
            //TODO
            return true;
        }

        /// <summary>
        /// Mines rocks until the inventory is full.
        /// Switches worlds if the rocks are depleted.
        /// Assumes that we are starting next to the rocks just west of the deposit stream
        /// </summary>
        /// <returns>true if successful</returns>
        protected bool MineRocks()
        {
            //TODO
            return true;
        }

        /// <summary>
        /// Switches to another world. Use when rocks are depleted.
        /// </summary>
        /// <returns>true if successful</returns>
        protected bool SwitchWorlds()
        {
            //TODO
            return true;
        }

        /// <summary>
        /// Moves to the hopper where rocks are cleaned in the stream.
        /// </summary>
        /// <returns>true if successful</returns>
        protected bool MoveToHopper()
        {
            //TODO
            return true;
        }

        /// <summary>
        /// Puts the rocks into the hopper and collects them from the sack.
        /// Assumes that we are starting next to the hopper.
        /// </summary>
        /// <returns></returns>
        protected bool CleanRocks()
        {
            //TODO
            return true;
        }

        /// <summary>
        /// Moves to the bank chest and deposits everything in inventory.
        /// Assumes that the bank icon is within sight on the minimap.
        /// </summary>
        /// <returns>true if successful</returns>
        protected bool BankRocks()
        {
            //TODO
            return true;
        }

        /// <summary>
        /// Attempts to recover after the bot finds itself in a fail state
        /// </summary>
        /// <returns>true if successful</returns>
        protected bool Recover()
        {
            //TODO
            return false;
        }
    }
}
