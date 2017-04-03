﻿using RunescapeBot.ImageTools;
using System.Drawing;

namespace RunescapeBot.BotPrograms
{
    /// <summary>
    /// Smiths gold bracelets at the furnace in Port Phasmatys
    /// </summary>
    public class GoldBracelets : BotProgram
    {
        private ColorRange FurnaceIconOrange;
        private ColorRange BankIconDollar;
        private ColorRange Furnace;

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


            DefaultCamera();
        }

        protected override bool Execute()
        {
            MoveToFurnace();
            Inventory.ClickInventory(0, 1);
            ClickFurnace();

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
        /// Clicks on the furnace
        /// </summary>
        /// <returns></returns>
        private bool ClickFurnace()
        {
            for (int i = 0; i < 15; i++)
            {
                ReadWindow();
                bool[,] furnace = ColorFilter(Furnace);
                Blob furnaceBlob = ImageProcessing.BiggestBlob(furnace);
                if (furnaceBlob != null && furnaceBlob.Size > 100)
                {
                    LeftClick(furnaceBlob.Center.X, furnaceBlob.Center.Y);
                    return true;
                }
                else
                {
                    SafeWait(1000);
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        private void GetReferenceColors()
        {
            FurnaceIconOrange = ColorFilters.FurnaceIconOrange();
            BankIconDollar = ColorFilters.BankIconDollar();
            Furnace = ColorFilters.Furnace();
        }
    }
}
