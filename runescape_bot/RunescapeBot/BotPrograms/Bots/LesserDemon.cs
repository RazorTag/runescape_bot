using RunescapeBot.ImageTools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;

namespace RunescapeBot.BotPrograms
{
    /// <summary>
    /// Targets the lesser demon trapped in the Wizards' Tower
    /// </summary>
    public class LesserDemon : BotProgram
    {
        private const int maxDemonSpawnTime = 28000;    //max possible lesser demon spawn time in milliseconds
        private ColorRange LesserDemonSkin;
        private ColorRange LesserDemonHorn;
        private ColorRange RuneMedHelm;
        private ColorRange MithrilArmor;

        /// <summary>
        /// Count of the number of consecutive prior frames where no demon has been found
        /// </summary>
        private int MissedDemons;

        /// <summary>
        /// The minimum required proportion of screen for a lesser demon
        /// </summary>
        private double MinDemonSize;

        /// <summary>
        /// The last location where a demon was found. Set to (0, 0) is no demon has been found yet.
        /// </summary>
        private Point LastDemonLocation;


        public LesserDemon(StartParams startParams) : base(startParams)
        {
            GetReferenceColors();
            MinDemonSize = 0.001;
        }

        protected override void Run()
        {
            //ColorRange empty = ColorFilters.EmptyInventorySlot();
            //bool[,] mask = ColorFilter(empty);
            //DebugUtilities.TestMask(Bitmap, ColorArray, empty, mask, "C:\\Projects\\Roboport\\test_pictures\\mask_tests\\", "emptySlot");
            //Inventory.GrabAndAlch(1500, 800);
            //Inventory.FirstEmptySlot();
        }

        /// <summary>
        /// Called periodically on a timer
        /// </summary>
        protected override bool Execute()
        {
            int xOffset, yOffset, maxOffset;
            bool[,] skinPixels = ColorFilter(LesserDemonSkin);
            if (StopFlag) { return false; }   //quit immediately if the stop flag has been raised
            EraseClientUIFromMask(ref skinPixels);
            Blob demon = ImageProcessing.BiggestBlob(skinPixels);
            if (StopFlag) { return false; }   //quit immediately if the stop flag has been raised

            Point demonCenter = demon.Center;
            double cloveRange = 2 * Math.Sqrt(demon.Size);
            if (MinimumSizeMet(demon) && ClovesWithinRange(demonCenter, cloveRange))
            {
                maxOffset = (int) (0.05 * cloveRange);
                xOffset = RNG.Next(-maxOffset, maxOffset + 1);
                yOffset = RNG.Next(-maxOffset, maxOffset + 1);
                LeftClick(demonCenter.X, demonCenter.Y);
                MissedDemons = 0;
                MinDemonSize = ArtifactSize(demon) / 2.0;
                LastDemonLocation = demonCenter;
            }
            else
            {
                MissedDemons++;
            }

            //During the first frame that the bot program cant find a demon, look for a rune med helm drop
            if (MissedDemons == 1 && CheckDrops())
            {
                MissedDemons = 0;
            }

            //Reduce the minimum size of the demon in a desperate attempt to find a demon
            if (MissedDemons * RunParams.FrameTime > maxDemonSpawnTime)
            {
                MinDemonSize /= 2.0;
                DefaultCamera();
            }

            //Give up, log out of the game, go outside, and play
            if ((MissedDemons * RunParams.FrameTime) > (3 * maxDemonSpawnTime))
            {
                Logout();
            }

            return true;
        }

        /// <summary>
        /// Telegrabs a rune med helm if one is found on the ground
        /// </summary>
        /// <returns>True if a drop is found</returns>
        private bool CheckDrops()
        {
            int dropRange = 350;
            int dropRangeLeft = LastDemonLocation.X - dropRange;
            int dropRangeRight = LastDemonLocation.X + dropRange;
            int dropRangeTop = LastDemonLocation.Y - dropRange;
            int dropRangeBottom = LastDemonLocation.Y + dropRange;
            Point trimOffset;
            Color[,] screenDropArea = ScreenPiece(dropRangeLeft, dropRangeRight, dropRangeTop, dropRangeBottom, out trimOffset);

            if (FindAndAlch(screenDropArea, trimOffset, RuneMedHelm, 70))
            {
                return true;
            }
            if (FindAndAlch(screenDropArea, trimOffset, MithrilArmor, 100))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Looks for, picks up, and alchs a drop that matches a ColorRange
        /// </summary>
        /// <param name="screenDropArea"></param>
        /// <param name="referenceColor"></param>
        /// <param name="minimumSize">minimum number of pixels needed to </param>
        /// <returns>True if an item is found, picked up, and alched. May be false if no item is found or if there isn't inventory space to pick it up.</returns>
        private bool FindAndAlch(Color[,] screenDropArea, Point offset, ColorRange referenceColor, int minimumSize = 50)
        {
            bool[,] matchedPixels = ColorFilter(screenDropArea, referenceColor);
            EraseClientUIFromMask(ref matchedPixels);
            Blob biggestBlob = ImageProcessing.BiggestBlob(matchedPixels);

            if (biggestBlob.Size > minimumSize)
            {
                Point blobCenter = biggestBlob.Center;
                return Inventory.GrabAndAlch(blobCenter.X + offset.X, blobCenter.Y + offset.Y);
            }
            return false;
        }

        /// <summary>
        /// Determines if a demon blob meets the minimum size requirement
        /// </summary>
        /// <param name="demon"></param>
        /// <returns></returns>
        private bool MinimumSizeMet(Blob demon)
        {
            double demonScreenSize = ArtifactSize(demon);
            if (demonScreenSize > MinDemonSize)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines if there are enough cloves close enough to the demon
        /// </summary>
        /// <param name="demonCenter"></param>
        /// <param name="cloveRange"></param>
        /// <returns></returns>
        private bool ClovesWithinRange(Point demonCenter, double cloveRange)
        {
            int requiredCloves = 3;
            int clovesToCheck = 8;
            int clovesFound = 0;
            bool[,] hornPixels = ColorFilter(LesserDemonHorn);
            EraseClientUIFromMask(ref hornPixels);
            List<Blob> demonCloves = Blob.SortBlobs(ImageProcessing.FindBlobs(hornPixels));
            clovesToCheck = Math.Min(demonCloves.Count, 8);

            for (int i = 0; i < clovesToCheck; i++)
            {
                if (demonCloves[i].DistanceTo(demonCenter) < cloveRange)
                {
                    clovesFound++;
                }

                if (clovesFound >= requiredCloves)
                {
                    return true;
                }
            }

            return true;
        }

        public int SizeOfMatch(bool[,] mask)
        {
            int matches = 0;

            for (int x = 0; x < mask.GetLength(0); x++)
            {
                for (int y = 0; y < mask.GetLength(1); y++)
                {
                    if (mask[x, y])
                    {
                        matches++;
                    }
                }
            }
            return matches;
        }

        /// <summary>
        /// Sets the reference colors for the lesser demon's parts if they haven't been set already
        /// </summary>
        private void GetReferenceColors()
        {
            LesserDemonSkin = ColorFilters.LesserDemonSkin();
            LesserDemonHorn = ColorFilters.LesserDemonHorn();
            RuneMedHelm = ColorFilters.RuneMedHelm();
            MithrilArmor = ColorFilters.MithrilArmor();
        }
    }
}
