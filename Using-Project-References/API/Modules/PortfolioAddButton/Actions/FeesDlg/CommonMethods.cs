/*
 * Created by SharpDevelop.
 * User: John
 * Date: 29/06/2020
 * Time: 5:43 PM
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
namespace Actions.PortfolioAddButton.FeesDlg
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
				Obj = ActionBase.GetCollectionValue("Data", "Option2Id");
				TempItem.Option2IdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "GridId");
				TempItem.GridIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "FindId");
				TempItem.FindIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ExpenseId");
				TempItem.ExpenseIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "EventType");
				TempItem.EventTypeValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "SubLedgerId");
				TempItem.SubLedgerIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
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
		
		public static string EASeupFeesAddEditStart(string ConnectionString,
		                                            int CompanyId,
		                                            int ClientId,
		                                            int TransactionId,
		                                            int MasterSubLedgerId,
		                                            int TransactionFeeId,
		                                            int FindId,
		                                            int ExpenseId,
		                                            int EventType,
		                                            int SubLedgerId,
		                                            ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "EASeupFeesAddEditStart";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string QueryText = 
					"SELECT OUTPUTLINE FROM EASeupFeesAddEditStart(" +
					"@CompanyId, @TransactionId, @MasterSubLedgerId, @TransactionFeeId, @FindId, @ExpenseId, @EventType, @LoginId, @SubLedgerId)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				Params.Add(new FbParameterItem("TransactionId", TransactionId));
				Params.Add(new FbParameterItem("MasterSubLedgerId", MasterSubLedgerId));
				Params.Add(new FbParameterItem("TransactionFeeId", TransactionFeeId));
				Params.Add(new FbParameterItem("FindId", FindId));
				Params.Add(new FbParameterItem("ExpenseId", ExpenseId));
				Params.Add(new FbParameterItem("EventType", EventType));
				Params.Add(new FbParameterItem("LoginId", ClientId));
				Params.Add(new FbParameterItem("SubLedgerId", SubLedgerId));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				
				DataTable tbl = null;
				object Obj = null;
				string SqlStatement = "";
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, QueryText, Params, 0, FirebirdQueryCheckTypes.HasTables, out Result);
					if (string.IsNullOrEmpty(Result))
					{
						foreach (DataRow row in tbl.Rows)
						{
							Obj = row["OUTPUTLINE"];
							SqlStatement = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
						}
					}
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(SqlStatement)) Result = "Cannot find the Query value";
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					SqlStatement = SqlStatement.Replace("__", "'");
					WindowsEventLoggingUtilities.WriteToInfoEvent("SqlStatement:" + Environment.NewLine + SqlStatement, Method, 0);
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, SqlStatement, null, 0, FirebirdQueryCheckTypes.HasTables, out Result);
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("After EASeupFeesAddEditStart Result: " + Result, Method, 0);
			}
			
			return Result;
		}
		
		public static string LoadAllPageEntriesRequestItem(ActionBase ActionBase,
		                                                   ref AllPageEntriesRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadAllPageEntriesRequestItem";
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
	                TempItem.FindIdValue = Item.FindIdValue;
	                TempItem.ExpenseIdValue = Item.ExpenseIdValue;
	                TempItem.EventTypeValue = Item.EventTypeValue;
	                TempItem.SubLedgerIdValue = Item.SubLedgerIdValue;
				}

				Object Obj = ActionBase.GetCollectionValue("Data", "CreditorSubLedger");
				TempItem.CreditorSubLedgerValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ChartCode");
				TempItem.ChartCodeValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Exclusion1");
				TempItem.Exclusion1Value = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Exclusion2");
				TempItem.Exclusion2Value = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "InvoiceGSTReceipts");
				TempItem.InvoiceGSTReceipts = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Revenue");
				TempItem.RevenueValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Detail");
				TempItem.Detail = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "PercentRate");
				TempItem.PercentRateValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "FixedAmount");
				TempItem.FixedAmountValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ReverseInvoiceSign");
				TempItem.ReverseInvoiceSign = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "RecallDate");
				TempItem.RecallDate = (Obj == null) ? "" : Convert.ToString(Obj);
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
		                             AllPageEntriesRequestItem Request)
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
					EASeupFeesAddEditStart(
						ConnectionString, CompanyId, ClientId, 
							Request.PortfolioId, Request.Option2Id, Request.GridId, Request.FindId,
							Request.ExpenseId, Request.EventType, Request.SubLedgerId, ref Entries);
				
				string Source = "";
				string QueryText = "";
				if (string.IsNullOrEmpty(Result))
				{
					Source =
						JsonUtilities.StartJson() +
						Entries.ToString() +
						JsonUtilities.EndJson();
					QueryText = JsonUtilities.GetJsonPathValueStr(Source, "$.SaveSQL", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the Save entry";					
				}
				
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					Params.Add(new FbParameterItem("SUBLEDGERID", Request.SubLedgerId));
					Params.Add(new FbParameterItem("FINDID", Request.FindId));
					Params.Add(new FbParameterItem("EXPENSEID", Request.ExpenseId));
					Params.Add(new FbParameterItem("REVENUEID", Request.Revenue));
					Params.Add(new FbParameterItem("TOSUBLEDGERID", Request.CreditorSubLedger));
					Params.Add(new FbParameterItem("CHARTID", Request.ChartCode));
					Params.Add(new FbParameterItem("EVENTTYPE", Request.EventType));
					Params.Add(new FbParameterItem("DETAIL", Request.Detail));
					Params.Add(new FbParameterItem("PERCENTAGERATE", Request.PercentRate));
					Params.Add(new FbParameterItem("FIXEDAMOUNT", Request.FixedAmount));
					Params.Add(new FbParameterItem("ADDASREVERSED", Request.ReverseInvoiceSign));
					Params.Add(new FbParameterItem("EXCLUSION1ID", Request.Exclusion1));
					Params.Add(new FbParameterItem("EXCLUSION2ID", Request.Exclusion2));
					Params.Add(new FbParameterItem("INVOICEGSTRECEIPTS", Request.InvoiceGSTReceipts));
					Params.Add(new FbParameterItem("loginid", ClientId));
					Params.Add(new FbParameterItem("recalldate", Request.RecallDate));
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);

					if (string.IsNullOrEmpty(Result))
					{
						Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
					}
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("After SaveCmd Result: " + Result, Method, 0);
			}
			
			return Result;
		}
		
		public static string CopyCmd(string ConnectionString, 
		                             int CompanyId,
		                             int ClientId,
		                             AllPageEntriesRequestItem Request)
		{
			string Result = "";
			string Method = "CopyCmd";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				StringBuilder Entries = new StringBuilder();
				if (string.IsNullOrEmpty(Result))
				{
					Result = SaveCmd(ConnectionString, CompanyId, ClientId, Request);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = 
						EASeupFeesAddEditStart(
							ConnectionString, CompanyId, ClientId, 
							Request.PortfolioId, Request.Option2Id, Request.GridId, Request.FindId,
							Request.ExpenseId, Request.EventType, Request.SubLedgerId, ref Entries);
				}
				
				string Source = "";
				string QueryText = "";
				if (string.IsNullOrEmpty(Result))
				{
					Source =
						JsonUtilities.StartJson() +
						Entries.ToString() +
						JsonUtilities.EndJson();
					QueryText = JsonUtilities.GetJsonPathValueStr(Source, "$.CopySQL", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the Copy entry";
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("After CopyCmd Result: " + Result, Method, 0);
			}
			
			return Result;
		}
		
		public static string ActivateCmd(string ConnectionString, 
		                                 int CompanyId,
		                                 int ClientId,
		                                 AllPageEntriesRequestItem Request)
		{
			string Result = "";
			string Method = "ActivateCmd";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				StringBuilder Entries = new StringBuilder();
				if (string.IsNullOrEmpty(Result))
				{
					Result = SaveCmd(ConnectionString, CompanyId, ClientId, Request);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = 
						EASeupFeesAddEditStart(
							ConnectionString, CompanyId, ClientId, 
							Request.PortfolioId, Request.Option2Id, Request.GridId, Request.FindId,
							Request.ExpenseId, Request.EventType, Request.SubLedgerId, ref Entries);
				}
				
				string Source = "";
				string QueryText = "";
				if (string.IsNullOrEmpty(Result))
				{
					Source =
						JsonUtilities.StartJson() +
						Entries.ToString() +
						JsonUtilities.EndJson();
					QueryText = JsonUtilities.GetJsonPathValueStr(Source, "$.ActivateSQL", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the Activate entry";					
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = QueryText.Replace("__", "'");
					
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, null, Method, 0);

					if (string.IsNullOrEmpty(Result))
					{
						Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText);
					}
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("After ActivateCmd Result: " + Result, Method, 0);
			}
			
			return Result;
		}
		
		public static string DeactivateCmd(string ConnectionString,
		                                   int CompanyId,
		                                   int ClientId,
		                                   AllPageEntriesRequestItem Request)
		{
			string Result = "";
			string Method = "DeactivateCmd";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				StringBuilder Entries = new StringBuilder();
				if (string.IsNullOrEmpty(Result))
				{
					Result = SaveCmd(ConnectionString, CompanyId, ClientId, Request);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = 
						EASeupFeesAddEditStart(
							ConnectionString, CompanyId, ClientId, 
							Request.PortfolioId, Request.Option2Id, Request.GridId, Request.FindId,
							Request.ExpenseId, Request.EventType, Request.SubLedgerId, ref Entries);
				}
				
				string Source = "";
				string QueryText = "";
				if (string.IsNullOrEmpty(Result))
				{
					Source =
						JsonUtilities.StartJson() +
						Entries.ToString() +
						JsonUtilities.EndJson();
					QueryText = JsonUtilities.GetJsonPathValueStr(Source, "$.DeActivateSQL", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the Deactivate entry";					
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = QueryText.Replace("__", "'");
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, null, Method, 0);

					if (string.IsNullOrEmpty(Result))
					{
						Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText);
					}
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("After DeactivateCmd Result: " + Result, Method, 0);
			}
			
			return Result;
		}
	}
}
