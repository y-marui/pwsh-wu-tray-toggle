# Changelog

All notable changes to this project will be documented here.

## [Unreleased]

### Changed
- Rewrote the tray app from a PowerShell script (`src/tray.ps1`/`src/install.ps1`) to a self-contained C# (.NET 8, WinForms) single-file exe (`src/WuTrayToggle/`), to fix instability in the `powershell.exe`-based launch path (console flash, execution-policy/AMSI delay, no multi-instance guard). The desktop shortcut now launches the exe directly, and the exe installs/uninstalls its own shortcut via `--install`/`--uninstall`.
