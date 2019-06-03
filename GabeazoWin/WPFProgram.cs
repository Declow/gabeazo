using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;

namespace GabeazoWin
{
    class WPFProgram : Window
    {
        private System.Windows.Point startLocation;
        private System.Windows.Point endLocation;
        private readonly string filename = "imagedata.png";
        private Canvas canvas;
        private System.Windows.Shapes.Rectangle myRect = new System.Windows.Shapes.Rectangle();
        private WebClient client;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetProcessDPIAware();

        private CancellationTokenSource cancellationTokenSource;

        public WPFProgram()
        {
            //SetProcessDPIAware();

            int screenLeft = SystemInformation.VirtualScreen.Left;
            int screenTop = SystemInformation.VirtualScreen.Top;

            this.Left = screenLeft;
            this.Top = screenTop;

            client = new WebClient();
            client.Encoding = Encoding.UTF8;
            client.UploadFileCompleted += Client_UploadFileCompleted;

            var bc = new BrushConverter();
            this.Background = (System.Windows.Media.Brush)bc.ConvertFrom("#0100ffff");
            this.AllowsTransparency = true;
            canvas = new Canvas();
            canvas.Opacity = 0.25;
            this.Content = canvas;

            this.ShowInTaskbar = false;
            this.WindowStyle = WindowStyle.None;
            this.Topmost = true;
       
            myRect.Stroke = System.Windows.Media.Brushes.Black;
            myRect.Fill = System.Windows.Media.Brushes.SkyBlue;
            myRect.VerticalAlignment = VerticalAlignment.Center;
            canvas.Children.Add(myRect);

            Rectangle desktop;
            Screen[] screens;

            desktop = Rectangle.Empty;
            screens = Screen.AllScreens;

            for (int i = 0; i < screens.Length; i++)
            {
                Screen screen;

                screen = screens[i];

                desktop = Rectangle.Union(desktop, screen.Bounds);
            }

            this.Height = desktop.Height;
            this.Width = desktop.Width;

            //this.WindowState = WindowState.Maximized;

            this.MouseLeftButtonDown += WPFProgram_LefMouseDown;
            this.MouseLeftButtonUp += WPFProgram_LeftMouseUp;
            this.MouseMove += WPFProgram_MouseMove;
            this.MouseRightButtonDown += WPFProgram_RightMouseDown;
                        

            Console.WriteLine(Height + " " + Width);

            Mouse.OverrideCursor = System.Windows.Input.Cursors.Cross;
        }

        private void WPFProgram_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            endLocation = Mouse.GetPosition(this);

            if (startLocation.X != 0)
            {
                var size = new System.Windows.Size();

                if (startLocation.X > endLocation.X && startLocation.Y > endLocation.Y)
                {
                    size.Width = startLocation.X - endLocation.X;
                    size.Height = startLocation.Y - endLocation.Y;
                    myRect.Height = size.Height;
                    myRect.Width = size.Width;
                    
                    Canvas.SetTop(myRect, endLocation.Y);
                    Canvas.SetLeft(myRect, endLocation.X);
                }
                else if (startLocation.X < endLocation.X && startLocation.Y > endLocation.Y)
                {
                    size.Width = endLocation.X - startLocation.X;
                    size.Height = startLocation.Y - endLocation.Y;
                    myRect.Height = size.Height;
                    myRect.Width = size.Width;

                    Canvas.SetTop(myRect, endLocation.Y);
                    Canvas.SetLeft(myRect, startLocation.X);
                }
                else if (startLocation.X > endLocation.X && startLocation.Y < endLocation.Y)
                {
                    size.Width = startLocation.X - endLocation.X;
                    size.Height = endLocation.Y - startLocation.Y;
                    myRect.Height = size.Height;
                    myRect.Width = size.Width;

                    Canvas.SetTop(myRect, startLocation.Y);
                    Canvas.SetLeft(myRect, endLocation.X);
                }
                else if (startLocation.X < endLocation.X && startLocation.Y < endLocation.Y)
                {
                    size.Width = endLocation.X - startLocation.X;
                    size.Height = endLocation.Y - startLocation.Y;
                    myRect.Height = size.Height;
                    myRect.Width = size.Width;

                    Canvas.SetTop(myRect, startLocation.Y);
                    Canvas.SetLeft(myRect, startLocation.X);
                }
            }
        }

