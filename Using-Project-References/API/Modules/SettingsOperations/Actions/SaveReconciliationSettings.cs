/*
 * Created by SharpDevelop.
 * User: John
 * Date: 16/10/2019
 * Time: 3:00 PM
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
	/// Description of SaveReconciliationSettings.
	/// </summary>
	public class SaveReconciliationSettings : ActionBase
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

			SaveReconciliationSettings inst = new SaveReconciliationSettings();
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
				Result = SaveReconciliationSettingsItems();
				
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
		
		private string SaveReconciliationSettingsItems()
		{
			string Result = "";
			int IntValue = 0;
			string StrValue = "";
			
			if (string.IsNullOrEmpty(Result))
			{
				StrValue = (string)DataObject["bankAccountTypes"];
				IntValue = StringUtilities.StrToIntDef(StrValue, -1);
				if (IntValue < 0)
				{
					Result = "Invalid value passed for Bank Account Types";
				}
				else
				{
					Result =
						SaveBankAccountTypes(
							FbConnectionString, CommonItem.CompanyIdValue, IntValue);
				}
			}
			
			if (string.IsNullOrEmpty(Result))
			{
				StrValue = (string)DataObject["EndOfMonthInProgress"];
				Result = SaveDisplayEOMSetting(FbConnectionString, CommonItem.CompanyIdValue, StrValue);
			}
			return Result;
		}
		
		private string SaveBankAccountTypes(string ConnectionString,
		                                    Int64 CompanyId,
		                                    int Value)
		{
			string Result = "";
			string Method = "SaveBankAccountTypes";
			string QueryText =
				"UPDATE EACOMPANYSETTINGS SET B_CO_ACTIVEBANKACCOUNT = @Value WHERE I_Settings_CompanyId = @CompanyId";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("Value", Value));
			Params.Add(new FbParameterItem("CompanyId", CompanyId));
			Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
			if (string.IsNullOrEmpty(Result))
			{
				Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
			}
			
			return Result;
		}
		
		private string SaveDisplayEOMSetting(string ConnectionString,
		                                     Int64 CompanyId,
		                                     string Value)
		{
			string Result = "";
			string Method = "SaveDisplayEOMSetting";
			string QueryText =
				"UPDATE EACOMPANY SET B_CO_ENDOFMONTHINPROGRESS = @Value WHERE I_CO_COMPANYCLIENTID = @CompanyId";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("Value", Value));
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
