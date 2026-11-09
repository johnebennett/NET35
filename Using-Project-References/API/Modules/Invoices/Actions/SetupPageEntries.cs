/*
 * Created by SharpDevelop.
 * User: John
 * Date: 23/12/2019
 * Time: 11:23 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text;
using System.Data;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CommonUtilities;
using EAOfficeAPI;

namespace Actions.Invoices
{
	/// <summary>
	/// Description of SetupPageEntries.
	/// </summary>
	public class SetupPageEntries : ActionBase
	{
		private string FModuleName = "Invoices";
		private SetupPageEntriesRequestItem FRequestItem;
		
		public static string Execute(int MasterId,
		                             APISettingsItem SettingsItem,
		                             string Request,
		                             out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			SetupPageEntries inst = new SetupPageEntries();
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
				FRequestItem = new SetupPageEntriesRequestItem();
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
				
				StringBuilder entries = new StringBuilder();
					
				//ErrorMsg = BuildOutputEntries(ref entries);				
				ErrorMsg = BuildPageEntries(ref entries);
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					//entries.AppendLine(",");
					ErrorMsg = BuildGridEntries(ref entries);
				}
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Result =
						JsonUtilities.StartJson() +
						entries.ToString() +
						//JsonUtilities.AddPropertyNameAndValue("FileName", @"/Docs/" + OutputFileName, false) +
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
			
			return Result;
		}
		
		private string BuildPageEntries(ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "BuildPageEntries";
            string NewManagerId = "";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			LogOnInsideMethod(Method);
			try 
			{
				Result = 
					CommonMethods.EAMainInvoiceScreenGroup(
						FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue,
						FRequestItem.BankId, FRequestItem.BranchId, FRequestItem.ManagerId, FRequestItem.DisplayType,
						FRequestItem.ExpenseId, FRequestItem.AssignedToId, FRequestItem.DateFrom, FRequestItem.DateTo,
						FRequestItem.CreditorId, FRequestItem.TransactionId, FRequestItem.AccountId, FRequestItem.LedgerType,
						ref Entries);
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
		
		private string BuildGridEntries(ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "BuildGridEntries";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            string NewManagerId = "";
            if (string.IsNullOrEmpty(Result))
            {
                string Str =
                    JsonUtilities.StartJson() +
                    Entries.ToString() +
                    JsonUtilities.EndJson();
                NewManagerId = JsonUtilities.GetJsonPathValueStr(Str, "$.ManagerID", out Result);
            }

            if (string.IsNullOrEmpty(NewManagerId)) NewManagerId = "-1";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Calculated ManagerID: " + NewManagerId, Method, 0);

            LogOnInsideMethod(Method);
			try 
			{
				Result =
					CommonMethods.SetupPageGridEntries(
						FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue,
						//FRequestItem.BankId, FRequestItem.BranchId, FRequestItem.ManagerId, FRequestItem.DisplayType,
                        FRequestItem.BankId, FRequestItem.BranchId, Convert.ToInt32(NewManagerId), FRequestItem.DisplayType,
                        FRequestItem.TransactionId, FRequestItem.AccountId, FRequestItem.ExpenseId, 
						FRequestItem.DateFrom, FRequestItem.DateTo, FRequestItem.AssignedToId, FRequestItem.CreditorId, 
						FRequestItem.LedgerType, ref Entries);
				WindowsEventLoggingUtilities.WriteToInfoEvent("BuildGridEntries Result:" + Result, Method, 0);
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
	}
}