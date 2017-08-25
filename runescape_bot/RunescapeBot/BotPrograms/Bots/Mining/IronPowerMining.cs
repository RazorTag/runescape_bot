using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RunescapeBot.ImageTools;
using System.Drawing;

namespace RunescapeBot.BotPrograms
{
    public class IronPowerMining : BotProgram
    {
        //protected int TreeTextChars;
        //protected int TreeTextWidth;
        //protected List<Blob> Trees;
        RGBHSBRange IronFilter = RGBHSBRangeFactory.IronRock();
        int minIronBlobPxSize; 


        public IronPowerMining(RunParams startParams) : base(startParams)
        {
            RunParams.Run = true;
            RunParams.FrameTime = 1000;
            minIronBlobPxSize = ArtifactSize(0.00025);
        }

        /// <summary>
        /// Called once when the bot starts running
        /// </summary>
        protected override bool Run()
        {
            //ReadWindow();
            //bool[,] bankBooth = ColorFilter(ironFilter);
            //DebugUtilities.TestMask(Bitmap, ColorArray, ironFilter, bankBooth, "C:\\Users\\markq\\Documents\\runescape_bot\\training_pictures\\ironore\\", "ironRockPic");

            Inventory.SetEmptySlots(); // this tells the inv to record which spots are empty
            return true;
        }

        /// <summary>
        /// Called once for each iteration of the bot
        /// </summary>
        /// <returns></returns>
        protected override bool Execute()
        {
            ReadWindow();
            if (!Inventory.SlotIsEmpty(Inventory.INVENTORY_COLUMNS - 1, Inventory.INVENTORY_ROWS - 2))
            {
                Inventory.DropInventory();
            }
            else {
                Blob rockLocation = StationaryLocateUnminedOre();
                if (rockLocation != null) {
                    Point rockPoint = (Point)rockLocation.RandomBlobPixel();
                    LeftClick(rockPoint.X, rockPoint.Y);
                    SafeWaitPlus(700, 500);
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
            if (LocateStationaryObject(IronFilter, out rockLocation, 15, 5000, minIronBlobPxSize, LocateUnminedOre))
            {

            }
            //Blob rockLocation = ImageProcessing.ClosestBlob(ironBoolArray, Center, 3);
            // 51 px 
            // 692 height 
            // (51/692)^2 
            // 0.00010650205
            return rockLocation;
        }

                    
        protected bool LocateUnminedOre(RGBHSBRange ironFilter, out Blob foundObject, int minimumSize)
        {
            ReadWindow();
            bool[,] ironBoolArray = ColorFilter(ironFilter);
            foundObject = ImageProcessing.ClosestBlob(ironBoolArray, Center, minimumSize);
            return foundObject != null;
        }
    }
}

