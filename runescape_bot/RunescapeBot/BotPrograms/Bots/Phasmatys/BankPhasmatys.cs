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
        protected ColorRange BuildingFloor;
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
            LeftClick(clickLocation.X, clickLocation.Y + 5, 3);
            SafeWait(runTimeFromFurnaceToBank);

            return true;
        }

        /// <summary>
        /// Finds the bank icon on the minimap
        /// Adjusts for the icon's tendency to float around
        /// </summary>
        /// <returns>true if the icon is probably found correctly, false otherwise</returns>
        protected Point? BankIconLocation()
        {
            ReadWindow();
            Point offset;
            bool[,] minimapBankIcon = MinimapFilter(BankIconDollar, out offset);
            Blob bankBlob = ImageProcessing.BiggestBlob(minimapBankIcon);
            if (bankBlob == null || bankBlob.Size < 10) { return null; }

            int bankX, bankY;
            bool[,] minimapBankFloor = MinimapFilter(BuildingFloor, out offset);
            Blob bankFloor = ImageProcessing.ClosestBlob(minimapBankFloor, bankBlob.Center, 100);
            if (bankFloor == null)
            {
                bankX = bankBlob.Center.X;
                bankY = bankBlob.Center.Y;
            }
            else
            {
                if (bankFloor.Width > 50)
                {
                    bankX = ((bankFloor.Center.X + bankFloor.LeftBound) / 2) + 6;
                }
                else
                {
                    bankX = bankFloor.Center.X + 6;
                }
                bankY = bankFloor.Center.Y + 5;
            }

            int x = bankX + offset.X + RNG.Next(-1, 2);
            int y = bankY + offset.Y + RNG.Next(-1, 2);
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
            bool[,] minimapFurnace = MinimapFilter(FurnaceIconOrange, out offset);
            Blob furnaceBlob = ImageProcessing.BiggestBlob(minimapFurnace);
            if (furnaceBlob == null || furnaceBlob.Size < 3) { return null; }

            bool[,] minimapFurnaceFloor = MinimapFilter(BuildingFloor);
            List<Blob> floors = ImageProcessing.BlobsWithinRange(minimapFurnaceFloor, furnaceBlob.Center, 20, true);
            Blob furnaceFloor = Blob.Combine(floors);

            int furnaceX, furnaceY;
            if (furnaceFloor == null)
            {
                furnaceX = furnaceBlob.Center.X;
                furnaceY = furnaceBlob.Center.Y;
            }
            else
            {
                furnaceX = furnaceFloor.Center.X;
                furnaceY = furnaceFloor.Center.Y;
            }

            int x = furnaceX + offset.X + RNG.Next(-2, 3);
            int y = furnaceY + offset.Y + RNG.Next(-2, 3);
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
                        LeftClick(bankBoothLocation.Value.X, bankBoothLocation.Value.Y, 10);
                        SafeWait(1000, 120); //TODO verify that the bank opened
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
        /// Finds the closest bank booth in the Port Phasmatys bank
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
                bankBooth = Geometry.RandomMidpoint(rightBooth.Center, secondRightBooth.Center);
                int y = (int) Probability.BoundedGaussian(bankBooth.Value.Y, 3.0, bankBooth.Value.Y - 8.0, bankBooth.Value.Y + 8.0);  //randomize the height of the click
                bankBooth = new Point(bankBooth.Value.X, y);
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
            LeftClick(clickLocation.X, clickLocation.Y, 3);
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
            BuildingFloor = ColorFilters.PhasmatysBuildingFloor();
            Furnace = ColorFilters.Furnace();
            BankBooth = ColorFilters.BankBoothPhasmatys();
        }
    }
}
