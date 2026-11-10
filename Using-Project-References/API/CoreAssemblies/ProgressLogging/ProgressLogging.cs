/*
 * Created by SharpDevelop.
 * User: John
 * Date: 23/10/2018
 * Time: 8:05 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Reflection;
using System.Data.SQLite;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Collections.Generic;

using CommonUtilities;
using EAOfficeAPI;

namespace EAOfficeAPI
{
	public enum GeneralResponseStatus
	{
		LogOnException = -1,
		
		LogOnSuccess = 0,
		
		LogOnInsideMethod = 1,
		LogOnLeavingMethod = 2,
		LogOnErrorValidatingData = 3,
		LogOnErrorSelectingData = 4,
		LoggingEntry = 5,
		LoggingOnErrorAuthorising = 6,
		LoggingOnErrorProcessing = 7
	}

	public enum JsonContentEntryType 
	{
		Request,
		Response,
		Logging_Entry,
		Validation_Error,
		Warning,
		Error,
		QueryText,
		Debugging
	}
	
	public partial class ProgressLogging
	{
		public static string CurrentLoggingFileName()
		{
            string FileName = DateTime.Now.ToString("yyyyMMdd");
            FileName += ".sqlite";
			
			string AssemblyLocalPath = new Uri(Assembly.GetExecutingAssembly().GetName().CodeBase).LocalPath;
			AssemblyLocalPath = Path.GetDirectoryName(AssemblyLocalPath);
			string PathSep = @"\";
			if (!AssemblyLocalPath.EndsWith(PathSep)) AssemblyLocalPath += PathSep;
			AssemblyLocalPath += "LogFiles" + PathSep;
			
			FileName = AssemblyLocalPath + FileName;
			
			return FileName;
		}
		
		private string CreateCurrentLoggingDB(out string FileName/*out string ErrorMsg*/)
		{
			string Result = "";
			FileName = CurrentLoggingFileName();
			
            try 
            {
	            var fileInfo = new FileInfo(FileName);
	            if (!fileInfo.Exists)
	            {
	            	SQLiteConnection.CreateFile(FileName);
				
	            	using(System.Data.SQLite.SQLiteConnection sqlite2 = new SQLiteConnection("Data Source=" + FileName))
				    {
				        sqlite2.Open();
				        string sql = TableStructure();
				        SQLiteCommand command = new SQLiteCommand(sql, sqlite2);
				        command.ExecuteNonQuery();
				    }
				}		
            } 
            catch (Exception Ex) 
            {
            	Result = Ex.Message + Environment.NewLine + Ex.StackTrace;
            }
            
            return Result;
		}
		
		private string TableStructure()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("CREATE TABLE 'Entries' (");
			sb.AppendLine("	'ID'	INTEGER PRIMARY KEY AUTOINCREMENT,");
			sb.AppendLine("'MasterId'	INTEGER NOT NULL,");
			sb.AppendLine("'Plugin'	TEXT,");
			sb.AppendLine("'Command'	TEXT,");
			sb.AppendLine("'Method'	TEXT,");
			sb.AppendLine("'EntryType'	TEXT,");
			sb.AppendLine("'Content'	TEXT,");
			sb.AppendLine("'CompanyId'	INTEGER,");
			sb.AppendLine("'ClientId'	INTEGER,");
			sb.AppendLine("'DateAdded'  TEXT,");
			sb.AppendLine("'TimeAdded'  TEXT");
			sb.AppendLine(");");			
			
			return sb.ToString();
		}

		public static string BuildSqliteConnectionString(out string ErrorMsg)
		{
			ErrorMsg = "";
			string FileName = CurrentLoggingFileName();
			string Result = "Data Source={0}; Version=3;";
			Result = string.Format(Result, FileName);
			return Result;
		}

		public static bool BuildContentAndLog(bool LogEntry,
		                                      int MasterId,
		                                      GeneralResponseStatus StatusCode,
		                                      string Comment,
		                                      string Message,
		                                      string Plugin,
		                                      string Command,
		                                      string Method,
		                                      JsonContentEntryType EntryType,
		                                      string DataAsJson,
		                                      int CompanyId,
		                                      int ClientId,
		                                      out int NewRecordId,
		                                      out string Content,
		                                      out string ErrorMsg)
		{
			int StatusCodeValue = (int)StatusCode;
			string EntryTypeValue = GetEntryTypeString(EntryType);
			return 
				BuildContentAndLog(
					LogEntry, MasterId, StatusCodeValue,
					Comment, Message, Plugin, Command, Method, EntryTypeValue, 
					DataAsJson, CompanyId, ClientId, out NewRecordId, out Content, out ErrorMsg);
		}
		
		/// <summary>
		/// Sqlite version of logging 
		/// </summary>
		/*
		public static bool BuildContentAndLog(bool LogEntry,
		                                      int MasterId,
		                                      int StatusCode,
		                                      string Comment,
		                                      string Message,
		                                      string Plugin,
		                                      string Command,
		                                      string Method,
		                                      string EntryType,
		                                      string DataAsJson,
		                                      int CompanyId,
		                                      int ClientId,
		                                      out int NewRecordId,
		                                      out string Content,
		                                      out string ErrorMsg)
		{
			bool result = true;
			
			Content = "";
			ErrorMsg = "";
			NewRecordId = 0;
			
			string ConnectionString = BuildSqliteConnectionString(out ErrorMsg);
			if (!string.IsNullOrEmpty(ErrorMsg)) return false;
			
			if (LogEntry == true)
			{			
				// Only generate a new record if entry will be logged
				NewRecordId = GetNextRecordId(ConnectionString, MasterId, CompanyId, ClientId, out ErrorMsg);
				if (NewRecordId < 0)
				{
					result = false;
					ErrorMsg = 
						"Error generating the next record Id in BuildContentAndLog" +
						Environment.NewLine + ErrorMsg;
					CommonUtilities.WindowsEventLoggingUtilities.WriteToErrorEvent(ErrorMsg, "BuildContentAndLog", 0);
				}
			}
				
			if (result)
			{
				ProgressLogging logging = new ProgressLogging();
				result = logging.BuildContent(StatusCode, NewRecordId, Comment, Message, DataAsJson, Plugin, Command, Method, out Content, out ErrorMsg);
				result &= (string.IsNullOrEmpty(ErrorMsg));
				
				if (result)
				{
					result =
						logging.UpdateRecordWithValues(
							ConnectionString, LogEntry, Plugin, Command, Method, EntryType, Content, NewRecordId, out ErrorMsg);
					result &= (string.IsNullOrEmpty(ErrorMsg));
				}
			}
			
			return result;
		}
		*/
		
		/// <summary>
		/// Firebird version of logging 
		/// </summary>
		public static bool BuildContentAndLog(bool LogEntry,
		                                      int MasterId,
		                                      int StatusCode,
		                                      string Comment,
		                                      string Message,
		                                      string Plugin,
		                                      string Command,
		                                      string Method,
		                                      string EntryType,
		                                      string DataAsJson,
		                                      int CompanyId,
		                                      int ClientId,
		                                      out int NewRecordId,
		                                      out string Content,
		                                      out string ErrorMsg)
		{
			bool result = true;
			
			Content = "";
			ErrorMsg = "";
			NewRecordId = 0;
			//string MethodName = "BuildContentAndLog";
			
			string ConnectionString = "";
			if (!string.IsNullOrEmpty(ErrorMsg)) return false;
			
			try 
			{
				if (LogEntry == true)
				{			
					// Only generate a new record if entry will be logged
					if (!TryGetConnectionString(out ConnectionString, out ErrorMsg)) return false;
					NewRecordId = 
						UsefulDBMethods.GeneratorHelpers.GetNextGeneratorId(
							ConnectionString, "LOGGINGENTRIES_ID_GEN", CompanyId, ClientId, out ErrorMsg);				
					if (!string.IsNullOrEmpty(ErrorMsg))
					{
						ErrorMsg =
							"Error retrieving the next Generator Id for the logging table" + Environment.NewLine +
							ErrorMsg;
						WindowsEventLoggingUtilities.WriteToErrorEvent(ErrorMsg, "BuildContentAndLog", 0);
						result = false;					
					}
					else if (NewRecordId <= 0)
					{
						result = false;
						ErrorMsg = 
							"Error generating the next record Id in BuildContentAndLog" +
							Environment.NewLine + ErrorMsg;
						CommonUtilities.WindowsEventLoggingUtilities.WriteToErrorEvent(ErrorMsg, "BuildContentAndLog", 0);
					}
				}
	
				if (result)
				{
					ProgressLogging logging = new ProgressLogging();
					result = logging.BuildContent(StatusCode, NewRecordId, Comment, Message, DataAsJson, Plugin, Command, Method, out Content, out ErrorMsg);
					result &= (string.IsNullOrEmpty(ErrorMsg));
					if (result)
					{
						result = logging.InsertRecordWithValues(
							ConnectionString, MasterId, CompanyId, ClientId, Plugin, Command, Method, EntryType, Content, NewRecordId, out ErrorMsg);
						result &= (string.IsNullOrEmpty(ErrorMsg));
					}
					if (result == false) CommonUtilities.WindowsEventLoggingUtilities.WriteToErrorEvent(ErrorMsg, "BuildContentAndLog", 0);
				}
			}
			catch (Exception Ex)
			{
				string Msg =
					Ex.Message + Environment.NewLine +
					Ex.InnerException + Environment.NewLine +
					Ex.Source + Environment.NewLine +
					Ex.StackTrace;
				WindowsEventLoggingUtilities.WriteToErrorEvent(Msg, "BuildContentAndLog Exception", 0);
				//Console.WriteLine("BuildContentAndLog Exception" + Environment.NewLine + Msg);
			}
			
			return result;
		}
 
		public static int GetNextRecordId(string ConnectionString,
		                                  int MasterId,
		                                  int CompanyId,
		                                  int ClientId,
		                                  out string ErrorMsg)
    	{
			ErrorMsg = "";			
			int result = -1;
			
			DateTime CurrentDateTime = DateTime.Now;
			string CurrentDate = CurrentDateTime.ToString("yyyyMMdd");
			string CurrentTime = CurrentDateTime.ToString("HHmmss");
			
			string FileName = "";

			ProgressLogging inst = new ProgressLogging();
    		ErrorMsg = inst.CreateCurrentLoggingDB(out FileName);
			if (!string.IsNullOrEmpty(ErrorMsg))
			{
				WindowsEventLoggingUtilities.WriteToErrorEvent(
					"Error creating the sqlite database for " + CurrentDate + Environment.NewLine + ErrorMsg, 
					"ProgressLogging", 0);
				return result;
			}
			
			
			using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
			{
				conn.Open();
				
				StringBuilder QueryText = new StringBuilder();
				QueryText.AppendLine("INSERT INTO");
				QueryText.AppendLine("Entries(MasterId,  CompanyId,  ClientId,  DateAdded,  TimeAdded)");
				QueryText.AppendLine("VALUES (@MasterId, @CompanyId, @ClientId, @DateAdded, @TimeAdded);");
				QueryText.AppendLine("SELECT last_insert_rowid() AS NewId;");
				using (SQLiteCommand cmd = new SQLiteCommand(QueryText.ToString(), conn))
				{
					cmd.Parameters.Clear();
					cmd.Parameters.AddWithValue("MasterId", MasterId);
					cmd.Parameters.AddWithValue("CompanyId", CompanyId);
					cmd.Parameters.AddWithValue("ClientId", ClientId);
					cmd.Parameters.AddWithValue("DateAdded", CurrentDate);
					cmd.Parameters.AddWithValue("TimeAdded", CurrentTime);
					try 
					{
						result = (int)((long)cmd.ExecuteScalar());
					}
					catch (SQLiteException sex)
					{
						ErrorMsg = 
							"Sqlite Exception" + Environment.NewLine + 
							sex.Message + Environment.NewLine + 
							sex.StackTrace;
					}
					catch (Exception ex)
					{
						ErrorMsg = 
							ex.Message + Environment.NewLine + 
							ex.StackTrace;
					}
				}
				conn.Close();
			}
			
    		return result;
    	}
		
		public bool BuildContent(int StatusCode,
		                         int RecordId,		                         
		                         string Comment,
		                         string Message,
		                         string DataAsJson,
	                             string Plugin,
	                             string Command,
	                             string Method,
		                         out string Content,
                                 out string ErrorMsg)

		{
			Content = "";
			ErrorMsg = "";
	
			StringBuilder sb = new StringBuilder();
			StringWriter sw = new StringWriter(sb);
			using (JsonTextWriter writer = new JsonTextWriter(sw))
			{
				writer.Formatting = Formatting.Indented;
				writer.WriteStartObject();
					writer.WritePropertyName("Response");
					writer.WriteStartObject();
						JsonUtilities.AddPropertyNameAndValue(writer, "_comment", Comment);
						JsonUtilities.AddPropertyNameAndValue(writer, "status", Convert.ToString(StatusCode));
						JsonUtilities.AddPropertyNameAndValue(writer, "id", Convert.ToString(RecordId));
						
						if (!string.IsNullOrEmpty(Plugin) || !string.IsNullOrEmpty(Command) || !string.IsNullOrEmpty(Method))
						{
							writer.WritePropertyName("Info");
							writer.WriteStartObject();
								JsonUtilities.AddPropertyNameAndValue(writer, "Command", Command);
								JsonUtilities.AddPropertyNameAndValue(writer, "Plugin", Plugin);
								JsonUtilities.AddPropertyNameAndValue(writer, "Method", Method);
							writer.WriteEndObject();
						}
						
						writer.WritePropertyName("Message");
						writer.WriteStartObject();
							JsonUtilities.AddPropertyNameAndValue(writer, "errorNo", Convert.ToString(StatusCode));
							JsonUtilities.AddPropertyNameAndValue(writer, "message", Message);
						writer.WriteEndObject();
						
						writer.WritePropertyName("Data");
						writer.WriteStartObject();
							writer.WriteRaw(DataAsJson);
						writer.WriteEndObject();
						
					writer.WriteEndObject();
				writer.WriteEndObject();
			}
			Content = sb.ToString();
			return ((string.IsNullOrEmpty(ErrorMsg) && (!string.IsNullOrEmpty(Content))));
		}
		
		/// <summary>
		/// Populates the fields in the logging table that were not
		/// included when the record was created
		/// </summary>
		/// <returns> True or False depending on the success of the record update.</returns>
		public bool UpdateRecordWithValues(string ConnectionString,
		                                   bool LogEntry,
		                                   string Plugin,
		                                   string Command,
		                                   string Method,
		                                   string EntryType,
		                                   string Content,
		                                   int NewRecordId, 
		                                   out string ErrorMsg)
		{
			ErrorMsg = "";			
			bool result = true;
			
			ProgressLogging inst = new ProgressLogging();
			
			string CurrentDate = DateTime.Now.ToString("yyyyMMdd");
			string FileName = "";
			ErrorMsg = inst.CreateCurrentLoggingDB(out FileName);
			if (!string.IsNullOrEmpty(ErrorMsg))
			{
				WindowsEventLoggingUtilities.WriteToErrorEvent(
					"Error creating the sqlite database for " + CurrentDate + Environment.NewLine + ErrorMsg, 
					"ProgressLogging", 0);
				return result;
			}			
			
			using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
			{
				conn.Open();
				
				StringBuilder QueryText = new StringBuilder();
				QueryText.AppendLine("UPDATE Entries");
				QueryText.AppendLine("SET");
				QueryText.AppendLine("Plugin = @Plugin,");
				QueryText.AppendLine("Command = @Command,");
				QueryText.AppendLine("Method = @Method,");
				QueryText.AppendLine("EntryType = @EntryType,");			
				QueryText.AppendLine("Content = @Content");
				QueryText.AppendLine("WHERE ID = @NewId");
				
				using (SQLiteCommand cmd = new SQLiteCommand(QueryText.ToString(), conn))
				{
					cmd.CommandText = QueryText.ToString();
					cmd.Parameters.Clear();
					cmd.Parameters.AddWithValue("Plugin", Plugin);
					cmd.Parameters.AddWithValue("Command", Command);
					cmd.Parameters.AddWithValue("Method", Method);
					cmd.Parameters.AddWithValue("EntryType", EntryType);
					cmd.Parameters.AddWithValue("Content", Content); 
					cmd.Parameters.AddWithValue("NewId", NewRecordId); 

					try
					{
						cmd.ExecuteNonQuery();
					}
					catch (SQLiteException sex)
					{
						result = false;
						ErrorMsg = 
							"Sqlite Exception" + Environment.NewLine +
							sex.Message + Environment.NewLine + 
							sex.StackTrace;
						WindowsEventLoggingUtilities.WriteToErrorEvent(ErrorMsg, "UpdateRecordWithValues", 0);
					}
					catch (Exception ex)
					{
						result = false;
						ErrorMsg = 
							ex.Message + Environment.NewLine + 
							ex.StackTrace;
						WindowsEventLoggingUtilities.WriteToErrorEvent(ErrorMsg, "UpdateRecordWithValues", 0);
					}
				}
				conn.Close();
			}
			
    		return result;
		}		
		
		public bool InsertRecordWithValues(string ConnectionString,
		                                   int MasterId,
		                                   int CompanyId,
		                                   int ClientId,
		                                   string Plugin,
		                                   string Command,
		                                   string Method,
		                                   string EntryType,
		                                   string Content,
		                                   int NewRecordId, 
		                                   out string ErrorMsg)
		{
			ErrorMsg = "";			
			bool result = true;
			
			ProgressLogging inst = new ProgressLogging();
			string QueryText = 
				" INSERT INTO LoggingEntries ( Id,  MasterId,  Plugin,  Command,  Method,  EntryType,  Content,  CompanyId,  ClientId,  DateAdded,  TimeAdded)" +
				"                     Values (@Id, @MasterId, @Plugin, @Command, @Method, @EntryType, @Content, @CompanyId, @ClientId, @DateAdded, @TimeAdded)";
			//WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText:" + Environment.NewLine + QueryText, Method, 0);
			
			DateTime CurrentDateTime = DateTime.Now;
			string CurrentDate = CurrentDateTime.ToString("yyyyMMdd");
			string CurrentTime = CurrentDateTime.ToString("HHmmss");
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("Id", NewRecordId));

			Params.Add(new FbParameterItem("MasterId", MasterId));
			Params.Add(new FbParameterItem("Plugin", Plugin));
			Params.Add(new FbParameterItem("Command", Command));
			Params.Add(new FbParameterItem("Method", Method));
			Params.Add(new FbParameterItem("EntryType", EntryType));

			byte[] bytes = Encoding.ASCII.GetBytes(Content);
			Params.Add(new FbParameterItem("Content", bytes));
			
			Params.Add(new FbParameterItem("CompanyId", CompanyId));
			Params.Add(new FbParameterItem("ClientId", ClientId));
			Params.Add(new FbParameterItem("DateAdded", CurrentDate));
			Params.Add(new FbParameterItem("TimeAdded", CurrentTime));
			//UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, "ProgressLogging", MasterId);

			ErrorMsg = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
			result = (string.IsNullOrEmpty(ErrorMsg));
			if (result == false) WindowsEventLoggingUtilities.WriteToErrorEvent(ErrorMsg, QueryText, NewRecordId);
			
			/*
			string FileName = "";
			ErrorMsg = inst.CreateCurrentLoggingDB(out FileName);
			if (!string.IsNullOrEmpty(ErrorMsg))
			{
				WindowsEventLoggingUtilities.WriteToErrorEvent(
					"Error creating the sqlite database for " + CurrentDate + Environment.NewLine + ErrorMsg, 
					"ProgressLogging", 0);
				return result;
			}
			
			using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
			{
				conn.Open();
				
				StringBuilder QueryText = new StringBuilder();
				QueryText.AppendLine("UPDATE Entries");
				QueryText.AppendLine("SET");
				QueryText.AppendLine("Plugin = @Plugin,");
				QueryText.AppendLine("Command = @Command,");
				QueryText.AppendLine("Method = @Method,");
				QueryText.AppendLine("EntryType = @EntryType,");			
				QueryText.AppendLine("Content = @Content");
				QueryText.AppendLine("WHERE ID = @NewId");
				
				using (SQLiteCommand cmd = new SQLiteCommand(QueryText.ToString(), conn))
				{
					cmd.CommandText = QueryText.ToString();
					cmd.Parameters.Clear();
					cmd.Parameters.AddWithValue("Plugin", Plugin);
					cmd.Parameters.AddWithValue("Command", Command);
					cmd.Parameters.AddWithValue("Method", Method);
					cmd.Parameters.AddWithValue("EntryType", EntryType);
					cmd.Parameters.AddWithValue("Content", Content); 
					cmd.Parameters.AddWithValue("NewId", NewRecordId); 

					try
					{
						cmd.ExecuteNonQuery();
					}
					catch (SQLiteException sex)
					{
						result = false;
						ErrorMsg = 
							"Sqlite Exception" + Environment.NewLine +
							sex.Message + Environment.NewLine + 
							sex.StackTrace;
						WindowsEventLoggingUtilities.WriteToErrorEvent(ErrorMsg, "UpdateRecordWithValues", 0);
					}
					catch (Exception ex)
					{
						result = false;
						ErrorMsg = 
							ex.Message + Environment.NewLine + 
							ex.StackTrace;
						WindowsEventLoggingUtilities.WriteToErrorEvent(ErrorMsg, "UpdateRecordWithValues", 0);
					}
				}
				conn.Close();
			}
			*/
			
    		return result;
		}		

		/*
		public static string WriteParamsToInfoEvent(string QueryText, List<FbParameterItem> Params, string ModuleName, int MasterId)
		{
			string result = "";
			string text = "QueryText:" + Environment.NewLine + QueryText;
			try 
			{
				if (Params != null) {
					foreach (FbParameterItem current in Params) {
						text += Environment.NewLine;
						text = text + current.ParameterName + ": " + Convert.ToString(current.ParameterValue);
					}
				}
				result = WindowsEventLoggingUtilities.WriteToInfoEvent(text, ModuleName, MasterId);
			}
			catch (Exception ex) {
				result = ex.Message + Environment.NewLine + ex.InnerException;
			}
			return result;
		}
		*/
		
	    /* // Remove AddRecordWithId methods
		public static bool AddRecordWithId(bool LogEntry,
    	                                   int NewId,
		                                   int ModuleId,
		                                   string Method,
		                                   int MasterId,
		                                   int AddedByClientId,
		                                   string AddedBy,
		                                   string Content,
		                                   string ContentIsFile,
		                                   JsonResponseContent EntryType,
		                                   string DBConnectionString,
		                                   out string ErrorMsg)
		{
			return 
				AddRecordWithId(
					LogEntry, NewId, ModuleId, Method, MasterId, AddedByClientId, AddedBy, 
					Content, ContentIsFile, EntryType, DBConnectionString, out ErrorMsg);
		}
		
    	public static bool AddRecordWithId(bool LogEntry,
    	                                   int NewId,
		                                   int ModuleId,
		                                   string Method,
		                                   int MasterId,
		                                   int AddedByClientId,
		                                   string AddedBy,
		                                   string Content,
		                                   string ContentIsFile,
		                                   string EntryType,
		                                   string DBConnectionString,
		                                   out string ErrorMsg)
		{
			return 
				AddRecordWithId(
					LogEntry, NewId, ModuleId, Method, MasterId, AddedByClientId, AddedBy, 
					Content, ContentIsFile, EntryType, "", DBConnectionString, out ErrorMsg);
		}
		
	    public static bool AddRecordWithId(bool LogEntry,
    	                                   int NewId,
		                                   int ModuleId,
		                                   string Method,
		                                   int MasterId,
		                                   int AddedByClientId,
		                                   string AddedBy,
		                                   string Content,
		                                   string ContentIsFile,
		                                   JsonContentEntryType EntryType,
		                                   string Command,
		                                   string DBConnectionString,
		                                   out string ErrorMsg)
		{
			string EntryTypeStr = GetEntryTypeString(EntryType);
			return
				AddRecordWithId(
					LogEntry, NewId, ModuleId, Method, MasterId, AddedByClientId, AddedBy,
					Content, ContentIsFile, EntryTypeStr, Command, DBConnectionString, out ErrorMsg);			
		}
    	
    	public static bool AddRecordWithId(bool LogEntry,
    	                                   int NewId,
		                                   int ModuleId,
		                                   string Method,
		                                   int MasterId,
		                                   int AddedByClientId,
		                                   string AddedBy,
		                                   string Content,
		                                   string ContentIsFile,
		                                   string EntryType,
		                                   string Command,
		                                   string DBConnectionString,
		                                   out string ErrorMsg)
		{
			ErrorMsg = "";
    		if (LogEntry)
    		{
				string QueryText = "";		
				string dt = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ssss");
				string ConnectionString = DBConnectionString;
				if (String.IsNullOrEmpty(DBConnectionString))
				{
					ConnectionString = BuildSqliteConnectionString(out ErrorMsg);
				}
				if (string.IsNullOrEmpty(ErrorMsg))
				{		
					using (SQLiteConnection Conn = new SQLiteConnection(ConnectionString))
					{
						Conn.Open();
						using (SQLiteCommand cmd = new SQLiteCommand())
						{
								cmd.Connection = Conn;
								
								QueryText =
									"INSERT INTO " + FTableName +
									"(RAPML_ID, RAPML_ModuleId, RAPML_Method, RAPML_MasterId, RAPML_DateCreated," +
									"RAPML_AddedByClientId, RAPML_AddedBy, RAPML_Content, RAPML_ContentIsFile," +
									"RAPML_Command, RAPML_EntryType)" +
									"VALUES(@ID, @ModuleId, @Method, @MasterId, @DateCreated," +
									"@AddedByClientId, @AddedBy, @Content, @ContentIsFile," +
									"@Command, @EntryType)";
								cmd.CommandText = QueryText;
								cmd.Parameters.AddWithValue("@ID", NewId);
								cmd.Parameters.AddWithValue("@ModuleId", ModuleId);
								cmd.Parameters.AddWithValue("@Method", Method);
								cmd.Parameters.AddWithValue("@MasterId", MasterId);
								cmd.Parameters.AddWithValue("@DateCreated", DateTime.Now);
								cmd.Parameters.AddWithValue("@AddedByClientId", AddedByClientId);
								cmd.Parameters.AddWithValue("@AddedBy", AddedBy);
								cmd.Parameters.AddWithValue("@Content", Content);
								cmd.Parameters.AddWithValue("@ContentIsFile", ContentIsFile);
								cmd.Parameters.AddWithValue("@Command", Command);
								cmd.Parameters.AddWithValue("@EntryType", EntryType);
								try 
								{
									cmd.ExecuteNonQuery();
								}
								catch (Exception Ex)
								{
									ErrorMsg = Ex.Message + Environment.NewLine + Ex.StackTrace;
								}
						}
						
						Conn.Close();
					}
				}
    		}
			return (string.IsNullOrEmpty(ErrorMsg));
    	}    	
    	*/
 
		public static string GetEntryTypeString(JsonContentEntryType EntryType)
    	{
    		string Result = "";
    		switch (EntryType)
    		{
    			case JsonContentEntryType.Request: 			Result = "Request"; break;
    			case JsonContentEntryType.Response:         Result = "Response"; break;
    			case JsonContentEntryType.Logging_Entry:    Result = "Logging_Entry"; break;
    			case JsonContentEntryType.Validation_Error:	Result = "Validation Error"; break;
    			case JsonContentEntryType.Warning:          Result = "Warning"; break;
    			case JsonContentEntryType.Error:            Result = "Error"; break;
    			case JsonContentEntryType.Debugging:        Result = "Debugging"; break;
    			case JsonContentEntryType.QueryText:        Result = "QueryText"; break;
    		}
    		return Result;
    	}
 
		/* // Remove these methods
		public static int GetNextActivityId()
    	{
    		int result = -1;
    		string ErrorMsg = "";
    		string ConnectionString = BuildConnectionString(out ErrorMsg);
    		if (string.IsNullOrEmpty(ErrorMsg))
    		{
    			result = GetNextActivityId(ConnectionString);
    		}
			
    		return result;
    	}
    	
    	public static int GetNextActivityId(string ConnectionString)
    	{
    		//string TableName = "RPOWebServiceActivity";
    		string GeneratorName = FTableName + "_GEN";
    		
			int result = -1;
			string ExceptionText = "";
			object FieldValue = DBNull.Value;
			string QueryText = "SELECT GEN_ID({0}, 1) AS ResultValue FROM rdb$database";
			QueryText = string.Format(QueryText, GeneratorName);
			DataTable dt = FirebirdUtilities.GetFirstDataTable(ConnectionString, QueryText);
			
			if (dt.Rows.Count != 1)
			{
				ExceptionText =
					"Exception getting the next value in" + Environment.NewLine +
					"  TUsefulDBMethods.GetNextGeneratorValue";
				throw(new Exception(ExceptionText));
			}
			else 
			{
				FieldValue = dt.Rows[0]["ResultValue"];
				if (FieldValue != DBNull.Value) result = Convert.ToInt32(FieldValue);				
			}
			
			return result;
    	}
    	*/
    	
    	/*
    	public static bool AddRecord(bool LogEntry,
		                             int MasterId,
    	                             string Plugin,
    	                             string Command,
		                             string Method,
		                             int AddedByClientId,
		                             string AddedBy,
		                             string Content,
		                             string ContentIsFile,
		                             JsonContentEntryType EntryType,
		                             string DBConnectionString,
		                             out int NewId,
		                             out string ErrorMsg)
		{
    		string EntryTypeStr = GetEntryTypeString(EntryType);
			return 
				AddRecord(
					LogEntry, ModuleId, Method, MasterId, AddedByClientId, AddedBy, 
					Content, ContentIsFile, EntryTypeStr, DBConnectionString, out NewId, out ErrorMsg);
		}

		public static bool AddRecord(bool LogEntry,
    	                             int MasterId,
    	                             string Plugin,
    	                             string Command,
    	                             string Method,
    	                             string EntryType,
    	                             string Content,
    	                             int CompanyId,
    	                             int ClientId,
    	                             out int NewRecordId,
    	                             out string ErrorMsg)


		public static bool AddRecord(bool LogEntry,
    	                             int MasterId,
    	                             string Plugin,
    	                             string Command,
		                             string Method,
		                             string EntryType,
		                             int CompanyIdId,
		                             int ClientId,
		                             string Content,
		                             out int NewId,
		                             out string ErrorMsg)
		{
			return 
				AddRecord(
					LogEntry, MasterId, Method, MasterId, AddedByClientId, AddedBy, 
					Content, ContentIsFile, EntryType, "", DBConnectionString, out NewId, out ErrorMsg);
		}
		*/
    	
		public static bool AddRecord(bool LogEntry,
    	                             int MasterId,
    	                             string Plugin,
    	                             string Command,
		                             string Method,
		                             string Content,
		                             JsonContentEntryType EntryType,
		                             int CompanyId,
		                             int ClientId,
		                             out int NewId,
		                             out string ErrorMsg)
		{
    		string EntryTypeStr = GetEntryTypeString(EntryType);
			return 
				AddRecord(LogEntry, MasterId, Plugin, Command, Method, EntryTypeStr, 
				          Content, CompanyId, ClientId, out NewId, out ErrorMsg);
		}
    	
		/// <summary>
		/// Sqlite version of AddRecord 
		/// </summary>
		/*
		public static bool AddRecord(bool LogEntry,
    	                             int MasterId,
    	                             string Plugin,
    	                             string Command,
    	                             string Method,
    	                             string EntryType,
    	                             string Content,
    	                             int CompanyId,
    	                             int ClientId,
    	                             out int NewRecordId,
    	                             out string ErrorMsg)
		{
			ErrorMsg = "";
			bool Result = false;
			NewRecordId = 0;

			if (LogEntry == false)
			{
				// If not logging then quit before doing anything else
				return false;
			}
			
			string ConnectionString = BuildSqliteConnectionString(out ErrorMsg);
			if (!string.IsNullOrEmpty(ErrorMsg)) return false;
			
			if (LogEntry == true)
			{			
				// Only generate a new record if entry will be logged
				NewRecordId = GetNextRecordId(ConnectionString, MasterId, CompanyId, ClientId, out ErrorMsg);
				if (NewRecordId < 0)
				{
					Result = false;
					ErrorMsg = 
						"Error generating the next record Id in AddRecord" +
						Environment.NewLine + ErrorMsg;
					CommonUtilities.WindowsEventLoggingUtilities.WriteToErrorEvent(ErrorMsg, "BuildContentAndLog", 0);
				}
			}
				
			if (NewRecordId > 0)
			{
				ProgressLogging logging = new ProgressLogging();
				
				WindowsEventLoggingUtilities.WriteToInfoEvent("Content: " + Content, "BuildContent", 0);
			
				Result =
					logging.UpdateRecordWithValues(
						ConnectionString, LogEntry, Plugin, Command, Method, EntryType, Content, NewRecordId, out ErrorMsg);
				Result &= (string.IsNullOrEmpty(ErrorMsg));
			}

			return Result;
		}
		*/
		
		/// <summary>
		/// Firebird version of AddRecord 
		/// </summary>
		public static bool AddRecord(bool LogEntry,
    	                             int MasterId,
    	                             string Plugin,
    	                             string Command,
    	                             string Method,
    	                             string EntryType,
    	                             string Content,
    	                             int CompanyId,
    	                             int ClientId,
    	                             out int NewRecordId,
    	                             out string ErrorMsg)
		{
			ErrorMsg = "";
			bool Result = false;
			NewRecordId = 0;

			if (LogEntry == false)
			{
				// If not logging then quit before doing anything else
				return false;
			}			
			
			string ConnectionString = "";
			
			if (LogEntry == true)
			{			
				// Only generate a new record if entry will be logged
				if (!TryGetConnectionString(out ConnectionString, out ErrorMsg)) return false;
				int UseCompanyId = CompanyId;
				if (UseCompanyId < 0) UseCompanyId = 0;
				int UseClientId = ClientId;
				if (UseClientId < 0) UseClientId = 0;
				NewRecordId = 
					UsefulDBMethods.GeneratorHelpers.GetNextGeneratorId(
						ConnectionString, "LOGGINGENTRIES_ID_GEN", UseCompanyId, UseClientId, out ErrorMsg);
				if (!string.IsNullOrEmpty(ErrorMsg)) WindowsEventLoggingUtilities.WriteToErrorEvent("GetNextGeneratorId: " + ErrorMsg, Method, MasterId);

				if (NewRecordId <= 0)
				{
					Result = false;
					ErrorMsg = 
						"Error generating the next record Id in AddRecord" +
						Environment.NewLine + ErrorMsg;
					CommonUtilities.WindowsEventLoggingUtilities.WriteToErrorEvent(ErrorMsg, "BuildContentAndLog", 0);
				}
			}
				
			if (NewRecordId > 0)
			{
				ProgressLogging logging = new ProgressLogging();
				
				//WindowsEventLoggingUtilities.WriteToErrorEvent("Content: " + Content, "BuildContent", 0);
			
				Result =
					logging.InsertRecordWithValues(
						ConnectionString, MasterId, CompanyId, ClientId, Plugin, Command, Method, EntryType, Content, NewRecordId, out ErrorMsg);
				Result &= (string.IsNullOrEmpty(ErrorMsg));
			}

			return Result;
		}
    	
    	/*		
		public string LogOnException(string Method,
		                             Exception Ex,
		                             string DataAsJson)	
		{
			string Msg = Ex.Message + Environment.NewLine + Ex.InnerException;
			string result = LogOnException(Method, Msg, DataAsJson);
			
			return result;
		}
		
		public string LogOnException(string Method, 	
		                             string Msg, 		
		                             string DataAsJson)	
		{
			
			string result = "";
			try				
			{	
				this.LogToErrorEvent("LogOnException: " + Method + Environment.NewLine + Msg);
				//WinLoggingUtils.WriteToInfoEvent("LogOnException: " + Method + Environment.NewLine + Msg, this.Module, 0);
				string ErrorMsg = "";
				int TempId = 0;
				
				int StatusCode = (int)GeneralResponseStatus.LogOnException;
				bool res =
					BuildContentAndLog(
						true, StatusCode, "Exception in Method: " + Method, Msg, DataAsJson, JsonContentEntryType.Error,
						this.Module, Method, out TempId, out result, out ErrorMsg);
				this.NewId = TempId;
			}
			catch
			{
				// trap any error that may occur during logging
			}
			
			return result;
		}
		
		public string LogOnInsideMethod(string Method)
		{
			return LogOnInsideMethod(Method, "");
		}
		
		public string LogOnInsideMethodWithMessage(string Method,
		                               			   string Message)			
		{
			string lJsonMsg = JsonUtilities.EscapeJSONString_JsonConvert(Message);
			
			string DataAsJson = "";
			
			DataAsJson += JsonUtilities.AddPropertyNameAndValue("Message", lJsonMsg, false);
			
			return LogOnInsideMethod(Method, DataAsJson);
		}
		
		public string LogOnInsideMethod(string Method,		//: String;
		                                string DataAsJson)	//: String = ''): String;
		{
			this.LogToInfoEvent("LogOnInsideMethod: " + Method, DataAsJson);
			//WinLoggingUtils.WriteToInfoEvent("LogOnInsideMethod: " + Method, this.Module, 0);
			string result = "";
			string ErrorMsg = "";
			int TempId = 0;
			int StatusCode = 0;
			bool res = false;
			string Str = "Inside Method: " + Method;
			
			try
			{
				StatusCode = (int)GeneralResponseStatus.LogOnInsideMethod;
				res = 
					BuildContentAndLog(
						this.DebugMode, StatusCode, "", Str, DataAsJson, JsonContentEntryType.Logging_Entry,
						this.Module, Method, out TempId, out result, out ErrorMsg);
				this.NewId = TempId;
				
				result = ErrorMsg;
			}
			catch (Exception Ex)
			{
				//WinLoggingUtils.WriteToInfoEvent(Ex.Message + Environment.NewLine + Ex.InnerException, this.Module, 0);
				this.LogToErrorEvent(Ex.Message + Environment.NewLine + Ex.InnerException);
				result = LogOnException(Method, Ex, "");
			}
			
			return result;		
		}
		
		public string LogOnLeavingMethod(string Method)
		{
			return LogOnLeavingMethod(Method, "");
		}
		
		public string LogOnLeavingMethod(string Method,
		                                 string DataAsJson)
		{
			this.LogToInfoEvent("LogOnLeavingMethod: " + Method, DataAsJson);
			//WinLoggingUtils.WriteToInfoEvent("LogOnLeavingMethod: " + Method, this.Module, 0);
			string result = "";
			string ErrorMsg = "";
			int TempId = 0;
			int StatusCode = (int)GeneralResponseStatus.LogOnLeavingMethod;
			bool res = false;
			string Str = "Leaving Method: " + Method;
			
			try
			{
				res =
					BuildContentAndLog(
						this.DebugMode, StatusCode, "", Str, DataAsJson, JsonContentEntryType.Logging_Entry,
						this.Module, Method, out TempId, out result, out ErrorMsg);
				this.NewId = TempId;
				
				result = ErrorMsg;
			}
			catch (Exception Ex)
			{
				result = LogOnException(Method, Ex, "");
			}
			
			return result;
		}
		
		public static string LogOnErrorValidatingData(string Method,
		                                       string Msg,
		                                       string DataAsJson)	//: String = ''): String;
		{
			//this.LogToInfoEvent("LogOnErrorValidatingData: " + Method, Msg, DataAsJson);
			//WinLoggingUtils.WriteToInfoEvent("LogOnErrorValidatingData: " + Method, this.Module, 0);
			
			this.LogToErrorEvent("LogOnErrorValidatingData: " + Method, Msg, DataAsJson);
			
			string result = "";
			string ErrorMsg = "";
			int TempId = 0;
			int StatusCode = (int)GeneralResponseStatus.LogOnErrorValidatingData;
			bool res = false;
			string Str = "Inside Method: " + Method;
			
			try
			{
				res =
					BuildContentAndLog(
						true, StatusCode, Str, Msg, DataAsJson, JsonContentEntryType.Error,
						this.Module, Method, out TempId, out result, out ErrorMsg);
				this.NewId = TempId;
				
				result = ErrorMsg;
			}
			catch (Exception Ex)
			{
				result = LogOnException(Method, Ex, "");
			}

			return result;
		}
		
		public string LogOnErrorSelectingData(string Method,
		                                      string Msg,
		                                      string QueryText)
		{
			this.LogToInfoEvent("LogOnErrorSelectingData: " + Method, QueryText);
			//WinLoggingUtils.WriteToInfoEvent("LogOnErrorSelectingData: " + Method, this.Module, 0);
			string result = "";
			string ErrorMsg = "";
			int TempId = 0;
			int StatusCode = (int)GeneralResponseStatus.LogOnErrorSelectingData;
			bool res = false;
			string Str = "Inside Method: " + Method;
			string DataAsJson = QueryText; // make this a JSON valid string
			
			try
			{
				res =
					BuildContentAndLog(
						true, StatusCode, Str, Msg, DataAsJson, JsonContentEntryType.Error,
						this.Module, Method, out TempId, out result, out ErrorMsg);
				this.NewId = TempId;
				
				result = ErrorMsg;
			}
			catch (Exception Ex)
			{
				result = LogOnException(Method, Ex, "");
			}

			return result;
		}
		
		public string LogResponse(string Method, string DataAsJson)
		{
			this.LogToInfoEvent("LogResponse: " + Method, DataAsJson);
			//WinLoggingUtils.WriteToInfoEvent("LogResponse: " + Method, this.Module, 0);
			string result = "";
			string ErrorMsg = "";
			int TempId = 0;
			int StatusCode = (int)GeneralResponseStatus.LogOnSuccess;
			bool res = false;
			
			try
			{
				res =
					BuildContentAndLog(
						true, StatusCode, "SUCCESS", "SUCCESS", DataAsJson, JsonContentEntryType.Response,
						this.Module, Method, out TempId, out result, out ErrorMsg);
				this.NewId = TempId;
				
				if (!string.IsNullOrEmpty(ErrorMsg)) result = ErrorMsg;
			}
			catch (Exception Ex)
			{
				result = LogOnException(Method, Ex, "");
			}

			return result;
		}
		
		public string LoggingEntry(string Method, string Msg, string DataAsJson)
		{
			this.LogToInfoEvent("LoggingEntry: " + Method, DataAsJson);
			//WinLoggingUtils.WriteToInfoEvent("LoggingEntry: " + Method, this.Module, 0);
			string result = "";
			string ErrorMsg = "";
			int TempId = 0;
			int StatusCode = (int)GeneralResponseStatus.LoggingEntry;
			bool res = false;
			
			try
			{
				res =
					BuildContentAndLog(
						true, StatusCode, "", Msg, DataAsJson, JsonContentEntryType.Logging_Entry,
						this.Module, Method, out TempId, out result, out ErrorMsg);
				this.NewId = TempId;
				
				if (!string.IsNullOrEmpty(ErrorMsg)) result = ErrorMsg;
			}
			catch (Exception Ex)
			{
				result = LogOnException(Method, Ex, "");
			}

			return result;
		}
		
		public string LogOnAuthorisationError(string Method, string Msg, string DataAsJson)
		{
			
			string result = "";
			try				
			{	
				this.LogToErrorEvent("LogOnAuthorisationError: " + Method + Environment.NewLine + Msg);
				//WinLoggingUtils.WriteToInfoEvent("LogOnException: " + Method + Environment.NewLine + Msg, this.Module, 0);
				string ErrorMsg = "";
				int TempId = 0;
				
				int StatusCode = (int)GeneralResponseStatus.LoggingOnErrorAuthorising;
				bool res =
					BuildContentAndLog(
						true, StatusCode, "Authorisation Error: " + Method, Msg, "", JsonContentEntryType.Error,
						this.Module, Method, out TempId, out result, out ErrorMsg);
				this.NewId = TempId;
			}
			catch
			{
				// trap any error that may occur during logging
			}
			
			return result;
		}
		
		public string LogOnProcessingError(string Method, string Msg, string DataAsJson)
		{
			
			string result = "";
			try				
			{	
				this.LogToErrorEvent("LogOnProcessingError: " + Method + Environment.NewLine + Msg);
				//WinLoggingUtils.WriteToInfoEvent("LogOnException: " + Method + Environment.NewLine + Msg, this.Module, 0);
				string ErrorMsg = "";
				int TempId = 0;
				
				int StatusCode = (int)GeneralResponseStatus.LoggingOnErrorProcessing;
				bool res =
					BuildContentAndLog(
						true, StatusCode, "Processing Error: " + Method, Msg, "", JsonContentEntryType.Error,
						this.Module, Method, out TempId, out result, out ErrorMsg);
				this.NewId = TempId;
			}
			catch
			{
				// trap any error that may occur during logging
			}
			
			return result;
		}
		*/
	}
}