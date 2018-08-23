using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Reflection;
using Microsoft.VisualBasic;
using System.IO;

namespace MonitorSwitcher
{
    public partial class MainForm : Form
    {
        ConfigHandler configHandler = new ConfigHandler();
        ContextMenuStrip contextMenuStrip;
        //List<Button> contextMenuButtons = new List<Button>();
        bool shutdownActivated = false;

        Process shutdownProc;

        ProcessStartInfo procInfo = new ProcessStartInfo()
        {
            CreateNoWindow = true,
            UseShellExecute = false,
            FileName = "shutdown",
            WindowStyle = ProcessWindowStyle.Hidden,
        };

        public MainForm()
        {
            InitializeComponent();
            SetContextMenuStrip();
            notifyIcon.Visible = true;

            procInfo.Arguments = "/a";
            shutdownProc =  Process.Start(procInfo);
        }
       
        private void SetContextMenuStrip()
        {
            contextMenuStrip = new ContextMenuStrip();

            var toolStrip = new ToolStripMenuItem()
            {
                Text = "Add config"
            };
            toolStrip.MouseDown += new MouseEventHandler(OnClick_Add);
            contextMenuStrip.Items.Add(toolStrip);

            toolStrip = new ToolStripMenuItem()
            {
                Text = "Shutdown 60"
            };
            toolStrip.MouseDown += new MouseEventHandler(OnClick_Shutdown);
            contextMenuStrip.Items.Add(toolStrip);

            toolStrip = new ToolStripMenuItem()
            {
                Text = "Shutdown 30"
            };
            toolStrip.MouseDown += new MouseEventHandler(OnClick_Shutdown);
            contextMenuStrip.Items.Add(toolStrip);

            foreach (var configName in configHandler.GetConfigElements())
            {
                toolStrip = new ToolStripMenuItem()
                {
                    Name = configName,
                    Text = configName
                };
                toolStrip.MouseDown += new MouseEventHandler(OnClick_ConfigItem);
                contextMenuStrip.Items.Add(toolStrip);
            }
           
            notifyIcon.ContextMenuStrip = contextMenuStrip;
        }

        private void OnClick_Shutdown(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                if (!shutdownActivated)
                {
                    string toolItemText = ((ToolStripMenuItem)sender).Text;
                    toolItemText = toolItemText.Substring(toolItemText.Length - 2);
                    ushort utime = Convert.ToUInt16(toolItemText);
                    utime *= 60;

                    procInfo.Arguments = $"/s /t {utime}";
                    shutdownActivated = !shutdownActivated;
                }
                else
                {
                    procInfo.Arguments = "/a";
                    shutdownActivated = !shutdownActivated;
                }
                try
                {
                    shutdownProc = Process.Start(procInfo);
                }
                catch { }
            }
        }

        private void OnClick_Add(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var dcw = new DisplayChangerWrapper();

                string newConfigName = Interaction.InputBox("Enter a name");
                if (!string.IsNullOrWhiteSpace(newConfigName))
                {
                    Console.WriteLine(newConfigName);
                    dcw.CreateNewConfig(newConfigName);
                    configHandler.AddConfigElement(newConfigName);
                    SetContextMenuStrip();
                }
            }
        }

        private void OnClick_ConfigItem(object sender, MouseEventArgs e)
        {
            var dcw = new DisplayChangerWrapper();
            notifyIcon.ContextMenuStrip.Close();
            var clickedToolStripItem = (ToolStripItem)sender;
            var configName = clickedToolStripItem.Text;
            // Sets the config
            if (e.Button == MouseButtons.Left)
            {
                dcw.SetConfig(configName);
            }
            // deletes the config
            else if(e.Button == MouseButtons.Right)
            {
                dcw.DeleteConfig(configName);
                configHandler.DeleteConfig(configName);
                SetContextMenuStrip();
            }
        }

        private void OnClick_NotifyIcon(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                notifyIcon.ContextMenuStrip.Hide();
            }
            else if (e.Button == MouseButtons.Left)
            {
                UnsafeNativeMethods.SetForegroundWindow(
                    new System.Runtime.InteropServices.HandleRef(
                        notifyIcon.ContextMenuStrip,
                        notifyIcon.ContextMenuStrip.Handle));
                notifyIcon.ContextMenuStrip.Show(MousePosition);
            }
        }
        
        private void MainForm_Closing(object sender, CancelEventArgs e)
        {
            notifyIcon.Visible = false;
        }
    }
    
}
