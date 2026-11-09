param([string]$Label = 'baseline', [string]$Configuration = 'Debug')
$ErrorActionPreference = 'Stop'
$root = Split-Path $PSScriptRoot -Parent
$logRoot = Join-Path $PSScriptRoot ('Build-' + $Label + '-' + $Configuration)
New-Item -ItemType Directory -Path $logRoot -Force | Out-Null
$results = @()
$projects = @{}
foreach ($file in (Get-ChildItem -LiteralPath $root -Recurse -Filter *.csproj | Where-Object { $_.FullName -notlike "$PSScriptRoot\*" })) { $projects[$file.FullName] = $file }
$ordered = New-Object 'System.Collections.Generic.List[System.IO.FileInfo]'
$visiting = @{}
$visited = @{}
function Visit($path) {
    if ($visited.ContainsKey($path)) { return }
    if ($visiting.ContainsKey($path)) { throw "Circular module dependency: $path" }
    $visiting[$path] = $true
    [xml]$project = [IO.File]::ReadAllText($path)
    foreach ($reference in $project.SelectNodes('//*[local-name()="ProjectReference"]')) {
        $dependency = [IO.Path]::GetFullPath((Join-Path (Split-Path $path -Parent) $reference.Include))
        if ($projects.ContainsKey($dependency)) { Visit $dependency }
    }
    $visiting.Remove($path)
    $visited[$path] = $true
    $ordered.Add($projects[$path])
}
foreach ($path in ($projects.Keys | Sort-Object)) { Visit $path }
foreach ($file in $ordered) {
    $relative = $file.FullName.Substring($root.Length + 1)
    $log = Join-Path $logRoot ($relative.Replace('\','_') + '.log')
    & msbuild $file.FullName /t:Rebuild /nologo /m:1 /nr:false /v:minimal "/p:Configuration=$Configuration" /p:BuildProjectReferences=false /p:PostBuildEvent= /p:UseSharedCompilation=false "/p:CustomBeforeMicrosoftCommonTargets=$PSScriptRoot\LocalBuildOutput.targets" *> $log
    $code = $LASTEXITCODE
    $results += [pscustomobject]@{ Project=$relative; ExitCode=$code; Log=$log }
    Write-Output "$code $relative"
}
$results | Export-Csv -LiteralPath (Join-Path $logRoot 'results.csv') -NoTypeInformation
Write-Output "Passed: $(@($results | Where-Object ExitCode -eq 0).Count)/$($results.Count). Logs: $logRoot"
