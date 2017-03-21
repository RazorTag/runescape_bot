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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Start));
            this.StartButton = new System.Windows.Forms.Button();
            this.BotActionSelect = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Login = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Iterations = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.RunUntil = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.Password = new System.Windows.Forms.TextBox();
            this.GlobalEventProvider = new RunescapeBot.GMA.GlobalEventProvider();
            ((System.ComponentModel.ISupportInitialize)(this.Iterations)).BeginInit();
            this.SuspendLayout();
            // 
            // StartButton
            // 
            this.StartButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartButton.Location = new System.Drawing.Point(151, 257);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(125, 55);
            this.StartButton.TabIndex = 11;
            this.StartButton.Text = "&Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // BotActionSelect
            // 
            this.BotActionSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BotActionSelect.FormattingEnabled = true;
            this.BotActionSelect.Location = new System.Drawing.Point(123, 110);
            this.BotActionSelect.MaxDropDownItems = 12;
            this.BotActionSelect.Name = "BotActionSelect";
            this.BotActionSelect.Size = new System.Drawing.Size(287, 28);
            this.BotActionSelect.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(23, 116);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 18);
            this.label1.TabIndex = 5;
            this.label1.Text = "Action:";
            // 
            // Login
            // 
            this.Login.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Login.Location = new System.Drawing.Point(123, 20);
            this.Login.Name = "Login";
            this.Login.Size = new System.Drawing.Size(287, 26);
            this.Login.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(23, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 18);
            this.label2.TabIndex = 1;
            this.label2.Text = "Login:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(23, 209);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 18);
            this.label3.TabIndex = 9;
            this.label3.Text = "Iterations:";
            // 
            // Iterations
            // 
            this.Iterations.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Iterations.Location = new System.Drawing.Point(123, 207);
            this.Iterations.Name = "Iterations";
            this.Iterations.Size = new System.Drawing.Size(89, 26);
            this.Iterations.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(23, 164);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 18);
            this.label4.TabIndex = 7;
            this.label4.Text = "Run Until:";
            // 
            // RunUntil
            // 
            this.RunUntil.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RunUntil.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.RunUntil.Location = new System.Drawing.Point(123, 159);
            this.RunUntil.Name = "RunUntil";
            this.RunUntil.Size = new System.Drawing.Size(287, 26);
            this.RunUntil.TabIndex = 8;
            this.RunUntil.ValueChanged += new System.EventHandler(this.RunUntil_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(23, 71);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 18);
            this.label5.TabIndex = 3;
            this.label5.Text = "Password:";
            // 
            // Password
            // 
            this.Password.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Password.Location = new System.Drawing.Point(123, 65);
            this.Password.Name = "Password";
            this.Password.PasswordChar = '*';
            this.Password.Size = new System.Drawing.Size(287, 26);
            this.Password.TabIndex = 4;
            // 
            // GlobalEventProvider
            // 
            this.GlobalEventProvider.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.GlobalEventProvider_KeyPress);
            // 
            // Start
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(437, 324);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Password);
            this.Controls.Add(this.RunUntil);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Iterations);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Login);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BotActionSelect);
            this.Controls.Add(this.StartButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "Start";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Roboport";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Start_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Start_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.Iterations)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.ComboBox BotActionSelect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Login;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown Iterations;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker RunUntil;
        private GlobalEventProvider GlobalEventProvider;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox Password;
    }
}

