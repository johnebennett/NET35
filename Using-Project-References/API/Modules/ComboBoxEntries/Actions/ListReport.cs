/*
 * Created by SharpDevelop.
 * User: John
 * Date: 10/01/2019
 * Time: 4:38 PM
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

namespace Actions.ComboBoxEntries
{
	/// <summary>
	/// ComboBoxEntriesItem represents the item being passed
	/// to the method for processing
	/// </summary>
	public class ListReportItem
	{
		private string FClassName = "";
		
		public string SourceName { get; set; }		
		private ActionParameterStatus SourceNameStatus = ActionParameterStatus.OK;
		
		public ListReportItem()
		{
			this.FClassName = this.GetType().ToString();
			this.SourceName = "";
			this.SourceNameStatus = ActionParameterStatus.OK;
		}
		
		public void LoadFromRequest(JObject DataObject, out string ErrorMsg)
		{
			string Method = "LoadFromRequest";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " + Method, FClassName, 0);
			ErrorMsg = "";
			
			try 
			{
				string Key = "";				
				Object obj = null;
				
				try 
				{
					Key = "source";
					obj = DataObject[Key];
					SourceName = "";
					SourceNameStatus = ActionParameterStatus.OK;
					if (obj == null)
					{
						SourceNameStatus = ActionParameterStatus.NotProvided;
					}
					else 
					{
						SourceName = (string)DataObject[Key];
						WindowsEventLoggingUtilities.WriteToInfoEvent("SourceName Value: " + SourceName, FClassName, 0);
					}
					WindowsEventLoggingUtilities.WriteToInfoEvent("Finished Building SourceName", FClassName, 0);
				}
				catch (Exception Ex)
				{
					ErrorMsg = Ex.Message;
				}				
			}
			catch (Exception Ex)
			{
				ErrorMsg = Ex.Message;
			}
		}
		
		public void Validate(out string ErrorMsg)
		{
			ErrorMsg = "";
			string Key = "";
			string BaseFormat = "{0} is {1}";
			string NotProvided = "Not Provided";
			string Invalid = "Invalid";
			
			try 
			{
				Key = "SourceNameStatus";
				if (SourceNameStatus == ActionParameterStatus.NotProvided)
				{
					ErrorMsg = string.Format(BaseFormat, Key, NotProvided);
					return;
				}
				if (SourceNameStatus == ActionParameterStatus.Invalid)
				{
					ErrorMsg = string.Format(BaseFormat, Key, Invalid);
					return;
				}
			}
			catch (Exception Ex)
			{
				ErrorMsg = Ex.Message;
			}
		}
	}
	
	/// <summary>
	/// Description of GetFilterListForMgmtType.
	/// </summary>
	public class ListReport : ActionBase
	{
		private string FModuleName = "ComboBoxEntries";
		private ListReportItem WorkItem = new ListReportItem();
		
		public static string Execute(int MasterId,
		                             RequestCommonItem CommonItem,
		                             JObject DataObject,
		                             APISettingsItem SettingsItem,
		                             out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			ListReport inst = new ListReport();
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
				WorkItem.LoadFromRequest(DataObject, out Result);
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
				WorkItem.Validate(out Result);
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
				string QueryText = "SELECT ReportName, ReportId FROM ListReports({0}, '{1}')";
				QueryText = 
					string.Format(QueryText, StringUtilities.StrToIntDef(CommonItem.CompanyId, 0), WorkItem.SourceName);
				WindowsEventLoggingUtilities.WriteToInfoEvent("Query" + Environment.NewLine + QueryText, FModuleName, MasterId);
				DataSet ds = FirebirdUtilities.GetDataSetFromQuery(this.FbConnectionString, QueryText, out ErrorMsg);
				if (!string.IsNullOrEmpty(ErrorMsg))
				{
						ErrorMsg = 
							"Error Executing Query" + Environment.NewLine +
							ErrorMsg + Environment.NewLine +
							"QueryText:" + Environment.NewLine + QueryText;
						WindowsEventLoggingUtilities.WriteToInfoEvent(ErrorMsg, FModuleName, MasterId);
						Result = LogOnErrorSelectingData(Method, ErrorMsg, QueryText);
				}
				else if (ds.Tables.Count == 0) 
				{
					ErrorMsg = "No tables in the dataset to process";
					Result = LogOnErrorSelectingData(Method, ErrorMsg, QueryText);
				}
				else Result = BuildResult(ds.Tables[0], out ErrorMsg);
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
		
		private string BuildResult(System.Data.DataTable Source,
		                           out string ErrorMsg)
		{
			ErrorMsg = "";
			string Result = "";
			const string Method = "BuildResult";
			
			LogOnInsideMethod(Method);
			try 
			{
				string ColName = "";
				int RowCount = Source.Rows.Count;
				DataRow CurrentRow = null;
				List<string> RowList = new List<string>();
				object Obj = null;
				string FilterName = "";
				string FilterId = "";
				string Str = "";

	            StringBuilder sb = new StringBuilder();
				sb.Append(CommonUtilities.JsonUtilities.StartJson());				
	            sb.Append(CommonUtilities.JsonUtilities.StartJsonArray("Items"));
				sb.Append(string.Join(",", RowList.ToArray()));

				// Row entries
				RowList.Clear();
				/*
				Str =
					CommonUtilities.JsonUtilities.StartJsonObject("") +
					CommonUtilities.JsonUtilities.AddPropertyNameAndValue("Name", "Select a Filter", true) +
					CommonUtilities.JsonUtilities.AddPropertyNameAndValue("Value", "0") +
					CommonUtilities.JsonUtilities.EndJsonObject();
				RowList.Add(Str);
				*/
				for (int RowIndex = 0; RowIndex < RowCount; RowIndex++)
				{
					CurrentRow = Source.Rows[RowIndex];
					ColName = "ReportName";
					Obj = CurrentRow[ColName];
					FilterName = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					ColName = "ReportId";
					Obj = CurrentRow[ColName];
					FilterId = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					Str =
						CommonUtilities.JsonUtilities.StartJsonObject("") +
						CommonUtilities.JsonUtilities.AddPropertyNameAndValue("Name", FilterName, true) +
						CommonUtilities.JsonUtilities.AddPropertyNameAndValue("Value", FilterId, false) +
						CommonUtilities.JsonUtilities.EndJsonObject();
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
	}
}
