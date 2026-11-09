# Module reference migration

The complete original-file backup is `Backups-20261105-183536`. Paths inside this folder mirror the Modules folder. Its `changes.csv` records all 70 changed projects and reference counts. Earlier backup folders were created during conversion checks; use the complete backup identified here for review.

Changes:
- Replaced 343 assembly references to ActionBase, CommonUtilities, ProgressLogging, RequestProcessor, and UsefulDBMethods with references to their projects under API/CoreAssemblies.
- Redirected 73 package assembly references to the shared packages folder: Newtonsoft.Json 6.0.1 (net35), FirebirdSql.Data.FirebirdClient 3.1.0.0 (net35), and System.Data.SQLite 1.0.119.0 (net20, matching the core projects).
- Added packages.config to each of the 70 projects. These are new files, with no previous file to back up.
- Added the SQLite package build import and native-library content setting to UserOperations, matching the core ProgressLogging project.
- Preserved target framework versions, source files, and unrelated assembly references.

Validation checked all 70 project XML files, all 343 project reference paths and GUIDs, package DLL paths, package configuration inclusion, and unchanged target framework declarations. No legacy references to the five core assemblies remain in active module projects.

A sample Debug build of LogsOperations succeeded with BuildProjectReferences=false and its post-build copy disabled. It reported source warnings and MSB3202 warnings while traversing references inside the existing core projects; the build log is sample-build.log. This verifies a sample module against available core outputs, not a full rebuild of every module and core dependency.

Update-References.ps1 records the conversion procedure; it is intended for the original projects and will refuse to overwrite existing packages.config files. Do not rerun it against the migrated tree.
