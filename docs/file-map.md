# File Map

_最終更新: 2026-04-24_

## Core

| ファイル | 役割 | 主な依存先 |
|---|---|---|
| `src/tray.ps1` | トレイアイコン常駐・メニュー操作・WU 制御 | `System.Windows.Forms`, `System.Drawing` |
| `src/install.ps1` | デスクトップショートカット作成 | `WScript.Shell` (COM) |
