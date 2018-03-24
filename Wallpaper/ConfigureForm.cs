using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Wallpaper
{
    public partial class ConfigureForm : Form
    {
        public ConfigureForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string execPath = Application.ExecutablePath;
                RegistryKey rk = Registry.LocalMachine;
                //RegistryKey rk2 = rk.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run");
                RegistryKey rk2 = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run",
               RegistryKeyPermissionCheck.ReadWriteSubTree, System.Security.AccessControl.RegistryRights.FullControl);
                //RegistryKey localMachine = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                if (radioButton1.Checked)
                {
                    rk2.SetValue("MyExec", execPath);
                    MessageBox.Show("[注册表操作]添加注册表键值：path = {0}, key = {1}, value = {2} 成功");
                    Console.WriteLine(string.Format("[注册表操作]添加注册表键值：path = {0}, key = {1}, value = {2} 成功", rk2.Name, "TuniuAutoboot", execPath));
                }
                else
                {
                    rk2.DeleteValue("MyExec", false);
                    Console.WriteLine(string.Format("[注册表操作]删除注册表键值：path = {0}, key = {1} 成功", rk2.Name, "TuniuAutoboot"));
                }
                rk2.Close();
                rk.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("[注册表操作]向注册表写开机启动信息失败, Exception: {0}", ex.Message));
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }
    }
}
