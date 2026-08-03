using System.ComponentModel;
using System.Diagnostics;

namespace WuTrayToggle;

internal sealed class TrayApplicationContext : ApplicationContext
{
    private static readonly (AppLanguage? Language, Func<string> Name)[] LanguageOptions =
    {
        (null, () => Strings.MenuLanguageSystem),
        (AppLanguage.Japanese, () => Strings.LanguageNameJapanese),
        (AppLanguage.English, () => Strings.LanguageNameEnglish),
        (AppLanguage.Chinese, () => Strings.LanguageNameChinese),
        (AppLanguage.Hindi, () => Strings.LanguageNameHindi),
        (AppLanguage.Spanish, () => Strings.LanguageNameSpanish),
        (AppLanguage.French, () => Strings.LanguageNameFrench),
        (AppLanguage.Portuguese, () => Strings.LanguageNamePortuguese),
    };

    private readonly NotifyIcon _notifyIcon;
    private readonly ToolStripItem _checkItem;
    private readonly ToolStripMenuItem _startupItem;
    private readonly ToolStripMenuItem _languageMenu;
    private readonly ToolStripMenuItem[] _languageItems;
    private readonly ToolStripItem _stopItem;
    private readonly ToolStripItem _startItem;
    private readonly ToolStripItem _exitItem;

    public TrayApplicationContext()
    {
        var menu = new ContextMenuStrip();
        _checkItem = menu.Items.Add(string.Empty);
        menu.Items.Add(new ToolStripSeparator());
        _startupItem = (ToolStripMenuItem)menu.Items.Add(string.Empty);

        _languageMenu = new ToolStripMenuItem();
        _languageItems = new ToolStripMenuItem[LanguageOptions.Length];
        for (var i = 0; i < LanguageOptions.Length; i++)
        {
            var language = LanguageOptions[i].Language;
            var item = new ToolStripMenuItem();
            item.Click += (_, _) => ChangeLanguage(language);
            _languageMenu.DropDownItems.Add(item);
            _languageItems[i] = item;
        }

        menu.Items.Add(_languageMenu);
        menu.Items.Add(new ToolStripSeparator());
        _stopItem = menu.Items.Add(string.Empty);
        _startItem = menu.Items.Add(string.Empty);
        menu.Items.Add(new ToolStripSeparator());
        _exitItem = menu.Items.Add(string.Empty);

        _checkItem.Click += (_, _) => ShowStatus();
        _startupItem.Click += (_, _) => ToggleStartup();
        _stopItem.Click += (_, _) => RunElevatedAction("--elevated-stop", TrayState.Stopped);
        _startItem.Click += (_, _) => RunElevatedAction("--elevated-start", TrayState.Running);
        _exitItem.Click += (_, _) => ExitThread();

        menu.Opening += (_, _) => RefreshMenuText();

        _notifyIcon = new NotifyIcon
        {
            ContextMenuStrip = menu,
            Visible = true,
        };

        RefreshMenuText();
        RefreshStatus();
    }

    private void RefreshMenuText()
    {
        _checkItem.Text = Strings.MenuCheckStatus;

        _startupItem.Text = Strings.MenuStartup;
        _startupItem.Checked = ShortcutManager.IsStartupEnabled();

        _languageMenu.Text = Strings.MenuLanguage;
        var currentOverride = Localization.UserOverride;
        for (var i = 0; i < LanguageOptions.Length; i++)
        {
            _languageItems[i].Text = LanguageOptions[i].Name();
            _languageItems[i].Checked = LanguageOptions[i].Language == currentOverride;
        }

        _stopItem.Text = Strings.MenuStop;
        _startItem.Text = Strings.MenuStart;
        _exitItem.Text = Strings.MenuExit;
    }

    private void ChangeLanguage(AppLanguage? language)
    {
        if (!Localization.SetOverride(language))
        {
            ShowBalloon(Strings.BalloonLanguageSaveFailed, ToolTipIcon.Error);
            return;
        }

        RefreshMenuText();
        RefreshStatus();
    }

    private static void ShowStatus()
    {
        MessageBox.Show(WindowsUpdateController.GetStatusReport(), Strings.StatusTitle);
    }

    private void ToggleStartup()
    {
        if (ShortcutManager.IsStartupEnabled())
        {
            ShortcutManager.DisableStartup();
        }
        else
        {
            ShortcutManager.EnableStartup();
        }

        _startupItem.Checked = ShortcutManager.IsStartupEnabled();
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
            ShowBalloon(Strings.BalloonCancelled, ToolTipIcon.Warning);
        }
        else if (WindowsUpdateController.GetState() == expectedState)
        {
            var message = expectedState == TrayState.Stopped
                ? Strings.BalloonStopped
                : Strings.BalloonResumed;
            ShowBalloon(message, ToolTipIcon.Info);
        }
        else
        {
            ShowBalloon(Strings.BalloonFailed, ToolTipIcon.Error);
        }
    }

    private void ShowBalloon(string text, ToolTipIcon icon)
    {
        _notifyIcon.BalloonTipTitle = Strings.TrayTitle;
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
