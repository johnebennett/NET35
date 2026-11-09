	/*
 * Created by SharpDevelop.
 * User: John
 * Date: 12/08/2019
 * Time: 6:47 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;

/*
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
*/

using CommonUtilities;
using EAOfficeAPI;

namespace Actions.PaymentOperations
{
	/// <summary>
	/// Description of ProcessPreviewReport.
	/// </summary>
	public class ProcessPreviewReport : ActionBase
	{
		private string FModuleName = "PaymentOperations";
		private ProcessPreviewReportRequestItem FRequestItem;
		
		/*
		public static string Execute(int MasterId,
									 RequestCommonItem CommonItem,
									 JObject DataObject,
									 APISettingsItem SettingsItem,
									 out string ErrorMsg)
		*/
		public static string Execute(int MasterId,
									 APISettingsItem SettingsItem,
									 string Request,
									 out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			ProcessPreviewReport inst = new ProcessPreviewReport();
			//if (!inst.Setup(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg))
			if (!inst.Setup(MasterId, SettingsItem, Request, out ErrorMsg))
				return "";
			
			inst.LogOnInsideMethod(Method);
			try
			{
				Result = inst.Init();				
				if (!string.IsNullOrEmpty(Result))
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent(Result, Method, MasterId);
					return "";
				}
				
				Result = inst.Validate();
				if (!string.IsNullOrEmpty(Result))
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent(Result, Method, MasterId);
					return "";
				}

				Result = inst.Output();
				UsefulDBMethods.Helpers.RunEAMainOfficeProcess(inst.FbConnectionString, inst.CommonItem.CompanyIdValue);
			}
			finally
			{
				inst.LogOnLeavingMethod(Method);
			}
			
