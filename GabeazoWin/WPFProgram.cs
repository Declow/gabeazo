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
        

        private CancellationTokenSource cancellationTokenSource;

        public WPFProgram()
        {
            int screenLeft = SystemInformation.VirtualScreen.Left;
            int screenTop = SystemInformation.VirtualScreen.Top;

            this.Left = screenLeft;
            this.Top = screenTop;

            client = new WebClient();
            client.Encoding = Encoding.UTF8;
            client.UploadFileCompleted += Client_UploadFileCompleted;

            var bc = new BrushConverter();
            //this.Background = (System.Windows.Media.Brush)bc.ConvertFrom("#0100ffff");
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
            var s = getScalingFactor();
            Console.WriteLine(s);
            this.Height = SystemInformation.VirtualScreen.Height*2;
            this.Width = SystemInformation.VirtualScreen.Width*2;

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

        private void WPFProgram_LefMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            cancellationTokenSource = new CancellationTokenSource();
            var pointToWindow = Mouse.GetPosition(this);

            startLocation = new System.Windows.Point(pointToWindow.X, pointToWindow.Y);

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
            Console.WriteLine(size);
            SetProcessDPIAware();
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
            var scale = getScalingFactor();
            var rect = new Rectangle(Convert.ToInt32(location.X*scale), Convert.ToInt32(location.Y*scale), Convert.ToInt32(size.Width), Convert.ToInt32(size.Height));
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

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool SetProcessDPIAware();

        [DllImport("gdi32.dll")]
        static extern int GetDeviceCaps(IntPtr hdc, int nIndex);
        public enum DeviceCap
        {
            VERTRES = 10,
            DESKTOPVERTRES = 117,

            // http://pinvoke.net/default.aspx/gdi32/GetDeviceCaps.html
        }

        private float getScalingFactor()
        {
            Graphics g = Graphics.FromHwnd(IntPtr.Zero);
            IntPtr desktop = g.GetHdc();
            int LogicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.VERTRES);
            int PhysicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.DESKTOPVERTRES);

            float ScreenScalingFactor = (float)PhysicalScreenHeight / (float)LogicalScreenHeight;

            return ScreenScalingFactor; // 1.25 = 125%
        }

    }
}
