$ErrorActionPreference = 'Stop'
$root = Split-Path $PSScriptRoot -Parent
$backup = Join-Path $PSScriptRoot ('Backups-' + (Get-Date -Format 'yyyyMMdd-HHmmss'))
$coreRoot = [IO.Path]::GetFullPath((Join-Path $root '..\CoreAssemblies'))
$packageRoot = [IO.Path]::GetFullPath((Join-Path $root '..\..\packages'))
$utf8 = New-Object Text.UTF8Encoding($false)
function RelativePath($directory, $target) {
    [Uri]::UnescapeDataString(([Uri]($directory.TrimEnd('\') + '\')).MakeRelativeUri([Uri]$target).ToString()).Replace('/', '\')
}
$cores = @{}
foreach ($name in 'ActionBase','CommonUtilities','ProgressLogging','RequestProcessor','UsefulDBMethods') {
    $path = Join-Path $coreRoot "$name\$name.csproj"
    [xml]$xml = [IO.File]::ReadAllText($path)
    $cores[$name] = @{ Path = $path; Guid = [string]($xml.SelectSingleNode('//*[local-name()="ProjectGuid"]').InnerText) }
}
$packages = @{
    'Newtonsoft.Json' = @{ Id='Newtonsoft.Json'; Version='6.0.1'; Dll='Newtonsoft.Json.6.0.1\lib\net35\Newtonsoft.Json.dll' }
    'FirebirdSql.Data.FirebirdClient' = @{ Id='FirebirdSql.Data.FirebirdClient'; Version='3.1.0.0'; Dll='FirebirdSql.Data.FirebirdClient.3.1.0.0\lib\net35\FirebirdSql.Data.FirebirdClient.dll' }
    'System.Data.SQLite' = @{ Id='Stub.System.Data.SQLite.Core.NetFramework'; Version='1.0.119.0'; Dll='Stub.System.Data.SQLite.Core.NetFramework.1.0.119.0\lib\net20\System.Data.SQLite.dll' }
}
$files = @(Get-ChildItem -LiteralPath $root -Recurse -Filter *.csproj | Where-Object { $_.FullName -notlike "$PSScriptRoot\*" })
$report = @()
foreach ($file in $files) {
    $relative = RelativePath $root $file.FullName
    $original = [IO.File]::ReadAllText($file.FullName)
    $newline = if ($original.Contains("`r`n")) { "`r`n" } else { "`n" }
    [xml]$before = $original
    $framework = [string]($before.Project.PropertyGroup.TargetFrameworkVersion | Where-Object { $_ } | Select-Object -First 1)
    $state = @{ Core=0; Package=0; Used=@{} }
    $updated = [regex]::Replace($original, '(?ms)^(?<indent>[ \t]*)<Reference\b[^>]*\bInclude="(?<include>[^"]+)"[^>]*?(?:/>|>.*?</Reference>)', {
        param($match)
        $name = ($match.Groups['include'].Value -split ',')[0]
        $indent = $match.Groups['indent'].Value
        if ($cores.ContainsKey($name)) {
            $state.Core++
            $path = RelativePath $file.DirectoryName $cores[$name].Path
            $extra = [regex]::Match($match.Value, '<Reference\b(?<attrs>[^>]*)>').Groups['attrs'].Value
            $condition = [regex]::Match($extra, '\sCondition="[^"]*"').Value
            $metadata = [regex]::Matches($match.Value, '<(Private|Aliases|EmbedInteropTypes)>.*?</\1>') | ForEach-Object { "$indent  $($_.Value)$newline" }
            return "$indent<ProjectReference Include=`"$path`"$condition>$newline$indent  <Project>$($cores[$name].Guid)</Project>$newline$indent  <Name>$name</Name>$newline$($metadata -join '')$indent</ProjectReference>"
        }
        if ($packages.ContainsKey($name)) {
            $state.Package++
            $state.Used[$name] = $true
            $dll = Join-Path $packageRoot $packages[$name].Dll
            if (!(Test-Path -LiteralPath $dll)) { throw "Missing package DLL: $dll" }
            $path = RelativePath $file.DirectoryName $dll
            return "$indent<Reference Include=`"$name`">$newline$indent  <HintPath>$path</HintPath>$newline$indent  <Private>True</Private>$newline$indent</Reference>"
        }
        return $match.Value
    })
    if ($state.Package -gt 0) {
        $configPath = Join-Path $file.DirectoryName 'packages.config'
        if (Test-Path -LiteralPath $configPath) { throw "Existing packages.config requires merge: $configPath" }
        $entries = foreach ($name in ($state.Used.Keys | Sort-Object)) {
            $p = $packages[$name]
            "  <package id=`"$($p.Id)`" version=`"$($p.Version)`" targetFramework=`"$($framework.Replace('v','net').Replace('.',''))`" />"
            if ($name -eq 'System.Data.SQLite') { '  <package id="System.Data.SQLite.Core" version="1.0.119.0" targetFramework="net35" />' }
        }
        $config = '<?xml version="1.0" encoding="utf-8"?>' + $newline + '<packages>' + $newline + ($entries -join $newline) + $newline + '</packages>' + $newline
        $updated = [regex]::Replace($updated, '</Project>\s*\z', "  <ItemGroup>$newline    <None Include=`"packages.config`" />$newline  </ItemGroup>$newline</Project>")
        if ($state.Used.ContainsKey('System.Data.SQLite')) {
            $target = RelativePath $file.DirectoryName (Join-Path $packageRoot 'Stub.System.Data.SQLite.Core.NetFramework.1.0.119.0\build\net20\Stub.System.Data.SQLite.Core.NetFramework.targets')
            if (!(Test-Path -LiteralPath (Join-Path $file.DirectoryName $target))) { throw 'SQLite targets missing' }
            $addition = @"
  <PropertyGroup>
    <ContentSQLiteInteropFiles>true</ContentSQLiteInteropFiles>
  </PropertyGroup>
  <Import Project="$target" Condition="Exists('$target')" />
  <Target Name="EnsureNuGetPackageBuildImports" BeforeTargets="PrepareForBuild">
    <Error Condition="!Exists('$target')" Text="NuGet packages are missing. Restore packages before building." />
  </Target>
"@
            $updated = [regex]::Replace($updated, '</Project>\s*\z', ($addition -replace '\r?\n', $newline) + $newline + '</Project>')
        }
    }
    if ($updated -ne $original) {
        [xml]$after = $updated
        if (($before.SelectNodes('//*[local-name()="TargetFrameworkVersion"]').OuterXml -join '|') -ne ($after.SelectNodes('//*[local-name()="TargetFrameworkVersion"]').OuterXml -join '|')) { throw "Framework changed: $relative" }
        $destination = Join-Path $backup $relative
        New-Item -ItemType Directory -Path (Split-Path $destination -Parent) -Force | Out-Null
        Copy-Item -LiteralPath $file.FullName -Destination $destination
        [IO.File]::WriteAllText($file.FullName, $updated, $utf8)
        if ($state.Package -gt 0) { [IO.File]::WriteAllText($configPath, $config, $utf8) }
        $report += [pscustomobject]@{ Project=$relative; CoreReferences=$state.Core; PackageReferences=$state.Package; Framework=$framework }
    }
}
$report | Export-Csv -LiteralPath (Join-Path $backup 'changes.csv') -NoTypeInformation
Write-Output "Backup: $backup"
$report | Format-Table -AutoSize
Write-Output "Updated $($report.Count) projects; core references: $(($report | Measure-Object CoreReferences -Sum).Sum); package references: $(($report | Measure-Object PackageReferences -Sum).Sum)"
