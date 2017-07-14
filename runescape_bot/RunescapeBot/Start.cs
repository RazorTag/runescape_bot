using RunescapeBot.BotPrograms;
using RunescapeBot.FileIO;
using RunescapeBot.UITools;
using System;
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
        /// List of running bot programs
        /// </summary>
        private BotProgram RunningBot;

        /// <summary>
        /// Saved startup settings for each bot program
        /// </summary>
        private BotSettings settings;

        /// <summary>
        /// True if a bot is currently running
        /// </summary>
        private bool botIsRunning;

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
            settings = new BotSettings();
            InitializeComponent();
            Array actions = Enum.GetValues(typeof(BotRegistry.BotActions));
            string[] names = new string[actions.Length];
            for (int i = 0; i < actions.Length; i++)
            {
                names[i] = GetDescription((Enum)actions.GetValue(i));
            }
            BotActionSelect.DataSource = names;
            Login.Text = settings.Login;
            Password.Text = settings.Password;
            ClientLocation.Text = settings.ClientFilePath;
            BotActionSelect.SelectedIndex = (int)settings.BotAction;
            Iterations.Value = settings.Iterations;
            SetIdleState();
            settings.Save();
        }

        /// <summary>
        /// Starts a bot program when the user presses Start
        /// You can specify timeout and/or iterations for bot program
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        private void StartButton_Click(object sender, EventArgs e)
        {
            if (botIsRunning)
            {
                StopBot();
                return;
            }

            RunParams startParams = CollectStartParams();
            RunningBot = BotRegistry.GetSelectedBot(startParams, (BotRegistry.BotActions)BotActionSelect.SelectedIndex);
            RunBotProgram(RunningBot);
        }

        /// <summary>
        /// Gets the start parameters specified by the user in the startup form
        /// </summary>
        /// <returns></returns>
        private RunParams CollectStartParams()
        {
            RunParams startParams = new RunParams();
            startParams.Login = Login.Text;
            startParams.Password = Password.Text;
            startParams.Iterations = (int) Iterations.Value;
            startParams.RunUntil = RunUntil.Value;
            startParams.TaskComplete = new BotResponse(BotDone);
            startParams.BotAction = (BotRegistry.BotActions)BotActionSelect.SelectedIndex;
            startParams.ClientFilePath = ClientLocation.Text;

            return startParams;
        }

        /// <summary>
        /// Starts the chosen bot program
        /// </summary>
        /// <param name="botProgram">bot program to start</param>
        private void RunBotProgram(BotProgram botProgram)
        {
            if(botProgram == null) { return; }

            SetActiveState();
            botProgram.Start();
            UpdateTimer.Enabled = true;
            settings.SaveBot(botProgram.RunParams);
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
        private void StopBot()
        {
            User32.SetForegroundWindow(Handle.ToInt32());
            UpdateTimer.Enabled = false;

            if (RunningBot != null)
            {
                StartButton.Enabled = false;
                SetTransitionalState();
                settings.SaveBot(RunningBot.RunParams);
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
            RunParams botSettings = CollectStartParams();
            settings.SaveBot(botSettings);
        }

        /// <summary>
        /// Sets up the Start form for the idle state
        /// </summary>
        private void SetIdleState()
        {
            RunningBot = null;
            botIsRunning = false;
            Text = FORM_NAME;
            StartButton.Text = "Start";
            StartButton.BackColor = ColorTranslator.FromHtml("#527E3F");
            StatusMessage.Text = "";
        }

        /// <summary>
        /// Sets up the Start for for a transitional state
        /// </summary>
        private void SetTransitionalState()
        {
            Text = GetBotName() + " " + BOT_STOPPING;
            StartButton.Text = "";
            StartButton.BackColor = ColorTranslator.FromHtml("#7E7E37");
            StatusMessage.Text = "";
        }

        /// <summary>
        /// Sets up the Start form for when a bot is running
        /// </summary>
        private void SetActiveState()
        {
            botIsRunning = true;
            Text = GetBotName() + " " + BOT_RUNNING;
            StartButton.Text = "Stop";
            StartButton.BackColor = ColorTranslator.FromHtml("#874C48");
            StatusMessage.Text = "";
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
                ClientLocation.Text = FileSelect.FileName;
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
    }
}
