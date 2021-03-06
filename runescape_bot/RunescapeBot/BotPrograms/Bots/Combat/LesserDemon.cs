﻿using RunescapeBot.FileIO;
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
        protected const int MAX_DEMON_SPAWN_TIME = 30000;    //max possible lesser demon spawn time in milliseconds
        protected const int DEMON_ENGAGE_TIME = 3000;
        protected const int CHAOS_RUNE_MIN_SIZE = 3;
        protected const int DEATH_RUNE_MIN_SIZE = 5;
        protected RGBHSBRange LesserDemonSkin;
        protected RGBHSBRange LesserDemonHorn;
        protected RGBHSBRange RuneMedHelm;
        protected RGBHSBRange MithrilArmor;
        protected RGBHSBRange ChaosRune;
        protected RGBHSBRange DeathRune;
        protected RGBHSBRange MouseoverTextNPC;
        protected Blob LastDemon;
        protected bool PickUpStackables;
        protected bool PickUpAlchables;
        protected bool AlchAlchables;
        protected int RuneMinSize { get { return Screen.ArtifactArea(0.0000694); } }
        protected int MithrilMinSize { get { return Screen.ArtifactArea(0.0000992); } }
        protected int MinDemonSize { get { return Screen.ArtifactArea(0.000944); } }

        /// <summary>
        /// Count of the number of consecutive prior frames where no demon has been found
        /// </summary>
        private int MissedDemons;

        /// <summary>
        /// The last time when a demon was found.
        /// </summary>
        private DateTime LastDemonTime;


        public LesserDemon(RunParams startParams) : base(startParams)
        {
            GetReferenceColors();
            PickUpStackables = RunParams.CustomSettingsData.LesserDemon.Telegrab;
            PickUpAlchables = RunParams.CustomSettingsData.LesserDemon.Telegrab && RunParams.CustomSettingsData.LesserDemon.HighAlch;
            AlchAlchables = RunParams.CustomSettingsData.LesserDemon.HighAlch;
            LastDemonTime = DateTime.Now;
        }

        protected override bool Run()
        {
            #region debugging

            //ReadWindow();
            //ColorRange empty = RGBHSBRanges.ChaosRuneOrange();
            //bool[,] mask = ColorFilter(empty);
            //DebugUtilities.TestMask(Bitmap, ColorArray, empty, mask, "C:\\Projects\\Roboport\\test_pictures\\mask_tests\\", "chaosRune");

            //ReadWindow();
            //ColorRange filter = RGBHSBRanges.ChaosRuneOrange();
            //bool[,] binary = ColorFilter(filter);
            //double test = ImageProcessing.FractionalMatch(binary);

            //ColorRange filter = RGBHSBRanges.DeathRuneWhite();
            //bool[,] mask = ColorFilter(filter);
            //DebugUtilities.TestMask(Bitmap, ColorArray, filter, mask, "C:\\Projects\\Roboport\\test_pictures\\mask_tests\\", "deathRune");
            //double test = ImageProcessing.FractionalMatch(mask);

            //ColorRange filterM = RGBHSBRanges.MithrilArmor();
            //bool[,] binaryM = ColorFilter(filterM);
            //double testM = ImageProcessing.FractionalMatch(binaryM);

            //ColorRange color = RGBHSBRanges.LesserDemonHorn();
            //bool[,] mask = ColorFilter(color);
            //DebugUtilities.TestMask(Bitmap, ColorArray, color, mask, "C:\\Projects\\Roboport\\test_pictures\\mask_tests\\", "cloves");

            //ReadWindow();
            //RGBHSBRange rune = RGBHSBRangeFactory.RuneMedHelm();
            //bool[,] runeMask = ColorFilter(rune);
            //DebugUtilities.TestMask(Bitmap, ColorArray, rune, runeMask, "C:\\Projects\\Roboport\\test_pictures\\mask_tests\\", "rune");

            //ReadWindow();
            //RGBHSBRange mithril = RGBHSBRangeFactory.MithrilArmor();
            //bool[,] mithrilMask = ColorFilter(mithril);
            //DebugUtilities.TestMask(Bitmap, ColorArray, mithril, mithrilMask, "C:\\Projects\\Roboport\\test_pictures\\mask_tests\\", "mithril");

            //SafeWait(3000);
            //ReadWindow();
            //ColorRange color = RGBHSBRanges.MouseoverTextNPC();
            //bool[,] mask = ColorFilter(color);
            //DebugUtilities.TestMask(Bitmap, ColorArray, color, mask, "C:\\Projects\\Roboport\\test_pictures\\mask_tests\\", "mouseoverNPCText");

            //LastDemonLocation = Center;
            //CheckDrops();

            //Point trimOffset;
            //Color[,] screenDropArea = ScreenPiece(0, 1800, 0, 900, out trimOffset);
            //FindAndGrabChaosRune(screenDropArea, trimOffset, ChaosRune, CHAOS_RUNE_MIN_SIZE);

            //LastDemonLocation = Center;
            //LastDemonLocation.Y += 50;
            //CheckDrops();

            //Inventory.Telegrab(Center.X, Center.Y);

            #endregion

            return true;
        }

        /// <summary>
        /// Called periodically on a timer
        /// </summary>
        protected override bool Execute()
        {
            Blob demon;
            double cloveRange = Math.Max(2.0 * Math.Sqrt(MinDemonSize), Screen.ArtifactLength(0.05));

            if (Vision.LocateObject(LesserDemonSkin, out demon, MinDemonSize) && ClovesWithinRange(demon.Center, cloveRange))
            {                
                if (InCombat() && HitpointsHaveDecreased())    //engage the demon
                {
                    FoundDemon(demon);
                    return true;
                }

                Mouse.Move(demon.Center.X, demon.Center.Y);
                if (Vision.WaitForMouseOverText(MouseoverTextNPC, 3000))
                {
                    FoundDemon(demon);
                    LeftClick(demon.Center.X, demon.Center.Y, 0, 0);
                    Mouse.RadialOffset(187, 689, 6, 223);   //arbitrary region to rest the mouse in
                    SafeWait(DEMON_ENGAGE_TIME);
                    return true;
                }

            }
            return MissedDemon();
        }

        /// <summary>
        /// Recordkeeping in response to locating a demon
        /// </summary>
        /// <param name="demon"></param>
        private void FoundDemon(Blob demon)
        {
            MissedDemons = 0;
            LastDemonTime = DateTime.Now;
            LastDemon = demon;
        }

        /// <summary>
        /// Called when a demon is not found
        /// </summary>
        private bool MissedDemon()
        {
            MissedDemons++;

            if (MissedDemons == 1)
            {
                SafeWait(1000);
                RunParams.Iterations--;
                CheckDrops();
            }

            //Reduce the minimum required size of the demon in a desperate attempt to find a demon
            if ((DateTime.Now - LastDemonTime).TotalMilliseconds > MAX_DEMON_SPAWN_TIME)
            {
                LogError.ScreenShot(Screen, "long-spawn-" + (DateTime.Now - LastDemonTime).TotalMilliseconds);
                DefaultCamera();
            }

            //Give up, log out of the game, go outside and play
            if ((MissedDemons * RunParams.FrameTime) > (5 * MAX_DEMON_SPAWN_TIME))
            {
                LogError.ScreenShot(Screen, MissedDemons + "-missed-demons");
                Logout();
                return false;
            }

            return true;    //keep trying
        }

        /// <summary>
        /// Telegrabs a drop if one is found on the ground
        /// Alchs the drop if applicable
        /// </summary>
        /// <returns>True if a drop is found</returns>
        private bool CheckDrops()
        {
            if (LastDemon == null) { return false; }    //No demon has been found yet

            Screen.ReadWindow();

            int dropRange = (int) (4 * Math.Sqrt(MinDemonSize));
            int dropRangeLeft = (int) (LastDemon.Center.X - 1.18 * dropRange);
            int dropRangeRight = (int) (LastDemon.Center.X + 0.82 * dropRange);
            int dropRangeTop = (int) (LastDemon.Center.Y - 0.24 * dropRange);
            int dropRangeBottom = (int) (LastDemon.Center.Y + 0.76 * dropRange);
            Point trimOffset;
            Color[,] screenDropArea = Vision.ScreenPiece(dropRangeLeft, dropRangeRight, dropRangeTop, dropRangeBottom, out trimOffset);

            if (PickUpStackables)
            {
                if (FindAndGrabChaosRune(screenDropArea, trimOffset, ChaosRune, CHAOS_RUNE_MIN_SIZE)) { return true; }
                if (FindAndGrab(screenDropArea, trimOffset, DeathRune, DEATH_RUNE_MIN_SIZE)) { return true; }
            }

            if (PickUpAlchables)
            {
                if (FindAndAlch(screenDropArea, trimOffset, RuneMedHelm, RuneMinSize)) { return true; }
                if (FindAndAlch(screenDropArea, trimOffset, MithrilArmor, MithrilMinSize)) { return true; }
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
        private bool FindAndAlch(Color[,] screenDropArea, Point offset, ColorFilter referenceColor, int minimumSize)
        {
            bool[,] matchedPixels = Vision.ColorFilter(screenDropArea, referenceColor);
            Blob biggestBlob = ImageProcessing.BiggestBlob(matchedPixels);

            if (biggestBlob.Size < minimumSize)
            {
                return false;   //Nothing to grab.
            }

            Point blobCenter = biggestBlob.Center;
            if (AlchAlchables)
            {
                Inventory.GrabAndAlch(blobCenter.X + offset.X, blobCenter.Y + offset.Y);
                Inventory.OpenInventory();
            }
            else
            {
                Inventory.Telegrab(blobCenter.X + offset.X, blobCenter.Y + offset.Y);
                Inventory.OpenInventory();
            }
                
            return true;
        }

        /// <summary>
        /// Looks for, picks up, and alchs a drop that matches a ColorRange
        /// </summary>
        /// <param name="screenDropArea"></param>
        /// <param name="referenceColor"></param>
        /// <param name="minimumSize">minimum number of pixels needed to </param>
        /// <returns>True if an item is found and telegrabbed. May be false if no item is found or if there isn't inventory space to pick it up.</returns>
        private bool FindAndGrab(Color[,] screenDropArea, Point offset, ColorFilter referenceColor, int minimumSize = 50)
        {
            bool[,] matchedPixels = Vision.ColorFilter(screenDropArea, referenceColor);
            Blob biggestBlob = ImageProcessing.BiggestBlob(matchedPixels);

            if (biggestBlob.Size > minimumSize)
            {
                Point blobCenter = biggestBlob.Center;
                Inventory.Telegrab(blobCenter.X + offset.X, blobCenter.Y + offset.Y);
                Inventory.OpenInventory();
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
        private bool FindAndGrabChaosRune(Color[,] screenDropArea, Point offset, ColorFilter referenceColor, int minimumSize = 50)
        {
            bool[,] matchedPixels = Vision.ColorFilter(screenDropArea, referenceColor);
            List<Blob> chaosRunes = ImageProcessing.FindBlobs(matchedPixels, true);

            for (int i = 0; i < Math.Min(10, chaosRunes.Count); i++)
            {
                if ((chaosRunes[i].Size > minimumSize) && chaosRunes[i].IsCircle(0.4))
                {
                    Point blobCenter = chaosRunes[i].Center;
                    Inventory.Telegrab(blobCenter.X + offset.X, blobCenter.Y + offset.Y);
                    Inventory.OpenInventory();
                    return true;
                }
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
            int requiredCloves = 1;
            int minCloveSize = MinDemonSize / 400;
            bool[,] cloves = Vision.ColorFilterPiece(LesserDemonHorn, demonCenter, (int)cloveRange);
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
            LesserDemonSkin = RGBHSBRangeFactory.LesserDemonSkin();
            LesserDemonHorn = RGBHSBRangeFactory.LesserDemonHorn();
            RuneMedHelm = RGBHSBRangeFactory.RuneMedHelm();
            MithrilArmor = RGBHSBRangeFactory.MithrilArmor();
            ChaosRune = RGBHSBRangeFactory.ChaosRuneOrange();
            DeathRune = RGBHSBRangeFactory.DeathRuneWhite();
            MouseoverTextNPC = RGBHSBRangeFactory.MouseoverTextNPC();
        }
    }
}
