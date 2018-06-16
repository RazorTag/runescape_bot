using RunescapeBot.BotPrograms.FixedUIComponents;
using RunescapeBot.BotPrograms.Popups;
using RunescapeBot.BotPrograms.Settings;
using RunescapeBot.Common;
using RunescapeBot.ImageTools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace RunescapeBot.BotPrograms
{
    public class NatureRings : BotProgram
    {
        const int STAMINA_DURATION = 120000; //duration of a dose of stamina potion in milliseconds
        Stopwatch StaminaTimer;

        RGBHSBRange FairyRingWhite = RGBHSBRangeFactory.FairyRingMushroom();
        RGBHSBRange FairyRingTeleport = RGBHSBRangeFactory.FairyRingTeleport();

        protected bool ringSlotEmpty;
        protected bool pouchDamaged;
        protected NatureRingsSettingsData UserSelections;

        protected Point BankSlotPureEssence;
        protected Point BankSlotStaminaPotion;
        protected Point BankSlotCosmicRunes, BankSlotAstralRunes, BankSlotAirRunes;
        protected Point BankSlotAmuletOfGlory;
        protected Point BankSlotChargeDragonstone;

        protected Point InventorySlotSmallPouch, InventorySlotMediumPouch, InventorySlotLargePouch, InventorySlotHugePouch;
        protected Point[] PouchSlots;
        protected Point InventorySlotCraftedRunes;
        protected Point InventorySlotStaminaPotion;
        protected Point InventoryDepletedGlory;

        bool LowStamina, DepletedGlory, DamagedPouches, FairyRingConfigured;

        public NatureRings(RunParams startParams) : base(startParams)
        {
            RunParams.Run = true;
            RunParams.RunLoggedIn = true;
            UserSelections = startParams.CustomSettingsData.NatureRings;
            LowStamina = true;
            StaminaTimer = new Stopwatch();

            SetItemSlots();
            FairyRingConfigured = false;
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

            InventorySlotSmallPouch = new Point(0, 0);
            InventorySlotMediumPouch = new Point(1, 0);
            InventorySlotLargePouch = new Point(2, 0);
            InventorySlotHugePouch = new Point(3, 0);
            PouchSlots = new Point[4];
            PouchSlots[0] = InventorySlotSmallPouch;
            PouchSlots[1] = InventorySlotMediumPouch;
            PouchSlots[2] = InventorySlotLargePouch;
            PouchSlots[3] = InventorySlotHugePouch;

            InventorySlotCraftedRunes = Inventory.InventoryIndexToCoordinates(UserSelections.NumberOfPouches);
            InventorySlotStaminaPotion = InventorySlotCraftedRunes;
            InventoryDepletedGlory = Inventory.InventoryIndexToCoordinates(UserSelections.NumberOfPouches + 1);
        }

        protected override bool Run()
        {
            //ReadWindow();
            //DebugUtilities.SaveImageToFile(Bitmap, "C:\\Projects\\Roboport\\test_pictures\\nature rings\\test.png");
            //MaskTest(RGBHSBRangeFactory.BankBoothEdgeville());
            //MaskTest(RGBHSBRangeFactory.FairyRing());

            //Minimap.MoveToPosition(170, 1, true);
            //MoveToFairyRing();
            //EnterFairyRing(new Point(Center.X, Center.Y + 119), 250);

            return true;
        }

        protected override bool Execute()
        {
            if (!Bank() || !FairyRing())
            {
                return false;
            }

            RunParams.Iterations--;
            return true;
        }

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
            Inventory.OpenInventory();
            if (!Minimap.WaitDuringMovement(8000, 0)) { return false; }

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
            MoveMouse(Minimap.Center.X + 35, Minimap.Center.Y + 7, 12);
            SafeWait(Inventory.TELEPORT_DURATION);

            //Start moving to bank booth
            Point bankIconOffsetTarget = new Point(2 * MinimapGauge.GRID_SQUARE_SIZE, MinimapGauge.GRID_SQUARE_SIZE/2);
            MoveToBank(0, true, 4, 2, bankIconOffsetTarget);

            CheckItems();
            MoveMouse(Center.X, Center.Y + 50, 75);
            return true;
        }

        /// <summary>
        /// Check for damaged pouches and a depleted glory.
        /// </summary>
        protected void CheckItems()
        {
            //Check items on the way to bank
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
            //TODO determine if a pouch is damaged
            Color[,] slotImage = Inventory.SlotPicture(x, y, readWindow);

            return false;
        }

        /// <summary>
        /// Uses Contact NPC to contact the Dark Mage and have him repair pouches
        /// </summary>
        /// <returns>true if successful</returns>
        protected bool RepairPouches()
        {
            //TODO contact dark mage using Contact NPC
            DamagedPouches = false;
            return false;
        }

        /// <summary>
        /// Replenishes pure essence. Drinks a dose of stamina potion on every other trip.
        /// </summary>
        /// <returns></returns>
        protected bool RefreshItems()
        {
            LowStamina = StaminaTimer.ElapsedMilliseconds > (STAMINA_DURATION - UnitConversions.SecondsToMilliseconds(30));

            //Fill small-large pouches. Service if not using all four pouches.
            bool earlyService = LowStamina && UserSelections.NumberOfPouches <= 3;
            if (StopFlag || !FillSmallMediumLargePouches())    //TODO replace glory and pouches
            {
                return false;
            }

            //Fill huge pouch
            bool lateService = LowStamina && !earlyService;
            if (StopFlag || UserSelections.NumberOfPouches >= 4 && !FillHugePouch())
            {
                return false;
            }

            //Refill inventory
            if (UserSelections.NumberOfPouches > 0 || LowStamina)
            {
                Bank bank;
                if (!OpenBank(out bank)) { return false; }
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
        /// Withdraws a stamina potion.
        /// Assumes that the bank is open.
        /// </summary>
        /// <returns>true if successful</returns>
        protected bool WithdrawStaminaPotion(Bank bank)
        {
            //TODO use 1, 2, and 3 dose stamina potions before using the main 4 dose stack
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
            Bank bank;

            if (!OpenBank(out bank))
            {
                SafeWait(200);
                if (!OpenBank(out bank))
                {
                    return false;
                }
            }
            bank.DepositAll(InventorySlotCraftedRunes);

            BankWithdrawForServicing(bank);
            bank.WithdrawAll(BankSlotPureEssence.X, BankSlotPureEssence.Y);
            bank.Close();

            Servicing();
            return true;
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
        protected void Servicing()
        {
            if (DamagedPouches)
            {
                RepairPouches();
            }
            for (int i = 0; i < Math.Min(UserSelections.NumberOfPouches, 3); i++)
            {
                Inventory.ClickInventory(PouchSlots[i]);
                if (SafeWaitPlus(0, 50)) { return; }
            }
            if (LowStamina)
            {
                if (DepletedGlory)
                {
                    Inventory.ClickInventory(InventoryDepletedGlory);
                    DepletedGlory = false;
                }
                DrinkStaminaPotion();
                LowStamina = false;
            }
            else if (DepletedGlory)
            {
                Inventory.ClickInventory(InventorySlotCraftedRunes);
                DepletedGlory = false;
            }
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

            if (!OpenBank(out bank)) { return false; }
            bank.WithdrawAll(BankSlotPureEssence.X, BankSlotPureEssence.Y);
            bank.Close();
            Inventory.ClickInventory(InventorySlotHugePouch);

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
            waypoints.Add(new MinimapWaypoint(293, 1, 7000));
            if (!Minimap.MoveAlongPath(waypoints, 3, null))
            {
                return false;
            }
            Point expectedFairyRingLocation = new Point(Center.X, Center.Y + 119);
            MoveMouse(expectedFairyRingLocation.X, expectedFairyRingLocation.Y, 68);

            int waitTime = 500;
            for (int i = 0; i < 10; i++)
            {
                if (SafeWait(waitTime)) { return false; }
                if (EnterFairyRing(expectedFairyRingLocation, 250))
                {
                    return true;
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

            ReadWindow();
            Color[,] testImage = ScreenPiece(left, right, top, bottom);
            DebugUtilities.SaveImageToFile(testImage);

            List<Blob> mushrooms = LocateObjects(FairyRingWhite, left, right, top, bottom, true, 1, ArtifactArea(0.0000326));   //ex 0.0000261
            if (mushrooms == null)
            {
                return null;
            }
            Blob ringCenter = new Blob();
            foreach(Blob mushroom in mushrooms)
            {
                ringCenter.AddBlob(mushroom);
            }
            return ringCenter.Center;
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
            RightClick fairyRingOptions = new RightClick(x, y, RSClient);
            if (!fairyRingOptions.WaitForPopup(5000))
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

            //SafeWait(4500); //wait for the fairy ring teleport animation
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
            FairyRingsConfigure menu = new FairyRingsConfigure(ColorArray, RSClient);
            menu.SetConfiguration('c', 'k', 'r');
            menu.Teleport(true);
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
            while (watch.ElapsedMilliseconds < 10000)
            {
                ReadWindow();
                bool[,] portal = ColorFilterPiece(FairyRingTeleport, Center, 50);
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
    }
}
