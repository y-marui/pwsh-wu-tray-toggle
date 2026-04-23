# Specification

## Overview

Windows Update の自動更新をシステムトレイから停止・再開する PowerShell 製トレイアプリ。

## Windows Update 制御

| 操作 | レジストリキー | 値 | サービス操作 |
|---|---|---|---|
| 停止 | `HKLM:\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU` | `NoAutoUpdate = 1` | `Stop-Service wuauserv -Force` |
| 再開 | 同上 | `NoAutoUpdate = 0` | `Start-Service wuauserv` |

管理者権限が必要なため、`Start-Process powershell -Verb RunAs` で昇格プロセスを起動して操作する。

## アイコン表示

| 状態 | アイコン | トレイテキスト |
|---|---|---|
| 稼働中 | RoyalBlue の弧 + 矢印 | `WU: 稼働中 (通常モード)` |
| 停止中 | Red の × | `WU: 停止中 (制御モード)` |

アイコンは `Get-MyIcon` 関数で 64×64 ビットマップをプログラム生成し、`Drawing.Icon.FromHandle` で変換する。

## インストール

`src/install.ps1` がデスクトップに `WU_TrayIcon.lnk` を作成する。
ショートカットは `-NoProfile -STA -WindowStyle Hidden -ExecutionPolicy Bypass` で `tray.ps1` を起動する。

## 既知の制約

- Windows 専用（System.Windows.Forms が必要）
- レジストリ操作とサービス制御に管理者権限が必要
- UI テキストが日本語ハードコード（Issue #3 参照）
