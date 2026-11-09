/*
 * Created by SharpDevelop.
 * User: John
 * Date: 05/04/2020
 * Time: 2:35 PM
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
	/// Description of ChartOfAccountSettings.
	/// </summary>
	public class ChartOfAccountSettings : ActionBase
	{
		//private string FModuleName = "SettingsOperations";
		
		public ChartOfAccountSettings()
		{
		}
		
		#region LoadSettings		
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
				StringBuilder Settings = new StringBuilder();
				Result = GatherSettings(ActionBase, DataCollection, ref Settings);
				
				if (string.IsNullOrEmpty(Result))
				{
					string Str = JsonUtilities.StartJson() + Settings.ToString() + JsonUtilities.EndJson();
					WindowsEventLoggingUtilities.WriteToInfoEvent(
						"End of LoadSettings." + Environment.NewLine + "Json Text:" + Environment.NewLine + Str, Method, 0);
					
					//if ((Entries.Length > 0) && (!CommonMethods.EndsWith(Entries, ","))) Entries.Append(",");
					if ((Entries.Length > 0) && (!StringUtilities.EndsWith(Entries, ","))) Entries.Append(",");
					Entries.Append(Settings.ToString());
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
		
		#region SaveSettings
		
		#endregion
		
		#region CommonMethods
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
				string QueryText = "SELECT OutputLine FROM Init_Settings_ChartOfAccounts(@CompanyId, @CategoryOnlyId, @ShowHidden)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", ActionBase.CommonItem.CompanyIdValue));
				
				object Obj = DataCollection["CategoryOnlyId"];
				string Value = (Obj == null) ? "" : Convert.ToString(Obj);
				if (Value == "") Value = "0";
				Params.Add(new FbParameterItem("CategoryOnlyId", Value));
				
				Obj = DataCollection["ShowHidden"];
				Value = (Obj == null) ? "" : Convert.ToString(Obj);
				if (string.IsNullOrEmpty(Value)) Value = "N";
				Params.Add(new FbParameterItem("ShowHidden", Value));
				
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, ActionBase.MasterId);
				
				DataTable tbl = null;
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ActionBase.FbConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
				}
				
				StringBuilder sb = new StringBuilder();
				if (string.IsNullOrEmpty(Result))
				{
					foreach (DataRow Row in tbl.Rows)
					{
						Obj = Row["OUTPUTLINE"];
						sb.AppendLine((Obj == DBNull.Value) ? "" : Convert.ToString(Obj));
					}
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					if (sb.Length == 0) Result = "No content from ChartOfAccount.GatherSettings";
					else
					{
						//if (Entries.Length > 0) Entries.Append(",");
						//if ((Entries.Length > 0) && (!CommonMethods.EndsWith(Entries, ","))) Entries.Append(",");
						if ((Entries.Length > 0) && (!StringUtilities.EndsWith(Entries, ","))) Entries.Append(",");
						Entries.Append(sb.ToString());
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
		
		public static string GatherChartOfAccountItems(string ConnectionString,
		                                               Int64 CompanyId,
		                                               int RecordId,
		                                               int DlgMode,
		                                               ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "GatherChartOfAccountItems";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				//string QueryText = "SELECT OutputLine FROM EAChartOfAccountGroup2(@CompanyId, @RecordId, @DlgMode)";
				string QueryText = GetGroup2QueryText(ConnectionString, CompanyId, RecordId, DlgMode, out Result);
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, null, Method, 0);
				
				DataTable tbl = null;
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, QueryText, null, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
				}
				
				StringBuilder sb = new StringBuilder();
				object Obj = null;
				if (string.IsNullOrEmpty(Result))
				{
					foreach (DataRow Row in tbl.Rows)
					{
						Obj = Row["OUTPUTLINE"];
						sb.AppendLine((Obj == DBNull.Value) ? "" : Convert.ToString(Obj));
					}
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					if (sb.Length == 0) Result = "No content from " + Method;
					else
					{
						//if (Entries.Length > 0) Entries.Append(","); xxxx
						//if ((Entries.Length > 0) && (!CommonMethods.EndsWith(Entries, ","))) Entries.Append(",");
						if ((Entries.Length > 0) && (!StringUtilities.EndsWith(Entries, ","))) Entries.Append(",");
						Entries.Append(JsonUtilities.AddPropertyNameAndValue("RecordId", RecordId));
						Entries.Append(sb.ToString());
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
		
		public static string GetGroup2QueryText(string ConnectionString,
		                                        Int64 CompanyId,
		                                        int RecordId,
		                                        int DlgMode,
		                                        out string ErrorMsg)
		{
			string Result = "";
			string Method = "GetGroup2QueryText";
			ErrorMsg = "";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase= new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string QueryText = "SELECT SqlOut FROM EAChartOfAccountGroupSelect(@CompanyId, @RecordId, @EditMode)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				Params.Add(new FbParameterItem("RecordId", RecordId));
				Params.Add(new FbParameterItem("EditMode", DlgMode));
				ErrorMsg = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				
				DataTable tbl = null;
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out ErrorMsg);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg) && (tbl != null) && (tbl.Rows.Count > 0))
				{
					Object Obj = tbl.Rows[0]["SqlOut"];
					Result = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
				}
				
				if (!string.IsNullOrEmpty(Result))
				{
					Result = Result.Replace("__", "'");
				}
				else ErrorMsg = "Cannot determine the Group Select query";
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
		
		#region OnChange Methods
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
				object Obj = ActionBase.GetCollectionValue("Data", "ChangeAction"); 
				string ChangeAction = (Obj == null) ? "" : Convert.ToString(Obj);
				if (string.IsNullOrEmpty(ChangeAction))
				{
					Result = "Invalid or Missing Action value";
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					switch (ChangeAction)
					{
						case "GridSelectors": Result = OnChange_GridSelectors(ActionBase, DataCollection, ref Entries); break;
						case "ChartCodeGroup": Result = OnChange_ChartCodeGroup(ActionBase, DataCollection, ref Entries); break;
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
		
		public static string OnChange_GridSelectors(ActionBase ActionBase,
		                                            NameValueCollection DataCollection, 
		                                            ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "OnChange_GridSelectors";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				Result = GatherSettings(ActionBase, DataCollection, ref Entries);
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
		
		public static string OnChange_ChartCodeGroup(ActionBase ActionBase,
		                                             NameValueCollection DataCollection,
		                                             ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "OnChange_ChartCodeGroup";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				if (string.IsNullOrEmpty(Result))
				{
					Result = Validate_OnChange_ChartCodeGroup(DataCollection);
				}
				
				List<FbParameterItem> Params = new List<FbParameterItem>();
				string DropdownId = CollectionUtilities.GetCollectionValue(DataCollection, "ItemValue", "");
				if (string.IsNullOrEmpty(Result))
				{
					Params.Add(new FbParameterItem("CompanyId", ActionBase.CommonItem.CompanyIdValue));
					Params.Add(new FbParameterItem("DropDownId", DropdownId));
				}
				
				string GroupNumber = "";
				string QueryText = "";
				if (string.IsNullOrEmpty(Result))
				{
					GroupNumber = CollectionUtilities.GetCollectionValue(DataCollection, "GroupNumber", "");
					QueryText =
						" SELECT FIRST 1 I_ACHART_GROUP" + GroupNumber + "ORDER from eachart" +
						" where i_achart_companyid = @CompanyId and I_ACHART_GROUP" + GroupNumber + "ID = @dropdownid";
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				}
				
				DataTable tbl = null;
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ActionBase.FbConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
				}
				
				int ChartGroup = 0;
				object Obj = null;
				if (string.IsNullOrEmpty(Result) && (tbl != null))
				{
					foreach (DataRow row in tbl.Rows)
					{
						Obj = row["I_AChart_Group" + GroupNumber + "Order"];
						ChartGroup = (Obj == DBNull.Value) ? 0 : Convert.ToInt32(Obj);
					}
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					//if (Entries.Length > 0) Entries.Append(","); xxxx
					//if ((Entries.Length > 0) && (!CommonMethods.EndsWith(Entries, ","))) Entries.Append(",");
					if ((Entries.Length > 0) && (!StringUtilities.EndsWith(Entries, ","))) Entries.Append(",");
					Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("GroupNumber", GroupNumber));
					Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("ChartGroup", ChartGroup, false));
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
		
		public static string Validate_OnChange_ChartCodeGroup(NameValueCollection DataCollection)
		{
			string Result = "";
			string Method = "Validate_OnChange_ChartCodeGroup";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			ActionBase ActionBase = new ActionBase();
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string Value = CollectionUtilities.GetCollectionValue(DataCollection, "GroupNumber", "");
				Result = ValidationChecks.ErrorIfEmptyString(Value, "Group Number");
				
				if (string.IsNullOrEmpty(Result))
				{
					Value = CollectionUtilities.GetCollectionValue(DataCollection, "ItemValue", "");
					Result = ValidationChecks.ErrorIfEmptyString(Value, "Item Value");
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
		
		#region SettingMethod
		public static string SettingMethod(ActionBase ActionBase,
		                                   NameValueCollection DataCollection,
		                                   ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "SettingMethod";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				object Obj = ActionBase.GetCollectionValue("Data", "SettingMethod"); 
				string SettingMethodStr = (Obj == null) ? "" : Convert.ToString(Obj);
				if (string.IsNullOrEmpty(SettingMethodStr))
				{
					Result = "Invalid or Missing Method value";
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					switch (SettingMethodStr)
					{
						//case "UpdateCPIRate": /*Result = OnUpdateCPIRate(ActionBase, DataCollection, ref Entries);*/ break;
						case "ShowReportItem": Result = OnShowReportItem(ActionBase, DataCollection, ref Entries); break;
						default: Result = "Invalid or unknown Method value"; break;
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
		
		public static string OnShowReportItem(ActionBase ActionBase,
		                                      NameValueCollection DataCollection,
		                                      ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "OnShowReportItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string OutputFileName = "";
				string UniqueId = GetCollectionValueDef(DataCollection, "UniqueId", "", out Result);
				if (string.IsNullOrEmpty(Result))
				{
					if (string.IsNullOrEmpty(UniqueId)) Result = "Invalid or missing Unique ID";
				}
				
				int UniqueIdValue = StringUtilities.StrToIntDef(UniqueId, -1);
				if (UniqueIdValue < 0)
				{
					Result = "Invalid or missing Unique ID value";
				}
				
				string ReportSource = "";
				if (string.IsNullOrEmpty(Result))
				{
					ReportSource = GetOnShowReportItemReportSource(ActionBase, DataCollection, out Result);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					if (string.IsNullOrEmpty(ReportSource)) Result = "Missing or Empty ReportSource";
				}
				
				if (string.IsNullOrEmpty(Result))
				{			
					Int64 NewId =
						SystemReportUtilities.Execute(
							ActionBase.FbConnectionString, ActionBase.CommonItem.CompanyIdValue, ActionBase.CommonItem.ClientIdValue, UniqueIdValue, ReportSource, 
							out Result, out OutputFileName);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					string Str =
						JsonUtilities.AddPropertyNameAndValue("FileName", @"/Docs/" + OutputFileName, false);
					Entries.Append(Str);
				}
				else
				{
					Result = ActionBase.LogOnProcessingError(Method, Result, "");
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
		
		public static string GetOnShowReportItemReportSource(ActionBase ActionBase,
		                                                     NameValueCollection DataCollection,
		                                                     out string ErrorMsg)
		{
			string Result = "";
			string Method = "GetOnShowReportItemReportSource";
			ErrorMsg = "";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try				
			{
				StringBuilder TempEntries = new StringBuilder();
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg = GatherSettings(ActionBase, DataCollection, ref TempEntries);
				}
				
				WindowsEventLoggingUtilities.WriteToInfoEvent("After GatherSettings", Method, ActionBase.MasterId);
				WindowsEventLoggingUtilities.WriteToInfoEvent("TempEntries.Length: " + TempEntries.Length, Method, 0);
				WindowsEventLoggingUtilities.WriteToInfoEvent("JsonText: " + Environment.NewLine + TempEntries.ToString(), Method, 0);
				WindowsEventLoggingUtilities.WriteToInfoEvent("ErrorMsg: " + ErrorMsg, Method, 0);

				string Str = "";
				JObject JObj = null;
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Str =
						JsonUtilities.StartJson() +
						TempEntries.ToString() +
						JsonUtilities.EndJson();			
					WindowsEventLoggingUtilities.WriteToInfoEvent("JsonText: " + Environment.NewLine + Str, Method, 0);

					if (TempEntries.Length > 0)
					{
						JObj = JObject.Parse(Str);
					}
				}
				
				if (string.IsNullOrEmpty(ErrorMsg) && (JObj != null))
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent("JObj IS NOT NULL", Method, 0);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg) && (JObj == null))
				{
					ErrorMsg = "JObj is NULL";
				}
				
				string UniqueId = "";
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					UniqueId = GetCollectionValueDef(DataCollection, "UniqueId", "", out ErrorMsg);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					if (string.IsNullOrEmpty(UniqueId)) ErrorMsg = "Invalid or missing Unique ID";
				}				

				string ButtonId = "";
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ButtonId = GetCollectionValueDef(DataCollection, "ButtonId", "", out ErrorMsg);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					if (string.IsNullOrEmpty(ButtonId)) ErrorMsg = "Invalid or missing Button ID";
				}
			
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					string JPath = @"$.chartofaccounts.gridentries[?(@.Value == '" + UniqueId + "')]";
					WindowsEventLoggingUtilities.WriteToInfoEvent("JPath: " + JPath, Method, 0);
					JToken Record = JObj.SelectToken(JPath);
					if (Record == null)
						WindowsEventLoggingUtilities.WriteToInfoEvent("Record IS NULL", Method, 0);
					else
					{
						WindowsEventLoggingUtilities.WriteToInfoEvent("Record: " + Record.ToString(), Method, 0);
						Result = (string)Record["ZL20_B" + ButtonId];				
						WindowsEventLoggingUtilities.WriteToInfoEvent("Result: " + Result, Method, 0);
					}
				}				
			}
			catch (Exception Ex)
			{
				ErrorMsg = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
			}
			
			return Result;
		}
		
		
		#endregion
		
		#region Utilities
		public static object GetCollectionValueDef(NameValueCollection Collection, string Key, object DefaultValue, out string ErorMsg)
		{
			ErorMsg = "";
			string Method = "GetCollectionValue";
			object Result = null;
			ActionBase ActionBase = new ActionBase();
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				Result = Collection[Key];
				if (Result == null) Result = DefaultValue;
			}
			catch (Exception Ex)
			{
				ErorMsg = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
			}
			
			return Result;
		}
		
		public static string GetCollectionValueDef(NameValueCollection Collection, string Key, string DefaultValue, out string ErorMsg)
		{
			ErorMsg = "";
			string Method = "GetCollectionValue";
			string Result = "";
			ActionBase ActionBase = new ActionBase();
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				Result = Collection[Key];
				if (Result == null) Result = DefaultValue;
			}
			catch (Exception Ex)
			{
				ErorMsg = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
			}
			
			return Result;
		}
		#endregion
		
		#region PerformCommand Methods
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
				string MethodName = (Obj == null) ? "" : Convert.ToString(Obj);
				if (string.IsNullOrEmpty(MethodName))
				{
					Result = "Invalid or Missing Method value";
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					switch (MethodName)
					{
						case "GatherChartOfAcccountItems": Result = OnPerform_GatherChartOfAcountItems(ActionBase, DataCollection, ref Entries); break;
						case "RemoveAddRecord": Result = OnPerform_RemoveAddRecord(ActionBase, DataCollection, ref Entries); break;
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
		
		public static string OnPerform_GatherChartOfAcountItems(ActionBase ActionBase,
		                                                        NameValueCollection DataCollection,
		                                                        ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "OnPerform_GatherChartOfAcountItems";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string DlgModeValue = "";
				if (string.IsNullOrEmpty(Result))
				{
					DlgModeValue = GetCollectionValueDef(DataCollection, "DlgMode", "", out Result);
					if (string.IsNullOrEmpty(DlgModeValue)) Result = "Invalid or missing Mode value";
				}
				
				string RecordIdValue = "";
				if (string.IsNullOrEmpty(Result))
				{
					RecordIdValue = GetCollectionValueDef(DataCollection, "RecordId", "", out Result);
					if (string.IsNullOrEmpty(RecordIdValue)) Result = "Invalid or missing Record value";
				}
				
				string ChartType = "";
				if (string.IsNullOrEmpty(Result))
				{
					ChartType = GetCollectionValueDef(DataCollection, "ChartType", "", out Result);
					if (string.IsNullOrEmpty(ChartType)) Result = "Invalid or missing Chart Type value";
				}
				
				int DlgMode = StringUtilities.StrToIntDef(DlgModeValue, -1);
				int RecordId = StringUtilities.StrToIntDef(RecordIdValue, -1);
				if (string.IsNullOrEmpty(Result) && (DlgMode < 0))
				{
					Result = "Invalid Mode value (" + DlgModeValue + ")";
				}
				if (string.IsNullOrEmpty(Result) && (RecordId < 0))
				{
					Result = "Invalid Record value (" + RecordIdValue + ")";
				}
				
				if (string.IsNullOrEmpty(Result) && (RecordId == 0))
				{
					RecordId = 
						UsefulDBMethods.GeneratorHelpers.GetNextGeneratorId(
							ActionBase.FbConnectionString, "I_ACHART_ID_GEN", 
							ActionBase.CommonItem.CompanyIdValue, 
							ActionBase.CommonItem.ClientIdValue, out Result);
					
					if (string.IsNullOrEmpty(Result) && (RecordId > 0))
					{
						Result = 
							ExecuteEAChartCodeInitialAdd(
								ActionBase.FbConnectionString,
								ActionBase.CommonItem.CompanyIdValue,
								RecordId, ChartType);
					}
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = 
						GatherChartOfAccountItems(
							ActionBase.FbConnectionString, ActionBase.CommonItem.CompanyIdValue, RecordId, DlgMode, ref Entries);
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
		
		public static string ExecuteEAChartCodeInitialAdd(string ConnectionString,
		                                                  Int64 CompanyId,
		                                                  int RecordId,
		                                                  string ChartType)
		{
			string Result = "";
			string Method = "ExecuteEAChartCodeInitialAdd";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			ActionBase ActionBase = new ActionBase();
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string QueryText = "EXECUTE PROCEDURE EAChartCodeInitialAdd(@CompanyId, @RecordId, @ChartType)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				Params.Add(new FbParameterItem("RecordId", RecordId));
				Params.Add(new FbParameterItem("ChartType", ChartType));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
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
		
		public static string OnPerform_RemoveAddRecord(ActionBase ActionBase,
		                                               NameValueCollection DataCollection, 
		                                               ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "OnPerform_RemoveAddRecord";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				bool Found = false;
				string RecordId = CollectionUtilities.GetCollectionValue(DataCollection, "RecordId", "",  out Found);
				if (string.IsNullOrEmpty(RecordId) || (Found == false)) 
					Result = "Invalid or missing Record value provided.";
				
				string QueryText = "delete from eachart where i_achart_id = @RecordID and AChart_AccountNumber = '000.00'";				
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					Params.Add(new FbParameterItem("RecordID", RecordId));
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(ActionBase.FbConnectionString, QueryText, Params, 0);
				}
				
				if (string.IsNullOrEmpty(RecordId))
				{
					if (Entries.Length > 0) Entries.AppendLine(",");
					Entries.Append(JsonUtilities.AddPropertyNameAndValue("Status", "Success", false));
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
