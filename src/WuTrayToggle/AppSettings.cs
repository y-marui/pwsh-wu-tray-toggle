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
        catch (Exception ex) when (IsFileSystemException(ex))
        {
            return null;
        }
    }

    public static bool SetLanguageOverride(string? code)
    {
        try
        {
            if (code is null)
            {
                if (File.Exists(FilePath))
                {
                    File.Delete(FilePath);
                }

                return true;
            }

            var directory = Path.GetDirectoryName(FilePath);
            if (directory is not null)
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(FilePath, code);
            return true;
        }
        catch (Exception ex) when (IsFileSystemException(ex))
        {
            return false;
        }
    }

    private static bool IsFileSystemException(Exception exception)
    {
        return exception is IOException
            or UnauthorizedAccessException
            or System.Security.SecurityException;
    }
}
