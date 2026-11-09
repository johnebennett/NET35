/*
 * Created by SharpDevelop.
 * User: John
 * Date: 06/02/2019
 * Time: 10:38 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CommonUtilities;
using EAOfficeAPI;
using UsefulDBMethods;

namespace Actions.Portfolio
{
	/// <summary>
	/// Description of SearchResults.
	/// </summary>
	public class SearchResults : ActionBase
	{
		/*
		public SearchResults()
		{
		}
		*/
		private string FModuleName = "Portfolio";
		
		private SearchGridRequestItem FSearchGridRequestItem = new SearchGridRequestItem();
		
		public static string Execute(int MasterId,
		                             RequestCommonItem CommonItem,
		                             JObject DataObject,
		                             APISettingsItem SettingsItem,
		                             out string ErrorMsg)
		{
			string Method = "DefaultMethods - Execute";
			string Result = "";
			ErrorMsg = "";

			SearchResults inst = new SearchResults();
			if (!inst.Setup(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg))
				return "";
			
			inst.LogOnInsideMethod(Method);
			try
			{
				Result = inst.ExecuteCommon(out ErrorMsg);
			}
			finally
			{
				inst.LogOnLeavingMethod(Method);
			}
			
			return Result;
		}
		
		private string ExecuteCommon(out string ErrorMsg)
		{
			ErrorMsg = "";
			string Method = "ExecuteCommon";
			LogOnInsideMethod(Method);
			
			string Result = "";
			try 
			{
				if (CommonUtilities.StringUtilities.SameText(this.CommonItem.Action, "SearchResults"))
				    Result = ExecuteSearchResults(out ErrorMsg);
			}
			catch (Exception Ex)
			{
				Result = LogOnException(Method, Ex, "");
			}
			finally
			{
				LogOnLeavingMethod(Method);
			}
			
			return Result;
		}
		
		private string ExecuteSearchResults(out string ErrorMsg)
		{
			ErrorMsg = "";
			string Result = "";
			
			int Filter1Id = 0;
			int Filter2Id = 0;

			int Filter1EventId = 0;
			int Filter2EventId = 0;
			
			string AskLater = "";
			string AskFor = "";
			string FromValue = "";
			string FromDate = "";
			string ToValue = "";
			string ToDate = "";

			const string Method = "ExecuteCommon";
			string ConnectionString = CommonDBMethods.Common.ConnectionString(APISettings);
			LogOnInsideMethod(Method);
			try 
			{
				ErrorMsg = FSearchGridRequestItem.Load(DataObject);
				if (!string.IsNullOrEmpty(ErrorMsg)) return Result;
				
				string UseDefaults = (string)DataObject["useDefaults"];
				if (StringUtilities.SameText(UseDefaults, "Y"))
				{
					SearchGridRequestItem tempItem = new SearchGridRequestItem();					
					ErrorMsg = tempItem.Load(ConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue);
					if (!string.IsNullOrEmpty(ErrorMsg)) return Result;
					
					FSearchGridRequestItem.OfficeIdValue = tempItem.OfficeIdValue;
					FSearchGridRequestItem.ManagerIdValue = tempItem.ManagerIdValue;
					FSearchGridRequestItem.PortfolioIdValue = tempItem.PortfolioIdValue;
				}
				ErrorMsg = FSearchGridRequestItem.Validate();
				if (!string.IsNullOrEmpty(ErrorMsg))
				{
					Result = LogOnErrorValidatingData(Method, ErrorMsg, "");
					return Result;
				}
			
				WindowsEventLoggingUtilities.WriteToInfoEvent("Inside ExecuteSearchResults", FModuleName, MasterId);
				Filter1Id = StringUtilities.StrToIntDef((string)DataObject["filter1Id"], 0);
				Filter2Id = StringUtilities.StrToIntDef((string)DataObject["filter2Id"], 0);
			
				if (Filter1Id > 0)
				{
					Filter1EventId = 
						UsefulDBMethods.EventHelpers.SetupFilterEventRecord(
							this.FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue, Filter1Id, out ErrorMsg);
					if (!string.IsNullOrEmpty(ErrorMsg)) return this.LogOnProcessingError(Method, ErrorMsg, "");
					WindowsEventLoggingUtilities.WriteToInfoEvent("Filter1EventId" + Filter1EventId.ToString(), FModuleName, MasterId);
					
					if (Filter1EventId > 0)
					{
						ErrorMsg = CommonMethods.GetFilterValues(1, DataObject, out AskLater, out AskFor, out FromValue, out FromDate, out ToValue, out ToDate);
						if (!string.IsNullOrEmpty(ErrorMsg)) return this.LogOnProcessingError(Method, ErrorMsg, "");
						if (CommonUtilities.StringUtilities.SameText(AskLater, "1"))
					    {
							ErrorMsg = 
								EventHelpers.UpdateFilterEventRecordItems(
									Filter1EventId, AskLater, AskFor, FromValue, FromDate, ToValue, ToDate, this.FbConnectionString);
							if (!string.IsNullOrEmpty(ErrorMsg)) return this.LogOnProcessingError(Method, ErrorMsg, "");
							ErrorMsg = EventHelpers.CallFilterEventSqlCreate(Filter1EventId, this.FbConnectionString);
							if (!string.IsNullOrEmpty(ErrorMsg)) return this.LogOnProcessingError(Method, ErrorMsg, "");
						}
					}
				}
				
				
				if (Filter2Id > 0)
				{
					Filter2EventId = 
						UsefulDBMethods.EventHelpers.SetupFilterEventRecord(
							this.FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue, Filter2Id, out ErrorMsg);
					if (!string.IsNullOrEmpty(ErrorMsg)) return this.LogOnProcessingError(Method, ErrorMsg, "");
					
					if (Filter2EventId > 0)
					{
						WindowsEventLoggingUtilities.WriteToInfoEvent("Filter2EventId" + Filter2EventId.ToString(), FModuleName, MasterId);
						ErrorMsg = CommonMethods.GetFilterValues(2, DataObject, out AskLater, out AskFor, out FromValue, out FromDate, out ToValue, out ToDate);
						if (!string.IsNullOrEmpty(ErrorMsg)) return this.LogOnProcessingError(Method, ErrorMsg, "");
						if (CommonUtilities.StringUtilities.SameText(AskLater, "1"))
					    {
							ErrorMsg = 
								EventHelpers.UpdateFilterEventRecordItems(
									Filter2EventId, AskLater, AskFor, FromValue, FromDate, ToValue, ToDate, this.FbConnectionString);
							if (!string.IsNullOrEmpty(ErrorMsg)) return this.LogOnProcessingError(Method, ErrorMsg, "");
							ErrorMsg = EventHelpers.CallFilterEventSqlCreate(Filter2EventId, this.FbConnectionString);
							if (!string.IsNullOrEmpty(ErrorMsg)) return this.LogOnProcessingError(Method, ErrorMsg, "");
						}
					}
				}
			
				StringBuilder sb = new StringBuilder();
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (!string.IsNullOrEmpty(ConnectionString)) {
					WindowsEventLoggingUtilities.WriteToInfoEvent("ConnectionString: " + ConnectionString, FModuleName, MasterId);
					string QueryText = 
						"SELECT SQLSTRING FROM ListPortfolio(" + 
						"@CompanyId, @OfficeId, @ManagerId, @PortfolioType, @SearchText, @LedgerTypeId, @Ledger, @Filter1EventId, @ActiveStatus, @Filter2EventId)";
					Params.Add(new FbParameterItem("@CompanyId", CommonItem.CompanyId));
					
					/*
					Params.Add(new FbParameterItem("@OfficeId",	StringUtilities.StrToIntDef((string)DataObject["officeId"], 0)));
					Params.Add(new FbParameterItem("@ManagerId", StringUtilities.StrToIntDef((string)DataObject["managerId"], 0)));
					Params.Add(new FbParameterItem("@PortfolioType", StringUtilities.StrToIntDef((string)DataObject["busTypeId"], 33)));
					*/

					Params.Add(new FbParameterItem("@OfficeId",	FSearchGridRequestItem.OfficeId));
					Params.Add(new FbParameterItem("@ManagerId", FSearchGridRequestItem.ManagerId));
					Params.Add(new FbParameterItem("@PortfolioType", FSearchGridRequestItem.PortfolioId));

					Params.Add(new FbParameterItem("@SearchText", DataObject["searchText"]));
					Params.Add(new FbParameterItem("@LedgerTypeId", StringUtilities.StrToIntDef((string)DataObject["ledgerTypeId"], 0)));
					Params.Add(new FbParameterItem("@Ledger", StringUtilities.StrToIntDef((string)DataObject["ledger"], 0)));
					Params.Add(new FbParameterItem("@Filter1EventId", Filter1EventId));
					Params.Add(new FbParameterItem("@ActiveStatus", StringUtilities.StrToIntDef((string)DataObject["activeStatusId"], 0)));
					Params.Add(new FbParameterItem("@Filter2EventId", Filter2EventId));

					ErrorMsg = ExecuteSqlStartString(ConnectionString, Params);
					if (!string.IsNullOrEmpty(ErrorMsg)) return this.LogOnProcessingError(Method, ErrorMsg, "");
					
					//WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText:" + Environment.NewLine + QueryText, FModuleName, MasterId);
					UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);

					DataSet dataSetFromQuery = FirebirdUtilities.GetDataSet(ConnectionString, QueryText, Params, 0);
					if (dataSetFromQuery == null)
					{
						ErrorMsg = 
							"Error Executing Query" + Environment.NewLine +
							ErrorMsg + Environment.NewLine +
							"QueryText:" + Environment.NewLine + QueryText;
						WindowsEventLoggingUtilities.WriteToInfoEvent(ErrorMsg, "EAOfficeAPI", MasterId);
						Result = LogOnErrorSelectingData(Method, ErrorMsg, QueryText);
						return Result;
					}
					else if (dataSetFromQuery.Tables.Count == 0) {
						ErrorMsg = "No tables in the dataset to process";
						Result = LogOnErrorSelectingData(Method, ErrorMsg, QueryText);
						return Result;
					}
					foreach (DataRow dataRow in dataSetFromQuery.Tables[0].Rows) {
						sb.AppendLine((dataRow[0] == DBNull.Value) ? "" : Convert.ToString(dataRow[0]));
					}
					
					int ReportEventId = 
						CommonMethods.FilterEventForPortfolio(
							ConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue, 
							Filter1EventId, Filter2EventId, sb.ToString(), "ListPortfolio-SearchResults", out ErrorMsg);
					if (!string.IsNullOrEmpty(ErrorMsg))
					{
						Result = LogOnProcessingError(Method, Result, "");
					}
					else // We now have the query to call for the Portfolio Search Results
						Result = CallQueryAndBuildResult(sb.ToString(), ConnectionString, ReportEventId, out ErrorMsg);
				}
			}
			catch (Exception Ex)
			{
				Result = LogOnException(Method, Ex, "");
			}
			finally
			{
				LogOnLeavingMethod(Method);
			}				

			return Result;
		}
		
		/*		
		private int SetupFilterEventRecord(int CompanyId, 
		                                   int ClientId, 
		                                   int FilterId,
		                                   out string ErrorMsg)
		{
			ErrorMsg = "";
			int Result = 0;
			
			Result = GetNextGeneratorId("EA_FILTEREVENTID_GEN", CompanyId, ClientId, out ErrorMsg);
			if (!string.IsNullOrEmpty(ErrorMsg))
			{
				WindowsEventLoggingUtilities.WriteToErrorEvent("Error getting next generator: " + ErrorMsg, FModuleName, MasterId);
				return 0;
			}
			
			// do the insert into the FilterEvent table
			string QueryText = 
				"INSERT INTO EAFilterEvent (EA_FilterEventId, EA_CompanyClientId, EA_RequestedByClientId, ea_FilterListId, ea_RequestedByDate) " +
				"VALUES({0}, {1}, {2}, {3}, '{4}')";
			WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText" + Environment.NewLine + QueryText, FModuleName, MasterId);
			QueryText =
				string.Format(
					QueryText, Result, CompanyId, ClientId, FilterId, "01/01/2019");
			ErrorMsg = FirebirdUtilities.DBExecute(FbConnectionString, QueryText);
			if (!string.IsNullOrEmpty(ErrorMsg))
			{
				WindowsEventLoggingUtilities.WriteToErrorEvent("Error executing query: " + ErrorMsg, FModuleName, MasterId);
				return 0;
			}
			
			return Result;
		}
		
		private int GetNextGeneratorId(string GeneratorName, 
		                               int CompanyId,
		                               int ClientId,
		                               out string ErrorMsg)
		{
			ErrorMsg = "";
			int Result = 0;
			DataTable dt = null;			
			
			string QueryText = "SELECT Gen_ID({0}, 1) AS GenId FROM EALOGIN WHERE I_ALOGIN_CompanyId = {1} AND I_ALOGIN_CLIENTID = {2}";
			QueryText = string.Format(QueryText, GeneratorName, CompanyId, ClientId);
			WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText:" + Environment.NewLine + QueryText, "GetNextGeneratorId", MasterId);
			try 
			{
				dt = FirebirdUtilities.GetFirstDataTable(this.FbConnectionString, QueryText);
				if (dt == null)
				{
					ErrorMsg = 
						"Error retrieving the Generator Id for the Filter Event Record" + Environment.NewLine +
						"QueryText" + Environment.NewLine + QueryText;
				}
				else
				{
					// get the value from the table
					object obj = dt.Rows[0]["GenId"];
					Result = (obj == DBNull.Value) ? 0  : Convert.ToInt32(obj);
				}
			}
			catch (Exception Ex)
			{
				ErrorMsg = Ex.Message + Environment.NewLine + Ex.StackTrace;
			}
			
			return Result;
		}
		*/
		
		private string CallQueryAndBuildResult(string QueryText,
		                                       string ConnectionString,
		                                       int ReportEventId,
		                                       out string ErrorMsg)
		{
			ErrorMsg = "";
			string Result = "";
			const string Method = "CallQueryAndBuildResult";
			
			LogOnInsideMethod(Method);
			try 
			{
				QueryText = QueryText.Replace("\"", "'");
				WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText" + Environment.NewLine + QueryText, "EAOfficeAPI", MasterId);
				DataSet dataSetFromQuery = FirebirdUtilities.GetDataSetFromQuery(ConnectionString, QueryText, out ErrorMsg);
				if (!string.IsNullOrEmpty(ErrorMsg)) {
					ErrorMsg = 
						"Error Executing Query" + Environment.NewLine +
						ErrorMsg + Environment.NewLine +
						"QueryText:" + Environment.NewLine + QueryText;
					WindowsEventLoggingUtilities.WriteToInfoEvent(ErrorMsg, "EAOfficeAPI", MasterId);
					Result = LogOnErrorSelectingData(Method, ErrorMsg, QueryText);
					return Result;
				}
				else if (dataSetFromQuery.Tables.Count == 0) {
					ErrorMsg = "No tables in the dataset to process";
					Result = LogOnErrorSelectingData(Method, ErrorMsg, QueryText);
					return Result;
				}

				DataTable tbl = dataSetFromQuery.Tables[0];
				string Str = "";
				string ColName = "";
				string ColValue = "";
				int ColCount = tbl.Columns.Count;
				int RowCount = tbl.Rows.Count;
				//DataRow CurrentRow = null;
				List<string> RowList = new List<string>();
				List<string> StrList = new List<string>();
				//string QuotedStr = "\"{0}\"";

	            StringBuilder sb = new StringBuilder();
				sb.Append(CommonUtilities.JsonUtilities.StartJson());
				sb.Append(CommonUtilities.JsonUtilities.AddPropertyNameAndValue("reportEventId", ReportEventId));

	            sb.Append(CommonUtilities.JsonUtilities.StartJsonArray("Items"));
				RowList.Clear();				
				object Obj = null;
				Str = "";
				foreach(DataRow ThisRow in tbl.Rows)
				{
					StrList.Clear();
					foreach(DataColumn ThisCol in tbl.Columns)
					{
						ColName = ThisCol.ColumnName;
						Obj = ThisRow[ColName];
						ColValue = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
						StrList.Add(CommonUtilities.JsonUtilities.AddPropertyNameAndValue(ColName, ColValue, false));
					}
					Str =
						CommonUtilities.JsonUtilities.StartJsonObject("") +
						string.Join(",", StrList.ToArray()) +
						CommonUtilities.JsonUtilities.EndJsonObject();
					RowList.Add(Str);
				}
				
				sb.Append(string.Join(",", RowList.ToArray()));
	            sb.Append(CommonUtilities.JsonUtilities.EndJsonArray());

	            sb.Append(CommonUtilities.JsonUtilities.EndJson());
				Result = sb.ToString();
			}
			catch (Exception Ex)
			{
				Result = LogOnException(Method, Ex, "");
			}
			finally
			{
				LogOnLeavingMethod(Method);
			}
			
			return Result;			
		}
		
		private string ExecuteSqlStartString(string ConnectionString,
		                                     List<FbParameterItem> Params)
		{
			string Result = "";
			string Method = "ExecuteSqlStartString";
			try
			{
					
				string QueryText = 
					"SELECT SqlStartString FROM ListPortfolio(" + 
					"@CompanyId, @OfficeId, @ManagerId, @PortfolioType, @SearchText, @LedgerTypeId, @Ledger, @Filter1EventId, @ActiveStatus, @Filter2EventId)";
				UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
				
				string SqlStartString = "";
				DataTable tbl =
					UsefulDBMethods.Helpers.GetFirstDataTableEx(ConnectionString, QueryText, Params, 0, FirebirdQueryCheckTypes.HasTables, out Result);
				if (string.IsNullOrEmpty(Result))
				{
					if (tbl.Rows.Count > 0)
					{
						object Obj = tbl.Rows[0]["SqlStartString"];
						SqlStartString = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					}
					
					if (!string.IsNullOrEmpty(SqlStartString))
					{
						// We don't do anything with the results, just primes the database for the big query that is to come
						// when we get back to the calling method
						SqlStartString = SqlStartString.Replace("\"", "'");
						WindowsEventLoggingUtilities.WriteToInfoEvent("SqlStartString:" + Environment.NewLine + SqlStartString, Method, 0);
						tbl = UsefulDBMethods.Helpers.GetFirstDataTableEx(ConnectionString, SqlStartString, null, 0, FirebirdQueryCheckTypes.None, out Result);
					}
					if (!string.IsNullOrEmpty(Result))
					{
						Result = LogOnErrorSelectingData(Method, Result, SqlStartString);
					}
				}
				else
				{
					Result = LogOnErrorSelectingData(Method, Result, QueryText);
					WindowsEventLoggingUtilities.WriteToInfoEvent("Error" + Environment.NewLine + Result, "EAOfficeAPI", MasterId);
				}
					
			}
			catch (Exception Ex)
			{
				Result = LogOnException(Method, Ex, "");
			}
			finally
			{
				LogOnLeavingMethod(Method);
			}
			
			return Result;			
		}
	}
}