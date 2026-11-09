/*
 * Created by SharpDevelop.
 * User: jbennett
 * Date: 10/10/2018
 * Time: 11:44 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Reflection;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EAOfficeAPI
{
	/// <summary>
	/// Description of APISettingsConfig.
	/// </summary>
	public enum APIAppType {Unknown, Winforms, Service};
	
	/// <summary>
	/// </summary>
	public class APISettingsItem
	{
		public APISettingsItem()
		{
			this.HostTimerInterval = 0d;
			this.HostInitLoggingHour = 0;
			this.APISessionName = "";
			this.APIServiceHostName = "";
			this.ServiceURL = "";
			this.HttpPort = "";		
			this.TCPPort = "";
			this.DBServer = "";
			this.DBName = "";
            this.DBUserName = "";
            this.DBPassword = "";
			this.DebugLogging = false;
			this.EventViewerLogging = false;
			this.MaintenanceStart = DateTime.MinValue;
			this.MaintenanceEnd = DateTime.MinValue;
		}
		
		//double hostTimerInterval = 0d;
		//int hostInitLoggingHour = 0;
		//string aPISessionName = "";
		//string aPIServiceHostName;
		//string serviceURL = "";
		//string httpPort = "";
		//string tCPPort = "";
		/*
		short dBProfile = -1;
		
		public short DBProfile {
			get { return dBProfile; }
			set { dBProfile = value; }
		}
		*/
		//string dBServer = "";
		//string dBName = "";
		//bool debugLogging = false;
		//bool eventViewerLogging = false;
		//DateTime maintenanceStart;
		//DateTime maintenanceEnd;
		
		public double HostTimerInterval  { get; set; }		
		public int HostInitLoggingHour { get; set; }
		public string APISessionName { get; set; }
		public string APIServiceHostName { get; set; }
		public string ServiceURL { get; set; }
		public string HttpPort { get; set; }
		public string TCPPort { get; set; }
		public string DBServer { get; set; }
		public string DBName { get; set; }
        public string DBUserName { get; set; }
        public string DBPassword { get; set; }
        public bool DebugLogging { get; set; }
		public bool EventViewerLogging { get; set; }
		public DateTime MaintenanceStart { get; set; }
		public DateTime MaintenanceEnd { get; set; }
	}
	
	/// <summary>
	/// Description of APISettingsFile class
	/// </summary>
	public static class APISettingsFile
	{
		private const string FSettingsFile = "APISettings.config";
		
		private const string FDefaultTimerInterval_ms = "30000";
		private const string FDefaultInitLoggingHour = "22";
		
		
		public static APISettingsItem GetAPISettings(APIAppType AppType, 
		                                             out string ErrorMessage)
		{
			return GetAPISettings(AppType, "", out ErrorMessage);
		}
		
		public static APISettingsItem GetAPISettings(APIAppType AppType, 
		                                             string AppDir, 
		                                             out string ErrorMessage)
		{
			ErrorMessage = "";
			APISettingsItem apiSettingsItem = new APISettingsItem();
			APISettingsItem result;
			try
			{
				string text = "";				
				
				switch (AppType)
				{
				case APIAppType.Winforms:
					text = Environment.CurrentDirectory;
					break;
				case APIAppType.Service:
					text = new Uri(Assembly.GetExecutingAssembly().GetName().CodeBase).LocalPath;
					text = Path.GetDirectoryName(text);
					break;
				default:
					text = Environment.CurrentDirectory;
					break;
				}
				
				if (!String.IsNullOrEmpty(AppDir))
				{
					text = AppDir;
				}
				
				if (string.IsNullOrEmpty(text))
				{
					ErrorMessage = "Error  in GetAPISettings - Unable to determine current directory";
					result = null;
					return result;
				}
				string path = Path.Combine(text, FSettingsFile);
				string value = "";
				using (StreamReader streamReader = new StreamReader(path))
				{
					value = streamReader.ReadToEnd();
				}

                JObject JSONSource = JObject.Parse(value);
                JObject jObject = JsonConvert.DeserializeObject<JObject>(value);
				if (jObject == null)
				{
					ErrorMessage = "Error deserializing the APISettings file.";
				}
				else
				{
					string lErrorMessage = "";
					bool flag = GetHostSettings(jObject, apiSettingsItem, out lErrorMessage);
					if (!flag)
					{
						ErrorMessage += Environment.NewLine + Environment.NewLine + lErrorMessage;
					}
					flag = GetServiceSettings(jObject, apiSettingsItem, out lErrorMessage);
					if (!flag)
					{
						ErrorMessage += Environment.NewLine + Environment.NewLine + lErrorMessage;
					}
					flag = GetDatabaseSettings(jObject, apiSettingsItem, out lErrorMessage);
					if (!flag)
					{
						ErrorMessage += Environment.NewLine + Environment.NewLine + lErrorMessage;
					}

                    if (!string.IsNullOrEmpty(ErrorMessage))
                    {
                        //WriteToOutput(ErrorMessage);
                    }
                    else
                    {
                        apiSettingsItem.DBUserName =
                            CommonUtilities.JsonUtilities.GetJsonPathValueStr(JSONSource, "$.Database.dbUserName", out ErrorMessage);
                        //WriteToOutput("ErrorMsg: " + ErrorMessage);
                        //WriteToOutput("DBUserName: " + apiSettingsItem.DBUserName);
                        if (string.IsNullOrEmpty(apiSettingsItem.DBUserName))
                        {
                            apiSettingsItem.DBUserName = "EAOFFICEUSER";
                        }
                    }
                    flag = GetLoggingSettings(jObject, apiSettingsItem, out lErrorMessage);
					if (!flag)
					{
						ErrorMessage += Environment.NewLine + Environment.NewLine + lErrorMessage;
					}				
					flag = GetMaintenanceSettings(jObject, apiSettingsItem, out lErrorMessage);
					if (!flag)
					{
						ErrorMessage += Environment.NewLine + Environment.NewLine + lErrorMessage;
					}
				}
			}
			catch (Exception ex)
			{
				ErrorMessage = "Error processing APISettings File" + Environment.NewLine + Environment.NewLine + ex.ToString();
			}
			result = apiSettingsItem;
			return result;
		}
		
		private static string GetStringValueForJSONElement(JObject Source, 
		                                                   string ElementName)
		{
			string Result = "";
			
			if (Source[ElementName] != null)
			{
				Result = Source[ElementName].ToString();
			}
			
			return Result;
		}
		
		private static void SetStringValueForJSONElement(JObject Source, 
		                                                 string ElementName, 
		                                                 string Value)
		{
			try
			{
				if (Source[ElementName] == null)
				{
					Source.Add(ElementName, JToken.Parse("\"" + Value + "\""));
				}
				else
				{
					string lValueTemp = Source[ElementName].ToString();
					
					if (!String.Equals(Value, lValueTemp))
					{
						Source[ElementName] = Value;
					}
				}

/*				
				JObject jObject = aObj[aElementName] as JObject;
//				JProperty jProperty = aObj[aElementName] as JProperty;				
				if (jProperty == null)
				{
					aObj.Add(aElementName, JToken.Parse("\"" + aValue + "\""));
				}
				else
				{
					string lValue = jProperty.Value.ToString();
					
					if (!String.Equals(aValue, lValue))
					{
						//var lObj = aObj;
						
						//aObj[aElementName] = aValue;
						
						//jObject.Value = aValue;
						jProperty.Value = aValue;
					}
				}
*/

			}
			catch(Exception e)
			{
				throw new Exception("Error modifying JSON string value in Settings", e);
			}
				
			
			//return lResult;
		}		
		
/*		
		private static bool GetHostSettings_Obsolete(JObject aObj, APISettingsItem aSettings)
		{
//			string lErrorMessage = "";
			
			bool lResult = false;

			string lTimerIntervalAsStr = "";
			string lLoggingHourStr = "";			
			
			try 
			{			
				JObject HostSettingsObj = aObj["HostSettings"] as JObject;
				if (HostSettingsObj == null)
				{
					// Dont warn if no host settings - use defaults instead
				}
				else
				{
					lTimerIntervalAsStr = GetStringValueForJSONElement(HostSettingsObj, "timerInterval_ms");
					
					lLoggingHourStr = GetStringValueForJSONElement(HostSettingsObj, "initLoggingHour");
				}
				
				if (String.IsNullOrEmpty(lTimerIntervalAsStr))
				{
					lTimerIntervalAsStr = gDefaultTimerInterval_ms;
					
				}
				aSettings.HostTimerInterval = Convert.ToDouble(lTimerIntervalAsStr);
				
				if (String.IsNullOrEmpty(lLoggingHourStr))
				{
					lLoggingHourStr = gDefaultInitLoggingHour;
				}
				
				aSettings.HostInitLoggingHour = Convert.ToInt32(lLoggingHourStr);
				
				lResult = true;
			}
			catch (Exception ex)
			{
				lResult = false;
				//aErrorMessage = "Error getting Host settings from APISettings file -" + Environment.NewLine + ex.ToString();
				throw new Exception("Error getting Host settings from APISettings file.", ex);				
			}	
			
			return lResult;
		}
*/		
		
		private static bool GetHostSettings(JObject Source,
                                            APISettingsItem Item, 
                                            out string ErrorMessage)
		{
			ErrorMessage = "";
			bool result = false;
			string value = "";
			string value2 = "";
			try
			{
				JObject jObject = Source["HostSettings"] as JObject;
				if (jObject != null)
				{
					value = APISettingsFile.GetStringValueForJSONElement(jObject, "timerInterval_ms");
					value2 = APISettingsFile.GetStringValueForJSONElement(jObject, "initLoggingHour");
				}
				if (string.IsNullOrEmpty(value))
				{
					value = "30000";
				}
				Item.HostTimerInterval = Convert.ToDouble(value);
				if (string.IsNullOrEmpty(value2))
				{
					value2 = "22";
				}
				Item.HostInitLoggingHour = Convert.ToInt32(value2);
				result = true;
			}
			catch (Exception ex)
			{
				result = false;
				ErrorMessage = "Error getting Host settings from APISettings file -" + Environment.NewLine + ex.ToString();
			}
			return result;
		}		
		
/*		
		private static bool GetServiceSettings_Obsolete(JObject aObj, APISettingsItem aSettings)
		{
			string lErrorMessage = "";
			
			bool lResult = false;
			
			JObject ServiceSettingsObj = aObj["Service"] as JObject;
			if (ServiceSettingsObj == null)
			{				
				lErrorMessage = "No Service settings in the APISettings file.";
			}
			else
			{				
				string lAPISessionNameStr = GetStringValueForJSONElement(ServiceSettingsObj, "aPISessionName");
				lResult = (!String.IsNullOrEmpty(lAPISessionNameStr));
				if (lResult)
				{
					aSettings.APISessionName = lAPISessionNameStr;
				}	
				else
					lErrorMessage = "No Session Name specified in the APISettings file.";
					
				if (lResult)
				{
					string lServiceURL = GetStringValueForJSONElement(ServiceSettingsObj, "serviceUrl");
					lResult = (!String.IsNullOrEmpty(lServiceURL));
					if (lResult)
					{
						aSettings.ServiceURL = lServiceURL;
					}	
					else
						lErrorMessage = "No Service URL specified in the APISettings file.";
				}
				
				if (lResult)
				{
					string lHttpPort = GetStringValueForJSONElement(ServiceSettingsObj, "httpPort");
					lResult = (!String.IsNullOrEmpty(lHttpPort));
					if (lResult)
					{
						aSettings.HttpPort = lHttpPort;
					}	
					else
						lErrorMessage = "No HTTP Port specified in the APISettings file.";
				}
				
				if (lResult)
				{
					string lTCPPort = GetStringValueForJSONElement(ServiceSettingsObj, "tcpPort");
					lResult = (!String.IsNullOrEmpty(lTCPPort));
					if (lResult)
					{
						aSettings.TCPPort = lTCPPort;
					}	
					else
						lErrorMessage = "No TCP Port specified in the APISettings file.";
				}
			}
					

			if (String.IsNullOrEmpty(lErrorMessage))
				return lResult;
			else
				throw new Exception(lErrorMessage);
		}
*/		
		
		private static bool GetServiceSettings(JObject Source, 
                                               APISettingsItem Settings, 
                                               out string ErrorMessage)
		{
			ErrorMessage = "";
			bool flag = false;
			JObject jObject = Source["Service"] as JObject;
			if (jObject == null)
			{
				ErrorMessage = "No Service settings in the APISettings file.";
			}
			else
			{
				string stringValueForJSONElement = APISettingsFile.GetStringValueForJSONElement(jObject, "aPISessionName");
				flag = !string.IsNullOrEmpty(stringValueForJSONElement);
				if (flag)
				{
					Settings.APISessionName = stringValueForJSONElement;
				}
				else
				{
				}
				if (flag)
				{
					string stringValueForJSONElement2 = APISettingsFile.GetStringValueForJSONElement(jObject, "serviceUrl");
					flag = !string.IsNullOrEmpty(stringValueForJSONElement2);
					if (flag)
					{
						Settings.ServiceURL = stringValueForJSONElement2;
					}
					else
					{
						ErrorMessage = "No Service URL specified in the APISettings file.";
					}
				}
				if (flag)
				{
					string stringValueForJSONElement3 = APISettingsFile.GetStringValueForJSONElement(jObject, "httpPort");
					flag = !string.IsNullOrEmpty(stringValueForJSONElement3);
					if (flag)
					{
						Settings.HttpPort = stringValueForJSONElement3;
					}
					else
					{
						ErrorMessage = "No HTTP Port specified in the APISettings file.";
					}
				}
				if (flag)
				{
					string stringValueForJSONElement4 = APISettingsFile.GetStringValueForJSONElement(jObject, "tcpPort");
					flag = !string.IsNullOrEmpty(stringValueForJSONElement4);
					if (flag)
					{
						Settings.TCPPort = stringValueForJSONElement4;
					}
					else
					{
						ErrorMessage = "No TCP Port specified in the APISettings file.";
					}
				}
				if (flag)
				{
					string stringValueForJSONElement5 = APISettingsFile.GetStringValueForJSONElement(jObject, "aPIServiceHostName");
					if (flag)
					{
						Settings.APIServiceHostName = stringValueForJSONElement5;
					}
				}						
			}
			return flag;
		}
		
/*		
		private static bool GetDatabaseSettings_Obsolete(JObject aObj, APISettingsItem aSettings)
		{
			string lErrorMessage = "";
			
			bool lResult = false;
			
			JObject DBSettingsObj = aObj["Database"] as JObject;
			if (DBSettingsObj == null)
			{
				lErrorMessage = "No database settings in the APISettings file.";
			}
			else
			{
				string lDBProfileAsStr = GetStringValueForJSONElement(DBSettingsObj, "profile");
				short lDBProfileTemp = 0;
				lResult = (!String.IsNullOrEmpty(lDBProfileAsStr));
				if (lResult)
				{
					
					if (!short.TryParse(lDBProfileAsStr, out lDBProfileTemp))
					{
					    	lDBProfileTemp = 0;
					}
				
					aSettings.DBProfile = lDBProfileTemp;
				}
				else
					lErrorMessage = "No database profile specified in the APISettings file.";
				
				if (aSettings.DBProfile < 1)
				{
					string lDBServer = GetStringValueForJSONElement(DBSettingsObj, "server");
					lResult = (!String.IsNullOrEmpty(lDBServer));
					if (lResult)
					{
						lErrorMessage = "";
						aSettings.DBServer = lDBServer;
					}
					else
					{
						if (String.IsNullOrEmpty(lErrorMessage))
	  						lErrorMessage = "No database server specified in the APISettings file.";
					}
					
					if (lResult)
					{
						string lDBName = GetStringValueForJSONElement(DBSettingsObj, "databaseName");
						lResult = (!String.IsNullOrEmpty(lDBName));
						if (lResult)
						{
							lErrorMessage = "";
							aSettings.DBName = lDBName;
						}
						else
						{
							if (String.IsNullOrEmpty(lErrorMessage))
								lErrorMessage = "No database name in the APISettings file.";
						}
					}
				}
			}			
			
			if (String.IsNullOrEmpty(lErrorMessage))
				return lResult;
			else
				throw new Exception(lErrorMessage);
		}
*/		
		
		private static bool GetDatabaseSettings(JObject Source,
                                                APISettingsItem Settings,
                                                out string ErrorMessage)
		{
			
			ErrorMessage = "";
			bool Result = false;
			
			JObject DBSettingsObj = Source["Database"] as JObject;
			if (DBSettingsObj == null)
			{
				ErrorMessage = "No database settings in the APISettings file.";
			}
			else
			{
				string DBProfileAsStr = GetStringValueForJSONElement(DBSettingsObj, "profile");
				short DBProfileTemp = -1;
				Result = (!String.IsNullOrEmpty(DBProfileAsStr));
				if (Result)
				{					
					if (!short.TryParse(DBProfileAsStr, out DBProfileTemp))
					{
					    	DBProfileTemp = -1;
					}
				}

				string DBServer = GetStringValueForJSONElement(DBSettingsObj, "server");				
				string DBName = GetStringValueForJSONElement(DBSettingsObj, "databaseName");

                //string DBUserName = GetStringValueForJSONElement(DBSettingsObj, "dbUserName");
                //string DBPassword = GetStringValueForJSONElement(DBSettingsObj, "dbPassword");

                string DBUserName = 
                    CommonUtilities.JsonUtilities.GetJsonPathValueStr(DBSettingsObj, "$.dbUserName", out ErrorMessage);
                string DBPassword =
                    CommonUtilities.JsonUtilities.GetJsonPathValueStr(DBSettingsObj, "$.dbPassword", out ErrorMessage);

                if (string.IsNullOrEmpty(DBUserName))
                {
                    DBUserName = "EAOFFICEUSER";
                }

                if (string.IsNullOrEmpty(DBPassword))
                {
                    DBPassword = "!bcdeaoffice!";
                }

                if (String.IsNullOrEmpty(DBServer))
				{
					ErrorMessage = "No database server specified in the APISettings file.";					
				}
				
				if ((String.IsNullOrEmpty(DBName)) && (String.IsNullOrEmpty(ErrorMessage)))
				{
					ErrorMessage = "No database name in the APISettings file.";
				}
				
				Result = (String.IsNullOrEmpty(ErrorMessage));
				
				Settings.DBServer = DBServer;
				Settings.DBName = DBName;
                Settings.DBUserName = DBUserName;
                Settings.DBPassword = DBPassword;
			}			
			
			return Result;
		}
		
/*		
		private static bool GetLoggingSettings_Obsolete(JObject aObj, APISettingsItem aSettings)
		{
			bool lResult = false;			
			try 
			{
				JObject LoggingSettingsObj = aObj["Logging"] as JObject;
				if (LoggingSettingsObj == null)
				{
					// dont warn if no logging settings
				}
				else
				{
					string lDebugLogging = GetStringValueForJSONElement(LoggingSettingsObj, "debugLogging");
					aSettings.DebugLogging = (String.Compare(lDebugLogging, "true", false) == 0);
				}
				
				lResult = true;
			}
			catch (Exception ex)
			{
				//aErrorMessage = "Error getting logging settings from APISettings file - " + Environment.NewLine + ex.ToString();				
				
				throw new Exception("Error getting logging settings from APISettings file.", ex);
			}			
			
			return lResult;
		}
*/		
		
		private static bool GetLoggingSettings(JObject Source,
                                               APISettingsItem Settings, 
                                               out string ErrorMessage)
		{
			ErrorMessage = "";
			bool result = false;
			try
			{
				JObject jObject = Source["Logging"] as JObject;
				if (jObject != null)
				{
					string stringValueForJSONElement = GetStringValueForJSONElement(jObject, "debugLogging");
					Settings.DebugLogging = (string.Compare(stringValueForJSONElement, "true", false) == 0);
					
					string lEventViewerLogging = GetStringValueForJSONElement(jObject, "eventViewerLogging");
					Settings.EventViewerLogging = (String.Compare(lEventViewerLogging, "true", false) == 0);					
				}
				result = true;
			}
			catch (Exception ex)
			{
				ErrorMessage = "Error getting logging settings from APISettings file - " + Environment.NewLine + ex.ToString();
			}
			return result;
		}
		
		private static bool GetMaintenanceSettings(JObject Source,
                                                   APISettingsItem Settings, 
                                                   out string ErrorMessage)
		{
			ErrorMessage = "";
			bool result = false;
			try
			{
				JObject jObject = Source["Maintenance"] as JObject;
				if (jObject != null)
				{
					string stringValueForJSONElement = GetStringValueForJSONElement(jObject, "startTime");
					Settings.MaintenanceStart = DateTime.ParseExact(stringValueForJSONElement, "HH:mm", System.Globalization.CultureInfo.CurrentCulture);
					
					stringValueForJSONElement = GetStringValueForJSONElement(jObject, "endTime");
					Settings.MaintenanceEnd = DateTime.ParseExact(stringValueForJSONElement, "HH:mm", System.Globalization.CultureInfo.CurrentCulture);
				}
				result = true;
			}
			catch (Exception ex)
			{
				ErrorMessage = "Error getting Maintenance settings from APISettings file - " + Environment.NewLine + ex.ToString();
			}
			return result;
		}				
		
		private static bool SameAPISettings(APISettingsItem LHS, 
                                            APISettingsItem RHS)
		{
			bool lResult = false;
			
			// Check if any changes
			if (
			    (LHS.HostTimerInterval == RHS.HostTimerInterval) &&
			    (LHS.HostInitLoggingHour == RHS.HostInitLoggingHour) &&
				
				(LHS.APISessionName == RHS.APISessionName) &&
				(LHS.APIServiceHostName == RHS.APIServiceHostName) &&
				(LHS.ServiceURL == RHS.ServiceURL) &&			
			    (LHS.HttpPort == RHS.HttpPort) &&
				(LHS.TCPPort == RHS.TCPPort) &&

			    //(LHS.DBProfile == RHS.DBProfile) &&
			    (LHS.DBServer == RHS.DBServer) &&
			    (LHS.DBName == RHS.DBName) &&
                (LHS.DBUserName == RHS.DBUserName) &&
                (LHS.DBPassword == RHS.DBPassword) &&

                (LHS.DebugLogging == RHS.DebugLogging) &&
			    (LHS.EventViewerLogging == RHS.EventViewerLogging) &&
			    
			    (LHS.MaintenanceStart == RHS.MaintenanceStart) &&
			    (LHS.MaintenanceEnd == RHS.MaintenanceEnd)
			    
			   )
			{
				lResult = true;
			}
			
			return lResult;
		}
		
		/// <summary>
		/// Process the update of the API settings and save them back to the JSON file (APISettings.config).
		/// Only update the file if new settings are different to the original
		/// </summary>
		/// <param name="NewSettings"></param>
		/// <param name="ErrorMessage"></param>
		/// <returns></returns>
		public static bool UpdateAPISettings(APIAppType AppType,
		                                     APISettingsItem NewSettings, 
		                                     out string ErrorMessage)
		{
			return UpdateAPISettings(AppType, "", NewSettings, out ErrorMessage);
		}
		
		public static bool UpdateAPISettings(APIAppType AppType, 
		                                     string AppDir, 
		                                     APISettingsItem NewSettings, 
		                                     out string ErrorMessage)
		{
			bool Result = false;
				
			APISettingsItem CurrentSettings = GetAPISettings(AppType, AppDir, out ErrorMessage);
			
			ErrorMessage = ""; //Ignore any error messages when attempting to update the settings
			
			if (!SameAPISettings(NewSettings, CurrentSettings))
			{
				Result = SaveAPISettings(AppType, AppDir, NewSettings, CurrentSettings, out ErrorMessage);				
			}
			else
			{
				Result = true;
			}
			
			return Result;
		}
		
		public static string GetAPISettingsFileName(APIAppType AppType, 
		                                            out string ErrorMessage)
		{
			return GetAPISettingsFileName(AppType, "", out ErrorMessage);
		}
		
		public static string GetAPISettingsFileName(APIAppType AppType, 
		                                            string AppDir, 
		                                            out string ErrorMessage)
		{
			string Result = "";
			
			ErrorMessage = "";
			
			string text = "";
			switch (AppType)
			{
				case APIAppType.Winforms:
					text = Environment.CurrentDirectory;
					break;
				case APIAppType.Service:
					text = new Uri(Assembly.GetExecutingAssembly().GetName().CodeBase).LocalPath;
					text = Path.GetDirectoryName(text);
					break;
			}
			
			if (!String.IsNullOrEmpty(AppDir))
			{
				text = AppDir;
			}
			
			if (string.IsNullOrEmpty(text))
			{
				ErrorMessage = "Error  in GetAPISettings - Unable to determine current directory";
				return Result;
			}
			
			Result = Path.Combine(text, FSettingsFile);

			return Result;
		}		
		
		/// <summary>
		/// Read the existing file into a JSON object and replace any values which have changed.
		/// Continue by converting this object back to a string and writing back to the original file.
		/// </summary>
		/// <param name="aAppType"></param>
		/// <param name="aNewSettings"></param>
		/// <param name="aErrorMessage"></param>
		/// <returns></returns>
		/// 
		private static bool SaveAPISettings(APIAppType AppType, 
		                                    APISettingsItem NewSettings, 
		                                    APISettingsItem OldSettings, 
		                                    out string ErrorMessage)
		{
			return SaveAPISettings(AppType, "", NewSettings, OldSettings, out ErrorMessage);
		}
		
		private static bool SaveAPISettings(APIAppType AppType, 
		                                    string AppDir, 
		                                    APISettingsItem NewSettings, 
		                                    APISettingsItem OldSettings, 
		                                    out string aErrorMessage)
		{
			bool Result = false;			
			aErrorMessage = "";
			try
			{				
				string FileName = GetAPISettingsFileName(AppType, AppDir, out aErrorMessage);
				
				if (!String.IsNullOrEmpty(aErrorMessage))
				{
					return Result;
				}					
	
				string value = "";
				using (StreamReader streamReader = new StreamReader(FileName))
				{
					value = streamReader.ReadToEnd();
				}
				JObject jObject = JsonConvert.DeserializeObject<JObject>(value);
				if (jObject == null)
				{
					aErrorMessage = "Error deserializing the APISettings file.";
				}
				else
				{
					bool flag = APISettingsFile.UpdateHostSettings(jObject, NewSettings, OldSettings, out aErrorMessage);
					if (flag)
					{
						flag = APISettingsFile.UpdateServiceSettings(jObject, NewSettings, OldSettings, out aErrorMessage);
					}
					if (flag)
					{
						flag = APISettingsFile.UpdateDatabaseSettings(jObject, NewSettings, OldSettings, out aErrorMessage);
					}
					if (flag)
					{
						flag = APISettingsFile.UpdateLoggingSettings(jObject, NewSettings, OldSettings, out aErrorMessage);
					}
					if (flag)
					{
						flag = APISettingsFile.UpdateMaintenanceSettings(jObject, NewSettings, OldSettings, out aErrorMessage);
					}
					
					
					if ((flag == false) || (String.IsNullOrEmpty(aErrorMessage)))
					{
						string lJson = JsonConvert.SerializeObject(jObject);
						
						lJson = jObject.ToString();
						
						Result	= SaveAPISettingsJsonString(AppType, AppDir, lJson, OldSettings, out aErrorMessage);
					}
				}
			}
			catch (Exception ex)
			{
				aErrorMessage = "Error processing APISettings File" + Environment.NewLine + Environment.NewLine + ex.ToString();
			}
			return Result;
		}		

		private static bool UpdateHostSettings(JObject Source, 
		                                       APISettingsItem NewSettings, 
		                                       APISettingsItem OldSettings, 
		                                       out string ErrorMessage)
		{
			bool lResult = false;			
			ErrorMessage = "";
			try
			{
				JObject jHostSettings = Source["HostSettings"] as JObject;
				
				if (jHostSettings != null)
				{
					if (NewSettings.HostTimerInterval != OldSettings.HostTimerInterval)
					{					
						SetStringValueForJSONElement(jHostSettings, "timerInterval_ms", NewSettings.HostTimerInterval.ToString());
					}					
					
					if (NewSettings.HostInitLoggingHour != OldSettings.HostInitLoggingHour)
					{					
						SetStringValueForJSONElement(jHostSettings, "initLoggingHour", NewSettings.HostInitLoggingHour.ToString());
					}			
				}
				
				lResult = true;
			}
			catch(Exception e)
			{
				ErrorMessage = e.ToString();
			}			
			return lResult;
		}
		
		
		private static bool UpdateServiceSettings(JObject Source, 
		                                          APISettingsItem NewSettings, 
		                                          APISettingsItem OldSettings, 
		                                          out string ErrorMessage)
		{
			bool Result = false;			
			ErrorMessage = "";
			try
			{
				JObject jSettings = Source["Service"] as JObject;
				
				if (jSettings != null)
				{
					if (NewSettings.APISessionName != OldSettings.APISessionName)
					{					
						SetStringValueForJSONElement(jSettings, "aPISessionName", NewSettings.APISessionName);
					}
					
					if (NewSettings.ServiceURL != OldSettings.ServiceURL)
					{					
						SetStringValueForJSONElement(jSettings, "serviceUrl", NewSettings.ServiceURL);
					}					
					
					if (NewSettings.HttpPort != OldSettings.HttpPort)
					{					
						SetStringValueForJSONElement(jSettings, "httpPort", NewSettings.HttpPort);
					}					

					if (NewSettings.TCPPort != OldSettings.TCPPort)
					{					
						SetStringValueForJSONElement(jSettings, "tcpPort", NewSettings.TCPPort);
					}										
					
					if (NewSettings.APIServiceHostName != OldSettings.APIServiceHostName)
					{					
						SetStringValueForJSONElement(jSettings, "aPIServiceHostName", NewSettings.APIServiceHostName);
					}															
				}
				Result = true;				
			}
			catch(Exception e)
			{
				ErrorMessage = e.ToString();
			}			
			return Result;
		}

		private static bool UpdateDatabaseSettings(JObject Source, 
		                                           APISettingsItem NewSettings, 
		                                           APISettingsItem OldSettings, 
		                                           out string ErrorMessage)
		{
			bool Result = false;		
			ErrorMessage = "";						
			try
			{
				JObject jHostSettings = Source["Database"] as JObject;
				
				if (jHostSettings != null)
				{
					/*
					if (NewSettings.DBProfile != OldSettings.DBProfile)
					{					
						SetStringValueForJSONElement(jHostSettings, "profile", NewSettings.DBProfile.ToString());
					}					
					*/
					
					if (NewSettings.DBServer != OldSettings.DBServer)
					{					
						SetStringValueForJSONElement(jHostSettings, "server", NewSettings.DBServer);
					}					
					
					if (NewSettings.DBName != OldSettings.DBName)
					{					
						SetStringValueForJSONElement(jHostSettings, "databaseName", NewSettings.DBName);
					}										
				}
				Result = true;				
			}
			catch(Exception e)
			{
				ErrorMessage = e.ToString();
			}			
			return Result;
		}
					
		private static bool UpdateLoggingSettings(JObject Source, 
		                                          APISettingsItem NewSettings, 
		                                          APISettingsItem OldSettings, 
		                                          out string ErrorMessage)
		{
			bool Result = false;	
			ErrorMessage = "";			
			string BoolFlag = "";			
			try
			{
				JObject jHostSettings = Source["Logging"] as JObject;
				
				if (jHostSettings != null)
				{
					if (NewSettings.DebugLogging != OldSettings.DebugLogging)
					{					
						if (NewSettings.DebugLogging)
							BoolFlag = "true";
						else
							BoolFlag = "false";													
						SetStringValueForJSONElement(jHostSettings, "debugLogging", BoolFlag);
					}					
					
					if (NewSettings.EventViewerLogging != OldSettings.EventViewerLogging)
					{					
						if (NewSettings.EventViewerLogging)
							BoolFlag = "true";
						else
							BoolFlag = "false";													
						SetStringValueForJSONElement(jHostSettings, "eventViewerLogging", BoolFlag);
					}					
				}
				Result = true;				
			}
			catch(Exception e)
			{
				ErrorMessage = e.ToString();
			}			
			return Result;
		}				
		
		private static bool UpdateMaintenanceSettings(JObject Source, 
		                                              APISettingsItem NewSettings, 
		                                              APISettingsItem OldSettings, 
		                                              out string ErrorMessage)
		{			
			bool Result = false;	
			ErrorMessage = "";						
			try
			{
				JObject jHostSettings = Source["Maintenance"] as JObject;
				
				if (jHostSettings != null)
				{
					if (NewSettings.MaintenanceStart != OldSettings.MaintenanceStart)
					{													
						SetStringValueForJSONElement(jHostSettings, "startTime", NewSettings.MaintenanceStart.ToString("HH:mm"));
					}					
					
					if (NewSettings.MaintenanceEnd != OldSettings.MaintenanceEnd)
					{					
						SetStringValueForJSONElement(jHostSettings, "endTime", NewSettings.MaintenanceEnd.ToString("HH:mm"));
					}										
				}
				Result = true;				
			}
			catch(Exception e)
			{
				ErrorMessage = e.ToString();
			}			
			return Result;			
		}				

		private static bool SaveAPISettingsJsonString(APIAppType AppType, 
		                                              string JsonString,
		                                              APISettingsItem OldSettings,
		                                              out string aErrorMessage)
		{
			return SaveAPISettingsJsonString(AppType, "", JsonString, OldSettings, out aErrorMessage);
		}
		
		private static bool SaveAPISettingsJsonString(APIAppType AppType, 
		                                              string AppDir, 
		                                              string JsonString, 
		                                              APISettingsItem OldSettings, 
		                                              out string ErrorMessage)
		{
			bool Result = false;
			ErrorMessage = "";
			try
			{
				if (String.IsNullOrEmpty(JsonString))
				{
					ErrorMessage = "Unable to write to settings file - Contents are blank";
						
					return Result;
				}
				
				string FileName = GetAPISettingsFileName(AppType, AppDir, out ErrorMessage);				
												
				// for testing save to different file name				
				/*
				lFileName = @"C:\Source\DotNet\EAOffice\EAOfficeAPIService\EAOfficeAPISetup\bin\Debug\APISettings1.config";
				System.IO.File.WriteAllText(lFileName, aJsonString);
				aErrorMessage = "bypass saving of text file";
				*/

				if (String.IsNullOrEmpty(ErrorMessage))
				{
					System.IO.File.WriteAllText(FileName, JsonString);
				}
				
				Result = (String.IsNullOrEmpty(ErrorMessage));
			}
			catch (Exception ex)
			{
				ErrorMessage = "Error processing APISettings File" + Environment.NewLine + Environment.NewLine + ex.ToString();
			}			
			return Result;
		}

        public static void WriteToOutput(string LineText)
        {
            string TodayStr = DateTime.Today.ToString("yyyyMMdd");
            string FileName = "ActionBase_" + TodayStr + ".txt";
            string FWorkingDir = Directory.GetCurrentDirectory();
            if (!FWorkingDir.EndsWith(Path.DirectorySeparatorChar.ToString())) FWorkingDir += Path.DirectorySeparatorChar;
            Console.WriteLine("WorkingDir: " + FWorkingDir);

            FileName = FWorkingDir + FileName;
            using (StreamWriter writer = new StreamWriter(FileName, true))
            {
                writer.WriteLine(LineText);
                Console.WriteLine(LineText);
            }
        }
    }
}
