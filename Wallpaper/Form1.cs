using System;
using System.Windows.Forms;

namespace Wallpaper
{
    public partial class Form1 : Form
{
    // 指向 Program Manager 窗口句柄
    private IntPtr programIntPtr = IntPtr.Zero;

    // 桌面背景窗口
    private BgForm bgForm = null;

    public Form1()
    {
        InitializeComponent();

        // 设置循环播放
        axWindowsMediaPlayer1.settings.setMode("loop", true);
    }

    private void Form1_Load(object sender, EventArgs e)
    {
            // 把打开的视频路径，给播放器。
            axWindowsMediaPlayer1.URL = @"D:\MyVedio\123.mp4";

            // 播放视频。
            axWindowsMediaPlayer1.Ctlcontrols.play();
            button1_Click(null, null);
        }

    public void Init()
    {
        // 通过类名查找一个窗口，返回窗口句柄。
        programIntPtr = Win32.FindWindow("Progman", null);

        // 窗口句柄有效
        if(programIntPtr != IntPtr.Zero)
        {   

            IntPtr result = IntPtr.Zero;

            // 向 Program Manager 窗口发送 0x52c 的一个消息，超时设置为0x3e8（1秒）。
            Win32.SendMessageTimeout(programIntPtr, 0x52c, IntPtr.Zero, IntPtr.Zero, 0, 0x3e8, result);

            // 遍历顶级窗口
            Win32.EnumWindows((hwnd, lParam) =>
            {
                // 找到包含 SHELLDLL_DefView 这个窗口句柄的 WorkerW
                if (Win32.FindWindowEx(hwnd,IntPtr.Zero, "SHELLDLL_DefView", null) != IntPtr.Zero)
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

    // 打开视频按钮 的事件
    private void button2_Click(object sender, EventArgs e)
    {
        // 创建对话框
        OpenFileDialog dialog = new OpenFileDialog();
        // 设置过滤器，只允许 .wmv 和 mp4 格式的视频。
        dialog.Filter = "视频(*.wmv;*.mp4)|*.wmv;*.mp4";

        DialogResult result = dialog.ShowDialog();

        if(result == DialogResult.OK)
        {
            // 把打开的视频路径，给播放器。
            axWindowsMediaPlayer1.URL = dialog.FileName;

            // 播放视频。
            axWindowsMediaPlayer1.Ctlcontrols.play();
        }
    }

    // 设置壁纸 按钮事件
    private void button1_Click(object sender, EventArgs e)
    {
        if(bgForm == null)
        {
            // 创建背景窗口
            bgForm = new BgForm();

            // 初始化桌面窗口
            Init();
                
            // 窗口置父，设置背景窗口的父窗口为 Program Manager 窗口
            Win32.SetParent(bgForm.Handle, programIntPtr);

            // 显示背景窗口
            bgForm.Show();
        }

        // 预览窗口视频暂停播放
        axWindowsMediaPlayer1.Ctlcontrols.pause();

        // 背景窗口视频播放
        bgForm.Play(axWindowsMediaPlayer1);
    }

    // 播放器的状态发生改变，为了解决无缝视频循环。
    private void axWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
    {
        // 播放结束
        if(axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsMediaEnded)
        {
            // 无黑屏循环播放视频
            axWindowsMediaPlayer1.Ctlcontrols.currentPosition = 0;
        }
    }
}
}
