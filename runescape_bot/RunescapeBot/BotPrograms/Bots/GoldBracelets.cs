using RunescapeBot.Common;
using RunescapeBot.ImageTools;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace RunescapeBot.BotPrograms
{
    /// <summary>
    /// Smiths gold bracelets at the furnace in Port Phasmatys
    /// </summary>
    public class GoldBracelets : BotProgram
    {
        private const double stationaryObjectTolerance = 50.0;
        private const int bankBoothMinSize = 400;
        private ColorRange FurnaceIconOrange;
        private ColorRange BankIconDollar;
        private ColorRange Furnace;
        private ColorRange BankBooth;

        public GoldBracelets(StartParams startParams) : base(startParams)
        {
            GetReferenceColors();
        }

        protected override void Run()
        {
            //Debug
            //ReadWindow();
            //bool[,] furnaceIcon = ColorFilter(FurnaceIconOrange);
            //DebugUtilities.TestMask(Bitmap, ColorArray, FurnaceIconOrange, furnaceIcon, "C:\\Projects\\Roboport\\test_pictures\\mask_tests\\", "furnaceIcon");
            //ReadWindow();
            //bool[,] bankIcon = ColorFilter(BankIconDollar);
            //DebugUtilities.TestMask(Bitmap, ColorArray, BankIconDollar, bankIcon, "C:\\Projects\\Roboport\\test_pictures\\mask_tests\\", "bankIcon");
            //ReadWindow();
            //bool[,] furnace = ColorFilter(Furnace);
            //DebugUtilities.TestMask(Bitmap, ColorArray, Furnace, furnace, "C:\\Projects\\Roboport\\test_pictures\\mask_tests\\", "furnace");
            //ReadWindow();
            //bool[,] bankBooth = ColorFilter(BankBooth);
            //DebugUtilities.TestMask(Bitmap, ColorArray, BankBooth, bankBooth, "C:\\Projects\\Roboport\\test_pictures\\mask_tests\\", "bankBooth");
        }

        protected override bool Execute()
        {
            MoveToBank();
            if (StopFlag) { return false; }
            ClickBankBooth();
            if (StopFlag) { return false; }
            //deposit all inventory, withdraw 1 bracelet mould, withdraw 27 gold bars

            MoveToFurnace();
            if (StopFlag) { return false; }
            Inventory.ClickInventory(0, 1);
            ClickStationaryObject(Furnace, stationaryObjectTolerance, 1000);
            if (StopFlag) { return false; }
            //make 27 bars and wait

            return false;
        }

        /// <summary>
        /// Moves the character to the Port Phasmatys bank
        /// </summary>
        /// <returns>true if the bank icon is found</returns>
        private bool MoveToBank()
        {
            ReadWindow();
            Point offset;
            bool[,] bankIcon = MinimapFilter(BankIconDollar, out offset);
            Blob bankBlob = ImageProcessing.BiggestBlob(bankIcon);
            if (bankBlob == null || bankBlob.Size < 10)
            {
                //TODO guess the bank location based on the furnace location
                return false;
            }
            int x = bankBlob.Center.X + offset.X;
            int y = bankBlob.Center.Y + offset.Y;
            LeftClick(x, y);
            SafeWait(5000);

            return true;
        }

        /// <summary>
        /// Finds the Port Phasmatys bank booths and clicks on them to open the bank.
        /// Assumes that the player has a ghostspeak amulet equipped.
        /// Fails if the Port Phasmatys bank booths are not visible on the screen.
        /// </summary>
        /// <returns>True if the bank is opened</returns>
        private bool ClickBankBooth()
        {
            Point? bankBoothLocation = null;
            Point? lastPosition = null;

            for (int i = 0; i < 40; i++)
            {
                if (StopFlag) { return false; }

                ReadWindow();
                if (LocateBankBooth(out bankBoothLocation))
                {
                    if (Geometry.DistanceBetweenPoints(bankBoothLocation, lastPosition) <= stationaryObjectTolerance)
                    {
                        LeftClick(bankBoothLocation.Value.X, bankBoothLocation.Value.Y);
                        SafeWait(1000);
                        return true;
                    }
                    else
                    {
                        lastPosition = bankBoothLocation;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Finds the midpoint of the two east most bank booths in the Port Phasmatys bank
        /// </summary>
        /// <returns>True if the bank booths are found</returns>
        private bool LocateBankBooth(out Point? bankBooth)
        {
            bankBooth = null;
            const int numberOfBankBooths = 6;
            bool[,] bankBooths = ColorFilter(BankBooth);
            List<Blob> boothBlobs = ImageProcessing.FindBlobs(bankBooths, true);    //list of blobs from biggest to smallest
            Blob blob;
            int blobIndex = 0;

            //Remove blobs that aren't bank booths
            while (blobIndex < numberOfBankBooths)
            {
                if (blobIndex > boothBlobs.Count - 1)
                {
                    return false;   //We did not find the expected number of bank booths
                }

                blob = boothBlobs[blobIndex];

                if (blob.Size < bankBoothMinSize)
                {
                    return false;   //We did not find the expected number of bank booths
                }

                if (blob.Width > (4 * blob.Height))
                {
                    boothBlobs.RemoveAt(blobIndex); //This blob is too wide to be a bank booth counter.
                }
                else
                {
                    blobIndex++;
                }
            }

            //Reduce the blob list to the bank booths
            boothBlobs = boothBlobs.GetRange(0, numberOfBankBooths);
            boothBlobs.Sort(new BlobHorizontalComparer());

            Blob rightBooth = boothBlobs[numberOfBankBooths - 1];
            Blob secondRightBooth = boothBlobs[numberOfBankBooths - 2];

            bankBooth = Numerical.Average(rightBooth.Center, secondRightBooth.Center);
            return true;
        }

        /// <summary>
        /// Moves the character to the Port Phasmatys furnace
        /// </summary>
        /// <returns>true if the furnace icon is found</returns>
        private bool MoveToFurnace()
        {
            ReadWindow();
            Point offset;
            bool[,] furnaceIcon = MinimapFilter(FurnaceIconOrange, out offset);
            Blob furnaceBlob = ImageProcessing.BiggestBlob(furnaceIcon);
            if (furnaceBlob == null || furnaceBlob.Size < 3)
            {
                //TODO guess the furnace location based on the bank location
                return false;
            }
            int x = furnaceBlob.Center.X - 4 + offset.X;
            int y = furnaceBlob.Center.Y + offset.Y;
            LeftClick(x, y);
            SafeWait(4000);

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        private void GetReferenceColors()
        {
            FurnaceIconOrange = ColorFilters.FurnaceIconOrange();
            BankIconDollar = ColorFilters.BankIconDollar();
            Furnace = ColorFilters.Furnace();
            BankBooth = ColorFilters.BankBoothPhasmatys();
        }
    }
}
