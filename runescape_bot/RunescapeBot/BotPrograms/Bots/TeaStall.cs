using RunescapeBot.ImageTools;
using RunescapeBot.UITools;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace RunescapeBot.BotPrograms
{
    public class TeaStall : BotProgram
    {
        private const int FAIL_LIMIT = 5;
        private Point TeaInventorySlot;
        private ColorRange TeaStallRoof;
        private ColorRange MouseOverText;
        private int minTeaStallSize;
        private int FailedRuns;

        public TeaStall(RunParams startParams) : base(startParams)
        {
            RunParams.FrameTime = 0;
            GetReferenceColors();
        }

        /// <summary>
        /// Determines which inventory slot the teas will be picked up into
        /// </summary>
        protected override bool Run()
        {
            //ReadWindow();
            //bool[,] mask = ColorFilter(TeaStallRoof);
            //DebugUtilities.TestMask(Bitmap, ColorArray, TeaStallRoof, mask, "C:\\Projects\\Roboport\\test_pictures\\mask_tests\\", "teaStall");

            //SafeWait(3000);
            //ReadWindow();
            //bool[,] mask = ColorFilter(MouseOverText);
            //DebugUtilities.TestMask(Bitmap, ColorArray, MouseOverText, mask, "C:\\Projects\\Roboport\\test_pictures\\mask_tests\\", "mouseoverText");


            minTeaStallSize = ArtifactSize(0.0015);
            Point? teaSlot = Inventory.FirstEmptySlot();
            if (teaSlot == null)
            {
                MessageBox.Show("Clear at least one inventory slot for the stolen tea");
                return false;
            }
            TeaInventorySlot = (Point)teaSlot;
            FailedRuns = 0;
            return true;
        }


        protected override bool Execute()
        {
            if (!ReadWindow()) { return false; }
            Blob teaStall;
            if (!LocateObject(TeaStallRoof, out teaStall, minTeaStallSize))
            {
                MessageBox.Show("Unable to locate a tea stall");
                return false;
            }
            Point click = (Point) teaStall.RandomBlobPixel();
            Mouse.MoveMouse(click.X, click.Y, RSClient);
            SafeWait(2000);
            if (!WaitForMouseOverText(MouseOverText))
            {
                return (++FailedRuns < FAIL_LIMIT);
            }
            SafeWaitPlus(0, 150);
            LeftClick(click.X, click.Y);

            SafeWait(1000);
            Stopwatch watch = new Stopwatch();
            watch.Start();
            while (Inventory.SlotIsEmpty(TeaInventorySlot.X, TeaInventorySlot.Y, true, false))
            {
                SafeWait(200);
                if (watch.ElapsedMilliseconds > 5000)
                {
                    return (++FailedRuns < FAIL_LIMIT);
                }
            }
            Inventory.DropItem(TeaInventorySlot.X, TeaInventorySlot.Y, false, new int[1] { 0 });

            FailedRuns = 0;
            RunParams.Iterations--;
            return true;
        }


        private void GetReferenceColors()
        {
            TeaStallRoof = RGBHSBRanges.TeaStallRoof();
            MouseOverText = RGBHSBRanges.MouseoverTextStationaryObject();
        }
    }
}
