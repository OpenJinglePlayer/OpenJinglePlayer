using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenJinglePlayer
{
    public partial class fmEditName : Form
    {
        public string OldFileName = String.Empty;
        public string NewFileName = String.Empty;

        public fmEditName()
        {
            InitializeComponent();
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            NewFileName = tbNewName.Text;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            tbOldName.Text = OldFileName;
            tbNewName.Text = NewFileName;

            tbNewName.Focus();
        }
    }
}
