<#
.SYNOPSIS
Builds every active module project and its project dependencies in dependency order.
.DESCRIPTION
See BUILD.md alongside this script for prerequisites, outputs, and examples.
Use Build.cmd from Command Prompt. No project files or target frameworks are changed.
#>
[CmdletBinding()]
param(
    [ValidateSet('Debug', 'Release')][string]$Configuration = 'Release',
    [ValidateSet('AnyCPU', 'x86', 'x64')][string]$Platform = 'AnyCPU',
    [switch]$Rebuild,
    [string]$MSBuildPath,
    [switch]$RunPostBuildEvents,
    [switch]$FailFast,
    [switch]$ListProjects
)

$ErrorActionPreference = 'Stop'
$root = [IO.Path]::GetFullPath($PSScriptRoot)
$outputTargets = Join-Path $root 'Build.Outputs.targets'
$logRoot = $null

try {
    # Skip generated and historical projects before descending into their folders.
    $excludedDirectories = @('bin', 'obj', 'artifacts', 'packages', 'Backups', '.git', '.codex', '.agents', '.reference-migration')
    $directories = New-Object 'System.Collections.Generic.Stack[string]'
    $directories.Push($root)
    $modulePaths = New-Object 'System.Collections.Generic.List[string]'
    while ($directories.Count -gt 0) {
        $directory = $directories.Pop()
        foreach ($file in (Get-ChildItem -LiteralPath $directory -File)) {
            if ($file.Extension -in '.csproj', '.vbproj', '.fsproj') { $modulePaths.Add($file.FullName) }
        }
        foreach ($child in (Get-ChildItem -LiteralPath $directory -Directory)) {
            if ($child.Name -notin $excludedDirectories -and ($child.Attributes -band [IO.FileAttributes]::ReparsePoint) -eq 0) {
                $directories.Push($child.FullName)
            }
        }
    }
    if ($modulePaths.Count -eq 0) { throw "No module projects found beneath $root" }

    $visited = @{}
    $visiting = @{}
    $ordered = New-Object 'System.Collections.Generic.List[string]'
    function Visit-Project([string]$Path) {
        if ($visited.ContainsKey($Path)) { return }
        if ($visiting.ContainsKey($Path)) { throw "Circular project reference involving $Path" }
        if (!(Test-Path -LiteralPath $Path -PathType Leaf)) { throw "Referenced project does not exist: $Path" }
        $visiting[$Path] = $true
        [xml]$project = [IO.File]::ReadAllText($Path)
        foreach ($reference in $project.SelectNodes('//*[local-name()="ProjectReference"]')) {
            $include = $reference.GetAttribute('Include')
            if ($include -match '[$@%]\(|[*?]') { throw "Cannot resolve nonliteral project reference '$include' in $Path" }
            $dependency = [IO.Path]::GetFullPath((Join-Path (Split-Path $Path -Parent) $include))
            Visit-Project $dependency
        }
        $visiting.Remove($Path)
        $visited[$Path] = $true
        $ordered.Add($Path)
    }
    foreach ($path in ($modulePaths | Sort-Object)) { Visit-Project $path }
    $moduleSet = @{}
    foreach ($path in $modulePaths) { $moduleSet[$path] = $true }
    $dependencyCount = $ordered.Count - $modulePaths.Count
    Write-Host "Found $($modulePaths.Count) module projects and $dependencyCount dependency projects."

    if ($ListProjects) {
        foreach ($path in $ordered) {
            $kind = if ($moduleSet.ContainsKey($path)) { 'Module' } else { 'Dependency' }
            Write-Output "[$kind] $path"
        }
        exit 0
    }

    if (!$MSBuildPath) {
        $command = Get-Command MSBuild.exe -ErrorAction SilentlyContinue
        if ($command) { $MSBuildPath = $command.Source }
        else {
            $programFilesX86 = ${env:ProgramFiles(x86)}
            if ($programFilesX86) {
                $vswhere = Join-Path $programFilesX86 'Microsoft Visual Studio\Installer\vswhere.exe'
                if (Test-Path -LiteralPath $vswhere) {
                    $MSBuildPath = & $vswhere -latest -products '*' -requires Microsoft.Component.MSBuild -find 'MSBuild\**\Bin\MSBuild.exe' | Select-Object -First 1
                }
            }
        }
    }
    if (!$MSBuildPath -or !(Test-Path -LiteralPath $MSBuildPath -PathType Leaf)) {
        throw 'Install Visual Studio/Build Tools with MSBuild, or supply -MSBuildPath with the full path to MSBuild.exe.'
    }
    $MSBuildPath = (Resolve-Path -LiteralPath $MSBuildPath).Path
    if (!(Test-Path -LiteralPath $outputTargets)) { throw "Missing build support file: $outputTargets" }

    $logRoot = Join-Path $root ('artifacts\logs\' + (Get-Date -Format 'yyyyMMdd-HHmmss-fff') + '-' + $Configuration)
    New-Item -ItemType Directory -Path $logRoot -Force | Out-Null
    $target = if ($Rebuild) { 'Rebuild' } else { 'Build' }
    Write-Host "MSBuild: $MSBuildPath"
    Write-Host "$target / $Configuration / $Platform. Logs: $logRoot"
    $results = New-Object 'System.Collections.Generic.List[object]'
    $failedProjects = @{}
    $index = 0
    foreach ($path in $ordered) {
        $index++
        $projectDirectory = Split-Path $path -Parent
        $name = [IO.Path]::GetFileNameWithoutExtension($path)
        $kind = if ($moduleSet.ContainsKey($path)) { 'Module' } else { 'Dependency' }
        $log = Join-Path $logRoot ('{0:D3}-{1}.log' -f $index, $name)
        [xml]$project = [IO.File]::ReadAllText($path)
        $blockedBy = @(
            foreach ($reference in $project.SelectNodes('//*[local-name()="ProjectReference"]')) {
                $dependency = [IO.Path]::GetFullPath((Join-Path $projectDirectory $reference.GetAttribute('Include')))
                if ($failedProjects.ContainsKey($dependency)) { $dependency }
            }
        )
        $started = Get-Date
        if ($blockedBy.Count -gt 0) {
            $status = 'Skipped'
            $code = 1
            $failedProjects[$path] = $true
            "Skipped because project dependencies failed or were skipped:`r`n$($blockedBy -join "`r`n")" | Set-Content -LiteralPath $log -Encoding UTF8
        }
        else {
            $arguments = @(
                $path, "/t:$target", '/nologo', '/m:1', '/nr:false', '/v:minimal',
                "/p:Configuration=$Configuration", "/p:Platform=$Platform",
                '/p:BuildProjectReferences=false', '/p:UseSharedCompilation=false',
                "/p:CustomBeforeMicrosoftCommonTargets=$outputTargets"
            )
            if (!$RunPostBuildEvents) { $arguments += '/p:PostBuildEvent=' }
            # Some legacy projects require the process working directory to be their own directory.
            Push-Location -LiteralPath $projectDirectory
            try {
                & $MSBuildPath @arguments *> $log
                $code = $LASTEXITCODE
            }
            finally { Pop-Location }
            $status = if ($code -eq 0) { 'Passed' } else { 'Failed' }
            if ($code -ne 0) { $failedProjects[$path] = $true }
        }
        $seconds = [Math]::Round(((Get-Date) - $started).TotalSeconds, 2)
        $results.Add([pscustomobject]@{ Project=$path; Kind=$kind; Status=$status; ExitCode=$code; Seconds=$seconds; Log=$log })
        $results | Export-Csv -LiteralPath (Join-Path $logRoot 'results.csv') -NoTypeInformation -Encoding UTF8
        Write-Host "[$index/$($ordered.Count)] $status $kind $name ($seconds s)"
        if ($status -eq 'Failed') {
            Get-Content -LiteralPath $log -Tail 12 | ForEach-Object { Write-Host $_ }
        }
        if ($FailFast -and $code -ne 0) { break }
    }
    $passedModules = @($results | Where-Object { $_.Kind -eq 'Module' -and $_.Status -eq 'Passed' }).Count
    $failures = @($results | Where-Object Status -eq 'Failed').Count
    $skipped = @($results | Where-Object Status -eq 'Skipped').Count
    Write-Host "Modules passed: $passedModules/$($modulePaths.Count). Failed projects: $failures. Skipped projects: $skipped."
    Write-Host "Build summary: $(Join-Path $logRoot 'results.csv')"
    if ($failedProjects.Count -gt 0) { exit 1 }
    exit 0
}
catch {
    [Console]::Error.WriteLine("Build failed: $($_.Exception.Message)")
    if ($logRoot) { [Console]::Error.WriteLine("Logs: $logRoot") }
    exit 1
}
