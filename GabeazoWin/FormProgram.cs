﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GabeazoWin
{
    class FormProgram : Form
    {

        private Point startLocation;
        private Point endLocation;
        private Form rubberband;
        private bool FirstDraw = true;

        public FormProgram()
        {
            var screenLeft = SystemInformation.VirtualScreen.Left;
            var screenTop = SystemInformation.VirtualScreen.Top;

            StartPosition = FormStartPosition.Manual;

            this.Location = new Point(screenLeft, screenTop);

            this.FormBorderStyle = FormBorderStyle.None;
            this.Opacity = 0.25;

            //this.TransparencyKey = Color.White;
            this.BackColor = Color.White;

            var screenWidth = SystemInformation.VirtualScreen.Width;
            var screenHeight = SystemInformation.VirtualScreen.Height;

            this.Size = new Size(screenWidth, screenHeight);


            this.MouseDown += FormProgram_MouseDown;
            this.MouseMove += FormProgram_MouseMove;
            this.MouseUp += FormProgram_MouseUp;

            Cursor = Cursors.Cross;
            
            SetupRubberband();
            this.TopMost = true;

            startLocation.X = -1;
        }

        private void SetupRubberband()
        {
            rubberband = new Form();
            rubberband.WindowState = FormWindowState.Normal;
            rubberband.FormBorderStyle = FormBorderStyle.None;
            rubberband.StartPosition = FormStartPosition.Manual;
            rubberband.Opacity = 0.25;
        }
        private void FormProgram_MouseMove(object sender, MouseEventArgs e)
        {
            endLocation = e.Location;
            if (startLocation.X != -1)
            {
                if (FirstDraw)
                    rubberband.Show();

                var size = new Size();

                if (startLocation.X > endLocation.X && startLocation.Y > endLocation.Y)
                {
                    size.Width = startLocation.X - endLocation.X;
                    size.Height = startLocation.Y - endLocation.Y;

                }
                else if (startLocation.X < endLocation.X && startLocation.Y > endLocation.Y)
                {
                    size.Width = endLocation.X - startLocation.X;
                    size.Height = startLocation.Y - endLocation.Y;
                }
                else if (startLocation.X > endLocation.X && startLocation.Y < endLocation.Y)
                {
                    size.Width = startLocation.X - endLocation.X;
                    size.Height = endLocation.Y - startLocation.Y;

                }
                else if (startLocation.X < endLocation.X && startLocation.Y < endLocation.Y)
                {
                    size.Width = endLocation.X - startLocation.X;
                    size.Height = endLocation.Y - startLocation.Y;
                }

                Console.WriteLine(startLocation);
                Console.WriteLine(size);
                
                MoveWindow(rubberband.Handle, startLocation.X + SystemInformation.VirtualScreen.Left, startLocation.Y + SystemInformation.VirtualScreen.Top, size.Width, size.Height, true);

                this.TopMost = true;
                rubberband.TopMost = true;


            }
        }

        private void FormProgram_MouseUp(object sender, MouseEventArgs e)
        {
            endLocation = e.Location;
            var size = GetSize();
            var region = new Rectangle(startLocation, size);
            rubberband.Close();
            this.Close();

            CaptureDesktop(region);
        }

        private void FormProgram_MouseDown(object sender, MouseEventArgs e)
        {

            switch (e.Button)
            {
                case MouseButtons.Left:
                    startLocation = e.Location;
                    break;
                case MouseButtons.Right:
                    rubberband.Close();
                    Close();
                    break;
            }
        }


        public Size GetSize()
        {
            var size = new Size();
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

        public void CaptureDesktop(Rectangle region)
        {
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

            CaptureRegion(desktop, region);
        }

        public void CaptureRegion(Rectangle region, Rectangle region2)
        {
            IntPtr desktophWnd;
            IntPtr desktopDc;
            IntPtr memoryDc;
            IntPtr bitmap;
            IntPtr oldBitmap;
            bool success;
            Bitmap result;
            Bitmap result2;

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

                result = Image.FromHbitmap(bitmap);
                result2 = result.Clone(region2, result.PixelFormat);
                Console.WriteLine(region2);
                result.Dispose();
            }
            finally
            {
                SelectObject(memoryDc, oldBitmap);
                DeleteObject(bitmap);
                DeleteDC(memoryDc);
                ReleaseDC(desktophWnd, desktopDc);
            }

            result2.Save("NewMethod.png", ImageFormat.Png);
            result2.Dispose();
        }

        [DllImport("User32.dll")]
        static extern bool MoveWindow(IntPtr handle, int x, int y, int width, int height, bool redraw);

        [DllImport("User32.dll")]
        public static extern IntPtr GetDC(IntPtr hwnd);

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