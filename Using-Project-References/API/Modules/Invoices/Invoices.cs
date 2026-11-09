	/*
 * Created by SharpDevelop.
 * User: John
 * Date: 23/12/2019
 * Time: 11:16 AM
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
	public class Invoices
	{
		private const string FModuleName = "Invoices";
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
				if (StringUtilities.SameText(CommonItem.Action, "SetupPageEntries"))
					result = Actions.Invoices.SetupPageEntries.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "GatherInvoices"))
					result = Actions.Invoices.GatherInvoices.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "InvoicesOnChange"))
					result = Actions.Invoices.GatherInvoices.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "InvoicesTransfer"))
					result = Actions.Invoices.InvoicesTransfer.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "InvoicesReverseTransfer"))
					result = Actions.Invoices.InvoicesReverseTransfer.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "InvoicesShowReportItem"))
					result = Actions.Invoices.GetSystemReport.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "InvoicesDisplayTypeReport"))
					result = Actions.Invoices.InvoicesGetDisplayTypeReport.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "InvoicesReverseCheck"))
					result = Actions.Invoices.InvoicesReverseCheck.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "InvoicesReverseProcess"))
					result = Actions.Invoices.InvoiceReverseProcess.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "AddScannedInvoices"))
					result = Actions.Invoices.AddScannedInvoices.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "AddInvoice"))
					result = Actions.Invoices.AddInvoiceMaster.Execute(MasterId, SettingsItem, Request, out ErrorMsg);

				else WindowsEventLoggingUtilities.WriteToErrorEvent("No implementation for the action: " + CommonItem.Action, FModuleName, MasterId);
			}
			
			return result;
		}
	}
}