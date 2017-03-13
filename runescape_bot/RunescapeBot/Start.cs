using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;
using RunescapeBot.BotPrograms;

namespace RunescapeBot
{
    public partial class Start : Form
    {
        /// <summary>
        /// List of running bot programs
        /// </summary>
        private List<BotProgram> RunningBots;

        /// <summary>
        /// Set to true if a user has selected a date to run until
        /// </summary>
        private bool EndTimeSelected;

        /// <summary>
        /// List of existing bot programs. Add a new bot program to this list.
        /// </summary>
        public enum BotActions : int {
                [Description("Lesser Demon")]
                LesserDemon,
                [Description("Gold Bracelets")]
                GoldBracelets
            };

        /// <summary>
        /// Gets the display name for an enum
        /// </summary>
        /// <param name="value"></param>
        /// <returns>The description attribute from the given enum value</returns>
        public string GetDescription(Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    DescriptionAttribute attr =
                           Attribute.GetCustomAttribute(field,
                             typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Initializes the controls on the startup form
        /// </summary>
        public Start()
        {
            InitializeComponent();
            Array actions = Enum.GetValues(typeof(BotActions));
            string[] names = new string[actions.Length];
            for (int i = 0; i < actions.Length; i++)
            {
                names[i] = GetDescription((Enum) actions.GetValue(i));
            }
            BotActionSelect.DataSource = names;
            RunningBots = new List<BotProgram>();
        }

        /// <summary>
        /// Starts a bot program when the user presses Start
        /// You can specify timeout and/or iterations for bot program
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        private void StartButton_Click(object sender, EventArgs e)
        {
            StartParams startParams = CollectStartParams();
            BotProgram botProgram = null;

            switch ((BotActions)BotActionSelect.SelectedIndex)
            {
                case BotActions.GoldBracelets:
                    botProgram = new GoldBracelets(startParams);
                    break;

                case BotActions.LesserDemon:
                    startParams.FrameTime = 5000;
                    startParams.RandomizeFrames = true;
                    botProgram = new LesserDemon(startParams);
                    break;

                default:
                    break;
            }

            RunBotProgram(botProgram);
        }

        /// <summary>
        /// Gets the start parameters specified by the user in the startup form
        /// </summary>
        /// <returns></returns>
        private StartParams CollectStartParams()
        {
            StartParams startParams = new StartParams();
            startParams.username = Username.Text;
            startParams.RunUntil = RunUntil.Value;
            startParams.Iterations = (int) Iterations.Value;
            if (EndTimeSelected)
            {
                startParams.EndTime = RunUntil.Value;
            }
            else
            {
                startParams.EndTime = DateTime.MaxValue;
            }
            
            return startParams;
        }

        /// <summary>
        /// Starts the chosen bot program
        /// </summary>
        /// <param name="botProgram">bot program to start</param>
        private void RunBotProgram(BotProgram botProgram)
        {
            if(botProgram == null) { return; }

            botProgram.Start(RunningBots);
        }

        /// <summary>
        /// Flags the end time as selected when a user selects a DateTime
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RunUntil_ValueChanged(object sender, EventArgs e)
        {
            if (RunUntil.Value < DateTime.Now)
            {
                EndTimeSelected = true;
            }
        }

        /// <summary>
        /// Stops execution of any remaining bot programs before closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Start_FormClosing(object sender, FormClosingEventArgs e)
        {
            while (RunningBots.Count > 0)
            {
                RunningBots[RunningBots.Count - 1].Stop();
            }
        }
    }
}
