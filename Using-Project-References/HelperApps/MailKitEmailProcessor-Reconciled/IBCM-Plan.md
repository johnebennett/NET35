# IBCM OAuth SMTP implementation plan

Prepared 13 October 2026 for MailKitEmailProcessor. This is a proposed implementation guide for review, including code to add later. Only this document has been created; none of the proposed SQL, source changes, credential provisioning or live tests has been executed.

## 1. Outcome, boundaries and reading order

Implement unattended Microsoft 365 SMTP sending using OAuth 2.0 client credentials with a client secret, as requested in `Handoff for the Vendor GOAT SMTP.pdf`. Keep the target framework at **.NET Framework 4.5.2** and preserve existing password and `BYPASSAUTHCHECK` sending for other accounts.

Your three assumptions are correct: add company-specific database settings, extend `AccountDetails`, and integrate a new authentication path into the sending process. The actual sending file is **`Actions/SendEmail.cs`**, singular. Add token acquisition and secure credential retrieval as separate classes rather than putting that logic into `RunIdProcessor` or MIME composition.

This plan supports the numeric RunId/database entry point used by `Program.Main`. The `/FromFile:` path remains legacy-only in the initial release and must explicitly reject OAuth file requests. A complete optional file-contract implementation is described separately; do not imply that it works merely because `AccountDetails` gained fields.

The first release supports the handoff's secret-based application flow only. Delegated user sign-in, certificate/federated application credentials, Graph transport and client-certificate SMTP relay are separate product decisions.

Read sections 2–4 first. Implement in section order on an isolated development branch after approval. Compile after each source step, then complete section 12 before deployment. SQL and code fences are proposed additions/replacements, not instructions to run against production now.

### Design overview

```text
RunIdProcessor
  -> existing company SMTP fields + new OAuth identifiers/reference
  -> AccountDetails -> SendEmailParam.CopyAccountDetailsFrom
  -> existing SendEmail MIME composition
       LEGACY -> existing STARTTLS/password-or-bypass code
       M365_CLIENT_SECRET -> Microsoft365OAuthSmtpSender
          -> Windows Credential Manager -> app secret
          -> Microsoft365TokenProvider -> cached bearer token
          -> STARTTLS -> XOAUTH2 -> submit the same MIME message
```

The proposed two mode values are `LEGACY` and `M365_CLIENT_SECRET`. `LEGACY` continues to use the existing bypass flag to choose password versus no AUTH. This avoids changing existing bypass semantics. A missing mode is `LEGACY`. Unknown nonempty mode values fail; they never downgrade to legacy.

### Compatibility rules

| Existing/new configuration | Result |
| --- | --- |
| Missing/NULL/empty mode, bypass false/NULL | Existing password path |
| Missing/NULL/empty mode, bypass true | Existing bypass path |
| Explicit `LEGACY` | Existing path selected by bypass |
| `M365_CLIENT_SECRET`, bypass false, valid settings | New OAuth path; mailbox password may be blank |
| OAuth plus bypass true | Configuration failure; never omit OAuth AUTH |
| Unknown mode or invalid OAuth settings | Configuration failure; no fallback |

Legacy host/port validation retains its current rules. Stricter port, host and mailbox checks apply only to the new profile. Other clients do not acquire tokens, read Credential Manager or gain IBCM-specific sender restrictions.

## 2. Review decisions and prerequisites

These decisions need confirming before deployment, but they do not prevent preparing the implementation:

1. **Execution route:** confirm IBCM uses numeric RunId. If it needs `/FromFile:`, implement section 11 before onboarding.
2. **Credential store:** proposed baseline is Windows Credential Manager, accessed as the same Windows identity that runs this console program. Confirm IBCM accepts it as the handoff's required managed secret storage. If an enterprise vault is mandatory, replace `IClientSecretStore`'s implementation; keep the rest of the design. Do not switch to plaintext database/file secrets.
3. **Windows identity/profile:** scheduled tasks/services must have a persistent usable profile and access to that identity's credential vault. Verify this on the actual hosting arrangement; interactive developer tests are insufficient.
4. **Reply-To:** obtain an approved monitored address. `office@ibcm.com.au` is mentioned in the handoff's bounce message but is not automatically the approved Reply-To.
5. **Database:** confirm Firebird server version and deployment migration tooling. Proposed new identifiers below are within the traditional 31-character identifier limit. Review character sets and column lengths against the installed database.
6. **Volume:** this code does not create a durable SMTP queue or shared rate limiter. If concurrent jobs/batches can exceed Microsoft's service limits, resolve pacing and durable retry ownership before production. Do not overload the existing document-generation `RetryCount` setting for email retries.
7. **Downstream JSON:** generated email-request files currently contain a password field. Identify consumers; initial OAuth request files must not be treated as replayable credentials. Section 10 explicitly marks the new mode without exporting the secret/token.

### IBCM's values

| Setting | Value |
| --- | --- |
| Host / port | `smtp.office365.com` / `587` |
| TLS | Required STARTTLS |
| Existing SMTP username | `goat@ibcm.com.au` |
| Mode | `M365_CLIENT_SECRET` |
| Tenant | `0299386a-a135-4d14-9d36-b571b6e3a637` |
| Client ID | `da247c5e-5598-4a2d-9391-7277f282a828` |
| Allowed From | `goat@ibcm.com.au` |
| Secret reference | Proposed `EAOffice/MailKitEmailProcessor/IBCM`; actual credential value supplied securely |
| Scope | Fixed in code: `https://outlook.office365.com/.default` |
| Grant | Fixed in code: `client_credentials` |
| Token URL | Construct from the validated tenant GUID: `https://login.microsoftonline.com/{tenant}/oauth2/v2.0/token` |

Do not store the client secret in `CO_EMAILPASSWORD`. Do not store access tokens, refresh tokens or token endpoints in the database. This bounded profile needs no refresh token and derives its endpoint from a validated tenant GUID; arbitrary endpoints would unnecessarily expose credentials to misconfiguration.

