# File Map

_最終更新: 2026-08-03_

## Core

| ファイル | 役割 | 主な依存先 |
|---|---|---|
| `src/WuTrayToggle/WuTrayToggle.csproj` | プロジェクト定義（net8.0-windows, 自己完結単一ファイル発行設定） | `System.ServiceProcess.ServiceController` (NuGet) |
| `src/WuTrayToggle/Program.cs` | エントリポイント。引数解析（`--install`/`--uninstall`/`--elevated-stop`/`--elevated-start`）、多重起動防止 | `TrayApplicationContext`, `ShortcutManager`, `WindowsUpdateController` |
| `src/WuTrayToggle/TrayApplicationContext.cs` | トレイアイコン常駐・メニュー操作 | `System.Windows.Forms`, `IconFactory`, `WindowsUpdateController` |
| `src/WuTrayToggle/IconFactory.cs` | 64×64 アイコンのプログラム生成 | `System.Drawing` |
| `src/WuTrayToggle/WindowsUpdateController.cs` | レジストリ確認・サービス状態確認・WU 停止/再開 | `Microsoft.Win32.Registry`, `System.ServiceProcess` |
| `src/WuTrayToggle/ShortcutManager.cs` | デスクトップショートカット作成・削除 | `IShellLinkW`/`IPersistFile` (COM interop) |
| `src/WuTrayToggle/TrayState.cs` | トレイ状態(Running/Stopped)の列挙型 | — |
