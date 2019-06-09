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

using GabeazoWin.Properties;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;

namespace GabeazoWin
{
    class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            App app = new App(Resources.gabeazoL);
            app.Run();
        }
    }

    public class App : System.Windows.Application
    {
        private NotifyIcon trayIcon;
        public Bitmap icon;
        private KeyboardHook hook;
        private FormProgram form;
        SettingsPopup settingsForm = new SettingsPopup();

        public App(Bitmap icon)
        {
            this.icon = icon;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            // Initialize Tray Icon
            trayIcon = new NotifyIcon()
            {       
                Icon = Icon.FromHandle(icon.GetHicon()),
                ContextMenu = new ContextMenu(new MenuItem[] {
                new MenuItem("Settings", Setting),
                new MenuItem("Exit", Exit)
            }),
                Visible = true
            };

            ShutdownMode = ShutdownMode.OnExplicitShutdown;
            form = new FormProgram(true);

            hook = new KeyboardHook();
            hook.KeyDown += new KeyboardHook.HookEventHandler(OnHookKeyDown);

            if (Settings.Default.Key == "")
            {
                settingsForm.ShowDialog();
            }
        }

        private void OnHookKeyDown(object sender, HookEventArgs e)
        {
            bool isTrigged = LoadSettings(e);
            if (isTrigged)
            {
                if (form.IsDisposed)
                {
                    form = new FormProgram(true);
                }

                form.Show();
                form.Activate();
                form.TopMost = true;
            }

        }

        void Exit(object sender, EventArgs e)
        {
            // Hide tray icon, otherwise it will remain shown until user mouses over it
            trayIcon.Visible = false;
            Current.Shutdown();
        }

        void Setting(object sender, EventArgs e)
        {
            try
            {
                settingsForm.ShowDialog();
            }
            catch (InvalidOperationException)
            {
                settingsForm.BringToFront();
            }
        }

        bool LoadSettings(HookEventArgs e)
        {
            bool keyComboTriggered = false;

            string globKey = e.Key.ToString();
            if (settingsForm.Keybound.Focused)
            {
                settingsForm.Keybound.Text = globKey;
                Settings.Default.Key = globKey;
                Settings.Default.Save();
            }

            bool CrtlBox = Settings.Default.Crtl;
            bool ShiftBox = Settings.Default.Shift;
            bool AltBox = Settings.Default.Alt;
            string Keybound = Settings.Default.Key;

            if (e.Key.ToString().ToUpper() == Keybound.ToUpper() && e.Control == CrtlBox && e.Shift == ShiftBox && e.Alt == AltBox)
            {
                keyComboTriggered = true;
            }

            return keyComboTriggered;
        }
    }

}
