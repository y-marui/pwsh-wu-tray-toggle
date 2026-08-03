namespace WuTrayToggle;

internal static class AppSettings
{
    private static readonly string FilePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "WuTrayToggle",
        "language.txt");

    public static string? GetLanguageOverride()
    {
        try
        {
            return File.Exists(FilePath) ? File.ReadAllText(FilePath).Trim() : null;
        }
        catch (IOException)
        {
            return null;
        }
    }

    public static void SetLanguageOverride(string? code)
    {
        var directory = Path.GetDirectoryName(FilePath);
        if (directory is not null)
        {
            Directory.CreateDirectory(directory);
        }

        if (code is null)
        {
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }

            return;
        }

        File.WriteAllText(FilePath, code);
    }
}
