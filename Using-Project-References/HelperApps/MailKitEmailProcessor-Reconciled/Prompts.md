## Prompt 1 ##
In the Handoff document the discussion is around OAUTH 2.0. Previously we introduced the existing bypass feature to allow for one of our clients to use Microsoft 365 for sending emails. This process was not an OAuth based method for sending emails. Is the OAuth method detailed in the pdf document indicative of common setups for clients to send email using their Microsoft 365 / Office 365 accounts? We added the bypass feature because we thought it to be the best of breed setups and that it would future proof us. Now we need to consider another approach to allow sending of emails via Microsoft 365 / Office 365 clients. Our current application MailKitEmailProcessor is also used by clients who do not use Microsoft 365 / Office 365 to send emails. We are reluctant to make too many changes to this application in case it hampers those users who are already getting everything they need from it. IF we undertake to make the modifications in the Handoff document are there other methods of setting up Microsoft 365 accounts that we will need to implement that won't be covered by this or the existing bypass method. We cannot afford to implement every solution new or existing clients come up with.
Do not modify any source code at this time. Append output from this prompt to the file Prompts.md in this folder.

## Response: Microsoft 365 support strategy — 13 October 2026

**The OAuth setup in the handoff is a standard, documented Microsoft 365 approach for unattended applications. Your bypass approach can also be valid, but it serves a different deployment model. Neither covers every Microsoft 365 configuration. You do not need to implement every model: define a small supported set and require clients to provision one of those configurations.**

### Is OAuth a common setup?

