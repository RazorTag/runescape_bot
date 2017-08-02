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
        /// The Start form's status for the bot idling
        /// </summary>
        private bool BotIdle;

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
            if (RunningBot == null || RunParams.ActiveBot == null)
            {
                return string.Empty;
            }

            return GetDescription(RunParams.ActiveBot.BotAction);
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
            SetPhasmatysBotSelector(RunParams.SelectedPhasmatysBot);
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
        private void SetPhasmatysBotSelector(int selection)
        {
            string[] botnames = new string[RunParams.PhasmatysParams.Count];
            for (int i = 0; i < botnames.Length; i++)
            {
                botnames[i] = ((PhasmatysRunParams)RunParams.PhasmatysParams[i]).BotName;
            }
            PhasmatysBotSelector.DataSource = botnames;
            PhasmatysBotSelection = selection;
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
            RunParams.BotManager = BotRegistry.BotManager.Standard;
            StartButtonClicked((Button)sender);
        }

        /// <summary>
        /// Starts a bot program when the user presses Start
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        private void PhasmatysStartButton_Click(object sender, EventArgs e)
        {
            RunParams.BotManager = BotRegistry.BotManager.Phasmatys;
            StartButtonClicked((Button)sender);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="botManager"></param>
        private void StartButtonClicked(Button sender)
        {
            if (BotIsRunning)
            {
                StopBot(sender);
                return;
            }
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
            RunningBot = BotRegistry.GetSelectedBot(RunParams, RunParams.BotManager, (BotRegistry.BotActions)BotActionSelect.SelectedIndex);
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
                RunningBot.Stop(false);
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
                    SaveBot();
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
                RunningBot.Stop(false);
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
            RunParams.BotManager = BotRegistry.BotManager.None;
            StatusMessage.Text = "";
            PhasmatysStatus.Text = "";

            foreach (Button button in StartButtons)
            {
                button.Text = "Start";
                button.BackColor = ColorTranslator.FromHtml("#527E3F");
                button.Enabled = true;
            }

            BringToFront();
            WindowState = FormWindowState.Normal;
        }

        /// <summary>
        /// Sets up the Start form for a transitional state
        /// </summary>
        private void SetTransitionalState()
        {
            Text = GetBotName() + " " + BOT_STOPPING;
            RunParams.BotManager = BotRegistry.BotManager.None;
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
            if (RunningBot == null || RunParams.ActiveBot == null) { return; }

            //make sure that the date for the RunUntil field doesn't exceed the max value for that field
            RunUntil.Value = RunParams.ActiveBot.RunUntil < RunUntil.MaxDate ? RunParams.ActiveBot.RunUntil : RunUntil.MaxDate;
                        
            string currentState, nextState;
            SetRotationFormData();
            GetBotStates(out currentState, out nextState);
            UpdateStatusMessage(GetActiveTextBox(), currentState, nextState);
            HideShowStartForm();
        }

        /// <summary>
        /// Sets any new dropdown values for the bot selection dropdowns
        /// </summary>
        private void SetRotationFormData()
        {
            switch (RunParams.BotManager)
            {
                case BotRegistry.BotManager.Rotation:
                    //TODO set bot selector
                    break;
                case BotRegistry.BotManager.Phasmatys:
                    SetPhasmatysBotSelector(RunParams.PhasmatysParams.ActiveBot);
                    WritePhasmatysSettings();
                    break;
                default:
                    Iterations.Value = RunParams.ActiveBot.Iterations;
                    break;
            }
        }

        /// <summary>
        /// Determines which TextBox should have its status message updated the the state of the running bot
        /// </summary>
        /// <returns>the TextBox to update</returns>
        private TextBox GetActiveTextBox()
        {
            switch (RunParams.BotManager)
            {
                case BotRegistry.BotManager.Rotation:
                    return StatusMessage;  //TODO change to the text bow for the standard rotation manager tab
                case BotRegistry.BotManager.Phasmatys:
                    return PhasmatysStatus;
                default:
                    return StatusMessage;
            }
        }

        /// <summary>
        /// Updates the status message in the specified text box
        /// </summary>
        /// <param name="activeTextBox">the text box to write in</param>
        /// <param name="currentState">the running, brea or sleep state off the bot</param>
        /// <param name="nextState">the state that the bot will hit after completing th current state</param>
        private void UpdateStatusMessage(TextBox activeTextBox, string currentState, string nextState)
        {
            Text = GetBotName();
            Text += " " + currentState;
            TimeSpan timeRemaining = (DateTime.Now < RunParams.ActiveBot.CurrentStateEnd) ? (RunParams.ActiveBot.CurrentStateEnd - DateTime.Now) : TimeSpan.Zero;
            string stateTimeRemaining = timeRemaining.ToString(@"hh\:mm\:ss");
            Text += " (" + stateTimeRemaining + ")";

            string stateStartTime = RunParams.ActiveBot.CurrentStateStart.ToString("MM/dd/yyyy h:mm tt");
            string stateEndTime = RunParams.ActiveBot.CurrentStateEnd.ToString("MM/dd/yyyy h:mm tt");
            activeTextBox.Text = GetBotName() + " " + currentState + "." + " It has " + stateTimeRemaining + " left before " + nextState + ".";
            activeTextBox.Text += " It entered this state at " + stateStartTime + " and will complete this state at " + stateEndTime + ".";
        }

        /// <summary>
        /// Shows or hides the start from base on whether the current bot is idling
        /// </summary>
        private void HideShowStartForm()
        {
            //Show the start form for periods when the bot is idling
            if (BotIdle != RunParams.ActiveBot.BotIdle)
            {
                BotIdle = RunParams.ActiveBot.BotIdle;
                if (BotIdle)
                {
                    WindowState = FormWindowState.Normal;
                    BringToFront();
                }
                else
                {
                    SendToBack();
                    WindowState = FormWindowState.Minimized;
                }
            }
        }

        /// <summary>
        /// Gets the status strings for the current next states of the running bot
        /// </summary>
        /// <param name="currentState"></param>
        /// <param name="nextState"></param>
        private void GetBotStates(out string currentState, out string nextState)
        {
            switch (RunParams.ActiveBot.BotState)
            {
                case BotProgram.BotState.Running:
                    currentState = BOT_RUNNING;
                    nextState = STOPPING;
                    break;

                case BotProgram.BotState.Break:
                    currentState = BOT_TAKING_A_BREAK;
                    nextState = RESUMING;
                    break;

                case BotProgram.BotState.Sleep:
                    currentState = BOT_SLEEPING;
                    nextState = RESUMING;
                    break;

                default:
                    currentState = BOT_RUNNING;
                    nextState = STOPPING;
                    break;
            }
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
            SetPhasmatysBotSelector(PhasmatysBotSelector.SelectedIndex);
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
