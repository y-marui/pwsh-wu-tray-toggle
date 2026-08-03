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
        var startupItem = (ToolStripMenuItem)menu.Items.Add("ログイン時に自動起動");
        menu.Items.Add(new ToolStripSeparator());
        var stopItem = menu.Items.Add("停止 (制御開始)");
        var startItem = menu.Items.Add("再開 (通常)");
        menu.Items.Add(new ToolStripSeparator());
        var exitItem = menu.Items.Add("終了");

        startupItem.Checked = ShortcutManager.IsStartupEnabled();

        checkItem.Click += (_, _) => ShowStatus();
        startupItem.Click += (_, _) => ToggleStartup(startupItem);
        stopItem.Click += (_, _) => RunElevatedAction("--elevated-stop", TrayState.Stopped);
        startItem.Click += (_, _) => RunElevatedAction("--elevated-start", TrayState.Running);
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

    private static void ToggleStartup(ToolStripMenuItem item)
    {
        if (ShortcutManager.IsStartupEnabled())
        {
            ShortcutManager.DisableStartup();
        }
        else
        {
            ShortcutManager.EnableStartup();
        }

        item.Checked = ShortcutManager.IsStartupEnabled();
    }

    private void RunElevatedAction(string argument, TrayState expectedState)
    {
        var exePath = Environment.ProcessPath;
        if (exePath is null)
        {
            return;
        }

        var cancelled = false;
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
            // UACでキャンセルされた場合 (ERROR_CANCELLED)
            cancelled = true;
        }

        RefreshStatus();

        if (cancelled)
        {
            ShowBalloon("操作をキャンセルしました", ToolTipIcon.Warning);
        }
        else if (WindowsUpdateController.GetState() == expectedState)
        {
            var message = expectedState == TrayState.Stopped
                ? "Windows Update を停止しました"
                : "Windows Update を再開しました";
            ShowBalloon(message, ToolTipIcon.Info);
        }
        else
        {
            ShowBalloon("操作に失敗しました", ToolTipIcon.Error);
        }
    }

    private void ShowBalloon(string text, ToolTipIcon icon)
    {
        _notifyIcon.BalloonTipTitle = "WU トレイ";
        _notifyIcon.BalloonTipText = text;
        _notifyIcon.BalloonTipIcon = icon;
        _notifyIcon.ShowBalloonTip(3000);
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
