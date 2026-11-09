/*
 * Created by SharpDevelop.
 * User: John
 * Date: 17/01/2019
 * Time: 5:28 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.ComponentModel;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CommonUtilities;
using EAOfficeAPI;
using UsefulDBMethods;

namespace Actions.Portfolio
{
	/// <summary>
	/// Description of ProcessReport.
	/// </summary>
	public class ProcessReport : ActionBase
	{
		private string FModuleName = "Portfolio";
		
		public static string Execute(int MasterId,
		                             RequestCommonItem CommonItem,
		                             JObject DataObject,
		                             APISettingsItem SettingsItem,
		                             out string ErrorMsg)
		{
			string Method = "DefaultMethods - Execute";
			string Result = "";
			ErrorMsg = "";

			ProcessReport inst = new ProcessReport();
			if (!inst.Setup(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg))
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
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " +Method, FModuleName, MasterId);
			LogOnInsideMethod(Method);
			try 
			{
				/*
				string BaseFormat = "{0} is {1}";
				string NotProvided = "Not Provided";
				string Invalid = "Invalid";				
				string Key = "";
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
		
		private string Output()
		{
			string Method = "Output";
			string Result = "";
			string ErrorMsg = "";
			LogOnInsideMethod(Method);
			try 
			{
				//Result = "{\"message\": \"hello\"}";
				Result = BuildOutput(out ErrorMsg);
				if (!string.IsNullOrEmpty(ErrorMsg))
					WindowsEventLoggingUtilities.WriteToErrorEvent(ErrorMsg, Method, MasterId);
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
		
		private string BuildOutput(out string ErrorMsg)
		{
			ErrorMsg = "";
			string Result = "";
			int Filter1Id = 0;
			int Filter2Id = 0;

			int Filter1EventId = 0;
			int Filter2EventId = 0;
			
			string AskLater = "";
			string AskFor = "";
			string FromValue = "";
			string FromDate = "";
			string ToValue = "";
			string ToDate = "";

			const string Method = "ExecuteCommon";
			string ConnectionString = CommonDBMethods.Common.ConnectionString(APISettings);
			LogOnInsideMethod(Method);
			try 
			{
				WindowsEventLoggingUtilities.WriteToInfoEvent("Inside ExecuteSearchResults", FModuleName, MasterId);
				Filter1Id = StringUtilities.StrToIntDef((string)DataObject["filter1Id"], 0);
				Filter2Id = StringUtilities.StrToIntDef((string)DataObject["filter2Id"], 0);
			
				if (Filter1Id > 0)
				{
					Filter1EventId = 
						UsefulDBMethods.EventHelpers.SetupFilterEventRecord(
							this.FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue, Filter1Id, out ErrorMsg);
					if (!string.IsNullOrEmpty(ErrorMsg)) return this.LogOnProcessingError(Method, ErrorMsg, "");
					WindowsEventLoggingUtilities.WriteToInfoEvent("Filter1EventId: " + Filter1EventId.ToString(), FModuleName, MasterId);
					
					if (Filter1EventId > 0)
					{
						ErrorMsg = CommonMethods.GetFilterValues(1, DataObject, out AskLater, out AskFor, out FromValue, out FromDate, out ToValue, out ToDate);
						if (!string.IsNullOrEmpty(ErrorMsg)) return this.LogOnProcessingError(Method, ErrorMsg, "");
						if (CommonUtilities.StringUtilities.SameText(AskLater, "1"))
					    {
							ErrorMsg = 
								EventHelpers.UpdateFilterEventRecordItems(
									Filter1EventId, AskLater, AskFor, FromValue, FromDate, ToValue, ToDate, this.FbConnectionString);
							if (!string.IsNullOrEmpty(ErrorMsg)) return this.LogOnProcessingError(Method, ErrorMsg, "");
							ErrorMsg = EventHelpers.CallFilterEventSqlCreate(Filter1EventId, this.FbConnectionString);
							if (!string.IsNullOrEmpty(ErrorMsg)) return this.LogOnProcessingError(Method, ErrorMsg, "");
						}
					}
				}
				
				if (Filter2Id > 0)
				{
					Filter2EventId = 
						UsefulDBMethods.EventHelpers.SetupFilterEventRecord(
							this.FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue, Filter2Id, out ErrorMsg);
					if (!string.IsNullOrEmpty(ErrorMsg)) return this.LogOnProcessingError(Method, ErrorMsg, "");
					
					if (Filter2EventId > 0)
					{
						WindowsEventLoggingUtilities.WriteToInfoEvent("Filter2EventId: " + Filter2EventId.ToString(), FModuleName, MasterId);
						ErrorMsg = CommonMethods.GetFilterValues(2, DataObject, out AskLater, out AskFor, out FromValue, out FromDate, out ToValue, out ToDate);
						if (!string.IsNullOrEmpty(ErrorMsg)) return this.LogOnProcessingError(Method, ErrorMsg, "");
						if (CommonUtilities.StringUtilities.SameText(AskLater, "1"))
					    {
							ErrorMsg = 
								EventHelpers.UpdateFilterEventRecordItems(
									Filter2EventId, AskLater, AskFor, FromValue, FromDate, ToValue, ToDate, this.FbConnectionString);
							if (!string.IsNullOrEmpty(ErrorMsg)) return this.LogOnProcessingError(Method, ErrorMsg, "");
							ErrorMsg = EventHelpers.CallFilterEventSqlCreate(Filter2EventId, this.FbConnectionString);
							if (!string.IsNullOrEmpty(ErrorMsg)) return this.LogOnProcessingError(Method, ErrorMsg, "");
						}
					}
				}
			
				if (!string.IsNullOrEmpty(ConnectionString)) {
					List<FbParameterItem> Params = new List<FbParameterItem>();
					WindowsEventLoggingUtilities.WriteToInfoEvent("ConnectionString: " + ConnectionString, FModuleName, MasterId);
					string QueryText = 
						"SELECT SqlReportString FROM ListPortfolio(" + 
						"@CompanyId, @OfficeId, @ManagerId, @PortfolioType, @SearchText, @LedgerTypeId, @Ledger, @Filter1EventId, @ActiveStatus, @Filter2EventId)";
					Params.Add(new FbParameterItem("@CompanyId", CommonItem.CompanyId));
					Params.Add(new FbParameterItem("@OfficeId",	StringUtilities.StrToIntDef((string)DataObject["officeId"], 0)));
					Params.Add(new FbParameterItem("@ManagerId", StringUtilities.StrToIntDef((string)DataObject["managerId"], 0)));
					//Params.Add(new FbParameterItem("@PortfolioType", StringUtilities.StrToIntDef((string)DataObject["busTypeId"], 33)));
					Params.Add(new FbParameterItem("@PortfolioType", StringUtilities.StrToIntDef((string)DataObject["portfolioId"], 33)));
					Params.Add(new FbParameterItem("@SearchText", DataObject["searchText"]));
					Params.Add(new FbParameterItem("@LedgerTypeId", StringUtilities.StrToIntDef((string)DataObject["ledgerTypeId"], 0)));
					Params.Add(new FbParameterItem("@Ledger", StringUtilities.StrToIntDef((string)DataObject["ledger"], 0)));
					Params.Add(new FbParameterItem("@Filter1EventId", Filter1EventId));
					Params.Add(new FbParameterItem("@ActiveStatus", StringUtilities.StrToIntDef((string)DataObject["activeStatusId"], 0)));
					Params.Add(new FbParameterItem("@Filter2EventId", Filter2EventId));

					//WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText:" + Environment.NewLine + QueryText, FModuleName, MasterId);
					UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
					

					DataSet dataSetFromQuery = FirebirdUtilities.GetDataSet(ConnectionString, QueryText, Params, 0);
					if (dataSetFromQuery == null)
					{
						ErrorMsg = 
							"Error Executing Query" + Environment.NewLine +
							ErrorMsg + Environment.NewLine +
							"QueryText:" + Environment.NewLine + QueryText;
						WindowsEventLoggingUtilities.WriteToInfoEvent(ErrorMsg, "EAOfficeAPI", MasterId);
						Result = LogOnErrorSelectingData(Method, ErrorMsg, QueryText);
						return Result;
					}
					else if (dataSetFromQuery.Tables.Count == 0) {
						ErrorMsg = "No tables in the dataset to process";
						Result = LogOnErrorSelectingData(Method, ErrorMsg, QueryText);
						return Result;
					}
					string Str = "";

					foreach (DataRow dataRow in dataSetFromQuery.Tables[0].Rows) {
						Str = (dataRow[0] == DBNull.Value) ? "" : Convert.ToString(dataRow[0]);
					}

					// We now have the query what do we do with it					
					if (string.IsNullOrEmpty(Str))
					{
						ErrorMsg = "No SqlReportString available";
						Result = LogOnErrorSelectingData(Method, ErrorMsg, QueryText);
					}
					else
					{
						string ReportType = (string)DataObject["reportType"];
						switch (ReportType)
						{
							case "PDF": ReportType = "0"; break;
							case "ABA": ReportType = "1"; break;
							case "XLS": ReportType = "2"; break;
							default: ReportType = ""; break;
						}
						
						int ReportEventId = 
							CommonMethods.FilterEventForPortfolio(
								ConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue, 
								Filter1EventId, Filter2EventId, Str, "ListPortfolio-ProcessReport", out ErrorMsg);
						if (!string.IsNullOrEmpty(ErrorMsg))
						{
							Result = LogOnProcessingError(Method, ErrorMsg, "");
						}
						else // We now have the query to call for the Portfolio Search Results
							Result = ProcessReportString(Str, ReportType, ReportEventId, out ErrorMsg);
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
		
		private string ProcessReportString(string SqlReportString,
		                                   string ReportType,
		                                   int ReportEventId,
		                                   out string ErrorMsg)
		{
			ErrorMsg = "";
			string Method = "ProcessReportString";
			string Result = "";
			
			string ReportId = (string)DataObject["reportId"];
			// store the SqlReportString wherever it needs to be stored
			WindowsEventLoggingUtilities.WriteToInfoEvent("Before calling AddReportEvent", this.FModuleName, MasterId);
			int RunId = 
				UsefulDBMethods.EventHelpers.AddReportEvent(
					this.FbConnectionString, 
					this.CommonItem.CompanyIdValue,
					StringUtilities.StrToIntDef ((string)DataObject["officeId"], 0),
					StringUtilities.StrToIntDef ((string)DataObject["managerId"], 0),
					StringUtilities.StrToIntDef ((string)DataObject["busTypeId"], 0),
					true, // ForMyself
					StringUtilities.StrToIntDef(ReportType, 0),
					StringUtilities.StrToIntDef(ReportId, 0),
					SqlReportString, "Portfolio", this.CommonItem.ClientIdValue, out ErrorMsg);
			if (!string.IsNullOrEmpty(ErrorMsg))
			{
				Result = this.LogOnProcessingError(Method, ErrorMsg, "");
				return Result;
			}					
			
			WindowsEventLoggingUtilities.WriteToInfoEvent("RunId: " + RunId.ToString(), this.FModuleName, MasterId);
			
			if (RunId == 0) 
			{
				ErrorMsg = "Error building Report Event";
				Result = LogOnProcessingError(Method, ErrorMsg, "");
				return Result;
			}
			
			string CommandLine =
				UsefulDBMethods.EventHelpers.GetEventCommandLineForRunId(
					FbConnectionString, RunId, out ErrorMsg);
			if (!string.IsNullOrEmpty(ErrorMsg))
			{
				ErrorMsg = this.LogOnProcessingError(Method, ErrorMsg, "");
				return "";
			}
			
			// call the report app with the appropriate parameters
			//C:\rod\report>testrpo /reportid:1 /eventid:1 /ouputid:0
			
			//string CmdBase = @"E:\temp\report\{0}";
			string CmdBase = global::UsefulDBMethods.CompanyFolderSettings.TestRPOLocation(FbConnectionString, CommonItem.CompanyIdValue, out ErrorMsg);
			CmdBase += "{0}";
			if (!string.IsNullOrEmpty(ErrorMsg))
			{
				ErrorMsg = this.LogOnProcessingError(Method, ErrorMsg, "");
				return "";
			}
			
			string AppName = CommandLine.Substring(0, CommandLine.IndexOf(" /"));
			AppName = string.Format(CmdBase, AppName);
			
			//string ArgsBase = "/reportid:{0} /eventid:{1} /outputid:{2}";
			string ArgsBase = CommandLine.Substring(CommandLine.IndexOf("/"));
			WindowsEventLoggingUtilities.WriteToInfoEvent("AppName: " + AppName, this.FModuleName, MasterId);
			WindowsEventLoggingUtilities.WriteToInfoEvent("Args: " + ArgsBase, this.FModuleName, MasterId);
			
			ReportId = "1"; // for testing the method
			//string EventId = "1"; // for testing the method
			StringBuilder sb = new StringBuilder();
			
			Process runApp = new Process();
			runApp.StartInfo.FileName = AppName;
			runApp.StartInfo.Arguments = ArgsBase; // string.Format(ArgsBase, ReportId, EventId, ReportType);
			runApp.Start();
			//runApp.WaitForExit(10000);
			int Tries = 1000;
			for (int Index = 0; Index < Tries; Index++)
			{
				if (!runApp.HasExited) System.Threading.Thread.Sleep(1000);
			}
			if (!runApp.HasExited)
			{
				Result = "Application did not finish in time.";
			}
			
			WindowsEventLoggingUtilities.WriteToInfoEvent("After calling: " + AppName, FModuleName, MasterId);
			
			string BlobField = "";
			string BlobTable = "";
			string BlobFileUniqueFieldName = "";
			int BlobFieldUniqueId = 0;
			string BlobFileExt = "";
			if (string.IsNullOrEmpty(Result))
			{
				Result = 
					GetEventRunValues(
						RunId, out BlobField, out BlobTable, out BlobFileUniqueFieldName, out BlobFieldUniqueId, out BlobFileExt);
			}

            // GoatImport will place the file BlobFieldUniqueId.BlobFileExt in /Docs
            // replace the following and only use this file in the docs location

            string OutputFileName = "";
            string FileNameOnly = "";
            //string DateStr = MiscUtilities.DateTimeToYYYYMMDDTHHNNSS(DateTime.Now);
            string OutputFolder = global::UsefulDBMethods.CompanyFolderSettings.DocsLocation(FbConnectionString, CommonItem.CompanyIdValue, out Result);
            if (!OutputFolder.EndsWith(Path.DirectorySeparatorChar.ToString())) 
                OutputFolder += Path.DirectorySeparatorChar.ToString();
            FileNameOnly = BlobFieldUniqueId.ToString() + BlobFileExt;
            OutputFileName = OutputFolder + FileNameOnly;
            WindowsEventLoggingUtilities.WriteToInfoEvent("OutputFileName: " + OutputFileName, Method, MasterId);
            if (!File.Exists(OutputFileName)) 
                Result = "The file: " + FileNameOnly + " does not exist";

            /*
			string OutputFileName = "";
			string FileNameOnly = "";
			string DateStr = MiscUtilities.DateTimeToYYYYMMDDTHHNNSS(DateTime.Now);
			if (string.IsNullOrEmpty(Result))
			{
				string SqlToUse =
					"SELECT " + BlobField + " FROM " + BlobTable + " WHERE " + BlobFileUniqueFieldName + " = " + BlobFieldUniqueId.ToString();
				string OutputFolder = global::UsefulDBMethods.CompanyFolderSettings.DocsLocation(FbConnectionString, CommonItem.CompanyIdValue, out Result);
				if (!OutputFolder.EndsWith(Path.DirectorySeparatorChar.ToString())) OutputFolder += Path.DirectorySeparatorChar.ToString();
				FileNameOnly = BlobFieldUniqueId.ToString() + "-" + DateStr;
				OutputFileName = OutputFolder + FileNameOnly;
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = global::UsefulDBMethods.Reporting.SystemReportUtilities.WriteOutputFile(FbConnectionString, SqlToUse, OutputFileName, BlobFileExt);
				}				
			}
			*/

            try
            {
				if (string.IsNullOrEmpty(Result))
				{
					sb.AppendLine(JsonUtilities.StartJson());
					//sb.AppendLine(JsonUtilities.AddPropertyNameAndValue("OutputFileName", OutputFileName + BlobFileExt));
					//sb.AppendLine(JsonUtilities.AddPropertyNameAndValue("FileName", "/Docs/" + FileNameOnly + BlobFileExt));
                    sb.AppendLine(JsonUtilities.AddPropertyNameAndValue("OutputFileName", OutputFileName));
                    sb.AppendLine(JsonUtilities.AddPropertyNameAndValue("FileName", "/Docs/" + FileNameOnly));
                    sb.AppendLine(JsonUtilities.AddPropertyNameAndValue("reportEventId", ReportEventId));
					sb.AppendLine(JsonUtilities.AddPropertyNameAndValue("runId", RunId, false));
					sb.AppendLine(JsonUtilities.EndJson());
					Result = sb.ToString();
				}
				else
				{
					Result = LogOnProcessingError(Method, Result, "");
				}
			}
			catch (Exception Ex)
			{
				Result = LogOnException(Method, Ex, "");
				return Result;
			}
			
			return Result;
		}
		
		private string GetEventRunValues(int ReportEventId,
		                                 out string BlobField,
		                                 out string BlobTable, 
		                                 out string BlobFileUniqueFieldName,
		                                 out int BlobFieldUniqueId,
		                                 out string BlobFileExt)
		{
			string Result = "";
			string Method = "GetEventRunValues";
			BlobField = "";
			BlobTable = "";
			BlobFileUniqueFieldName = "";
			BlobFieldUniqueId = 0;
			BlobFileExt = "";
			
			try 
			{
				string QueryText =
					" SELECT BlobField, BlobTable, BlobFileUniqueFieldName, BlobFieldUniqueId, BlobFileExt" +
					" FROM EventRunDisplayActions(@CompanyId, @RunId, 0, 0, 0, 0)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("@CompanyId", CommonItem.CompanyIdValue));
				Params.Add(new FbParameterItem("@RunId", ReportEventId));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
			
				DataTable tbl = null;
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							FbConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
				}
			
				if ((string.IsNullOrEmpty(Result)) && (tbl.Rows.Count > 0))
				{
					DataRow CurrentRow = tbl.Rows[0];
					Object Obj = CurrentRow["BlobField"];
					BlobField = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					Obj = CurrentRow["BlobTable"];
					BlobTable = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					Obj = CurrentRow["BlobFileUniqueFieldName"];
					BlobFileUniqueFieldName = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					Obj = CurrentRow["BlobFieldUniqueId"];
					BlobFieldUniqueId = (Obj == DBNull.Value) ? 0 : Convert.ToInt32(Obj);
					Obj = CurrentRow["BlobFileExt"];
					BlobFileExt = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
				}
				
			}
			catch (Exception Ex)
			{
				Result =
					Method + Environment.NewLine +
					Ex.Message + Environment.NewLine +
					Ex.StackTrace;
			}
			return Result;
		}
	}
}