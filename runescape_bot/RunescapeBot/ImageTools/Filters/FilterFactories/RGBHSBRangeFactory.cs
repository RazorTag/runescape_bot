using System.Drawing;

namespace RunescapeBot.ImageTools
{
    public static class RGBHSBRangeFactory
    {
        #region login
        /// <summary>
        /// Creates the color range to represent a lesser demon's red skin
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange WelcomeScreenClickHere()
        {
            Color dark = Color.FromArgb(25, 0, 0);
            Color light = Color.FromArgb(140, 70, 50);
            HSBRange hsbRange = new HSBRange(355, 20, 0.4f, 1f, 0.05f, 0.5f);
            return new RGBHSBRange(dark, light, hsbRange);
        }
        #endregion

        #region monsters
        /// <summary>
        /// Creates the color range to represent a lesser demon's red skin
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange LesserDemonSkin()
        {
            Color dark = Color.FromArgb(25, 5, 0);
            Color light = Color.FromArgb(140, 70, 50);
            HSBRange hsbRange = new HSBRange(355, 20, 0.4f, 1f, 0.05f, 0.5f);
            return new RGBHSBRange(dark, light, hsbRange);

        }

        /// <summary>
        /// Creates the color range to represent a lesser demon's horns, hoofs, and tail spike
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange LesserDemonHorn()
        {
            Color dark = Color.FromArgb(0, 0, 0);
            Color light = Color.FromArgb(30, 30, 30);
            HSBRange hsbRange = new HSBRange(355, 5, 0.030f, 0.400f, 0.020f, 0.120f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The purple cloak of a knight of Ardougne
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange KnightPurple()
        {
            Color dark = Color.FromArgb(28, 0, 62);
            Color light = Color.FromArgb(144, 39, 251);
            HSBRange hsbRange = new HSBRange(262.6f, 275.6f, 0.724f, 0.933f, 0.109f, 0.579f);
            return new RGBHSBRange(dark, light, hsbRange);
        }
        #endregion

        #region stationary objcts

        /// <summary>
        /// The gray top of a furnace
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange Furnace()
        {
            Color dark = Color.FromArgb(46, 43, 43);
            Color light = Color.FromArgb(85, 80, 78);
            HSBRange hsbRange = new HSBRange(0, 360, 0.02f, 0.07f, 0.18f, 0.32f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The white tarp above a tea stall
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange TeaStallRoof()
        {
            Color dark = Color.FromArgb(173, 159, 135);
            Color light = Color.FromArgb(203, 189, 168);
            HSBRange hsbRange = new HSBRange(31, 44, 0.169f, 0.260f, 0.603f, 0.728f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// Neutral purple from a house portal.
        /// Does not cover all of the purple shades seen in the portal
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange HousePortalPurple()
        {
            Color dark = Color.FromArgb(59, 15, 78);
            Color light = Color.FromArgb(141, 78, 176);
            HSBRange hsbRange = new HSBRange(273.6f, 288.7f, 0.293f, 0.724f, 0.152f, 0.529f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// Brown wood from a bank chest (like at castle wars)
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange BankChest()
        {
            Color dark = Color.FromArgb(30, 21, 14);
            Color light = Color.FromArgb(156, 118, 87);
            HSBRange hsbRange = new HSBRange(23.0f, 29.2f, 0.270f, 0.322f, 0.050f, 0.507f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// White mushroom parts in a fairy ring
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange FairyRingMushroom()
        {
            Color dark = Color.FromArgb(177, 167, 167);
            Color light = Color.FromArgb(255, 255, 255);
            HSBRange hsbRange = new HSBRange(-1f, 1f, 0.000f, 0.168f, 0.614f, 1.000f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// Red flowers seen during the fairy ring teleport animation
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange FairyRingTeleport()
        {
            Color dark = Color.FromArgb(180, 46, 17);
            Color light = Color.FromArgb(251, 113, 99);
            HSBRange hsbRange = new HSBRange(-3f, 19f, 0.603f, 0.801f, 0.375f, 0.587f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The gray of a runcrafting altar (exterior or interior)
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange RunecraftingAltar()
        {
            Color dark = Color.FromArgb(0, 0, 0);
            Color light = Color.FromArgb(255, 255, 255);
            HSBRange hsbRange = new HSBRange(0, 360, 0.000f, 0.140f, 0.093f, 0.539f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The green glow of a runcrafting altar
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange NatureAltarGlow()
        {
            Color dark = Color.FromArgb(44, 50, 40);
            Color light = Color.FromArgb(140, 145, 132);
            HSBRange hsbRange = new HSBRange(73, 135, 0.001f, 0.141f, 0.166f, 0.554f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        #endregion

        #region bank

        /// <summary>
        /// The dark yellow 0 shown as the item counter for an empty bank slot placeholder
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange BankSlotPlaceholderZero()
        {
            Color dark = Color.FromArgb(141, 137, 14);
            Color light = Color.FromArgb(179, 175, 87);
            HSBRange hsbRange = new HSBRange(51.146f, 61.345f, 0.225f, 0.851f, 0.278f, 0.547f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The teller side counter of a Port Phasmatys bank booth
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange BankBoothPhasmatys()
        {
            Color dark = Color.FromArgb(50, 55, 19);
            Color light = Color.FromArgb(72, 76, 37);
            HSBRange hsbRange = new HSBRange(64, 72, 0.340f, 0.480f, 0.127f, 0.242f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The teller side counter of a Port Phasmatys bank booth
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange BankBoothVarrockWest()
        {
            Color dark = Color.FromArgb(78, 67, 34);
            Color light = Color.FromArgb(101, 87, 46);
            HSBRange hsbRange = new HSBRange(43, 47, 0.358f, 0.395f, 0.217f, 0.290f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The teller side counter of a Port Phasmatys bank booth
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange BankBoothSeersVillage()
        {
            Color dark = Color.FromArgb(95, 80, 52);
            Color light = Color.FromArgb(105, 90, 62);
            HSBRange hsbRange = new HSBRange(37, 41, 0.244f, 0.304f, 0.278f, 0.338f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The dark gray bar on the front of an Edgeville bank booth
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange BankBoothEdgeville()
        {
            Color dark = Color.FromArgb(55, 50, 50);
            Color light = Color.FromArgb(65, 60, 60);
            HSBRange hsbRange = new HSBRange(-2, 2, 0.036f, 0.051f, 0.191f, 0.260f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        #endregion

        #region agility obstacles

        /// <summary>
        /// The teller side counter of a Port Phasmatys bank booth
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange LogBalance()
        {
            Color dark = Color.FromArgb(61, 33, 0);
            Color light = Color.FromArgb(110, 68, 14);
            HSBRange hsbRange = new HSBRange(26, 40, 0.728f, 1.000f, 0.079f, 0.274f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The wooden frame for a cargo net
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange CargoNetFrameTop()
        {
            Color dark = Color.FromArgb(90, 54, 8);
            Color light = Color.FromArgb(92, 56, 10);
            HSBRange hsbRange = new HSBRange(33, 35, 0.815f, 0.825f, 0.191f, 0.201f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// Climbable tree branch from the Gnome Stronghold agility course
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange GnomeTreeBranch()
        {
            Color dark = Color.FromArgb(52, 39, 8);
            Color light = Color.FromArgb(147, 121, 71);
            HSBRange hsbRange = new HSBRange(37, 44, 0.355f, 0.668f, 0.127f, 0.418f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The cutout of the tree trunk for a climbable tree
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange GnomeTreeTrunk()
        {
            Color dark = Color.FromArgb(75, 59, 39);
            Color light = Color.FromArgb(148, 120, 86);
            HSBRange hsbRange = new HSBRange(30, 37, 0.263f, 0.302f, 0.233f, 0.449f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// Traversable tightrope
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange Tightrope()
        {
            Color dark = Color.FromArgb(119, 95, 65);
            Color light = Color.FromArgb(174, 142, 101);
            HSBRange hsbRange = new HSBRange(31, 36, 0.264f, 0.308f, 0.370f, 0.530f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// THe rim of the opening of a drain pipe
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange DrainPipe()
        {
            Color dark = Color.FromArgb(86, 80, 80);
            Color light = Color.FromArgb(129, 121, 121);
            HSBRange hsbRange = new HSBRange(355, 12, 0.027f, 0.044f, 0.335f, 0.483f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The gray window sill on the Seers' bank wall that starts the agility course
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange SeersBankWindowSill()
        {
            Color dark = Color.FromArgb(158, 160, 146);
            Color light = Color.FromArgb(168, 170, 156);
            HSBRange hsbRange = new HSBRange(66, 72, 0.052f, 0.092f, 0.600f, 0.640f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The white flag on rooftop agility courses
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange WhiteFlag()
        {
            Color dark = Color.FromArgb(244, 244, 244);
            Color light = Color.FromArgb(254, 254, 254);
            HSBRange hsbRange = new HSBRange(-3, 3, 0.000f, 0.020f, 0.956f, 0.996f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The yellow background on a mark of grace
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange MarkOfGraceYellow()
        {
            Color dark = Color.FromArgb(159, 129, 0);
            Color light = Color.FromArgb(222, 192, 67);
            HSBRange hsbRange = new HSBRange(37, 64, 0.518f, 0.971f, 0.275f, 0.608f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The red person on a mark of grace
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange MarkOfGraceRed()
        {
            Color dark = Color.FromArgb(97, 4, 0);
            Color light = Color.FromArgb(195, 78, 82);
            HSBRange hsbRange = new HSBRange(337, 31, 0.406f, 1.000f, 0.120f, 0.494f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        #endregion

        #region items (ground)
        /// <summary>
        /// Creates the color range to represent the rune color
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange RuneMedHelm()
        {
            Color dark = Color.FromArgb(52, 70, 77);
            Color light = Color.FromArgb(95, 131, 137);
            HSBRange hsbRange = new HSBRange(190, 200, 0.18f, 0.21f, 0.245f, 0.48f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// Creates the color range to represent the mithril color
        /// we have to be careful with this filter because it also picks up water and similarly colored shades
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange MithrilArmor()
        {
            Color dark = Color.FromArgb(45, 45, 70);
            Color light = Color.FromArgb(105, 105, 130);
            HSBRange hsbRange = new HSBRange(215, 315, 0.100f, 0.200f, 0.203f, 0.460f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The orange outline on a chaos rune
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange ChaosRuneOrange()
        {
            Color dark = Color.FromArgb(218, 161, 20);
            Color light = Color.FromArgb(229, 174, 33);
            HSBRange hsbRange = new HSBRange(40, 46, 0.728f, 0.848f, 0.436f, 0.544f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The white skull and crossbones on a death rune
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange DeathRuneWhite()
        {
            Color dark = Color.FromArgb(223, 219, 219);
            Color light = Color.FromArgb(234, 230, 230);
            HSBRange hsbRange = new HSBRange(357, 3, 0.021f, 0.121f, 0.840f, 0.940f);
            return new RGBHSBRange(dark, light, hsbRange);
        }
        #endregion

        #region minimap

        /// <summary>
        /// The black person of an agility course icon
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange AgilityIconBlack()
        {
            Color dark = Color.FromArgb(0, 0, 0);
            Color light = Color.FromArgb(5, 5, 6);
            HSBRange hsbRange = new HSBRange(0, 360, 0.000f, 1.000f, 0.000f, 0.102f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The yellow background of the run energy orb when run is turned on
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange RunEnergyBackground()
        {
            Color dark = Color.FromArgb(196, 158, 0);
            Color light = Color.FromArgb(216, 178, 11);
            HSBRange hsbRange = new HSBRange(39, 59, 0.890f, 1.000f, 0.306f, 0.506f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The yellow version of the run energy foot icon
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange RunEnergyFoot()
        {
            Color dark = Color.FromArgb(197, 160, 39);
            Color light = Color.FromArgb(255, 238, 130);
            HSBRange hsbRange = new HSBRange(36, 62, 0.575f, 0.948f, 0.441f, 0.778f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The orange version of the run energy foot icon while a stamina potion is active
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange RunEnergyFootStamina()
        {
            Color dark = Color.FromArgb(208, 95, 41);
            Color light = Color.FromArgb(248, 135, 81);
            HSBRange hsbRange = new HSBRange(9, 29, 0.656f, 0.856f, 0.467f, 0.667f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// Middle yellow to green range for the gauges aroound the minimap. Roughly 50% - 100%
        /// (hitpoints, prayer points, run energy, special attack energy)
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange MinimapGaugeYellowGreen()
        {
            Color dark = Color.FromArgb(0, 163, 0);
            Color light = Color.FromArgb(194, 255, 121);
            HSBRange hsbRange = new HSBRange(67, 130, 0.174f, 1.000f, 0.400f, 0.663f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The red color of the hitpoints digits by the minimap when low on health
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange HitpointsRed()
        {
            Color dark = Color.FromArgb(250, 0, 0);
            Color light = Color.FromArgb(255, 16, 5);
            HSBRange hsbRange = new HSBRange(-1, 5, 0.990f, 1.000f, 0.490f, 0.510f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The orange part of a furnace minimap icon
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange FurnaceIconOrange()
        {
            Color dark = Color.FromArgb(249, 110, 25);
            Color light = Color.FromArgb(255, 120, 35);
            HSBRange hsbRange = new HSBRange(20.8f, 24.8f, 0.941f, 1.000f, 0.516f, 0.606f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The yellow part of a bank minimap icon
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange BankIconDollar()
        {
            Color dark = Color.FromArgb(227, 203, 69);
            Color light = Color.FromArgb(237, 213, 79);
            HSBRange hsbRange = new HSBRange(48, 54, 0.725f, 0.825f, 0.550f, 0.650f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The brown floor of a builidng on the minimap
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange PhasmatysBuildingFloorLight()
        {
            Color dark = Color.FromArgb(25, 31, 19);
            Color light = Color.FromArgb(99, 103, 82);
            HSBRange hsbRange = new HSBRange(334, 95, 0.083f, 0.191f, 0.117f, 0.363f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The brown floor of a builidng on the minimap
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange PhasmatysBuildingFloorDark()
        {
            Color dark = Color.FromArgb(25, 31, 19);
            Color light = Color.FromArgb(99, 103, 82);
            HSBRange hsbRange = new HSBRange(334, 80, 0.083f, 0.191f, 0.117f, 0.363f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The brown bridge on a minimap.
        /// A bridge consists of two distinct alternating colors which could be isolated to provide greater precision for this filter
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange BridgeIcon()
        {
            Color dark = Color.FromArgb(65, 31, 0);
            Color light = Color.FromArgb(121, 93, 54);
            HSBRange hsbRange = new HSBRange(24.2f, 42.5f, 0.332f, 1.000f, 0.047f, 0.404f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The dark blues in a fishing minimap icon
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange FishingIcon()
        {
            Color dark = Color.FromArgb(0, 47, 209);
            Color light = Color.FromArgb(19, 111, 255);
            HSBRange hsbRange = new HSBRange(212.1f, 230.8f, 0.918f, 0.980f, 0.423f, 0.528f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        #endregion

        #region Inventory

        /// <summary>
        /// Catches most of the colors found in the opaque background of the inventory
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange EmptyInventorySlot()
        {
            Color dark = Color.FromArgb(56, 48, 35);
            Color light = Color.FromArgb(66, 58, 45);
            HSBRange hsbRange = new HSBRange(28, 40, 0.15f, 0.25f, 0.15f, 0.25f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// Catches most of the colors found in the opaque background of the inventory
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange InventorySlotWindows10Watermark()
        {
            Color dark = Color.FromArgb(56, 48, 35);
            Color light = Color.FromArgb(151, 149, 146);
            HSBRange hsbRange = new HSBRange(18, 42, 0.000f, 0.25f, 0.15f, 0.700f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// Catches most of the colors found in the opaque background of the inventory
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange BackpackBrown()
        {
            Color dark = Color.FromArgb(101, 55, 9);
            Color light = Color.FromArgb(152, 94, 47);
            HSBRange hsbRange = new HSBRange(21.9f, 35.0f, 0.521f, 0.758f, 0.174f, 0.432f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// Inventory overload potion black
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange OverloadInventory()
        {
            Color dark = Color.FromArgb(16, 12, 12);
            Color light = Color.FromArgb(20, 16, 16);
            HSBRange hsbRange = new HSBRange(-5f, 5f, 0.115f, 0.135f, 0.053f, 0.073f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// Gray-brown background of the potion timer in OSBuddy
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange PotionTimerBackground()
        {
            Color dark = Color.FromArgb(66, 57, 48);
            Color light = Color.FromArgb(80, 71, 62);
            HSBRange hsbRange = new HSBRange(26.2f, 33.6f, 0.114f, 0.172f, 0.219f, 0.283f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// Timer overload potion black
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange OverloadTimer()
        {
            Color dark = Color.FromArgb(9, 8, 8);
            Color light = Color.FromArgb(13, 12, 12);
            HSBRange hsbRange = new HSBRange(-5f, 5f, 0.038f, 0.058f, 0.031f, 0.051f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// Inventory apsorption potion light blue
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange Absorption()
        {
            Color dark = Color.FromArgb(76, 93, 100);
            Color light = Color.FromArgb(205, 213, 217);
            HSBRange hsbRange = new HSBRange(192.5f, 210.7f, 0.074f, 0.152f, 0.323f, 0.849f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// Inventory apsorption potion light blue
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange RunecraftingPouchUndamaged()
        {
            Color dark = Color.FromArgb(55, 43, 16);
            Color light = Color.FromArgb(65, 53, 26);
            HSBRange hsbRange = new HSBRange(38.5f, 44.5f, 0.431f, 0.531f, 0.109f, 0.209f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// Inventory apsorption potion light blue
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange RunecraftingPouchDamaged()
        {
            Color dark = Color.FromArgb(33, 33, 0);
            Color light = Color.FromArgb(43, 43, 5);
            HSBRange hsbRange = new HSBRange(57, 63, 0.950f, 1.000f, 0.025f, 0.125f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        #endregion

        #region textBoxes

        /// <summary>
        /// The gray background color of a right-click menu
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange RightClickPopup()
        {
            Color dark = Color.FromArgb(93, 84, 71);
            Color light = Color.FromArgb(93, 84, 71);
            HSBRange hsbRange = new HSBRange(35.355f, 35.555f, 0.133f, 0.135f, 0.321f, 0.323f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The orange title text in the bank screen.
        /// Also applies to NPC Contact popup title.
        /// Also applies to lots of inventory text.
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange BankTitle()
        {
            Color dark = Color.FromArgb(253, 150, 29);
            Color light = Color.FromArgb(255, 154, 33);
            HSBRange hsbRange = new HSBRange(31, 33, 0.995f, 1.000f, 0.556f, 0.566f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The orange text for an item in an inventory right click menu
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange InventoryRightClickItemName()
        {
            Color dark = Color.FromArgb(245, 139, 59);
            Color light = Color.FromArgb(255, 149, 69);
            HSBRange hsbRange = new HSBRange(23, 27, 0.990f, 1.000f, 0.615f, 0.635f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The red text for a dialog box title
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange DialogBoxTitle()
        {
            Color dark = Color.FromArgb(128, 0, 0);
            Color light = Color.FromArgb(128, 0, 0);
            HSBRange hsbRange = new HSBRange(359.0f, 1.0f, 0.995f, 1.000f, 0.246f, 0.256f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The blue text for a dialog box body
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange DialogBoxBodyBlue()
        {
            Color dark = Color.FromArgb(0, 0, 128);
            Color light = Color.FromArgb(0, 0, 128);
            HSBRange hsbRange = new HSBRange(239.0f, 241.0f, 0.995f, 1.000f, 0.246f, 0.256f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The tan background in the chat box.
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange ChatBoxBackground()
        {
            Color dark = Color.FromArgb(139, 126, 99);
            Color light = Color.FromArgb(221, 204, 171);
            HSBRange hsbRange = new HSBRange(34.1f, 47.4f, 0.109f, 0.425f, 0.456f, 0.779f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        #endregion

        #region OSBuddy
        /// <summary>
        /// The gray background on the OSBuddy popup in the top-left of the screen that shows the current target's hitpoints
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange OSBuddyEnemyHitpointsBackground()
        {
            Color dark = Color.FromArgb(66, 60, 52);
            Color light = Color.FromArgb(68, 62, 54);
            HSBRange hsbRange = new HSBRange(33, 35, 0.116f, 0.118f, 0.234f, 0.236f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The green portion of the health bar on the OSBuddy popup in the top-left of the screen that shows the current target's hitpoints
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange OSBuddyEnemyHitpointsGreen()
        {
            Color dark = Color.FromArgb(45, 121, 35);
            Color light = Color.FromArgb(47, 123, 37);
            HSBRange hsbRange = new HSBRange(112, 114, 0.543f, 0.545f, 0.309f, 0.311f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The red portion of the health bar on the OSBuddy popup in the top-left of the screen that shows the current target's hitpoints
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange OSBuddyEnemyHitpointsRed()
        {
            Color dark = Color.FromArgb(125, 41, 35);
            Color light = Color.FromArgb(127, 43, 37);
            HSBRange hsbRange = new HSBRange(3, 5, 0.555f, 0.557f, 0.317f, 0.319f);
            return new RGBHSBRange(dark, light, hsbRange);
        }
        #endregion


        #region mouseover description

        /// <summary>
        /// The blue text in the top-left corner of the screen when mousing over a clickable stationary object
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange MouseoverTextStationaryObject()
        {
            Color dark = Color.FromArgb(0, 189, 188);
            Color light = Color.FromArgb(39, 246, 245);
            HSBRange hsbRange = new HSBRange(175.8f, 182.0f, 0.717f, 1.000f, 0.352f, 0.542f); ;
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The yellow text in the top-left corner of the screen when mousing over a clickable NPC
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange MouseoverTextNPC()
        {
            Color dark = Color.FromArgb(181, 181, 0);
            Color light = Color.FromArgb(225, 225, 16);
            HSBRange hsbRange = new HSBRange(58.0f, 62.0f, 0.970f, 1.000f, 0.336f, 0.462f); ;
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The orange-tan text in the top-left corner of the screen when mosuing over a dropped item
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange MouseoverTextDroppedItem()
        {
            Color dark = Color.FromArgb(194, 107, 44);
            Color light = Color.FromArgb(255, 167, 100);
            HSBRange hsbRange = new HSBRange(21.7f, 29.2f, 0.495f, 0.986f, 0.456f, 0.706f); ;
            return new RGBHSBRange(dark, light, hsbRange);
        }

        #endregion

        #region mining rocks

        /// <summary>
        /// The blue text in the top-left corner of the screen when mousing over a clickable stationary object
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange IronRock()
        {
            Color dark = Color.FromArgb(40, 25, 20);
            Color light = Color.FromArgb(95, 60, 45);
            HSBRange hsbRange = new HSBRange(14f, 20f, 0.35f, 0.43f, 0.09f, 0.27f) ;
            return new RGBHSBRange(dark, light, hsbRange);
        }

        #endregion

        #region trees

        /// <summary>
        /// The dark brown trunk of a willow tree
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange WillowTrunk()
        {
            Color dark = Color.FromArgb(55, 50, 29);
            Color light = Color.FromArgb(102, 95, 60);
            HSBRange hsbRange = new HSBRange(45.3f, 55.9f, 0.226f, 0.407f, 0.134f, 0.348f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        #endregion

        #region fishing
        /// <summary>
        /// classic blue color of fish sprinkles
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange FishingTile()
        {
            Color dark = Color.FromArgb(111, 151, 208);
            Color light = Color.FromArgb(231, 241, 247);
            HSBRange hsbRange = new HSBRange(200.5f, 217.5f, 0.331f, 0.711f, 0.595f, 0.968f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        public static RGBHSBRange FishingPole()
        {
            Color dark = Color.FromArgb(45, 31, 0);
            Color light = Color.FromArgb(61, 41, 0);
            HSBRange hsbRange = new HSBRange(40f, 43f, 0.98f, 1.02f, 0.089f, 0.119f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        #endregion

        #region hunter

        /// <summary>
        /// The black spots on a kebbit's back.
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange KebbitBlackSpot()
        {
            Color dark = Color.FromArgb(21, 17, 17);
            Color light = Color.FromArgb(21, 17, 17);
            HSBRange hsbRange = new HSBRange(-0.1f, 0.1f, 0.1043f, 0.1063f, 0.0735f, 0.0755f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The white spots on a kebbit's back.
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange KebbitWhiteSpot()
        {
            Color dark = Color.FromArgb(150, 138, 118);
            Color light = Color.FromArgb(213, 204, 190);
            HSBRange hsbRange = new HSBRange(-35.5f, 39.4f, 0.088f, 0.259f, 0.495f, 0.821f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The brown fur of a spotted kebbit.
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange KebbitSpottedFur()
        {
            Color dark = Color.FromArgb(50, 39, 24);
            Color light = Color.FromArgb(145, 124, 99);
            HSBRange hsbRange = new HSBRange(-31.6f, 35.6f, 0.147f, 0.427f, 0.114f, 0.509f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The brown fur of a dashing kebbit.
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange KebbitDashingFur()
        {
            Color dark = Color.FromArgb(98, 79, 53);
            Color light = Color.FromArgb(192, 156, 111);
            HSBRange hsbRange = new HSBRange(-32.0f, 35.7f, 0.223f, 0.423f, 0.266f, 0.625f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The portion of a dark kebbit's fur with a blue hue.
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange KebbitDarkFurBlue()
        {
            Color dark = Color.FromArgb(33, 33, 35);
            Color light = Color.FromArgb(75, 75, 81);
            HSBRange hsbRange = new HSBRange(235f, 245f, 0.000f, 0.096f, 0.103f, 0.336f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The portion of a dark kebbit's fur with a red hue.
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange KebbitDarkFurRed()
        {
            Color dark = Color.FromArgb(16, 12, 12);
            Color light = Color.FromArgb(98, 92, 92);
            HSBRange hsbRange = new HSBRange(-5f, 5f, 0.000f, 0.155f, 0.024f, 0.403f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        /// <summary>
        /// The flashing yellow arrow above a Gyr falcon that has caught a kebbit.
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange FlashingArrow()
        {
            Color dark = Color.FromArgb(245, 242, 29);
            Color light = Color.FromArgb(255, 255, 184);
            HSBRange hsbRange = new HSBRange(57.6f, 60.4f, 0.906f, 1.000f, 0.507f, 0.899f);
            return new RGBHSBRange(dark, light, hsbRange);
        }

        #endregion

        #region generic colors

        /// <summary>
        /// perfect black
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange Black()
        {
            Color dark = Color.FromArgb(0, 0, 0);
            Color light = Color.FromArgb(0, 0, 0);
            return new RGBHSBRange(dark, light, null);
        }

        /// <summary>
        /// perfect white
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange White()
        {
            Color dark = Color.FromArgb(255, 255, 255);
            Color light = Color.FromArgb(255, 255, 255);
            return new RGBHSBRange(dark, light, null);
        }

        /// <summary>
        /// Perfect yellow
        /// </summary>
        /// <returns></returns>
        public static RGBHSBRange Yellow()
        {
            Color dark = Color.FromArgb(255, 255, 0);
            Color light = Color.FromArgb(255, 255, 0);
            return new RGBHSBRange(dark, light, null);
        }

        /// <summary>
        /// Makes a color filter that perfectly fits the provided color
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static RGBHSBRange GenericColor(Color color)
        {
            return new RGBHSBRange(color, color, null);
        }
        #endregion
    }
}