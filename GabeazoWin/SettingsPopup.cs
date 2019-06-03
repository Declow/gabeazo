using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GabeazoWin
{
    public partial class SettingsPopup : Form
    {
        public SettingsPopup()
        {
            InitializeComponent();
            this.CrtlBox.Checked = Settings.Default.Crtl;
            this.ShiftBox.Checked = Settings.Default.Shift;
            this.AltBox.Checked = Settings.Default.Alt;
            this.Keybound.Text = Settings.Default.Key;
            this.Startup.Checked = Settings.Default.RunStartup;
        }

        private void Save_Click(object sender, EventArgs e)
        {

        }

        private void Keybound_TextChanged(object sender, EventArgs e)
        {
            this.Keybound.Text = Settings.Default.Key;
        }

        private void AltBox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.Alt = this.AltBox.Checked;
            Settings.Default.Save();
        }

        private void ShiftBox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.Shift = this.ShiftBox.Checked;
            Settings.Default.Save();
        }

        private void CrtlBox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.Crtl = this.CrtlBox.Checked;
            Settings.Default.Save();
        }

        private void Startup_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.RunStartup = this.Startup.Checked;
            Settings.Default.Save();
        }
    }
}