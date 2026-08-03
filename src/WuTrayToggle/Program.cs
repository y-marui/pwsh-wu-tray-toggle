using System.Threading;

namespace WuTrayToggle;

internal static class Program
{
    private const string MutexName = "Global\\WuTrayToggle.SingleInstance";

    [STAThread]
    private static void Main(string[] args)
    {
        if (args.Length > 0)
        {
            switch (args[0])
            {
                case "--install":
                    ShortcutManager.Install();
                    return;
                case "--uninstall":
                    ShortcutManager.Uninstall();
                    return;
                case "--elevated-stop":
                    WindowsUpdateController.Stop();
                    return;
                case "--elevated-start":
                    WindowsUpdateController.Start();
                    return;
            }
        }

        using var mutex = new Mutex(initiallyOwned: true, MutexName, out var createdNew);
        if (!createdNew)
        {
            MessageBox.Show(Strings.AlreadyRunning, Strings.TrayTitle);
            return;
        }

        Application.SetHighDpiMode(HighDpiMode.SystemAware);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new TrayApplicationContext());
    }
}
