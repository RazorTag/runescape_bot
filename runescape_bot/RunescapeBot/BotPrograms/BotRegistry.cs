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
        /// Number of milliseconds in a sngle game tick
        /// </summary>
        public const int GAME_TICK = 600;

        /// <summary>
        /// List of existing bot programs. Add a new bot program to this list.
        /// </summary>
        public enum BotActions : int
        {
            [Description("Agility - Gnome Stronghold")]
            AgilityGnomeStronghold,
            [Description("Agility - Seers' Village")]
            AgilitySeersVillage,
            [Description("Combat - Lesser Demon")]
            LesserDemon,
            [Description("Combat - Lesser Demon (no pickups)")]
            LesserDemonSimple,
            [Description("Combat - Nightmare Zone")]
            NightmareZoneD,
            [Description("Crafting - Cut Gems")]
            CutGems,
            [Description("Crafting - Gold Bracelets")]
            GoldBracelets,
            [Description("Fletching - Short Bows")]
            FletchShortBows,
            [Description("Fletching - String Bows")]
            StringBows,
            [Description("Herblore - Make Unfinished Potions")]
            MakeUnfinishedPotions,
            [Description("Herblore - Make Prepared Potions")]
            MakePotionsSimple,
            [Description("Herblore - Make Potions from Scratch")]
            MakePotionsFull,
            [Description("Magic - Enchant Level 2")]
            EnchantLevel2,
            [Description("Mining - Iron Ore")]
            IronOre,
            [Description("Smithing - Cannonballs")]
            Cannonballs,
            [Description("Thieving - Tea Stall")]
            TeaStall,
            [Description("Woodcutting - Willows (drop)")]
            WillowTrees
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
                    return new Woodcutting(runParams);
                case BotActions.TeaStall:
                    return new TeaStall(runParams);
                case BotActions.CutGems:
                    return new Use1On27(runParams, Use1On27.CUT_GEM_TIME);
                case BotActions.EnchantLevel2:
                    return new Enchant(runParams, 2);
                case BotActions.IronOre:
                    return new IronPowerMining(runParams);
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
