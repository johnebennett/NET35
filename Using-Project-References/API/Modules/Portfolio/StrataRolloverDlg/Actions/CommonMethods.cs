/*
 * Created by SharpDevelop.
 * User: John
 * Date: 07/08/2020
 * Time: 4:15 PM
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
namespace Actions.Portfolio.StrataRolloverDlg
{
	/// <summary>
	/// Description of CommonMethods.
	/// </summary>
	public class CommonMethods
	{
		public CommonMethods()
		{
		}
		
		public static string GetModuleName() { return "StrataRolloverDlg"; }
		
		public static string LoadSetupPageEntriesRequestItem(ActionBase ActionBase,
		                                                     ref SetupPageEntriesRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadSetupPageEntriesRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				Object Obj = ActionBase.GetCollectionValue("Data", "PortfolioId");
				TempItem.PortfolioIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "EOYId");
				TempItem.EOYIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "SubLedgerId");
				TempItem.SubLedgerIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
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
		
		public static string SetupPageEntries(string ConnectionString,
		                                      int CompanyId,
		                                      int ClientId,
		                                      SetupPageEntriesRequestItem Request,
		                                      ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "SetupPageEntries";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				int NewId = 0;
				if (string.IsNullOrEmpty(Result) && (Request.EOYId == 0))
				{
					NewId =
						UsefulDBMethods.GeneratorHelpers.GetNextGeneratorId(ConnectionString, "I_EOY_ID_GEN", out Result);
					if (string.IsNullOrEmpty(Result))
					{
						Request.EOYId = NewId;						
					}
				}
				
				if (string.IsNullOrEmpty(Result) && (Request.EOYId > 0))
				{
					if (Entries.Length > 0) Entries.Append(",");
					Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("EOYId", Request.EOYId, false));
				}
				
				string QueryText = "";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = 
						"SELECT OutputLine FROM OCEndOfYearGroup(@CompanyId, @TransactionId, @LoginId, @EndOfYearId, @SubLedgerId)";
					Params.Add(new FbParameterItem("CompanyId", CompanyId));
					Params.Add(new FbParameterItem("TransactionId", Request.PortfolioId));
					Params.Add(new FbParameterItem("LoginId", ClientId));
					Params.Add(new FbParameterItem("EndOfYearId", Request.EOYId));
					Params.Add(new FbParameterItem("SubLedgerId", Request.SubLedgerId));
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("After SetupPageEntries Result: " + Result, Method, 0);
			}
			
			return Result;
		}
		
		public static string LoadAllPageEntriesRequestItem(ActionBase ActionBase,
		                                                   ref AllPageEntriesRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadAllPageEntriesRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				SetupPageEntriesRequestItem Item = new SetupPageEntriesRequestItem();
				Result = Item.Load(ActionBase);
				if (string.IsNullOrEmpty(Result))
				{
	                TempItem.PortfolioIdValue = Item.PortfolioIdValue;
	                TempItem.SubLedgerIdValue = Item.SubLedgerIdValue;
	                TempItem.EOYIdValue = Item.EOYIdValue;
					
					string DefaultDateStr = "01/01/1900";
					Object Obj = ActionBase.GetCollectionValue("Data", "PriorTo");
					TempItem.PriorTo = (Obj == null) ? DefaultDateStr : Convert.ToString(Obj);
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
		
		private static string CopyRequestObject(AllPageEntriesRequestItem From,
		                                        ref SetupPageEntriesRequestItem To)
		{
			string Result = "";
			string Method = "CopyRequestObject";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
                To.PortfolioIdValue = From.PortfolioIdValue;
                To.EOYIdValue = From.EOYIdValue;
                To.SubLedgerIdValue = From.SubLedgerIdValue;
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
				WindowsEventLoggingUtilities.WriteToInfoEvent("After CopyRequestObject Result: " + Result, Method, 0);
			}
			
			return Result;
		}
		
		public static string Post(string ConnectionString,
		                          int CompanyId,
		                          int ClientId,
		                          AllPageEntriesRequestItem Request,
		                          ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "Post";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				SetupPageEntriesRequestItem Item = new SetupPageEntriesRequestItem();
				if (string.IsNullOrEmpty(Result))
				{
					Result = CopyRequestObject(Request, ref Item);
				}
				
				StringBuilder TempEntries = new StringBuilder();
				if (string.IsNullOrEmpty(Result))
				{
					Result = SetupPageEntries(ConnectionString, CompanyId, ClientId, Item, ref TempEntries);
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
					QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.PostSQL1", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the first Post query";
				}
				
				DataTable tbl = null;
				object Obj = null;
				int EvaId = 0;
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(ConnectionString, QueryText, FirebirdQueryCheckTypes.HasTables, out Result);
					if (string.IsNullOrEmpty(Result))
					{
						foreach (DataRow Row in tbl.Rows)
						{
							Obj = Row["ID"];
							EvaId = (Obj == DBNull.Value) ? 0 : Convert.ToInt32(Obj);
						}
					}
					if (string.IsNullOrEmpty(Result) && (EvaId == 0)) Result = "Error retrieving the required ID";
				}
				
				QueryText = "";
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.PostSQL2", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the second Post query";
				}
				
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = QueryText.Replace("__", "'");
					Params.Add(new FbParameterItem("ID", EvaId));
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, GetModuleName(), 0);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
				}
				
				QueryText = "";
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.PostSQL3", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the third Post query";
				}

                if (string.IsNullOrEmpty(Result))
                {
                    Result = ExecutePostSql3(ConnectionString, CompanyId, ClientId, QueryText, Request);
                }

                QueryText = "";
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.PostSQL4", out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the fourth Post query";
                }

                Params.Clear();
				if (string.IsNullOrEmpty(Result))
				{
					Params.Add(new FbParameterItem("subledgerid", Request.SubLedgerId));
					Params.Add(new FbParameterItem("priorto", Request.PriorTo));
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
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = SetupPageEntries(ConnectionString, CompanyId, ClientId, Item, ref Entries);
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

        public static string ExecutePostSql3(string ConnectionString,
                                             int CompanyId,
                                             int ClientId,
                                             string QueryText,
                                             AllPageEntriesRequestItem Request)
        {
            string Result = "";
            string Method = GetModuleName() + ".ExecutePostSql3";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                DataTable tbl = null;
                object Obj = null;
                string CommandLine = "";
                string TestRPOLocation = "";
                if (string.IsNullOrEmpty(Result))
                {
                    TestRPOLocation = global::UsefulDBMethods.CompanyFolderSettings.TestRPOLocation(ConnectionString, CompanyId, out Result);
                    if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(TestRPOLocation)) 
                        Result = "Cannot determine the Test location";
                }

                if (string.IsNullOrEmpty(Result))
                {
                    tbl =
                        UsefulDBMethods.Helpers.GetFirstDataTableEx(ConnectionString, QueryText, FirebirdQueryCheckTypes.HasTables, out Result);
                    if (string.IsNullOrEmpty(Result))
                    {
                        foreach (DataRow Row in tbl.Rows)
                        {
                            Obj = Row["EC_CommandLine"];
                            CommandLine = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                            if (string.IsNullOrEmpty(Result) && !string.IsNullOrEmpty(CommandLine))
                            {
                                Result = 
                                    UsefulDBMethods.Helpers.ProcessTestRPOCommandLine(CommandLine, TestRPOLocation, 1000);
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
            }

            return Result;
        }
    }
}
