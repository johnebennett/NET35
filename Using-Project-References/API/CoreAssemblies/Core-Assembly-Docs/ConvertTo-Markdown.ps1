[CmdletBinding()]
param()
$ErrorActionPreference = 'Stop'
$pages = @{}
$anchors = @{}
$files = @(Get-ChildItem -LiteralPath $PSScriptRoot -Filter '*.html' -File)
foreach ($file in $files) {
    $html = [IO.File]::ReadAllText($file.FullName)
    $body = [regex]::Match($html, '(?s)<body>(.*)</body>')
    if (!$body.Success) { throw "Missing body: $($file.Name)" }
    [xml]$document = '<root>' + $body.Groups[1].Value + '</root>'
    $pages[$file.Name] = $document
    $anchors[$file.Name] = @{}
    foreach ($element in $document.SelectNodes('//*[@id]')) {
        $heading = $element.SelectSingleNode('./h2|./h3')
        if (!$heading) { throw "Missing anchor heading: $($element.id)" }
        $anchors[$file.Name][$element.id] = $heading.InnerText
    }
}

function Convert-Inline($node, [string]$page) {
    $parts = foreach ($child in $node.ChildNodes) {
        if ($child.NodeType -eq 'Text' -or $child.NodeType -eq 'Whitespace') {
            # Escape Markdown punctuation outside signatures and link destinations.
            [regex]::Replace($child.Value, '([\\`*_\[\]<>])', '\$1')
        } elseif ($child.Name -eq 'code') {
            if ($child.InnerText.Contains('`')) { throw 'Unexpected backtick in signature' }
            '`' + $child.InnerText + '`'
        } elseif ($child.Name -eq 'a') {
            $href = $child.GetAttribute('href')
            $label = $child.InnerText
            $segments = $href -split '#', 2
            $target = if ($segments[0]) { $segments[0] } else { $page }
            if ($pages.ContainsKey($target)) {
                $destination = if ($segments[0]) { [IO.Path]::GetFileNameWithoutExtension($target) } else { '' }
                if ($segments.Count -eq 2) {
                    if (!$anchors[$target].ContainsKey($segments[1])) { throw "Broken anchor: $page -> $href" }
                    $destination += '#' + $anchors[$target][$segments[1]]
                }
                '[[' + $destination + '|' + $label + ']]'
            } else {
                '[' + $label + '](' + $href.Replace(' ', '%20') + ')'
            }
        } else { throw "Unexpected inline element: $($child.Name)" }
    }
    $parts -join ''
}

function Convert-Blocks($node, [string]$page) {
    $parts = foreach ($child in $node.ChildNodes) {
        switch ($child.Name) {
            { $_ -match '^h[1-6]$' } {
                ('#' * [int]$child.Name.Substring(1)) + ' ' + $child.InnerText + "`n`n"
            }
            'nav' {
                $links = foreach ($link in $child.SelectNodes('./a')) {
                    $container = $child.OwnerDocument.CreateElement('p')
                    [void]$container.AppendChild($link.CloneNode($true))
                    Convert-Inline $container $page
                }
                '> ' + ($links -join (' ' + [char]0x00B7 + ' ')) + "`n`n"
            }
            'p' {
                $text = Convert-Inline $child $page
                if ($child.GetAttribute('class') -eq 'meta') { '*' + $text + "*`n`n" }
                else { $text + "`n`n" }
            }
            'ul' {
                foreach ($item in $child.SelectNodes('./li')) { '- ' + (Convert-Inline $item $page) + "`n" }
                "`n"
            }
            'section' { Convert-Blocks $child $page }
            'article' { "---`n`n"; Convert-Blocks $child $page }
            '#whitespace' { }
            default { throw "Unexpected block element: $($child.Name)" }
        }
    }
    $parts -join ''
}

# Convert and validate everything before writing any notes.
$outputs = @{}
foreach ($file in $files) {
    $document = $pages[$file.Name]
    $markdown = "---`ncssclasses:`n  - project-reference`n---`n`n" + (Convert-Blocks $document.DocumentElement $file.Name)
    $signatures = @($document.SelectNodes('//code') | ForEach-Object InnerText)
    $convertedSignatures = @([regex]::Matches($markdown, '(?m)^- `([^`]+)`\r?$') | ForEach-Object { $_.Groups[1].Value })
    if (($signatures -join "`n") -cne ($convertedSignatures -join "`n")) { throw "Signature mismatch: $($file.Name)" }
    foreach ($heading in $anchors[$file.Name].Values) {
        if ($markdown -notmatch ('(?m)^#{2,3} ' + [regex]::Escape($heading) + '\r?$')) { throw "Missing heading: $heading" }
    }
    $outputs[$file.BaseName + '.md'] = $markdown
    Write-Output "$($file.Name): $($signatures.Count) signatures; all anchor targets validated."
}
$existing = @($outputs.Keys | Where-Object { Test-Path -LiteralPath (Join-Path $PSScriptRoot $_) })
if ($existing.Count) {
    $backup = Join-Path $PSScriptRoot ('Backups/' + (Get-Date -Format 'yyyyMMdd-HHmmss-fff'))
    New-Item -ItemType Directory -Path $backup -Force | Out-Null
    foreach ($name in $existing) { Copy-Item -LiteralPath (Join-Path $PSScriptRoot $name) -Destination $backup }
}
foreach ($name in $outputs.Keys) {
    [IO.File]::WriteAllText((Join-Path $PSScriptRoot $name), $outputs[$name], (New-Object Text.UTF8Encoding($false)))
}
Write-Output "Created $($outputs.Count) Markdown notes. HTML files were not modified."