        private void WPFProgram_LeftMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (cancellationTokenSource.Token.IsCancellationRequested)
            {
                return;
            }

            endLocation = Mouse.GetPosition(this);

            myRect.Visibility = Visibility.Hidden;

            Action emptyDelegate = delegate { };
            canvas.Dispatcher.Invoke(emptyDelegate, DispatcherPriority.Render);
            var size = GetSize();

            if(size.Height == 0 || size.Width == 0)
            {
                return;
            }

            SaveImage(startLocation, size);
            UploadImage();
        }

        public struct POINT
        {
            public int X;
            public int Y;
        }

        [DllImport("user32.dll")]
        static extern bool GetPhysicalCursorPos(out POINT lpPoint);
        [DllImport("User32.dll")]
        static extern IntPtr GetDC(IntPtr hwnd);
        [DllImport("gdi32.dll")]
        static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        private System.Windows.Point ConvertPixelsToUnits(double x, double y)
        {
            // get the system DPI
            IntPtr dDC = GetDC(IntPtr.Zero); // Get desktop DC
            int dpi = GetDeviceCaps(dDC, 88);
            bool rv = ReleaseDC(IntPtr.Zero, dDC);

            // WPF's physical unit size is calculated by taking the 
            // "Device-Independant Unit Size" (always 1/96)
            // and scaling it by the system DPI
            double physicalUnitSize = (1d / 96d) * (double)dpi;
            System.Windows.Point wpfUnits = new System.Windows.Point(physicalUnitSize * (double)x,
                physicalUnitSize * (double)y);

            return wpfUnits;
        }


        private void WPFProgram_LefMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            cancellationTokenSource = new CancellationTokenSource();

            startLocation = Mouse.GetPosition(this);

            POINT p;
            if (GetPhysicalCursorPos(out p))
            {
                Console.WriteLine(p.X + " " + p.Y);
            }
            else
            {
                Console.WriteLine("Unable to get pos of mouse");
            }
            System.Windows.Point point = canvas.PointFromScreen(startLocation);
            Console.WriteLine(point);

            System.Windows.Point point1 = ConvertPixelsToUnits(point.X, point.Y);

            Console.WriteLine(point1);


            myRect.Width = 0;
            myRect.Height = 0;
            myRect.Visibility = Visibility.Visible;
        }

        private void WPFProgram_RightMouseDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        public System.Windows.Size GetSize()
        {
            var size = new System.Windows.Size();
            if (startLocation.X > endLocation.X && startLocation.Y > endLocation.Y)
            {
                size.Width = startLocation.X - endLocation.X;
                size.Height = startLocation.Y - endLocation.Y;

                var temp = startLocation;
                startLocation = endLocation;
                endLocation = temp;
            }
            else if (startLocation.X < endLocation.X && startLocation.Y > endLocation.Y)
            {
                size.Width = endLocation.X - startLocation.X;
                size.Height = startLocation.Y - endLocation.Y;

                var temp = startLocation;
                startLocation.Y = endLocation.Y;
                endLocation.Y = temp.Y;
            }
            else if (startLocation.X > endLocation.X && startLocation.Y < endLocation.Y)
            {
                size.Width = startLocation.X - endLocation.X;
                size.Height = endLocation.Y - startLocation.Y;

                var temp = startLocation;
                startLocation.X = endLocation.X;
                endLocation.X = temp.X;
            }
            else if (startLocation.X < endLocation.X && startLocation.Y < endLocation.Y)
            {
                size.Width = endLocation.X - startLocation.X;
                size.Height = endLocation.Y - startLocation.Y;
            }
            return size;
        }

        private void SaveImage(System.Windows.Point location, System.Windows.Size size)
        {
            Rectangle rectangle = new Rectangle
            {
                X = Convert.ToInt32(location.X),
                Y = Convert.ToInt32(location.Y),
                Width = Convert.ToInt32(size.Width),
                Height = Convert.ToInt32(size.Height)
            };
            CaptureDesktop(false, rectangle);
            // create the bitmap to copy the screen shot to
            Bitmap bitmap = new Bitmap(Convert.ToInt32(this.Width), Convert.ToInt32(this.Height));
            // now copy the screen image to the graphics device from the bitmap
            using (Graphics gr = Graphics.FromImage(bitmap))
            {
                gr.CopyFromScreen(new System.Drawing.Point(Convert.ToInt32(System.Windows.Application.Current.MainWindow.Left)
                    ,Convert.ToInt32(System.Windows.Application.Current.MainWindow.Top))
                    , new System.Drawing.Point(0, 0), new System.Drawing.Size(Convert.ToInt32(this.Width), Convert.ToInt32(this.Height)));
            }
            bitmap.Save("test.png", ImageFormat.Png);

            var rect = new Rectangle(Convert.ToInt32(location.X), Convert.ToInt32(location.Y), Convert.ToInt32(size.Width), Convert.ToInt32(size.Height));
            var cropped = bitmap.Clone(rect, bitmap.PixelFormat);
            cropped.Save(filename, ImageFormat.Png);
            bitmap.Dispose();
            cropped.Dispose();
        }

        private void UploadImage()
        {
            const string url = "https://gabeazo.com/gabeazo.php";

            var server = new Uri(url);
            client.UploadFileAsync(server, filename);
        }

        private void Client_UploadFileCompleted(object sender, UploadFileCompletedEventArgs e)
        {
            string response = Encoding.UTF8.GetString(e.Result);
            System.Windows.Clipboard.SetText(response);
            System.Diagnostics.Process.Start(response);
            File.Delete(filename);

            Close();
        }


        public void CaptureRegion(Rectangle region)
        {
            IntPtr desktophWnd;
            IntPtr desktopDc;
            IntPtr memoryDc;
            IntPtr bitmap;
            IntPtr oldBitmap;
            bool success;
            Bitmap result;

            desktophWnd = GetDesktopWindow();
            desktopDc = GetWindowDC(desktophWnd);
            memoryDc = CreateCompatibleDC(desktopDc);
            bitmap = CreateCompatibleBitmap(desktopDc, region.Width, region.Height);
            oldBitmap = SelectObject(memoryDc, bitmap);

            success = BitBlt(memoryDc, 0, 0, region.Width, region.Height, desktopDc, region.Left, region.Top, SRCCOPY | CAPTUREBLT);

            try
            {
                if (!success)
                {
                    throw new System.ComponentModel.Win32Exception();
                }

                result = System.Drawing.Image.FromHbitmap(bitmap);
                //result = result.Clone(rect, result.PixelFormat);
            }
            finally
            {
                SelectObject(memoryDc, oldBitmap);
                DeleteObject(bitmap);
                DeleteDC(memoryDc);
                ReleaseDC(desktophWnd, desktopDc);
            }

            result.Save(filename+"NewMethod.png", ImageFormat.Png);
        }

        public void CaptureDesktop(bool workingAreaOnly, Rectangle rect)
        {
            Rectangle desktop;
            Screen[] screens;

            desktop = Rectangle.Empty;
            screens = Screen.AllScreens;

            for (int i = 0; i < screens.Length; i++)
            {
                Screen screen;

                screen = screens[i];

                desktop = Rectangle.Union(desktop, workingAreaOnly ? screen.WorkingArea : screen.Bounds);
            }

            CaptureRegion(desktop);
        }


        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            var source = PresentationSource.FromVisual(this) as HwndSource;
            source?.AddHook(WndProc);
        }

        private const int WM_DPICHANGED = 0x02E0;

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_DPICHANGED)
            {
                handled = true;
            }
            return IntPtr.Zero;
        }


        [DllImport("gdi32.dll")]
        static extern bool BitBlt(IntPtr hdcDest, int nxDest, int nyDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);

        [DllImport("gdi32.dll")]
        static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int width, int nHeight);

        [DllImport("gdi32.dll")]
        static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("gdi32.dll")]
        static extern IntPtr DeleteDC(IntPtr hdc);

        [DllImport("gdi32.dll")]
        static extern IntPtr DeleteObject(IntPtr hObject);

        [DllImport("user32.dll")]
        static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        static extern IntPtr GetWindowDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDc);

        [DllImport("gdi32.dll")]
        static extern IntPtr SelectObject(IntPtr hdc, IntPtr hObject);

        const int SRCCOPY = 0x00CC0020;

        const int CAPTUREBLT = 0x40000000;

    }
}
