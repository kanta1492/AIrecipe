# AI献立ノート

スマホ向けの Blazor / C# 献立アプリです。登録食材、期限、食事タイプから献立を提案し、冷蔵庫管理から記録までをまとめて扱えます。

## 起動方法

1. Visual Studio で `AiMealPlanner.sln` を開きます。
2. `AiMealPlanner` をスタートアッププロジェクトにします。
3. F5 または `dotnet run` で起動します。
4. 最初に登録したユーザーが管理者になります。

## 他の人に使ってもらう

### 同じWi-Fi内で使う

Visual Studio の起動プロファイルで `lan-http` を選んで実行します。起動後、同じWi-FiにいるスマホやPCから次の形式で開きます。

```text
http://PCのIPv4アドレス:5254
```

このPCでは現在のIPv4アドレスが `192.168.10.106` なので、同じWi-Fi内では次のURLです。

```text
http://192.168.10.106:5254
```

開けない場合は、Windows Defender ファイアウォールで `dotnet.exe` または TCP `5254` のプライベートネットワーク通信を許可してください。

### GitHub + Render + Supabase で公開する

家の外の人にも使わせる場合は、この開発用URLをそのまま公開しないでください。GitHubでソースコードを管理し、RenderでWebアプリ、SupabaseでPostgreSQLを使う構成を想定しています。

1. GitHubで新しいリポジトリを作り、このプロジェクトをpushします。
2. Supabaseで新しいプロジェクトを作り、PostgreSQLの接続URLを取得します。
3. Renderで `New +` -> `Blueprint` または Docker Web Service を作り、このリポジトリを接続します。
4. Renderの環境変数に Supabase の接続URLを `DATABASE_URL` として登録します。
5. Renderのデプロイ完了後に発行される `https://...onrender.com` が公開URLです。

Render用の `Dockerfile` と `render.yaml` は同梱済みです。ローカルではSQLite、Renderでは `DATABASE_URL` がある場合だけSupabase PostgreSQLに接続します。

写真ファイルとログイン暗号鍵は `UPLOADS_PATH` と `DATA_PROTECTION_KEYS_PATH` に保存します。本番運用ではRender DiskまたはSupabase Storageへの移行を検討してください。

## 主な機能

- ログイン / ログアウト / パスキー / ロックアウト
- 最初のユーザーを管理者化
- 通常ユーザー画面と管理者画面の分離
- ログイン画面左上の隠しタップ領域から管理者ログインモードへ切り替え
- 冷蔵庫の食材登録
- スマホカメラ / 写真アップロードによる写真記録
- 写真メモから食材候補を抽出
- 献立提案
- 献立履歴、お気に入り、写真履歴
- アプリ内通知とブラウザ通知
- 管理者によるユーザー権限管理

## 管理者ログイン

通常のログイン画面の左上に、見えない管理者入口があります。そこをタップすると管理者ログイン画面に切り替わります。

管理者画面へ入れるのは `Admin` ロールを持つユーザーだけです。隠し入口は画面切り替え用であり、権限そのものはサーバー側のロール認可で判定します。

## 献立提案について

写真メモや確認済み食材をもとに候補抽出します。

献立提案は登録食材、期限、食事タイプ、気分を使って作成します。

## セキュリティ方針

- ASP.NET Core Identity によるパスワードハッシュ化
- ロール認可による管理者画面保護
- ログイン失敗時のロックアウト
- CSRF対策として AntiforgeryToken を使用
- 写真は `wwwroot` ではなく `Data/uploads` に保存
- 写真取得は `/private/photos/{id}` 経由で本人または管理者だけに制限
- セキュリティヘッダーを設定
- 入力値に長さ制限と型検証を設定

「完璧なセキュリティ」は保証できません。公開運用する場合は HTTPS、バックアップ、監査ログ、脆弱性アップデート、秘密情報管理、運用権限管理を追加してください。

## 既知の注意

`.NET 10.0.8` の `Microsoft.EntityFrameworkCore.Sqlite` は、推移依存の `SQLitePCLRaw.lib.e_sqlite3 2.1.11` について NuGet 監査警告 `GHSA-2m69-gcr7-jv3q` を表示します。公式NuGet上ではこの依存パッケージの更新先が確認できなかったため、警告は隠していません。

SQL Server LocalDB 構成も検討しましたが、このPCでは `sqllocaldb start MSSQLLocalDB` が失敗したため、単体で起動しやすいSQLite構成にしています。
