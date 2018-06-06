using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms
{
    public class BotRegistry
    {
        /// <summary>
        /// Number of milliseconds in a single game tick
        /// </summary>
        public const int GAME_TICK = 603;   //theoretically 600 ms per game tick, but the server tends to be slightly slower

        /// <summary>
        /// List of existing bot programs. Add a new bot program to this list.
        /// </summary>
        public enum BotActions : int
        {
            [Description("Log In")]
            Login,
            [Description("Agility - Gnome Stronghold")]
            AgilityGnomeStronghold,
            [Description("Agility - Seers' Village")]
            AgilitySeersVillage,
            [Description("Combat - Lesser Demon (Wizards' Tower)")]
            LesserDemon,
            [Description("Combat - Lesser Demon (no pickups)")]
            LesserDemonSimple,
            [Description("Combat - Nightmare Zone")]
            NightmareZoneD,
            [Description("Construction - Butler Sawmill (Camelot PvP)")]
            ButlerSawmill,
            [Description("Crafting - Cut Gems (Varrock west)")]
            CutGems,
            [Description("Crafting - Gold Bracelets (Phasmatys)")]
            GoldBracelets,
            [Description("Firemaking - (Phasmatys)")]
            Firemaking,
            [Description("Fishing - Barbarian Fishing (drop)")]
            BarbarianFishing,
            [Description("Fletching - Short Bows (Varrock west)")]
            FletchShortBows,
            [Description("Fletching - String Bows (Varrock west)")]
            StringBows,
            [Description("Herblore - Make Unfinished Potions (Varrock west)")]
            MakeUnfinishedPotions,
            [Description("Herblore - Make Prepared Potions (Varrock west)")]
            MakePotionsSimple,
            [Description("Herblore - Make Potions from Scratch (Varrock west)")]
            MakePotionsFull,
            [Description("Magic - Enchant Level 2 (Varrock west)")]
            EnchantLevel2,
            [Description("Magic - Tan Hides (Varrock west)")]
            TanHides,
            [Description("Mining - Iron Ore")]
            IronOre,
            [Description("Mining - Motherlode Mine")]
            MotherlodeMine,
            [Description("Runecrafting - Fairy Ring Nature Runes")]
            NatureRings,
            [Description("Smithing - Cannonballs (Phasmatys)")]
            Cannonballs,
            [Description("Thieving - Tea Stall")]
            TeaStall,
            [Description("Thieving - Knight of Ardougne")]
            KnightOfArdougne,
            [Description("Woodcutting - Willows (drop)")]
            WillowTrees,
            [Description("Do Nothing")]
            DoNothing
        };


        public static BotProgram GetSelectedBot(RunParams runParams)
        {
            BotProgram bot = null;

            switch (runParams.BotAction)
            {
                case BotActions.LesserDemon:
                    return new LesserDemon(runParams);
                case BotActions.LesserDemonSimple:
                    return new LesserDemon(runParams, false, false);
                case BotActions.GoldBracelets:
                    return new GoldBracelets(runParams);
                case BotActions.Cannonballs:
                    return new Cannonballs(runParams);
                case BotActions.NightmareZoneD:
                    return new NightmareZoneD(runParams);
                case BotActions.FletchShortBows:
                    return new Use1On27(runParams, Use14On14.FLETCH_BOW_TIME);
                case BotActions.StringBows:
                    return new Use14On14(runParams, Use14On14.STRING_BOW_TIME);
                case BotActions.MakeUnfinishedPotions:
                    return new UnfinishedPotions(runParams);
                case BotActions.MakePotionsSimple:
                    return new Use14On14(runParams, Herblore.MAKE_UNFINISHED_POTION_TIME);
                case BotActions.MakePotionsFull:
                    return new MakePotionFull(runParams);
                case BotActions.AgilityGnomeStronghold:
                    return new AgilityGnomeStronghold(runParams);
                case BotActions.AgilitySeersVillage:
                    return new AgilitySeersVillage(runParams);
                case BotActions.WillowTrees:
                    return new WillowChopping(runParams);
                case BotActions.TeaStall:
                    return new TeaStall(runParams);
                case BotActions.CutGems:
                    return new Use1On27(runParams, Use1On27.CUT_GEM_TIME);
                case BotActions.EnchantLevel2:
                    return new Enchant(runParams, 2);
                case BotActions.IronOre:
                    return new IronPowerMining(runParams);
                case BotActions.Login:
                    return new LogInToGame(runParams);
                case BotActions.DoNothing:
                    return new DoNothing(runParams);
                case BotActions.ButlerSawmill:
                    return new ButlerSawmill(runParams);
                case BotActions.BarbarianFishing:
                    return new BarbarianFishing(runParams);
                case BotActions.Firemaking:
                    return new Firemaking(runParams);
                case BotActions.KnightOfArdougne:
                    return new KnightOfArdougne(runParams);
                case BotActions.TanHides:
                    return new TanHides(runParams);
                case BotActions.MotherlodeMine:
                    return new MotherlodeMine(runParams);
                case BotActions.NatureRings:
                    return new NatureRings(runParams);
            }
            return bot;
        }

        /// <summary>
        /// List of managers to run bot programs
        /// </summary>
        public enum BotManager : int
        {
            None,
            Standard,
            Rotation,
            Phasmatys
        }

        /// <summary>
        /// Creates an instance of the selected bot
        /// </summary>
        /// <param name="runParams"></param>
        /// <param name="botManager"></param>
        /// <returns></returns>
        public static BotProgram GetSelectedBot(RunParams runParams, BotManager botManager)
        {
            switch (botManager)
            {
                case BotManager.Standard:
                    return GetSelectedBot(runParams);
                case BotManager.Rotation:
                    return new SimpleRotation(runParams, runParams.RotationParams);
                case BotManager.Phasmatys:
                    return new PhasmatysRotation(runParams, runParams.PhasmatysParams);
                default:
                    return null;
            }
        }
    }
}
