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
        RGBHSBRange ironFilter = RGBHSBRangeFactory.IronRock();


        public IronPowerMining(RunParams startParams) : base(startParams)
        {
            RunParams.Run = true;
            RunParams.FrameTime = 1000;
        }

        /// <summary>
        /// Called once when the bot starts running
        /// </summary>
        protected override bool Run()
        {
            ReadWindow();
            bool[,] bankBooth = ColorFilter(ironFilter);
            DebugUtilities.TestMask(Bitmap, ColorArray, ironFilter, bankBooth, "C:\\Users\\markq\\Documents\\runescape_bot\\training_pictures\\ironore\\", "ironRockPic");

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
            if (!Inventory.SlotIsEmpty(Inventory.INVENTORY_COLUMNS - 1, Inventory.INVENTORY_ROWS - 1))
            {
                Inventory.DropInventory();
            }
            else {
                Blob rockLocation = LocateUnminedOre();
                if (rockLocation != null) {
                    Point rockPoint = (Point)rockLocation.RandomBlobPixel();
                    LeftClick(rockPoint.X, rockPoint.Y);
                }
            }
            return true;
        }

        /// <summary>
        /// Find all of the unmined iron ores on the screen and sorts them by proximity to the player
        /// </summary>
        /// <returns>true if any ores are located</returns>
        protected Blob LocateUnminedOre()
        {
            ReadWindow();
            bool[,] ironBoolArray = ColorFilter(ironFilter);
            Blob rockLocation = ImageProcessing.ClosestBlob(ironBoolArray, Center, 3);
            return rockLocation;
        }
    }
}

