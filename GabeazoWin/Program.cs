using GabeazoWin.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace GabeazoWin
{
    class Program
    {

        //[System.Runtime.InteropServices.DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        //private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        //[System.Runtime.InteropServices.DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        //[return: MarshalAs(UnmanagedType.Bool)]
        //private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        //[System.Runtime.InteropServices.DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        //private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        //[System.Runtime.InteropServices.DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        //private static extern IntPtr GetModuleHandle(string lpModuleName);
        //private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);


        //private static IntPtr SetHook(LowLevelKeyboardProc proc)
        //{
        //    using (Process curProcess = Process.GetCurrentProcess())
        //    using (ProcessModule curModule = curProcess.MainModule)
        //    {

        //        return SetWindowsHookEx(WH_KEYBOARD_LL, proc,

        //            GetModuleHandle(curModule.ModuleName), 0);

        //    }
        //}


        //private static List<Keys> list;
        //private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        //{

        //    if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)

        //    {

        //        int vkCode = Marshal.ReadInt32(lParam);
        //        list.Add((Keys)vkCode);

        //        if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.LeftShift) && Keyboard.IsKeyDown(Key.A))
        //        {
        //            System.Windows.Application app = new System.Windows.Application();
        //            app.Run(new WPFProgram());
        //            app.Shutdown();
        //        }

        //        Console.WriteLine((Keys)vkCode);

        //    }

        //    return CallNextHookEx(_hookID, nCode, wParam, lParam);

        //}

        //private const int WH_KEYBOARD_LL = 13;
        //private const int WM_KEYDOWN = 0x0100;
        //private static LowLevelKeyboardProc _proc = HookCallback;
        //private static IntPtr _hookID = IntPtr.Zero;

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

            hook = new KeyboardHook();
            hook.KeyDown += new KeyboardHook.HookEventHandler(OnHookKeyDown);
        }

        private void OnHookKeyDown(object sender, HookEventArgs e)
        {
            if (!program.IsLoaded)
            {
                program = new WPFProgram();
            }

            program.Show();
            program.Activate();
            program.Topmost = true;
        }

        void Exit(object sender, EventArgs e)
        {
            // Hide tray icon, otherwise it will remain shown until user mouses over it
            trayIcon.Visible = false;
            Current.Shutdown();
        }
    }

    //public class AppContext : ApplicationContext
    //{
    //    private NotifyIcon trayIcon;
    //    HotKey _hotKey;

    //    public AppContext()
    //    {
    //        // Initialize Tray Icon
    //        trayIcon = new NotifyIcon()
    //        {
    //            Icon = Icon.FromHandle(Resources.AppIcon.GetHicon()),
    //            ContextMenu = new ContextMenu(new MenuItem[] {
    //            new MenuItem("Exit", Exit)
    //        }),
    //            Visible = true
    //        };

    //        Console.WriteLine("Test hotkey");

    //        _hotKey = new HotKey(Key.C, KeyModifier.Shift | KeyModifier.Win, OnHotKeyHandler);

    //    }

    //    private void OnHotKeyHandler(HotKey hotKey)
    //    {
    //        Console.WriteLine("Test hotkey");
    //    }

    //    void Exit(object sender, EventArgs e)
    //    {
    //        // Hide tray icon, otherwise it will remain shown until user mouses over it
    //        trayIcon.Visible = false;

    //        System.Windows.Forms.Application.Exit();
    //    }
    //}
}
