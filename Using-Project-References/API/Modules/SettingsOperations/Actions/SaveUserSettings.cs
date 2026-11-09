/*
 * Created by SharpDevelop.
 * User: John
 * Date: 26/06/2019
 * Time: 12:04 PM
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
	/// Description of SavePageEntries_UserSettings.
	/// </summary>
	public class SaveUserSettings : ActionBase
	{
		private string FModuleName = "SettingsOperations";
		
		private UserSettingsItem FUserSettingsItem = new UserSettingsItem();
		
		public static string Execute(int MasterId,
		                             RequestCommonItem CommonItem,
		                             JObject DataObject,
		                             APISettingsItem SettingsItem,
		                             out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			SaveUserSettings inst = new SaveUserSettings();
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
				Result = FUserSettingsItem.Load((EAOfficeAPI.ActionBase)this, this.DataObject);
				WindowsEventLoggingUtilities.WriteToInfoEvent("FUserSettingsItem.ManagerIdValue: " + FUserSettingsItem.ManagerIdValue, FModuleName, MasterId);
				WindowsEventLoggingUtilities.WriteToInfoEvent("FUserSettingsItem.OfficeIdValue: " + FUserSettingsItem.OfficeIdValue, FModuleName, MasterId);
				WindowsEventLoggingUtilities.WriteToInfoEvent("FUserSettingsItem.PortfolioIdValue: " + FUserSettingsItem.PortfolioTypeIdValue, FModuleName, MasterId);
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
					Result = FUserSettingsItem.Validate();
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
				WindowsEventLoggingUtilities.WriteToInfoEvent("FUserSettingsItem.ManagerIdValue: " + FUserSettingsItem.ManagerIdValue, FModuleName, MasterId);
				WindowsEventLoggingUtilities.WriteToInfoEvent("FUserSettingsItem.OfficeIdValue: " + FUserSettingsItem.OfficeIdValue, FModuleName, MasterId);
				WindowsEventLoggingUtilities.WriteToInfoEvent("FUserSettingsItem.PortfolioIdValue: " + FUserSettingsItem.PortfolioTypeIdValue, FModuleName, MasterId);
				Result = FUserSettingsItem.Save((ActionBase)this, FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue);
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
	}
}
