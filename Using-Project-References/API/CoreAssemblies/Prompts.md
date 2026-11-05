# Prompt 1
```
This folder contains 5 projects that form the core assemblies of the GOAT system of software.
I would like to move the files CommonDBMethods.cs and RequestCommon.cs from the CommonSourceFiles folder directly into the ActionBase project. Currently these files are included into the ActionBase project but do not appear to be used in any of the other projects.
Can you identify any reason they should not be added to the ActionBase project?
Can you examine the other projects in this folder and determine if any of the projects are using any of the files in the CommonSourceFiles folder. Note, the projects originally came from a different folder structure to this. The paths in the projects to the other projects or to the files in the CommonSourceFiles folder will not be correct. It is the folder names that should be used to determine the files, not the path to the folder.
Append the results to the file Prompts.md.
```
## Examination results — 22 October 2026

The two files can be moved into ActionBase without changing which assembly owns their types. ActionBase already compiles them as linked source files. However, CommonDBMethods.cs is used by two other projects through their references to ActionBase.dll; it is not exclusive to ActionBase at the type/API level.

This review examined all five .csproj files and their C# source, matching folder and file names rather than attempting to resolve the obsolete relative paths. References to another assembly were distinguished from inclusion of its source, and commented-out code was excluded from active usage.

### Project usage

| Project | Source included from CommonSourceFiles | Active use of types defined in that folder |
| --- | --- | --- |
| ActionBase | CommonDBMethods.cs and RequestCommon.cs only (ActionBase.csproj:63–68). | Uses CommonDBMethods.Common.ConnectionString (ActionBase.cs:382, 1113) and RequestCommonItem / RequestCommonProcessor (for example :38, 131–134, 359). Compiles its own ActionBase.cs and APISettingsConfig.cs. Uses ProgressLogging and its enums through ProgressLogging.dll. |
| CommonUtilities | None. | WindowsEventLoggingUtilities.cs:142 calls CommonDBMethods.Common.ConnectionString(). CommonUtilities.csproj:56–59 references ActionBase.dll, which currently supplies that type. |
| ProgressLogging | None. | ProgressLogging.cs:248 and :989 call CommonDBMethods.Common.ConnectionString(). ProgressLogging.csproj:36–38 references ActionBase.dll. Compiles its own ProgressLogging.cs, including GeneralResponseStatus and JsonContentEntryType. |
| RequestProcessor | None. | No direct active references found to the types declared in CommonSourceFiles. It references CommonUtilities.dll and dynamically loads plugin assemblies; those can have further dependencies outside this review. Its parsing of the JSON "common" element does not use RequestCommonItem or RequestCommonProcessor. |
| UsefulDBMethods | None. | No direct active references found to the types declared in CommonSourceFiles. The ActionBase call in EventHelpers.cs:850 is commented out. It does reference CommonUtilities.dll, so indirect dependencies remain. |

The inter-assembly references above are DLL references with HintPath entries, not ProjectReference entries. None of the other four projects compiles any file from CommonSourceFiles, regardless of the old path prefix.

### Inventory of CommonSourceFiles

| File | Finding |
| --- | --- |
| CommonDBMethods.cs | Compiled only by ActionBase; its public Common class is also consumed by CommonUtilities and ProgressLogging as described above. |
| RequestCommon.cs | Compiled only by ActionBase; no active consumers found in the other four projects. Defines EAOfficeAPI.RequestCommonItem and EAOfficeAPI.RequestCommonProcessor. |
| ActionBase.cs | Not included by any project from this folder. ActionBase compiles its own, different version. Do not overwrite the project version with this copy. |
| APISettingsConfig.cs | Not included by any project from this folder. ActionBase compiles its own copy, which is byte-for-byte identical (SHA-256 comparison). CommonDBMethods uses APISettingsItem, APISettingsFile and APIAppType from that project copy. |
| ProgressLogging.cs | Not included by any project from this folder. ProgressLogging compiles its own, different version. ActionBase consumes that assembly's logging types. Do not overwrite the project version with this copy. |
| JsonResponseContent.cs | Not included by any project. The references in the project versions of ActionBase.cs:1435 and ProgressLogging.cs:647 are inside block comments. No active consumer found in these five projects. |
| SqliteLogging.cs | Not included by any project. The reference in ActionBase.cs:1101 is inside a block comment; the active method delegates to ProgressLogging.BuildSqliteConnectionString. No active consumer found in these five projects. |
| 547_20.rtf | Not included by any of the five project files. |

