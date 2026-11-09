/*
 * Created by SharpDevelop.
 * User: John
 * Date: 09/07/2019
 * Time: 5:10 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Data;
using System.Runtime.Hosting;
using System.Text;
using System.Collections.Generic;
using System.Collections.Specialized;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CommonUtilities;
using EAOfficeAPI;

namespace Actions.PaymentOperations
{
	/// <summary>
	/// Description of CommonMethods.
	/// </summary>
	public class CommonMethods
	{
		private string FModuleName = "PaymentOperations.CommonMethods";
		
		public CommonMethods()
		{
		}

        public static string GetModuleName() { return "PaymentOperations."; }

        /// <summary>
        /// BuildJsonArrayFromDataTable is used to generate JSON entries
        /// that can be used in the web page for grid displays
        /// </summary>
        /// <param name="tblSource">A DataTable containing the values to generate the content from.
        /// This table can be empty</param>
        /// <param name="ArrayName">The name that will be used for the JSON array of values </param>
        /// <param name="Entries">The StringBuilder that will be used to contain the generated content</param>
        /// <returns></returns>
        public static string BuildJsonArrayFromDataTable(DataTable tblSource,
		                                                 string ArrayName,
		                                                 ref StringBuilder Entries)
		{
			string Result = "";
			if (tblSource == null)
			{
				Result = "Source Table is NULL";
			}
			
			if (!string.IsNullOrEmpty(Result)) return Result;			
			
			string Str = "";
			string ColName = "";
			string ColValue = "";
			List<string> RowList = new List<string>();
			List<string> StrList = new List<string>();
			object Obj = null;
			try 
			{
				Entries.AppendLine(JsonUtilities.StartJsonArray(ArrayName));
				foreach (DataRow ThisRow in tblSource.Rows)
				{
					StrList.Clear();
					foreach (DataColumn ThisCol in tblSource.Columns)
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
				Result =
					Ex.Message + Environment.NewLine +
					Ex.InnerException + Environment.NewLine +
					Ex.StackTrace;
			}
			
			return Result;
		}
		
		public string BuildJsonArrayFromDataTable(DataTable Source, 
		                                          string ArrayName,
		                                          ref List<string> Entries)
		{
			string Result = "Not Implemented";
			return Result;
		}
		
        public static string LoadStatementRunItem(string ConnectionString,
		                                         Int64 CompanyId, 
		                                         Int64 CloseAccountRunId,
		                                         int MasterId,
		                                         ref StatementRunItem RunItem)
        {
            string Result = "";

            CommonMethods Inst = new CommonMethods();
            try
            {
                string QueryText =
                        "SELECT * from EASTATEMENTRUNVALUES(@CompanyId, @CloseAccountRunId)";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                Params.Add(new FbParameterItem("CompanyId", CompanyId));
                Params.Add(new FbParameterItem("CloseAccountRunId", CloseAccountRunId));
                WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText:" + Environment.NewLine + QueryText, Inst.FModuleName, MasterId);
				
                foreach (FbParameterItem Param in Params)
                {
                    WindowsEventLoggingUtilities.WriteToInfoEvent(
                        Param.ParameterName + ": " + Convert.ToString(Param.ParameterValue), Inst.FModuleName, MasterId);
                }
	
                DataTable dt = null;
                try 
                {
                    dt = FirebirdUtilities.GetFirstDataTable(ConnectionString, QueryText, Params, 0);
                    if (dt == null)
                    {
                        Result = "No tables in the dataset to process";
                        return Result;
                    }
                }					
                catch (Exception Ex)
                {
                    Result = 
                    	Ex.Message + Environment.NewLine +
                    	Ex.InnerException + Environment.NewLine +
                    	Ex.StackTrace;
                    return Result;
                }
				
                object Obj = null;
                RunItem.Clear();
                foreach (DataRow Row in dt.Rows)
                {
                    RunItem.RunIdValue = Convert.ToString(CloseAccountRunId);
                    Obj = Row["ASTR_Type"];
                    RunItem.RunType = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                    Obj = Row["ASTR_DirectEntryDiskCreated"];
                    RunItem.DirectEntryDiskCreated = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                    Obj = Row["B_ASTR_MultiDirectRun"];
                    RunItem.MultiDirectRun = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                    Obj = Row["B_ASTR_BPA"];
                    RunItem.BPA = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                    Obj = Row["I_ASTR_PaymentEventId"];
                    RunItem.PaymentEventIdValue = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                    Obj = Row["B_ASTR_StatementPreview"];
                    RunItem.StatementPreview = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                    Obj = Row["I_ASTR_AccountType"];
                    RunItem.AccountTypeValue = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                    Obj = Row["D_ASTR_Date"];
                    RunItem.RunDate = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                    Obj = Row["M_ASTR_Amount"];
                    RunItem.AmountValue = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                    Obj = Row["I_ASTR_Entries"];
                    RunItem.EntriesValue = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                    Obj = Row["ASTR_PaymentFileStatus"];
                    RunItem.PaymentFileStatus = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                    Obj = Row["I_ASTATE_Id"];
                    RunItem.IdValue = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                    Obj = Row["ASTR_WHOBY"];
                    RunItem.CreatedBy = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                    Obj = Row["B_ASTR_FROMCONVERSION"];
                    RunItem.FromConversion = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                    Obj = Row["B_ASTR_EventRunCreated"];
                    RunItem.EventRunCreated = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                    Obj = Row["D_FILE_CreatedOn"];
                    RunItem.FileCreatedOn = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                    Obj = Row["File_CreatedBy"];
                    RunItem.FileCreatedBy = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                    Obj = Row["ShowPost"];
                    RunItem.ShowPost = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                    Obj = Row["ShowReverse"];
                    RunItem.ShowReverse = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                    Obj = Row["ShowPresent"];
                    RunItem.ShowPresent = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                    Obj = Row["ShowChequeNumbers"];
                    RunItem.ShowChequeNumbers = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                }
                Result = RunItem.Validate();
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
		
        public static string LoadNewPaymentPreviewApplyRequestItem(JObject DataObject, 
		                                                          ref NewPaymentPreviewApplyRequestItem TempItem)
        {
            string Result = "";
            string Method = "LoadNewPaymentRunApplyRequestItem";
            try 
            {
                TempItem.RunIdValue = (string)DataObject["runId"];
                TempItem.BankIdValue = (string)DataObject["bankId"];
                TempItem.AccountTypeIdValue = (string)DataObject["accountTypeId"];
                TempItem.BranchIdValue = (string)DataObject["branchId"];
                TempItem.ManagerIdValue = (string)DataObject["managerId"];
                TempItem.OrderIdValue = (string)DataObject["orderId"];
                TempItem.MidMonth = (string)DataObject["midMonth"];
                TempItem.AccountIdValue = (string)DataObject["accountId"];
                TempItem.RunNumberIdValue = (string)DataObject["runNumberId"];
                TempItem.AllIsSelected = (string)DataObject["allIsSelected"];
                TempItem.Preview = (string)DataObject["preview"];
                TempItem.MultiDirectRun = (string)DataObject["multiDirectRun"];
                TempItem.DirectEntryUserNumberValue = (string)DataObject["directEntryUserNumber"];
                TempItem.BPay = (string)DataObject["bPay"];
                TempItem.WhoBy = (string)DataObject["whoBy"];
                TempItem.StatementDate = (string)DataObject["statementDate"];
                TempItem.BPayBankUserIdValue = (string)DataObject["bPayBankUserId"];
                TempItem.BPayMinimumValue = (string)DataObject["bPayMinimumValue"];
                TempItem.PaymentEventIdValue = (string)DataObject["paymentEventId"];
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
		
		public static string BuildNewPaymentPreviewGridResults(ActionBase AActionBase,
		                                                       string ConnectionString,
		                                                       Int64 CompanyId,
		                                                       //Int64 ClientId,
		                                                       Int64 RunId,
		                                                       string JsonArrayName,
		                                                       ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "BuildNewPaymentPreviewGridResults";
			CommonMethods inst = new CommonMethods();
			
			string QueryText = StatementRunGetSQL(AActionBase, ConnectionString, CompanyId, RunId, out Result);
			if (!string.IsNullOrEmpty(Result)) return Result;
			if (string.IsNullOrEmpty(QueryText))
			{
				Result = "Empty or invalid QueryText returned";
				return Result;
			}
			
			WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText " + QueryText, inst.FModuleName, 0);
			DataTable dt = FirebirdUtilities.GetFirstDataTable(ConnectionString, QueryText);
			
			try 
			{
				if ((dt != null) && (dt.Rows != null) && (dt.Rows.Count > 0))
				{
					CommonMethods.BuildJsonArrayFromDataTable(dt, JsonArrayName, ref Entries);
				}
			}					
			catch (Exception Ex)
			{
				Result = AActionBase.LogOnException(Method, Ex, "");
				return Result;
			}
			return Result;
		}
		
		public static string StatementRunGetSQL(ActionBase AActionBase,
		                                        string ConnectionString,
		                                        Int64 CompanyClientId,
		                                        Int64 RunId,
		                                        out string ErrorMsg)
		{
			string Result = "";
			string Method = "StatementRunGetSQL";
			ErrorMsg = "";
			CommonMethods inst = new CommonMethods();
			
			AActionBase.LogOnInsideMethod(Method);
			try
			{
				string QueryText = "SELECT OUTPUTSQL FROM EASTATEMENTRUNGETSQL(@COMPANYCLIENTID, @RUNID)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyClientId", CompanyClientId));
				Params.Add(new FbParameterItem("RunId", RunId));
				UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, inst.FModuleName, 0);
				DataTable dt = 
					UsefulDBMethods.Helpers.GetFirstDataTableEx(
						ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out ErrorMsg);
				if (!string.IsNullOrEmpty(ErrorMsg)) return Result;
				
				object Obj = null;
				string Str = "";
				foreach (DataRow Row in dt.Rows)
				{
					Obj = Row["OutputSql"];
					Str = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					if (!string.IsNullOrEmpty(Str)) Result = Str;
				}
			}
			catch (Exception Ex)
			{
				ErrorMsg = 
					Method + Environment.NewLine +
					Ex.Message + Environment.NewLine +
					Ex.StackTrace;					
			}
			finally
			{
				AActionBase.LogOnLeavingMethod(Method);
			}
			
			return Result;
		}
		
		public static string LoadNewPaymentPreviewGatherRequestItem(JObject DataObject, 
		                                                           ref NewPaymentPreviewGatherRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadNewPaymentPreviewGatherRequestItem";
			try 
			{
				TempItem.RunIdValue = (string)DataObject["runId"];
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
		
		public static string LoadNewPaymentPreviewAcceptPreviewRequestItem(JObject DataObject,
		                                                                   ref NewPaymentPreviewAcceptPreviewRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadNewPaymentPreviewAcceptPreviewRequestItem";
			try 
			{
				TempItem.RunIdValue = (string)DataObject["runId"];
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
		
		public static string LoadNewPaymentPreviewStartAgainRequestItem(JObject DataObject,
		                                                                ref NewPaymentPreviewStartAgainRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadNewPaymentPreviewStartAgainRequestItem";
			try 
			{
				TempItem.RunIdValue = (string)DataObject["runId"];
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
		
		public static string LoadNewPaymentPreviewRemoveEntryRequestItem(JObject DataObject,
		                                                                 ref NewPaymentPreviewRemoveEntryRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadNewPaymentPreviewRemoveEntryRequestItem";
			try 
			{
				TempItem.IdValue = (string)DataObject["id"];
				TempItem.RunIdValue = (string)DataObject["runId"];
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
		
		public static string LoadStatementNextChqNumberRequestItem(JObject DataObject,
		                                                           ref StatementNextChqNumberRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadStatementNextChqNumberRequestItem";
			try 
			{
				TempItem.RunIdValue = (string)DataObject["runId"];
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
		
		public static Int64 EAStatementNextChqNumber(string ConnectionString,
		                                             Int64 CompanyClientId,
		                                             Int64 BankNameId,
		                                             out string ErrorMsg)
		{
			Int64 Result = 0;
			ErrorMsg = "";
			
			CommonMethods inst = new CommonMethods();
			string QueryText = "SELECT NextChequeNumber FROM EAStatementNextChqNumber(@CompanyClientId, @BankNameId)";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("CompanyClientId", CompanyClientId));
			Params.Add(new FbParameterItem("BankNameId", BankNameId));
			ErrorMsg = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, inst.FModuleName, 0);
			DataTable tbl = null;
			object Obj = null;
			if (string.IsNullOrEmpty(ErrorMsg))
			{
				tbl = 
					UsefulDBMethods.Helpers.GetFirstDataTableEx(
						ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTablesAndRows, out ErrorMsg);
			}
			if (string.IsNullOrEmpty(ErrorMsg))
			{
				Result = 0;
				foreach (DataRow Row in tbl.Rows)
				{
					Obj = Row["NextChequeNumber"];
					Result = (Obj == DBNull.Value) ? 0 : Convert.ToInt64(Obj);
					if (Result > 0) break;
				}
			}			
			
			return Result;
		}
		
		public static Int64 EAStatementNextChqNoForRun(string ConnectionString,
		                                               Int64 CompanyClientId,
		                                               Int64 RunId,
		                                               out string ErrorMsg)
		{
			Int64 Result = 0;
			ErrorMsg = "";
			
			CommonMethods inst = new CommonMethods();
			string QueryText = "SELECT NextChqNo FROM EAStatementNextChqNoFOrRun(@CompanyClientId, @SummaryId)";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("CompanyClientId", CompanyClientId));
			Params.Add(new FbParameterItem("SummaryId", RunId));
			ErrorMsg = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, inst.FModuleName, 0);
			DataTable tbl = null;
			object Obj = null;
			if (string.IsNullOrEmpty(ErrorMsg))
			{
				tbl = 
					UsefulDBMethods.Helpers.GetFirstDataTableEx(
						ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTablesAndRows, out ErrorMsg);
			}
			if (string.IsNullOrEmpty(ErrorMsg))
			{
				Result = 0;
				foreach (DataRow Row in tbl.Rows)
				{
					Obj = Row["NextChqNo"];
					Result = (Obj == DBNull.Value) ? 0 : Convert.ToInt64(Obj);
					if (Result > 0) break;
				}
			}			
			
			return Result;
		}
		
		public static string LoadStatementCancelPreviewRequestItem(JObject DataObject,
		                                                           ref StatementCancelPreviewRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadStatementCancelPreviewRequestItem";
			try 
			{
				TempItem.RunIdValue = (string)DataObject["runId"];
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
		
		public static string LoadReverseRunEntryRequestItem(JObject DataObject,
		                                                    ref ReverseRunEntryRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadReverseRunEntryRequestItem";
			try 
			{
				TempItem.RunIdValue = (string)DataObject["runId"];
				TempItem.SummaryIdValue = (string)DataObject["summaryId"];
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
		
		public static string BuildPaymentSearchGridResults(ActionBase AActionBase,
		                                                   string ConnectionString,
		                                                   Int64 CompanyClientId,
		                                                   StatementRunItem StmtRunItem,
		                                                   string NonPreviewReportSource,
		                                                   string ManagementReportSource,
		                                                   ref StringBuilder Entries)
		{
            string Result = "";
            string Method = "BuildPaymentSearchGridResults";
            CommonMethods inst = new CommonMethods();
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
                "   I_SUB_LEDGER_ID AS I_SUB_ID," +
                "   I_Client_ID," +
                "   ZL20_FROM_BANK," +
                "   I_Bank_ID," +
                "   I_Invoice_ID," +
                "	ZC8_CONVERSION," +
                "	ZC8_UNASSIGNED," +
                //"  ManagerId," +
                //"  BranchId," +
                "   ZL20_Sub_Ledger," +
                "   ZR8_Statement_Id," +
                "   ZL3_B1," +
                "   ZL3_B2," +
                "   ZL3_B3" +
                "   , ShowEdit" +
                "   , ShowTrash" +
                " FROM EAPAYMENTLIST (1, @RunId, @CompanyId, 0, 0, 0, 0, 0, 0, 0)" +
                " ORDER BY runid desc, ALE_CODE, i_ledger_id, zl20_sub_ledger, i_sub_ledger_id";


            List<FbParameterItem> Params = new List<FbParameterItem>();
			
			AActionBase.LogOnInsideMethod(Method);
			try
			{
                Params.Add(new FbParameterItem("RunId", StmtRunItem.RunIdValue));
                Params.Add(new FbParameterItem("CompanyId", CompanyClientId));
                Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, inst.FModuleName, 0);
                if (string.IsNullOrEmpty(Result))
                {
                    DataTable dt = 
                        UsefulDBMethods.Helpers.GetFirstDataTableEx(
                            ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
					
                    //Entries.AppendLine(JsonUtilities.StartJson());

                    Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("RecordId", StmtRunItem.IdValue));
                    Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("RunType", StmtRunItem.RunType));
                    Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("RunDate", StmtRunItem.RunDate));
                    Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("Amount", StmtRunItem.Amount.ToString()));
                    Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("Entries", StmtRunItem.Entries.ToString()));
                    Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("CreatedBy", StmtRunItem.CreatedBy));
                    Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("StatementPreview", StmtRunItem.StatementPreview));
                    Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("FromConversion", StmtRunItem.FromConversion));
                    Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("AccountType", StmtRunItem.AccountType));
                    Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("EventRunCreated", StmtRunItem.EventRunCreated));
                    Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("FileCreatedOn", StmtRunItem.FileCreatedOn));
                    Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("FileCreatedBy", StmtRunItem.FileCreatedBy));
                    Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("DEDiskCreated", StmtRunItem.DirectEntryDiskCreated));
                    Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("ShowPost", StmtRunItem.ShowPost));
                    Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("ShowReverse", StmtRunItem.ShowReverse));
                    Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("ShowPresent", StmtRunItem.ShowPresent));
                    Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("ShowChequeNumbers", StmtRunItem.ShowChequeNumbers));

                    Int64 StartingChqNo = 
                            Actions.PaymentOperations.CommonMethods.EAStatementNextChqNoForRun(
							ConnectionString, CompanyClientId, StmtRunItem.RunId, out Result);

                    if (string.IsNullOrEmpty(Result) && (StringUtilities.SameText(StmtRunItem.StatementPreview, "Y")))
                    {
                    	Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("StartingChqNo", StartingChqNo));
                    }
					
                    if (string.IsNullOrEmpty(Result))
                    {
                    	Result = CommonMethods.BuildJsonArrayFromDataTable(dt, "Items", ref Entries);
                    }
					
                    if (string.IsNullOrEmpty(Result) && (StringUtilities.SameText(StmtRunItem.StatementPreview, "Y")))
                    {
                    	AActionBase.LoggingEntry(Method, "Before BuildStatementGetPreviewRep", "");
                    	Result = inst.BuildStatementGetPreviewRep(AActionBase, ConnectionString, CompanyClientId, StmtRunItem.RunId, ref Entries);
                    	AActionBase.LoggingEntry(Method, "After BuildStatementGetPreviewRep", "");
                    	AActionBase.LoggingEntry(Method, "Result: " + Result, "");
                    }				
                    else if (string.IsNullOrEmpty(Result) && (StringUtilities.SameText(StmtRunItem.StatementPreview, "N")))
                    {
                    	AActionBase.LoggingEntry(Method, "Before retrieving report list", "");
                    	//Result = inst.BuildStatementGetPreviewRep(AActionBase, ConnectionString, CompanyClientId, StmtRunItem.RunId, ref Entries);
                    	string JsonList = 
                    		new global::UsefulDBMethods.Reporting.ReportUtilities().EAReportGetDefaultJsonList(
                    			ConnectionString, (int)CompanyClientId, NonPreviewReportSource, StmtRunItem.RunId, "reports", out Result);
                    	AActionBase.LoggingEntry(Method, "After retrieving report list", "");
                    	AActionBase.LoggingEntry(Method, "Result: " + Result, "");
                    	AActionBase.LoggingEntry(Method, "JsonList: " + JsonList, "");
                    	if (string.IsNullOrEmpty(Result))
                    	{
                    		if (Entries.Length > 0) Entries.AppendLine(",");
                    		Entries.Append(JsonList);
                    	}
                    	
                    	if (StmtRunItem.AccountType == 39)
                    	{
                    		AActionBase.LoggingEntry(Method, "Before retrieving management report list", "");
                    		JsonList =
                    			new global::UsefulDBMethods.Reporting.ReportUtilities().EAReportGetDefaultJsonList(
                    				ConnectionString, (int)CompanyClientId, ManagementReportSource, StmtRunItem.RunId, "mgmtReports", out Result);
                    		AActionBase.LoggingEntry(Method, "Before retrieving management report list", "");
                    		AActionBase.LoggingEntry(Method, "Result: " + Result, "");
                    		AActionBase.LoggingEntry(Method, "JsonList: " + JsonList, "");
                    		if (string.IsNullOrEmpty(Result))
                    		{
                    			if (Entries.Length > 0) Entries.AppendLine(",");
                    			Entries.Append(JsonList);
                    		}
                    	}
                    }
				
					//Entries.AppendLine(JsonUtilities.EndJson());
				}
			}
			catch (Exception Ex)
			{
				Result = AActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				AActionBase.LogOnLeavingMethod(Method);
			}

			return Result;
		}
		                                                   
		
		private string BuildStatementGetPreviewRep(ActionBase AActionBase,
		                                           string ConnectionString,
		                                           Int64 CompanyId,
		                                           Int64 RunId,
		                                           ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "BuildStatementGetPreviewRep";
			CommonMethods inst = new CommonMethods();
			
			AActionBase.LogOnInsideMethod(Method);
			try
			{
				WindowsEventLoggingUtilities.WriteToInfoEvent("Inside BuildOutput", inst.FModuleName, 0);
				
				string QueryText =
					" SELECT ReportName, ReportId, DefaultValue" +
					" FROM EAStatementGetPreviewRep(@CompanyId, @RunId)";				
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("RunId", RunId));
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, inst.FModuleName, 0);

				DataTable dt = 
					UsefulDBMethods.Helpers.GetFirstDataTableEx(
						ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
				if (string.IsNullOrEmpty(Result))
				{				
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
			}
			catch (Exception Ex)
			{
				Result = AActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				AActionBase.LogOnLeavingMethod(Method);
			}
			
			return Result;
		}
		
		public static string LoadPostPreviewRequestItem(JObject DataObject,
		                                                 ref PostPreviewRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadPostPreviewRequestItem";
			try 
			{
				TempItem.RunIdValue = (string)DataObject["runId"];
                TempItem.ChqNumberValue = (string)DataObject["ChqNumber"];
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

		public static string LoadPaymentAttachmentListRequestItem(JObject DataObject,
		                                                          ref PaymentAttachmentListRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadPaymentAttachmentListRequestItem";
			try 
			{
				TempItem.SummaryIdValue = (string)DataObject["summaryId"];
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
		
		public static string LoadPaymentListReportRequestItem(ActionBase AActionBase, 
		                                                      ref PaymentListReportRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadPaymentListReportRequestItem";
			object Obj = null;
			try 
			{
				Obj = AActionBase.GetCollectionValue("Data", "runId");
				TempItem.RunIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = AActionBase.GetCollectionValue("Data", "reportId");
				TempItem.ReportIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
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
		
		public static string LoadProcessPreviewReportRequestItem(ActionBase AActionBase, 
		                                                         ref ProcessPreviewReportRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadProcessPreviewReportRequestItem";
			object Obj = null;
			try 
			{
				Obj = AActionBase.GetCollectionValue("Data", "runId");
				TempItem.RunIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = AActionBase.GetCollectionValue("Data", "reportId");
				TempItem.ReportIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
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
		
		public static string BuildPaymentFile(string ConnectionString,
		                                      int RunId,
		                                      Int64 CompanyClientId,
		                                      Int64 ClientId,
		                                      string OutputArrayName,
		                                      out string ErrorMsg)
		{
			string Result = "";
			ErrorMsg = "";
			
			/*
			 * Generate EventRunId UsefulDBMethods.GeneratorHelpers.GetNextGeneratorId(ConnectionString, "EVA_ID_GEN", CompanyClientId, AddedById, out ErrorMsg);
			 * call EXECUTE PROCEDURE EASTATEMENTPAYFILE ( COMPANYCLIENTID , RUNID , WHOBYID , EVENTRUNID )
			 * get command line from EAEventCreateFile for EventRunId (UsefulDBMethods.EventHelpers.GetEventCommandLineForRunId)
			 * global::UsefulDBMethods.Reporting.ReportUtilities.BuildReportFileListEntries
			 * 
			*/
			int EventRunId = 
				UsefulDBMethods.GeneratorHelpers.GetNextGeneratorId(
					ConnectionString, "EVA_ID_GEN", (int)CompanyClientId, (int)ClientId, out ErrorMsg);
			if (string.IsNullOrEmpty(ErrorMsg))
			{
				ExecuteEAStatementPayFile(ConnectionString, (int)CompanyClientId, (int)ClientId, RunId, EventRunId, out ErrorMsg);
			}
			
			string CommandLine = "";
			if (string.IsNullOrEmpty(ErrorMsg))
			{
				CommandLine = UsefulDBMethods.EventHelpers.GetEventCommandLineForRunId(ConnectionString, EventRunId, out ErrorMsg);
			}
			
			string AppLocation = "";
			string DocsLocation = "";
			global::UsefulDBMethods.Reporting.ReportUtilities reportUtils = new global::UsefulDBMethods.Reporting.ReportUtilities();
			if (string.IsNullOrEmpty(ErrorMsg))
			{
				reportUtils.GetCompanySettingsItems(ConnectionString, CompanyClientId, out AppLocation, out DocsLocation, out ErrorMsg);
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					if  (!AppLocation.EndsWith(@"\")) AppLocation += @"\";
					AppLocation += "{0}";
				}
			}
			
			
			StringBuilder Entries = new StringBuilder();
			if (string.IsNullOrEmpty(ErrorMsg))
			{
				reportUtils.BuildReportFileListEntries(
					ConnectionString, EventRunId, CommandLine, AppLocation, DocsLocation, OutputArrayName, ref Entries, out ErrorMsg);
			}
			
			if (string.IsNullOrEmpty(ErrorMsg))
			{
				Result = Entries.ToString();
			}
			
			return Result;
		}
		
		private static void ExecuteEAStatementPayFile(string ConnectionString,
		                                              int CompanyClientId,
		                                              int ClientId,
		                                              int RunId,
		                                              int EventRunId,
		                                              out string ErrorMsg)
		{
			ErrorMsg = "";
			try
			{
				PaymentOperations.CommonMethods inst = new CommonMethods();
				string QueryText =
					" EXECUTE PROCEDURE EASTATEMENTPAYFILE (@CompanyClientId, @RunId, @WhoBy, @EventRunId)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyClientId", CompanyClientId));
				Params.Add(new FbParameterItem("RunId", RunId));
				Params.Add(new FbParameterItem("WhoBy", ClientId));
				Params.Add(new FbParameterItem("EventRunId", EventRunId));
				//WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText:" + Environment.NewLine + QueryText, Inst.FModuleName, MasterId);
				ErrorMsg = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, inst.FModuleName, 0);
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
				}
			}
			catch (Exception Ex)
			{
				ErrorMsg = 
					Ex.Message + Environment.NewLine +
					Ex.InnerException + Environment.NewLine +
					Ex.StackTrace;
			}
		}
		
		public static string LoadDisplayPmtPdfReportRequestItem(ActionBase AActionBase,
		                                                        ref DisplayPmtPdfReportRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadDisplayPmtPdfReportRequestItem";
			object Obj = null;
			try 
			{
				Obj = AActionBase.GetCollectionValue("Data", "RunId");
				TempItem.RunIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = AActionBase.GetCollectionValue("Data", "PassedId");
				TempItem.PassedIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = AActionBase.GetCollectionValue("Data", "BtnId");
				TempItem.BtnIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
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
		
		public static string LoadNewPaymentPreviewGridRptRequestItem(ActionBase ActionBase,
		                                                             ref NewPaymentPreviewGridRptRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadNewPaymentPreviewGridRptRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				Object Obj = ActionBase.GetCollectionValue("Data", "runId");
				TempItem.RunIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "PassedId");
				TempItem.PassedIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ButtonId");
				TempItem.ButtonIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
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

        public static string GetSearchGridResults(string ConnectionString,
                                                  int CompanyId,
                                                  int ClientId,
                                                  SearchGridRequestItem Request,
                                                  ref StringBuilder Entries)
        {
            string Method = "BuildOutputEntries";
            string Result = "";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                string QueryText =
                    " SELECT " +
                    "   RUNID as Run_ID," +
                    "   ZL15_Type AS ZL10_Run_Type," +
                    "   ZL15_AddedBy as ZL20_Detail," +
                    "   ZL20_LEDGER_TYPES_PAID AS ZL10_Ledger_Type" +
                    "   ,ZC10_DATE AS ZC6_Date" +
                    "   ,(select OUTPUTVALUE from formatcurrencyforlist(ZR10_AMOUNT)) as M_Total" +
                    "   ,i_payments as ZC6_PAYMENTS" +
                    "   ,i_directs as ZC6_Directs" +
                    "   ,i_cheques as ZC6_Cheques" +
                    "   ,ZC8_Archived as ZC6_Archived" +
                    "   ,ZC8_EMAILS_SENT" +
                    "   ,ZC8_EMAILS_NOTSENT" +
                    "   ,ShowEdit" +
                    "   ,ShowTrash" +
                    "   ,ZL3_B1" +
                    "   ,ZL3_B2" +
                    "   ,ZL3_B3" +
                    " FROM EAPAYMENTLIST (" +
                    "   @DisplayType," + // 0, - Display List, 1 - Display Statement Run ID entries
                    "   @StatementRunId," + // 0, - Statement RUNID  0  is all
                    "   @CompanyClientId," +
                    "   @BankId," + // BANKID ,      0  is all
                    "   @BranchId," + // BRANCHID ,    0  is all
                    "   @ManagerId," + // MANAGERID,    0  is all
                    "   @LedgerTypeId," + // LEDGERTYPEID, 0  is all
                    "   @LedgerId," + // LEDGERID,     0  is all
                    "   @PaymentTypeId," + // PAYMENTTYPE ) '' is all, ACCEPTS 'PAYMENT,STATEMENT,PREVIEW,ADVANCE,BPAY,MULTI'
                    "   @DateRangeId" +
                    " )" +
                    " ORDER BY RunId DESC";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                Params.Add(new FbParameterItem("DisplayType", Request.DisplayType));
                Params.Add(new FbParameterItem("StatementRunId", Request.StatementRunId));
                Params.Add(new FbParameterItem("CompanyClientId", CompanyId));
                Params.Add(new FbParameterItem("BankId", Request.BankNameId));
                Params.Add(new FbParameterItem("BranchId", Request.OfficeId));
                Params.Add(new FbParameterItem("ManagerId", Request.ManagerId));
                Params.Add(new FbParameterItem("LedgerTypeId", Request.LedgerTypeId));
                Params.Add(new FbParameterItem("LedgerId", Request.LedgerId));
                Params.Add(new FbParameterItem("PaymentTypeId", Request.PaymentTypeId));
                Params.Add(new FbParameterItem("DateRangeId", Request.DateRangeId));
                UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
                /*
                WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText:" + Environment.NewLine + QueryText, FModuleName, MasterId);
                
                foreach (FbParameterItem Param in Params)
                {
                    WindowsEventLoggingUtilities.WriteToInfoEvent(
                        Param.ParameterName + ": " + Convert.ToString(Param.ParameterValue), FModuleName, MasterId);
                }
                */

                DataTable dt = null;
                try
                {
                    dt = FirebirdUtilities.GetFirstDataTable(ConnectionString, QueryText, Params, 0);
                    if (dt == null)
                    {
                        Result = "No tables in the dataset to process";
                        Result = ActionBase.LoggingEntry(Method, Result, "");
                        return Result;
                    }
                }
                catch (Exception Ex)
                {
                    Result = ActionBase.LogOnException(Method, Ex, "");
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
                Result = ActionBase.LogOnException(Method, Ex, "");
            }
            finally
            {
                ActionBase.LogOnLeavingMethod(Method);
            }

            return Result;
        }

        public static string LoadSearchGridRequestItem(JObject DataObject,
                                                       ref SearchGridRequestItem TempItem)
        {
            string Result = "";
            string Method = "LoadSearchGridRequestItem";
            try
            {
                TempItem.OfficeIdValue = (string)DataObject["officeId"];
                TempItem.ManagerIdValue = (string)DataObject["managerId"];
                TempItem.LedgerTypeIdValue = (string)DataObject["ledgerTypeId"];
                TempItem.LedgerIdValue = (string)DataObject["ledger"];
                TempItem.BankNameIdValue = (string)DataObject["bankNameId"];
                TempItem.PaymentTypeIdValue = (string)DataObject["paymentTypeId"];
                TempItem.DisplayTypeValue = (string)DataObject["displayType"];
                TempItem.StatementRunIdValue = (string)DataObject["statementRunId"];
                TempItem.DateRangeIdValue = (string)DataObject["dateRangeId"];
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

        public static string LoadDisplayPaymentsGridReportRequestItem(JObject DataObject,
                                                                      ref DisplayPaymentsGridReportRequestItem TempItem)
        {
            string Result = "";
            string Method = "LoadDisplayPaymentsGridReportRequestItem";
            try
            {
                SearchGridRequestItem Item = new SearchGridRequestItem();
                if (string.IsNullOrEmpty(Result))
                {
                    Result = LoadSearchGridRequestItem(DataObject, ref Item);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    TempItem.OfficeIdValue = Item.OfficeIdValue;
                    TempItem.ManagerIdValue = Item.ManagerIdValue;
                    TempItem.LedgerTypeIdValue = Item.LedgerTypeIdValue;
                    TempItem.LedgerIdValue = Item.LedgerIdValue;
                    TempItem.BankNameIdValue = Item.BankNameIdValue;
                    TempItem.PaymentTypeIdValue = Item.PaymentTypeIdValue;
                    TempItem.DisplayTypeValue = Item.DisplayTypeValue;
                    TempItem.StatementRunIdValue = Item.StatementRunIdValue;
                    TempItem.DateRangeIdValue = Item.DateRangeIdValue;
                }

                TempItem.PassedIdValue = (string)DataObject["PassedId"];
                TempItem.BtnIdValue = (string)DataObject["BtnId"];
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

        public static string CopyRequestObject(DisplayPaymentsGridReportRequestItem From,
                                               ref SearchGridRequestItem To)
        {
            string Result = "";
            string Method = "CopyRequestObject";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                From.OfficeIdValue = To.OfficeIdValue;
                From.ManagerIdValue = To.ManagerIdValue;
                From.LedgerTypeIdValue = To.LedgerTypeIdValue;
                From.LedgerIdValue = To.LedgerIdValue;
                From.BankNameIdValue = To.BankNameIdValue;
                From.PaymentTypeIdValue = To.PaymentTypeIdValue;
                From.DisplayTypeValue = To.DisplayTypeValue;
                From.StatementRunIdValue = To.StatementRunIdValue;
                From.DateRangeIdValue = To.DateRangeIdValue;
            }
            catch (Exception Ex)
            {
                Result = ActionBase.LogOnException(Method, Ex, "");
            }
            finally
            {
                ActionBase.LogOnLeavingMethod(Method);
                WindowsEventLoggingUtilities.WriteToInfoEvent("After CopyRequestObject Result: " + Result, GetModuleName(), 0);
            }

            return Result;
        }

        public static string DisplayPaymentsGridReport(string ConnectionString,
                                                       int CompanyId,
                                                       int ClientId,
                                                       DisplayPaymentsGridReportRequestItem Request,
                                                       ref StringBuilder Entries)
        {
            string Result = "";
            string Method = GetModuleName() + "DisplayPaymentsGridReport";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                string ReportSource = "";
                if (string.IsNullOrEmpty(Result))
                {
                    ReportSource = GetPaymentsGridReportSource(ConnectionString, CompanyId, ClientId, Request, out Result);
                }

                string OutputFileName = "";
                if (string.IsNullOrEmpty(Result))
                {
                    Int64 NewId =
                        global::UsefulDBMethods.Reporting.SystemReportUtilities.Execute(
                            ConnectionString, CompanyId, ClientId,
                            Request.PassedId, ReportSource, out Result, out OutputFileName);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    if (Entries.Length > 0) Entries.Append(",");
                    Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("FileName", @"/Docs/" + OutputFileName, false));
                }
                else
                {
                    Result = ActionBase.LogOnProcessingError(Method, Result, "");
                }
            }
            catch (Exception Ex)
            {
                Result = ActionBase.LogOnException(Method, Ex, "");
            }
            finally
            {
                ActionBase.LogOnLeavingMethod(Method);
                WindowsEventLoggingUtilities.WriteToInfoEvent("After DisplayGridReport Result: " + Result, Method, 0);
            }

            return Result;
        }

        private static string GetPaymentsGridReportSource(string ConnectionString,
                                                          int CompanyId,
                                                          int ClientId,
                                                          DisplayPaymentsGridReportRequestItem Request,
                                                          out string ErrorMsg)
        {
            string Result = "";
            ErrorMsg = "";
            string Method = "GetPaymentsGridReportSource";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                StringBuilder Entries = new StringBuilder();

                SearchGridRequestItem Item = new SearchGridRequestItem();
                if (string.IsNullOrEmpty(ErrorMsg))
                {
                    ErrorMsg = CopyRequestObject(Request, ref Item);
                }

                if (string.IsNullOrEmpty(ErrorMsg))
                {
                    ErrorMsg = GetSearchGridResults(ConnectionString, CompanyId, ClientId, Item, ref Entries);
                }

                string Str =
                    JsonUtilities.StartJson() +
                    Entries.ToString() +
                    JsonUtilities.EndJson();

                JObject JObj = null;
                try
                {
                    if (Entries.Length > 0)
                    {
                        JObj = JObject.Parse(Str);
                    }
                }
                catch (Exception Ex)
                {
                    ErrorMsg =
                        Method + Environment.NewLine +
                        Ex.Message + Environment.NewLine +
                        Ex.StackTrace;
                }

                if (string.IsNullOrEmpty(ErrorMsg))
                {
                    string JPath = @"$.Items[?(@.RUN_ID == '" + Request.PassedId + "')]";
                    WindowsEventLoggingUtilities.WriteToInfoEvent("JPath: " + JPath, Method, 0);
                    JToken Record = JObj.SelectToken(JPath);
                    if (Record == null)
                        WindowsEventLoggingUtilities.WriteToInfoEvent("Record IS NULL", Method, 0);
                    else
                    {
                        WindowsEventLoggingUtilities.WriteToInfoEvent("Record: " + Record.ToString(), Method, 0);
                        Result = (string)Record["ZL3_B" + Request.BtnId];
                        WindowsEventLoggingUtilities.WriteToInfoEvent("Result: " + Result, Method, 0);
                    }
                }
            }
            catch (Exception Ex)
            {
                Result = ActionBase.LogOnException(Method, Ex, "");
            }
            finally
            {
                ActionBase.LogOnLeavingMethod(Method);
                WindowsEventLoggingUtilities.WriteToInfoEvent("After GetPaymentsGridReportSource Result: " + Result, Method, 0);
            }

            return Result;
        }
    }
}
