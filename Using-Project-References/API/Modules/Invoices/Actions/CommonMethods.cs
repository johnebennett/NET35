/*
 * Created by SharpDevelop.
 * User: John
 * Date: 23/12/2019
 * Time: 11:37 AM
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

namespace Actions.Invoices
{
	/// <summary>
	/// Description of CommonMethods.
	/// </summary>
	public class CommonMethods
	{
		public CommonMethods()
		{
		}

        public static string GetModuleName() { return "Actions.Invoices."; }

        public static string LoadSetupPageEntriesRequestItem(ActionBase ActionBase,
		                                                     ref SetupPageEntriesRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadSetupPageEntriesRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				Object Obj = ActionBase.GetCollectionValue("Data", "BankId");
				TempItem.BankIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "OfficeId");
				TempItem.BranchIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "ManagerId");
				TempItem.ManagerIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "DisplayType");
				TempItem.DisplayTypeValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "ExpenseId");
				TempItem.ExpenseIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "AssignedToId");
				TempItem.AssignedToIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "DateFrom");
				TempItem.DateFrom = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "DateTo");
				TempItem.DateTo = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "CreditorId");
				TempItem.CreditorIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "TransactionId");
				TempItem.TransactionIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "AccountId");
				TempItem.AccountIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "LedgerType");
				TempItem.LedgerTypeValue = (Obj == null) ? "" : Convert.ToString(Obj); 
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
		
		public static string EAMainInvoiceScreenGroup(string ConnectionString,
		                                              Int64 CompanyId,
		                                              int ClientId,
		                                              int BankId,
		                                              int BranchId,
		                                              int ManagerId,
		                                              int DisplayType,
		                                              int ExpenseId,
		                                              int AssignedToId,
		                                              string DateFrom,
		                                              string DateTo,
		                                              int CreditorId,
		                                              int TransactionId,
		                                              int AccountId,
		                                              int LedgerType,
		                                              ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "EAMainInvoiceScreenGroup";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			try 
			{
				Result = UsefulDBMethods.Helpers.RunEAMainOfficeProcess(ConnectionString, CompanyId);
				string QueryText = "";
				List<FbParameterItem> Params = null;
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = 
						" SELECT OUTPUTLINE FROM EAMainInvoiceScreenGroupNew(@CompanyId, @LoginId, @BankId, @BranchId, @ManagerId, @DisplayType," +
						" @ExpenseId, @AssignedToId, @DateFrom, @DateTo, @CreditorId, @TransactionId, @AccountId, @LedgerType)";
					Params = new List<FbParameterItem>();
					Params.Add(new FbParameterItem("CompanyId", CompanyId));
					Params.Add(new FbParameterItem("LoginId", ClientId));
					Params.Add(new FbParameterItem("BankId", BankId));
					Params.Add(new FbParameterItem("BranchId", BranchId));
					Params.Add(new FbParameterItem("ManagerId", ManagerId));
					Params.Add(new FbParameterItem("DisplayType", DisplayType));
					Params.Add(new FbParameterItem("ExpenseId", ExpenseId));
					Params.Add(new FbParameterItem("AssignedToId", AssignedToId));
					Params.Add(new FbParameterItem("DateFrom", DateFrom));
					Params.Add(new FbParameterItem("DateTo", DateTo));
					
					Params.Add(new FbParameterItem("CreditorId", CreditorId));
					Params.Add(new FbParameterItem("TransactionId", TransactionId));
					Params.Add(new FbParameterItem("AccountId", AccountId));
					Params.Add(new FbParameterItem("LedgerType", LedgerType));
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				}

				DataTable dt = null;
				if (string.IsNullOrEmpty(Result))
				{
					try 
					{
						dt = 
							UsefulDBMethods.Helpers.GetFirstDataTableEx(
								ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
					}
					catch (Exception Ex)
					{
						Result = Ex.Message + Environment.NewLine + Ex.InnerException;
					}
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent("RecordCount: " + dt.Rows.Count, Method, 0);
					foreach (DataRow dataRow in dt.Rows)
					{
						Entries.AppendLine((dataRow[0] == DBNull.Value) ? "" : Convert.ToString(dataRow[0]));
					}
					WindowsEventLoggingUtilities.WriteToInfoEvent("Entries.Length: " + Entries.Length, Method, 0);


                    /*
                     * Need to get the ManagerID value from the JSON in the above method
                     * use stored procedure :-               
                     *  SELECT OUTPUTLINE FROM EAMainInvoiceScreenGroupNew(1068, 8, 0, 0, 0, -1, 0, 0, '04/28/2021', '04/28/2021', 0, 0, 0, 0)               
                    */
                    string NewManagerId = "";
                    if (string.IsNullOrEmpty(Result))
                    {
                        string Str =
                            JsonUtilities.StartJson() +
                            Entries.ToString() +
                            JsonUtilities.EndJson();
                        NewManagerId = JsonUtilities.GetJsonPathValueStr(Str, "$.ManagerID", out Result);
                    }

                    if (string.IsNullOrEmpty(NewManagerId)) NewManagerId = "-1";
                    WindowsEventLoggingUtilities.WriteToInfoEvent("Calculated ManagerID: " + NewManagerId, Method, 0);
                }
            }
			catch (Exception Ex)
			{
				Result = Method + Environment.NewLine + Ex.Message + Environment.NewLine + Ex.InnerException;
			}
			
			return Result;
		}
		
		public static string GetInvoicesGridSql(string ConnectionString,
		                                          Int64 CompanyId,
                                                  Int64 ClientId,
		                                          int BankId,
		                                          int BranchId,
		                                          int ManagerId,
		                                          int DisplayType,
		                                          int TransactionId,
		                                          int AccountId,
		                                          int ChartOfAccountId,
		                                          string DateFrom,
		                                          string DateTo,
		                                          int AssignedToId,
		                                          int CreditorId,
		                                          int LedgerType,
		                                          out string ErrorMsg)
		{
			string Result = "";
			ErrorMsg = "";
			string Method = "GetInvoicesGridSql";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			try 
			{
				string QueryText = "";
				List<FbParameterItem> Params = null;
				if (string.IsNullOrEmpty(ErrorMsg))
				{				
					QueryText = 
						" SELECT SQLSCREEN FROM EAListInvoicesNew(@CompanyId, @BankId, @BranchId, @ManagerId, @DisplayType," +
						" @TransactionSearch, @AccountSearch, @ChartOfAccountId, @DateFrom, @DateTo, @AssignedToId," +
                        " @CreditorId, @LedgerType, @LoginId)";
					Params = new List<FbParameterItem>();
					Params.Add(new FbParameterItem("CompanyId", CompanyId));
					Params.Add(new FbParameterItem("BankId", BankId));
					Params.Add(new FbParameterItem("BranchId", BranchId));
					Params.Add(new FbParameterItem("ManagerId", ManagerId));
					Params.Add(new FbParameterItem("DisplayType", DisplayType));
	
					Params.Add(new FbParameterItem("TransactionSearch", TransactionId));
					Params.Add(new FbParameterItem("AccountSearch", AccountId));
					
					Params.Add(new FbParameterItem("ChartOfAccountId", ChartOfAccountId));
					Params.Add(new FbParameterItem("DateFrom", DateFrom));
					Params.Add(new FbParameterItem("DateTo", DateTo));
					Params.Add(new FbParameterItem("AssignedToId", AssignedToId));
					Params.Add(new FbParameterItem("CreditorId", CreditorId));
					Params.Add(new FbParameterItem("LedgerType", LedgerType));
                    Params.Add(new FbParameterItem("LoginId", ClientId));
                    ErrorMsg = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				}
				
				DataTable dt = null;
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					try 
					{
						//WindowsEventLoggingUtilities.WriteToInfoEvent("ParamCount: " + Params.Count, Method, 0);
						dt = 
							UsefulDBMethods.Helpers.GetFirstDataTableEx(
								ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out ErrorMsg);
						//WindowsEventLoggingUtilities.WriteToInfoEvent("RowCount: " + dt.Rows.Count, Method, 0);
						//WindowsEventLoggingUtilities.WriteToInfoEvent("ErrorMsg:" + Environment.NewLine + ErrorMsg, Method, 0);						
						WindowsEventLoggingUtilities.WriteToInfoEvent("Error getting the SqlScreen:" + Environment.NewLine + ErrorMsg, Method, 0);
						
					}
					catch (Exception Ex)
					{
						WindowsEventLoggingUtilities.WriteToInfoEvent("Exception getting the SqlScreen:" + Environment.NewLine + ErrorMsg, Method, 0);
						ErrorMsg = Ex.Message + Environment.NewLine + Ex.InnerException;
					}
				}
				
				//if (string.IsNullOrEmpty(ErrorMsg))
				//{
					object Obj = null;
					if ((string.IsNullOrEmpty(ErrorMsg)) && (dt.Rows.Count > 0))
					{
						Obj = dt.Rows[0]["SqlScreen"];
						Result = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
						Result = Result.Replace("\"", "'");
						WindowsEventLoggingUtilities.WriteToInfoEvent("SqlScreen QueryText:" + Environment.NewLine + Result, Method, 0);
					}
				//}
			}
			catch (Exception Ex)
			{
				ErrorMsg = Method + Environment.NewLine + Ex.Message + Environment.NewLine + Ex.InnerException;
			}
			
			return Result;
		}
		
		
		public static string SetupPageGridEntries(string ConnectionString,
		                                          Int64 CompanyId,
                                                  Int64 ClientId,
		                                          int BankId,
		                                          int BranchId,
		                                          int ManagerId,
		                                          int DisplayType,
		                                          int TransactionId,
		                                          int AccountId,
		                                          int ChartOfAccountId,
		                                          string DateFrom,
		                                          string DateTo,
		                                          int AssignedToId,
		                                          int CreditorId,
		                                          int LedgerType,
		                                          ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "SetupPageGridEntries";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			try 
			{
				Result = EAInvoicePayCheck(ConnectionString, CompanyId);
				
				if (string.IsNullOrEmpty(Result))
				{
					UsefulDBMethods.Helpers.RunEAMainOfficeProcess(ConnectionString, CompanyId);
				}
				
				string QueryText = 
					GetInvoicesGridSql(
						ConnectionString, CompanyId, ClientId, BankId, BranchId, ManagerId, DisplayType,
						TransactionId, AccountId, ChartOfAccountId, DateFrom, DateTo,
						AssignedToId, CreditorId, LedgerType, out Result);
				
				DataTable dt = null;
				if ((string.IsNullOrEmpty(Result)) && (!string.IsNullOrEmpty(QueryText)))
				{
					dt = 
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, QueryText, null, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
                    if (Entries.Length > 0) Entries.Append(",");
					Result = CommonUtilities.JsonUtilities.BuildJsonArrayFromDataTable(dt, "gridEntries", ref Entries);
				}
			}
			catch (Exception Ex)
			{
				Result = Method + Environment.NewLine + Ex.Message + Environment.NewLine + Ex.InnerException;
			}
			
			return Result;
		}
		
		public static string LoadInvoicesOnChangeRequestItem(ActionBase ActionBase,
		                                                     ref InvoicesOnChangeRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadInvoicesOnChangeRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				Object Obj = ActionBase.GetCollectionValue("Data", "OnChangeType");
				TempItem.OnChangeType = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "PassedId");
				TempItem.PassedIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
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
	
		public static string EAInvoicePayCheck(string ConnectionString,
		                                       Int64 CompanyId)
		{
			string Result = "";
			string Method = "EAInvoicePayCheck";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			try 
			{
				string QueryText = "EXECUTE PROCEDURE EAInvoicePayCheck(@CompanyId)";				
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
				}
			}
			catch (Exception Ex)
			{
				Result = Method + Environment.NewLine + Ex.Message + Environment.NewLine + Ex.InnerException; 
			}
			
			return Result;
		}	
		
		public static string TransferFundsProcess(string ConnectionString,
		                                          Int64 CompanyId,
		                                          int AddedById,
		                                          string SqlScreen,
		                                          Int64 TransactionId,
		                                          string TransferDate)
		{
			string Result = "";
			string Method = "TransferFundsProcess";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			try 
			{
				Result = EAInvoicePayCheck(ConnectionString, CompanyId);
				if (string.IsNullOrEmpty(Result))
				{
					Result = UsefulDBMethods.Helpers.RunEAMainOfficeProcess(ConnectionString, CompanyId);
				}
				string QueryText = "";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = "EXECUTE PROCEDURE EAInvoiceTransferRunAdd(@CompanyId, @AddedById, @SqlScreen, @TransactionId, @TransferDate)";
					Params.Add(new FbParameterItem("CompanyId", CompanyId));
					Params.Add(new FbParameterItem("AddedById", AddedById));
					Params.Add(new FbParameterItem("SqlScreen", SqlScreen));
					Params.Add(new FbParameterItem("TransactionId", TransactionId));
					Params.Add(new FbParameterItem("TransferDate", TransferDate));
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
				}
			}
			catch (Exception Ex)
			{
				Result = Method + Environment.NewLine + Ex.Message + Environment.NewLine + Ex.InnerException; 
			}
			
			return Result;
		}
		
		public static string LoadInvoicesTransferRequestItem(ActionBase ActionBase,
		                                                     ref InvoicesTransferRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadInvoicesTransferRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				Object Obj = ActionBase.GetCollectionValue("Data", "BankId");
				TempItem.BankIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "OfficeId");
				TempItem.BranchIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "ManagerId");
				TempItem.ManagerIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "DisplayType");
				TempItem.DisplayTypeValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "ExpenseId");
				TempItem.ExpenseIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "AssignedToId");
				TempItem.AssignedToIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "DateFrom");
				TempItem.DateFrom = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "DateTo");
				TempItem.DateTo = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "CreditorId");
				TempItem.CreditorIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "TransactionId");
				TempItem.TransactionIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "AccountId");
				TempItem.AccountIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "LedgerType");
				TempItem.LedgerTypeValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "PassedId");
				TempItem.PassedIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "TransferDate");
				TempItem.TransferDate = (Obj == null) ? "" : Convert.ToString(Obj); 
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
		
		public static string EATransferReverse(string ConnectionString,
		                                       Int64 CompanyId,
		                                       Int64 ClientId,
		                                       Int64 UniqueId,
		                                       Int64 NewId)
		{
			string Result = "";
			string Method = "EATransferReverse";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			try 
			{
				string QueryText = "EXECUTE PROCEDURE EATransferReverse(@CompanyId, @UniqueId, @AddedById, @NewId)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				Params.Add(new FbParameterItem("UniqueId", UniqueId));
				Params.Add(new FbParameterItem("AddedById", ClientId));
				Params.Add(new FbParameterItem("NewId", NewId));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
				}
			}
			catch (Exception Ex)
			{
				Result = Method + Environment.NewLine + Ex.Message + Environment.NewLine + Ex.InnerException; 
			}
			
			return Result;
		}
		
		public static string GetEACashBookReverseMessage(string ConnectionString,
		                                                 Int64 NewId)
		{
			string Result = "";
			string Method = "GetEACashBookReverseMessage";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			try 
			{
				string QueryText =
					"SELECT First 1 B_CBR_Proceed, CBR_Message FROM EACashbookReverse WHERE I_CBR_ID = @NewId";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("NewId", NewId));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				
				DataTable tbl = null;
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
				}
				
				if (string.IsNullOrEmpty(Result) && (tbl.Rows.Count > 0))
				{
					DataRow row = tbl.Rows[0];
					object Obj = row["B_CBR_Proceed"];
					string Proceed = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);					
					WindowsEventLoggingUtilities.WriteToInfoEvent("Proceed: " + Proceed, Method, 0);
					Obj = row["CBR_Message"];
					string Msg = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					WindowsEventLoggingUtilities.WriteToInfoEvent("Message: " + Msg, Method, 0);
					if (Proceed == "N") Result = Msg;
				}
			}
			catch (Exception Ex)
			{
				Result = Method + Environment.NewLine + Ex.Message + Environment.NewLine + Ex.InnerException; 
			}
			
			return Result;
		}
		
		public static string LoadInvoicesReverseTransferRequestItem(ActionBase ActionBase,
		                                                            ref InvoicesReverseTransferRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadInvoicesTransferRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				Object Obj = ActionBase.GetCollectionValue("Data", "BankId");
				TempItem.BankIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "OfficeId");
				TempItem.BranchIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "ManagerId");
				TempItem.ManagerIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "DisplayType");
				TempItem.DisplayTypeValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "ExpenseId");
				TempItem.ExpenseIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "AssignedToId");
				TempItem.AssignedToIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "DateFrom");
				TempItem.DateFrom = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "DateTo");
				TempItem.DateTo = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "CreditorId");
				TempItem.CreditorIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "TransactionId");
				TempItem.TransactionIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "AccountId");
				TempItem.AccountIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "LedgerType");
				TempItem.LedgerTypeValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "PassedId");
				TempItem.PassedIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
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
		
		public static string LoadGetSystemReportRequestItem(ActionBase ActionBase,
		                                                    ref GetSystemReportRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadSetupPageEntriesRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				Object Obj = ActionBase.GetCollectionValue("Data", "BankId");
				TempItem.BankIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "OfficeId");
				TempItem.BranchIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "ManagerId");
				TempItem.ManagerIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "DisplayType");
				TempItem.DisplayTypeValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "ExpenseId");
				TempItem.ExpenseIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "AssignedToId");
				TempItem.AssignedToIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "DateFrom");
				TempItem.DateFrom = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "DateTo");
				TempItem.DateTo = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "CreditorId");
				TempItem.CreditorIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "TransactionId");
				TempItem.TransactionIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "AccountId");
				TempItem.AccountIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "LedgerType");
				TempItem.LedgerTypeValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "ReportId");
				TempItem.ReportIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "ButtonId");
				TempItem.ButtonIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
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
		
		public static string LoadInvoicesReverseCheckRequestItem(ActionBase ActionBase,
		                                                         ref InvoicesReverseCheckRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadInvoicesReverseCheckRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				object Obj = ActionBase.GetCollectionValue("Data", "PassedId");
				TempItem.PassedIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
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
		
		public static string EAInvoiceReverseEventAdd(string ConnectionString,
		                                              Int64 CompanyId,
		                                              Int64 NewId,
		                                              Int64 AddedById,
		                                              Int64 UniqueId,
		                                              string ReverseDate,
		                                              string ReverseInvoice,
		                                              string ReverseCharges)
		{
			string Result = "";
			string Method = "EAInvoiceReverseEventAdd";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			try 
			{
				string QueryText =
					//"EXECUTE PROCEDURE EAInvoiceReverseEventAdd(" +
					"EXECUTE PROCEDURE EAInvoiceReversEventAdd(" +
					"  @CompanyId, @Id, @AddedById, @UniqueId, @ReverseDate, @ReverseInvoice, @ReverseCharges)";				
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				Params.Add(new FbParameterItem("Id", NewId));
				Params.Add(new FbParameterItem("AddedById", AddedById));
				Params.Add(new FbParameterItem("UniqueId", UniqueId));
				Params.Add(new FbParameterItem("ReverseDate", ReverseDate));
				Params.Add(new FbParameterItem("ReverseInvoice", ReverseInvoice));
				Params.Add(new FbParameterItem("ReverseCharges", ReverseCharges));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
				}
			}
			catch (Exception Ex)
			{
				Result = Method + Environment.NewLine + Ex.Message + Environment.NewLine + Ex.InnerException; 
			}
			
			return Result;
		}
		
		public static string EAInvoiceReverseEventView(string ConnectionString,
		                                               Int64 CompanyId,
		                                               Int64 NewId,
		                                               ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "EAInvoiceReverseEventView";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			try 
			{
				string QueryText =
					"SELECT outputline from EAInvoiceReverseEventView(@CompanyId, @NewId)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				Params.Add(new FbParameterItem("NewId", NewId));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				
				DataTable tbl = null;
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					foreach (DataRow dataRow in tbl.Rows)
					{
						Entries.AppendLine((dataRow[0] == DBNull.Value) ? "" : Convert.ToString(dataRow[0]));
					}
				}
			}
			catch (Exception Ex)
			{
				Result = Method + Environment.NewLine + Ex.Message + Environment.NewLine + Ex.InnerException; 
				WindowsEventLoggingUtilities.WriteToInfoEvent("Exception " + Result, Method, 0);
			}
			finally
			{
				WindowsEventLoggingUtilities.WriteToInfoEvent("Leaving " + Method, Method, 0);
			}
			
			return Result;
		}
		
		public static string LoadInvoiceReverseProcessRequestItem(ActionBase ActionBase,
		                                                          ref InvoiceReverseProcessRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadInvoiceReverseProcessRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				Object Obj = ActionBase.GetCollectionValue("Data", "ProcessId");
				TempItem.ProcessIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "PassedId");
				TempItem.PassedIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "ReverseDate");
				TempItem.ReverseDate = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "ReverseInvoice");
				TempItem.ReverseInvoice = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "ReverseDebt");
				TempItem.ReverseDebt = (Obj == null) ? "" : Convert.ToString(Obj);
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
		
		public static string EAInvoicesImportGroup(string ConnectionString,
		                                           Int64 CompanyId,
		                                           Int64 ClientId,
		                                           string InputDirectory,
		                                           ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "EAInvoicesImportGroup";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string QueryText = "SELECT OutputLine FROM EAInvoicesImportGroup(@CompanyId, @LoginId, @InputDirectory)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				Params.Add(new FbParameterItem("LoginId", ClientId));
				Params.Add(new FbParameterItem("InputDirectory", InputDirectory));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				
				DataTable tbl = null;
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					foreach (DataRow dataRow in tbl.Rows)
					{
						Entries.AppendLine((dataRow[0] == DBNull.Value) ? "" : Convert.ToString(dataRow[0]));
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


        public static string SetupPageEntries(string ConnectionString,
                                              int CompanyId,
                                              int ClientId,
                                              SetupPageEntriesRequestItem Request,
                                              ref StringBuilder Entries)
        {
            string Result = "";
            string Method = "SetupPageEntries";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                string QueryText = "";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    Result =
                        EAMainInvoiceScreenGroup(
                            ConnectionString, CompanyId, ClientId,
                            Request.BankId, Request.BranchId, Request.ManagerId, Request.DisplayType,
                            Request.ExpenseId, Request.AssignedToId, Request.DateFrom, Request.DateTo,
                            Request.CreditorId, Request.TransactionId, Request.AccountId,
                            Request.LedgerType, ref Entries);
                }

                /*
                 * Need to get the ManagerID value from the JSON in the above method
                 * use stored procedure :-               
                 *  SELECT OUTPUTLINE FROM EAMainInvoiceScreenGroupNew(1068, 8, 0, 0, 0, -1, 0, 0, '04/28/2021', '04/28/2021', 0, 0, 0, 0)               
                */
                string ManagerId = "";
                if (string.IsNullOrEmpty(Result))
                {
                    string Str =
                        JsonUtilities.StartJson() +
                        Entries.ToString() +
                        JsonUtilities.EndJson();
                    ManagerId = JsonUtilities.GetJsonPathValueStr(Str, "$.ManagerID", out Result);
                }

                if (string.IsNullOrEmpty(ManagerId)) ManagerId = "-1";
                WindowsEventLoggingUtilities.WriteToInfoEvent("Calculated ManagerID: " + ManagerId, Method, 0);

                if (string.IsNullOrEmpty(Result))
                {
                    Result =
                        SetupPageGridEntries(
                            ConnectionString, CompanyId, ClientId,
                            //Request.BankId, Request.BranchId, Request.ManagerId, Request.DisplayType,
                            Request.BankId, Request.BranchId, Convert.ToInt32(ManagerId), Request.DisplayType,
                            Request.TransactionId, Request.AccountId, Request.ExpenseId,
                            Request.DateFrom, Request.DateTo, Request.AssignedToId,
                            Request.CreditorId, Request.LedgerType, ref Entries);
                }
            }
            catch (Exception Ex)
            {
                Result = ActionBase.LogOnException(Method, Ex, "");
            }
            finally
            {
                ActionBase.LogOnLeavingMethod(Method);
                WindowsEventLoggingUtilities.WriteToInfoEvent("After SetupPageEntries Result: " + Result, Method, 0);
            }

            return Result;
        }
    }
}