Microsoft documents this SMTP application flow and XOAUTH2 in [SMTP OAuth authentication](https://learn.microsoft.com/en-us/exchange/client-developer/legacy-protocols/how-to-authenticate-an-imap-pop-smtp-application-by-using-oauth). Its [client credentials flow](https://learn.microsoft.com/en-us/entra/identity-platform/v2-oauth2-client-creds-grant-flow) documents the token request. These references justify the protocol choices; the following project design and code are proposed specifically for this repository.

## 3. Database migration and deployment switch

### New company fields

Add these nullable columns to `EACompanySettings`. Leave existing accounts NULL; do not bulk-update bypass or passwords.

```sql
ALTER TABLE EACompanySettings ADD CO_EMAILAUTHMODE VARCHAR(24);
ALTER TABLE EACompanySettings ADD CO_OAUTHTENANTID VARCHAR(36);
ALTER TABLE EACompanySettings ADD CO_OAUTHCLIENTID VARCHAR(36);
ALTER TABLE EACompanySettings ADD CO_OAUTHSECRETREF VARCHAR(200);
ALTER TABLE EACompanySettings ADD CO_OAUTHFROMADDRESS VARCHAR(254);
COMMIT;
```

This is a one-time migration, not an idempotent script. The migration runner/DBA must check which columns already exist and record the applied version. Do not rerun blindly. There is deliberately no new column for plaintext secrets, tokens, scope, password or Reply-To. SMTP username uses the existing column; Reply-To remains in the existing event/address pipeline.

Configure only the selected IBCM company after migration, using a parameterised administration operation:

```sql
UPDATE EACompanySettings
SET CO_EMAILHOST = 'smtp.office365.com',
    I_CO_EMAILPORT = 587,
    CO_EMAILUSERNAME = 'goat@ibcm.com.au',
    CO_EMAILPASSWORD = NULL,
    BYPASSAUTHCHECK = 0,
    CO_EMAILAUTHMODE = 'M365_CLIENT_SECRET',
    CO_OAUTHTENANTID = '0299386a-a135-4d14-9d36-b571b6e3a637',
    CO_OAUTHCLIENTID = 'da247c5e-5598-4a2d-9391-7277f282a828',
    CO_OAUTHSECRETREF = 'EAOffice/MailKitEmailProcessor/IBCM',
    CO_OAUTHFROMADDRESS = 'goat@ibcm.com.au'
WHERE I_SETTINGS_COMPANYID = :CompanyClientId;
```

`:CompanyClientId` is a parameter, not literal SQL to paste without binding. Verify that exactly the intended row changed before committing. The sample assumes the existing bypass column is numeric; inspect its actual type and use its established false representation if different. Use the correct client ID for the company record, not the OAuth application client ID.

### Add an opt-in deployment property

In `Actions/ConfigSettings.cs`, add inside `ConfigSettings`:

```csharp
public bool EnableMicrosoft365OAuth { get; set; }
```

No constructor change is needed: .NET bool and missing JSON values default to false. Add `"EnableMicrosoft365OAuth": false` to `MailKitEmailProcessorSettings.example.json`, with valid commas. For a migrated OAuth deployment, set the real executable-adjacent settings JSON to true. This property is a genuine JSON Boolean, not the existing string `"Y"` convention.

When false, the loader below runs the original column list. Therefore this build can still run against an existing database that lacks the new columns. When true, all five columns must exist; a schema failure is reported rather than silently falling back. The switch is deployment-wide; the actual account mode is company-specific.

**Ordering matters:** never activate an OAuth company row in a deployment whose switch is false. A disabled switch intentionally uses legacy configuration; it is not a safe “OAuth disabled” rollback for an already activated row. Roll back by stopping/quarantining IBCM sends and restoring that company's previous compatible configuration if one exists. Other companies can continue normally. Never change IBCM to bypass as a workaround.

## 4. Extend `AccountDetails` safely

In `Actions/CommonClasses.cs`, add the enum below immediately before `AccountDetails`, inside the existing namespace. Replace only the entire existing `AccountDetails` class with the following. Leave `SenderDetailsItem`, recipient classes and the rest of the file intact. Existing constructor calls remain source-compatible.

```csharp
public enum EmailAuthenticationMode
{
    Legacy = 0,
    Microsoft365ClientSecret = 1
}

public class AccountDetails
{
    public string Host { get; set; }
    public int Port { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public bool BypassAuthCheck { get; set; }

    public string AuthenticationMode { get; set; }
    public string OAuthTenantId { get; set; }
    public string OAuthClientId { get; set; }
    public string OAuthSecretReference { get; set; }
    public string OAuthFromAddress { get; set; }

    public AccountDetails()
    {
        Host = "";
        Port = 0;
        UserName = "";
        Password = "";
        BypassAuthCheck = false;
        AuthenticationMode = "";
        OAuthTenantId = "";
        OAuthClientId = "";
        OAuthSecretReference = "";
        OAuthFromAddress = "";
    }

    public AccountDetails(string Host, int Port, string UserName,
        string Password, bool BypassAuthCheck = false) : this()
    {
        this.Host = Host;
        this.Port = Port;
        this.UserName = UserName;
        this.Password = Password;
        this.BypassAuthCheck = BypassAuthCheck;
    }

    public string ResolveMode(out EmailAuthenticationMode mode)
    {
        mode = EmailAuthenticationMode.Legacy;
        string value = (AuthenticationMode ?? "").Trim();
        if (value.Length == 0 ||
            string.Equals(value, "LEGACY", StringComparison.OrdinalIgnoreCase))
            return "";

        if (string.Equals(value, "M365_CLIENT_SECRET",
            StringComparison.OrdinalIgnoreCase))
        {
            mode = EmailAuthenticationMode.Microsoft365ClientSecret;
            return "";
        }

        // Do not include arbitrary input in log messages.
        return "Unsupported email authentication mode";
    }

    public string Validate()
    {
        EmailAuthenticationMode mode;
        string result = ResolveMode(out mode);
        if (!string.IsNullOrEmpty(result)) return result;
        if (string.IsNullOrEmpty(Host))
            return "Empty or invalid Host provided";
        if (Port == 0)
            return "No Port number provided";

        if (mode == EmailAuthenticationMode.Legacy)
        {
            // Preserve the old validation order and error strings.
            if (!BypassAuthCheck && string.IsNullOrEmpty(UserName))
                return "Empty or invalid UserName provided";
            if (!BypassAuthCheck && string.IsNullOrEmpty(Password))
                return "Empty or invalid Password provided";
            return "";
        }

        if (BypassAuthCheck)
            return "OAuth mode cannot be combined with BYPASSAUTHCHECK";
        if (!string.Equals(Host, "smtp.office365.com",
            StringComparison.OrdinalIgnoreCase) || Port != 587)
            return "Microsoft 365 OAuth requires smtp.office365.com on port 587";
        if (string.IsNullOrWhiteSpace(UserName) ||
            string.IsNullOrWhiteSpace(OAuthFromAddress))
            return "OAuth mailbox username and allowed From address are required";

        MimeKit.MailboxAddress mailbox;
        if (!MimeKit.MailboxAddress.TryParse(UserName, out mailbox) ||
            !string.Equals(mailbox.Address, UserName,
                StringComparison.OrdinalIgnoreCase))
            return "OAuth username must be a bare mailbox email address";
        if (!string.Equals(UserName, OAuthFromAddress,
            StringComparison.OrdinalIgnoreCase))
            return "This OAuth profile requires From to match the SMTP mailbox";

        Guid tenant;
        Guid client;
        if (!Guid.TryParse(OAuthTenantId, out tenant) || tenant == Guid.Empty)
            return "A valid OAuth tenant GUID is required";
        if (!Guid.TryParse(OAuthClientId, out client) || client == Guid.Empty)
            return "A valid OAuth client GUID is required";
        if (string.IsNullOrWhiteSpace(OAuthSecretReference) ||
            !OAuthSecretReference.StartsWith("EAOffice/MailKitEmailProcessor/",
                StringComparison.Ordinal) || OAuthSecretReference.Length > 200)
            return "A valid managed OAuth secret reference is required";
        if (!string.IsNullOrEmpty(Password))
            return "Leave the mailbox password empty in OAuth mode";
        return "";
    }

    public string Display()
    {
        return "Host: " + Host + Environment.NewLine +
            "Port: " + Port + Environment.NewLine +
            "UserName: " + UserName + Environment.NewLine +
            "Password: [redacted]" + Environment.NewLine +
            "AuthenticationMode: " +
                (string.IsNullOrEmpty(AuthenticationMode) ? "LEGACY" : AuthenticationMode) +
                Environment.NewLine;
        // Do not add secrets, access tokens or token responses here.
    }

    public string CopyFrom(AccountDetails Source)
    {
        try
        {
            Host = Source.Host;
            Port = Source.Port;
            UserName = Source.UserName;
            Password = Source.Password;
            BypassAuthCheck = Source.BypassAuthCheck;
            AuthenticationMode = Source.AuthenticationMode;
            OAuthTenantId = Source.OAuthTenantId;
            OAuthClientId = Source.OAuthClientId;
            OAuthSecretReference = Source.OAuthSecretReference;
            OAuthFromAddress = Source.OAuthFromAddress;
            return "";
        }
        catch (Exception ex)
        {
            return ex.Message + Environment.NewLine + ex.StackTrace;
        }
    }
}
```

Redacting the existing password display changes diagnostics, not authentication or message delivery. It prevents new work from perpetuating credential logging. Notify support staff that passwords will no longer appear in account displays.

There are **two account copy paths**. In the same file, replace `SendEmailParam.CopyAccountDetailsFrom` with:

```csharp
public string CopyAccountDetailsFrom(AccountDetails Source)
{
    return AccountDetails.CopyFrom(Source);
}
```

The constructor already creates `AccountDetails`, and this delegates all fields through one implementation. Do not update only one copy path or the OAuth mode/settings will be lost before sending. Copying a legacy account over an OAuth account must clear all new values as well.

## 5. Load database settings

Replace `PopulateEmailSettings` in `Actions/RunIdProcessor.cs` with the following method and helper. Existing imports already provide `System`, `System.Data` and collections; existing `FbParameterItem`/Firebird helper usage is retained.

```csharp
private static string ReadSetting(DataRow row, string name)
{
    object value = row[name];
    return value == DBNull.Value ? "" : Convert.ToString(value);
}

private string PopulateEmailSettings(Int64 CompanyClientId,
    ref AccountDetails AccountDetails)
{
    bool loadOAuth = m_ConfigSettings != null &&
        m_ConfigSettings.EnableMicrosoft365OAuth;

    string query =
        " SELECT CO_EMAILHOST, I_CO_EMAILPORT, CO_EMAILUSERNAME," +
        " CO_EMAILPASSWORD, BYPASSAUTHCHECK";
    if (loadOAuth)
        query += ", CO_EMAILAUTHMODE, CO_OAUTHTENANTID, CO_OAUTHCLIENTID," +
            " CO_OAUTHSECRETREF, CO_OAUTHFROMADDRESS";
    query += " FROM EACompanySettings" +
        " WHERE I_SETTINGS_COMPANYID = @CompanyClientId";

    List<FbParameterItem> parameters = new List<FbParameterItem>();
    parameters.Add(new FbParameterItem("CompanyClientId", CompanyClientId));
    UsefulDBMethods.Helpers.WriteParamsToInfoEvent(query, parameters,
        "PopulateEmailSettings", 0);
    WriteParamsToOutput(query, parameters);

    DataTable table = FirebirdUtilities.GetFirstDataTable(
        m_ConnectionString, query, parameters, 0);
    if (table == null || table.Rows.Count == 0)
        return "No record returned for the CompanyClientId: " +
            Convert.ToString(CompanyClientId);

    // Preserve existing multiple-row behaviour. The DBA should establish
    // that there is one settings row per company as part of onboarding.
    foreach (DataRow row in table.Rows)
    {
        AccountDetails.Host = ReadSetting(row, "CO_EMAILHOST");
        object value = row["I_CO_EMAILPORT"];
        AccountDetails.Port = value == DBNull.Value ? 0 : Convert.ToInt32(value);
        AccountDetails.UserName = ReadSetting(row, "CO_EMAILUSERNAME");
        AccountDetails.Password = ReadSetting(row, "CO_EMAILPASSWORD");
        value = row["BYPASSAUTHCHECK"];
        AccountDetails.BypassAuthCheck = value != DBNull.Value &&
            Convert.ToBoolean(value);

        // Explicitly reset every new field if an object is reused.
        AccountDetails.AuthenticationMode = loadOAuth ?
            ReadSetting(row, "CO_EMAILAUTHMODE") : "";
        AccountDetails.OAuthTenantId = loadOAuth ?
            ReadSetting(row, "CO_OAUTHTENANTID") : "";
        AccountDetails.OAuthClientId = loadOAuth ?
            ReadSetting(row, "CO_OAUTHCLIENTID") : "";
        AccountDetails.OAuthSecretReference = loadOAuth ?
            ReadSetting(row, "CO_OAUTHSECRETREF") : "";
        AccountDetails.OAuthFromAddress = loadOAuth ?
            ReadSetting(row, "CO_OAUTHFROMADDRESS") : "";
    }

    EmailAuthenticationMode mode;
    string error = AccountDetails.ResolveMode(out mode);
    if (!string.IsNullOrEmpty(error)) return error;
    // Do not introduce full-account validation into the legacy route.
    return mode == EmailAuthenticationMode.Microsoft365ClientSecret ?
        AccountDetails.Validate() : "";
}
```

No secret is returned by the database. Do not catch missing-column errors and retry with the old query when the switch is true: that could turn intended OAuth into the wrong authentication path. Let the existing execution error handling report a deployment/schema problem.

## 6. New file: `Actions/WindowsCredentialSecretStore.cs`

This is a Windows-specific, .NET 4.5.2-compatible Credential Manager adapter. Keep it separate so a future vault integration replaces only this adapter. The credential is generic, with the target matching the database reference and its blob encoded as UTF-16LE. Never assume arbitrary third-party stored blobs use that encoding.

```csharp
using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace MailKitEmailProcessor.Actions
{
    public sealed class OAuthSetupException : Exception
    {
        // Callers must use only constant/sanitised messages.
        public OAuthSetupException(string message) : base(message) { }
    }

    public sealed class ClientSecretMaterial
    {
        public string Value { get; private set; }
        public string Fingerprint { get; private set; }

        public ClientSecretMaterial(string value, string fingerprint)
        {
            Value = value;
            Fingerprint = fingerprint;
        }
    }

    public interface IClientSecretStore
    {
        ClientSecretMaterial Read(string reference);
    }

    public sealed class WindowsCredentialSecretStore : IClientSecretStore
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct NativeCredential
        {
            public uint Flags;
            public uint Type;
            public IntPtr TargetName;
            public IntPtr Comment;
            public uint LastWrittenLow;
            public uint LastWrittenHigh;
            public uint CredentialBlobSize;
            public IntPtr CredentialBlob;
            public uint Persist;
            public uint AttributeCount;
            public IntPtr Attributes;
            public IntPtr TargetAlias;
            public IntPtr UserName;
        }

        [DllImport("advapi32.dll", EntryPoint = "CredReadW",
            CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CredRead(string target, uint type,
            uint flags, out IntPtr credential);

        [DllImport("advapi32.dll", SetLastError = false)]
        private static extern void CredFree(IntPtr credential);

        public ClientSecretMaterial Read(string reference)
        {
            if (string.IsNullOrWhiteSpace(reference) ||
                !reference.StartsWith("EAOffice/MailKitEmailProcessor/",
                    StringComparison.Ordinal) || reference.Length > 200)
                throw new OAuthSetupException("Invalid OAuth secret reference");

            IntPtr pointer;
            if (!CredRead(reference, 1, 0, out pointer)) // 1 = generic credential
                throw new OAuthSetupException(
                    "OAuth credential unavailable to the execution identity; Windows error " +
                    Marshal.GetLastWin32Error());

            byte[] bytes = null;
            try
            {
                NativeCredential item = (NativeCredential)Marshal.PtrToStructure(
                    pointer, typeof(NativeCredential));
                if (item.CredentialBlobSize == 0 ||
                    item.CredentialBlobSize > 2560 ||
                    item.CredentialBlobSize % 2 != 0 ||
                    item.CredentialBlob == IntPtr.Zero)
                    throw new OAuthSetupException("Invalid stored OAuth credential format");

                bytes = new byte[(int)item.CredentialBlobSize];
                Marshal.Copy(item.CredentialBlob, bytes, 0, bytes.Length);
                string secret = Encoding.Unicode.GetString(bytes);
                if (string.IsNullOrWhiteSpace(secret))
                    throw new OAuthSetupException("Stored OAuth credential is empty");
                using (SHA256 sha = SHA256.Create())
                {
                    return new ClientSecretMaterial(secret,
                        Convert.ToBase64String(sha.ComputeHash(bytes)));
                }
            }
            finally
            {
                if (bytes != null) Array.Clear(bytes, 0, bytes.Length);
                CredFree(pointer);
            }
        }
    }
}
```

The fingerprint detects rotation for the in-memory token cache. It is not written to logs or the database. Managed strings cannot reliably be erased; keep the secret local to token acquisition and never attach it to account objects, exception details or static caches. Windows administrators and the execution identity remain trusted parties.

### Credential provisioning proposal

Provision on the host, as the **exact account that will run the processor**, using the following PowerShell procedure after approval. This writes a generic credential using a masked prompt; the secret never appears in a command-line argument. The C# P/Invoke helper here belongs to the provisioning script, not the application project.

```powershell
$ErrorActionPreference = 'Stop'
Add-Type @'
using System;
using System.Runtime.InteropServices;
public static class GoatCredentialWriter
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct Credential
    {
        public uint Flags;
        public uint Type;
        public string TargetName;
        public string Comment;
        public uint LastWrittenLow;
        public uint LastWrittenHigh;
        public uint CredentialBlobSize;
        public IntPtr CredentialBlob;
        public uint Persist;
        public uint AttributeCount;
        public IntPtr Attributes;
        public string TargetAlias;
        public string UserName;
    }
    [DllImport("advapi32.dll", EntryPoint = "CredWriteW",
        CharSet = CharSet.Unicode, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool Write(ref Credential credential, uint flags);
}
'@
$secretInput = Read-Host 'OAuth client secret value' -AsSecureString
if ($secretInput.Length -eq 0 -or $secretInput.Length -gt 1280) {
    throw 'Invalid credential length'
}
$secretPointer = [Runtime.InteropServices.Marshal]::SecureStringToBSTR($secretInput)
try {
    $credential = New-Object GoatCredentialWriter+Credential
    $credential.Type = 1
    $credential.TargetName = 'EAOffice/MailKitEmailProcessor/IBCM'
    $credential.UserName = 'GOAT application'
    $credential.Persist = 2 # Local-machine persistence for this user's vault
    $credential.CredentialBlobSize = [uint32]($secretInput.Length * 2)
    $credential.CredentialBlob = $secretPointer
    if (-not [GoatCredentialWriter]::Write([ref]$credential, 0)) {
        throw ('Credential write failed; Windows error ' +
            [Runtime.InteropServices.Marshal]::GetLastWin32Error())
    }
}
finally {
    [Runtime.InteropServices.Marshal]::ZeroFreeBSTR($secretPointer)
    $secretInput.Dispose()
}
```

`Persist=2` means the credential persists on this host in the user's vault; it does not make it available to every Windows account. Test retrieval from the actual scheduled execution context. Rotation runs this procedure again with the same target and a replacement secret. The token provider rereads the credential before cache reuse, so a changed value invalidates its token cache. Arrange overlapping valid secrets with IBCM and delete/revoke the old one only after successful testing. Record expiry/rotation reminders operationally; the application cannot derive the secret's expiry from its value.

## 7. New file: `Actions/Microsoft365TokenProvider.cs`

This implementation uses framework `HttpWebRequest` and the existing Newtonsoft.Json DLL. It avoids an MSAL upgrade and uses C# 5-compatible syntax throughout. Request timeout, response size and error output are bounded. It performs no automatic retries; existing run/error handling must reschedule failures deliberately. Missing roles and invalid secrets must not trigger repeated SMTP attempts.

```csharp
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace MailKitEmailProcessor.Actions
{
    public sealed class OAuthAccessToken
    {
        public string Value { get; private set; }
        public DateTime RenewAfterUtc { get; private set; }
        public int ExpiresInSeconds { get; private set; }

        public OAuthAccessToken(string value, DateTime renewAfterUtc, int seconds)
        {
            Value = value;
            RenewAfterUtc = renewAfterUtc;
            ExpiresInSeconds = seconds;
        }
    }

    public sealed class Microsoft365TokenProvider
    {
        private const string Scope = "https://outlook.office365.com/.default";
        private readonly IClientSecretStore secretStore;
        private readonly object sync = new object();
        private readonly Dictionary<string, CacheEntry> cache =
            new Dictionary<string, CacheEntry>(StringComparer.Ordinal);

        private sealed class CacheEntry
        {
            public string Fingerprint;
            public OAuthAccessToken Token;
        }

        public Microsoft365TokenProvider(IClientSecretStore secretStore)
        {
            if (secretStore == null) throw new ArgumentNullException("secretStore");
            this.secretStore = secretStore;
        }

        public OAuthAccessToken GetToken(AccountDetails account)
        {
            string error = account.Validate();
            if (!string.IsNullOrEmpty(error)) throw new OAuthSetupException(error);
            EmailAuthenticationMode mode;
            error = account.ResolveMode(out mode);
            if (!string.IsNullOrEmpty(error) ||
                mode != EmailAuthenticationMode.Microsoft365ClientSecret)
                throw new OAuthSetupException("Token requests require OAuth mode");

            string tenant = new Guid(account.OAuthTenantId).ToString("D");
            string client = new Guid(account.OAuthClientId).ToString("D");
            string key = tenant + "|" + client + "|" + account.OAuthSecretReference +
                "|" + account.UserName.ToLowerInvariant();

            // Serialize acquisition to avoid duplicate requests within this process.
            // A simple lock is adequate for this currently synchronous sender.
            lock (sync)
            {
                ClientSecretMaterial material = secretStore.Read(account.OAuthSecretReference);
                CacheEntry entry;
                if (cache.TryGetValue(key, out entry) &&
                    string.Equals(entry.Fingerprint, material.Fingerprint,
                        StringComparison.Ordinal) &&
                    DateTime.UtcNow < entry.Token.RenewAfterUtc)
                    return entry.Token;

                // Do not retain an old token if replacement acquisition fails.
                cache.Remove(key);
                OAuthAccessToken token = RequestToken(tenant, client, material.Value);
                cache[key] = new CacheEntry {
                    Fingerprint = material.Fingerprint, Token = token
                };
                return token;
            }
        }

        private static string FormValue(string value)
        {
            return Uri.EscapeDataString(value);
        }

        private static string ReadLimited(Stream stream)
        {
            if (stream == null)
                throw new OAuthSetupException("Token endpoint returned no response body");
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                char[] buffer = new char[2048];
                StringBuilder text = new StringBuilder();
                int count;
                while ((count = reader.Read(buffer, 0, buffer.Length)) > 0)
                {
                    if (text.Length + count > 65536)
                        throw new OAuthSetupException("Token endpoint response exceeded size limit");
                    text.Append(buffer, 0, count);
                }
                return text.ToString();
            }
        }

        private static OAuthAccessToken RequestToken(string tenant,
            string client, string secret)
        {
            DateTime startedUtc = DateTime.UtcNow;
            // .NET 4.5.2 HttpWebRequest has no per-request TLS protocol selector.
            // Add TLS 1.2 without removing protocols used by existing integrations.
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(
                "https://login.microsoftonline.com/" + tenant + "/oauth2/v2.0/token");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "application/json";
            request.Timeout = 30000;
            request.ReadWriteTimeout = 30000;
            request.AllowAutoRedirect = false;

            byte[] body = Encoding.UTF8.GetBytes(
                "client_id=" + FormValue(client) +
                "&client_secret=" + FormValue(secret) +
                "&scope=" + FormValue(Scope) +
                "&grant_type=client_credentials");
            request.ContentLength = body.Length;
            try
            {
                using (Stream stream = request.GetRequestStream())
                    stream.Write(body, 0, body.Length);
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new OAuthSetupException("Token endpoint HTTP " +
                            ((int)response.StatusCode).ToString(CultureInfo.InvariantCulture));

                    JObject json;
                    try { json = JObject.Parse(ReadLimited(response.GetResponseStream())); }
                    catch (Newtonsoft.Json.JsonException)
                    {
                        throw new OAuthSetupException("Token endpoint returned invalid JSON");
                    }

                    string type = (string)json["token_type"];
                    string value = (string)json["access_token"];
                    int seconds;
                    if (!string.Equals(type, "Bearer", StringComparison.OrdinalIgnoreCase) ||
                        string.IsNullOrWhiteSpace(value) || value.Length > 60000 ||
                        !int.TryParse(Convert.ToString(json["expires_in"],
                            CultureInfo.InvariantCulture), NumberStyles.Integer,
                            CultureInfo.InvariantCulture, out seconds) ||
                        seconds < 60 || seconds > 86400)
                        throw new OAuthSetupException("Token endpoint response has invalid fields");

                    RequireSmtpRole(value);
                    int margin = Math.Min(300, Math.Max(10, seconds / 10));
                    DateTime renew = startedUtc.AddSeconds(seconds - margin);
                    if (renew <= DateTime.UtcNow)
                        throw new OAuthSetupException("Token response arrived too late to reuse safely");
                    return new OAuthAccessToken(value, renew, seconds);
                }
            }
            catch (WebException ex)
            {
                HttpWebResponse response = ex.Response as HttpWebResponse;
                if (response == null)
                    throw new OAuthSetupException("Token endpoint request failed: " + ex.Status);
                using (response)
                {
                    string aadCode = "";
                    try
                    {
                        // Extract a numeric AADSTS identifier only; never echo raw JSON.
                        string text = ReadLimited(response.GetResponseStream());
                        Match match = Regex.Match(text, @"\bAADSTS[0-9]{5,10}\b");
                        if (match.Success) aadCode = " " + match.Value;
                    }
                    catch { /* retain HTTP status even if the error body is unreadable */ }
                    throw new OAuthSetupException("Token endpoint HTTP " +
                        ((int)response.StatusCode).ToString(CultureInfo.InvariantCulture) + aadCode);
                }
            }
            finally { Array.Clear(body, 0, body.Length); }
        }

        private static void RequireSmtpRole(string token)
        {
            // Local diagnostic inspection for the particular handoff profile.
            // This is not signature validation; Exchange validates the bearer token.
            try
            {
                string[] parts = token.Split('.');
                if (parts.Length != 3) throw new FormatException();
                string encoded = parts[1].Replace('-', '+').Replace('_', '/');
                int remainder = encoded.Length % 4;
                if (remainder == 1) throw new FormatException();
                if (remainder != 0) encoded = encoded.PadRight(
                    encoded.Length + 4 - remainder, '=');
                JObject claims = JObject.Parse(Encoding.UTF8.GetString(
                    Convert.FromBase64String(encoded)));
                JArray roles = claims["roles"] as JArray;
                if (roles != null)
                    foreach (JToken role in roles)
                        if (role.Type == JTokenType.String &&
                            string.Equals((string)role, "SMTP.SendAsApp",
                                StringComparison.Ordinal))
                            return;
            }
            catch
            {
                throw new OAuthSetupException("Cannot inspect the SMTP application token locally");
            }
            throw new OAuthSetupException("Token lacks the required SMTP.SendAsApp role");
        }
    }
}
```

The token is an in-process secret; do not log `OAuthAccessToken.Value`, serialise the object or override `ToString` to return it. The static sender in section 8 reuses one provider per process, so a large RunId can reuse a token. Separate process launches each acquire their own token. The cache lock serialises token acquisitions for different accounts too; this is an intentional simple first implementation, not a distributed cache.

The global TLS line runs only on OAuth acquisition and preserves existing flags. It can nevertheless influence other `HttpWebRequest` calls in the same process by allowing TLS 1.2 negotiation. Test existing HTTPS document/image downloads on a mixed-client deployment. Do not temporarily toggle global protocols around a request, which would race concurrent work. MailKit SMTP has separate TLS handling; this line does not configure MailKit's TLS stack. Never bypass either certificate validator.

The response reader limits memory growth; `Timeout` and `ReadWriteTimeout` bound request/read waits but are not a single strict deadline for an entire trickling HTTP response. A strict wall-clock cancellation design is a separately reviewable improvement if operational requirements demand it.

The role check deliberately supports the handoff's `SMTP.SendAsApp` grant model. Do not claim support for alternative Exchange permission models whose tokens omit that role. If Microsoft changes token representation, fail and investigate; do not parse remote tokens on a website or silently skip the role check.

## 8. New file: `Actions/Microsoft365OAuthSmtpSender.cs`

This helper sends the **already composed** MIME message. It does not duplicate attachment/image/body building. It sets recipients from the existing lists, validates the configured From identity and explicitly uses the same identity for the SMTP envelope.

```csharp
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace MailKitEmailProcessor.Actions
{
    public static class Microsoft365OAuthSmtpSender
    {
        private static readonly Microsoft365TokenProvider Tokens =
            new Microsoft365TokenProvider(new WindowsCredentialSecretStore());

        private static void AddRecipients(InternetAddressList destination,
            List<NameAndAddressItem> source)
        {
            destination.Clear();
            foreach (NameAndAddressItem item in source)
                destination.Add(new MailboxAddress(item.Name, item.Address));
        }

        private static string Redact(string text, string token, string mailbox)
        {
            if (text == null) return "";
            if (!string.IsNullOrEmpty(token))
            {
                text = text.Replace(token, "[redacted]");
                string payload = "user=" + mailbox + "\u0001auth=Bearer " +
                    token + "\u0001\u0001";
                text = text.Replace(Convert.ToBase64String(
                    Encoding.UTF8.GetBytes(payload)), "[redacted]");
            }
            return text;
        }

        public static string Send(AccountDetails account, MimeMessage message,
            List<NameAndAddressItem> recipients,
            List<NameAndAddressItem> cc,
            List<NameAndAddressItem> bcc, int runId)
        {
            string bearer = null;
            string phase = "configuration";
            bool accepted = false;
            try
            {
                string error = account.Validate();
                if (!string.IsNullOrEmpty(error)) throw new OAuthSetupException(error);
                EmailAuthenticationMode mode;
                error = account.ResolveMode(out mode);
                if (!string.IsNullOrEmpty(error) ||
                    mode != EmailAuthenticationMode.Microsoft365ClientSecret)
                    throw new OAuthSetupException("OAuth sender requires OAuth mode");

                if (message.From.Count != 1 || message.From.Mailboxes == null)
                    throw new OAuthSetupException("OAuth requires one configured From mailbox");
                MailboxAddress from = message.From[0] as MailboxAddress;
                if (from == null || !string.Equals(from.Address,
                    account.OAuthFromAddress, StringComparison.OrdinalIgnoreCase))
                    throw new OAuthSetupException("Message From conflicts with the OAuth sending mailbox");
                if (message.Sender != null && !string.Equals(message.Sender.Address,
                    account.OAuthFromAddress, StringComparison.OrdinalIgnoreCase))
                    throw new OAuthSetupException("Message Sender conflicts with the OAuth sending mailbox");

                AddRecipients(message.To, recipients);
                AddRecipients(message.Cc, cc);
                AddRecipients(message.Bcc, bcc);
                List<MailboxAddress> envelopeRecipients = new List<MailboxAddress>();
                foreach (MailboxAddress item in message.To.Mailboxes) envelopeRecipients.Add(item);
                foreach (MailboxAddress item in message.Cc.Mailboxes) envelopeRecipients.Add(item);
                foreach (MailboxAddress item in message.Bcc.Mailboxes) envelopeRecipients.Add(item);
                if (envelopeRecipients.Count == 0)
                    throw new OAuthSetupException("OAuth send requires at least one recipient");

                phase = "token acquisition";
                OAuthAccessToken token = Tokens.GetToken(account);
                bearer = token.Value;
                CommonMethods.WriteToOutputLog(runId,
                    "OAuth token ready; expires_in=" + token.ExpiresInSeconds +
                    "; SMTP.SendAsApp present; UTC=" + DateTime.UtcNow.ToString("o"));

                using (SmtpClient client = new SmtpClient())
                {
                    client.Timeout = 30000; // New profile only; leave legacy timeout unchanged.
                    phase = "SMTP STARTTLS connection";
                    client.Connect(account.Host, account.Port,
                        SecureSocketOptions.StartTls, CancellationToken.None);
                    if (!client.AuthenticationMechanisms.Contains("XOAUTH2"))
                        throw new OAuthSetupException("SMTP server does not advertise XOAUTH2");

                    phase = "SMTP XOAUTH2 authentication";
                    client.Authenticate(new SaslMechanismOAuth2(account.UserName, bearer),
                        CancellationToken.None);
                    CommonMethods.WriteToOutputLog(runId, "OAuth SMTP authenticated");

                    phase = "SMTP submission";
                    string response = client.Send(message,
                        new MailboxAddress("", account.OAuthFromAddress),
                        envelopeRecipients, CancellationToken.None, null);
                    // Once accepted, QUIT failure must not invite duplicate resubmission.
                    accepted = true;
                    CommonMethods.WriteToOutputLog(runId,
                        "OAuth SMTP accepted: " + Redact(response, bearer, account.UserName));
                    try { client.Disconnect(true); }
                    catch { /* Dispose closes transport; submission already accepted. */ }
                }
                return "";
            }
            catch (OAuthSetupException ex)
            {
                return Report(runId, "Error: OAuth " + phase + ": " + ex.Message);
            }
            catch (SmtpCommandException ex)
            {
                if (accepted) return "";
                return Report(runId, "Error: OAuth " + phase + ": SMTP " +
                    (int)ex.StatusCode + ": " + Redact(ex.Message, bearer, account.UserName));
            }
            catch (MailKit.Security.AuthenticationException ex)
            {
                return Report(runId, "Error: OAuth " + phase + ": " +
                    Redact(ex.Message, bearer, account.UserName));
            }
            catch (Exception ex)
            {
                if (accepted) return "";
                // Do not echo arbitrary HTTP/JSON/credential exception messages.
                // During SMTP submission a transport failure can be ambiguous.
                return Report(runId, "Error: OAuth " + phase + " failed (" +
                    ex.GetType().Name + "). " +
                    (phase == "SMTP submission" ?
                        "Delivery state is uncertain; review before resending." : ""));
            }
        }

        private static string Report(int runId, string error)
        {
            CommonMethods.WriteToOutputLog(runId, error);
            return error;
        }
    }
}
```

The helper returns the same contract as the existing sender: empty string means SMTP accepted the message, and a nonempty error string means failure/uncertain submission. It performs **no automatic resend**. A timeout after DATA can mean Microsoft accepted the message even though the client did not receive the acknowledgement. The “uncertain” message is operational guidance, not proof of rejection.

After acceptance, disposal/logging failures should not mark the email unsent. The generic exception branch protects the accepted state; log infrastructure failures still need monitoring outside this method. Review whether `Report` can itself throw in the deployment's logging implementation and retain the existing top-level error handler. There is no attempt here to redesign the entire status/log subsystem.

## 9. Integrate into `Actions/SendEmail.cs`

### First insertion: resolve mode near method entry

In the overload that takes `AccountDetails`, immediately after `string Result = "";` and before MIME construction, insert:

```csharp
EmailAuthenticationMode authenticationMode;
Result = AccountDetails.ResolveMode(out authenticationMode);
if (string.IsNullOrEmpty(Result) &&
    authenticationMode == EmailAuthenticationMode.Microsoft365ClientSecret)
    Result = AccountDetails.Validate();
if (!string.IsNullOrEmpty(Result))
{
    Result = "Error: " + Result;
    CommonMethods.WriteToOutputLog(RunId, Result);
    return Result;
}
```

Resolve unknown modes before any SMTP action. Only OAuth receives new validation here; do not call `SendEmailParam.Validate()` for all accounts, because that introduces additional requirements and could change legacy behaviour.

### Second insertion: branch after MIME composition

Immediately after the existing `message.Body = multipart;`, and before the existing `try` containing `new SmtpClient()`, insert:

```csharp
if (authenticationMode == EmailAuthenticationMode.Microsoft365ClientSecret)
{
    return Microsoft365OAuthSmtpSender.Send(AccountDetails, message,
        Recipients, CCRecipients, BCCRecipients, RunId);
}
```

Leave the **entire original SMTP try/catch** below it intact for legacy accounts. Its conditional `Authenticate(AccountDetails.UserName, AccountDetails.Password)` and mandatory STARTTLS remain unchanged. This prevents OAuth from overwriting the old password/bypass branch and makes regression review straightforward.

The existing MIME construction occurs before the SMTP try/catch and can throw for missing assets or invalid addresses. This plan does not relocate that legacy code. OAuth-specific configuration is checked first; the existing caller's error handling still covers pre-send composition failures. Do not mistakenly report such failures as token failures.

### Fixed From and Reply-To

Configure IBCM's `EventRunShowClients` source data so `FromAddress` is `goat@ibcm.com.au` and `ReplyToAddress` is the approved monitored mailbox. The procedure definition is not in this repository; the database/application owner must identify its underlying data or update the procedure in its own migration project.

Do not silently replace a message's From in the processor: that would leave generated requests and stored sent metadata inconsistent with what was submitted. The helper rejects a conflict so the source data can be fixed. Display names can remain whatever the business approves.

The existing pipeline sets blank Reply-To to From. That behaviour stays unchanged for other clients. For IBCM, ensure a nonblank approved Reply-To upstream, otherwise replies will bounce by design. A Reply-To address changes user replies but not the SMTP envelope sender or delivery-failure routing.

## 10. Project registration, JSON diagnostics and onboarding logs

### `.csproj`

Add these entries to the existing `Compile` item group in `MailKitEmailProcessor.csproj`:

```xml
<Compile Include="Actions\WindowsCredentialSecretStore.cs" />
<Compile Include="Actions\Microsoft365TokenProvider.cs" />
<Compile Include="Actions\Microsoft365OAuthSmtpSender.cs" />
```

No extra NuGet package is required by this design. Cryptography, P/Invoke, HTTP and collections are already in the framework references; Newtonsoft.Json, MailKit and MimeKit are existing local references. Keep:

```xml
<TargetFrameworkVersion>v4.5.2</TargetFrameworkVersion>
```

Use ordinary C# 5 syntax. Do not introduce `using var`, interpolated strings, nullable reference types, tuples, `HttpClient` APIs from modern .NET, `System.Text.Json`, `RandomNumberGenerator.Fill`, `DateTime.UnixEpoch`, or `Task.WaitAsync`. The sample code intentionally avoids them.

### Existing JSON request builders

The current methods are **`BuildOriginalJsonRequest`** and **`BuildJsonRequest`** in `Actions/RunIdProcessor.cs`. Both export `emailPassword`. Their generated requests are not the `/FromFile:` input format. Do not assume these two contracts can be interchanged.

Keep the legacy request shape and password values unchanged until their consumers are reviewed. For OAuth, `Password` is already required empty, so the existing `emailPassword` export stays empty. Immediately after each existing `emailPassword` property append, add:

```csharp
EmailAuthenticationMode requestMode;
string modeError = EmailSettings.ResolveMode(out requestMode);
if (!string.IsNullOrEmpty(modeError))
    throw new OAuthSetupException(modeError);
if (requestMode == EmailAuthenticationMode.Microsoft365ClientSecret)
{
    Request.Append(JsonUtilities.AddPropertyNameAndValue(
        "emailAuthenticationMode", "M365_CLIENT_SECRET"));
    Request.Append(JsonUtilities.AddPropertyNameAndValue(
        "requiresCompanyOAuthSettings", "Y"));
}
```

These two fields explicitly identify an OAuth request without secrets. They are diagnostic markers, not a complete replay contract. Consumers that need to send must retrieve approved company settings on the sending host or receive a separately versioned contract. Verify downstream readers tolerate additive properties before enabling IBCM. The new properties must follow the JSON utility's existing comma convention; parse generated output as JSON in verification rather than judging commas by eye.

Never add the secret or bearer token to these builders, `SendEmailParam.Display`, `BuildOriginalJsonRequest` logs, database parameters or output logs. A credential reference is not itself a password but should not be exported gratuitously. Existing legacy password export is an existing behaviour requiring a separate consumer review; changing it globally is not necessary to enable OAuth.

### Optional protocol transcript for the handoff test

Ordinary phase logs above do not produce the handoff's complete SMTP session. Enable a temporary controlled transcript for the **OAuth test only**, using MailKit's built-in secret redaction, and then disable it for routine production.

The installed MailKit assembly was inspected: version `3.0.0.0` exposes `ProtocolLogger(string, bool)`, `RedactSecrets`, `LogTimestamps`, and `SmtpClient(IProtocolLogger)`. Verify deployed DLLs match. For a controlled developer test, replace the helper's `using (SmtpClient client = new SmtpClient())` with these nested using blocks and retain its body:

```csharp
string transcript = System.IO.Path.Combine(
    AppDomain.CurrentDomain.BaseDirectory,
    "IBCM-SMTP-" + Guid.NewGuid().ToString("N") + ".log");
using (MailKit.ProtocolLogger logger = new MailKit.ProtocolLogger(transcript, false))
{
    logger.RedactSecrets = true;
    logger.LogTimestamps = true;
    using (SmtpClient client = new SmtpClient(logger))
    {
        // Existing OAuth connection/authentication/submission body goes here.
    }
}
```

This is an explicitly optional substitution, not an additional SMTP session. Use a harmless test message: protocol transcripts can contain message bodies, recipients and attachments despite authentication redaction. Restrict file ACLs and retention. Before sharing anything, verify the token and its XOAUTH2 base64 representation are absent, including when authentication fails or uses a continuation response. Do not assume redaction is correct just because the property is true; test it against the actual library. Token HTTP request/response bodies must never be logged.

The first report must include UTC/time zone, product/library version, token HTTP status and expiry, role result, SMTP response codes, exact safe error text, and external reception evidence. The helper logs readiness only after HTTP 200 and role checking; include that meaning in the report. Screenshots of an SMTP setup screen must come from the owning application, because this repository is a console processor.

## 11. File entry point and administration changes

### Initial release: reject OAuth files explicitly

In `Actions/FileNameProcessor.cs`, after successfully parsing `JsonSource` and before reading SMTP fields, insert:

```csharp
if (string.IsNullOrEmpty(Result))
{
    JToken modeToken = JsonSource.SelectToken("$.EmailAuthenticationMode");
    string requestedMode = modeToken == null || modeToken.Type == JTokenType.Null ?
        "" : Convert.ToString(modeToken);
    if (!string.IsNullOrWhiteSpace(requestedMode) &&
        !string.Equals(requestedMode.Trim(), "LEGACY",
            StringComparison.OrdinalIgnoreCase))
        Result = "OAuth file input is not supported in this release; use RunId/company settings";
}
```

Old files without this property behave as before. No JSON flag should allow an unimplemented OAuth request to be treated as password authentication. Maintain this rejection until the full optional contract is implemented and reviewed.

### If file-based OAuth is confirmed as required

Replace the rejection with an actual mode loader, and then change the existing username/password-reading blocks to be conditional. Do not combine this optional path with the rejection above. Add these two helper methods to `FileNameProcessor`:

```csharp
private static string OptionalString(JObject source, string path)
{
    JToken token = source.SelectToken(path);
    if (token == null || token.Type == JTokenType.Null) return "";
    if (token.Type != JTokenType.String)
        throw new OAuthSetupException("An OAuth JSON setting has an invalid type");
    return (string)token;
}

private static string LoadOAuthFileAccount(JObject source, AccountDetails account)
{
    try
    {
        account.AuthenticationMode = OptionalString(source, "$.EmailAuthenticationMode");
        EmailAuthenticationMode mode;
        string error = account.ResolveMode(out mode);
        if (!string.IsNullOrEmpty(error)) return error;
        if (mode == EmailAuthenticationMode.Legacy) return "";

        account.OAuthTenantId = OptionalString(source, "$.OAuthTenantId");
        account.OAuthClientId = OptionalString(source, "$.OAuthClientId");
        account.OAuthSecretReference = OptionalString(source, "$.OAuthSecretReference");
        account.OAuthFromAddress = OptionalString(source, "$.OAuthFromAddress");
        account.UserName = OptionalString(source, "$.SmtpUserName");
        account.Password = OptionalString(source, "$.SmtpPassword");
        JToken bypass = source.SelectToken("$.BypassAuthCheck");
        if (bypass != null && bypass.Type != JTokenType.Null)
        {
            if (bypass.Type != JTokenType.Boolean)
                return "BypassAuthCheck must be a JSON Boolean";
            account.BypassAuthCheck = (bool)bypass;
        }
        // Host and port have not been loaded yet; validate at SendEmail entry.
        return "";
    }
    catch (OAuthSetupException ex) { return ex.Message; }
}
```

After JSON parsing, insert the following block instead of the initial-release rejection. Declare `fileAuthenticationMode` once in `Execute`, in the same scope as its existing `Result` variable:

```csharp
EmailAuthenticationMode fileAuthenticationMode = EmailAuthenticationMode.Legacy;
if (string.IsNullOrEmpty(Result))
    Result = LoadOAuthFileAccount(JsonSource, SendEmailParam.AccountDetails);
if (string.IsNullOrEmpty(Result))
    Result = SendEmailParam.AccountDetails.ResolveMode(out fileAuthenticationMode);
```

Read host and port as today. Replace only the existing username and password blocks with:

```csharp
if (string.IsNullOrEmpty(Result) &&
    fileAuthenticationMode == EmailAuthenticationMode.Legacy)
{
    Path = "$.SmtpUserName";
    SendEmailParam.AccountDetails.UserName = CommonMethods.GetJsonPathValueAsString(
        JsonSource, Path, "Email User Name", out Result);
}
if (string.IsNullOrEmpty(Result) &&
    fileAuthenticationMode == EmailAuthenticationMode.Legacy)
{
    Path = "$.SmtpPassword";
    SendEmailParam.AccountDetails.Password = CommonMethods.GetJsonPathValueAsString(
        JsonSource, Path, "Email Password", out Result);
}
```

The optional helper already loaded those fields for OAuth without requiring a mailbox password. Other required fields remain unchanged. The initialisation to `Legacy` satisfies C# definite assignment when parsing has failed. The optional file route is selected explicitly by its input mode; it does not load the RunId deployment settings JSON and therefore does not use `EnableMicrosoft365OAuth`. Release it only when its producer and access controls have also been approved.

Add optional `ReplyToAddress` loading after the file's `FromAddress` block, for OAuth only:

```csharp
if (string.IsNullOrEmpty(Result) &&
    fileAuthenticationMode == EmailAuthenticationMode.Microsoft365ClientSecret)
{
    try
    {
        string replyTo = OptionalString(JsonSource, "$.ReplyToAddress");
        SendEmailParam.Sender.ReplyToAddress = string.IsNullOrWhiteSpace(replyTo) ?
            SendEmailParam.Sender.Address : replyTo;
    }
    catch (OAuthSetupException ex) { Result = ex.Message; }
}
```

Example additive fields for an OAuth file (merge into a complete existing valid message JSON; this is not a standalone send request):

```json
{
  "EmailAuthenticationMode": "M365_CLIENT_SECRET",
  "SmtpServer": "smtp.office365.com",
  "SmtpPort": 587,
  "SmtpUserName": "goat@ibcm.com.au",
  "SmtpPassword": "",
  "BypassAuthCheck": false,
  "OAuthTenantId": "0299386a-a135-4d14-9d36-b571b6e3a637",
  "OAuthClientId": "da247c5e-5598-4a2d-9391-7277f282a828",
  "OAuthSecretReference": "EAOffice/MailKitEmailProcessor/IBCM",
  "OAuthFromAddress": "goat@ibcm.com.au",
  "FromAddress": "goat@ibcm.com.au",
  "ReplyToAddress": "<approved-monitored-address>"
}
```

Treat this input as trusted application configuration with file ACLs and controlled producers. An arbitrary caller who can name credential references and execute sends under the service identity can potentially use that identity's credentials. The initial database route avoids granting a new untrusted file interface by default.

### Upstream settings UI

The UI is outside this repository. Its owner must add a mode selector, tenant/client GUIDs, credential reference and allowed From. In OAuth mode, disable/clear mailbox password and bypass controls. Preserve the old screen behaviour for legacy clients. Client secret entry/rotation belongs to the approved vault provisioning workflow, not a field that persists into `EACompanySettings` or request files. Update upstream validation and permissions accordingly.

## 12. Verification, acceptance and release checklist

### Build and review

The main `AccountDetails`/enum and all three complete new application classes were extracted from this document and successfully compiled in memory with `/langversion:5 /nostdlib+`, the installed .NET Framework **4.5.2 reference assemblies**, and the repository's MailKit, MimeKit and Newtonsoft.Json DLLs. Minimal temporary in-memory stand-ins supplied the existing logging and recipient types. This check found no compiler errors and wrote no application source changes.

That verifies the proposed core code's syntax and referenced APIs, not the entire integration, native Credential Manager behaviour, Firebird migration or live Microsoft service. The loader/insertion fragments and optional file route still require compilation within the real project after implementation. The implementing developer must complete those checks and resolve any deployed dependency discrepancies before merge.

```powershell
msbuild MailKitEmailProcessor.sln /t:Build /p:Configuration=Debug
msbuild MailKitEmailProcessor.sln /t:Build /p:Configuration=Release
powershell -NoProfile -ExecutionPolicy Bypass -File tests/Verify-Reconciliation.ps1 -AssemblyPath bin/Release/MailKitEmailProcessor.exe -ExpectBypass
```

The existing reconciliation checks exercise legacy account validation/copying but do not exercise live SMTP or OAuth. Extend/add focused checks for the new modes and cache/error behaviours. Keep tests separate from production data. A senior reviewer should verify the P/Invoke layout on both the actual 32-bit-preferred build and any 64-bit host/test process, and verify token/SMTP errors cannot leak credentials.

### Required test matrix

| Test | Expected result |
| --- | --- |
| Existing database without new columns, switch absent/false | Original query succeeds; no new schema dependency |
| Migrated database, NULL mode, switch true | Existing company behaves as before |
| Legacy password account | Same host/port/TLS, credentials and MIME behaviour |
| Legacy bypass, blank username/password | No AUTH; host/port still required |
| Unknown nonempty mode | Error before SMTP; no fallback |
| Copy OAuth account via both copy methods | All new values survive |
| Copy legacy over OAuth | New values cleared; no stale client identity |
| OAuth with bypass true/nonempty password | Clear configuration failure |
| Invalid tenant/client/host/mailbox | Error before secret/token request |
| Vault read under incorrect Windows identity | Safe error, no SMTP, no credential value in logs |
| Secret with `+`, `&`, `=` and other special characters | Correct form encoding; no parameter splitting |
| HTTP 400 invalid/expired secret | HTTP status and safe AADSTS code; no response body dump |
| HTTP redirect | Not followed with credentials |
| Missing role or malformed token response | Stop before SMTP |
| Valid token reused across two messages | One acquisition within a process until renewal threshold |
| Expiry/renewal threshold | New token obtained before expiry; no refresh-token use |
| Secret rotation | Fingerprint change forces acquisition with replacement secret |
| Different tenants/apps/mailboxes | Separate cache entries |
| SMTP has no STARTTLS/XOAUTH2 | Fail; no downgrade |
| From/Sender conflict | Reject; never send as arbitrary mailbox |
| To/CC/BCC and attachments/images | Preserve recipients/content; BCC headers remain hidden |
| SMTP failure before submission | Error; no successful sent-status update |
| Transport failure during submission | Uncertain state reported; no blind retry |
| QUIT failure after acceptance | Success remains accepted; no duplicate resend |
| Dry run with send/status switches both `N` | Neither SMTP sending nor sent-state updates |
| Transcript on successful and failed authentication | No raw token or encoded AUTH payload |
| Existing request JSON | Parseable and unchanged for legacy; OAuth markers present and password blank |
| Mixed account HTTPS downloads | TLS 1.2 addition does not disrupt asset retrieval |

Use stubbed secret stores for token-provider tests. For deterministic expiry/HTTP tests, the developer may extract clock/HTTP transport interfaces before writing tests; keep the production tenant host fixed, and do not introduce an arbitrary database token endpoint for testing. Stub SMTP verifies the authentication sequence; the final Microsoft acceptance test remains essential.

### Live IBCM acceptance

1. Confirm IBCM's app permission/admin consent, Exchange principal, mailbox grants and SMTP AUTH exception are effective. The handoff says these exist but are untested.
2. Provision the secret as the real execution identity. Confirm DNS, outbound HTTPS 443 and SMTP 587 and system time.
3. Run a single controlled message to an external mailbox you control, from `goat@`, with approved Reply-To.
4. Record token HTTP 200/expiry/role result, SMTP `235`, submission response and received headers. Verify the message externally; submission alone is not proof of inbox delivery.
5. If a step fails, stop and give the handoff sender a redacted partial report. For first-day `535 5.7.3`, check configuration and arrange a delayed retry after permission propagation; do not implement an hours-long blocking loop.
6. Repeat after token renewal and after a controlled secret rotation. Confirm the full production scheduling identity works.
7. Complete the PDF's screenshots/configuration/library/version/report requirements without sharing credentials or bearer tokens.

No mailbox reception is implemented. Replies or delivery-failure notices addressed to `goat@` are rejected by IBCM's rule; agree with IBCM how Exchange message tracing will support delivery investigation.

### Capacity and production gate

Microsoft documents three concurrent submission connections, 30 messages/minute and 10,000 recipients/day for this sending service in [SMTP submission throttling guidance](https://learn.microsoft.com/en-us/troubleshoot/exchange/send-emails/smtp-submission-improvements). Assess the entire mailbox workload, including other processes and CC/BCC fan-out. These snippets do not enforce those limits. For a small controlled rollout, limit scheduler concurrency and agreed batch volume. If existing production jobs can exceed them, a separately specified queue/pacing change is required **before** production acceptance.

Do not interpret the current `RetryCount` as SMTP retries: it is used while waiting for document creation. Preserve current sent-status flags and distinguish accepted submission from final delivery. Do not automatically resend authentication failures, missing-role failures or ambiguous post-DATA failures.

### Deployment order

1. Approve scope, credential store, entry point, Reply-To and capacity policy.
2. Implement/review/build the new source in development; keep the feature switch false by default.
3. Apply the additive migration only to the deployment(s) enabling OAuth. Back up and record migration completion.
4. Deploy the matching executable, config and dependency DLLs to test; provision the credential.
5. Set the switch true only after schema verification. Activate only the intended company row and sender/Reply-To data.
6. Complete tests and handoff feedback. Stage production rollout; monitor errors and credential expiry.
7. For rollback, stop IBCM sends, restore its compatible configuration if available, then disable/remove the new deployment as required. Leave other clients' settings intact. Retain additive columns during rollback; do not destructively remove schema or erase old client passwords.

## 13. What this document intentionally leaves for review

The core proposed implementation is complete for the RunId-based, Windows Credential Manager, secret-based SMTP OAuth profile. Remaining environmental/business choices are explicit: the real Windows identity/vault acceptance, database deployment tooling, Reply-To, actual entry point, generated-request consumers and workload controls. Do not let a junior developer guess those production settings.

No framework upgrade, broad SMTP rewrite, app-secret database column, password workaround or automatic authentication fallback is required. The design adds one opt-in account profile and isolates token/credential handling while preserving the old sending path.
