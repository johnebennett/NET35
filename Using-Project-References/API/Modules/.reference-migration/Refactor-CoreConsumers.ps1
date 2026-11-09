$ErrorActionPreference = 'Stop'
$root = Split-Path $PSScriptRoot -Parent
$backup = Join-Path $PSScriptRoot ('CoreConsumerBackups-' + (Get-Date -Format 'yyyyMMdd-HHmmss'))
$rows = @()
foreach ($file in (Get-ChildItem -LiteralPath $root -Recurse -Filter *.cs | Where-Object { $_.FullName -notlike "$PSScriptRoot\*" -and $_.FullName -notmatch '\\(obj|bin)\\' })) {
    $original = [IO.File]::ReadAllText($file.FullName)
    $updated = [regex]::Replace($original, '\bCommonUtilities\.(ReportUtilities|SystemReportUtilities|GeneralReportParameters|EAReportGetDefaultItem)\b', 'global::UsefulDBMethods.Reporting.$1')
    $updated = [regex]::Replace($updated, '(?<![\w.])(?:ReportUtilities|SystemReportUtilities)\b(?=\s*(?:[.(]|\w+\s*=))', 'global::UsefulDBMethods.Reporting.$0')
    $updated = [regex]::Replace($updated, '(?<![\w.])(?:CommonUtilities\.)?FolderHelpers\.(DocsLocation|ImportStorageLocation|TestRPOLocation|EAOfficeServiceLocation)\b', 'global::UsefulDBMethods.CompanyFolderSettings.$1')
    if ($file.FullName -eq (Join-Path $root 'Managements\Actions\DefaultMethods.cs')) {
        $updated = $updated.Replace('CommonDBMethods.Common.CommonComboEntries(out ErrorMsg)', 'CommonDBMethods.Common.CommonComboEntries(this.CommonItem.CompanyIdValue, this.CommonItem.ClientIdValue, CommonUtilities.StringUtilities.StrToIntDef((string)DataObject["type"], -1), out ErrorMsg)')
    }
    if ($updated -eq $original) { continue }
    $relative = $file.FullName.Substring($root.Length + 1)
    $destination = Join-Path $backup $relative
    New-Item -ItemType Directory -Path (Split-Path $destination -Parent) -Force | Out-Null
    Copy-Item -LiteralPath $file.FullName -Destination $destination
    $bytes = [IO.File]::ReadAllBytes($file.FullName)
    $withBom = $bytes.Length -ge 3 -and $bytes[0] -eq 239 -and $bytes[1] -eq 187 -and $bytes[2] -eq 191
    [IO.File]::WriteAllText($file.FullName, $updated, (New-Object Text.UTF8Encoding($withBom)))
    $rows += [pscustomobject]@{ File=$relative; Backup=$destination }
}
$rows | Export-Csv -LiteralPath (Join-Path $backup 'changes.csv') -NoTypeInformation
Write-Output "Updated $($rows.Count) source files; backup: $backup"
