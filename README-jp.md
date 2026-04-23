# pwsh-wu-tray-toggle

> **このファイルは正本（日本語版）です。**
> 英語版（参照）は [README.md](README.md) を参照してください。

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
[![CI](https://github.com/y-marui/pwsh-wu-tray-toggle/actions/workflows/ci.yml/badge.svg)](https://github.com/y-marui/pwsh-wu-tray-toggle/actions/workflows/ci.yml)
[![Charter Check](https://github.com/y-marui/pwsh-wu-tray-toggle/actions/workflows/dev-charter-check.yml/badge.svg)](https://github.com/y-marui/pwsh-wu-tray-toggle/actions/workflows/dev-charter-check.yml)

Windows Update の自動更新を通知領域のトレイアイコンから停止・再開できる PowerShell 製トレイアプリ。

## Setup

PowerShell と管理者権限が必要です。

```powershell
make install
```

デスクトップに `WU_TrayIcon.lnk` ショートカットが作成されます。ダブルクリックで起動します。

## Usage

| コマンド | 説明 |
|---|---|
| `make install` | デスクトップにショートカットをインストール |
| `make uninstall` | ショートカットを削除 |

トレイアイコンを右クリックしてメニューを操作します：

- **現在の状態を確認** — ポリシーとサービスの状態をポップアップ表示
- **停止 (制御開始)** — グループポリシー経由で Windows Update を停止
- **再開 (通常)** — Windows Update を通常モードに戻す

## License

[MIT](LICENSE)

---
*この文書には英語版 [README.md](README.md) があります。編集時は同一コミットで更新してください。*
