/*
 * Created by SharpDevelop.
 * User: John
 * Date: 22/06/2020
 * Time: 9:05 AM
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
using UsefulDBMethods;
namespace Actions.PortfolioAddButton.SubLedgerDlg
{
	/// <summary>
	/// Description of CommonMethods.
	/// </summary>
	public class CommonMethods
	{
		public CommonMethods()
		{
		}
		
		public static string LoadSetupPageEntriesRequestItem(ActionBase ActionBase,
		                                                     ref SetupPageEntriesRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadSetupPageEntriesRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				Object Obj = ActionBase.GetCollectionValue("Data", "PortfolioId");
				TempItem.PortfolioIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Option1Id");
				TempItem.Option1IdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				//WindowsEventLoggingUtilities.WriteToInfoEvent("Option1IdValue: " + TempItem.Option1IdValue, Method, 0);
				Obj = ActionBase.GetCollectionValue("Data", "Option2Id");
				TempItem.Option2IdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				//WindowsEventLoggingUtilities.WriteToInfoEvent("Option2IdValue: " + TempItem.Option2IdValue, Method, 0);
				Obj = ActionBase.GetCollectionValue("Data", "GridId");
				TempItem.GridIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
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
		
		public static string EASubLedgerAddEditGroup(string ConnectionString,
		                                             int CompanyId,
		                                             int ClientId,
		                                             int TransactionId,
		                                             int SubLedgerId,
		                                             ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "EASubLedgerAddEditGroup";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string QueryText = 
					" SELECT OutputLine FROM EASubLedgerAddEditGroup(@CompanyId, @TransactionId, @SubLedgerId, @LoginId)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				Params.Add(new FbParameterItem("TransactionId", TransactionId));
				Params.Add(new FbParameterItem("SubLedgerId", SubLedgerId));
				Params.Add(new FbParameterItem("LoginId", ClientId));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				
				DataTable tbl = null;
				object Obj = null;
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, QueryText, Params, 0, FirebirdQueryCheckTypes.HasTables, out Result);
					if (string.IsNullOrEmpty(Result))
					{
						if ((Entries.Length > 0) && (tbl.Rows.Count > 0)) Entries.Append(",");
						foreach (DataRow Row in tbl.Rows)
						{
							Obj = Row["OUTPUTLINE"];
							Entries.AppendLine((Obj == DBNull.Value) ? "" : Convert.ToString(Obj));
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("After EASubLedgerAddEditGroup Result: " + Result, Method, 0);
			}
			
			return Result;
		}
		
		public static string LoadSaveCmdRequestItem(ActionBase ActionBase,
		                                            ref SaveCmdRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadSetupPageEntriesRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				SetupPageEntriesRequestItem Item = new SetupPageEntriesRequestItem();
				Result = Item.Load(ActionBase);
				if (string.IsNullOrEmpty(Result))
				{
					TempItem.PortfolioIdValue = Item.PortfolioIdValue;
	                TempItem.Option1IdValue = Item.Option1IdValue;
	                TempItem.Option2IdValue = Item.Option2IdValue;
	                TempItem.GridIdValue = Item.GridIdValue;
                
					Object Obj = ActionBase.GetCollectionValue("Data", "BankId");
					TempItem.BankIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "SubLedgerName");
					TempItem.SubLedgerName = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "RunNumber");
					TempItem.RunNumberValue = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "ChargeStmtFee");
					TempItem.ChargeStmtFee = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "StmtFee");
					TempItem.StmtFeeValue = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "HoldOverAmount");
					TempItem.HoldOverAmountValue = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "HoldOverReason");
					TempItem.HoldOverReason = (Obj == null) ? "" : Convert.ToString(Obj);					
					Obj = ActionBase.GetCollectionValue("Data", "StatementName");
					TempItem.StatementName = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "StatementAddress1");
					TempItem.StatementAddress1 = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "StatementAddress2");
					TempItem.StatementAddress2 = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "StatementAddress3");
					TempItem.StatementAddress3 = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "StatementAddress4");
					TempItem.StatementAddress4 = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "BSB");
					TempItem.BSB = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "AccountNumber");
					TempItem.AccountNumber = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "ABN");
					TempItem.ABN = (Obj == null) ? "" : Convert.ToString(Obj);

					Obj = ActionBase.GetCollectionValue("Data", "CanChangeStmt");
					TempItem.CanChangeStmt = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "CanChangeBSB");
					TempItem.CanChangeBSB = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "ExpenseId");
					TempItem.ExpenseIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "RevenueId");
					TempItem.RevenueIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "ToStmtFeeId");
					TempItem.ToStmtFeeIdValue = (Obj == null) ? "" : Convert.ToString(Obj);

                    Obj = ActionBase.GetCollectionValue("Data", "PrepareStatement");
                    TempItem.PrepareStatement = (Obj == null) ? "" : Convert.ToString(Obj);

                    Obj = ActionBase.GetCollectionValue("Data", "AccountOwner");
                    TempItem.AccountOwner = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "PropertyDisplay");
                    TempItem.PropertyDisplay = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "ExcludeFromEOYRun");
                    TempItem.ExcludeFromEOYRun = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "DoNotInvoiceEOYRun");
                    TempItem.DoNotInvoiceEOYRun = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "UseBroadStatement");
                    TempItem.UseBroadStatement = (Obj == null) ? "" : Convert.ToString(Obj);
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
		
		public static string SaveCmd(string ConnectionString,
		                             int CompanyId,
		                             int ClientId,
		                             SaveCmdRequestItem Request)
		{
			string Result = "";
			string Method = "SaveCmd";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				StringBuilder Entries = new StringBuilder();
				Result = 
					EASubLedgerAddEditGroup(
						ConnectionString, CompanyId, ClientId, 
						Request.PortfolioId, Request.GridId, ref Entries);
				
				string Source = "";
				string SaveSql = "";
				if (string.IsNullOrEmpty(Result))
				{
					Source =
						JsonUtilities.StartJson() +
						Entries.ToString() +
						JsonUtilities.EndJson();
					SaveSql = JsonUtilities.GetJsonPathValueStr(Source, "$.SaveSQL", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(SaveSql)) Result = "Cannot find the first Save entry";					
				}
				
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					Params.Add(new FbParameterItem("SubLedgerId", Request.GridId));
					Params.Add(new FbParameterItem("bankid", Request.BankId));
					Params.Add(new FbParameterItem("subledgername", Request.SubLedgerName));
					Params.Add(new FbParameterItem("StatementRunNumber", Request.RunNumber));
					Params.Add(new FbParameterItem("chargestatementfee", Request.ChargeStmtFee));
					Params.Add(new FbParameterItem("statementfree", Request.StmtFee));
					Params.Add(new FbParameterItem("heldover", Request.HoldOverAmount));

					Params.Add(new FbParameterItem("heldoverreason", Request.HoldOverReason));
					Params.Add(new FbParameterItem("changestatement", Request.CanChangeStmt));
					Params.Add(new FbParameterItem("changestatementname", Request.StatementName));
					Params.Add(new FbParameterItem("changestatementaddress1", Request.StatementAddress1));
					Params.Add(new FbParameterItem("changestatementaddress2", Request.StatementAddress2));
					Params.Add(new FbParameterItem("changestatementaddress3", Request.StatementAddress3));
					Params.Add(new FbParameterItem("changestatementaddress4", Request.StatementAddress4));

					Params.Add(new FbParameterItem("changebsbdetails", Request.CanChangeBSB));
					Params.Add(new FbParameterItem("changebsb", Request.BSB));
					Params.Add(new FbParameterItem("changeaccountnumber", Request.AccountNumber));
					Params.Add(new FbParameterItem("changeocabn", Request.ABN));
					Params.Add(new FbParameterItem("expenseid", Request.ExpenseId));
					Params.Add(new FbParameterItem("revenueid", Request.RevenueId));
					Params.Add(new FbParameterItem("tostatementfeeid", Request.ToStmtFeeId));
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(SaveSql, Params, Method, 0);

					if (string.IsNullOrEmpty(Result))
					{
						Result = FirebirdUtilities.DBExecute(ConnectionString, SaveSql, Params, 0);
					}
				}

                if (string.IsNullOrEmpty(Result))
                {
                    SaveSql = JsonUtilities.GetJsonPathValueStr(Source, "$.SaveSQL2", out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(SaveSql)) Result = "Cannot find the second Save entry";
                }

                Params.Clear();
                if (string.IsNullOrEmpty(Result))
                {
                    Params.Add(new FbParameterItem("RECEIVESTATEMENT", Request.PrepareStatement));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(SaveSql, Params, Method, 0);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = FirebirdUtilities.DBExecute(ConnectionString, SaveSql, Params, 0);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    SaveSql = JsonUtilities.GetJsonPathValueStr(Source, "$.SaveSQL3", out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(SaveSql)) Result = "Cannot find the third Save entry";
                }

                Params.Clear();
                if (string.IsNullOrEmpty(Result))
                {
                    Params.Add(new FbParameterItem("ACCOUNTOWNER", Request.AccountOwner));
                    Params.Add(new FbParameterItem("PROPERTYDISPLAY", Request.PropertyDisplay));
                    Params.Add(new FbParameterItem("B_ALE_EXCLUDEFROMEOYRUN", Request.ExcludeFromEOYRun));
                    Params.Add(new FbParameterItem("B_ALE_DONOTINVOICEEOYRUN", Request.DoNotInvoiceEOYRun));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(SaveSql, Params, Method, 0);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = FirebirdUtilities.DBExecute(ConnectionString, SaveSql, Params, 0);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    SaveSql = JsonUtilities.GetJsonPathValueStr(Source, "$.SaveSQL4", out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(SaveSql)) Result = "Cannot find the fourth Save entry";
                }

                Params.Clear();
                if (string.IsNullOrEmpty(Result))
                {
                    Params.Add(new FbParameterItem("BROADSTATEMENT", Request.UseBroadStatement));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(SaveSql, Params, Method, 0);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = FirebirdUtilities.DBExecute(ConnectionString, SaveSql, Params, 0);
                }

                if (string.IsNullOrEmpty(Result))
				{
					UsefulDBMethods.Helpers.RunEAMainOfficeProcess(ConnectionString, CompanyId);
					UsefulDBMethods.Helpers.RunEAMainOfficeProcess(ConnectionString, CompanyId);
				}
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
				WindowsEventLoggingUtilities.WriteToInfoEvent("After SaveChanges Result: " + Result, Method, 0);
			}
			
			return Result;
		}
		
		public static string RemoveGridEntry(string ConnectionString,
		                                     int CompanyId,
		                                     int ClientId,
		                                     SetupPageEntriesRequestItem Request)
		{
			string Result = "";
			string Method = "RemoveGridEntry";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				StringBuilder Entries = new StringBuilder();
				Result =
					EASubLedgerAddEditGroup(
						ConnectionString, CompanyId, ClientId, 
						Request.PortfolioId, Request.GridId, ref Entries);
				
				string Source = "";
				string QueryText = "";
				if (string.IsNullOrEmpty(Result))
				{
					Source =
						JsonUtilities.StartJson() +
						Entries.ToString() +
						JsonUtilities.EndJson();
					QueryText = JsonUtilities.GetJsonPathValueStr(Source, "$.RemoveSQL", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the Remove entry";					
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
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
				WindowsEventLoggingUtilities.WriteToInfoEvent("After CreateRevenue Result: " + Result, Method, 0);
			}
			
			return Result;
		}
	}
}
