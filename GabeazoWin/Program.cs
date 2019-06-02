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
            // AppContext context = new AppContext();
            App app = new App(Resources.gabeazoL);
            app.Run();
        }
    }

    public class App : System.Windows.Application
    {
        private NotifyIcon trayIcon;
        public Bitmap icon;
        private KeyboardHook hook;
        WPFProgram program;
        private FormProgram form;

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
                new MenuItem("Exit", Exit)
            }),
                Visible = true
            };

            ShutdownMode = ShutdownMode.OnExplicitShutdown;
            program = new WPFProgram();
            form = new FormProgram();

            hook = new KeyboardHook();
            hook.KeyDown += new KeyboardHook.HookEventHandler(OnHookKeyDown);
        }

        private void OnHookKeyDown(object sender, HookEventArgs e)
        {
            if (e.Key == Keys.C && e.Control && e.Shift)
            {
                if (!program.IsLoaded)
                {
                    program = new WPFProgram();
                }

                program.Show();
                program.Activate();
                program.Topmost = true;
            }

            if (e.Key == Keys.X && e.Control && e.Shift)
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
    }

}
