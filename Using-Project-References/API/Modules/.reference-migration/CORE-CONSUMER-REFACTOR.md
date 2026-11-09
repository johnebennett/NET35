# Module consumers of the refactored core assemblies

## Changes

Updated 84 C# source files to use the current core APIs, based on the five markdown API references in `API/CoreAssemblies/Core-Assembly-Docs`, `REFACTORING.md`, and the core sources.

- Reporting calls and declarations now use `UsefulDBMethods.Reporting.ReportUtilities` and `UsefulDBMethods.Reporting.SystemReportUtilities`.
- Database folder lookups now use `UsefulDBMethods.CompanyFolderSettings`. General assembly-directory lookups remain on `CommonUtilities.FolderHelpers`.
- Managements now supplies the company ID, client ID, and request `type` to `CommonDBMethods.Common.CommonComboEntries`, matching the documented signature and the existing Portfolio consumer. An absent or invalid type uses the same -1 default as Portfolio.
- Fully qualified `global::UsefulDBMethods` names avoid ambiguity with module namespaces.

Resolved the additional dependency failures exposed by compilation in 11 project files:

- Converted seven module assembly references to project references for ComboBoxEntries, ContactOperations, Portfolio, and SettingsOperations. The validation script builds modules in dependency order and checks for cycles.
- Redirected eight third-party references to the existing CLR 2/.NET 3.5-compatible Otp.NET, BCrypt.Net-Next, and QrCodeGenerator DLLs, copied under `Modules/ThirdParty`. `ThirdParty/manifest.json` records their original locations, assembly identities, CLR versions, and SHA-256 hashes. No dependency was downloaded or substituted with a new version.

The core sources, target frameworks/profiles, package references, and existing RunTime post-build destinations were preserved. Logging initialization belongs to the application host: as described in the core refactoring guide, the host must call one `EAOfficeAPI.CoreRuntimeInitialization.InitializeLogging(...)` startup path before actions/workers run. This change does not initialize global logging separately in each module.

## Backups

- `CoreConsumerBackups-20261105-185512`: originals of all 84 modified C# source files, with paths relative to Modules and a `changes.csv` index.
- `DependencyBackups-20261105-185630`: originals of all 11 modified project files, with paths relative to Modules and a `changes.csv` index.
- The scripts, report, and ThirdParty files added for this task have no previous versions to back up.

Use these two backup directories when reviewing this task. Earlier reference and post-build backup folders belong to the earlier migrations.

## Validation and repeatable builds

Final result: **70/70 module projects rebuilt successfully in Debug and 70/70 in Release**, against freshly compiled core assemblies in each configuration.

All five core assemblies were compiled directly from their current sources in Debug and Release. Their validation outputs and intermediate files are isolated under `.reference-migration/CoreBuild`; their source projects were not modified. Each module is rebuilt against those core outputs with post-build copying disabled and its validation output redirected to its own `bin/<Configuration>` folder. Original project OutputPath values are not changed.

From Modules, run:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File .\.reference-migration\Build-CoreSources.ps1 -Configuration Debug
powershell -NoProfile -ExecutionPolicy Bypass -File .\.reference-migration\Build-Modules.ps1 -Label source-verified -Configuration Debug
powershell -NoProfile -ExecutionPolicy Bypass -File .\.reference-migration\Build-CoreSources.ps1 -Configuration Release
powershell -NoProfile -ExecutionPolicy Bypass -File .\.reference-migration\Build-Modules.ps1 -Label source-verified -Configuration Release
```

The `Build-source-verified-Debug` and `Build-source-verified-Release` directories contain one log per project and `results.csv`. Core build logs are in `CoreBuild/logs-Debug` and `CoreBuild/logs-Release`. Earlier baseline/intermediate logs are diagnostic history; use the source-verified logs for final results.

Validation also checked all 70 unchanged target framework versions and profiles, all 350 project reference paths/GUIDs (343 core and seven module references), all 81 DLL reference paths, original source backups, and preserved RunTime post-build events. No references to the moved CommonUtilities reporting classes or database folder methods remain in active module C# source.

Compilation does not exercise live Firebird queries, reporting executables, authentication flows, logging configuration, or runtime deployment. Existing compiler warnings and warnings from MSBuild traversing core project metadata remain visible in the logs.
