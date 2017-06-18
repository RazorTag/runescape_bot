using RunescapeBot.Common;
using RunescapeBot.ImageTools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace RunescapeBot.BotPrograms
{
    class AgilityGnomeStronghold : BotProgram
    {
        protected const double STATIONARY_OBJECT_TOLERANCE = 15.0;
        private const int WAIT_FOR_NEXT_OBSTACLE = 15000;
        private ColorRange LogBalance;
        private ColorRange CargoNet;
        private ColorRange TreeBranch;
        private ColorRange TreeTrunk;
        private ColorRange Tightrope;
        private ColorRange DrainPipe;

        public AgilityGnomeStronghold(StartParams startParams) : base(startParams)
        {
            RunParams.Run = true;
            GetReferenceColors();
        }

        protected override void Run()
        {
            //PassLogBalance();
            //PassCargoNet();
            //PassTreeBranch();
            //PassTightRope();
            //PassTreeBranch();
            //PassCargoNet();
            //PassDrainPipe();

            //ReadWindow();
            //bool[,] logBalance = ColorFilter(LogBalance);
            //DebugUtilities.TestMask(Bitmap, ColorArray, LogBalance, logBalance, "C:\\Projects\\Roboport\\test_pictures\\mask_tests\\", "logBalance");

            //ReadWindow();
            //bool[,] cargoNet = ColorFilter(CargoNet);
            //DebugUtilities.TestMask(Bitmap, ColorArray, CargoNet, cargoNet, "C:\\Projects\\Roboport\\test_pictures\\mask_tests\\", "cargoNet");

            //ReadWindow();
            //bool[,] treeBranch = ColorFilter(TreeBranch);
            //DebugUtilities.TestMask(Bitmap, ColorArray, TreeBranch, treeBranch, "C:\\Projects\\Roboport\\test_pictures\\mask_tests\\", "treeBranch");

            //ReadWindow();
            //bool[,] treeTrunk = ColorFilter(TreeTrunk);
            //DebugUtilities.TestMask(Bitmap, ColorArray, TreeTrunk, treeTrunk, "C:\\Projects\\Roboport\\test_pictures\\mask_tests\\", "treeTrunk");
            //Blob trunk;
            //FindTreeTrunk(TreeTrunk, out trunk);

            //ReadWindow();
            //bool[,] tightrope = ColorFilter(Tightrope);
            //DebugUtilities.TestMask(Bitmap, ColorArray, Tightrope, tightrope, "C:\\Projects\\Roboport\\test_pictures\\mask_tests\\", "tightrope");

            //ReadWindow();
            //bool[,] drainPipe = ColorFilter(DrainPipe);
            //DebugUtilities.TestMask(Bitmap, ColorArray, DrainPipe, drainPipe, "C:\\Projects\\Roboport\\test_pictures\\mask_tests\\", "drainPipe");
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
            Blob log;
            if (!LocateStationaryObject(LogBalance, out log, STATIONARY_OBJECT_TOLERANCE, WAIT_FOR_NEXT_OBSTACLE, 500))
            {
                return false;   //unable to locate the log balance
            }
            Point logStart = log.GetTop();
            Point logEnd = log.GetBottom();
            Line logAxis = new Line(logStart, logEnd);
            Point click = logAxis.OffsetFromStart(30);
            LeftClick(click.X, click.Y, 2);

            //wait for the player to cross the log
            SafeWait(8000); //approximate milliseconds needed to go from the end of the pipe obstacle to the end of the log balance
            return true;
        }

        /// <summary>
        /// Passes a cargo net agility obstacle
        /// </summary>
        /// <returns>true if successful</returns>
        private bool PassCargoNet()
        {
            Blob cargoNet;
            if (!LocateStationaryObject(CargoNet, out cargoNet, STATIONARY_OBJECT_TOLERANCE, WAIT_FOR_NEXT_OBSTACLE, 50))
            {
                return false;   //unable to locate a cargo net frame
            }
            bool[,] cargoNetFrame = ColorFilter(CargoNet);
            EraseClientUIFromMask(ref cargoNetFrame);
            int minSize = ArtifactSize(0.0002);
            List<Blob> cargoNetFrames = ImageProcessing.FindBlobs(cargoNetFrame, false, minSize);
            if (cargoNetFrames.Count < 3)
            {
                return false;
            }
            else
            {
                cargoNetFrames = ClosestFrameSet(cargoNetFrames);
                Blob middleFrame = MiddleCargoNet(cargoNetFrames);
                Line middleFrameLine = new Line(middleFrame.GetLeft(), middleFrame.GetRight());
                Point click = middleFrameLine.OffsetFromStart(RNG.NextDouble());
                LeftClick(click.X, click.Y, 4);

                SafeWait(3300); //wait for the play to climb up the net
                return true;
            }
        }

        /// <summary>
        /// Finds the closest three blobs from a list of blobs
        /// </summary>
        /// <param name="cargoNetFrames">list of cargo net frames to search through</param>
        /// <returns>a list of the closest three blobs from cargoNetFrames</returns>
        private List<Blob> ClosestFrameSet(List<Blob> cargoNetFrames)
        {
            cargoNetFrames.Sort(new BlobProximityComparer(Center));
            List<Blob> closestFrames = new List<Blob>();
            for (int i = 0; i < 3; i++)
            {
                closestFrames.Add(cargoNetFrames[i]);
            }
            return closestFrames;
        }

        /// <summary>
        /// Chooses the blob out of the first three in the list that is in the middle horizontally
        /// </summary>
        /// <param name="cargoNetFrames">list of cargo net frame tops</param>
        /// <returns>the horizontally middle blod of the first three</returns>
        private Blob MiddleCargoNet(List<Blob> cargoNetFrames)
        {
            cargoNetFrames.Sort(new BlobHorizontalComparer());
            return cargoNetFrames[1];
        }

        /// <summary>
        /// Passes a climbable tree obstacle using the topmost branch (the northern branch if the screen is at compass default
        /// </summary>
        /// <returns>true if successful</returns>
        private bool PassTreeBranch()
        {
            Blob treeTrunk;
            if (!LocateStationaryObject(TreeTrunk, out treeTrunk, STATIONARY_OBJECT_TOLERANCE, WAIT_FOR_NEXT_OBSTACLE, 15, FindTreeTrunk))
            {
                return false;   //unable to locate a tree trunk
            }

            Blob treeBranches;
            if (!FindTreeBranches(TreeBranch, out treeBranches)) { return false; }   //didn't find tree to climb

            Line topBranch = new Line(treeTrunk.Center, treeBranches.GetTop());
            Point click = topBranch.OffsetFromEnd(0.0447 * ScreenHeight);   //~45 pixels in from the end of the branch on a full HD screen
            LeftClick(click.X, click.Y, 2);

            SafeWait(2000); //wait for the player to climb the tree
            return true;
        }

        /// <summary>
        /// Finds the closest trunk for a climbable tree
        /// </summary>
        /// <param name="trunkColor">color range defining a climbable tree trunk</param>
        /// <param name="trunk">returns a climbable tree trunk if found</param>
        /// <param name="minimumSize">not used</param>
        /// <returns>true if a climbable tree trunk is found</returns>
        private bool FindTreeTrunk(ColorRange trunkColor, out Blob trunk, int minimumSize = 1)
        {
            trunk = null;
            ReadWindow();
            bool[,] objectPixels = ColorFilter(trunkColor);
            EraseClientUIFromMask(ref objectPixels);
            minimumSize = ArtifactSize(0.00009);
            int maximumSize = ArtifactSize(0.00015);
            List<Blob> branches = ImageProcessing.FindBlobs(objectPixels, false, minimumSize, maximumSize);
            if (branches.Count == 0) { return false; }

            trunk = branches[0];
            double offset;
            double closestOffset = Geometry.DistanceBetweenPoints(Center, trunk.Center);
            for (int i = 1; i < branches.Count; i++)
            {
                offset = Geometry.DistanceBetweenPoints(Center, branches[i].Center);
                if (offset < closestOffset)
                {
                    trunk = branches[i];
                    closestOffset = offset;
                }
            }
            return true;
        }

        /// <summary>
        /// Finds the closest climbable tree
        /// </summary>
        /// <param name="treeColor">color range defining tree branches</param>
        /// <param name="tree">returns the tree blob if founf</param>
        /// <param name="minimumSize">not used</param>
        /// <returns>true if a set of branches is found</returns>
        private bool FindTreeBranches(ColorRange treeColor, out Blob tree, int minimumSize = 1)
        {
            tree = null;
            ReadWindow();
            bool[,] objectPixels = ColorFilter(treeColor);
            EraseClientUIFromMask(ref objectPixels);
            minimumSize = ArtifactSize(0.0045); //converts fraction of screen to pixels
            List<Blob> branches = ImageProcessing.FindBlobs(objectPixels, false, minimumSize);
            if (branches.Count == 0) { return false; }

            tree = branches[0];
            double offset;
            double closestOffset = Geometry.DistanceBetweenPoints(Center, tree.Center);
            for (int i = 1; i < branches.Count; i++)
            {
                offset = Geometry.DistanceBetweenPoints(Center, branches[i].Center);
                if (offset < closestOffset)
                {
                    tree = branches[i];
                    closestOffset = offset;
                }
            }
            return true;
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
            CargoNet = ColorFilters.CargoNetFrameTop();
            TreeBranch = ColorFilters.GnomeTreeBranch();
            TreeTrunk = ColorFilters.GnomeTreeTrunk();
            Tightrope = ColorFilters.Tightrope();
            DrainPipe = ColorFilters.DrainPipe();
        }
    }
}
