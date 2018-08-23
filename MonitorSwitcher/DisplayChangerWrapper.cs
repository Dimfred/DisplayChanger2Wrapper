using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.IO;

namespace MonitorSwitcher
{
    public class DisplayChangerWrapper
    {
        private static readonly string configFolder = "ConfigFiles\\";
        ProcessStartInfo dcProc = new ProcessStartInfo()
        {
            CreateNoWindow = true,
            UseShellExecute = false,
            FileName = "Utilities\\dc2.exe",
            WindowStyle = ProcessWindowStyle.Hidden
        };

        public DisplayChangerWrapper()
        {

        }

        public void DeleteConfig(string name)
        {
            File.Delete(string.Concat(configFolder, name, ".xml"));
        }

        public void CreateNewConfig(string name)
        {
            dcProc.Arguments = $"-create=\"{configFolder}{name}.xml\"";
            Invoke();
        }

        public void SetConfig(string name)
        {
            dcProc.Arguments = $"-force -configure=\"{configFolder}{name}.xml\"";
            Invoke();
        }

        private void Invoke()
        {
            try
            {
                using (Process proc = Process.Start(dcProc))
                {
                    proc.WaitForExit();
                    Thread.Sleep(800);
                }
            }
            catch (Exception)
            {
            }
        }

    }
}
