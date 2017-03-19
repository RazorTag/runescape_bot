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
            this.GlobalEventProvider = new GlobalEventProvider();
            this.label5 = new System.Windows.Forms.Label();
            this.Password = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.Iterations)).BeginInit();
            this.SuspendLayout();
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(168, 257);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(100, 41);
            this.StartButton.TabIndex = 11;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // BotActionSelect
            // 
            this.BotActionSelect.FormattingEnabled = true;
            this.BotActionSelect.Location = new System.Drawing.Point(100, 110);
            this.BotActionSelect.MaxDropDownItems = 12;
            this.BotActionSelect.Name = "BotActionSelect";
            this.BotActionSelect.Size = new System.Drawing.Size(310, 21);
            this.BotActionSelect.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(23, 113);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 16);
            this.label1.TabIndex = 5;
            this.label1.Text = "Action:";
            // 
            // Login
            // 
            this.Login.Location = new System.Drawing.Point(100, 20);
            this.Login.Name = "Login";
            this.Login.Size = new System.Drawing.Size(310, 20);
            this.Login.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(23, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Login:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(23, 207);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 16);
            this.label3.TabIndex = 9;
            this.label3.Text = "Iterations:";
            // 
            // Iterations
            // 
            this.Iterations.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Iterations.Location = new System.Drawing.Point(100, 205);
            this.Iterations.Name = "Iterations";
            this.Iterations.Size = new System.Drawing.Size(89, 22);
            this.Iterations.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(23, 164);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 16);
            this.label4.TabIndex = 7;
            this.label4.Text = "Run Until:";
            // 
            // RunUntil
            // 
            this.RunUntil.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RunUntil.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.RunUntil.Location = new System.Drawing.Point(100, 159);
            this.RunUntil.Name = "RunUntil";
            this.RunUntil.Size = new System.Drawing.Size(310, 22);
            this.RunUntil.TabIndex = 8;
            this.RunUntil.ValueChanged += new System.EventHandler(this.RunUntil_ValueChanged);
            // 
            // GlobalEventProvider
            // 
            this.GlobalEventProvider.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.GlobalEventProvider_KeyPress);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(23, 66);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 16);
            this.label5.TabIndex = 3;
            this.label5.Text = "Password:";
            // 
            // Password
            // 
            this.Password.Location = new System.Drawing.Point(100, 65);
            this.Password.Name = "Password";
            this.Password.Size = new System.Drawing.Size(310, 20);
            this.Password.TabIndex = 4;
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
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "Start";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Runescape Bot";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Start_FormClosing);
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