### Recommendation and existing constraints

Move CommonDBMethods.cs and RequestCommon.cs into the ActionBase directory and replace their linked Compile entries with ordinary local entries:

```xml
<Compile Include="CommonDBMethods.cs" />
<Compile Include="RequestCommon.cs" />
```

Remove the corresponding Link metadata and obsolete external Compile paths. Preserve the existing namespaces and public type names: CommonDBMethods.Common, EAOfficeAPI.RequestCommonItem and EAOfficeAPI.RequestCommonProcessor. A physical source move then preserves their assembly identity and the API used by other projects. Do not also compile duplicate copies into the consuming projects.

There is no source-location reason in these five projects to prevent this move. The following existing issues matter when making the whole folder build, but are not introduced or fixed by moving the files:

- ActionBase depends on CommonUtilities and ProgressLogging, and both depend back on ActionBase. CommonUtilities and UsefulDBMethods also reference one another. Converting the current DLL references directly to project references would therefore expose circular build dependencies. CommonDBMethods contributes to these cycles through its use of CommonUtilities and API settings in ActionBase. Breaking the cycles would require a separate refactor, such as moving configuration/contracts into a lower-level assembly and passing connection settings into logging/utilities.
- ActionBase targets .NET Framework 4.8.1 (ActionBase.csproj:11); the other four projects target 3.5. The CommonUtilities and ProgressLogging source dependencies on ActionBase therefore require framework alignment for a consistent rebuild. Moving source within ActionBase does not resolve this mismatch.
- Projects outside this folder may still link the original shared files. This review cannot establish whether removing the original paths would affect those external projects; their references would need updating if they exist.

Validation was static inspection of project entries, source references, comment boundaries and duplicate-file hashes. No build was attempted because the supplied project paths/references are known to need relocation, and this task concerns dependency analysis. Only this report was appended; no source files were moved and no project files were changed.

# Prompt 2
```
Acknowledging there are circular references in the projects can you build a document Remove-Circular-Dependencies-Plan.md with suggestions of the best way to refactor these projects to remove the circular references. Assume that all other projects that depend on these assemblies will also need to be refactored to allow for these changes.
Make the refactor as minimal as it could be but also try to keep like things together such that they make sense to someone working on the refactoring of the other projects that these assemblies are referenced in. Do not modify any source code at this time.
Append any useful output that does not belong in the plan document to the file Prompts.md.
```

### Prompt 2 output — 22 October 2026

Created Remove-Circular-Dependencies-Plan.md with a proposed five-assembly dependency graph, file/type ownership changes, startup wiring, downstream migration mappings, implementation order and acceptance criteria. The recommendation uses narrow logging callbacks/providers rather than introducing a new configuration assembly. It retains API settings, CommonDBMethods and request types in ActionBase.

Additional inspection confirmed that the reverse CommonUtilities → UsefulDBMethods dependency is not confined to logging: ReportUtilities, SystemReportUtilities and four database-backed FolderHelpers methods also depend on it. Conversely, the apparent ActionBase call in UsefulDBMethods/EventHelpers.cs and the Utilities.FirebirdUtilities configuration calls in CommonUtilities/FirebirdUtilities.cs are commented-out code, not additional active dependencies.

Only this log and the new plan document were edited for this request. No source/project files were changed and no build was run. External consumers were assumed to be available for migration, but were not inspected because they are outside the supplied five-project scope.

# Prompt 3
```
Copy the existing source to the folder ..\NEW-Refactored.
Once the files have been copied and using the content of the file Remove-Circular-Dependencies-Plan.md copy modify the source in ..\NEW-Refactored to implement the changes in the plan.
Make a backup of any files changed for review.
If possible, compile the projects to ensure they can be compiled.
Create any scripts necessary that will help with the building of clean assemblies.
Append any output to the file Prompts.md
```


### Prompt 3 output - 23 October 2026

