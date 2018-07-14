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
        RGBHSBRange WillowTrunk = RGBHSBRangeFactory.WillowTrunk();
        protected int MinTreeSize;
        protected List<Blob> Trees;
        protected int FailedTreeSearches;


        public WillowChopping(RunParams startParams) : base(startParams)
        {
            RunParams.Run = true;
            MinTreeSize = ArtifactArea(0.00006768);
            FailedTreeSearches = 0;
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
            if (!LocateTrees() || !ChopTree())
            {
                FailedTreeSearches++;
                if (FailedTreeSearches > 10)
                {
                    Inventory.DropInventory(false, true);
                    return false;
                }
                return true;
            }

            SafeWait(2000); //Give the player time to start the chopping animation
            WaitDuringPlayerAnimation();

            //Drop logs when inventory fills up. Use the second from bottom row to avoid looking at the Windows 10 watermark.
            if (!Inventory.SlotIsEmpty(Inventory.INVENTORY_COLUMNS - 1, Inventory.INVENTORY_ROWS - 4))
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
            Trees = LocateObjects(WillowTrunk, MinTreeSize);
            //Trees.Sort(new BlobSizeComparer());
            //Trees.Reverse();
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
                if (StopFlag) { return false; }

                click = tree.Center;
                click = Probability.GaussianCircle(click, 3);
                Mouse.Move(click.X, click.Y, RSClient);

                if (WaitForMouseOverText(RGBHSBRangeFactory.MouseoverTextStationaryObject(), 1000))
                {
                    LeftClick(click.X, click.Y, 0, 0);
                    FailedTreeSearches = 0;
                    return true;
                }
            }
            return false;
        }
    }
}
