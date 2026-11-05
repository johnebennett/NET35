# NuGet dependencies

All five core projects use packages.config and remain on .NET Framework 3.5.
References between GOAT projects remain ProjectReference entries.

| Package | Pinned version | Used by | Assembly asset |
| --- | --- | --- | --- |
| Newtonsoft.Json | 6.0.1 | All five projects | lib/net35 |
| FirebirdSql.Data.FirebirdClient | 3.1.0.0 | CommonUtilities | lib/net35 |
| System.Data.SQLite.Core | 1.0.119.0 | ProgressLogging | Dependency on the stub package below |
| Stub.System.Data.SQLite.Core.NetFramework | 1.0.119.0 | ProgressLogging | lib/net20 (compatible with .NET 3.5) |

The selected DLLs match the original ThirdParty DLL assembly identities and file
versions: Newtonsoft.Json 6.0.0.0 / 6.0.1.17001, Firebird 3.1.0.0 / 3.1.0.0,
and SQLite 1.0.119.0 / 1.0.119.0. Package folder names retain the versions declared
in their package metadata, including four-part versions where applicable.

SQLite's imported package targets supply the x86 and x64 SQLite.Interop.dll files.
ContentSQLiteInteropFiles is enabled so those files also propagate to projects
referencing ProgressLogging. Build.ps1 collects the resolved dependencies and both
native architecture directories into artifacts/Release (or artifacts/Debug).

## Building after cloning

Install Visual Studio Build Tools with MSBuild 16.5 or newer and .NET Framework
3.5 reference assemblies. From this folder run:

```powershell
.\Build.ps1
```

The script restores packages before building. It no longer requires the sibling
ThirdParty folder or accepts its previous -ThirdPartyPath parameter.
For restore alone, use:

```powershell
MSBuild.exe .\GOAT.CoreAssemblies.sln /t:Restore /p:RestorePackagesConfig=true
```

Alternatively, with nuget.exe installed:

```powershell
nuget restore .\GOAT.CoreAssemblies.sln -ConfigFile .\NuGet.Config
```

NuGet.Config fixes the package location at ../../packages, beside API, so
opening an individual project's solution uses the same package paths. Commit the
project files, packages.config files, both NuGet.Config files, both .gitignore files, Build.ps1 and these notes.
The downloaded packages and temporary .nuget-tools directory are ignored by Git.
Restore retrieves pinned versions; upgrades require deliberate dependency changes.

## Validation and backups

Original copies of the five modified project files and Build.ps1 are in
Backups/NuGetMigration-20261105-174308, preserving their relative paths.
New files have no prior versions to back up. Original ThirdParty DLLs were not changed.

The Release solution build succeeded with the package references and SQLite native
files were checked in the output. The existing Test-Refactor.ps1 cannot run in this
checkout because its Verification project is absent.

NuGet audit reports NU1903 for Newtonsoft.Json 6.0.1:
https://github.com/advisories/GHSA-5crp-9r3c-p9vr
This version is retained to match the original DLL, rather than silently upgrading
the dependency. Audit remains enabled. A later security upgrade should be tested
separately.

## Shared package directory

The shared root NuGet.Config (beside API) uses repositoryPath=packages. Other
packages.config projects beneath that root inherit this setting unless they override
it. CoreAssemblies/NuGet.Config uses ../../packages for the same location, including
when Build.ps1 explicitly selects that configuration. PackageReference projects
use NuGet's global cache instead; this setting applies to packages.config.

Backups for this relocation: Backups\SharedPackages-20261105-174939 (including SharedRoot.gitignore).
