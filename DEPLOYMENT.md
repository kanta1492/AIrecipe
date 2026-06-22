# 公開手順

## 1. GitHub

GitHubで空のリポジトリを作成し、このプロジェクトにリモートを設定します。

```powershell
git remote add origin https://github.com/<owner>/<repo>.git
git push -u origin main
```

すでに `origin` がある場合は、次のように変更します。

```powershell
git remote set-url origin https://github.com/<owner>/<repo>.git
git push -u origin main
```

補助スクリプトも用意しています。

```powershell
.\scripts\Push-GitHub.ps1 -RepositoryUrl https://github.com/<owner>/<repo>.git
```

## 2. Supabase

Supabaseでプロジェクトを作成し、PostgreSQLの接続文字列を取得します。Renderには接続文字列を `DATABASE_URL` として登録します。

アプリは `DATABASE_URL` がある場合にPostgreSQLへ接続し、ない場合はローカルSQLiteで動作します。

## 3. Render

RenderでGitHubリポジトリを接続し、BlueprintまたはDocker Web Serviceとして作成します。

設定値:

- `DATABASE_URL`: SupabaseのPostgreSQL接続文字列
- `DATA_PROTECTION_KEYS_PATH`: `/var/data/keys`
- `UPLOADS_PATH`: `/var/data/uploads`

`scripts/Render-Env.sample.txt` に環境変数のサンプルがあります。

Renderのデプロイが完了すると、`https://...onrender.com` の公開URLが発行されます。

## 4. 初回ログイン

最初に登録したユーザーが管理者になります。管理者画面はログイン画面左上の隠し入口から入れます。
