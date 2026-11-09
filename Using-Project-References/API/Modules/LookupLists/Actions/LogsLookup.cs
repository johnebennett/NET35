/*
 * Created by SharpDevelop.
 * User: John
 * Date: 25/01/2019
 * Time: 10:48 AM
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
	//public enum LogsLookupTypeId { RunId, Unknown };
	
	/// <summary>
	/// Description of LogsLookup.
	/// </summary>
	public class LogsLookup : ActionBase
	{
		public LogsLookup()
		{
		}

		private string FModuleName = "LogsLookup";
		
		/*
		private int TypeIdValue = 0;
		private LogsLookupTypeId TypeId = LogsLookupTypeId.Unknown;
		*/
		private int RunId = 0;
		private int TransactionId = 0;
		private int ClientId = 0;
		private int SummaryId = 0;
		private int AddedById = 0;
		
		public static string Execute(int MasterId,
			RequestCommonItem CommonItem,
			JObject DataObject,
			APISettingsItem SettingsItem,
			out string ErrorMsg)
		{
			string Method = "LogsLookup - Execute";
			string Result = "";
			ErrorMsg = "";

			LogsLookup inst = new LogsLookup();
			if (!inst.Setup(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg))
				return "";
			
			inst.LogOnInsideMethod(Method);
			try 
			{
				Result = inst.Init();				
				if (!string.IsNullOrEmpty(Result))
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent(Result, Method, MasterId);
					return "";
				}
				
				Result = inst.Validate();
				if (!string.IsNullOrEmpty(Result))
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent(Result, Method, MasterId);
					return "";
				}

				Result = inst.Output();
			} 
			finally 
			{
				inst.LogOnLeavingMethod(Method);
			}
			
			return Result;
		}		
		
		private string Init()
		{
			string Method = "Init";
			string Result = "";
			
			LogOnInsideMethod(Method);
			try 
			{
				/*
				TypeIdValue = (int)DataObject["typeId"];
				switch (TypeIdValue)
				{
					case 00: TypeId = LogsLookupTypeId.RunId; break;
					default: TypeId = LogsLookupTypeId.Unknown; break;
				}
				*/
				/*
				RunId = (int)DataObject["runId"];
				TransactionId = (int)DataObject["transId"];
				ClientId = (int)DataObject["clientId"];
				SummaryId = (int)DataObject["summaryId"];
				AddedById = (int)DataObject["addedById"];
				*/
				RunId = StringUtilities.StrToIntDef((string)DataObject["runId"], 0);
				TransactionId = StringUtilities.StrToIntDef((string)DataObject["transId"], 0);
				ClientId = StringUtilities.StrToIntDef((string)DataObject["clientId"], 0);
				SummaryId = StringUtilities.StrToIntDef((string)DataObject["summaryId"], 0);
				AddedById = StringUtilities.StrToIntDef((string)DataObject["addedById"], 0);
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
		
		private string Validate()
		{
			string Method = "Validate";
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
		
		private string Output()
		{
			string Method = "Output";
			string Result = "";
			string ErrorMsg = "";
			int tries = 60;
			
			LogOnInsideMethod(Method);
			try
			{
				string ConnectionString = CommonDBMethods.Common.ConnectionString(APISettings);
				if (!string.IsNullOrEmpty(ConnectionString)) 
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent("ConnectionString: " + ConnectionString, FModuleName, MasterId);
					string QueryText = "SELECT * FROM EventRunDisplayActions({0}, {1}, {2}, {3}, {4}, {5})";
					QueryText = 
						string.Format(
							QueryText, CommonItem.CompanyIdValue, RunId, TransactionId, ClientId, SummaryId, AddedById);
					WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText: " + Environment.NewLine + QueryText, FModuleName, MasterId);
					
					DataTable tbl = null;
					for (int Index = 1; Index <= tries; Index++)
					{					
						WindowsEventLoggingUtilities.WriteToInfoEvent("Attempt: " + Convert.ToString(Index), "EventRunDisplayActions", 0);
						tbl = FirebirdUtilities.GetFirstDataTable(FbConnectionString, QueryText);
						if (tbl == null) {
							ErrorMsg = 
								"Error Executing Query" + Environment.NewLine +
								ErrorMsg + Environment.NewLine +
								"QueryText:" + Environment.NewLine + QueryText;
							WindowsEventLoggingUtilities.WriteToInfoEvent(ErrorMsg, "EAOfficeAPI", MasterId);
							Result = LogOnErrorSelectingData(Method, ErrorMsg, QueryText);
							return Result;
						}
						else if (tbl.Rows.Count == 0) {
							//Index++;
							System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
							if (Index == tries)
							{
								ErrorMsg = "No rows returned in the table to process";
								Result = LogOnErrorSelectingData(Method, ErrorMsg, QueryText);
								return Result;
							}
						}
						else Index = tries + 1;
					}
					
					// We now have the query to call for the Portfolio Search Results
					Result = BuildResultFromTable(tbl);
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
		
		private string BuildResultFromTable(DataTable Source)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(CommonUtilities.JsonUtilities.StartJson());
				
	        sb.Append(CommonUtilities.JsonUtilities.StartJsonArray("fieldNames"));
	        List<string> StrList = new List<string>();
			foreach (DataColumn column in Source.Columns)
			{
				StrList.Add(string.Format("\"{0}\"", column.ColumnName));
			}
			sb.Append(string.Join(",", StrList.ToArray()));
            sb.Append(CommonUtilities.JsonUtilities.EndJsonArray());
	            
            // Row entries
            sb.Append(",");
	            
            sb.Append(CommonUtilities.JsonUtilities.StartJsonArray("rows"));
			StrList.Clear();
			string FullColName = "";
			string ColName = "";
			string ColValue = "";
			string Str = "";
	        List<string> RowList = new List<string>();
			foreach(DataRow ARow in Source.Rows)
			//for (int RowIndex = 0; RowIndex < RowCount; RowIndex++)
			{
				StrList.Clear();
            	//for (int ColIndex = 0; ColIndex < tbl.Columns.Count; ColIndex++)
            	foreach (DataColumn column in Source.Columns)
	            {
            		FullColName = column.ColumnName;
            		ColName = FullColName;
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
            		      
            		ColValue = ((ARow[FullColName] == DBNull.Value) ? "" : Convert.ToString(ARow[FullColName]));
            		StrList.Add(CommonUtilities.JsonUtilities.AddPropertyNameAndValue(ColName, ColValue, false));
	            	}
	            	Str =
	            		CommonUtilities.JsonUtilities.StartJsonObject("") +
	            		string.Join(",", StrList.ToArray()) +
	            		CommonUtilities.JsonUtilities.EndJsonObject();
	            	RowList.Add(Str);	            	
				}
			sb.Append(string.Join(",", RowList.ToArray()));
	        sb.AppendLine(CommonUtilities.JsonUtilities.EndJsonArray());
				
			sb.Append(CommonUtilities.JsonUtilities.EndJson());
			return sb.ToString();
		}
	}
}
