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
            this.GlobalEventProvider = new RunescapeBot.GMA.GlobalEventProvider();
            this.FileSelect = new System.Windows.Forms.OpenFileDialog();
            this.UpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.BotManagerType = new System.Windows.Forms.TabControl();
            this.SoloBot = new System.Windows.Forms.TabPage();
            this.StatusMessage = new System.Windows.Forms.TextBox();
            this.StartButton = new System.Windows.Forms.Button();
            this.ClientLocation = new System.Windows.Forms.TextBox();
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
            this.PhasmatysManager = new System.Windows.Forms.TabPage();
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
            this.runParamsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.BotManagerType.SuspendLayout();
            this.SoloBot.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Iterations)).BeginInit();
            this.PhasmatysManager.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Bows)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GoldBars)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SteelBars)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.runParamsBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // GlobalEventProvider
            // 
            this.GlobalEventProvider.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.GlobalEventProvider_KeyPress);
            // 
            // UpdateTimer
            // 
            this.UpdateTimer.Interval = 1000;
            this.UpdateTimer.Tick += new System.EventHandler(this.UpdateTimer_Tick);
            // 
            // BotManagerType
            // 
            this.BotManagerType.Controls.Add(this.SoloBot);
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
            this.SoloBot.Controls.Add(this.StatusMessage);
            this.SoloBot.Controls.Add(this.StartButton);
            this.SoloBot.Controls.Add(this.ClientLocation);
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
            // StatusMessage
            // 
            this.StatusMessage.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StatusMessage.Location = new System.Drawing.Point(6, 363);
            this.StatusMessage.Multiline = true;
            this.StatusMessage.Name = "StatusMessage";
            this.StatusMessage.Size = new System.Drawing.Size(455, 81);
            this.StatusMessage.TabIndex = 30;
            // 
            // StartButton
            // 
            this.StartButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartButton.Location = new System.Drawing.Point(173, 293);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(125, 55);
            this.StartButton.TabIndex = 8;
            this.StartButton.Text = "&Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // ClientLocation
            // 
            this.ClientLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClientLocation.Location = new System.Drawing.Point(103, 15);
            this.ClientLocation.Name = "ClientLocation";
            this.ClientLocation.Size = new System.Drawing.Size(320, 26);
            this.ClientLocation.TabIndex = 1;
            // 
            // OSBuddySelect
            // 
            this.OSBuddySelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OSBuddySelect.Image = ((System.Drawing.Image)(resources.GetObject("OSBuddySelect.Image")));
            this.OSBuddySelect.Location = new System.Drawing.Point(423, 14);
            this.OSBuddySelect.Name = "OSBuddySelect";
            this.OSBuddySelect.Size = new System.Drawing.Size(35, 27);
            this.OSBuddySelect.TabIndex = 2;
            this.OSBuddySelect.Text = ". . .";
            this.OSBuddySelect.UseVisualStyleBackColor = true;
            this.OSBuddySelect.Click += new System.EventHandler(this.OSBuddySelect_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(3, 208);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 18);
            this.label4.TabIndex = 22;
            this.label4.Text = "Run Until:";
            // 
            // BotActionSelect
            // 
            this.BotActionSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BotActionSelect.FormattingEnabled = true;
            this.BotActionSelect.Location = new System.Drawing.Point(103, 154);
            this.BotActionSelect.MaxDropDownItems = 12;
            this.BotActionSelect.Name = "BotActionSelect";
            this.BotActionSelect.Size = new System.Drawing.Size(355, 28);
            this.BotActionSelect.TabIndex = 5;
            // 
            // Iterations
            // 
            this.Iterations.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Iterations.Location = new System.Drawing.Point(103, 251);
            this.Iterations.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.Iterations.Name = "Iterations";
            this.Iterations.Size = new System.Drawing.Size(355, 26);
            this.Iterations.TabIndex = 7;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(3, 19);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 18);
            this.label6.TabIndex = 28;
            this.label6.Text = "OSBuddy:";
            // 
            // RunUntil
            // 
            this.RunUntil.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RunUntil.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.RunUntil.Location = new System.Drawing.Point(103, 203);
            this.RunUntil.Name = "RunUntil";
            this.RunUntil.Size = new System.Drawing.Size(355, 26);
            this.RunUntil.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 160);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 18);
            this.label1.TabIndex = 20;
            this.label1.Text = "Action:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, 253);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 18);
            this.label3.TabIndex = 24;
            this.label3.Text = "Iterations:";
            // 
            // Password
            // 
            this.Password.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Password.Location = new System.Drawing.Point(103, 104);
            this.Password.Name = "Password";
            this.Password.PasswordChar = '*';
            this.Password.Size = new System.Drawing.Size(355, 26);
            this.Password.TabIndex = 4;
            // 
            // Login
            // 
            this.Login.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Login.Location = new System.Drawing.Point(103, 59);
            this.Login.Name = "Login";
            this.Login.Size = new System.Drawing.Size(355, 26);
            this.Login.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 18);
            this.label2.TabIndex = 16;
            this.label2.Text = "Login:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(3, 110);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 18);
            this.label5.TabIndex = 18;
            this.label5.Text = "Password:";
            // 
            // PhasmatysManager
            // 
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
            // PhasmatysLogin
            // 
            this.PhasmatysLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PhasmatysLogin.Location = new System.Drawing.Point(95, 67);
            this.PhasmatysLogin.Name = "PhasmatysLogin";
            this.PhasmatysLogin.Size = new System.Drawing.Size(366, 26);
            this.PhasmatysLogin.TabIndex = 10;
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
            this.Bows.Location = new System.Drawing.Point(95, 255);
            this.Bows.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.Bows.Name = "Bows";
            this.Bows.Size = new System.Drawing.Size(366, 26);
            this.Bows.TabIndex = 14;
            // 
            // PhasmatysStartButton
            // 
            this.PhasmatysStartButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PhasmatysStartButton.Location = new System.Drawing.Point(167, 287);
            this.PhasmatysStartButton.Name = "PhasmatysStartButton";
            this.PhasmatysStartButton.Size = new System.Drawing.Size(125, 55);
            this.PhasmatysStartButton.TabIndex = 15;
            this.PhasmatysStartButton.Text = "&Start";
            this.PhasmatysStartButton.UseVisualStyleBackColor = true;
            this.PhasmatysStartButton.Click += new System.EventHandler(this.PhasmatysStartButton_Click);
            // 
            // GoldBars
            // 
            this.GoldBars.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GoldBars.Location = new System.Drawing.Point(95, 166);
            this.GoldBars.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.GoldBars.Name = "GoldBars";
            this.GoldBars.Size = new System.Drawing.Size(366, 26);
            this.GoldBars.TabIndex = 12;
            // 
            // SteelBars
            // 
            this.SteelBars.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SteelBars.Location = new System.Drawing.Point(95, 210);
            this.SteelBars.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.SteelBars.Name = "SteelBars";
            this.SteelBars.Size = new System.Drawing.Size(366, 26);
            this.SteelBars.TabIndex = 13;
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
            this.PhasmatysStatus.Size = new System.Drawing.Size(455, 81);
            this.PhasmatysStatus.TabIndex = 45;
            // 
            // PhasmatysBotSelector
            // 
            this.PhasmatysBotSelector.FormattingEnabled = true;
            this.PhasmatysBotSelector.Location = new System.Drawing.Point(95, 12);
            this.PhasmatysBotSelector.MaxDropDownItems = 3;
            this.PhasmatysBotSelector.Name = "PhasmatysBotSelector";
            this.PhasmatysBotSelector.Size = new System.Drawing.Size(366, 28);
            this.PhasmatysBotSelector.TabIndex = 9;
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
            this.PhasmatysPassword.Location = new System.Drawing.Point(95, 116);
            this.PhasmatysPassword.Name = "PhasmatysPassword";
            this.PhasmatysPassword.PasswordChar = '*';
            this.PhasmatysPassword.Size = new System.Drawing.Size(366, 26);
            this.PhasmatysPassword.TabIndex = 11;
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
        private System.Windows.Forms.TextBox ClientLocation;
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
    }
}

