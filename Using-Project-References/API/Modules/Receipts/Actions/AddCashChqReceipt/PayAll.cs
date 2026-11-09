/*
 * Created by SharpDevelop.
 * User: John
 * Date: 10/05/2020
 * Time: 7:03 PM
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
namespace Actions.Receipts.AddCashChqReceipt
{
	/// <summary>
	/// Description of PayAll.
	/// </summary>
	public class PayAll : ActionBase
	{
		private string FModuleName = "Receipts";
		private SetupPageEntriesRequestItem FRequestItem;		
		
		public static string Execute(int MasterId,
									 APISettingsItem SettingsItem,
									 string Request,
									 out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			PayAll inst = new PayAll();
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
				Entries.Append(JsonUtilities.AddPropertyNameAndValue("MasterEventId", FRequestItem.MasterEventId));
				Entries.Append(JsonUtilities.AddPropertyNameAndValue("BankingId", FRequestItem.BankingId, false));

				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg =
						AddCashChqReceipt.CommonMethods.EAReceiptScreenMixAdd(
							FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue, FRequestItem, ref Entries);
				}
				
				/*
				string ActiveBanking = "";
				string ActiveBankingDescription = "";
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ActiveBanking = JsonUtilities.GetJsonPathValueStr(Entries.ToString(), "$.ActiveBanking", out ErrorMsg);
					if (string.IsNullOrEmpty(ErrorMsg) && (StringUtilities.SameText(ActiveBanking, "N")))
					{
						ActiveBankingDescription = JsonUtilities.GetJsonPathValueStr(Entries.ToString(), "$.ActiveBankingdescription", out ErrorMsg);
						if (ActiveBankingDescription == "") ActiveBankingDescription = "Unknown Reason";
						if (string.IsNullOrEmpty(ErrorMsg)) ErrorMsg = ActiveBankingDescription;
					}
				}
				*/
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg = CommonMethods.GetActiveBankingDescription(Entries);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg = 
						Receipts.CommonMethods.ValidateReceiptEntries(
							FRequestItem.Detail, FRequestItem.Payee, FRequestItem.TakenBy, FRequestItem.ReceiptNo, FRequestItem.ReceiptType,
							FRequestItem.BankId, FRequestItem.SubLedgerId, FRequestItem.RevenueId);
				}
				
				string PayAllSql = "";
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					PayAllSql = JsonUtilities.GetJsonPathValueStr(Entries.ToString(), "$.PayAllSQL", out ErrorMsg);
					if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(PayAllSql)) ErrorMsg = "Cannot find the PayAll entry";
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent("PayAllSql:" + Environment.NewLine + PayAllSql, Method, 0);
					ErrorMsg = FirebirdUtilities.DBExecute(FbConnectionString, PayAllSql);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Entries.Length = 0;
					Entries.Append(JsonUtilities.AddPropertyNameAndValue("MasterEventId", FRequestItem.MasterEventId));
					Entries.Append(JsonUtilities.AddPropertyNameAndValue("BankingId", FRequestItem.BankingId, false));
					ErrorMsg =
						AddCashChqReceipt.CommonMethods.EAReceiptScreenMixAdd(
							FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue, FRequestItem, ref Entries); 
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
