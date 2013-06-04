using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Media;

namespace OpenJinglePlayer
{
    public partial class VideoWindow : Form
    {
        private bool _DoClose;
        private bool _Fullscreen;
        private FormBorderStyle _BrdStyle;
        private Rectangle _Bounds;

        public VideoWindow()
        {
            InitializeComponent();
            _DoClose = false;
            _Fullscreen = false;
        }

        public void DoClose()
        {
            _DoClose = true;
            this.Close();
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = !_DoClose;
            base.OnClosing(e);
            Program.Status.VideoScreenVisible = false;
            Hide();
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            _ToggleFullScreen();
            base.OnMouseDoubleClick(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            int h = ClientSize.Height;
            int w = ClientSize.Width;
            int y = 0;
            int x = 0;
            elementHost1.SetBounds(x, y, w, h);
        }

        public void SetSource(System.Windows.Media.Brush Brush)
        {
            vcVideo.SetSource(Brush);            
        }

        public bool IsFullScreen()
        {
            return _Fullscreen;
        }

        public void EnterFullScreen()
        {
            if (!_Fullscreen)
                _Maximize(this);
        }

        public void LeaveFullScreen()
        {
            if (_Fullscreen)
                _Restore(this);
        }

        #region FullScreenStuff
        private void _ToggleFullScreen()
        {
            if (!_Fullscreen)
                _Maximize(this);
            else
                _Restore(this);
        }

        private void _Maximize(Form targetForm)
        {
            if (!_Fullscreen)
            {
                _Save(targetForm);

                int ScreenNr = 0;
                for (int i = 0; i < Screen.AllScreens.Length; i++)
                {
                    Screen scr = Screen.AllScreens[i];
                    int midx = targetForm.Left + targetForm.Width / 2;
                    int midy = targetForm.Top + targetForm.Height / 2;

                    if (scr.Bounds.Top <= midy && scr.Bounds.Left <= midx && scr.Bounds.Bottom >= midy && scr.Bounds.Right >= midx)
                        ScreenNr = i;
                }

                targetForm.SetDesktopLocation(Screen.AllScreens[ScreenNr].Bounds.Left, Screen.AllScreens[ScreenNr].Bounds.Top);
                targetForm.ClientSize = new Size(Screen.AllScreens[ScreenNr].Bounds.Width, Screen.AllScreens[ScreenNr].Bounds.Height);

                targetForm.FormBorderStyle = FormBorderStyle.None;
                _Fullscreen = true;
            }
        }

        private void _Save(Form targetForm)
        {
            _BrdStyle = targetForm.FormBorderStyle;
            _Bounds = targetForm.Bounds;
        }

        private void _Restore(Form targetForm)
        {
            targetForm.FormBorderStyle = _BrdStyle;
            targetForm.Bounds = _Bounds;
            _Fullscreen = false;
        }
        #endregion FullScreenStuff
    }
}
