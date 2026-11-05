[CmdletBinding()]
param(
    [ValidateSet('Debug','Release')][string]$Configuration = 'Release',
    [switch]$Clean,
    [string]$MSBuildPath
)
$ErrorActionPreference = 'Stop'
$root = [IO.Path]::GetFullPath($PSScriptRoot)
$projects = @('CommonUtilities','UsefulDBMethods','ProgressLogging','ActionBase','RequestProcessor')
if (!$MSBuildPath) {
    $command = Get-Command MSBuild.exe -ErrorAction SilentlyContinue
    if ($command) { $MSBuildPath = $command.Source }
    else {
        $vswhere = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio\Installer\vswhere.exe'
        if (Test-Path -LiteralPath $vswhere) {
            $MSBuildPath = & $vswhere -latest -products '*' -requires Microsoft.Component.MSBuild -find 'MSBuild\**\Bin\MSBuild.exe' | Select-Object -First 1
        }
    }
}
if (!$MSBuildPath -or !(Test-Path -LiteralPath $MSBuildPath)) { throw 'Install Visual Studio Build Tools with .NET Framework 3.5 reference assemblies, or supply -MSBuildPath.' }
$referenceRoot = Join-Path ${env:ProgramFiles(x86)} 'Reference Assemblies\Microsoft\Framework\v3.5'
if (!(Test-Path -LiteralPath $referenceRoot)) { throw '.NET Framework 3.5 reference assemblies is required.' }

# Validate all reference edges before building. No core DLL hint paths are allowed.
$edges = @{}
foreach ($name in $projects) {
    [xml]$project = Get-Content -LiteralPath (Join-Path $root "$name\$name.csproj") -Raw
    $ns = New-Object Xml.XmlNamespaceManager($project.NameTable); $ns.AddNamespace('m', $project.DocumentElement.NamespaceURI)
    $edges[$name] = @()
    foreach ($reference in $project.SelectNodes('//m:Reference', $ns)) {
        if ($projects -contains ($reference.Include.Split(',')[0])) { throw "Core DLL reference remains in $name : $($reference.Include)" }
    }
    foreach ($reference in $project.SelectNodes('//m:ProjectReference', $ns)) {
        $resolved = [IO.Path]::GetFullPath((Join-Path (Join-Path $root $name) $reference.Include))
        if (!$resolved.StartsWith($root + '\', [StringComparison]::OrdinalIgnoreCase) -or !(Test-Path -LiteralPath $resolved)) { throw "Invalid project reference in $name" }
        $edges[$name] += [IO.Path]::GetFileNameWithoutExtension($resolved)
    }
}
$visiting = @{}; $visited = @{}
function Visit([string]$name) {
    if ($visiting[$name]) { throw "Circular dependency at $name" }
    if ($visited[$name]) { return }
    if (!$edges.ContainsKey($name)) { throw "Unexpected project $name" }
    $visiting[$name] = $true
    foreach ($dependency in $edges[$name]) { Visit $dependency }
    $visiting.Remove($name); $visited[$name] = $true
}
foreach ($name in $projects) { Visit $name }

# Restore packages.config dependencies before building. Requires MSBuild 16.5+.
& $MSBuildPath (Join-Path $root 'GOAT.CoreAssemblies.sln') /nologo /t:Restore /p:RestorePackagesConfig=true "/p:RestoreConfigFile=$root\NuGet.Config"
if ($LASTEXITCODE -ne 0) { throw 'NuGet restore failed.' }
if ($Clean) {
    # Only these generated directories beneath this checkout may be removed.
    $generated = @('artifacts')
    foreach ($name in $projects) { $generated += "$name\bin"; $generated += "$name\obj" }
    $generated += 'Verification\bin'; $generated += 'Verification\obj'
    foreach ($relative in $generated) {
        $path = [IO.Path]::GetFullPath((Join-Path $root $relative))
        if (!$path.StartsWith($root + '\', [StringComparison]::OrdinalIgnoreCase)) { throw 'Clean path outside checkout' }
        if (Test-Path -LiteralPath $path) { Remove-Item -LiteralPath $path -Recurse -Force }
    }
}
$logRoot = Join-Path $root 'artifacts\logs'
New-Item -ItemType Directory -Path $logRoot -Force | Out-Null
$log = Join-Path $logRoot "build-$Configuration.log"
$binlog = Join-Path $logRoot "build-$Configuration.binlog"
& $MSBuildPath (Join-Path $root 'GOAT.CoreAssemblies.sln') /nologo /m:1 /nr:false /p:UseSharedCompilation=false /t:Build "/p:Configuration=$Configuration" '/p:Platform=Any CPU' /v:minimal /fl "/flp:logfile=$log;verbosity=normal" "/bl:$binlog"
if ($LASTEXITCODE -ne 0) { throw "Build failed ($LASTEXITCODE). See $log" }
$output = Join-Path $root "artifacts\$Configuration"
New-Item -ItemType Directory -Path $output -Force | Out-Null
foreach ($name in $projects) {
    foreach ($extension in 'dll','pdb') {
        $file = Join-Path $root "$name\bin\$Configuration\$name.$extension"
        if (Test-Path -LiteralPath $file) { Copy-Item -LiteralPath $file -Destination $output -Force }
    }
}
# Collect the dependencies actually resolved by MSBuild, including SQLite native DLLs.
foreach ($name in $projects) {
    Get-ChildItem -LiteralPath (Join-Path $root "$name\bin\$Configuration") -Filter '*.dll' -File | ForEach-Object { Copy-Item -LiteralPath $_.FullName -Destination $output -Force }
}
foreach ($arch in 'x86','x64') { Copy-Item -LiteralPath (Join-Path $root "ProgressLogging\bin\$Configuration\$arch") -Destination $output -Recurse -Force }
Get-ChildItem -LiteralPath $output -File -Recurse | Get-FileHash | Select-Object Path,Hash | ConvertTo-Json | Set-Content -LiteralPath (Join-Path $logRoot "assemblies-$Configuration.json") -Encoding UTF8
Write-Output "Build succeeded. Assemblies: $output"
