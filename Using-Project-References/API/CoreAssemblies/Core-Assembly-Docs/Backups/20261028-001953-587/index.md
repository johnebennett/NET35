---
cssclasses:
  - project-reference
---

# Combined project reference

> [[index|Combined index]] Â· [[ActionBase|ActionBase]] Â· [[CommonUtilities|CommonUtilities]] Â· [[ProgressLogging|ProgressLogging]] Â· [[RequestProcessor|RequestProcessor]] Â· [[UsefulDBMethods|UsefulDBMethods]]

Declared classes and methods from the five selected projects’ Compile source entries (Debug symbols: DEBUG, TRACE). Partial classes are combined; nested classes use their containing class name. Inherited methods, properties, accessors, fields and compiler-generated members are excluded.

Namespaces and classes are alphabetized. Methods are alphabetized within public, private, then other visibility groups; overloads are sorted by signature. Constructors appear separately.

*5 namespaces · 31 classes · 268 methods*

## Namespaces

- [[#CommonDBMethods|CommonDBMethods]]
- [[#CommonUtilities|CommonUtilities]]
- [[#EAOfficeAPI|EAOfficeAPI]]
- [[#UsefulDBMethods|UsefulDBMethods]]
- [[#UsefulDBMethods.Reporting|UsefulDBMethods.Reporting]]

## CommonDBMethods

- [[#Common|Common]] — ActionBase

---

### Common

Project: [[ActionBase#Common|ActionBase]]

*Source: [CommonDBMethods.cs](../../ActionBase/CommonDBMethods.cs)*

#### public methods (3)

- `public static string CommonComboEntries(int CompanyId, int ClientId, int ComboType, out string ErrorMsg)`
- `public static string ConnectionString()`
- `public static string ConnectionString(APISettingsItem Source)`

## CommonUtilities

- [[#CollectionUtilities|CollectionUtilities]] — CommonUtilities
- [[#CurrencyUtilities|CurrencyUtilities]] — CommonUtilities
- [[#FbParameterItem|FbParameterItem]] — CommonUtilities
- [[#FirebirdUtilities|FirebirdUtilities]] — CommonUtilities
- [[#FolderHelpers|FolderHelpers]] — CommonUtilities
- [[#JsonUtilities|JsonUtilities]] — CommonUtilities
- [[#MiscUtilities|MiscUtilities]] — CommonUtilities
- [[#StringUtilities|StringUtilities]] — CommonUtilities
- [[#UtilityApps|UtilityApps]] — CommonUtilities
- [[#ValidationChecks|ValidationChecks]] — CommonUtilities
- [[#WindowsEventLoggingUtilities|WindowsEventLoggingUtilities]] — CommonUtilities

---

### CollectionUtilities

Project: [[CommonUtilities#CollectionUtilities|CommonUtilities]]

*Source: [CollectionUtilities.cs](../../CommonUtilities/CollectionUtilities.cs)*

#### public methods (9)

- `public static string GetCollectionInfo(NameValueCollection Source)`
- `public static int GetCollectionValue(NameValueCollection Source, string Key, int DefaultValue)`
- `public static int GetCollectionValue(NameValueCollection Source, string Key, int DefaultValue, out bool KeyFound)`
- `public static object GetCollectionValue(NameValueCollection Source, string Key)`
- `public static string GetCollectionValue(NameValueCollection Source, string Key, string DefaultValue)`
- `public static string GetCollectionValue(NameValueCollection Source, string Key, string DefaultValue, out bool KeyFound)`
- `public static NameValueCollection MakeCollection(JObject Source, out string ErrorMsg)`
- `public static NameValueCollection MakeCollection(string JsonSource, out string ErrorMsg)`
- `public void WriteCollectionInfo(NameValueCollection Source)`

#### Constructors

#### public constructors (1)

- `public CollectionUtilities()`

---

### CurrencyUtilities

Project: [[CommonUtilities#CurrencyUtilities|CommonUtilities]]

*Source: [CurrencyUtilities.cs](../../CommonUtilities/CurrencyUtilities.cs)*

#### public methods (2)

- `public static string BuildCurrencyDisplay(string Value)`
- `public static string BuildCurrencyDisplay(string Value, bool ExcludeDollarSymbol)`

#### Constructors

#### public constructors (1)

- `public CurrencyUtilities()`

---

### FbParameterItem

Project: [[CommonUtilities#FbParameterItem|CommonUtilities]]

*Source: [FirebirdUtilities.cs](../../CommonUtilities/FirebirdUtilities.cs)*

No declared methods.

#### Constructors

#### public constructors (1)

- `public FbParameterItem(string ParameterName, object ParameterValue)`

---

### FirebirdUtilities

Project: [[CommonUtilities#FirebirdUtilities|CommonUtilities]]

*Source: [FirebirdUtilities.cs](../../CommonUtilities/FirebirdUtilities.cs)*

#### public methods (30)

- `public static string AddMissingField(string connectionString, string TableName, string FieldName, DBFieldType FieldType)`
- `public static string AddMissingField(string connectionString, string TableName, string FieldName, DBFieldType FieldType, int VarCharFieldSize)`
- `public static string AddMissingField(string connectionString, string TableName, string FieldName, string FieldType)`
- `public static string AddMissingField(string connectionString, string TableName, string FieldName, string FieldType, int VarCharFieldSize)`
- `public static void AddMissingField(string connectionString, string TableName, string FieldName, DBFieldType FieldType, StringBuilder Errors)`
- `public static void AddMissingField(string connectionString, string TableName, string FieldName, DBFieldType FieldType, int VarCharFieldSize, StringBuilder Errors)`
- `public static string AddTableFieldErrorMsg(string TableName, string FieldName)`
- `public static string BuildConfigConnectionString()`
- `public static string BuildConnectionString(string UserId, string Password, string DBFileName, string DataSource)`
- `public static string BuildConnectionString(string UserId, string Password, string DBFileName, string DataSource, bool Pooling)`
- `public static string DateTimeToDbDateString(DateTime Source)`
- `public static string DateTimeToDbDateString(DateTime Source, string Separator)`
- `public static string DBExecute(string connectionString, string QueryText)`
- `public static string DBExecute(string connectionString, string QueryText, List<FbParameterItem> Parameters, int timeoutSeconds)`
- `public static bool FieldExists(string connectionString, string TableName, string FieldName)`
- `public static DataSet GetDataSet(string connectionString, string QueryText)`
- `public static DataSet GetDataSet(string connectionString, string QueryText, List<FbParameterItem> Parameters, int timeoutSeconds)`
- `public static DataSet GetDataSetFromQuery(string connectionString, string queryText, out string errorMessage)`
- `public static DBFieldType GetDBFieldTypeFromString(string FieldType)`
- `public static int GetDBTableValueAsInt(DataSet aDataSet, int aTableInd, int aRowInd, string aFieldName)`
- `public static Int64 GetDBTableValueAsInt64(DataSet aDataSet, int aTableInd, int aRowInd, string aFieldName)`
- `public static string GetDBTableValueAsString(DataSet aDataSet, int aTableInd, int aRowInd, string aFieldName)`
- `public static object GetFieldValueFromFirstRecordOfQuery(string connectionString, string queryText, string aFieldName)`
- `public static string GetFirebirdYNResponseFromBool(bool value)`
- `public static DataTable GetFirstDataTable(string connectionString, string QueryText)`
- `public static DataTable GetFirstDataTable(string connectionString, string QueryText, List<FbParameterItem> Parameters, int timeoutSeconds)`
- `public static bool TableExists(string connectionString, string TableName)`
- `public static bool TestDBConnection(string connectionString, out string aErrorMessage)`
- `public static string YYYYMMDDToDbDateString(string Source)`
- `public static string YYYYMMDDToDbDateString(string Source, string Separator)`

#### private methods (1)

- `private static string PrepareExceptionError(string aMainMessage, Exception aException, string aCategory, string aSQLText)`

#### Constructors

#### public constructors (1)

- `public FirebirdUtilities()`

---

### FolderHelpers

Project: [[CommonUtilities#FolderHelpers|CommonUtilities]]

*Source: [FolderHelpers.cs](../../CommonUtilities/FolderHelpers.cs)*

#### public methods (1)

- `public static string GetAssemblyDirectoryName(Assembly AssemblyName)`

#### Constructors

#### public constructors (1)

- `public FolderHelpers()`

---

### JsonUtilities

Project: [[CommonUtilities#JsonUtilities|CommonUtilities]]

*Source: [JsonUtilities.cs](../../CommonUtilities/JsonUtilities.cs)*

#### public methods (26)

- `public static string AddPropertyNameAndValue(string PropertyName, object PropertyValue)`
- `public static string AddPropertyNameAndValue(string PropertyName, object PropertyValue, bool AppendComma)`
- `public static string AddPropertyNameAndValue(string PropertyName, string PropertyValue)`
- `public static string AddPropertyNameAndValue(string PropertyName, string PropertyValue, bool AppendComma)`
- `public static void AddPropertyNameAndValue(JsonTextWriter Writer, string PropertyName, string Value)`
- `public static string BuildInvalidRequestType()`
- `public static string BuildJsonArrayFromDataTable(DataTable tblSource, string ArrayName, ref StringBuilder Entries)`
- `public static string CleanForJSON(string s)`
- `public static string EndJson()`
- `public static string EndJsonArray()`
- `public static string EndJsonObject()`
- `public static string EscapeJSONString_JsonConvert(string aValue)`
- `public static JObject GetJSONObject(string JSONString)`
- `public static JToken GetJsonPathValue(JObject Source, string Path, out string ErrorMsg)`
- `public static JToken GetJsonPathValue(string Source, string Path, out string ErrorMsg)`
- `public static string GetJsonPathValueStr(JObject Source, string Path, out string ErrorMsg)`
- `public static string GetJsonPathValueStr(string Source, string Path, out string ErrorMsg)`
- `public static string MethodNotImplemented()`
- `public static string MissingTag(string TagName)`
- `public static JObject ParseJSONSource(string Source, out string ErrorMsg)`
- `public static bool PopulateListFromArray(JObject DataObject, string ArrayName, ref List<string> StrList)`
- `public static string StartJson()`
- `public static string StartJsonArray(string ArrayName)`
- `public static string StartJsonObject(string PropertyName)`
- `public static string WriteJSONResult(string aValue)`
- `public static string WriteJSONResultWithEmbeddedObject(string aValue)`

#### private methods (1)

- `private static string BuildPropertyName(string PropertyName)`

---

### MiscUtilities

Project: [[CommonUtilities#MiscUtilities|CommonUtilities]]

*Source: [MiscUtilities.cs](../../CommonUtilities/MiscUtilities.cs)*

#### public methods (6)

- `public static string BuildDateDisplay(string Value, SourceDateFormat Format)`
- `public static string BuildNumberDisplay(string Value, int DecimalPlaces)`
- `public static string DateTimeToYYYYMMDD(DateTime Source)`
- `public static string DateTimeToYYYYMMDDTHHNNSS(DateTime Source)`
- `public static DateTime YYYYMMDDToDateTime(string Source)`
- `public static DateTime YYYYMMDDToDateTime(string Source, DateTime Default)`

#### private methods (2)

- `private static string BuildDateDisplayFromDDMMYYYY(string Value)`
- `private static string BuildDateDisplayFromMMDDYYYY(string Value)`

---

### StringUtilities

Project: [[CommonUtilities#StringUtilities|CommonUtilities]]

*Source: [StringUtilities.cs](../../CommonUtilities/StringUtilities.cs)*

#### public methods (5)

- `public static bool EndsWith(StringBuilder Source, string Value)`
- `public static string JObjectToString(JObject Source)`
- `public static bool SameText(string LeftStr, string RightStr)`
- `public static bool SameText(string LeftStr, string RightStr, bool IgnoreCase)`
- `public static int StrToIntDef(string s, int @default)`

#### Constructors

#### public constructors (1)

- `public StringUtilities()`

---

### UtilityApps

Project: [[CommonUtilities#UtilityApps|CommonUtilities]]

*Source: [UtilityApps.cs](../../CommonUtilities/UtilityApps.cs)*

#### public methods (3)

- `public static string HtmlToRtf(string SourceFileName, string DestinationFileName)`
- `public static string HtmlToRtf(string SourceTableName, string SourceSelectFieldName, string SourceUniqueFieldName, int SourceTableId, string DestinationTableName, string DestinationSelectFieldName, string DestinationUniqueFieldName, int DestinationTableId)`
- `public static string HtmlToRtf(string SourceTableName, string SourceSelectFieldName, string SourceUniqueFieldName, string SourceTableId, string DestinationTableName, string DestinationSelectFieldName, string DestinationUniqueFieldName, string DestinationTableId)`

#### Constructors

#### public constructors (1)

- `public UtilityApps()`

---

### ValidationChecks

Project: [[CommonUtilities#ValidationChecks|CommonUtilities]]

*Source: [ValidationChecks.cs](../../CommonUtilities/ValidationChecks.cs)*

#### public methods (15)

- `public static string ErrorIfDateNotInRange(DateTime DateValue, DateTime MinDate, DateTime MaxDate, string ErrorString)`
- `public static string ErrorIfEmptyString(string StrValue, string ErrorString)`
- `public static string ErrorIfInvalidPositiveInteger(string IntValueStr, string ErrorString)`
- `public static string ErrorIfNegativeCurrency(double CurrValue, string ErrorString)`
- `public static string ErrorIfNegativeInteger(int IntValue, string ErrorString)`
- `public static string ErrorIfNeitherString(string FirstString, string SecondString, string FirstStringName, string SecondStringName)`
- `public static string ErrorIfNoneOf(string FirstString, string SecondString, string ThirdString, string FirstStringName, string SecondStringName, string ThirdStringName)`
- `public static string ErrorIfNotInteger(string Input, string ErrorString)`
- `public static string ErrorIfNotValidEmailAddress(string Input, string ErrorString)`
- `public static string ErrorIfStringLongerThan(string StrValue, string ErrorString, int MaxLength)`
- `public static string ErrorIfStringNotInList(string StrValue, List<string> StrList, string ErrorString)`
- `public static string ErrorIfStringNotInRange(string StrValue, string ErrorString, int MinLength, int MaxLength)`
- `public static string ErrorIfStringShorterThan(string StrValue, string ErrorString, int MinLength)`
- `public static string ErrorIfZeroCurrency(double CurrValue, string ErrorString)`
- `public static string ErrorIfZeroInteger(int IntValue, string ErrorString)`

---

### WindowsEventLoggingUtilities

Project: [[CommonUtilities#WindowsEventLoggingUtilities|CommonUtilities]]

*Source: [EventLoggingHooks.cs](../../CommonUtilities/EventLoggingHooks.cs), [WindowsEventLoggingUtilities.cs](../../CommonUtilities/WindowsEventLoggingUtilities.cs)*

#### public methods (5)

- `public static void ConfigureEventRecordWriter(EventRecordWriter writer)`
- `public static string GetMessageSubStringForEventViewer(string aMessage, double aByteLimit)`
- `public static string WriteToErrorEvent(string aMessage, string aSource, int aEventID)`
- `public static string WriteToInfoEvent(string aMessage, string aSource, int aEventID)`
- `public static string WriteToWarningEvent(string aMessage, string aSource, int aEventID)`

#### private methods (3)

- `private static string EventLogEntryTypeToString(EventLogEntryType Type)`
- `private static string LogEventViewerRecord(string level, string source, int eventId, string message)`
- `private static string WriteToEventLog(string aMessage, string aSource, EventLogEntryType aType, int aEventID)`

## EAOfficeAPI

- [[#ActionBase|ActionBase]] — ActionBase
- [[#APISettingsFile|APISettingsFile]] — ActionBase
- [[#APISettingsItem|APISettingsItem]] — ActionBase
- [[#CoreRuntimeInitialization|CoreRuntimeInitialization]] — ActionBase
- [[#ProgressLogging|ProgressLogging]] — ProgressLogging
- [[#RequestCommonItem|RequestCommonItem]] — ActionBase
- [[#RequestCommonProcessor|RequestCommonProcessor]] — ActionBase
- [[#RequestProcessor|RequestProcessor]] — RequestProcessor

---

### ActionBase

Project: [[ActionBase#ActionBase|ActionBase]]

*Source: [ActionBase.cs](../../ActionBase/ActionBase.cs)*

#### public methods (31)

- `public bool BuildContentAndLog(GeneralResponseStatus StatusCode, string Comment, string Message, string Method, JsonContentEntryType EntryType, string DataAsJson, out int NewId, out string Content, out string ErrorMsg)`
- `public bool BuildContentAndLog(bool LogEntry, GeneralResponseStatus StatusCode, string Comment, string Message, string Method, JsonContentEntryType EntryType, string DataAsJson, out int NewId, out string Content, out string ErrorMsg)`
- `public string BuildErrorResult(string ErrorMsg)`
- `public static string CreateDBComponents(string ConnectionString)`
- `public int GetCollectionValue(string Source, string Key, int DefaultValue)`
- `public object GetCollectionValue(string Source, string Key)`
- `public string GetCollectionValue(string Source, string Key, string DefaultValue)`
- `public string LoggingEntry(string Method, string Msg, string DataAsJson)`
- `public string LogOnAuthorisationError(string Method, string Msg, string DataAsJson)`
- `public string LogOnErrorSelectingData(string Method, string Msg, string QueryText)`
- `public string LogOnErrorValidatingData(string Method, string Msg, string DataAsJson)`
- `public string LogOnException(string Method, //: String;
 Exception Ex, string DataAsJson)`
- `public string LogOnException(string Method, //: String;
 string Msg, //: String;
 string DataAsJson)`
- `public string LogOnInsideMethod(string Method)`
- `public string LogOnInsideMethod(string Method, //: String;
 string DataAsJson)`
- `public string LogOnInsideMethodWithMessage(string Method, string Message)`
- `public string LogOnLeavingMethod(string Method)`
- `public string LogOnLeavingMethod(string Method, string DataAsJson)`
- `public string LogOnProcessingError(string Method, string Msg, string DataAsJson)`
- `public string LogResponse(string Method, string DataAsJson)`
- `public void LogToErrorEvent(string Message)`
- `public void LogToErrorEvent(string Message1, string Message2, string Message3)`
- `public void LogToInfoEvent(string Message)`
- `public void LogToInfoEvent(string Message, string Message2)`
- `public void LogToInfoEvent(string Message, string Message2, string Message3)`
- `public void LogToWarningEvent(string Message)`
- `public string MakeCollections(JObject ADataObject)`
- `public string MakeDataCollectionFromDataObject(JObject ADataObject)`
- `public bool Setup(int MasterId, APISettingsItem APISettings, string Request, out string ErrorMsg)`
- `public bool Setup(int MasterId, RequestCommonItem CommonItem, JObject DataObject, APISettingsItem APISettings, out string ErrorMsg)`
- `public void WriteCollectionInfo()`

#### private methods (8)

- `private static string BuildConnectionString(out string ErrorMsg)`
- `private JObject BuildDataObject(out string ErrorMsg)`
- `private JObject BuildDataObject(string Source, out string ErrorMsg)`
- `private static string BuildErrorAddingTable(string TableName, string FieldName)`
- `private RequestCommonItem BuildRequestCommonItem(out string ErrorMsg)`
- `private RequestCommonItem BuildRequestCommonItem(string Source, out string ErrorMsg)`
- `private static string BuildSqliteConnectionString(out string ErrorMsg)`
- `private bool GetRPOClientReferenceFromDB(string aDBConnectionString, out string aClientReference, bool aLogging, out string aErrorMsg)`

#### Constructors

#### public constructors (1)

- `public ActionBase()`

---

### APISettingsFile

Project: [[ActionBase#APISettingsFile|ActionBase]]

*Source: [APISettingsConfig.cs](../../ActionBase/APISettingsConfig.cs)*

#### public methods (7)

- `public static APISettingsItem GetAPISettings(APIAppType AppType, out string ErrorMessage)`
- `public static APISettingsItem GetAPISettings(APIAppType AppType, string AppDir, out string ErrorMessage)`
- `public static string GetAPISettingsFileName(APIAppType AppType, out string ErrorMessage)`
- `public static string GetAPISettingsFileName(APIAppType AppType, string AppDir, out string ErrorMessage)`
- `public static bool UpdateAPISettings(APIAppType AppType, APISettingsItem NewSettings, out string ErrorMessage)`
- `public static bool UpdateAPISettings(APIAppType AppType, string AppDir, APISettingsItem NewSettings, out string ErrorMessage)`
- `public static void WriteToOutput(string LineText)`

#### private methods (17)

- `private static bool GetDatabaseSettings(JObject Source, APISettingsItem Settings, out string ErrorMessage)`
- `private static bool GetHostSettings(JObject Source, APISettingsItem Item, out string ErrorMessage)`
- `private static bool GetLoggingSettings(JObject Source, APISettingsItem Settings, out string ErrorMessage)`
- `private static bool GetMaintenanceSettings(JObject Source, APISettingsItem Settings, out string ErrorMessage)`
- `private static bool GetServiceSettings(JObject Source, APISettingsItem Settings, out string ErrorMessage)`
- `private static string GetStringValueForJSONElement(JObject Source, string ElementName)`
- `private static bool SameAPISettings(APISettingsItem LHS, APISettingsItem RHS)`
- `private static bool SaveAPISettings(APIAppType AppType, APISettingsItem NewSettings, APISettingsItem OldSettings, out string ErrorMessage)`
- `private static bool SaveAPISettings(APIAppType AppType, string AppDir, APISettingsItem NewSettings, APISettingsItem OldSettings, out string aErrorMessage)`
- `private static bool SaveAPISettingsJsonString(APIAppType AppType, string AppDir, string JsonString, APISettingsItem OldSettings, out string ErrorMessage)`
- `private static bool SaveAPISettingsJsonString(APIAppType AppType, string JsonString, APISettingsItem OldSettings, out string aErrorMessage)`
- `private static void SetStringValueForJSONElement(JObject Source, string ElementName, string Value)`
- `private static bool UpdateDatabaseSettings(JObject Source, APISettingsItem NewSettings, APISettingsItem OldSettings, out string ErrorMessage)`
- `private static bool UpdateHostSettings(JObject Source, APISettingsItem NewSettings, APISettingsItem OldSettings, out string ErrorMessage)`
- `private static bool UpdateLoggingSettings(JObject Source, APISettingsItem NewSettings, APISettingsItem OldSettings, out string ErrorMessage)`
- `private static bool UpdateMaintenanceSettings(JObject Source, APISettingsItem NewSettings, APISettingsItem OldSettings, out string ErrorMessage)`
- `private static bool UpdateServiceSettings(JObject Source, APISettingsItem NewSettings, APISettingsItem OldSettings, out string ErrorMessage)`

---

### APISettingsItem

Project: [[ActionBase#APISettingsItem|ActionBase]]

*Source: [APISettingsConfig.cs](../../ActionBase/APISettingsConfig.cs)*

No declared methods.

#### Constructors

#### public constructors (1)

- `public APISettingsItem()`

---

### CoreRuntimeInitialization

Project: [[ActionBase#CoreRuntimeInitialization|ActionBase]]

*Source: [CoreRuntimeInitialization.cs](../../ActionBase/CoreRuntimeInitialization.cs)*

#### public methods (3)

- `public static void InitializeLogging()`
- `public static void InitializeLogging(APISettingsItem settings)`
- `public static void InitializeLogging(Func<string> provider)`

#### private methods (1)

- `private static bool IsWhiteSpace(string value)`

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

---

### RequestCommonItem

Project: [[ActionBase#RequestCommonItem|ActionBase]]

*Source: [RequestCommon.cs](../../ActionBase/RequestCommon.cs)*

#### private methods (3)

- `private void SetClientIdValue(string Value)`
- `private void SetCompanyIdValue(string Value)`
- `private void SetDebugValue(string Value)`

---

### RequestCommonProcessor

Project: [[ActionBase#RequestCommonProcessor|ActionBase]]

*Source: [RequestCommon.cs](../../ActionBase/RequestCommon.cs)*

#### public methods (2)

- `public static RequestCommonItem LoadFromRequest(string Request, out string ErrorMsg)`
- `public static string ValidateCommonItem(RequestCommonItem Item)`

---

### RequestProcessor

Project: [[RequestProcessor#RequestProcessor|RequestProcessor]]

*Source: [RequestProcessor.cs](../../RequestProcessor/RequestProcessor.cs)*

#### public methods (3)

- `public static string Execute(string Request)`
- `public static string ExecuteDefaultMethod(string Request)`
- `public static string ExecuteRequestWithAssemblyReference(string Request)`

#### private methods (1)

- `private static void WriteInfoToEventLog(string Message)`

## UsefulDBMethods

- [[#CompanyFolderSettings|CompanyFolderSettings]] — UsefulDBMethods
- [[#DateHelpers|DateHelpers]] — UsefulDBMethods
- [[#EventHelpers|EventHelpers]] — UsefulDBMethods
- [[#EventViewerLogging|EventViewerLogging]] — UsefulDBMethods
- [[#GeneratorHelpers|GeneratorHelpers]] — UsefulDBMethods
- [[#Helpers|Helpers]] — UsefulDBMethods
- [[#Universal|Universal]] — UsefulDBMethods

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

- [[#EAReportGetDefaultItem|EAReportGetDefaultItem]] — UsefulDBMethods
- [[#GeneralReportParameters|GeneralReportParameters]] — UsefulDBMethods
- [[#ReportUtilities|ReportUtilities]] — UsefulDBMethods
- [[#SystemReportUtilities|SystemReportUtilities]] — UsefulDBMethods

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

