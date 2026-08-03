using System.ComponentModel;
using System.Diagnostics;

namespace WuTrayToggle;

internal sealed class TrayApplicationContext : ApplicationContext
{
    private readonly NotifyIcon _notifyIcon;

    public TrayApplicationContext()
    {
        var menu = new ContextMenuStrip();
        var checkItem = menu.Items.Add("現在の状態を確認");
        menu.Items.Add(new ToolStripSeparator());
        var stopItem = menu.Items.Add("停止 (制御開始)");
        var startItem = menu.Items.Add("再開 (通常)");
        menu.Items.Add(new ToolStripSeparator());
        var exitItem = menu.Items.Add("終了");

        checkItem.Click += (_, _) => ShowStatus();
        stopItem.Click += (_, _) => RunElevatedAction("--elevated-stop");
        startItem.Click += (_, _) => RunElevatedAction("--elevated-start");
        exitItem.Click += (_, _) => ExitThread();

        _notifyIcon = new NotifyIcon
        {
            ContextMenuStrip = menu,
            Visible = true,
        };

        RefreshStatus();
    }

    private static void ShowStatus()
    {
        MessageBox.Show(WindowsUpdateController.GetStatusReport(), "WU 状態確認");
    }

    private void RunElevatedAction(string argument)
    {
        var exePath = Environment.ProcessPath;
        if (exePath is null)
        {
            return;
        }

        try
        {
            using var process = Process.Start(new ProcessStartInfo
            {
                FileName = exePath,
                Arguments = argument,
                UseShellExecute = true,
                Verb = "runas",
            });
            process?.WaitForExit();
        }
        catch (Win32Exception)
        {
            // UACでキャンセルされた場合 (ERROR_CANCELLED) は何もしない
        }

        RefreshStatus();
    }

    private void RefreshStatus()
    {
        var oldIcon = _notifyIcon.Icon;
        _notifyIcon.Icon = IconFactory.Create(WindowsUpdateController.GetState());
        _notifyIcon.Text = WindowsUpdateController.GetTrayText();
        oldIcon?.Dispose();
    }

    protected override void ExitThreadCore()
    {
        _notifyIcon.Visible = false;
        _notifyIcon.Dispose();
        base.ExitThreadCore();
    }
}
