/*
 * Created by SharpDevelop.
 * User: John
 * Date: 13/07/2020
 * Time: 5:30 PM
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
namespace Actions.PortfolioAddButton.RelationsAddDlg
{
	/// <summary>
	/// Description of CommonMethods.
	/// </summary>
	public class CommonMethods
	{
		public CommonMethods()
		{
		}
		
		public static string GetModuleName() { return "RelationsAddDlg"; }
		
		public static string LoadSetupPageEntriesRequestItem(ActionBase ActionBase,
		                                                     ref SetupPageEntriesRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadSetupPageEntriesRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), ActionBase.MasterId);
					
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
				Obj = ActionBase.GetCollectionValue("Data", "RelationTypeId");
				TempItem.RelationTypeIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
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
		
		public static string EARelationTempOptionalAdd(string ConnectionString,
		                                               int CompanyId,
		                                               int ClientId,
		                                               int TransactionId,
		                                               int FindId,
		                                               int RelationTypeId,
		                                               ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "EARelationTempOptionalAdd";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string QueryText = 
					"SELECT OUTPUTLINE FROM EARelationTempOptionalAdd(" +
					"@CompanyId, @TransactionId, @LoginId, @RelationTypeId, @FindId)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					Params.Add(new FbParameterItem("CompanyId", CompanyId));
					Params.Add(new FbParameterItem("TransactionId", TransactionId));
					Params.Add(new FbParameterItem("LoginId", ClientId));
					Params.Add(new FbParameterItem("RelationTypeId", RelationTypeId));
					Params.Add(new FbParameterItem("FindId", FindId));
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				}
				
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("After EARelationTempOptionalAdd Result: " + Result, Method, 0);
			}
			
			return Result;
		}
		
		public static string PostCommand(string ConnectionString,
		                                 int CompanyId,
		                                 int ClientId,
		                                 SetupPageEntriesRequestItem Request)
		{
			string Result = "";
			string Method = "PostCommand";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				StringBuilder TempEntries = new StringBuilder();
				if (string.IsNullOrEmpty(Result))
				{
					Result = 
						EARelationTempOptionalAdd(
							ConnectionString, CompanyId, ClientId, 
							Request.PortfolioId, Request.FindId, 
							Request.RelationTypeId, ref TempEntries);
				}				

				string Value = "";
				JObject JSONSource = null;
				if (string.IsNullOrEmpty(Result))
				{
					Value =
						JsonUtilities.StartJson() +
						TempEntries.ToString() +
						JsonUtilities.EndJson();
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(Value)) Result = "Cannot determine the Group entries";
					
					if (string.IsNullOrEmpty(Result))
					{
						JSONSource = JObject.Parse(Value);
						if (JSONSource == null) Result = "Cannot parse the Group source";
						Value = "";
					}
				}
				
				string QueryText = "";
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.PostSQL", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the query";
				}
				
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = QueryText.Replace("__", "'");
					Params.Add(new FbParameterItem("clientid", Request.FindId));
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, GetModuleName(), 0);
				}				
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("After PostCommand Result: " + Result, Method, 0);
			}
			
			return Result;
		}
	}
}
