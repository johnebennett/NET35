/*
 * Created by SharpDevelop.
 * User: John
 * Date: 09/01/2019
 * Time: 11:11 AM
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
	public class GetFilterListForMgmtTypeItem
	{
		private string FClassName = "";
		
		public string ManagementTypeId { get; set; }
		
		private ActionParameterStatus ManagementTypeIdStatus = ActionParameterStatus.OK;
		
		public GetFilterListForMgmtTypeItem()
		{
			this.FClassName = this.GetType().ToString();
			this.ManagementTypeId = "";
			this.ManagementTypeIdStatus = ActionParameterStatus.OK;
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
				int DummyInt =  0;
				
				try 
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent("Building ManagementTypeId", FClassName, 0);
					Key = "mgmtTypeId";
					obj = DataObject[Key];
					ManagementTypeId = "";
					ManagementTypeIdStatus = ActionParameterStatus.OK;
					if (obj == null)
					{
						WindowsEventLoggingUtilities.WriteToInfoEvent("ManagementTypeId IS NULL", FClassName, 0);
						ManagementTypeIdStatus = ActionParameterStatus.NotProvided;
					}
					else 
					{
						ManagementTypeId = (string)DataObject[Key];
						WindowsEventLoggingUtilities.WriteToInfoEvent("ManagementTypeId Value: " + ManagementTypeId, FClassName, 0);
						if (CommonUtilities.StringUtilities.SameText(ManagementTypeId, "") ||
						    !int.TryParse(ManagementTypeId,  out DummyInt))
						{
							ManagementTypeIdStatus = ActionParameterStatus.Invalid;
						}
					}
					WindowsEventLoggingUtilities.WriteToInfoEvent("Finished Building ManagementTypeId", FClassName, 0);
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
				Key = "ManagementTypeIdStatus";
				if (ManagementTypeIdStatus == ActionParameterStatus.NotProvided)
				{
					ErrorMsg = string.Format(BaseFormat, Key, NotProvided);
					return;
				}
				if (ManagementTypeIdStatus == ActionParameterStatus.Invalid)
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
	public class GetFilterListForMgmtType : ActionBase
	{
		private string FModuleName = "ComboBoxEntries";
		private GetFilterListForMgmtTypeItem WorkItem = new GetFilterListForMgmtTypeItem();
		
		public static string Execute(int MasterId,
		                             RequestCommonItem CommonItem,
		                             JObject DataObject,
		                             APISettingsItem SettingsItem,
		                             out string ErrorMsg)
		{
			string Method = "DefaultMethods - Execute";
			string Result = "";
			ErrorMsg = "";

			GetFilterListForMgmtType inst = new GetFilterListForMgmtType();
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
				string QueryText = "SELECT * FROM EAGetFilterListByTypeId({0}, {1})";
				QueryText = 
					string.Format(
						QueryText,
						StringUtilities.StrToIntDef(CommonItem.CompanyId, 0),
						StringUtilities.StrToIntDef(WorkItem.ManagementTypeId, 0));
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
				Str =
					CommonUtilities.JsonUtilities.StartJsonObject("") +
					CommonUtilities.JsonUtilities.AddPropertyNameAndValue("Name", "Select a Filter", true) +
					CommonUtilities.JsonUtilities.AddPropertyNameAndValue("Value", "0") +
					CommonUtilities.JsonUtilities.EndJsonObject();
				RowList.Add(Str);
				for (int RowIndex = 0; RowIndex < RowCount; RowIndex++)
				{
					CurrentRow = Source.Rows[RowIndex];
					ColName = "FilterName";
					Obj = CurrentRow[ColName];
					FilterName = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					ColName = "FilterId";
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
