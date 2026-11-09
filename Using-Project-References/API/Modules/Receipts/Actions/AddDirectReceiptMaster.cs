/*
 * Created by SharpDevelop.
 * User: John
 * Date: 01/05/2020
 * Time: 2:50 PM
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
	/// Description of AddDirectReceiptMaster.
	/// </summary>
	public class AddDirectReceiptMaster : ActionBase
	{
		private string FModuleName = "Receipts";
		private string FCategory = "";
		private static bool m_PerformEAMainOffice = true;
		
		public static string Execute(int MasterId,
									 APISettingsItem SettingsItem,
									 string Request,
									 out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			AddDirectReceiptMaster inst = new AddDirectReceiptMaster();
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
				if (m_PerformEAMainOffice)
				{
					UsefulDBMethods.Helpers.RunEAMainOfficeProcess(inst.FbConnectionString, inst.CommonItem.CompanyIdValue);
				}
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
			m_PerformEAMainOffice = true;
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " +Method, FModuleName, MasterId);
			LogOnInsideMethod(Method);
			try 
			{
				WindowsEventLoggingUtilities.WriteToInfoEvent("Category: " + FCategory, FModuleName, MasterId);
				//StringBuilder Entries = new StringBuilder();
				switch (FCategory)
				{
					case "SetupPageEntries":
						Result = AddDirectReceipt.SetupPageEntries.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
						break;
                    case "DisplayReceiptMessage":
                        Result = AddDirectReceipt.DisplayReceiptMessage.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
                        break;
                    case "CancelReceipt":
						Result = AddDirectReceipt.CancelReceipt.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
						break;
					case "SaveReceipt":
						Result = AddDirectReceipt.SaveReceipt.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
						m_PerformEAMainOffice = false;
						break;
					case "Regenerate":
						Result = AddDirectReceipt.Regenerate.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
						break;
					case "StartAgain":
						Result = AddDirectReceipt.StartAgain.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
						break;
					case "PayAll":
						Result = AddDirectReceipt.PayAll.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
						break;
					case "DisplayDebts":
						Result = AddDirectReceipt.DisplayDebts.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
						break;
					case "ChangeIncludeStatus":
						Result = AddDirectReceipt.ChangeIncludeStatus.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
						break;
					case "ValidateReceipt":
						Result = AddDirectReceipt.ValidateReceipt.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
						break;
					case "SubLedgerChange":
						Result = AddDirectReceipt.SubLedgerChange.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
						break;
					case "RevenueCodeChange":
						Result = AddDirectReceipt.RevenueCodeChange.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
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
			WindowsEventLoggingUtilities.WriteToInfoEvent("Leaving: " + Method, FModuleName, MasterId);
			return Result;
		}
	}
}
