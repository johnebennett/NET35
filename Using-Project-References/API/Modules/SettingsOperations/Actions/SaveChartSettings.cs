/*
 * Created by SharpDevelop.
 * User: John
 * Date: 17/09/2019
 * Time: 6:06 PM
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
using Actions.SettingsOperations;

namespace Actions.SettingsOperations
{
	/// <summary>
	/// Description of SaveChartSettings.
	/// </summary>
	public class SaveChartSettings : ActionBase
	{
		private string FModuleName = "SettingsOperations";
		
		public static string Execute(int MasterId,
		                             RequestCommonItem CommonItem,
		                             JObject DataObject,
		                             APISettingsItem SettingsItem,
		                             out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			SaveChartSettings inst = new SaveChartSettings();
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
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " +Method, FModuleName, MasterId);
			LogOnInsideMethod(Method);
			try 
			{
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
		
		private string Validate()
		{
			string Method = "Validate";
			string Result = "";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " +Method, FModuleName, MasterId);
			LogOnInsideMethod(Method);
			try 
			{
				if (string.IsNullOrEmpty(Result))
				{
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
		
		private string Output()
		{
			string Method = "Output";
			string Result = "";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " +Method, FModuleName, MasterId);
			LogOnInsideMethod(Method);
			try 
			{
				List<string> DisplayList = GetDistinctDisplayNameList(out Result);
				if (string.IsNullOrEmpty(Result))
				{
					Result = ProcessDisplayList(DisplayList);
				}
				
				if (!string.IsNullOrEmpty(Result))
				{
					Result = LogOnProcessingError(Method, Result, "");
				}
				else
				{
					Result = LogResponse(Method, "");
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
			WindowsEventLoggingUtilities.WriteToInfoEvent("Leaving: " +Method, FModuleName, MasterId);
			return Result;
		}
		
		private List<string> GetDistinctDisplayNameList(out string ErrorMsg)
		{
			List<string> Result = new List<string>();
			ErrorMsg = "";
			string QueryText = "SELECT DISTINCT DisplayName from EASettingsChartEntries(@CompanyId)";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("CompanyId", CommonItem.CompanyIdValue));
			ErrorMsg = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
			DataTable tbl = null;
			if (string.IsNullOrEmpty(ErrorMsg))
			{
				tbl = 
					UsefulDBMethods.Helpers.GetFirstDataTableEx(
						FbConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out ErrorMsg);
			}
			
			if (string.IsNullOrEmpty(ErrorMsg))
			{
				string Value = "";
				foreach (DataRow Row in tbl.Rows)
				{
					Value = (Row[0] == DBNull.Value) ? "" : Convert.ToString(Row[0]);
					if (!string.IsNullOrEmpty(Value))
					{
						Value = Value.Replace("_", " ");
						Result.Add(Value);
					}
				}
			}
			
			return Result;
		}
		
		private string ProcessDisplayList(List<string> DisplayList)
		{
			string Result = "";
			string QueryText = "";
			string DisplayItem = "";
			int ChartId = 0;
			string StrValue = "";
			
			for (int Index = 0; Index < DisplayList.Count && string.IsNullOrEmpty(Result); Index++)
			{
				DisplayItem = DisplayList[Index];
				WindowsEventLoggingUtilities.WriteToInfoEvent("Processing: " + DisplayItem, FModuleName, MasterId);
				StrValue = (string)DataObject[DisplayItem.Replace(" ", "_")];
				ChartId = StringUtilities.StrToIntDef(StrValue, -1);
				if (ChartId < 0)
				{
					Result = "Cannot determine the ChartId for the: " + DisplayItem + " item.";
				}
				WindowsEventLoggingUtilities.WriteToInfoEvent("Processing ChartId: " + ChartId.ToString(), FModuleName, MasterId);
				
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = GetUpdateStatement(DisplayItem.Replace("_", " "), ChartId, out Result);
				}
				
				if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText))
					Result = "Cannot determine the update command for the item: " + DisplayItem;
				
				if (string.IsNullOrEmpty(Result))
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent("Executing:" + Environment.NewLine + QueryText, FModuleName, MasterId);
					Result = FirebirdUtilities.DBExecute(FbConnectionString, QueryText);
				}
			}
			return Result;
		}
		
		private string GetUpdateStatement(string DisplayName, int ChartId, out string ErrorMsg)
		{
			string Result = "";
			ErrorMsg = "";
			string QueryText = 
				" SELECT First 1 SQLToUpdate FROM EASettingsChartEntries(@CompanyId)" +
				" WHERE DisplayName = @DisplayName AND ChartId = @ChartId";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("CompanyId", CommonItem.CompanyIdValue));
			Params.Add(new FbParameterItem("DisplayName", DisplayName));
			Params.Add(new FbParameterItem("ChartId", ChartId));
			ErrorMsg = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
			DataTable tbl = null;
			if (string.IsNullOrEmpty(ErrorMsg))
			{
				tbl =
					UsefulDBMethods.Helpers.GetFirstDataTableEx(
						FbConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTablesAndRows, out ErrorMsg);
			}
			
			if (string.IsNullOrEmpty(ErrorMsg))
			{
				// To get to this point there must be at least ( and realistically only 1) Row and Column
				Object Obj = tbl.Rows[0][0];
				Result = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
			}
			return Result;
		}
	}
}
