using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms.Bots.Thieving
{
    public class SummerGarden : BotProgram
    {
        protected Point Pestle; //location of the pestle and mortar in the inventory
        protected Point SecondSquirk; //location of the second squirk in the inventory to check to see if we are ready to make another squirk juice

        public SummerGarden(RunParams startParams) : base(startParams)
        {
            Pestle = new Point(0, 0);
            SecondSquirk = new Point(Inventory.INVENTORY_COLUMNS - 1, Inventory.INVENTORY_ROWS - 1);
        }

        protected override bool Run()
        {
            return true;
        }

        protected override bool Execute()
        {
            //Start from the fountain on each run.
            //Look for the red of a door on the minimap to identify garden doors. Use the northernmost door as the summer door.
            //Click on the summer door on the minimap to move next to it.
            //Hover over locations just above the player until we find blue hover text for the door. Left click to enter the garden.
            //TODO
            //After picking a fruit and being teleported out, check to see if the last two inventory spots (fruit spots) are occupied. If so, then use the pestle and mortar on the last inventory slot to make a squirk juice.

            //FAILSAFES
            //After each movement, check to see if the player is south of any of the red doors on the minimap. This would indicate expulsion from the garden.

            return true;
        }
    }
}
