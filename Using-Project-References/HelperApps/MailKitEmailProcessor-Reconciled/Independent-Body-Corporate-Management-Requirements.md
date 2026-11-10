# Independent Body Corporate Management (IBCM): GOAT SMTP requirements

Assessment date: 13 October 2026.

Scope: interpretation of `Handoff for the Vendor GOAT SMTP.pdf`, impact on this source tree, and proposed implementation and acceptance requirements. This assessment does not implement, deploy, contact IBCM, request tokens, or send email.

## 1. Main conclusion

**The existing `BYPASSAUTHCHECK` feature is insufficient for this client. IBCM requires authenticated SMTP using OAuth 2.0 client credentials and XOAUTH2.**

“No mailbox password” means that the application authenticates using an application secret to obtain a short-lived token, then uses that token to authenticate to SMTP. It does not mean that SMTP authentication can be skipped. A mailbox username is still required: `goat@ibcm.com.au`.

The existing sender has two behaviours: authenticate with a username/password, or omit authentication entirely. Neither implements the handoff. Enabling bypass would omit the required XOAUTH2 login; disabling bypass and entering the client secret as the SMTP password would use the wrong authentication mechanism. Neither is a supported solution.

This requires a scoped application enhancement plus client configuration and live acceptance testing. The SMTP host, port and STARTTLS behaviour already fit the handoff, and the bundled MailKit library already contains its XOAUTH2 mechanism. The existing message composition can largely be retained.

## 2. What the handoff means

The PDF describes a setup that IBCM says it configured on 13 October 2026. It explicitly says no message has yet been sent through that setup. These are reported tenant settings, not independently verified facts about the live tenant.

| Handoff item | Meaning for this application |
| --- | --- |
| Shared mailbox `goat@ibcm.com.au` | The permitted SMTP login and sending identity. The PDF describes it as unlicensed and without a mailbox password. |
| Entra app registration “GOAT - Vendor SMTP” | A separate identity for the unattended application. |
| Tenant ID and client ID | Identify the client's directory and app when requesting tokens. They are configuration identifiers, not passwords. |
| Client secret | An application credential, supplied separately through a secure channel. It has an expiry date. It must never be used as an SMTP password. |
| OAuth client credentials grant | The service obtains tokens without a person signing in. No browser login, user consent interaction, or refresh token is part of this handoff. |
| `SMTP.SendAsApp` application role | The token is expected to contain this role. The handoff requires stopping and reporting if it is missing. |
| Exchange service principal, FullAccess and SendAs grants | IBCM reports mailbox permissions that permit the app to use `goat@`. A token alone does not establish access to every mailbox. |
| SMTP AUTH enabled on `goat@` | IBCM reports a mailbox exception while the tenant default is disabled. OAuth still uses the SMTP AUTH protocol. |
| XOAUTH2 | The SMTP authentication mechanism that presents the mailbox address and bearer token. |
| STARTTLS on port 587 | The SMTP connection must upgrade to TLS before authentication. |
| Reply rejection rule | Mail addressed to `goat@`, including replies and delivery-failure notices, is rejected by design. |

IBCM reports that only `goat@` has been granted access. Its statement that other mailbox logins would be refused is explicitly untested in the PDF. Do not broaden permissions or assume other sending addresses will work.

### Exact client configuration

The following values come from PDF pages 1–3. The complete token endpoint below joins the line wrapped in the PDF.

| Setting | Required value |
| --- | --- |
| Tenant ID | `0299386a-a135-4d14-9d36-b571b6e3a637` |
| Client ID | `da247c5e-5598-4a2d-9391-7277f282a828` |
| Token endpoint | `https://login.microsoftonline.com/0299386a-a135-4d14-9d36-b571b6e3a637/oauth2/v2.0/token` |
| Token request content type | `application/x-www-form-urlencoded` |
| Grant type | `client_credentials` |
| Scope | `https://outlook.office365.com/.default` |
| Client secret | Securely supplied separately; store as a secret reference, not in this document |
| SMTP server | `smtp.office365.com` |
| SMTP port | `587` |
| Encryption | Required STARTTLS |
| SMTP authentication | OAuth 2.0 / `AUTH XOAUTH2` |
| SMTP username | `goat@ibcm.com.au` |
| From address | `goat@ibcm.com.au` only |
| Reply-To | An IBCM-approved monitored address if replies are required; confirm whether `office@ibcm.com.au` is appropriate |

