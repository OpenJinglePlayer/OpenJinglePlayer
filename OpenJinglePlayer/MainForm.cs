﻿using System;
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
using System.Xml;
using System.Xml.XPath;

using OpenJinglePlayer.Lib.Draw;

namespace OpenJinglePlayer
{
    public partial class MainForm : Form
    {
        private Status status;

        private Shemes shemes;
        private Bitmap backBuffer;
        private Bitmap videoImage;
        private Graphics graphics;
        private Color clearColor;

        private int mouseX = 0, mouseY = 0;
        private int menuX = 0, menuY = 0;
        
        private bool running = true;
        private bool saved = false;

        fmEditName EditNameForm = new fmEditName();
        fmAbout AboutForm = new fmAbout();

        Object mutex = new object();

        public MainForm()
        {
            InitializeComponent();

            CSound.PlaybackInit();
            CVideo.Init();

            CDraw.InitDraw();

            status = new Status();
            clearColor = this.BackColor;
            backBuffer = new Bitmap(ClientSize.Width, ClientSize.Height);
            graphics = Graphics.FromImage(backBuffer);
            graphics.Clear(clearColor);
            
            shemes = new Shemes();
            tscbShemes.Items.Clear();
            tscbShemes.Items.Add(shemes.GetCurrentShemeName());
            tscbShemes.SelectedIndex = 0;
            videoImage = null;

            LoadLastShemeName();

            FlipBuffer();
        }

        private void SaveLastShemeName(string FilePath)
        {
            try
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.Encoding = Encoding.UTF8;
                settings.ConformanceLevel = ConformanceLevel.Document;

                string file = Path.Combine(Environment.CurrentDirectory, "LastFile.xml");
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
                    string file = Path.Combine(Environment.CurrentDirectory, "LastFile.xml");
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
            Run();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            lock (mutex)
            {
                running = false;
            }
            base.OnClosing(e);

            CSound.CloseAllStreams();
            CVideo.VdCloseAll();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (ClientSize.Width == 0 || ClientSize.Height == 0)
                return;

            Graphics gFrontBuffer = Graphics.FromHwnd(this.Handle);
            gFrontBuffer.Clear(clearColor);

            backBuffer = new Bitmap(ClientSize.Width, ClientSize.Height);
            graphics = Graphics.FromImage(backBuffer);
            graphics.Clear(clearColor);

            FlipBuffer();
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
                    shemes.StopAll();
                    videoImage = null;
                    tile.Play();
                }
                else
                {
                    tile.Stop();
                    videoImage = null;  
                }
            }
        }
        #endregion mouse handling

        private void Run()
        {
            while (running)
            {
                lock (mutex)
                {
                    Application.DoEvents();
                    
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

                        Draw();
                    }

                    //Thread.Sleep(1);
                }
            }
        }

        #region drawing
        private void Draw()
        {
            const float ratio = 0.75f;

            int h = this.ClientSize.Height;
            int w = this.ClientSize.Width;

            Graphics g = Graphics.FromImage(backBuffer);

            if (status.VideoScreenVisible)
                CDraw.BeforeDraw();

            Bitmap bmp = null;

            if (w > 0 && h > 0)
            {
                bmp = shemes.Draw(g, 10, 60, (int)(ratio * w), h - 58, 8, mouseX, mouseY, status.VideoScreenVisible);
                
                int iw = (int)((1f - ratio) * w) - 20;
                int ih = (int)(iw * 3f / 4f);

                shemes.DrawList(g, 10 + (int)(ratio * w), 60, iw, h - ih - 70, 8, mouseX, mouseY);
            }

            if (status.VideoScreenVisible)
                CDraw.AfterDraw();


            if (bmp != null && w > 0 && h > 0)
            {
                videoImage = bmp;               
            }

            if (videoImage != null && w > 0 && h > 0)
            {
                int x = (int)(ratio * w) + 10;
                int iw = (int)((1f - ratio) * w) - 20;
                int ih = (int)(iw * 3f / 4f);
                int y = h - ih - 10;

                RectangleF bounds = new RectangleF(x, y, iw, ih);
                RectangleF rect = new RectangleF(0f, 0f, videoImage.Width, videoImage.Height);
                CHelper.SetRect(bounds, ref rect, rect.Width / rect.Height, EAspect.Crop);

                int dx = (int)((bounds.X - rect.X) * 0.5f / bounds.Width * videoImage.Width);
                int dy = (int)((bounds.Y - rect.Y) * 0.5f / bounds.Height * videoImage.Height);

                int dw = videoImage.Width - 2 * dx;
                int dh = videoImage.Height - 2 * dy;

                g.DrawImage(videoImage, new Rectangle(x, y, iw, ih), dx, dy, dw, dh, GraphicsUnit.Pixel);
            }

            if (w > 0 && h > 0)
                FlipBuffer(); 
        }

        private void FlipBuffer()
        {
            DrawBuffer();
            graphics.Clear(clearColor);
        }

        private void DrawBuffer()
        {
            Graphics gFrontBuffer = Graphics.FromHwnd(this.Handle);
            gFrontBuffer.DrawImage(backBuffer, 0f, 0f);
        }
        #endregion drawing

        private void tsbtVideoScreen_Click(object sender, EventArgs e)
        {
            lock (mutex)
            {
                status.VideoScreenVisible = !status.VideoScreenVisible;

                if (status.VideoScreenVisible)
                {
                    CDraw.Show();
                    tsbtToggleFullscreen.Enabled = true;
                }
                else
                {
                    CDraw.Hide();
                    tsbtToggleFullscreen.Enabled = false;
                }
            }
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
                videoImage = null;
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
            if (!status.VideoScreenVisible)
                return;

            if (!CDraw.IsFullScreen())
            {
                CDraw.EnterFullScreen();
                tsbtToggleFullscreen.Image = Properties.Resources.view_restore;
            }
            else
            {
                CDraw.LeaveFullScreen();
                tsbtToggleFullscreen.Image = Properties.Resources.view_fullscreen;
            }
        }
    }
}
