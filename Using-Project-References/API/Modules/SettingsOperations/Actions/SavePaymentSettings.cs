/*
 * Created by SharpDevelop.
 * User: John
 * Date: 15/07/2019
 * Time: 5:27 PM
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
	/// Description of SavePaymentSettings.
	/// </summary>
	public class SavePaymentSettings : ActionBase
	{
		private string FModuleName = "SettingsOperations";
		
		private PaymentSettingsItem FPaymentSettingsItem = new PaymentSettingsItem();
		
		public static string Execute(int MasterId,
		                             RequestCommonItem CommonItem,
		                             JObject DataObject,
		                             APISettingsItem SettingsItem,
		                             out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			SavePaymentSettings inst = new SavePaymentSettings();
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
				Result = FPaymentSettingsItem.Load((EAOfficeAPI.ActionBase)this, this.DataObject);
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
					Result = FPaymentSettingsItem.Validate();
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
				/*
				Result = FPaymentSettingsItem.Save((ActionBase)this, FbConnectionString, CommonItem.CompanyIdValue);
				if (string.IsNullOrEmpty(Result)) Result = LogResponse(Method, "");
				else Result = LogOnProcessingError(Method, Result, "");
				*/
				Result = FPaymentSettingsItem.Save((ActionBase)this, FbConnectionString, CommonItem.CompanyIdValue);
				if (string.IsNullOrEmpty(Result)) 
				{
					// Update the values for the singlePaymentItems entries
					Result = UpdateSinglePaymentItems();
				}
				
				if (string.IsNullOrEmpty(Result)) Result = LogResponse(Method, "");
				else Result = LogOnProcessingError(Method, Result, "");
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
		
		private string UpdateSinglePaymentItems()
		{
			string Result = "";
			string Method = "UpdateSinglePaymentItems";
			string QueryText = "";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " + Method, FModuleName, MasterId);
			LogOnInsideMethod(Method);
			try 
			{
				// EACompanySettings table items
				QueryText = 
					" UPDATE EACompanySettings" +
					" SET B_CO_HOLDFORUNPAIDINVOICES = @HOLDFORUNPAIDINVOICES," +
					"     B_CO_UNCLEAREDFUNDS = @UNCLEAREDFUNDS," +
					"     B_CO_ZEROBALANCESUBACCOUNTS = @ZEROBALANCESUBACCOUNTS," +
					"     B_CO_EXCLUDEHOLDOVERFROMSTMT = @EXCLUDEHOLDOVERFROMSTMT," +
					"     B_CO_PRINTFUTUREINVOICES = @PRINTFUTUREINVOICES," +
					"     B_CO_DONOTSHOWZEROPAIDZEROOWING = @DONOTSHOWZEROPAIDZEROOWING" +
					" WHERE I_SETTINGS_COMPANYID = @CompanyId";
				Params.Clear();
				Params.Add(new FbParameterItem("HOLDFORUNPAIDINVOICES", (string)DataObject["HoldForUnpaidInvoices"]));
				Params.Add(new FbParameterItem("UNCLEAREDFUNDS", (string)DataObject["HoldForUnclearedFunds"]));
				Params.Add(new FbParameterItem("ZEROBALANCESUBACCOUNTS", (string)DataObject["IncludeZeroBalanceLedgersCommercial"]));
				Params.Add(new FbParameterItem("EXCLUDEHOLDOVERFROMSTMT", (string)DataObject["ExcludeLedgerWhereBalanceEqualsHoldoverFromPreviousStatement"]));
				Params.Add(new FbParameterItem("PRINTFUTUREINVOICES", (string)DataObject["PrintUnpaidInvoicesList"]));
				Params.Add(new FbParameterItem("DONOTSHOWZEROPAIDZEROOWING", (string)DataObject["DoNotShowZeroPaidZeroOwing"]));
				Params.Add(new FbParameterItem("CompanyId", CommonItem.CompanyIdValue));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(FbConnectionString, QueryText, Params, 0);
				}
				
				// EACompanySettings2 table items
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = 
						" UPDATE EACompanySettings2" +
						" SET I_CO_MINAMT = @MINAMT" +
						" WHERE I_COMPANYSETTINGS2_COMPANYID = @CompanyId";
					Params.Clear();
					Params.Add(new FbParameterItem("MINAMT", (string)DataObject["StmtMinValue"]));
					Params.Add(new FbParameterItem("CompanyId", CommonItem.CompanyIdValue));
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
				}
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(FbConnectionString, QueryText, Params, 0);
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
	}
}
