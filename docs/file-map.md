# File Map

_最終更新: 2026-08-03_

## Core

| ファイル | 役割 | 主な依存先 |
|---|---|---|
| `src/WuTrayToggle/WuTrayToggle.csproj` | プロジェクト定義（net8.0-windows, 自己完結単一ファイル発行設定） | `System.ServiceProcess.ServiceController` (NuGet) |
| `src/WuTrayToggle/Program.cs` | エントリポイント。引数解析（`--install`/`--uninstall`/`--elevated-stop`/`--elevated-start`）、多重起動防止 | `TrayApplicationContext`, `ShortcutManager`, `WindowsUpdateController` |
| `src/WuTrayToggle/TrayApplicationContext.cs` | トレイアイコン常駐・メニュー操作 | `System.Windows.Forms`, `IconFactory`, `WindowsUpdateController` |
| `src/WuTrayToggle/IconFactory.cs` | 埋め込み`.ico`リソース(`Assets/`)からのアイコン読み込み | `System.Drawing` |
| `src/WuTrayToggle/Assets/app.ico` | exeファイルアイコン(盾+歯車、`ApplicationIcon`) | — |
| `src/WuTrayToggle/Assets/tray-running.ico` | トレイアイコン(稼働中、盾+更新矢印) | — |
| `src/WuTrayToggle/Assets/tray-stopped.ico` | トレイアイコン(停止中、盾+赤X) | — |
| `src/WuTrayToggle/WindowsUpdateController.cs` | レジストリ確認・サービス状態確認・WU 停止/再開 | `Microsoft.Win32.Registry`, `System.ServiceProcess` |
| `src/WuTrayToggle/ShortcutManager.cs` | デスクトップショートカット作成・削除、ログイン時自動起動(スタートアップフォルダ)の登録・解除 | `IShellLinkW`/`IPersistFile` (COM interop) |
| `src/WuTrayToggle/TrayState.cs` | トレイ状態(Running/Stopped)の列挙型 | — |
| `src/WuTrayToggle/AppLanguage.cs` | 対応7言語の列挙型 | — |
| `src/WuTrayToggle/Localization.cs` | 表示言語の解決(ユーザー設定＞システム言語＞英語)。`GetUserDefaultUILanguage` をP/Invoke | `AppSettings` |
| `src/WuTrayToggle/AppSettings.cs` | 言語設定の永続化(`%APPDATA%\WuTrayToggle\language.txt`) | — |
| `src/WuTrayToggle/Strings.cs` | UI文字列テーブル(7言語) | `Localization` |
| `archives/icons/*.png` | `Assets/*.ico` の元絵(透過PNG)。差し替え時はここから正方形化・多重解像度化して再生成する | — |
