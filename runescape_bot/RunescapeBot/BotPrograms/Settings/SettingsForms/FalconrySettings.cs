using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RunescapeBot.BotPrograms.Settings.SettingsForms
{
    public partial class FalconrySettings : Form
    {
        private FalconrySettingsData settings;


        public FalconrySettings(CustomSettingsData settingsData)
        {
            InitializeComponent();

            settings = settingsData.Falconry;
            LoadPriorSelections();
        }


        private void LoadPriorSelections()
        {
            CatchSpottedKebbits.Checked = settings.CatchSpottedKebbits;
            CatchDarkKebbits.Checked = settings.CatchDarkKebbits;
            CatchDashingKebbits.Checked = settings.CatchDashingKebbits;
        }

        private void SaveSettings_Click(object sender, EventArgs e)
        {
            settings.CatchSpottedKebbits = CatchSpottedKebbits.Checked;
            settings.CatchDarkKebbits = CatchDarkKebbits.Checked;
            settings.CatchDashingKebbits = CatchDashingKebbits.Checked;

            Close();
        }
    }
}
