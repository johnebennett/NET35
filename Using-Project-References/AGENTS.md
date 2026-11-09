# Workspace guidance

## Purpose and scope

This folder contains the projects being streamlined and stabilised for the legacy GOAT / EAOffice API, modules, services, and helper applications. These instructions apply to this folder and its descendants, including projects added later. Read any applicable nested AGENTS.md or AGENTS.override.md before changing files in that subtree.

Keep changes focused on the requested project and its necessary dependencies. The dependency policy below guides work on a project; it is not a request to migrate every project whenever a task starts.

## Compatibility and existing behaviour

- Preserve each project's target framework, assembly identity, public contracts, and runtime behaviour unless the task explicitly changes them. The five core projects target .NET Framework 3.5; verify other projects individually.
- Preserve the existing legacy project format and packages.config approach unless migration is requested. Do not automatically convert to SDK-style projects or PackageReference.
- Keep source close to its original assembly-reference counterpart where practical. Do not bring additional logging from the .NET 10 version into these legacy assemblies.
- Avoid unrelated formatting, dependency upgrades, and broad refactoring. Preserve existing user changes.

## First-party dependencies: use project references

API/CoreAssemblies is the authoritative source for these shared assemblies:

| Assembly | Project path relative to this folder |
| --- | --- |
| ActionBase | API/CoreAssemblies/ActionBase/ActionBase.csproj |
| CommonUtilities | API/CoreAssemblies/CommonUtilities/CommonUtilities.csproj |
| ProgressLogging | API/CoreAssemblies/ProgressLogging/ProgressLogging.csproj |
| RequestProcessor | API/CoreAssemblies/RequestProcessor/RequestProcessor.csproj |
| UsefulDBMethods | API/CoreAssemblies/UsefulDBMethods/UsefulDBMethods.csproj |

When working on a project that references one of these DLLs, replace its assembly Reference with a ProjectReference to the corresponding source project where compatible and required by the task. Use a path relative to the consuming project, with the actual project GUID and name required by its project format. Remove the superseded DLL reference so both forms do not coexist.

Check framework compatibility and the dependency graph before changing references. Do not introduce circular project references or resolve cycles by restoring DLL references. Report any cycle or incompatible target that prevents the intended conversion. Update affected solution entries/configurations when needed for a consistent build. Do not copy core source files into consumers as a substitute for project references.

## Third-party dependencies: use the shared package assets

- The root packages directory contains shared NuGet package contents. Use the appropriate compatible assembly from these packages instead of loose third-party DLL references from legacy folders, runtime copies, or machine-specific locations.
- Package DLLs remain assembly Reference entries with relative HintPath values into packages; they are not ProjectReference entries unless actual third-party source projects are explicitly introduced.
- Match the package ID, pinned version, assembly identity, and compatible framework asset. For .NET 3.5, prefer net35; use an older compatible asset only after checking compatibility. Never select a newer framework asset simply because its version is higher.
- Keep packages.config, HintPath values, package imports/targets, and required native/content assets consistent. Preserve Copy Local behaviour and runtime deployment requirements, including SQLite x86/x64 interop files where applicable.
- Respect the root NuGet.Config and any applicable nested NuGet.Config. Do not create separate package directories per project or hardcode absolute paths.
- Some shared packages are locally adapted builds (for example versions containing localnet35). Do not replace these with upstream packages or assume nuget.org can restore them. Inspect their local provenance and applicable documentation before changing them.
- If the required compatible package is missing or the mapping is uncertain, report the dependency and evidence rather than silently upgrading or substituting an unrelated assembly. Do not modify package binaries as part of ordinary reference cleanup.

## Validation and reporting

Inspect the affected project/solution and existing build documentation before choosing commands. Read API/CoreAssemblies/NUGET-MIGRATION.md for core package setup and native dependency requirements; inspect current scripts/project files if older notes disagree with them.

For core changes, the existing entry point is API/CoreAssemblies/Build.ps1 (run from that directory). Inspect its parameters and side effects before use. Use the affected project's documented build entry point for other projects. Validate reference paths and build the changed project plus relevant consumers when feasible. Run existing relevant tests when available; do not assume a named verification script has all its required projects present.

Report the references changed, validation performed, remaining unresolved dependencies, and any environment limitation preventing a build. Do not claim runtime equivalence from compilation alone.

## Maintaining instructions

Keep shared policy here. Add nested instruction files only for lasting subtree-specific facts, build commands, or exceptions, and avoid duplicating this file. Keep temporary task plans and progress logs in separate documents. See AGENTS-GUIDE.md for authoring and discovery guidance.
