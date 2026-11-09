/*
 * Created by SharpDevelop.
 * User: John
 * Date: 10/07/2020
 * Time: 3:26 PM
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
namespace Actions.PortfolioAddButton.RelationsDlg
{
	/// <summary>
	/// Description of CommonMethods.
	/// </summary>
	public class CommonMethods
	{
		public CommonMethods()
		{
		}
		
		public static string GetModuleName() { return "RelationsDlg"; }
		
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
				Obj = ActionBase.GetCollectionValue("Data", "BuildDone");
				TempItem.BuildDone = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "EntryId");
				TempItem.EntryIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
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
		
		public static string EARelationTempAddEditBuild(string ConnectionString,
		                                                int CompanyId,
		                                                int ClientId,
		                                                int TransactionId,
		                                                int RelationId)
		{
			string Result = "";
			string Method = "EARelationTempAddEditBuild";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string QueryText = 
					"EXECUTE PROCEDURE EARelationTempAddEditBuild(@CompanyId, @TransactionId, @RelationId, @LoginId)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				Params.Add(new FbParameterItem("TransactionId", TransactionId));
				Params.Add(new FbParameterItem("RelationId", RelationId));
				Params.Add(new FbParameterItem("LoginId", ClientId));
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("After EARelationTempAddEditBuild Result: " + Result, Method, 0);
			}
			
			return Result;
		}
		
		public static string EARelationTempAddEditGroup(string ConnectionString,
		                                                int CompanyId,
		                                                int ClientId,
		                                                int TransactionId,
		                                                int RelationId,
		                                                int EntryId,
		                                                string BuildDone,
		                                                ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "EARelationTempAddEditGroup";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				if (string.IsNullOrEmpty(Result) && (BuildDone == "N"))
				{
					Result = EARelationTempAddEditBuild(ConnectionString, CompanyId, ClientId, TransactionId, RelationId);
					if (string.IsNullOrEmpty(Result)) BuildDone = "Y";
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					if (Entries.Length > 0) Entries.Append(",");
					Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("BuildDone", BuildDone, false));
				}
				
				string QueryText = 
					"SELECT OUTPUTLINE FROM EARelationTempAddEditGroup(@CompanyId, @TransactionId, @LoginId, @EntityId, @EntryId)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					Params.Add(new FbParameterItem("CompanyId", CompanyId));
					Params.Add(new FbParameterItem("TransactionId", TransactionId));
					Params.Add(new FbParameterItem("LoginId", ClientId));
					Params.Add(new FbParameterItem("EntityId", RelationId));
					Params.Add(new FbParameterItem("EntryId", EntryId));
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("After EARelationTempAddEditGroup Result: " + Result, Method, 0);
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
					TempItem.BuildDone = Item.BuildDone;
					TempItem.EntryIdValue = Item.EntryIdValue;
					
					Object Obj = ActionBase.GetCollectionValue("Data", "FindId");
					TempItem.FindIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
					Obj = ActionBase.GetCollectionValue("Data", "RemoveFromList");
					TempItem.RemoveFromList = (Obj == null) ? "" : Convert.ToString(Obj);
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
		
		public static string AddToList(string ConnectionString,
		                               int CompanyId,
		                               int ClientId,
		                               AllPageEntriesRequestItem Request,
		                               ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "AddToList";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				StringBuilder TempEntries = new StringBuilder();
				Result = 
					EARelationTempAddEditGroup(
						ConnectionString, CompanyId, ClientId,
						Request.PortfolioId, Request.GridId,
						Request.EntryId, Request.BuildDone, ref TempEntries);

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
					QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.AddToListSQL", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the query";
				}
				
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					Params.Add(new FbParameterItem("findid", Request.FindId));
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, GetModuleName(), 0);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = 
					EARelationTempAddEditGroup(
						ConnectionString, CompanyId, ClientId,
						Request.PortfolioId, Request.GridId,
						Request.EntryId, Request.BuildDone, ref Entries);
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
		
		public static string RemoveFromList(string ConnectionString,
		                                    int CompanyId,
		                                    int ClientId,
		                                    AllPageEntriesRequestItem Request,
		                                    ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "RemoveFromList";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				StringBuilder TempEntries = new StringBuilder();
				Result = 
					EARelationTempAddEditGroup(
						ConnectionString, CompanyId, ClientId,
						Request.PortfolioId, Request.GridId,
						Request.EntryId, Request.BuildDone, ref TempEntries);

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
					QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.RemoveFromListSQL", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the query";
				}
				
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					Params.Add(new FbParameterItem("removelist", Request.RemoveFromList));
					Params.Add(new FbParameterItem("dropdownid", Request.EntryId));
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, GetModuleName(), 0);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = 
					EARelationTempAddEditGroup(
						ConnectionString, CompanyId, ClientId,
						Request.PortfolioId, Request.GridId,
						Request.EntryId, Request.BuildDone, ref Entries);
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
				StringBuilder Entries = new StringBuilder();
				Result = 
					EARelationTempAddEditGroup(
						ConnectionString, CompanyId, ClientId,
						Request.PortfolioId, Request.GridId,
						Request.EntryId, Request.BuildDone, ref Entries);

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
						Value = "";
					}
				}
				
				string QueryText = "";
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.PostSQL", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the query";
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText);
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
