/*
 * Created by SharpDevelop.
 * User: John
 * Date: 17/09/2019
 * Time: 10:11 PM
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
	/// Description of SaveBankSettings.
	/// </summary>
	public class SaveBankSettings : ActionBase
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

			SaveBankSettings inst = new SaveBankSettings();
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
				Result = SaveBankSettingsItems();
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
		
		private string SaveBankSettingsItems()
		{
			string Result = "";
			int IntValue = 0;
			string StrValue = "";
			
			if (string.IsNullOrEmpty(Result))
			{
				StrValue = (string)DataObject["CurrentMonth"];
				IntValue = StringUtilities.StrToIntDef(StrValue, -1);
				if (IntValue < 0)
				{
					Result = "Invalid value passed for CurrentMonth";
				}
				else
				{
					Result =
						SaveCurrentMonth(
							FbConnectionString, CommonItem.CompanyIdValue, IntValue);
				}
			}
			
			if (string.IsNullOrEmpty(Result))
			{
				StrValue = (string)DataObject["CurrentYear"];
				IntValue = StringUtilities.StrToIntDef(StrValue, -1);
				if (IntValue < 0)
				{
					Result = "Invalid value passed for CurrentYear";
				}
				else
				{
					Result =
						SaveCurrentYear(
							FbConnectionString, CommonItem.CompanyIdValue, IntValue);
				}
			}
			
			if (string.IsNullOrEmpty(Result))
			{
				StrValue = (string)DataObject["DirectEntryFileType"];
				IntValue = StringUtilities.StrToIntDef(StrValue, -1);
				if (IntValue < 0)
				{
					Result = "Invalid value passed for DirectEntryFileType";
				}
				else
				{
					Result =
						CommonMethods.BankSettings_SaveDirectEntryFileType(
							FbConnectionString, CommonItem.CompanyIdValue, IntValue);
				}
			}
			
			if (string.IsNullOrEmpty(Result))
			{
				StrValue = (string)DataObject["DirectEntryCustomerNumber"];
				Result = 
					CommonMethods.BankSettings_SaveDirectEntryCustomerNumber(
						FbConnectionString, CommonItem.CompanyIdValue, StrValue);
			}
			
			if (string.IsNullOrEmpty(Result))
			{
				StrValue = (string)DataObject["DirectDebitCustomerNumber"];
				Result = 
					CommonMethods.BankSettings_SaveDirectDebitCustomerNumber(
						FbConnectionString, CommonItem.CompanyIdValue, StrValue);
			}
			
			if (string.IsNullOrEmpty(Result))
			{
				StrValue = (string)DataObject["BPayDefaults"];
				IntValue = StringUtilities.StrToIntDef(StrValue, -1);
				if (IntValue < 0)
				{
					Result = "Invalid value passed for DirectEntryFileType";
				}
				else
				{
					Result = 
						SaveBPayDefaults(
							FbConnectionString, CommonItem.CompanyIdValue, IntValue);
				}
			}
			
			if (string.IsNullOrEmpty(Result))
			{
				StrValue = (string)DataObject["BPayCustomerNumber"];
				Result = 
					SaveBPayCustomerNumber(
						FbConnectionString, CommonItem.CompanyIdValue, StrValue);
			}
			
			if (string.IsNullOrEmpty(Result))
			{
				StrValue = (string)DataObject["NSWUniqueTrustId"];
				Result = 
					CommonMethods.BankSettings_SaveNSWUniqueTrustId(
						FbConnectionString, CommonItem.CompanyIdValue, StrValue);
			}
			
			if (string.IsNullOrEmpty(Result))
			{
				StrValue = (string)DataObject["EffectiveOfficer"];
				Result = 
					CommonMethods.BankSettings_SaveExecutiveOfficer(
						FbConnectionString, CommonItem.CompanyIdValue, StrValue);
			}

            if (string.IsNullOrEmpty(Result))
            {
                StrValue = (string)DataObject["BankImportTypes"];
                Result =
                    CommonMethods.BankSettings_SaveBankImportTypes(
                        FbConnectionString, CommonItem.CompanyIdValue, StrValue);
            }

            //
            return Result;
		}
		
		private string SaveBPayDefaults(string ConnectionString,
		                                Int64 CompanyId,
		                                int BPayDefaultValue)
		{
			string Result = "";
			string Method = "SaveBPayDefaults";
			string QueryText =
				"UPDATE EACOMPANY SET I_CO_BPAYDEFAULT = @BPayDefaultValue WHERE I_CO_COMPANYCLIENTID = @CompanyId";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("BPayDefaultValue", BPayDefaultValue));
			Params.Add(new FbParameterItem("CompanyId", CompanyId));
			Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
			if (string.IsNullOrEmpty(Result))
			{
				Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
			}
			
			return Result;
		}
		
		private string SaveBPayCustomerNumber(string ConnectionString,
		                                      Int64 CompanyId,
		                                      string BPayCustomerNumber)
		{
			string Result = "";
			string Method = "SaveBPayCustomerNumber";
			string QueryText =
				"UPDATE EACOMPANY SET CO_BPAYCUSTOMERNUMBER = @BPayCustomerNumber WHERE I_CO_COMPANYCLIENTID = @CompanyId";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("BPayCustomerNumber", BPayCustomerNumber));
			Params.Add(new FbParameterItem("CompanyId", CompanyId));
			Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
			if (string.IsNullOrEmpty(Result))
			{
				Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
			}
			
			return Result;
		}
		
		private string SaveCurrentMonth(string ConnectionString,
		                                Int64 CompanyId,
		                                int CurrentMonth)
		{
			string Result = "";
			string Method = "SaveCurrentMonth";
			string QueryText =
				"UPDATE EACOMPANY SET I_CO_CURRENTMONTH = @CurrentMonth WHERE I_CO_COMPANYCLIENTID = @CompanyId";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("CurrentMonth", CurrentMonth));
			Params.Add(new FbParameterItem("CompanyId", CompanyId));
			Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
			if (string.IsNullOrEmpty(Result))
			{
				Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
			}
			
			return Result;
		}
		
		private string SaveCurrentYear(string ConnectionString,
		                               Int64 CompanyId,
		                               int CurrentYear)
		{
			string Result = "";
			string Method = "SaveCurrentYear";
			string QueryText =
				"UPDATE EACOMPANY SET I_CO_CURRENTYEAR = @CurrentYear WHERE I_CO_COMPANYCLIENTID = @CompanyId";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("CurrentYear", CurrentYear));
			Params.Add(new FbParameterItem("CompanyId", CompanyId));
			Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
			if (string.IsNullOrEmpty(Result))
			{
				Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
			}
			
			return Result;
		}
	}
}
