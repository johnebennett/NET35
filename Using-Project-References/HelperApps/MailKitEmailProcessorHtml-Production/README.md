# MailKit_EmailProcessorHtml
Email Processor HTML that depends on MailKit and MimeKit
This tree starts with `MailKit_EmailProcessorHtml-main` and restores the behavior recovered from the supplied `EmailProcessorHtml.exe`, while retaining the source tree's `BYPASSAUTHCHECK` feature. Original files remain untouched. Open `MailKitEmailProcessor.sln` to work on this version.

The executable's internal assembly name is **MailKitEmailProcessor**, version **1.0.9140.16598**. Its supplied filename is a deployment rename. Production input SHA-256:

`CBC1C7B5ACE9BA5F7AF4842F70D8A08FB40E4F2329A25378AE99647DFE4B5721`

## Build

Use Visual Studio/MSBuild with the .NET Framework 4.5.2 targeting pack:

```powershell
msbuild MailKitEmailProcessor.sln /t:Build /p:Configuration=Release
```

Output: `bin/Release/MailKitEmailProcessor.exe`. The project retains the original assembly name, AnyCPU/prefer-32-bit settings, and version-generation convention. Checked arithmetic is enabled for both configurations to preserve overflow behavior seen in production. Core dependencies use project references into `../../API/CoreAssemblies`; third-party references use the shared `../../packages` directory and pinned versions in `packages.config`.

## Reconciled behavior

- `Actions/ConfigSettings.cs`: restores production JSON settings loading from `MailKitEmailProcessorSettings.json` beside the executable.
- `Actions/RunIdProcessor.cs`: restores configured local paths, retry count, `RunningOnEmailServer` processing, asset synchronization, the `SendEmail` and `UpdateSentValues` switches, reply-to database reads/writes and JSON fields, and production logging.
- `Actions/CommonMethods.cs`: restores shared report/document copying, company signature/logo/image copying, logging overloads, and the production stub methods.
- `Actions/CommonClasses.cs`: restores `SenderDetailsItem`, including reply-to copying and fallback. The account bypass property, constructor default, validation and copy behavior remain from the supplied source.
- `Actions/SendEmail.cs`: uses production reply-to handling and retains the source's conditional authentication. STARTTLS connection behavior is unchanged.
- `BYPASSAUTHCHECK` is selected from `EACompanySettings`. Database NULL maps to false; other values use the source's `Convert.ToBoolean` conversion. True permits blank username/password and skips `Authenticate`; host and port remain required. False retains authentication.

Original source comments, regions, meaningful local names, formatting, inactive examples and the uncompiled `FileNameProcessor - Copy.cs` are retained. Only production-only classes/helpers needed recovery from decompiled code. `Program.cs`, `Processor.cs`, `EmailItems.cs`, and `FileNameProcessor.cs` retain the supplied source implementation. Two unused private fields in `RunIdProcessor` are retained despite their absence in production.

## Production configuration and compatibility

Use the actual deployment's configuration and supporting runtime files. They cannot be recovered from the executable. `MailKitEmailProcessorSettings.example.json` documents the recovered property names and is deliberately not copied to the build output. The API/database configuration and any additional runtime dependencies must come from the deployment. The supplied DLLs allow compilation but have not been verified against the production DLL versions.

Flags are exact, case-sensitive `"Y"` comparisons. Production maps both Word and TestRPO execution paths to `LocalCreateDocsLocation`, even though separate Word/TestRPO properties exist. Preserve trailing directory separators where production concatenates paths.

Some unusual production behavior is intentionally retained:

- `CopyEmailTemplateToEmailServer` and `LoadAppSettings` are no-ops returning an empty string.
- Signature/logo/image synchronization runs regardless of `RunningOnEmailServer`; report/document copying is gated by it.
- Asset directories must already exist. Directory enumeration failures can escape; individual copy errors are logged.
- `SendEmail` and `UpdateSentValues` are independent switches. Disabling sending does not automatically disable updating sent status.
- Existing connection/account logging remains unchanged, including its existing handling of credential values.
- The file-based input path is preserved as supplied/compiled in production; no new JSON parsing for bypass or reply-to was invented.

The database must support the production reply-to columns and the source feature's `EACompanySettings.BYPASSAUTHCHECK` column. No database/schema changes are included.

If deploying under `EmailProcessorHtml.exe`, rename its matching `.exe.config` accordingly. The JSON settings filename remains `MailKitEmailProcessorSettings.json`. Nothing in this task deploys or replaces the supplied executable.

## Verification

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File tests/Verify-Reconciliation.ps1 -AssemblyPath bin/Release/MailKitEmailProcessor.exe -ExpectBypass
```

The checks exercise account validation/copying, sender and reply-to copying, configuration loading, and asset copying into isolated test directories. They do not connect to SMTP or the database. Test folders and logs remain beside the tested assembly for inspection.

See `RECONCILIATION.md` for comparison evidence and validation limitations.


## Reference migration

Backups of the modified project, solution and README are in `Backups\ReferenceMigration-20261110-140635`. The .NET Framework 4.5.2 target is preserved. Existing loose DLL versions were unavailable for comparison.

MimeKit 3.0.0 requires System.Buffers 4.5.1, recorded in packages.config. Its shared package has a net45 reference assembly but no net45 runtime assembly. Its net461 runtime requires a newer framework; the netstandard1.1 runtime requires additional support packages absent from the shared directory. No incompatible runtime or reference-only DLL is copied into the output. This deployment dependency remains unresolved.

Validation: all project/package HintPaths exist. The Release solution build compiled all four referenced core projects, then failed with CS0246 in Actions/RunIdProcessor.cs:60 because ConfigSettings is absent from this checkout. The README's tests/Verify-Reconciliation.ps1 is also absent. Application compilation and runtime compatibility could not be verified.
