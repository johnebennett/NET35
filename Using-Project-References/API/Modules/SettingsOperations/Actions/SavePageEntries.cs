/*
 * Created by SharpDevelop.
 * User: John
 * Date: 26/06/2019
 * Time: 11:50 AM
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

namespace Actions.SettingsOperations
{
	/// <summary>
	/// Description of SavePageEntries.
	/// </summary>
	public class SavePageEntries : ActionBase
	{
		private string FModuleName = "SettingsOperations";
		
		private string FSection = "";
		private APISettingsItem FSettingsItem = null;
		
		public static string Execute(int MasterId,
		                             RequestCommonItem CommonItem,
		                             JObject DataObject,
		                             APISettingsItem SettingsItem,
		                             out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			SavePageEntries inst = new SavePageEntries();
			if (!inst.Setup(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg))
				return "";
			
			inst.FSettingsItem = SettingsItem;
			
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
				FSection = (string)DataObject["section"];
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
			string Key = "";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " +Method, FModuleName, MasterId);
			LogOnInsideMethod(Method);
			try 
			{
				string BaseFormat = "{0} is {1}";
				string NotProvided = "Not Provided";
				//string Invalid = "Invalid";				
				
				if (string.IsNullOrEmpty(Result))
				{
					Key = "Section";
					if (string.IsNullOrEmpty(FSection)) Result = string.Format(BaseFormat, Key, NotProvided);
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
			string ErrorMsg = "";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " +Method, FModuleName, MasterId);
			LogOnInsideMethod(Method);
			try 
			{
				switch (FSection)
				{
					case "UserSettings": 
						Result = 
							SettingsOperations.SaveUserSettings.Execute(
								MasterId, CommonItem, DataObject, FSettingsItem, out ErrorMsg);
						break;
					case "PaymentSettings": 
						Result = 
							SettingsOperations.SavePaymentSettings.Execute(
								MasterId, CommonItem, DataObject, FSettingsItem, out ErrorMsg);
						break;
					case "TemplateSettings":
						Result =
							SettingsOperations.SaveTemplateSettings.Execute(
								MasterId, CommonItem, DataObject, FSettingsItem, out ErrorMsg);
						break;
					case "ChartSettings":
						Result =
							SettingsOperations.SaveChartSettings.Execute(
								MasterId, CommonItem, DataObject, FSettingsItem, out ErrorMsg);
						break;
					case "BankSettings":
						Result =
							SettingsOperations.SaveBankSettings.Execute(
								MasterId, CommonItem, DataObject, FSettingsItem, out ErrorMsg);
						break;
					case "ReconciliationSettings":
						Result =
							SettingsOperations.SaveReconciliationSettings.Execute(
								MasterId, CommonItem, DataObject, FSettingsItem, out ErrorMsg);
						break;
					case "ResBonds":
						Result =
							SettingsOperations.SaveResidentialBonds.Execute(
								MasterId, CommonItem, DataObject, FSettingsItem, out ErrorMsg);
						break;
					case "Commercial":
						Result =
							SettingsOperations.SaveCommercialSettings.Execute(
								MasterId, CommonItem, DataObject, FSettingsItem, out ErrorMsg);
						break;
					case "ChartOfAccountsAddEdit":
						Result =
							SettingsOperations.SaveChartOfAccountEdit.Execute(
								MasterId, CommonItem, DataObject, FSettingsItem, out ErrorMsg);
						break;
					case "SystemSettings":
						Result =
							SettingsOperations.SaveSystemSettings.Execute(
								MasterId, CommonItem, DataObject, FSettingsItem, out ErrorMsg);
						break;
                    case "ReceiptSettings":
                        Result =
                            SettingsOperations.SaveReceiptSettings.Execute(
                                MasterId, CommonItem, DataObject, FSettingsItem, out ErrorMsg);
                        break;
                    default: ErrorMsg = "Unknown Section: " + FSection; break;
				}
				
				if (!string.IsNullOrEmpty(ErrorMsg))
				{
					Result = LogOnProcessingError(Method, ErrorMsg, "");
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
