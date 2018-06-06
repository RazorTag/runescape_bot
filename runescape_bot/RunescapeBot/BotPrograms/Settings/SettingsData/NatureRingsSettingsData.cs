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

        public NatureRingsSettingsData()
        {

        }
    }
}
