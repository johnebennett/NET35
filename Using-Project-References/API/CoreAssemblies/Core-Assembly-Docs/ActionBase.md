---
cssclasses:
  - project-reference
---

# ActionBase project reference

> [[index|Combined index]] · [[ActionBase|ActionBase]] · [[CommonUtilities|CommonUtilities]] · [[ProgressLogging|ProgressLogging]] · [[RequestProcessor|RequestProcessor]] · [[UsefulDBMethods|UsefulDBMethods]]

Declared classes and methods from the five selected projects’ Compile source entries (Debug symbols: DEBUG, TRACE). Partial classes are combined; nested classes use their containing class name. Inherited methods, properties, accessors, fields and compiler-generated members are excluded.

Namespaces and classes are alphabetized. Methods are alphabetized within public, private, then other visibility groups; overloads are sorted by signature. Constructors appear separately.

*2 namespaces · 7 classes · 75 methods*

## Namespaces

- [[#CommonDBMethods|CommonDBMethods]]
- [[#EAOfficeAPI|EAOfficeAPI]]

## CommonDBMethods

- [[#Common|Common]]

---

### Common

Project: [[ActionBase#Common|ActionBase]]

*Source: [CommonDBMethods.cs](../../ActionBase/CommonDBMethods.cs)*

#### public methods (3)

- `public static string CommonComboEntries(int CompanyId, int ClientId, int ComboType, out string ErrorMsg)`
- `public static string ConnectionString()`
- `public static string ConnectionString(APISettingsItem Source)`

## EAOfficeAPI

- [[#ActionBase|ActionBase]]
- [[#APISettingsFile|APISettingsFile]]
- [[#APISettingsItem|APISettingsItem]]
- [[#CoreRuntimeInitialization|CoreRuntimeInitialization]]
- [[#RequestCommonItem|RequestCommonItem]]
- [[#RequestCommonProcessor|RequestCommonProcessor]]

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

