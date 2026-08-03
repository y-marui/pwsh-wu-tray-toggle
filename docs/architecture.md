# Architecture

## Overview

C# (.NET 8, WinForms) 製システムトレイアプリ。`NotifyIcon` でトレイアイコンを常駐させ、
右クリックメニューから Windows Update の停止・再開を操作する。
レジストリポリシーとサービス制御で WU を制御し、操作には管理者昇格が必要。

## Entry Points

- `src/WuTrayToggle/Program.cs` — エントリポイント。引数なしならトレイ常駐（`Mutex` で多重起動防止）、`--install`/`--uninstall`/`--elevated-stop`/`--elevated-start` を解釈する
- `src/WuTrayToggle/TrayApplicationContext.cs` — トレイアイコン・コンテキストメニューの常駐処理

## Directory Structure

| ディレクトリ | 役割 |
|---|---|
| `src/WuTrayToggle/` | C# プロジェクト本体（.csproj + ソース） |
| `docs/dev-charter/` | 開発憲章（git subtree） |
| `docs/` | プロジェクトドキュメント |

## Key Dependencies

| ライブラリ / モジュール | 用途 |
|---|---|
| `System.Windows.Forms` | トレイアイコン・コンテキストメニュー・メッセージボックス |
| `System.Drawing` | 埋め込み`.ico`リソースからの `Icon` 読み込み |
| `Microsoft.Win32.Registry` | WU ポリシー(`NoAutoUpdate`)・サービス起動種別の読み書き |
| `System.ServiceProcess.ServiceController` (NuGet) | `wuauserv` サービスの状態取得・起動・停止 |
| `IShellLinkW`/`IPersistFile` (COM interop) | ショートカット（.lnk）ファイルの作成・削除 |
