﻿using RunescapeBot.BotPrograms.FixedUIComponents;
using RunescapeBot.BotPrograms.Popups;
using RunescapeBot.BotPrograms.Settings;
using RunescapeBot.Common;
using RunescapeBot.ImageTools;
using RunescapeBot.UITools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace RunescapeBot.BotPrograms
{
    public class NatureRings : BotProgram
    {
        #region properties

        const int STAMINA_DURATION = 120000; //duration of a dose of stamina potion in milliseconds
        Stopwatch StaminaTimer;

        RGBHSBRange FairyRingWhite = RGBHSBRangeFactory.FairyRingMushroom();
        RGBHSBRange FairyRingTeleport = RGBHSBRangeFactory.FairyRingTeleport();
        RGBHSBRange NatureAltar = RGBHSBRangeFactory.RunecraftingAltar();
        RGBHSBRange DamagedPouchColor = RGBHSBRangeFactory.RunecraftingPouchDamaged();

        protected bool ringSlotEmpty;
        protected bool pouchDamaged;
        protected NatureRingsSettingsData UserSelections;

        protected Point BankSlotPureEssence;
        protected Point BankSlotStaminaPotion;
        protected Point BankSlotCosmicRunes, BankSlotAstralRunes, BankSlotAirRunes;
        protected Point BankSlotAmuletOfGlory;
        protected Point BankSlotChargeDragonstone;
        protected Point[] BankSlotStaminaPotions;

        protected Point InventorySlotSmallPouch, InventorySlotMediumPouch, InventorySlotLargePouch, InventorySlotGiantPouch;
        protected Point[] PouchSlots;
        protected Point InventorySlotCraftedRunes;
        protected Point InventorySlotEssenceCheck;
        protected Point InventorySlotStaminaPotion;
        protected Point InventoryDepletedGlory;

        bool LowStamina, DepletedGlory, DamagedPouches, FairyRingConfigured;
        int FailedRuns;

        #endregion

        #region iteration

        public NatureRings(RunParams startParams) : base(startParams)
        {
            RunParams.Run = true;
            RunParams.RunAbove = 0.04;

            UserSelections = startParams.CustomSettingsData.NatureRings;
            LowStamina = true;
            StaminaTimer = new Stopwatch();

            SetItemSlots();
            FairyRingConfigured = false;
            FailedRuns = 0;
        }

        /// <summary>
        /// Sets slots in inventory and bank where items are expected to be
        /// </summary>
        protected void SetItemSlots()
        {
            BankSlotPureEssence = new Point(7, 0);
            BankSlotStaminaPotion = new Point(6, 0);
            BankSlotCosmicRunes = new Point(5, 0);
            BankSlotAstralRunes = new Point(4, 0);
            BankSlotAirRunes = new Point(3, 0);
            BankSlotAmuletOfGlory = new Point(2, 0);
            BankSlotChargeDragonstone = new Point(1, 0);
            BankSlotStaminaPotions = new Point[4];
            BankSlotStaminaPotions[0] = new Point(5, 1);
            BankSlotStaminaPotions[1] = new Point(6, 1);
            BankSlotStaminaPotions[2] = new Point(7, 1);
            BankSlotStaminaPotions[3] = BankSlotStaminaPotion;

            InventorySlotSmallPouch = new Point(0, 0);
            InventorySlotMediumPouch = new Point(1, 0);
            InventorySlotLargePouch = new Point(2, 0);
            InventorySlotGiantPouch = new Point(3, 0);
            PouchSlots = new Point[4];
            PouchSlots[0] = InventorySlotSmallPouch;
            PouchSlots[1] = InventorySlotMediumPouch;
            PouchSlots[2] = InventorySlotLargePouch;
            PouchSlots[3] = InventorySlotGiantPouch;

            InventorySlotCraftedRunes = Inventory.InventoryIndexToCoordinates(UserSelections.NumberOfPouches);
            InventorySlotEssenceCheck = Inventory.InventoryIndexToCoordinates(UserSelections.NumberOfPouches + 1);
            InventorySlotStaminaPotion = InventorySlotCraftedRunes;
            InventoryDepletedGlory = Inventory.InventoryIndexToCoordinates(UserSelections.NumberOfPouches + 1);
        }

        protected override bool Run()
        {
            //ReadWindow();
            //DebugUtilities.SaveImageToFile(Bitmap, "C:\\Projects\\Roboport\\test_pictures\\nature rings\\test.png");

            //MaskTest(RGBHSBRangeFactory.BankBoothEdgeville());
            //MaskTest(RGBHSBRangeFactory.FairyRing());
            //MaskTest(RGBHSBRangeFactory.Furnace());
            //MaskTest(RGBHSBRangeFactory.RunecraftingAltar());
            //MaskTest(RGBHSBRangeFactory.RunecraftingAltar());
            //MaskTest(RGBHSBRangeFactory.BankSlotPlaceholderZero());

            //Minimap.MoveToPosition(170, 1, true);
            //MoveToFairyRing();
            //EnterFairyRing(new Point(Center.X, Center.Y + 119), 250);
            //MoveInsideNatureAltar();

            //ReadWindow();
            //Bank bank = new Bank(RSClient, Inventory);
            //bool slotIsEmpty = bank.SlotIsEmpty(7, 1, ColorArray);

            //Bank bank = new Bank(RSClient, Inventory);
            //WithdrawStaminaPotion(bank);

            //Point click = RightClick(Center.X, Center.Y, 100);
            //RightClick popup = new RightClick(click.X, click.Y, RSClient);
            //popup.WaitForPopup();
            //WaitForFairyRingTeleport();
            //RepairPouches();
            //EvaluateStamina();

            //Point expectedFairyRingLocation = new Point(Center.X, Center.Y + 100);
            //MoveMouse(expectedFairyRingLocation.X, expectedFairyRingLocation.Y, 68);
            //SafeWaitPlus(1000, 100);

            return true;
        }

        protected override bool Execute()
        {
            if (Bank()
                && FairyRing()
                && MoveToNatureAltar()
                && EnterAltar()
                && CraftAllPouches())
            {
                FailedRuns = 0;
                RunParams.Iterations -= EssencePerRun;
            }
            else
            {
                FailedRuns++;
            }

            bool giveUp = FailedRuns > 10;
            return !giveUp;
        }

        /// <summary>
        /// The expected number of essence for the player to craft in each successful run.
        /// </summary>
        protected int EssencePerRun
        {
            get
            {
                switch (UserSelections.NumberOfPouches)
                {
                    case 0:
                        return 28;
                    case 1:
                        return 30;
                    case 2:
                        return 35;
                    case 3:
                        return 43;
                    case 4:
                        return 54;
                    default:
                        return 28;
                }
            }
        }

        #endregion

        #region banking

        /// <summary>
        /// Selects a banking route to use based on the user's selection
        /// </summary>
        /// <returns>true if successful</returns>
        protected bool Bank()
        {
            switch(UserSelections.BankChoice)
            {
                case NatureRingsSettingsData.BankOptions.Edgeville:
                    if (!MoveToEdgevilleBank()) { return false; }
                    break;
                default:
                    return false;
            }

            Inventory.OpenInventory();  //Inventory should already be open, but just to make sure
            if (!Minimap.WaitDuringMovement(8000, 0)) { return false; }
            SafeWait(2 * BotRegistry.GAME_TICK);

            return RefreshItems();
        }

        /// <summary>
        /// Teleports to Edgeville and moves to the Edgeville bank
        /// </summary>
        /// <returns>true if successful</returns>
        protected bool MoveToEdgevilleBank()
        {
            //teleport to Edgeville bank
            Inventory.GloryTeleport(Inventory.GloryTeleports.Edgeville, false);
            Stopwatch watch = new Stopwatch();
            watch.Start();
            MoveMouse(Minimap.Center.X + 35, Minimap.Center.Y + 8, 35);
            SafeWait(Inventory.TELEPORT_DURATION);

            //Start moving to bank booth
            Point bankIconOffsetTarget = new Point(2 * MinimapGauge.GRID_SQUARE_SIZE, MinimapGauge.GRID_SQUARE_SIZE / 2);
            if (!Banking.MoveToBank(0, true, 4, 2, bankIconOffsetTarget)) { return false; }

            CheckItems();
            Inventory.OpenInventory();
            MoveMouse(Screen.Center.X, Screen.Center.Y + 30, 15);
            return true;
        }

        /// <summary>
        /// Check for damaged pouches and a depleted glory on the way to the bank.
        /// </summary>
        protected void CheckItems()
        {
            Screen.ReadWindow();

            for (int i = 0; i < PouchSlots.Length; i++)
            {
                Point pouchLocation = PouchSlots[i];
                if (PouchIsDamaged(pouchLocation.X, pouchLocation.Y))
                {
                    DamagedPouches = true;
                    break;
                }
            }
            if (UserSelections.GloryType != NatureRingsSettingsData.GloryOptions.EternalGlory)
            {
                DepletedGlory = GloryIsDepleted();
            }
        }

        /// <summary>
        /// Check is the amulet of glory worn by the user has run out of charges
        /// </summary>
        /// <returns>true if glory has zero teleport charges</returns>
        protected bool GloryIsDepleted()
        {
            //TODO check amulet of glory for depletion
            return false;
        }

        /// <summary>
        /// Determines if a pouch in a given inventory slot is damaged
        /// </summary>
        /// <param name="x">column</param>
        /// <param name="y">row</param>
        /// <returns>true if pouch is damaged</returns>
        protected bool PouchIsDamaged(int x, int y, bool readWindow = true)
        {
            return Inventory.SlotMatchesColorFilter(x, y, DamagedPouchColor, 0.005);
        }

        /// <summary>
        /// Uses Contact NPC to contact the Dark Mage and have him repair pouches.
        /// Assumes that the required runes are already in the player's inventory.
        /// </summary>
        /// <returns>true if successful</returns>
        protected bool RepairPouches()
        {
            Inventory.ClickSpellbookLunar(5, 0);
            NPCContact npcContact = new NPCContact(RSClient);
            if (!npcContact.WaitForPopup()) { return false; }
            if (npcContact.RepairPouches(Textbox))
            {
                DamagedPouches = false;
                return true;
            }

            return false;   //failed to repair pouches
        }
        public const int REPAIR_POUCHES_DURATION = NPCContact.NPC_CONTACT_CAST_TIME + 3000; //approximate time needed to go through the full pouch repair process

        /// <summary>
        /// Replenishes pure essence. Drinks a dose of stamina potion on every other trip.
        /// </summary>
        /// <returns></returns>
        protected bool RefreshItems()
        {
            EvaluateStamina();  //determine if the player should drink a dose of stamina potion

            //Fill small-large pouches. Service if not using all four pouches.
            if (StopFlag || !FillSmallMediumLargePouches())
            {
                return false;
            }

            //Fill huge pouch
            if (StopFlag || UserSelections.NumberOfPouches >= 4 && !FillHugePouch())
            {
                return false;
            }

            //Refill inventory
            if (UserSelections.NumberOfPouches > 0 || LowStamina)
            {
                Bank bank;
                if (!Banking.OpenBank(out bank)) { return false; }
                if (LowStamina)
                {
                    bank.DepositItem(InventorySlotStaminaPotion);
                    LowStamina = false;
                }
                bank.WithdrawAll(BankSlotPureEssence.X, BankSlotPureEssence.Y); //don't waste time closing the bank since it will close itself when we start running
            }

            return true;
        }

        /// <summary>
        /// Determine if the player should drink a dose of stamina potion
        /// </summary>
        protected void EvaluateStamina()
        {
            const int drinkBelow = 75;  //Do not drink above 75 run energy.
            const int mostEarlyDrink = 100;  //Never drink more than 80 seconds before a stamina potion runs out.
            double runEnergy = 100 * Minimap.RunEnergy(true);
            long timeSinceLastDose = StaminaTimer.ElapsedMilliseconds;

            if (DamagedPouches)
            {
                timeSinceLastDose += REPAIR_POUCHES_DURATION;
            }
            if (!StaminaTimer.IsRunning) //drink a stamina potion for the first time
            {
                timeSinceLastDose = STAMINA_DURATION;
            }

            int drinkEarly = (int) (mostEarlyDrink * ((drinkBelow - (int)runEnergy) / (double)drinkBelow));
            LowStamina = timeSinceLastDose > (STAMINA_DURATION - UnitConversions.SecondsToMilliseconds(drinkEarly));
        }

        /// <summary>
        /// Withdraws the lowest dose stamina potion available.
        /// Assumes that the bank is open.
        /// </summary>
        /// <returns>true if successful</returns>
        protected bool WithdrawStaminaPotion(Bank bank)
        {
            Screen.ReadWindow();
            for (int i = 0; i < Math.Min(3, UserSelections.NumberOfPouches); i++)
            {
                if (!bank.SlotIsEmpty(BankSlotStaminaPotions[i].X, BankSlotStaminaPotions[i].Y, Screen))
                {
                    bank.WithdrawOne(BankSlotStaminaPotions[i].X, BankSlotStaminaPotions[i].Y);
                    return true;
                }
            }
            bank.WithdrawOne(BankSlotStaminaPotion.X, BankSlotStaminaPotion.Y);
            return true;
        }

        /// <summary>
        /// Drinks a dose of stamina potion.
        /// Assumes that a stamina potion is already in its designated inventory slot.
        /// </summary>
        protected void DrinkStaminaPotion()
        {
            Inventory.ClickInventory(InventorySlotStaminaPotion);
            StaminaTimer.Restart();
        }

        /// <summary>
        /// Fills the small, medium, and large pouches.
        /// Refreshes stamina if it is almost out.
        /// Replaces any depleted jewelry.
        /// </summary>
        /// <param name="refreshStamina">set to true to drink a dose of stamina potion</param>
        /// <returns>true if successful</returns>
        protected bool FillSmallMediumLargePouches()
        {
            Point cursorLocation = new Point(Mouse.X, Mouse.Y);
            Bank bank;

            if (HandEye.MouseOverStationaryObject(new Blob(cursorLocation), true, 0, 0))    //try mousing over the default guess location first
            {
                bank = new Bank(RSClient, Inventory, Keyboard);
                if (!bank.WaitForPopup(6000))
                {
                    return false;
                }
            }    
            else if (!Banking.OpenBank(out bank, 3))    //Look for the bank if guessing fails
            {
                return false;
            }

            bank.DepositAll(InventorySlotCraftedRunes);
            BankWithdrawForServicing(bank);
            bank.WithdrawAll(BankSlotPureEssence.X, BankSlotPureEssence.Y);
            bank.Close();

            return Servicing();
        }

        /// <summary>
        /// Withdraws items needed for servicing.
        /// Servicing repairs damaged pouches, replaces depleted glory, replenishes stamina, and fills three smallest pouches
        /// </summary>
        /// <param name="bank">bank popup</param>
        protected void BankWithdrawForServicing(Bank bank)
        {
            if (LowStamina)
            {
                WithdrawStaminaPotion(bank);
            }
            if (DepletedGlory)
            {
                bank.WithdrawOne(BankSlotAmuletOfGlory.X, BankSlotAmuletOfGlory.Y);
            }
            if (DamagedPouches)
            {
                bank.WithdrawOne(BankSlotCosmicRunes.X, BankSlotCosmicRunes.Y);
                bank.WithdrawOne(BankSlotAstralRunes.X, BankSlotAstralRunes.Y);
                bank.WithdrawOne(BankSlotAirRunes.X, BankSlotAirRunes.Y);
                SafeWaitPlus(40, 15);
                bank.WithdrawOne(BankSlotAirRunes.X, BankSlotAirRunes.Y);
            }
        }

        /// <summary>
        /// Repairs damaged pouches, replaces depleted glory, replenishes stamina, and fills three smallest pouches.
        /// </summary>
        protected bool Servicing()
        {
            if (DamagedPouches)
            {
                RepairPouches();
            }
            for (int i = 0; i < Math.Min(UserSelections.NumberOfPouches, 3); i++)
            {
                Inventory.ClickInventory(PouchSlots[i]);
                if (SafeWaitPlus(0, 50)) { return false; }
            }
            if (LowStamina)
            {
                if (DepletedGlory)
                {
                    Inventory.ClickInventory(InventoryDepletedGlory);
                    DepletedGlory = false;
                }
                DrinkStaminaPotion();
            }
            else if (DepletedGlory)
            {
                Inventory.ClickInventory(InventorySlotCraftedRunes);
                DepletedGlory = false;
            }

            return true;
        }

        /// <summary>
        /// Fills the small, medium, and large pouches.
        /// Refreshes stamina if it is almost out.
        /// </summary>
        /// <param name="refreshStamina">set to true to drink a dose of stamina potion</param>
        /// <returns>true if successful</returns>
        protected bool FillHugePouch()
        {
            Bank bank;

            if (!Banking.OpenBank(out bank)) { return false; }
            if (LowStamina)
            {
                bank.DepositItem(InventorySlotStaminaPotion);
                LowStamina = false;
            }
            bank.WithdrawAll(BankSlotPureEssence.X, BankSlotPureEssence.Y);
            bank.Close();
            Inventory.ClickInventory(InventorySlotGiantPouch);

            return true;
        }

        #endregion

        #region fairy rings

        /// <summary>
        /// Moves to the fairy ring chosen by the user and teleports to Karamja.
        /// </summary>
        /// <returns>true if successful</returns>
        protected bool FairyRing()
        {
            switch(UserSelections.FairyRing)
            {
                case NatureRingsSettingsData.FairyRingOptions.Edgeville:
                    return FairyRingEdgeville();
                default:
                    return false;
            }
        }

        /// <summary>
        /// Moves from the Edgeville bank to the Edgeville fairy ring and teleports to Karamja.
        /// </summary>
        /// <returns>true if successful</returns>
        protected bool FairyRingEdgeville()
        {
            List<MinimapWaypoint> waypoints = new List<MinimapWaypoint>();
            waypoints.Add(new MinimapWaypoint(45, 1, 0));
            waypoints.Add(new MinimapWaypoint(29, 1, 0));
            waypoints.Add(new MinimapWaypoint(297, 1, 7000));
            if (!Minimap.MoveAlongPath(waypoints, 3, null))
            {
                return false;
            }
            Point expectedFairyRingLocation = new Point(Screen.Center.X, Screen.Center.Y + 100);
            MoveMouse(expectedFairyRingLocation.X, expectedFairyRingLocation.Y, 68);
            SafeWaitPlus(1000, 100);

            int waitTime = 200;
            for (int searchRadius = 250; searchRadius < 1000; searchRadius += 100)
            {
                if (SafeWait(waitTime)) { return false; }
                if (EnterFairyRing(expectedFairyRingLocation, searchRadius))
                {
                    return true;
                }
                else
                {
                    MoveMouse(Mouse.X + 10, Mouse.Y - 50, 25);
                }
            }

            return false;
        }

        /// <summary>
        /// Finds the center of a fairy ring.
        /// </summary>
        /// <param name="expectedLocation">The point at the expected center of the fairy ring.</param>
        /// <param name="searchRadius">The maximum expected deviation of the fairy ring's center from the expected center.</param>
        /// <returns>The point at the center off the fairy ring. Null if no fairy ring is not found.</returns>
        protected Point? LocateFairyRing(Point expectedLocation, int searchRadius)
        {
            const int fairyRingRadius = 100;
            int left = expectedLocation.X - searchRadius - fairyRingRadius;
            int right = expectedLocation.X + searchRadius + fairyRingRadius;
            int top = expectedLocation.Y - searchRadius - fairyRingRadius;
            int bottom = expectedLocation.Y + searchRadius + fairyRingRadius;

            List<Blob> mushrooms = Vision.LocateObjects(FairyRingWhite, left, right, top, bottom, true, 1, Screen.ArtifactArea(0.0000326));   //ex 0.0000261
            if (mushrooms == null)
            {
                return null;
            }

            return Blob.ClusterCenter(mushrooms);
        }

        /// <summary>
        /// Finds the fairy ring and teleports to Karamja
        /// </summary>
        /// <param name="expectedLocation">The point at the expected center of the fairy ring.</param>
        /// <param name="searchRadius">The maximum expected deviation of the fairy ring's center from the expected center.</param>
        /// <returns>true if successful</returns>
        protected bool EnterFairyRing(Point expectedLocation, int searchRadius)
        {
            Point? ringCenter = LocateFairyRing(expectedLocation, searchRadius);
            if (ringCenter == null)
            {
                return false;
            }

            int x = ringCenter.Value.X;
            int y = ringCenter.Value.Y;
            RightClick(x, y, 6);
            RightClick fairyRingOptions = new RightClick(x, y, RSClient, 6);
            if (!fairyRingOptions.WaitForPopup(2000, Popups.RightClick.CheckHeight.Half))
            {
                return false;
            }

            if (!FairyRingConfigured)
            {
                ConfigureFairyRing(fairyRingOptions);
            }
            else
            {
                fairyRingOptions.CustomOption(2);   //teleport to the last location
            }

            MoveMouse(Minimap.Center.X + 55, Minimap.Center.Y, 40);
            SafeWait(2500);
            WaitForFairyRingTeleport();
            return true;
        }

        /// <summary>
        /// Configures and teleports to a specified location.
        /// Assumes that a fairy ring was just right-clicked on.
        /// </summary>
        /// <param name="expectedLocation">Expected location of the center of the fairy ring.</param>
        /// <param name="searchRadius">The maximum expected deviation of the fairy ring's center from the expected center.</param>
        /// <returns></returns>
        protected void ConfigureFairyRing(RightClick fairyRingOptions)
        {
            fairyRingOptions.CustomOption(1);   //open the configuration popup
            SafeWaitPlus(2000, 400);
            FairyRingsConfigure menu = new FairyRingsConfigure(Screen, RSClient);
            menu.SetConfiguration('c', 'k', 'r');
            menu.Teleport(false);
            FairyRingConfigured = true;
        }

        /// <summary>
        /// Waits until the fairy rings teleport completes
        /// </summary>
        protected void WaitForFairyRingTeleport()
        {
            bool teleportStarted = false;
            Stopwatch watch = new Stopwatch();
            watch.Start();
            double match;
            while (watch.ElapsedMilliseconds < 4000 && !StopFlag)
            {
                Screen.ReadWindow();
                bool[,] portal = Vision.ColorFilterPiece(FairyRingTeleport, Screen.Center, 50);
                match = ImageProcessing.FractionalMatch(portal);

                if (match > 0.001)
                {
                    teleportStarted = true;
                }
                if (match < 0.001 && teleportStarted)
                {
                    return;
                }
            }
        }

        #endregion

        #region Karamja

        /// <summary>
        /// Moves the player from Karamja fairy ring to the inside of the nature altar
        /// </summary>
        /// <returns>true if successful</returns>
        protected bool MoveToNatureAltar()
        {
            List<MinimapWaypoint> waypoints = new List<MinimapWaypoint>();
            waypoints.Add(new MinimapWaypoint(10, 1, 10000));
            waypoints.Add(new MinimapWaypoint(0, 1, 0));
            waypoints.Add(new MinimapWaypoint(-5, 1, 0));
            waypoints.Add(new MinimapWaypoint(23, 0.8, 0));
            if (!Minimap.MoveAlongPath(waypoints, 3, null))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Crafts all runes in pouches.
        /// </summary>
        /// <returns>true if successful</returns>
        protected bool CraftAllPouches()
        {
            if (!CraftSmallMediumLargePouches()
                || !CraftGiantPouch())
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Empties up to the first three pouches and crafts their runes.
        /// Does nothing if the player is not using small, medium, or large pouch.
        /// </summary>
        /// <returns>true if successful</returns>
        protected bool CraftSmallMediumLargePouches()
        {
            if (UserSelections.NumberOfPouches == 0)
            {
                return true;
            }

            for (int i = 0; i < Math.Min(3, UserSelections.NumberOfPouches); i++)
            {
                Inventory.RightClickInventoryOption(PouchSlots[i].X, PouchSlots[i].Y, 1);
            }

            Point altarLocation = new Point(Screen.Center.X, Screen.Center.Y - Screen.ArtifactLength(0.120));
            if (UserSelections.NumberOfPouches > 3)
            {
                Inventory.InventoryToScreen(ref InventorySlotGiantPouch);
                CraftInventory(altarLocation, InventorySlotGiantPouch);
                WaitForRunesToCraft();
            }
            else
            {
                CraftInventory(altarLocation, null);
                WaitForRunesToCraft(true);
            }

            return true;
        }

        /// <summary>
        /// Empties the giant pouch and crafts its runes.
        /// Does nothing if the user is not using a giant pouch.
        /// </summary>
        /// <returns>true if successful</returns>
        protected bool CraftGiantPouch()
        {
            if (UserSelections.NumberOfPouches < 4)
            {
                return true;
            }

            Inventory.RightClickInventoryOption(InventorySlotGiantPouch.X, InventorySlotGiantPouch.Y, 1);
            Point equipmentSlot = new Point(Screen.Width - Inventory.TAB_RIGHT_OFFSET_RIGHT - 2 * Inventory.TAB_HORIZONTAL_GAP);
            CraftInventory(new Point(Screen.Center.X, Screen.Center.Y - Screen.ArtifactLength(0.120)), null);
            WaitForRunesToCraft(true);
            return true;
        }

        /// <summary>
        /// Clicks the exterior nature altar to enter it and clicks the nature altar to craft the first inventory of essence
        /// </summary>
        /// <returns>true if successful</returns>
        protected bool EnterAltar()
        {
            int minimumSize = Screen.ArtifactArea(0.01);   //ex 0.0236
            Blob exteriorAltar;
            Point searchCenter = new Point(Screen.Center.X + Screen.ArtifactLength(0.25), Screen.Center.Y - Screen.ArtifactLength(0.24));

            if (!HandEye.MouseOverStationaryObject(new Blob(searchCenter), true, 10, 1000))  //click on the exterior nature altar to enter
            {
                if (!LocateAltar(out exteriorAltar, searchCenter, Screen.ArtifactLength(0.3)))
                {
                    return false;
                }
                Vision.WaitDuringPlayerAnimation(3000);
                if (!HandEye.MouseOverStationaryObject(exteriorAltar, true, 10, 2000))
                {
                    return false;
                }
            }
            SafeWaitPlus((int)(2.5 * BotRegistry.GAME_TICK), 150);

            //click on the interior nature altar to craft inventory by guessing the location
            Point altarLocation = new Point(Screen.Center.X, Screen.Center.Y - Screen.ArtifactLength(0.311));
            int x = InventorySlotSmallPouch.X, y = InventorySlotSmallPouch.Y;
            Inventory.InventoryToScreen(ref x, ref y);

            for (int i = 0; i < 5; i++)
            {
                CraftInventory(altarLocation, new Point(x, y));
                if (WaitForRunesToCraft(false, 3 * BotRegistry.GAME_TICK))
                {
                    break;
                }
                SafeWait(BotRegistry.GAME_TICK);
            }
            
            return true;
        }

        /// <summary>
        /// Looks for the exterior or interior of a runecrafting altar
        /// </summary>
        /// <param name="altar">returns the altar blob if found</param>
        /// <param name="searchCenter">center of area to look at in first pass</param>
        /// <param name="searchRadius">vertical and horitzontal offset from the center defining the swuare to search in</param>
        /// <returns>true if an altar is found</returns>
        protected bool LocateAltar(out Blob altar, Point? searchCenter = null, int searchRadius = int.MaxValue)
        {
            searchCenter = searchCenter ?? Screen.Center;
            int minimumSize = Screen.ArtifactArea(0.01);   //ex 0.0236

            if (!Vision.LocateObject(NatureAltar, out altar, searchCenter.Value, Screen.ArtifactLength(0.3), minimumSize))
            {
                return Vision.LocateObject(NatureAltar, out altar);
            }
            return true;
        }

        /// <summary>
        /// Waits for inventory runes to be crafted and disappear from the inventory
        /// </summary>
        /// <param name="shortWait">Waits a constant time without visual confirmation</param>
        /// <returns></returns>
        protected bool WaitForRunesToCraft(bool shortWait = false, int waitToStartLooking = 2 * BotRegistry.GAME_TICK, int timeout = 10 * BotRegistry.GAME_TICK)
        {
            if (shortWait)
            {
                SafeWaitPlus((long)(1.5 * BotRegistry.GAME_TICK), 100);
                return true;
            }

            Stopwatch watch = new Stopwatch();
            watch.Start();
            if (SafeWait(waitToStartLooking)) { return false; }

            while (watch.ElapsedMilliseconds < timeout)
            {
                Screen.ReadWindow();
                if (Inventory.SlotIsEmpty(InventorySlotEssenceCheck, true))
                {
                    if (SafeWait((int) (1.5 * BotRegistry.GAME_TICK))) { return false; }
                    return true;
                }
                else if (SafeWait(100)) { return false; }
            }
            return false;
        }

        /// <summary>
        /// Clicks the interior nature altar
        /// </summary>
        /// <returns>true if successful</returns>
        protected bool CraftInventory(Point altarLocation, Point? restMouse)
        {
            //click on the interior nature altar to craft inventory by guessing the location
            altarLocation = Probability.GaussianCircle(altarLocation, 5, 0, 360, 15);
            Blob interiorAltar = new Blob(altarLocation);

            //guess the location before searching
            if (!HandEye.MouseOverStationaryObject(interiorAltar, true, 20, 5000))
            {
                if (!LocateAltar(out interiorAltar) || !HandEye.MouseOverStationaryObject(interiorAltar, true, 20, 3000))
                {
                    return false;
                }
            }
            if (restMouse != null)
            {
                MoveMouse(restMouse.Value.X, restMouse.Value.Y);    //leave the mouse close to where it will be needed next
            }

            return true;
        }

        #endregion
    }
}
