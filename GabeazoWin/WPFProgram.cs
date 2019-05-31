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

        public WPFProgram()
        {
            int screenLeft = SystemInformation.VirtualScreen.Left;
            int screenTop = SystemInformation.VirtualScreen.Top;
            int screenWidth = SystemInformation.VirtualScreen.Width;
            int screenHeight = SystemInformation.VirtualScreen.Height;

            this.Height = screenHeight+10;
            this.Width = screenWidth+20;
            this.Left = screenLeft-10;
            this.Top = screenTop;

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

            this.MouseDown += WPFProgram_MouseDown;
            this.MouseUp += WPFProgram_MouseUp;
            this.MouseMove += WPFProgram_MouseMove;

            Mouse.OverrideCursor = System.Windows.Input.Cursors.Cross;
            

            myRect.Stroke = System.Windows.Media.Brushes.Black;
            myRect.Fill = System.Windows.Media.Brushes.SkyBlue;
            myRect.VerticalAlignment = VerticalAlignment.Center;
            canvas.Children.Add(myRect);

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

        private void WPFProgram_MouseUp(object sender, MouseButtonEventArgs e)
        {
            endLocation = Mouse.GetPosition(this);
            this.Content = null;

            canvas.Children.Remove(myRect);
            canvas.Opacity = 0;
            canvas.InvalidateVisual();
            canvas.UpdateLayout();

            Action emptyDelegate = delegate { };
            canvas.Dispatcher.Invoke(emptyDelegate, DispatcherPriority.Render);
            var size = GetSize();
            SaveImage(startLocation, size);
            UploadImage();
            File.Delete(filename);
            System.Windows.Application.Current.Shutdown();
        }

        private void WPFProgram_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var pointToWindow = Mouse.GetPosition(this);
            startLocation = new System.Windows.Point(pointToWindow.X, pointToWindow.Y);
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

            using (WebClient client = new WebClient())
            {
                var server = new Uri(url);
                client.Encoding = Encoding.UTF8;
                
                var res = client.UploadFile(server, filename);

                string response = Encoding.UTF8.GetString(res);
                System.Windows.Clipboard.SetText(response);
                System.Diagnostics.Process.Start(response);
            }
        }
    }
}
