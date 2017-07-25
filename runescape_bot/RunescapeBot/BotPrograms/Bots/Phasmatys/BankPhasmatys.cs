using RunescapeBot.BotPrograms.Popups;
using RunescapeBot.Common;
using RunescapeBot.ImageTools;
using System.Collections.Generic;
using System.Drawing;

namespace RunescapeBot.BotPrograms
{
    public class BankPhasmatys : BotProgram
    {
        protected const int FURNACE_TO_BANK_ICON_OFFSET_HORIZONTAL = 15;
        protected const int FURNACE_TO_BANK_ICON_OFFSET_VERICAL = 50;
        protected const double STATIONARY_OBJECT_TOLERANCE = 15.0;
        protected RGBHSBRange FurnaceIconOrange;
        protected RGBHSBRange BankIconDollar;
        protected RGBHSBRange BuildingFloor;
        protected RGBHSBRange Furnace;
        protected RGBHSBRange BankBooth;
        protected FurnaceCrafting CraftPopup;
        protected Bank BankPopup;
        protected int BankFloorMaxSize { get { return ArtifactSize(0.0015); } }
        protected int FurnaceFloorMaxSize { get { return ArtifactSize(0.00035); } }


        protected BankPhasmatys(RunParams startParams) : base(startParams)
        {
            GetReferenceColors();
        }

        /// <summary>
        /// Moves the character to the Port Phasmatys bank
        /// </summary>
        /// <returns>true if the bank icon is found</returns>
        protected override bool MoveToBank(int minRunTimeToBank = 6500, bool readWindow = false)
        {
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
            LeftClick(clickLocation.X, clickLocation.Y, 1);
            SafeWait(minRunTimeToBank);

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
            if (bankFloor == null || bankFloor.Size > BankFloorMaxSize)
            {
                bankX = bankBlob.Center.X;
                bankY = bankBlob.Center.Y + 4;
            }
            else
            {
                Geometry.AddMinimapIconToBlob(ref bankFloor, bankBlob.Center);
                if (bankFloor.Width > 40)
                {
                    bankX = ((bankFloor.Center.X + bankFloor.LeftBound) / 2) + 6;
                    bankY = bankFloor.Center.Y + 8;
                }
                else
                {
                    bankX = bankFloor.Center.X + 8;
                    bankY = bankFloor.Center.Y + 6;
                }
            }

            int x = bankX + offset.X;
            int y = bankY + offset.Y;
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
            if (furnaceFloor == null || furnaceFloor.Size > FurnaceFloorMaxSize)
            {
                furnaceX = furnaceBlob.Center.X;
                furnaceY = furnaceBlob.Center.Y;
            }
            else
            {
                Geometry.AddMinimapIconToBlob(ref furnaceFloor, furnaceBlob.Center);
                furnaceX = furnaceFloor.Center.X + 8;
                furnaceY = furnaceFloor.Center.Y;
            }

            int x = furnaceX + offset.X;
            int y = furnaceY + offset.Y;
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
            Blob bankBooth;
            const int maxWaitTime = 12000;
            if (!LocateStationaryObject(BankBooth, out bankBooth, 15, maxWaitTime, BankBoothMinSize, LocateBankBoothPhasmatys))
            {
                return false;
            }
            LeftClick(bankBooth.Center.X, bankBooth.Center.Y, 10);
            SafeWait(200, 120); //TODO verify that the bank opened
            return true;
        }

        /// <summary>
        /// Moves the character to the Port Phasmatys furnace
        /// </summary>
        /// <returns>true if the furnace icon is found</returns>
        protected bool MoveToFurnace()
        {
            const int runTimeFromBankToFurnace = 5500;  //appproximate milliseconds needed to run from the bank to the furnace
            const int selectFirstItemTime = 800;

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
            LeftClick(clickLocation.X, clickLocation.Y, 1);
            SafeWait(runTimeFromBankToFurnace - selectFirstItemTime);

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        private void GetReferenceColors()
        {
            FurnaceIconOrange = RGBHSBRanges.FurnaceIconOrange();
            BankIconDollar = RGBHSBRanges.BankIconDollar();
            BuildingFloor = RGBHSBRanges.PhasmatysBuildingFloor();
            Furnace = RGBHSBRanges.Furnace();
            BankBooth = RGBHSBRanges.BankBoothPhasmatys();
        }
    }
}
