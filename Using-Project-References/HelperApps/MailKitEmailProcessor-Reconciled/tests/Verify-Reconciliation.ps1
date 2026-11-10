param(
    [Parameter(Mandatory=$true)][string]$AssemblyPath,
    [switch]$ExpectBypass
)
$ErrorActionPreference = 'Stop'
$assemblyFile = (Resolve-Path -LiteralPath $AssemblyPath).Path
$assembly = [Reflection.Assembly]::UnsafeLoadFrom($assemblyFile)
$checks = 0
function Assert-True($condition, $description) {
    if (-not $condition) { throw "FAIL: $description" }
    $script:checks++
}
function New-ItemOfType($name) {
    return [Activator]::CreateInstance($assembly.GetType("MailKitEmailProcessor.Actions.$name", $true))
}
$account = New-ItemOfType 'AccountDetails'
$hasBypass = $null -ne $account.GetType().GetProperty('BypassAuthCheck')
Assert-True ($hasBypass -eq [bool]$ExpectBypass) 'Expected account feature set'
$account.Host = 'localhost'
$account.Port = 587
Assert-True ($account.Validate() -eq 'Empty or invalid UserName provided') 'Credentials required by default'
$account.UserName = 'test-user'
Assert-True ($account.Validate() -eq 'Empty or invalid Password provided') 'Password required by default'
$account.Password = 'test-password'
Assert-True ($account.Validate() -eq '') 'Complete account validates'
if ($ExpectBypass) {
    Assert-True (-not $account.BypassAuthCheck) 'Bypass defaults to false'
    $account.BypassAuthCheck = $true
    $account.UserName = ''
    $account.Password = ''
    Assert-True ($account.Validate() -eq '') 'Relay account accepts empty credentials'
    $account.Host = ''
    Assert-True ($account.Validate() -eq 'Empty or invalid Host provided') 'Bypass still requires host'
    $account.Host = 'localhost'
    $account.Port = 0
    Assert-True ($account.Validate() -eq 'No Port number provided') 'Bypass still requires port'
    $account.Port = 587
    $copy = New-ItemOfType 'AccountDetails'
    Assert-True ($copy.CopyFrom($account) -eq '' -and $copy.BypassAuthCheck) 'Account copy retains bypass'
    $param = New-ItemOfType 'SendEmailParam'
    Assert-True ($param.CopyAccountDetailsFrom($account) -eq '' -and $param.AccountDetails.BypassAuthCheck) 'Send parameters retain bypass'
    $account.BypassAuthCheck = $false
    $null = $copy.CopyFrom($account)
    $null = $param.CopyAccountDetailsFrom($account)
    Assert-True (-not $copy.BypassAuthCheck -and -not $param.AccountDetails.BypassAuthCheck) 'Copy resets bypass to false'
}
$sender = New-ItemOfType 'SenderDetailsItem'
$sender.Address = 'sender@example.test'
$param = New-ItemOfType 'SendEmailParam'
Assert-True ($param.CopySenderFrom($sender) -eq '') 'Sender copy succeeds'
Assert-True ($param.Sender.Name -eq $sender.Address) 'Sender name fallback matches production'
Assert-True ($param.Sender.ReplyToAddress -eq $sender.Address) 'Reply-to fallback matches production'
$sender.ReplyToAddress = 'reply@example.test'
$null = $param.CopySenderFrom($sender)
Assert-True ($param.Sender.ReplyToAddress -eq 'reply@example.test') 'Explicit reply-to retained'
$senderCopy = New-ItemOfType 'SenderDetailsItem'
Assert-True ($senderCopy.CopyFrom($sender) -eq '' -and $senderCopy.ReplyToAddress -eq $sender.ReplyToAddress) 'Sender details copy retains reply-to'

# All test files are isolated beneath the tested assembly directory.
$testRoot = Join-Path (Split-Path $assemblyFile) ('verification-' + [Guid]::NewGuid().ToString('N'))
$null = New-Item -ItemType Directory -Path $testRoot
$configFile = Join-Path $testRoot 'settings.json'
'{"RunningOnEmailServer":"Y","SendEmail":"N","UpdateSentValues":"N","RetryCount":7,"LocalDocsLocation":"test-docs"}' | Set-Content -LiteralPath $configFile
$configType = $assembly.GetType('MailKitEmailProcessor.Actions.ConfigSettings', $true)
$config = $configType.GetMethod('LoadFromJson').Invoke($null, @([string]$configFile))
Assert-True ($config.RunningOnEmailServer -eq 'Y' -and $config.SendEmail -eq 'N' -and $config.UpdateSentValues -eq 'N') 'Production flags deserialize'
Assert-True ($config.RetryCount -eq 7 -and $config.LocalDocsLocation -eq 'test-docs') 'Production path and retry settings deserialize'
$config.ShareCreateDocsLocation = Join-Path $testRoot 'share'
$config.LocalCreateDocsLocation = Join-Path $testRoot 'local'
foreach ($folder in @('signature', 'logo', 'images')) {
    $source = Join-Path $config.ShareCreateDocsLocation "$folder\42"
    $target = Join-Path $config.LocalCreateDocsLocation "$folder\42"
    $null = New-Item -ItemType Directory -Force -Path $source, $target
    [IO.File]::WriteAllText((Join-Path $source 'asset.txt'), "new-$folder")
    [IO.File]::WriteAllText((Join-Path $target 'asset.txt'), 'old')
}
$common = $assembly.GetType('MailKitEmailProcessor.Actions.CommonMethods', $true)
$result = $common.GetMethod('SyncCreateDocsItems').Invoke($null, @([int]0, [long]42, $config.psobject.BaseObject))
Assert-True ($result -eq '') 'Shared asset synchronization succeeds'
foreach ($folder in @('signature', 'logo', 'images')) {
    $actual = [IO.File]::ReadAllText((Join-Path $config.LocalCreateDocsLocation "$folder\42\asset.txt"))
    Assert-True ($actual -eq "new-$folder") "Shared $folder asset overwritten as in production"
}
Assert-True ($common.GetMethod('CopyEmailTemplateToEmailServer').Invoke($null, @([long]42)) -eq '') 'Production template-copy stub retained'
Assert-True ($common.GetMethod('LoadAppSettings').Invoke($null, @()) -eq '') 'Production settings stub retained'
Write-Output "PASS: $checks checks; assembly=$assemblyFile; bypass=$ExpectBypass"
