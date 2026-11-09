/*
 * Created by SharpDevelop.
 * User: John
 * Date: 28/06/2020
 * Time: 3:17 PM
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
namespace Actions.PortfolioAddButton.RecurringInvoicesDlg
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
				Obj = ActionBase.GetCollectionValue("Data", "RevenueId");
				TempItem.RevenueIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "OwnerSubLedgerId");
				TempItem.OwnerSubLedgerIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "CreditorSubLedgerId");
				TempItem.CreditorSubLedgerIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
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
		
		public static string EAInvoiceRecurrAddEditStart(string ConnectionString,
		                                                 int CompanyId,
		                                                 int ClientId,
		                                                 int TransactionId,
		                                                 int InvoiceId,
		                                                 int OwnerSubLedgerId,
		                                                 int FindId,
		                                                 int CreditorSubLedgerId,
		                                                 int ExpenseId,
		                                                 int RevenueId,
		                                                 ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "EAChargeAddEditStart";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string QueryText = 
					"SELECT SqlStatement FROM EAInvoiceRecurrAddEditStart(" +
					"@CompanyId, @TransactionId, @InvoiceId, @OwnerSubLedgerId, @FindId, @CreditorSubLedgerId, @ExpenseId, @LoginId, @RevenueId)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				Params.Add(new FbParameterItem("TransactionId", TransactionId));
				Params.Add(new FbParameterItem("InvoiceId", InvoiceId));
				Params.Add(new FbParameterItem("OwnerSubLedgerId", OwnerSubLedgerId));
				Params.Add(new FbParameterItem("FindId", FindId));
				Params.Add(new FbParameterItem("CreditorSubLedgerId", CreditorSubLedgerId));
				Params.Add(new FbParameterItem("ExpenseId", ExpenseId));
				Params.Add(new FbParameterItem("LoginId", ClientId));
				Params.Add(new FbParameterItem("RevenueId", RevenueId));
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
							Obj = row["SqlStatement"];
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("After EAChargeAddEditStart Result: " + Result, Method, 0);
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
	                TempItem.RevenueIdValue = Item.RevenueIdValue;
	                TempItem.ExpenseIdValue = Item.ExpenseIdValue;
	                TempItem.OwnerSubLedgerIdValue = Item.OwnerSubLedgerIdValue;
	                TempItem.CreditorSubLedgerIdValue = Item.CreditorSubLedgerIdValue;
				}
				Object Obj = ActionBase.GetCollectionValue("Data", "FrequencyId");
				TempItem.FrequencyIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "FreqPeriod");
				TempItem.FreqPeriodValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "InvAmount");
				TempItem.InvAmountValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "GSTAmount");
				TempItem.GSTAmountValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Created");
				TempItem.Created = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "InvDetail");
				TempItem.InvDetail = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "BPAY");
				TempItem.BPAY = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Starts");
				TempItem.Starts = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Stops");
				TempItem.Stops = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "NextIncDate");
				TempItem.NextIncDate = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "NextIncAmount");
				TempItem.NextIncAmountValue = (Obj == null) ? "" : Convert.ToString(Obj);
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
						EAInvoiceRecurrAddEditStart(
							ConnectionString, CompanyId, ClientId, 
							Request.PortfolioId, Request.GridId, Request.OwnerSubLedgerId,
							Request.FindId, Request.CreditorSubLedgerId, Request.ExpenseId,
							Request.RevenueId, ref Entries);
				}
				
				string Source = "";
				string QueryText = "";
				if (string.IsNullOrEmpty(Result))
				{
					Source =
						JsonUtilities.StartJson() +
						Entries.ToString() +
						JsonUtilities.EndJson();
					QueryText = JsonUtilities.GetJsonPathValueStr(Source, "$.SQLActivate", out Result);
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
						EAInvoiceRecurrAddEditStart(
							ConnectionString, CompanyId, ClientId, 
							Request.PortfolioId, Request.GridId, Request.OwnerSubLedgerId,
							Request.FindId, Request.CreditorSubLedgerId, Request.ExpenseId,
							Request.RevenueId, ref Entries);
				}
				
				string Source = "";
				string QueryText = "";
				if (string.IsNullOrEmpty(Result))
				{
					Source =
						JsonUtilities.StartJson() +
						Entries.ToString() +
						JsonUtilities.EndJson();
					QueryText = JsonUtilities.GetJsonPathValueStr(Source, "$.SQLDeactivate", out Result);
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
		
		public static string GenerateCmd(string ConnectionString, 
		                                int CompanyId,
		                                int ClientId,
		                                AllPageEntriesRequestItem Request)
		{
			string Result = "";
			string Method = "GenerateCmd";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				StringBuilder Entries = new StringBuilder();
				if (string.IsNullOrEmpty(Result))
				{
					Result = SaveCmd(ConnectionString, CompanyId, ClientId, Request);
				
					if (string.IsNullOrEmpty(Result))
					{
						UsefulDBMethods.Helpers.RunEAMainOfficeProcess(ConnectionString, CompanyId);
						UsefulDBMethods.Helpers.RunEAMainOfficeProcess(ConnectionString, CompanyId);
					}
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = 
						EAInvoiceRecurrAddEditStart(
							ConnectionString, CompanyId, ClientId, 
							Request.PortfolioId, Request.GridId, Request.OwnerSubLedgerId,
							Request.FindId, Request.CreditorSubLedgerId, Request.ExpenseId,
							Request.RevenueId, ref Entries);
				}
				
				string Source = "";
				string QueryText = "";
				if (string.IsNullOrEmpty(Result))
				{
					Source =
						JsonUtilities.StartJson() +
						Entries.ToString() +
						JsonUtilities.EndJson();
					QueryText = JsonUtilities.GetJsonPathValueStr(Source, "$.GenerateSQL", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the Deactivate entry";					
				}
				
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);

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
				WindowsEventLoggingUtilities.WriteToInfoEvent("After GenerateCmd Result: " + Result, Method, 0);
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
					EAInvoiceRecurrAddEditStart(
						ConnectionString, CompanyId, ClientId, 
						Request.PortfolioId, Request.GridId, Request.OwnerSubLedgerId,
						Request.FindId, Request.CreditorSubLedgerId, Request.ExpenseId,
						Request.RevenueId, ref Entries);
				
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
					Params.Add(new FbParameterItem("invoiceAMOUNT", Request.InvAmount));
					Params.Add(new FbParameterItem("GSTAMOUNT", Request.GSTAmount));
					Params.Add(new FbParameterItem("createdate", Request.Created));
					Params.Add(new FbParameterItem("DETAIL", Request.InvDetail));
					Params.Add(new FbParameterItem("frequency", Request.FrequencyId));
					Params.Add(new FbParameterItem("frequencyperiod", Request.FreqPeriod));
					Params.Add(new FbParameterItem("stopdate", Request.Stops));
					Params.Add(new FbParameterItem("startdate", Request.Starts));
					Params.Add(new FbParameterItem("BPAYNUMBER", Request.BPAY));

					Params.Add(new FbParameterItem("nextincreasedate", Request.NextIncDate));
					Params.Add(new FbParameterItem("nextincreaseamount", Request.NextIncAmount));
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
	}
}
