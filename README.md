# pwsh-wu-tray-toggle

> **This is the reference (English) version.**
> The canonical (Japanese) version is [README-jp.md](README-jp.md).

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
[![CI](https://github.com/y-marui/pwsh-wu-tray-toggle/actions/workflows/ci.yml/badge.svg)](https://github.com/y-marui/pwsh-wu-tray-toggle/actions/workflows/ci.yml)
[![Charter Check](https://github.com/y-marui/pwsh-wu-tray-toggle/actions/workflows/dev-charter-check.yml/badge.svg)](https://github.com/y-marui/pwsh-wu-tray-toggle/actions/workflows/dev-charter-check.yml)
[![GitHub Sponsors](https://img.shields.io/github/sponsors/y-marui?style=social)](https://github.com/sponsors/y-marui)
[![Buy Me a Coffee](https://img.shields.io/badge/Buy%20Me%20a%20Coffee-donate-yellow.svg)](https://www.buymeacoffee.com/y.marui)

A PowerShell system tray app to stop and resume Windows Update from the notification area.

## Setup

Requires PowerShell and administrator privileges.

```powershell
make install
```

Creates a shortcut `WU_TrayIcon.lnk` on the desktop. Double-click it to launch the tray icon.

## Usage

| Command | Description |
|---|---|
| `make install` | Install desktop shortcut |
| `make uninstall` | Remove desktop shortcut |

Right-click the tray icon to access the menu:

- **Check current status** — Shows registry policy and service state
- **Stop (disable auto-update)** — Stops Windows Update via group policy
- **Resume (normal)** — Re-enables Windows Update

## License

[MIT](LICENSE)

---
*This document has a Japanese canonical version [README-jp.md](README-jp.md). Update both in the same commit when editing.*
