param(
    [Parameter(Mandatory = $true)]
    [ValidatePattern('^https://github\.com/.+/.+(\.git)?$')]
    [string] $RepositoryUrl
)

$ErrorActionPreference = 'Stop'

if (-not (Test-Path '.git')) {
    throw 'This folder is not a Git repository.'
}

$origin = git remote get-url origin 2>$null
if ($LASTEXITCODE -eq 0 -and -not [string]::IsNullOrWhiteSpace($origin)) {
    git remote set-url origin $RepositoryUrl
}
else {
    git remote add origin $RepositoryUrl
}

git push -u origin main
