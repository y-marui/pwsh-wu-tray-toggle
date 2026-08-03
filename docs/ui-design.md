# UI Design

## Components

### System Tray Icon

- `src/WuTrayToggle/Assets/` の静的 `.ico`（盾モチーフ）を埋め込みリソースとして持ち、`IconFactory.Create` が状態に応じて読み込む
- 状態に応じてアイコンを切り替え（詳細は `docs/specification.md` 参照）
- ダーク/ライトモード切り替え: 未対応

### Context Menu

右クリックで表示するメニュー（`src/WuTrayToggle/TrayApplicationContext.cs`）：

| メニュー項目 | 動作 |
|---|---|
| 現在の状態を確認 | ポリシーとサービス状態をメッセージボックスで表示 |
| ログイン時に自動起動 | チェック可能項目。クリックでスタートアップフォルダへのショートカット登録/解除をトグル |
| 停止 (制御開始) | 管理者昇格して WU 停止 |
| 再開 (通常) | 管理者昇格して WU 再開 |
| 終了 | トレイアイコンを終了 |

## Notes

- UI テキストのローカライズ未対応（日本語ハードコード）→ Issue #3 参照
