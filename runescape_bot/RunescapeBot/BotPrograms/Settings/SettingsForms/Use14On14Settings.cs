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
    public partial class Use14On14Settings : Form
    {
        private Use14On14SettingsData settings;

        public Use14On14Settings(CustomSettingsData settingsData)
        {
            InitializeComponent();

            settings = settingsData.Use14On14;
            LoadPriorSelections();
        }

        private void LoadPriorSelections()
        {
            SingleItemMakeTime.Value = settings.MakeTime;
        }

        private void SaveSelections()
        {
            settings.MakeTime = (int) SingleItemMakeTime.Value;
        }

        private void SaveSettings_Click(object sender, EventArgs e)
        {
            SaveSelections();
            Close();
        }
    }
}
