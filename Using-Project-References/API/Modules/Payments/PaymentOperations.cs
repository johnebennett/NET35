/*
 * Created by SharpDevelop.
 * User: John
 * Date: 17/06/2019
 * Time: 6:16 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

using CommonUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System.Reflection;
using System.Reflection.Emit;

namespace EAOfficeAPI
{
	/// <summary>
	/// Methods relaing to Payment operations
	/// </summary>
	public class PaymentOperations
	{
		private const string FModuleName = "PaymentOperations";
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
				/* Payments Actions */
				if (StringUtilities.SameText(CommonItem.Action, "InitPageEntries"))
					result = Actions.PaymentOperations.InitPageEntries.Execute(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "GetSearchGridResults"))
					result = Actions.PaymentOperations.GetSearchGridResults.Execute(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "ViewPaymentRunId"))
					result = Actions.PaymentOperations.GetSearchGridResults.Execute(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg);
				
				/* Payment Actions */
				else if (StringUtilities.SameText(CommonItem.Action, "GetPaymentSearchGridResults"))
					result = Actions.PaymentOperations.GetPaymentSearchGridResults.Execute(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "RefreshStartingChqNo"))
					result = Actions.PaymentOperations.StatementNextChqNumber.Execute(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "CancelPreview"))
					result = Actions.PaymentOperations.StatementCancelPreview.Execute(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "ReverseRunEntry"))
					//result = Actions.PaymentOperations.ReverseRunEntry.Execute(MasterId, CommonItem, DataObject, SettingsItem, Request, out ErrorMsg);
					result = Actions.PaymentOperations.ReverseRunEntry.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "PostPreview"))
					result = Actions.PaymentOperations.PostPreview.Execute(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "PaymentAttachmentList"))
					result = Actions.PaymentOperations.PaymentAttachmentList.Execute(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg);
				/*
				else if (StringUtilities.SameText(CommonItem.Action, "PaymentListReport"))
					result = Actions.PaymentOperations.PaymentListReport.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				*/
				else if (StringUtilities.SameText(CommonItem.Action, "ProcessPreviewReport"))
					result = Actions.PaymentOperations.ProcessPreviewReport.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "CreatePaymentFile"))
					result = Actions.PaymentOperations.CreatePaymentFile.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				
				/* New Payment Run Actions - Removed
				else if (StringUtilities.SameText(CommonItem.Action, "InitNewPaymentRunEntries"))
					result = Actions.PaymentOperations.InitNewPaymentRunEntries.Execute(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "ValidateNewPaymentRun"))
					result = Actions.PaymentOperations.ValidateNewPaymentRun.Execute(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "NewPaymentRunApply"))
					result = Actions.PaymentOperations.NewPaymentRunApply.Execute(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg);
				*/
				
				/* New Payment Preview Actions */
				else if (StringUtilities.SameText(CommonItem.Action, "InitNewPaymentPreviewEntries"))
					result = Actions.PaymentOperations.InitNewPaymentPreviewEntries.Execute(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "NewPaymentPreviewApply"))
					result = Actions.PaymentOperations.NewPaymentPreviewApply.Execute(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "NewPaymentPreviewGather"))
					result = Actions.PaymentOperations.NewPaymentPreviewGather.Execute(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "NewPaymentPreviewStartAgain"))
					result = Actions.PaymentOperations.NewPaymentPreviewStartAgain.Execute(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "NewPaymentPreviewAcceptPreview"))
					result = Actions.PaymentOperations.NewPaymentPreviewAcceptPreview.Execute(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "NewPaymentPreviewRemoveEntry"))
					result = Actions.PaymentOperations.NewPaymentPreviewRemoveEntry.Execute(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "NewPaymentPreviewGridRpt"))
					result = Actions.PaymentOperations.NewPaymentPreviewGridRpt.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				
				else if (StringUtilities.SameText(CommonItem.Action, "BuildReportListForReportId"))
					result = Actions.PaymentOperations.ProcessPreviewReport.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "DisplayPmtPdfReport"))
					result = Actions.PaymentOperations.DisplayPmtPdfReport.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
                else if (StringUtilities.SameText(CommonItem.Action, "DisplayPaymentsGridReport"))
                    result = Actions.PaymentOperations.DisplayPaymentsGridReport.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
                else
                {
                    WindowsEventLoggingUtilities.WriteToErrorEvent("No implementation for the action: " + CommonItem.Action, FModuleName, MasterId);
                    result = new ActionBase().LogOnProcessingError(FModuleName, "No implementation for the action: " + CommonItem.Action, "");
                }
            }

            return result;
		}
	}
}