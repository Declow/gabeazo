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
            form = new FormProgram();

            hook = new KeyboardHook();
            hook.KeyDown += new KeyboardHook.HookEventHandler(OnHookKeyDown);
        }

        private void OnHookKeyDown(object sender, HookEventArgs e)
        {
            bool isTrigged = LoadSettings(e);
            if (isTrigged)
            {
                if (form.IsDisposed)
                {
                    form = new FormProgram();
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
            settingsForm.ShowDialog();
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
