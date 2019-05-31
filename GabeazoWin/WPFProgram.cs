using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
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
        private bool isAttached;
        private WebClient client;
        private KeyboardHook hook;


        private CancellationTokenSource cancellationTokenSource;

        public WPFProgram()
        {
            int screenLeft = SystemInformation.VirtualScreen.Left;
            int screenTop = SystemInformation.VirtualScreen.Top;
            //int screenWidth = SystemInformation.VirtualScreen.Width;
            //int screenHeight = SystemInformation.VirtualScreen.Height;

            this.Left = screenLeft-10;
            this.Top = screenTop;

            client = new WebClient();
            client.Encoding = Encoding.UTF8;
            client.UploadFileCompleted += Client_UploadFileCompleted;

            //this.Opacity = 0.01;
            var bc = new BrushConverter();
            this.Background = (System.Windows.Media.Brush)bc.ConvertFrom("#0100ffff");
            this.AllowsTransparency = true;
            canvas = new Canvas();
            canvas.Opacity = 0.25;
            this.Content = canvas;

            this.ShowInTaskbar = false;
            this.WindowStyle = WindowStyle.None;
            this.Topmost = true;
            //this.WindowState = WindowState.Maximized;

            //this.MouseLeftButtonDown+= WPFProgram_LefMouseDown;
            //this.MouseLeftButtonUp += WPFProgram_LeftMouseUp;
            //this.MouseMove += WPFProgram_MouseMove;
            //this.MouseRightButtonDown += WPFProgram_RightMouseDown;

            //Mouse.OverrideCursor = System.Windows.Input.Cursors.Cross;
       
            myRect.Stroke = System.Windows.Media.Brushes.Black;
            myRect.Fill = System.Windows.Media.Brushes.SkyBlue;
            myRect.VerticalAlignment = VerticalAlignment.Center;

            hook = new KeyboardHook();
            hook.KeyDown += new KeyboardHook.HookEventHandler(OnHookKeyDown);
        }

        private void OnHookKeyDown(object sender, HookEventArgs e)
        {     
            if (e.Key == Keys.X)
            {
                if (isAttached)
                {
                    return;
                }

                MouseEventsAttach();
            }
        }

        private void WPFProgram_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.X)
            {
                if (isAttached)
                {
                    return;
                }

                MouseEventsAttach();
            }
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


        private void MouseEventsAttach()
        {
            this.Height = SystemInformation.VirtualScreen.Height + 10;
            this.Width = SystemInformation.VirtualScreen.Width + 20;

            this.MouseLeftButtonDown += WPFProgram_LefMouseDown;
            this.MouseLeftButtonUp += WPFProgram_LeftMouseUp;
            this.MouseMove += WPFProgram_MouseMove;
            this.MouseRightButtonDown += WPFProgram_RightMouseDown;

            Mouse.OverrideCursor = System.Windows.Input.Cursors.Cross;

            isAttached = true;
        }

        private void MouseEventsDetatch()
        {
            this.Height = 0;
            this.Height = 0;

            this.MouseLeftButtonDown -= WPFProgram_LefMouseDown;
            this.MouseLeftButtonUp -= WPFProgram_LeftMouseUp;
            this.MouseMove -= WPFProgram_MouseMove;
            this.MouseRightButtonDown -= WPFProgram_RightMouseDown;

            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;

            isAttached = false;
        }

        private void WPFProgram_LeftMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (cancellationTokenSource.Token.IsCancellationRequested)
            {
                return;
            }

            endLocation = Mouse.GetPosition(this);

            canvas.Children.Remove(myRect);
            //canvas.Opacity = 0;
            //canvas.InvalidateVisual();
            //canvas.UpdateLayout();

            Action emptyDelegate = delegate { };
            canvas.Dispatcher.Invoke(emptyDelegate, DispatcherPriority.Render);
            var size = GetSize();
            SaveImage(startLocation, size);
            UploadImage();
            // System.Windows.Application.Current.Shutdown();
            MouseEventsDetatch();
        }

        private void WPFProgram_LefMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            cancellationTokenSource = new CancellationTokenSource();
            var pointToWindow = Mouse.GetPosition(this);
            canvas.Children.Add(myRect);
            startLocation = new System.Windows.Point(pointToWindow.X, pointToWindow.Y);
        }

        private void WPFProgram_RightMouseDown(object sender, MouseButtonEventArgs e)
        {
            cancellationTokenSource?.Cancel();
            canvas.Children.Remove(myRect);
            MouseEventsDetatch();
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
            // create the bitmap to copy the screen shot to
            Bitmap bitmap = new Bitmap(Convert.ToInt32(this.Width), Convert.ToInt32(this.Height));
            // now copy the screen image to the graphics device from the bitmap
            using (Graphics gr = Graphics.FromImage(bitmap))
            {
                gr.CopyFromScreen(new System.Drawing.Point(Convert.ToInt32(System.Windows.Application.Current.MainWindow.Left)
                    ,Convert.ToInt32(System.Windows.Application.Current.MainWindow.Top))
                    , new System.Drawing.Point(0, 0), new System.Drawing.Size(Convert.ToInt32(this.Width), Convert.ToInt32(this.Height)));
                Console.WriteLine("test");
            }

            var rect = new Rectangle(Convert.ToInt32(location.X), Convert.ToInt32(location.Y), Convert.ToInt32(size.Width), Convert.ToInt32(size.Height));
            var cropped = bitmap.Clone(rect, bitmap.PixelFormat);
            cropped.Save(filename, ImageFormat.Png);
        }

        private void UploadImage()
        {
            const string url = "http://gabeazo.com/gabeazo.php";

                var server = new Uri(url);
                client.UploadFileAsync(server, filename);
        }

        private void Client_UploadFileCompleted(object sender, UploadFileCompletedEventArgs e)
        {
            string response = Encoding.UTF8.GetString(e.Result);
            System.Windows.Clipboard.SetText(response);
            System.Diagnostics.Process.Start(response);
            File.Delete(filename);
        }
    }
}