Copied the existing NEW folder into E:\Source\GitHub\NET35-CoreAssemblies\NEW-Refactored, then implemented Remove-Circular-Dependencies-Plan.md in that copy. Original source/project files in NEW were not modified. Filesystem approval was obtained for the sibling checkout outside the writable workspace.

Implemented changes:

- Kept all five assemblies and replaced the old core DLL HintPath references with an acyclic ProjectReference graph. Added GOAT.CoreAssemblies.sln and removed obsolete post-build copies and external output paths.
- Moved ReportUtilities, SystemReportUtilities and their DTOs to UsefulDBMethods/Reporting with namespace UsefulDBMethods.Reporting. Moved the four database folder lookups to UsefulDBMethods.CompanyFolderSettings, retaining GetAssemblyDirectoryName in CommonUtilities.FolderHelpers.
- Extracted the EVENT_VIEWER database writer into UsefulDBMethods.EventViewerLogging. CommonUtilities retains the marker gate beside CommonUtilities.dll and invokes a startup-configured primitive-only delegate, with error handling and a per-thread recursion guard.
- Replaced the two implicit Firebird connection lookups in ProgressLogging with a startup-configured Func<string>. Missing/empty/throwing providers return errors through the existing result convention. Added CoreRuntimeInitialization in ActionBase to wire both logging hooks without constructing an action or resolving settings early.
- Moved CommonDBMethods.cs and RequestCommon.cs into ActionBase as ordinary local source files. Their namespaces, public names and owning assembly remain unchanged. Other unused CommonSourceFiles copies were retained and are not compiled.
- Aligned all projects to .NET Framework 4.8.1, matching the existing ActionBase target. Downstream applications previously targeting 3.5 must be migrated and rebuilt.
- Collected existing local Newtonsoft.Json (assembly version 6.0.0.0), FirebirdSql.Data.FirebirdClient (3.1.0.0), and System.Data.SQLite (1.0.119.0) with x86/x64 native interop into ThirdParty. manifest.json records source paths, file versions and SHA-256 hashes. No packages were downloaded and no prebuilt core DLLs were used to satisfy project dependencies.

Review/build files in NEW-Refactored:

- Backups/Before-Refactor preserves originals of changed/removed files at their original relative paths. Backups/Source-Changes.md and Source-Changes.json list changed source/project paths and original/refactored hashes. All modified/removed original source/project paths were checked against their backup hashes.
- Build.ps1 validates the project graph and dependency hashes, cleans only generated paths in this checkout, builds the root solution, and collects assemblies under artifacts/Release. Text/binary build logs and output hashes are under artifacts/logs.
- Test-Refactor.ps1 and Verification contain focused executable regression checks. REFACTORING.md documents build commands, startup initialization, consumer namespace/reference changes and integration limits.

Commands run from NEW-Refactored through the approved refactor runner:

```powershell
.\Build.ps1 -Configuration Release -Clean
.\Test-Refactor.ps1
```

Build result: all five projects compiled successfully, 0 errors, 1 existing CS0219 warning in ProgressLogging/ProgressLogging.cs:238 (MethodName assigned but never used). Assemblies are in artifacts/Release; the full log is artifacts/logs/build-Release.log and the MSBuild binary log is build-Release.binlog.

Verification output:

```text
PASS graph
PASS unconfigured
PASS empty
PASS throwing
PASS sink
PASS concurrency
PASS bootstrap
PASS sqlite
All 8 verification scenarios passed.
```

These checks cover compiled core reference edges and type ownership, missing logging initialization, disabled AddRecord behavior, both implicit Firebird APIs with empty/throwing providers, marker gating, recursive sink suppression and guard recovery, concurrent writers, startup composition and a real in-memory SQLite query. Tests may emit diagnostics through the existing Windows Event Log fallback. Full verification output is in artifacts/logs/verification.log.

No configured application Firebird database or downstream application sources were supplied in this folder. Real report queries, EVENT_VIEWER inserts, Firebird progress persistence, and rebuilt external host/plugin execution remain integration checks for the consuming applications. Each relevant executable must initialize logging before workers or plugin dispatch; new delegates cannot replace configured application-wide providers during requests. Rebuild and deploy the coordinated new assembly set rather than mixing it with old binaries.