Microsoft explicitly supports OAuth for SMTP and documents application access using the client credentials flow. The handoff is therefore a mainstream supported integration pattern, rather than a bespoke protocol invented by IBCM. I have not established what percentage of customers use it, so “common” here means a recognised, supported pattern rather than a measured market share. [Microsoft SMTP OAuth documentation](https://learn.microsoft.com/en-us/exchange/client-developer/legacy-protocols/how-to-authenticate-an-imap-pop-smtp-application-by-using-oauth).

The key distinction is between an application sending unattended and a person using Outlook. Microsoft notes that modern Outlook clients generally do not use SMTP AUTH to send messages. Their account setup should not be treated as the specification for this background processor. [Microsoft SMTP AUTH guidance](https://learn.microsoft.com/en-us/exchange/clients-and-mobile-in-exchange-online/authenticated-client-smtp-submission).

IBCM's specific choices — one app, one sending mailbox, an application secret, and a rule rejecting replies — are its operational policy. Other clients can use the same application sending mechanism with their own tenant ID, client ID, mailbox and credentials. Those changes should be configuration, not new code for each client. The reply rejection rule is not an OAuth requirement.

### Was adding bypass the wrong decision?

No. Skipping SMTP username/password authentication is appropriate when the destination relay authorises the sending system through another trust arrangement. Microsoft's connector relay can identify the sender by its public IP address or a TLS certificate. Direct Send is another password-free option, but cannot relay messages to external recipients. [Microsoft application sending options](https://learn.microsoft.com/en-us/Exchange/mail-flow-best-practices/how-to-set-up-a-multifunction-device-or-application-to-send-email-using-microsoft-365-or-office-365).

However, `BYPASSAUTHCHECK` only suppresses the application's SMTP authentication call. It does not establish relay permission, configure a connector, or authorise sending. The previous client's actual host, port, connector and network arrangement would need checking before we can identify precisely which model it uses. It may also submit to an intermediary relay that subsequently delivers through Microsoft 365.

The earlier assumption that bypass would future-proof all Microsoft 365 use was too broad. It future-proofs neither tenant policy nor hosting constraints. It remains useful for compatible relay deployments; OAuth addresses authenticated mailbox submission. “No mailbox password” applies to both, but their trust mechanisms are different.

### What would remain outside bypass plus the handoff implementation?

| Setup | Coverage and proposed position |
| --- | --- |
| Existing SMTP providers using username/password | Already supported within the application's existing STARTTLS connection model. Preserve this path. |
| Microsoft 365 connector relay based on public IP | Existing bypass can fit the SMTP side when the connector, source IP, host, port and TLS are correct. Client administrators own provisioning. |
| Microsoft 365 connector relay based on a TLS client certificate | Bypass alone is insufficient: the processor currently does not configure a client certificate. Require a customer-managed compatible relay or a separately approved enhancement. |
| Microsoft 365 Direct Send | Existing bypass may fit compatible TLS endpoints, but this is internal delivery only; unsuitable for sending to external property owners and contacts. |
| SMTP OAuth using client credentials and a client secret | Covered by implementing the handoff as a configurable reusable feature. Recommended initial Microsoft 365 OAuth profile. |
| SMTP OAuth using client credentials and a certificate | Same SMTP token authentication, but different token acquisition and certificate/key management. Not automatically included in the secret-based implementation. Possible later extension. |
| SMTP OAuth using delegated user sign-in | Requires a different token lifecycle, initial user authorisation and refresh-token handling. Exclude from the initial unattended-service profile. |
| Microsoft Graph sending | An HTTP API transport with its own permissions and delivery integration. Neither bypass nor SMTP XOAUTH2 implements it. Separate scope. |
| Microsoft 365 High Volume Email | A separate service aimed at internal recipients. Do not assume that implementing the handoff qualifies this application for it. |
| Other transactional email services | Some offer compatible SMTP; others require their own API. Accept compatible SMTP configurations and assess API integrations separately. |

Microsoft's [OAuth documentation](https://learn.microsoft.com/en-us/exchange/client-developer/legacy-protocols/how-to-authenticate-an-imap-pop-smtp-application-by-using-oauth) describes both delegated and application access. Its [client credentials documentation](https://learn.microsoft.com/en-us/entra/identity-platform/v2-oauth2-client-creds-grant-flow) supports secrets, certificates and federated credentials; these are distinct credential-acquisition options, not automatically covered by adding XOAUTH2. [Graph sendMail](https://learn.microsoft.com/en-us/graph/api/user-sendmail?view=graph-rest-1.0) documents the separate API. [High Volume Email guidance](https://learn.microsoft.com/en-us/exchange/mail-flow-best-practices/high-volume-mails-m365) states that HVE accounts cannot deliver externally.

For connector relay, Microsoft specifies the tenant MX endpoint and port 25, with an eligible source identity. Its guidance also excludes direct relay from third-party hosted services such as Azure. Therefore an IP relay that works at a customer's premises should not be assumed to work unchanged on your shared hosted server. Confirm deployment eligibility before promising this profile. [Microsoft relay requirements](https://learn.microsoft.com/en-us/Exchange/mail-flow-best-practices/how-to-set-up-a-multifunction-device-or-application-to-send-email-using-microsoft-365-or-office-365).

**Other methods exist, but their existence does not oblige you to implement them.** A client whose security policy disallows your supported profile must either provide an approved SMTP endpoint compatible with the product, fund an explicitly scoped extension, or choose another integration. For example, a tenant that prohibits SMTP AUTH altogether cannot use XOAUTH2 SMTP merely because it permits OAuth for Graph. Its administrator must approve a compatible arrangement.

### Recommended support policy

Offer three application authentication profiles:

1. **SMTP username/password:** retain existing functionality for compatible providers.
2. **SMTP without an AUTH exchange:** retain bypass for a customer-authorised relay with required STARTTLS. Publish relay eligibility and network prerequisites.
3. **Microsoft 365 SMTP application OAuth:** add client credentials with a secret, a tenant-specific registration, approved mailbox permissions, XOAUTH2, and STARTTLS on port 587.

For profile 3, require a dedicated approved sending mailbox, client-administered Entra/Exchange provisioning, secure credential delivery and rotation, an approved Reply-To policy, and volumes that fit mailbox submission limits. Publish the onboarding specification before clients commission their IT providers to configure accounts.

Initially exclude delegated sign-in, certificate/federated token credentials, direct client-certificate relay, Graph APIs and specialised high-volume services. These exclusions are a proposed commercial scope boundary, not statements that those technologies are invalid. A later credential-provider extension can be evaluated once actual customer demand justifies it.

Use customer-specific tenant registrations for the initial rollout. A vendor-owned multitenant application introduces a different consent and security operating model and should not be quietly included in this enhancement.

Do not promise “all Microsoft 365 accounts supported”. Promise “Microsoft 365 supported using our published application OAuth or eligible relay profile”. Clients remain responsible for selecting a compatible setup and maintaining its permissions, network identity and credentials.

### Protecting existing non-Microsoft clients

The change can be narrowly scoped. The bundled MailKit already contains `SaslMechanismOAuth2`, so the handoff does not inherently require replacing the SMTP library or message-building pipeline.

- Add an explicit account-level authentication mode. When absent, retain today's interpretation of `BYPASSAUTHCHECK`; existing accounts must not change behaviour merely because a new executable is deployed.
- Add a separate token provider used only by the OAuth mode. Keep application secrets and tokens out of existing password fields and logs.
- Keep sender restrictions specific to each configured OAuth account. Do not impose IBCM's `goat@` address or reply policy on other customers.
- Preserve the existing password/bypass branches and message composition. Do not automatically try another authentication mode after an error.
- Verify copying, settings loading, database compatibility and the entry points actually used by clients. New settings queries must not break databases that have not yet received a migration.
- Run targeted regression checks for password, bypass and OAuth accounts before a staged rollout. Avoid bundling unrelated framework/library upgrades into this feature unless a demonstrated requirement makes them necessary.

Alternatively, OAuth can sit in a separately operated SMTP gateway, allowing the processor to keep using its existing SMTP interface. This can reduce changes to the executable, but shifts responsibility to another deployed service: secure relay access, queueing, monitoring, credential rotation and availability. It also changes whether the processor observes acceptance by the gateway or by Microsoft. Given the existing MailKit capability, I recommend a bounded account-level OAuth addition unless you already operate a suitable supported gateway.

### What “future-proof” can realistically mean

No fixed implementation guarantees compatibility with every future tenant policy or Microsoft service change. A practical goal is to standardise a small set of supported configurations, isolate authentication from message composition, and make credentials/configuration replaceable without client-specific code changes.

Do not base new Microsoft 365 onboarding on mailbox password authentication. Microsoft has announced retirement of SMTP AUTH Basic Authentication, with a revised timeline; this is distinct from retiring OAuth SMTP or connector relay. See the [Exchange team's updated deprecation timeline](https://techcommunity.microsoft.com/blog/exchange/updated-exchange-online-smtp-auth-basic-authentication-deprecation-timeline/4489835). Older articles contain superseded deadlines, so check that current announcement when scheduling migrations. This does not require removing password SMTP support for non-Microsoft providers.

**Recommendation:** keep the current password and bypass capabilities, add only the handoff's reusable application OAuth profile, and issue a supported-configuration policy. Other Microsoft 365 arrangements should be compatibility checks or separately approved product work, rather than automatic obligations triggered by each client's preferred setup.

This response was appended to `Prompts.md`. No application source code was modified, and no live tenant changes or email tests were performed.

## Prompt 2 ##
To accommodate OAuth 2.0 in the project MailKitEmailProcessor what would need to implement?
I assume we would need additional fields added to the database for whatever values we need to store.
I assume we would need to modify the AccountDetails object to accommodate the new values.
I assume we would need to modify the change the process for sending emails in SendEmails.cs.
Can you creat a document named IBCM-Plan.md that details what we have to do to implement this requirement.
Do not modify any code at this stage but, in the document, give details explanations of what we need to modify, where it is best to do so and also include code changes that are required to implement the requirements. Make the code changes as details and comprehensive as possible. Assume that, after review and any further clarifications, this code will be added to the existing source. Keep the TargetFrameworkVersion unchanged and make all code compliant with this TargetFrameworkVersion.
Assume that this code will be implemented by a fresh new Junior developer as one of their first projects with us.
The code should not interfere with the current process for existing clients.

## Prompt 3 ##
Looking over the IBCM-Plan.md document the suggestion is to add an entry to the config settings for EnableMicrosoft365OAuth. The purpose of the config settings is for universal settings not individual customer / client settings. The client settings are what the table query that contains, for example, the bypass option come from. I could add this field to the database and use it here to build the extended AccountDetails values with and then use the AccountDetails for the OAuth processing later if that is what is needed. Does this sound like a workable solution for the rest of the IBCM-Plan?
Append the response for this prompt to the file Prompts.md
## Response to Prompt 3: company-specific OAuth selection — 14 October 2026

**Yes. Selecting OAuth from the company's database settings and carrying those settings in `AccountDetails` is workable and fits your separation between universal configuration and individual client settings. The token provider and OAuth sender proposed in `IBCM-Plan.md` can continue to work with that design.**

The proposed `EnableMicrosoft365OAuth` configuration switch was intended as a deployment/schema compatibility gate: it stopped older databases being queried for columns they did not yet contain. It was not intended to select individual clients. However, it adds a second switch and an activation-order dependency that are unnecessary if you manage the database migration before deploying the new executable. I recommend removing that proposed configuration entry and making the database account settings the authority for authentication selection.

### Prefer the mode field already proposed in the plan

The plan already proposes `EACompanySettings.CO_EMAILAUTHMODE`. That field can perform the job you describe without adding a separate `EnableMicrosoft365OAuth` Boolean:

| Company setting | Behaviour |
| --- | --- |
| `CO_EMAILAUTHMODE` is NULL, empty or `LEGACY`; bypass false | Existing username/password authentication |
| `CO_EMAILAUTHMODE` is NULL, empty or `LEGACY`; bypass true | Existing bypass behaviour |
| `CO_EMAILAUTHMODE = 'M365_CLIENT_SECRET'`; bypass false | Load and validate OAuth settings, then authenticate with XOAUTH2 |
| OAuth mode plus bypass true | Configuration error; do not skip authentication |
| Unrecognised nonempty mode | Configuration error; do not silently use the legacy path |

Only the IBCM company row would initially receive the OAuth mode and identifiers. Existing companies can keep NULL mode and retain their existing username/password or bypass behaviour.

A Boolean database field is also possible, but **do not maintain both an OAuth Boolean and an independent authentication mode unless there is a specific need**. Two fields can disagree. Using `CO_EMAILAUTHMODE` is clearer because it identifies the supported method and maps directly to the plan's existing `AccountDetails.AuthenticationMode`, `ResolveMode`, validation and sender selection.

If you prefer a Boolean, use it as the sole database selector: true maps to `AccountDetails.AuthenticationMode = "M365_CLIENT_SECRET"`; false/NULL maps to `LEGACY`. Do not also independently populate a conflicting mode column. Keep the rest of `AccountDetails` and the later processing based on the resolved mode, so token acquisition and SMTP sending do not need to know which database representation was chosen.

### How the revised flow would work

1. `PopulateEmailSettings` queries the settings row for the current `CompanyClientId`.
2. It reads the company's authentication mode alongside the existing SMTP fields and bypass option.
3. For an OAuth company, it populates `OAuthTenantId`, `OAuthClientId`, `OAuthSecretReference` and `OAuthFromAddress` in `AccountDetails`. It validates the required values and rejects bypass conflicts.
4. For a legacy company, it keeps the new OAuth properties empty and preserves existing validation and authentication behaviour. Reset these properties explicitly if an account object can be reused.
5. `SendEmailParam.CopyAccountDetailsFrom` copies the complete account settings, including the authentication mode and OAuth properties.
6. `SendEmail` uses that mode to select either the original sending path or `Microsoft365OAuthSmtpSender`. The OAuth helper retrieves the secret and token only for an OAuth account.

Selecting the additional columns for every company does not itself enable OAuth. The account's mode controls whether those values are used. With a fully migrated database, the simplest implementation is one query containing the original fields and all five proposed new fields, followed by mode-dependent population and validation. A second query for the OAuth fields is possible, but it adds complexity and still requires a way to read the selector in the first query.

### What needs to change in the plan

- **Section 3:** remove the proposed `ConfigSettings.EnableMicrosoft365OAuth` property, example JSON entry and deployment-switch instructions. Retain the database migration and company-specific mode.
- **Section 5:** remove the dependency on `m_ConfigSettings.EnableMicrosoft365OAuth`. Select the new columns in the company-settings query, resolve the database mode, and populate/validate the OAuth properties only when that company uses OAuth.
- **Sections 4, 6–9:** retain the proposed account properties/copying, secure credential adapter, token provider, OAuth sender and mode-based sending integration. These already operate on `AccountDetails` and do not require the universal configuration switch.
- **Section 10:** retain the project references, JSON diagnostic markers and safe logging requirements. No OAuth activation entry belongs in the universal runtime JSON.
- **Sections 11–12:** remove references to the universal OAuth switch and update the schema compatibility tests and deployment order. The optional file route still needs an explicit authentication mode in its own approved input contract if it is required; it does not automatically read company settings.

### Database compatibility is the remaining decision

A company-level value controls behaviour; it cannot make a column exist in an older database. SQL generally resolves the selected columns before returning a row. Therefore, a query selecting `CO_EMAILAUTHMODE` and the other new columns fails against an unmigrated database even when every company would otherwise be legacy.

**Recommended approach:** apply the additive migration to every database that will run the new executable before deploying it. Leave the new fields NULL for existing clients. The existing executable ignores the added columns, and the new executable can use one consistent query. This preserves existing client behaviour while avoiding a universal OAuth switch.

If you must run the same new executable against both migrated and unmigrated databases, the loader needs a separately designed schema-version/capability check. It would use the original query for a verified old schema and the extended query for a verified new schema. Do not catch arbitrary database errors and silently fall back: that could hide an OAuth deployment error. This is a database compatibility requirement, not a per-client OAuth setting.

### Protecting existing clients and handling rollback

The database change must be additive, with no bulk changes to existing usernames, passwords, bypass values or sender addresses. The new mode defaults to legacy interpretation. Only an explicitly configured OAuth company receives OAuth validation and token acquisition.

Clearing the OAuth selector does not disable sending; it selects legacy authentication. For IBCM, that would normally leave an empty mailbox password and cause failure, so it is not a valid OAuth rollback strategy. Stop that company's sends or restore a previously working, authorised configuration. Never set bypass true as an OAuth workaround.

The client secret remains in the approved credential store. The database and `AccountDetails` carry its reference, not the secret or access token. This change does not require a framework upgrade and leaves the proposed .NET Framework 4.5.2-compatible OAuth classes applicable.

**Recommendation:** use the already proposed database `CO_EMAILAUTHMODE` as the single company-level selector, remove the universal `EnableMicrosoft365OAuth` proposal, and require schema migration before deployment. The rest of the plan remains workable with the loader, deployment and verification adjustments described above.

Only this response was appended to `Prompts.md`. `IBCM-Plan.md` and application source code were not modified.

## Prompt 4 ##
For all OAuth 2.0 and Microsoft 365 solutions in particular, do they all require values for:
OAuthTenantId, OAuthClientId, OAuthSecretReference, OAuthFromAddress, FromAddress, ,ReplyToAddress
In the database query and the AccountDetails object I would add the new OAuth fields as detailed here but would I be better prefixing the FromAddress and ReplyToAddress fields with OAuth so as not to confuse them with existing or future fields that may need to be added to the class? This way they would also be a form of self documentation as to their intended use?
Apart from adding these fields to the database query are there any other processes in RunIdProcessor.cs that need to be modified?
Is the only place these new values need to be considered when we come to sending the email itself from SendEmail.cs?
Append the response for this prompt to the file Prompts.md
## Response: OAuth fields, sender naming and RunId processing — 14 October 2026

**No: those six fields are not universal requirements for every OAuth 2.0 solution. For the specific IBCM Microsoft 365 client-secret SMTP profile, keep authentication configuration in `AccountDetails`, and keep the actual From and Reply-To addresses in the existing `SenderDetailsItem`. The new settings must be loaded, copied, validated and handled safely before the SMTP connection is made.**

### Which values are required?

| Proposed value | Purpose | Required for IBCM's proposed implementation? | Universal OAuth requirement? |
| --- | --- | --- | --- |
| `OAuthTenantId` | Identifies the Microsoft directory used to request a token | Yes | No. Other OAuth providers do not necessarily have Microsoft-style tenants. |
| `OAuthClientId` | Identifies the registered application | Yes | Common to application OAuth flows, but the exact representation and whether your application stores it per customer depend on the integration. |
| `OAuthSecretReference` | Locates the application secret in your approved secure store | Yes, for the proposed secret-store design | No. This is our configuration abstraction, not an OAuth protocol parameter. Certificate, federated and other flows use different credentials. |
| `OAuthFromAddress` | Restricts which sending address this account may use | Yes, under the current plan | No. This is an application sending policy, not a token-request field. |
| `FromAddress` | Actual message From address | Yes for the email message, but it already exists in the sender pipeline | No. OAuth also authenticates applications doing work unrelated to email. |
| `ReplyToAddress` | Directs a recipient's reply to a monitored address | Required operationally if IBCM wants replies delivered elsewhere; not needed to obtain a token or authenticate SMTP | No. An email can omit an explicit Reply-To, in which case replies ordinarily target its From address. |

Microsoft's [client credentials documentation](https://learn.microsoft.com/en-us/entra/identity-platform/v2-oauth2-client-creds-grant-flow) distinguishes secret, certificate and federated credentials. Microsoft's [SMTP OAuth documentation](https://learn.microsoft.com/en-us/exchange/client-developer/legacy-protocols/how-to-authenticate-an-imap-pop-smtp-application-by-using-oauth) describes token acquisition and the mailbox username presented through XOAUTH2. Neither turns message reply routing into an OAuth credential.

The list also needs the authentication mode, SMTP host/port and SMTP mailbox username. For IBCM, the existing `UserName` supplies `goat@ibcm.com.au`. The scope and grant type are required token-request values but can remain fixed for this bounded implementation, as the plan proposes. An access token is obtained at runtime and should not become a stored database setting or an `AccountDetails` property.

IBCM's From restriction and rejection of incoming mail to `goat@` are its mailbox/permissions/business configuration. They should not be mistaken for rules imposed on every Microsoft 365 OAuth client. Our initial supported profile deliberately requires the permitted From to match the authenticated mailbox; a future product decision could support separately authorised SendAs identities, but the present implementation should not claim that support.

### Naming From and Reply-To

I recommend **not** adding `OAuthFromAddress` and `OAuthReplyToAddress` as duplicate versions of the actual message addresses. From and Reply-To have the same meaning whether authentication uses a password, bypass or OAuth.

The current structure already separates their roles:

- `AccountDetails` contains connection/authentication settings and account-level policies.
- `SenderDetailsItem.Address` contains the actual From address.
- `SenderDetailsItem.ReplyToAddress` contains the actual Reply-To address.
- `BuildEmailAddressItems` reads `FromAddress` and `ReplyToAddress` from `EventRunShowClients` and populates the sender object.

Consequently, do not add a second actual `FromAddress` or `ReplyToAddress` to `AccountDetails` merely to implement OAuth. That would create two competing sources of truth and make it easier for the sent message, generated requests and sent-history records to disagree.

**There is a naming improvement to the existing proposal:** `OAuthFromAddress` in the plan is an allowed-address policy, not the actual message From field. `AllowedFromAddress` describes that role more precisely. For example:

| Object | Recommended property | Meaning |
| --- | --- | --- |
| `AccountDetails` | `OAuthTenantId`, `OAuthClientId`, `OAuthSecretReference` | OAuth-specific authentication settings |
| `AccountDetails` | `AllowedFromAddress` | Approved sending address against which the message is validated |
| `SenderDetailsItem` | Existing `Address` | Actual From address for this message |
| `SenderDetailsItem` | Existing `ReplyToAddress` | Actual Reply-To address for this message |

If you want the policy name to indicate the current scope explicitly, `OAuthAllowedFromAddress` is also reasonable. It documents both purpose and current applicability. My preference is `AllowedFromAddress`, with validation applied only to the new OAuth profile initially. The name need not force that policy on legacy clients.

If adopting this rename, update it consistently in the planned database-to-object mapping, account constructors/copying, validation and OAuth sender. Decide the database column name before migration. This response does not rename any existing source or revise the plan document.

For IBCM's Reply-To, use the existing `ReplyToAddress` pipeline. Configure the event/address source to return the approved monitored address. If a company-level default is required, introduce an explicitly named policy such as `DefaultReplyToAddress`, with a documented precedence rule against message-specific values. That is an additional business requirement, not something OAuth itself requires. Do not introduce it just to prefix the field with OAuth.

### What else in `RunIdProcessor.cs` needs attention?

Adding columns to the SQL SELECT is only the first part. The reviewed source has the following relevant stages:

| Location/process | Required action or review |
| --- | --- |
| `PopulateEmailSettings` | Read the company authentication mode and OAuth values, populate/reset `AccountDetails`, and validate OAuth configuration. With the latest agreed database-driven design, remove the proposed dependency on the universal configuration switch. |
| `ProcessRun`: `SendEmailParam.CopyAccountDetailsFrom(AccountDetails)` | Ensure the expanded account is copied completely. The actual copy-method changes belong in `CommonClasses.cs`, including `AccountDetails.CopyFrom`. The RunId call can remain as it is. |
| `BuildEmailAddressItems` | Preserve its actual From/Reply-To fields. Ensure IBCM's upstream data produces the approved values. A source-code change here is needed only if you choose to introduce a new company-default/override policy. |
| Immediately after `CopySenderFrom`, before request generation | Recommended: validate the final OAuth sender against the account's allowed address. This catches a configuration mismatch before generating request files. Keep the same check at the actual sending boundary as protection for other callers. |
| `BuildOriginalJsonRequest` and `BuildJsonRequest` | Identify OAuth requests with the planned non-secret mode markers; keep secrets/tokens out. Review consumers before enabling an account. Do not rename their existing sender fields solely because OAuth is being added. |
| `SendEmailParam.Display()` and output logging | Redact credential data and never display tokens. The main display change is in `CommonClasses.cs`, but RunId processing calls it and writes its result. |
| `EmailSender.Send(...)`, result processing and `UpdateSentValues` | Retain the existing success/error contract. OAuth configuration/token/authentication failures must not mark an email sent. Stored From/Reply-To must match the message actually submitted. No new OAuth fields are needed in sent-history updates merely to authenticate. |

The early sender validation should run only when the account is in OAuth mode. It should inspect the sender after `CopySenderFrom`, because that method applies the existing sender fallback behaviour. It must return a normal error into the current `Result` flow so later request generation/sending is skipped; do not silently change the From address.

If the upstream database data already returns correct From and Reply-To values, `BuildEmailAddressItems` and `UpdateSentValues` do not inherently need rewriting. They need verification with the new profile. This keeps the change focused and avoids changing sender behaviour for existing customers.

The actual `EventRunShowClients` definition and settings administration interface are outside this repository. Their owners must configure the company mode/identifiers, permitted From and approved Reply-To. New database columns do not automatically update those upstream interfaces.

### Are the new values considered only in `SendEmail.cs`?

**No. Only their eventual SMTP use belongs at the sending stage.** Their earlier handling is essential:

1. Load and validate company settings in `PopulateEmailSettings`.
2. Preserve them through both account-copy paths in `CommonClasses.cs`.
3. Validate sender policy after sender construction and again before SMTP submission.
4. Keep generated requests and diagnostics consistent and free of credentials.
5. Resolve `OAuthSecretReference` in the secure-store adapter.
6. Use tenant/client identifiers and the resolved secret in the token provider, which caches and renews access tokens.
7. Select the sending branch in `SendEmail.cs`; the separate OAuth sender performs STARTTLS and XOAUTH2 with the mailbox username and token.
8. Carry the final send result through the existing error/sent-status processing.

Do not put token HTTP requests or secret-store access into `RunIdProcessor`: it should prepare settings and coordinate the existing workflow. The proposed separate provider and sender classes keep authentication mechanics isolated.

**Recommended field model:** retain the proposed OAuth identifiers/reference and company authentication mode in `AccountDetails`; name the sending restriction `AllowedFromAddress` or `OAuthAllowedFromAddress`; retain actual From and Reply-To in the existing sender object. That gives useful self-documentation without duplicating email fields or tying ordinary message headers to one authentication method.

Only this response was appended to `Prompts.md`; no application source, database or plan-document changes were made.
