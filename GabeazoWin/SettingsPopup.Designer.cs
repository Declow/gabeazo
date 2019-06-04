namespace GabeazoWin
{
    partial class SettingsPopup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsPopup));
            this.Keybound = new System.Windows.Forms.TextBox();
            this.Startup = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.AltBox = new System.Windows.Forms.CheckBox();
            this.ShiftBox = new System.Windows.Forms.CheckBox();
            this.CrtlBox = new System.Windows.Forms.CheckBox();
            this.Save = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Keybound
            // 
            this.Keybound.Location = new System.Drawing.Point(15, 94);
            this.Keybound.Name = "Keybound";
            this.Keybound.ReadOnly = true;
            this.Keybound.Size = new System.Drawing.Size(70, 20);
            this.Keybound.TabIndex = 55;
            // 
            // Startup
            // 
            this.Startup.AutoSize = true;
            this.Startup.Location = new System.Drawing.Point(136, 9);
            this.Startup.Name = "Startup";
            this.Startup.Size = new System.Drawing.Size(86, 17);
            this.Startup.TabIndex = 56;
            this.Startup.Text = "Run on Start";
            this.Startup.UseVisualStyleBackColor = true;
            this.Startup.CheckedChanged += new System.EventHandler(this.Startup_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 54;
            this.label1.Text = "Keybinds";
            // 
            // AltBox
            // 
            this.AltBox.AutoSize = true;
            this.AltBox.Location = new System.Drawing.Point(15, 71);
            this.AltBox.Name = "AltBox";
            this.AltBox.Size = new System.Drawing.Size(38, 17);
            this.AltBox.TabIndex = 53;
            this.AltBox.Text = "Alt";
            this.AltBox.UseVisualStyleBackColor = true;
            this.AltBox.CheckedChanged += new System.EventHandler(this.AltBox_CheckedChanged);
            // 
            // ShiftBox
            // 
            this.ShiftBox.AutoSize = true;
            this.ShiftBox.Location = new System.Drawing.Point(15, 48);
            this.ShiftBox.Name = "ShiftBox";
            this.ShiftBox.Size = new System.Drawing.Size(47, 17);
            this.ShiftBox.TabIndex = 52;
            this.ShiftBox.Text = "Shift";
            this.ShiftBox.UseVisualStyleBackColor = true;
            this.ShiftBox.CheckedChanged += new System.EventHandler(this.ShiftBox_CheckedChanged);
            // 
            // CrtlBox
            // 
            this.CrtlBox.AutoSize = true;
            this.CrtlBox.Location = new System.Drawing.Point(15, 25);
            this.CrtlBox.Name = "CrtlBox";
            this.CrtlBox.Size = new System.Drawing.Size(41, 17);
            this.CrtlBox.TabIndex = 51;
            this.CrtlBox.Text = "Ctrl";
            this.CrtlBox.UseVisualStyleBackColor = true;
            this.CrtlBox.CheckedChanged += new System.EventHandler(this.CrtlBox_CheckedChanged);
            // 
            // Save
            // 
            this.Save.Location = new System.Drawing.Point(147, 176);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(75, 23);
            this.Save.TabIndex = 57;
            this.Save.Text = "Save";
            this.Save.UseVisualStyleBackColor = true;
            this.Save.Visible = false;
            this.Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // SettingsPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(234, 211);
            this.Controls.Add(this.Keybound);
            this.Controls.Add(this.Startup);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.AltBox);
            this.Controls.Add(this.ShiftBox);
            this.Controls.Add(this.CrtlBox);
            this.Controls.Add(this.Save);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsPopup";
            this.Text = "SettingsPopup";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox Startup;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox AltBox;
        private System.Windows.Forms.CheckBox ShiftBox;
        private System.Windows.Forms.CheckBox CrtlBox;
        private System.Windows.Forms.Button Save;
        public System.Windows.Forms.TextBox Keybound;
    }
}