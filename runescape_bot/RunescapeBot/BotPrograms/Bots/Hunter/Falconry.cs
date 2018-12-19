using RunescapeBot.BotPrograms.Popups;
using RunescapeBot.BotPrograms.Settings;
using RunescapeBot.Common;
using RunescapeBot.FileIO;
using RunescapeBot.ImageTools;
using RunescapeBot.UITools;
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
            RunParams.FrameTime = 0;
            RunParams.Run = true;
            RunParams.ClosedChatBox = true;
            UserSelections = startParams.CustomSettingsData.Falconry;
            FailedRuns = 0;
        }

        protected FalconrySettingsData UserSelections;
        protected int FailedRuns;
        protected int KebbitSearchRadius;
        protected bool FalconRanAway;


        protected override bool Run()
        {
            //DebugUtilities.SaveImageToFile(GameScreen);

            //MaskTest(Kebbit.KebbitBlackSpot, "black_spot");
            //MaskTest(KebbitWhiteSpot, "white_spot");
            //MaskTest(KebbitSpottedFur, "spotted_fur");
            //MaskTest(KebbitDashingFur, "dashing_fur");
            //MaskTest(Kebbit.KebbitDarkFur, "dark_dur");
            //MaskTest(FlashingArrow, "arrow");

            //RetrieveFalcon(new Point(957, 502));
            //Inventory.DropInventory(false, false);

            Inventory.SetEmptySlots();
            return true;
        }

        protected override bool Execute()
        {
            //Drop kebbit pickups to make room for more.
            if (!Inventory.SlotIsEmpty(18, true))
            {
                Inventory.DropInventory(false);
            }

            List<Kebbit> kebbits = LocateKebbits();
            kebbits = SortAndFilterKebbits(kebbits);
            kebbits = kebbits.GetRange(0, Math.Min(kebbits.Count, 2));   //Only try the first 2 kebbits.

            foreach (Kebbit kebbit in kebbits)
            {
                if (StopFlag) { return false; }

                if (HandEye.MouseOverNPC(new Blob(kebbit.Location.Center), true, 1, 500))
                {
                    if (RetrieveFalcon(kebbit.Location.Center))
                    {
                        FailedRuns = 0;
                        RunParams.Iterations--;
                        return true;
                    }
                    break;  //Don't keep trying after a failed catch attempt.
                }
            }

            FailedRuns++;
            if (FailedRuns >= 5)
            {
                if (FailedRuns % 10 == 0)
                {
                    Minimap.MoveToPosition(225, 0.95, true, 3, 2500, null, 10000);
                    Vision.WaitDuringPlayerAnimation(6000);
                }
                else if (FailedRuns % 5 == 0)
                {
                    Minimap.MoveToPosition(315, 0.95, true, 3, 2500, null, 10000);
                    Vision.WaitDuringPlayerAnimation(6000);
                }
            }
            return FailedRuns < 60 && !FalconRanAway;
        }

        /// <summary>
        /// Attempts to retrieve a falcon after launching it at a kebbit.
        /// </summary>
        /// <param name="target">The expected location of the falcon catching the kebbit.</param>
        /// <returns>True if the falcon catches the kebbit and is successfully retrieved.</returns>
        protected bool RetrieveFalcon(Point target)
        {
            //Find the second inventory slot that should fill when we retrieve the falcon so as not to confuse with accidentally picking up an item from the ground.
            Point? nextEmptyInventorySlot = Inventory.FirstEmptySlot(false, 2);
            if (nextEmptyInventorySlot == null)
            {
                return false;
            }

            if (!WaitForCatch(ref target) || !WaitForFalconToStop(ref target, (Point)nextEmptyInventorySlot))
            {
                return false;
            }

            Stopwatch retrieveWatch = new Stopwatch();
            retrieveWatch.Start();
            while (retrieveWatch.ElapsedMilliseconds < 60000 && !StopFlag)
            {
                if (HandEye.MouseOverNPC(new Blob(target), true, 0))
                {
                    SafeWait((int)(RunTime(target) / 2));
                    if (Inventory.WaitForSlotToFill((Point)nextEmptyInventorySlot, 5000))
                    {
                        return true;
                    }
                }
                else if (HandEye.MouseOverStationaryObject(new Blob(target), false, 0, 0))
                {
                    //LeftClick(target.X, target.Y, 12);
                    //WaitDuringPlayerAnimation((int) (RunTime(target) * 1.5));

                    RightClick(target.X, target.Y, 0);
                    RightClick falconMenu = new RightClick(target.X, target.Y, RSClient, Keyboard);
                    falconMenu.WaitForPopup();
                    falconMenu.CustomOption(1);
                    if (SafeWait(1000)) { return false; }
                    //TODO Search for the first row in the popup with NPC yellow text instead of picking the second row.
                }

                //We accidentally collected the falcon at some point.
                if (!Inventory.SlotIsEmpty(nextEmptyInventorySlot.Value, true))
                {
                    return true;
                }
                //The falcon ran away.
                else if (!LocateFlashingArrow(ref target))
                {
                    FalconRanAway = true;
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        /// Determines the position of a falcon that has caught a kebbit based on the position of the flashing arrow above it.
        /// </summary>
        /// <param name="arrow">Center of a flashing arrow that points to a falcon.</param>
        /// <returns>Position of a falcon that has caught a kebbit.</returns>
        protected Point ArrowToFalcon(Point arrow)
        {
            Point falcon = new Point(arrow.X, arrow.Y + Screen.ArtifactLength(0.0229));
            return falcon;
        }

        /// <summary>
        /// Collects the Gyr falcon after it kills a kebbit.
        /// </summary>
        /// <returns>true if the falcon catches a kebbit </returns>
        protected bool WaitForFalconToStop(ref Point target, Point nextEmptyInventorySlot)
        {
            Point lastFalcon = new Point(-1000, -1000);
            Stopwatch watch = new Stopwatch();
            watch.Start();
            while (watch.ElapsedMilliseconds < 12000 && !StopFlag)
            {
                if (LocateFlashingArrow(ref target))
                {
                    if (Geometry.DistanceBetweenPoints(lastFalcon, target) < Screen.ArtifactLength(0.01))
                    {
                        return true;
                    }
                    else
                    {
                        lastFalcon = target;
                        if (Geometry.DistanceBetweenPoints(Screen.Center, target) > Screen.ArtifactLength(0.25))
                        {
                            LeftClick(target.X, target.Y, 0);
                            if (SafeWait((long)(RunTime(target) / 2))) { return false; }
                            Vision.WaitDuringPlayerAnimation(3000);
                            if (!Inventory.SlotIsEmpty(nextEmptyInventorySlot, true))
                            {
                                return true;
                            }
                        }
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
            if (Vision.LocateObject(FlashingArrow, out arrow, target, Screen.ArtifactLength(1), 1, int.MaxValue))
            {
                target = ArrowToFalcon(arrow.Center);
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
            Screen.ReadWindow();
            bool[,] blackSpotMatches = Vision.ColorFilter(Kebbit.KebbitBlackSpot);
            Vision.EraseClientUIFromMask(ref blackSpotMatches);
            List<Blob> blackSpots = ImageProcessing.FindBlobs(blackSpotMatches, false, 1, Screen.ArtifactArea(0.00002183));    //ex 1-11 (0.000000992-0.00001091)
            List<Cluster> kebbitLocations = ImageProcessing.ClusterBlobs(blackSpots, Screen.ArtifactLength(0.0349));

            List<Kebbit> kebbits = new List<Kebbit>();
            foreach (Cluster kebbitSpots in kebbitLocations)
            {
                kebbits.Add(new Kebbit(Screen, kebbitSpots, Screen.Center));
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
                //Reject kebbits that are probably the player.
                if (Geometry.DistanceBetweenPoints(Screen.Center, kebbits[i].Location.Center) < Screen.ArtifactLength(0.028))
                {
                    continue;
                }

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
