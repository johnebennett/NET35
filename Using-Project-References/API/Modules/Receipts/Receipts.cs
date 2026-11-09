/*
 * Created by SharpDevelop.
 * User: John
 * Date: 20/04/2020
 * Time: 3:56 PM
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
	public class Receipts
	{
		private const string FModuleName = "Receipts";
		
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
				if (StringUtilities.SameText(CommonItem.Action, "GatherReceipts"))
					result = Actions.Receipts.GatherReceipts.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "CreateOptions"))
					result = Actions.Receipts.CreateOptionsMaster.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "EventProcessOptions"))
					result = Actions.Receipts.EventProcessOptionsMaster.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "DisplayPastEventsReport"))
					result = Actions.Receipts.DisplayPastEventsReport.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "CreateDirectDebitFile"))
					result = Actions.Receipts.EventProcessOptions.CreateDirectDebitFile.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "AddDirectReceipt"))
					result = Actions.Receipts.AddDirectReceiptMaster.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "ReceiptScreenChargePopup"))
					result = Actions.Receipts.ReceiptScreenChargePopup.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "ScreenChargePopupEditPayingAmount"))
					result = Actions.Receipts.ScreenChargePopupEditPayingAmount.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "AddCashChqReceipt"))
					result = Actions.Receipts.AddCashChqReceiptMaster.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "SetupFindDialog"))
					result = Actions.Receipts.SetupFindDialog.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "CheckOKToReceipt"))
					result = Actions.Receipts.CheckOKToReceipt.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
                else if (StringUtilities.SameText(CommonItem.Action, "DisplayGridReport"))
                    result = Actions.Receipts.DisplayGridReport.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
                else if (StringUtilities.SameText(CommonItem.Action, "ProcessEmail"))
                    result = Actions.Receipts.ProcessEmail.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
                else if (StringUtilities.SameText(CommonItem.Action, "RemoveGridEntry"))
                    result = Actions.Receipts.RemoveGridEntry.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
                /*
				else if (StringUtilities.SameText(CommonItem.Action, "Debts_ShowReportItem"))
					result = Actions.Debts.Debts_ShowReportItem.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				*/
                /*
				else if (StringUtilities.SameText(CommonItem.Action, "BusinessContacts"))
					result = Actions.Portfolio.GetBusinessContacts.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				*/
            }

            return result;
		}
	}
}