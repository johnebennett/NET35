# BYPASSAUTHCHECK

Ported from `../MailKitEmailProcessorHtml/4.8Full-Test-Castran` while retaining this project's references to `bin/Debug` assemblies, database helpers, and logging.

- Database input: `EACOMPANYSETTINGS.BYPASSAUTHCHECK` must exist before running database processing. Use `1` to bypass SMTP authentication, or `0`/NULL to authenticate. Native boolean values are also supported.
- Email JSON input: set the top-level property `"BypassAuthCheck": true` to bypass authentication. Omitted, false, or null keeps authentication enabled. Non-boolean values are rejected.
- Bypass allows empty SMTP username/password and skips `client.Authenticate`. Host and port remain required; STARTTLS remains enabled.
- The flag is preserved when account details are copied into send parameters.

Build: `msbuild MailKitEmailProcessor.csproj /t:Build /p:Configuration=Debug`
Output: `bin/Debug/MailKitEmailProcessor.exe`.

No database schema changes or live SMTP sends were performed.
Original modified sources and previous application build files were backed up to: "E:\Source\EAOffice\HelperApps\MailKitEmailProcessorHtml-Castran\Backups\BypassAuthCheck-20260912-182225".

