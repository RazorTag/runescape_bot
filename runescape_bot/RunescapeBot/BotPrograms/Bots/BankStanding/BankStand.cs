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
        protected const int WAIT_FOR_BANK_LOCATION = 8000;
        protected List<BankLocator> PossibleBankTypes;
        protected BankLocator BankBoothLocator;
        protected int FailedRuns;


        public BankStand(RunParams startParams) : base(startParams)
        {
            PossibleBankTypes = new List<BankLocator>();
            PossibleBankTypes.Add(LocateBankBoothVarrock);
            PossibleBankTypes.Add(LocateBankBoothPhasmatys);
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
        /// Locates and opens an unknown bank type
        /// Refer to member PossibleBankBooths for a list of possible bank types
        /// </summary>
        /// <returns>true if the bank is opened</returns>
        protected bool OpenBank(out Bank bankPopup)
        {
            bankPopup = null;

            if (OpenKnownBank(out bankPopup))
            {
                return true;
            }
            else
            {
                return IdentifyBank() && OpenKnownBank(out bankPopup);
            }
        }

        /// <summary>
        /// Locates and opens a nown bank type
        /// </summary>
        /// <param name="bankPopup"></param>
        /// <returns></returns>
        protected bool OpenKnownBank(out Bank bankPopup)
        {
            bankPopup = null;
            if (BankBoothLocator == null) { return false; }

            Blob bankBooth;
            if (BankBoothLocator(out bankBooth))
            {
                LeftClick(bankBooth.Center.X, bankBooth.Center.Y, 10);
                bankPopup = new Bank(RSClient, Inventory);
                return bankPopup.WaitForPopup(WAIT_FOR_BANK_WINDOW_TIMEOUT);
            }
            return false;
        }

        /// <summary>
        /// Determines if any of the possible bank types appear on screen.
        /// Sets BankBoothCounter to the first match found.
        /// </summary>
        /// <returns>true if any of them do</returns>
        protected bool IdentifyBank()
        {
            Blob booth;
            ReadWindow();
            foreach (BankLocator bankLocator in PossibleBankTypes)
            {
                if (bankLocator(out booth))
                {
                    BankBoothLocator = bankLocator;
                    return true;
                }
            }
            return false;
        }
    }
}
