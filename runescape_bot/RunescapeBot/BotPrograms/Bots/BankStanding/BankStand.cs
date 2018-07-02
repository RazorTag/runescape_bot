using RunescapeBot.BotPrograms.Popups;
using RunescapeBot.ImageTools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms
{
    /// <summary>
    /// Targeted at the Varrock west bank
    /// This probably works with many other banks but has only been tested for the Varrock west bank
    /// This would theoreticaly work at banks where the color of a bank booth's counter matches the color of theVarrock west bank booths' counters
    /// </summary>
    public class BankStand : BotProgram
    {
        protected int FailedRuns;


        public BankStand(RunParams startParams) : base(startParams)
        {

        }

        protected override bool Run()
        {
            //ReadWindow();
            //DebugUtilities.SaveImageToFile(Bitmap);

            //ReadWindow();
            //bool[,] bankCounter = ColorFilter(RGBHSBRangeFactory.BankBoothVarrockWest());
            //DebugUtilities.TestMask(Bitmap, ColorArray, RGBHSBRangeFactory.BankBoothVarrockWest(), bankCounter, "C:\\Projects\\Roboport\\test_pictures\\mask_tests\\", "bankCounter");

            return true;
        }

        /// <summary>
        /// Open bank, withdraw items, close bank, do work with items
        /// </summary>
        /// <returns>true if successful</returns>
        protected override bool Execute()
        {
            Bank bank;
            if (!OpenBank(out bank) || !WithdrawItems(bank))
            {
                return false;
            }
            bank.Close();
            if (StopFlag || !ProcessInventory())
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Used to withdraw items from the bank
        /// </summary>
        /// <returns>true if successful</returns>
        protected virtual bool WithdrawItems(Bank bank)
        {
            return true;
        }

        /// <summary>
        /// Used to do work on the items in the inventory after being withdrawn from the bank
        /// </summary>
        /// <returns></returns>
        protected virtual bool ProcessInventory()
        {
            return true;
        }
    }
}
