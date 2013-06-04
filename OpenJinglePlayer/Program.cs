using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace OpenJinglePlayer
{
    static class Program
    {
        public static bool PauseInsteadOfStop = true;
        public static Status Status;

        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Status = new Status();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }

    class Status
    {
        public const string ProgramNameVersionString = "OpenJinglePlayer v2.0";
        public bool VideoScreenVisible = false;
    }
}
