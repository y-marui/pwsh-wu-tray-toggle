# Changelog

All notable changes to this project will be documented here.

## [Unreleased]

## [1.0.0] - 2026-08-03

### Added
- Tray menu now has a checkable "ログイン時に自動起動" item that toggles a shortcut in the current user's Startup folder, so the app can launch automatically at login without a persistent registry install.
- Balloon-tip feedback for stop/resume actions (success/failure/UAC-cancelled), and static shield-themed icons (`src/WuTrayToggle/Assets/`) replacing the runtime-drawn shapes for the exe file icon and the two tray states.
- Localized all UI text (menu, tray tooltip, message boxes, balloon tips) into the 7 languages listed in `docs/dev-charter/LOCALIZATION_POLICY.md` (Japanese/English/Chinese/Hindi/Spanish/French/Portuguese), with a tray "Language" submenu (system default / per-language override, priority: user setting > system UI language > English). Closes #3.

### Changed
- Rewrote the tray app from a PowerShell script (`src/tray.ps1`/`src/install.ps1`) to a self-contained C# (.NET 8, WinForms) single-file exe (`src/WuTrayToggle/`), to fix instability in the `powershell.exe`-based launch path (console flash, execution-policy/AMSI delay, no multi-instance guard). The desktop shortcut now launches the exe directly, and the exe installs/uninstalls its own shortcut via `--install`/`--uninstall`.
- Embedded version 1.0.0 in the executable and added it to the status dialog.
- Language-setting file errors no longer terminate the tray application.
