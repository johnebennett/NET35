/*
 * Created by SharpDevelop.
 * User: John
 * Date: 16/04/2020
 * Time: 5:39 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Collections.Specialized;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CommonUtilities;
using EAOfficeAPI;

namespace Actions.SettingsOperations
{
	/// <summary>
	/// Description of SystemSettings.
	/// </summary>
	public class SystemSettings : ActionBase
	{
		public SystemSettings()
		{
		}

        public static string GetModuleName() { return "SettingsOperations.SystemSettings."; }

        public static string LoadSettings(ActionBase ActionBase,
		                                  NameValueCollection DataCollection,
		                                  ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "LoadSettings";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " + Method, Method, ActionBase.MasterId);			
			
			ActionBase.LogOnInsideMethod(Method);
			try
			{
				string ConnectionString = ActionBase.FbConnectionString;
				string Value = CollectionUtilities.GetCollectionValue(DataCollection, "CategoryOnlyId", "");
				if (Value == "") Value = "0";
				int CategoryOnlyId = StringUtilities.StrToIntDef(Value, 0);
				Int64 CompanyId = ActionBase.CommonItem.CompanyIdValue;
				Result = LoadSettings(ConnectionString, CompanyId, CategoryOnlyId, ref Entries);
				/*
				Result = GatherSettings(ConnectionString, CompanyId, CategoryOnlyId, ref Entries);
				
				StringBuilder SystemEntries = new StringBuilder();
				Result = GatherSettings(ActionBase, DataCollection, ref SystemEntries);
				
				
				if (string.IsNullOrEmpty(Result))
				{
					if ((Entries.Length > 0) && (!StringUtilities.EndsWith(Entries, ","))) Entries.Append(",");
					Entries.Append(SystemEntries.ToString());
				}
				*/
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
		
		public static string LoadSettings(string ConnectionString,
		                                  Int64 CompanyId,
		                                  int CategoryOnlyId,
		                                  ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "LoadSettings";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
			ActionBase.LogOnInsideMethod(Method);
			try
			{
				StringBuilder SystemEntries = new StringBuilder();
				//Result = GatherSettings(ActionBase, DataCollection, ref SystemEntries);
				Result = GatherSettings(ConnectionString, CompanyId, CategoryOnlyId, ref SystemEntries);
				
				
				if (string.IsNullOrEmpty(Result))
				{
					if ((Entries.Length > 0) && (!StringUtilities.EndsWith(Entries, ","))) Entries.Append(",");
					Entries.Append(SystemEntries.ToString());
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
		
		public static string GatherSettings(string ConnectionString,
		                                    Int64 CompanyId,
		                                    int CategoryOnlyId,
		                                    ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "GatherSettings";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
			
			ActionBase.LogOnInsideMethod(Method);
			try
			{
				string QueryText = 
					"SELECT OutputLine FROM Init_Settings_SystemItems(@CompanyId, @CategoryOnlyId)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				Params.Add(new FbParameterItem("CategoryOnlyId", CategoryOnlyId));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, ActionBase.MasterId);
				
				DataTable tbl = null;
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					object Obj;
					foreach (DataRow Row in tbl.Rows)
					{
						Obj = Row["OUTPUTLINE"];
						Entries.AppendLine((Obj == DBNull.Value) ? "" : Convert.ToString(Obj));
					}
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					if (Entries.Length == 0) Result = "No content from SystemSettings.GatherSettings";
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
		
/*
		public static string GatherSettings(ActionBase ActionBase,
		                                    NameValueCollection DataCollection,
		                                    ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "GatherSettings";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " + Method, Method, ActionBase.MasterId);			
			
			ActionBase.LogOnInsideMethod(Method);
			try
			{
				string ConnectionString = ActionBase.FbConnectionString;
				string Value = CollectionUtilities.GetCollectionValue(DataCollection, "CategoryOnlyId", "");
				if (Value == "") Value = "0";
				int CategoryOnlyId = StringUtilities.StrToIntDef(Value, 0);
				Int64 CompanyId = ActionBase.CommonItem.CompanyIdValue;
				Result = GatherSettings(ConnectionString, CompanyId, CategoryOnlyId, ref Entries);
//				string QueryText = 
//					"SELECT OutputLine FROM Init_Settings_SystemItems(@CompanyId, @CategoryOnlyId)";
//				List<FbParameterItem> Params = new List<FbParameterItem>();
//				Params.Add(new FbParameterItem("CompanyId", ActionBase.CommonItem.CompanyIdValue));
//				string Value = CollectionUtilities.GetCollectionValue(DataCollection, "CategoryOnlyId", "");
//				if (Value == "") Value = "0";
//				Params.Add(new FbParameterItem("CategoryOnlyId", Value));
//				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, ActionBase.MasterId);
//				
//				DataTable tbl = null;
//				if (string.IsNullOrEmpty(Result))
//				{
//					tbl =
//						UsefulDBMethods.Helpers.GetFirstDataTableEx(
//							ActionBase.FbConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
//				}
//				
//				if (string.IsNullOrEmpty(Result))
//				{
//					object Obj;
//					foreach (DataRow Row in tbl.Rows)
//					{
//						Obj = Row["OUTPUTLINE"];
//						Entries.AppendLine((Obj == DBNull.Value) ? "" : Convert.ToString(Obj));
//					}
//				}
//				
//				if (string.IsNullOrEmpty(Result))
//				{
//					if (Entries.Length == 0) Result = "No content from SystemSettings.GatherSettings";
//				}
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
*/		
		
		#region OnChange
		public static string OnChange(ActionBase ActionBase,
		                              NameValueCollection DataCollection,
		                              ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "OnChange";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				bool KeyFound = false;
				//object Obj = ActionBase.GetCollectionValue("Data", "ChangeAction"); 
				//string ChangeAction = (Obj == null) ? "" : Convert.ToString(Obj);
				string ChangeAction = CollectionUtilities.GetCollectionValue(ActionBase.DataCollection, "ChangeAction", "", out KeyFound);
				if (string.IsNullOrEmpty(ChangeAction) && !KeyFound)
				{
					Result = "Invalid or Missing Action value";
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					switch (ChangeAction)
					{
						case "PrintGroups_Groups": 
							Result = OnChange_PrintGroups_Groups(ActionBase, DataCollection, ref Entries);
							break;
						default: Result = "Invalid or unknown Action value"; break;
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
			}
			
			return Result;
		}
		
		public static string OnChange_PrintGroups_Groups(ActionBase ActionBase, 
		                                                 NameValueCollection DataCollection, 
		                                                 ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "OnChange_PrintGroups_Groups";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				Result = LoadSettings(ActionBase, DataCollection, ref Entries);
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
		#endregion
		
		#region PerformCommand
		public static string PerformCommand(ActionBase ActionBase,
		                                    NameValueCollection DataCollection,
		                                    ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "PerformCommand";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				object Obj = ActionBase.GetCollectionValue("Data", "MethodName"); 
				string MethodName = CollectionUtilities.GetCollectionValue(DataCollection, "MethodName", "");
				Result = ValidationChecks.ErrorIfEmptyString(MethodName, "Method Name");
				
				if (string.IsNullOrEmpty(Result))
				{
					switch (MethodName)
					{
						case "PrintGroups_AddNewGroup": 
							Result = OnPerform_PrintGroups_AddNewGroup(ActionBase, DataCollection, ref Entries);
							break;
						case "PrintGroups_ChangeGroupName": 
							Result = OnPerform_PrintGroups_ChangeGroupName(ActionBase, DataCollection, ref Entries);
							break;
						case "PrintGroups_RemoveSelectedChartCode": 
							Result = OnPerform_PrintGroups_RemoveSelectedChartCode(ActionBase, DataCollection, ref Entries);
							break;
						case "PrintGroups_AddChartCodeToGroup": 
							Result = OnPerform_PrintGroups_AddChartCodeToGroup(ActionBase, DataCollection, ref Entries);
							break;
						case "PrintGroups_RemovePrintGroup": 
							Result = OnPerform_PrintGroups_PrintGroups_RemovePrintGroup(ActionBase, DataCollection, ref Entries);
							break;
						default: Result = "Invalid or unknown Action value"; break;
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
			}
			
			return Result;
		}
		
		public static string OnPerform_PrintGroups_AddNewGroup(ActionBase ActionBase,
		                                                       NameValueCollection DataCollection, 
		                                                       ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "OnPerform_PrintGroups_AddNewGroup";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string ConnectionString = ActionBase.FbConnectionString;
				Int64 CompanyId = ActionBase.CommonItem.CompanyIdValue;
				// When adding a new group the CategoryOnlyId can be zero
				int CategoryOnlyId = 0;				
				
				// When adding a new group the new group name must be provided
				string GroupName = CollectionUtilities.GetCollectionValue(DataCollection, "GroupName", "");
				Result = ValidationChecks.ErrorIfEmptyString(GroupName, "Group Name");
				
				int NewId = 0;
				if (string.IsNullOrEmpty(Result))
				{
					NewId = 
						UsefulDBMethods.GeneratorHelpers.GetNextGeneratorId(
							ActionBase.FbConnectionString, "I_EAPR_ID_GEN",
							ActionBase.CommonItem.CompanyIdValue, ActionBase.CommonItem.ClientIdValue, 
							out Result);
				}
				
				StringBuilder Settings = new StringBuilder();
				if (string.IsNullOrEmpty(Result))
				{
					//Result = LoadSettings(ActionBase, DataCollection, ref Settings);
					Result = LoadSettings(ConnectionString, CompanyId, CategoryOnlyId, ref Settings);
				}
				
				string QueryText = "";
				if (string.IsNullOrEmpty(Result))
				{
					string Path = "$.SystemSettings.PrintGroups.sqltocreategroup";
					QueryText = JsonUtilities.GetJsonPathValueStr(Settings.ToString(), Path, out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the sqltocreategroup entry";
				}
				
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					Params.Add(new FbParameterItem("GetId", NewId));
					Params.Add(new FbParameterItem("GroupName", GroupName));
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
					if (string.IsNullOrEmpty(Result))
					{
						Result = FirebirdUtilities.DBExecute(ActionBase.FbConnectionString, QueryText, Params, 0);
					}
					
					// if all is well here get the new settings result for the newly added group using
					// its new ID from above
					if (string.IsNullOrEmpty(Result))
					{
						Result = LoadSettings(ConnectionString, CompanyId, NewId, ref Entries);
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
			}
			
			return Result;
		}
		
		public static string OnPerform_PrintGroups_ChangeGroupName(ActionBase ActionBase,
		                                                           NameValueCollection DataCollection,
		                                                           ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "OnPerform_PrintGroups_ChangeGroupName";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string ConnectionString = ActionBase.FbConnectionString;
				Int64 CompanyId = ActionBase.CommonItem.CompanyIdValue;
				int CategoryOnlyId = 0;
				
				// When changing the group name a non empty value must be provided
				string GroupName = CollectionUtilities.GetCollectionValue(DataCollection, "GroupName", "");
				Result = ValidationChecks.ErrorIfEmptyString(GroupName, "Group Name");
				
				string Value = "";
				if (string.IsNullOrEmpty(Result))
				{
					Value = CollectionUtilities.GetCollectionValue(DataCollection, "CategoryOnlyId", "");
					Result = ValidationChecks.ErrorIfEmptyString(Value, "Category ID");
					if (string.IsNullOrEmpty(Result)) CategoryOnlyId = StringUtilities.StrToIntDef(Value, 0);
				}
				
				StringBuilder Settings = new StringBuilder();
				if (string.IsNullOrEmpty(Result))
				{
					Result = LoadSettings(ConnectionString, CompanyId, CategoryOnlyId, ref Settings);
				}
				
				string QueryText = "";
				if (string.IsNullOrEmpty(Result))
				{
					string Path = "$.SystemSettings.PrintGroups.sqltoupdateName";
					QueryText = JsonUtilities.GetJsonPathValueStr(Settings.ToString(), Path, out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the sqltoupdateName entry";
				}
				
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					Params.Add(new FbParameterItem("GroupName", GroupName));
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
					if (string.IsNullOrEmpty(Result))
					{
						Result = FirebirdUtilities.DBExecute(ActionBase.FbConnectionString, QueryText, Params, 0);
					}
					
					// if all is well here get the new settings result for the changed group name
					// by re-gathering using the same values above
					if (string.IsNullOrEmpty(Result))
					{
						Result = LoadSettings(ConnectionString, CompanyId, CategoryOnlyId, ref Entries);
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
			}
			
			return Result;
		}		
		
		public static string OnPerform_PrintGroups_RemoveSelectedChartCode(ActionBase ActionBase,
		                                                                   NameValueCollection DataCollection,
		                                                                   ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "OnPerform_PrintGroups_RemoveSelectedChartCode";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string ConnectionString = ActionBase.FbConnectionString;
				Int64 CompanyId = ActionBase.CommonItem.CompanyIdValue;
				int CategoryOnlyId = 0;
				
				string Value = "";
				if (string.IsNullOrEmpty(Result))
				{
					Value = CollectionUtilities.GetCollectionValue(DataCollection, "CategoryOnlyId", "");
					Result = ValidationChecks.ErrorIfEmptyString(Value, "Category ID");
					if (string.IsNullOrEmpty(Result)) CategoryOnlyId = StringUtilities.StrToIntDef(Value, 0);
				}
				
				int PassedId = 0;
				if (string.IsNullOrEmpty(Result))
				{
					Value = CollectionUtilities.GetCollectionValue(DataCollection, "PassedId", "");
					Result = ValidationChecks.ErrorIfEmptyString(Value, "Passed ID");
					if (string.IsNullOrEmpty(Result)) PassedId = StringUtilities.StrToIntDef(Value, 0);
				}
				
				StringBuilder Settings = new StringBuilder();
				if (string.IsNullOrEmpty(Result))
				{
					Result = LoadSettings(ConnectionString, CompanyId, CategoryOnlyId, ref Settings);
				}
				
				string QueryText = "";
				if (string.IsNullOrEmpty(Result))
				{
					string Path = "$.SystemSettings.PrintGroups.sqltoremove";
					QueryText = JsonUtilities.GetJsonPathValueStr(Settings.ToString(), Path, out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the sqltoremove entry";
				}
				
				/*
				string ChartId = "";
				if (string.IsNullOrEmpty(Result))
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent("3", Method, 0);
					string Path = "$.SystemSettings.PrintGroups.gridentries[?(@.Value == '" + PassedId + "')].ZC5_Chart_Code";
					WindowsEventLoggingUtilities.WriteToInfoEvent("Path: " + Path, Method, 0);
					ChartId = JsonUtilities.GetJsonPathValueStr(Settings.ToString(), Path, out Result);					
					WindowsEventLoggingUtilities.WriteToInfoEvent("ChartId: " + ChartId, Method, 0);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(ChartId)) Result = "Cannot find the chart code entry";
					WindowsEventLoggingUtilities.WriteToInfoEvent("4", Method, 0);
				}
				*/
				
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					Params.Add(new FbParameterItem("ChartId", PassedId));
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
					if (string.IsNullOrEmpty(Result))
					{
						Result = FirebirdUtilities.DBExecute(ActionBase.FbConnectionString, QueryText, Params, 0);
					}
					
					// if all is well here get the new settings result for above CategoryOnlyId
					if (string.IsNullOrEmpty(Result))
					{
						Result = LoadSettings(ConnectionString, CompanyId, CategoryOnlyId, ref Entries);
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
			}
			
			return Result;
		}
		
		public static string OnPerform_PrintGroups_AddChartCodeToGroup(ActionBase ActionBase,
		                                                               NameValueCollection DataCollection,
		                                                               ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "OnPerform_PrintGroups_AddChartCodeToGroup";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string ConnectionString = ActionBase.FbConnectionString;
				Int64 CompanyId = ActionBase.CommonItem.CompanyIdValue;
				int CategoryOnlyId = 0;
				
				string Value = "";
				if (string.IsNullOrEmpty(Result))
				{
					Value = CollectionUtilities.GetCollectionValue(DataCollection, "CategoryOnlyId", "");
					Result = ValidationChecks.ErrorIfEmptyString(Value, "Category ID");
					if (string.IsNullOrEmpty(Result)) CategoryOnlyId = StringUtilities.StrToIntDef(Value, 0);
				}
				
				int ChartId = 0;
				if (string.IsNullOrEmpty(Result))
				{
					Value = CollectionUtilities.GetCollectionValue(DataCollection, "ChartId", "");
					Result = ValidationChecks.ErrorIfEmptyString(Value, "Chart ID");
					if (string.IsNullOrEmpty(Result)) ChartId = StringUtilities.StrToIntDef(Value, 0);
				}
				
				StringBuilder Settings = new StringBuilder();
				if (string.IsNullOrEmpty(Result))
				{
					Result = LoadSettings(ConnectionString, CompanyId, CategoryOnlyId, ref Settings);
				}
				
				string QueryText = "";
				if (string.IsNullOrEmpty(Result))
				{
					string Path = "$.SystemSettings.PrintGroups.sqltoadd";
					QueryText = JsonUtilities.GetJsonPathValueStr(Settings.ToString(), Path, out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the sqltoadd entry";
				}
				
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					Params.Add(new FbParameterItem("ChartId", ChartId));
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
					if (string.IsNullOrEmpty(Result))
					{
						Result = FirebirdUtilities.DBExecute(ActionBase.FbConnectionString, QueryText, Params, 0);
					}
					
					// if all is well here get the new settings result for above CategoryOnlyId
					if (string.IsNullOrEmpty(Result))
					{
						Result = LoadSettings(ConnectionString, CompanyId, CategoryOnlyId, ref Entries);
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
			}
			
			return Result;
		}
		
		public static string OnPerform_PrintGroups_PrintGroups_RemovePrintGroup(ActionBase ActionBase, 
		                                                                        NameValueCollection DataCollection,
		                                                                        ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "OnPerform_PrintGroups_PrintGroups_RemovePrintGroup";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string ConnectionString = ActionBase.FbConnectionString;
				Int64 CompanyId = ActionBase.CommonItem.CompanyIdValue;
				int CategoryOnlyId = 0;
				
				string Value = "";
				if (string.IsNullOrEmpty(Result))
				{
					Value = CollectionUtilities.GetCollectionValue(DataCollection, "CategoryOnlyId", "");
					Result = ValidationChecks.ErrorIfEmptyString(Value, "Category ID");
					if (string.IsNullOrEmpty(Result)) CategoryOnlyId = StringUtilities.StrToIntDef(Value, 0);
				}
				
				StringBuilder Settings = new StringBuilder();
				if (string.IsNullOrEmpty(Result))
				{
					Result = LoadSettings(ConnectionString, CompanyId, CategoryOnlyId, ref Settings);
				}
				
				string QueryText = "";
				if (string.IsNullOrEmpty(Result))
				{
					string Path = "$.SystemSettings.PrintGroups.sqlprintgroupremove";
					QueryText = JsonUtilities.GetJsonPathValueStr(Settings.ToString(), Path, out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(QueryText)) Result = "Cannot find the sqlprintgroupremove entry";
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, null, Method, 0);
					if (string.IsNullOrEmpty(Result))
					{
						Result = FirebirdUtilities.DBExecute(ActionBase.FbConnectionString, QueryText);
					}
					
					// if all is well here get the new settings result using 0 for the CategoryOnlyId
					// since we just removed the one used in calling this process
					if (string.IsNullOrEmpty(Result))
					{
						CategoryOnlyId = 0;
						Result = LoadSettings(ConnectionString, CompanyId, CategoryOnlyId, ref Entries);
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
			}
			
			return Result;
		}		
		#endregion
	}
}
