namespace RunescapeBot.BotPrograms.Settings.SettingsForms
{
    partial class LesserDemonSettings
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
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SaveSettings = new System.Windows.Forms.Button();
            this.TelegrabSelection = new System.Windows.Forms.CheckBox();
            this.AlchSelection = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(82, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 18);
            this.label2.TabIndex = 33;
            this.label2.Text = "Alch:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(82, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 18);
            this.label1.TabIndex = 31;
            this.label1.Text = "Telegrab:";
            // 
            // SaveSettings
            // 
            this.SaveSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SaveSettings.Location = new System.Drawing.Point(85, 124);
            this.SaveSettings.Name = "SaveSettings";
            this.SaveSettings.Size = new System.Drawing.Size(125, 55);
            this.SaveSettings.TabIndex = 29;
            this.SaveSettings.Text = "&Save";
            this.SaveSettings.UseVisualStyleBackColor = true;
            this.SaveSettings.Click += new System.EventHandler(this.SaveSettings_Click);
            // 
            // TelegrabSelection
            // 
            this.TelegrabSelection.AutoSize = true;
            this.TelegrabSelection.Location = new System.Drawing.Point(201, 18);
            this.TelegrabSelection.Name = "TelegrabSelection";
            this.TelegrabSelection.Size = new System.Drawing.Size(15, 14);
            this.TelegrabSelection.TabIndex = 34;
            this.TelegrabSelection.UseVisualStyleBackColor = true;
            // 
            // AlchSelection
            // 
            this.AlchSelection.AutoSize = true;
            this.AlchSelection.Location = new System.Drawing.Point(201, 63);
            this.AlchSelection.Name = "AlchSelection";
            this.AlchSelection.Size = new System.Drawing.Size(15, 14);
            this.AlchSelection.TabIndex = 35;
            this.AlchSelection.UseVisualStyleBackColor = true;
            // 
            // LesserDemonSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(305, 200);
            this.Controls.Add(this.AlchSelection);
            this.Controls.Add(this.TelegrabSelection);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SaveSettings);
            this.Name = "LesserDemonSettings";
            this.Text = "Lesser Demon Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button SaveSettings;
        private System.Windows.Forms.CheckBox TelegrabSelection;
        private System.Windows.Forms.CheckBox AlchSelection;
    }
}