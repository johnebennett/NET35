# Authoring and using AGENTS.md

Yes: shared dependency locations and reference policies belong in a root AGENTS.md. They are durable facts that Codex needs across projects. The AGENTS.md beside this guide records your core-project and shared-package policies without authorising an automatic migration of the entire tree.

## What to include

Use short, concrete instructions that help Codex make correct decisions:

| Topic | Useful content for this workspace |
| --- | --- |
| Purpose | Stabilise the legacy source while replacing appropriate DLL references. |
| Layout | Where core projects, modules, services, helper apps, and shared packages live. |
| Dependency policy | First-party core assemblies use project references; third-party dependencies use compatible package assets. |
| Compatibility | Target frameworks, public contracts, runtime requirements, and pinned package versions. |
| Build and checks | Verified commands, working directory, prerequisites, and known limitations. |
| Change boundaries | Preserve legacy format and behaviour; keep unrelated refactoring separate. |
| Exceptions | A particular project's special native dependencies or build requirements. |
| Supporting documents | Relative links to detailed setup, architecture, or dependency notes. |

Explain the reason when it prevents a plausible mistake. For example, locally adapted net35 packages need their provenance preserved because an upstream package with a similar name may not be equivalent.

Avoid secrets, credentials, copied source code, large inventories that quickly become stale, and long session transcripts. Put task-specific goals in the task prompt or a separate plan. Keep completed work and build evidence in normal project documentation. An instruction file should describe what to do; a historical report should describe what happened.

## Discovery and hierarchy

At startup, Codex loads global guidance from CODEX_HOME (normally ~/.codex), then project guidance from the detected root down to the working directory. Without a detected root, only the working directory is checked. At each level, AGENTS.override.md takes precedence over AGENTS.md; configured fallback names follow. At most one file is selected per directory. Deeper guidance overrides conflicting earlier guidance; compatible instructions accumulate. Empty files are skipped. Project guidance defaults to a combined 32 KiB limit. Restart after changing instructions to ensure rediscovery. See the [official discovery guide](https://learn.chatgpt.com/docs/agent-configuration/agents-md).

For this workspace, an example chain when starting inside ActionBase is:

```text
Codex home/AGENTS.md                         personal defaults, if present
NET35/AGENTS.md                              Git-root guidance, if present
NET35/Using-Project-References/AGENTS.md      shared workspace policy
  API/AGENTS.md                             optional API-specific rules
  API/CoreAssemblies/AGENTS.md              optional core-specific rules
  API/CoreAssemblies/ActionBase/AGENTS.md   optional project-specific rules
```

These are candidate locations, not files you must create. Intermediate folders need no instruction file. Sibling guidance does not belong to this chain. Startup discovery does not recursively load every descendant; when working from the workspace root, have Codex inspect applicable nested files before editing a project. The new root file includes that instruction.

## Does every project need one?

No. Start with the root file. Add a nested file only when a subtree has additional requirements. For example, use API/CoreAssemblies/AGENTS.md for shared core build details, or a module-level file for a special build/deployment requirement. A project with no special instructions inherits the shared policy.

A useful optional project file could look like this, after replacing the placeholders with verified facts:

```markdown
# Project-specific guidance

## Purpose
Describe this project's responsibility and its consumers.

## Build and validation
Record the exact command, working directory, and prerequisites.

## Compatibility and dependencies
Record any extra constraints or explicitly explain an exception to root policy.
```

Do not create empty files throughout the tree or repeat the root reference policy in every project. Update the shared policy once and keep exceptions close to their affected code.

## How far upwards, and can a file stop the search?

The detected project root bounds project discovery, not the drive root. Here, inspection found NET35/.git one level above this workspace; no ancestor AGENTS files were found during this task. An AGENTS.md cannot declare itself a discovery boundary, and AGENTS.override.md replaces only the file at its own level. Global guidance remains separate. See [official discovery behaviour](https://learn.chatgpt.com/docs/agent-configuration/agents-md).

Codex exposes project_root_markers in its [configuration reference](https://learn.chatgpt.com/docs/config-file/config-reference). To establish a different project boundary, configure root detection using an intentional marker; this is configuration, not text inside AGENTS.md. Do not create a nested Git repository merely to manipulate instructions. This task has not changed root detection, Git layout, or your Codex configuration.

For your current arrangement, retaining the parent Git root and putting shared policy in this workspace is sufficient. If you later need an independent boundary, configure it deliberately and verify the loaded sources in a fresh session by asking Codex to list them. Instruction files also do not override system/developer requirements, sandbox permissions, or explicit task instructions.

## Suggested maintenance workflow

1. Add a project under this workspace and inspect its framework, references, and build entry point.
2. Apply the shared root policy; add a nested file only for enduring differences.
3. Record verified commands and dependency exceptions as they are discovered.
4. Ask a fresh Codex session to list applicable instruction sources when checking discovery.
5. Remove obsolete guidance when project structure or tooling changes.

Keep AGENTS.md concise so the operational rules are easy to find. Keep this explanatory guide separate from automatically loaded instructions.

Documentation checked against official OpenAI guidance on 9 November 2026. Discovery behaviour can depend on the Codex client/version and its configuration.
