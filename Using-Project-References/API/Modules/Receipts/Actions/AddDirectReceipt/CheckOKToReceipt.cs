/*
 * Created by SharpDevelop.
 * User: John
 * Date: 02/05/2020
 * Time: 6:49 PM
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
namespace Actions.Receipts.AddDirectReceipt
{
	/// <summary>
	/// Description of CheckOKToReceipt.
	/// </summary>
	public class CheckOKToReceipt : ActionBase
	{
		private string FModuleName = "Receipts";
		private CheckOKToReceiptRequestItem FRequestItem;		
		
		public static string Execute(int MasterId,
									 APISettingsItem SettingsItem,
									 string Request,
									 out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			CheckOKToReceipt inst = new CheckOKToReceipt();
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
				FRequestItem = new CheckOKToReceiptRequestItem();
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
				
				if (string.IsNullOrEmpty(Result))
				{
					if (FRequestItem.ReceiptType == 0) Result = "Invalid Receipt Type provided";
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					if (FRequestItem.FindId <= 0) Result = "Invalid Find ID provided";
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
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " + Method, FModuleName, MasterId);
			LogOnInsideMethod(Method);
			try 
			{
				string OKToReceipt = "";
				string OKToReceiptMessage = "";
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg = 
						AddDirectReceipt.CommonMethods.EABankAddReceiptCheck(
							FbConnectionString, CommonItem.CompanyIdValue, FRequestItem.FindId, out OKToReceipt, out OKToReceiptMessage);
					if (string.IsNullOrEmpty(ErrorMsg) && !string.IsNullOrEmpty(OKToReceiptMessage)) ErrorMsg = OKToReceiptMessage;
					if (string.IsNullOrEmpty(ErrorMsg) && (OKToReceipt != "Y")) ErrorMsg = "Not OK to receipt";
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg =
						AddDirectReceipt.CommonMethods.EAReceiptDirectPrepare(
							FbConnectionString, CommonItem.CompanyIdValue, FRequestItem.MasterEventId, FRequestItem.FindId, CommonItem.ClientIdValue);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Result = LogResponse(Method, "");
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
