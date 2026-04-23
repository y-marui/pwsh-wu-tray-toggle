# Developing

## Requirements

- Windows
- PowerShell 5.1 or later
- Administrator privileges (for registry and service access)

## Setup

```powershell
make install
```

Creates `WU_TrayIcon.lnk` on the desktop. Double-click to launch.

## Running Directly

```powershell
powershell -NoProfile -STA -WindowStyle Hidden -ExecutionPolicy Bypass -File src/tray.ps1
```

## Lint

```powershell
Install-Module -Name PSScriptAnalyzer -Force -Scope CurrentUser
Invoke-ScriptAnalyzer -Path src/ -Recurse -Severity Error,Warning
```

## Conventions

- **Naming:** PascalCase for functions (`Get-MyIcon`), camelCase for local variables
- **Comments:** Explain *why*, not *what* — see `docs/dev-charter/CODE_STYLE.md`
- **Commit messages:** Conventional Commits format (`feat:`, `fix:`, `docs:`, `chore:`)
- **Branching:** One branch per feature/fix; merge to `main` via PR

## Architecture

See [docs/architecture.md](docs/architecture.md).
