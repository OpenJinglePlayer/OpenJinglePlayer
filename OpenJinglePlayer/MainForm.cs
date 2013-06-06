using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.XPath;


namespace OpenJinglePlayer
{
    public partial class MainForm : Form
    {
        private Thread _Runner;
        private Thread _Loader;
        private BufferedGraphicsContext context;
        private BufferedGraphics grafx;
        private SolidBrush brush;        

        private Shemes shemes;

        private int mouseX = 0, mouseY = 0;
        private int menuX = 0, menuY = 0;
        private Rectangle rect;
        
        private bool running = true;
        private bool saved = false;

        fmEditName EditNameForm = new fmEditName();
        fmAbout AboutForm = new fmAbout();

        Object mutex = new object();

        private VideoWindow _VideoWindow;
        private System.Windows.Media.Brush _Brush;

        public MainForm()
        {
            InitializeComponent();

            _VideoWindow = new VideoWindow();
            _Brush = null;
            
            shemes = new Shemes();
            tscbShemes.Items.Clear();
            tscbShemes.Items.Add(shemes.GetCurrentShemeName());
            tscbShemes.SelectedIndex = 0;

            LoadLastShemeName();

            if (Program.PauseInsteadOfStop)
                tsmiPauseInsteadStop.Image = Properties.Resources.ok;
            else
                tsmiPauseInsteadStop.Image = null;

            brush = new SolidBrush(BackColor);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            this.Resize += new System.EventHandler(this.OnResize);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.OnPaint);

            UpdateRect();
            context = BufferedGraphicsManager.Current;
            context.MaximumBuffer = rect.Size;
            grafx = context.Allocate(this.CreateGraphics(), rect);
            DrawToBuffer(grafx.Graphics);

            _Runner = new Thread(Runner);
            _Runner.Start();

            _Loader = new Thread(new ParameterizedThreadStart(LoadTile));
        }

        void UpdateRect()
        {
            rect = new Rectangle(0, 0, ClientSize.Width, ClientSize.Height - statusStrip1.Height);
            if (rect.Height < 0)
                rect.Height = 0;
        }

