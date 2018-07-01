using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RunescapeBot.BotPrograms.Settings
{
    [Serializable]
    public class CustomSettingsData
    {
        public LesserDemonSettingsData LesserDemon;
        public NatureRingsSettingsData NatureRings;

        public CustomSettingsData()
        {
            LesserDemon = new LesserDemonSettingsData();
            NatureRings = new NatureRingsSettingsData();
        }
    }
}
