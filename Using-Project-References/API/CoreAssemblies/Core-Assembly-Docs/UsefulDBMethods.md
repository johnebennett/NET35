---
cssclasses:
  - project-reference
---

# UsefulDBMethods project reference

> [[index|Combined index]] · [[ActionBase|ActionBase]] · [[CommonUtilities|CommonUtilities]] · [[ProgressLogging|ProgressLogging]] · [[RequestProcessor|RequestProcessor]] · [[UsefulDBMethods|UsefulDBMethods]]

Declared classes and methods from the five selected projects’ Compile source entries (Debug symbols: DEBUG, TRACE). Partial classes are combined; nested classes use their containing class name. Inherited methods, properties, accessors, fields and compiler-generated members are excluded.

Namespaces and classes are alphabetized. Methods are alphabetized within public, private, then other visibility groups; overloads are sorted by signature. Constructors appear separately.

*2 namespaces · 11 classes · 64 methods*

## Namespaces

- [[#UsefulDBMethods|UsefulDBMethods]]
- [[#UsefulDBMethods.Reporting|UsefulDBMethods.Reporting]]

## UsefulDBMethods

- [[#CompanyFolderSettings|CompanyFolderSettings]]
- [[#DateHelpers|DateHelpers]]
- [[#EventHelpers|EventHelpers]]
- [[#EventViewerLogging|EventViewerLogging]]
- [[#GeneratorHelpers|GeneratorHelpers]]
- [[#Helpers|Helpers]]
- [[#Universal|Universal]]

---

### CompanyFolderSettings

Project: [[UsefulDBMethods#CompanyFolderSettings|UsefulDBMethods]]

*Source: [CompanyFolderSettings.cs](../../UsefulDBMethods/CompanyFolderSettings.cs)*

#### public methods (4)

- `public static string DocsLocation(string ConnectionString, Int64 CompanyClientId, out string ErrorMsg)`
- `public static string EAOfficeServiceLocation(string ConnectionString, Int64 CompanyClientId, out string ErrorMsg)`
- `public static string ImportStorageLocation(string ConnectionString, Int64 CompanyClientId, out string ErrorMsg)`
- `public static string TestRPOLocation(string ConnectionString, Int64 CompanyClientId, out string ErrorMsg)`

---

### DateHelpers

Project: [[UsefulDBMethods#DateHelpers|UsefulDBMethods]]

*Source: [DateHelpers.cs](../../UsefulDBMethods/DateHelpers.cs)*

#### public methods (2)

- `public static string YYYYMMDDToDbDateString(string Source)`
- `public static string YYYYMMDDToDbDateString(string Source, string Separator)`

#### Constructors

#### public constructors (1)

- `public DateHelpers()`

---

### EventHelpers

Project: [[UsefulDBMethods#EventHelpers|UsefulDBMethods]]

*Source: [EventHelpers.cs](../../UsefulDBMethods/EventHelpers.cs)*

#### public methods (17)

- `public static int AddEmailEvent(string ConnectionString, int CompanyClientId, //int OfficeId,
//int ManagerId,
//int TransactionTypeId,
//bool ForMyself,
//int ForMyselfPdfOrXls,
//int ReportId,
int FilterEventId, string SourceScreen, int AddedById, out string ErrorMsg)`
- `public static int AddReportEvent(string ConnectionString, int CompanyClientId, int OfficeId, int ManagerId, int TransactionTypeId, bool ForMyself, int ForMyselfPdfOrXls, int ReportId, string SqlText, string SourceScreen, int AddedById, out string ErrorMsg)`
- `public static string CallFilterEventSqlCreate(int FilterEventId, string ConnectionString)`
- `public static string CreateXEmailTableForRunId(string ConnectionString, int EventRunId)`
- `public static string ExecuteEventRunCreate(string ConnectionString, int RunId, int CompanyClientId, int OfficeId, int ManagerId, int TransactionTypeId, // portfolioType
 int AddedById, int TaskType, string SqlText, string WhoTo1, string WhoTo2, int EmailOption, string DistinctClients, string AddAsExcluded, int EmailTemplateId, string SubjectLine, string CCTo, string BCCTo, string ExclusionTo, string SourceSceen, string ForMyself, int ForMyselfPdfOrXls, int ReportId, int StatementRunId, int LedgerId, int SubAccountLedgerId, string DateFrom, string DateTo, string RandomQuestion1, int TransactionId)`
- `public static Int64 GetCustomerClientIdForEventRunId(string ConnectionString, Int64 RunId, out string ErrorMsg)`
- `public static List<string> GetEventCommandLine(string ConnectionString, int EventRunClientId, bool TemplateEntriesOnly, out string ErrorMsg)`
- `public static List<string> GetEventCommandLineForEventRunClientId(string ConnectionString, int EventRunClientId, out string ErrorMsg)`
- `public static string GetEventCommandLineForRunId(string ConnectionString, int RunId, out string ErrorMsg)`
- `public static string GetEventFileLocationForRunId(string ConnectionString, int RunId, out string ErrorMsg)`
- `public static string GetFilterEventSql(string ConnectionString, int FilterEventId, out string ErrorMsg)`
- `public static int GetNextFilterEventGeneratorId(string ConnectionString, int CompanyId, int ClientId, out string ErrorMsg)`
- `public static Int64 GetNextStatementRunId(string ConnectionString, Int64 CompanyClientId, Int64 ClientId, out string ErrorMsg)`
- `public static string SetEndOfMonthInProgress(string ConnectionString, Int64 CompanyClientId, string NewValue)`
- `public static int SetupFilterEventRecord(string ConnectionString, int CompanyId, int ClientId, int FilterId, out string ErrorMsg)`
- `public static void SetupFilterEventRecord(string ConnectionString, int FilterEventId, int CompanyId, int ClientId, int FilterId, out string ErrorMsg)`
- `public static string UpdateFilterEventRecordItems(int FilterEventId, string AskLater, string AskFor, string FromValue, string FromDate, string ToValue, string ToDate, string ConnectionString)`

#### Constructors

#### public constructors (1)

- `public EventHelpers()`

---

### EventViewerLogging

Project: [[UsefulDBMethods#EventViewerLogging|UsefulDBMethods]]

*Source: [EventViewerLogging.cs](../../UsefulDBMethods/EventViewerLogging.cs)*

#### public methods (1)

- `public static string WriteRecord(string ConnectionString, string Level, string Source, int EventId, string Message)`

---

### GeneratorHelpers

Project: [[UsefulDBMethods#GeneratorHelpers|UsefulDBMethods]]

*Source: [GeneratorHelpers.cs](../../UsefulDBMethods/GeneratorHelpers.cs)*

#### public methods (4)

- `public static int GetNextGeneratorId(string ConnectionString, string GeneratorName, int CompanyId, int ClientId, out string ErrorMsg)`
- `public static int GetNextGeneratorId(string ConnectionString, string GeneratorName, out string ErrorMsg)`
- `public static void WriteParamsToOutput(string QueryText, List<FbParameterItem> Params)`
- `public static void WriteToOutput(string LineText)`

#### Constructors

#### public constructors (1)

- `public GeneratorHelpers()`

---

### Helpers

Project: [[UsefulDBMethods#Helpers|UsefulDBMethods]]

*Source: [Helpers.cs](../../UsefulDBMethods/Helpers.cs)*

#### public methods (5)

- `public static DataTable GetFirstDataTableEx(string ConnectionString, string QueryText, List<FbParameterItem> Params, int Timeout, FirebirdQueryCheckTypes CheckType, out string ErrorMsg)`
- `public static DataTable GetFirstDataTableEx(string connectionString, string QueryText, FirebirdQueryCheckTypes CheckType, out string ErrorMsg)`
- `public static string ProcessTestRPOCommandLine(string CommandLine, string TestRPOLocation, int Tries)`
- `public static string RunEAMainOfficeProcess(string ConnectionString, Int64 CompanyClientId)`
- `public static string WriteParamsToInfoEvent(string QueryText, List<FbParameterItem> Params, string ModuleName, int MasterId)`

#### Constructors

#### public constructors (1)

- `public Helpers()`

---

### Universal

Project: [[UsefulDBMethods#Universal|UsefulDBMethods]]

*Source: [Universal.cs](../../UsefulDBMethods/Universal.cs)*

#### public methods (2)

- `public static string RelationGetDisplayType(string ConnectionString, int CompanyId, int RelationId, out string DisplayType, out int DisplayId)`
- `public static string TransactionGetDisplayType(string ConnectionString, int CompanyId, int TransactionId, out string DisplayType)`

#### Constructors

#### public constructors (1)

- `public Universal()`

## UsefulDBMethods.Reporting

- [[#EAReportGetDefaultItem|EAReportGetDefaultItem]]
- [[#GeneralReportParameters|GeneralReportParameters]]
- [[#ReportUtilities|ReportUtilities]]
- [[#SystemReportUtilities|SystemReportUtilities]]

---

### EAReportGetDefaultItem

Project: [[UsefulDBMethods#EAReportGetDefaultItem|UsefulDBMethods]]

*Source: [ReportClasses.cs](../../UsefulDBMethods/Reporting/ReportClasses.cs)*

#### public methods (1)

- `public string AsJsonObject()`

---

### GeneralReportParameters

Project: [[UsefulDBMethods#GeneralReportParameters|UsefulDBMethods]]

*Source: [ReportClasses.cs](../../UsefulDBMethods/Reporting/ReportClasses.cs)*

No declared methods.

#### Constructors

#### public constructors (1)

- `public GeneralReportParameters()`

---

### ReportUtilities

Project: [[UsefulDBMethods#ReportUtilities|UsefulDBMethods]]

*Source: [ReportUtilities.cs](../../UsefulDBMethods/Reporting/ReportUtilities.cs)*

#### public methods (9)

- `public StringBuilder BuildReportFileList(string ConnectionString, int ReportId, Int64 CompanyClientId, Int64 ClientId, NameValueCollection CommonCollection, NameValueCollection DataCollection, string OutputArrayName, out string ErrorMsg)`
- `public void BuildReportFileListEntries(string ConnectionString, int EventRunId, string CommandLine, string AppLocation, string DocsLocation, string ArrayName, ref StringBuilder Entries, out string ErrorMsg)`
- `public void BuildReportFileListEntries_ExcludingBase64(string ConnectionString, int EventRunId, string CommandLine, string AppLocation, string DocsLocation, string ArrayName, ref StringBuilder Entries, out string ErrorMsg)`
- `public StringBuilder BuildReportFileList_ExcludingBase64(string ConnectionString, int ReportId, Int64 CompanyClientId, Int64 ClientId, NameValueCollection CommonCollection, NameValueCollection DataCollection, string OutputArrayName, out string ErrorMsg)`
- `public void EAEventRunBase64Entries(string ConnectionString, int EventRunId, string DocsLocation, ref StringBuilder Entries, out string ErrorMsg)`
- `public void EAEventRunEntries(string ConnectionString, int EventRunId, string DocsLocation, ref StringBuilder Entries, out string ErrorMsg)`
- `public DataTable EAReportGetDefault(string ConnectionString, int CompanyClientId, string ReportSource, int RunId, out string ErrorMsg)`
- `public string EAReportGetDefaultJsonList(string ConnectionString, int CompanyClientId, string ReportSource, int RunId, string JsonArrayName, out string ErrorMsg)`
- `public void GetCompanySettingsItems(string ConnectionString, Int64 CompanyClientId, out string WordLocation, out string DocsLocation, out string ErrorMsg)`

#### private methods (7)

- `private List<GeneralReportParameters> BuildReportParameterList(string ConnectionString, int ReportId, out string ErrorMsg)`
- `private string BuildReportSql(string ConnectionString, int ReportId, Int64 CompanyClientId, Int64 ClientId, NameValueCollection CommonCollection, NameValueCollection DataCollection, out int EventRunId, out string ErrorMsg)`
- `private void ExecuteEventRunCreate(string ConnectionString, int EventRunId, int ReportId, string ReportSql, Int64 CompanyClientId, Int64 ClientId, out string ErrorMsg)`
- `private object GetCollectionValue(string Source, string Key, NameValueCollection CommonCollection, NameValueCollection DataCollection)`
- `private string GetEventRunReportSqlStatement(string ConnectionString, int ReportId, int EventRunId, out string ErrorMsg)`
- `private void ProcessReportParameterList(string ConnectionString, Int64 CompanyClientId, Int64 ClientId, NameValueCollection CommonCollection, NameValueCollection DataCollection, List<GeneralReportParameters> ReportParameterList, out int EventRunId, out string ErrorMsg)`
- `private void WriteCollectionInfo(NameValueCollection CommonCollection, NameValueCollection DataCollection)`

#### Constructors

#### public constructors (1)

- `public ReportUtilities()`

---

### SystemReportUtilities

Project: [[UsefulDBMethods#SystemReportUtilities|UsefulDBMethods]]

*Source: [SystemReportUtilities.cs](../../UsefulDBMethods/Reporting/SystemReportUtilities.cs)*

#### public methods (5)

- `public static Int64 Execute(string ConnectionString, Int64 CompanyId, Int64 ClientId, Int64 PassedId, string ReportSource, out string ErrorMsg, out string OutputFileName)`
- `public static string WriteBinaryOutputFile(string ConnectionString, string SqlToUse, string FileName, string Extension)`
- `public static string WriteBinaryOutputFile(string ConnectionString, string SqlToUse, string FileNameIncludingExtn)`
- `public static string WriteOutputFile(string ConnectionString, string SqlToUse, string FileName)`
- `public static string WriteOutputFile(string ConnectionString, string SqlToUse, string FileName, string Extension)`

#### private methods (7)

- `private string GetCreatedSystemReportFileName(string SqlToUse)`
- `private Int64 GetNewSystemReportId()`
- `private void GetSqlToUse(out string SqlToUse, out string FileName, out string FileNameOnly, out string Extension)`
- `private string GetSys_FileCreated()`
- `private void InsertEASystemReportRecord()`
- `private void PerformStep1()`
- `private void PerformTestRPO(string CommandLine)`

#### Constructors

#### public constructors (1)

- `public SystemReportUtilities(string ConnectionString, Int64 CompanyId, Int64 ClientId, Int64 PassedId, string ReportSource)`

