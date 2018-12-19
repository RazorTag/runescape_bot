using RunescapeBot.BotPrograms.Popups;
using RunescapeBot.Common;
using RunescapeBot.ImageTools;
using RunescapeBot.UITools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms
{
    public class Banking
    {
        #region properties

        protected const int WAIT_FOR_BANK_WINDOW_TIMEOUT = 2500;
        protected const int WAIT_FOR_BANK_LOCATION = 8000;
        protected List<BankLocator> PossibleBankTypes;
        protected BankLocator BankBoothLocator;
        protected GameScreen Screen;
        protected Vision Vision;
        protected HandEye HandEye;
        protected RSClient RSClient;
        protected Keyboard Keyboard;
        protected Inventory Inventory;
        protected MinimapGauge Minimap;

        /// <summary>
        /// Delegate for custom bank locators
        /// </summary>
        /// <param name="bankBooth"></param>
        /// <returns></returns>
        protected delegate bool BankLocator(out Blob bankBooth);

        internal int MinBankBoothSize { get { return Screen.ArtifactArea(0.000292); } } //ex 0.000583
        internal int MaxBankBoothSize { get { return Screen.ArtifactArea(0.001145); } } //ex 0.001045

        #endregion

        #region constructors

        internal Banking(GameScreen screen, Vision vision, HandEye handEye, RSClient client, Keyboard keyboard, Inventory inventory, MinimapGauge minimap)
        {
            Screen = screen;
            Vision = vision;
            HandEye = handEye;
            RSClient = client;
            Keyboard = keyboard;
            Inventory = inventory;
            Minimap = minimap;

            PossibleBankTypes = new List<BankLocator>();
            PossibleBankTypes.Add(LocateBankBoothVarrock);
            PossibleBankTypes.Add(LocateBankBoothSeersVillage);
            PossibleBankTypes.Add(LocateBankBoothPhasmatys);
            PossibleBankTypes.Add(LocateBankBoothEdgeville);
            Bank.ResetNAmount();
        }

        #endregion

        #region methods

        /// <summary>
        /// Locates and opens an unknown bank type
        /// Refer to member PossibleBankBooths for a list of possible bank types
        /// </summary>
        /// <returns>true if the bank is opened</returns>
        internal bool OpenBank(out Bank bankPopup, int allowedAttempts = 1)
        {
            bankPopup = null;

            if (OpenKnownBank(out bankPopup, allowedAttempts))
            {
                return true;
            }
            return IdentifyBank() && OpenKnownBank(out bankPopup, 1);
        }

        /// <summary>
        /// Locates and opens a nown bank type
        /// </summary>
        /// <param name="bankPopup"></param>
        /// <returns></returns>
        internal bool OpenKnownBank(out Bank bankPopup, int allowedAttempts = 1)
        {
            bankPopup = null;
            if (BankBoothLocator == null) { return false; }

            Blob bankBooth;
            for (int i = 0; i < allowedAttempts; i++)
            {
                if (BankBoothLocator(out bankBooth) && HandEye.MouseOverStationaryObject(bankBooth, true, 10))
                {
                    bankPopup = new Bank(RSClient, Inventory, Keyboard);
                    if (bankPopup.WaitForPopup(WAIT_FOR_BANK_WINDOW_TIMEOUT))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Determines if any of the possible bank types appear on screen.
        /// Sets BankBoothCounter to the first match found.
        /// </summary>
        /// <returns>true if any of them do</returns>
        internal bool IdentifyBank()
        {
            Blob booth;
            Screen.ReadWindow();
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

        /// <summary>
        /// Finds the closest bank booth counter that matches a given color
        /// </summary>
        /// <param name="bankBoothColor">not used</param>
        /// <param name="bankBooth">returns the found bank booth blob</param>
        /// <returns>true if a bank booth is found</returns>
        internal bool LocateBankBooth(ColorFilter bankBoothColor, out Blob bankBooth)
        {
            bankBooth = null;

            if (!Screen.ReadWindow()) { return false; }
            bool[,] bankBooths = Vision.ColorFilter(bankBoothColor);
            List<Blob> potentialBoothBlobs = ImageProcessing.FindBlobs(bankBooths, false, MinBankBoothSize, MaxBankBoothSize);
            List<Blob> booths = new List<Blob>();

            foreach (Blob potentialBooth in potentialBoothBlobs)
            {
                if (Geometry.Rectangularity(potentialBooth) > 0.8)
                {
                    booths.Add(potentialBooth);
                }
            }

            bankBooth = Blob.ClosestBlob(Screen.Center, booths);

            return bankBooth != null;
        }

        /// <summary>
        /// Locates a bank booth with the counter color from the Varrock west bank
        /// </summary>
        /// <param name="bankBooth"></param>
        /// <returns>true if a bank booth is found</returns>
        internal bool LocateBankBoothVarrock(out Blob bankBooth)
        {
            return LocateBankBooth(RGBHSBRangeFactory.BankBoothVarrockWest(), out bankBooth);
        }

        /// <summary>
        /// Locates a bank booth in the Seers' Village bank
        /// </summary>
        /// <param name="bankBooth"></param>
        /// <returns>true if a bank booth is found</returns>
        internal bool LocateBankBoothSeersVillage(out Blob bankBooth)
        {
            bankBooth = null;
            List<Blob> possibleBankBooths = Vision.LocateObjects(RGBHSBRangeFactory.BankBoothSeersVillage(), MinBankBoothSize, MaxBankBoothSize);
            if (possibleBankBooths == null) { return false; }

            List<Blob> bankBooths = new List<Blob>();
            foreach (Blob booth in possibleBankBooths)
            {
                double widthToHeight = booth.Width / (double)booth.Height;
                if (Numerical.WithinBounds(widthToHeight, 2.3, 3.2))
                {
                    bankBooths.Add(booth);
                }
            }

            if (bankBooths.Count != 9)
            {
                return false;
            }
            bankBooths.Sort(new BlobHorizontalComparer());
            bankBooths.RemoveAt(5); //remove closed bank booths
            bankBooths.RemoveAt(4);
            bankBooths.RemoveAt(2);
            bankBooth = Blob.ClosestBlob(Screen.Center, bankBooths);
            return true;
        }

        /// <summary>
        /// Finds the closest bank booth in the Port Phasmatys bank
        /// </summary>
        /// <returns>True if the bank booths are found</returns>
        internal bool LocateBankBoothPhasmatys(out Blob bankBooth)
        {
            bankBooth = null;
            const int numberOfBankBooths = 6;
            const double minBoothWidthToHeightRatio = 2.3667;   //ex 2.6667
            const double maxBoothWidthToHeightRatio = 3.1333;   //ex 2.8333
            int left = Screen.Center.X - Screen.ArtifactLength(0.5);
            int right = Screen.Center.X + Screen.ArtifactLength(0.3);
            int top = Screen.Center.Y - Screen.ArtifactLength(0.15);
            int bottom = Screen.Center.Y + Screen.ArtifactLength(0.2);

            Screen.ReadWindow();
            Point offset;
            bool[,] bankBooths = Vision.ColorFilterPiece(RGBHSBRangeFactory.BankBoothPhasmatys(), left, right, top, bottom, out offset);
            List<Blob> boothBlobs = new List<Blob>();
            List<Blob> possibleBoothBlobs = ImageProcessing.FindBlobs(bankBooths, false, MinBankBoothSize, MaxBankBoothSize);  //list of blobs from biggest to smallest
            possibleBoothBlobs.Sort(new BlobProximityComparer(Screen.Center));
            double widthToHeightRatio, rectangularity;

            //Remove blobs that aren't bank booths
            foreach (Blob possibleBooth in possibleBoothBlobs)
            {
                widthToHeightRatio = (possibleBooth.Width / (double)possibleBooth.Height);
                rectangularity = Geometry.Rectangularity(possibleBooth);
                if (Numerical.WithinBounds(widthToHeightRatio, minBoothWidthToHeightRatio, maxBoothWidthToHeightRatio) && rectangularity > 0.75)
                {
                    boothBlobs.Add(possibleBooth);
                }
            }

            if (boothBlobs.Count != numberOfBankBooths)
            {
                return false;   //We either failed to locate all of the booths or identified something that was not actually a booth.
            }

            //Reduce the blob list to the bank booths
            boothBlobs.Sort(new BlobHorizontalComparer());  //sort from left to right
            boothBlobs.RemoveAt(3); //remove the unusable booths without tellers
            boothBlobs.RemoveAt(0);
            bankBooth = Blob.ClosestBlob(new Point(Screen.Center.X - left, Screen.Center.Y - top), boothBlobs);
            bankBooth.ShiftPixels(offset.X, offset.Y);
            return true;
        }

        /// <summary>
        /// Finds the closest bank booth in the Port Phasmatys bank
        /// </summary>
        /// <returns>True if a bank booths is found</returns>
        internal bool LocateBankBoothPhasmatys(ColorFilter bankBoothColor, out Blob bankBooth, int minimumSize = 1, int maximumSize = int.MaxValue)
        {
            return LocateBankBoothPhasmatys(out bankBooth);
        }

        /// <summary>
        /// Finds the closest bank booth in the Edgeville bank out of the northern two booths
        /// </summary>
        /// <param name="bankBooth">returns a blob for a found bank booth</param>
        /// <returns>True if a bank booth is found</returns>
        internal bool LocateBankBoothEdgeville(out Blob bankBooth)
        {
            int searchRadius = Screen.ArtifactLength(0.1475);  //ex 150 pixels on a 1080p screen
            int left = Screen.Center.X - searchRadius;
            int right = Screen.Center.X + searchRadius;
            int top = Screen.Center.Y - searchRadius;
            int bottom = Screen.Center.Y + searchRadius;
            List<Blob> bankBooths = Vision.LocateObjects(RGBHSBRangeFactory.BankBoothEdgeville(), left, right, top, bottom, true, Screen.ArtifactArea(0.0000754), Screen.ArtifactArea(0.0004));   //ex 0.000151 - 0.000211

            if (bankBooths.Count == 0)
            {
                bankBooth = null;
                return false;
            }
            bankBooths.Sort(new BlobProximityComparer(Screen.Center));
            bankBooth = bankBooths[0];
            if (bankBooth.Width > bankBooth.Height)
            {
                bankBooth.ShiftPixels(0, 24);
            }
            else
            {
                bankBooth.ShiftPixels(15, 0);
            }

            return true;
        }

        /// <summary>
        /// Moves the character to a bank icon on the minimap
        /// </summary>
        /// <returns>true if the bank icon is found</returns>
        internal virtual bool MoveToBank(int maxRunTimeToBank = 10000, bool readWindow = true, int minBankIconSize = 4, int randomization = 3, Point? moveTarget = null)
        {
            if (readWindow) { Screen.ReadWindow(); }
            if (moveTarget == null) { moveTarget = new Point(0, 0); }

            Point offset;
            bool[,] minimapBankIcon = Minimap.MinimapFilter(RGBHSBRangeFactory.BankIconDollar(), out offset);
            Blob bankBlob = ImageProcessing.BiggestBlob(minimapBankIcon);
            if (bankBlob == null || bankBlob.Size < minBankIconSize) { return false; }

            Point clickLocation = new Point(offset.X + bankBlob.Center.X + moveTarget.Value.X, offset.Y + bankBlob.Center.Y + moveTarget.Value.Y);
            Mouse.LeftClick(clickLocation.X, clickLocation.Y, randomization);
            BotProgram.SafeWait(maxRunTimeToBank);

            return true;
        }

        #endregion
    }
}
