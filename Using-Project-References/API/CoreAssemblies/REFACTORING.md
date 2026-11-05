# Refactored core assemblies

This copy implements Remove-Circular-Dependencies-Plan.md. The original NEW source remains unchanged apart from its requested Prompts.md log. Changed or removed copied files are preserved by their original relative paths in Backups/Before-Refactor. New files have no pre-refactor version. The old CommonSourceFiles copies not involved in the move are retained for review and are not compiled.

## Build

Requires Visual Studio/MSBuild and the .NET Framework 3.5 reference assemblies. All five core assemblies and the verification executable target .NET Framework 3.5; deploy with .NET Framework 3.5 SP1. The circular-reference refactor is preserved. Files changed during retargeting and previous generated outputs are backed up under Backups/Before-Net35-* without altering Backups/Before-Refactor.

```powershell
.\Build.ps1 -Configuration Release -Clean
.\Test-Refactor.ps1
```

From Command Prompt, use the launchers when local PowerShell execution policy disables scripts:

```bat
Build.cmd -Configuration Release -Clean
Test-Refactor.cmd
```

The launchers forward arguments and exit codes and locate their scripts relative to the launcher, so they also work from other directories. They use Windows PowerShell with a process-scoped execution-policy bypass; no permanent policy change is made. PowerShell must be installed and permitted to run. Group Policy, AppLocker, or Windows Defender Application Control restrictions may still prevent execution.

Build.ps1 validates the project graph and ThirdParty hashes, cleans only generated directories in this checkout, builds GOAT.CoreAssemblies.sln, and collects the five newly built core DLLs plus dependencies under artifacts/Release. Logs and binary logs are in artifacts/logs. No restore/network operation or prebuilt core DLL is required. ThirdParty/manifest.json records the origin, version and SHA-256 of the existing local dependencies selected for this copy. The SQLite x86 and x64 native interop DLLs are included. Keep dependency license/distribution obligations from the original application when redistributing these binaries.

The old per-project solutions remain for comparison; use the new root solution for the coordinated build. Old post-build copy commands and output paths into CommonAPIReferences have been removed.

## Consumer migration

Reference CommonUtilities -> no other core assembly; UsefulDBMethods -> CommonUtilities; ProgressLogging -> CommonUtilities and UsefulDBMethods; ActionBase -> all three; RequestProcessor -> CommonUtilities. Arrows here describe dependencies, not build sequence. The solution builds the graph in dependency order.

Moved reporting classes and DTOs now live in UsefulDBMethods.Reporting. The four database folder lookups now live in UsefulDBMethods.CompanyFolderSettings. GetAssemblyDirectoryName remains in CommonUtilities.FolderHelpers. Update references, using directives and public signatures in consuming projects. No upward wrappers are retained.

APISettingsConfig, CommonDBMethods and RequestCommon types remain in ActionBase with their existing names/namespaces. CommonDBMethods.cs and RequestCommon.cs are ordinary local ActionBase source files, no longer linked from CommonSourceFiles.

Before starting workers, constructing actions or dispatching plugins, API hosts should call exactly one startup path:

```csharp
EAOfficeAPI.CoreRuntimeInitialization.InitializeLogging(); // resolves service settings on demand
// OR InitializeLogging(settings); // uses the supplied APISettingsItem
// OR InitializeLogging(connectionStringProvider); // Func<string>
```

Initialization is application-wide. Repeating initialization with the same delegate is allowed; attempting to replace a configured provider/writer throws. Repeating the APISettingsItem overload creates a new delegate, so call that overload only once. Do not mix direct hook configuration with the ActionBase helper. Do not switch a global provider per request or database. Explicit connection APIs remain the appropriate path for independent databases.

Applications without ActionBase can configure ProgressLogging.ConfigureConnectionStringProvider directly and CommonUtilities.WindowsEventLoggingUtilities.ConfigureEventRecordWriter with a delegate that calls UsefulDBMethods.EventViewerLogging.WriteRecord using their own connection provider. Only primitive arguments cross the event callback boundary. Provider exceptions and empty connections become error results; disabled AddRecord paths do not require a provider. SQLite APIs do not require a Firebird provider.

EVENT_VIEWER logging retains the Event_Viewer.txt gate beside CommonUtilities.dll. If the file exists but no callback was registered, the call returns a configuration error and uses the independent Windows Event Log error path. Reentrant information logging bypasses database writes through a thread-local guard and uses the independent error path. Windows Event Log access can require permissions; failures continue to use the existing error-string convention.

## Verification scope

Verification explicitly checks that it is running on CLR 2.0, preventing CLR 4 execution from masking .NET 3.5 compatibility issues. The build scripts disable shared compilation, and the core build uses one MSBuild node to avoid compiler-server and worker-process stalls in restricted environments.

Test-Refactor.ps1 runs isolated processes for assembly ownership/reference edges, missing configuration, disabled AddRecord, empty/throwing providers through both implicit Firebird APIs, event marker gating, recursive sink behavior and guard recovery after exceptions, concurrent writers, startup composition, and an in-memory SQLite query. The sink tests may write diagnostic errors to the Windows Application Event Log using the existing fallback. Marker files are confined to Verification/bin and removed after each scenario.

Real Firebird report queries, EVENT_VIEWER inserts and progress record persistence require an application database and configuration and are not simulated as successful database integrations. Rebuild and exercise downstream hosts/plugins against the coordinated new assembly set before deployment. The plan document describes those additional integration checks.

## .NET 3.5 retarget validation

Release and Debug builds succeeded for all five assemblies. The eight verification scenarios passed on CLR 2.0, including the in-memory SQLite query. All five Release assemblies report runtime v2.0.50727 and reference mscorlib 2.0.0.0. The existing CS0219 warning in ProgressLogging.cs (unused MethodName) remains. Real Firebird/database integration and downstream hosts still require application-specific validation. Modules were not changed.
