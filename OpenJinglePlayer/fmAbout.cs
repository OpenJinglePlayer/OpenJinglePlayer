﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenJinglePlayer
{
    public partial class fmAbout : Form
    {
        public fmAbout()
        {
            InitializeComponent();

            label1.Text = Status.ProgramNameVersionString;
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
