/*
 * Created by SharpDevelop.
 * User: John
 * Date: 20/05/2020
 * Time: 11:52 AM
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
namespace Actions.Invoices.AddInvoice
{
	public enum InvoiceActionTaken
	{
		Add = 0,
		Update = 1,
		Delete = 2,
		Cancel = 3,
		Posted = 4,
		Approved = 5,
		Unknown = 99
	}
	
	/// <summary>
	/// Description of CommonMethods.
	/// </summary>
	public class CommonMethods
	{
        public static string GetModuleName() { return "Invoices.AddInvoice."; }

        public static string LoadSetupPageEntriesRequestItem(ActionBase ActionBase,
		                                                     ref AddInvoice.SetupPageEntriesRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadSetupPageEntriesRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				Object Obj = ActionBase.GetCollectionValue("Data", "InvoiceMasterId");
				TempItem.InvoiceMasterIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "InvoicesScannedId");
				TempItem.InvoicesScannedIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "InvoicesSelectedId");
				TempItem.InvoicesSelectedIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "CreditorId");
				TempItem.CreditorIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "TransactionId");
				TempItem.TransactionIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "SubAccountId");
				TempItem.SubAccountIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "UncompletedTaskId");
				TempItem.UncompletedTaskIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "ExpenseId");
				TempItem.ExpenseIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "RevenueId");
				TempItem.RevenueIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "InvoiceDate");
				TempItem.InvoiceDate = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "InvoiceReleaseDate");
				TempItem.InvoiceReleaseDate = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "InvoiceNumber");
				TempItem.InvoiceNo = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "InvoiceBPay");
				TempItem.InvoiceBPay = (Obj == null) ? "" : Convert.ToString(Obj); 

				Obj = ActionBase.GetCollectionValue("Data", "InvoiceDetail");
				TempItem.InvoiceDetail = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "InvoiceAmount");
				TempItem.InvoiceAmountValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "InvoiceGSTAmount");
				TempItem.InvoiceGSTAmountValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "InvoicePriority");
				TempItem.InvoicePriorityValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "ChargeTransactionId");
				TempItem.ChargeTransactionIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "ChargeGross");
				TempItem.ChargeGrossValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "ChargeGST");
				TempItem.ChargeGSTValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "EmailTaxInvoiceId");
				TempItem.EmailTaxInvoiceIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "InstalmentId");
				TempItem.InstalmentIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "SplitInvoiceById");
				TempItem.SplitInvoiceByIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "OnlyACharge");
				TempItem.OnlyACharge = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "HoldInvoiceTillClientPays");
				TempItem.HoldTillClientPays = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "ChargeFromDate");
				TempItem.ChargeFromDate = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "ChargeToDate");
				TempItem.ChargeToDate = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "ChargeDueDate");
				TempItem.ChargeDueDate = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "ChargePrintDate");
				TempItem.ChargePrintDate = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "ApprovalRequiredBy");
				TempItem.ApprovalRequiredByValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "ApprovedComment");
				TempItem.ApprovedComment = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "ShowGSTAmount");
				TempItem.ShowGSTAmount = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "CalculateGST");
				TempItem.CalculateGST = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "AlterGSTAmount");
				TempItem.AlterGSTAmount = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "ShowEmailTaxInvoice");
				TempItem.ShowEmailTaxInvoice = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "Approved");
				TempItem.Approved = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "ApprovalRequired");
				TempItem.ApprovalRequired = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "ChargeBusiness");
				TempItem.ChargeBusiness = (Obj == null) ? "" : Convert.ToString(Obj); 

				Obj = ActionBase.GetCollectionValue("Data", "SelectedCreditor");
				TempItem.SelectedCreditor = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "SelectedProperty");
				TempItem.SelectedProperty = (Obj == null) ? "" : Convert.ToString(Obj); 
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
		
		public static string EAInvoiceMasterAdd(string ConnectionString,
		                                        Int64 CompanyId,
		                                        Int64 ClientId,
		                                        InvoiceActionTaken ActionTaken,
		                                        string INVMSource,
		                                        SetupPageEntriesRequestItem RequestItem)
		{
			string Result = "";
			string Method = "EAInvoiceMasterAdd";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			ActionBase ActionBase = new ActionBase();
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string QueryText =
					"EXECUTE PROCEDURE EAInvoiceMasterAdd(@CompanyId, @MasterInvoiceId, @ActionTaken, @AddedById, @INVMSource," +
					" @AttachmentId, @CreditorBusinessId, @TransactionBusinessId, @InvoiceAmount, @InvoiceGSTAmount," +
					" @InvoiceSubLedger, @InvoiceTaskId, @InvoiceExpenseId, @InvoiceRevenueId, @InvoiceDate, @InvoicePayDate," +
					" @InvoiceNumber, @InvoiceBPayNumber, @InvoiceDetail, @InvoicePriority, @ChargeTransactionId, @EmailTaxInvoice," + 
					" @HoldTillTenantPays, @OnlyCreateCharge, @InstalmentId, @SplitInvoiceId, @ApprovalRequired, @AssignedToId," + 
					" @Approved, @ApprovedComment, @ChargeAmount, @ChargeGST, @ChargeFromDate, @ChargeToDate, @ChargeDueDate," + 
					" @ChargePrintDate,@B_INVM_ChargeBusiness, @InvoicesScannedId)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				Params.Add(new FbParameterItem("MasterInvoiceId", RequestItem.InvoiceMasterId));
				Params.Add(new FbParameterItem("ActionTaken", (int)ActionTaken));
				Params.Add(new FbParameterItem("AddedById", ClientId));
				Params.Add(new FbParameterItem("INVMSource", INVMSource));
				
				Params.Add(new FbParameterItem("AttachmentId", RequestItem.InvoicesSelectedId));
				Params.Add(new FbParameterItem("CreditorBusinessId", RequestItem.CreditorId));
				Params.Add(new FbParameterItem("TransactionBusinessId", RequestItem.TransactionId));
				Params.Add(new FbParameterItem("InvoiceAmount", RequestItem.InvoiceAmount));
				Params.Add(new FbParameterItem("InvoiceGSTAmount", RequestItem.InvoiceGSTAmount));
				
				Params.Add(new FbParameterItem("InvoiceSubLedger", RequestItem.SubAccountId));
				Params.Add(new FbParameterItem("InvoiceTaskId", RequestItem.UncompletedTaskId));
				Params.Add(new FbParameterItem("InvoiceExpenseId", RequestItem.ExpenseId));
				Params.Add(new FbParameterItem("InvoiceRevenueId", RequestItem.RevenueId));
				Params.Add(new FbParameterItem("InvoiceDate", RequestItem.InvoiceDate));
				Params.Add(new FbParameterItem("InvoicePayDate", RequestItem.InvoiceReleaseDate));
				
				Params.Add(new FbParameterItem("InvoiceNumber", RequestItem.InvoiceNo));
				Params.Add(new FbParameterItem("InvoiceBPayNumber", RequestItem.InvoiceBPay));
				Params.Add(new FbParameterItem("InvoiceDetail", RequestItem.InvoiceDetail));
				Params.Add(new FbParameterItem("InvoicePriority", RequestItem.InvoicePriority));
				Params.Add(new FbParameterItem("ChargeTransactionId", RequestItem.ChargeTransactionId));
				Params.Add(new FbParameterItem("EmailTaxInvoice", RequestItem.EmailTaxInvoiceId));
				
				Params.Add(new FbParameterItem("HoldTillTenantPays", RequestItem.HoldTillClientPays));
				Params.Add(new FbParameterItem("OnlyCreateCharge", RequestItem.OnlyACharge));
				Params.Add(new FbParameterItem("InstalmentId", RequestItem.InstalmentId));
				Params.Add(new FbParameterItem("SplitInvoiceId", RequestItem.SplitInvoiceById));
				Params.Add(new FbParameterItem("ApprovalRequired", RequestItem.ApprovalRequired));
				Params.Add(new FbParameterItem("AssignedToId", ClientId));
				
				Params.Add(new FbParameterItem("Approved", RequestItem.Approved));
				Params.Add(new FbParameterItem("ApprovedComment", RequestItem.ApprovedComment));
				Params.Add(new FbParameterItem("ChargeAmount", RequestItem.ChargeGross));
				Params.Add(new FbParameterItem("ChargeGST", RequestItem.ChargeGST));
				Params.Add(new FbParameterItem("ChargeFromDate", RequestItem.ChargeFromDate));
				Params.Add(new FbParameterItem("ChargeToDate", RequestItem.ChargeToDate));
				Params.Add(new FbParameterItem("ChargeDueDate", RequestItem.ChargeDueDate));
				
				Params.Add(new FbParameterItem("ChargePrintDate", RequestItem.ChargePrintDate));
				Params.Add(new FbParameterItem("B_INVM_ChargeBusiness", RequestItem.ChargeBusiness));
				Params.Add(new FbParameterItem("InvoicesScannedId", RequestItem.InvoicesScannedId));
				
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				
				if (string.IsNullOrEmpty(Result))
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent("Before Execute", Method, 0);
					Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
					WindowsEventLoggingUtilities.WriteToInfoEvent("After Execute", Method, 0);
				}
				
				/*
				if (string.IsNullOrEmpty(Result))
				{
					Result = UsefulDBMethods.Helpers.RunEAMainOfficeProcess(ConnectionString, CompanyId);
				}
				*/
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
		
		/*
		public static string EAInvoicesToAddGroup(string ConnectionString,
		                                          Int64 CompanyId,
		                                          Int64 ClientId,
		                                          SetupPageEntriesRequestItem RequestItem,
		                                          ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "EAInvoicesToAddGroup";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			ActionBase ActionBase = new ActionBase();
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string QueryText =
					"SELECT OutputLine FROM EAInvoicesToAddGroup(@CompanyId, @LoginId, @InvoicesScannedId, @InvoiceSelectedId, @InvoiceMasterId," +	// 5					
					" @CreditorId, @TransactionId, @TransactionSubAccount, @UncompletedTaskId, @ExpenseId, @RevenueId, @InvoiceDate," +			//7					
					" @InvoiceReleaseDate, @InvoiceNumber, @InvoiceBPayNumber, @InvoiceDetail, @InvoiceAmount, @ShowGSTAmount," +				//6					
					" @CalculateGST, @AlterGSTAmount, @InvoiceGSTAmount, @InvoicePriority, @ChargeTransactionId, @ChargeGross," + 				//6					
					" @ChargeGST, @ShowEmailTaxInvoice, @EmailTaxInvoice, @InstalmentId, @SplitInvoiceBy, @OnlyACharge," +						//6					
					" @HoldTillClientPays, @ChargeFromDate, @ChargeToDate, @ChargeDueDate, @ChargePrintDate, @ApprovalRequired," +				//6					
					" @ApprovalRequiredBy, @Approved, @ApprovedComment, @ChargeBusCbx)";														// 4
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				Params.Add(new FbParameterItem("LoginId", ClientId));
				Params.Add(new FbParameterItem("InvoicesScannedId", RequestItem.InvoicesScannedId));
				Params.Add(new FbParameterItem("InvoiceSelectedId", RequestItem.InvoicesSelectedId));
				Params.Add(new FbParameterItem("InvoiceMasterId", RequestItem.InvoiceMasterId));

				Params.Add(new FbParameterItem("CreditorId", RequestItem.CreditorId));
				Params.Add(new FbParameterItem("TransactionId", RequestItem.TransactionId));
				Params.Add(new FbParameterItem("TransactionSubAccount", RequestItem.SubAccountId));
				Params.Add(new FbParameterItem("UncompletedTaskId", RequestItem.UncompletedTaskId));
				Params.Add(new FbParameterItem("ExpenseId", RequestItem.ExpenseId));
				Params.Add(new FbParameterItem("RevenueId", RequestItem. RevenueId));
				Params.Add(new FbParameterItem("InvoiceDate", RequestItem.InvoiceDate));

				Params.Add(new FbParameterItem("InvoiceReleaseDate", RequestItem.InvoiceReleaseDate));
				Params.Add(new FbParameterItem("InvoiceNumber", RequestItem.InvoiceNo));
				Params.Add(new FbParameterItem("InvoiceBPayNumber", RequestItem.InvoiceBPay));
				Params.Add(new FbParameterItem("InvoiceDetail", RequestItem.InvoiceDetail));
				Params.Add(new FbParameterItem("InvoiceAmount", RequestItem.InvoiceAmount));
				Params.Add(new FbParameterItem("ShowGSTAmount", RequestItem.ShowGSTAmount));
				
				Params.Add(new FbParameterItem("CalculateGST", RequestItem.CalculateGST));
				Params.Add(new FbParameterItem("AlterGSTAmount", RequestItem.AlterGSTAmount));
				Params.Add(new FbParameterItem("InvoiceGSTAmount", RequestItem.InvoiceGSTAmount));
				Params.Add(new FbParameterItem("InvoicePriority", RequestItem.InvoicePriority));
				Params.Add(new FbParameterItem("ChargeTransactionId", RequestItem.ChargeTransactionId));
				Params.Add(new FbParameterItem("ChargeGross", RequestItem.ChargeGross));

				Params.Add(new FbParameterItem("ChargeGST", RequestItem.ChargeGST));
				Params.Add(new FbParameterItem("ShowEmailTaxInvoice", RequestItem.ShowEmailTaxInvoice));
				Params.Add(new FbParameterItem("EmailTaxInvoice", RequestItem.EmailTaxInvoiceId));
				Params.Add(new FbParameterItem("InstalmentId", RequestItem.InstalmentId));
				Params.Add(new FbParameterItem("SplitInvoiceBy", RequestItem.SplitInvoiceById));
				Params.Add(new FbParameterItem("OnlyACharge", RequestItem.OnlyACharge));

				Params.Add(new FbParameterItem("HoldTillClientPays", RequestItem.HoldTillClientPays));
				Params.Add(new FbParameterItem("ChargeFromDate", RequestItem.ChargeFromDate));
				Params.Add(new FbParameterItem("ChargeToDate", RequestItem.ChargeToDate));
				Params.Add(new FbParameterItem("ChargeDueDate", RequestItem.ChargeDueDate));
				Params.Add(new FbParameterItem("ChargePrintDate", RequestItem.ChargePrintDate));
				Params.Add(new FbParameterItem("ApprovalRequired", RequestItem.ApprovalRequired));
				
				Params.Add(new FbParameterItem("ApprovalRequiredBy", RequestItem.ApprovalRequiredBy));
				Params.Add(new FbParameterItem("Approved", RequestItem.Approved));
				Params.Add(new FbParameterItem("ApprovedComment", RequestItem.ApprovedComment));
				Params.Add(new FbParameterItem("ChargeBusCbx", RequestItem.ChargeBusiness));
				
				
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				
				object Obj = null;
				DataTable tbl = null;
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
					if (string.IsNullOrEmpty(Result) && (tbl.Rows.Count == 0)) Result = "No rows to process";
					if (string.IsNullOrEmpty(Result))
					{
						if (Entries.Length > 0) Entries.Append(",");
						Obj = null;
						foreach (DataRow Row in tbl.Rows)
						{
							Obj = Row["OUTPUTLINE"];
							Entries.AppendLine((Obj == DBNull.Value) ? "" : Convert.ToString(Obj));
						}
					}
				}
				
				string Str = "";
				string OwnerNoteSql = "";
				string OwnersInstructions = "";
				if (string.IsNullOrEmpty(Result))
				{
					Str =
						JsonUtilities.StartJson() +
						Entries.ToString() +
						JsonUtilities.EndJson();
					OwnerNoteSql = JsonUtilities.GetJsonPathValueStr(Str, "$.OwnerNoteSQL", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(OwnerNoteSql)) Result = "Cannot find the Owner Notes entry";
					if (string.IsNullOrEmpty(Result))
					{
						OwnerNoteSql = OwnerNoteSql.Replace("__", "'");
						WindowsEventLoggingUtilities.WriteToInfoEvent("OwnerNoteSql:" + Environment.NewLine + OwnerNoteSql, Method, 0);
						tbl =
							UsefulDBMethods.Helpers.GetFirstDataTableEx(
								ConnectionString, OwnerNoteSql, null, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
						WindowsEventLoggingUtilities.WriteToInfoEvent("Row Count:" + tbl.Rows.Count.ToString(), Method, 0);
						if (string.IsNullOrEmpty(Result))
						{
							foreach (DataRow Row in tbl.Rows)
							{
								WindowsEventLoggingUtilities.WriteToInfoEvent("Processing a DataRow: " + Row[0], Method, 0);
								Obj = Row[0];
								if (Obj != DBNull.Value)
								{
									byte[] byteArray = (byte[])Obj;
									OwnersInstructions = System.Text.Encoding.Default.GetString(byteArray);

            					}
							}
							if (Entries.Length > 0) Entries.Append(",");
							Entries.Append(JsonUtilities.AddPropertyNameAndValue("OwnersInstructions", OwnersInstructions, false));
						}
					}
				}
				
				string GridSql = "";
				if (string.IsNullOrEmpty(Result))
				{
					GridSql = JsonUtilities.GetJsonPathValueStr(Str, "$.GRIDSQL", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(GridSql))
					{
						WindowsEventLoggingUtilities.WriteToInfoEvent("NOT Building GridEntries", Method, 0);
						if (Entries.Length > 0) Entries.Append(",");
						Entries.Append(JsonUtilities.StartJsonArray("GridEntries"));
						Entries.Append(JsonUtilities.EndJsonArray());
					}
					else if (string.IsNullOrEmpty(Result) && !string.IsNullOrEmpty(GridSql))
					{
						tbl =
							UsefulDBMethods.Helpers.GetFirstDataTableEx(
								ConnectionString, GridSql, null, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
						if (string.IsNullOrEmpty(Result) && (tbl == null)) Result = "Error calling query in " + Method;
					
						if (string.IsNullOrEmpty(Result))
						{
							WindowsEventLoggingUtilities.WriteToInfoEvent("Building GridEntries", Method, 0);
							if (Entries.Length > 0) Entries.Append(",");
							Result = JsonUtilities.BuildJsonArrayFromDataTable(tbl, "GridEntries", ref Entries);
						}
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
			}
			
			return Result;
		}
		*/
		
		public static string EAInvoicesToAddGroup(string ConnectionString,
		                                          Int64 CompanyId,
		                                          Int64 ClientId,
		                                          Int64 InvoiceMasterId,
		                                          string SelectedCreditor,
		                                          string SelectedProperty,
		                                          ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "EAInvoicesToAddGroup";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			ActionBase ActionBase = new ActionBase();
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				if (Entries.Length > 0) Entries.Append(",");
				Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("InvoiceMasterId", InvoiceMasterId));
				Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("SelectedCreditor", SelectedCreditor));
				Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("SelectedProperty", SelectedProperty, false));
				
				string SelectQuery = "";
				string QueryText = "SELECT INVM_SqlScreen FROM EAInvoiceMaster WHERE I_INVM_ID = @InvoiceMasterId";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("InvoiceMasterId", InvoiceMasterId));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				
				object Obj = null;
				DataTable tbl = null;
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
					if (string.IsNullOrEmpty(Result) && (tbl.Rows.Count == 0)) Result = "No rows to process";
					if (string.IsNullOrEmpty(Result))
					{
						Obj = null;
						foreach (DataRow Row in tbl.Rows)
						{
							Obj = Row["INVM_SqlScreen"];
							SelectQuery = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
						}
						
						if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(SelectQuery)) Result = "Cannot find the Select Query";
					}
					WindowsEventLoggingUtilities.WriteToInfoEvent("SqlScreenValue:" + Environment.NewLine + SelectQuery, Method, 0);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					SelectQuery = SelectQuery.Replace("__", "'");
					SelectQuery = SelectQuery.Replace("\"", "'");
					WindowsEventLoggingUtilities.WriteToInfoEvent("SelectQuery:" + Environment.NewLine + SelectQuery, Method, 0);
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, SelectQuery, null, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
					WindowsEventLoggingUtilities.WriteToInfoEvent("SelectQuery - After GetFirstDataTable " + Result, Method, 0);
					if (string.IsNullOrEmpty(Result))
					{
						WindowsEventLoggingUtilities.WriteToInfoEvent("SelectQuery - Result" + Result, Method, 0);
						if (Entries.Length > 0) Entries.Append(",");
						Obj = null;
						foreach (DataRow Row in tbl.Rows)
						{
							Obj = Row["OUTPUTLINE"];
							Entries.AppendLine((Obj == DBNull.Value) ? "" : Convert.ToString(Obj));
						}
					}
					WindowsEventLoggingUtilities.WriteToInfoEvent("SelectQuery - After GetFirstDataTable2 " + Result, Method, 0);
				}
				WindowsEventLoggingUtilities.WriteToInfoEvent("After processing SelectQuery", Method, 0);
				
				string Str = "";
				string OwnerNoteSql = "";
				string OwnersInstructions = "";
				if (string.IsNullOrEmpty(Result))
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent("Start - OwnerInstructions", Method, 0);
					Str =
						JsonUtilities.StartJson() +
						Entries.ToString() +
						JsonUtilities.EndJson();
					WindowsEventLoggingUtilities.WriteToInfoEvent("Before", Method, 0);
					OwnerNoteSql = JsonUtilities.GetJsonPathValueStr(Str, "$.OwnerNoteSQL", out Result);
					WindowsEventLoggingUtilities.WriteToInfoEvent("After", Method, 0);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(OwnerNoteSql)) Result = "Cannot find the Owner Notes entry";
					if (string.IsNullOrEmpty(Result))
					{
						OwnerNoteSql = OwnerNoteSql.Replace("__", "'");
						WindowsEventLoggingUtilities.WriteToInfoEvent("OwnerNoteSql:" + Environment.NewLine + OwnerNoteSql, Method, 0);
						tbl =
							UsefulDBMethods.Helpers.GetFirstDataTableEx(
								ConnectionString, OwnerNoteSql, null, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
						if (string.IsNullOrEmpty(Result))
						{
							foreach (DataRow Row in tbl.Rows)
							{
								Obj = Row[0];
								//OwnersInstructions = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
								if (Obj != DBNull.Value)
								{
									byte[] byteArray = (byte[])Obj;
									OwnersInstructions = System.Text.Encoding.Default.GetString(byteArray);

            					}
							}
							if (Entries.Length > 0) Entries.Append(",");
							Entries.Append(JsonUtilities.AddPropertyNameAndValue("OwnersInstructions", OwnersInstructions, false));
						}
					}
				}
				
				string GridSql = "";
				if (string.IsNullOrEmpty(Result))
				{
					GridSql = JsonUtilities.GetJsonPathValueStr(Str, "$.GRIDSQL", out Result);
					WindowsEventLoggingUtilities.WriteToInfoEvent("GridSql:" + Environment.NewLine + GridSql, Method, 0);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(GridSql))
					{
						WindowsEventLoggingUtilities.WriteToInfoEvent("NOT Building GridEntries", Method, 0);
						if (Entries.Length > 0) Entries.Append(",");
						Entries.Append(JsonUtilities.StartJsonArray("GridEntries"));
						Entries.Append(JsonUtilities.EndJsonArray());
					}
					else if (string.IsNullOrEmpty(Result) && !string.IsNullOrEmpty(GridSql))
					{
						tbl =
							UsefulDBMethods.Helpers.GetFirstDataTableEx(
								ConnectionString, GridSql, null, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
						if (string.IsNullOrEmpty(Result) && (tbl == null)) Result = "Error calling query in " + Method;
					
						if (string.IsNullOrEmpty(Result))
						{
							WindowsEventLoggingUtilities.WriteToInfoEvent("Building GridEntries", Method, 0);
							if (Entries.Length > 0) Entries.Append(",");
							Result = JsonUtilities.BuildJsonArrayFromDataTable(tbl, "GridEntries", ref Entries);
						}
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
			}
			
			return Result;
		}

		public static string LoadInvoicePayableDetailsRequestItem(ActionBase ActionBase,
		                                                          ref AddInvoice.InvoicePayableDetailsRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadInvoicePayableDetailsRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				Object Obj = ActionBase.GetCollectionValue("Data", "IsForCreditor");
				TempItem.IsForCreditor = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ProcessingId");
				TempItem.ProcessingIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
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

		public static string LoadRemoveGridEntryRequestItem(ActionBase ActionBase,
		                                                    ref AddInvoice.RemoveGridEntryRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadRemoveGridEntryRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				SetupPageEntriesRequestItem SetupRequestItem = new SetupPageEntriesRequestItem();
				if (string.IsNullOrEmpty(Result))
				{
					Result = SetupRequestItem.Load(ActionBase);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					TempItem.AlterGSTAmount = SetupRequestItem.AlterGSTAmount;
					TempItem.ApprovalRequired = SetupRequestItem.ApprovalRequired;
					TempItem.ApprovalRequiredByValue = SetupRequestItem.ApprovalRequiredByValue;
					TempItem.Approved = SetupRequestItem.Approved;
					TempItem.ApprovedComment = SetupRequestItem.ApprovedComment;
					
					TempItem.CalculateGST = SetupRequestItem.CalculateGST;
					TempItem.ChargeBusiness = SetupRequestItem.ChargeBusiness;
					TempItem.ChargeDueDate = SetupRequestItem.ChargeDueDate;
					TempItem.ChargeFromDate = SetupRequestItem.ChargeFromDate;
					TempItem.ChargeGrossValue = SetupRequestItem.ChargeGrossValue;
					TempItem.ChargeGSTValue = SetupRequestItem.ChargeGSTValue;
					TempItem.ChargePrintDate = SetupRequestItem.ChargePrintDate;
					TempItem.ChargeToDate = SetupRequestItem.ChargeToDate;
					TempItem.ChargeTransactionIdValue = SetupRequestItem.ChargeTransactionIdValue;
					TempItem.CreditorIdValue = SetupRequestItem.CreditorIdValue;
					
					TempItem.EmailTaxInvoiceIdValue = SetupRequestItem.EmailTaxInvoiceIdValue;
					TempItem.ExpenseIdValue = SetupRequestItem.ExpenseIdValue;
					
					TempItem.HoldTillClientPays = SetupRequestItem.HoldTillClientPays;
					
					TempItem.InstalmentIdValue = SetupRequestItem.InstalmentIdValue;
					TempItem.InvoiceAmountValue = SetupRequestItem.InvoiceAmountValue;
					TempItem.InvoiceBPay = SetupRequestItem.InvoiceBPay;
					TempItem.InvoiceDate = SetupRequestItem.InvoiceDate;
					TempItem.InvoiceDetail = SetupRequestItem.InvoiceDetail;
					TempItem.InvoiceGSTAmountValue = SetupRequestItem.InvoiceGSTAmountValue;
					TempItem.InvoiceMasterIdValue = SetupRequestItem.InvoiceMasterIdValue;
					TempItem.InvoiceNo = SetupRequestItem.InvoiceNo;
					TempItem.InvoicePriorityValue = SetupRequestItem.InvoicePriorityValue;
					TempItem.InvoiceReleaseDate = SetupRequestItem.InvoiceReleaseDate;
					TempItem.InvoicesScannedIdValue = SetupRequestItem.InvoicesScannedIdValue;
					TempItem.InvoicesSelectedIdValue = SetupRequestItem.InvoicesSelectedIdValue;
					
					TempItem.OnlyACharge = SetupRequestItem.OnlyACharge;
					
					TempItem.RevenueIdValue = SetupRequestItem.RevenueIdValue;
					
					TempItem.SelectedCreditor = SetupRequestItem.SelectedCreditor;
					TempItem.SelectedProperty = SetupRequestItem.SelectedProperty;
					TempItem.ShowEmailTaxInvoice = SetupRequestItem.ShowEmailTaxInvoice;
					TempItem.ShowGSTAmount = SetupRequestItem.ShowGSTAmount;
					TempItem.SplitInvoiceByIdValue = SetupRequestItem.SplitInvoiceByIdValue;
					TempItem.SubAccountIdValue = SetupRequestItem.SubAccountIdValue;
					
					TempItem.TransactionIdValue = SetupRequestItem.TransactionIdValue;
					
					TempItem.UncompletedTaskIdValue = SetupRequestItem.UncompletedTaskIdValue;
					Object Obj = ActionBase.GetCollectionValue("Data", "RemoveRecordId");
					TempItem.RemoveRecordIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				}
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
		
		public static string RemoveAddInvoiceGridEntry(string ConnectionString,
		                                               Int64 RemoveId)
		{
			string Result = "";
			string Method = "RemoveAddInvoiceGridEntry";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			List<FbParameterItem> Params = new List<FbParameterItem>();
			try 
			{
				string QueryText = "DELETE FROM EAINVOICESLAVE WHERE I_INVS_ID = @RemoveId";
				Params.Add(new FbParameterItem("RemoveId", RemoveId));
				
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
				}
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
		
		public static string GetDebtEventId(string ConnectionString,
		                                    Int64 MasterInvoiceId,
		                                    out int DebtEventId)
		{
			string Result = "";
			string Method = "GetDebtEventId";
			DebtEventId = -99;
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string QueryText = "SELECT I_INVM_DebtEventId FROM EAInvoiceMaster WHERE I_INVM_ID = @MasterInvoiceId";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("MasterInvoiceId", MasterInvoiceId));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				
				object Obj = null;
				DataTable tbl = null;
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
					if (string.IsNullOrEmpty(Result) && (tbl.Rows.Count == 0)) Result = "No rows to process";
					if (string.IsNullOrEmpty(Result))
					{
						foreach (DataRow Row in tbl.Rows)
						{
							Obj = Row["I_INVM_DebtEventId"];
							DebtEventId = (Obj == DBNull.Value) ? -1 : Convert.ToInt32(Obj);
							WindowsEventLoggingUtilities.WriteToInfoEvent("DebtEventId: " + DebtEventId.ToString(), Method, 0);
						}
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
			}
			
			return Result;
		}

        public static string DeleteScannedInvoices(string ConnectionString,
                                                   int CompanyId,
                                                   int ClientId,
                                                   SetupPageEntriesRequestItem Request,
                                                   ref StringBuilder Entries)
        {
            string Result = "";
            string Method = GetModuleName() + "DeleteScannedInvoices";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                StringBuilder TempEntries = new StringBuilder();
                if (string.IsNullOrEmpty(Result))
                {
                    Result = EAInvoicesToAddGroup(ConnectionString, CompanyId, ClientId, 
                        Request.InvoiceMasterId, Request.SelectedCreditor, Request.SelectedProperty, ref TempEntries);
                }

                string Value = "";
                JObject JSONSource = null;
                if (string.IsNullOrEmpty(Result))
                {
                    Value =
                        JsonUtilities.StartJson() +
                        TempEntries.ToString() +
                        JsonUtilities.EndJson();
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(Value)) Result = "Cannot determine the Group entries";

                    if (string.IsNullOrEmpty(Result))
                    {
                        JSONSource = JObject.Parse(Value);
                        if (JSONSource == null) Result = "Cannot parse the Group source";
                        Value = "";
                    }
                }

                string QueryText = "";
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.DeleteScannedInvoiceSQL", out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the first Delete query";
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText);
                }

                QueryText = "";
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.DeleteScannedInvoiceSQL2", out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the second Delete query";
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    UsefulDBMethods.Helpers.RunEAMainOfficeProcess(ConnectionString, CompanyId);
                    UsefulDBMethods.Helpers.RunEAMainOfficeProcess(ConnectionString, CompanyId);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = EAInvoicesToAddGroup(ConnectionString, CompanyId, ClientId,
                        Request.InvoiceMasterId, Request.SelectedCreditor, Request.SelectedProperty, ref Entries);
                }
            }
            catch (Exception Ex)
            {
                Result = ActionBase.LogOnException(Method, Ex, "");
            }
            finally
            {
                ActionBase.LogOnLeavingMethod(Method);
                WindowsEventLoggingUtilities.WriteToInfoEvent("After DeleteScannedInvoices Result: " + Result, Method, 0);
            }

            return Result;
        }
    }
}
