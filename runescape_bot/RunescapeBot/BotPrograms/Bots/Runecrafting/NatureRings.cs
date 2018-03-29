using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms.Bots.Runecrafting
{
    public class NatureRings : BotProgram
    {
        protected bool ringSlotEmpty;
        protected bool pouchDamaged;

        public NatureRings(RunParams startParams) : base(startParams)
        {
            GetReferenceColors();
            RunParams.Run = true;
        }

        /// <summary>
        /// Sets the filter colors for landmarks
        /// </summary>
        private void GetReferenceColors()
        {

        }

        protected override bool Run()
        {
            //ReadWindow();
            //DebugUtilities.SaveImageToFile(Bitmap, "C:\\Projects\\Roboport\\training_pictures\\nature rings\\test.png");
            return true;
        }

        protected override bool Execute()
        {
            //Start from the castle wars lobby
            //Locate the bank icon on the minimap and click on it
            //While running, determine if the ring equipment slot if empty and set the ring flag if it is
            //While running, determine if any of the pouches are damaged and set the damaged pouch flag if so.
            //---Banking:
            //Locate the bank chest and click on it to open bank (use blue "Bank booth" text in upper-right to confirm before clicking
            //Deposit all nature runes
            //If the ring flag is set, then withdraw a ring of dueling
            //If the damaged pouch flag is set, then withdraw 1 cosmic, 1 astral, and 2 air runes
            //Withdraw all pure essence
            //Close bank
            //If the ring flag is set, then equip the withdrawn ring of dueling and turn off the ring flag
            //If the damaged pouch flag is set, then cast NPC Contact and talk to the dark mage to repair the pouches. Turn off the damaged pouch flag.
            //Fill small, medium, and (if #pouches >2) large pouches
            //Click on bank chest to open bank
            //Withdraw all pure essence
            //Close bank
            //Fill giant pouch if #pouches >3
            //Click on bank chest to open bank
            //Withdraw all pure essence
            //Close bank

            //Open equipped tab
            //Right click on quest cape and select option <Legends' Guild teleport>
            //Locate the fairy rings by looking for red mushrooms
            //Attempt to mouse over middle fairy ring using blue "Fairy ring" text to confirm
            //[reach goal] First run only: Configure the location to CKR (2 clicks on each mushroom). Click the travel option. Skip the next step.
            //Right-click and select the third option (assumes that the fairy ring system is already set to CKR).

            //Click near the edge of the minimap at around 0 degrees and wait for movement to stop
            //Click near the edge of the minimap at around 0 degrees and wait for movement to stop
            //Click near the edge of the minimap at around -5 degrees and wait for movement to stop
            //Click about 75% of the way to the edge of the minimap at around 40 degrees and wait for movement to stop
            //Click on large gray mass in middle of mystery ruins to enter the nature altar

            //Click on large gray mass to the north to craft runes and wait for runes to disappear in inventory
            //Empty small, medium, and (if #pouches >2) large pouches and craft runes again
            //If >3 pouches: Empty giant pouch and craft runes again
            //Switch to equipped tab
            //Right-click ring of dueling and select third option for castle wars


            //---Failsafes:
            //Use a glory teleport to bank in edgeville if we find ourselves without a ring of dueling
            //If inventory gets messed up, the deposit all at the bank and withdraw pouches


            return true;    //assumes success
        }
    }
}
