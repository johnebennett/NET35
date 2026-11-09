/*
 * Created by SharpDevelop.
 * User: John
 * Date: 14/01/2019
 * Time: 10:48 AM
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

namespace Actions.Portfolio
{
	/// <summary>
	/// In the browser, when the portfolio type dropdown is changed
	/// we need to get updated filter and report entries for the
	/// respective dropdown boxes
	/// </summary>
	public class RefreshPortfolioTypeEntries : ActionBase
	{
		private string FModuleName = "Portfolio";
		
		private int PortfolioId = 0;
		private ActionParameterStatus PortfolioIdStatus = ActionParameterStatus.OK;

		public static string Execute(int MasterId,
		                             RequestCommonItem CommonItem,
		                             JObject DataObject,
		                             APISettingsItem SettingsItem,
		                             out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			RefreshPortfolioTypeEntries inst = new RefreshPortfolioTypeEntries();
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
				PortfolioId = StringUtilities.StrToIntDef((string)DataObject["portfolioType"], -1);
				if (PortfolioId < 0)
					PortfolioIdStatus = ActionParameterStatus.Invalid;
				else
					PortfolioIdStatus = ActionParameterStatus.OK;
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
				string BaseFormat = "{0} is {1}";
				string NotProvided = "Not Provided";
				string Invalid = "Invalid";				
				string Key = "";
				
				Key = "PortfolioIdStatus";				
				if (PortfolioIdStatus == ActionParameterStatus.NotProvided)
				{
					Result = string.Format(BaseFormat, Key, NotProvided);
				}
				
				if ((string.IsNullOrEmpty(Result)) && (PortfolioIdStatus == ActionParameterStatus.Invalid))
				{
					Result = string.Format(BaseFormat, Key, Invalid);
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
			string ErrorMsg = "";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " +Method, FModuleName, MasterId);
			LogOnInsideMethod(Method);
			try 
			{
				//Result = "{\"message\": \"hello\"}";
				Result = BuildOutput(out ErrorMsg);
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
		
		private string BuildOutput(out string ErrorMsg)
		{
			ErrorMsg = "";
			string Result = "";
			string Method = "BuildOutput";
			
			LogOnInsideMethod(Method);
			try
			{
				WindowsEventLoggingUtilities.WriteToInfoEvent("Inside BuildOutput", FModuleName, MasterId);
				
				StringBuilder entries = new StringBuilder();
				entries.AppendLine(JsonUtilities.StartJson());
				
				/*
				ErrorMsg =
					CommonMethods.BuildFilterComboBoxEntries(
						FbConnectionString, CommonItem.CompanyIdValue, MasterId, PortfolioId, ref entries);
				if (!string.IsNullOrEmpty(ErrorMsg)) return "";				
				
				entries.Append(",");
				ErrorMsg = 
					CommonMethods.BuildListReportEntries(
						FbConnectionString, CommonItem.CompanyIdValue, MasterId, PortfolioId, ref entries);
				if (!string.IsNullOrEmpty(ErrorMsg)) return "";				
				*/
				
				ErrorMsg = BuildOutputEntries(ref entries);				

					
				entries.AppendLine(JsonUtilities.EndJson());
				Result = entries.ToString();
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
		
		private string BuildOutputEntries(ref StringBuilder Entries)
		{
			string Method = "BuildOutputEntries";
			string Result = "";			
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, FModuleName, MasterId);
			
			LogOnInsideMethod(Method);
			try
			{
				string QueryText = "SELECT * FROM Portfolio_Select_OnChange_Items({0}, {1}, {2})";
				QueryText = string.Format(QueryText, CommonItem.CompanyIdValue, CommonItem.ClientIdValue, PortfolioId);
				WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText:" + Environment.NewLine + QueryText, FModuleName, MasterId);
				DataTable dt = null;
				try 
				{
					dt = FirebirdUtilities.GetFirstDataTable(FbConnectionString, QueryText);					
					if (dt == null)
					{
						Result = "No tables in the dataset to process";
						return Result;
					}
				}					
				catch (Exception Ex)
				{
					Result = LogOnException(Method, Ex, "");
					return Result;
				}
				
				foreach (DataRow dataRow in dt.Rows)
				{
					Entries.AppendLine((dataRow[0] == DBNull.Value) ? "" : Convert.ToString(dataRow[0]));
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
		private string BuildCommonComboBoxEntries(ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "BuildCommonComboBoxEntries";
			
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside BuildCommonComboBoxEntries", FModuleName, MasterId);
			int[] ComboBoxIds = new int[] {1, 2, 3, 5}; // offices, managers, portfolioTypes, businessActiveTypes
			string QueryTextBase = "SELECT * FROM CommonComboEntriesOnly({0}, {1}, {2}, 0)";
			string QueryText = "";
			DataSet ds = null;
				
			foreach(int ComboBoxId in ComboBoxIds)
			{
				QueryText =
					string.Format(QueryTextBase, CommonItem.CompanyIdValue, CommonItem.ClientIdValue, ComboBoxId);
				WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText:" + Environment.NewLine + QueryText, FModuleName, MasterId);
				ds = FirebirdUtilities.GetDataSetFromQuery(this.FbConnectionString, QueryText, out Result);
				if (!string.IsNullOrEmpty(Result))
				{
						Result = 
							"Error Executing Query" + Environment.NewLine +
							Result + Environment.NewLine +
							"QueryText:" + Environment.NewLine + QueryText;
						WindowsEventLoggingUtilities.WriteToInfoEvent(Result, FModuleName, MasterId);
						LogOnErrorSelectingData(Method, Result, QueryText);
						return Result;
				}
				Result = BuildCommonComboBoxEntryItem(ds, ref Entries);
				if (!string.IsNullOrEmpty(Result)) return Result;
			}
			//Entries.AppendLine(JsonUtilities.EndJson());

			return Result;
		}
		
		private string BuildCommonComboBoxEntryItem(DataSet Source,
		                                            //bool IsLastEntry,
		                                            ref StringBuilder Entries)
		{
			string Result = "";
			
			try
			{
				if (Source.Tables.Count == 0) return "No tables in the dataset to process";			
				
				foreach (DataRow dataRow in Source.Tables[0].Rows)
				{
					Entries.AppendLine((dataRow[0] == DBNull.Value) ? "" : Convert.ToString(dataRow[0]));
				}
				//if (!IsLastEntry) 
				Entries.Append(",");
			}
			finally
			{
				//Entries.AppendLine(JsonUtilities.EndJsonArray());
			}

			return Result;
		}
		
		private string BuildFilterComboBoxEntries(ref StringBuilder Entries, 
		                                          int DefaultPortfolioId)
		{
			string Result = "";
			string Method = "BuildFilterComboBoxEntries";
			
			LogOnInsideMethod(Method);
			try
			{
				WindowsEventLoggingUtilities.WriteToInfoEvent("Inside BuildOutput", FModuleName, MasterId);
				WindowsEventLoggingUtilities.WriteToInfoEvent("DefaultPortfolioId: " + DefaultPortfolioId, FModuleName, MasterId);
				string QueryText = "SELECT * FROM EAGetFilterListByTypeId({0}, {1})";
				QueryText = string.Format(QueryText, CommonItem.CompanyIdValue, DefaultPortfolioId);
				DataSet ds = FirebirdUtilities.GetDataSetFromQuery(this.FbConnectionString, QueryText, out Result);
				if (!string.IsNullOrEmpty(Result))
				{
						Result = 
							"Error Executing Query" + Environment.NewLine +
							Result + Environment.NewLine +
							"QueryText:" + Environment.NewLine + QueryText;
						WindowsEventLoggingUtilities.WriteToInfoEvent(Result, FModuleName, MasterId);
						Result = LogOnErrorSelectingData(Method, Result, QueryText);
						return Result;
				}
				else if (ds.Tables.Count == 0) 
				{
					Result = "No tables in the dataset to process";
					Result = LogOnErrorSelectingData(Method, Result, QueryText);
					return Result;
				}
				else
				{
					Result = BuildFilterComboBoxEntryItems("filters1", ds.Tables[0], ref Entries);
					if (!string.IsNullOrEmpty(Result)) return Result;
					
					Entries.Append(",");
					Result = BuildFilterComboBoxEntryItems("filters2", ds.Tables[0], ref Entries);
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
		
		private string BuildFilterComboBoxEntryItems(string ArrayName,
		                                             DataTable Source,
		                                             ref StringBuilder Entries)
		{
			string Result = "";
			const string Method = "BuildFilterComboBoxEntryItems";
			
			LogOnInsideMethod(Method);
			try 
			{
				List<string> RowList = new List<string>();
				object Obj = null;
				string FilterName = "";
				string FilterId = "";
				string Str = "";

	            Entries.AppendLine(CommonUtilities.JsonUtilities.StartJsonArray(ArrayName));

				// Row entries
				RowList.Clear();
				Str =
					CommonUtilities.JsonUtilities.StartJsonObject("") +
					CommonUtilities.JsonUtilities.AddPropertyNameAndValue("Name", "Select a Filter", true) +
					CommonUtilities.JsonUtilities.AddPropertyNameAndValue("Value", "0", false) +
					CommonUtilities.JsonUtilities.EndJsonObject();
				RowList.Add(Str);
				foreach(DataRow ARow in Source.Rows)
				{
					Obj = ARow["FilterName"];
					FilterName = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					Obj = ARow["FilterId"];
					FilterId = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					Str =
						CommonUtilities.JsonUtilities.StartJsonObject("") +
						CommonUtilities.JsonUtilities.AddPropertyNameAndValue("Name", FilterName, true) +
						CommonUtilities.JsonUtilities.AddPropertyNameAndValue("Value", FilterId, false) +
						CommonUtilities.JsonUtilities.EndJsonObject();
					RowList.Add(Str);
				}
				Entries.Append(string.Join(",", RowList.ToArray()));
	            Entries.AppendLine(CommonUtilities.JsonUtilities.EndJsonArray());
				
				//Result = sb.ToString();
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
		
		private string BuildListReportEntries(ref StringBuilder Entries, 
		                                      int DefaultPortfolioId)
		{
			string Result = "";
			string Method = "BuildListReportEntries";
			
			LogOnInsideMethod(Method);
			try
			{
				WindowsEventLoggingUtilities.WriteToInfoEvent("Inside BuildOutput", FModuleName, MasterId);
				string QueryText = "SELECT ReportName, ReportId FROM ListReports({0}, '{1}')";
				QueryText = string.Format(QueryText, CommonItem.CompanyIdValue, "Portfolio" + DefaultPortfolioId);
				WindowsEventLoggingUtilities.WriteToInfoEvent("Query" + Environment.NewLine + QueryText, FModuleName, MasterId);
				DataSet ds = FirebirdUtilities.GetDataSetFromQuery(this.FbConnectionString, QueryText, out Result);
				if (!string.IsNullOrEmpty(Result))
				{
						Result = 
							"Error Executing Query" + Environment.NewLine +
							Result + Environment.NewLine +
							"QueryText:" + Environment.NewLine + QueryText;
						WindowsEventLoggingUtilities.WriteToInfoEvent(Result, FModuleName, MasterId);
						Result = LogOnErrorSelectingData(Method, Result, QueryText);
				}
				else if (ds.Tables.Count == 0) 
				{
					Result = "No tables in the dataset to process";
					Result = LogOnErrorSelectingData(Method, Result, QueryText);
				}				
				else Result = BuildListReportEntryItems(ref Entries, ds.Tables[0]);
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
		
		private string BuildListReportEntryItems(ref StringBuilder Entries,
		                                         System.Data.DataTable Source)
		{
			string Result = "";
			const string Method = "BuildListReportEntryItems";
			
			LogOnInsideMethod(Method);
			try 
			{
				List<string> RowList = new List<string>();
				object Obj = null;
				string ReportName = "";
				string ReportId = "";
				string Str = "";

	            Entries.Append(CommonUtilities.JsonUtilities.StartJsonArray("reports"));

				// Row entries
				RowList.Clear();
				foreach (DataRow Row in Source.Rows)
				{
					Obj = Row["ReportName"];
					ReportName = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					Obj = Row["ReportId"];
					ReportId = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					Str =
						CommonUtilities.JsonUtilities.StartJsonObject("") +
						CommonUtilities.JsonUtilities.AddPropertyNameAndValue("Name", ReportName, true) +
						CommonUtilities.JsonUtilities.AddPropertyNameAndValue("Value", ReportId, false) +
						CommonUtilities.JsonUtilities.EndJsonObject();
					RowList.Add(Str);
				}
				Entries.Append(string.Join(",", RowList.ToArray()));
	            Entries.AppendLine(CommonUtilities.JsonUtilities.EndJsonArray());
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
		*/
	}
}
