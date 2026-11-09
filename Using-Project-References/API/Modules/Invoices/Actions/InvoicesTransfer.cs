/*
 * Created by SharpDevelop.
 * User: John
 * Date: 13/01/2020
 * Time: 4:38 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text;
using System.Data;
using System.Collections.Generic;

using Actions.Invoices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CommonUtilities;
using EAOfficeAPI;

namespace Actions.Invoices
{
	/// <summary>
	/// Description of InvoicesTransfer.
	/// </summary>
	public class InvoicesTransfer : ActionBase
	{
		private string FModuleName = "Invoices";
		private InvoicesTransferRequestItem FRequestItem;
		
		public static string Execute(int MasterId,
		                             APISettingsItem SettingsItem,
		                             string Request,
		                             out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			InvoicesTransfer inst = new InvoicesTransfer();
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
				FRequestItem = new InvoicesTransferRequestItem();
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
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " +Method, FModuleName, MasterId);
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
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " +Method, FModuleName, MasterId);
			LogOnInsideMethod(Method);
			try 
			{
				Result = BuildOutput(out ErrorMsg);
				if (!string.IsNullOrEmpty(ErrorMsg))
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
		
		private string BuildOutput(out string ErrorMsg)
		{
			ErrorMsg = "";
			string Result = "";
			string Method = "BuildOutput";
			
			LogOnInsideMethod(Method);
			try
			{
				WindowsEventLoggingUtilities.WriteToInfoEvent("Inside BuildOutput", FModuleName, MasterId);

                ErrorMsg = UsefulDBMethods.EventHelpers.SetEndOfMonthInProgress(FbConnectionString, CommonItem.CompanyIdValue, "I");

                StringBuilder Entries = new StringBuilder();
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg = CommonMethods.EAInvoicePayCheck(FbConnectionString, CommonItem.CompanyIdValue);
				}
				
				string SqlString = "";
				if (FRequestItem.PassedId == 0)
				{
					SqlString = 
						CommonMethods.GetInvoicesGridSql(
							FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue,
							FRequestItem.BankId, FRequestItem.BranchId, FRequestItem.ManagerId, FRequestItem.DisplayType,
							FRequestItem.TransactionId, FRequestItem.AccountId, FRequestItem.ExpenseId, 
							FRequestItem.DateFrom, FRequestItem.DateTo, FRequestItem.AssignedToId, FRequestItem.CreditorId, 
							FRequestItem.LedgerType, out ErrorMsg);
					WindowsEventLoggingUtilities.WriteToInfoEvent("SqlString:" + Environment.NewLine + SqlString, FModuleName, MasterId);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg) && (FRequestItem.PassedId > 0))
				{
					ErrorMsg =
						UsefulDBMethods.Helpers.RunEAMainOfficeProcess(FbConnectionString, CommonItem.CompanyIdValue);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg = 
						CommonMethods.TransferFundsProcess(
							FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue, SqlString, FRequestItem.PassedId, FRequestItem.TransferDate);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg =
						UsefulDBMethods.Helpers.RunEAMainOfficeProcess(FbConnectionString, CommonItem.CompanyIdValue);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg =
						CommonMethods.SetupPageGridEntries(
							FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue,
							FRequestItem.BankId, FRequestItem.BranchId, FRequestItem.ManagerId, FRequestItem.DisplayType,
							FRequestItem.TransactionId, FRequestItem.AccountId, FRequestItem.ExpenseId, 
							FRequestItem.DateFrom, FRequestItem.DateTo, FRequestItem.AssignedToId, FRequestItem.CreditorId, 
							FRequestItem.LedgerType, ref Entries);
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
                UsefulDBMethods.EventHelpers.SetEndOfMonthInProgress(FbConnectionString, CommonItem.CompanyIdValue, "N");
                LogOnLeavingMethod(Method);
			}
			
			return Result;
		}
	}
}