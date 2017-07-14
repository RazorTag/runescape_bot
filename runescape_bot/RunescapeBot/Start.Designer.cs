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
            this.tabControl1 = new System.Windows.Forms.TabControl();
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
            this.DoubleBot = new System.Windows.Forms.TabPage();
            this.DoubleStatusMessageRight = new System.Windows.Forms.TextBox();
            this.DoubleBotActionRight = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.DoublePasswordRight = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.DoubleLoginRight = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.DoubleStatusMessageLeft = new System.Windows.Forms.TextBox();
            this.DoubleStartButton = new System.Windows.Forms.Button();
            this.DoubleBotActionLeft = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.DoublePasswordLeft = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.DoubleLoginLeft = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.SoloBot.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Iterations)).BeginInit();
            this.DoubleBot.SuspendLayout();
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
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.SoloBot);
            this.tabControl1.Controls.Add(this.DoubleBot);
            this.tabControl1.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(6, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(842, 482);
            this.tabControl1.TabIndex = 16;
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
            this.SoloBot.Size = new System.Drawing.Size(834, 449);
            this.SoloBot.TabIndex = 0;
            this.SoloBot.Text = "Solo Bot";
            this.SoloBot.UseVisualStyleBackColor = true;
            // 
            // StatusMessage
            // 
            this.StatusMessage.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StatusMessage.Location = new System.Drawing.Point(487, 15);
            this.StatusMessage.Multiline = true;
            this.StatusMessage.Name = "StatusMessage";
            this.StatusMessage.Size = new System.Drawing.Size(341, 428);
            this.StatusMessage.TabIndex = 30;
            // 
            // StartButton
            // 
            this.StartButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartButton.Location = new System.Drawing.Point(173, 324);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(125, 55);
            this.StartButton.TabIndex = 26;
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
            this.ClientLocation.TabIndex = 27;
            // 
            // OSBuddySelect
            // 
            this.OSBuddySelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OSBuddySelect.Image = ((System.Drawing.Image)(resources.GetObject("OSBuddySelect.Image")));
            this.OSBuddySelect.Location = new System.Drawing.Point(423, 14);
            this.OSBuddySelect.Name = "OSBuddySelect";
            this.OSBuddySelect.Size = new System.Drawing.Size(35, 27);
            this.OSBuddySelect.TabIndex = 29;
            this.OSBuddySelect.Text = ". . .";
            this.OSBuddySelect.UseVisualStyleBackColor = true;
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
            this.BotActionSelect.TabIndex = 21;
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
            this.Iterations.TabIndex = 25;
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
            this.RunUntil.TabIndex = 23;
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
            this.Password.TabIndex = 19;
            // 
            // Login
            // 
            this.Login.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Login.Location = new System.Drawing.Point(103, 59);
            this.Login.Name = "Login";
            this.Login.Size = new System.Drawing.Size(355, 26);
            this.Login.TabIndex = 17;
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
            // DoubleBot
            // 
            this.DoubleBot.Controls.Add(this.DoubleStatusMessageRight);
            this.DoubleBot.Controls.Add(this.DoubleBotActionRight);
            this.DoubleBot.Controls.Add(this.label7);
            this.DoubleBot.Controls.Add(this.DoublePasswordRight);
            this.DoubleBot.Controls.Add(this.label10);
            this.DoubleBot.Controls.Add(this.DoubleLoginRight);
            this.DoubleBot.Controls.Add(this.label13);
            this.DoubleBot.Controls.Add(this.DoubleStatusMessageLeft);
            this.DoubleBot.Controls.Add(this.DoubleStartButton);
            this.DoubleBot.Controls.Add(this.DoubleBotActionLeft);
            this.DoubleBot.Controls.Add(this.label9);
            this.DoubleBot.Controls.Add(this.DoublePasswordLeft);
            this.DoubleBot.Controls.Add(this.label12);
            this.DoubleBot.Controls.Add(this.DoubleLoginLeft);
            this.DoubleBot.Controls.Add(this.label11);
            this.DoubleBot.Location = new System.Drawing.Point(4, 29);
            this.DoubleBot.Name = "DoubleBot";
            this.DoubleBot.Padding = new System.Windows.Forms.Padding(3);
            this.DoubleBot.Size = new System.Drawing.Size(834, 449);
            this.DoubleBot.TabIndex = 1;
            this.DoubleBot.Text = "Double Bot";
            this.DoubleBot.UseVisualStyleBackColor = true;
            // 
            // DoubleStatusMessageRight
            // 
            this.DoubleStatusMessageRight.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DoubleStatusMessageRight.Location = new System.Drawing.Point(436, 229);
            this.DoubleStatusMessageRight.Multiline = true;
            this.DoubleStatusMessageRight.Name = "DoubleStatusMessageRight";
            this.DoubleStatusMessageRight.Size = new System.Drawing.Size(392, 214);
            this.DoubleStatusMessageRight.TabIndex = 53;
            // 
            // DoubleBotActionRight
            // 
            this.DoubleBotActionRight.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DoubleBotActionRight.FormattingEnabled = true;
            this.DoubleBotActionRight.Location = new System.Drawing.Point(516, 103);
            this.DoubleBotActionRight.MaxDropDownItems = 12;
            this.DoubleBotActionRight.Name = "DoubleBotActionRight";
            this.DoubleBotActionRight.Size = new System.Drawing.Size(312, 28);
            this.DoubleBotActionRight.TabIndex = 51;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(433, 109);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 18);
            this.label7.TabIndex = 50;
            this.label7.Text = "Action:";
            // 
            // DoublePasswordRight
            // 
            this.DoublePasswordRight.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DoublePasswordRight.Location = new System.Drawing.Point(516, 59);
            this.DoublePasswordRight.Name = "DoublePasswordRight";
            this.DoublePasswordRight.PasswordChar = '*';
            this.DoublePasswordRight.Size = new System.Drawing.Size(312, 26);
            this.DoublePasswordRight.TabIndex = 49;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(433, 65);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(82, 18);
            this.label10.TabIndex = 48;
            this.label10.Text = "Password:";
            // 
            // DoubleLoginRight
            // 
            this.DoubleLoginRight.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DoubleLoginRight.Location = new System.Drawing.Point(516, 14);
            this.DoubleLoginRight.Name = "DoubleLoginRight";
            this.DoubleLoginRight.Size = new System.Drawing.Size(312, 26);
            this.DoubleLoginRight.TabIndex = 47;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(433, 20);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(51, 18);
            this.label13.TabIndex = 46;
            this.label13.Text = "Login:";
            // 
            // DoubleStatusMessageLeft
            // 
            this.DoubleStatusMessageLeft.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DoubleStatusMessageLeft.Location = new System.Drawing.Point(6, 229);
            this.DoubleStatusMessageLeft.Multiline = true;
            this.DoubleStatusMessageLeft.Name = "DoubleStatusMessageLeft";
            this.DoubleStatusMessageLeft.Size = new System.Drawing.Size(392, 214);
            this.DoubleStatusMessageLeft.TabIndex = 45;
            // 
            // DoubleStartButton
            // 
            this.DoubleStartButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DoubleStartButton.Location = new System.Drawing.Point(354, 151);
            this.DoubleStartButton.Name = "DoubleStartButton";
            this.DoubleStartButton.Size = new System.Drawing.Size(125, 55);
            this.DoubleStartButton.TabIndex = 41;
            this.DoubleStartButton.Text = "&Start";
            this.DoubleStartButton.UseVisualStyleBackColor = true;
            // 
            // DoubleBotActionLeft
            // 
            this.DoubleBotActionLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DoubleBotActionLeft.FormattingEnabled = true;
            this.DoubleBotActionLeft.Location = new System.Drawing.Point(85, 103);
            this.DoubleBotActionLeft.MaxDropDownItems = 12;
            this.DoubleBotActionLeft.Name = "DoubleBotActionLeft";
            this.DoubleBotActionLeft.Size = new System.Drawing.Size(313, 28);
            this.DoubleBotActionLeft.TabIndex = 36;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(2, 109);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(56, 18);
            this.label9.TabIndex = 35;
            this.label9.Text = "Action:";
            // 
            // DoublePasswordLeft
            // 
            this.DoublePasswordLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DoublePasswordLeft.Location = new System.Drawing.Point(85, 59);
            this.DoublePasswordLeft.Name = "DoublePasswordLeft";
            this.DoublePasswordLeft.PasswordChar = '*';
            this.DoublePasswordLeft.Size = new System.Drawing.Size(313, 26);
            this.DoublePasswordLeft.TabIndex = 34;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(2, 65);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(82, 18);
            this.label12.TabIndex = 33;
            this.label12.Text = "Password:";
            // 
            // DoubleLoginLeft
            // 
            this.DoubleLoginLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DoubleLoginLeft.Location = new System.Drawing.Point(85, 14);
            this.DoubleLoginLeft.Name = "DoubleLoginLeft";
            this.DoubleLoginLeft.Size = new System.Drawing.Size(313, 26);
            this.DoubleLoginLeft.TabIndex = 32;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(2, 20);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(51, 18);
            this.label11.TabIndex = 31;
            this.label11.Text = "Login:";
            // 
            // Start
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(850, 485);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "Start";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Roboport";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Start_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Start_FormClosed);
            this.tabControl1.ResumeLayout(false);
            this.SoloBot.ResumeLayout(false);
            this.SoloBot.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Iterations)).EndInit();
            this.DoubleBot.ResumeLayout(false);
            this.DoubleBot.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private GlobalEventProvider GlobalEventProvider;
        private System.Windows.Forms.OpenFileDialog FileSelect;
        private System.Windows.Forms.Timer UpdateTimer;
        private System.Windows.Forms.TabControl tabControl1;
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
        private System.Windows.Forms.TabPage DoubleBot;
        private System.Windows.Forms.TextBox DoubleStatusMessageRight;
        private System.Windows.Forms.ComboBox DoubleBotActionRight;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox DoublePasswordRight;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox DoubleLoginRight;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox DoubleStatusMessageLeft;
        private System.Windows.Forms.Button DoubleStartButton;
        private System.Windows.Forms.ComboBox DoubleBotActionLeft;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox DoublePasswordLeft;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox DoubleLoginLeft;
        private System.Windows.Forms.Label label11;
    }
}

