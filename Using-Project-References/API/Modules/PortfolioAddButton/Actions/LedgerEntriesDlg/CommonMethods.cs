/*
 * Created by SharpDevelop.
 * User: John
 * Date: 01/07/2020
 * Time: 4:38 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text;
using System.Data;
using System.Collections.Generic;

using Actions.PortfolioAddButton.LedgerEntriesDlg;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CommonUtilities;
using EAOfficeAPI;
using UsefulDBMethods;
namespace Actions.PortfolioAddButton.LedgerEntriesDlg
{
	/// <summary>
	/// Description of CommonMethods.
	/// </summary>
	public class CommonMethods
	{
		public CommonMethods()
		{
		}		
		
		public static string GetModuleName() { return "LedgerEntriesDlg"; }
		
		public static string LoadCommonRequestItem(ActionBase ActionBase,
		                                           ref CommonRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadCommonRequestItem";
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
		
		public static string EALedgerEntryAddEditGroup(string ConnectionString,
		                                               int CompanyId,
		                                               int ClientId,
		                                               int TransactionId,
		                                               int LedgerId,
		                                               ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "EALedgerEntryAddEditGroup";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string QueryText = 
					" SELECT OutputLine FROM EALedgerEntryAddEditGroup(@CompanyId, @TransactionId, @LoginId, @LedgerId)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				Params.Add(new FbParameterItem("TransactionId", TransactionId));
				Params.Add(new FbParameterItem("LoginId", ClientId));
				Params.Add(new FbParameterItem("LedgerId", LedgerId));
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("After EALedgerEntryAddEditGroup Result: " + Result, Method, 0);
			}
			
			return Result;
		}		
		
		public static string LoadEdit_AllPageEntriesRequestItem(ActionBase ActionBase,
		                                                        ref Edit_AllPageEntriesRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadEdit_AllPageEntriesRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				/*
				CommonRequestItem Item = new CommonRequestItem();
				Result = Item.Load(ActionBase);
				if (string.IsNullOrEmpty(Result))
				{
	                TempItem.PortfolioIdValue = Item.PortfolioIdValue;
	                TempItem.Option1IdValue = Item.Option1IdValue;
	                TempItem.Option2IdValue = Item.Option2IdValue;
	                TempItem.GridIdValue = Item.GridIdValue;
				}
				*/
				Object Obj = ActionBase.GetCollectionValue("Data", "PortfolioId");
				TempItem.PortfolioIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Option1Id");
				TempItem.Option1IdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Option2Id");
				TempItem.Option2IdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "GridId");
				TempItem.GridIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Date");
				TempItem.Date = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Released");
				TempItem.Released = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Reference");
				TempItem.Reference = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Amount");
				TempItem.AmountValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "GSTAmount");
				TempItem.GSTAmountValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Detail");
				TempItem.Detail = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Payee");
				TempItem.Payee = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ChartId");
				TempItem.ChartIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
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
		
		public static string Edit_UpdateCmd(string ConnectionString,
		                                    int CompanyId,
		                                    int ClientId,
		                                    Edit_AllPageEntriesRequestItem Request)
		{
			string Result = "";
			string Method = "Edit_UpdateCmd";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				StringBuilder Entries = new StringBuilder();
				if (string.IsNullOrEmpty(Result))
				{
					Result = 
						EALedgerEntryAddEditGroup(
							ConnectionString, CompanyId, ClientId, 
							Request.PortfolioId, Request.GridId, ref Entries);
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
				
				string UpdateSql = "";
				string AllowEditDate = "";
				string AllowEditReference = "";
				string AllowEditReleaseDate = "";
				string AllowChartCodeChange = "";
				string AllowEditPayee = "";
				string AllowEditDetail = "";
				if (string.IsNullOrEmpty(Result))
				{
					UpdateSql = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.ShowUpdateSQL", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(UpdateSql)) Result = "Cannot find the Update entry";
					
					if (string.IsNullOrEmpty(Result))
					{
						AllowEditDate = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.AllowEditDate", out Result);
						if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(AllowEditDate)) Result = "Cannot determine the Edit Date entry";
					}
					
					if (string.IsNullOrEmpty(Result))
					{
						AllowEditReference = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.AllowEditReference", out Result);
						if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(AllowEditReference)) Result = "Cannot determine the Edit Reference entry";
					}
					
					if (string.IsNullOrEmpty(Result))
					{
						AllowEditReleaseDate = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.AllowEditReleaseDate", out Result);
						if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(AllowEditReleaseDate)) Result = "Cannot determine the Edit Release Date entry";
					}
					
					if (string.IsNullOrEmpty(Result))
					{
						AllowChartCodeChange = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.AllowChartCodeChange", out Result);
						if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(AllowChartCodeChange)) Result = "Cannot determine the Edit Chart Code entry";
					}
					
					if (string.IsNullOrEmpty(Result))
					{
						AllowEditPayee = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.AllowEditPayee", out Result);
						if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(AllowEditPayee)) Result = "Cannot determine the Edit Payee entry";
					}
					
					if (string.IsNullOrEmpty(Result))
					{
						AllowEditDetail = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.AllowEditDetail", out Result);
						if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(AllowEditDetail)) Result = "Cannot determine the Edit Detail entry";
					}
				}
				
				Value = "";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					if (AllowEditDate == "Y") Params.Add(new FbParameterItem("date", Request.Date));
					if (AllowEditReference == "Y") Params.Add(new FbParameterItem("reference", Request.Reference));
					if (AllowEditReleaseDate == "Y") Params.Add(new FbParameterItem("releasedate", Request.Released));
					if (AllowChartCodeChange == "Y") Params.Add(new FbParameterItem("chartid", Request.ChartId));
					if (AllowEditPayee == "Y") Params.Add(new FbParameterItem("payee", Request.Payee));
					if (AllowEditDetail == "Y") Params.Add(new FbParameterItem("detail", Request.Detail));
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(UpdateSql, Params, Method, 0);
					
					if (string.IsNullOrEmpty(Result))
						Result = FirebirdUtilities.DBExecute(ConnectionString, UpdateSql, Params, 0);
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("After Edit_UpdateCmd Result: " + Result, Method, 0);
			}
			
			return Result;
		}
		
		public static string Edit_ReverseCmd(string ConnectionString,
		                                     int CompanyId,
		                                     int ClientId,
		                                     Edit_AllPageEntriesRequestItem Request)
		{
			string Result = "";
			string Method = "Edit_ReverseCmd";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				StringBuilder Entries = new StringBuilder();
				if (string.IsNullOrEmpty(Result))
				{
					Result = 
						EALedgerEntryAddEditGroup(
							ConnectionString, CompanyId, ClientId, 
							Request.PortfolioId, Request.GridId, ref Entries);
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
				string ReverseSql1 = "";
				string ReverseSql2 = "";
				string ReverseSql3 = "";
				if (string.IsNullOrEmpty(Result))
				{
					ReverseSql1 = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.ReverseSQL1", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(ReverseSql1)) Result = "Cannot find the first Reverse entry";
					
					if (string.IsNullOrEmpty(Result))
					{
						ReverseSql2 = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.ReverseSQL2", out Result);
						if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(ReverseSql2)) Result = "Cannot determine the Second Reverse entry";
					}
					
					if (string.IsNullOrEmpty(Result))
					{
						ReverseSql3 = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.ReverseSQL3", out Result);
						if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(ReverseSql3)) Result = "Cannot determine the Third Reverse entry";
					}
				}
				
				int NewId = 0;
				DataTable tbl = null;
				object Obj = null;
				if (string.IsNullOrEmpty(Result))
				{
					tbl = UsefulDBMethods.Helpers.GetFirstDataTableEx(ConnectionString, ReverseSql1, null, 0, FirebirdQueryCheckTypes.HasTables, out Result);
					if (string.IsNullOrEmpty(Result))
					{
						foreach(DataRow Row in tbl.Rows)
						{
							Obj = Row["cc"];
							NewId = (Obj == DBNull.Value) ? 0 : Convert.ToInt32(Obj);
						}
						if (NewId == 0) Result = "Invalid generation of Record Id";
					}
				}
				
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					ReverseSql2 = ReverseSql2.Replace("__", "'");
					Params.Add(new FbParameterItem("Sql1Id", NewId));
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(ReverseSql2, Params, Method, 0);
					
					if (string.IsNullOrEmpty(Result))
						Result = FirebirdUtilities.DBExecute(ConnectionString, ReverseSql2, Params, 0);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					UsefulDBMethods.Helpers.RunEAMainOfficeProcess(ConnectionString, CompanyId);
					UsefulDBMethods.Helpers.RunEAMainOfficeProcess(ConnectionString, CompanyId);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Params.Clear();					
					Params.Add(new FbParameterItem("Sql1Id", NewId));
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(ReverseSql3, Params, GetModuleName(), 0);
					
					if (string.IsNullOrEmpty(Result))
					{
						tbl =
							UsefulDBMethods.Helpers.GetFirstDataTableEx(
								ConnectionString, ReverseSql3, Params, 0, FirebirdQueryCheckTypes.HasTables, out Result);
						foreach(DataRow Row in tbl.Rows)
						{
							Obj = Row["CBR_Message"];
							Result = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("After Edit_ReverseCmd Result: " + Result, Method, 0);
			}
			
			return Result;
		}
		
		public static string LoadAdd_SetupPageEntriesRequestItem(ActionBase ActionBase,
		                                                         ref Add_SetupPageEntriesRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadAdd_SetupPageEntriesRequestItem";
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
				Obj = ActionBase.GetCollectionValue("Data", "FindId");
				TempItem.FindIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "EventId");
				TempItem.EventIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "DirectEntry");
				TempItem.DirectEntry = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ExpenseId");
				TempItem.ExpenseIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Reference");
				TempItem.ReferenceValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "ToSubLedgerId");
                TempItem.ToSubLedgerIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
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
		
		public static string EALedgerAddPayJnlStart(string ConnectionString,
		                                               int CompanyId,
		                                               int ClientId,		                                               
		                                               int TransactionId,
		                                               int SubLedgerId,
		                                               int FindId,
		                                               int EventId,
		                                               string DirectEntry,		                                               
		                                               int ExpenseId,
		                                               int Reference,
                                                       int ToSubLedgerId,
		                                               ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "EALedgerAddPayJnlStart";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string QueryText = 
					" SELECT SqlStatement FROM EALedgerAddPayJnlStart" +
					" (@CompanyId, @TransactionId, @SubLedgerId, @FindId, @EventId, @DirectEntry, @ExpenseId," +
                    "  @LoginId, @Reference, @ToSubLedgerId)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				Params.Add(new FbParameterItem("TransactionId", TransactionId));
				Params.Add(new FbParameterItem("SubLedgerId", SubLedgerId));
				Params.Add(new FbParameterItem("FindId", FindId));
				Params.Add(new FbParameterItem("EventId", EventId));
				Params.Add(new FbParameterItem("DirectEntry", DirectEntry));
				Params.Add(new FbParameterItem("ExpenseId", ExpenseId));
				Params.Add(new FbParameterItem("LoginId", ClientId));
				Params.Add(new FbParameterItem("Reference", Reference));
                Params.Add(new FbParameterItem("ToSubLedgerId", ToSubLedgerId));
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
					//SqlStatement = SqlStatement.Replace(")", "," + ClientId.ToString() + ")");
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("After EALedgerAddPayJnlStart Result: " + Result, Method, 0);
			}
			
			return Result;
		}		
		
		public static string LoadAdd_AllPageEntriesRequestItem(ActionBase ActionBase,
		                                                       ref Add_AllPageEntriesRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadAdd_AllPageEntriesRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				Add_SetupPageEntriesRequestItem Item = new Add_SetupPageEntriesRequestItem();
				Result = Item.Load(ActionBase);
				if (string.IsNullOrEmpty(Result))
				{
					TempItem.PortfolioIdValue = Item.PortfolioIdValue;
					TempItem.Option1IdValue = Item.Option1IdValue;
					TempItem.Option2IdValue = Item.Option2IdValue;
					TempItem.FindIdValue = Item.FindIdValue;
					TempItem.EventIdValue = Item.EventIdValue;
					TempItem.DirectEntry = Item.DirectEntry;
					TempItem.ExpenseIdValue = Item.ExpenseIdValue;
					TempItem.ReferenceValue = Item.ReferenceValue;
					
					Object Obj = ActionBase.GetCollectionValue("Data", "RevenueId");  
					TempItem.RevenueIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "Date");
					TempItem.Date = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "Detail");
					TempItem.Detail = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "Detail");
					TempItem.Detail = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "Payee");
					TempItem.Payee = (Obj == null) ? "" : Convert.ToString(Obj);
	
					Obj = ActionBase.GetCollectionValue("Data", "Amount");
					TempItem.AmountValue = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "GSTAmount");
					TempItem.GSTAmountValue = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "ReverseSign");
					TempItem.ReverseSign = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "ToSubLedgerId");
					TempItem.ToSubLedgerIdValue = (Obj == null) ? "" : Convert.ToString(Obj);

					Obj = ActionBase.GetCollectionValue("Data", "BSB");
					TempItem.BSB = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "AccountNumber");
					TempItem.AccountNumber = (Obj == null) ? "" : Convert.ToString(Obj);
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
		
		public static string Add_SaveCmd(string ConnectionString,
		                                 int CompanyId,
		                                 int ClientId,
		                                 Add_AllPageEntriesRequestItem Request,
		                                 ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "Add_SaveCmd";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				StringBuilder Temp = new StringBuilder();
				if (string.IsNullOrEmpty(Result))
				{
					Result = 
						EALedgerAddPayJnlStart(
							ConnectionString, CompanyId, ClientId, 
							Request.PortfolioId, Request.Option2Id, Request.FindId, 
							Request.EventId, Request.DirectEntry, Request.ExpenseId, 
							Request.Reference, Request.ToSubLedgerId, ref Temp);
				}
				
				string Value = "";
				JObject JSONSource = null;
				if (string.IsNullOrEmpty(Result))
				{
					Value =
						JsonUtilities.StartJson() +
						Temp.ToString() +
						JsonUtilities.EndJson();
					
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(Value)) Result = "Cannot determine the Group entries";
					
					if (string.IsNullOrEmpty(Result))
					{
						JSONSource = JObject.Parse(Value);
						if (JSONSource == null) Result = "Cannot parse the Group source";
						else Value = "";
					}
				}
				
				string SaveSql1 = "";
				string SaveSql2 = "";
				string CreateFileSql1 = "";
				string CreateFileSql2 = "";
				string CreateFileSql3 = "";
				if (string.IsNullOrEmpty(Result))
				{
					SaveSql1 = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.SaveSQL1", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(SaveSql1)) Result = "Cannot find the first Save entry";
					
					if (string.IsNullOrEmpty(Result))
					{
						SaveSql2 = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.SaveSQL2", out Result);
						if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(SaveSql2)) Result = "Cannot determine the second Save entry";
					}
					
					if (Request.DirectEntry == "Y")
					{
						if (string.IsNullOrEmpty(Result))
						{
							CreateFileSql1 = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.CreateFileSQL1", out Result);
							if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(CreateFileSql1)) Result = "Cannot determine the first Create entry";
						}
						
						if (string.IsNullOrEmpty(Result))
						{
							CreateFileSql2 = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.CreateFileSQL2", out Result);
							if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(CreateFileSql2)) Result = "Cannot determine the second Create entry";
						}					
						
						if (string.IsNullOrEmpty(Result))
						{
							CreateFileSql3 = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.CreateFileSQL3", out Result);
							if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(CreateFileSql3)) Result = "Cannot determine the third Create entry";
						}					
					}
				}
				
				        Value = "SaveSql1:" + Environment.NewLine + SaveSql1 + Environment.NewLine;
				Value = Value + "SaveSql2:" + Environment.NewLine + SaveSql2 + Environment.NewLine;
				Value = Value + "CreateFileSql1:" + Environment.NewLine + CreateFileSql1 + Environment.NewLine;
				Value = Value + "CreateFileSql2:" + Environment.NewLine + CreateFileSql2 + Environment.NewLine;
				Value = Value + "CreateFileSql3:" + Environment.NewLine + CreateFileSql3;
				WindowsEventLoggingUtilities.WriteToInfoEvent(Value, Method, 0);
				
				int NewId = 0;
				if (string.IsNullOrEmpty(Result))
				{
					Result = Process_SaveSql1(ConnectionString, SaveSql1, out NewId);
					if (!string.IsNullOrEmpty(Result)) Result = "SaveSql1 - " + Result;
					WindowsEventLoggingUtilities.WriteToInfoEvent("NewId: " + NewId, Method, 0);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = Process_SaveSql2(ConnectionString, Request, SaveSql2, NewId);
					if (!string.IsNullOrEmpty(Result)) Result = "SaveSql2 - " + Result;
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					if (Entries.Length > 0) Entries.Append(",");
					Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("SaveStatus", "SUCCESS", false));

					UsefulDBMethods.Helpers.RunEAMainOfficeProcess(ConnectionString, CompanyId);
					UsefulDBMethods.Helpers.RunEAMainOfficeProcess(ConnectionString, CompanyId);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					if (Request.DirectEntry == "Y")
					{
						int EvaId = 0;
						Result = Process_CreateFileSql1(ConnectionString, CreateFileSql1, out EvaId);
						if (!string.IsNullOrEmpty(Result)) Result = "CreateFileSql1 - " + Result;
						WindowsEventLoggingUtilities.WriteToInfoEvent("EvaId: " + EvaId, Method, 0);
						
						int StmtRunId = 0;
						if (string.IsNullOrEmpty(Result))
						{
							Result = Process_CreateFileSql2(ConnectionString, CreateFileSql2, NewId, out StmtRunId);
							if (!string.IsNullOrEmpty(Result)) Result = "CreateFileSql2 - " + Result;
						}
						
						if (string.IsNullOrEmpty(Result))
						{
							Result = Process_CreateFileSql3(ConnectionString, CreateFileSql3, EvaId, StmtRunId);
							if (!string.IsNullOrEmpty(Result)) Result = "CreateFileSql3 - " + Result;
						}
						
						string commandLine = "";
						if (string.IsNullOrEmpty(Result))
						{
							commandLine = UsefulDBMethods.EventHelpers.GetEventCommandLineForRunId(ConnectionString, EvaId, out Result);
						}
						
						string TestRPOLocation = "";
						if (string.IsNullOrEmpty(Result))
						{
							TestRPOLocation = global::UsefulDBMethods.CompanyFolderSettings.TestRPOLocation(ConnectionString, CompanyId, out Result);
						}
						
						string DocsLocation = "";
						if (string.IsNullOrEmpty(Result))
						{
							DocsLocation = global::UsefulDBMethods.CompanyFolderSettings.DocsLocation(ConnectionString, CompanyId, out Result);
						}
			
						if (string.IsNullOrEmpty(Result))
						{
							// run the command line etc
							Result = UsefulDBMethods.Helpers.ProcessTestRPOCommandLine(commandLine, TestRPOLocation, 0);
							// extract the base64 field and put the file in the docs location
							// open / download the output file
							try
							{
								if (Entries.Length > 0) Entries.Append(",");
								Entries.Append(JsonUtilities.StartJsonArray("OutputFileList"));
								global::UsefulDBMethods.Reporting.ReportUtilities repUtils = new global::UsefulDBMethods.Reporting.ReportUtilities();
								repUtils.EAEventRunBase64Entries(ConnectionString, EvaId, DocsLocation, ref Entries, out Result);
							}
							finally
							{
								Entries.Append(JsonUtilities.EndJsonArray());
							}
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("After Add_SaveCmd Result: " + Result, Method, 0);
			}
			
			return Result;
		}
		
		private static string Process_SaveSql1(string ConnectionString,
		                                       string QueryText,
		                                       out int NewId)
		{
			string Result = "";
			string Method = "Process_SaveSql1";
			NewId = 0;
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText:" + Environment.NewLine + QueryText, Method, 0);
				DataTable tbl = null;
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, QueryText, null, 0, FirebirdQueryCheckTypes.HasTables, out Result);
					object Obj = null;
					foreach(DataRow Row in tbl.Rows)
					{
						Obj = Row["CC"];
						NewId = (Obj == DBNull.Value) ? 0 : Convert.ToInt32(Obj);
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
		
		private static string Process_SaveSql2(string ConnectionString,
		                                       Add_AllPageEntriesRequestItem Request,
		                                       string QueryText,
		                                       int NewId)
		{
			string Result = "";
			string Method = "Process_SaveSql2";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText:" + Environment.NewLine + QueryText, Method, 0);
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("FINDID", Request.FindId));
				Params.Add(new FbParameterItem("EXPENSEID", Request.ExpenseId));
				Params.Add(new FbParameterItem("REVENUEID", Request.RevenueId));
				Params.Add(new FbParameterItem("AMOUNT", Request.Amount));
				Params.Add(new FbParameterItem("GSTAMOUNT", Request.GSTAmount));
				Params.Add(new FbParameterItem("TRANSACTIONDATE", Request.Date));
				Params.Add(new FbParameterItem("REFERENCE", Request.Reference));
				Params.Add(new FbParameterItem("DETAIL", Request.Detail));
				Params.Add(new FbParameterItem("PAYEE", Request.Payee));
				Params.Add(new FbParameterItem("DIRECTENTRY", Request.DirectEntry));
				Params.Add(new FbParameterItem("REVERSESIGN", Request.ReverseSign));
				Params.Add(new FbParameterItem("CC", NewId));				
				Params.Add(new FbParameterItem("BSB", Request.BSB));				
				Params.Add(new FbParameterItem("accnum", Request.AccountNumber));
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
		
		private static string Process_CreateFileSql1(string ConnectionString,
		                                             string QueryText,
		                                             out int NewId)
		{
			string Result = "";
			string Method = "Process_CreateFileSql1";
			NewId = 0;
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText:" + Environment.NewLine + QueryText, Method, 0);
				DataTable tbl = null;
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, QueryText, null, 0, FirebirdQueryCheckTypes.HasTables, out Result);
					object Obj = null;
					foreach(DataRow Row in tbl.Rows)
					{
						Obj = Row["EVA"];
						NewId = (Obj == DBNull.Value) ? 0 : Convert.ToInt32(Obj);
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
		
		private static string Process_CreateFileSql2(string ConnectionString,
		                                             string QueryText,
		                                             int EvaId,
		                                             out int NewId)
		{
			string Result = "";
			string Method = "Process_CreateFileSql2";
			NewId = 0;
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText:" + Environment.NewLine + QueryText, Method, 0);
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CC", EvaId));
				DataTable tbl = null;
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, QueryText, Params, 0, FirebirdQueryCheckTypes.HasTables, out Result);
					object Obj = null;
					foreach(DataRow Row in tbl.Rows)
					{
						Obj = Row["SRunId"];
						NewId = (Obj == DBNull.Value) ? 0 : Convert.ToInt32(Obj);
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
		
		private static string Process_CreateFileSql3(string ConnectionString,
		                                             string QueryText,
		                                             int EvaId,
		                                             int StmtRunId)
		{
			string Result = "";
			string Method = "Process_CreateFileSql3";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText:" + Environment.NewLine + QueryText, Method, 0);
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("EVA", EvaId));
				Params.Add(new FbParameterItem("SRunId", StmtRunId));
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
	}
}
