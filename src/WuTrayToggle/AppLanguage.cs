namespace WuTrayToggle;

// Matches docs/dev-charter/LOCALIZATION_POLICY.md's supported-language list (minus "System",
// which is represented as a null Localization.UserOverride rather than an AppLanguage value).
internal enum AppLanguage
{
    Japanese,
    English,
    Chinese,
    Hindi,
    Spanish,
    French,
    Portuguese,
}
