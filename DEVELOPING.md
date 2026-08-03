# Developing

## Requirements

- Windows
- .NET 8 SDK
- Administrator privileges (for registry and service access)

## Setup

```powershell
make install
```

Builds a self-contained single-file exe and creates `WU_TrayIcon.lnk` on the desktop. Double-click to launch.

## Running Directly

```powershell
dotnet run --project src/WuTrayToggle
```

## Lint

```powershell
dotnet format src/WuTrayToggle/WuTrayToggle.csproj --verify-no-changes
```

## Conventions

- **Naming:** PascalCase for classes/methods (`WindowsUpdateController`, `GetState`), camelCase for local variables/fields
- **Comments:** Explain *why*, not *what* — see `docs/dev-charter/CODE_STYLE.md`
- **Commit messages:** Conventional Commits format (`feat:`, `fix:`, `docs:`, `chore:`)
- **Branching:** One branch per feature/fix; merge to `main` via PR

## Architecture

See [docs/architecture.md](docs/architecture.md).
