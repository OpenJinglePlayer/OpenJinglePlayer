using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading;

using OpenJinglePlayer.Lib.Draw;

namespace OpenJinglePlayer
{
    enum FileType
    {
        Empty,
        Unsupported,
        Audio,
        Video,
        AudioVideo,
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
        private int _stream;
        private VideoPlayer _video;
        public STexture VideoTexture;
        public bool Loop;
        public Bitmap Image;

        private Thread _ThreadOpener;
        private bool _Opened;
        
        private static Object _MutexOpening = new object();

        int x = 0, y = 0, w = 0, h = 0;
        
        public Tile(int Nr)
        {
            Status = State.Empty;
            Type = FileType.Empty;
            FilePath = String.Empty;
            _stream = -1;
            Length = 0f;
            Name = String.Empty;
            Position = 0f;
            _video = new VideoPlayer();
            VideoTexture = new STexture(-1);
            this.Nr = Nr;
            Loop = false;
            _Opened = false;
            Image = null;
        }

        private void Opener()
        {
            string ext = (Path.GetExtension(FilePath)).ToLower();

            lock (_MutexOpening)
            {
                if (ext == ".jpeg" || ext == ".jpg" || ext == ".png" || ext == ".bmp")
                {
                    Type = FileType.Image;
                    _stream = -1;
                    Length = 0;
                    try
                    {
                        Image = new Bitmap(FilePath);
                        VideoTexture = CDraw.AddTexture(Image);
                    }
                    catch (Exception)
                    {
                        Remove();
                        return;
                    }
                }
                else if (ext == ".mp3" || ext == ".wav")
                {
                    Type = FileType.Audio;
                    _stream = CSound.Load(FilePath);
                    Length = CSound.GetLength(_stream);
                    if (Length == 0.0)
                    {
                        Remove();
                        return;
                    }
                }
                else
                {
                    _stream = CSound.Load(FilePath);
                    Length = CSound.GetLength(_stream);
                    CSound.Close(_stream);
                    if (Length > 0.0)
                        Type = FileType.AudioVideo;
                    else
                    {
                        Type = FileType.Video;
                        int stream = CVideo.VdLoad(FilePath);

                        Length = CVideo.VdGetLength(stream);
                        CVideo.VdClose(stream);
                    }
                } 
            }
            Status = State.NotPlayed;
            Name = Path.GetFileNameWithoutExtension(FilePath);
            _Opened = true;
            
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

            _ThreadOpener = new Thread(Opener);
            _ThreadOpener.Start();
        }

        public void Remove()
        {
            CSound.Close(_stream);

            if (_video != null)
            {
                _video.Close();
                _video = null;
            }

            Status = State.Empty;
            Type = FileType.Empty;
            FilePath = String.Empty;
            _stream = -1;
            Length = 0f;
            Name = String.Empty;
            Position = 0f;
            _video = new VideoPlayer();
            CDraw.RemoveTexture(ref VideoTexture);
            Loop = false;
            Image = null;
        }

        public void Play()
        {
            if (!_Opened)
                return;

            Stop();

            if (_video == null)
                _video = new VideoPlayer();

            _video.Load(FilePath);
            _stream = CSound.Load(FilePath);
            CSound.SetStreamVolume(_stream, 0f);

            _video.Start();
            CSound.Play(_stream);
            CSound.Fade(_stream, 100f, 0.5f);
            Status = State.Playing;          
        }

        public void Pause()
        {
            if (!_Opened)
                return;

            if (Status == State.Playing)
            {

                _video.Pause();
                CSound.Pause(_stream);
                Status = State.Paused;
            }
            else if (Status == State.Paused)
            {
                _video.Resume();
                CSound.Play(_stream);    

                Status = State.Playing;
            }
        }

        public void Stop()
        {
            if (!_Opened)
                return;

            CSound.Close(_stream);

            if (_video != null)
            {
                _video.Close();
                _video = null;
            }

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

        private void CheckStatus(bool DoDraw)
        {
            if (!_Opened)
                return;

            bool draw = DoDraw;
            if (Status == State.Playing)
            {
                if (Type == FileType.Video)
                {
                    if (_video.IsFinished)
                    {
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
                        Position = _video.GetPosition();
                        VideoTexture = _video.Draw(DoDraw, -1f);
                    }
                }
                else if (Type != FileType.Image)
                {
                    if (CSound.IsFinished(_stream))
                    {
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
                        Position = CSound.GetPosition(_stream);
                        VideoTexture = _video.Draw(DoDraw, Position);
                    }
                }
                else
                {
                    Position = 0;

                    RectangleF bounds = new RectangleF(0f, 0f, CDraw.GetScreenWidth(), CDraw.GetScreenHeight());
                    RectangleF rect = new RectangleF(0f, 0f, VideoTexture.width, VideoTexture.height);
                    CHelper.SetRect(bounds, ref rect, rect.Width / rect.Height, EAspect.Crop);
                    CDraw.DrawTexture(VideoTexture, new SRectF(rect.X, rect.Y, rect.Width, rect.Height, 100f / 4));

                    VideoTexture.NewImage = true;
                }
            }
            else
                draw = false;

            if (draw)
                CDraw.DrawColor(new Lib.Draw.SColorF(0f, 0f, 0f, 0f), new Lib.Draw.SRectF(0f, 0f, CDraw.GetScreenWidth(), CDraw.GetScreenHeight(), -1));
        }

        public bool isMouseOver(int mx, int my)
        {
            return mx >= x && mx <= x + w && my >= y && my <= y + h;
        }

        public void Draw(Graphics g, int x, int y, int w, int h, int mx, int my, bool VideoWindowOpen)
        {
            CheckStatus(VideoWindowOpen);

            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;

            Brush brush = Brushes.Black;
            String text = "---";
            String duration = "--:--";

            if (Type == FileType.Video && _video != null)
                Length = _video.GetLength();

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

            g.FillRectangle(brush, x, y, w, h);

            if (mx >= x && mx <= x + w && my >= y && my <= y + h)
            {
                g.DrawRectangle(new Pen(Brushes.Blue, 3), new Rectangle(x, y, w, h));
            }

            g.DrawString(text,
                new Font(FontFamily.GenericSansSerif, h / 4, FontStyle.Regular, GraphicsUnit.Pixel, 0, false),
                Brushes.Black,
                x + 5,
                y + h / 6 + 2);

            g.DrawString(duration,
                new Font(FontFamily.GenericSansSerif, h / 4, FontStyle.Regular, GraphicsUnit.Pixel, 0, false),
                Brushes.Black,
                x + 5,
                y + h / 6*3 + 2);
        }
    }
}
