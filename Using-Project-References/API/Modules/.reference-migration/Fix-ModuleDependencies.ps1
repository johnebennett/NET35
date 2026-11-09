$ErrorActionPreference = 'Stop'
$root = Split-Path $PSScriptRoot -Parent
$backup = Join-Path $PSScriptRoot ('DependencyBackups-' + (Get-Date -Format 'yyyyMMdd-HHmmss'))
$thirdParty = Join-Path $root 'ThirdParty'
New-Item -ItemType Directory -Path $thirdParty -Force | Out-Null
$sources = @{
    'Otp.NET' = Join-Path $root '..\CoreAssemblies\artifacts\Release\Otp.NET.dll'
    'BCrypt.Net-Next' = Join-Path $root '..\CoreAssemblies\artifacts\Release\BCrypt.Net-Next.dll'
    'QrCodeGenerator' = Join-Path $root '..\..\..\..\NET35-Identified-Projects-Source\SettingsOperations.TwoFADlg\11-EAOffice_EAOfficeAPI_Modules_SettingsOperations_2FADlg-5f8437eec0\Source\EAOffice\CommonAPIReferences\QrCodeGenerator.dll'
}
$manifest = foreach ($name in ($sources.Keys | Sort-Object)) {
    $source = (Resolve-Path -LiteralPath $sources[$name]).Path
    $destination = Join-Path $thirdParty ($name + '.dll')
    if (Test-Path -LiteralPath $destination) { throw "Refusing to overwrite $destination" }
    Copy-Item -LiteralPath $source -Destination $destination
    $assembly = [Reflection.Assembly]::ReflectionOnlyLoadFrom($destination)
    if ($assembly.ImageRuntimeVersion -ne 'v2.0.50727') { throw "Dependency is not built for CLR 2: $name" }
    [pscustomobject]@{ File=($name + '.dll'); Assembly=$assembly.FullName; Runtime=$assembly.ImageRuntimeVersion; Source=$source; SHA256=(Get-FileHash -LiteralPath $destination -Algorithm SHA256).Hash }
}
$manifest | ConvertTo-Json | Set-Content -LiteralPath (Join-Path $thirdParty 'manifest.json') -Encoding UTF8
$modules = @{
    'ComboBoxEntries' = Join-Path $root 'ComboBoxEntries\ComboBoxEntries.csproj'
    'ContactOperations' = Join-Path $root 'ContactOperations\ContactOperations.csproj'
    'Portfolio' = Join-Path $root 'Portfolio\Portfolio.csproj'
    'SettingsOperations' = Join-Path $root 'SettingsOperations\SettingsOperations.csproj'
}
function RelativePath($directory, $target) {
    [Uri]::UnescapeDataString(([Uri]($directory + '\')).MakeRelativeUri([Uri]$target).ToString()).Replace('/', '\')
}
$changes = @()
foreach ($file in (Get-ChildItem -LiteralPath $root -Recurse -Filter *.csproj | Where-Object { $_.FullName -notlike "$PSScriptRoot\*" })) {
    $original = [IO.File]::ReadAllText($file.FullName)
    $newline = if ($original.Contains("`r`n")) { "`r`n" } else { "`n" }
    $updated = [regex]::Replace($original, '(?ms)^(?<indent>[ \t]*)<Reference\b[^>]*\bInclude="(?<include>[^"]+)"[^>]*?(?:/>|>.*?</Reference>)', {
        param($match)
        $name = ($match.Groups['include'].Value -split ',')[0]
        $indent = $match.Groups['indent'].Value
        if ($modules.ContainsKey($name)) {
            $path = RelativePath $file.DirectoryName $modules[$name]
            [xml]$target = [IO.File]::ReadAllText($modules[$name])
            $guid = $target.SelectSingleNode('//*[local-name()="ProjectGuid"]').InnerText
            return "$indent<ProjectReference Include=`"$path`">$newline$indent  <Project>$guid</Project>$newline$indent  <Name>$name</Name>$newline$indent</ProjectReference>"
        }
        if ($sources.ContainsKey($name)) {
            $path = RelativePath $file.DirectoryName (Join-Path $thirdParty ($name + '.dll'))
            return "$indent<Reference Include=`"$name`">$newline$indent  <HintPath>$path</HintPath>$newline$indent  <Private>True</Private>$newline$indent</Reference>"
        }
        return $match.Value
    })
    if ($updated -eq $original) { continue }
    [xml]$validate = $updated
    $relative = $file.FullName.Substring($root.Length + 1)
    $destination = Join-Path $backup $relative
    New-Item -ItemType Directory -Path (Split-Path $destination -Parent) -Force | Out-Null
    Copy-Item -LiteralPath $file.FullName -Destination $destination
    $bytes = [IO.File]::ReadAllBytes($file.FullName)
    $withBom = $bytes.Length -ge 3 -and $bytes[0] -eq 239 -and $bytes[1] -eq 187 -and $bytes[2] -eq 191
    [IO.File]::WriteAllText($file.FullName, $updated, (New-Object Text.UTF8Encoding($withBom)))
    $changes += [pscustomobject]@{ Project=$relative; Backup=$destination }
}
$changes | Export-Csv -LiteralPath (Join-Path $backup 'changes.csv') -NoTypeInformation
Write-Output "Fixed dependencies in $($changes.Count) projects; backup: $backup"
