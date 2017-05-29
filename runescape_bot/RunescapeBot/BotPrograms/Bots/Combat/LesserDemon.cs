using RunescapeBot.ImageTools;
using RunescapeBot.UITools;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace RunescapeBot.BotPrograms
{
    /// <summary>
    /// Targets the lesser demon trapped in the Wizards' Tower
    /// </summary>
    public class LesserDemon : Combat
    {
        private const int MAX_DEMON_SPAWN_TIME = 25000;    //max possible lesser demon spawn time in milliseconds
        private const int DEMON_ENGAGE_TIME = 3000;
        private const int RUNE_MED_HELM_MIN_SIZE = 70;
        private const int MITHRIL_ARMOR_MIN_SIZE = 100;
        private const int CHAOS_RUNE_MIN_SIZE = 3;
        private const int DEATH_RUNE_MIN_SIZE = 5;
        private ColorRange LesserDemonSkin;
        private ColorRange LesserDemonHorn;
        private ColorRange RuneMedHelm;
        private ColorRange MithrilArmor;
        private ColorRange ChaosRune;
        private ColorRange DeathRune;

        /// <summary>
        /// Count of the number of consecutive prior frames where no demon has been found
        /// </summary>
        private int MissedDemons;

        /// <summary>
        /// The minimum required proportion of screen for a lesser demon
        /// </summary>
        private int MinDemonSize;

        /// <summary>
        /// The last location where a demon was found. Set to (0, 0) is no demon has been found yet.
        /// </summary>
        private Point LastDemonLocation;

        /// <summary>
        /// The last time when a demon was found.
        /// </summary>
        private DateTime LastDemonTime;


        public LesserDemon(StartParams startParams) : base(startParams)
        {
            GetReferenceColors();
            MinDemonSize = 2000;
        }

        protected override void Run()
        {
            //ReadWindow();
            //ColorRange empty = ColorFilters.ChaosRuneOrange();
            //bool[,] mask = ColorFilter(empty);
            //DebugUtilities.TestMask(Bitmap, ColorArray, empty, mask, "C:\\Projects\\Roboport\\test_pictures\\mask_tests\\", "chaosRune");

            //ReadWindow();
            //ColorRange filter = ColorFilters.ChaosRuneOrange();
            //bool[,] binary = ColorFilter(filter);
            //double test = ImageProcessing.FractionalMatch(binary);

            //ColorRange filter = ColorFilters.DeathRuneWhite();
            //bool[,] mask = ColorFilter(filter);
            //DebugUtilities.TestMask(Bitmap, ColorArray, filter, mask, "C:\\Projects\\Roboport\\test_pictures\\mask_tests\\", "deathRune");
            //double test = ImageProcessing.FractionalMatch(mask);

            //ColorRange filterM = ColorFilters.MithrilArmor();
            //bool[,] binaryM = ColorFilter(filterM);
            //double testM = ImageProcessing.FractionalMatch(binaryM);

            //LastDemonLocation = Center;
            //CheckDrops();

            //Point trimOffset;
            //Color[,] screenDropArea = ScreenPiece(0, 1800, 0, 900, out trimOffset);
            //FindAndGrabChaosRune(screenDropArea, trimOffset, ChaosRune, CHAOS_RUNE_MIN_SIZE);
        }

        /// <summary>
        /// Called periodically on a timer
        /// </summary>
        protected override bool Execute()
        {
            Blob demon;
            double cloveRange = 2 * Math.Sqrt(MinDemonSize);

            if (LocateObject(LesserDemonSkin, out demon, MinDemonSize) && ClovesWithinRange(demon.Center, cloveRange))
            {
                LastDemonTime = DateTime.Now;
                MinDemonSize = demon.Size / 2;
                int maxOffset = (int)(0.05 * cloveRange);
                if (!InCombat())    //engage the demon
                {
                    LeftClick(demon.Center.X, demon.Center.Y, 200, maxOffset);
                    Mouse.RadialOffset(187, 689, 6, 223);   //arbitrary region to rest the mouse in
                }
                MissedDemons = 0;
                LastDemonLocation = demon.Center;
                SafeWait(DEMON_ENGAGE_TIME);
            }
            else
            {
                MissedDemon();
            }

            return true;
        }

        /// <summary>
        /// Called when a demon is not found
        /// </summary>
        private void MissedDemon()
        {
            MissedDemons++;

            //During the first frame that the bot program can't find a demon, look for drops
            if (MissedDemons == 1)
            {
                CheckDrops();
            }

            //Reduce the minimum required size of the demon in a desperate attempt to find a demon
            if ((DateTime.Now - LastDemonTime).TotalMilliseconds > MAX_DEMON_SPAWN_TIME)
            {
                MinDemonSize /= 2;
                DefaultCamera();
            }

            //Give up, log out of the game, go outside and play
            if ((MissedDemons * RunParams.FrameTime) > (3 * MAX_DEMON_SPAWN_TIME))
            {
                Logout();
                StopFlag = true;
            }
        }

        /// <summary>
        /// Telegrabs a rune med helm if one is found on the ground
        /// </summary>
        /// <returns>True if a drop is found</returns>
        private bool CheckDrops()
        {
            int dropRange = (int) (3 * Math.Sqrt(MinDemonSize));
            int dropRangeLeft = LastDemonLocation.X - dropRange;
            int dropRangeRight = LastDemonLocation.X + dropRange;
            int dropRangeTop = LastDemonLocation.Y - dropRange;
            int dropRangeBottom = LastDemonLocation.Y + dropRange;
            Point trimOffset;
            Color[,] screenDropArea = ScreenPiece(dropRangeLeft, dropRangeRight, dropRangeTop, dropRangeBottom, out trimOffset);

            if (FindAndAlch(screenDropArea, trimOffset, RuneMedHelm, RUNE_MED_HELM_MIN_SIZE)){ return true; }
            if (FindAndAlch(screenDropArea, trimOffset, MithrilArmor, MITHRIL_ARMOR_MIN_SIZE)) { return true; }
            if (FindAndGrabChaosRune(screenDropArea, trimOffset, ChaosRune, CHAOS_RUNE_MIN_SIZE)) { return true; }
            if (FindAndGrab(screenDropArea, trimOffset, DeathRune, DEATH_RUNE_MIN_SIZE)) { return true; }

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
            Blob biggestBlob = ImageProcessing.BiggestBlob(matchedPixels);

            if (biggestBlob.Size > minimumSize)
            {
                Point blobCenter = biggestBlob.Center;
                return Inventory.GrabAndAlch(blobCenter.X + offset.X, blobCenter.Y + offset.Y);
            }
            return false;
        }

        /// <summary>
        /// Looks for, picks up, and alchs a drop that matches a ColorRange
        /// </summary>
        /// <param name="screenDropArea"></param>
        /// <param name="referenceColor"></param>
        /// <param name="minimumSize">minimum number of pixels needed to </param>
        /// <returns>True if an item is found and telegrabbed. May be false if no item is found or if there isn't inventory space to pick it up.</returns>
        private bool FindAndGrab(Color[,] screenDropArea, Point offset, ColorRange referenceColor, int minimumSize = 50)
        {
            bool[,] matchedPixels = ColorFilter(screenDropArea, referenceColor);
            Blob biggestBlob = ImageProcessing.BiggestBlob(matchedPixels);

            if (biggestBlob.Size > minimumSize)
            {
                Point blobCenter = biggestBlob.Center;
                Inventory.Telegrab(blobCenter.X + offset.X, blobCenter.Y + offset.Y);
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
        /// <returns>True if an item is found and telegrabbed. May be false if no item is found or if there isn't inventory space to pick it up.</returns>
        private bool FindAndGrabChaosRune(Color[,] screenDropArea, Point offset, ColorRange referenceColor, int minimumSize = 50)
        {
            bool[,] matchedPixels = ColorFilter(screenDropArea, referenceColor);
            List<Blob> chaosRunes = ImageProcessing.FindBlobs(matchedPixels, true);

            for (int i = 0; i < Math.Min(10, chaosRunes.Count); i++)
            {
                if ((chaosRunes[i].Size > minimumSize) && chaosRunes[i].IsCircle(0.4))
                {
                    Point blobCenter = chaosRunes[i].Center;
                    Inventory.Telegrab(blobCenter.X + offset.X, blobCenter.Y + offset.Y);
                    return true;
                }
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
            int minCloveSize = MinDemonSize / 400;
            bool[,] cloves = ColorFilterPiece(LesserDemonHorn, demonCenter, (int)cloveRange);
            List<Blob> demonCloves = ImageProcessing.FindBlobs(cloves, true);
            if (demonCloves.Count >= requiredCloves)
            {
                return demonCloves[requiredCloves - 1].Size > minCloveSize;
            }
            return false;
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
            ChaosRune = ColorFilters.ChaosRuneOrange();
            DeathRune = ColorFilters.DeathRuneWhite();
        }
    }
}
