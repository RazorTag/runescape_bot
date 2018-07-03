namespace RunescapeBot.BotPrograms.Settings.SettingsForms
{
    partial class Use14On14Settings
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
            this.label4 = new System.Windows.Forms.Label();
            this.SaveSettings = new System.Windows.Forms.Button();
            this.SingleItemMakeTime = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.SingleItemMakeTime)).BeginInit();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(21, 15);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(200, 18);
            this.label4.TabIndex = 31;
            this.label4.Text = "Single item make time [ms]:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // SaveSettings
            // 
            this.SaveSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SaveSettings.Location = new System.Drawing.Point(24, 105);
            this.SaveSettings.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SaveSettings.Name = "SaveSettings";
            this.SaveSettings.Size = new System.Drawing.Size(188, 85);
            this.SaveSettings.TabIndex = 29;
            this.SaveSettings.Text = "&Save";
            this.SaveSettings.UseVisualStyleBackColor = true;
            this.SaveSettings.Click += new System.EventHandler(this.SaveSettings_Click);
            // 
            // SingleItemMakeTime
            // 
            this.SingleItemMakeTime.Location = new System.Drawing.Point(56, 36);
            this.SingleItemMakeTime.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.SingleItemMakeTime.Name = "SingleItemMakeTime";
            this.SingleItemMakeTime.Size = new System.Drawing.Size(120, 26);
            this.SingleItemMakeTime.TabIndex = 33;
            // 
            // Use14On14Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(236, 212);
            this.Controls.Add(this.SingleItemMakeTime);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.SaveSettings);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Use14On14Settings";
            this.Text = "Half and Half Settings";
            ((System.ComponentModel.ISupportInitialize)(this.SingleItemMakeTime)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button SaveSettings;
        private System.Windows.Forms.NumericUpDown SingleItemMakeTime;
    }
}