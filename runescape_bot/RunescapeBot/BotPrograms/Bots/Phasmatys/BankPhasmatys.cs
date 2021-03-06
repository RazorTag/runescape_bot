﻿using RunescapeBot.BotPrograms.Popups;
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
        protected const int BankFloorMinSize = 500;
        protected const int BankFloorMaxSize = 2500;
        protected const int FurnaceFloorMinSize = 100;
        protected const int FurnaceFloorMaxSize = 1000;
        protected int BankIconMinSize;


        protected BankPhasmatys(RunParams startParams) : base(startParams)
        {
            GetReferenceColors();
            BankIconMinSize = 4;   //ex. 19
        }

        /// <summary>
        /// Moves the character to the Port Phasmatys bank
        /// </summary>
        /// <returns>true if the bank icon is found</returns>
        protected bool MoveToBankPhasmatys(int minRunTimeToBank = 6500, bool readWindow = true)
        {
            Point? bankIcon = BankClickLocation();
            Point clickLocation;

            if (bankIcon == null)
            {
                bankIcon = FurnaceClickLocation();
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
        protected Point? BankClickLocation()
        {
            Screen.ReadWindow();
            Point offset;
            Blob bankIcon, bankFloor;
            int bankX, bankY;

            if (!BankLocation(out bankIcon, out bankFloor, out offset, BuildingFloor))
            {
                return null;
            }

            if ((bankFloor == null) || !BankFloorSizeCheck(bankFloor.Size))
            {
                bankX = bankIcon.Center.X;
                bankY = bankIcon.Center.Y + 4;
                ScanForBuildingFloor();
            }
            else
            {
                Geometry.AddMinimapIconToBlob(ref bankFloor, bankIcon.Center);
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
        /// Determines if the bank floor blob from the minimap is apropriately size
        /// </summary>
        /// <param name="bankFloorSize">number of pixels in the bank floor blob</param>
        /// <returns>true if bank floor blob is within size bounds</returns>
        protected bool BankFloorSizeCheck(int bankFloorSize)
        {
            return Numerical.WithinBounds(bankFloorSize, BankFloorMinSize, BankFloorMaxSize);
        }

        /// <summary>
        /// Locates the bank icon and bank floor on the minimap.
        /// Uses the most recent screenshot.
        /// </summary>
        /// <param name="bankIcon"></param>
        /// <param name="bankFloor"></param>
        /// <param name="offset"></param>
        /// <returns>true if at least the bank icon is found</returns>
        protected bool BankLocation(out Blob bankIcon, out Blob bankFloor, out Point offset, RGBHSBRange floorColor)
        {
            floorColor = floorColor ?? BuildingFloor;
            bool[,] minimapBankIcon = Minimap.MinimapFilter(BankIconDollar, out offset);
            bankIcon = ImageProcessing.BiggestBlob(minimapBankIcon);
            if (bankIcon == null || bankIcon.Size < BankIconMinSize)
            {
                bankFloor = null;
                return false;
            }

            bool[,] minimapBankFloor = Minimap.MinimapFilter(floorColor);
            bankFloor = ImageProcessing.ClosestBlob(minimapBankFloor, bankIcon.Center, 100);
            return true;
        }

        /// <summary>
        /// Finds the furnace icon on the minimap
        /// </summary>
        /// <returns>true if the icon is probably found correctly, false otherwise</returns>
        protected Point? FurnaceClickLocation()
        {
            Screen.ReadWindow();
            Point offset;
            Blob furnaceIcon, furnaceFloor;
            if (!FurnaceLocation(out furnaceIcon, out furnaceFloor, out offset, BuildingFloor))
            {
                return null;
            }

            int furnaceX, furnaceY;
            if ((furnaceFloor == null) || !FurnaceFloorSizeCheck(furnaceFloor.Size))
            {
                furnaceX = furnaceIcon.Center.X;
                furnaceY = furnaceIcon.Center.Y;
                ScanForBuildingFloor();
            }
            else
            {
                Geometry.AddMinimapIconToBlob(ref furnaceFloor, furnaceIcon.Center);
                furnaceX = furnaceFloor.Center.X + 8;
                furnaceY = furnaceFloor.Center.Y + 1;
            }

            int x = furnaceX + offset.X;
            int y = furnaceY + offset.Y;
            return new Point(x, y);
        }

        /// <summary>
        /// Determines if the furnace floor blob from the minimap is appropriately size
        /// </summary>
        /// <param name="furnaceFloorSize">number of pixels in the furnace floor blob</param>
        /// <returns>true if size is within bounds</returns>
        protected bool FurnaceFloorSizeCheck(int furnaceFloorSize)
        {
            return Numerical.WithinBounds(furnaceFloorSize, FurnaceFloorMinSize, FurnaceFloorMaxSize);
        }

        /// <summary>
        /// Locates the funace icon and floor on the minimap.
        /// Uses the most recent screenshot.
        /// </summary>
        /// <param name="furnaceIcon"></param>
        /// <param name="furnaceFloor"></param>
        /// <param name="offset"></param>
        /// <returns>true if at least the furnace icon is found</returns>
        protected bool FurnaceLocation(out Blob furnaceIcon, out Blob furnaceFloor, out Point offset, ColorFilter floorColor)
        {
            floorColor = floorColor ?? BuildingFloor;
            bool[,] minimapFurnace = Minimap.MinimapFilter(FurnaceIconOrange, out offset);
            furnaceIcon = ImageProcessing.BiggestBlob(minimapFurnace);
            if (furnaceIcon == null || furnaceIcon.Size < 3)
            {
                furnaceFloor = null;
                return false;
            }

            bool[,] minimapFurnaceFloor = Minimap.MinimapFilter(floorColor);
            List<Blob> floors = ImageProcessing.BlobsWithinRange(minimapFurnaceFloor, furnaceIcon.Center, 20, true);
            furnaceFloor = Blob.Combine(floors);
            return true;
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
            if (!Vision.LocateStationaryObject(BankBooth, out bankBooth, 15, maxWaitTime, Banking.MinBankBoothSize, int.MaxValue, Banking.LocateBankBoothPhasmatys))
            {
                return false;
            }
            
            LeftClick(bankBooth.Center.X, bankBooth.Center.Y, 10);
            SafeWait(200, 120);
            Bank bankPopup = new Bank(RSClient, Inventory, Keyboard);
            bool bankOpened = bankPopup.WaitForPopup();
            return bankOpened;
        }

        /// <summary>
        /// Moves the character to the Port Phasmatys furnace
        /// </summary>
        /// <returns>true if the furnace icon is found</returns>
        protected bool MoveToFurnace(int runTimeFromBankToFurnace = 6200)
        {
            const int selectFirstItemTime = 800;

            Point? furnaceIcon = FurnaceClickLocation();
            Point clickLocation;

            if (furnaceIcon == null)
            {
                furnaceIcon = BankClickLocation();
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
        /// Loads default color ranges
        /// </summary>
        private void GetReferenceColors()
        {
            FurnaceIconOrange = RGBHSBRangeFactory.FurnaceIconOrange();
            BankIconDollar = RGBHSBRangeFactory.BankIconDollar();
            BuildingFloor = RGBHSBRangeFactory.PhasmatysBuildingFloorLight();
            Furnace = RGBHSBRangeFactory.Furnace();
            BankBooth = RGBHSBRangeFactory.BankBoothPhasmatys();
        }

        /// <summary>
        /// Determines an appropriate value to use for building floor color range
        /// </summary>
        protected bool ScanForBuildingFloor()
        {
            List<RGBHSBRange> colorRanges = new List<RGBHSBRange>() { RGBHSBRangeFactory.PhasmatysBuildingFloorDark(), RGBHSBRangeFactory.PhasmatysBuildingFloorLight() };
            Blob bankIcon, bankFloor, furnaceIcon, furnaceFloor;
            Point offset;
            foreach (RGBHSBRange colorRange in colorRanges)
            {
                if (BankLocation(out bankIcon, out bankFloor, out offset, colorRange) && (bankFloor != null) && BankFloorSizeCheck(bankFloor.Size)
                    && FurnaceLocation(out furnaceIcon, out furnaceFloor, out offset, colorRange) && (furnaceFloor != null) && FurnaceFloorSizeCheck(furnaceFloor.Size))
                {
                    BuildingFloor = colorRange;
                    return true;
                }
            }
            return false;
        }
    }
}
