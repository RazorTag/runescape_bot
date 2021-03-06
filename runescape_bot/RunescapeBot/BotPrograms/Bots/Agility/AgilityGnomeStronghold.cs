﻿using RunescapeBot.Common;
using RunescapeBot.ImageTools;
using RunescapeBot.UITools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace RunescapeBot.BotPrograms
{
    class AgilityGnomeStronghold : Agility
    {
        protected const double STATIONARY_OBJECT_TOLERANCE = 10.0;
        private const int WAIT_FOR_NEXT_OBSTACLE = 10000;
        private const int WAIT_FOR_VERIFICATION = 9000;

        private RGBHSBRange LogBalance;
        private RGBHSBRange CargoNet;
        private RGBHSBRange TreeBranch;
        private RGBHSBRange TreeTrunk;
        private RGBHSBRange Tightrope;
        private RGBHSBRange DrainPipe;
        private RGBHSBRange Black;

        private int MinLogSize { get { return Screen.ArtifactArea(0.00049); } }
        private int MinCargoNetSize { get { return Screen.ArtifactArea(0.0002); } }
        private int MinTreeBranchSize { get { return Screen.ArtifactArea(0.00113); } }
        private int MinTreeTrunkSize { get { return Screen.ArtifactArea(0.000189); } }
        private int MaxTreeTrunkSize { get { return Screen.ArtifactArea(0.000378); } }
        private int MinTightropeSize { get { return Screen.ArtifactArea(0.000189); } }
        private int MinDrainPipeSize { get { return Screen.ArtifactArea(0.000566); } }


        public AgilityGnomeStronghold(RunParams startParams) : base(startParams)
        {
            GetReferenceColors();
            RunParams.ClosedChatBox = true;
        }

        protected override bool Run()
        {
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

            //PassLogBalance();
            //PassCargoNet();
            //PassTreeBranch();
            //PassTightRope();
            //PassTreeTrunk();
            //PassCargoNet();
            //PassDrainPipe();

            //VerifyClimbedUpTree();
            //VerifyClimbedDownTree();

            return true;
        }

        protected override bool Execute()
        {
            if (TryPassObstacle(PassLogBalance, VerifyPassedLogBalance)
                && TryPassObstacle(PassCargoNet, VerifyPassedCargoNetSouth)
                && TryPassObstacle(PassTreeBranch, VerifyClimbedUpTree)
                && TryPassObstacle(PassTightRope, VerifyPassedTightrope)
                && TryPassObstacle(PassTreeTrunk, VerifyClimbedDownTree)
                && TryPassObstacle(PassCargoNet, VerifyPassedCargoNetNorth)
                && TryPassObstacle(PassDrainPipe, VerifyPassedDrainPipe))
            {
                RunParams.Iterations--;
                return true;
            }
            
            //TODO recover from an unknown error state
            return false;
        }

        /// <summary>
        /// Locates and walks over the log balance obstacle
        /// </summary>
        /// <returns>true if successful</returns>
        private bool PassLogBalance()
        {
            Blob log;
            if (!Vision.LocateStationaryObject(LogBalance, out log, STATIONARY_OBJECT_TOLERANCE, WAIT_FOR_NEXT_OBSTACLE, MinLogSize, int.MaxValue, null, 2))
            {
                return false;   //unable to locate the log balance
            }
            Point logStart = log.GetTop();
            Point logEnd = log.GetBottom();
            Line logAxis = new Line(logStart, logEnd);
            Point click = logAxis.OffsetFromStart(30);
            LeftClick(click.X, click.Y, 2);

            //wait for the player to cross the log
            SafeWait(7500); //approximate milliseconds needed to go from the end of the pipe obstacle to the end of the log balance
            return true;
        }

        /// <summary>
        /// Verifies that the player has passed the log balance obstacle
        /// </summary>
        /// <returns>true if the player has passed the obstacle</returns>
        private bool VerifyPassedLogBalance()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            Blob log;

            while (watch.ElapsedMilliseconds < WAIT_FOR_VERIFICATION && !StopFlag)
            {
                if (Vision.LocateObject(LogBalance, out log, MinLogSize) && (log.Center.Y < Screen.Center.Y))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Passes a cargo net agility obstacle
        /// </summary>
        /// <returns>true if successful</returns>
        private bool PassCargoNet()
        {
            Blob cargoNet;
            if (!Vision.LocateStationaryObject(CargoNet, out cargoNet, STATIONARY_OBJECT_TOLERANCE, WAIT_FOR_NEXT_OBSTACLE, MinCargoNetSize, int.MaxValue, LocateMiddleCargoNet))
            {
                return false;   //unable to locate a set of cargo net frames
            }
            Point click = Geometry.RandomMidpoint(cargoNet.GetLeft(), cargoNet.GetRight());
            LeftClick(click.X, click.Y + 10, 4);

            SafeWait(2000); //wait for the play to climb up the net
            return true;
        }

        /// <summary>
        /// Waits for the player to pass a cargo net obstacle going south
        /// </summary>
        /// <returns>true if verified</returns>
        private bool VerifyPassedCargoNetSouth()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            Blob net;

            while (watch.ElapsedMilliseconds < WAIT_FOR_VERIFICATION && !StopFlag)
            {
                if (Vision.LocateObject(CargoNet, out net, MinCargoNetSize) && (net.Center.Y < Screen.Center.Y))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Waits for the player to pass a cargo net obstacle going north
        /// </summary>
        /// <returns>true if verified</returns>
        private bool VerifyPassedCargoNetNorth()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            Blob net;

            while (watch.ElapsedMilliseconds < WAIT_FOR_VERIFICATION && !StopFlag)
            {
                if (Vision.LocateObject(CargoNet, out net, MinCargoNetSize) && (net.Center.Y > Screen.Center.Y))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Locates the top of the frame of the middle cargo net of the closest group of three
        /// </summary>
        /// <param name="cargoNetFrameColor">defines the top of a cargo net color</param>
        /// <param name="cargoNet">returns the top of the middle cargo net</param>
        /// <param name="minimumSize">conservative size floor</param>
        /// <returns>true if a set of cargo nets is found</returns>
        private bool LocateMiddleCargoNet(ColorFilter cargoNetFrameColor, out Blob cargoNet, int minimumSize = 1, int maximumSize = int.MaxValue)
        {
            cargoNet = null;
            Screen.ReadWindow();
            bool[,] cargoNetFrame = Vision.ColorFilter(CargoNet);
            Vision.EraseClientUIFromMask(ref cargoNetFrame);
            List<Blob> cargoNetFrames = ImageProcessing.FindBlobs(cargoNetFrame, false, MinCargoNetSize);
            if (cargoNetFrames.Count < 3)
            {
                return false;
            }
            else
            {
                cargoNetFrames = ClosestFrameSet(cargoNetFrames);
                cargoNet = MiddleCargoNet(cargoNetFrames);
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
            cargoNetFrames.Sort(new BlobProximityComparer(Screen.Center));
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
            if (!Vision.LocateStationaryObject(TreeTrunk, out treeTrunk, STATIONARY_OBJECT_TOLERANCE, WAIT_FOR_NEXT_OBSTACLE, 0, int.MaxValue, FindTreeTrunk))
            {
                return false;   //unable to locate a tree trunk
            }

            Blob treeBranches;
            if (!FindTreeBranches(out treeBranches, treeTrunk.Center)) { return false; }   //didn't find tree to climb

            Point branchEnd = treeBranches.GetTop();
            Line topBranch = new Line(treeTrunk.Center, branchEnd);
            Point click = topBranch.OffsetFromEnd(0.0447 * Screen.Height);   //~45 pixels in from the end of the branch on a full HD screen
            LeftClick(click.X, click.Y);

            SafeWait(1000); //wait for the player to climb the tree
            return true;
        }

        /// <summary>
        /// Waits for the player to climb up the west tree onto the upper level
        /// </summary>
        /// <returns>true if verified</returns>
        private bool VerifyClimbedUpTree()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            bool[,] minimap;
            const double minBlackMatch = 0.66;
            double blackMatch;

            while (watch.ElapsedMilliseconds < WAIT_FOR_VERIFICATION && !StopFlag)
            {
                Screen.ReadWindow();
                minimap = Minimap.MinimapFilter(Black);
                blackMatch = ImageProcessing.FractionalMatch(minimap);
                if (blackMatch >= minBlackMatch)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Waits for the player to climb down the east tree to the ground
        /// </summary>
        /// <returns>true if verified</returns>
        private bool VerifyClimbedDownTree()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            bool[,] minimap;
            const double maxBlackMatch = 0.1;
            double blackMatch;

            while (watch.ElapsedMilliseconds < WAIT_FOR_VERIFICATION && !StopFlag)
            {
                Screen.ReadWindow();
                minimap = Minimap.MinimapFilter(Black);
                blackMatch = ImageProcessing.FractionalMatch(minimap);
                if (blackMatch <= maxBlackMatch)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Finds the closest trunk for a climbable tree
        /// </summary>
        /// <param name="trunkColor">color range defining a climbable tree trunk</param>
        /// <param name="trunk">returns a climbable tree trunk if found</param>
        /// <param name="minimumSize">not used</param>
        /// <returns>true if a climbable tree trunk is found</returns>
        private bool FindTreeTrunk(ColorFilter trunkColor, out Blob trunk, int minimumSize = 1, int maximumSize = int.MaxValue)
        {
            trunk = null;
            Screen.ReadWindow();
            bool[,] objectPixels = Vision.ColorFilter(trunkColor);
            Vision.EraseClientUIFromMask(ref objectPixels);
            List<Blob> possibleTrunks = ImageProcessing.FindBlobs(objectPixels, false, MinTreeTrunkSize, MaxTreeTrunkSize);
            if (possibleTrunks.Count == 0) {
                return false;
            }

            trunk = possibleTrunks[0];
            double rectangularity;
            double mostRectangular = Geometry.Rectangularity(trunk);
            for (int i = 1; i < possibleTrunks.Count; i++)
            {
                rectangularity = Geometry.Rectangularity(possibleTrunks[i]);
                if (rectangularity > mostRectangular)
                {
                    trunk = possibleTrunks[i];
                    mostRectangular = rectangularity;
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
        private bool FindTreeBranches(out Blob tree, Point treeTrunk)
        {
            tree = null;
            Screen.ReadWindow();
            bool[,] objectPixels = Vision.ColorFilter(TreeBranch);
            Vision.EraseClientUIFromMask(ref objectPixels);
            List<Blob> branches = ImageProcessing.FindBlobs(objectPixels, false, MinTreeBranchSize);
            if (branches.Count == 0) { return false; }

            tree = branches[0];
            double offset;
            double closestOffset = Geometry.DistanceBetweenPoints(treeTrunk, branches[0].GetBottom());
            for (int i = 1; i < branches.Count; i++)
            {
                offset = Geometry.DistanceBetweenPoints(treeTrunk, branches[i].GetBottom());
                if (offset < closestOffset)
                {
                    tree = branches[i];
                    closestOffset = offset;
                }
            }
            return true;
        }

        /// <summary>
        /// Passes a tight rope obstacle from left to right
        /// </summary>
        /// <returns>true if successful</returns>
        private bool PassTightRope()
        {
            Blob tightrope;
            if (!Vision.LocateStationaryObject(Tightrope, out tightrope, STATIONARY_OBJECT_TOLERANCE, WAIT_FOR_NEXT_OBSTACLE, MinTightropeSize))
            {
                return false;
            }
            Line tightropeAxis = new Line(tightrope.GetLeft(), tightrope.GetRight());
            Point click = tightropeAxis.OffsetFromStart(25);
            LeftClick(click.X, click.Y, 10);
            SafeWait(500);
            Mouse.RadialOffset(150, 500, 0, 360);
            SafeWait(4700);
            return true;
        }

        /// <summary>
        /// Waits for the player to cross the tightrope
        /// </summary>
        /// <returns>true if verified</returns>
        private bool VerifyPassedTightrope()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            Blob tightrope;

            while (watch.ElapsedMilliseconds < WAIT_FOR_VERIFICATION && !StopFlag)
            {
                if (Vision.LocateObject(Tightrope, out tightrope, MinTightropeSize) && (tightrope.Center.X < Screen.Center.X))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Passes a climbable tree obstacle using the topmost branch (the northern branch if the screen is at compass default
        /// </summary>
        /// <returns>true if successful</returns>
        private bool PassTreeTrunk()
        {
            Blob treeTrunk;
            if (!Vision.LocateStationaryObject(TreeTrunk, out treeTrunk, STATIONARY_OBJECT_TOLERANCE, WAIT_FOR_NEXT_OBSTACLE, 0, int.MaxValue, FindTreeTrunk))
            {
                return false;   //unable to locate a tree trunk
            }

            Point branchOffset = new Point(-Screen.ArtifactLength(0.0914), Screen.ArtifactLength(0.0177));
            Point click = Geometry.AddPoints(treeTrunk.Center, branchOffset);   //~45 pixels in from the end of the branch on a full HD screen
            LeftClick(click.X, click.Y, 6);
            SafeWait(1500); //wait for the player to climb the tree
            return true;
        }

        /// <summary>
        /// Passes through a drain pipe obstacle
        /// </summary>
        /// <returns>true if successful</returns>
        private bool PassDrainPipe()
        {
            Blob drainPipe;
            if (!Vision.LocateStationaryObject(DrainPipe, out drainPipe, STATIONARY_OBJECT_TOLERANCE, WAIT_FOR_NEXT_OBSTACLE, MinDrainPipeSize, int.MaxValue, LocateDrainPipe, 2))
            {
                return false;
            }

            LeftClick(drainPipe.Center.X, drainPipe.Center.Y, 5);
            SafeWait(7000);
            return true;
        }

        /// <summary>
        /// Finds the leftmost drain pipe
        /// </summary>
        /// <param name="drainPipeColor"></param>
        /// <param name="drainPipe"></param>
        /// <param name="minimumSize"></param>
        /// <returns></returns>
        private bool LocateDrainPipe(ColorFilter drainPipeColor, out Blob drainPipe, int minimumSize = 1, int maximumSize = int.MaxValue)
        {
            drainPipe = null;
            Screen.ReadWindow();
            bool[,] drains = Vision.ColorFilter(DrainPipe);
            List<Blob> drainPipes = ImageProcessing.FindBlobs(drains, false, MinDrainPipeSize);
            if (drainPipes.Count == 0)
            {
                return false;
            }

            drainPipes.Sort(new BlobHorizontalComparer());
            drainPipe = drainPipes[0];
            return true;
        }

        /// <summary>
        /// Waits for the player to crawl through the drain pipe
        /// </summary>
        /// <returns>true if verified</returns>
        private bool VerifyPassedDrainPipe()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            List<Blob> drainPipes;
            bool[,] drain;
            Blob lowestBlob;

            while (watch.ElapsedMilliseconds < WAIT_FOR_VERIFICATION && !StopFlag)
            {
                Screen.ReadWindow();
                drain = Vision.ColorFilter(DrainPipe);
                drainPipes = ImageProcessing.FindBlobs(drain, false, MinDrainPipeSize);
                if (drainPipes.Count > 0)
                {
                    drainPipes.Sort(new BlobVerticalComparer());
                    lowestBlob = drainPipes[drainPipes.Count - 1];
                    if (lowestBlob.Center.Y > Screen.Center.Y)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Sets the filter colors for the obstacles
        /// </summary>
        private void GetReferenceColors()
        {
            LogBalance = RGBHSBRangeFactory.LogBalance();
            CargoNet = RGBHSBRangeFactory.CargoNetFrameTop();
            TreeBranch = RGBHSBRangeFactory.GnomeTreeBranch();
            TreeTrunk = RGBHSBRangeFactory.GnomeTreeTrunk();
            Tightrope = RGBHSBRangeFactory.Tightrope();
            DrainPipe = RGBHSBRangeFactory.DrainPipe();
            Black = RGBHSBRangeFactory.Black();
        }
    }
}
