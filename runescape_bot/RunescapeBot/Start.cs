using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;
using RunescapeBot.BotPrograms;
using RunescapeBot.UITools;
using RunescapeBot.FileIO;
using RunescapeBot.ImageTools;

namespace RunescapeBot
{
    /// <summary>
    /// Used by the bot to inform that is has completed its task
    /// </summary>
    //public delegate void BotResponse();

    public partial class Start : Form
    {
        private const string FORM_NAME = "Roboport";
        private const string BOT_STOPPING = " is stopping";
        private const string BOT_RUNNING = " is running";

        /// <summary>
        /// List of running bot programs
        /// </summary>
        private BotProgram RunningBot;

        /// <summary>
        /// Saved startup settings for each bot program
        /// </summary>
        private BotSettings Settings;

        /// <summary>
        /// True if a bot is currently running
        /// </summary>
        private bool BotIsRunning;

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
        private string GetDescription(Enum value)
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
        /// Gets the bot's user-facing status
        /// </summary>
        /// <returns>The bot's display name and current status</returns>
        private string GetBotName()
        {
            if (RunningBot == null)
            {
                return string.Empty;
            }

            return GetDescription(RunningBot.RunParams.BotAction);
        }

        /// <summary>
        /// Initializes the controls on the startup form
        /// </summary>
        public Start()
        {
            Settings = new BotSettings();
            InitializeComponent();
            Array actions = Enum.GetValues(typeof(BotActions));
            string[] names = new string[actions.Length];
            for (int i = 0; i < actions.Length; i++)
            {
                names[i] = GetDescription((Enum) actions.GetValue(i));
            }
            BotActionSelect.DataSource = names;
            BotActionSelect.SelectedIndex = (int) Settings.BotAction;
            Login.Text = Settings.Login;
            Password.Text = Settings.Password;
        }

        /// <summary>
        /// Starts a bot program when the user presses Start
        /// You can specify timeout and/or iterations for bot program
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        private void StartButton_Click(object sender, EventArgs e)
        {
            if (BotIsRunning)
            {
                MessageBox.Show("There is already a bot running.");
                return;
            }

            StartParams startParams = CollectStartParams();

            switch ((BotActions)BotActionSelect.SelectedIndex)
            {
                case BotActions.GoldBracelets:
                    RunningBot = new GoldBracelets(startParams);
                    break;

                case BotActions.LesserDemon:
                    startParams.FrameTime = 5000;
                    RunningBot = new LesserDemon(startParams);
                    break;

                default:
                    return;
            }

            RunBotProgram(RunningBot);
        }

        /// <summary>
        /// Gets the start parameters specified by the user in the startup form
        /// </summary>
        /// <returns></returns>
        private StartParams CollectStartParams()
        {
            StartParams startParams = new StartParams();
            startParams.Login = Login.Text;
            startParams.Password = Password.Text;
            startParams.Iterations = (int) Iterations.Value;
            if (EndTimeSelected)
            {
                startParams.RunUntil = RunUntil.Value;
            }
            else
            {
                startParams.RunUntil = DateTime.MaxValue;
            }
            startParams.TaskComplete = new BotResponse(BotDone);

            return startParams;
        }

        /// <summary>
        /// Starts the chosen bot program
        /// </summary>
        /// <param name="botProgram">bot program to start</param>
        private void RunBotProgram(BotProgram botProgram)
        {
            if(botProgram == null) { return; }

            this.Text = GetBotName() + BOT_RUNNING;
            botProgram.Start();
            BotIsRunning = true;
            Settings.SaveBot(botProgram.RunParams);
        }

        /// <summary>
        /// Flags the end time as selected when a user selects a DateTime
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RunUntil_ValueChanged(object sender, EventArgs e)
        {
            EndTimeSelected = true;
        }

        /// <summary>
        /// Stops execution of any remaining bot programs before closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Start_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (RunningBot != null)
            {
                RunningBot.Stop();
            }
        }

        /// <summary>
        /// Respond to the bot stopping
        /// </summary>
        public void BotDone()
        {
            BotIsRunning = false;
            RunningBot = null;
            if (this.IsHandleCreated)
            {
                this.Invoke((MethodInvoker)(() =>
                {
                    this.Text = FORM_NAME;
                    User32.RECT windowRect = new User32.RECT();
                    User32.GetWindowRect(this.Handle, ref windowRect);
                    MouseActions.MoveMouse(windowRect.left + 150, windowRect.top + 15);
                }));
            }
        }

        /// <summary>
        /// Stops the currently running bot if one exists
        /// </summary>
        private void StopBot()
        {
            User32.SetForegroundWindow(Handle.ToInt32());

            if (RunningBot != null)
            {
                this.Text = GetBotName() + BOT_STOPPING;
                RunningBot.Stop();
            }
            else
            {
                this.Text = FORM_NAME;
            }
        }

        /// <summary>
        /// Responds to keyboard input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GlobalEventProvider_KeyPress(object sender, KeyPressEventArgs e)
        {
            int keyCode = (int) e.KeyChar;

            switch (keyCode)
            {
                case (int) Keys.Escape:
                    StopBot();
                    break;
            }
        }

        /// <summary>
        /// Saves the user's form settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Start_FormClosed(object sender, FormClosedEventArgs e)
        {
            Settings.Save();
        }
    }
}
