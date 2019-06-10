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
            this.CaptureRegion = new System.Windows.Forms.GroupBox();
            this.CaptureScreen = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.KeyboundScreen = new System.Windows.Forms.TextBox();
            this.CrtlBoxScreen = new System.Windows.Forms.CheckBox();
            this.ShiftBoxScreen = new System.Windows.Forms.CheckBox();
            this.AltBoxScreen = new System.Windows.Forms.CheckBox();
            this.CaptureRegion.SuspendLayout();
            this.CaptureScreen.SuspendLayout();
            this.SuspendLayout();
            // 
            // Keybound
            // 
            this.Keybound.Location = new System.Drawing.Point(9, 101);
            this.Keybound.Name = "Keybound";
            this.Keybound.ReadOnly = true;
            this.Keybound.Size = new System.Drawing.Size(70, 20);
            this.Keybound.TabIndex = 55;
            // 
            // Startup
            // 
            this.Startup.AutoSize = true;
            this.Startup.Location = new System.Drawing.Point(238, 12);
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
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 54;
            this.label1.Text = "Keybinds";
            // 
            // AltBox
            // 
            this.AltBox.AutoSize = true;
            this.AltBox.Location = new System.Drawing.Point(9, 78);
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
            this.ShiftBox.Location = new System.Drawing.Point(9, 55);
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
            this.CrtlBox.Location = new System.Drawing.Point(9, 32);
            this.CrtlBox.Name = "CrtlBox";
            this.CrtlBox.Size = new System.Drawing.Size(41, 17);
            this.CrtlBox.TabIndex = 51;
            this.CrtlBox.Text = "Ctrl";
            this.CrtlBox.UseVisualStyleBackColor = true;
            this.CrtlBox.CheckedChanged += new System.EventHandler(this.CrtlBox_CheckedChanged);
            // 
            // Save
            // 
            this.Save.Location = new System.Drawing.Point(249, 298);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(75, 23);
            this.Save.TabIndex = 57;
            this.Save.Text = "Save";
            this.Save.UseVisualStyleBackColor = true;
            this.Save.Visible = false;
            this.Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // CaptureRegion
            // 
            this.CaptureRegion.Controls.Add(this.label1);
            this.CaptureRegion.Controls.Add(this.Keybound);
            this.CaptureRegion.Controls.Add(this.CrtlBox);
            this.CaptureRegion.Controls.Add(this.ShiftBox);
            this.CaptureRegion.Controls.Add(this.AltBox);
            this.CaptureRegion.Location = new System.Drawing.Point(12, 12);
            this.CaptureRegion.Name = "CaptureRegion";
            this.CaptureRegion.Size = new System.Drawing.Size(136, 130);
            this.CaptureRegion.TabIndex = 58;
            this.CaptureRegion.TabStop = false;
            this.CaptureRegion.Text = "Region capture";
            // 
            // CaptureScreen
            // 
            this.CaptureScreen.Controls.Add(this.label2);
            this.CaptureScreen.Controls.Add(this.KeyboundScreen);
            this.CaptureScreen.Controls.Add(this.CrtlBoxScreen);
            this.CaptureScreen.Controls.Add(this.ShiftBoxScreen);
            this.CaptureScreen.Controls.Add(this.AltBoxScreen);
            this.CaptureScreen.Location = new System.Drawing.Point(12, 148);
            this.CaptureScreen.Name = "CaptureScreen";
            this.CaptureScreen.Size = new System.Drawing.Size(136, 130);
            this.CaptureScreen.TabIndex = 59;
            this.CaptureScreen.TabStop = false;
            this.CaptureScreen.Text = "Region screen";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 54;
            this.label2.Text = "Keybinds";
            // 
            // KeyboundScreen
            // 
            this.KeyboundScreen.Location = new System.Drawing.Point(9, 101);
            this.KeyboundScreen.Name = "KeyboundScreen";
            this.KeyboundScreen.ReadOnly = true;
            this.KeyboundScreen.Size = new System.Drawing.Size(70, 20);
            this.KeyboundScreen.TabIndex = 55;
            // 
            // CrtlBoxScreen
            // 
            this.CrtlBoxScreen.AutoSize = true;
            this.CrtlBoxScreen.Location = new System.Drawing.Point(9, 32);
            this.CrtlBoxScreen.Name = "CrtlBoxScreen";
            this.CrtlBoxScreen.Size = new System.Drawing.Size(41, 17);
            this.CrtlBoxScreen.TabIndex = 51;
            this.CrtlBoxScreen.Text = "Ctrl";
            this.CrtlBoxScreen.UseVisualStyleBackColor = true;
            this.CrtlBoxScreen.CheckedChanged += new System.EventHandler(this.CrtlBoxScreen_CheckedChanged);
            // 
            // ShiftBoxScreen
            // 
            this.ShiftBoxScreen.AutoSize = true;
            this.ShiftBoxScreen.Location = new System.Drawing.Point(9, 55);
            this.ShiftBoxScreen.Name = "ShiftBoxScreen";
            this.ShiftBoxScreen.Size = new System.Drawing.Size(47, 17);
            this.ShiftBoxScreen.TabIndex = 52;
            this.ShiftBoxScreen.Text = "Shift";
            this.ShiftBoxScreen.UseVisualStyleBackColor = true;
            this.ShiftBoxScreen.CheckedChanged += new System.EventHandler(this.ShiftBoxScreen_CheckedChanged);
            // 
            // AltBoxScreen
            // 
            this.AltBoxScreen.AutoSize = true;
            this.AltBoxScreen.Location = new System.Drawing.Point(9, 78);
            this.AltBoxScreen.Name = "AltBoxScreen";
            this.AltBoxScreen.Size = new System.Drawing.Size(38, 17);
            this.AltBoxScreen.TabIndex = 53;
            this.AltBoxScreen.Text = "Alt";
            this.AltBoxScreen.UseVisualStyleBackColor = true;
            this.AltBoxScreen.CheckedChanged += new System.EventHandler(this.AltBoxScreen_CheckedChanged);
            // 
            // SettingsPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(336, 333);
            this.Controls.Add(this.CaptureScreen);
            this.Controls.Add(this.CaptureRegion);
            this.Controls.Add(this.Startup);
            this.Controls.Add(this.Save);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsPopup";
            this.Text = "SettingsPopup";
            this.CaptureRegion.ResumeLayout(false);
            this.CaptureRegion.PerformLayout();
            this.CaptureScreen.ResumeLayout(false);
            this.CaptureScreen.PerformLayout();
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
        private System.Windows.Forms.GroupBox CaptureRegion;
        private System.Windows.Forms.GroupBox CaptureScreen;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.TextBox KeyboundScreen;
        private System.Windows.Forms.CheckBox CrtlBoxScreen;
        private System.Windows.Forms.CheckBox ShiftBoxScreen;
        private System.Windows.Forms.CheckBox AltBoxScreen;
    }
}