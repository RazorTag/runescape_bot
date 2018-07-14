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
        public Use14On14SettingsData Use14On14;
        public FalconrySettingsData Falconry;

        public CustomSettingsData()
        {
            LesserDemon = new LesserDemonSettingsData();
            NatureRings = new NatureRingsSettingsData();
            Use14On14 = new Use14On14SettingsData();
            Falconry = new FalconrySettingsData();
        }
    }
}
