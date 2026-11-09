/*
 * Created by SharpDevelop.
 * User: John
 * Date: 21/05/2020
 * Time: 12:11 PM
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
namespace Actions.Invoices
{
	/// <summary>
	/// Description of AddInvoiceMaster.
	/// </summary>
	public class AddInvoiceMaster : ActionBase
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

			AddInvoiceMaster inst = new AddInvoiceMaster();
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
				//UsefulDBMethods.Helpers.RunEAMainOfficeProcess(inst.FbConnectionString, inst.CommonItem.CompanyIdValue);
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
						Result = AddInvoice.SetupPageEntries.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
						break;
					case "ShowDetails":
						Result = AddInvoice.ShowDetails.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
						break;
					case "AddToList":
						Result = AddInvoice.AddToList.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
						break;
					case "InvoicePayableDetails":
						Result = AddInvoice.InvoicePayableDetails.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
						break;
					case "ChargeTransactionChecked":
						Result = AddInvoice.ChargeTransactionChecked.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
						break;
					case "ExpenseChange":
						Result = AddInvoice.ExpenseChange.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
						break;
					case "InstalmentChange":
						Result = AddInvoice.ExpenseChange.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
						break;
					case "RemoveGridEntry":
						Result = AddInvoice.RemoveGridEntry.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
						break;
					case "ScannedInvoicesChange":
						Result = AddInvoice.ScannedInvoicesChange.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
						break;
					case "Post":
						Result = AddInvoice.Post.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
						break;
					case "Delete":
						Result = AddInvoice.Delete.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
						break;
					case "DisplayReport":
						Result = AddInvoice.DisplayReport.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
						break;
					case "ApplyChanges":
						Result = AddInvoice.ApplyChanges.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
						break;
                    case "DeleteScannedInvoices":
                        Result = AddInvoice.DeleteScannedInvoices.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
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
