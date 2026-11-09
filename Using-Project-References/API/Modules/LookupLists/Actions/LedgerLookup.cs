/*
 * Created by SharpDevelop.
 * User: John
 * Date: 22/11/2018
 * Time: 11:21 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Data;
using System.Text;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CommonUtilities;
using EAOfficeAPI;

namespace Actions.LookupLists
{
	/// <summary>
	/// Description of LedgerLookup.
	/// </summary>
	public class LedgerLookup : ActionBase
	{
		private string FModuleName = "LedgerLookup";
		
		private string FSearchText = "";
		
		public static string Execute(int MasterId,
			RequestCommonItem CommonItem,
			JObject DataObject,
			APISettingsItem SettingsItem,
			out string ErrorMsg)
		{
			string Method = "LedgerLookup - Execute";
			string Result = "";
			ErrorMsg = "";

			LedgerLookup inst = new LedgerLookup();
			if (!inst.Setup(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg))
				return "";
			
			inst.LogOnInsideMethod(Method);
			try 
			{
				Result = inst.Init_LedgerLookup();
				if (!string.IsNullOrEmpty(Result))
					return "";
				
				Result = inst.Validate_LedgerLookup();
				if (!string.IsNullOrEmpty(Result))
					return Result;
				
				Result = inst.Output_LedgerLookup();
			} 
			finally 
			{
				inst.LogOnLeavingMethod(Method);
			}
			
			return Result;
		}		
		
		private string Init_LedgerLookup()
		{
			string Method = "Init_LedgerLookup";
			string Result = "";
			
			LogOnInsideMethod(Method);
			try 
			{
				FSearchText = (string)DataObject["searchText"];
			}
			catch (Exception ex) 
			{
				Result = LogOnException(Method, ex, "");
			}
			finally 
			{
				LogOnLeavingMethod(Method);
			}			
			return Result;
		}
		
		private string Validate_LedgerLookup()
		{
			string Method = "Validate_LedgerLookup";
			string Result = "";
			
			LogOnInsideMethod(Method);
			try 
			{
			}
			catch (Exception ex)
			{
				Result = LogOnException(Method, ex, "");
			}
			finally
			{
				LogOnLeavingMethod(Method);
			}			
			return Result;
		}
		
		private string Output_LedgerLookup()
		{
			string Method = "Output_LedgerLookup";
			string Result = "";
			string ErrorMsg = "";
			
			LogOnInsideMethod(Method);
			try
			{
				string ConnectionString = CommonDBMethods.Common.ConnectionString(APISettings);
				if (!string.IsNullOrEmpty(ConnectionString)) 
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent("ConnectionString: " + ConnectionString, FModuleName, MasterId);
					string QueryText = "SELECT * FROM ea_LedgerList('{0}', {1})";
					QueryText = string.Format(QueryText, FSearchText, CommonItem.CompanyIdValue);
					WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText: " + Environment.NewLine + QueryText, FModuleName, MasterId);
					DataSet dataSet = FirebirdUtilities.GetDataSetFromQuery(ConnectionString, QueryText, out ErrorMsg);
					if (!string.IsNullOrEmpty(ErrorMsg)) {
						ErrorMsg = 
							"Error Executing Query" + Environment.NewLine +
							ErrorMsg + Environment.NewLine +
							"QueryText:" + Environment.NewLine + QueryText;
						WindowsEventLoggingUtilities.WriteToInfoEvent(ErrorMsg, "EAOfficeAPI", MasterId);
						Result = LogOnErrorSelectingData(Method, ErrorMsg, QueryText);
						return Result;
					}
					else if (dataSet.Tables.Count == 0) {
						ErrorMsg = "No tables in the dataset to process";
						Result = LogOnErrorSelectingData(Method, ErrorMsg, QueryText);
						return Result;
					}
					
					// We now have the query to call for the Portfolio Search Results
					Result = BuildResltFromDataSet(dataSet);
				}
				//SELECT * FROM ea_LedgerList("")
			}
			catch (Exception ex)
			{
				Result = LogOnException(Method, ex, "");
			}
			finally
			{
				LogOnLeavingMethod(Method);
			}
			return Result;
		}
		
		private string BuildResltFromDataSet(DataSet Source)
		{
			string Result = "";
			const string Method = "BuildResltFromDataSet";
			
			LogOnInsideMethod(Method);
			try 
			{
				DataTable tbl = Source.Tables[0];
				string Str = "";
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
	            
	            sb.Append(",");
	            
	            sb.Append(CommonUtilities.JsonUtilities.StartJsonArray("rows"));
				RowList.Clear();
				for (int RowIndex = 0; RowIndex < RowCount; RowIndex++)
				{
					CurrentRow = tbl.Rows[RowIndex];
					StrList.Clear();
	            	for (int Index = 0; Index < ColCount; Index++)
	            	{
	            		ColValue = ((CurrentRow[Index] == DBNull.Value) ? "" : Convert.ToString(CurrentRow[Index]));
	            		ColValue = CommonUtilities.JsonUtilities.CleanForJSON(ColValue);	            		
	            		ColValue = string.Format(QuotedStr, ColValue);
	            		StrList.Add(ColValue);
	            	}
	            	Str =
	            		CommonUtilities.JsonUtilities.StartJsonArray("") +
	            		string.Join(",", StrList.ToArray()) +
	            		CommonUtilities.JsonUtilities.EndJsonArray();
	            	RowList.Add(Str);
	            	
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
		public static string cleanForJSON(string s)
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
