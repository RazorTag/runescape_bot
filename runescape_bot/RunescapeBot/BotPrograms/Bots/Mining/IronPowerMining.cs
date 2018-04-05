using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RunescapeBot.ImageTools;
using System.Drawing;
using System.Windows.Forms;

namespace RunescapeBot.BotPrograms
{
    public class IronPowerMining : BotProgram
    {
        RGBHSBRange IronFilter = RGBHSBRangeFactory.IronRock();
        int minIronBlobPxSize;
        int missedRocks;


        public IronPowerMining(RunParams startParams) : base(startParams)
        {
            RunParams.Run = true;
            RunParams.FrameTime = 1800;
            minIronBlobPxSize = ArtifactSize(0.00025);
            missedRocks = 0;
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

            Inventory.SetEmptySlots(); // this tells the inventory to record which spots are empty
            return true;
        }

        /// <summary>
        /// Called once for each iteration of the bot
        /// </summary>
        /// <returns></returns>
        protected override bool Execute()
        {
            ReadWindow();
            if (!Inventory.SlotIsEmpty(Inventory.INVENTORY_COLUMNS - 1, Inventory.INVENTORY_ROWS - 4))
            {
                Inventory.DropInventory(false, true);
            }
            else {
                Blob rockLocation = StationaryLocateUnminedOre();
                if (rockLocation == null)
                {
                    missedRocks++;
                    if (missedRocks > 5)
                    {
                        MessageBox.Show("Unable to find any iron rocks");
                        return false;
                    }
                }
                else
                {
                    Point rockPoint = (Point)rockLocation.RandomBlobPixel();
                    LeftClick(rockPoint.X, rockPoint.Y);
                    SafeWaitPlus(1200, 100);
                    RunParams.Iterations--;
                    missedRocks = 0;
                }
            }
            return true;
        }

        /// <summary>
        /// Find all of the unmined iron ores on the screen and sorts them by proximity to the player
        /// </summary>
        /// <returns>true if any ores are located</returns>
        protected Blob StationaryLocateUnminedOre()
        {
            Blob rockLocation;
            LocateStationaryObject(IronFilter, out rockLocation, 15, 5000, minIronBlobPxSize, 5*minIronBlobPxSize, LocateUnminedOre);
            return rockLocation;
        }

        /// <summary>
        /// Locates the closest unmined iron ore
        /// </summary>
        /// <param name="ironFilter"></param>
        /// <param name="foundObject"></param>
        /// <param name="minimumSize"></param>
        /// <returns>true if an ore rock is found</returns>
        protected bool LocateUnminedOre(RGBHSBRange ironFilter, out Blob foundObject, int minimumSize, int maximumSize = int.MaxValue)
        {
            ReadWindow();
            bool[,] ironBoolArray = ColorFilter(ironFilter);
            foundObject = ImageProcessing.ClosestBlob(ironBoolArray, Center, minimumSize);
            return foundObject != null;
        }
    }
}

