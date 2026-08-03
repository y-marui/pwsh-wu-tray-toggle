# pwsh-wu-tray-toggle

> **This is the reference (English) version.**
> The canonical (Japanese) version is [README-jp.md](README-jp.md).

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
[![CI](https://github.com/y-marui/pwsh-wu-tray-toggle/actions/workflows/ci.yml/badge.svg)](https://github.com/y-marui/pwsh-wu-tray-toggle/actions/workflows/ci.yml)
[![Charter Check](https://github.com/y-marui/pwsh-wu-tray-toggle/actions/workflows/dev-charter-check.yml/badge.svg)](https://github.com/y-marui/pwsh-wu-tray-toggle/actions/workflows/dev-charter-check.yml)

A C# system tray app to stop and resume Windows Update from the notification area.

## Setup

Requires Windows and administrator privileges (building requires the .NET 8 SDK).

```powershell
make install
```

Builds a self-contained single-file exe and creates a shortcut `WU_TrayIcon.lnk` on the desktop. Double-click it to launch the tray icon.

## Usage

| Command | Description |
|---|---|
| `make install` | Install desktop shortcut |
| `make uninstall` | Remove desktop shortcut |

Right-click the tray icon to access the menu:

- **Check current status** — Shows the app version, registry policy, and service state
- **Stop (disable auto-update)** — Stops Windows Update via group policy
- **Resume (normal)** — Re-enables Windows Update

## License

[MIT](LICENSE)

---
*This document has a Japanese canonical version [README-jp.md](README-jp.md). Update both in the same commit when editing.*
