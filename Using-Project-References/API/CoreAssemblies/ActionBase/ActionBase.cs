/*
 * Created by SharpDevelop.
 * User: John
 * Date: 23/10/2018
 * Time: 8:34 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CommonUtilities;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace EAOfficeAPI
{
	public enum ActionParameterStatus { OK, NotProvided, Invalid};
	/// <summary>
	/// Description of MyClass.
	/// </summary>
	public class ActionBase
	{
		public ActionBase()
		{
			this.CommonCollection = new NameValueCollection();
			this.DataCollection = new NameValueCollection();
			string Result = "";
			this.APISettings = APISettingsFile.GetAPISettings(APIAppType.Service, out Result);
			this.APISettings.DebugLogging = true;
			this.DebugMode = true;
			this.ModuleSessionName = "EAOfficeAPI_" + this.APISettings.APISessionName;
			this.EventViewerLogging = this.APISettings.EventViewerLogging;
		}
		
		public int MasterId { get; set; }
		public APISettingsItem APISettings { get; set; }
		public RequestCommonItem CommonItem { get; set; }
		public JObject DataObject { get; set; }
		public string FbConnectionString { get; set; }
		public string SqliteConnectionString { get; set; }
		public string ModuleSessionName { get; set; }
		public bool DebugMode { get; set; }
		public bool EventViewerLogging { get; set; }		
		
		public string JsonSource { get; set; }
		public NameValueCollection CommonCollection { get; set; }
		public NameValueCollection DataCollection { get; set; }
		
		/*
		public string Request { get; set; }
		public string ModuleMethodName { get; set; }
		public string Module { get; set; }
		public string Version { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public string ClientReference { get; set; }
		public APISettingsItem APISettings { get; set; }
		
		
		public string ConnectionString { get; set; }
		public string SqliteConnectionString { get; set; }

		// Where do I get these from?
		public int ModuleId { get; set; }
		public int AddedByClientId { get; set; }
		public string AddedBy { get; set; }		
		
		//public string Command { get; set; }
		public int NewId { get; set; }
		public string Dummy { get; set; }
		
		/*
		private JObject FMethodObject = null;
		public JObject MethodObject { get { return FMethodObject; } }
		private JObject FDataObject = null;
		public JObject DataObject { get { return FDataObject; } }
		*/

		public const int InvalidValue	= -1;
		public const int NotProvided	= -2;
		public const string SuccessMessage = "SUCCESS";
		
		public bool Setup(int MasterId,
		                  APISettingsItem APISettings,
		                  string Request,
		                  out string ErrorMsg)
		{
			ErrorMsg = "";
			string MethodName = "Setup";
			bool Result = true; // set to true and prove otherwise
			
			RequestCommonItem ARequestCommonItem;
			JObject ADataObject;
			
			try
			{
				this.JsonSource = Request;
				ARequestCommonItem = BuildRequestCommonItem(out ErrorMsg);
				LogOnInsideMethod(MethodName);
				//this.CommonItem = BuildRequestCommonItem(Request, out ErrorMsg);
				Result = (string.IsNullOrEmpty(ErrorMsg) && (ARequestCommonItem != null));
				if (!Result) return Result;			

				//JObject ADataObject = BuildDataObject(Request, out ErrorMsg);
				ADataObject = BuildDataObject(out ErrorMsg);
				Result = (string.IsNullOrEmpty(ErrorMsg) && (ADataObject != null));
				if (!Result) return Result;
				
				ErrorMsg = MakeCollections(ADataObject);
				Result = (string.IsNullOrEmpty(ErrorMsg));
				if (!Result) return Result;
				
			}
			finally
			{
				LogOnLeavingMethod(MethodName);
			}
			
			return Setup(MasterId, ARequestCommonItem, ADataObject, APISettings, out ErrorMsg);
		}
		
		private RequestCommonItem BuildRequestCommonItem(out string ErrorMsg)
		{
			return BuildRequestCommonItem(this.JsonSource, out ErrorMsg);
		}
		
		private RequestCommonItem BuildRequestCommonItem(string Source, 
		                                                 out string ErrorMsg)
		{
			RequestCommonItem Result = RequestCommonProcessor.LoadFromRequest(Source, out ErrorMsg);
			if (string.IsNullOrEmpty(ErrorMsg))
			{
				ErrorMsg = RequestCommonProcessor.ValidateCommonItem(Result);				
			}
			
			return Result;
		}
		
		private JObject BuildDataObject(out string ErrorMsg)
		{
			return BuildDataObject(this.JsonSource, out ErrorMsg);
		}
		
		private JObject BuildDataObject(string Source,
		                                out string ErrorMsg)
		{
			ErrorMsg = "";
			string MethodName = "BuildDataObject";
			JObject Result = null;
			JObject jobj = JsonConvert.DeserializeObject<JObject>(Source);
			if (jobj == null)
			{
				ErrorMsg = "Error deserializing the Request";
			}
			else
			{
				Result = (jobj["data"] as JObject);
				if (Result == null)
				{
					ErrorMsg = "Missing data section in Request.";
				}
			}
			
			if (!string.IsNullOrEmpty(ErrorMsg))
			{
				WindowsEventLoggingUtilities.WriteToInfoEvent(ErrorMsg, MethodName, 0);
			}
			
			if (Result == null) ErrorMsg = "Inside: " + MethodName + " Result (DataObject) IS NULL";
			
			return Result;
		}
		
		public string MakeDataCollectionFromDataObject(JObject ADataObject)
		{
			string Result = "";
			string Method = "MakeDataCollectionFromDataObject";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, MasterId);
					
			LogOnInsideMethod(Method);
			try 
			{
				Dictionary<string, string> dict;

				if (ADataObject == null)
				{
					Result = "Source DataObject IS NULL";
				}
				
				WindowsEventLoggingUtilities.WriteToInfoEvent("DataObject as string" + Environment.NewLine + ADataObject.ToString(), Method, 0);
				
				JObject DataObj = null;
				if (string.IsNullOrEmpty(Result))
				{
					DataObj = (ADataObject["data"] as JObject);
					if (DataObj == null)
					{
						DataObj = ADataObject;
					}
				}
				
				if (string.IsNullOrEmpty(Result) && (DataObj != null))
				{
					dict = DataObj.ToObject<Dictionary<string, string>>();
					if (dict == null) Result = "DataObject Dictionary IS NULL";
					
					if (string.IsNullOrEmpty(Result) && (dict != null))
					{
						foreach (KeyValuePair<string, string> Item in dict)
						{
							DataCollection.Add(Item.Key, Item.Value);
						}
					}
				}
			}
			catch (Exception Ex)
			{
				Result = LogOnException(Method, Ex, "");
			}
			finally
			{
				LogOnLeavingMethod(Method);
			}
			
			return Result;
		}
			
		public string MakeCollections(JObject ADataObject)
		{
			string Result = "";
			string Method = "MakeCollections";
			LogOnInsideMethod(Method);
			try
			{
				Dictionary<string, string> dict;

				JObject jobj = JsonConvert.DeserializeObject<JObject>(this.JsonSource);
				if (jobj == null)
				{
					Result = "Error deserializing the Request";
					return Result;
				}

				JObject CommonObject = (jobj["common"] as JObject);
				if (CommonObject == null)
				{
					Result = "CommonObject IS NULL";
					return Result;
				}
				if (CommonObject != null)
				{
					dict = CommonObject.ToObject<Dictionary<string, string>>();
					if (dict == null) return "CommonObject Dictionary IS NULL";

					if (dict != null)
					{
						foreach (var Item in dict)
						{
							CommonCollection.Add(Item.Key, Item.Value);
						}
					}
				}

				if (ADataObject == null) return "DataObject IS NULL";

				foreach (JProperty Prop in (JToken)ADataObject)
				{
					//string Msg = Prop.Name + ": " + Prop.Value;
					//WindowsEventLoggingUtilities.WriteToInfoEvent(Msg, Method, 0);
					string PropValue = "" + Prop.Value;
					//WindowsEventLoggingUtilities.WriteToInfoEvent(Prop.Name + ": " + PropValue, Method, 0);
					DataCollection.Add(Prop.Name, PropValue);
				}
				//WindowsEventLoggingUtilities.WriteToInfoEvent("6", Method, 0);

				/*
				dict = ADataObject.ToObject<Dictionary<string, string>>();
				WindowsEventLoggingUtilities.WriteToInfoEvent("7", Method, 0);
				if (dict == null) return "DataObject Dictionary IS NULL";

				if (dict != null)
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent("7", Method, 0);
					foreach (var Item in dict)
					{
						DataCollection.Add(Item.Key, Item.Value);
					}
					WindowsEventLoggingUtilities.WriteToInfoEvent("8", Method, 0);
				}
				*/
	
			}
			catch (Exception Ex)
			{
				Result =
					Ex.Message + Environment.NewLine +
					Ex.InnerException + Environment.NewLine +
					Ex.StackTrace;
			}
			
			return Result;
		}
		
		public object GetCollectionValue(string Source, string Key)
		{
			NameValueCollection Coll;
			if (StringUtilities.SameText(Source, "Common")) Coll = CommonCollection; else Coll = DataCollection;
			return Coll[Key];
		}
		
		public string GetCollectionValue(string Source, string Key, string DefaultValue)
		{
			string Result = DefaultValue;
			object CollObj = GetCollectionValue(Source, Key);
			if (CollObj != null) Result = Convert.ToString(CollObj);
			return Result;
		}
		
		public int GetCollectionValue(string Source, string Key, int DefaultValue)
		{
			int Result = DefaultValue;
			object CollObj = GetCollectionValue(Source, Key);
			if (CollObj != null) Result = Convert.ToInt32(CollObj);
			return Result;
		}
		
		public void WriteCollectionInfo()
		{
			string Str = "";
			string Method = "WriteCollectionInfo";
			List<string> StrList = new List<string>();
			
			StrList.Add("CommonCollection.Count: " + CommonCollection.Count.ToString());
			if (CommonCollection.Count > 0)
			{
				foreach (string TheKey in CommonCollection.Keys)
				{
					Str = TheKey + ":" + CommonCollection.Get(TheKey);
					StrList.Add(Str);
				}
			}

			StrList.Add("DataCollection.Count: " + DataCollection.Count.ToString());
			if (DataCollection.Count > 0)
			{
				foreach (string TheKey in DataCollection.Keys)
				{
					Str = TheKey + ":" + DataCollection.Get(TheKey);
					StrList.Add(Str);
				}
			}
			
			WindowsEventLoggingUtilities.WriteToInfoEvent(string.Join(Environment.NewLine, StrList.ToArray()), Method, 0);
		}
		

		public bool Setup(int MasterId,
		                  RequestCommonItem CommonItem,
		                  JObject DataObject,
		                  APISettingsItem APISettings,
		                  out string ErrorMsg)
		{
			ErrorMsg = "";
			string MethodName = "Setup";
			bool Result = true; // set to true and prove otherwise
			
			LogOnInsideMethod(MethodName);
			try
			{
				this.MasterId = MasterId;
				this.APISettings = APISettings;
				this.CommonItem = CommonItem;
				this.DataObject = DataObject;
				this.SqliteConnectionString = ProgressLogging.BuildSqliteConnectionString(out ErrorMsg);
				Result = (string.IsNullOrEmpty(ErrorMsg) && !string.IsNullOrEmpty(this.SqliteConnectionString));
				if (!Result) return Result;			
			
				// If appropriate setup the DB Connection for all commands
				if ((Result) && (string.IsNullOrEmpty(this.FbConnectionString)))
	    		{
	    			this.FbConnectionString = CommonDBMethods.Common.ConnectionString(APISettings);
					Result = Result && (!string.IsNullOrEmpty(this.FbConnectionString));
				}
				
				/*
				if (Result)
			    {
					if ((this.DataCollection.Count == 0) && (this.DataObject != null))
					{
						//WindowsEventLoggingUtilities.WriteToInfoEvent("Collection entries before", MethodName, 0);
						//WriteCollectionInfo();
						ErrorMsg = MakeDataCollectionFromDataObject(this.DataObject);
						if (!string.IsNullOrEmpty(ErrorMsg))
						{
							WindowsEventLoggingUtilities.WriteToErrorEvent("Error performing MakeCollections:" + Environment.NewLine + ErrorMsg, MethodName, 0);
						}
						//WindowsEventLoggingUtilities.WriteToInfoEvent("Collection entries after", MethodName, 0);
						//WriteCollectionInfo();
					}
			    }
			    */
				
				Result = (string.IsNullOrEmpty(ErrorMsg));
				if (Result)
				{
					this.ModuleSessionName = "EAOfficeAPI_" + this.APISettings.APISessionName;
					this.DebugMode = this.CommonItem.DebugValue;
					this.EventViewerLogging = this.APISettings.EventViewerLogging;
				}
				else
				{
					// Keep at the bottom
					if (!string.IsNullOrEmpty(ErrorMsg))
					{
						LogOnErrorValidatingData(MethodName, ErrorMsg, "");
					}				
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("ActionBase.Setup:" + Environment.NewLine + ex.StackTrace);
			}
			finally
			{
				LogOnLeavingMethod(MethodName);
			}
			
			return Result;
		}
		
		public string BuildErrorResult(string ErrorMsg)
		{
			return ErrorMsg;
		}
		
		public void LogToInfoEvent(string Message)
		{
			//LogToInfoEvent(Message, this.Module, 0);
			if ((this.DebugMode) && (this.EventViewerLogging))
			{
				string lMessage = 
					WindowsEventLoggingUtilities.GetMessageSubStringForEventViewer(Message, WindowsEventLoggingUtilities.EventViewerLogDefaultByteLimit);
				
				WindowsEventLoggingUtilities.WriteToInfoEvent(lMessage, this.ModuleSessionName, this.MasterId);
			}
		}
		
		public void LogToInfoEvent(string Message, string Message2)
		{
			string lMessage = Message;
			
			if (!String.IsNullOrEmpty(Message2))
			{
			  lMessage += Environment.NewLine + Environment.NewLine + Message2;
			}
			
			LogToInfoEvent(lMessage);
		}		

		public void LogToInfoEvent(string Message, string Message2, string Message3)
		{
			string AMessage = Message;
			
			if (!String.IsNullOrEmpty(Message2))
			{
			  AMessage += Environment.NewLine + Environment.NewLine + Message2;
			}
			
			if (!String.IsNullOrEmpty(Message3))
			{
			  AMessage += Environment.NewLine + Environment.NewLine + Message3;
			}
			
			LogToInfoEvent(AMessage);
		}				
		
		/*
		public void LogToInfoEvent(string Message, string Source, int EventId)
		{
			if (this.DebugMode)
				WinLoggingUtils.WriteToInfoEvent(Message, Source + "_" + this.RPOAPISettings.APISessionName, EventId);
		}
		*/
		
		public void LogToErrorEvent(string Message)
		{
			//LogToErrorEvent(Message, this.Module, 0);
			// Always log errors
			WindowsEventLoggingUtilities.WriteToErrorEvent(Message, this.ModuleSessionName, this.MasterId);
		}
		
		public void LogToErrorEvent(string Message1, string Message2, string Message3)
		{
			//LogToErrorEvent(Message, this.Module, 0);
			// Always log errors
			
			string Message = Message1;
			
			if (!String.IsNullOrEmpty(Message2))
			{
			  Message += Environment.NewLine + Environment.NewLine + Message2;
			}
			
			if (!String.IsNullOrEmpty(Message3))
			{
			  Message += Environment.NewLine + Environment.NewLine + Message3;
			}
			
			WindowsEventLoggingUtilities.WriteToErrorEvent(Message, this.ModuleSessionName, this.MasterId);
		}
		
		/*
		public void LogToErrorEvent(string Message, string Source, int EventId)
		{
			// Always log errors
			WinLoggingUtils.WriteToErrorEvent(Message, Source, EventId);			
		}
		*/
		
		public void LogToWarningEvent(string Message)
		{
			//LogToWarningEvent(Message, this.Module, 0);
			if (this.DebugMode)
				WindowsEventLoggingUtilities.WriteToWarningEvent(Message, this.ModuleSessionName, this.MasterId);
		}
		
		private bool GetRPOClientReferenceFromDB(string aDBConnectionString, out string aClientReference, bool aLogging, out string aErrorMsg)
		{
			bool lResult = false;
				
			aErrorMsg = "";
			
			aClientReference = "";
			
			//WriteToLog("Starting Process...", "GetStrataUserNameAndPasswordFromDB", "", aLogging);						
			LogToInfoEvent("Starting Process... in GetStrataUserNameAndPasswordFromDB");
			try
			{				
				//aClientReference = RPOSystemSetupDBHelpers.GetRPOSystemClientID(aDBConnectionString);
				lResult = true;
				
				//WriteToLog("Finishing Process", "GetStrataUserNameAndPasswordFromDB", "", aLogging);																		
				LogToInfoEvent("Finishing Process in GetStrataUserNameAndPasswordFromDB");
			}
			catch (Exception e)
			{
				aErrorMsg = e.Message;
				
				//WriteToErrorLog(e.ToString(), "GetStrataUserNameAndPasswordFromDB", "Error", aLogging);
				LogToErrorEvent(
					"GetStrataUserNameAndPasswordFromDB" + Environment.NewLine +
					e.ToString());
			}			
			return lResult;
		}
		
		/*
		public bool BuildContentAndLog(bool LogEntry,
		                               int StatusCode,
		                               string Comment,
		                               string Message,
		                               string DataAsJson,
		                               JsonContentEntryType EntryType,
		                               out int NewRecordId,
		                               out string Content,
		                               out string ErrorMsg)
		{
			return
				BuildContentAndLog(
					LogEntry, StatusCode, Comment, Message, DataAsJson, EntryType,
					"", "", out NewRecordId, out Content, out ErrorMsg);
		}
		
		public bool BuildContentAndLog(bool LogEntry,
		                               int StatusCode,
		                               string Comment,
		                               string Message,
		                               string DataAsJson,
		                               JsonContentEntryType EntryType,
		                               string Module,
		                               string Method,
		                               out int NewRecordId,
		                               out string Content,
		                               out string ErrorMsg)
		{
			string EntryTypeStr = GetEntryTypeString(EntryType);
			
			return 
				BuildContentAndLog(
					LogEntry, StatusCode, Comment, Message, DataAsJson, EntryTypeStr, 
					Module, Method, out NewRecordId, out Content, out ErrorMsg);
		}
		
		public bool BuildContentAndLog(bool LogEntry,
		                               int StatusCode,
		                               string Comment,
                                       string Message,
                                       string DataAsJson,
                                       string EntryType,
                                       string Module, 
                                       string Method,
                                       out int NewRecordId,
                                       out string Content,
                                       out string ErrorMsg)
		{
			bool result = true;
			
			Content = "";
			ErrorMsg = "";
			
			if (LogEntry == true)
			{			
				// Only generate a new record if entry will be logged
				NewRecordId = GetNextActivityId(this.SqliteConnectionString);
				if (NewRecordId < 0)
				{
					ErrorMsg = "Error generating the next record Id in BuildContentAndLog";
					LogToErrorEvent(ErrorMsg);
					result = false;
				}
			}
			else
			{
				NewRecordId = 0;
			}
				
			if (result)
			{
				result = BuildContent(StatusCode, NewRecordId, Comment, Message, DataAsJson, out Content, out ErrorMsg);
				result &= (string.IsNullOrEmpty(ErrorMsg));
			}
			
			if (result)
			{
				result =
					AddRecordWithId(
						LogEntry, NewRecordId, this.ModuleId, Method, this.MasterId,
						this.AddedByClientId, this.AddedBy, Content, "N", EntryType, this.SqliteConnectionString,
						out ErrorMsg);
				result &= (string.IsNullOrEmpty(ErrorMsg));
			}
			
			return result;
		}
		
		public bool BuildContent(int StatusCode,
	                             int MasterId,
		                         string Comment,
		                         string Message,
		                         string DataAsJson,
		                         out string Content,
		                         out string ErrorMsg)
		{
			return BuildContent(StatusCode, MasterId, Comment, Message, DataAsJson, "", "", out Content, out ErrorMsg);
		}
		
	    public bool BuildContent(int StatusCode,
	                             int MasterId,
		                         string Comment,
		                         string Message,
	                             string DataAsJson,
	                             string Module,
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
						JsonUtilities.AddPropertyNameAndValue(writer, "id", Convert.ToString(MasterId));
						
						if (!string.IsNullOrEmpty(Module) || !string.IsNullOrEmpty(Method))
						{
							writer.WritePropertyName("Info");
							writer.WriteStartObject();
								JsonUtilities.AddPropertyNameAndValue(writer, "Module", Module);
								JsonUtilities.AddPropertyNameAndValue(writer, "Method", Method);
							writer.WriteEndObject();
						}
						
						writer.WritePropertyName("Messages");
						writer.WriteStartArray();
							writer.WriteStartObject();
								JsonUtilities.AddPropertyNameAndValue(writer, "errorNo", Convert.ToString(StatusCode));
								JsonUtilities.AddPropertyNameAndValue(writer, "message", Message);
							writer.WriteEndObject();
						writer.WriteEndArray();
						
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
		*/
		
		/// <summary>
		/// Wrapper around the ProgressLogging version of the method of the same name.
		/// This calling style is shorter because the missing values in the parameter list
		/// here can be made up from the internal objects which house these values.
		/// </summary>
		/// <param name="StatusCode"></param>
		/// <param name="Comment"></param>
		/// <param name="Message"></param>
		/// <param name="Method"></param>
		/// <param name="EntryType"></param>
		/// <param name="DataAsJson"></param>
		/// <param name="NewId"></param>
		/// <param name="Content"></param>
		/// <param name="ErrorMsg"></param>
		/// <returns></returns>
		public bool BuildContentAndLog(GeneralResponseStatus StatusCode,
                                       string Comment,
                                       string Message,
                                       string Method,
                                       JsonContentEntryType EntryType,
                                       string DataAsJson,
                                       out int NewId,
                                       out string Content,
                                       out string ErrorMsg)
		{
			return 
				BuildContentAndLog(
					true, StatusCode, Comment, Message, Method,					
					EntryType, DataAsJson, out NewId, out Content, out ErrorMsg);
		}
		public bool BuildContentAndLog(bool LogEntry,
		                               GeneralResponseStatus StatusCode,
                                       string Comment,
                                       string Message,
                                       string Method,
                                       JsonContentEntryType EntryType,
                                       string DataAsJson,
                                       out int NewId,
                                       out string Content,
                                       out string ErrorMsg)
		{
			string Plugin = "Unknown Plugin";
			string Command = "Unknown Command";
			int  CompanyIdValue = 0;
			int ClientIdValue = 0;
			
			if (this.CommonItem != null)
			{
				Plugin = this.CommonItem.Plugin;
				Command = this.CommonItem.Command;
				CompanyIdValue = this.CommonItem.CompanyIdValue;
				ClientIdValue = this.CommonItem.ClientIdValue;
			}
			return
				/*
				ProgressLogging.BuildContentAndLog(
					true, this.MasterId, StatusCode, Comment, Message, 
					this.CommonItem.Plugin, this.CommonItem.Command, Method,
					EntryType, DataAsJson, this.CommonItem.CompanyIdValue, this.CommonItem.ClientIdValue,
					out NewId, out Content, out ErrorMsg);
				*/
				ProgressLogging.BuildContentAndLog(
					true, this.MasterId, StatusCode, Comment, Message, 
					Plugin, Command, Method,
					EntryType, DataAsJson, CompanyIdValue, ClientIdValue,
					out NewId, out Content, out ErrorMsg);
		}
		
		public string LogOnException(string Method, //: String;
		                             Exception Ex,
		                             string DataAsJson)	//DataAsXml: String = ''): String; overload;
		{
			string Msg = Ex.Message + Environment.NewLine + Ex.InnerException + Environment.NewLine + Ex.StackTrace + Environment.NewLine + Ex.Source;
			string result = LogOnException(Method, Msg, DataAsJson);
			
			return result;
		}
		
		public string LogOnException(string Method, 	//: String;
		                             string Msg, 		//: String;
		                             string DataAsJson)	//: String = ''): String; overload;
		{
			
			string result = "";
			try				
			{	
				this.LogToErrorEvent("LogOnException: " + Method + Environment.NewLine + Msg);
				string ErrorMsg = "";
				int TempId = 0;
				
				GeneralResponseStatus StatusCode = GeneralResponseStatus.LogOnException;
				bool res =
					BuildContentAndLog(
						StatusCode, 
						"Exception in Method: " + Method, 
						Msg, Method, 
						JsonContentEntryType.Error, 
						DataAsJson, 
						out TempId, out result, out ErrorMsg);
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
			string JsonMsg = JsonUtilities.EscapeJSONString_JsonConvert(Message);
			
			string DataAsJson = "";
			
			DataAsJson += JsonUtilities.AddPropertyNameAndValue("Message", JsonMsg, false);
			
			return LogOnInsideMethod(Method, DataAsJson);
		}
		
		public string LogOnInsideMethod(string Method,		//: String;
		                                string DataAsJson)	//: String = ''): String;
		{
			//this.LogToErrorEvent("LogOnInsideMethod: " + Method, DataAsJson, "");
			//WindowsEventLoggingUtilities.WriteToErrorEvent("LogOnInsideMethod: " + Method, Method, 0);
			this.LogToInfoEvent("LogOnInsideMethod: " + Method, DataAsJson);
			string Result = "";
			string ErrorMsg = "";
			int TempId = 0;
			bool res = false;
			string Str = "Inside Method: " + Method;
			
			try
			{
				res = 
					BuildContentAndLog(
						this.DebugMode,
						GeneralResponseStatus.LogOnInsideMethod,
						"", Str, Method, 
						JsonContentEntryType.Logging_Entry, DataAsJson,
						out TempId, out Result, out ErrorMsg);
			}
			catch (Exception Ex)
			{
				Str = "Exception in LogOnInsideMethod";
				WindowsEventLoggingUtilities.WriteToErrorEvent("ActionBase", Str, 0);
				this.LogToErrorEvent(Str);
				Result = LogOnException(Method, Ex, "");
			}
			
			return Result;		
		}
		
		public string LogOnLeavingMethod(string Method)
		{
			return LogOnLeavingMethod(Method, "");
		}
		
		public string LogOnLeavingMethod(string Method,
		                                 string DataAsJson)
		{
			this.LogToInfoEvent("LogOnLeavingMethod: " + Method, DataAsJson);
			string Result = "";
			string ErrorMsg = "";
			int TempId = 0;
			bool res = false;
			string Str = "Leaving Method: " + Method;
			
			try
			{
				res =
					BuildContentAndLog(
						this.DebugMode,
						GeneralResponseStatus.LogOnLeavingMethod, 
						"", Str, Method,
						JsonContentEntryType.Logging_Entry, DataAsJson,
						out TempId, out Result, out ErrorMsg);
			}
			catch (Exception Ex)
			{
				Str = "Exception in LogOnLeavingMethod";				
				WindowsEventLoggingUtilities.WriteToErrorEvent("ActionBase", Str, 0);
				this.LogToErrorEvent(Str);
				Result = LogOnException(Method, Ex, "");
			}
			
			return Result;
		}
		
		public string LogOnErrorValidatingData(string Method,
		                                       string Msg,
		                                       string DataAsJson)
		{
			this.LogToErrorEvent("LogOnErrorValidatingData: " + Method, Msg, DataAsJson);
			
			string Result = "";
			string ErrorMsg = "";
			int TempId = 0;
			bool res = false;
			string Str = "Inside Method: " + Method;
			
			try
			{
				res =
					BuildContentAndLog(
						true, 
						GeneralResponseStatus.LogOnErrorValidatingData,
						Str, Msg, Method, JsonContentEntryType.Error, DataAsJson,						
						out TempId, out Result, out ErrorMsg);
			}
			catch (Exception Ex)
			{
				Result = LogOnException(Method, Ex, "");
			}

			return Result;
		}
		
		/// <summary>
		/// Logs an entry in the Event Viewer and also
		/// to the database for the error provided
		/// </summary>
		/// <param name="Method"></param>
		/// <param name="Msg"></param>
		/// <param name="QueryText">
		/// This should not be a formatted JSON strng.
		/// This will be converted to a JSON string inside the method
		/// </param>
		/// <returns></returns>
		public string LogOnErrorSelectingData(string Method,
		                                      string Msg,
		                                      string QueryText)
		{
			this.LogToInfoEvent("LogOnErrorSelectingData: " + Method, QueryText);
			string Result = "";
			string ErrorMsg = "";
			int TempId = 0;
			bool res = false;
			string Str = "Inside Method: " + Method;
			string DataAsJson =
				JsonUtilities.AddPropertyNameAndValue(
					"QueryText", JsonUtilities.EscapeJSONString_JsonConvert(QueryText), false); // make this a JSON valid string
			
			try
			{
				res =
					BuildContentAndLog(
						true, 
						GeneralResponseStatus.LogOnErrorSelectingData, 
						Str, Msg, Method, 
						JsonContentEntryType.Error, DataAsJson,
						out TempId, out Result, out ErrorMsg);
			}
			catch (Exception Ex)
			{
				Result = LogOnException(Method, Ex, "");
			}

			return Result;
		}
		
		public string LogResponse(string Method,
		                          string DataAsJson)
		{
			this.LogToInfoEvent("LogResponse: " + Method, DataAsJson);
			string Result = "";
			string ErrorMsg = "";
			int TempId = 0;
			bool res = false;
			
			try
			{
				res =
					BuildContentAndLog(
						true, 
						GeneralResponseStatus.LogOnSuccess, 
						"SUCCESS", "SUCCESS", Method,
						JsonContentEntryType.Response, DataAsJson,
						out TempId, out Result, out ErrorMsg);
				
				if (!string.IsNullOrEmpty(ErrorMsg)) Result = ErrorMsg;
			}
			catch (Exception Ex)
			{
				Result = LogOnException(Method, Ex, "");
			}

			return Result;
		}
		
		public string LoggingEntry(string Method,
		                           string Msg,
		                           string DataAsJson)
		{
			this.LogToInfoEvent("LoggingEntry: " + Method, DataAsJson);
			string Result = "";
			string ErrorMsg = "";
			int TempId = 0;
			bool res = false;
			
			try
			{
				res =
					BuildContentAndLog(
						true, 
						GeneralResponseStatus.LoggingEntry, 
						"", Msg, Method, 
						JsonContentEntryType.Logging_Entry, DataAsJson, 
						out TempId, out Result, out ErrorMsg);
				//if (!string.IsNullOrEmpty(ErrorMsg)) Result = ErrorMsg;
			}
			catch (Exception Ex)
			{
				Result = LogOnException(Method, Ex, "");
			}

			return Result;
		}
		
		public string LogOnAuthorisationError(string Method,
		                                      string Msg,
		                                      string DataAsJson)
		{
			
			string Result = "";
			try				
			{	
				this.LogToErrorEvent("LogOnAuthorisationError: " + Method + Environment.NewLine + Msg);
				string ErrorMsg = "";
				int TempId = 0;
				
				bool res =
					BuildContentAndLog(
						true, 
						GeneralResponseStatus.LoggingOnErrorAuthorising, 
						"Authorisation Error: " + Method, Msg, Method, 
						JsonContentEntryType.Error, DataAsJson,
						out TempId, out Result, out ErrorMsg);
			}
			catch
			{
				// trap any error that may occur during logging
			}
			
			return Result;
		}
		
		public string LogOnProcessingError(string Method,
		                                   string Msg,
		                                   string DataAsJson)
		{
			
			string Result = "";
			try				
			{	
				this.LogToErrorEvent("LogOnProcessingError: " + Method + Environment.NewLine + Msg);
				string ErrorMsg = "";
				int TempId = 0;
				
				bool res =
					BuildContentAndLog(
						true, 
						GeneralResponseStatus.LoggingOnErrorProcessing, 
						"Processing Error: " + Method, Msg, Method, 
						JsonContentEntryType.Error, DataAsJson,
						out TempId, out Result, out ErrorMsg);
			}
			catch
			{
				// trap any error that may occur during logging
			}
			
			return Result;
		}
		//private const string FTableName = "RPOAPIModuleLog";
		//private const string FFieldPrefix = "RAPML_";
		
		private static string BuildSqliteConnectionString(out string ErrorMsg)
		{
			return ProgressLogging.BuildSqliteConnectionString(out ErrorMsg);
			/*
				
			ErrorMsg = "";
			string FileName = SqliteLogging.FileNameForTodaysLog();
			string Result = "Data Source={0}; Version=3;";
			Result = string.Format(Result, FileName);
			return Result;
			*/
		}
    	
    	private static string BuildConnectionString(out string ErrorMsg)
    	{
    		string lResult = "";
    		ErrorMsg = "";
    		
    		lResult = CommonDBMethods.Common.ConnectionString();
    		
/*    		
    		string lDBServer = "";
    		string lDBName = "";
    		
    		RPOAPISettingsItem rpoAPISettings =
				RPOAPISettingsFile.GetRPOAPISettings(RPOAPIAppType.Service, out ErrorMsg);
    		
    		if (String.IsNullOrEmpty(ErrorMsg))
    		{
    			lDBServer = rpoAPISettings.DBServer;
    			lDBName = rpoAPISettings.DBName;
    			if (RPORegUtils.GetDBServerAndNameFromRegistry(rpoAPISettings.DBProfile, ref lDBServer, ref lDBName, out ErrorMsg))
    			{    			
    		    	lResult = CommonDBMethods.BuildConnectionString(lDBServer, lDBName, out ErrorMsg);    		    	    		    	
    			}
    		}
*/    		
    		
    		return lResult;
    	}
    	
    	/*
    	private static bool GetDBServerAndNameFromRegistry(short aDBProfile, ref string aDBServer, ref string aDBName, out string aErrorMsg)
		{
    		bool lResult = true;
    		
    		aErrorMsg = "";
    		    		
			if (aDBProfile >= 0)
			{
				CommonUtilities.RPORegistryDBProfileItem profileItem = RPORegUtils.GetRPODBProfileAtIndex(aDBProfile);
				if (profileItem == null)
				{
					lResult = false;
					aErrorMsg = "No profile setup with index " + aDBProfile.ToString();
					aDBServer = "Not setup for specified index";
					aDBName = "Not setup for specified index";
				}
				else
				{
					aDBServer = profileItem.DBProfileComputerName;
					aDBName = profileItem.DBProfileDBName;
				}
			}
			
			return lResult;
		}
		*/

    	private static string BuildErrorAddingTable(string TableName,
		                                            string FieldName)
		{
			string result = "Error adding the field: {0} to the table: {1}";
			result = string.Format(result, FieldName, TableName);
			return result;
		}
		
		/*
    	private static string BuildLoggingTable(string ConnectionString)
		{
			string FieldName = "";
			string QueryText = "";
			string Str = "";
			StringBuilder StrList = new StringBuilder();					
			
			if (!FBUtils.TableExists(FTableName, ConnectionString))
			{
      			QueryText = "create Generator {0}_GEN";
      			QueryText = string.Format(QueryText, FTableName);
      			Str = FBUtils.DBExecute(ConnectionString, QueryText);
      			
      			if (string.IsNullOrEmpty(Str))
      			{
      				// if we can't create the table we need to exit the method
	      			FieldName = FFieldPrefix + "ID";
	      			QueryText = "CREATE TABLE {0} ({1} Integer NOT NULL)";
	      			QueryText = string.Format(QueryText, FTableName, FieldName);
	      			Str = FBUtils.DBExecute(ConnectionString, QueryText);
      			}
      			
      			if (string.IsNullOrEmpty(Str))
      			{
      				// if we can't create the primary key we need to exit the method
      				QueryText = "ALTER TABLE {0} add constraint PK_{1} primary key ({2})";
      				QueryText = string.Format(QueryText, FTableName, FTableName, FieldName);
      				Str = FBUtils.DBExecute(ConnectionString, QueryText);
      			}
      			
      			if (!string.IsNullOrEmpty(Str)) StrList.AppendLine(Str);
			}
			
			if (StrList.Length == 0)
			{
	  			// start processing the rest of the fields.
	  			FieldName = FFieldPrefix + "ModuleId";
	  			Str = FBUtils.AddMissingField(ConnectionString, FTableName, FieldName, DBFieldType.ftInteger);
	  			if (!string.IsNullOrEmpty(Str)) StrList.AppendLine(BuildErrorAddingTable(FTableName, FieldName));
  			
	  			FieldName = FFieldPrefix + "Method";
	  			Str = FBUtils.AddMissingField(ConnectionString, FTableName, FieldName, DBFieldType.ftString, 255);
	  			if (!string.IsNullOrEmpty(Str)) StrList.AppendLine(BuildErrorAddingTable(FTableName, FieldName));
	  			
	  			FieldName = FFieldPrefix + "MasterId";
	  			Str = FBUtils.AddMissingField(ConnectionString, FTableName, FieldName, DBFieldType.ftInteger);
	  			if (!string.IsNullOrEmpty(Str)) StrList.AppendLine(BuildErrorAddingTable(FTableName, FieldName));
	  			
	  			FieldName = FFieldPrefix + "DateCreated";
	  			Str = FBUtils.AddMissingField(ConnectionString, FTableName, FieldName, DBFieldType.ftDateTime);
	  			if (!string.IsNullOrEmpty(Str)) StrList.AppendLine(BuildErrorAddingTable(FTableName, FieldName));
	  			
	  			FieldName = FFieldPrefix + "AddedByClientId";
	  			Str = FBUtils.AddMissingField(ConnectionString, FTableName, FieldName, DBFieldType.ftInteger);
	  			if (!string.IsNullOrEmpty(Str)) StrList.AppendLine(BuildErrorAddingTable(FTableName, FieldName));
	  			
	  			FieldName = FFieldPrefix + "AddedBy";
	  			Str = FBUtils.AddMissingField(ConnectionString, FTableName, FieldName, DBFieldType.ftString, 60);
	  			if (!string.IsNullOrEmpty(Str)) StrList.AppendLine(BuildErrorAddingTable(FTableName, FieldName));
	  			
	  			FieldName = "Content";
	  			//Str = FBUtils.AddMissingBlobField(ConnectionString, TableName, FieldName, DBFieldType. NOTE, DataSet);
	  			if (!string.IsNullOrEmpty(Str)) StrList.AppendLine(BuildErrorAddingTable(FTableName, FieldName));
	  			
	  			FieldName = FFieldPrefix + "ContentIsFile";
	  			Str = FBUtils.AddMissingField(ConnectionString, FTableName, FieldName, DBFieldType.ftString, 1);
	  			if (!string.IsNullOrEmpty(Str)) StrList.AppendLine(BuildErrorAddingTable(FTableName, FieldName));
	  			
	  			FieldName = FFieldPrefix + "Command";
	  			Str = FBUtils.AddMissingField(ConnectionString, FTableName, FieldName, DBFieldType.ftString, 255);
	  			if (!string.IsNullOrEmpty(Str)) StrList.AppendLine(BuildErrorAddingTable(FTableName, FieldName));
	  			
	  			FieldName = FFieldPrefix + "EntryType";
	  			Str = FBUtils.AddMissingField(ConnectionString, FTableName, FieldName, DBFieldType.ftString, 255);
	  			if (!string.IsNullOrEmpty(Str)) StrList.AppendLine(BuildErrorAddingTable(FTableName, FieldName));
			}

  			return StrList.ToString();
		}
		*/
		
		/*
    	public static string GetEntryTypeString(JsonContentEntryType EntryType)
    	{
    		string Result = "";
    		switch (EntryType)
    		{
    			case JsonContentEntryType.Request: 			Result = "Request"; break;
    			case JsonContentEntryType.Response:         	Result = "Response"; break;
    			case JsonContentEntryType.Logging_Entry:    	Result = "Logging_Entry"; break;
    			case JsonContentEntryType.Validation_Error:	Result = "Validation Error"; break;
    			case JsonContentEntryType.Warning:          	Result = "Warning"; break;
    			case JsonContentEntryType.Error:            	Result = "Error"; break;
    			case JsonContentEntryType.Debugging:        	Result = "Debugging"; break;
    			case JsonContentEntryType.QueryText:        	Result = "QueryText"; break;
    		}
    		return Result;
    	}
    	*/
    	
    	public static string CreateDBComponents(string ConnectionString)
    	{
    		//return BuildLoggingTable(ConnectionString);
    		return "Method not implemented";
    	}
    	
    	/*
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
    	                             int ModuleId,
		                             string Method,
		                             int MasterId,
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
    	                             int ModuleId,
		                             string Method,
		                             int MasterId,
		                             int AddedByClientId,
		                             string AddedBy,
		                             string Content,
		                             string ContentIsFile,
		                             string EntryType,
		                             string DBConnectionString,
		                             out int NewId,
		                             out string ErrorMsg)
		{
			return 
				AddRecord(
					LogEntry, ModuleId, Method, MasterId, AddedByClientId, AddedBy, 
					Content, ContentIsFile, EntryType, "", DBConnectionString, out NewId, out ErrorMsg);
		}
    	
	    public static bool AddRecord(bool LogEntry,
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
		                             out int NewId,
		                             out string ErrorMsg)
		{
    		string EntryTypeStr = GetEntryTypeString(EntryType);
			return 
				AddRecord(LogEntry, ModuleId, Method, MasterId, AddedByClientId, AddedBy,
				          Content, ContentIsFile, EntryTypeStr, Command, DBConnectionString, out NewId, out ErrorMsg);
		}
    	
	    public static bool AddRecord(bool LogEntry,
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
		                             out int NewId,
		                             out string ErrorMsg)
		{
			ErrorMsg = "";
			bool result = false;

			if (LogEntry == false)
			{
				// If not logging then quit before doing anything else
				NewId = -1;				
				return result;
			}
			
			if (String.IsNullOrEmpty(DBConnectionString))
				NewId = GetNextActivityId();
			else			
				NewId = GetNextActivityId(DBConnectionString);
			if (NewId <= 0)
			{
				ErrorMsg = "Error returning the next Logging ID";
			}
			
			if (string.IsNullOrEmpty(ErrorMsg))
			{
				result = 
					AddRecordWithId(
						LogEntry, NewId, ModuleId, Method, MasterId, AddedByClientId, AddedBy,
						Content, ContentIsFile, EntryType, Command, DBConnectionString, out ErrorMsg);
			}
			
			return result;
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
	}
}
