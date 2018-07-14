using RunescapeBot.BotPrograms.Settings;
using RunescapeBot.Common;
using RunescapeBot.FileIO;
using RunescapeBot.ImageTools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms
{
    public class Falconry : BotProgram
    {
        public static ColorFilter FlashingArrow = RGBHSBRangeFactory.FlashingArrow();

        public Falconry(RunParams startParams) : base(startParams)
        {
            RunParams.Run = true;
            RunParams.ClosedChatBox = true;
            UserSelections = startParams.CustomSettingsData.Falconry;
            FailedRuns = 0;
            KebbitSearchRadius = ArtifactLength(0.5);
        }

        protected FalconrySettingsData UserSelections;
        protected int FailedRuns;
        protected int KebbitSearchRadius;


        protected override bool Run()
        {
            //DebugUtilities.SaveImageToFile(GameScreen);

            //MaskTest(KebbitBlackSpot, "black_spot");
            //MaskTest(KebbitWhiteSpot, "white_spot");
            //MaskTest(KebbitSpottedFur, "spotted_fur");
            //MaskTest(KebbitDashingFur, "dashing_fur");
            //MaskTest(KebbitDarkFur, "dark_dur");
            //MaskTest(FlashingArrow, "arrow");

            //RetrieveFalcon(new Point(957, 502));

            Inventory.SetEmptySlots();
            return true;
        }

        protected override bool Execute()
        {
            //Drop kebbit pickups to make room for more.
            if (!Inventory.SlotIsEmpty(19, true))
            {
                Inventory.DropInventory(false);
            }

            List<Kebbit> kebbits = LocateKebbits();
            kebbits = SortAndFilterKebbits(kebbits);
            kebbits = kebbits.GetRange(0, Math.Min(kebbits.Count, 1));   //Only try the first kebbit.

            foreach (Kebbit kebbit in kebbits)
            {
                if (StopFlag) { return false; }

                if (MouseOverNPC(new Blob(kebbit.Location.Center), true, 1, 500) && RetrieveFalcon(kebbit.Location.Center))
                {
                    FailedRuns = 0;
                    RunParams.Iterations--;
                    KebbitSearchRadius = 2 * (int)Geometry.DistanceBetweenPoints(Center, kebbit.Location.Center);
                    return true;
                }
            }

            FailedRuns++;
            KebbitSearchRadius *= 2;
            return FailedRuns < 25;
        }

        /// <summary>
        /// Attempts to retrieve a falcon after launching it at a kebbit.
        /// </summary>
        /// <param name="target">The expected location of the falcon catching the kebbit.</param>
        /// <returns>True if the falcon catches the kebbit and is successfully retrieved.</returns>
        protected bool RetrieveFalcon(Point target)
        {
            if (!WaitForCatch(ref target) || !WaitForFalconToStop(ref target))
            {
                return false;
            }

            //Find an inventory slot that should fill when we retrieve the falcon.
            Point? nextEmptyInventorySlot = Inventory.FirstEmptySlot(false);
            if (nextEmptyInventorySlot == null)
            {
                return false;
            }

            do
            {
                target = ArrowToFalcon(target);
                if (MouseOverNPC(new Blob(target), true, 4))
                {
                    SafeWait((int)(RunTime(target) / 2));
                    WaitDuringPlayerAnimation(5000);
                    return true;
                }
            }
            while (Inventory.SlotIsEmpty(nextEmptyInventorySlot.Value) && !StopFlag);

            return false;
        }

        /// <summary>
        /// Determines the position of a falcon that has caught a kebbit based on the position of the flashing arrow above it.
        /// </summary>
        /// <param name="arrow">Center of a flashing arrow that points to a falcon.</param>
        /// <returns>Position of a falcon that has caught a kebbit.</returns>
        protected Point ArrowToFalcon(Point arrow)
        {
            Point falcon = new Point(arrow.X, arrow.Y + ArtifactLength(0.0229));
            return falcon;
        }

        /// <summary>
        /// Collects the Gyr falcon after it kills a kebbit.
        /// </summary>
        /// <returns>true if the falcon catches a kebbit </returns>
        protected bool WaitForFalconToStop(ref Point target)
        {
            Point falcon;
            Point arrowLocation = new Point(-1000, -1000);
            Stopwatch watch = new Stopwatch();
            watch.Start();
            while (watch.ElapsedMilliseconds < 12000 && !StopFlag)
            {
                if (LocateFlashingArrow(ref target))
                {
                    if (Geometry.DistanceBetweenPoints(arrowLocation, target) < ArtifactLength(0.01))
                    {
                        return true;
                    }
                    else
                    {
                        arrowLocation = target;
                        falcon = ArrowToFalcon(target);
                        MoveMouse(falcon.X, falcon.Y);
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Waits for the Gyr falcon to catch the kebbit.
        /// </summary>
        /// <param name="target">The target location toward which the falcon was launched.</param>
        /// <returns>True if a kebbit is caught. False if the falcon arrow does not appear.</returns>
        protected bool WaitForCatch(ref Point target)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            while (watch.ElapsedMilliseconds < 5000 && !StopFlag)
            {
                if (LocateFlashingArrow(ref target))
                {
                    return true;
                }
            }
            
            return false;
        }

        /// <summary>
        /// Looks for the flashing arrow that appears above a falcon after catching a kebbit.
        /// </summary>
        /// <param name="target">The expected location of the flashing arrow.</param>
        /// <returns>True if arrow is found.</returns>
        protected bool LocateFlashingArrow(ref Point target)
        {
            Blob arrow;
            if (LocateObject(FlashingArrow, out arrow, target, ArtifactLength(0.75), 1, int.MaxValue))
            {
                target = arrow.Center;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Finds the possible kebbits.
        /// </summary>
        /// <returns>A list of kebbits in the order that they should be checked.</returns>
        protected List<Kebbit> LocateKebbits()
        {
            ReadWindow();
            bool[,] blackSpotMatches = ColorFilter(Kebbit.KebbitBlackSpot);
            EraseClientUIFromMask(ref blackSpotMatches);
            int playerExclusionOffset = ArtifactLength(0.02988);    //ex -27, +22, -27 +21
            EraseFromMask(ref blackSpotMatches, Center.X - playerExclusionOffset, Center.X + playerExclusionOffset, Center.Y - playerExclusionOffset, Center.Y + playerExclusionOffset);
            List<Blob> blackSpots = ImageProcessing.FindBlobs(blackSpotMatches, false, 1, ArtifactArea(0.00002183));    //ex 1-11 (0.000000992-0.00001091)
            List<Cluster> kebbitLocations = ImageProcessing.ClusterBlobs(blackSpots, ArtifactLength(0.0349));

            List<Kebbit> kebbits = new List<Kebbit>();
            foreach (Cluster kebbitSpots in kebbitLocations)
            {
                kebbits.Add(new Kebbit(GameScreen, kebbitSpots, Center));
            }

            return kebbits;
        }

        /// <summary>
        /// Removed kebbits of types that the user has chosen not to catch.
        /// Sorts kebbits based on proximity and type.
        /// </summary>
        /// <param name="kebbits">List of kebbits to sort.</param>
        /// <returns>A list of kebbits in the order that they should be attempted.</returns>
        protected List<Kebbit> SortAndFilterKebbits(List<Kebbit> kebbits)
        {
            List<Kebbit> approvedKebbits = new List<Kebbit>();

            //Remove undesired kebbits(not selected by user).
            for (int i = 0; i < kebbits.Count; i++)
            {
                switch (kebbits[i].Type)
                {
                    case Kebbit.KebbitType.Spotted:
                        if (UserSelections.CatchSpottedKebbits)
                        {
                            approvedKebbits.Add(kebbits[i]);
                        }
                        break;

                    case Kebbit.KebbitType.Dark:
                        if (UserSelections.CatchDarkKebbits)
                        {
                            approvedKebbits.Add(kebbits[i]);
                        }
                        break;

                    case Kebbit.KebbitType.Dashing:
                        if (UserSelections.CatchDashingKebbits)
                        {
                            approvedKebbits.Add(kebbits[i]);
                        }
                        break;
                }
            }

            approvedKebbits.Sort(new KebbitComparer()); //Sorts kebbits based on proximity and experience.
            return approvedKebbits;
        }
    }
}
