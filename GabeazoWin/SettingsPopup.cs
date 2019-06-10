//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.

//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
//    GNU General Public License for more details.

//    You should have received a copy of the GNU General Public License
//    along with this program.If not, see<https://www.gnu.org/licenses/>.

using System;
using System.Windows.Forms;

namespace GabeazoWin
{
    public partial class SettingsPopup : Form
    {
        public SettingsPopup()
        {
            InitializeComponent();
            //Region
            this.CrtlBox.Checked = Settings.Default.Crtl;
            this.ShiftBox.Checked = Settings.Default.Shift;
            this.AltBox.Checked = Settings.Default.Alt;
            this.Keybound.Text = Settings.Default.Key;
            this.Startup.Checked = Settings.Default.RunStartup;

            //Screen
            this.CrtlBoxScreen.Checked = Settings.Default.CrtlScreen;
            this.ShiftBoxScreen.Checked = Settings.Default.ShiftScreen;
            this.AltBoxScreen.Checked = Settings.Default.AltScreen;
            this.KeyboundScreen.Text = Settings.Default.KeyScreen;
        }

        private void Save_Click(object sender, EventArgs e)
        {

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
            bool toggleStartUp = this.Startup.Checked;
            Settings.Default.RunStartup = toggleStartUp;
            Settings.Default.Save();
            Startup startup = new Startup();

            if (toggleStartUp)
            {
                startup.SetStartup();
            }
            else
            {
                startup.RemoveStartup();
            }
        }

        private void CrtlBoxScreen_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.CrtlScreen = CrtlBoxScreen.Checked;
            Settings.Default.Save();
        }

        private void ShiftBoxScreen_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.ShiftScreen = ShiftBoxScreen.Checked;
            Settings.Default.Save();
        }

        private void AltBoxScreen_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.AltScreen = AltBoxScreen.Checked;
            Settings.Default.Save();
        }

    }
}