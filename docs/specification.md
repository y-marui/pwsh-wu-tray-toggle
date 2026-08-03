# Specification

## Overview

Windows Update の自動更新をシステムトレイから停止・再開する C# 製トレイアプリ。

## Windows Update 制御

| 操作 | レジストリキー | 値 | サービス操作 |
|---|---|---|---|
| 停止 | `HKLM:\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU` | `NoAutoUpdate = 1` | `wuauserv` を停止 |
| 再開 | 同上 | `NoAutoUpdate = 0` | 起動種別を Manual に設定し `wuauserv` を起動 |

管理者権限が必要なため、トレイ本体は非昇格で常駐し、停止・再開クリック時のみ自exeを `--elevated-stop` / `--elevated-start` 引数・`Verb=runas` で起動して昇格プロセスに処理させる（`src/WuTrayToggle/WindowsUpdateController.cs`）。

## アイコン表示

| 状態 | アイコン | トレイテキスト |
|---|---|---|
| 稼働中 | RoyalBlue の弧 + 矢印 | `WU: 稼働中 (通常モード)` |
| 停止中 | Red の × | `WU: 停止中 (制御モード)` |

アイコンは `IconFactory.Create` 関数で 64×64 ビットマップをプログラム生成し、`Icon.FromHandle` で変換する。

## インストール

`src/WuTrayToggle/ShortcutManager.cs` が `--install` 引数呼び出し時にデスクトップへ `WU_TrayIcon.lnk` を作成する（`IShellLinkW`/`IPersistFile` COM interop）。
ショートカットは自exe（`Environment.ProcessPath`）を直接指すため、PowerShellの起動を経由しない。
`--uninstall` 引数、または `make uninstall` でショートカットを削除する。

## 既知の制約

- Windows 専用（System.Windows.Forms が必要）
- レジストリ操作とサービス制御に管理者権限が必要
- UI テキストが日本語ハードコード（Issue #3 参照）
