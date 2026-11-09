/*
 * Created by SharpDevelop.
 * User: John
 * Date: 02/07/2019
 * Time: 6:46 PM
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
	/// Description of GetPaymentSearchGridResults.
	/// </summary>
	public class GetPaymentSearchGridResults : ActionBase
	{
		private string FModuleName = "PaymentOperations";		
		private PaymentSearchGridRequestItem FRequestItem = new PaymentSearchGridRequestItem();
		
		public static string Execute(int MasterId,
		                             RequestCommonItem CommonItem,
		                             JObject DataObject,
		                             APISettingsItem SettingsItem,
		                             out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			GetPaymentSearchGridResults inst = new GetPaymentSearchGridResults();
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
				Result = FRequestItem.Load(DataObject);
				LogOnInsideMethod("Init RunId: " + FRequestItem.RunIdValue);
				WindowsEventLoggingUtilities.WriteToInfoEvent("RunId: " + FRequestItem.RunIdValue, FModuleName, MasterId);
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
				StatementRunItem RunItem = new StatementRunItem();
				ErrorMsg = RunItem.Load((EAOfficeAPI.ActionBase)this, FbConnectionString, CommonItem.CompanyIdValue, FRequestItem.RunId);
				if (!string.IsNullOrEmpty(ErrorMsg))
				{
					Result = LogOnProcessingError(Method, ErrorMsg, "");
					return Result;
				}
				
				Result = BuildOutput(RunItem, out ErrorMsg);
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
		
		private string BuildOutput(StatementRunItem StmtRunItem, out string ErrorMsg)
		{
			ErrorMsg = "";
			string Result = "";
			string Method = "BuildOutput";
			
			LogOnInsideMethod(Method);
			try
			{
				WindowsEventLoggingUtilities.WriteToInfoEvent("Inside BuildOutput", FModuleName, MasterId);
				/******************************************************************************************
				 * 
				 *           Move this processing to CommonMethods.BuildPaymentSearchGridResults          *
				 * 
				******************************************************************************************/
				StringBuilder Entries = new StringBuilder();
				Entries.AppendLine(JsonUtilities.StartJson());
				
				ErrorMsg =
					CommonMethods.BuildPaymentSearchGridResults(
						(ActionBase)this, FbConnectionString, CommonItem.CompanyIdValue, StmtRunItem, 
						"STATEMENT-REPORT", "STATEMENTS-MANAGEMENTMANAGER", ref Entries);
				
				Entries.AppendLine(JsonUtilities.EndJson());
				if (string.IsNullOrEmpty(ErrorMsg)) Result = Entries.ToString();
				
				/*
				string QueryText =
					" SELECT" +
					"   i_summary_id as I_Summary_ID," +
					"   ZC10_Date AS D_Date," +
					"   (select OUTPUTVALUE from formatcurrencyforlist(ZR10_Payment)) as ZR10_Payment," +
					"   ZL20_Client," +
					"   ZL20_Property as ZL30_Property," +
					"   ZC8_Direct," +
					"   I_Reference," +
					"   ZL20_BANK_NAME as ZL20_Payment_Details," +
					"   ZL30_EMAIL_TO as ZL30_eMailed_To," +
					"   ZL30_EMAIL_COMMENTS," +
					"   ZL20_LEDGER_TYPE as ZC15_Ledger_Type," +
					"   ZC8_ARCHIVED," + 
					"   I_LEDGER_ID," +
					"   I_SUB_LEDGER_ID," +
					"   I_Client_ID," +
					"   ZL20_FROM_BANK," +
					"   I_Bank_ID," +
					"   I_Invoice_ID," +
					"	ZC8_CONVERSION," +
					"	ZC8_UNASSIGNED," +
					//"  ManagerId," +
					//"  BranchId," +
					"   ZL20_Sub_Ledger" +
					" FROM EAPAYMENTLIST (1, @RunId, @CompanyId, 0, 0, 0, 0, 0, 0, 0)";
				
				List<FbParameterItem> Params = new List<FbParameterItem>();
				//Params.Add(new FbParameterItem("RunId", FRequestItem.RunId));
				Params.Add(new FbParameterItem("RunId", StmtRunItem.RunIdValue));
				Params.Add(new FbParameterItem("CompanyId", CommonItem.CompanyId));
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


				StringBuilder Entries = new StringBuilder();
				Entries.AppendLine(JsonUtilities.StartJson());
				
				Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("RecordId", StmtRunItem.IdValue));
				Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("RunType", StmtRunItem.RunType));
				Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("RunDate", StmtRunItem.RunDate));
				Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("Amount", StmtRunItem.Amount.ToString()));
				Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("Entries", StmtRunItem.Entries.ToString()));
				Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("CreatedBy", StmtRunItem.CreatedBy));
				Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("StatementPreview", StmtRunItem.StatementPreview));
				
				Int64 StartingChqNo = 
					Actions.PaymentOperations.CommonMethods.EAStatementNextChqNoForRun(
						FbConnectionString, CommonItem.CompanyIdValue, StmtRunItem.RunId, out ErrorMsg);
				if (string.IsNullOrEmpty(ErrorMsg) && (StringUtilities.SameText(StmtRunItem.StatementPreview, "Y")))
				{
					Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("StartingChqNo", StartingChqNo));
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg = CommonMethods.BuildJsonArrayFromDataTable(dt, "Items", ref Entries);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg) && (StringUtilities.SameText(StmtRunItem.StatementPreview, "Y")))
				{
					ErrorMsg = BuildPreviewReportsList(CommonItem.CompanyIdValue, StmtRunItem.RunId, ref Entries);
				}				
					
				Entries.AppendLine(JsonUtilities.EndJson());
				Result = Entries.ToString();
				*/
			}
			catch (Exception Ex)
			{
				ErrorMsg = LogOnException(Method, Ex, "");
				Result = "";
			}
			finally
			{
				LogOnLeavingMethod(Method);
			}
			
			return Result;
		}
		
		private string BuildPreviewReportsList(Int64 CompanyId,
		                                       Int64 RunId,
		                                       ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "BuildPreviewReportsList";
			
			LogOnInsideMethod(Method);
			try
			{
				WindowsEventLoggingUtilities.WriteToInfoEvent("Inside BuildOutput", FModuleName, MasterId);
				
				string QueryText =
					" SELECT ReportName, ReportId, DefaultValue" +
					" FROM EAStatementGetPreviewRep(@CompanyId, @RunId)";				
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("RunId", RunId));
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText:" + Environment.NewLine + QueryText, FModuleName, MasterId);
				
				foreach (FbParameterItem Param in Params)
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent(
						Param.ParameterName + ": " + Convert.ToString(Param.ParameterValue), FModuleName, MasterId);
				}

				DataTable dt = 
					UsefulDBMethods.Helpers.GetFirstDataTableEx(
						FbConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
				if (!string.IsNullOrEmpty(Result)) return Result;
				
				string Str = "";
				string ColValue = "";
				List<string> RowList = new List<string>();
				List<string> StrList = new List<string>();
				object Obj = null;
				StringBuilder Reports = new StringBuilder();

				Reports.AppendLine(JsonUtilities.StartJsonArray("previewReports"));
				foreach (DataRow ThisRow in dt.Rows)
				{
					StrList.Clear();
					Obj = ThisRow["ReportName"];
					ColValue = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					StrList.Add(JsonUtilities.AddPropertyNameAndValue("Name", ColValue, false));
					Obj = ThisRow["ReportId"];
					ColValue = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					StrList.Add(JsonUtilities.AddPropertyNameAndValue("Value", ColValue, false));
					Obj = ThisRow["DefaultValue"];
					ColValue = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					if (!string.IsNullOrEmpty(ColValue))
						StrList.Add(JsonUtilities.AddPropertyNameAndValue("Default", "1", false));
					Str =
						JsonUtilities.StartJsonObject("") +
						string.Join(",", StrList.ToArray()) +
						JsonUtilities.EndJsonObject();
					RowList.Add(Str);
				}
				Reports.Append(string.Join(",", RowList.ToArray()));
				Reports.AppendLine(JsonUtilities.EndJsonArray());
				
				if (Entries.Length > 0) Entries.AppendLine(",");
				Entries.Append(Reports.ToString());
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
	}
}
