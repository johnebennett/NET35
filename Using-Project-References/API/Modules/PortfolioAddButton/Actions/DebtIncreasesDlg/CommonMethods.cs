/*
 * Created by SharpDevelop.
 * User: John
 * Date: 20/06/2020
 * Time: 6:19 PM
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
namespace Actions.PortfolioAddButton.DebtIncreasesDlg
{
	/// <summary>
	/// Description of CommonMethods.
	/// </summary>
	public class CommonMethods
	{
		public CommonMethods()
		{
		}

        public static string GetModuleName() { return "PortfolioAddButton.DebtIncreasesDlg."; }

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
				Obj = ActionBase.GetCollectionValue("Data", "IncreaseId");
				TempItem.IncreaseIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
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
		
		public static string EAChargeAddEditIncStart(string ConnectionString,
		                                             int CompanyId,
		                                             int ClientId,
		                                             int TransactionId,
		                                             int RecurringId,
		                                             int IncreaseId,
		                                             ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "EAChargeAddEditIncStart";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string QueryText = 
					" SELECT OutputLine FROM EAChargeAddEditIncStart(@CompanyId, @TransactionId, @RecurringId, @LoginId, @IncreaseId)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				Params.Add(new FbParameterItem("TransactionId", TransactionId));
				Params.Add(new FbParameterItem("RecurringId", RecurringId));
				Params.Add(new FbParameterItem("LoginId", ClientId));
				Params.Add(new FbParameterItem("IncreaseId", IncreaseId));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				
				DataTable tbl = null;
				object Obj = null;
				string OutputLine = "";
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, QueryText, Params, 0, FirebirdQueryCheckTypes.HasTables, out Result);
					if (string.IsNullOrEmpty(Result))
					{
						foreach (DataRow row in tbl.Rows)
						{
							Obj = row["OutputLine"];
							OutputLine = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
						}
					}
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(OutputLine)) Result = "Cannot find the Query value";
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					OutputLine = OutputLine.Replace("__", "'");
					WindowsEventLoggingUtilities.WriteToInfoEvent("SqlStatement:" + Environment.NewLine + OutputLine, Method, 0);
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, OutputLine, null, 0, FirebirdQueryCheckTypes.HasTables, out Result);
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("After EAChargeAddEditIncStart Result: " + Result, Method, 0);
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
	                TempItem.IncreaseIdValue = Item.IncreaseIdValue;
                
					Object Obj = ActionBase.GetCollectionValue("Data", "PastOrFuture");
					TempItem.PastOrFutureValue = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "IncreaseDate");
					TempItem.IncreaseDate = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "FixedAmount");
					TempItem.FixedAmountValue = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "PercentIncrease");
					TempItem.PercentIncreaseValue = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "Detail");
					TempItem.Detail = (Obj == null) ? "" : Convert.ToString(Obj);					

					Obj = ActionBase.GetCollectionValue("Data", "CPIIncrease");
					TempItem.CPIIncrease = (Obj == null) ? "" : Convert.ToString(Obj);					
					Obj = ActionBase.GetCollectionValue("Data", "CPIPlusExtraInterest");
					TempItem.CPIPlusExtraInterestValue = (Obj == null) ? "" : Convert.ToString(Obj);					
					Obj = ActionBase.GetCollectionValue("Data", "Operand");
					TempItem.OperandValue = (Obj == null) ? "" : Convert.ToString(Obj);					
					Obj = ActionBase.GetCollectionValue("Data", "ComparePercentage");
					TempItem.ComparePercentageValue = (Obj == null) ? "" : Convert.ToString(Obj);					
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
					EAChargeAddEditIncStart(
						ConnectionString, CompanyId, ClientId,  
							Request.PortfolioId, Request.Option2Id, Request.GridId, ref Entries);
				
				string Source = "";
				string SaveSql = "";
				if (string.IsNullOrEmpty(Result))
				{
					Source =
						JsonUtilities.StartJson() +
						Entries.ToString() +
						JsonUtilities.EndJson();
					SaveSql = JsonUtilities.GetJsonPathValueStr(Source, "$.SAVESQL", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(SaveSql)) Result = "Cannot find the Save entry";					
				}
				
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					Params.Add(new FbParameterItem("PastOrFuture", Request.PastOrFuture));
					Params.Add(new FbParameterItem("dateofincrease", Request.IncreaseDate));
					Params.Add(new FbParameterItem("fixedamount", Request.FixedAmount));
					Params.Add(new FbParameterItem("PERCENTAGEINCREASE", Request.PercentIncrease));
					Params.Add(new FbParameterItem("cpi", Request.CPIIncrease));
					Params.Add(new FbParameterItem("EXTRAINTEREST", Request.CPIPlusExtraInterest));
					Params.Add(new FbParameterItem("operand", Request.Operand));
					Params.Add(new FbParameterItem("COMPAREPERCENTAGE", Request.ComparePercentage));
					Params.Add(new FbParameterItem("INCREASEDETAIL", Request.Detail));
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

        public static string DeleteCmd(string ConnectionString,
                                       int CompanyId,
                                       int ClientId,
                                       SetupPageEntriesRequestItem Request)
        {
            string Result = "";
            string Method = GetModuleName() + "DeleteCmd";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                StringBuilder Entries = new StringBuilder();
                Result =
                    EAChargeAddEditIncStart(
                        ConnectionString, CompanyId, ClientId,
                            Request.PortfolioId, Request.Option2Id, Request.GridId, ref Entries);

                string Source = "";
                string SaveSql = "";
                if (string.IsNullOrEmpty(Result))
                {
                    Source =
                        JsonUtilities.StartJson() +
                        Entries.ToString() +
                        JsonUtilities.EndJson();
                    SaveSql = JsonUtilities.GetJsonPathValueStr(Source, "$.DeleteSQL", out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(SaveSql)) Result = "Cannot find the Delete entry";
                    WindowsEventLoggingUtilities.WriteToInfoEvent("SaveSql" + Environment.NewLine + SaveSql, "DeleteCmd", 0);
                }

                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(SaveSql, Params, Method, 0);

                    if (string.IsNullOrEmpty(Result))
                    {
                        Result = FirebirdUtilities.DBExecute(ConnectionString, SaveSql);
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
                WindowsEventLoggingUtilities.WriteToInfoEvent("After DeleteCmd Result: " + Result, Method, 0);
            }

            return Result;
        }
    }
}
