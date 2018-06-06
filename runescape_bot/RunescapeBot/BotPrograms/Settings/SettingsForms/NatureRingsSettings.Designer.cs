namespace RunescapeBot.BotPrograms.Settings
{
    partial class NatureRingsSettings
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
            this.SaveSettings = new System.Windows.Forms.Button();
            this.FairyRingSelect = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.GloryTypeSelect = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // SaveSettings
            // 
            this.SaveSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SaveSettings.Location = new System.Drawing.Point(128, 113);
            this.SaveSettings.Name = "SaveSettings";
            this.SaveSettings.Size = new System.Drawing.Size(125, 55);
            this.SaveSettings.TabIndex = 11;
            this.SaveSettings.Text = "&Save";
            this.SaveSettings.UseVisualStyleBackColor = true;
            this.SaveSettings.Click += new System.EventHandler(this.SaveSettings_Click);
            // 
            // FairyRingSelect
            // 
            this.FairyRingSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FairyRingSelect.FormattingEnabled = true;
            this.FairyRingSelect.Items.AddRange(new object[] {
            "Edgeville"});
            this.FairyRingSelect.Location = new System.Drawing.Point(97, 12);
            this.FairyRingSelect.MaxDropDownItems = 12;
            this.FairyRingSelect.Name = "FairyRingSelect";
            this.FairyRingSelect.Size = new System.Drawing.Size(270, 28);
            this.FairyRingSelect.TabIndex = 21;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(10, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 18);
            this.label1.TabIndex = 22;
            this.label1.Text = "Fairy ring:";
            // 
            // GloryTypeSelect
            // 
            this.GloryTypeSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GloryTypeSelect.FormattingEnabled = true;
            this.GloryTypeSelect.Items.AddRange(new object[] {
            "Amulet of Eternal Glory"});
            this.GloryTypeSelect.Location = new System.Drawing.Point(97, 56);
            this.GloryTypeSelect.MaxDropDownItems = 12;
            this.GloryTypeSelect.Name = "GloryTypeSelect";
            this.GloryTypeSelect.Size = new System.Drawing.Size(270, 28);
            this.GloryTypeSelect.TabIndex = 23;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(10, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 18);
            this.label2.TabIndex = 24;
            this.label2.Text = "Glory type:";
            // 
            // NatureRingsSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(379, 180);
            this.Controls.Add(this.GloryTypeSelect);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.FairyRingSelect);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SaveSettings);
            this.Name = "NatureRingsSettings";
            this.Text = "NatureRings Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SaveSettings;
        private System.Windows.Forms.ComboBox FairyRingSelect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox GloryTypeSelect;
        private System.Windows.Forms.Label label2;
    }
}