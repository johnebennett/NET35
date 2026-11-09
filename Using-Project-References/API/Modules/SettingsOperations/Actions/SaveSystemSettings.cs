/*
 * Created by SharpDevelop.
 * User: John
 * Date: 23/07/2020
 * Time: 9:25 AM
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
	/// Description of SaveSystemSettings.
	/// </summary>
	public class SaveSystemSettings : ActionBase
	{
		private string FModuleName = "SettingsOperations";
        private SaveSystemSettingsRequestItem FRequestItem;

        public static string Execute(int MasterId,
		                             RequestCommonItem CommonItem,
		                             JObject DataObject,
		                             APISettingsItem SettingsItem,
		                             out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			SaveSystemSettings inst = new SaveSystemSettings();
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
                DataCollection = CommonMethods.MakeCollectionFromJObject(DataObject, out Result);
                FRequestItem = new SaveSystemSettingsRequestItem();
                Result = FRequestItem.Load((ActionBase)this);
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
				Result = SaveSystemSettingsItems();
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

        private string SaveSystemSettingsItems()
        {
            string Result = "";
            string Method = "SaveSettings";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, this.MasterId);

            LogOnInsideMethod(Method);
            try
            {
                string QueryText = "";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText =
                       " UPDATE eacompanysettings2" +
                       " SET I_CO_SALECOMMTO = @SaleCommTo," +
                       "     I_CO_LETTINGFEECHARTID = @ResLettingFeeExpense," +
                       "     I_CO_LETTINGFEECHARTIDREV = @ResLettingFeeRevenue," +
                       "     I_CO_ADVERTISINGCHARTID = @AdvertisingExpense," +
                       "     I_CO_ADVERTISINGCHARTIDREV = @AdvertisingRevenue," +
                       "     I_CO_LFEECHARTIDCOMEXP = @CommercialLettingFeeExpense," +
                       "     I_CO_LFEECHARTIDCOMREV = @CommercialLettingFeeRevenue" +
                       " WHERE I_COMPANYSETTINGS2_COMPANYID = @CompanyId";
                    Params.Add(new FbParameterItem("SaleCommTo", FRequestItem.SaleCommToLedger));
                    Params.Add(new FbParameterItem("ResLettingFeeExpense", FRequestItem.ResLettingFeeExpense));
                    Params.Add(new FbParameterItem("ResLettingFeeRevenue", FRequestItem.ResLettingFeeRevenue));
                    Params.Add(new FbParameterItem("AdvertisingExpense", FRequestItem.AdvertisingExpense));
                    Params.Add(new FbParameterItem("AdvertisingRevenue", FRequestItem.AdvertisingRevenue));
                    Params.Add(new FbParameterItem("CommercialLettingFeeExpense", FRequestItem.CommercialLettingFeeExpense));
                    Params.Add(new FbParameterItem("CommercialLettingFeeRevenue", FRequestItem.CommercialLettingFeeRevenue));
                    Params.Add(new FbParameterItem("CompanyId", CommonItem.CompanyIdValue));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, MasterId);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = FirebirdUtilities.DBExecute(FbConnectionString, QueryText, Params, 0);
                }

                QueryText = "";
                Params.Clear();
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText =
                       " UPDATE eacompanysettings" +
                       " SET B_CO_AUTOPOSTAPPROVED = @AutoPostApproved" +
                       " WHERE I_SETTINGS_COMPANYID = @CompanyId";
                    Params.Add(new FbParameterItem("AutoPostApproved", FRequestItem.AutoPostApproved));
                    Params.Add(new FbParameterItem("CompanyId", CommonItem.CompanyIdValue));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, MasterId);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = FirebirdUtilities.DBExecute(FbConnectionString, QueryText, Params, 0);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    QueryText = "UPDATE EACompany SET B_CO_USE2FA = @Use2FA WHERE I_CO_CompanyCLientId = @CompanyId";
                    Params.Clear();
                    Params.Add(new FbParameterItem("Use2FA", FRequestItem.Use2FA));
                    Params.Add(new FbParameterItem("CompanyId", CommonItem.CompanyIdValue));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, MasterId);
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

            return Result;

        }
    }
}
