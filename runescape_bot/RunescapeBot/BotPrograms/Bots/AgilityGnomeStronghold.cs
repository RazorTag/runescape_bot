using RunescapeBot.Common;
using RunescapeBot.ImageTools;
using System.Drawing;

namespace RunescapeBot.BotPrograms
{
    class AgilityGnomeStronghold : BotProgram
    {
        protected const double STATIONARY_OBJECT_TOLERANCE = 15.0;
        private const int WAIT_FOR_NEXT_OBSTACLE = 15000;
        private ColorRange LogBalance;

        public AgilityGnomeStronghold(StartParams startParams) : base(startParams)
        {
            RunParams.Run = true;
            GetReferenceColors();
        }

        protected override void Run()
        {
            //ReadWindow();
            //bool[,] logBalance = ColorFilter(LogBalance);
            //DebugUtilities.TestMask(Bitmap, ColorArray, LogBalance, logBalance, "C:\\Projects\\Roboport\\test_pictures\\mask_tests\\", "logBalance");
        }

        protected override bool Execute()
        {
            PassLogBalance();
            PassCargoNet();
            PassTreeBranch();
            PassTightRope();
            PassTreeBranch();
            PassCargoNet();
            PassDrainPipe();

            return true;
        }

        /// <summary>
        /// Locates and walks over the log balance obstacle
        /// </summary>
        /// <returns>true if successful</returns>
        private bool PassLogBalance()
        {
            const int logBalanceTime = 8000; //approximate milliseconds needed to go from the end of the pipe obstacle to the end of the log balance

            Blob log;
            if (!LocateStationaryObject(LogBalance, out log, STATIONARY_OBJECT_TOLERANCE, WAIT_FOR_NEXT_OBSTACLE, 5000))
            {
                return false;   //unable to locate the log balance
            }
            Point logStart = log.GetTop();
            Point logEnd = log.GetBottom();
            Line logAxis = new Line(logStart, logEnd);
            Point click = logAxis.OffsetFromStart(30);
            LeftClick(click.X, click.Y, 2);

            SafeWait(logBalanceTime);   //wait for the player to cross the log
            return true;
        }


        private bool PassCargoNet()
        {
            return false;
        }


        private bool PassTreeBranch()
        {
            return false;
        }


        private bool PassTightRope()
        {
            return false;
        }


        private bool PassDrainPipe()
        {
            return false;
        }

        /// <summary>
        /// Sets the filter colors for the obstacles
        /// </summary>
        private void GetReferenceColors()
        {
            LogBalance = ColorFilters.LogBalance();
        }
    }
}
