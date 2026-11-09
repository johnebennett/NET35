/*
 * Created by SharpDevelop.
 * User: John
 * Date: 28/06/2020
 * Time: 5:08 PM
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
namespace Actions.PortfolioAddButton
{
	/// <summary>
	/// Description of RecurringInvoicesDlgMaster.
	/// </summary>
	public class RecurringInvoicesDlgMaster : ActionBase
	{
		private string FModuleName = "PortfolioAddButton";
		private string FCategory = "";
		
		public static string Execute(int MasterId,
									 APISettingsItem SettingsItem,
									 string Request,
									 out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			RecurringInvoicesDlgMaster inst = new RecurringInvoicesDlgMaster();
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
			string Method = "RecurringInvoicesDlgMaster.Output";
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
						Result = RecurringInvoicesDlg.SetupPageEntries.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
						break;
					case "GenerateCmd":
						Result = RecurringInvoicesDlg.GenerateCmd.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
						break;
					case "SaveCmd":
						Result = RecurringInvoicesDlg.SaveCmd.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
						break;
					case "ActivateCmd":
						Result = RecurringInvoicesDlg.ActivateCmd.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
						break;
					case "DeActivateCmd":
						Result = RecurringInvoicesDlg.DeactivateCmd.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
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
