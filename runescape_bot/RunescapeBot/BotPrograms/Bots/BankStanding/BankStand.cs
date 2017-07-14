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
        protected const int WAIT_FOR_BANK_WINDOW_TIMEOUT = 5000;
        protected ColorRange BankBoothCounter;
        protected int FailedRuns;


        public BankStand(RunParams startParams) : base(startParams)
        {
            GetReferenceColors();
        }

        /// <summary>
        /// Open bank, withdraw items, close bank, do work with items
        /// </summary>
        /// <returns>true if successful</returns>
        protected override bool Execute()
        {
            Bank bank;
            if (!OpenBank(out bank) || !WithdrawItems(bank)) { return false; }
            bank.CloseBank();
            if (!ProcessInventory()) { return false; }
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

        /// <summary>
        /// Finds the Port Phasmatys bank booths and clicks on them to open the bank.
        /// Assumes that the player has a ghostspeak amulet equipped.
        /// Fails if the Port Phasmatys bank booths are not visible on the screen.
        /// </summary>
        /// <returns>True if the bank is opened</returns>
        protected bool OpenBank(out Bank bankPopup)
        {
            const int maxWaitTime = 8000;
            Blob bankBooth;
            if (!LocateStationaryObject(BankBoothCounter, out bankBooth, 15, maxWaitTime, 1, LocateBankBooth))
            {
                bankPopup = null;
                return false;
            }
            LeftClick(bankBooth.Center.X, bankBooth.Center.Y, 10);
            bankPopup = new Bank(RSClient);
            return bankPopup.WaitForPopup(WAIT_FOR_BANK_WINDOW_TIMEOUT);
        }

        /// <summary>
        /// Finds the closest bank booth in the Varrock west bank
        /// </summary>
        /// <param name="bankBoothColor">not used</param>
        /// <param name="bankBooth">returns the found bank booth blob</param>
        /// <param name="minimumSize">not used</param>
        /// <returns>true if a bank booth is found</returns>
        protected bool LocateBankBooth(ColorRange bankBoothColor, out Blob bankBooth, int minimumSize = 1)
        {
            bankBooth = null;
            int bankBoothMinSize = ArtifactSize(0.000156);
            int bankBoothMaxSize = ArtifactSize(0.000416);

            if (!ReadWindow()) { return false; }
            bool[,] bankBooths = ColorFilter(BankBoothCounter);
            List<Blob> boothBlobs = ImageProcessing.FindBlobs(bankBooths, false, bankBoothMinSize, bankBoothMaxSize);
            bankBooth = Blob.ClosestBlob(Center, boothBlobs);

            return bankBooth != null;
        }

        /// <summary>
        /// Gets color filters for multiple use
        /// </summary>
        private void GetReferenceColors()
        {
            BankBoothCounter = RGBHSBRanges.BankBoothVarrockWest();
        }
    }
}
