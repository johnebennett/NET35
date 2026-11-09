/*
 * Created by SharpDevelop.
 * User: John
 * Date: 10/05/2020
 * Time: 7:43 PM
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
	/// Description of Regenerate.
	/// </summary>
	public class Regenerate : ActionBase
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

			Regenerate inst = new Regenerate();
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
				
				string RegenerateSql1 = "";
				string RegenerateSql2 = "";
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					RegenerateSql1 = JsonUtilities.GetJsonPathValueStr(Entries.ToString(), "$.RegenerateSQL1", out ErrorMsg);
					if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(RegenerateSql1)) ErrorMsg = "Cannot find the first Regenerate entry";
					if (string.IsNullOrEmpty(ErrorMsg))
					{
						RegenerateSql2 = JsonUtilities.GetJsonPathValueStr(Entries.ToString(), "$.RegenerateSQL2", out ErrorMsg);
						if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(RegenerateSql2)) ErrorMsg = "Cannot find the second Regenerate entry";
					}
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg = FirebirdUtilities.DBExecute(FbConnectionString, RegenerateSql1);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg = FirebirdUtilities.DBExecute(FbConnectionString, RegenerateSql2);
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
