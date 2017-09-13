using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms
{
    public class CamelotHouse : BotProgram
    {
        public CamelotHouse(RunParams startParams) : base(startParams)
        {
            RunParams.Run = true;
            RunParams.LoginWorld = 337;
        }

        protected override bool Run()
        {
            ReadWindow();
            DebugUtilities.SaveImageToFile(Bitmap);

            //Inventory.OpenSpellbook(false);
            //Inventory.OpenOptions(false);

            //Inventory.TeleportHasRunes(5, 3);

            return true;
        }

        protected override bool Execute()
        {
            //TODO find bank chest
            //TODO click noted oak logs and hover over bank chest
            //TODO verify that the blue RGBHSBRangeFactory.MouseoverTextStationaryObject text shows up
            //TODO click bank chest to un-note logs
            if (!Inventory.TeleportHasRunes(Inventory.StandardTeleports.House))
            {
                return false;
            }
            Inventory.StandardTeleport(Inventory.StandardTeleports.House);
            Construct();
            Inventory.StandardTeleport(Inventory.StandardTeleports.Camelot);
            return true;
        }

        /// <summary>
        /// Do activities in a player owned house before returning to Camelot
        /// </summary>
        /// <returns>true if successful</returns>
        protected virtual bool Construct()
        {
            return true;
        }

        /// <summary>
        /// Selects call servant from the house options
        /// </summary>
        protected void CallServant()
        {
            Inventory.OpenOptions(false);
            //TODO click on house options
            //TODO click on call servant
        }
    }
}
