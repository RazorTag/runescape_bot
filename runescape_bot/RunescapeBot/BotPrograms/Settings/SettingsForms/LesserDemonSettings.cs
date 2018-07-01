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
    public partial class LesserDemonSettings : Form
    {
        private LesserDemonSettingsData settings;


        public LesserDemonSettings(CustomSettingsData settingsData)
        {
            InitializeComponent();

            settings = settingsData.LesserDemon;
            LoadPriorSelections();
        }

        /// <summary>
        /// Populates the checkboxes with the previous selections
        /// </summary>
        private void LoadPriorSelections()
        {
            TelegrabSelection.Checked = settings.Telegrab;
            AlchSelection.Checked = settings.HighAlch;
        }

        /// <summary>
        /// Saves the user's selections
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveSettings_Click(object sender, EventArgs e)
        {
            settings.Telegrab = TelegrabSelection.Checked;
            settings.HighAlch = AlchSelection.Checked;

            Close();
        }
    }
}
