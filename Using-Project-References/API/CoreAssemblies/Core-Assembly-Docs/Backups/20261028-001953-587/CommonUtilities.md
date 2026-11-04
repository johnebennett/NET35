---
cssclasses:
  - project-reference
---

# CommonUtilities project reference

> [[index|Combined index]] Â· [[ActionBase|ActionBase]] Â· [[CommonUtilities|CommonUtilities]] Â· [[ProgressLogging|ProgressLogging]] Â· [[RequestProcessor|RequestProcessor]] Â· [[UsefulDBMethods|UsefulDBMethods]]

Declared classes and methods from the five selected projects’ Compile source entries (Debug symbols: DEBUG, TRACE). Partial classes are combined; nested classes use their containing class name. Inherited methods, properties, accessors, fields and compiler-generated members are excluded.

Namespaces and classes are alphabetized. Methods are alphabetized within public, private, then other visibility groups; overloads are sorted by signature. Constructors appear separately.

*1 namespaces · 11 classes · 109 methods*

## Namespaces

- [[#CommonUtilities|CommonUtilities]]

## CommonUtilities

- [[#CollectionUtilities|CollectionUtilities]]
- [[#CurrencyUtilities|CurrencyUtilities]]
- [[#FbParameterItem|FbParameterItem]]
- [[#FirebirdUtilities|FirebirdUtilities]]
- [[#FolderHelpers|FolderHelpers]]
- [[#JsonUtilities|JsonUtilities]]
- [[#MiscUtilities|MiscUtilities]]
- [[#StringUtilities|StringUtilities]]
- [[#UtilityApps|UtilityApps]]
- [[#ValidationChecks|ValidationChecks]]
- [[#WindowsEventLoggingUtilities|WindowsEventLoggingUtilities]]

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

