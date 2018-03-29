using RunescapeBot.Common;
using RunescapeBot.ImageTools;
using RunescapeBot.UITools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms
{
    public class WillowChopping : BotProgram
    {
        RGBHSBRange willowTrunk = RGBHSBRangeFactory.WillowTrunk();
        protected int minTreeSize;
        protected int TreeTextChars;
        protected int TreeTextWidth;
        protected List<Blob> Trees;


        public WillowChopping(RunParams startParams) : base(startParams)
        {
            RunParams.Run = true;
            minTreeSize = ArtifactSize(0.0002);
        }

        /// <summary>
        /// Called once when the bot starts running
        /// </summary>
        protected override bool Run()
        {
            //ReadWindow();
            //DebugUtilities.SaveImageToFile(Bitmap, "C:\\Projects\\Roboport\\test_pictures\\woodcutting\\test.png");

            //ReadWindow();
            //bool[,] bankBooth = ColorFilter(willowTrunk);
            //DebugUtilities.TestMask(Bitmap, ColorArray, willowTrunk, bankBooth, "C:\\Projects\\Roboport\\test_pictures\\mask_tests\\", "willow");

            Inventory.SetEmptySlots();
            return true;
        }

        /// <summary>
        /// Called once for each iteration of the bot
        /// </summary>
        /// <returns></returns>
        protected override bool Execute()
        {
            if (!LocateTrees())
            {
                return false;
            }
            ChopTree();
            SafeWait(2000); //Give the player time to start the chopping animation
            WaitDuringPlayerAnimation();

            //Drop logs when inventory fills up
            if (!Inventory.SlotIsEmpty(Inventory.INVENTORY_COLUMNS - 1, Inventory.INVENTORY_ROWS - 1))
            {
                Inventory.DropInventory(false, true);
            }
            return true;
        }

        /// <summary>
        /// Finds all of the trees on the screen and sorts them by proximity to the player
        /// </summary>
        /// <returns>true if any trees are located</returns>
        protected bool LocateTrees()
        {
            Trees = LocateObjects(willowTrunk, minTreeSize);
            Trees.Sort(new BlobProximityComparer(Center));
            return Trees.Count > 0;
        }

        /// <summary>
        /// Tries each found tree from closest to farthest away until an actual tree is confirmed
        /// </summary>
        /// <returns>true if a tree is found and chopped, false if no tree is confirmed found</returns>
        protected bool ChopTree()
        {
            Point click;

            foreach (Blob tree in Trees)
            {
                click = tree.Center;
                click = Probability.GaussianCircle(click, 3);
                Mouse.MoveMouse(click.X, click.Y, RSClient);

                if (WaitForMouseOverText(RGBHSBRangeFactory.MouseoverTextStationaryObject(), 1000))
                {
                    LeftClick(click.X, click.Y, 0, 0);
                    return true;
                }
            }
            return false;
        }
    }
}
