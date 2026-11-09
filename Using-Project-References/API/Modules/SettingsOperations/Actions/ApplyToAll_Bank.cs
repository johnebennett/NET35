/*
 * Created by SharpDevelop.
 * User: John
 * Date: 18/09/2019
 * Time: 10:35 AM
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
	/// Description of ApplyToAll_Bank.
	/// </summary>
	public class ApplyToAll_Bank : ActionBase
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

			ApplyToAll_Bank inst = new ApplyToAll_Bank();
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
				Result = ProcessItem();
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
		
		private string ProcessItem()
		{
			string Result = "";
			string ItemType = (string)DataObject["type"];
			string StrValue = (string)DataObject["value"];
			switch (ItemType)
			{
				case "DirectEntryFileType":
					Result = ProcessDirectEntryFileType(StrValue);
					break;
				case "DirectEntryCustomerNumber":
					Result = ProcessDirectEntryCustomerNumber(StrValue);
					break;
				case "DirectDebitCustomerNumber":
					Result = ProcessDirectDebitCustomerNumber(StrValue);
					break;
				case "NSWUniqueTrustId":
					Result = ProcessNSWUniqueTrustId(StrValue);
					break;
				case "EffectiveOfficer":
					Result = ProcessEffectiveOfficer(StrValue);
					break;
				default: Result = "Unknown Type: " + ItemType; break;
			}

			return Result;
		}
		
		private string ProcessDirectEntryFileType(string Value)
		{
			string Result = "";
			int IntValue = StringUtilities.StrToIntDef(Value, -1);
			
			if (IntValue < 0)
			{
				Result = "Invalid value provided for DirectEntryFileType";
			}
			
			if (string.IsNullOrEmpty(Result))
			{
				Result =
					CommonMethods.BankSettings_SaveDirectEntryFileType(
						FbConnectionString, CommonItem.CompanyIdValue, IntValue);
			}
			
			if (string.IsNullOrEmpty(Result))
			{
				// do the update all here
				string QueryText =
					//"UPDATE eabankname SET I_ABANK_BALANCINGOPTION = @directentryid WHERE I_CO_COMPANYCLIENTID = @CompanyId";
                    "UPDATE eabankname SET I_ABANK_BALANCINGOPTION = @directentryid WHERE I_ABANK_COMPANYCLIENTID = @CompanyId";
                List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("DirectEntryId", Value));
				Params.Add(new FbParameterItem("CompanyId", CommonItem.CompanyIdValue));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(FbConnectionString, QueryText, Params, 0);
				}
			}
			
			return Result;			
		}
		
		private string ProcessDirectEntryCustomerNumber(string Value)
		{
			string Result = "";

			if (string.IsNullOrEmpty(Result))
			{
				Result =
					CommonMethods.BankSettings_SaveDirectEntryCustomerNumber(
						FbConnectionString, CommonItem.CompanyIdValue, Value);
			}
			
			if (string.IsNullOrEmpty(Result))
			{
				// do the update all here
				string QueryText =
					" UPDATE eabankname" +
                    "   SET abank_directentryusernumber = (SELECT FIRST 1 CO_DIRECTENTRYCUSTOMERNUMBER FROM EACOMPANY WHERE I_CO_COMPANYCLIENTID = @CompanyId)" +
					" WHERE i_abank_companyclientid = @CompanyId";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CommonItem.CompanyIdValue));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(FbConnectionString, QueryText, Params, 0);
				}
			}
			
			return Result;
		}
		
		private string ProcessDirectDebitCustomerNumber(string Value)
		{
			string Result = "";

			if (string.IsNullOrEmpty(Result))
			{
				Result =
					CommonMethods.BankSettings_SaveDirectDebitCustomerNumber(
						FbConnectionString, CommonItem.CompanyIdValue, Value);
			}
			
			if (string.IsNullOrEmpty(Result))
			{
				// do the update all here
				string QueryText =
					" UPDATE eabankname" +
					"   SET ABANK_DIRECTDEBITUSERNUMBER = (SELECT FIRST 1 CO_DIRECTDEBITCUSTOMERNUMBER FROM EACOMPANY WHERE I_CO_COMPANYCLIENTID = @CompanyId)" +
					" WHERE i_abank_companyclientid = @CompanyId";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CommonItem.CompanyIdValue));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(FbConnectionString, QueryText, Params, 0);
				}
			}
			
			return Result;
		}
		
		private string ProcessNSWUniqueTrustId(string Value)
		{
			string Result = "";

			if (string.IsNullOrEmpty(Result))
			{
				Result =
					CommonMethods.BankSettings_SaveDirectDebitCustomerNumber(
						FbConnectionString, CommonItem.CompanyIdValue, Value);
			}
			
			if (string.IsNullOrEmpty(Result))
			{
				// do the update all here
				string QueryText =
					" UPDATE eabankname" +
					"   SET ABANK_NSWUNIQUEID = (SELECT FIRST 1 CO_NSW_TRUSTID FROM EACOMPANY WHERE I_CO_COMPANYCLIENTID = @CompanyId)" +
					" WHERE i_abank_companyclientid = @CompanyId";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CommonItem.CompanyIdValue));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(FbConnectionString, QueryText, Params, 0);
				}
			}
			
			return Result;
		}
		
		private string ProcessEffectiveOfficer(string Value)
		{
			string Result = "";

			if (string.IsNullOrEmpty(Result))
			{
				Result =
					CommonMethods.BankSettings_SaveDirectDebitCustomerNumber(
						FbConnectionString, CommonItem.CompanyIdValue, Value);
			}
			
			if (string.IsNullOrEmpty(Result))
			{
				// do the update all here
				string QueryText =
					" UPDATE eabankname" +
					"   SET ABANK_OFFICER = (SELECT FIRST 1 CO_EFFECTIVE_OFFICER FROM EACOMPANY WHERE I_CO_COMPANYCLIENTID = @CompanyId)" +
					" WHERE i_abank_companyclientid = @CompanyId";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CommonItem.CompanyIdValue));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(FbConnectionString, QueryText, Params, 0);
				}
			}
			
			return Result;
		}
	}
}
