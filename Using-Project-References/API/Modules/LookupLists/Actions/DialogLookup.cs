/*
 * Created by SharpDevelop.
 * User: John
 * Date: 21/06/2019
 * Time: 8:57 PM
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
	/// Description of DialogLookup.
	/// </summary>
	public class DialogLookup : ActionBase
	{
		private string FModuleName = "LedgerLookup";
		
		private string FSearchText = "";
		private string FTableToSearch = "";
		private string FIncludeInactive = "";
		private int FTypeId = 0;
		private string FTypeIdValue = "";
		
		public static string Execute(int MasterId,
			RequestCommonItem CommonItem,
			JObject DataObject,
			APISettingsItem SettingsItem,
			out string ErrorMsg)
		{
			string Method = "LedgerLookup - Execute";
			string Result = "";
			ErrorMsg = "";

			DialogLookup inst = new DialogLookup();
			if (!inst.Setup(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg))
				return "";
			
			inst.LogOnInsideMethod(Method);
			try 
			{
				Result = inst.Init();
				if (!string.IsNullOrEmpty(Result))
					return "";
				
				Result = inst.Validate();
				if (!string.IsNullOrEmpty(Result))
					return Result;
				
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
				FSearchText = (string)DataObject["searchText"];
				FTableToSearch = (string)DataObject["tableToSearch"];
				FIncludeInactive = (string)DataObject["includeInactive"];
				FTypeIdValue = (string)DataObject["typeId"];
				// The TypeId can be Zero
				FTypeId = StringUtilities.StrToIntDef(FTypeIdValue, 0);
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
				string BaseFormat = "{0} is {1}";
				string NotProvided = "Not Provided";
				string Invalid = "Invalid";				
				string Key = "";
				
				if (string.IsNullOrEmpty(Result))
				{
					Key = "TableToSearch";
					if (string.IsNullOrEmpty(FTableToSearch)) Result = string.Format(BaseFormat, Key, NotProvided + " or " + Invalid);
				}
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
			
			LogOnInsideMethod(Method);
			try
			{
				//string ConnectionString = CommonDBMethods.Common.ConnectionString(APISettings);
				if (!string.IsNullOrEmpty(FbConnectionString)) 
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent("ConnectionString: " + FbConnectionString, FModuleName, MasterId);
					string QueryText = 
						"SELECT SqlStatement FROM EAFindScreenSql(@CompanyClientId, @TypeId, @TableToSearch, @WhatToFind, @IncludeInactive)";
					List<FbParameterItem> Params = new List<FbParameterItem>();
					Params.Add(new FbParameterItem("CompanyClientId", CommonItem.CompanyIdValue));
					Params.Add(new FbParameterItem("TypeId", FTypeId));
					Params.Add(new FbParameterItem("TableToSearch", FTableToSearch));
					Params.Add(new FbParameterItem("WhatToFind", FSearchText));
					Params.Add(new FbParameterItem("IncludeInactive", FIncludeInactive));
					WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText: " + Environment.NewLine + QueryText, FModuleName, MasterId);
					
					foreach (FbParameterItem Param in Params)
					{
						WindowsEventLoggingUtilities.WriteToInfoEvent(
							Param.ParameterName + ": " + Convert.ToString(Param.ParameterValue), FModuleName, MasterId);
					}
					
					DataTable dt = null;
					try 
					{
						dt = FirebirdUtilities.GetFirstDataTable(FbConnectionString, QueryText, Params, 0);
						if ((dt == null) || (dt.Rows.Count == 0))
						{
							Result = "No tables in the dataset to process";
							Result = LoggingEntry(Method, Result, "");
							return Result;
						}
					}					
					catch (Exception Ex)
					{
						Result = LogOnException(Method, Ex, "");
						return Result;
					}
					
					object Obj = dt.Rows[0]["SqlStatement"];
					string SqlStatement = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					if (string.IsNullOrEmpty(SqlStatement))
					{
						Result = "Empty SqlStatement returned for dialog.";
						Result = LoggingEntry(Method, Result, "");
						return Result;
					}
					
					Result = BuildOutput(SqlStatement);
				}
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
		
		private string BuildOutput(string SqlStatement)
		{
			string Result = "";
			const string Method = "BuildOutput";
			
			LogOnInsideMethod(Method);
			try 
			{
				DataTable tbl = FirebirdUtilities.GetFirstDataTable(FbConnectionString, SqlStatement);
				string Str = "";
				string ColName = "";
				string ColValue = "";
				List<string> RowList = new List<string>();
				List<string> StrList = new List<string>();
				object Obj = null;

	            StringBuilder sb = new StringBuilder();
				sb.Append(CommonUtilities.JsonUtilities.StartJson());
				
	            sb.Append(CommonUtilities.JsonUtilities.StartJsonArray("fieldNames"));
	            foreach (DataRow ThisRow in tbl.Rows)
	            {
	            	StrList.Clear();
					foreach (DataColumn ThisCol in tbl.Columns)
					{
						ColName = ThisCol.ColumnName;
						Obj = ThisRow[ColName];
						ColValue = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
						StrList.Add(JsonUtilities.AddPropertyNameAndValue(ColName, ColValue, false));
					}
					Str =
						JsonUtilities.StartJsonObject("") +
						string.Join(",", StrList.ToArray()) +
						JsonUtilities.EndJsonObject();
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
	}
}