        void LoadTile(object o)
        {
            Tile tile = null;
            try
            {
                tile = (Tile)o;
            }
            catch {}

            if (tile != null)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    tile.OpenFile();
                });

                while (tile.WaitForOpen && running)
                    Thread.Sleep(10);
            }
        }

        void Runner()
        {
            while (running)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    DrawToBuffer(grafx.Graphics);
                    grafx.Render(Graphics.FromHwnd(this.Handle));
                });

                Tile t = shemes.GetNextNotOpenedTile();
                if (t != null && !_Loader.IsAlive)
                {
                    this.Invoke((MethodInvoker)delegate { tsslStatus.Text = "Lädt..."; });
                    _Loader = new Thread(new ParameterizedThreadStart(LoadTile));
                    _Loader.Start(t);
                }
                else
                {
                    if (t == null)
                        this.Invoke((MethodInvoker)delegate { tsslStatus.Text = "Laden abgeschlossen"; });

                    Thread.Sleep(50);
                }
            }

            this.Invoke((MethodInvoker)delegate { this.Close(); });
        }

        private void SaveLastShemeName(string FilePath)
        {
            try
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.Encoding = Encoding.UTF8;
                settings.ConformanceLevel = ConformanceLevel.Document;

                string file = Path.Combine(Application.StartupPath, "LastFile.xml");
                XmlWriter writer = XmlWriter.Create(file, settings);

                writer.WriteStartDocument();
                writer.WriteStartElement("root");

                writer.WriteElementString("LastFile", FilePath);

                // End of File
                writer.WriteEndElement(); //end of root
                writer.WriteEndDocument();

                writer.Flush();
                writer.Close();
            }
            catch (Exception)
            {
                ;
            }
        }

        private void LoadLastShemeName()
        {
            try
            {
                XPathDocument xPathDoc = null;
                XPathNavigator navigator = null;

                bool opened = false;
                try
                {
                    string file = Path.Combine(Application.StartupPath, "LastFile.xml");
                    xPathDoc = new XPathDocument(file);
                    navigator = xPathDoc.CreateNavigator();
                    opened = true;
                }
                catch (Exception)
                {
                    if (navigator != null)
                        navigator = null;

                    if (xPathDoc != null)
                        xPathDoc = null;
                }

                if (opened)
                {

                    string filename = string.Empty;
                    CHelper.GetValueFromXML("LastFile", navigator, ref filename, filename);
                    OpenSheme(filename);
                }

            }
            catch (Exception)
            {
                ;
            }
        }

        #region form events
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);           
        }

        private void OnPaint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            grafx.Render(e.Graphics);
        }

        private void OnResize(object sender, System.EventArgs e)
        {
            UpdateRect();
            // Re-create the graphics buffer for a new window size.
            context.MaximumBuffer = rect.Size;
            if (grafx != null)
            {
                grafx.Dispose();
                grafx = null;
            }
            grafx = context.Allocate(this.CreateGraphics(), rect);

            // Cause the background to be cleared and redraw.
            DrawToBuffer(grafx.Graphics);
            this.Refresh();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (running)
            {
                running = false;
                e.Cancel = true;
                shemes.StopAll();
            }
            
            base.OnFormClosing(e);
        }

        private void DrawToBuffer(Graphics g)
        {
            RectangleF Rect = g.VisibleClipBounds;
            // Clear the graphics buffer
            g.FillRectangle(brush , Rect);

            if (running)
            {
                Text = Status.ProgramNameVersionString + " - " + Path.GetFileNameWithoutExtension(shemes.FileName);

                if (!saved)
                    Text += "*";

                tsbtSaveFile.Enabled = shemes.FileName != String.Empty;
                tscbShemes.Enabled = shemes.ShemeList != null;
                tsbtSaveAs.Enabled = shemes.ShemeList != null;
                tsbtEdit.Enabled = shemes.ShemeList != null;
                tsbtRemove.Enabled = shemes.ShemeList != null;

                tsmiSaveAs.Enabled = shemes.ShemeList != null;
                tsmiEdit.Enabled = shemes.ShemeList != null;
                tsmiRemove.Enabled = shemes.ShemeList != null;
                Draw(g);
            }
        }
        #endregion form events

        #region mouse handling
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            mouseX = e.X;
            mouseY = e.Y;
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            mouseX = e.X;
            mouseY = e.Y;

            lock (mutex)
            {
                int index = shemes.MouseClick(e.X, e.Y);
                if (index >= 0)
                {
                    tscbShemes.SelectedIndex = index;
                    return;
                }

                Tile tile = shemes.GetTile(e.X, e.Y);

                if (tile == null)
                    return;

                if (e.Button == System.Windows.Forms.MouseButtons.Left && (tile.Status == State.Empty || tile.Status == State.FileMissing))
                {
                    ofdMedia.FileName = String.Empty;
                    if (ofdMedia.ShowDialog() == DialogResult.OK)
                    {
                        tile.SetFile(ofdMedia.FileName);
                        saved = false;
                    }
                }
                else if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    tsmiDelete.Enabled = tile.Status != State.Empty;
                    tsmiReset.Enabled = tsmiDelete.Enabled;
                    tsmiChangeName.Enabled = tsmiDelete.Enabled;
                    tsmiLoop.Enabled = tsmiDelete.Enabled;

                    if (!tile.Loop)
                        tsmiLoop.Image = null;
                    else
                        tsmiLoop.Image = Properties.Resources.ok;

                    menuX = e.X;
                    menuY = e.Y;
                    cmsTile.Show(this, e.X, e.Y);
                }
                else if (tile.Status == State.Finished || tile.Status == State.NotPlayed || tile.Status == State.Paused)
                {
                    if (!Program.PauseInsteadOfStop)
                    {
                        shemes.StopAll();
                        _Brush = tile.Play();
                    }
                    else
                    {
                        shemes.StopAll(tile);
                        _Brush = tile.Pause();
                    }
                }
                else
                {
                    if (Program.PauseInsteadOfStop)
                        _Brush = tile.Pause();
                    else
                    {
                        tile.Stop();
                        _Brush = null;
                    }
                }

                _VideoWindow.SetSource(_Brush);
                vcMain.Background = _Brush;
            }
        }
        #endregion mouse handling

        #region drawing
        private void Draw(Graphics g)
        {
            const float ratio = 0.75f;

            int h = rect.Height;
            int w = rect.Width;

            if (w > 0 && h > 0)
            {
                shemes.Draw(g, 10, 60, (int)(ratio * w), h - 58, 8, mouseX, mouseY);

                int ew = (int)((1f - ratio) * w) - 20;
                int eh = (int)(ew * 3f / 4f);

                shemes.DrawList(g, 10 + (int)(ratio * w), 60, ew, h - eh - 70, 8, mouseX, mouseY);
            }

            int x = (int)(ratio * w) + 10;
            int iw = (int)((1f - ratio) * w) - 20;
            int ih = (int)(iw * 3f / 4f);
            int y = h - ih - 10;

            RectangleF bounds = new RectangleF(x, y, iw, ih);
            elementHost1.SetBounds((int)bounds.X, (int)bounds.Y, (int)bounds.Width, (int)bounds.Height);
        }
        #endregion drawing

        private void tsbtVideoScreen_Click(object sender, EventArgs e)
        {
            ToggleVideoScreen();
        }

        #region tile handling
        private void tmsiDelete_Click(object sender, EventArgs e)
        {
            Tile tile = shemes.GetTile(menuX, menuY);

            if (tile == null)
                return;

            tile.Remove();
            saved = false;
        }

        private void tsmiNew_Click(object sender, EventArgs e)
        {
            Tile tile = shemes.GetTile(menuX, menuY);

            if (tile == null)
                return;

            ofdMedia.FileName = String.Empty;
            if (ofdMedia.ShowDialog() == DialogResult.OK)
            {
                tile.SetFile(ofdMedia.FileName);
                saved = false;
            }
        }

        private void tsmiReset_Click(object sender, EventArgs e)
        {
            Tile tile = shemes.GetTile(menuX, menuY);

            if (tile == null)
                return;

            tile.Reset();
        }

        private void tsmiChangeName_Click(object sender, EventArgs e)
        {
            Tile tile = shemes.GetTile(menuX, menuY);

            if (tile == null)
                return;

            EditNameForm.OldFileName = tile.Name;
            EditNameForm.NewFileName = tile.Name;

            if (EditNameForm.ShowDialog() == DialogResult.OK)
            {
                if (EditNameForm.NewFileName != String.Empty)
                {
                    tile.Name = EditNameForm.NewFileName;
                    saved = false;
                }
            }
        }

        private void tsmiLoop_Click(object sender, EventArgs e)
        {
            Tile tile = shemes.GetTile(menuX, menuY);

            if (tile == null)
                return;

            if (tile.Status != State.Empty)
            {
                tile.Loop = !tile.Loop;
                saved = false;
            }
        }
        #endregion tile handling

        #region files
        private void NewSheme()
        {
            lock (mutex)
            {
                shemes.StopAll();
                shemes = new Shemes();
                tscbShemes.Items.Clear();
                tscbShemes.Items.Add(shemes.GetCurrentShemeName());
                tscbShemes.SelectedIndex = 0;
            }
        }

        private void OpenSheme()
        {
            ofdShemes.FileName = String.Empty;
            if (ofdShemes.ShowDialog() == DialogResult.OK)
            {
                OpenSheme(ofdShemes.FileName);
            }
        }

        private void OpenSheme(string FileName)
        {
            if (FileName == null)
                return;

            if (FileName == String.Empty)
                return;

            if (!File.Exists(FileName))
                return;

            lock (mutex)
            {
                shemes.StopAll();
                if (shemes.Load(FileName))
                {
                    saved = true;
                    SaveLastShemeName(FileName);
                }

                tscbShemes.Items.Clear();
                String[] sl = shemes.ShemeList;
                if (sl != null)
                {
                    tscbShemes.Items.AddRange(sl);
                    tscbShemes.SelectedIndex = shemes.GetSelection();
                }
            }
        }

        private void SaveSheme()
        {
            if (shemes.FileName == String.Empty)
                return;

            if (!File.Exists(shemes.FileName))
                return;

            lock (mutex)
            {
                saved = shemes.Save();
                SaveLastShemeName(shemes.FileName);
            }
        }

        private void SaveShemeAs()
        {
            sfdShemes.FileName = String.Empty;
            if (sfdShemes.ShowDialog() == DialogResult.OK)
            {
                if (sfdShemes.FileName != String.Empty)
                    saved = shemes.SaveAs(sfdShemes.FileName);

                if (saved)
                    SaveLastShemeName(shemes.FileName);
            }
        }
        #endregion files

        #region shemes
        private void RemoveSheme()
        {
            lock (mutex)
            {
                shemes.StopAll();
                shemes.DeleteSheme();
                tscbShemes.Items.Clear();
                String[] sl = shemes.ShemeList;
                if (sl != null)
                {
                    tscbShemes.Items.AddRange(sl);
                    tscbShemes.SelectedIndex = shemes.GetSelection();

                    SaveLastShemeName(shemes.GetCurrentShemeName());
                }
                else
                {
                    tscbShemes.Items.Clear();
                    tscbShemes.Text = String.Empty;
                }
            }
        }

        private void EditSheme()
        {
            String name = shemes.GetCurrentShemeName();
            if (name == null)
                return;

            EditNameForm.OldFileName = name;
            EditNameForm.NewFileName = name;

            if (EditNameForm.ShowDialog() == DialogResult.OK)
            {
                if (EditNameForm.NewFileName != String.Empty)
                {
                    shemes.SetCurrentShemeName(EditNameForm.NewFileName);
                    saved = false;
                }
            }

            tscbShemes.Items.Clear();
            String[] sl = shemes.ShemeList;
            if (sl != null)
            {
                tscbShemes.Items.AddRange(sl);
                tscbShemes.SelectedIndex = shemes.GetSelection();                
            }
        }

        private void AddSheme()
        {
            lock (mutex)
            {
                shemes.StopAll();
                shemes.AddSheme();
                tscbShemes.Items.Clear();
                String[] sl = shemes.ShemeList;
                if (sl != null)
                {
                    shemes.Select(sl.Length - 1);
                    tscbShemes.Items.AddRange(sl);
                    tscbShemes.SelectedIndex = sl.Length - 1;
                }
            }
        }
        #endregion shemes

        #region other
        private void ToggleVideoScreen()
        {
            lock (mutex)
            {
                Program.Status.VideoScreenVisible = !Program.Status.VideoScreenVisible;

                if (Program.Status.VideoScreenVisible)
                {
                    _VideoWindow.Show();
                    tsbtToggleFullscreen.Enabled = true;
                    tsmiShowVideoScreen.Image = Properties.Resources.ok;
                }
                else
                {
                    _VideoWindow.Hide();
                    tsbtToggleFullscreen.Enabled = false;
                    tsmiShowVideoScreen.Image = null;
                }
            }
        }
        private void ToggleFullScreen()
        {
            if (!Program.Status.VideoScreenVisible)
                return;

            if (!_VideoWindow.IsFullScreen())
            {
                _VideoWindow.EnterFullScreen();
                tsbtToggleFullscreen.Image = Properties.Resources.view_restore;
            }
            else
            {
                _VideoWindow.LeaveFullScreen();
                tsbtToggleFullscreen.Image = Properties.Resources.view_fullscreen;
            }
        }
        #endregion

        #region menu events
        private void tsmiExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tsbtNewFile_Click(object sender, EventArgs e)
        {
            NewSheme();
        }

        private void tsmiNewFile_Click(object sender, EventArgs e)
        {
            NewSheme();
        }

        private void tsbtSaveFile_Click(object sender, EventArgs e)
        {
            SaveSheme();
        }

        private void tsmiSaveFile_Click(object sender, EventArgs e)
        {
            SaveSheme();
        }

        private void tsbtSaveAs_Click(object sender, EventArgs e)
        {
            SaveShemeAs();
        }

        private void tsmiSaveAs_Click(object sender, EventArgs e)
        {
            SaveShemeAs();
        }

        private void tsbtOpenFile_Click(object sender, EventArgs e)
        {
            OpenSheme();
        }

        private void tsmiOpenFile_Click(object sender, EventArgs e)
        {
            OpenSheme();
        }

        private void tsbtAdd_Click(object sender, EventArgs e)
        {
            AddSheme();
        }

        private void tsmiAdd_Click(object sender, EventArgs e)
        {
            AddSheme();
        }

        private void tsbtRemove_Click(object sender, EventArgs e)
        {
            RemoveSheme();
        }

        private void tsmiRemove_Click(object sender, EventArgs e)
        {
            RemoveSheme();
        }

        private void tsbtEdit_Click(object sender, EventArgs e)
        {
            EditSheme();
        }

        private void tsmiEdit_Click(object sender, EventArgs e)
        {
            EditSheme();
        }
        #endregion menu events

        private void tscbShemes_SelectedIndexChanged(object sender, EventArgs e)
        {
            lock (mutex)
            {
                shemes.StopAll();
                shemes.Select(tscbShemes.SelectedIndex);
            }   
        }

        private void tsmiAbout_Click(object sender, EventArgs e)
        {
            AboutForm.Show();
        }

        private void tsbtToggleFullscreen_Click(object sender, EventArgs e)
        {
            ToggleFullScreen();
        }

        private void tsmiShowVideoScreen_Click(object sender, EventArgs e)
        {
            ToggleVideoScreen();
        }

        private void tsmiPauseInsteadStop_Click(object sender, EventArgs e)
        {
            Program.PauseInsteadOfStop = !Program.PauseInsteadOfStop;

            if (Program.PauseInsteadOfStop)
                tsmiPauseInsteadStop.Image = Properties.Resources.ok;
            else
                tsmiPauseInsteadStop.Image = null;
        }
    }
}
