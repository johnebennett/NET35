/*
 * Created by SharpDevelop.
 * User: John
 * Date: 18/06/2020
 * Time: 6:11 PM
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
namespace Actions.PortfolioAddButton.RecurringDebtsDlg
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
				
				Obj = ActionBase.GetCollectionValue("Data", "Frequency");
				TempItem.FrequencyValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "FromDate");
				TempItem.FromDate = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "Inclusive");
				TempItem.Inclusive = (Obj == null) ? "" : Convert.ToString(Obj);
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
		
		public static string EARecurringAddEditStart(string ConnectionString,
		                                             int CompanyId,
		                                             int ClientId,
		                                             int TransactionId,
		                                             int ChargeId,
		                                             int FindId,
		                                             int RevenueId,
		                                             int SubLedgerId,
		                                             int Frequency,
		                                             string FromDate,
		                                             string Inclusive,
		                                             ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "EARecurringAddEditStart";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string QueryText = 
					" SELECT SqlStatement FROM EARecurringAddEditStart" +
					" (@CompanyId, @TransactionId, @ChargeId, @LoginId, @FindId, @SubLedgerId, @RevenueId," +
				    "  @Frequency, @FromDate, @Inclusive)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				Params.Add(new FbParameterItem("TransactionId", TransactionId));
				Params.Add(new FbParameterItem("ChargeId", ChargeId));
				Params.Add(new FbParameterItem("LoginId", ClientId));
				Params.Add(new FbParameterItem("FindId", FindId));
				Params.Add(new FbParameterItem("SubLedgerId", SubLedgerId));
				Params.Add(new FbParameterItem("RevenueId", RevenueId));
				Params.Add(new FbParameterItem("Frequency", Frequency));
				Params.Add(new FbParameterItem("FromDate", FromDate));
				Params.Add(new FbParameterItem("Inclusive", Inclusive));
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("After EARecurringAddEditStart Result: " + Result, Method, 0);
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
	                TempItem.FindIdValue = Item.FindIdValue;
	                TempItem.RevenueIdValue = Item.RevenueIdValue;
	                TempItem.SubLedgerIdValue = Item.SubLedgerIdValue;
	                TempItem.FrequencyValue = Item.FrequencyValue;
	                TempItem.FromDate = Item.FromDate;
	                TempItem.Inclusive = Item.Inclusive;
                
					Object Obj = ActionBase.GetCollectionValue("Data", "ChargeAmount");
					TempItem.ChargeAmountValue = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "GSTAmount");
					TempItem.GSTAmountValue = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "ToDate");
					TempItem.ToDate = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "Detail");
					TempItem.Detail = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "DueBefore");
					TempItem.DueBeforeValue = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "PrintBefore");
					TempItem.PrintBeforeValue = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "CreateBefore");
					TempItem.CreateBeforeValue = (Obj == null) ? "" : Convert.ToString(Obj);					
					Obj = ActionBase.GetCollectionValue("Data", "CreateStarts");
					TempItem.CreateStarts = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "CreateStops");
					TempItem.CreateStops = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "StartDay");
					TempItem.StartDayValue = (Obj == null) ? "" : Convert.ToString(Obj);
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
			string Method = "SaveChanges";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				StringBuilder Entries = new StringBuilder();
				Result = 
					EARecurringAddEditStart(
						ConnectionString, CompanyId, ClientId, Request.PortfolioId, Request.GridId, Request.FindId,
						Request.RevenueId, Request.SubLedgerId, Request.Frequency, Request.FromDate, Request.Inclusive, ref Entries);
				
				string Source = "";
				string SaveSql = "";
				if (string.IsNullOrEmpty(Result))
				{
					Source =
						JsonUtilities.StartJson() +
						Entries.ToString() +
						JsonUtilities.EndJson();
					SaveSql = JsonUtilities.GetJsonPathValueStr(Source, "$.SaveSQL", out Result);
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

					Params.Add(new FbParameterItem("DueDate", Request.FromDate));
					Params.Add(new FbParameterItem("PrintDate", Request.FromDate));
					Params.Add(new FbParameterItem("AST_PeriodInteger", Request.Frequency));
					Params.Add(new FbParameterItem("DueBefore", Request.DueBefore));
					Params.Add(new FbParameterItem("PrintBefore", Request.PrintBefore));
					Params.Add(new FbParameterItem("CreateBefore", Request.CreateBefore));
					Params.Add(new FbParameterItem("B_AST_Inclusive", Request.Inclusive));
					Params.Add(new FbParameterItem("D_AST_STOP", Request.CreateStops));
					Params.Add(new FbParameterItem("D_AST_Create", Request.CreateStarts));
					Params.Add(new FbParameterItem("StartDay", Request.StartDay));
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
		
		public static string ActivateCmd(string ConnectionString,
		                                 int CompanyId,
		                                 int ClientId,
		                                 SetupPageEntriesRequestItem Request)
		{
			string Result = "";
			string Method = "ActivateCmd";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				StringBuilder Entries = new StringBuilder();
				Result = 
					EARecurringAddEditStart(
						ConnectionString, CompanyId, ClientId, Request.PortfolioId, Request.GridId, Request.FindId,
						Request.RevenueId, Request.SubLedgerId, Request.Frequency, Request.FromDate, Request.Inclusive, ref Entries);
				
				string Source = "";
				string SqlActivate = "";
				if (string.IsNullOrEmpty(Result))
				{
					Source =
						JsonUtilities.StartJson() +
						Entries.ToString() +
						JsonUtilities.EndJson();
					SqlActivate = JsonUtilities.GetJsonPathValueStr(Source, "$.SQLActivate", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(SqlActivate)) Result = "Cannot find the Activate entry";					
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					SqlActivate = SqlActivate.Replace("__", "'");
					Result = FirebirdUtilities.DBExecute(ConnectionString, SqlActivate);
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
		
		public static string DeActivateCmd(string ConnectionString,
		                                   int CompanyId,
		                                   int ClientId,
		                                   SetupPageEntriesRequestItem Request)
		{
			string Result = "";
			string Method = "DeActivateCmd";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				StringBuilder Entries = new StringBuilder();
				Result = 
					EARecurringAddEditStart(
						ConnectionString, CompanyId, ClientId, Request.PortfolioId, Request.GridId, Request.FindId,
						Request.RevenueId, Request.SubLedgerId, Request.Frequency, Request.FromDate, Request.Inclusive, ref Entries);
				
				string Source = "";
				string SqlDeactivate = "";
				if (string.IsNullOrEmpty(Result))
				{
					Source =
						JsonUtilities.StartJson() +
						Entries.ToString() +
						JsonUtilities.EndJson();
					SqlDeactivate = JsonUtilities.GetJsonPathValueStr(Source, "$.SQLDeactivate", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(SqlDeactivate)) Result = "Cannot find the DeActivate entry";					
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					SqlDeactivate = SqlDeactivate.Replace("__", "'");
					Result = FirebirdUtilities.DBExecute(ConnectionString, SqlDeactivate);
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

        public static string GenerateCharge(string ConnectionString,
                                            int CompanyId,
                                            int ClientId,
                                            SetupPageEntriesRequestItem Request)
        {
            string Result = "";
            string Method = "GenerateCharge";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                StringBuilder Entries = new StringBuilder();
                Result =
                    EARecurringAddEditStart(
                        ConnectionString, CompanyId, ClientId, Request.PortfolioId, Request.GridId, Request.FindId,
                        Request.RevenueId, Request.SubLedgerId, Request.Frequency, Request.FromDate, Request.Inclusive, ref Entries);

                string Source = "";
                string QueryText = "";
                if (string.IsNullOrEmpty(Result))
                {
                    Source =
                        JsonUtilities.StartJson() +
                        Entries.ToString() +
                        JsonUtilities.EndJson();
                    QueryText = JsonUtilities.GetJsonPathValueStr(Source, "$.GenerateSQL", out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the Generate entry";
                }

                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = QueryText.Replace("__", "'");
                    Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText);
                }

                if (string.IsNullOrEmpty(Result))
                {
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
                WindowsEventLoggingUtilities.WriteToInfoEvent("After GenerateCharge Result: " + Result, Method, 0);
            }

            return Result;
        }
    }
}
