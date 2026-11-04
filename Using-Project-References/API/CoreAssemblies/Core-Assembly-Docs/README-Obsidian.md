# Project reference notes in Obsidian

Copy these six notes together into one folder in your vault, then open **index.md**:

- [Combined index](index.md)
- [ActionBase](ActionBase.md)
- [CommonUtilities](CommonUtilities.md)
- [ProgressLogging](ProgressLogging.md)
- [RequestProcessor](RequestProcessor.md)
- [UsefulDBMethods](UsefulDBMethods.md)

The notes preserve the HTML heading hierarchy, namespace and class indexes, navigation, metadata, method visibility groups, constructor groups, and every signature. Internal page and anchor links use Obsidian wikilinks to the corresponding notes and headings. See [Obsidian's internal link documentation](https://obsidian.md/help/links).

Use Reading view for the closest presentation to the HTML. Markdown uses separators between classes instead of HTML article cards. Your Obsidian theme controls the default appearance.

## Optional styling

Copy **project-reference.css** into your vault's **.obsidian/snippets** folder, then enable **project-reference** under **Settings → Appearance → CSS snippets**. Each generated note has a `cssclasses` property that scopes these styles to the reference notes. The snippet provides wider content, similar spacing, heading borders, a navigation panel, and wrapping for long signatures. Light mode uses the HTML's text and link colors; dark mode retains your theme's colors. No community plugin is required.

## Source file links

The original relative C# source links are preserved, such as `../../ActionBase/CommonDBMethods.cs`. They work only when those source files are available at the same relative locations and Obsidian permits opening that file type. Copying only the six notes does not copy the C# files. All links between the reference notes work independently of those source files.

## Regenerate

From this folder, run:

```cmd
powershell.exe -NoProfile -ExecutionPolicy Bypass -File ConvertTo-Markdown.ps1
```

The converter reads the existing six HTML pages without modifying them, checks internal anchor targets and signature preservation, and writes UTF-8 Markdown. Existing generated notes are backed up before replacement. The HTML remains the source for regeneration; manual edits to generated notes will be replaced on the next run.
