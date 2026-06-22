param(
    [Parameter(Mandatory = $true)]
    [string] $Host,

    [Parameter(Mandatory = $true)]
    [string] $Password,

    [string] $User = 'postgres',

    [string] $Database = 'postgres',

    [int] $Port = 5432
)

$encodedUser = [Uri]::EscapeDataString($User)
$encodedPassword = [Uri]::EscapeDataString($Password)
$encodedDatabase = [Uri]::EscapeDataString($Database)

"postgresql://$($encodedUser):$($encodedPassword)@$($Host):$($Port)/$($encodedDatabase)?sslmode=require"
