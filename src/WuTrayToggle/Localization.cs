using System.Runtime.InteropServices;

namespace WuTrayToggle;

internal static partial class Localization
{
    public static AppLanguage Current { get; private set; } = Resolve();

    public static bool IsJapanese => Current == AppLanguage.Japanese;

    public static AppLanguage? UserOverride => AppSettings.GetLanguageOverride() switch
    {
        "ja" => AppLanguage.Japanese,
        "en" => AppLanguage.English,
        "zh" => AppLanguage.Chinese,
        "hi" => AppLanguage.Hindi,
        "es" => AppLanguage.Spanish,
        "fr" => AppLanguage.French,
        "pt" => AppLanguage.Portuguese,
        _ => null,
    };

    public static void SetOverride(AppLanguage? language)
    {
        AppSettings.SetLanguageOverride(language switch
        {
            AppLanguage.Japanese => "ja",
            AppLanguage.English => "en",
            AppLanguage.Chinese => "zh",
            AppLanguage.Hindi => "hi",
            AppLanguage.Spanish => "es",
            AppLanguage.French => "fr",
            AppLanguage.Portuguese => "pt",
            _ => null,
        });
        Current = Resolve();
    }

    private static AppLanguage Resolve()
    {
        return UserOverride ?? DetectSystemLanguage();
    }

    private static AppLanguage DetectSystemLanguage()
    {
        // Primary language IDs (low 10 bits of a Windows LANGID, from winnt.h).
        var primaryLangId = (ushort)(GetUserDefaultUILanguage() & 0x3FF);
        return primaryLangId switch
        {
            0x11 => AppLanguage.Japanese, // LANG_JAPANESE
            0x04 => AppLanguage.Chinese, // LANG_CHINESE
            0x39 => AppLanguage.Hindi, // LANG_HINDI
            0x0a => AppLanguage.Spanish, // LANG_SPANISH
            0x0c => AppLanguage.French, // LANG_FRENCH
            0x16 => AppLanguage.Portuguese, // LANG_PORTUGUESE
            _ => AppLanguage.English, // includes LANG_ENGLISH and any unsupported language
        };
    }

    // Read directly via Win32 instead of CultureInfo: the project builds with
    // <InvariantGlobalization>true</InvariantGlobalization>, under which CultureInfo
    // no longer reflects the OS UI language.
    [LibraryImport("kernel32.dll")]
    private static partial ushort GetUserDefaultUILanguage();
}
