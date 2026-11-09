param([string]$Configuration = 'Debug')
$ErrorActionPreference = 'Stop'
$coreRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..\..\CoreAssemblies'))
$logRoot = Join-Path $PSScriptRoot ('CoreBuild\logs-' + $Configuration)
New-Item -ItemType Directory -Path $logRoot -Force | Out-Null
foreach ($name in 'CommonUtilities','UsefulDBMethods','ProgressLogging','ActionBase','RequestProcessor') {
    Push-Location -LiteralPath (Join-Path $coreRoot $name)
    try {
        & msbuild ($name + '.csproj') /t:Build /nologo /m:1 /nr:false /v:minimal "/p:Configuration=$Configuration" /p:BuildProjectReferences=false /p:PostBuildEvent= /p:UseSharedCompilation=false "/p:CustomBeforeMicrosoftCommonTargets=$PSScriptRoot\LocalBuildOutput.targets" *> (Join-Path $logRoot ($name + '.log'))
        if ($LASTEXITCODE -ne 0) { throw "Core build failed: $name. See $logRoot" }
        Write-Output "Built $name ($Configuration) from source."
    }
    finally { Pop-Location }
}
