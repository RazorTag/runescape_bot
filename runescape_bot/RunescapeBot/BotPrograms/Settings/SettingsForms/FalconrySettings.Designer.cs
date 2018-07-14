namespace RunescapeBot.BotPrograms.Settings.SettingsForms
{
    partial class FalconrySettings
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
            this.CatchDarkKebbits = new System.Windows.Forms.CheckBox();
            this.CatchSpottedKebbits = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SaveSettings = new System.Windows.Forms.Button();
            this.CatchDashingKebbits = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // CatchDarkKebbits
            // 
            this.CatchDarkKebbits.AutoSize = true;
            this.CatchDarkKebbits.Location = new System.Drawing.Point(212, 66);
            this.CatchDarkKebbits.Name = "CatchDarkKebbits";
            this.CatchDarkKebbits.Size = new System.Drawing.Size(15, 14);
            this.CatchDarkKebbits.TabIndex = 40;
            this.CatchDarkKebbits.UseVisualStyleBackColor = true;
            // 
            // CatchSpottedKebbits
            // 
            this.CatchSpottedKebbits.AutoSize = true;
            this.CatchSpottedKebbits.Location = new System.Drawing.Point(212, 21);
            this.CatchSpottedKebbits.Name = "CatchSpottedKebbits";
            this.CatchSpottedKebbits.Size = new System.Drawing.Size(15, 14);
            this.CatchSpottedKebbits.TabIndex = 39;
            this.CatchSpottedKebbits.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(93, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 18);
            this.label2.TabIndex = 38;
            this.label2.Text = "Dark:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(93, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 18);
            this.label1.TabIndex = 37;
            this.label1.Text = "Spotted:";
            // 
            // SaveSettings
            // 
            this.SaveSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SaveSettings.Location = new System.Drawing.Point(96, 165);
            this.SaveSettings.Name = "SaveSettings";
            this.SaveSettings.Size = new System.Drawing.Size(125, 55);
            this.SaveSettings.TabIndex = 36;
            this.SaveSettings.Text = "&Save";
            this.SaveSettings.UseVisualStyleBackColor = true;
            this.SaveSettings.Click += new System.EventHandler(this.SaveSettings_Click);
            // 
            // CatchDashingKebbits
            // 
            this.CatchDashingKebbits.AutoSize = true;
            this.CatchDashingKebbits.Location = new System.Drawing.Point(212, 111);
            this.CatchDashingKebbits.Name = "CatchDashingKebbits";
            this.CatchDashingKebbits.Size = new System.Drawing.Size(15, 14);
            this.CatchDashingKebbits.TabIndex = 42;
            this.CatchDashingKebbits.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(93, 107);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 18);
            this.label3.TabIndex = 41;
            this.label3.Text = "Dashing:";
            // 
            // FalconrySettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(321, 246);
            this.Controls.Add(this.CatchDashingKebbits);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.CatchDarkKebbits);
            this.Controls.Add(this.CatchSpottedKebbits);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SaveSettings);
            this.Name = "FalconrySettings";
            this.Text = "Falconry Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox CatchDarkKebbits;
        private System.Windows.Forms.CheckBox CatchSpottedKebbits;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button SaveSettings;
        private System.Windows.Forms.CheckBox CatchDashingKebbits;
        private System.Windows.Forms.Label label3;
    }
}