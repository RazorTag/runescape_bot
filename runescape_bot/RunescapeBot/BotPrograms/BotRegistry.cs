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
            [Description("Smithing - Cannonballs")]
            Cannonballs,
            [Description("Thieving - Tea Stall")]
            TeaStall,
            [Description("Woodcutting - Willows (drop)")]
            WillowTrees
        };


        public static BotProgram GetSelectedBot(RunParams runParams, BotActions selectedBot)
        {
            BotProgram bot = null;

            switch (selectedBot)
            {
                case BotActions.LesserDemon:
                    bot = new LesserDemon(runParams);
                    break;
                case BotActions.LesserDemonSimple:
                    bot = new LesserDemon(runParams, false, false);
                    break;
                case BotActions.GoldBracelets:
                    bot = new GoldBracelets(runParams);
                    break;
                case BotActions.Cannonballs:
                    bot = new Cannonballs(runParams);
                    break;
                case BotActions.NightmareZoneD:
                    bot = new NightmareZoneD(runParams);
                    break;
                case BotActions.FletchShortBows:
                    bot = new Use1On27(runParams, 50000);
                    break;
                case BotActions.StringBows:
                    bot = new Use14On14(runParams, 16800);
                    break;
                case BotActions.MakeUnfinishedPotions:
                    bot = new UnfinishedPotions(runParams);
                    break;
                case BotActions.MakePotionsSimple:
                    bot = new Use14On14(runParams, 8400);
                    break;
                case BotActions.MakePotionsFull:
                    bot = new MakePotionFull(runParams);
                    break;
                case BotActions.AgilityGnomeStronghold:
                    bot = new AgilityGnomeStronghold(runParams);
                    break;
                case BotActions.AgilitySeersVillage:
                    bot = new AgilitySeersVillage(runParams);
                    break;
                case BotActions.WillowTrees:
                    bot = new Woodcutting(runParams);
                    break;
                case BotActions.TeaStall:
                    bot = new TeaStall(runParams);
                    break;
                case BotActions.CutGems:
                    bot = new Use1On27(runParams, 32400);
                    break;
                case BotActions.EnchantLevel2:
                    bot = new Enchant(runParams, 2);
                    break;
            }
            return bot;
        }
    }
}
