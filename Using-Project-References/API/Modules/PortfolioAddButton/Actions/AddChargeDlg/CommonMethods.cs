/*
 * Created by SharpDevelop.
 * User: John
 * Date: 14/06/2020
 * Time: 4:35 PM
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
namespace Actions.PortfolioAddButton.AddChargeDlg
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
				Obj = ActionBase.GetCollectionValue("Data", "FindId");
				TempItem.FindIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "RevenueId");
				TempItem.RevenueIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "SubLedgerId");
				TempItem.SubLedgerIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "ToDate");
                TempItem.ToDate = (Obj == null) ? "" : Convert.ToString(Obj);
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
		
		public static string EAChargeAddEditStart(string ConnectionString,
		                                          int CompanyId,
		                                          int ClientId,
		                                          int TransactionId,
		                                          int ChargeId,
		                                          int FindId,
		                                          int RevenueId,
		                                          int SubLedgerId,
                                                  string ToDate,
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
					"SELECT SqlStatement FROM EAChargeAddEditStart(" +
                    "@CompanyId, @TransactionId, @ChargeId, @LoginId, @FindId, @RevenueId, @SubLedgerId, @ToDate)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				Params.Add(new FbParameterItem("TransactionId", TransactionId));
				Params.Add(new FbParameterItem("ChargeId", ChargeId));
				Params.Add(new FbParameterItem("LoginId", ClientId));
				Params.Add(new FbParameterItem("FindId", FindId));
				Params.Add(new FbParameterItem("RevenueId", RevenueId));
				Params.Add(new FbParameterItem("SubLedgerId", SubLedgerId));
                Params.Add(new FbParameterItem("ToDate", ToDate));
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

		public static string LoadSaveChangesRequestItem(ActionBase ActionBase,
		                                                ref SaveChangesRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadSaveChangesRequestItem";
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
	                TempItem.SubLedgerIdValue = Item.SubLedgerIdValue;
                    TempItem.ToDate = Item.ToDate;
                }
                Object Obj = ActionBase.GetCollectionValue("Data", "ChargeAmount");
				TempItem.ChargeAmountValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "GSTAmount");
				TempItem.GSTAmountValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "FromDate");
				TempItem.FromDate = (Obj == null) ? "" : Convert.ToString(Obj);
				
                //Obj = ActionBase.GetCollectionValue("Data", "ToDate");
				//TempItem.ToDate = (Obj == null) ? "" : Convert.ToString(Obj);
				              
				Obj = ActionBase.GetCollectionValue("Data", "Detail");
				TempItem.Detail = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "DueDate");
				TempItem.DueDate = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "PrintDate");
				TempItem.PrintDate = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "AmountDue");
				TempItem.AmountDueValue = (Obj == null) ? "" : Convert.ToString(Obj);
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
		
		public static string SaveChanges(string ConnectionString,
		                                 int CompanyId,
		                                 int ClientId,
		                                 SaveChangesRequestItem Request)
		{
			string Result = "";
			string Method = "SaveChanges";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				StringBuilder Entries = new StringBuilder();
				Result = 
					EAChargeAddEditStart(
						ConnectionString, CompanyId, ClientId, Request.PortfolioId, Request.GridId, Request.FindId,
						Request.RevenueId, Request.SubLedgerId, Request.ToDate, ref Entries);
				
				string Source = "";
				string SaveSql = "";
				if (string.IsNullOrEmpty(Result))
				{
					Source =
						JsonUtilities.StartJson() +
						Entries.ToString() +
						JsonUtilities.EndJson();
					SaveSql = JsonUtilities.GetJsonPathValueStr(Source, "$.SaveSQLStatement", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(SaveSql)) Result = "Cannot find the Save entry";					
				}
				
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					Params.Add(new FbParameterItem("SubLedgerId", Request.SubLedgerId));
					Params.Add(new FbParameterItem("RevenueId", Request.RevenueId));
					Params.Add(new FbParameterItem("ChargeAmount", Request.ChargeAmount));
					Params.Add(new FbParameterItem("GSTAmount", Request.GSTAmount));
					Params.Add(new FbParameterItem("FromDate", Request.FromDate));
					Params.Add(new FbParameterItem("ToDate", Request.ToDate));
					Params.Add(new FbParameterItem("Detail", Request.Detail));
					Params.Add(new FbParameterItem("DueDate", Request.DueDate));
					Params.Add(new FbParameterItem("PrintDate", Request.PrintDate));
					Params.Add(new FbParameterItem("AmountDue", Request.AmountDue));
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(SaveSql, Params, Method, 0);

					if (string.IsNullOrEmpty(Result))
					{
						Result = FirebirdUtilities.DBExecute(ConnectionString, SaveSql, Params, 0);
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("After SaveChanges Result: " + Result, Method, 0);
			}
			
			return Result;
		}
		
		public static string ReverseCharge(string ConnectionString,
		                                   int CompanyId,
		                                   int ClientId,
		                                   ReverseChargeRequestItem Request)
		{
			string Result = "";
			string Method = "ReverseCharge";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				StringBuilder Entries = new StringBuilder();
				Result = 
					EAChargeAddEditStart(
						ConnectionString, CompanyId, ClientId, Request.PortfolioId, Request.GridId, Request.FindId,
						Request.RevenueId, Request.SubLedgerId, Request.ToDate, ref Entries);
				
				string Source = "";
				string ReverseSql = "";
				if (string.IsNullOrEmpty(Result))
				{
					Source =
						JsonUtilities.StartJson() +
						Entries.ToString() +
						JsonUtilities.EndJson();
					ReverseSql = JsonUtilities.GetJsonPathValueStr(Source, "$.ReverseSQL", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(ReverseSql)) Result = "Cannot find the Reverse entry";					
				}
				
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					ReverseSql = ReverseSql.Replace("__", "'");
					Params.Add(new FbParameterItem("ReverseDate", Request.ReverseDate));
					Params.Add(new FbParameterItem("ReverseInvoice", Request.InvoiceAlso));
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(ReverseSql, Params, Method, 0);

					if (string.IsNullOrEmpty(Result))
					{
						Result = FirebirdUtilities.DBExecute(ConnectionString, ReverseSql, Params, 0);
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("After ReverseCharge Result: " + Result, Method, 0);
			}
			
			return Result;
		}
		
		public static string UndoReverseCharge(string ConnectionString,
		                                       int CompanyId,
		                                       int ClientId,
		                                       SetupPageEntriesRequestItem Request)
		{
			string Result = "";
			string Method = "UndoReverseCharge";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				StringBuilder Entries = new StringBuilder();
				Result = 
					EAChargeAddEditStart(
						ConnectionString, CompanyId, ClientId, Request.PortfolioId, Request.GridId, Request.FindId,
						Request.RevenueId, Request.SubLedgerId, Request.ToDate, ref Entries);
				
				string Source = "";
				string UndoReverseSql = "";
				if (string.IsNullOrEmpty(Result))
				{
					Source =
						JsonUtilities.StartJson() +
						Entries.ToString() +
						JsonUtilities.EndJson();
					UndoReverseSql = JsonUtilities.GetJsonPathValueStr(Source, "$.UndoReverseSQL", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(UndoReverseSql)) Result = "Cannot find the Reverse entry";					
				}
				
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					UndoReverseSql = UndoReverseSql.Replace("__", "'");
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(UndoReverseSql, null, Method, 0);

					if (string.IsNullOrEmpty(Result))
					{
						Result = FirebirdUtilities.DBExecute(ConnectionString, UndoReverseSql);
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("After UndoReverseCharge Result: " + Result, Method, 0);
			}
			
			return Result;
		}

		public static string LoadReverseChargeRequestItem(ActionBase ActionBase,
		                                                  ref ReverseChargeRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadReverseChangeRequestItem";
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
	                TempItem.SubLedgerIdValue = Item.SubLedgerIdValue;
                    TempItem.ToDate = Item.ToDate;
				}
				Object Obj = ActionBase.GetCollectionValue("Data", "InvoiceAlso");
				TempItem.InvoiceAlso = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ReverseDate");
				TempItem.ReverseDate = (Obj == null) ? "" : Convert.ToString(Obj);
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
