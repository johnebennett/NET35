/*
 * Created by SharpDevelop.
 * User: John
 * Date: 06/03/2019
 * Time: 3:33 PM
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
	/// Methods relaing to Contact operations
	/// including Email, SMS etc
	/// </summary>
	public class ContactOperations
	{
		private const string FModuleName = "ContactOpeations";
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
				if (StringUtilities.SameText(CommonItem.Action, "InitEmailItems"))
					result = Actions.ContactOperations.InitEmailItems.Execute(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "UpdateEventRun"))
					result = Actions.ContactOperations.UpdateEventRun.Execute(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "RemoveEmailRecipient"))
					result = Actions.ContactOperations.RemoveEmailRecipient.Execute(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "AddEmailUploadAttachments"))
					result = Actions.ContactOperations.AddEmailUploadAttachments.Execute(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "AddEmailDocumentAttachment"))
					result = Actions.ContactOperations.AddEmailDocumentAttachment.Execute(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "RemoveEmailAttachment"))
					result = Actions.ContactOperations.RemoveEmailAttachment.Execute(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "EmailCategoryOnChange"))
					result = Actions.ContactOperations.EmailCategoryOnChange.Execute(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "EmailTemplateOnChange"))
					result = Actions.ContactOperations.EmailTemplateOnChange.Execute(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "ProcessRun"))
					result = Actions.ContactOperations.ProcessRun.Execute(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg);

				else if (StringUtilities.SameText(CommonItem.Action, "EmailStatements-SetupPageEntries"))
					result = Actions.ContactOperations.EmailStatements.SetupPageEntries.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "EmailStatements-ProcessRun"))
					result = Actions.ContactOperations.EmailStatements.ProcessRun.Execute(MasterId, SettingsItem, Request, out ErrorMsg);

				else if (StringUtilities.SameText(CommonItem.Action, "BulkEmail_SetupPageEntries"))
					result = Actions.ContactOperations.BulkEmail_SetupPageEntries.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "BulkEmail_GatherRecipients"))
					result = Actions.ContactOperations.BulkEmail_GatherRecipients.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "BulkEmail_RemoveRecipient"))
					result = Actions.ContactOperations.BulkEmail_RemoveRecipient.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "BulkEmail_ProcessRun"))
					result = Actions.ContactOperations.BulkEmail_ProcessRun.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "BulkEmail_ProcessRun"))
					result = Actions.ContactOperations.BulkEmail_ProcessRun.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "BulkEmail_AddEmailUploadAttachments"))
					result = Actions.ContactOperations.BulkEmail_AddEmailUploadAttachment.Execute(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg);
				else if (StringUtilities.SameText(CommonItem.Action, "BulkEmail_RemoveEmailAttachment"))
					result = Actions.ContactOperations.BulkEmail_RemoveEmailAttachment.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
                else if (StringUtilities.SameText(CommonItem.Action, "UpdateHtmlTemplate"))
                    result = Actions.ContactOperations.UpdateHtmlTemplate.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
                else if (StringUtilities.SameText(CommonItem.Action, "ConvertRTFTemplates"))
                    result = Actions.ContactOperations.ConvertRTFTemplates.Execute(MasterId, SettingsItem, Request, out ErrorMsg);

                else if (StringUtilities.SameText(CommonItem.Action, "SendJobEmail"))
                    result = Actions.ContactOperations.SendJobEmail.Execute(MasterId, SettingsItem, Request, out ErrorMsg);
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