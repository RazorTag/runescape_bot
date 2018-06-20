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
        RGBHSBRange FishingIcon = RGBHSBRangeFactory.FishingIcon();
        int maxFishingPoleDistance;


        public BarbarianFishing(RunParams startParams) : base(startParams)
        {
            RunParams.Run = true;
            RunParams.FrameTime = 5000;
            RunParams.RunLoggedIn = true;
            maxFishingPoleDistance = ArtifactLength(0.0597);
            emptySlotsSet = false;
        }

        /// <summary>
        /// Called once when the bot starts running
        /// </summary>
        protected override bool Run()
        {
            //ReadWindow();
            //DebugUtilities.SaveImageToFile(Bitmap, "C:\\Projects\\Roboport\\test_pictures\\barbarian_fishing\\test.png");

            //ReadWindow();
            //bool[,] bankBooth = ColorFilter(ironFilter);
            //DebugUtilities.TestMask(Bitmap, ColorArray, ironFilter, bankBooth, "C:\\Users\\markq\\Documents\\runescape_bot\\training_pictures\\ironore\\", "ironRockPic");

            //ReadWindow();
            //bool[,] bankBooth = ColorFilter(RGBHSBRangeFactory.FishingTile());
            //DebugUtilities.TestMask(Bitmap, ColorArray, RGBHSBRangeFactory.FishingTile(), bankBooth);

            //ReadWindow();
            //bool[,] bankBooth = ColorFilter(FishingPoleFilter);
            //DebugUtilities.TestMask(Bitmap, ColorArray, FishingPoleFilter, bankBooth, "C:\\Users\\markq\\Documents\\rs_bot\\training_pictures\\barbarian_fishing\\", "fishingPolePic");
            //DebugUtilities.SaveImageToFile(Bitmap, "C:\\Users\\markq\\Documents\\rs_bot\\training_pictures\\barbarian_fishing\\fishing-training-1.png");

            //ReadWindow();
            //bool[,] thing = ColorFilter(FishingIcon);
            //DebugUtilities.TestMask(Bitmap, ColorArray, FishingIcon, thing, "C:\\Projects\\Roboport\\test_pictures\\mask_tests\\", "fishingIcon");

            //MoveToNewFishingSpot();

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
                for (int x = 0; x < Inventory.INVENTORY_COLUMNS; x++)
                {
                    for (int y = Inventory.INVENTORY_ROWS - 2; y < Inventory.INVENTORY_ROWS; y++)
                    {
                        Inventory.SetEmptySlot(x, y, false);
                    }
                }
                emptySlotsSet = true;
            }
            ReadWindow();
            if (!Inventory.SlotIsEmpty(Inventory.INVENTORY_COLUMNS - 1, Inventory.INVENTORY_ROWS - 3, true))
            {
                Inventory.DropInventory(false, true);
            }
            else
            {
                if (!IsCurrentlyFishing())
                {
                    Blob fishLocation = LocateClosestObject(FishTileFilter);
                    if (fishLocation != null)
                    {
                        if (fishLocation.Center.X != 0 && fishLocation.Center.Y != 0)
                        {
                            Point fishPoint = (Point)fishLocation.RandomBlobPixel();
                            LeftClick(fishPoint.X, fishPoint.Y);
                            Mouse.RadialOffset(187, 689, 6, 223);
                            SafeWaitPlus(8000, 1200);
                        }
                    }
                    else if (!MoveToNewFishingSpot())
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Moves the player to a new fishing spot using the fishing icon(s) on the minimap
        /// </summary>
        /// <returns>true if a new fishing spot is found</returns>
        protected bool MoveToNewFishingSpot()
        {
            Point offset;
            bool[,] fishingMap = Minimap.MinimapFilter(FishingIcon, out offset);
            List<Blob> fishingSpots = ImageProcessing.FindBlobs(fishingMap, false, 11, 51);

            Point minimapCenter = Minimap.Center;
            foreach (Blob fishingSpot in fishingSpots)
            {   //new fishing spot cannot be more than 30 pixels right of center
                if (fishingSpot.Center.X - minimapCenter.X > 30)
                {
                    fishingSpots.Remove(fishingSpot);
                }
            }

            Blob newFishingSpot = Geometry.FarthestBlobFromPoint(fishingSpots, minimapCenter);
            if (newFishingSpot == null)
            {
                return false;
            }
            Point click = Geometry.AddPoints(offset, newFishingSpot.Center);
            LeftClick(click.X, click.Y, 3);
            SafeWait(8000);

            return true;
        }

        /// <summary>
        /// Locates the closest unmined iron ore
        /// </summary>
        /// <param name="ironFilter"></param>
        /// <param name="foundObject"></param>
        /// <param name="minimumSize"></param>
        /// <returns>true if an ore rock is found</returns>
        protected bool LocateFishingTile(ColorFilter fishFilter, out Blob foundObject, int minimumSize, int maximumSize = int.MaxValue)
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
            foundObject = Geometry.ClosestBlobToPoint(allMatches, Center);

            return foundObject != null;
        }

        /// <summary>
        /// This method detects whether or not the player is currently still fishing based on the distance of 
        /// the closest fishing pole to the center
        /// 
        /// </summary>
        /// <returns></returns>
        protected bool IsCurrentlyFishing()
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