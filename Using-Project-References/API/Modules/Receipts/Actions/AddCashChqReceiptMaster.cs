/*
 * Created by SharpDevelop.
 * User: John
 * Date: 09/05/2020
 * Time: 12:08 PM
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

using CommonUtilities;
using EAOfficeAPI;
namespace Actions.Receipts
{
	/// <summary>
	/// Description of AddCashChqReceiptMaster.
	/// </summary>
	public class AddCashChqReceiptMaster : ActionBase
	{
		private string FModuleName = "Receipts";
		private string FCategory = "";
		
		public static string Execute(int MasterId,
									 APISettingsItem SettingsItem,
									 string Request,
									 out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			AddCashChqReceiptMaster inst = new AddCashChqReceiptMaster();
			if (!inst.Setup(MasterId, SettingsItem, Request, out ErrorMsg))
				return ErrorMsg;
			
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
				this.FCategory = (string)GetCollectionValue("Data", "Category");
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
				Result = ValidationChecks.ErrorIfEmptyString(FCategory, "Category");
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
			string Method = "AddDirectReceiptMaster.Output";
			string Result = "";
			string ErrorMsg = "";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " +Method, FModuleName, MasterId);
			LogOnInsideMethod(Method);
			try 
			{
				WindowsEventLoggingUtilities.WriteToInfoEvent("Category: " + FCategory, FModuleName, MasterId);
				StringBuilder Entries = new StringBuilder();
				switch (FCategory)
				{
					case "SetupPageEntries":
						Result = AddCashChqReceipt.SetupPageEntries.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
						break;
					case "DisplayDebts":
						Result = AddCashChqReceipt.DisplayDebts.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
						break;
					case "PayAll":
						Result = AddCashChqReceipt.PayAll.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
						break;
					case "CancelReceipt":
						Result = AddCashChqReceipt.CancelReceipt.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
						break;
					case "Regenerate":
						Result = AddCashChqReceipt.Regenerate.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
						break;
					case "StartAgain":
						Result = AddCashChqReceipt.StartAgain.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
						break;
					case "SaveReceipt":
						Result = AddCashChqReceipt.SaveReceipt.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
						break;
					case "ChangeIncludeStatus":
						Result = AddCashChqReceipt.ChangeIncludeStatus.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
						break;
					case "ValidateReceipt":
						Result = AddCashChqReceipt.ValidateReceipt.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
						break;
					case "SubLedgerChange":
						Result = AddCashChqReceipt.SubLedgerChange.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
						break;
					case "RevenueCodeChange":
						Result = AddCashChqReceipt.RevenueCodeChange.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
						break;
					default: break;
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
