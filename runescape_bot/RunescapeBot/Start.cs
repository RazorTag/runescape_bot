using RunescapeBot.BotPrograms;
using RunescapeBot.FileIO;
using RunescapeBot.UITools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace RunescapeBot
{
    /// <summary>
    /// Used by the bot to inform that is has completed its task
    /// </summary>

    public partial class Start : Form
    {
        private const string FORM_NAME = "Roboport";
        private const string BOT_STOPPING = "is stopping";
        private const string BOT_RUNNING = "is running";
        private const string BOT_TAKING_A_BREAK = "is taking a break";
        private const string BOT_SLEEPING = "is sleeping";
        private const string STOPPING = "stopping for a break";
        private const string RESUMING = "getting back to work";

        /// <summary>
        /// Saved start form settings
        /// </summary>
        private BotSettings Settings;

        /// <summary>
        /// List of running bot programs
        /// </summary>
        private BotProgram RunningBot;

        /// <summary>
        /// Start parameters and state information
        /// </summary>
        private RunParams RunParams;

        /// <summary>
        /// True if a bot is currently running
        /// </summary>
        private bool BotIsRunning;

        /// <summary>
        /// Indicates which tab is currently running a bot
        /// Set to -1 if not bot is running
        /// </summary>
        private BotRegistry.BotManager BotManager;

        /// <summary>
        /// Start button for each bot manager
        /// </summary>
        private List<Button> StartButtons;

        /// <summary>
        /// The most recently selected phasmatys bot.
        /// Used to save for data to the bot before selecting a new bot.
        /// </summary>
        private int PhasmatysBotSelection;


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
            InitializeComponent();

            Array actions = Enum.GetValues(typeof(BotRegistry.BotActions));
            string[] names = new string[actions.Length];
            for (int i = 0; i < actions.Length; i++)
            {
                names[i] = GetDescription((Enum)actions.GetValue(i));
            }
            BotActionSelect.DataSource = names;

            StartButtons = new List<Button>();
            StartButtons.Add(StartButton);
            StartButtons.Add(PhasmatysStartButton);

            Settings = new BotSettings();
            RunParams = Settings.LoadSettings();
            BotManagerType.SelectedIndex = RunParams.SelectedTab;
            SetSoloBotForm();

            //Phasmatys rotation bot manager
            PhasmatysBotSelection = RunParams.SelectedPhasmatysBot;
            SetPhasmatysBotSelector();
            WritePhasmatysSettings();

            SetIdleState();
        }

        /// <summary>
        /// Sets field values for the solo bot form
        /// </summary>
        private void SetSoloBotForm()
        {
            Login.Text = RunParams.Login;
            Password.Text = RunParams.Password;
            JagexClientLocation.Text = RunParams.JagexClient;
            OSBuddyClientLocation.Text = RunParams.OSBuddyClient;
            BotActionSelect.SelectedIndex = (int)RunParams.BotAction;
            Iterations.Value = RunParams.Iterations;
        }

        /// <summary>
        /// Sets the list for the Phasmatys bot selector
        /// </summary>
        private void SetPhasmatysBotSelector()
        {
            string[] botnames = new string[RunParams.PhasmatysParams.Count];
            for (int i = 0; i < botnames.Length; i++)
            {
                botnames[i] = ((PhasmatysRunParams)RunParams.PhasmatysParams[i]).BotName;
            }
            PhasmatysBotSelector.DataSource = botnames;
            PhasmatysBotSelector.SelectedIndex = PhasmatysBotSelection;
        }

        /// <summary>
        /// Starts a bot program when the user presses Start
        /// You can specify timeout and/or iterations for bot program
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        private void StartButton_Click(object sender, EventArgs e)
        {
            StartButtonClicked((Button)sender, BotRegistry.BotManager.Standard);
        }

        /// <summary>
        /// Starts a bot program when the user presses Start
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        private void PhasmatysStartButton_Click(object sender, EventArgs e)
        {
            StartButtonClicked((Button)sender, BotRegistry.BotManager.Phasmatys);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="botManager"></param>
        private void StartButtonClicked(Button sender, BotRegistry.BotManager botManager)
        {
            if (BotIsRunning)
            {
                StopBot(sender);
                return;
            }
            BotManager = botManager;
            RunBotProgram(sender);
        }

        /// <summary>
        /// Gets the start parameters specified by the user in the startup form
        /// </summary>
        /// <returns></returns>
        private void CollectStartParams()
        {
            CollectGeneralSettings();
            if (PhasmatysBotSelection >= 0 && PhasmatysBotSelection < RunParams.PhasmatysParams.Count)
            {
                RunParams.PhasmatysParams[PhasmatysBotSelection] = CollectPhasmatysSettings();
            }
        }

        /// <summary>
        /// Collects start parameters from solo bot form and general settings
        /// </summary>
        /// <returns></returns>
        private void CollectGeneralSettings()
        {
            RunParams = RunParams ?? new RunParams();
            RunParams.Login = Login.Text;
            RunParams.Password = Password.Text;
            RunParams.Iterations = (int)Iterations.Value;
            RunParams.RunUntil = RunUntil.Value;
            RunParams.TaskComplete = new BotResponse(BotDone);
            RunParams.BotAction = (BotRegistry.BotActions)BotActionSelect.SelectedIndex;
            RunParams.JagexClient = JagexClientLocation.Text;
            RunParams.OSBuddyClient = OSBuddyClientLocation.Text;
        }

        /// <summary>
        /// Collects the start parameters for the Phasmatys rotation manager form
        /// </summary>
        /// <returns></returns>
        private PhasmatysRunParams CollectPhasmatysSettings()
        {
            PhasmatysRunParams phasmatysParams = new PhasmatysRunParams();
            phasmatysParams.BotName = PhasmatysBotSelector.Text;
            phasmatysParams.Login = PhasmatysLogin.Text;
            phasmatysParams.Password = PhasmatysPassword.Text;
            phasmatysParams.GoldBars = (int)GoldBars.Value;
            phasmatysParams.SteelBars = (int)SteelBars.Value;
            phasmatysParams.Bows = (int)Bows.Value;
            return phasmatysParams;
        }

        /// <summary>
        /// Populates the Phasmatys manager form with the values of the selected bot
        /// </summary>
        private void WritePhasmatysSettings()
        {
            PhasmatysRunParams settings = (PhasmatysRunParams)RunParams.PhasmatysParams[PhasmatysBotSelection];
            PhasmatysLogin.Text = settings.Login;
            PhasmatysPassword.Text = settings.Password;
            GoldBars.Value = settings.GoldBars;
            SteelBars.Value = settings.SteelBars;
            Bows.Value = settings.Bows;
        }

        /// <summary>
        /// Starts the chosen bot program
        /// </summary>
        /// <param name="botProgram">bot program to start</param>
        private void RunBotProgram(Button startButton)
        {
            CollectStartParams();
            RunningBot = BotRegistry.GetSelectedBot(RunParams, BotManager, (BotRegistry.BotActions)BotActionSelect.SelectedIndex);
            SetActiveState(startButton);
            RunningBot.Start();
            UpdateTimer.Enabled = true;
            SaveBot();
        }

        /// <summary>
        /// Saves the current form data to disk
        /// </summary>
        private void SaveBot()
        {
            RunParams.SelectedTab = BotManagerType.SelectedIndex;
            RunParams.SelectedPhasmatysBot = PhasmatysBotSelection;
            if (!Settings.SaveSettings(RunParams))
            {
                MessageBox.Show("Unable to save bot settings");
            }
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
            if (IsHandleCreated)
            {
                Invoke((MethodInvoker)(() =>
                {
                    SetIdleState();
                    StartButton.Enabled = true;
                }));
            }
        }

        /// <summary>
        /// Stops the currently running bot if one exists
        /// </summary>
        private void StopBot(Button startButton)
        {
            User32.SetForegroundWindow(Handle.ToInt32());
            UpdateTimer.Enabled = false;

            if (RunningBot != null)
            {
                StartButton.Enabled = false;
                SetTransitionalState();
                RunningBot.Stop();
                UpdateTimer_Tick(null, null);
            }
            else
            {
                SetIdleState();
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
                    StopBot(null);
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
            CollectStartParams();
            SaveBot();
        }

        /// <summary>
        /// Sets up the Start form for the idle state
        /// </summary>
        private void SetIdleState()
        {
            RunningBot = null;
            BotIsRunning = false;
            Text = FORM_NAME;
            BotManager = BotRegistry.BotManager.None;
            StatusMessage.Text = "";
            PhasmatysStatus.Text = "";

            foreach (Button button in StartButtons)
            {
                button.Text = "Start";
                button.BackColor = ColorTranslator.FromHtml("#527E3F");
                button.Enabled = true;
            }
        }

        /// <summary>
        /// Sets up the Start for for a transitional state
        /// </summary>
        private void SetTransitionalState()
        {
            Text = GetBotName() + " " + BOT_STOPPING;
            BotManager = BotRegistry.BotManager.None;
            StatusMessage.Text = "";
            PhasmatysStatus.Text = "";

            foreach (Button button in StartButtons)
            {
                button.Text = "";
                button.BackColor = ColorTranslator.FromHtml("#7E7E37");
            }
        }

        /// <summary>
        /// Sets up the Start form for when a bot is running
        /// </summary>
        private void SetActiveState(Button startButton)
        {
            if (startButton == null) { return; }

            BotIsRunning = true;
            Text = GetBotName() + " " + BOT_RUNNING;

            //Disabled other start buttons
            foreach (Button button in StartButtons)
            {
                button.Text = "";
                button.BackColor = Color.Gray;
                button.Enabled = false;
            }
            startButton.Text = "Stop";
            startButton.BackColor = ColorTranslator.FromHtml("#874C48");
            startButton.Enabled = true;

            StatusMessage.Text = "";
            PhasmatysStatus.Text = "";
        }

        /// <summary>
        /// Opens a file dialog for the user to select the OSBuddy executable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OSBuddySelect_Click(object sender, EventArgs e)
        {
            DialogResult result = FileSelect.ShowDialog();
            if (result == DialogResult.OK)
            {
                OSBuddyClientLocation.Text = FileSelect.FileName;
            }
        }

        /// <summary>
        /// Opens a file dialog for the user to select the Jagex client executable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void JagexClientSelect_Click(object sender, EventArgs e)
        {
            DialogResult result = FileSelect.ShowDialog();
            if (result == DialogResult.OK)
            {
                JagexClientLocation.Text = FileSelect.FileName;
            }
        }

        /// <summary>
        /// Updates the form with the status of the running bot
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            if (RunningBot == null) { return; }

            //Show the start form for periods when the bot is idling
            if (TopMost != RunParams.BotIdle)
            {
                TopMost = RunParams.BotIdle;
                if (TopMost)
                {
                    BringToFront();
                    WindowState = FormWindowState.Normal;
                }
                else
                {
                    SendToBack();
                    WindowState = FormWindowState.Minimized;
                }
            }

            //make sure that the date for the RunUntil field doesn't exceed the max value for that field
            RunUntil.Value = RunningBot.RunParams.RunUntil < RunUntil.MaxDate ? RunningBot.RunParams.RunUntil : RunUntil.MaxDate;
            Iterations.Value = RunningBot.RunParams.Iterations;

            Text = GetBotName();
            string botState = "";
            string nextState = "";
            switch (RunningBot.RunParams.BotState)
            {
                case BotProgram.BotState.Running:
                    botState = BOT_RUNNING;
                    nextState = STOPPING;
                    break;

                case BotProgram.BotState.Break:
                    botState = BOT_TAKING_A_BREAK;
                    nextState = RESUMING;
                    break;

                case BotProgram.BotState.Sleep:
                    botState = BOT_SLEEPING;
                    nextState = RESUMING;
                    break;

                default:
                    botState = BOT_RUNNING;
                    nextState = STOPPING;
                    break;
            }
            Text += " " + botState;
            TimeSpan timeRemaining = (DateTime.Now < RunningBot.RunParams.CurrentStateEnd) ? (RunningBot.RunParams.CurrentStateEnd - DateTime.Now) : TimeSpan.Zero;
            string stateTimeRemaining = timeRemaining.ToString(@"hh\:mm\:ss");
            Text += " (" + stateTimeRemaining + ")";

            string stateStartTime = RunningBot.RunParams.CurrentStateStart.ToString("MM/dd/yyyy h:mm tt");
            string stateEndTime = RunningBot.RunParams.CurrentStateEnd.ToString("MM/dd/yyyy h:mm tt"); 
            StatusMessage.Text = GetBotName() + " " + botState + "." + " It has " + stateTimeRemaining + " left before " + nextState + ".";
            StatusMessage.Text += " It entered this state at " + stateStartTime + " and will complete this state at " + stateEndTime + ".";
        }

        /// <summary>
        /// Saves the currently selected Phasmatys bot
        /// </summary>
        private void SavePhasmatysBot()
        {
            if (PhasmatysBotSelection >= 0)
            {
                RunParams.PhasmatysParams[PhasmatysBotSelection] = CollectPhasmatysSettings();
            }
            PhasmatysBotSelection = PhasmatysBotSelector.SelectedIndex;
            SetPhasmatysBotSelector();
        }

        /// <summary>
        /// Saves the current bot and loads the new one
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PhasmatysBotSelector_SelectionChangeCommitted(object sender, EventArgs e)
        {
            SavePhasmatysBot();
            WritePhasmatysSettings();
        }
    }
}
