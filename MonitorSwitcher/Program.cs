using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;
using System.IO;

namespace MonitorSwitcher
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var appIsOpen = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(
               Assembly.GetEntryAssembly().Location)).Count() > 1;

            if (!appIsOpen)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                MainForm form1 = new MainForm();
                Application.Run(form1);
            }
        }
    }
}
