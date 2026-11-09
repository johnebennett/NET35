/*
 * Created by SharpDevelop.
 * User: John
 * Date: 25/07/2019
 * Time: 10:06 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text;
using System.Data;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CommonUtilities;
using EAOfficeAPI;

namespace Actions.PaymentOperations
{
	/// <summary>
	/// Description of NewPaymentPreviewProcessList.
	/// </summary>
	public class NewPaymentPreviewAcceptPreview : ActionBase
	{
		private string FModuleName = "PaymentOperations";
		private NewPaymentPreviewAcceptPreviewRequestItem FRequestItem;
		
		public static string Execute(int MasterId,
		                             RequestCommonItem CommonItem,
		                             JObject DataObject,
		                             APISettingsItem SettingsItem,
		                             out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			NewPaymentPreviewAcceptPreview inst = new NewPaymentPreviewAcceptPreview();
			if (!inst.Setup(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg))
				return "";
			
			inst.LogOnInsideMethod(Method);
			try
			{
				inst.FRequestItem = new NewPaymentPreviewAcceptPreviewRequestItem();
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
				Result = FRequestItem.Load((ActionBase)this, DataObject);
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
			//string ErrorMsg = "";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " +Method, FModuleName, MasterId);
			LogOnInsideMethod(Method);
			try 
			{
				Result = UsefulDBMethods.Helpers.RunEAMainOfficeProcess(FbConnectionString, CommonItem.CompanyIdValue);
				if (!string.IsNullOrEmpty(Result)) return Result;
				
				/*
				 * Call EAStatementPrepareForPayment
				*/
				Result = EAStatementHideCreatePreview();
				if (string.IsNullOrEmpty(Result))
				{
					Result = RunEAStatementPrepareForPayment();
				}
				if (string.IsNullOrEmpty(Result))
				{
					Result = RunEAStatementProcessList();
				}
				
				if (!string.IsNullOrEmpty(Result)) return Result;
				
				Result = LogResponse(Method, "");
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
		
		/*		
		private string BuildOutput(out string ErrorMsg)
		{
			ErrorMsg = "";
			string Result = "";
			string Method = "BuildOutput";
			
			LogOnInsideMethod(Method);
			try
			{
				WindowsEventLoggingUtilities.WriteToInfoEvent("Inside BuildOutput", FModuleName, MasterId);
				
				StringBuilder entries = new StringBuilder();
				entries.AppendLine(JsonUtilities.StartJson());
					
				ErrorMsg = BuildOutputEntries(ref entries);				
				if (!string.IsNullOrEmpty(ErrorMsg)) return "";
				
				entries.AppendLine(JsonUtilities.EndJson());
				Result = entries.ToString();
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
		
		private string EAStatementHideCreatePreview()
		{
			string Result = "";
			string Method = "EAStatementHideCreatePreview";
			
			LogOnInsideMethod(Method);
			try
			{
				string QueryText = "EXECUTE PROCEDURE EAStatementHideCreatePreview(@CompanyClientId, @RunId, @Hide)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyClientId", CommonItem.CompanyIdValue));
				Params.Add(new FbParameterItem("RunId", FRequestItem.RunId));
				Params.Add(new FbParameterItem("Hide", "Y"));
				UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
				Result = FirebirdUtilities.DBExecute(FbConnectionString, QueryText, Params, 0);
			}
			catch (Exception ex) {
				Result = ex.Message + Environment.NewLine + ex.InnerException;
			}
			finally
			{
				LogOnLeavingMethod(Method);
			}
			return Result;
		}
		
		private string RunEAStatementPrepareForPayment()
		{
			string Result = "";
			string Method = "RunEAStatementPrepareForPayment";
			
			LogOnInsideMethod(Method);
			try
			{
				string QueryText = "EXECUTE PROCEDURE EAStatementPrepareForPayment(@CompanyClientId, @RunId)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyClientId", CommonItem.CompanyIdValue));
				Params.Add(new FbParameterItem("RunId", FRequestItem.RunId));
				UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
				Result = FirebirdUtilities.DBExecute(FbConnectionString, QueryText, Params, 0);
			}
			catch (Exception ex) {
				Result = ex.Message + Environment.NewLine + ex.InnerException;
			}
			finally
			{
				LogOnLeavingMethod(Method);
			}
			return Result;
		}
		
		private string RunEAStatementProcessList()
		{
			string Result = "";
			string Method = "RunEAStatementProcessList";
			
			LogOnInsideMethod(Method);
			try
			{
				string QueryText = "EXECUTE PROCEDURE EAStatementProcessList(@CompanyClientId, @RunId)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyClientId", CommonItem.CompanyIdValue));
				Params.Add(new FbParameterItem("RunId", FRequestItem.RunId));
				UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
				Result = FirebirdUtilities.DBExecute(FbConnectionString, QueryText, Params, 0);
			}
			catch (Exception ex) {
				Result = ex.Message + Environment.NewLine + ex.InnerException;
			}
			finally
			{
				LogOnLeavingMethod(Method);
			}
			return Result;
		}
		
		/*		
		private string RunEAXFilesWhatFieldIsNeeded(int ReportId)
		{
			string Result = "";
			string Method = "RunEAXFilesWhatFieldIsNeeded";
			
			LogOnInsideMethod(Method);
			try
			{
				string QueryText = "SELECT * FROM EASFilesWhatFieldIsNeeded(@ReportId)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("ReportId", ReportId));
				UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
				DataTable dt = 
					UsefulDBMethods.Helpers.GetFirstDataTableEx(
						FbConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
				if (!string.IsNullOrEmpty(Result)) return Result;
				// What do we need to do with the results of this query 
			}
			catch (Exception ex) {
				Result = ex.Message + Environment.NewLine + ex.InnerException;
			}
			finally
			{
				LogOnLeavingMethod(Method);
			}
			return Result;
		}
		
		private string RunEAXFilesReportDataAdd()
		{
			string Result = "";
			string Method = "RunEAXFilesReportDataAdd";
			
			LogOnInsideMethod(Method);
			try
			{
				string QueryText = 
					" EXECUTE PROCEDURE EAXFilesReportDataAdd(" +
					" )";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				//Params.Add(new FbParameterItem("ReportId", ReportId));
				UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
				Result = FirebirdUtilities.DBExecute(FbConnectionString, QueryText, Params, 0);
			}
			catch (Exception ex) {
				Result = ex.Message + Environment.NewLine + ex.InnerException;
			}
			finally
			{
				LogOnLeavingMethod(Method);
			}
			
			return Result;
		}
		
		private string RunEAXFilesReportSql()
		{
			string Result = "";
			string Method = "RunEAXFilesReportSql";
			
			LogOnInsideMethod(Method);
			try
			{
				string QueryText = 
					" EXECUTE PROCEDURE EAXFilesReportDataAdd(" +
					" )";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				//Params.Add(new FbParameterItem("ReportId", ReportId));
				UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
				Result = FirebirdUtilities.DBExecute(FbConnectionString, QueryText, Params, 0);
			}
			catch (Exception ex) {
				Result = ex.Message + Environment.NewLine + ex.InnerException;
			}
			finally
			{
				LogOnLeavingMethod(Method);
			}
			
			return Result;
		}
		
		private string GetEAEventRunReportDataSqlStmt(Int64 EventRunId, out string ErrorMsg)
		{
			string Result = "";
			ErrorMsg = "";
			string Method = "GetEAEventRunReportDataSqlStmt";
			
			LogOnInsideMethod(Method);
			try
			{
				string QueryText = "SELECT EVA_SQLSTATEMENT FROM EAEVENTRUNREPORTDATA WHERE EVA_ID=@EVENTRUNID";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("EventRunId", EventRunId));
				UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
				DataTable tbl = 
					UsefulDBMethods.Helpers.GetFirstDataTableEx(
						FbConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTablesAndRows, out ErrorMsg);
				if (!string.IsNullOrEmpty(ErrorMsg)) return "";
				
				Result = "";
				foreach (DataRow CurrentRow in tbl.Rows)
				{
					if (!string.IsNullOrEmpty(Result)) break;
					object Obj = CurrentRow["EVA_SQLSTATEMENT"];
					Result = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
				}
				if (string.IsNullOrEmpty(Result))
					ErrorMsg = "The EAEventRunReportData Sql Stmt is empty";
			}
			catch (Exception ex) {
				Result = ex.Message + Environment.NewLine + ex.InnerException;
			}
			finally
			{
				LogOnLeavingMethod(Method);
			}

			return Result;
		}
		
		private string ExecuteEventRunCreate(string SqlText,
		                                    int EventRunId)
		{
			string Result = "";
			string Method = "ExecuteEventRunCreate";
			
			LogOnInsideMethod(Method);
			try
			{
				// Where do I get the values required for the ExecutEventRunCreate method.
				// Also, the EAXFILESREPORTDATAADD method contains values that are not used in the ExecuteEventRunCreate method **********
				Result =
					UsefulDBMethods.EventHelpers.ExecuteEventRunCreate(
						FbConnectionString, EventRunId, CommonItem.CompanyIdValue,
				// 
			}
			catch (Exception ex) {
				Result = ex.Message + Environment.NewLine + ex.InnerException;
			}
			finally
			{
				LogOnLeavingMethod(Method);
			}
			
			return Result;
		}
		
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText:" + Environment.NewLine + QueryText, FModuleName, MasterId);
				
				foreach (FbParameterItem Param in Params)
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent(
						Param.ParameterName + ": " + Convert.ToString(Param.ParameterValue), FModuleName, MasterId);
				}

				DataTable dt = null;
				try 
				{
					dt = FirebirdUtilities.GetFirstDataTable(FbConnectionString, QueryText, Params, 0);
					if (dt == null)
					{
						Result = "No tables in the dataset to process";
						Result = LoggingEntry(Method, Result, "");
						return Result;
					}
				}					
				catch (Exception Ex)
				{
					Result = LogOnException(Method, Ex, "");
					return Result;
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
