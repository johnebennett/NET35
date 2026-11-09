/*
 * Created by SharpDevelop.
 * User: John
 * Date: 11/05/2020
 * Time: 10:37 AM
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
	/// Description of ChangeIncludeStatus.
	/// </summary>
	public class ChangeIncludeStatus : ActionBase
	{
		private string FModuleName = "Receipts";
		private ChangeIncludeStatusRequestItem FRequestItem;		
		
		public static string Execute(int MasterId,
									 APISettingsItem SettingsItem,
									 string Request,
									 out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			ChangeIncludeStatus inst = new ChangeIncludeStatus();
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
				FRequestItem = new ChangeIncludeStatusRequestItem();
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

				/*
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg =
						AddCashChqReceipt.CommonMethods.EAReceiptScreenMixAdd(
							FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue, FRequestItem, ref Entries);
				}
				*/
				
				AddCashChqReceipt.SetupPageEntriesRequestItem PageItem = new SetupPageEntriesRequestItem();
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg = PageItem.Load(this);
				}

				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg =
						AddCashChqReceipt.CommonMethods.EAReceiptScreenMixAdd(
							FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue, PageItem, ref Entries);
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

				string IncludeSql = "";
				string Path = "";
				JObject JObj = null;
				string Str = "";
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Str =
						JsonUtilities.StartJson() +
						Entries.ToString() +
						JsonUtilities.EndJson();
					if (Entries.Length > 0)
					{
						JObj = JObject.Parse(Str);
					}
					if (JObj == null) ErrorMsg = "Error parsing Entries";
					if (string.IsNullOrEmpty(ErrorMsg))
					{
						Path = @"$.GridEntries[?(@.ID == '" + FRequestItem.UniqueIdValue + "')]";
						WindowsEventLoggingUtilities.WriteToInfoEvent("Path" + Environment.NewLine + Path, Method, 0);
						JToken Record = JObj.SelectToken(Path);
						if (Record == null)
							WindowsEventLoggingUtilities.WriteToInfoEvent("Record IS NULL", FModuleName, MasterId);
						else
						{
							WindowsEventLoggingUtilities.WriteToInfoEvent("Record: " + Record.ToString(), FModuleName, MasterId);
							IncludeSql = (string)Record["ZL30_INCLUDE_SQL"];
						}
						if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(IncludeSql)) ErrorMsg = "Cannot find the Include sql entry";
					}
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					IncludeSql = IncludeSql.Replace("\"", "'");
					WindowsEventLoggingUtilities.WriteToInfoEvent("IncludeSql" + Environment.NewLine + IncludeSql, Method, 0);
					ErrorMsg = FirebirdUtilities.DBExecute(FbConnectionString, IncludeSql);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Entries.Length = 0;
					Entries.Append(JsonUtilities.AddPropertyNameAndValue("MasterEventId", FRequestItem.MasterEventId));
					Entries.Append(JsonUtilities.AddPropertyNameAndValue("BankingId", FRequestItem.BankingId, false));
					ErrorMsg =
						AddCashChqReceipt.CommonMethods.EAReceiptScreenMixAdd(
							FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue, PageItem, ref Entries);
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
