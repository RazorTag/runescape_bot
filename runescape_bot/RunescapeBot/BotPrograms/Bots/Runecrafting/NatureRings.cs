using RunescapeBot.BotPrograms.Popups;
using RunescapeBot.BotPrograms.Settings;
using RunescapeBot.Common;
using RunescapeBot.ImageTools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RunescapeBot.BotPrograms
{
    public class NatureRings : BotProgram
    {
        const int STAMINA_DURATION = 120000; //duraion of a dose of stamina potion in milliseconds
        Stopwatch StaminaTimer;

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

        bool LowStamina, DepletedGlory, DamagedPouches;

        public NatureRings(RunParams startParams) : base(startParams)
        {
            RunParams.Run = true;
            RunParams.RunLoggedIn = true;
            UserSelections = startParams.CustomSettingsData.NatureRings;
            LowStamina = true;
            StaminaTimer = new Stopwatch();

            SetItemSlots();
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

            return true;
        }

        protected override bool Execute()
        {
            if (!Bank() || !MoveToFairyRing())
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
            if (!Minimap.WaitDuringMovement(8000)) { return false; }

            return RefreshItems();
        }

        /// <summary>
        /// Teleports to Edgeville and moves to the Edgeville bank
        /// </summary>
        /// <returns>true if successful</returns>
        protected bool MoveToEdgevilleBank()
        {
            //teleport to Edgeville bank
            Inventory.GloryTeleport(Inventory.GloryTeleports.Edgeville, true);

            //Start moving to bank booth
            Point bankIconOffsetTarget = new Point(2 * MinimapGauge.GRID_SQUARE_SIZE, MinimapGauge.GRID_SQUARE_SIZE);
            MoveToBank(0, true, 4, 2, bankIconOffsetTarget);

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

            return true;
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
        protected bool PouchIsDamaged(int x, int y)
        {
            //TODO determine if a pouch is damaged
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

        /// <summary>
        /// Moves to the fairy ring chosen by the user
        /// </summary>
        /// <returns>true if successful</returns>
        protected bool MoveToFairyRing()
        {
            switch(UserSelections.FairyRing)
            {
                case NatureRingsSettingsData.FairyRingOptions.Edgeville:
                    return MoveToFairyRingEdgeville();
                default:
                    return false;
            }
        }

        /// <summary>
        /// Moves from the Edgeville bank to the Edgeville fairy ring
        /// </summary>
        /// <returns>true if successful</returns>
        protected bool MoveToFairyRingEdgeville()
        {
            //TODO
            return false;
        }
    }
}
