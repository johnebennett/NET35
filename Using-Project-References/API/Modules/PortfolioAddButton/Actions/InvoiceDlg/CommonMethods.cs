/*
 * Created by SharpDevelop.
 * User: John
 * Date: 26/06/2020
 * Time: 12:31 PM
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
namespace Actions.PortfolioAddButton.InvoiceDlg
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
		
		public static string EAInvoiceAddEditGroup(string ConnectionString,
		                                    int CompanyId,
		                                    int ClientId,
		                                    int TransactionId,
		                                    int InvoiceId,
		                                    ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "EAInvoiceAddEditGroup";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string QueryText = 
					" SELECT OutputLine FROM EAInvoiceAddEditGroup(@CompanyId, @TransactionId, @LoginId, @InvoiceId)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				Params.Add(new FbParameterItem("TransactionId", TransactionId));
				Params.Add(new FbParameterItem("LoginId", ClientId));
				Params.Add(new FbParameterItem("InvoiceId", InvoiceId));
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("After EAInvoiceAddEditGroup Result: " + Result, Method, 0);
			}
			
			return Result;
		}		
		
		public static string LoadReverseInvoiceRequestItem(ActionBase ActionBase,
		                                                   ref ReverseInvoiceRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadReverseInvoiceRequestItem";
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
	                
					Object Obj = ActionBase.GetCollectionValue("Data", "ReverseDate");
					TempItem.ReverseDate = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "ReverseInvoice");
					TempItem.ReverseInvoice = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "ReverseCharge");
					TempItem.ReverseCharge = (Obj == null) ? "" : Convert.ToString(Obj);
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
		
		public static string ReverseInvoice(string ConnectionString,
		                                    int CompanyId,
		                                    int ClientId,
		                                    ReverseInvoiceRequestItem Request)
		                                    
		{
			string Result = "";
			string Method = "ReverseInvoice";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				StringBuilder Entries = new StringBuilder();
				if (string.IsNullOrEmpty(Result))
				{
					Result = EAInvoiceAddEditGroup(ConnectionString, CompanyId, ClientId, Request.PortfolioId, Request.GridId, ref Entries);
				}
				
				string Value = "";
				JObject JSONSource = null;
				if (string.IsNullOrEmpty(Result))
				{
					Value =
						JsonUtilities.StartJson() +
						Entries.ToString() +
						JsonUtilities.EndJson();
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(Value)) Result = "Cannot determine the Group entries";
					
					if (string.IsNullOrEmpty(Result))
					{
						JSONSource = JObject.Parse(Value);
						if (JSONSource == null) Result = "Cannot parse the Group source";
					}
				}
				
				Value = "";
				string QueryText = "";
				Int64 NewId = 0;
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.ReverseStartSQL1", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the First query";
					if (string.IsNullOrEmpty(Result))
					{
						Result = GetNewIdForQuery(ConnectionString, QueryText, out NewId);
					}
				}
				
				QueryText = "";
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.ReverseStartSQL2", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the Second query";
					if (string.IsNullOrEmpty(Result))
					{
						Result = ExecuteReverseStartSql2(ConnectionString, QueryText, Request, NewId);
					}
				}
				
				QueryText = "";
				string Reversable = "";
				string Reason = "";
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.ReverseCheckSQL", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the Reverse Check query";
					if (string.IsNullOrEmpty(Result))
					{
						Result = ReverseCheck(ConnectionString, QueryText, out Reversable, out Reason);
					}
					
					if (string.IsNullOrEmpty(Result) && !string.IsNullOrEmpty(Reason)) Result = Reason;
				}
				
				QueryText = "";
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.ReversePostSQL", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the Post query";
					if (string.IsNullOrEmpty(Result))
					{
						QueryText = QueryText.Replace("__", "'");
						List<FbParameterItem> Params = new List<FbParameterItem>();
						Params.Add(new FbParameterItem("reversecharge", Request.ReverseCharge));
						Params.Add(new FbParameterItem("reverseinvoice", Request.ReverseInvoice));
						Params.Add(new FbParameterItem("CC", NewId));
						Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
						
						if (string.IsNullOrEmpty(Result))
						{
							Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
						}
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("After ReverseInvoice Result: " + Result, Method, 0);
			}
			
			return Result;
		}
		
		private static string GetNewIdForQuery(string ConnectionString,
		                                       string QueryText,
		                                       out Int64 NewId)
		{
			string Result = "";
			string Method = "GetNewIdForQuery";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			NewId = 0;
			object Obj = null;
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, null, Method, 0);
				if (string.IsNullOrEmpty(Result))
				{
					DataTable tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(ConnectionString, QueryText, null, 0, FirebirdQueryCheckTypes.HasTables, out Result);
					if (string.IsNullOrEmpty(Result))
					{
						foreach (DataRow Row in tbl.Rows)
						{
							Obj = Row[0];
							NewId = (Obj == DBNull.Value) ? 0 : Convert.ToInt64(Obj);
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("After GetNewIdForQuery Result: " + Result, Method, 0);
			}
			
			return Result;
		}
		
		private static string ExecuteReverseStartSql2(string ConnectionString, 
		                                              string QueryText,
		                                              ReverseInvoiceRequestItem Request,
		                                              Int64 NewId)
		{
			string Result = "";
			string Method = "ExecuteReverseStartSql2";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					Params.Add(new FbParameterItem("reversedate", Request.ReverseDate));
					Params.Add(new FbParameterItem("reverseinvoice", Request.ReverseInvoice));
					Params.Add(new FbParameterItem("reversecharge", Request.ReverseCharge));
					Params.Add(new FbParameterItem("CC", NewId));
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				}
				
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("After ExecuteReverseStartSql2 Result: " + Result, Method, 0);
			}
			
			return Result;
		}
		
		private static string ReverseCheck(string ConnectionString,
		                                   string QueryText, 
		                                   out string Reversable, 
		                                   out string Reason)
		{
			string Result = "";
			string Method = "ReverseCheck";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			Reversable = "";
			Reason = "";
			object Obj = null;
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, null, Method, 0);
				if (string.IsNullOrEmpty(Result))
				{
					DataTable tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(ConnectionString, QueryText, null, 0, FirebirdQueryCheckTypes.HasTables, out Result);
					if (string.IsNullOrEmpty(Result))
					{
						foreach (DataRow Row in tbl.Rows)
						{
							Obj = Row["B_AIVR_REVERSABLE"];
							Reversable = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
							Obj = Row["AIVR_REASON"];
							Reason = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("After ReverseCheck Result: " + Result, Method, 0);
			}
			
			return Result;
		}
		
		public static string LoadSaveInvoiceRequestItem(ActionBase ActionBase,
		                                                ref SaveInvoiceRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadSaveInvoiceRequestItem";
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
	                
	                Object Obj = ActionBase.GetCollectionValue("Data", "InvDate");
					TempItem.InvDate = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "InvPayDate");
					TempItem.InvPayDate = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "InvNumber");
					TempItem.InvNumber = (Obj == null) ? "" : Convert.ToString(Obj);

					Obj = ActionBase.GetCollectionValue("Data", "InvBPay");
					TempItem.InvBPay = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "InvDetail");
					TempItem.InvDetail = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "InvAmount");
					TempItem.InvAmountValue = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "InvGST");
					TempItem.InvGSTValue = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "HoldForTenant");
					TempItem.HoldForTenant = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "HoldForManager");
					TempItem.HoldForManager = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "Priority");
					TempItem.PriorityValue = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "ExpenseId");
					TempItem.ExpenseIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "RevenueId");
					TempItem.RevenueIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
                    Obj = ActionBase.GetCollectionValue("Data", "SubLedgerId");
                    TempItem.SubLedgerIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
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
		
		public static string SaveInvoice(string ConnectionString,
		                                    int CompanyId,
		                                    int ClientId,
		                                    SaveInvoiceRequestItem Request)
		                                    
		{
			string Result = "";
			string Method = "SaveInvoice";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				StringBuilder Entries = new StringBuilder();
				if (string.IsNullOrEmpty(Result))
				{
					Result = EAInvoiceAddEditGroup(ConnectionString, CompanyId, ClientId, Request.PortfolioId, Request.GridId, ref Entries);
				}
				
				string Value = "";
				JObject JSONSource = null;
				if (string.IsNullOrEmpty(Result))
				{
					Value =
						JsonUtilities.StartJson() +
						Entries.ToString() +
						JsonUtilities.EndJson();
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(Value)) Result = "Cannot determine the Group entries";
					
					if (string.IsNullOrEmpty(Result))
					{
						JSONSource = JObject.Parse(Value);
						if (JSONSource == null) Result = "Cannot parse the Group source";
					}
				}
				
				Value = "";
				string QueryText = "";
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.SaveSQL", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot determine the Save entry";
				}
				
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = QueryText.Replace("__", "'");
					Params.Add(new FbParameterItem("invoicedate", Request.InvDate));
					Params.Add(new FbParameterItem("INVOICEPAYDATE", Request.InvPayDate));
					Params.Add(new FbParameterItem("INVOICENUMBER", Request.InvNumber));
					Params.Add(new FbParameterItem("INVOICEBPAY", Request.InvBPay));
					Params.Add(new FbParameterItem("INVOICEDETAIL", Request.InvDetail));
					Params.Add(new FbParameterItem("INVOICEAMOUNT", Request.InvAmount));
					Params.Add(new FbParameterItem("INVOICEGSTAMOUNT", Request.InvGST));
					Params.Add(new FbParameterItem("HOLDFORTENANT", Request.HoldForTenant));
					Params.Add(new FbParameterItem("HOLDFORMANAGER", Request.HoldForManager));
					Params.Add(new FbParameterItem("EXPENSEID", Request.ExpenseId));
					Params.Add(new FbParameterItem("REVENUEID", Request.RevenueId));
					Params.Add(new FbParameterItem("PRIORITY", Request.Priority));
                    Params.Add(new FbParameterItem("SUBLEDGERID", Request.SubLedgerId));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
					
					if (string.IsNullOrEmpty(Result))
					{
						Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("After SaveInvoice Result: " + Result, Method, 0);
			}
			
			return Result;
		}
	}
}
