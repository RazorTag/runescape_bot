using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RunescapeBot.ImageTools;
using System.Drawing;
using RunescapeBot.Common;
using RunescapeBot.UITools;

namespace RunescapeBot.BotPrograms
{
    public class BarbarianFishing : BotProgram
    {
        bool emptySlotsSet;
        RGBHSBRange FishTileFilter = RGBHSBRangeFactory.FishingTile();
        RGBHSBRange FishingPoleFilter = RGBHSBRangeFactory.FishingPole();
        int fishTileSearchRadius;
        int maxFishingPoleDistance;


        public BarbarianFishing(RunParams startParams) : base(startParams)
        {
            RunParams.Run = true;
            RunParams.FrameTime = 5000;
            //fishTileSearchRadius = ArtifactLength(0.03623);
            fishTileSearchRadius = ArtifactLength(0.02023);
            maxFishingPoleDistance = ArtifactLength(0.0597);
            emptySlotsSet = false;
        }

        /// <summary>
        /// Called once when the bot starts running
        /// </summary>
        protected override bool Run()
        {
            //ReadWindow();
            //bool[,] bankBooth = ColorFilter(ironFilter);
            //DebugUtilities.TestMask(Bitmap, ColorArray, ironFilter, bankBooth, "C:\\Users\\markq\\Documents\\runescape_bot\\training_pictures\\ironore\\", "ironRockPic");

            //ReadWindow();
            //bool[,] bankBooth = ColorFilter(RGBHSBRangeFactory.EmptyInventorySlot());
            //DebugUtilities.TestMask(Bitmap, ColorArray, RGBHSBRangeFactory.EmptyInventorySlot(), bankBooth);

            //ReadWindow();
            //bool[,] bankBooth = ColorFilter(FishingPoleFilter);
            //DebugUtilities.TestMask(Bitmap, ColorArray, FishingPoleFilter, bankBooth, "C:\\Users\\markq\\Documents\\rs_bot\\training_pictures\\barbarian_fishing\\", "fishingPolePic");
            //DebugUtilities.SaveImageToFile(Bitmap, "C:\\Users\\markq\\Documents\\rs_bot\\training_pictures\\barbarian_fishing\\fishing-training-1.png");
            return true;
        }

        /// <summary>
        /// Called once for each iteration of the bot
        /// </summary>
        /// <returns></returns>
        protected override bool Execute()
        {
            if (!emptySlotsSet)
            {
                Inventory.SetEmptySlots(); // this tells the inventory to record which spots are empty
                emptySlotsSet = true;
            }
            ReadWindow();
            if (!Inventory.SlotIsEmpty(Inventory.INVENTORY_COLUMNS - 1, Inventory.INVENTORY_ROWS - 1, true))
            {
                Inventory.DropInventory(false, true);
            }
            else
            {
                if (!isCurrentlyFishing())
                {
                    Blob fishLocation = StationaryLocateFishingSpot();
                    if (fishLocation != null)
                    {
                        if ((Point)fishLocation.RandomBlobPixel() != null)
                        {
                            Point fishPoint = (Point)fishLocation.RandomBlobPixel();
                            LeftClick(fishPoint.X, fishPoint.Y);
                            Mouse.RadialOffset(187, 689, 6, 223);
                            SafeWaitPlus(700, 500);
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Find all of the unmined iron ores on the screen and sorts them by proximity to the player
        /// </summary>
        /// <returns>true if any ores are located</returns>
        protected Blob StationaryLocateFishingSpot()
        {
            Blob fishLocation;
            if (LocateStationaryObject(FishTileFilter, out fishLocation, 50, 5000, 1, LocateFishingTile))
            {

            }
            //Blob rockLocation = ImageProcessing.ClosestBlob(ironBoolArray, Center, 3);
            return fishLocation;
        }

        /// <summary>
        /// Locates the closest unmined iron ore
        /// </summary>
        /// <param name="ironFilter"></param>
        /// <param name="foundObject"></param>
        /// <param name="minimumSize"></param>
        /// <returns>true if an ore rock is found</returns>
        protected bool LocateFishingTile(RGBHSBRange fishFilter, out Blob foundObject, int minimumSize)
        {
            foundObject = new Blob();
            ReadWindow();
            bool[,] fishBoolArray = ColorFilter(fishFilter);
            List<Blob> allMatches = ImageProcessing.FindBlobs(fishBoolArray);

            if (allMatches == null || allMatches.Count == 0)
            {
                return false;
            }
            allMatches.Sort(new BlobProximityComparer(Center));
            foundObject = Geometry.ClosestBlobToPoint(allMatches,Center);
            //foreach (Blob currentBlob in allMatches)
            //{
            //    if (WithinExpectedRange(currentBlob, foundObject))
            //    {
            //        foundObject.AddBlob(currentBlob);
            //    }
            //}
            return foundObject != null;
        }

        protected bool WithinExpectedRange(Blob blob1, Blob blob2)
        {
            if (Math.Sqrt((Math.Abs(blob1.Center.X - blob2.Center.X))^2 + (Math.Abs(blob1.Center.Y - blob2.Center.Y)) ^ 2) < fishTileSearchRadius)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// This method detects whether or not the player is currently still fishing based on the distance of 
        /// the closest fishing pole to the center
        /// 
        /// </summary>
        /// <returns></returns>
        protected bool isCurrentlyFishing()
        {
            ReadWindow();
            bool[,] poleBoolArray = ColorFilter(FishingPoleFilter);
            Blob closestObject;
            closestObject = ImageProcessing.ClosestBlob(poleBoolArray, Center, 7);

            if (closestObject != null)
            {
                if (Geometry.DistanceBetweenPoints(closestObject.Center, Center) <= maxFishingPoleDistance)
                {
                    return true;
                }
            }
            return false;
        }
    }
}