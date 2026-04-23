# Architecture

## Overview

PowerShell 製システムトレイアプリ。System.Windows.Forms でトレイアイコンを常駐させ、
右クリックメニューから Windows Update の停止・再開を操作する。
レジストリポリシーとサービス制御で WU を制御し、操作には管理者昇格が必要。

## Entry Points

- `src/tray.ps1` — トレイアイコン常駐プロセス（`-STA` モードで起動）
- `src/install.ps1` — デスクトップショートカット作成（`make install` から呼び出し）

## Directory Structure

| ディレクトリ | 役割 |
|---|---|
| `src/` | PowerShell スクリプト本体 |
| `docs/dev-charter/` | 開発憲章（git subtree） |
| `docs/` | プロジェクトドキュメント |

## Key Dependencies

| ライブラリ / モジュール | 用途 |
|---|---|
| `System.Windows.Forms` | トレイアイコン・コンテキストメニュー・メッセージボックス |
| `System.Drawing` | 64×64 アイコン画像のプログラム生成 |
| `WScript.Shell` (COM) | ショートカット（.lnk）ファイルの作成 |
