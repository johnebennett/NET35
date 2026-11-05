[CmdletBinding()]
param([string]$MSBuildPath)
$ErrorActionPreference = 'Stop'
if (!$MSBuildPath) {
    $command = Get-Command MSBuild.exe -ErrorAction SilentlyContinue
    if ($command) { $MSBuildPath = $command.Source }
    else {
        $vswhere = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio\Installer\vswhere.exe'
        $MSBuildPath = & $vswhere -latest -products '*' -requires Microsoft.Component.MSBuild -find 'MSBuild\**\Bin\MSBuild.exe' | Select-Object -First 1
    }
}
if (!$MSBuildPath) { throw 'MSBuild not found. Supply -MSBuildPath.' }
if (!(Test-Path -LiteralPath (Join-Path $PSScriptRoot 'artifacts\Release\ActionBase.dll'))) { throw 'Run .\Build.ps1 -Configuration Release -Clean first.' }
& $MSBuildPath (Join-Path $PSScriptRoot 'Verification\Verification.csproj') /nologo /nr:false /p:UseSharedCompilation=false /t:Rebuild /v:minimal
if ($LASTEXITCODE -ne 0) { throw 'Verification build failed' }
$testRoot = Join-Path $PSScriptRoot 'Verification\bin'
foreach ($dll in Get-ChildItem (Join-Path $PSScriptRoot 'artifacts\Release') -Filter '*.dll' -File) { Copy-Item -LiteralPath $dll.FullName -Destination $testRoot -Force }
foreach ($arch in 'x86','x64') {
    $dest = Join-Path $testRoot $arch
    New-Item -ItemType Directory -Path $dest -Force | Out-Null
    Copy-Item -LiteralPath (Join-Path $PSScriptRoot "ThirdParty\$arch\SQLite.Interop.dll") -Destination $dest -Force
}
$log = Join-Path $PSScriptRoot 'artifacts\logs\verification.log'
New-Item -ItemType Directory -Path (Split-Path $log -Parent) -Force | Out-Null
Set-Content -LiteralPath $log -Value ('Verification ' + (Get-Date -Format o)) -Encoding UTF8
foreach ($scenario in 'graph','unconfigured','empty','throwing','sink','concurrency','bootstrap','sqlite') {
    $result = & (Join-Path $testRoot 'RefactorVerification.exe') $scenario 2>&1
    $exitCode = $LASTEXITCODE
    $result | Out-File -LiteralPath $log -Append -Encoding UTF8
    $result
    if ($exitCode -ne 0) { throw "Verification failed: $scenario" }
}
Write-Output 'All 8 verification scenarios passed.'
