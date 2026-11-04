---
cssclasses:
  - project-reference
---

# ProgressLogging project reference

> [[index|Combined index]] Â· [[ActionBase|ActionBase]] Â· [[CommonUtilities|CommonUtilities]] Â· [[ProgressLogging|ProgressLogging]] Â· [[RequestProcessor|RequestProcessor]] Â· [[UsefulDBMethods|UsefulDBMethods]]

Declared classes and methods from the five selected projects’ Compile source entries (Debug symbols: DEBUG, TRACE). Partial classes are combined; nested classes use their containing class name. Inherited methods, properties, accessors, fields and compiler-generated members are excluded.

Namespaces and classes are alphabetized. Methods are alphabetized within public, private, then other visibility groups; overloads are sorted by signature. Constructors appear separately.

*1 namespaces · 1 classes · 16 methods*

## Namespaces

- [[#EAOfficeAPI|EAOfficeAPI]]

## EAOfficeAPI

- [[#ProgressLogging|ProgressLogging]]

---

### ProgressLogging

Project: [[ProgressLogging#ProgressLogging|ProgressLogging]]

*Source: [ProgressLogging.Configuration.cs](../../ProgressLogging/ProgressLogging.Configuration.cs), [ProgressLogging.cs](../../ProgressLogging/ProgressLogging.cs)*

#### public methods (12)

- `public static bool AddRecord(bool LogEntry, int MasterId, string Plugin, string Command, string Method, string Content, JsonContentEntryType EntryType, int CompanyId, int ClientId, out int NewId, out string ErrorMsg)`
- `public static bool AddRecord(bool LogEntry, int MasterId, string Plugin, string Command, string Method, string EntryType, string Content, int CompanyId, int ClientId, out int NewRecordId, out string ErrorMsg)`
- `public bool BuildContent(int StatusCode, int RecordId, string Comment, string Message, string DataAsJson, string Plugin, string Command, string Method, out string Content, out string ErrorMsg)`
- `public static bool BuildContentAndLog(bool LogEntry, int MasterId, GeneralResponseStatus StatusCode, string Comment, string Message, string Plugin, string Command, string Method, JsonContentEntryType EntryType, string DataAsJson, int CompanyId, int ClientId, out int NewRecordId, out string Content, out string ErrorMsg)`
- `public static bool BuildContentAndLog(bool LogEntry, int MasterId, int StatusCode, string Comment, string Message, string Plugin, string Command, string Method, string EntryType, string DataAsJson, int CompanyId, int ClientId, out int NewRecordId, out string Content, out string ErrorMsg)`
- `public static string BuildSqliteConnectionString(out string ErrorMsg)`
- `public static void ConfigureConnectionStringProvider(Func<string> provider)`
- `public static string CurrentLoggingFileName()`
- `public static string GetEntryTypeString(JsonContentEntryType EntryType)`
- `public static int GetNextRecordId(string ConnectionString, int MasterId, int CompanyId, int ClientId, out string ErrorMsg)`
- `public bool InsertRecordWithValues(string ConnectionString, int MasterId, int CompanyId, int ClientId, string Plugin, string Command, string Method, string EntryType, string Content, int NewRecordId, out string ErrorMsg)`
- `public bool UpdateRecordWithValues(string ConnectionString, bool LogEntry, string Plugin, string Command, string Method, string EntryType, string Content, int NewRecordId, out string ErrorMsg)`

#### private methods (4)

- `private string CreateCurrentLoggingDB(out string FileName /*out string ErrorMsg*/)`
- `private static bool IsWhiteSpace(string value)`
- `private string TableStructure()`
- `private static bool TryGetConnectionString(out string connectionString, out string errorMsg)`

