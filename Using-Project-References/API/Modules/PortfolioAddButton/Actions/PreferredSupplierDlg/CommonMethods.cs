/*
 * Created by SharpDevelop.
 * User: John
 * Date: 25/06/2020
 * Time: 3:14 PM
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
namespace Actions.PortfolioAddButton.PreferredSupplierDlg
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
				Obj = ActionBase.GetCollectionValue("Data", "EPSId");
				TempItem.EPSIdValue = (Obj == null) ? "" : Convert.ToString(Obj);

				Obj = ActionBase.GetCollectionValue("Data", "ServiceId");
				TempItem.ServiceIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "SupplierId");
				TempItem.SupplierIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
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
		
		public static string EAPreferredServiceAddEditStart(string ConnectionString,
		                                                    int CompanyId,
		                                                    int ClientId,
		                                                    int TransactionId,
		                                                    Int64 EPSId,
		                                                    int ServiceId,
		                                                    int SupplierId,
		                                                    ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "EAPreferredServiceAddEditStart";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string QueryText = 
					" SELECT SqlStatement FROM EAPreferredServiceAddEditStart(@CompanyId, @LoginId, @EPSId, @ServiceId, @SupplierId, @TransactionId)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				Params.Add(new FbParameterItem("LoginId", ClientId));
				Params.Add(new FbParameterItem("EPSId", EPSId));
				Params.Add(new FbParameterItem("ServiceId", ServiceId));
				Params.Add(new FbParameterItem("SupplierId", SupplierId));
				Params.Add(new FbParameterItem("TransactionId", TransactionId));
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
		
		public static string SaveCmd(string ConnectionString,
		                             int CompanyId,
		                             int ClientId,
		                             SetupPageEntriesRequestItem Request)
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
					EAPreferredServiceAddEditStart(
						ConnectionString, CompanyId, ClientId, Request.PortfolioId, Request.GridId, 
						Request.ServiceId, Request.SupplierId, ref Entries);
				
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
					Params.Add(new FbParameterItem("serviceid", Request.ServiceId));
					Params.Add(new FbParameterItem("supplierid", Request.SupplierId));
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("After SaveCmd Result: " + Result, Method, 0);
			}
			
			return Result;
		}
		
		public static string DeleteCmd(string ConnectionString,
		                               int CompanyId,
		                               int ClientId,
		                               SetupPageEntriesRequestItem Request)
		{
			string Result = "";
			string Method = "DeleteCmd";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				StringBuilder Entries = new StringBuilder();
				Result = 
					EAPreferredServiceAddEditStart(
						ConnectionString, CompanyId, ClientId, Request.PortfolioId, Request.GridId, 
						Request.ServiceId, Request.SupplierId, ref Entries);
				
				string Source = "";
				string DeleteSql = "";
				if (string.IsNullOrEmpty(Result))
				{
					Source =
						JsonUtilities.StartJson() +
						Entries.ToString() +
						JsonUtilities.EndJson();
					DeleteSql = JsonUtilities.GetJsonPathValueStr(Source, "$.DeleteSQL", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(DeleteSql)) Result = "Cannot find the Delete entry";					
				}
				
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					Params.Add(new FbParameterItem("serviceid", Request.ServiceId));
					Params.Add(new FbParameterItem("supplierid", Request.SupplierId));
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(DeleteSql, Params, Method, 0);

					if (string.IsNullOrEmpty(Result))
					{
						Result = FirebirdUtilities.DBExecute(ConnectionString, DeleteSql, Params, 0);
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
