namespace OpenJinglePlayer
{
    partial class MainForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tsmiFile = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiNewFile = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiOpenFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiSaveFile = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiExit = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiShemes = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiShowVideoScreen = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiPauseInsteadStop = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbtNewFile = new System.Windows.Forms.ToolStripButton();
            this.tsbtOpenFile = new System.Windows.Forms.ToolStripButton();
            this.tsbtSaveFile = new System.Windows.Forms.ToolStripButton();
            this.tsbtSaveAs = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tscbShemes = new System.Windows.Forms.ToolStripComboBox();
            this.tsbtEdit = new System.Windows.Forms.ToolStripButton();
            this.tsbtAdd = new System.Windows.Forms.ToolStripButton();
            this.tsbtRemove = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbtVideoScreen = new System.Windows.Forms.ToolStripButton();
            this.tsbtToggleFullscreen = new System.Windows.Forms.ToolStripButton();
            this.ofdMedia = new System.Windows.Forms.OpenFileDialog();
            this.sfdShemes = new System.Windows.Forms.SaveFileDialog();
            this.cmsTile = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiNew = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiReset = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiChangeName = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiLoop = new System.Windows.Forms.ToolStripMenuItem();
            this.ofdShemes = new System.Windows.Forms.OpenFileDialog();
            this.pbDummy = new System.Windows.Forms.PictureBox();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.cmsTile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbDummy)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiFile,
            this.tsmiShemes,
            this.tsmiOptions,
            this.tsmiHelp});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(856, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // tsmiFile
            // 
            this.tsmiFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiNewFile,
            this.tsmiOpenFile,
            this.toolStripSeparator1,
            this.tsmiSaveFile,
            this.tsmiSaveAs,
            this.toolStripSeparator2,
            this.tsmiExit});
            this.tsmiFile.Name = "tsmiFile";
            this.tsmiFile.Size = new System.Drawing.Size(46, 20);
            this.tsmiFile.Text = "Datei";
            // 
            // tsmiNewFile
            // 
            this.tsmiNewFile.Image = global::OpenJinglePlayer.Properties.Resources.document_new;
            this.tsmiNewFile.Name = "tsmiNewFile";
            this.tsmiNewFile.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.tsmiNewFile.Size = new System.Drawing.Size(168, 22);
            this.tsmiNewFile.Text = "Neu";
            this.tsmiNewFile.Click += new System.EventHandler(this.tsmiNewFile_Click);
            // 
            // tsmiOpenFile
            // 
            this.tsmiOpenFile.Image = global::OpenJinglePlayer.Properties.Resources.document_open;
            this.tsmiOpenFile.Name = "tsmiOpenFile";
            this.tsmiOpenFile.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.tsmiOpenFile.Size = new System.Drawing.Size(168, 22);
            this.tsmiOpenFile.Text = "Öffnen";
            this.tsmiOpenFile.Click += new System.EventHandler(this.tsmiOpenFile_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(165, 6);
            // 
            // tsmiSaveFile
            // 
            this.tsmiSaveFile.Enabled = false;
            this.tsmiSaveFile.Image = global::OpenJinglePlayer.Properties.Resources.document_save;
            this.tsmiSaveFile.Name = "tsmiSaveFile";
            this.tsmiSaveFile.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.tsmiSaveFile.Size = new System.Drawing.Size(168, 22);
            this.tsmiSaveFile.Text = "Speichern";
            this.tsmiSaveFile.Click += new System.EventHandler(this.tsmiSaveFile_Click);
            // 
            // tsmiSaveAs
            // 
            this.tsmiSaveAs.Image = global::OpenJinglePlayer.Properties.Resources.document_save_as;
            this.tsmiSaveAs.Name = "tsmiSaveAs";
            this.tsmiSaveAs.Size = new System.Drawing.Size(168, 22);
            this.tsmiSaveAs.Text = "Speichern unter...";
            this.tsmiSaveAs.Click += new System.EventHandler(this.tsmiSaveAs_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(165, 6);
            // 
            // tsmiExit
            // 
            this.tsmiExit.Name = "tsmiExit";
            this.tsmiExit.Size = new System.Drawing.Size(168, 22);
            this.tsmiExit.Text = "Beenden";
            this.tsmiExit.Click += new System.EventHandler(this.tsmiExit_Click);
            // 
            // tsmiShemes
            // 
            this.tsmiShemes.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiAdd,
            this.tsmiEdit,
            this.tsmiRemove});
            this.tsmiShemes.Name = "tsmiShemes";
            this.tsmiShemes.Size = new System.Drawing.Size(70, 20);
            this.tsmiShemes.Text = "Datensatz";
            // 
            // tsmiAdd
            // 
            this.tsmiAdd.Image = global::OpenJinglePlayer.Properties.Resources.list_add;
            this.tsmiAdd.Name = "tsmiAdd";
            this.tsmiAdd.Size = new System.Drawing.Size(136, 22);
            this.tsmiAdd.Text = "Hinzufügen";
            this.tsmiAdd.Click += new System.EventHandler(this.tsmiAdd_Click);
            // 
            // tsmiEdit
            // 
            this.tsmiEdit.Image = global::OpenJinglePlayer.Properties.Resources.edit;
            this.tsmiEdit.Name = "tsmiEdit";
            this.tsmiEdit.Size = new System.Drawing.Size(136, 22);
            this.tsmiEdit.Text = "Bearbeiten";
            this.tsmiEdit.Click += new System.EventHandler(this.tsmiEdit_Click);
            // 
            // tsmiRemove
            // 
            this.tsmiRemove.Image = global::OpenJinglePlayer.Properties.Resources.list_remove;
            this.tsmiRemove.Name = "tsmiRemove";
            this.tsmiRemove.Size = new System.Drawing.Size(136, 22);
            this.tsmiRemove.Text = "Löschen";
            this.tsmiRemove.Click += new System.EventHandler(this.tsmiRemove_Click);
            // 
            // tsmiOptions
            // 
            this.tsmiOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiShowVideoScreen,
            this.tsmiPauseInsteadStop});
            this.tsmiOptions.Name = "tsmiOptions";
            this.tsmiOptions.Size = new System.Drawing.Size(69, 20);
            this.tsmiOptions.Text = "Optionen";
            // 
            // tsmiShowVideoScreen
            // 
            this.tsmiShowVideoScreen.Name = "tsmiShowVideoScreen";
            this.tsmiShowVideoScreen.Size = new System.Drawing.Size(210, 22);
            this.tsmiShowVideoScreen.Text = "Videobildschirm anzeigen";
            this.tsmiShowVideoScreen.Click += new System.EventHandler(this.tsmiShowVideoScreen_Click);
            // 
            // tsmiPauseInsteadStop
            // 
            this.tsmiPauseInsteadStop.Name = "tsmiPauseInsteadStop";
            this.tsmiPauseInsteadStop.Size = new System.Drawing.Size(210, 22);
            this.tsmiPauseInsteadStop.Text = "Pausieren statt Stoppen";
            this.tsmiPauseInsteadStop.Click += new System.EventHandler(this.tsmiPauseInsteadStop_Click);
            // 
            // tsmiHelp
            // 
            this.tsmiHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiAbout});
            this.tsmiHelp.Name = "tsmiHelp";
            this.tsmiHelp.Size = new System.Drawing.Size(24, 20);
            this.tsmiHelp.Text = "?";
            // 
            // tsmiAbout
            // 
            this.tsmiAbout.Name = "tsmiAbout";
            this.tsmiAbout.Size = new System.Drawing.Size(107, 22);
            this.tsmiAbout.Text = "About";
            this.tsmiAbout.Click += new System.EventHandler(this.tsmiAbout_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbtNewFile,
            this.tsbtOpenFile,
            this.tsbtSaveFile,
            this.tsbtSaveAs,
            this.toolStripSeparator3,
            this.tscbShemes,
            this.tsbtEdit,
            this.tsbtAdd,
            this.tsbtRemove,
            this.toolStripSeparator4,
            this.tsbtVideoScreen,
            this.tsbtToggleFullscreen});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(856, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbtNewFile
            // 
            this.tsbtNewFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtNewFile.Image = global::OpenJinglePlayer.Properties.Resources.document_new;
            this.tsbtNewFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtNewFile.Name = "tsbtNewFile";
            this.tsbtNewFile.Size = new System.Drawing.Size(23, 22);
            this.tsbtNewFile.Text = "Neu";
            this.tsbtNewFile.Click += new System.EventHandler(this.tsbtNewFile_Click);
            // 
            // tsbtOpenFile
            // 
            this.tsbtOpenFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtOpenFile.Image = global::OpenJinglePlayer.Properties.Resources.document_open;
            this.tsbtOpenFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtOpenFile.Name = "tsbtOpenFile";
            this.tsbtOpenFile.Size = new System.Drawing.Size(23, 22);
            this.tsbtOpenFile.Text = "Öffnen";
            this.tsbtOpenFile.Click += new System.EventHandler(this.tsbtOpenFile_Click);
            // 
            // tsbtSaveFile
            // 
            this.tsbtSaveFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtSaveFile.Image = global::OpenJinglePlayer.Properties.Resources.document_save;
            this.tsbtSaveFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtSaveFile.Name = "tsbtSaveFile";
            this.tsbtSaveFile.Size = new System.Drawing.Size(23, 22);
            this.tsbtSaveFile.Text = "Speichern";
            this.tsbtSaveFile.Click += new System.EventHandler(this.tsbtSaveFile_Click);
            // 
            // tsbtSaveAs
            // 
            this.tsbtSaveAs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtSaveAs.Image = global::OpenJinglePlayer.Properties.Resources.document_save_as;
            this.tsbtSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtSaveAs.Name = "tsbtSaveAs";
            this.tsbtSaveAs.Size = new System.Drawing.Size(23, 22);
            this.tsbtSaveAs.Text = "Speichern unter...";
            this.tsbtSaveAs.Click += new System.EventHandler(this.tsbtSaveAs_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // tscbShemes
            // 
            this.tscbShemes.Name = "tscbShemes";
            this.tscbShemes.Size = new System.Drawing.Size(121, 25);
            this.tscbShemes.SelectedIndexChanged += new System.EventHandler(this.tscbShemes_SelectedIndexChanged);
            // 
            // tsbtEdit
            // 
            this.tsbtEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtEdit.Image = global::OpenJinglePlayer.Properties.Resources.edit;
            this.tsbtEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtEdit.Name = "tsbtEdit";
            this.tsbtEdit.Size = new System.Drawing.Size(23, 22);
            this.tsbtEdit.Text = "Datensatz bearbeiten";
            this.tsbtEdit.Click += new System.EventHandler(this.tsbtEdit_Click);
            // 
            // tsbtAdd
            // 
            this.tsbtAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtAdd.Image = global::OpenJinglePlayer.Properties.Resources.list_add;
            this.tsbtAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtAdd.Name = "tsbtAdd";
            this.tsbtAdd.Size = new System.Drawing.Size(23, 22);
            this.tsbtAdd.Text = "Datensatz hinzufügen";
            this.tsbtAdd.Click += new System.EventHandler(this.tsbtAdd_Click);
            // 
            // tsbtRemove
            // 
            this.tsbtRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtRemove.Image = global::OpenJinglePlayer.Properties.Resources.list_remove;
            this.tsbtRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtRemove.Name = "tsbtRemove";
            this.tsbtRemove.Size = new System.Drawing.Size(23, 22);
            this.tsbtRemove.Text = "Datensatz entfernen";
            this.tsbtRemove.Click += new System.EventHandler(this.tsbtRemove_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbtVideoScreen
            // 
            this.tsbtVideoScreen.Image = global::OpenJinglePlayer.Properties.Resources.datashow;
            this.tsbtVideoScreen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtVideoScreen.Name = "tsbtVideoScreen";
            this.tsbtVideoScreen.Size = new System.Drawing.Size(163, 22);
            this.tsbtVideoScreen.Text = "Videobildschirm anzeigen";
            this.tsbtVideoScreen.Click += new System.EventHandler(this.tsbtVideoScreen_Click);
            // 
            // tsbtToggleFullscreen
            // 
            this.tsbtToggleFullscreen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtToggleFullscreen.Enabled = false;
            this.tsbtToggleFullscreen.Image = global::OpenJinglePlayer.Properties.Resources.view_fullscreen;
            this.tsbtToggleFullscreen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtToggleFullscreen.Name = "tsbtToggleFullscreen";
            this.tsbtToggleFullscreen.Size = new System.Drawing.Size(23, 22);
            this.tsbtToggleFullscreen.Text = "Vollbild Ein/Aus";
            this.tsbtToggleFullscreen.Click += new System.EventHandler(this.tsbtToggleFullscreen_Click);
            // 
            // ofdMedia
            // 
            this.ofdMedia.Filter = "Supported Files|*.jpeg;*.jpg;*.png;*.bmp;*.mp3;*.wav;*.avi;*.mpg;*.mpeg;*.mp4;*.m" +
    "kv";
            // 
            // sfdShemes
            // 
            this.sfdShemes.Filter = "OpenJinglePlayer File|*.ojp";
            // 
            // cmsTile
            // 
            this.cmsTile.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiDelete,
            this.toolStripSeparator5,
            this.tsmiNew,
            this.tsmiReset,
            this.tsmiChangeName,
            this.toolStripSeparator6,
            this.tsmiLoop});
            this.cmsTile.Name = "cmsTile";
            this.cmsTile.Size = new System.Drawing.Size(178, 126);
            // 
            // tsmiDelete
            // 
            this.tsmiDelete.Image = global::OpenJinglePlayer.Properties.Resources.list_remove;
            this.tsmiDelete.Name = "tsmiDelete";
            this.tsmiDelete.Size = new System.Drawing.Size(177, 22);
            this.tsmiDelete.Text = "Löschen";
            this.tsmiDelete.Click += new System.EventHandler(this.tmsiDelete_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(174, 6);
            // 
            // tsmiNew
            // 
            this.tsmiNew.Image = global::OpenJinglePlayer.Properties.Resources.document_new;
            this.tsmiNew.Name = "tsmiNew";
            this.tsmiNew.Size = new System.Drawing.Size(177, 22);
            this.tsmiNew.Text = "Neu belegen";
            this.tsmiNew.Click += new System.EventHandler(this.tsmiNew_Click);
            // 
            // tsmiReset
            // 
            this.tsmiReset.Image = global::OpenJinglePlayer.Properties.Resources.view_refresh;
            this.tsmiReset.Name = "tsmiReset";
            this.tsmiReset.Size = new System.Drawing.Size(177, 22);
            this.tsmiReset.Text = "Status zurücksetzen";
            this.tsmiReset.Click += new System.EventHandler(this.tsmiReset_Click);
            // 
            // tsmiChangeName
            // 
            this.tsmiChangeName.Image = global::OpenJinglePlayer.Properties.Resources.edit;
            this.tsmiChangeName.Name = "tsmiChangeName";
            this.tsmiChangeName.Size = new System.Drawing.Size(177, 22);
            this.tsmiChangeName.Text = "Namen ändern";
            this.tsmiChangeName.Click += new System.EventHandler(this.tsmiChangeName_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(174, 6);
            // 
            // tsmiLoop
            // 
            this.tsmiLoop.Image = global::OpenJinglePlayer.Properties.Resources.ok;
            this.tsmiLoop.Name = "tsmiLoop";
            this.tsmiLoop.Size = new System.Drawing.Size(177, 22);
            this.tsmiLoop.Text = "Loop";
            this.tsmiLoop.Click += new System.EventHandler(this.tsmiLoop_Click);
            // 
            // ofdShemes
            // 
            this.ofdShemes.Filter = "OpenJinglePlayer Sheme|*.ojp";
            // 
            // pbDummy
            // 
            this.pbDummy.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbDummy.Location = new System.Drawing.Point(0, 52);
            this.pbDummy.Name = "pbDummy";
            this.pbDummy.Size = new System.Drawing.Size(856, 460);
            this.pbDummy.TabIndex = 2;
            this.pbDummy.TabStop = false;
            this.pbDummy.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(856, 524);
            this.Controls.Add(this.pbDummy);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "OpenJinglePlayer";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.cmsTile.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbDummy)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile;
        private System.Windows.Forms.ToolStripMenuItem tsmiNewFile;
        private System.Windows.Forms.ToolStripMenuItem tsmiOpenFile;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tsmiSaveFile;
        private System.Windows.Forms.ToolStripMenuItem tsmiSaveAs;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem tsmiExit;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbtNewFile;
        private System.Windows.Forms.ToolStripButton tsbtOpenFile;
        private System.Windows.Forms.ToolStripButton tsbtSaveFile;
        private System.Windows.Forms.ToolStripButton tsbtSaveAs;
        private System.Windows.Forms.ToolStripMenuItem tsmiShemes;
        private System.Windows.Forms.ToolStripMenuItem tsmiAdd;
        private System.Windows.Forms.ToolStripMenuItem tsmiEdit;
        private System.Windows.Forms.ToolStripMenuItem tsmiOptions;
        private System.Windows.Forms.ToolStripMenuItem tsmiHelp;
        private System.Windows.Forms.ToolStripMenuItem tsmiAbout;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton tsbtAdd;
        private System.Windows.Forms.ToolStripButton tsbtRemove;
        private System.Windows.Forms.ToolStripComboBox tscbShemes;
        private System.Windows.Forms.ToolStripButton tsbtEdit;
        private System.Windows.Forms.ToolStripMenuItem tsmiRemove;
        private System.Windows.Forms.OpenFileDialog ofdMedia;
        private System.Windows.Forms.SaveFileDialog sfdShemes;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton tsbtVideoScreen;
        private System.Windows.Forms.ContextMenuStrip cmsTile;
        private System.Windows.Forms.ToolStripMenuItem tsmiDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem tsmiNew;
        private System.Windows.Forms.ToolStripMenuItem tsmiReset;
        private System.Windows.Forms.ToolStripMenuItem tsmiChangeName;
        private System.Windows.Forms.ToolStripMenuItem tsmiLoop;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.OpenFileDialog ofdShemes;
        private System.Windows.Forms.ToolStripButton tsbtToggleFullscreen;
        private System.Windows.Forms.PictureBox pbDummy;
        private System.Windows.Forms.ToolStripMenuItem tsmiShowVideoScreen;
        private System.Windows.Forms.ToolStripMenuItem tsmiPauseInsteadStop;

    }
}

