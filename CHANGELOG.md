# Changelog

All notable changes to this project will be documented here.

## [Unreleased]

### Added
- Tray menu now has a checkable "ログイン時に自動起動" item that toggles a shortcut in the current user's Startup folder, so the app can launch automatically at login without a persistent registry install.
- Balloon-tip feedback for stop/resume actions (success/failure/UAC-cancelled), and static shield-themed icons (`src/WuTrayToggle/Assets/`) replacing the runtime-drawn shapes for the exe file icon and the two tray states.

### Changed
- Rewrote the tray app from a PowerShell script (`src/tray.ps1`/`src/install.ps1`) to a self-contained C# (.NET 8, WinForms) single-file exe (`src/WuTrayToggle/`), to fix instability in the `powershell.exe`-based launch path (console flash, execution-policy/AMSI delay, no multi-instance guard). The desktop shortcut now launches the exe directly, and the exe installs/uninstalls its own shortcut via `--install`/`--uninstall`.
