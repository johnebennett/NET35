/*
 * Created by SharpDevelop.
 * User: John
 * Date: 30/03/2020
 * Time: 9:00 PM
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

namespace Actions.Debts
{
	/// <summary>
	/// Description of Debts_SetupBulkEmail.
	/// </summary>
	public class Debts_SetupBulkEmail : ActionBase
	{
		private string FModuleName = "Debts";
		private GatherDebtsRequestItem FRequestItem;		
		
		public static string Execute(int MasterId,
									 APISettingsItem SettingsItem,
									 string Request,
									 out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			Debts_SetupBulkEmail inst = new Debts_SetupBulkEmail();
			//if (!inst.Setup(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg))
			if (!inst.Setup(MasterId, SettingsItem, Request, out ErrorMsg))
				return "";
			
			inst.LogOnInsideMethod(Method);
			try
			{
				UsefulDBMethods.Helpers.RunEAMainOfficeProcess(inst.FbConnectionString, inst.CommonItem.CompanyIdValue);
				UsefulDBMethods.Helpers.RunEAMainOfficeProcess(inst.FbConnectionString, inst.CommonItem.CompanyIdValue);
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
				FRequestItem = new GatherDebtsRequestItem();
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
				if (FRequestItem.MasterExclusionId == 0)
				{
					Result = "Invalid Master Exclusion ID value provided (" + FRequestItem.MasterExclusionId + ")";
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
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " +Method, FModuleName, MasterId);
			LogOnInsideMethod(Method);
			try 
			{
				StringBuilder Entries = new StringBuilder();
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Entries.Append(JsonUtilities.AddPropertyNameAndValue("MasterExclusionId", FRequestItem.MasterExclusionId));
					ErrorMsg =
						CommonMethods.Debts_GatherDebts(
							FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue,
							FRequestItem.MasterExclusionId, FRequestItem.OfficeId, FRequestItem.ManagerId, FRequestItem.LedgerId,
							FRequestItem.LedgerTypeId, FRequestItem.TransactionsId, FRequestItem.RangeFrom, FRequestItem.RangeTo,
							FRequestItem.ChargeOptionsId, FRequestItem.BankTypesId, FRequestItem.PrintGroupId, FRequestItem.ReportsId,
							FRequestItem.TransactionTypeId, FRequestItem.PastEventsId, ref Entries);
				}
				
				int NewDevId = 0;
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					NewDevId =
						UsefulDBMethods.GeneratorHelpers.GetNextGeneratorId(
							FbConnectionString, "I_DEV_ID_GEN", (int)CommonItem.CompanyIdValue, (int)CommonItem.ClientIdValue, out Result);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg =
						CommonMethods.ExecuteSqlInsertScript(
							FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue, NewDevId, Entries);
				}
				
				int NewId = 0;
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					NewId =
						CommonMethods.GetBulkEmailRunId(
							FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue, NewDevId, Entries, out ErrorMsg);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Entries.Append(",");
					Entries.Append(JsonUtilities.AddPropertyNameAndValue("RunNumber", NewId, false));
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
