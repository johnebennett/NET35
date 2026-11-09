/*
 * Created by SharpDevelop.
 * User: John
 * Date: 08/05/2020
 * Time: 3:25 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Diagnostics;

using Newtonsoft.Json.Linq;
using CommonUtilities;
using EAOfficeAPI;
//namespace Actions.Receipts.AddDirectReceipt

namespace Actions.Receipts.AddCashChqReceipt
{
	/// <summary>
	/// Description of CommonMethods.
	/// </summary>
	public class CommonMethods
	{
		public static string GetPrefix()
		{
			return "AddCashChqReceipt.";
		}		
		
		public static string LoadSetupPageEntriesRequestItem(ActionBase ActionBase,
		                                                     ref AddCashChqReceipt.SetupPageEntriesRequestItem TempItem)
		{
			string Result = "";
			string Method = GetPrefix() + "LoadSetupPageEntriesRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				Object Obj = ActionBase.GetCollectionValue("Data", "MasterEventId");
				TempItem.MasterEventIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ReceiptType");
				TempItem.ReceiptTypeValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "BankingId");
				TempItem.BankingIdValue = (Obj == null) ? "" : Convert.ToString(Obj);

				Obj = ActionBase.GetCollectionValue("Data", "FindText");
				TempItem.FindText = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "FindId");
				TempItem.FindIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "SubLedgerId");
				TempItem.SubLedgerIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "BankId");
				TempItem.BankIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "RevenueId");
				TempItem.RevenueIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "GST");
				TempItem.GST = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ReceiptAmount");
				TempItem.ReceiptAmountValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ReceiptGSTAmount");
				TempItem.ReceiptGSTAmountValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ReceiptNo");
				TempItem.ReceiptNoValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ReceiptDate");
				TempItem.ReceiptDate = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ReceiptClearance");
				TempItem.ReceiptClearance = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Detail");
				TempItem.Detail = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Payee");
				TempItem.Payee = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "TakenBy");
				TempItem.TakenBy = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ClientName");
				TempItem.ClientName = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Address1");
				TempItem.Address1 = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Address2");
				TempItem.Address2 = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Address3");
				TempItem.Address3 = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Address4");
				TempItem.Address4 = (Obj == null) ? "" : Convert.ToString(Obj);
				
				Obj = ActionBase.GetCollectionValue("Data", "CashAmount");
				TempItem.CashAmountValue = (Obj == null) ? "" : Convert.ToString(Obj);
				
				Obj = ActionBase.GetCollectionValue("Data", "ChqAmount1");
				TempItem.ChqAmount1Value = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ChqBank1");
				TempItem.ChqBank1 = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ChqBranch1");
				TempItem.ChqBranch1 = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ChqDrawer1");
				TempItem.ChqDrawer1 = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ChqNumber1");
				TempItem.ChqNumber1Value = (Obj == null) ? "" : Convert.ToString(Obj);
				
				Obj = ActionBase.GetCollectionValue("Data", "ChqAmount2");
				TempItem.ChqAmount2Value = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ChqBank2");
				TempItem.ChqBank2 = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ChqBranch2");
				TempItem.ChqBranch2 = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ChqDrawer2");
				TempItem.ChqDrawer2 = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ChqNumber2");
				TempItem.ChqNumber2Value = (Obj == null) ? "" : Convert.ToString(Obj);
				
				Obj = ActionBase.GetCollectionValue("Data", "ChqAmount3");
				TempItem.ChqAmount3Value = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ChqBank3");
				TempItem.ChqBank3 = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ChqBranch3");
				TempItem.ChqBranch3 = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ChqDrawer3");
				TempItem.ChqDrawer3 = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ChqNumber3");
				TempItem.ChqNumber3Value = (Obj == null) ? "" : Convert.ToString(Obj);
				
				Obj = ActionBase.GetCollectionValue("Data", "ChqAmount4");
				TempItem.ChqAmount4Value = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ChqBank4");
				TempItem.ChqBank4 = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ChqBranch4");
				TempItem.ChqBranch4 = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ChqDrawer4");
				TempItem.ChqDrawer4 = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ChqNumber4");
				TempItem.ChqNumber4Value = (Obj == null) ? "" : Convert.ToString(Obj);
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
		
		public static string EAReceiptScreenMixAdd(string ConnectionString,
		                                              Int64 CompanyId,
		                                              Int64 ClientId,
		                                              AddCashChqReceipt.SetupPageEntriesRequestItem Item,
		                                              ref StringBuilder Entries)
		{
			string Result = "";
			string Method = GetPrefix() + "EAReceiptScreenMixAdd";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			ActionBase ActionBase = new ActionBase();
			
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string QueryText = 
					"SELECT OutputLine FROM EAReceiptScreenMixAdd(" +
					"  @CompanyId, @ReceiptEventId, @LoginId, @ReceiptType, @BankingId, @FindText, @FindId," +
					"  @SubLedgerId, @BankId, @RevenueId, @GST, @ReceiptAmount, @ReceiptGSTAmount," +
					"  @ReceiptNo, @ReceiptDate, @ReceiptClearance, @Detail, @Payee, @TakenBy, @ClientName," +
					"  @Address1, @Address2, @Address3, @Address4," +
					"  @CashAmount," +
					"  @ChqAmount1, @ChqBank1, @ChqBranch1, @ChqDrawer1, @ChqNumber1," +
					"  @ChqAmount2, @ChqBank2, @ChqBranch2, @ChqDrawer2, @ChqNumber2," +
					"  @ChqAmount3, @ChqBank3, @ChqBranch3, @ChqDrawer3, @ChqNumber3," +
					"  @ChqAmount4, @ChqBank4, @ChqBranch4, @ChqDrawer4, @ChqNumber4)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				Params.Add(new FbParameterItem("ReceiptEventId", Item.MasterEventId));
				Params.Add(new FbParameterItem("LoginId", ClientId));
				Params.Add(new FbParameterItem("ReceiptType", Item.ReceiptType));
				Params.Add(new FbParameterItem("BankingId", Item.BankingIdValue));
				Params.Add(new FbParameterItem("FindText", Item.FindText));
				Params.Add(new FbParameterItem("FindId", Item.FindId));
				
				Params.Add(new FbParameterItem("SubLedgerId", Item.SubLedgerId));
				Params.Add(new FbParameterItem("BankId", Item.BankId));
				Params.Add(new FbParameterItem("RevenueId", Item.RevenueId));
				Params.Add(new FbParameterItem("GST", Item.GST));
				Params.Add(new FbParameterItem("ReceiptAmount", Item.ReceiptAmount));
				Params.Add(new FbParameterItem("ReceiptGSTAmount", Item.ReceiptGSTAmount));
				
				Params.Add(new FbParameterItem("ReceiptNo", Item.ReceiptNo));
				Params.Add(new FbParameterItem("ReceiptDate", Item.ReceiptDate));
				Params.Add(new FbParameterItem("ReceiptClearance", Item.ReceiptClearance));
				Params.Add(new FbParameterItem("Detail", Item.Detail));
				Params.Add(new FbParameterItem("Payee", Item.Payee));
				Params.Add(new FbParameterItem("TakenBy", Item.TakenBy));
				Params.Add(new FbParameterItem("ClientName", Item.ClientName));
				
				Params.Add(new FbParameterItem("Address1", Item.Address1));
				Params.Add(new FbParameterItem("Address2", Item.Address2));
				Params.Add(new FbParameterItem("Address3", Item.Address3));
				Params.Add(new FbParameterItem("Address4", Item.Address4));
				
				Params.Add(new FbParameterItem("CashAmount", Item.CashAmount));
				
				Params.Add(new FbParameterItem("ChqAmount1", Item.ChqAmount1));
				Params.Add(new FbParameterItem("ChqBank1", Item.ChqBank1));
				Params.Add(new FbParameterItem("ChqBranch1", Item.ChqBranch1));
				Params.Add(new FbParameterItem("ChqDrawer1", Item.ChqDrawer1));
				Params.Add(new FbParameterItem("ChqNumber1", Item.ChqNumber1));
				
				Params.Add(new FbParameterItem("ChqAmount2", Item.ChqAmount2));
				Params.Add(new FbParameterItem("ChqBank2", Item.ChqBank2));
				Params.Add(new FbParameterItem("ChqBranch2", Item.ChqBranch2));
				Params.Add(new FbParameterItem("ChqDrawer2", Item.ChqDrawer2));
				Params.Add(new FbParameterItem("ChqNumber2", Item.ChqNumber2));
				
				Params.Add(new FbParameterItem("ChqAmount3", Item.ChqAmount3));
				Params.Add(new FbParameterItem("ChqBank3", Item.ChqBank3));
				Params.Add(new FbParameterItem("ChqBranch3", Item.ChqBranch3));
				Params.Add(new FbParameterItem("ChqDrawer3", Item.ChqDrawer3));
				Params.Add(new FbParameterItem("ChqNumber3", Item.ChqNumber3));
				
				Params.Add(new FbParameterItem("ChqAmount4", Item.ChqAmount4));
				Params.Add(new FbParameterItem("ChqBank4", Item.ChqBank4));
				Params.Add(new FbParameterItem("ChqBranch4", Item.ChqBranch4));
				Params.Add(new FbParameterItem("ChqDrawer4", Item.ChqDrawer4));
				Params.Add(new FbParameterItem("ChqNumber4", Item.ChqNumber4));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				
				DataTable tbl = null;
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
					if (string.IsNullOrEmpty(Result) && (tbl == null)) Result = "Error calling query in " + Method;
				}
				
				if (string.IsNullOrEmpty(Result) && (tbl != null))
				{
					if ((Entries.Length > 0) && (tbl.Rows.Count > 0)) Entries.Append(",");
					foreach(DataRow Row in tbl.Rows)
					{
						Object Obj = Row["OUTPUTLINE"];
						Entries.AppendLine((Obj == DBNull.Value) ? "" : Convert.ToString(Obj));
					}
				}
				
				string GridSql = "";
				if (string.IsNullOrEmpty(Result))
				{
					GridSql = JsonUtilities.GetJsonPathValueStr(Entries.ToString(), "$.GridSQL", out Result);
					//if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(GridSql)) Result = "Cannot find the Grid SQL value";
					WindowsEventLoggingUtilities.WriteToInfoEvent("GridSQL:" + Environment.NewLine + GridSql, Method, 0);
					WindowsEventLoggingUtilities.WriteToInfoEvent("Result:" + Result, Method, 0);
				}
				
				tbl = null;
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
		
		public static string LoadChangeIncludeStatusRequestItem(ActionBase ActionBase,
		                                                        ref AddCashChqReceipt.ChangeIncludeStatusRequestItem TempItem)
		{
			string Result = "";
			string Method = GetPrefix() + "LoadChangeIncludeStatusRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				Object Obj = ActionBase.GetCollectionValue("Data", "MasterEventId");
				TempItem.MasterEventIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ReceiptType");
				TempItem.ReceiptTypeValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "BankingId");
				TempItem.BankingIdValue = (Obj == null) ? "" : Convert.ToString(Obj);

				Obj = ActionBase.GetCollectionValue("Data", "FindText");
				TempItem.FindText = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "FindId");
				TempItem.FindIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "SubLedgerId");
				TempItem.SubLedgerIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "BankId");
				TempItem.BankIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "RevenueId");
				TempItem.RevenueIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "GST");
				TempItem.GST = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ReceiptAmount");
				TempItem.ReceiptAmountValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ReceiptGSTAmount");
				TempItem.ReceiptGSTAmountValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ReceiptNo");
				TempItem.ReceiptNoValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ReceiptDate");
				TempItem.ReceiptDate = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ReceiptClearance");
				TempItem.ReceiptClearance = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Detail");
				TempItem.Detail = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Payee");
				TempItem.Payee = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "TakenBy");
				TempItem.TakenBy = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ClientName");
				TempItem.ClientName = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Address1");
				TempItem.Address1 = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Address2");
				TempItem.Address2 = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Address3");
				TempItem.Address3 = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Address4");
				TempItem.Address4 = (Obj == null) ? "" : Convert.ToString(Obj);
				
				Obj = ActionBase.GetCollectionValue("Data", "CashAmount");
				TempItem.CashAmountValue = (Obj == null) ? "" : Convert.ToString(Obj);
				
				Obj = ActionBase.GetCollectionValue("Data", "ChqAmount1");
				TempItem.ChqAmount1Value = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ChqBank1");
				TempItem.ChqBank1 = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ChqBranch1");
				TempItem.ChqBranch1 = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ChqDrawer1");
				TempItem.ChqDrawer1 = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ChqNumber1");
				TempItem.ChqNumber1Value = (Obj == null) ? "" : Convert.ToString(Obj);
				
				Obj = ActionBase.GetCollectionValue("Data", "ChqAmount2");
				TempItem.ChqAmount2Value = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ChqBank2");
				TempItem.ChqBank2 = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ChqBranch2");
				TempItem.ChqBranch2 = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ChqDrawer2");
				TempItem.ChqDrawer2 = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ChqNumber2");
				TempItem.ChqNumber2Value = (Obj == null) ? "" : Convert.ToString(Obj);
				
				Obj = ActionBase.GetCollectionValue("Data", "ChqAmount3");
				TempItem.ChqAmount3Value = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ChqBank3");
				TempItem.ChqBank3 = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ChqBranch3");
				TempItem.ChqBranch3 = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ChqDrawer3");
				TempItem.ChqDrawer3 = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ChqNumber3");
				TempItem.ChqNumber3Value = (Obj == null) ? "" : Convert.ToString(Obj);
				
				Obj = ActionBase.GetCollectionValue("Data", "ChqAmount4");
				TempItem.ChqAmount4Value = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ChqBank4");
				TempItem.ChqBank4 = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ChqBranch4");
				TempItem.ChqBranch4 = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ChqDrawer4");
				TempItem.ChqDrawer4 = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ChqNumber4");
				TempItem.ChqNumber4Value = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "UniqueId");
				TempItem.UniqueIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
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
		
		/// <summary>
		/// Check to see if the current banking is still valid 
		/// </summary>
		/// <param name="Entries"></param>
		/// <returns>
		/// Returns empty string if everything is OK,
		/// Otherwise returns the description of the current banking state.
		/// </returns>
		public static string GetActiveBankingDescription(StringBuilder Entries)
		{
			string Result = "";
			string Method = "GetActiveBankingDescription";
			string ActiveBanking = "N";
			string Description = "";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			ActionBase ActionBase = new ActionBase();
			
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				if (string.IsNullOrEmpty(Result))
				{
					ActiveBanking = JsonUtilities.GetJsonPathValueStr(Entries.ToString(), "$.ActiveBanking", out Result);
					if (string.IsNullOrEmpty(Result) && (StringUtilities.SameText(ActiveBanking, "N")))
					{
						Description = JsonUtilities.GetJsonPathValueStr(Entries.ToString(), "$.ActiveBankingdescription", out Result);
						if (Description == "") Description = "Unknown Reason";
						if (string.IsNullOrEmpty(Result)) Result = Description;
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
	}
}
