# Reconciliation evidence — 24 September 2026

## Method

1. Decompiled the supplied production executable with ILSpyCmd 9.1.0.7988, using C# 5 output and dependencies supplied in the source tree.
2. Built the actual supplied source into separate analysis directories. Used the Debug build for comparison because its checked arithmetic and retained locals more closely reflect the supplied production executable than the source's Release build.
3. Compared every decompiled C# file. Restored production changes into the original source, instead of replacing existing methods wholesale with decompiler output. Added recovered production-only classes/helpers and retained the bypass changes already in the source.
4. Built the reconciled Debug and Release configurations. Decompiled the rebuilt Debug executable and reviewed every remaining difference from production. The full comparison is `verification/production-vs-reconciled.diff`.

## Remaining differences reviewed

| Area | Explanation |
| --- | --- |
| `AccountDetails` | Requested bypass property, default/optional constructor argument, conditional credential validation and copying. |
| `RunIdProcessor.PopulateEmailSettings` | Requested database flag selection and NULL/boolean conversion. |
| `SendEmailParam.CopyAccountDetailsFrom` | Requested bypass propagation. |
| `SendEmail.Send` | Requested conditional authentication and associated authentication log. |
| `RunIdProcessor` fields | Two unused private source fields retained; declaration order differs. |
| Other method text differences | Compiler/decompiler local naming, local lifetime/reuse, equivalent object initializer lowering, integer-zero representation, exception string concatenation lowering, and method ordering. |
| Assembly metadata | Original source version wildcard retained; rebuilt version differs from production. Original metadata source/comments retained. |

No other application behavior difference was identified in this comparison. This is a source reconstruction and reviewed behavior comparison, not a claim of byte-identical output or exhaustive runtime equivalence.

## Results

- Debug and Release builds succeeded. Existing duplicate-using and unused-variable/field warnings remain.
- 17 isolated runtime checks passed against a copy of the supplied production executable.
- 24 isolated runtime checks passed against the reconciled Release executable, including the additional bypass validation/copy cases.
- Both executables produced the same three-line usage message when invoked without arguments.
- Confirmed by rebuilt-code inspection that SMTP authentication is conditional on `!AccountDetails.BypassAuthCheck`, while STARTTLS remains unconditional.
- Confirmed by rebuilt-code comparison that `RunningOnEmailServer`, configured directories/retry count, synchronization calls, reply-to handling, and independent send/update switches match production.

Logs and SHA-256 fingerprints are in `verification/`. Tests use local temporary fixture folders and do not send mail, connect to the database, or launch document generators. Production checks run against an isolated executable copy with the supplied dependency DLLs.

## Limits and deployment checks

Actual production configuration, database state/schema, dependency versions and external Word/TestRPO executables were not supplied as a complete deployment. End-to-end email delivery, Firebird queries and document generation therefore remain untested. Validate in staging with the real configuration/dependencies and both authenticated and bypass SMTP accounts before replacing production.

The source feature uses `Convert.ToBoolean` for database values, not a custom `Y`/`N` parser. The database column must return a compatible boolean/numeric value (or a boolean string); NULL means false. Production-only reply-to fields must also exist.

Analysis artifacts, the original decompilation, and the reconciliation helper scripts are in the sibling `reconciliation/` directory. They are not needed to build this source tree. `reconcile.py` records the initial reconstruction process and overwrites generated source if rerun; normal future maintenance should edit this tree directly.
