using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Wallpaper
{
    public partial class BgForm : Form
    {

        // 指向 Program Manager 窗口句柄
        private IntPtr programIntPtr = IntPtr.Zero;
        private static Image image = null;

        static BgForm()
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Runtime)
            {
                image = Image.FromFile(Application.StartupPath + @"\Resources\background.bmp");
            }
        }

        public BgForm()
        {
            InitializeComponent();
            this.BackgroundImage = image;
            // 隐藏播放器的ui
            axWindowsMediaPlayer1.uiMode = "none";

            // 最大化窗口（全屏）
            WindowState = FormWindowState.Maximized;

            // 如果最大化窗口，屏幕边缘出现缝隙。改用如下代码进行全屏：
            // this.Bounds = Screen.PrimaryScreen.Bounds;

            // 设置循环播放
            axWindowsMediaPlayer1.settings.setMode("loop", true);
            axWindowsMediaPlayer1.settings.mute = true;
            axWindowsMediaPlayer1.settings.volume = 0;
        }

        // 播放方法，在Form1中有调用。
        public void Play(AxWMPLib.AxWindowsMediaPlayer mediaPlayer)
        {
            // 使用Form1预览窗口中 url、音量。
            axWindowsMediaPlayer1.URL = mediaPlayer.URL;
            //axWindowsMediaPlayer1.settings.volume = mediaPlayer.settings.volume;
            axWindowsMediaPlayer1.settings.mute = true;
            axWindowsMediaPlayer1.settings.volume = 0;
            // 背景窗口播放器，播放视频。
            axWindowsMediaPlayer1.Ctlcontrols.play();
        }

        // 跟Form1预览窗口，中一样，解决无缝视频播放。
        private void axWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsMediaEnded)
            {
                axWindowsMediaPlayer1.Ctlcontrols.currentPosition = 0;
            }

        }


        private void BgForm_Load(object sender, EventArgs e)
        {

            Form2 form = new Form2();
            form.Location = new Point((Screen.PrimaryScreen.Bounds.Width - form.Width ) / 2, 0);
            form.OnStopListenerEvent += Form_OnStopListenerEvent;
            form.OnStateChangeListenerEvent += Form_OnStateChangeListenerEvent;
            form.Show();
            // 初始化桌面窗口
            Init();

            // 窗口置父，设置背景窗口的父窗口为 Program Manager 窗口
            Win32.SetParent(this.Handle, programIntPtr);
            // 把打开的视频路径，给播放器。
            axWindowsMediaPlayer1.URL = Application.StartupPath + @"\Resources\play.mp4";

            // 播放视频。
            axWindowsMediaPlayer1.Ctlcontrols.play();
        }

        private void Form_OnStateChangeListenerEvent()
        {
            if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPaused || axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsStopped)
            {
                axWindowsMediaPlayer1.Visible = true;
                axWindowsMediaPlayer1.Ctlcontrols.play();
            }
            else
            {
                axWindowsMediaPlayer1.Ctlcontrols.pause();
            }
        }

        private void Form_OnStopListenerEvent()
        {
            axWindowsMediaPlayer1.Ctlcontrols.stop();
            axWindowsMediaPlayer1.Visible = false;
        }

        public void Init()
        {
            // 通过类名查找一个窗口，返回窗口句柄。
            programIntPtr = Win32.FindWindow("Progman", null);

            // 窗口句柄有效
            if (programIntPtr != IntPtr.Zero)
            {

                IntPtr result = IntPtr.Zero;

                // 向 Program Manager 窗口发送 0x52c 的一个消息，超时设置为0x3e8（1秒）。
                Win32.SendMessageTimeout(programIntPtr, 0x52c, IntPtr.Zero, IntPtr.Zero, 0, 0x3e8, result);

                // 遍历顶级窗口
                Win32.EnumWindows((hwnd, lParam) =>
                {
                    // 找到包含 SHELLDLL_DefView 这个窗口句柄的 WorkerW
                    if (Win32.FindWindowEx(hwnd, IntPtr.Zero, "SHELLDLL_DefView", null) != IntPtr.Zero)
                    {
                        // 找到当前 WorkerW 窗口的，后一个 WorkerW 窗口。 
                        IntPtr tempHwnd = Win32.FindWindowEx(IntPtr.Zero, hwnd, "WorkerW", null);

                        // 隐藏这个窗口
                        Win32.ShowWindow(tempHwnd, 0);
                    }
                    return true;
                }, IntPtr.Zero);
            }
        }

        private void axWindowsMediaPlayer1_DoubleClickEvent(object sender, AxWMPLib._WMPOCXEvents_DoubleClickEvent e)
        {

        }
    }
}
