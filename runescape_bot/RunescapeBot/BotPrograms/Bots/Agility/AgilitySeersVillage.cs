﻿using RunescapeBot.BotPrograms.Popups;
using RunescapeBot.ImageTools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms
{
    public class AgilitySeersVillage : Agility
    {
        RGBHSBRange BankWall = RGBHSBRangeFactory.SeersBankWindowSill();
        RGBHSBRange WhiteFlag = RGBHSBRangeFactory.WhiteFlag();
        RGBHSBRange Tightrope = RGBHSBRangeFactory.Tightrope();
        RGBHSBRange MarkOfGraceBackground = RGBHSBRangeFactory.MarkOfGraceYellow();
        RGBHSBRange MarkOfGraceFigure = RGBHSBRangeFactory.MarkOfGraceRed();
        RGBHSBRange AgilityIcon = RGBHSBRangeFactory.AgilityIconBlack();
        RGBHSBRange StationaryObjectText = RGBHSBRangeFactory.MouseoverTextStationaryObject();

        protected int FailureLimit;
        protected bool[,] EmptySlots;
        List<Tuple<BasicAction, BasicAction>> Obstacles;
        protected int FailedCircuits;
        protected Point FoodBankSlot;

        public AgilitySeersVillage(RunParams startParams) : base(startParams)
        {
            RunParams.StartEatingBelow = 0.4;
            RunParams.StopEatingAbove = 0.9;
            RunParams.ClosedChatBox = true;

            MaxPassObstacleTries = 1;
            FailedCircuits = 0;
            FoodBankSlot = new Point(7, 0);

            Obstacles = new List<Tuple<BasicAction, BasicAction>>();
            Obstacles.Add(new Tuple<BasicAction, BasicAction>(ClimbBank, null));
            Obstacles.Add(new Tuple<BasicAction, BasicAction>(JumpToTightrope, null));
            Obstacles.Add(new Tuple<BasicAction, BasicAction>(CrossTightrope, null));
            Obstacles.Add(new Tuple<BasicAction, BasicAction>(JumpToLadderRoof, null));
            Obstacles.Add(new Tuple<BasicAction, BasicAction>(JumpToChurchRoof, null));
            Obstacles.Add(new Tuple<BasicAction, BasicAction>(JumpOffTreeRoof, null));

            FailureLimit = 12;
        }

        protected override bool Run()
        {
            #region debugging

            //ReadWindow();
            //DebugUtilities.SaveImageToFile(Bitmap);

            //MaskTest(BankWall);
            //MaskTest(Tightrope);
            //MaskTest(MarkOfGraceBackground, "MarkOfGraceBackground");
            //MaskTest(MarkOfGraceFigure, "MarkOfGraceFigure");
            //MaskTest(RGBHSBRangeFactory.BankIconDollar(), "bankIcon");
            //MaskTest(RGBHSBRangeFactory.ChatBoxBackground(), "chatBoxBackground");

            //ClimbBank();
            //JumpToTightrope();
            //CrossTightrope();
            //JumpToLadderRoof();
            //JumpToTreeRoof();
            //JumpOffTreeRoof();
            //ScanForMarkOfGrace();

            //Inventory.StandardTeleport(Inventory.StandardTeleports.Camelot);
            //Inventory.RightClickInventoryOption(1, 0, 5);

            #endregion

            ChatBox(true, false);  //Make sure that the user remembered to close the chat box.

            return true;
        }

        protected override bool Execute()
        {
            Screen.ReadWindow();

            if ((Minimap.Hitpoints() < RunParams.StartEatingBelow) && !BankAndHeal()) //Heal if hitpoints are low
            {
                return false;   //Quit immediately if we are unable to restore hitpoints
            }

            if (TryPassObstacles(Obstacles))
            {
                RunParams.Iterations--;
                FailedCircuits = 0;
            }
            else
            {
                FailedCircuits++;
            }

            if (!Inventory.StandardTeleport(Inventory.StandardTeleports.Camelot))
            {
                SafeWait(3000);
                if (!Inventory.StandardTeleport(Inventory.StandardTeleports.Camelot, true, true))
                {
                    return false;
                }
            }

            return FailedCircuits < FailureLimit;
        }

        /// <summary>
        /// Makes a queue of inventory slots with food in them in the order that they should be eaten
        /// </summary>
        protected override void SetFoodSlots()
        {
            FoodSlots = new Queue<int>();
            Inventory.SetEmptySlots();
            EmptySlots = Inventory.GetEmptySlots;
            Point inventorySlot;
            for (int i = 0; i < EmptySlots.Length - 8; i++) //don't touch the bottom 2 rows
            {
                inventorySlot = Inventory.InventoryIndexToCoordinates(i);
                if (EmptySlots[inventorySlot.X, inventorySlot.Y])
                {
                    FoodSlots.Enqueue(i);
                }
            }
        }

        /// <summary>
        /// Deposits leftover food into the bank. Assumes that the bank is open. Does not close the bank.
        /// </summary>
        protected void DepositFood(Bank bank)
        {
            Point slot;
            for (int i = 0; i < EmptySlots.Length; i++)
            {
                slot = Inventory.InventoryIndexToCoordinates(i);
                if (EmptySlots[slot.X, slot.Y] && !Inventory.SlotIsEmpty(i, false, false))
                {
                    bank.DepositAll(slot);
                    SafeWait(3 * BotRegistry.GAME_TICK);
                    Screen.ReadWindow();
                }
            }
        }

        /// <summary>
        /// Restore hitpoints at the Seers' Village bank and reset location to the Seers' bank teleport before continuing
        /// </summary>
        /// <returns></returns>
        private bool BankAndHeal()
        {
            Bank bank;
            SetFoodSlots(); //Record which inventory slots are currenty empty and will be occupied by food
            if (!Banking.MoveToBank(5000) || !Banking.OpenBank(out bank))
            {
                return false;
            }
            bank.WithdrawAll(FoodBankSlot.X, FoodBankSlot.Y);
            bank.Close();
            if (!ManageHitpoints() || !Banking.OpenBank(out bank))  //Eat food from initially empty inventory slots until we have very high health.
            {
                return false;
            }
            DepositFood(bank);
            bank.Close();

            if (!Inventory.StandardTeleport(Inventory.StandardTeleports.Camelot))
            {
                return Inventory.StandardTeleport(Inventory.StandardTeleports.Camelot, true, true);
            }
            return true;
        }

        /// <summary>
        /// Clicks on the window that starts the Seers' Village agility course
        /// </summary>
        /// <returns>true if successful</returns>
        private bool ClimbBank()
        {
            int left = Screen.Center.X;
            int right = Screen.Width - 1;
            int top = 0;
            int bottom = Screen.Center.Y;
            List<Blob> possibleBankWalls = Vision.LocateObjects(BankWall, left, right, top, bottom, true, Screen.ArtifactArea(0.00004), Screen.ArtifactArea(0.0004));    //ex 0.0000803, 0.0004
            Point? expectedWallLocation = ExpectedBankWallLocation();
            if (expectedWallLocation == null)
            {
                return false;
            }

            possibleBankWalls.Sort(new BlobProximityComparer((Point)expectedWallLocation));
            if (HandEye.MouseOver(possibleBankWalls, StationaryObjectText, true, 6, 1600))
            {
                SafeWait(4500);
                MoveMouse(Screen.Center.X - Screen.ArtifactLength(0.511), Screen.Center.Y - Screen.ArtifactLength(0.065));
                return true;
            }
            return false;
        }

        /// <summary>
        /// Determines approximately where the bank wall should be based on the location of the agility course icon on the minimap
        /// </summary>
        /// <returns></returns>
        private Point? ExpectedBankWallLocation()
        {
            Blob agilityIcon = Minimap.LocateObject(AgilityIcon, Screen.ArtifactArea(0.00002), Screen.ArtifactArea(0.00008), false);   //ex 0.00004
            if (agilityIcon == null || agilityIcon.Size == 0)
            {
                return null;
            }
            Point agilityIconLocation = agilityIcon.Center;
            agilityIconLocation.Y -= 5;
            Point bankWall = Minimap.ExtrapolateMinimapToGame(agilityIconLocation);
            return bankWall;
            
        }

        /// <summary>
        /// Jumps from the bank roof to the roof at the start of the tightrope and waits for the player to come to a stop.
        /// </summary>
        /// <returns>true if successful</returns>
        private bool JumpToTightrope()
        {
            Blob whiteFlag;
            if (Vision.LocateStationaryObject(WhiteFlag, out whiteFlag, 20, 7000, Screen.ArtifactArea(0.000065), Screen.ArtifactArea(0.000260))) //ex 0.000130
            {
                ScanForMarkOfGrace();
                Point expectedLocation = new Point(whiteFlag.Center.X - Screen.ArtifactLength(0.337), whiteFlag.Center.Y + Screen.ArtifactLength(0.0688));
                if (HandEye.MouseOverStationaryObject(new Blob(expectedLocation), true, 25, 3000))
                {
                    SafeWait(5500);
                    MoveMouse(Screen.Center.X - Screen.ArtifactLength(0.207), Screen.Center.Y + Screen.ArtifactLength(0.34));
                    Vision.WaitDuringPlayerAnimation(4000);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Crosses the tightrope
        /// </summary>
        /// <returns>true if successful</returns>
        private bool CrossTightrope()
        {
            Blob tightrope;
            ScanForMarkOfGrace(false);
            
            for (int i = 0; i < 3; i++) //finding the tightrope and failing the mouseover probably means that another player was blocking view of the tightrope during the screen capture
            {
                if (Vision.LocateObject(Tightrope, out tightrope, Screen.ArtifactArea(0.00009), Screen.ArtifactArea(0.714)))    //ex 0.000357
                {
                    Point tightropeStart = tightrope.GetTop();
                    Point hitboxCenter = new Point(tightropeStart.X, tightropeStart.Y - Screen.ArtifactLength(0.0269));
                    if (HandEye.MouseOverStationaryObject(new Blob(hitboxCenter), true, 10, 3000))
                    {
                        SafeWait(8000);
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// Jumps from the end of the tightrope roof to the ladder roof
        /// </summary>
        /// <returns>true if successful</returns>
        private bool JumpToLadderRoof()
        {
            Point hitboxCenter = new Point(Screen.Center.X, Screen.Center.Y + Screen.ArtifactLength(0.283));
            Blob hitboxBlob = new Blob(hitboxCenter);
            if (HandEye.MouseOverStationaryObject(hitboxBlob, false, 10, 4000))
            {
                ScanForMarkOfGrace();
                if (HandEye.MouseOverStationaryObject(hitboxBlob, true, 10, 3000))
                {
                    SafeWait(3500);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Jumps from the ladder roof to the church roof
        /// </summary>
        /// <returns>true if successful</returns>
        private bool JumpToChurchRoof()
        {
            Point hitboxCenter = new Point(Screen.Center.X - Screen.ArtifactLength(0.498), Screen.Center.Y + Screen.ArtifactLength(0.200));
            Blob hitboxBlob = new Blob(hitboxCenter);
            if (HandEye.MouseOverStationaryObject(hitboxBlob, false, 10, 2500))
            {
                ScanForMarkOfGrace();
                if (HandEye.MouseOverStationaryObject(new Blob(hitboxCenter), true, 10, 3000))
                {
                    SafeWait(5000);
                    return true;
                }
            }            
            return false;
        }

        /// <summary>
        /// Jumps off of the church roof
        /// </summary>
        /// <returns>true if successful</returns>
        private bool JumpOffTreeRoof()
        {
            Point hitboxCenter = new Point(Screen.Center.X + Screen.ArtifactLength(0.069), Screen.Center.Y + Screen.ArtifactLength(0.069));
            Blob hitboxBlob = new Blob(hitboxCenter);
            if (HandEye.MouseOverStationaryObject(hitboxBlob, false, 10, 3500))
            {
                ScanForMarkOfGrace();
                if (HandEye.MouseOverStationaryObject(new Blob(hitboxCenter), true, 10, 3000))
                {
                    SafeWait(3000);
                    Inventory.HoverStandardTeleport(Inventory.StandardTeleports.Camelot, false, false);
                    Vision.WaitDuringPlayerAnimation(3000);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Looks for a mark of grace to pick up and picks it up if found.
        /// Returns the player to his starting location after picking it up.
        /// </summary>
        /// <returns>true if a mark of grace is found</returns>
        private bool ScanForMarkOfGrace(bool returnToOriginalPosition = true)
        {
            Blob markOfGraceBackground, markOfGraceFigure;
            if (Vision.LocateObject(MarkOfGraceBackground, out markOfGraceBackground, Screen.ArtifactArea(0.000355), Screen.ArtifactArea(0.00142))   //ex 0.000710
                && Vision.LocateObject(MarkOfGraceFigure, out markOfGraceFigure, markOfGraceBackground.LeftBound, markOfGraceBackground.RightBound, markOfGraceBackground.TopBound, markOfGraceBackground.BottomBound, Screen.ArtifactArea(0.0000402)))
            {
                markOfGraceBackground.AddBlob(markOfGraceFigure);
                if (HandEye.MouseOverDroppedItem(markOfGraceBackground, true, 5, 3000))
                {
                    SafeWait(2000);
                    Vision.WaitDuringPlayerAnimation(8000);
                    if (returnToOriginalPosition)
                    {
                        LeftClick(2 * Screen.Center.X - markOfGraceBackground.Center.X, 2 * Screen.Center.Y - markOfGraceBackground.Center.Y);
                        SafeWait(2000);
                        Vision.WaitDuringPlayerAnimation(8000);
                    }
                }
            }
            return false;
        }
    }
}
