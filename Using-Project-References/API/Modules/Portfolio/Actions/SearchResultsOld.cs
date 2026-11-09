/*
 * Created by SharpDevelop.
 * User: John
 * Date: 05/11/2018
 * Time: 8:23 AM
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
	public class SearchResultsOld : ActionBase
	{
		/*
		public SearchResults()
		{
		}
		*/
		private string FModuleName = "Portfolio";
		
		public static string Execute(int MasterId,
		                             RequestCommonItem CommonItem,
		                             JObject DataObject,
		                             APISettingsItem SettingsItem,
		                             out string ErrorMsg)
		{
			string Method = "DefaultMethods - Execute";
			string Result = "";
			ErrorMsg = "";

			SearchResultsOld inst = new SearchResultsOld();
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

			const string Method = "ExecuteCommon";
			string ConnectionString = CommonDBMethods.Common.ConnectionString(APISettings);
			LogOnInsideMethod(Method);
			try 
			{
				WindowsEventLoggingUtilities.WriteToInfoEvent("Inside ExecuteSearchResults", FModuleName, MasterId);
				Filter1Id = StringUtilities.StrToIntDef((string)DataObject["filterId"], 0);
				Filter2Id = StringUtilities.StrToIntDef((string)DataObject["filter2Id"], 0);
			
				if (Filter1Id > 0)
				{
					Filter1EventId = 
						UsefulDBMethods.EventHelpers.SetupFilterEventRecord(
							this.FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue, Filter1Id, out ErrorMsg);
				}
				if (!string.IsNullOrEmpty(ErrorMsg)) return "";
				
				if (Filter2Id > 0)
				{
					Filter2EventId = 
						UsefulDBMethods.EventHelpers.SetupFilterEventRecord(
							this.FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue, Filter2Id, out ErrorMsg);
				}
				if (!string.IsNullOrEmpty(ErrorMsg)) return "";
			
				StringBuilder sb = new StringBuilder();
				if (!string.IsNullOrEmpty(ConnectionString)) {
					WindowsEventLoggingUtilities.WriteToInfoEvent("ConnectionString: " + ConnectionString, FModuleName, MasterId);
					//string QueryText = "SELECT * FROM ListPortfolio({0}, {1}, {2}, {3}, '{4}', {5}, {6}, {7}, {8}, {9})";
					string QueryText = "SELECT SQLSTRING FROM ListPortfolio({0}, {1}, {2}, {3}, '{4}', {5}, {6}, {7}, {8}, {9})";
					QueryText = 
						string.Format(
							QueryText,
							StringUtilities.StrToIntDef(CommonItem.CompanyId, 0),
							StringUtilities.StrToIntDef((string)DataObject["officeId"], 0),
							StringUtilities.StrToIntDef((string)DataObject["managerId"], 0),
							StringUtilities.StrToIntDef((string)DataObject["busTypeId"], 33),
							DataObject["searchText"],
							StringUtilities.StrToIntDef((string)DataObject["ledgerTypeId"], 0),
							StringUtilities.StrToIntDef((string)DataObject["ledger"], 0),
							Filter1EventId,
							StringUtilities.StrToIntDef((string)DataObject["activeStatusId"], 0),
							Filter2EventId);
					WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText:" + Environment.NewLine + QueryText, FModuleName, MasterId);
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
					foreach (DataRow dataRow in dataSetFromQuery.Tables[0].Rows) {
						sb.AppendLine((dataRow[0] == DBNull.Value) ? "" : Convert.ToString(dataRow[0]));
					}
					
					// We now have the query to call for the Portfolio Search Results
					Result = CallQueryAndBuildResult(sb.ToString(), ConnectionString, out ErrorMsg);
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
				DataRow CurrentRow = null;
				List<string> RowList = new List<string>();
				List<string> StrList = new List<string>();
				string QuotedStr = "\"{0}\"";

	            StringBuilder sb = new StringBuilder();
				sb.Append(CommonUtilities.JsonUtilities.StartJson());
				
	            sb.Append(CommonUtilities.JsonUtilities.StartJsonArray("fieldNames"));
	            StrList.Clear();
				foreach (DataColumn column in tbl.Columns)
				{
					StrList.Add(string.Format(QuotedStr, column.ColumnName));
				}
				sb.Append(string.Join(",", StrList.ToArray()));
	            sb.Append(CommonUtilities.JsonUtilities.EndJsonArray());
	            
	            // Do the extended field names array
	            /*
	            sb.Append(",");
	            
	            sb.Append(CommonUtilities.JsonUtilities.StartJsonArray("fieldNamesEx"));
	            RowList.Clear();
				foreach (DataColumn column in tbl.Columns)
				{
		            StrList.Clear();
					StrList.Add(CommonUtilities.JsonUtilities.AddPropertyNameAndValue("Name", column.ColumnName, false));
					StrList.Add(CommonUtilities.JsonUtilities.AddPropertyNameAndValue("Type", column.DataType.FullName, false));
					StrList.Add(CommonUtilities.JsonUtilities.AddPropertyNameAndValue("MaxLength", column.MaxLength.ToString(), false));
					Str =
						CommonUtilities.JsonUtilities.StartJsonObject("") +
						string.Join(",", StrList.ToArray()) +
						CommonUtilities.JsonUtilities.EndJsonObject();
					RowList.Add(Str);
				}
				sb.Append(string.Join(",", RowList.ToArray()));
	            sb.Append(CommonUtilities.JsonUtilities.EndJsonArray());
	            */
	            
	            // Row entries
	            sb.Append(",");
	            
	            sb.Append(CommonUtilities.JsonUtilities.StartJsonArray("rows"));
				RowList.Clear();
				for (int RowIndex = 0; RowIndex < RowCount; RowIndex++)
				{
					CurrentRow = tbl.Rows[RowIndex];
					StrList.Clear();
					/*
	            	for (int Index = 0; Index < ColCount; Index++)
	            	{
	            		ColValue = ((CurrentRow[Index] == DBNull.Value) ? "" : Convert.ToString(CurrentRow[Index]));
	            		ColValue = cleanForJSON(ColValue);
	            		ColValue = string.Format(QuotedStr, ColValue);
	            		StrList.Add(ColValue);
	            	}
	            	*/
	            	for (int ColIndex = 0; ColIndex < tbl.Columns.Count; ColIndex++)
	            	{
	            		ColName = tbl.Columns[ColIndex].ColumnName;
	            		if ((ColName.StartsWith("I_")) ||
	            		    (ColName.StartsWith("B_")) ||
	            		    (ColName.StartsWith("D_")) ||
	            		    (ColName.StartsWith("M_")))
	            		{
	            			ColName = ColName.Substring(2);
	            		}
	            		else if ((ColName.StartsWith("ZZ")) ||
	            		         (ColName.StartsWith("ZL")) ||
	            		         (ColName.StartsWith("ZR")) ||
	            		         (ColName.StartsWith("ZC")))
	            		{
	            			ColName = ColName.Substring(ColName.IndexOf("_") + 1);
	            		}
	            		      
	            		ColValue = ((CurrentRow[ColIndex] == DBNull.Value) ? "" : Convert.ToString(CurrentRow[ColIndex]));
	            		/*
	            		ColValue = cleanForJSON(ColValue);
	            		ColValue = string.Format(QuotedStr, ColValue);
	            		*/
	            		StrList.Add(CommonUtilities.JsonUtilities.AddPropertyNameAndValue(ColName, ColValue, false));
	            	}
	            	Str =
	            		CommonUtilities.JsonUtilities.StartJsonObject("") +
	            		string.Join(",", StrList.ToArray()) +
	            		CommonUtilities.JsonUtilities.EndJsonObject();
	            	RowList.Add(Str);
	            	
					//sb.AppendLine((dataRow[0] == DBNull.Value) ? "" : Convert.ToString(dataRow[0]));
				}
				sb.Append(string.Join(",", RowList.ToArray()));
	            sb.AppendLine(CommonUtilities.JsonUtilities.EndJsonArray());
				
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
		
		/*
		public static string CleanForJSON(string s)
		{
			//if (s == null || s.Length == 0) { return ""; }
			if (string.IsNullOrEmpty(s)) return "";
			
			char         c = '\0';
		    int          i;
		    int          len = s.Length;
		    StringBuilder sb = new StringBuilder(len + 4);
		    String       t;
		
		    for (i = 0; i < len; i += 1) 
		    {
		    	c = s[i];
		    	switch (c) 
		    	{
		    		case '\\':
		            case '"':
		    			sb.Append('\\');
		                sb.Append(c);
		                break;
		            case '/':
		            	sb.Append('\\');
		                sb.Append(c);
		                break;
		            case '\b':
		            	sb.Append("\\b");
		                break;
		            case '\t':
		            	sb.Append("\\t");
		                break;
		            case '\n':
		                sb.Append("\\n");
		                break;
		            case '\f':
		            	sb.Append("\\f");
		                break;
		            case '\r':
		                sb.Append("\\r");
		                break;
		            default:
		                if (c < ' ') 
		                {
		                	t = "000" + String.Format("X", c);
		                	sb.Append("\\u" + t.Substring(t.Length - 4));
		                }
		                else
		                {
		                	sb.Append(c);
		                }
		                break;
		             
		        }
		    }
		    return sb.ToString();
		}
		*/
	}
}