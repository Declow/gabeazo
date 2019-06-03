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
            string text = this.Keybound.Text;

            if (text.Length < 1)
            {
                text = text.Substring(1);
            }
            this.Keybound.Text = text;
            Settings.Default.Key = text;
        }

        private void AltBox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.Alt = this.AltBox.Checked;
        }

        private void ShiftBox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.Shift = this.ShiftBox.Checked;
        }

        private void CrtlBox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.Crtl = this.CrtlBox.Checked;
        }

        private void Startup_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.RunStartup = this.Startup.Checked;
        }
    }
}