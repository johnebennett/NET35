/*
 * Created by SharpDevelop.
 * User: John
 * Date: 08/07/2020
 * Time: 9:35 AM
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
namespace Actions.PortfolioAddButton.EOYBudgetDlg
{
	/// <summary>
	/// Description of CommonMethods.
	/// </summary>
	public class CommonMethods
	{
		public CommonMethods()
		{
		}

        public static string GetModuleName() { return "PortfolioAddButton.EOYBudgetDlg."; }

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
		
		public static string EAEOYBudgetAddEditGroup(string ConnectionString,
		                                             int CompanyId,
		                                             int ClientId,
		                                             int TransactionId,
		                                             int Option2,
		                                             int BudgetItemId,
		                                             ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "EAEOYBudgetAddEditGroup";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
			WindowsEventLoggingUtilities.WriteToInfoEvent("ActionBase.DebugMode: " + ActionBase.DebugMode, Method, 0);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string QueryText = 
					"SELECT OUTPUTLINE FROM EAEOYBudgetAddEditGroup(@CompanyId, @TransactionId, @Option2, @BudgetItemId)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				Params.Add(new FbParameterItem("TransactionId", TransactionId));
				Params.Add(new FbParameterItem("Option2", Option2));
				Params.Add(new FbParameterItem("BudgetItemId", BudgetItemId));
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("After EAEOYBudgetAddEditGroup Result: " + Result, Method, 0);
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
					
					Object Obj = ActionBase.GetCollectionValue("Data", "Year");
					TempItem.YearValue = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "ChartCodeId");
					TempItem.ChartCodeIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "Total");
					TempItem.TotalValue = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "July");
					TempItem.JulyValue = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "August");
					TempItem.AugustValue = (Obj == null) ? "" : Convert.ToString(Obj);

					Obj = ActionBase.GetCollectionValue("Data", "September");
					TempItem.SeptemberValue = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "October");
					TempItem.OctoberValue = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "November");
					TempItem.NovemberValue = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "December");
					TempItem.DecemberValue = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "January");
					TempItem.JanuaryValue = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "February");
					TempItem.FebruaryValue = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "March");
					TempItem.MarchValue = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "April");
					TempItem.AprilValue = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "May");
					TempItem.MayValue = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "June");
					TempItem.JuneValue = (Obj == null) ? "" : Convert.ToString(Obj);
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
					EAEOYBudgetAddEditGroup(
						ConnectionString, CompanyId, ClientId, 
							Request.PortfolioId, Request.Option2Id, Request.GridId, ref Entries);
				
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
					Params.Add(new FbParameterItem("chartid", Request.ChartCodeId));
					Params.Add(new FbParameterItem("yearof", Request.Year));
					Params.Add(new FbParameterItem("total", Request.Total));
					Params.Add(new FbParameterItem("july", Request.July));
					Params.Add(new FbParameterItem("august", Request.August));
					Params.Add(new FbParameterItem("september", Request.September));
					Params.Add(new FbParameterItem("october", Request.October));
					Params.Add(new FbParameterItem("november", Request.November));
					Params.Add(new FbParameterItem("december", Request.December));
					Params.Add(new FbParameterItem("january", Request.January));
					Params.Add(new FbParameterItem("february", Request.February));
					Params.Add(new FbParameterItem("march", Request.March));
					Params.Add(new FbParameterItem("april", Request.April));
					Params.Add(new FbParameterItem("may", Request.May));
					Params.Add(new FbParameterItem("june", Request.June));
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
		
		public static string CreateRevenue(string ConnectionString,
		                                   int CompanyId,
		                                   int ClientId,
		                                   AllPageEntriesRequestItem Request)
		{
			string Result = "";
			string Method = "CreateRevenue";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				StringBuilder Entries = new StringBuilder();
				Result = 
					EAEOYBudgetAddEditGroup(
						ConnectionString, CompanyId, ClientId, 
							Request.PortfolioId, Request.Option2Id, Request.GridId, ref Entries);
				
				string Source = "";
				string QueryText = "";
				if (string.IsNullOrEmpty(Result))
				{
					Source =
						JsonUtilities.StartJson() +
						Entries.ToString() +
						JsonUtilities.EndJson();
					QueryText = JsonUtilities.GetJsonPathValueStr(Source, "$.CreateRevenueSQL", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the Save entry";					
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
					EAEOYBudgetAddEditGroup(
						ConnectionString, CompanyId, ClientId, Request.PortfolioId, Request.Option2Id, 
						Request.GridId, ref Entries);
				
				string Source = "";
				string QueryText = "";
				if (string.IsNullOrEmpty(Result))
				{
					Source =
						JsonUtilities.StartJson() +
						Entries.ToString() +
						JsonUtilities.EndJson();
					QueryText = JsonUtilities.GetJsonPathValueStr(Source, "$.TrashSQL", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the Trash entry";					
				}
				
				if (string.IsNullOrEmpty(Result))
				{
                    WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText: " + QueryText, Method, 0);
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
		
		public static string DisplayPdfReport(string ConnectionString,
		                                      int CompanyId,
		                                      int ClientId,
		                                      SetupPageEntriesRequestItem Request,
		                                      ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "DisplayPdfReport";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				StringBuilder TempEntries = new StringBuilder();
				Result =
					EAEOYBudgetAddEditGroup(
						ConnectionString, CompanyId, ClientId, Request.PortfolioId, Request.Option2Id, 
						Request.GridId, ref TempEntries);
				
				string Source = "";
				string ReportSource = "";
				if (string.IsNullOrEmpty(Result))
				{
					Source =
						JsonUtilities.StartJson() +
						TempEntries.ToString() +
						JsonUtilities.EndJson();
					ReportSource = JsonUtilities.GetJsonPathValueStr(Source, "$.ZL20_B1", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(ReportSource)) Result = "Cannot find the Report entry";					
				}
				
				string OutputFileName = "";
				if (string.IsNullOrEmpty(Result))
				{
					long UniqueId = (long)DateTime.Now.Subtract(DateTime.MinValue.AddYears(1969)).TotalMilliseconds;
					Int64 NewId =
						CommonUtilities.SystemReportUtilities.Execute(
							ConnectionString, CompanyId, ClientId,
							UniqueId, ReportSource, out Result, out OutputFileName);
				}
			
				if (string.IsNullOrEmpty(Result))
				{
					if (Entries.Length > 0) Entries.Append(",");
					Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("FileName", @"/Docs/" + OutputFileName, false));
				}
				else
				{
					Result = ActionBase.LogOnProcessingError(Method, Result, "");
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
