/*
 * Created by SharpDevelop.
 * User: John
 * Date: 02/04/2020
 * Time: 7:07 PM
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
	/// Description of CommercialSettings.
	/// </summary>
	public class CommercialSettings : ActionBase
	{
		//private string FModuleName = "SettingsOperations";
		
		public CommercialSettings()
		{
		}
		
		public static string ValidatePassingValues(ActionBase ActionBase,
		                                           NameValueCollection DataCollection)
		{
			string Result = "";
			string Method = "ValidatePassingValues";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " + Method, Method, ActionBase.MasterId);			
			
			ActionBase.LogOnInsideMethod(Method);
			try
			{
				List<string> Items = new List<string>();
				Items.Add("Collection.Count: " + DataCollection.Count.ToString());
				string Item = "";
				foreach(string Key in DataCollection.Keys)
				{
					Item = Key + ": " + DataCollection.Get(Key);
					Items.Add(Item);
				}
				Item = string.Join(Environment.NewLine, Items.ToArray());
				WindowsEventLoggingUtilities.WriteToInfoEvent(Item, Method, 0);
				WindowsEventLoggingUtilities.WriteToInfoEvent("ConnectionString: " + Environment.NewLine + ActionBase.FbConnectionString, Method, 0);
				
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
		
		public static string LoadCommercialSettings(ActionBase ActionBase,
		                                            NameValueCollection DataCollection,
		                                            ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "LoadCommercialSettings";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " + Method, Method, ActionBase.MasterId);			
			
			ActionBase.LogOnInsideMethod(Method);
			try
			{
				StringBuilder CommercialEntries = new StringBuilder();
				Result = GatherSettings(ActionBase, DataCollection, ref CommercialEntries);
				
				string CPIRate = "";
				if (string.IsNullOrEmpty(Result))
				{
					CPIRate = GetCPIRateValue(ActionBase, CommercialEntries, out Result);
					if (string.IsNullOrEmpty(Result))
					{
						if (CommercialEntries.Length > 0) CommercialEntries.AppendLine(",");
						CommercialEntries.Append(JsonUtilities.AddPropertyNameAndValue("CPI_Rate", CPIRate, false));
					}
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					string Str = JsonUtilities.StartJson() + CommercialEntries.ToString() + JsonUtilities.EndJson();
					WindowsEventLoggingUtilities.WriteToInfoEvent("After CPIRate:" + Environment.NewLine + Str, Method, ActionBase.MasterId);			
					
					//if ((Entries.Length > 0) && (!CommonMethods.EndsWith(Entries, ","))) Entries.Append(",");
					if ((Entries.Length > 0) && (!StringUtilities.EndsWith(Entries, ","))) Entries.Append(",");
					Entries.Append(JsonUtilities.StartJsonObject("CommercialSettings"));
					Entries.Append(CommercialEntries.ToString());
					Entries.Append(JsonUtilities.EndJsonObject());
				}
				/*
				else
				{
					Result = ActionBase.LogOnProcessingError(Method, Result, ""); 
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
		
		public static string GatherSettings(ActionBase ActionBase,
		                                    NameValueCollection DataCollection,
		                                    ref StringBuilder CommercialEntries)
		{
			string Result = "";
			string Method = "LoadCommercialSettings";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " + Method, Method, ActionBase.MasterId);			
			
			ActionBase.LogOnInsideMethod(Method);
			try
			{
				if (string.IsNullOrEmpty(Result))
				{
					Result = ValidateGatherSettingsRequestItems(DataCollection);
				}
				string QueryText = 
					"SELECT OutputLine FROM Init_Settings_Commercial(@CompanyId, @CategoryOnlyId, @StmtKitReportNumber, @StmtKitOrderNumber, @StmtKitReportId)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", ActionBase.CommonItem.CompanyIdValue));
				string Value = CollectionUtilities.GetCollectionValue(DataCollection, "CategoryOnlyId", "");
				if (Value == "") Value = "0";
				Params.Add(new FbParameterItem("CategoryOnlyId", Value));
				
				Value = CollectionUtilities.GetCollectionValue(DataCollection, "StmtKitReportNumber", "0");
				Params.Add(new FbParameterItem("StmtKitReportNumber", Value));
				
				Value = CollectionUtilities.GetCollectionValue(DataCollection, "StmtKitOrderNumber", "0");
				Params.Add(new FbParameterItem("StmtKitOrderNumber", Value));
				
				Value = CollectionUtilities.GetCollectionValue(DataCollection, "StmtKitReportId", "0");
				Params.Add(new FbParameterItem("StmtKitReportId", Value));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, ActionBase.MasterId);
				
				DataTable tbl = null;
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ActionBase.FbConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					object Obj;
					foreach (DataRow Row in tbl.Rows)
					{
						Obj = Row["OUTPUTLINE"];
						CommercialEntries.AppendLine((Obj == DBNull.Value) ? "" : Convert.ToString(Obj));
					}
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					if (CommercialEntries.Length == 0) Result = "No content from Commercial.GatherSettings";
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
		
		public static string ValidateGatherSettingsRequestItems(NameValueCollection DataCollection)
		{
			string Result = "";
			string Method = "ValidateGatherSettingsRequestItems";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
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
		
		
		public static string GetCPIRateValue(ActionBase ActionBase, 
		                                    StringBuilder CommercialEntries, 
		                                    out string ErrorMsg)
		{
			string Result = "";
			string Method = "GetCPIRateValue";
			ErrorMsg = "";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string Str = JsonUtilities.StartJson() + CommercialEntries.ToString() + JsonUtilities.EndJson();
				//WindowsEventLoggingUtilities.WriteToInfoEvent("CommercialEntries: " + Environment.NewLine + Str, Method, 0);
				JObject JObj = null;
				if (CommercialEntries.Length > 0)
				{
					JObj = JObject.Parse(Str);
				}

				string QueryText = "";
				if (string.IsNullOrEmpty(ErrorMsg) && (JObj != null))
				{
					JToken token = JObj.SelectToken("$.commercial");
					QueryText = (string)token["sqltogetrate"];
					WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText: " + Environment.NewLine + QueryText, Method, 0);
					if (string.IsNullOrEmpty(QueryText)) ErrorMsg = "Cannot determine the QueryText in " + Method;
				}
				
				DataTable tbl = null;
				if (string.IsNullOrEmpty(ErrorMsg) && !string.IsNullOrEmpty(QueryText))
				{
					// replace the parameter in the udpate script
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ActionBase.FbConnectionString, QueryText, null, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTablesAndRows, out ErrorMsg);
					Str = (ErrorMsg == "") ? "No Issue" : ErrorMsg;
					WindowsEventLoggingUtilities.WriteToInfoEvent("Execution Result: " + Environment.NewLine + Str, Method, 0);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					object Obj = tbl.Rows[0]["M_CPI_RATE"];
					Result = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					if (string.IsNullOrEmpty(Result)) ErrorMsg = "CPI Rate is invalid";
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
						case "CPIType": 
							Result = OnChange_CPIType(ActionBase, DataCollection, ref Entries);
							break;
						case "StmtKitReportList": 
							Result = OnChange_StmtKitReportList(ActionBase, DataCollection, ref Entries);
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
		
		/// <summary>
		/// When changing the CPIType we only need to return the CPI Rate for
		/// the new CategoryOnlyId.
		/// The new value for the combo box is already set
		/// </summary>
		/// <param name="ActionBase"></param>
		/// <param name="DataCollection">The collection of form data values passed to the calling method</param>
		/// <param name="Entries">The returned CPIRate value for the provided CategoryOnlyId</param>
		/// <returns></returns>
		public static string OnChange_CPIType(ActionBase ActionBase,
		                                      NameValueCollection DataCollection,
		                                      ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "OnChange_CPIType";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				StringBuilder TempEntries = new StringBuilder();
				Result = GatherSettings(ActionBase, DataCollection, ref TempEntries);
				
				string CPIRate = "";
				if (string.IsNullOrEmpty(Result))
				{
					CPIRate = GetCPIRateValue(ActionBase, TempEntries, out Result);
					if (string.IsNullOrEmpty(Result))
					{
						if (Entries.Length > 0) Entries.AppendLine(",");
						Entries.Append(JsonUtilities.AddPropertyNameAndValue("CPI_Rate", CPIRate, false));
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
		
		public static string OnChange_StmtKitReportList(ActionBase ActionBase,
		                                                NameValueCollection DataCollection,
		                                                ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "OnChange_StmtKitReportList";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				/* Just return the gathered values.
				 * The calling method should only use the StmtKitGroup entries
				 * to fullfil its requriements
				*/
				Result = LoadCommercialSettings(ActionBase, DataCollection, ref Entries);
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
				/*
				object Obj = ActionBase.GetCollectionValue("Data", "SettingMethod");
				string SettingMethodStr = (Obj == null) ? "" : Convert.ToString(Obj);
				*/
				string SettingMethodStr = CollectionUtilities.GetCollectionValue(ActionBase.DataCollection, "SettingMethod", "");
				if (string.IsNullOrEmpty(SettingMethodStr))
				{
					Result = "Invalid or Missing Method value";
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					switch (SettingMethodStr)
					{
						case "UpdateCPIRate": 
							Result = OnUpdateCPIRate(ActionBase, DataCollection, ref Entries);
							break;
						case "UpdateStmtKitReport": 
							Result = OnUpdateStmtKitReport(ActionBase, DataCollection, ref Entries);
							break;
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
		
		public static string OnUpdateCPIRate(ActionBase ActionBase,
		                                     NameValueCollection DataCollection,
		                                     ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "OnUpdateCPIRate";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				object Obj = null;
				string TempStr = "";
				
				if (string.IsNullOrEmpty(Result))
				{
					Obj = DataCollection["CategoryOnlyId"];
					TempStr = (Obj == null) ? "" : Convert.ToString(Obj);
					if (string.IsNullOrEmpty(TempStr)) Result = "Invalid or missing Category value.";
				}
				
				
				string NewRate = "";
				if (string.IsNullOrEmpty(Result))
				{
					Obj = DataCollection["NewRate"];
					NewRate = (Obj == null) ? "" : Convert.ToString(Obj);
					if (string.IsNullOrEmpty(NewRate)) Result = "Invalid or missing Rate value.";
				}

				StringBuilder CommercialEntries = new StringBuilder();
				if (string.IsNullOrEmpty(Result))
				{
					Result = GatherSettings(ActionBase, DataCollection, ref CommercialEntries);
				}
				
				string UpdateString = "";
				if (string.IsNullOrEmpty(Result))
				{
					UpdateString = GetSqlForUpdate(ActionBase, CommercialEntries, out Result);
				}
				
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					UpdateString = UpdateString.Replace("NEWRATE", "@NewRate");
					Params.Add(new FbParameterItem("NewRate", NewRate));
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(UpdateString, Params, Method, ActionBase.MasterId);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(ActionBase.FbConnectionString, UpdateString, Params, 0);
					TempStr = (string.IsNullOrEmpty(Result)) ? "All OK" : Result;
					WindowsEventLoggingUtilities.WriteToInfoEvent("OnUpdateCPIRate Result: " + TempStr, Method, ActionBase.MasterId);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					/*
					if (Entries.Length > 0) Entries.AppendLine(",");
					Entries.Append(JsonUtilities.AddPropertyNameAndValue("Status", "OK", false));
					*/
					Result = LoadCommercialSettings(ActionBase, DataCollection, ref Entries);
				}
				
				if (!string.IsNullOrEmpty(Result))
				{
					Result = ActionBase.LogOnProcessingError(Method, Result, "");
				}
				WindowsEventLoggingUtilities.WriteToInfoEvent("Execute Result: " + Result, Method, ActionBase.MasterId);
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
		
		public static string GetSqlForUpdate(ActionBase ActionBase,
		                                     StringBuilder CommercialEntries,
		                                     out string ErrorMsg)
		{
			string Result = "";
			string Method = "GetSqlForUpdate";
			ErrorMsg = "";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string Str = JsonUtilities.StartJson() + CommercialEntries.ToString() + JsonUtilities.EndJson();
				JObject JObj = null;
				if (CommercialEntries.Length > 0)
				{
					JObj = JObject.Parse(Str);
				}

				if (string.IsNullOrEmpty(ErrorMsg) && (JObj != null))
				{
					JToken token = JObj.SelectToken("$.commercial.sqlforupdate");
					//Result = (string)token["sqlforupdate"];
					Result = (string)token;
					//WindowsEventLoggingUtilities.WriteToInfoEvent("SqlForUpdate " + Environment.NewLine + Result, Method, ActionBase.MasterId);
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
		
		public static string  OnUpdateStmtKitReport(ActionBase ActionBase,
		                                            NameValueCollection DataCollection,
		                                            ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "OnUpdateStmtKitReport";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string ReportId = CollectionUtilities.GetCollectionValue(DataCollection, "StmtKitReportId", "");
				Result = ValidationChecks.ErrorIfEmptyString(ReportId, "Report ID");
				
				string OrderNumber = "";
				if (string.IsNullOrEmpty(Result))
				{
					OrderNumber = CollectionUtilities.GetCollectionValue(DataCollection, "StmtKitOrderNumber", "");
					Result = ValidationChecks.ErrorIfEmptyString(ReportId, "Order Number");
				}
				
				
				StringBuilder TempEntries = new StringBuilder();					
				if (string.IsNullOrEmpty(Result))
				{
					Result = LoadCommercialSettings(ActionBase, DataCollection, ref TempEntries);
				}
				
				string Path = "";
				string QueryText = "";
				List<FbParameterItem> Params = new List<FbParameterItem>();				
				if (string.IsNullOrEmpty(Result))
				{
					Path = "$.CommercialSettings.commercial.StmtKitGroup.UpdateSQL";
					QueryText = JsonUtilities.GetJsonPathValueStr(TempEntries.ToString(), Path, out Result);
					if (string.IsNullOrEmpty(Result))
					{
						Params.Add(new FbParameterItem("reportid", ReportId));
						Params.Add(new FbParameterItem("ORDERNUMBER", OrderNumber));
						Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
					}
				}
				
				if (string.IsNullOrEmpty(Result) && !string.IsNullOrEmpty(QueryText))
				{
					Result = FirebirdUtilities.DBExecute(ActionBase.FbConnectionString, QueryText, Params, 0);
					string TempStr = (string.IsNullOrEmpty(Result)) ? "All OK" : Result;
					WindowsEventLoggingUtilities.WriteToInfoEvent("OnUpdateStmtKitReport Result: " + TempStr, Method, ActionBase.MasterId);
				}

				if (!string.IsNullOrEmpty(Result))
				{
					Result = ActionBase.LogOnProcessingError(Method, Result, "");
				}
				WindowsEventLoggingUtilities.WriteToInfoEvent("Execute Result: " + Result, Method, ActionBase.MasterId);
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
		
		public static string SaveSettings(ActionBase ActionBase, SaveCommercialSettingsRequestItem RequestItem)
		{
			string Result = "";
			string Method = "SaveSettings";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try
			{
                /*
                if (string.IsNullOrEmpty(Result))
				{
					Result = 
						SaveSettings_SaveMinTaxInvoiceAmount(
							ActionBase.FbConnectionString, ActionBase.CommonItem.CompanyIdValue, RequestItem.MinTaxInvoiceAmount);
				}
				*/

                string QueryText = "";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText =
                        " UPDATE eacompanysettings2" +
                        " SET I_CO_TAXINVOICEMINIMUM = @TaxInvoiceMin," +
                        "     I_CO_COMINSPECTIONADVANCE = @ComInspectionAdvance," +
                        "     B_CO_COMLEASEEXPIRY = @ComLeaseExpiry," +
                        "     B_CO_COMVACATEDATE = @ComVacateDate," +
                        "     B_CO_COMRENTREVIEW = @ComRentReview," +
                        "     B_CO_COMOPTIONBEGIN = @ComOptionBegin," +
                        "     B_CO_COMOPTIONEND = @ComOptionEnd," +
                        "     I_CO_COMLETACID = @COMLETACID," +
                        "     I_CO_COMADVACID = @COMADVACID" +
                        " WHERE I_COMPANYSETTINGS2_COMPANYID = @CompanyId";
                    Params.Add(new FbParameterItem("TaxInvoiceMin", RequestItem.MinTaxInvoiceAmount));
                    Params.Add(new FbParameterItem("ComInspectionAdvance", RequestItem.ComInspectionAdvance));
                    Params.Add(new FbParameterItem("ComLeaseExpiry", RequestItem.ComLeaseExpiry));
                    Params.Add(new FbParameterItem("ComVacateDate", RequestItem.ComVacateDate));
                    Params.Add(new FbParameterItem("ComRentReview", RequestItem.ComRentalReview));
                    Params.Add(new FbParameterItem("ComOptionBegin", RequestItem.ComOptionBegin));
                    Params.Add(new FbParameterItem("ComOptionEnd", RequestItem.ComOptionEnd));
                    Params.Add(new FbParameterItem("COMLETACID", RequestItem.LettingFeeAccountId));
                    Params.Add(new FbParameterItem("COMADVACID", RequestItem.AdvertisingAccountId));
                    Params.Add(new FbParameterItem("CompanyId", ActionBase.CommonItem.CompanyIdValue));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, ActionBase.MasterId);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = FirebirdUtilities.DBExecute(ActionBase.FbConnectionString, QueryText, Params, 0);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    QueryText =
                        " UPDATE eacompanysettings" +
                        " SET B_CO_COMENDOFYEARSTATEMENT = @ComEndOfYearStmt," +
                        "     I_CO_COMENDOFYEARSTATEMENTTO = @ComEndOfYearStmtTo," +
                        "     M_CO_COMENDOFYEARSTATEMENTFEE = @ComEndOfYearStmtFee" +
                        " WHERE I_SETTINGS_COMPANYID = @CompanyId";
                    Params.Clear();
                    Params.Add(new FbParameterItem("ComEndOfYearStmt", RequestItem.EndOfYearStatement));
                    Params.Add(new FbParameterItem("ComEndOfYearStmtTo", RequestItem.SendEndOfYearStmtTo));
                    Params.Add(new FbParameterItem("ComEndOfYearStmtFee", RequestItem.EndOfYearStmtFee));
                    Params.Add(new FbParameterItem("CompanyId", ActionBase.CommonItem.CompanyIdValue));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, ActionBase.MasterId);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = FirebirdUtilities.DBExecute(ActionBase.FbConnectionString, QueryText, Params, 0);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    QueryText =
                        " UPDATE eacompany" +
                        " SET M_CO_COMADDONPERCENTAGE = @COMADDONPERCENTAGE," +
                        "     I_CO_ESTIMATEDOUTGOPTION = @ESTIMATEDOUTGOPTION" +
                        " WHERE I_CO_CompanyClientId = @CompanyId";
                    Params.Clear();
                    Params.Add(new FbParameterItem("COMADDONPERCENTAGE", RequestItem.EstOutgoingsAddPercent));
                    Params.Add(new FbParameterItem("ESTIMATEDOUTGOPTION", RequestItem.EstOutgoingsSource));
                    Params.Add(new FbParameterItem("CompanyId", ActionBase.CommonItem.CompanyIdValue));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, ActionBase.MasterId);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = FirebirdUtilities.DBExecute(ActionBase.FbConnectionString, QueryText, Params, 0);
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
		
		public static string SaveSettings_SaveMinTaxInvoiceAmount(string ConnectionString,
		                                                          Int64 CompanyId,
		                                                          int MinTaxInvoiceAmount)
		{
			string Result = "";
			string Method = "MethodName";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			ActionBase ActionBase = new ActionBase();
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string QueryText =
					"UPDATE eacompanysettings2 SET I_CO_TAXINVOICEMINIMUM = @NewValue WHERE I_COMPANYSETTINGS2_COMPANYID = @CompanyId";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("NewValue", MinTaxInvoiceAmount));
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
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
		
	}
}
