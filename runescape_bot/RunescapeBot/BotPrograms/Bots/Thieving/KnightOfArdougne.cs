using RunescapeBot.Common;
using RunescapeBot.ImageTools;
using RunescapeBot.UITools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms
{
    public class KnightOfArdougne : BotProgram
    {
        protected RGBHSBRange KnightPurple = RGBHSBRangeFactory.KnightPurple();
        protected RGBHSBRange NPCMouseover = RGBHSBRangeFactory.MouseoverTextNPC();
        protected const int PICKPOCKET_TIME = 2 * BotRegistry.GAME_TICK;
        protected const int NPCClickRandomization = 5;
        protected int FailedCloakSearches;
        protected int MinPurpleCloakSize;
        protected int KnightSearchRadius;
        protected int GridSquareHeight;
        protected Point BlindSpot;  //first adjacent grid square to blind search

        public KnightOfArdougne(RunParams startParams) : base(startParams)
        {
            RunParams.AutoEat = true;
            RunParams.StartEatingBelow = 0.5;
            RunParams.StopEatingAbove = 0.8;
            FailedCloakSearches = 0;
            MinPurpleCloakSize = ArtifactArea(0.00002);
            KnightSearchRadius = ArtifactArea(0.0003);
            GridSquareHeight = ArtifactLength(0.055);
            BlindSpot = new Point(0, 0);
        }

        protected override bool Run()
        {
            //ReadWindow();
            //DebugUtilities.SaveImageToFile(Bitmap);

            //ReadWindow();
            //bool[,] thing = ColorFilter(KnightPurple);
            //DebugUtilities.TestMask(Bitmap, ColorArray, KnightPurple, thing, "C:\\Projects\\Roboport\\test_pictures\\mask_tests\\", "knightPurple");

            //ReadWindow();
            //AdjustLocation(0, -1);

            SetFoodSlots();

            return true;
        }

        protected override bool Execute()
        {
            if (!ManageHitpoints(true))
            {
                Logout();
                return false;
            }

            if (!ClickKnight(50) || StopFlag)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Clicks on a knight 10 times in short succession
        /// </summary>
        /// <param name="clickInterval">target time between clicks in milliseconds</param>
        /// <param name="clicks">number of pickpocket clicks to perform</param>
        /// <returns>true if a knight is clicked on</returns>
        protected bool ClickKnight(int clicks)
        {
            Stopwatch watch = new Stopwatch();
            Blob purpleCloak;

            for (int i = 0; i < clicks; i++)
            {
                watch.Restart();

                if (StopFlag) { return false; }
                if (LocateStationaryObject(KnightPurple, out purpleCloak, ArtifactLength(0.015), 10000, MinPurpleCloakSize, int.MaxValue, LocateClosestObject, 1))
                {
                    if (CheckPosition(purpleCloak.Center))
                    {
                        if (StopFlag) { return false; }
                        if (MouseOver(purpleCloak.Center, NPCMouseover, true, 0))
                        {
                            FailedCloakSearches = 0;
                        }
                        else
                        {
                            FailedCloakSearches++;
                        }

                        if (SafeWait(Math.Max(0, PICKPOCKET_TIME - watch.ElapsedMilliseconds), 200))
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    BlindSearch();
                }
            }

            return FailedCloakSearches < 300;
        }

        /// <summary>
        /// Makes sure that the player is north of the knight
        /// </summary>
        /// <returns>false if the player's position needs to be adjusted</returns>
        protected bool CheckPosition(Point knight)
        {
            if (knight.Y - Center.Y < GridSquareHeight / 2)
            {
                AdjustLocation(0, -1);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Tries to find a knight to click on without being able to see a purple cloak.
        /// Adjust position if the knight is found on the same grid square row as the player.
        /// </summary>
        /// <returns></returns>
        protected bool BlindSearch()
        {
            Point guess = new Point(Center.X + (BlindSpot.X * GridSquareHeight), Center.Y + (BlindSpot.Y * GridSquareHeight));
            if (MouseOver(guess, NPCMouseover, true, NPCClickRandomization))
            {
                return true;
            }
            
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (StopFlag) { return false; }

                    guess = new Point(Center.X + (x * GridSquareHeight), Center.Y + (y * GridSquareHeight));
                    if (MouseOver(guess, NPCMouseover, true, NPCClickRandomization))
                    {
                        BlindSpot = new Point(x, y);
                        return true;
                    }
                }
            }

            FailedCloakSearches++;
            return false;
        }

        /// <summary>
        /// Moves the player 1 grid square
        /// </summary>
        /// <param name="right">number of grid squares to move east</param>
        /// <param name="down">number of grid squares to move south</param>
        protected void AdjustLocation(int east, int south)
        {
            Point click = Center;
            click.X += east * GridSquareHeight;
            click.Y += south * GridSquareHeight;
            LeftClick(click.X, click.Y, 5);
            SafeWait(3 * BotRegistry.GAME_TICK);
            FailedCloakSearches++;
        }

        /// <summary>
        /// Makes a stack of inventory slots with food in them
        /// </summary>
        protected override void SetFoodSlots()
        {
            FoodSlots = new Queue<int>();
            for (int i = 0; i < Inventory.INVENTORY_CAPACITY - 1; i++)    //leave the last inventory spot for stolen coins
            {
                if (!Inventory.SlotIsEmpty(i))
                {
                    FoodSlots.Enqueue(i);
                }
            }
        }
    }
}
