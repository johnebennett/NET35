/*
 * Created by SharpDevelop.
 * User: John
 * Date: 29/04/2020
 * Time: 12:29 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Diagnostics;

using Newtonsoft.Json.Linq;
using CommonUtilities;
using EAOfficeAPI;
namespace Actions.Receipts.EventProcessOptions
{
	/// <summary>
	/// Description of RemoveReceiptOptionsAction.
	/// </summary>
	public class RemoveReceiptOptionsAction : ActionBase
	{
		private string FModuleName = "Receipts";
		private EventProcessOptions_OnChangeRequestItem FRequestItem;		
		
		public static string Execute(int MasterId,
									 APISettingsItem SettingsItem,
									 string Request,
									 out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			RemoveReceiptOptionsAction inst = new RemoveReceiptOptionsAction();
			if (!inst.Setup(MasterId, SettingsItem, Request, out ErrorMsg))
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
				UsefulDBMethods.Helpers.RunEAMainOfficeProcess(inst.FbConnectionString, inst.CommonItem.CompanyIdValue);
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
				FRequestItem = new EventProcessOptions_OnChangeRequestItem();
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
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " + Method, FModuleName, MasterId);
			LogOnInsideMethod(Method);
			try 
			{
				Result = FRequestItem.Validate();
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
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " + Method, FModuleName, MasterId);
			LogOnInsideMethod(Method);
			try 
			{
				StringBuilder Entries = new StringBuilder();
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg =
						CommonMethods.EAReceiptScreenGroup2(
							FbConnectionString, CommonItem.CompanyIdValue, FRequestItem.MasterEventId, CommonItem.ClientIdValue, 
							FRequestItem.PastEventId, FRequestItem.ItemId, ref Entries);
				}
				
				string ShowCancelButton = "";
				string CancelSql = "";
				string CancelSql2 = "";
				string Path = "";
				if (string.IsNullOrEmpty(ErrorMsg))					
				{
					Path = "$.ShowCancelButton";
					ShowCancelButton = JsonUtilities.GetJsonPathValueStr(Entries.ToString(), Path, out ErrorMsg);
					if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(ShowCancelButton))
						ErrorMsg = "Cannot find the Show Cancel Button value";
					if (string.IsNullOrEmpty(ErrorMsg) && (ShowCancelButton == "N"))
						ErrorMsg = "This item cannot be removed";
					if (string.IsNullOrEmpty(ErrorMsg))
					{
						Path = "$.CancelSQL";
						CancelSql = JsonUtilities.GetJsonPathValueStr(Entries.ToString(), Path, out ErrorMsg);
					if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(CancelSql))
						ErrorMsg = "Cannot find the Cancel SQL value";
					}
					if (string.IsNullOrEmpty(ErrorMsg))
						ErrorMsg = FirebirdUtilities.DBExecute(FbConnectionString, CancelSql);

					if (string.IsNullOrEmpty(ErrorMsg))
					{
						Path = "$.CancelSQL2";
						CancelSql2 = JsonUtilities.GetJsonPathValueStr(Entries.ToString(), Path, out ErrorMsg);
					if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(CancelSql2))
						ErrorMsg = "Cannot find the second Cancel SQL value";
					}
					if (string.IsNullOrEmpty(ErrorMsg))
						ErrorMsg = FirebirdUtilities.DBExecute(FbConnectionString, CancelSql2);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Entries.Length = 0;
					FRequestItem.ItemId = 0; // the original entry has been removed so we can't use it here
					ErrorMsg =
						CommonMethods.EAReceiptScreenGroup2(
							FbConnectionString, CommonItem.CompanyIdValue, FRequestItem.MasterEventId, CommonItem.ClientIdValue, 
							FRequestItem.PastEventId, FRequestItem.ItemId, ref Entries);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Result =
						JsonUtilities.StartJson() +
						Entries.ToString() +
						JsonUtilities.EndJson();
				}
				else
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
