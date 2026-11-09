# Building all Modules projects

`Build.ps1` discovers all active C#, Visual Basic, and F# projects beneath Modules and builds them in project-reference dependency order. It also builds their referenced projects, including the five core assemblies, before their consumers. There are currently 70 module projects and five core dependency projects. Backups, `.reference-migration`, generated `bin`/`obj`/`artifacts` directories, package folders, and directory links are excluded from discovery. Missing projects and reference cycles are reported before building.

`Build.cmd` launches the PowerShell script from a standard Windows Command Prompt. It forwards all arguments and returns the script's exit code. Both launchers locate files relative to their own location and work when invoked from another directory. The CMD launcher uses a process-scoped execution-policy bypass; it does not change the machine's PowerShell policy.

## Prerequisites

- Windows PowerShell 5.1.
- Visual Studio or Visual Studio Build Tools with MSBuild and .NET Framework 3.5 reference assemblies. MSBuild is found on PATH, then through Visual Studio's `vswhere.exe`; alternatively supply `-MSBuildPath`.
- The sibling `API/CoreAssemblies` sources, existing shared `packages` directory, and module `ThirdParty` DLLs. This script does not download or restore dependencies. Restore missing packages using the repository's dependency-restoration workflow before building.
- Keep `Build.Outputs.targets` next to the script. It controls output locations for this invocation without modifying project files or target frameworks.

## Examples

From a standard Command Prompt in Modules:

```bat
Build.cmd
Build.cmd -Configuration Debug
Build.cmd -Configuration Release -Rebuild
Build.cmd -ListProjects
Build.cmd -Configuration Release -RunPostBuildEvents
Build.cmd -MSBuildPath "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe"
```

From PowerShell:

```powershell
.\Build.ps1 -Configuration Debug -Rebuild
```

If local execution policy blocks scripts, use the CMD launcher or:

```powershell
powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\Build.ps1 -Configuration Release
```

From any other directory, supply the full path to the launcher, for example:

```bat
"E:\Source\GitHub\NET35\Using-Project-References\API\Modules\Build.cmd" -Configuration Debug
```

## Optional parameters

| Parameter | Default | Purpose |
| --- | --- | --- |
| `-Configuration Debug\|Release` | `Release` | Select the build configuration. |
| `-Platform AnyCPU\|x86\|x64` | `AnyCPU` | Forward the platform to MSBuild. Every selected project must support that platform; the current coordinated build is verified with AnyCPU. |
| `-Rebuild` | Off | Use MSBuild's Rebuild target instead of incremental Build. Cleans and rebuilds the selected configuration's generated outputs; does not delete source or backup folders. |
| `-MSBuildPath "full path"` | Auto-detected | Use a particular MSBuild.exe. |
| `-RunPostBuildEvents` | Off | Allow existing project post-build events to execute. The 14 existing module copy events write their output DLLs to the sibling RunTime folder. This does not deploy every module or all dependencies. The destination must exist and be writable. |
| `-FailFast` | Off | Stop at the first failed build. Normally independent projects continue and projects depending on a failure are skipped. |
| `-ListProjects` | Off | Print the dependency-ordered project list and exit without invoking MSBuild, creating logs, or building anything. Does not require MSBuild to be installed. |

## Outputs, logs, and exit codes

- Module DLLs: `<module project directory>\bin\<Configuration>\<Platform>\`.
- Core DLLs and dependencies: `Modules\artifacts\core\<project>\bin\<Configuration>\<Platform>\`.
- Core intermediate files: `Modules\artifacts\core\<project>\obj\<Configuration>\<Platform>\`.
- Module intermediate files retain their project-defined locations.
- Each run has its own `Modules\artifacts\logs\<timestamp>-<Configuration>\` directory, with numbered project logs and `results.csv` containing status, exit code, duration, and log path. The console reports progress and the last lines of failed builds. Compiler warnings remain visible in project logs.
- Exit code `0`: all discovered modules and their project dependencies succeeded (or `-ListProjects` succeeded).
- Exit code `1`: discovery, validation, tool detection, or compilation failed. With `-FailFast`, projects after the failure are not attempted; the CSV contains only attempted/skipped entries up to that point.

Default builds keep outputs local and suppress post-build events. Project references are traversed by the script, and each project is built once, with one MSBuild node and shared compilation disabled. No project source, target framework, framework profile, stored OutputPath, or stored RunTime copy command is edited. This build helper is independent of the earlier `.reference-migration` validation scripts.

Compilation does not test live database connections, report executables, authentication flows, or host logging initialization. Run the application-specific integration checks separately.

## Verification

Verified using `Build.cmd -Rebuild` with the default Release/AnyCPU settings: all 70 modules and five core dependencies passed. Also verified project listing from another working directory and a nonzero exit code when MSBuild cannot be found. Post-build deployment was disabled during verification.
