/*
 * Created by SharpDevelop.
 * User: John
 * Date: 27/12/2018
 * Time: 12:43 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using CommonUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EAOfficeAPI
{
	/// <summary>
	/// Description of MyClass.
	/// </summary>
	public class Reconcilliation
	{
		private const string FModuleName = "Reconcilliation";
		
		public static string Execute(string Request)
		{
			string Result = "";
			string Msg = "";
			string Dummy = "";
			
			RequestCommonItem Item = RequestCommonProcessor.LoadFromRequest(Request, out Result);
			if (string.IsNullOrEmpty(Result))
			{
				Result = RequestCommonProcessor.ValidateCommonItem(Item);
			}
			
			if (!string.IsNullOrEmpty(Result))
			{
				WindowsEventLoggingUtilities.WriteToInfoEvent("Result: " + Result, FModuleName, 0);
				return "";
			}
			
            int MasterId = 0;
            ProgressLogging.AddRecord(
            	true, 0, Item.Plugin, Item.Command, Item.Action, Request, 
            	JsonContentEntryType.Request, Item.CompanyIdValue, Item.ClientIdValue, 
            	out MasterId, out Result);
			if (!string.IsNullOrEmpty(Result))
			{
				WindowsEventLoggingUtilities.WriteToErrorEvent("After logging the Request. Result: " + Result, FModuleName, 0);
				return "";
			}

			if (!string.IsNullOrEmpty(Result))
			{
				WindowsEventLoggingUtilities.WriteToErrorEvent("Result: " + Result, FModuleName, 0);
				return "";
			}
			
			APISettingsItem SettingsItem = APISettingsFile.GetAPISettings(APIAppType.Service, out Result);
			if (!string.IsNullOrEmpty(Result))
			{
				WindowsEventLoggingUtilities.WriteToErrorEvent("Error Loading API Settings: " + Result, FModuleName, 0);
				return "";
			}
			
			JObject DataObject = null;
			JObject jobj = JsonConvert.DeserializeObject<JObject>(Request);
			if (jobj == null)
			{
				Result = "Error deserializing the Request";
			}
			else
			{
				DataObject = (jobj["data"] as JObject);
				if (DataObject == null)
				{
					Result = "Missing data section in Request.";
				}
			}
			
			if (!string.IsNullOrEmpty(Result))
			{
				WindowsEventLoggingUtilities.WriteToInfoEvent(Result, FModuleName, 0);
				return "";
			}

			bool aLogging = Item.DebugValue;
			
			if (string.IsNullOrEmpty(Result))
			{
				Msg = "Action: " + Item.Action + Environment.NewLine;
				WindowsEventLoggingUtilities.WriteToInfoEvent(Msg, FModuleName, 0);
			}
			
			if (!string.IsNullOrEmpty(Result)) return Result;
			
			
			if (string.IsNullOrEmpty(Result))
			{
				WindowsEventLoggingUtilities.WriteToInfoEvent("Before calling ExecuteMethod. Result: " + Result, FModuleName, MasterId);
				try 
				{
					//result = ExecuteMethod(Request, MasterId, Item.Action, Item.Method);
					Result = ExecuteMethod(MasterId, Item, DataObject, SettingsItem, Request);
					ProgressLogging.AddRecord(
						true, MasterId, Item.Plugin, Item.Command, Item.Method, Result, 
						JsonContentEntryType.Response, Item.CompanyIdValue, Item.ClientIdValue,
						out MasterId, out Dummy);
				} 
				catch (Exception Ex) 
				{
					Result = Ex.Message + Environment.NewLine + Ex.InnerException;
					WindowsEventLoggingUtilities.WriteToInfoEvent("Exception calling ExecuteMethod:" + Environment.NewLine + Result, FModuleName, 0);
				}
				WindowsEventLoggingUtilities.WriteToInfoEvent("Result: " + Result, FModuleName, 0);
				WindowsEventLoggingUtilities.WriteToInfoEvent("After calling ExecuteMethod. Result: " + Result, FModuleName, MasterId);
			}
			
			return Result;
		}
		
		private static string ExecuteMethod(int MasterId, 
		                                    RequestCommonItem CommonItem, 
		                                    JObject DataObject,
		                                    APISettingsItem SettingsItem,
		                                    string Request)
		{
			string result = "";
			string ErrorMsg = "";
			
			if (!string.IsNullOrEmpty(CommonItem.Action))
			{
				if (StringUtilities.SameText(CommonItem.Action, "GetBankAccountList"))
					result = Actions.Reconcilliation.BankAccountList.Execute(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "UpdateBankAccountItem"))
					//result = Actions.Portfolio.SearchResults.Execute(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg);
					result = Actions.Reconcilliation.UpdateBankAccountItem.Execute(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "GatherGridEntries"))
					result = Actions.Reconcilliation.GatherGridEntries.GatherGridEntries.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "GatherReportsGridEntries"))
					result = Actions.Reconcilliation.GatherReportsGridEntries.GatherReportsGridEntries.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "GetReconcilliationReport"))
					result = Actions.Reconcilliation.GetReconcilliationReport.GetReconcilliationReport.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "GatherReconcileGridEntries"))
					result = Actions.Reconcilliation.GatherReconcileGridEntries.GatherReconcileGridEntries.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "GatherSystemReportsGridEntries"))
					result = Actions.Reconcilliation.GatherSystemReportsGridEntries.GatherSystemReportsGridEntries.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "GetSystemReport"))
					result = Actions.Reconcilliation.GetSystemReport.GetSystemReport.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "PerformEOM"))
					result = Actions.Reconcilliation.PerformEOM.PerformEOM.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "SavePageNumber"))
					result = Actions.Reconcilliation.SavePageNumber.SavePageNumber.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "PresentUnpresent"))
					result = Actions.Reconcilliation.PresentUnpresent.PresentUnpresent.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "GenerateBankStmtRpt"))
					result = Actions.Reconcilliation.GenerateBankStmtRpt.GenerateBankStmtRpt.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "GenerateReconcileRpt"))
					result = Actions.Reconcilliation.GenerateReconcileRpt.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
			}
			
			return result;
		}
	}
}