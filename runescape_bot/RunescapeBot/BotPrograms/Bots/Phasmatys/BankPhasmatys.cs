using RunescapeBot.BotPrograms.Popups;
using RunescapeBot.Common;
using RunescapeBot.ImageTools;
using RunescapeBot.UITools;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace RunescapeBot.BotPrograms
{
    public class BankPhasmatys : BotProgram
    {
        protected const int BANK_BOOTH_MIN_SIZE = 100;
        protected const int FURNACE_TO_BANK_ICON_OFFSET_HORIZONTAL = 15;
        protected const int FURNACE_TO_BANK_ICON_OFFSET_VERICAL = 50;
        protected const double STATIONARY_OBJECT_TOLERANCE = 15.0;
        protected ColorRange FurnaceIconOrange;
        protected ColorRange BankIconDollar;
        protected ColorRange Furnace;
        protected ColorRange BankBooth;
        protected FurnaceCrafting CraftPopup;
        protected Bank BankPopup;


        protected BankPhasmatys(StartParams startParams) : base(startParams)
        {
            GetReferenceColors();
        }

        /// <summary>
        /// Moves the character to the Port Phasmatys bank
        /// </summary>
        /// <returns>true if the bank icon is found</returns>
        protected bool MoveToBank()
        {
            const int runTimeFromFurnaceToBank = 3000;  //approximate minimum milliseconds needed to run from the furnace to the bank

            Point? bankIcon = BankIconLocation();
            Point clickLocation;

            if (bankIcon == null)
            {
                bankIcon = FurnaceIconLocation();
                if (bankIcon == null)
                {
                    return false;
                }
                else
                {
                    clickLocation = new Point(bankIcon.Value.X + FURNACE_TO_BANK_ICON_OFFSET_HORIZONTAL, bankIcon.Value.Y + FURNACE_TO_BANK_ICON_OFFSET_VERICAL);
                }
            }
            else
            {
                clickLocation = (Point)bankIcon;
            }
            LeftClick(clickLocation.X, clickLocation.Y + 5, 200, 3);
            SafeWait(runTimeFromFurnaceToBank);

            return true;
        }

        /// <summary>
        /// Finds the bank icon on the minimap
        /// </summary>
        /// <returns>true if the icon is probably found correctly, false otherwise</returns>
        protected Point? BankIconLocation()
        {
            ReadWindow();
            Point offset;
            bool[,] bankIcon = MinimapFilter(BankIconDollar, out offset);
            Blob bankBlob = ImageProcessing.BiggestBlob(bankIcon);
            if (bankBlob == null || bankBlob.Size < 10)
            {
                return null;
            }
            int x = bankBlob.Center.X + offset.X;
            int y = bankBlob.Center.Y + offset.Y;
            return new Point(x, y);
        }

        /// <summary>
        /// Finds the furnace icon on the minimap
        /// </summary>
        /// <returns>true if the icon is probably found correctly, false otherwise</returns>
        protected Point? FurnaceIconLocation()
        {
            ReadWindow();
            Point offset;
            bool[,] furnaceIcon = MinimapFilter(FurnaceIconOrange, out offset);
            Blob furnaceBlob = ImageProcessing.BiggestBlob(furnaceIcon);
            if (furnaceBlob == null || furnaceBlob.Size < 3)
            {
                return null;
            }
            int x = furnaceBlob.Center.X - 4 + offset.X;
            int y = furnaceBlob.Center.Y + offset.Y;
            return new Point(x, y);
        }

        /// <summary>
        /// Finds the Port Phasmatys bank booths and clicks on them to open the bank.
        /// Assumes that the player has a ghostspeak amulet equipped.
        /// Fails if the Port Phasmatys bank booths are not visible on the screen.
        /// </summary>
        /// <returns>True if the bank is opened</returns>
        protected bool ClickBankBooth()
        {
            Point? bankBoothLocation = null;
            Point? lastPosition = null;
            const int scanInterval = 20; //time between checks in milliseconds
            const int maxWaitTime = 12000;
            Stopwatch watch = new Stopwatch();

            for (int i = 0; i < (maxWaitTime / ((double)scanInterval)); i++)
            {
                if (StopFlag) { return false; }
                watch.Restart();

                ReadWindow();
                if (LocateBankBooth(out bankBoothLocation, true))
                {
                    if (Geometry.DistanceBetweenPoints(bankBoothLocation, lastPosition) <= STATIONARY_OBJECT_TOLERANCE)
                    {
                        LeftClick(bankBoothLocation.Value.X, bankBoothLocation.Value.Y, 200, 10);
                        SafeWait(1000); //TODO verify that the bank opened
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
        protected bool LocateBankBooth(out Point? bankBooth, bool randomize = false)
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
            double leftness = RNG.NextDouble();

            if(randomize)
            {
                bankBooth = Numerical.RandomMidpoint(rightBooth.Center, secondRightBooth.Center);
            }
            else
            {
                bankBooth = Numerical.Average(rightBooth.Center, secondRightBooth.Center);
            }
            
            return true;
        }

        /// <summary>
        /// Moves the character to the Port Phasmatys furnace
        /// </summary>
        /// <returns>true if the furnace icon is found</returns>
        protected bool MoveToFurnace()
        {
            const int runTimeFromBankToFurnace = 3000;  //appproximate milliseconds needed to run from the bank to the furnace

            Point? furnaceIcon = FurnaceIconLocation();
            Point clickLocation;

            if (furnaceIcon == null)
            {
                furnaceIcon = BankIconLocation();
                if (furnaceIcon == null)
                {
                    return false;
                }
                else
                {
                    clickLocation = new Point(furnaceIcon.Value.X - FURNACE_TO_BANK_ICON_OFFSET_HORIZONTAL, furnaceIcon.Value.Y - FURNACE_TO_BANK_ICON_OFFSET_VERICAL);
                }
            }
            else
            {
                clickLocation = (Point)furnaceIcon;
            }
            LeftClick(clickLocation.X, clickLocation.Y, 200, 3);
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
