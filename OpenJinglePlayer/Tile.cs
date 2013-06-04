using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading;

namespace OpenJinglePlayer
{
    enum FileType
    {
        Empty,
        Unsupported,
        Audio,
        Video,
        Image
    }

    enum State
    {
        Empty,
        FileMissing,
        NotPlayed,
        Playing,
        Paused,
        Finished,
    }

    class Tile
    {
        public FileType Type;
        public State Status;
        public String FilePath;
        public String Name;
        public int Nr;
        public float Length;
        public float Position;
        public bool Loop;
        public System.Windows.Media.Imaging.BitmapImage Image;
        private bool _Opened;
        
        int x = 0, y = 0, w = 0, h = 0;

        private System.Windows.Media.MediaPlayer _MediaPlayer;
        private System.Windows.Media.VideoDrawing _VideoDrawing;

        public Size Size
        {
            get
            {
                Size res = new Size(10, 10);
                if (Type == FileType.Image)
                {
                    res.Height = Image.PixelHeight;
                    res.Width = Image.PixelWidth;
                }

                if (Type == FileType.Video && _Opened)
                {
                    res.Height = _MediaPlayer.NaturalVideoHeight;
                    res.Width = _MediaPlayer.NaturalVideoWidth;
                }

                return res;
            }
        }
        
        public Tile(int Nr)
        {
            Status = State.Empty;
            Type = FileType.Empty;
            FilePath = String.Empty;
            Length = 0f;
            Name = String.Empty;
            Position = 0f;
            this.Nr = Nr;
            Loop = false;
            _Opened = false;
            Image = null;

            _MediaPlayer = new System.Windows.Media.MediaPlayer();
            _MediaPlayer.MediaOpened += new EventHandler(_MediaPlayer_MediaOpened);
            _MediaPlayer.MediaEnded += new EventHandler(_MediaPlayer_MediaEnded);

            _VideoDrawing = new System.Windows.Media.VideoDrawing();
            _VideoDrawing.Player = _MediaPlayer;
            _VideoDrawing.Rect = new System.Windows.Rect(0, 0, 10, 10);
        }

        void _MediaPlayer_MediaEnded(object sender, EventArgs e)
        {
            if (Program.PauseInsteadOfStop && !Loop)
                Pause();

            if (Loop)
                Play();
        }

        private void _MediaPlayer_MediaOpened(object sender, EventArgs e)
        {
            var mp = (System.Windows.Media.MediaPlayer)sender;
            if (!_Opened)
            {
                if (!mp.HasVideo && !mp.HasAudio || mp.NaturalDuration.TimeSpan.TotalSeconds == 0)
                {
                    Remove();
                    return;
                }

                Length = (float)mp.NaturalDuration.TimeSpan.TotalSeconds;

                if (mp.HasVideo)
                {
                    Type = FileType.Video;
                    _VideoDrawing.Rect = new System.Windows.Rect(0, 0, mp.NaturalVideoWidth, mp.NaturalVideoHeight);
                }
                else if (mp.HasAudio)
                    Type = FileType.Audio;
                else
                {
                    Remove();
                    return;
                }

                mp.Close();

                Status = State.NotPlayed;
                Name = Path.GetFileNameWithoutExtension(FilePath);
                _Opened = true;
                return;
            }

            mp.Play();
            Status = State.Playing;
        }

        public void SetFile(string filePath)
        {
            Remove();

            if (!File.Exists(filePath))
            {
                Status = State.FileMissing;
                return;
            }

            FilePath = filePath;

            string ext = (Path.GetExtension(FilePath)).ToLower();
            if (ext == ".jpeg" || ext == ".jpg" || ext == ".png" || ext == ".bmp")
            {
                Type = FileType.Image;
                Length = 0;
                try
                {
                    Image = new System.Windows.Media.Imaging.BitmapImage(new Uri(FilePath, UriKind.RelativeOrAbsolute));
                    Status = State.NotPlayed;
                    Name = Path.GetFileNameWithoutExtension(FilePath);
                    _Opened = true;
                }
                catch (Exception)
                {
                    Remove();
                }
            }
            else
            {
                _MediaPlayer.Open(new Uri(FilePath, UriKind.RelativeOrAbsolute));           
            }
        }

        public void Remove()
        {
            if (_Opened)
                _MediaPlayer.Close();

            Status = State.Empty;
            Type = FileType.Empty;
            FilePath = String.Empty;
            Length = 0f;
            Name = String.Empty;
            Position = 0f;

            Loop = false;
            Image = null;
        }

        public System.Windows.Media.Brush Play()
        {
            if (!_Opened)
                return null;

            Stop();

            if (Type == FileType.Audio || Type == FileType.Video)
            {
                _MediaPlayer.Open(new Uri(FilePath, UriKind.RelativeOrAbsolute));
                return _ReturnBrush(_VideoDrawing);
            }

            Status = State.Playing;
            return _ReturnBrush(Image);
        }