### Sending sequence

1. Resolve the client's application secret from secure storage.
2. POST the client ID, secret, scope and grant type to the tenant-specific token endpoint. Properly form-encode the values.
3. Expect HTTP 200 with a bearer access token and `expires_in`. For the handoff test, inspect claims locally and confirm `roles` includes `SMTP.SendAsApp`. Never paste the token into an online decoder.
4. Connect to the SMTP server and complete STARTTLS.
5. Authenticate with XOAUTH2 using `goat@ibcm.com.au` and the access token. Expect SMTP `235` authentication success.
6. Submit the existing MIME message with the permitted From address and actual external recipients.
7. Record SMTP acceptance and verify that the external mailbox receives the message.

Cache and reuse valid tokens across sends; the PDF's explanatory “every send” sequence is conceptual and does not override its explicit caching instruction on page 4. Obtain a new token shortly before expiry using the client credentials grant again. There is no refresh token in this flow.

Microsoft's [SMTP OAuth documentation](https://learn.microsoft.com/en-us/exchange/client-developer/legacy-protocols/how-to-authenticate-an-imap-pop-smtp-application-by-using-oauth) confirms the client credentials/XOAUTH2 integration and SMTP-specific scope. Microsoft's [client credentials documentation](https://learn.microsoft.com/en-us/entra/identity-platform/v2-oauth2-client-creds-grant-flow) describes the token request and absence of refresh tokens.

## 3. Current project impact

The following findings come from reading the current source and inspecting the local MailKit assembly. They describe this checkout; the actual installed client executable and database still need verification.

| Component | Current behaviour and impact |
| --- | --- |
| `Program.cs` | A positive numeric argument uses `RunIdProcessor`; `/FromFile:` uses `FileNameProcessor`. Confirm which entry point the client will run. |
| `Actions/CommonClasses.cs`: `AccountDetails` | Contains host, port, username, password and bypass flag. No OAuth mode, tenant, client ID or secret reference. Validation requires a password unless bypass is true. Copy paths will need to preserve new settings. |
| `Actions/RunIdProcessor.cs`: `PopulateEmailSettings` | Reads `CO_EMAILHOST`, `I_CO_EMAILPORT`, `CO_EMAILUSERNAME`, `CO_EMAILPASSWORD`, and `BYPASSAUTHCHECK` from company-specific `EACompanySettings`. No OAuth configuration is loaded. Database NULL bypass maps to false. |
| `Actions/SendEmail.cs` | Always uses `SecureSocketOptions.StartTls`, then calls username/password `Authenticate` unless bypass is true. Add an explicit OAuth authentication branch and token acquisition integration. |
| `lib/MailKit.dll` | Local assembly reports version `3.0.0.0`; reflection confirms `MailKit.Security.SaslMechanismOAuth2` and its `(string, string)` constructor. The core SMTP OAuth mechanism is already available. This does not prove the deployed DLL has the same version. |
| `Actions/RunIdProcessor.cs`: `BuildEmailAddressItems` | Loads From and Reply-To from `EventRunShowClients`. A blank Reply-To falls back to From. Existing per-message sender values may conflict with the required fixed `goat@` identity. |
| `Actions/SendEmail.cs`: MIME construction | Already sets From and Reply-To and builds recipients, HTML, images and attachments. Retain this functionality while applying client sender rules. |
| `Actions/CommonClasses.cs`: `Display`, plus RunId logging | Account display includes the password, and RunId processing logs the parameter display. New secrets/tokens must never enter these displays. |
| `Actions/RunIdProcessor.cs`: request builders | `ExecuteEmail` and `CreateSendEmailRequest` include `emailPassword` in downstream JSON. Review these contracts and their logs before introducing OAuth configuration. Never repurpose that field for the app secret or token. |
| `Actions/FileNameProcessor.cs` | Loads SMTP username/password from JSON, but does not load bypass or OAuth fields. Its active send uses the shared `SendEmail` class. If this entry point is needed, update its configuration contract and producer too. |
| `Processor.cs`, `CommonMethods.BuildEmail` | Other compiled SMTP routines use password authentication. They are not the direct sending path selected by `Program.Main`; confirm external consumers before deciding whether to extend them. |
| `Actions/ConfigSettings.cs` and example JSON | Currently contain runtime paths and operational switches, not OAuth account configuration. Adding JSON values alone cannot enable OAuth in the present implementation. |
| `MailKitEmailProcessor.csproj` | Targets .NET Framework 4.5.2 and local dependency DLLs. Token-library compatibility and HTTPS/TLS behaviour need deliberate verification. |

The [MailKit XOAUTH2 API](https://mimekit.net/docs/html/T_MailKit_Security_SaslMechanismOAuth2.htm) documents the mechanism. Use that mechanism rather than manually assembling control characters and base64 strings.

### Authentication modes and backward compatibility

Recommended design: add a company/account authentication mode with three explicit values:

| Proposed mode | Validation | SMTP action |
| --- | --- | --- |
| `Password` | Require username and password | Existing username/password authentication |
| `None` | Require host/port and a separately authorised relay configuration | Preserve existing bypass behaviour |
| `OAuth2ClientCredentials` | Require SMTP username, tenant, client ID, resolvable secret reference and valid OAuth settings | Obtain/reuse token and authenticate using XOAUTH2 |

These names and any new configuration columns are proposals, not existing supported settings. For accounts without the new mode, map legacy `BYPASSAUTHCHECK=true` to `None`, and false/NULL to `Password`. Reject explicitly contradictory configurations instead of silently skipping OAuth authentication.

For IBCM, use the new OAuth mode and **keep legacy bypass false**. OAuth validation must allow an empty mailbox password while still requiring the SMTP username and application credentials. Do not enable bypass merely to satisfy today's password validation.

This preserves existing clients using permitted unauthenticated relay arrangements. A bypass flag does not itself configure or authorise Microsoft 365 relay; the handoff specifies authenticated client submission instead.

## 4. Proposed implementation requirements

### A. Configuration and secure credentials — required

- Add an authentication mode, OAuth tenant/client identifiers, secret-store reference, SMTP mailbox username, and an allowed sending identity to company/account configuration. The scope can be fixed for this provider or stored as validated non-secret configuration.
- Choose where non-secret configuration lives: extend company settings and its administration interface, or use a secured deployment configuration keyed by company ID. Avoid a single global credential for all clients.
- If database columns are added, supply a migration, backward-compatible defaults and a deployment order. `PopulateEmailSettings`, account copying and validation must support them together.
- Store the client secret in a secrets manager accessible to the application's Windows execution identity. Confirm that identity, access permissions, and rotation procedure. The service must load a replacement secret without a code change; a documented restart is acceptable if needed by the chosen design.
- Keep tokens in memory, separate from `AccountDetails.Password`, JSON payloads, progress records and diagnostic displays. Do not persist them in ordinary files or the database.
- Review parameter displays, downstream request logging, exception logging and SMTP protocol logging. Redact both raw tokens and their encoded authentication payloads before any output is written.

### B. Token provider — required

- Introduce a dedicated provider for client credentials tokens; do not embed HTTP token logic throughout message composition.
- Use the handoff's tenant-specific endpoint, form-encoded body and exact scope. Treat wrong/missing configuration and missing secret access as clear failures before SMTP submission.
- Validate the token response, retain its expiry, and reuse it with a safety margin such as five minutes. Key caches by account/tenant/app/scope so different clients cannot share the wrong token.
- Coordinate concurrent acquisition within a process. In this executable, an in-memory cache survives only that process; separately launched runs will obtain separate tokens unless an intentionally secure shared provider is introduced.
- Preserve safe HTTP status/error information and Microsoft correlation identifiers where available; never log the request body containing the secret or a successful response body containing the token.
- For onboarding, locally inspect the expected role and stop if absent. Claim decoding is diagnostic inspection, not proof of token authenticity; Exchange validates the presented token.
- Apply bounded HTTP timeouts and controlled retries for transient network/server errors. Invalid/expired secrets and missing permissions require correction, not continuous retries.

Implementation choice: a direct HTTPS request with structured JSON parsing is a possible minimal change. Adding `System.Net.Http` may be needed depending on the chosen API. MSAL is another option, but verify its supported framework and dependency versions first. A framework/library upgrade may be appropriate for maintenance, but is not established as a prerequisite by this handoff; local MailKit already supplies XOAUTH2. Verify secure outbound HTTPS and SMTP TLS on the actual Windows host rather than disabling certificate validation.

### C. SMTP and message identity — required

- Keep `smtp.office365.com:587` with mandatory STARTTLS and normal certificate validation.
- For the OAuth mode, call MailKit authentication with a `SaslMechanismOAuth2` built from the mailbox address and access token. Do not use LOGIN/PLAIN with either the client secret or token.
- Fail clearly if XOAUTH2 is unavailable or authentication fails. Never fall back automatically to password authentication or unauthenticated sending.
- Ensure both MIME From and SMTP envelope sender use `goat@ibcm.com.au`. Check database/template sender values before sending; configure them consistently and reject conflicting values rather than allowing arbitrary senders.
- Choose an approved Reply-To policy. The existing blank Reply-To fallback would point replies back to `goat@` and cause rejection. Set a monitored address if the business requires replies, and verify the resulting received headers.
- Retain subject, To/CC/BCC, exclusions, attachments and embedded images. Test them with the new authentication path.

### D. Failure handling, capacity and operational status — required

The sender currently creates a new SMTP connection for each send and returns an empty result on success, or an error string on failure. RunId processing updates sent values when the result is empty and `UpdateSentValues` is `Y`; it records an error comment otherwise. It has no OAuth provider or explicit SMTP pacing in the reviewed sending block. The existing `RetryCount` is used while waiting for document creation, not as an SMTP/token retry policy.

- Distinguish token acquisition, SMTP authentication, recipient rejection and message submission failures in safe diagnostics. Retain exact server error text and response codes for IBCM's report.
- Treat SMTP acceptance as “submitted/accepted”, not guaranteed inbox delivery. Late failures may occur after acceptance, and the PDF says delivery-failure notices to `goat@` are rejected. Agree on how IBCM will provide Exchange message tracing or other delivery investigation.
- Handle temporary throttling with bounded backoff and a durable retry/rescheduling approach appropriate to the existing workflow. Avoid blindly resending after an ambiguous connection failure following message submission, which can duplicate mail.
- Coordinate sending rate and concurrent jobs across all uses of this mailbox, not only one RunId loop. Obtain expected peak message and recipient volumes before rollout.
- Preserve existing sent-status behaviour deliberately. For a no-send rehearsal, use `SendEmail="N"` and `UpdateSentValues="N"`; these are independent switches. For a controlled live test, sending must be enabled, while status updates must follow the agreed test-data policy.

The handoff quotes approximate limits and says they are untested. Microsoft currently documents three concurrent SMTP submission connections, 30 messages per minute and 10,000 recipients per day in its [SMTP submission limits guidance](https://learn.microsoft.com/en-us/troubleshoot/exchange/send-emails/smtp-submission-improvements). These ceilings are not a delivery guarantee. Count recipient fan-out, including CC/BCC, and confirm any applicable tenant restrictions and expected batch sizes with IBCM. If GOAT's workload exceeds this service's capacity, discuss a different sending architecture with IBCM.

### E. Administration and deployment — required for the selected entry point

- Confirm the client uses the RunId route or `/FromFile:`. Extend the file contract only if it is part of the required client workflow; use secret references there too.
- Add or update the actual upstream settings interface so administrators can choose OAuth and supply non-secret identifiers/reference values. This repository is a console processor and does not contain the SMTP configuration screen requested by the handoff.
- Build and deploy the executable together with matching dependencies and runtime configuration. Confirm the installed executable includes the intended feature and the database supports the required settings.
- Provide a client-specific rollback plan and leave other companies' authentication settings intact. Do not roll back to bypass as an OAuth workaround.

## 5. Responsibility and information still needed

| Owner | Required action or confirmation |
| --- | --- |
| IBCM administrator | Supply the actual secret securely and its expiry date; nominate the rotation contact and procedure. |
| IBCM administrator | Confirm reported app consent, Exchange service principal, mailbox grants and SMTP AUTH exception if authentication fails. No tenant administration changes are assumed necessary if the handoff is correct. |
| IBCM/business owner | Approve From/Reply-To policy, including whether `office@ibcm.com.au` should receive replies, and explain how delivery issues will be investigated. |
| GOAT/project owner | Identify the installed processor version, execution identity, entry point, company settings record, configuration UI owner and related JSON/API consumers. |
| Hosting/network administrator | Verify DNS, outbound HTTPS on port 443 to the token endpoint, outbound SMTP on port 587, trusted certificates and correct system time on the execution host. |
| GOAT/project owner | Provide expected volumes, overlapping jobs and retry/status expectations; select secure credential storage. |
| Tester and IBCM contact | Nominate an external mailbox under the tester's control and arrange the handoff acceptance report. |

No Microsoft Graph integration, IMAP/POP reception, interactive login or mailbox password creation is required by the PDF. Those would be separate scope changes. The actual secret is not in the handoff and has not been obtained during this assessment.

## 6. Validation and acceptance plan

Before contacting the live service, verify these behaviours with controlled tests:

1. Legacy password and bypass accounts retain their validation, copying and sending modes; missing/NULL new settings preserve legacy interpretation.
2. OAuth accepts a blank mailbox password, requires the correct configuration, and rejects conflicting bypass/mode settings.
3. Token requests encode special characters correctly; valid tokens are cached, renewed before expiry and separated by client. Malformed responses, timeouts, invalid secrets and expiry are handled safely.
4. SMTP OAuth invokes XOAUTH2 only after STARTTLS. Failure cannot trigger an authentication downgrade.
5. Sender policy and Reply-To work with recipients, CC/BCC, attachments and images. Rejected sends do not update successful sent status.
6. Logs and exported request payloads contain no application secret, raw access token or encoded token payload, including on exceptions.
7. Pacing, concurrency and retries match the agreed workload. The existing reconciliation tests do not connect to SMTP or validate OAuth; add targeted verification for the new behaviours.

Then follow PDF pages 3–4 in order, stopping at the first failure and recording what is available:

1. Obtain a token: record HTTP status and `expires_in`, without recording the token.
2. Inspect claims locally: confirm `SMTP.SendAsApp`; if missing, stop and report.
3. Authenticate: capture a safely redacted SMTP session showing response codes and XOAUTH2 success.
4. Send one message from `goat@ibcm.com.au` to the approved external test mailbox. Do not use `goat@` as the test recipient.
5. Verify reception and headers, including From and Reply-To. Optionally test the intended bounce policy with an agreed controlled message/reply. A normal reply may go to a configured Reply-To instead of `goat@`, so record which address was actually targeted.

Acceptance requires successful token acquisition, the expected role, SMTP authentication, SMTP submission and confirmed external reception. A successful token request alone is insufficient. Repeat a controlled send after token renewal and verify credential rotation before relying on unattended operation.

### Required report to the handoff sender

- Date, time and time zone for each attempt.
- Token HTTP status, `expires_in` and expected-role result; no token.
- Full SMTP session and response codes with all authentication material redacted.
- Success or exact error text.
- Masked configuration screenshot, send-result/log screenshot and received-message screenshot, ideally including headers. If there is no suitable configuration UI, agree on a masked configuration extract with IBCM.
- Product/build version, library version, authentication mode, host, port, TLS setting and From address.
- Optional bounce result.

Preparing and sending this report is a later implementation/testing activity. No live test or external communication has been performed here.

## 7. Troubleshooting guide from the handoff

| Symptom | Interpretation/action |
| --- | --- |
| `AADSTS7000215` | Check that the supplied credential is the secret value, not its identifier; coordinate securely with IBCM. |
| `AADSTS7000222` | Secret expired; request rotation from IBCM. |
| Token lacks `SMTP.SendAsApp` | Stop before SMTP and report the role result; IBCM must review permission/consent configuration. |
| SMTP `535 5.7.3` | Check mailbox username, token scope and XOAUTH2 use. The handoff allows a delayed retry after a few hours because permissions may still be propagating. Persistent failure needs investigation. |
| SMTP `535 5.7.139` or authentication-disabled text | Ask IBCM to check effective mailbox/tenant SMTP AUTH settings. |
| SendAs rejection | Check From and envelope sender against `goat@ibcm.com.au`. |
| Rejection when addressing `goat@` | Expected consequence of IBCM's rejection rule, rather than a valid external delivery test. |
| Connection failure | Investigate DNS, firewall, proxy, TLS and reachability; blocked outbound port 587 is one possible cause. |

## 8. Recommended delivery order

1. Confirm deployment/entry point, volumes, Reply-To and credential storage with the relevant owners.
2. Implement the company-specific authentication mode, configuration loading, validation and token provider.
3. Integrate XOAUTH2, sender enforcement and safe diagnostics; update affected administration/JSON contracts.
4. Verify compatibility and error paths, then deploy to a controlled test environment.
5. Run the handoff's first end-to-end test and prepare its redacted report.
6. Resolve any tenant-side failures with IBCM, complete renewal/rotation and workload checks, then schedule production rollout.

The smallest supported solution is OAuth SMTP support in the existing processor. Configuration alone cannot meet this client's handoff. No source-code changes were made as part of this assessment.