			return Result;
		}
			
		private string Init()
		{
			string Method = "Init";
			string Result = "";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " +Method, FModuleName, MasterId);
			LogOnInsideMethod(Method);
			try 
			{
				FRequestItem = new ProcessPreviewReportRequestItem();
				Result = FRequestItem.Load((ActionBase)this);
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
			
		private string Validate()
		{
			string Method = "Validate";
			string Result = "";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " + Method, FModuleName, MasterId);
			LogOnInsideMethod(Method);
			try 
			{
				Result = FRequestItem.Validate();
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
		
		private string Output()
		{
			string Method = "Output";
			string Result = "";
			string ErrorMsg = "";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " +Method, FModuleName, MasterId);
			LogOnInsideMethod(Method);
			try 
			{
				Result = BuildOutput(out ErrorMsg);
				if (!string.IsNullOrEmpty(ErrorMsg))
				{
					Result = LogOnProcessingError(Method, ErrorMsg, "");
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
			WindowsEventLoggingUtilities.WriteToInfoEvent("Leaving: " +Method, FModuleName, MasterId);
			return Result;
		}
		
		private string BuildOutput(out string ErrorMsg)
		{
			ErrorMsg = "";
			string Result = "";
			string Method = "BuildOutput";
			
			LogOnInsideMethod(Method);
			try
			{
				WindowsEventLoggingUtilities.WriteToInfoEvent("Inside BuildOutput", FModuleName, MasterId);
				
				StringBuilder Entries = 
					//new global::UsefulDBMethods.Reporting.ReportUtilities().BuildReportFileList(
                    new global::UsefulDBMethods.Reporting.ReportUtilities().BuildReportFileList_ExcludingBase64(
                        FbConnectionString, FRequestItem.ReportId, CommonItem.CompanyIdValue, CommonItem.ClientIdValue,
						this.CommonCollection, this.DataCollection, "Items", out ErrorMsg);
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Result =
						JsonUtilities.StartJson() +
						Entries.ToString() +
						JsonUtilities.EndJson();						
				}
				/*
				string ReportSql = "";
				int EventRunId = 0;
				ErrorMsg = CommonMethods.BuildReportSql((ActionBase)this, FRequestItem.ReportId, out ReportSql, out EventRunId);
				WindowsEventLoggingUtilities.WriteToInfoEvent("ReportSql:" + Environment.NewLine + ReportSql, FModuleName, MasterId);
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg = ExecuteEventRunCreate(EventRunId, FRequestItem.ReportId, ReportSql);						
				}
				
				string CommandLine = "";
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					CommandLine = 
						UsefulDBMethods.EventHelpers.GetEventCommandLineForRunId(
							FbConnectionString, EventRunId, out ErrorMsg);
				}
				
				string AppLocation = "";				
				string DocsLocation = "";
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg = GetCompanySettingsItems(FbConnectionString, CommonItem.CompanyIdValue, out AppLocation, out DocsLocation);
				
					if (string.IsNullOrEmpty(ErrorMsg))
					{
						if  (!AppLocation.EndsWith(@"\")) AppLocation += @"\";
						AppLocation += "{0}";
					}
					
				}
				
				string AppName = "";
				string ArgsBase = "";
				StringBuilder Entries = new StringBuilder();
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					AppName = CommandLine.Substring(0, CommandLine.IndexOf(" /"));
					AppName = string.Format(AppLocation, AppName);
					ArgsBase = CommandLine.Substring(CommandLine.IndexOf("/"));
					WindowsEventLoggingUtilities.WriteToInfoEvent("AppName: " + AppName, FModuleName, MasterId);
					WindowsEventLoggingUtilities.WriteToInfoEvent("Args: " + ArgsBase, FModuleName, MasterId);
					
					Process runApp = new Process();
					runApp.StartInfo.FileName = AppName;
					runApp.StartInfo.Arguments = ArgsBase;
					runApp.Start();
					runApp.WaitForExit(10000);
					
					WindowsEventLoggingUtilities.WriteToInfoEvent("After calling: " + AppName, FModuleName, MasterId);
					
					Entries.Append(JsonUtilities.StartJson());
					Entries.Append(JsonUtilities.StartJsonArray("Items"));
					ErrorMsg = BuildFileList(FbConnectionString, EventRunId, DocsLocation, ref Entries);
			        Entries.Append(CommonUtilities.JsonUtilities.EndJsonArray());
					Entries.Append(CommonUtilities.JsonUtilities.EndJson());
				}
				*/
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
		
		private string ExecuteEventRunCreate(int EventRunId, int ReportId, string ReportSql)
		{
			string TodayStr = MiscUtilities.DateTimeToYYYYMMDD(DateTime.Now);
			int PdfType = 0;
			string Result = 
				UsefulDBMethods.EventHelpers.ExecuteEventRunCreate(
					FbConnectionString, EventRunId, CommonItem.CompanyIdValue, 0, 0, 0, CommonItem.ClientIdValue, 0,
					ReportSql, "", "", 0, "", "", 0, "", "", "", "", "Payment", "Y", PdfType, ReportId, 0, 0, 0, TodayStr, TodayStr, "", 0);
			
			return Result;
		}
		
		private string GetCompanySettingsItems(string ConnectionString,
		                                       int CompanyClientId,
		                                       out string WordLocation,
		                                       out string DocsLocation)
		{
			string Result = "";
			WordLocation = "";
			DocsLocation = "";
			
			try 
			{
				string QueryText = "SELECT CO_EAWordLocation, CO_DocsLocation FROM EACompanySettings WHERE I_Settings_CompanyId = @CompanyClientId";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyClientId", CompanyClientId));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
				if (string.IsNullOrEmpty(Result))
				{
					DataTable tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, QueryText, Params, 0, 
							UsefulDBMethods.FirebirdQueryCheckTypes.HasTablesAndRows, out Result);
					if (string.IsNullOrEmpty(Result))
					{
						object Obj = tbl.Rows[0]["CO_EAWordLocation"];
						WordLocation = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
						Obj = tbl.Rows[0]["CO_DocsLocation"];
						DocsLocation = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					}
				}
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
		
		private string BuildFileList(string ConnectionString,
		                             int EventRunId,
		                             string DocsLocation,
		                             ref StringBuilder Entries)
		{
			string Result = "";
			
			try 
			{
				string QueryText =
					" SELECT EC_ID, EC_Base64, EC_EventRunId, EC_FileExtension, EC_DisplayName" +
					" FROM EAEventRunBase64 WHERE EC_EventRunId = @EventRunId ORDER BY EC_ID";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("EventRunId", EventRunId));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
				
				if (string.IsNullOrEmpty(Result))
				{
					DataTable tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
					if (string.IsNullOrEmpty(Result))
					{
						object Obj = null;			
						string FileExt = "";
						string ECId = "";
						string RunId = "";
						string DisplayName= "";
						string FileNameOnly = "";
						string OutputLocation = "";
						string OutputFileName = "";
						string Base64String = "";
						string Str = "";
		        
		        		List<string> RowList = new List<string>();
		        		foreach(DataRow ARow in tbl.Rows)
		        		{
				        	Obj = ARow["EC_DisplayName"];
				        	DisplayName = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
				        	Obj = ARow["EC_FileExtension"];
				        	FileExt = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
				        	Obj = ARow["EC_ID"];
				        	ECId = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
				        	Obj = ARow["EC_EventRunId"];
				        	RunId = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
				        	FileNameOnly = "{0}-{1}{2}";
				        	FileNameOnly = string.Format(FileNameOnly, RunId, ECId, FileExt);
				        	OutputLocation = @"\Docs\" + FileNameOnly;
				        	OutputFileName = DocsLocation + FileNameOnly;
				        	WindowsEventLoggingUtilities.WriteToInfoEvent("OutputFileName" + Environment.NewLine + OutputFileName, FModuleName, MasterId);
				        	Obj = ARow["EC_Base64"];
				        	byte[] toBytes = (byte[])Obj;
				        	Base64String = System.Text.Encoding.Default.GetString(toBytes);
				        	File.WriteAllBytes(OutputFileName, Convert.FromBase64String(Base64String));	        	
				        	Str =
				        		JsonUtilities.StartJsonObject("") +
				        		JsonUtilities.AddPropertyNameAndValue("displayName", DisplayName) +
				        		JsonUtilities.AddPropertyNameAndValue("file", OutputFileName) +
				        		JsonUtilities.AddPropertyNameAndValue("href", OutputLocation, false) +
				        		JsonUtilities.EndJsonObject();
				        	RowList.Add(Str);
				        }
		        		Entries.Append(string.Join(",", RowList.ToArray()));
					}
				}
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
		
		/*
		private string BuildOutputEntries(ref StringBuilder Entries)
		{
			string Method = "BuildOutputEntries";
			string Result = "";			
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, FModuleName, MasterId);
			
			LogOnInsideMethod(Method);
			try
			{
				string QueryText = "";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				//Params.Add(new FbParameterItem("DisplayType", FSearchGridRequestItem.DisplayType));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
				
				if (string.IsNullOrEmpty(Result))
				{
					DataTable tbl = 
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							FbConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.None, out Result);
					if (string.IsNullOrEmpty(Result) && (tbl.Rows.Count > 0))
					{
						StringBuilder Entries = new StringBuilder();
						Entries.AppendLine(JsonUtilities.StartJson());
						
						Result = CommonMethods.BuildJsonArrayFromDataTable(tbl, "whyNotCancelled", ref Entries);

						Entries.AppendLine(JsonUtilities.EndJson());
						Result = Entries.ToString();
					}
				}

				
				string Str = "";
				string ColName = "";
				string ColValue = "";
				List<string> RowList = new List<string>();
				List<string> StrList = new List<string>();
				object Obj = null;
				Entries.AppendLine(JsonUtilities.StartJsonArray("Items"));
				foreach (DataRow ThisRow in dt.Rows)
				{
					StrList.Clear();
					foreach (DataColumn ThisCol in dt.Columns)
					{
						ColName = ThisCol.ColumnName;
						Obj = ThisRow[ColName];
						ColValue = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
						StrList.Add(JsonUtilities.AddPropertyNameAndValue(ColName, ColValue, false));
					}
					Str =
						JsonUtilities.StartJsonObject("") +
						string.Join(",", StrList.ToArray()) +
						JsonUtilities.EndJsonObject();
					RowList.Add(Str);
				}
				Entries.Append(string.Join(",", RowList.ToArray()));
				Entries.AppendLine(JsonUtilities.EndJsonArray());
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
		*/
	}
}
