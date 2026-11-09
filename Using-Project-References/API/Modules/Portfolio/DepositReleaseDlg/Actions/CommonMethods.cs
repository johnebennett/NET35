/*
 * Created by SharpDevelop.
 * User: John
 * Date: 23/07/2020
 * Time: 7:54 PM
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
namespace Actions.Portfolio.DepositReleaseDlg
{
	/// <summary>
	/// Description of CommonMethods.
	/// </summary>
	public class CommonMethods
	{
		public CommonMethods()
		{
		}
		
		public static string GetModuleName() { return "DepositReleaseDlg"; }
		
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
				Obj = ActionBase.GetCollectionValue("Data", "StatementRunId");
				TempItem.StatementRunIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "EventRunId");
				TempItem.EventRunIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "DepositReleaseType");
				TempItem.DepositReleaseTypeValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "StmtDate");
				TempItem.StmtDate = (Obj == null) ? "" : Convert.ToString(Obj);
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
				string QueryText = "";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result) && (Request.StatementRunId == 0))
				{
					Result = EADepositReleaseOK(ConnectionString, CompanyId, Request.PortfolioId);
					
					int StatementRunId = 0;
					int EventRunId = 0;
					if (string.IsNullOrEmpty(Result))
					{
						Result = 
							EADepositReleasePrepare(
								ConnectionString, CompanyId, ClientId, Request.PortfolioId, ref StatementRunId, ref EventRunId);
					}
					
					if (string.IsNullOrEmpty(Result))
					{
						Request.StatementRunId = StatementRunId;
						Request.EventRunId = EventRunId;						
					}
				
					if (string.IsNullOrEmpty(Result))
					{
						QueryText = "EXECUTE PROCEDURE EADepositReleasePrepare2(@CompanyId, @TransactionId, @StatementRunId, @EventRunId, @LoginId)";
	
						Params.Add(new FbParameterItem("CompanyId", CompanyId));
						Params.Add(new FbParameterItem("TransactionId", Request.PortfolioId));
						Params.Add(new FbParameterItem("StatementRunId", Request.StatementRunId));
						Params.Add(new FbParameterItem("EventRunId", Request.EventRunId));
						Params.Add(new FbParameterItem("LoginId", ClientId));
						Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, GetModuleName(), 0);
					}
				
					if (string.IsNullOrEmpty(Result))
					{
						Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
					}
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					if (Entries.Length > 0) Entries.Append(",");
					Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("StatementRunId", Request.StatementRunId));
					Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("EventRunId", Request.EventRunId, false));
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = 
						"SELECT OutputLine FROM EADepositRelease(" +
						"@CompanyId, @TransactionId, @LoginId, @EventRunId, @StatementRunId, @DepositReleaseType, @StatementDate)";
					Params.Clear();
					Params.Add(new FbParameterItem("CompanyId", CompanyId));
					Params.Add(new FbParameterItem("TransactionId", Request.PortfolioId));
					Params.Add(new FbParameterItem("LoginId", ClientId));
					Params.Add(new FbParameterItem("EventRunId", Request.EventRunId));
					Params.Add(new FbParameterItem("StatementRunId", Request.StatementRunId));
					Params.Add(new FbParameterItem("DepositReleaseType", Request.DepositReleaseType));
					Params.Add(new FbParameterItem("StatementDate", Request.StmtDate));
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
		
		public static string EADepositReleaseOK(string ConnectionString,
		                                        int CompanyId,
		                                        int TransactionId)
		{
			string Result = "";
			string Method = "EADepositReleaseOK";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string QueryText = "";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = "SELECT okmessage FROM EADepositReleaseOK(@CompanyId, @TransactionId)";
					Params.Add(new FbParameterItem("CompanyId", CompanyId));
					Params.Add(new FbParameterItem("TransactionId", TransactionId));
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
							Result = (Obj == DBNull.Value) ? "Bad OKMessage Object" : Convert.ToString(Obj);
							if (StringUtilities.SameText(Result, "YES")) Result = "";
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("After EADepositReleaseOK Result: " + Result, Method, 0);
			}
			
			return Result;
		}
		
		public static string EADepositReleasePrepare(string ConnectionString,
		                                             int CompanyId,
		                                             int ClientId,
		                                             int TransactionId,
		                                             ref int StatementRunId,
		                                             ref int EventRunId)
		{
			string Result = "";
			string Method = "EADepositReleasePrepare";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, GetModuleName(), 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				/*
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = "select StatementRunId, EventRunId from EADepositReleasePrepare( @CompanyId, @TransactionId, @LoginId)";
					Params.Add(new FbParameterItem("CompanyId", CompanyId));
					Params.Add(new FbParameterItem("TransactionId", TransactionId));
					Params.Add(new FbParameterItem("LoginId", ClientId));
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, GetModuleName(), 0);
				}
				*/
				int NewId = 0;
				if (string.IsNullOrEmpty(Result))
				{
					NewId = 
						UsefulDBMethods.GeneratorHelpers.GetNextGeneratorId(
							ConnectionString, "I_DEPRELEASEEVENT_ID_GEN", CompanyId, ClientId, out Result);
				}
				
				string QueryText = "";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					QueryText =
						" INSERT INTO EADEPOSITRELEASEEVENT (" +
						" I_DEPRELEASEEVENT_ID, I_DEP_COMPANYCLIENTID," +
						" I_DEP_TRANSACTIONID, I_DEP_LOGINID)" +
						" VALUES(@NewId, @CompanyId, @TransactionId, @LoginId)";
					Params.Add(new FbParameterItem("NewId", NewId));
					Params.Add(new FbParameterItem("CompanyId", CompanyId));
					Params.Add(new FbParameterItem("TransactionId", TransactionId));
					Params.Add(new FbParameterItem("LoginId", ClientId));
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, GetModuleName(), 0);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					QueryText =
						" SELECT I_DEP_STATEMENTRUNID, I_DEP_EVENTRUNID FROM EADEPOSITRELEASEEVENT" +
						" WHERE I_DEPRELEASEEVENT_ID = @NewId";
					Params.Clear();
					Params.Add(new FbParameterItem("NewId", NewId));
					Result = Helpers.WriteParamsToInfoEvent(QueryText, Params, GetModuleName(), 0);
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
							Obj = Row["I_DEP_STATEMENTRUNID"];
							StatementRunId = (Obj == DBNull.Value) ? 0: Convert.ToInt32(Obj);
							Obj = Row["I_DEP_EVENTRUNID"];
							EventRunId = (Obj == DBNull.Value) ? 0: Convert.ToInt32(Obj);
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("After EADepositReleasePrepare Result: " + Result, Method, 0);
			}
			
			return Result;
		}
		
		public static string DisplayReport(string ConnectionString,
		                                   int CompanyId,
		                                   int ClientId,
		                                   SetupPageEntriesRequestItem Request,
		                                   ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "DisplayReport";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
			
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string ReportSource = "";
				if (string.IsNullOrEmpty(Result))
				{
					ReportSource = GetReportSource(ConnectionString, CompanyId, ClientId, Request, out Result);
				}
				
				string OutputFileName = "";
				long UniqueId = (long)DateTime.Now.Subtract(DateTime.MinValue.AddYears(1969)).TotalMilliseconds;
				if (string.IsNullOrEmpty(Result))
				{
					Int64 NewId =
						global::UsefulDBMethods.Reporting.SystemReportUtilities.Execute(
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("After DisplayReport Result: " + Result, Method, 0);
			}
			
			return Result;
		}
		
		private static string GetReportSource(string ConnectionString,
		                                      int CompanyId,
		                                      int ClientId,
		                                      SetupPageEntriesRequestItem Request,
		                                      out string ErrorMsg)
		{
			string Result = "";
			ErrorMsg = "";
			string Method = "GetReportSource";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
			
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				StringBuilder Entries = new StringBuilder();				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg = SetupPageEntries(ConnectionString, CompanyId, ClientId, Request, ref Entries);
				}
				
				string Str =
					JsonUtilities.StartJson() +
					Entries.ToString() +
					JsonUtilities.EndJson();
				WindowsEventLoggingUtilities.WriteToInfoEvent("JsonText: " + Environment.NewLine + Str, Method, 0);
					
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Result = JsonUtilities.GetJsonPathValueStr(Str, "$.B1Value", out ErrorMsg);
				}				
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
				WindowsEventLoggingUtilities.WriteToInfoEvent("After GetReportSource Result: " + Result, Method, 0);
			}
			
			return Result;
		}
		
		public static string Post(string ConnectionString,
		                          int CompanyId,
		                          int ClientId,
		                          SetupPageEntriesRequestItem Request,
		                          ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "Post";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				StringBuilder TempEntries = new StringBuilder();
				Result = SetupPageEntries(ConnectionString, CompanyId, ClientId, Request, ref TempEntries);

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
				
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = QueryText.Replace("__", "'");
					Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					UsefulDBMethods.Helpers.RunEAMainOfficeProcess(ConnectionString, CompanyId);
					UsefulDBMethods.Helpers.RunEAMainOfficeProcess(ConnectionString, CompanyId);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = SetupPageEntries(ConnectionString, CompanyId, ClientId, Request, ref Entries);
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
		
		public static string SetupEmail(string ConnectionString,
		                                int CompanyId,
		                                int ClientId,
		                                SetupPageEntriesRequestItem Request,
		                                ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "SetupEmail";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				StringBuilder TempEntries = new StringBuilder();
				Result = SetupPageEntries(ConnectionString, CompanyId, ClientId, Request, ref TempEntries);

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
					QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.eMailSQL", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the query";
					if (string.IsNullOrEmpty(Result))
					{
						WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText:" + Environment.NewLine + QueryText, GetModuleName(), 0);
					}
				}				
				
				DataTable tbl = null;
				object Obj = null;
				int LedgerBulkId = 0;
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, QueryText, null, 0, FirebirdQueryCheckTypes.HasTables, out Result);
					if (string.IsNullOrEmpty(Result))
					{
						foreach (DataRow Row in tbl.Rows)
						{
							Obj = Row["ID"];
							LedgerBulkId = (Obj == DBNull.Value) ? 0: Convert.ToInt32(Obj);
						}
					}
					
					if (string.IsNullOrEmpty(Result) && (LedgerBulkId == 0))
					{
						Result = "Error building the required Id value.";
					}
				}				
				
				if (string.IsNullOrEmpty(Result))
				{
					if (Entries.Length > 0) Entries.Append(",");
					Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("LedgerBulkId", LedgerBulkId, false));
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
		
		public static string CreatePaymentFile(string ConnectionString,
		                                       int CompanyId,
		                                       int ClientId,
		                                       SetupPageEntriesRequestItem Request,
		                                       ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "CreatePaymentFile";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				StringBuilder TempEntries = new StringBuilder();
				Result = SetupPageEntries(ConnectionString, CompanyId, ClientId, Request, ref TempEntries);

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
					QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.CreateFileSQL1", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the first query";
					if (string.IsNullOrEmpty(Result))
					{
						WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText:" + Environment.NewLine + QueryText, GetModuleName(), 0);
					}
				}				
				
				int EventId = 0;
				DataTable tbl = null;
				object Obj = null;
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, QueryText, null, 0, FirebirdQueryCheckTypes.HasTables, out Result);
					if (string.IsNullOrEmpty(Result))
					{
						foreach (DataRow Row in tbl.Rows)
						{
							Obj = Row["EventId"];
							EventId = (Obj == DBNull.Value) ? 0: Convert.ToInt32(Obj);
						}
					}
					
					if (string.IsNullOrEmpty(Result) && (EventId == 0))
					{
						Result = "Error building the Event Id value.";
					}
				}				
				
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.CreateFileSQL2", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the second query";
				}
				
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					Params.Add(new FbParameterItem("EventId", EventId));
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, GetModuleName(), 0);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.CreateFileSQL3", out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the third query";
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Params.Clear();
					Params.Add(new FbParameterItem("EventId", EventId));
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, GetModuleName(), 0);
				}
				
				string CommandLine = "";
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, QueryText, Params, 0, FirebirdQueryCheckTypes.HasTables, out Result);
					if (string.IsNullOrEmpty(Result))
					{
						foreach (DataRow Row in tbl.Rows)
						{
							Obj = Row["ec_commandline"];
							CommandLine = (Obj == DBNull.Value) ? "": Convert.ToString(Obj);
						}
					}
					
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(CommandLine))
					{
						Result = "Error building the command line value.";
					}
					
					if (string.IsNullOrEmpty(Result))
					{
						WindowsEventLoggingUtilities.WriteToInfoEvent("CreateFileSql3 Result: " + Result, GetModuleName(), 0);
					}
				}
				
				string AppLocation = "";
				string DocsLocation = "";
				global::UsefulDBMethods.Reporting.ReportUtilities reportUtils = new global::UsefulDBMethods.Reporting.ReportUtilities();
				if (string.IsNullOrEmpty(Result))
				{
					reportUtils.GetCompanySettingsItems(ConnectionString, CompanyId, out AppLocation, out DocsLocation, out Result);
					
					if (string.IsNullOrEmpty(Result))
					{
						if  (!AppLocation.EndsWith(@"\")) AppLocation += @"\";
						AppLocation += "{0}";
					}
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent(
						"CommandLine: " + CommandLine + Environment.NewLine +
						"AppLocation: " + AppLocation + Environment.NewLine +
						"DocsLocation: " + DocsLocation, GetModuleName(), 0);
					reportUtils.BuildReportFileListEntries(
						ConnectionString, EventId, CommandLine, AppLocation, DocsLocation, "Items", ref Entries, out Result);
					WindowsEventLoggingUtilities.WriteToInfoEvent("Entries:" + Environment.NewLine + Entries.ToString(), GetModuleName(), 0);
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
