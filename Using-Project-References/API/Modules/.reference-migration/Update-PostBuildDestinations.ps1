$ErrorActionPreference = 'Stop'
$root = Split-Path $PSScriptRoot -Parent
$runtime = [IO.Path]::GetFullPath((Join-Path $root '..\..\RunTime'))
$backup = Join-Path $PSScriptRoot ('PostBuildBackups-' + (Get-Date -Format 'yyyyMMdd-HHmmss'))
$changes = @()
foreach ($file in (Get-ChildItem -LiteralPath $root -Recurse -File | Where-Object { $_.Extension -in '.csproj','.vbproj','.fsproj','.vcxproj' -and $_.FullName -notlike "$PSScriptRoot\*" })) {
    $original = [IO.File]::ReadAllText($file.FullName)
    [xml]$before = $original
    $relativeRuntime = [Uri]::UnescapeDataString(([Uri]($file.DirectoryName + '\')).MakeRelativeUri([Uri]($runtime + '\')).ToString()).Replace('/', '\')
    $destination = '"$(ProjectDir)' + $relativeRuntime + '"'
    $updated = [regex]::Replace($original, '(?s)<PostBuildEvent>(.*?)</PostBuildEvent>', {
        param($eventMatch)
        $body = [regex]::Replace($eventMatch.Groups[1].Value, '(?im)^(?<prefix>\s*copy\s+\S+\s+)(?<destination>[^\r\n]+?)\s*$', {
            param($copyMatch)
            if ($copyMatch.Groups['destination'].Value -notmatch '^([.][.]\\)+CommonAPIReferences\\$') { throw "Unexpected copy destination in $($file.FullName)" }
            $copyMatch.Groups['prefix'].Value + $destination
        })
        '<PostBuildEvent>' + $body + '</PostBuildEvent>'
    })
    if ($updated -eq $original) { continue }
    [xml]$after = $updated
    $beforeEvents = @($before.SelectNodes('//*[local-name()="PostBuildEvent"]') | ForEach-Object { $_.InnerText })
    $afterEvents = @($after.SelectNodes('//*[local-name()="PostBuildEvent"]') | ForEach-Object { $_.InnerText })
    foreach ($node in $before.SelectNodes('//*[local-name()="PostBuildEvent"]')) { $node.InnerText = '' }
    foreach ($node in $after.SelectNodes('//*[local-name()="PostBuildEvent"]')) { $node.InnerText = '' }
    if ($before.OuterXml -cne $after.OuterXml) { throw "Unexpected change outside post-build event: $($file.FullName)" }
    $resolved = [IO.Path]::GetFullPath((Join-Path $file.DirectoryName $relativeRuntime))
    if ($resolved.TrimEnd('\') -ne $runtime.TrimEnd('\')) { throw "Incorrect runtime destination: $resolved" }
    $relative = $file.FullName.Substring($root.Length + 1)
    $backupPath = Join-Path $backup $relative
    New-Item -ItemType Directory -Path (Split-Path $backupPath -Parent) -Force | Out-Null
    Copy-Item -LiteralPath $file.FullName -Destination $backupPath
    $bytes = [IO.File]::ReadAllBytes($file.FullName)
    $withBom = $bytes.Length -ge 3 -and $bytes[0] -eq 239 -and $bytes[1] -eq 187 -and $bytes[2] -eq 191
    [IO.File]::WriteAllText($file.FullName, $updated, (New-Object Text.UTF8Encoding($withBom)))
    $changes += [pscustomobject]@{ Project=$relative; Before=($beforeEvents -join "`n"); After=($afterEvents -join "`n"); ResolvedDestination=$resolved }
}
if ($changes.Count -gt 0) { $changes | Export-Csv -LiteralPath (Join-Path $backup 'changes.csv') -NoTypeInformation }
Write-Output "Updated and validated $($changes.Count) projects. Destination: $runtime"
Write-Output "Backup: $backup"
$changes | Select-Object Project,After | Format-Table -AutoSize
