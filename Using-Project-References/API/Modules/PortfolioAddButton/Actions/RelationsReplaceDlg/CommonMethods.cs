/*
 * Created by SharpDevelop.
 * User: John
 * Date: 12/07/2020
 * Time: 1:12 PM
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
namespace Actions.PortfolioAddButton.RelationsReplaceDlg
{
	/// <summary>
	/// Description of CommonMethods.
	/// </summary>
	public class CommonMethods
	{
		public CommonMethods()
		{
		}
		
		public static string GetModuleName() { return "RelationsReplaceDlg"; }
		
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
				Obj = ActionBase.GetCollectionValue("Data", "FindId");
				TempItem.FindIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "FindText");
				TempItem.FindText = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "SubLedgerId");
				TempItem.SubLedgerIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ToSubLedgerId");
				TempItem.ToSubLedgerIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "TransferId");
				TempItem.TransferIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "TransferDate");
				TempItem.TransferDate = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ToAccountId");
				TempItem.ToAccountIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
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
		
		public static string EATransactionTransferBuild(string ConnectionString,
		                                                int CompanyId,
		                                                int ClientId,
		                                                int TransactionId)
		{
			string Result = "";
			string Method = "EATransactionTransferBuild";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string QueryText = 
					"EXECUTE PROCEDURE EATransactionTransferBuild(@CompanyId, @TransactionId, @LoginId)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				Params.Add(new FbParameterItem("TransactionId", TransactionId));
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("After EATransactionTransferBuild Result: " + Result, Method, 0);
			}
			
			return Result;
		}
		
		public static string EATransactionTransferOwnership(string ConnectionString,
		                                                    int CompanyId,
		                                                    int ClientId,
		                                                    int TransactionId,
		                                                    int RelationId, // gridId
		                                                    int FindId,
		                                                    string FindText,
		                                                    int SubLedgerId,
		                                                    int ToSubLedgerId,
		                                                    int TransferOption,
		                                                    string TransferDate,
		                                                    int ToAccountId,
		                                                    string BuildDone,
		                                                    ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "EATransactionTransferOwnership";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				if (string.IsNullOrEmpty(Result) && (BuildDone == "N"))
				{
					Result = EATransactionTransferBuild(ConnectionString, CompanyId, ClientId, TransactionId);
					if (string.IsNullOrEmpty(Result)) BuildDone = "Y";
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					if (Entries.Length > 0) Entries.Append(",");
					Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("BuildDone", BuildDone, false));
				}
				
				string QueryText = 
					"SELECT OUTPUTLINE FROM EATransactionTransferOwnership(" +
					"@CompanyId, @TransactionId, @RelationId, @LoginId, @FindId, @FindText," +
					"@SubLedgerId, @ToSubLedgerId, @TransferOption, @TransferDate, @ToAccountId)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					Params.Add(new FbParameterItem("CompanyId", CompanyId));
					Params.Add(new FbParameterItem("TransactionId", TransactionId));
					Params.Add(new FbParameterItem("RelationId", RelationId));
					Params.Add(new FbParameterItem("LoginId", ClientId));
					Params.Add(new FbParameterItem("FindId", FindId));
					Params.Add(new FbParameterItem("FindText", FindText));
					Params.Add(new FbParameterItem("SubLedgerId", SubLedgerId));
					Params.Add(new FbParameterItem("ToSubLedgerId", ToSubLedgerId));
					Params.Add(new FbParameterItem("TransferOption", TransferOption));
					Params.Add(new FbParameterItem("TransferDate", TransferDate));
					Params.Add(new FbParameterItem("ToAccountId", ToAccountId));
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("After EATransactionTransferOwnership Result: " + Result, Method, 0);
			}
			
			return Result;
		}
		
		public static string OnTransferOptionChange(string ConnectionString,
		                                            int CompanyId,
		                                            int ClientId,
		                                            int TransactionId,
		                                            int RelationId, // gridId
		                                            int FindId,
		                                            string FindText,
		                                            int SubLedgerId,
		                                            int ToSubLedgerId,
		                                            int TransferOption,
		                                            string TransferDate,
		                                            int ToAccountId,
		                                            string BuildDone,
		                                            ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "OnTransferOptionChange";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				StringBuilder TempEntries = new StringBuilder();
				if (string.IsNullOrEmpty(Result))
				{
					Result = 
						EATransactionTransferOwnership(
							ConnectionString, CompanyId, ClientId, 
							TransactionId, RelationId,
							FindId, FindText, SubLedgerId,
							ToSubLedgerId, TransferOption,
							TransferDate, ToAccountId, BuildDone, ref TempEntries);
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
					QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.OnChangeSQL", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the query";
				}
				
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					Params.Add(new FbParameterItem("OPTIONID", TransferOption));
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, GetModuleName(), 0);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = 
						EATransactionTransferOwnership(
							ConnectionString, CompanyId, ClientId, 
							TransactionId, RelationId,
							FindId, FindText, SubLedgerId,
							ToSubLedgerId, TransferOption,
							TransferDate, ToAccountId, BuildDone, ref Entries);
				}
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
				WindowsEventLoggingUtilities.WriteToInfoEvent("After OnTransferOptionChange Result: " + Result, Method, 0);
			}
			
			return Result;
		}
		
		public static string PostCommand(string ConnectionString,
		                                 int CompanyId,
		                                 int ClientId,
		                                 int TransactionId,
		                                 int RelationId, // gridId
		                                 int FindId,
		                                 string FindText,
		                                 int SubLedgerId,
		                                 int ToSubLedgerId,
		                                 int TransferOption,
		                                 string TransferDate,
		                                 int ToAccountId,
		                                 string BuildDone)
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
						EATransactionTransferOwnership(
							ConnectionString, CompanyId, ClientId, 
							TransactionId, RelationId,
							FindId, FindText, SubLedgerId,
							ToSubLedgerId, TransferOption,
							TransferDate, ToAccountId, BuildDone, ref TempEntries);
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
					QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.PostSQLValidate", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the query";
				}
				
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					Params.Add(new FbParameterItem("subledgerid", SubLedgerId));
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, GetModuleName(), 0);
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
						foreach (DataRow Row in tbl.Rows)
						{
							Obj = Row["OKMessage"];
							Result = ((Obj == DBNull.Value) ? "" : Convert.ToString(Obj));
							if (StringUtilities.SameText(Result, "OK")) Result = "";
						}
					}
				}

				QueryText = "";
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.PostSQL", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the query";
				}
				
				Params.Clear();
				if (string.IsNullOrEmpty(Result))
				{
					Params.Add(new FbParameterItem("subledgerid", SubLedgerId));
					Params.Add(new FbParameterItem("toaccountid", ToAccountId));
					Params.Add(new FbParameterItem("tosubledgerid", ToSubLedgerId));
					Params.Add(new FbParameterItem("transferdate", TransferDate));
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, GetModuleName(), 0);
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("After PostCommand Result: " + Result, Method, 0);
			}
			
			return Result;
		}
		
		public static string AddLedger(string ConnectionString,
		                               int CompanyId,
		                               int ClientId,
		                               int TransactionId,
		                               int RelationId, // gridId
		                               int FindId,
		                               string FindText,
		                               int SubLedgerId,
		                               int ToSubLedgerId,
		                               int TransferOption,
		                               string TransferDate,
		                               int ToAccountId,
		                               string BuildDone)
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
						EATransactionTransferOwnership(
							ConnectionString, CompanyId, ClientId, 
							TransactionId, RelationId,
							FindId, FindText, SubLedgerId,
							ToSubLedgerId, TransferOption,
							TransferDate, ToAccountId, BuildDone, ref TempEntries);
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
				
				int NewId = 0;
				if (string.IsNullOrEmpty(Result))
				{
					NewId = UsefulDBMethods.GeneratorHelpers.GetNextGeneratorId(ConnectionString, "I_LCE_ID_GEN", CompanyId, ClientId, out Result);
					if (string.IsNullOrEmpty(Result) && (NewId == 0)) Result = "Invalid Id Generated";
				}
				
				string QueryText = "";
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.AddLedgerSQL1", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the first query";
				}
				
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					Params.Add(new FbParameterItem("GETGENID", NewId));
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, GetModuleName(), 0);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
				}
				
				QueryText = "";
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.AddLedgerSQL2", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the second query";
				}
				
				Params.Clear();
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = QueryText.Replace("__", "'");
					Params.Add(new FbParameterItem("GETGENID", NewId));
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