        public System.Windows.Media.Brush Pause()
        {
            if (!_Opened)
                return null;

            if (Status == State.Playing)
            {
                Status = State.Paused;

                if (Type == FileType.Audio || Type == FileType.Video)
                {
                    _MediaPlayer.Pause();
                    return _ReturnBrush(_VideoDrawing);
                }
                return _ReturnBrush(Image);
            }
            else if (Status == State.Paused)
            {
                Status = State.Playing;

                if (Type == FileType.Audio || Type == FileType.Video)
                {
                    _MediaPlayer.Play();
                    return _ReturnBrush(_VideoDrawing);
                }
                return _ReturnBrush(Image);
            }
            else
                return Play();
        }

        public void Stop()
        {
            if (!_Opened)
                return;

            if (Type == FileType.Audio || Type == FileType.Video)
                _MediaPlayer.Close();

            if (Status == State.Paused || Status == State.Playing)
                Status = State.Finished;
        }

        public void Reset()
        {
            if (!_Opened)
                return;

            Stop();
            if (Status != State.Empty && Status != State.FileMissing)
                Status = State.NotPlayed;
        }

        private System.Windows.Media.Brush _ReturnBrush(System.Windows.Media.VideoDrawing VideoDrawing)
        {
            return new System.Windows.Media.DrawingBrush(VideoDrawing) { Stretch = System.Windows.Media.Stretch.Uniform };
        }

        private System.Windows.Media.Brush _ReturnBrush(System.Windows.Media.Imaging.BitmapImage Image)
        {
            return new System.Windows.Media.ImageBrush(Image) { Stretch = System.Windows.Media.Stretch.Uniform };
        }

        private void CheckStatus()
        {
            if (!_Opened)
                return;

            if (Status == State.Playing || Status == State.Paused)
            {
                if (Type == FileType.Video || Type == FileType.Audio)
                {
                    if (_MediaPlayer.Position.TotalSeconds == Length)
                    {
                        if (!Program.PauseInsteadOfStop)
                            Stop();

                        if (!Loop)
                        {
                            Status = State.Finished;
                            Position = 0f;
                        }
                        else
                            Play();
                    }
                    else
                    {
                        Position = (float)_MediaPlayer.Position.TotalSeconds;
                    }
                }
                else
                {
                    Position = 0;
                }
            }
        }

        public bool isMouseOver(int mx, int my)
        {
            return mx >= x && mx <= x + w && my >= y && my <= y + h;
        }

        public void Draw(Graphics g, int x, int y, int w, int h, int mx, int my)
        {
            CheckStatus();

            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;

            Brush brush = Brushes.Black;
            String text = "---";
            String duration = "--:--";

            int seconds = (int)(Length - Position);
            int minutes = (int)((Length - Position) / 60f);
            seconds = seconds - minutes * 60;

            switch (Status)
            {
                case State.Empty:
                    brush = Brushes.Gray;
                    text = Nr.ToString("00");
                    break;
                case State.FileMissing:
                    text = Name;
                    brush = Brushes.LightGray;
                    break;
                case State.NotPlayed:
                    text = Name;
                    brush = Brushes.LightBlue;
                    duration = minutes.ToString("00") + ":" + seconds.ToString("00");
                    break;
                case State.Playing:
                    text = Name;
                    brush = Brushes.Red;
                    duration = minutes.ToString("00") + ":" + seconds.ToString("00");
                    break;
                case State.Paused:
                    text = Name;
                    brush = Brushes.Orange;
                    duration = minutes.ToString("00") + ":" + seconds.ToString("00");
                    break;
                case State.Finished:
                    text = Name;
                    brush = Brushes.LightGreen;
                    duration = minutes.ToString("00") + ":" + seconds.ToString("00");
                    break;
                default:
                    break;
            }

            if (Loop)
                duration += " (L)";

            switch (Type)
            {
                case FileType.Empty:
                    break;
                case FileType.Unsupported:
                    duration += " Error";
                    break;
                case FileType.Audio:
                    duration += " Audio";
                    break;
                case FileType.Video:
                    duration += " Video";
                    break;
                case FileType.Image:
                    duration += " Image";
                    break;
                default:
                    break;
            }

            g.FillRectangle(brush, x, y, w, h);

            if (mx >= x && mx <= x + w && my >= y && my <= y + h)
            {
                g.DrawRectangle(new Pen(Brushes.Blue, 3), new Rectangle(x, y, w, h));
            }

            g.DrawString(text,
                new Font(FontFamily.GenericSansSerif, h / 4f, FontStyle.Regular, GraphicsUnit.Pixel, 0, false),
                Brushes.Black,
                x + 5,
                y + h / 6 + 2);

            g.DrawString(duration,
                new Font(FontFamily.GenericSansSerif, h / 4f, FontStyle.Regular, GraphicsUnit.Pixel, 0, false),
                Brushes.Black,
                x + 5,
                y + h / 6*3 + 2);
        }
    }
}
