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

アイコンは `src/WuTrayToggle/Assets/` の静的 `.ico`（盾モチーフ、稼働中=更新矢印/停止中=赤X）を埋め込みリソースとして持ち、`IconFactory.Create` がリソースストリームから `Icon` を読み込む。exe自体のファイルアイコン（`Assets/app.ico`、盾+歯車）は `<ApplicationIcon>` で設定。

## インストール

`src/WuTrayToggle/ShortcutManager.cs` が `--install` 引数呼び出し時にデスクトップへ `WU_TrayIcon.lnk` を作成する（`IShellLinkW`/`IPersistFile` COM interop）。
ショートカットは自exe（`Environment.ProcessPath`）を直接指すため、PowerShellの起動を経由しない。
`--uninstall` 引数、または `make uninstall` でショートカットを削除する。

## ログイン時自動起動

トレイメニューの「ログイン時に自動起動」（チェック可能項目）で、スタートアップフォルダ（`Environment.SpecialFolder.Startup`、`%APPDATA%\Microsoft\Windows\Start Menu\Programs\Startup`）への自exeショートカット登録/解除をトグルする（`ShortcutManager.EnableStartup`/`DisableStartup`）。
レジストリの `Run` キー等は使用せず、ユーザー単位のスタートアップフォルダのみを使う（永続的なレジストリインストールを避ける方針に従う）。
`--uninstall`（`make uninstall` 相当）実行時は、このスタートアップ登録も合わせて解除される。

## ローカライゼーション

`docs/dev-charter/LOCALIZATION_POLICY.md` に従い、メニュー・トレイツールチップ・メッセージボックス・バルーン通知のテキストを日本語/英語/中国語/ヒンディー語/スペイン語/フランス語/ポルトガル語の7言語に対応する。

- 言語決定の優先順位: ユーザー設定（トレイメニュー「言語」で選択、`%APPDATA%\WuTrayToggle\language.txt` に永続化）＞システムのUI言語（`GetUserDefaultUILanguage` で判定）＞英語
- 文字列は `src/WuTrayToggle/Strings.cs` に集約。中国語/ヒンディー語/スペイン語/フランス語/ポルトガル語はネイティブレビュー未実施
- `<InvariantGlobalization>true</InvariantGlobalization>` のため `CultureInfo` はシステム言語を反映しない。そのため `Localization.cs` は Win32 API を直接 P/Invoke してシステムのUI言語を取得する

## 既知の制約

- Windows 専用（System.Windows.Forms が必要）
- レジストリ操作とサービス制御に管理者権限が必要
