/*
 * Created by SharpDevelop.
 * User: John
 * Date: 21/03/2020
 * Time: 4:49 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using CommonUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

//using Module.Portfolio.Actions;
//using Actions.Debits;

namespace EAOfficeAPI
{
	/// <summary>
	/// Description of MyClass.
	/// </summary>
	public class Debts
	{
		private const string FModuleName = "Debts";
		
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
				if (StringUtilities.SameText(CommonItem.Action, "GatherDebts"))
					result = Actions.Debts.GatherDebts.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "Debts_ShowReportItem"))
					result = Actions.Debts.Debts_ShowReportItem.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "Debts_UndoRedo"))
					result = Actions.Debts.Debts_UndoRedo.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "Debts_ReportAction"))
					result = Actions.Debts.Debts_ReportAction.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "Debts_PastEventsAction"))
					result = Actions.Debts.Debts_PastEventsAction.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "Debts_UndoRedoNonGrid"))
					result = Actions.Debts.Debts_UndoRedoNonGrid.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "Debts_ClearSelections"))
					result = Actions.Debts.Debts_ClearSelections.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "Debts_ChargeInterest"))
					result = Actions.Debts.Debts_ChargeInterest.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "Debts_SetupBulkEmail"))
					result = Actions.Debts.Debts_SetupBulkEmail.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				/*
				else if (StringUtilities.SameText(CommonItem.Action, "SearchResultsOld"))
					result = Actions.Portfolio.PortfolioSelectEntryItems.Execute(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "PortfolioSelectEntryItems"))
					result = Actions.Portfolio.PortfolioSelectEntryItems.Execute(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "RefreshPortfolioTypeEntries"))
					result = Actions.Portfolio.RefreshPortfolioTypeEntries.Execute(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "ProcessReport"))
					result = Actions.Portfolio.ProcessReport.Execute(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "DisplayReport"))
					result = Actions.Portfolio.DisplayReport.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "OwnersCertificateDialogContent"))
					result = Actions.Portfolio.OwnersCertificateDialogContent.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "UpdateOCCertificateValues"))
					result = Actions.Portfolio.UpdateOCCertificateValues.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "DisplayOCCertificateReport"))
					result = Actions.Portfolio.DisplayOCCertificateReport.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "DisplayMgmtPdfReport"))
					result = Actions.Portfolio.DisplayMgmtPdfReport.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "DisplayStmtKitReport"))
					result = Actions.Portfolio.DisplayStmtKitReport.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "IncludeStmtKitForAccount"))
					result = Actions.Portfolio.IncludeStmtKitForAccount.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "IncludeStmtKitForAccountReport"))
					result = Actions.Portfolio.IncludeStmtKitForAccountReport.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "DisplayResBondsReport"))
					result = Actions.Portfolio.DisplayResBondsReport.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "ProcessResBondsDirectDebit"))
					result = Actions.Portfolio.ProcessResBondsDirectDebit.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "ProcessResBondsBPay"))
					result = Actions.Portfolio.ProcessResBondsBPay.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "CommBondsTransfer"))
					result = Actions.Portfolio.CommBondsTransfer.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "CommBondsApplyChanges"))
					result = Actions.Portfolio.CommBondsApplyChanges.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
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