# AI_CONTEXT.md

## Reference Order

AI はタスク開始時に以下の順で参照する:

1. README.md（概要・セットアップ）
2. DEVELOPING.md（ビルド・実装規約・命名規則）

必要に応じて以下を参照する（順不同）:
- CONTRIBUTING.md（PR・Issue ルール）
- docs/architecture.md（モジュール・コンポーネント構造）
- docs/file-map.md（ファイルレベルの依存関係 ※情報が足りない・古い場合は適宜探索し、追記・更新する）
- docs/specification.md（機能仕様・データフロー）
- docs/ui-design.md（UI 設計・コンポーネント仕様）

## Project Overview

Windows Update の自動更新をシステムトレイから停止・再開できる PowerShell 製トレイアプリ。

- **言語:** PowerShell
- **対象OS:** Windows のみ
- **主要ファイル:**
  - `src/tray.ps1` — トレイアイコン本体（System.Windows.Forms を使用）
  - `src/install.ps1` — デスクトップショートカット作成スクリプト
  - `Makefile` — install / uninstall コマンド

## Applied Charter Principles

憲章参照: `docs/dev-charter/CHARTER_INDEX.md` でトピックを特定してから該当ファイルのみ読む。

- **言語ポリシー:** README.md（英語・参照）と README-jp.md（日本語・正本）を同一コミットで更新する → `docs/dev-charter/LANGUAGE_POLICY.md`
- **CI:** `.github/workflows/ci.yml` に PSScriptAnalyzer Lint + Build 集約 job → `docs/dev-charter/topics/CI_POLICY.md`
- **コーディング原則:** YAGNI・変更最小限・DRY3回ルール → `docs/dev-charter/PRINCIPLES.md`

## Document Sync Rule

仕様・ルール・構成に変更が生じたとき、変更と同じ作業内で関連ドキュメントを更新する。
対象は docs/ 内のファイルに限らず、AI_CONTEXT.md・README.md・README-jp.md 等のルートファイルも含む。

## Project-Specific Rules

- `src/tray.ps1` はレジストリ書き込みと Windows サービス操作を行うため、変更時は管理者権限要件を考慮する
- インストール先はデスクトップショートカット（`WU_TrayIcon.lnk`）のみ。レジストリへの永続インストールは行わない

## AI Tool Assignments

- **Claude Code:** 実装変更・リファクタリング・CI 設定・ドキュメント更新

## Prohibited Actions

- シークレット・認証情報のコミット
- Windows 以外のプラットフォーム向けコードの追加
- ソースコード以外のビルド成果物のコミット
