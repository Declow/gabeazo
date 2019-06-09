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
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace GabeazoWin
{
    class FormProgram : Form
    {

        private Point _startLocation;
        private Point _endLocation;
        private Form _rubberband;
        private bool _firstDraw = true;
        private readonly string _filename = "imagedata.png";
        private WebClient _client;
        private Recorder _recorder;
        private bool _oneScreen;

        public FormProgram(bool oneScreen = false)
        {
            var screenLeft = SystemInformation.VirtualScreen.Left;
            var screenTop = SystemInformation.VirtualScreen.Top;

            StartPosition = FormStartPosition.Manual;

            this.Location = new Point(screenLeft, screenTop);

            this.FormBorderStyle = FormBorderStyle.None;
            this.ShowInTaskbar = false;

            this.BackColor = Color.Red;
            this.TransparencyKey = Color.Red;

            var screenWidth = SystemInformation.VirtualScreen.Width;
            var screenHeight = SystemInformation.VirtualScreen.Height;

            this.Size = new Size(screenWidth, screenHeight);

            this.MouseDown += FormProgram_MouseDown;
            this.MouseMove += FormProgram_MouseMove;
            this.MouseUp += FormProgram_MouseUp;

            Cursor = Cursors.Cross;

            this.KeyDown += FormProgram_KeyDown;

            _recorder = new Recorder(_filename);
            _oneScreen = oneScreen;

            SetupRubberband();
            this.TopMost = true;

            _startLocation.X = -1;

            _client = new WebClient();
            _client.Encoding = Encoding.UTF8;
            _client.UploadFileCompleted += Client_UploadFileCompleted;

        }

        private void SetupRubberband()
        {
            _rubberband = new Form();
            _rubberband.WindowState = FormWindowState.Normal;
            _rubberband.FormBorderStyle = FormBorderStyle.None;
            _rubberband.StartPosition = FormStartPosition.Manual;
            _rubberband.ShowInTaskbar = false;
            _rubberband.Opacity = 0.40;
            _rubberband.BackColor = Color.FromArgb(66, 158, 244);
        }
        private void FormProgram_MouseMove(object sender, MouseEventArgs e)
        {
            _endLocation = e.Location;
            if (_startLocation.X != -1)
            {
                if (_firstDraw)
                    _rubberband.Show();

                var size = new Size();

                if (_startLocation.X > _endLocation.X && _startLocation.Y > _endLocation.Y)
                {
                    size.Width = _startLocation.X - _endLocation.X;
                    size.Height = _startLocation.Y - _endLocation.Y;

                    MoveWindow(_endLocation, size);
                }
                else if (_startLocation.X < _endLocation.X && _startLocation.Y > _endLocation.Y)
                {
                    size.Width = _endLocation.X - _startLocation.X;
                    size.Height = _startLocation.Y - _endLocation.Y;

                    MoveWindow(new Point(_startLocation.X, _endLocation.Y), size);
                }
                else if (_startLocation.X > _endLocation.X && _startLocation.Y < _endLocation.Y)
                {
                    size.Width = _startLocation.X - _endLocation.X;
                    size.Height = _endLocation.Y - _startLocation.Y;

                    MoveWindow(new Point(_endLocation.X, _startLocation.Y), size);
                }
                else if (_startLocation.X < _endLocation.X && _startLocation.Y < _endLocation.Y)
                {
                    size.Width = _endLocation.X - _startLocation.X;
                    size.Height = _endLocation.Y - _startLocation.Y;

                    MoveWindow(_startLocation, size);
                }
                
                this.TopMost = true;
                _rubberband.TopMost = true;
            }
        }

        private void MoveWindow(Point location, Size size)
        {
            MoveWindow(_rubberband.Handle, location.X + SystemInformation.VirtualScreen.Left, location.Y + SystemInformation.VirtualScreen.Top, size.Width, size.Height, true);
        }

        private void FormProgram_MouseUp(object sender, MouseEventArgs e)
        {
            _endLocation = e.Location;
            var size = GetSize();

            var region = new Rectangle(_startLocation, size);
            _rubberband.Close();
            Close();
            if (size.Width == 0 || size.Height == 0)
                return;
            _recorder.CaptureRegion(region);
            UploadImage();
        }

        private void FormProgram_MouseDown(object sender, MouseEventArgs e)
        {

            switch (e.Button)
            {
                case MouseButtons.Left:
                    if (_oneScreen)
                    {
                        _recorder.CaptureScreenFromPoint(e.Location);
                        _rubberband.Close();
                        Close();
                        UploadImage();
                    }

                    _startLocation = e.Location;
                    break;
                case MouseButtons.Right:
                    _rubberband.Close();
                    Close();
                    break;
            }
        }

        private void FormProgram_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                _rubberband.Close();
                Close();
            }
        }

        public Size GetSize()
        {
            var size = new Size();
            if (_startLocation.X > _endLocation.X && _startLocation.Y > _endLocation.Y)
            {
                size.Width = _startLocation.X - _endLocation.X;
                size.Height = _startLocation.Y - _endLocation.Y;

                var temp = _startLocation;
                _startLocation = _endLocation;
                _endLocation = temp;
            }
            else if (_startLocation.X < _endLocation.X && _startLocation.Y > _endLocation.Y)
            {
                size.Width = _endLocation.X - _startLocation.X;
                size.Height = _startLocation.Y - _endLocation.Y;

                var temp = _startLocation;
                _startLocation.Y = _endLocation.Y;
                _endLocation.Y = temp.Y;
            }
            else if (_startLocation.X > _endLocation.X && _startLocation.Y < _endLocation.Y)
            {
                size.Width = _startLocation.X - _endLocation.X;
                size.Height = _endLocation.Y - _startLocation.Y;

                var temp = _startLocation;
                _startLocation.X = _endLocation.X;
                _endLocation.X = temp.X;
            }
            else if (_startLocation.X < _endLocation.X && _startLocation.Y < _endLocation.Y)
            {
                size.Width = _endLocation.X - _startLocation.X;
                size.Height = _endLocation.Y - _startLocation.Y;
            }
            return size;
        }


        private void UploadImage()
        {
            const string url = "https://gabeazo.com/gabeazo.php";

            var server = new Uri(url);
            _client.UploadFileAsync(server, _filename);
        }

        private void Client_UploadFileCompleted(object sender, UploadFileCompletedEventArgs e)
        {
            if (e.Error != null)
                throw e.Error;

            string response = Encoding.UTF8.GetString(e.Result);
            System.Windows.Clipboard.SetText(response);
            System.Diagnostics.Process.Start(response);
            File.Delete(_filename);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProgram));
            this.SuspendLayout();
            // 
            // FormProgram
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormProgram";
            this.ResumeLayout(false);

        }

        [DllImport("User32.dll")]
        static extern bool MoveWindow(IntPtr handle, int x, int y, int width, int height, bool redraw);
    }
}
