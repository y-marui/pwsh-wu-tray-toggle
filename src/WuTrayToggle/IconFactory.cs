using System.Reflection;

namespace WuTrayToggle;

internal static class IconFactory
{
    public static Icon Create(TrayState state)
    {
        var resourceName = state == TrayState.Running
            ? "WuTrayToggle.Assets.tray-running.ico"
            : "WuTrayToggle.Assets.tray-stopped.ico";

        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException($"Embedded resource not found: {resourceName}");

        return new Icon(stream);
    }
}
