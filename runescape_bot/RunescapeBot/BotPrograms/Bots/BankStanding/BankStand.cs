using RunescapeBot.BotPrograms.Popups;
using RunescapeBot.ImageTools;
using System;
using System.Collections.Generic;
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
    public class GenericBank : BotProgram
    {
        protected const int WAIT_FOR_BANK_WINDOW_TIMEOUT = 5000;

        protected ColorRange BankBoothCounter;
        protected int MakeTime;
        protected int FailedRuns;

        public GenericBank(RunParams startParams, int makeTime) : base(startParams)
        {
            this.MakeTime = makeTime;
            GetReferenceColors();
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
            int bankBoothMinSize = ArtifactSize(0.000156);
            int bankBoothMaxSize = ArtifactSize(0.000416);

            ReadWindow();
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
            BankBoothCounter = ColorFilters.BankBoothVarrockWest();
        }
    }
}
