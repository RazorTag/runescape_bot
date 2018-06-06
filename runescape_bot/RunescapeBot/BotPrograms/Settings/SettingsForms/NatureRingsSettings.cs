using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RunescapeBot.BotPrograms.Settings
{
    public partial class NatureRingsSettings : Form
    {
        private NatureRingsSettingsData settings;

        public NatureRingsSettings(CustomSettingsData settingsData)
        {
            InitializeComponent();

            settings = settingsData.NatureRings;
            LoadPriorSelections();
        }

        /// <summary>
        /// Populates the combo boxes with saved values
        /// </summary>
        private void LoadPriorSelections()
        {
            FairyRingSelect.SelectedIndex = (int)settings.FairyRing;
            GloryTypeSelect.SelectedIndex = (int)settings.GloryType;
        }

        /// <summary>
        /// Saves the user-selected custom settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveSettings_Click(object sender, EventArgs e)
        {
            settings.FairyRing = (NatureRingsSettingsData.FairyRingOptions) FairyRingSelect.SelectedIndex;
            settings.GloryType = (NatureRingsSettingsData.GloryOptions) GloryTypeSelect.SelectedIndex;
            Close();
        }
    }
}
