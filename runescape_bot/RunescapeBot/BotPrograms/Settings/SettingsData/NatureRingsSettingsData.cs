using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms.Settings
{
    [Serializable]
    public class NatureRingsSettingsData
    {
        /// <summary>
        /// Specifies which fairy ring to use in getting to Karamja
        /// </summary>
        public FairyRingOptions FairyRing;
        public enum FairyRingOptions : int
        {
            Edgeville,
            LegendsGuild
        }

        /// <summary>
        /// Specifies which type of amulet of glory will be supplied if going through Edgeville
        /// </summary>
        public GloryOptions GloryType;
        public enum GloryOptions : int
        {
            EternalGlory,
            Glory4,
            Glory6
        }

        /// <summary>
        /// Specifies a bank to use
        /// </summary>
        public BankOptions BankChoice;
        public enum BankOptions : int
        {
            Edgeville,
            CastleWars,
            CraftingGuild
        }

        /// <summary>
        /// The number of runecrafting pouches being used.
        /// Assumes that we are not skipping pouches such as using small and large but not medium.
        /// </summary>
        public int NumberOfPouches;
    }
}
