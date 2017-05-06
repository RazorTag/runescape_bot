using RunescapeBot.BotPrograms.Popups;
using RunescapeBot.Common;
using RunescapeBot.ImageTools;
using RunescapeBot.UITools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace RunescapeBot.BotPrograms
{
    /// <summary>
    /// Smiths gold bracelets at the furnace in Port Phasmatys
    /// </summary>
    public class GoldBracelets : BotProgram
    {
        private const int CRAFTING_TIME = 50000;
        private const double STATIONARY_OBJECT_TOLERANCE = 15.0;
        private const int BANK_BOOTH_MIN_SIZE = 100;
        private const int WAIT_FOR_TIMEOUT = 5000;
        private ColorRange FurnaceIconOrange;
        private ColorRange BankIconDollar;
        private ColorRange Furnace;
        private ColorRange BankBooth;
        private Bank BankPopup;
        private FurnaceCrafting CraftPopup;
        private int failedRuns;

        public GoldBracelets(StartParams startParams) : base(startParams)
        {
            GetReferenceColors();
            RunParams.Run = true;
            failedRuns = 0;
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

            //ReadWindow();
            //bankPopup = new Bank(ScreenWidth, ScreenHeight);

            //ReadWindow();
            //MakeX makeX = new MakeX(0, 0, RSClient);
            //bool test = makeX.WaitForEnterAmount(60000);
        }

        protected override bool Execute()
        {
            //Move to the bank ad open it
            MoveToBank();
            ClickBankBooth();

            //Refresh inventory to a bracelet mould and 27 gold bars
            if (StopFlag) { return false; }
            BankPopup = new Bank(RSClient);
            if (!BankPopup.WaitForPopup(WAIT_FOR_TIMEOUT))
            {
                failedRuns++;
                return true;
            }
            BankPopup.DepositInventory();
            BankPopup.WithdrawOne(7, 0);
            BankPopup.WithdrawAll(6, 0);

            //Move to the furnace and use a gold bar on it
            if (StopFlag) { return false; }
            MoveToFurnace();
            Inventory.ClickInventory(0, 1);
            ClickStationaryObject(Furnace, STATIONARY_OBJECT_TOLERANCE, 100, 12000);

            //make 27 bars and wait
            if (StopFlag) { return false; }
            CraftPopup = new FurnaceCrafting(RSClient);
            if (!CraftPopup.WaitForPopup(WAIT_FOR_TIMEOUT))
            {   
                failedRuns++;
                return true;
            }
            CraftPopup.MakeBracelets(FurnaceCrafting.Jewel.None, 27);
            SafeWait(CRAFTING_TIME);
            //TODO verify that all gold bars have been crafted

            failedRuns = 0;
            return true;
        }

        /// <summary>
        /// Moves the character to the Port Phasmatys bank
        /// </summary>
        /// <returns>true if the bank icon is found</returns>
        private bool MoveToBank()
        {
            const int runTimeFromFurnaceToBank = 3000;  //approximate milliseconds needed to run from the furnace to the bank

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
            SafeWait(runTimeFromFurnaceToBank);

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
            Point guessLocation = new Point(Center.X + 100, Center.Y);
            const int scanInterval = 200; //time between checks in milliseconds
            const int maxWaitTime = 12000;
            Stopwatch watch = new Stopwatch();

            //Move the mouse to the neighborhood of where we expect the bank booth to be
            Mouse.MoveMouse(guessLocation.X + RNG.Next(-20, 26), guessLocation.Y + RNG.Next(-30, 41), RSClient);

            for (int i = 0; i < (maxWaitTime / ((double)scanInterval)); i++)
            {
                if (StopFlag) { return false; }
                watch.Restart();

                ReadWindow();
                if (LocateBankBooth(out bankBoothLocation))
                {
                    if (Geometry.DistanceBetweenPoints(bankBoothLocation, lastPosition) <= STATIONARY_OBJECT_TOLERANCE)
                    {
                        LeftClick(bankBoothLocation.Value.X, bankBoothLocation.Value.Y);
                        SafeWait(1000);
                        //TODO verify that the bank opened
                        return true;
                    }
                    else
                    {
                        lastPosition = bankBoothLocation;
                        watch.Stop();
                        SafeWait(scanInterval - ((int)watch.ElapsedMilliseconds));
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
            const double maxBoothHeightToWidthRatio = 3.2;
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

                if (blob.Size < BANK_BOOTH_MIN_SIZE)
                {
                    return false;   //We did not find the expected number of bank booths
                }

                if ((blob.Width / blob.Height) > maxBoothHeightToWidthRatio)
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
            const int runTimeFromBankToFurnace = 3000;  //appproximate milliseconds needed to run from the bank to the furnace

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
            SafeWait(runTimeFromBankToFurnace);

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
