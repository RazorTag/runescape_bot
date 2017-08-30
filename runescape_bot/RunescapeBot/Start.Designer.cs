using RunescapeBot.GMA;

namespace RunescapeBot
{
    /// <summary>
    /// UI for the user to select a bot program to run
    /// </summary>
    partial class Start
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Start));
            this.FileSelect = new System.Windows.Forms.OpenFileDialog();
            this.UpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.BotManagerType = new System.Windows.Forms.TabControl();
            this.SoloBot = new System.Windows.Forms.TabPage();
            this.JagexClientLocation = new System.Windows.Forms.TextBox();
            this.JagexClientSelect = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.StatusMessage = new System.Windows.Forms.TextBox();
            this.StartButton = new System.Windows.Forms.Button();
            this.OSBuddyClientLocation = new System.Windows.Forms.TextBox();
            this.OSBuddySelect = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.BotActionSelect = new System.Windows.Forms.ComboBox();
            this.Iterations = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.RunUntil = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Password = new System.Windows.Forms.TextBox();
            this.Login = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.RotationManager = new System.Windows.Forms.TabPage();
            this.QuickLogin = new System.Windows.Forms.Button();
            this.RotationBotSelector = new System.Windows.Forms.ComboBox();
            this.label19 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.RotationStart = new System.Windows.Forms.Button();
            this.RotationBotActionSelect = new System.Windows.Forms.ComboBox();
            this.RotationIterations = new System.Windows.Forms.NumericUpDown();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.RotationPassword = new System.Windows.Forms.TextBox();
            this.RotationLogin = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.PhasmatysManager = new System.Windows.Forms.TabPage();
            this.QuickLogInPhasmatys = new System.Windows.Forms.Button();
            this.PhasmatysLogin = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.Bows = new System.Windows.Forms.NumericUpDown();
            this.PhasmatysStartButton = new System.Windows.Forms.Button();
            this.GoldBars = new System.Windows.Forms.NumericUpDown();
            this.SteelBars = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.PhasmatysStatus = new System.Windows.Forms.TextBox();
            this.PhasmatysBotSelector = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.PhasmatysPassword = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.GlobalEventProvider = new RunescapeBot.GMA.GlobalEventProvider();
            this.runParamsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.BotManagerType.SuspendLayout();
            this.SoloBot.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Iterations)).BeginInit();
            this.RotationManager.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RotationIterations)).BeginInit();
            this.PhasmatysManager.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Bows)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GoldBars)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SteelBars)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.runParamsBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // UpdateTimer
            // 
            this.UpdateTimer.Interval = 1000;
            this.UpdateTimer.Tick += new System.EventHandler(this.UpdateTimer_Tick);
            // 
            // BotManagerType
            // 
            this.BotManagerType.Controls.Add(this.SoloBot);
            this.BotManagerType.Controls.Add(this.RotationManager);
            this.BotManagerType.Controls.Add(this.PhasmatysManager);
            this.BotManagerType.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BotManagerType.Location = new System.Drawing.Point(6, 0);
            this.BotManagerType.Name = "BotManagerType";
            this.BotManagerType.SelectedIndex = 0;
            this.BotManagerType.Size = new System.Drawing.Size(475, 482);
            this.BotManagerType.TabIndex = 0;
            // 
            // SoloBot
            // 
            this.SoloBot.Controls.Add(this.JagexClientLocation);
            this.SoloBot.Controls.Add(this.JagexClientSelect);
            this.SoloBot.Controls.Add(this.label13);
            this.SoloBot.Controls.Add(this.StatusMessage);
            this.SoloBot.Controls.Add(this.StartButton);
            this.SoloBot.Controls.Add(this.OSBuddyClientLocation);
            this.SoloBot.Controls.Add(this.OSBuddySelect);
            this.SoloBot.Controls.Add(this.label4);
            this.SoloBot.Controls.Add(this.BotActionSelect);
            this.SoloBot.Controls.Add(this.Iterations);
            this.SoloBot.Controls.Add(this.label6);
            this.SoloBot.Controls.Add(this.RunUntil);
            this.SoloBot.Controls.Add(this.label1);
            this.SoloBot.Controls.Add(this.label3);
            this.SoloBot.Controls.Add(this.Password);
            this.SoloBot.Controls.Add(this.Login);
            this.SoloBot.Controls.Add(this.label2);
            this.SoloBot.Controls.Add(this.label5);
            this.SoloBot.Location = new System.Drawing.Point(4, 29);
            this.SoloBot.Name = "SoloBot";
            this.SoloBot.Padding = new System.Windows.Forms.Padding(3);
            this.SoloBot.Size = new System.Drawing.Size(467, 449);
            this.SoloBot.TabIndex = 0;
            this.SoloBot.Text = "Solo Bot";
            this.SoloBot.UseVisualStyleBackColor = true;
            // 
            // JagexClientLocation
            // 
            this.JagexClientLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.JagexClientLocation.Location = new System.Drawing.Point(84, 6);
            this.JagexClientLocation.Name = "JagexClientLocation";
            this.JagexClientLocation.Size = new System.Drawing.Size(339, 26);
            this.JagexClientLocation.TabIndex = 1;
            // 
            // JagexClientSelect
            // 
            this.JagexClientSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.JagexClientSelect.Image = ((System.Drawing.Image)(resources.GetObject("JagexClientSelect.Image")));
            this.JagexClientSelect.Location = new System.Drawing.Point(423, 5);
            this.JagexClientSelect.Name = "JagexClientSelect";
            this.JagexClientSelect.Size = new System.Drawing.Size(35, 28);
            this.JagexClientSelect.TabIndex = 2;
            this.JagexClientSelect.Text = ". . .";
            this.JagexClientSelect.UseVisualStyleBackColor = true;
            this.JagexClientSelect.Click += new System.EventHandler(this.JagexClientSelect_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(3, 10);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(54, 18);
            this.label13.TabIndex = 33;
            this.label13.Text = "Jagex:";
            // 
            // StatusMessage
            // 
            this.StatusMessage.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StatusMessage.Location = new System.Drawing.Point(6, 363);
            this.StatusMessage.Multiline = true;
            this.StatusMessage.Name = "StatusMessage";
            this.StatusMessage.ReadOnly = true;
            this.StatusMessage.Size = new System.Drawing.Size(455, 81);
            this.StatusMessage.TabIndex = 30;
            // 
            // StartButton
            // 
            this.StartButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartButton.Location = new System.Drawing.Point(169, 302);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(125, 55);
            this.StartButton.TabIndex = 10;
            this.StartButton.Text = "&Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // OSBuddyClientLocation
            // 
            this.OSBuddyClientLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OSBuddyClientLocation.Location = new System.Drawing.Point(84, 45);
            this.OSBuddyClientLocation.Name = "OSBuddyClientLocation";
            this.OSBuddyClientLocation.Size = new System.Drawing.Size(339, 26);
            this.OSBuddyClientLocation.TabIndex = 3;
            // 
            // OSBuddySelect
            // 
            this.OSBuddySelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OSBuddySelect.Image = ((System.Drawing.Image)(resources.GetObject("OSBuddySelect.Image")));
            this.OSBuddySelect.Location = new System.Drawing.Point(423, 44);
            this.OSBuddySelect.Name = "OSBuddySelect";
            this.OSBuddySelect.Size = new System.Drawing.Size(35, 28);
            this.OSBuddySelect.TabIndex = 4;
            this.OSBuddySelect.Text = ". . .";
            this.OSBuddySelect.UseVisualStyleBackColor = true;
            this.OSBuddySelect.Click += new System.EventHandler(this.OSBuddySelect_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(3, 217);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 18);
            this.label4.TabIndex = 22;
            this.label4.Text = "Run Until:";
            // 
            // BotActionSelect
            // 
            this.BotActionSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BotActionSelect.FormattingEnabled = true;
            this.BotActionSelect.Location = new System.Drawing.Point(84, 168);
            this.BotActionSelect.MaxDropDownItems = 12;
            this.BotActionSelect.Name = "BotActionSelect";
            this.BotActionSelect.Size = new System.Drawing.Size(374, 28);
            this.BotActionSelect.TabIndex = 7;
            // 
            // Iterations
            // 
            this.Iterations.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Iterations.Location = new System.Drawing.Point(84, 255);
            this.Iterations.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.Iterations.Name = "Iterations";
            this.Iterations.Size = new System.Drawing.Size(374, 26);
            this.Iterations.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(3, 49);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 18);
            this.label6.TabIndex = 28;
            this.label6.Text = "OSBuddy:";
            // 
            // RunUntil
            // 
            this.RunUntil.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RunUntil.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.RunUntil.Location = new System.Drawing.Point(84, 212);
            this.RunUntil.Name = "RunUntil";
            this.RunUntil.Size = new System.Drawing.Size(374, 26);
            this.RunUntil.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 174);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 18);
            this.label1.TabIndex = 20;
            this.label1.Text = "Action:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, 257);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 18);
            this.label3.TabIndex = 24;
            this.label3.Text = "Iterations:";
            // 
            // Password
            // 
            this.Password.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Password.Location = new System.Drawing.Point(84, 127);
            this.Password.Name = "Password";
            this.Password.PasswordChar = '*';
            this.Password.Size = new System.Drawing.Size(374, 26);
            this.Password.TabIndex = 6;
            // 
            // Login
            // 
            this.Login.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Login.Location = new System.Drawing.Point(84, 86);
            this.Login.Name = "Login";
            this.Login.Size = new System.Drawing.Size(374, 26);
            this.Login.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 92);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 18);
            this.label2.TabIndex = 16;
            this.label2.Text = "Login:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(3, 133);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 18);
            this.label5.TabIndex = 18;
            this.label5.Text = "Password:";
            // 
            // RotationManager
            // 
            this.RotationManager.Controls.Add(this.QuickLogin);
            this.RotationManager.Controls.Add(this.RotationBotSelector);
            this.RotationManager.Controls.Add(this.label19);
            this.RotationManager.Controls.Add(this.textBox1);
            this.RotationManager.Controls.Add(this.RotationStart);
            this.RotationManager.Controls.Add(this.RotationBotActionSelect);
            this.RotationManager.Controls.Add(this.RotationIterations);
            this.RotationManager.Controls.Add(this.label15);
            this.RotationManager.Controls.Add(this.label16);
            this.RotationManager.Controls.Add(this.RotationPassword);
            this.RotationManager.Controls.Add(this.RotationLogin);
            this.RotationManager.Controls.Add(this.label17);
            this.RotationManager.Controls.Add(this.label18);
            this.RotationManager.Location = new System.Drawing.Point(4, 29);
            this.RotationManager.Name = "RotationManager";
            this.RotationManager.Size = new System.Drawing.Size(467, 449);
            this.RotationManager.TabIndex = 2;
            this.RotationManager.Text = "Rotation";
            this.RotationManager.UseVisualStyleBackColor = true;
            // 
            // QuickLogin
            // 
            this.QuickLogin.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.QuickLogin.Location = new System.Drawing.Point(404, 31);
            this.QuickLogin.Name = "QuickLogin";
            this.QuickLogin.Size = new System.Drawing.Size(58, 28);
            this.QuickLogin.TabIndex = 12;
            this.QuickLogin.Text = "Log In";
            this.QuickLogin.UseVisualStyleBackColor = true;
            this.QuickLogin.Click += new System.EventHandler(this.QuickLogin_Click);
            // 
            // RotationBotSelector
            // 
            this.RotationBotSelector.FormattingEnabled = true;
            this.RotationBotSelector.Location = new System.Drawing.Point(89, 31);
            this.RotationBotSelector.MaxDropDownItems = 3;
            this.RotationBotSelector.Name = "RotationBotSelector";
            this.RotationBotSelector.Size = new System.Drawing.Size(309, 28);
            this.RotationBotSelector.TabIndex = 11;
            this.RotationBotSelector.SelectionChangeCommitted += new System.EventHandler(this.RotationBotSelector_SelectionChangeCommitted);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(7, 35);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(36, 18);
            this.label19.TabIndex = 44;
            this.label19.Text = "Bot:";
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(7, 363);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(455, 81);
            this.textBox1.TabIndex = 42;
            // 
            // RotationStart
            // 
            this.RotationStart.Font = new System.Drawing.Font("Arial Narrow", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RotationStart.Location = new System.Drawing.Point(171, 292);
            this.RotationStart.Name = "RotationStart";
            this.RotationStart.Size = new System.Drawing.Size(125, 55);
            this.RotationStart.TabIndex = 16;
            this.RotationStart.Text = "&Start";
            this.RotationStart.UseVisualStyleBackColor = true;
            this.RotationStart.Click += new System.EventHandler(this.RotationStart_Click);
            // 
            // RotationBotActionSelect
            // 
            this.RotationBotActionSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RotationBotActionSelect.FormattingEnabled = true;
            this.RotationBotActionSelect.Location = new System.Drawing.Point(88, 188);
            this.RotationBotActionSelect.MaxDropDownItems = 12;
            this.RotationBotActionSelect.Name = "RotationBotActionSelect";
            this.RotationBotActionSelect.Size = new System.Drawing.Size(374, 28);
            this.RotationBotActionSelect.TabIndex = 14;
            // 
            // RotationIterations
            // 
            this.RotationIterations.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RotationIterations.Location = new System.Drawing.Point(88, 238);
            this.RotationIterations.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.RotationIterations.Name = "RotationIterations";
            this.RotationIterations.Size = new System.Drawing.Size(374, 26);
            this.RotationIterations.TabIndex = 15;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(7, 194);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(56, 18);
            this.label15.TabIndex = 39;
            this.label15.Text = "Action:";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(7, 240);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(75, 18);
            this.label16.TabIndex = 41;
            this.label16.Text = "Iterations:";
            // 
            // RotationPassword
            // 
            this.RotationPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RotationPassword.Location = new System.Drawing.Point(88, 142);
            this.RotationPassword.Name = "RotationPassword";
            this.RotationPassword.PasswordChar = '*';
            this.RotationPassword.Size = new System.Drawing.Size(374, 26);
            this.RotationPassword.TabIndex = 13;
            // 
            // RotationLogin
            // 
            this.RotationLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RotationLogin.Location = new System.Drawing.Point(88, 93);
            this.RotationLogin.Name = "RotationLogin";
            this.RotationLogin.Size = new System.Drawing.Size(374, 26);
            this.RotationLogin.TabIndex = 12;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(7, 99);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(51, 18);
            this.label17.TabIndex = 37;
            this.label17.Text = "Login:";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(7, 148);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(82, 18);
            this.label18.TabIndex = 38;
            this.label18.Text = "Password:";
            // 
            // PhasmatysManager
            // 
            this.PhasmatysManager.Controls.Add(this.QuickLogInPhasmatys);
            this.PhasmatysManager.Controls.Add(this.PhasmatysLogin);
            this.PhasmatysManager.Controls.Add(this.label10);
            this.PhasmatysManager.Controls.Add(this.Bows);
            this.PhasmatysManager.Controls.Add(this.PhasmatysStartButton);
            this.PhasmatysManager.Controls.Add(this.GoldBars);
            this.PhasmatysManager.Controls.Add(this.SteelBars);
            this.PhasmatysManager.Controls.Add(this.label9);
            this.PhasmatysManager.Controls.Add(this.label8);
            this.PhasmatysManager.Controls.Add(this.label7);
            this.PhasmatysManager.Controls.Add(this.PhasmatysStatus);
            this.PhasmatysManager.Controls.Add(this.PhasmatysBotSelector);
            this.PhasmatysManager.Controls.Add(this.label11);
            this.PhasmatysManager.Controls.Add(this.PhasmatysPassword);
            this.PhasmatysManager.Controls.Add(this.label12);
            this.PhasmatysManager.Location = new System.Drawing.Point(4, 29);
            this.PhasmatysManager.Name = "PhasmatysManager";
            this.PhasmatysManager.Padding = new System.Windows.Forms.Padding(3);
            this.PhasmatysManager.Size = new System.Drawing.Size(467, 449);
            this.PhasmatysManager.TabIndex = 1;
            this.PhasmatysManager.Text = "Phasmatys";
            this.PhasmatysManager.UseVisualStyleBackColor = true;
            // 
            // QuickLogInPhasmatys
            // 
            this.QuickLogInPhasmatys.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.QuickLogInPhasmatys.Location = new System.Drawing.Point(400, 16);
            this.QuickLogInPhasmatys.Name = "QuickLogInPhasmatys";
            this.QuickLogInPhasmatys.Size = new System.Drawing.Size(61, 28);
            this.QuickLogInPhasmatys.TabIndex = 18;
            this.QuickLogInPhasmatys.Text = "Log In";
            this.QuickLogInPhasmatys.UseVisualStyleBackColor = true;
            this.QuickLogInPhasmatys.Click += new System.EventHandler(this.QuickLogInPhasmatys_Click);
            // 
            // PhasmatysLogin
            // 
            this.PhasmatysLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PhasmatysLogin.Location = new System.Drawing.Point(88, 67);
            this.PhasmatysLogin.Name = "PhasmatysLogin";
            this.PhasmatysLogin.Size = new System.Drawing.Size(373, 26);
            this.PhasmatysLogin.TabIndex = 18;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(6, 71);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(51, 18);
            this.label10.TabIndex = 54;
            this.label10.Text = "Login:";
            // 
            // Bows
            // 
            this.Bows.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Bows.Location = new System.Drawing.Point(88, 255);
            this.Bows.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.Bows.Name = "Bows";
            this.Bows.Size = new System.Drawing.Size(373, 26);
            this.Bows.TabIndex = 22;
            // 
            // PhasmatysStartButton
            // 
            this.PhasmatysStartButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PhasmatysStartButton.Location = new System.Drawing.Point(168, 287);
            this.PhasmatysStartButton.Name = "PhasmatysStartButton";
            this.PhasmatysStartButton.Size = new System.Drawing.Size(125, 55);
            this.PhasmatysStartButton.TabIndex = 23;
            this.PhasmatysStartButton.Text = "&Start";
            this.PhasmatysStartButton.UseVisualStyleBackColor = true;
            this.PhasmatysStartButton.Click += new System.EventHandler(this.PhasmatysStartButton_Click);
            // 
            // GoldBars
            // 
            this.GoldBars.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GoldBars.Location = new System.Drawing.Point(88, 166);
            this.GoldBars.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.GoldBars.Name = "GoldBars";
            this.GoldBars.Size = new System.Drawing.Size(373, 26);
            this.GoldBars.TabIndex = 20;
            // 
            // SteelBars
            // 
            this.SteelBars.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SteelBars.Location = new System.Drawing.Point(88, 210);
            this.SteelBars.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.SteelBars.Name = "SteelBars";
            this.SteelBars.Size = new System.Drawing.Size(373, 26);
            this.SteelBars.TabIndex = 21;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(6, 257);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(73, 18);
            this.label9.TabIndex = 50;
            this.label9.Text = "Bows (u):";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(6, 212);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(85, 18);
            this.label8.TabIndex = 48;
            this.label8.Text = "Steel Bars:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(6, 168);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(82, 18);
            this.label7.TabIndex = 46;
            this.label7.Text = "Gold Bars:";
            // 
            // PhasmatysStatus
            // 
            this.PhasmatysStatus.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PhasmatysStatus.Location = new System.Drawing.Point(6, 362);
            this.PhasmatysStatus.Multiline = true;
            this.PhasmatysStatus.Name = "PhasmatysStatus";
            this.PhasmatysStatus.ReadOnly = true;
            this.PhasmatysStatus.Size = new System.Drawing.Size(455, 81);
            this.PhasmatysStatus.TabIndex = 45;
            // 
            // PhasmatysBotSelector
            // 
            this.PhasmatysBotSelector.FormattingEnabled = true;
            this.PhasmatysBotSelector.Location = new System.Drawing.Point(88, 16);
            this.PhasmatysBotSelector.MaxDropDownItems = 3;
            this.PhasmatysBotSelector.Name = "PhasmatysBotSelector";
            this.PhasmatysBotSelector.Size = new System.Drawing.Size(306, 28);
            this.PhasmatysBotSelector.TabIndex = 17;
            this.PhasmatysBotSelector.SelectionChangeCommitted += new System.EventHandler(this.PhasmatysBotSelector_SelectionChangeCommitted);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(6, 16);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(36, 18);
            this.label11.TabIndex = 31;
            this.label11.Text = "Bot:";
            // 
            // PhasmatysPassword
            // 
            this.PhasmatysPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PhasmatysPassword.Location = new System.Drawing.Point(88, 116);
            this.PhasmatysPassword.Name = "PhasmatysPassword";
            this.PhasmatysPassword.PasswordChar = '*';
            this.PhasmatysPassword.Size = new System.Drawing.Size(373, 26);
            this.PhasmatysPassword.TabIndex = 19;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(6, 120);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(82, 18);
            this.label12.TabIndex = 33;
            this.label12.Text = "Password:";
            // 
            // GlobalEventProvider
            // 
            this.GlobalEventProvider.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.GlobalEventProvider_KeyPress);
            // 
            // runParamsBindingSource
            // 
            this.runParamsBindingSource.DataSource = typeof(RunescapeBot.BotPrograms.RunParams);
            // 
            // Start
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 485);
            this.Controls.Add(this.BotManagerType);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "Start";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Roboport";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Start_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Start_FormClosed);
            this.BotManagerType.ResumeLayout(false);
            this.SoloBot.ResumeLayout(false);
            this.SoloBot.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Iterations)).EndInit();
            this.RotationManager.ResumeLayout(false);
            this.RotationManager.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RotationIterations)).EndInit();
            this.PhasmatysManager.ResumeLayout(false);
            this.PhasmatysManager.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Bows)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GoldBars)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SteelBars)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.runParamsBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private GlobalEventProvider GlobalEventProvider;
        private System.Windows.Forms.OpenFileDialog FileSelect;
        private System.Windows.Forms.Timer UpdateTimer;
        private System.Windows.Forms.TabControl BotManagerType;
        private System.Windows.Forms.TabPage SoloBot;
        private System.Windows.Forms.TextBox StatusMessage;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.TextBox OSBuddyClientLocation;
        private System.Windows.Forms.Button OSBuddySelect;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox BotActionSelect;
        private System.Windows.Forms.NumericUpDown Iterations;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker RunUntil;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox Password;
        private System.Windows.Forms.TextBox Login;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabPage PhasmatysManager;
        private System.Windows.Forms.TextBox PhasmatysStatus;
        private System.Windows.Forms.Button PhasmatysStartButton;
        private System.Windows.Forms.TextBox PhasmatysPassword;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox PhasmatysBotSelector;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown Bows;
        private System.Windows.Forms.NumericUpDown GoldBars;
        private System.Windows.Forms.NumericUpDown SteelBars;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox PhasmatysLogin;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.BindingSource runParamsBindingSource;
        private System.Windows.Forms.TextBox JagexClientLocation;
        private System.Windows.Forms.Button JagexClientSelect;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TabPage RotationManager;
        private System.Windows.Forms.ComboBox RotationBotSelector;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button RotationStart;
        private System.Windows.Forms.ComboBox RotationBotActionSelect;
        private System.Windows.Forms.NumericUpDown RotationIterations;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox RotationPassword;
        private System.Windows.Forms.TextBox RotationLogin;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Button QuickLogin;
        private System.Windows.Forms.Button QuickLogInPhasmatys;
    }
}

