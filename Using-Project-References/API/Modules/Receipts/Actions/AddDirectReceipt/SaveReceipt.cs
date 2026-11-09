/*
 * Created by SharpDevelop.
 * User: John
 * Date: 05/05/2020
 * Time: 11:25 AM
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
namespace Actions.Receipts.AddDirectReceipt
{
	/// <summary>
	/// Description of SaveReceipt.
	/// </summary>
	public class SaveReceipt : ActionBase
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

			SaveReceipt inst = new SaveReceipt();
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
				Entries.Append(JsonUtilities.AddPropertyNameAndValue("MasterEventId", FRequestItem.MasterEventId, false));

				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg =
						AddDirectReceipt.CommonMethods.EAReceiptScreenDirectAdd(
							FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue, FRequestItem, ref Entries);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg = 
						Receipts.CommonMethods.ValidateReceiptEntries(
							FRequestItem.Detail, FRequestItem.Payee, FRequestItem.TakenBy, FRequestItem.ReceiptNo, FRequestItem.ReceiptType,
							FRequestItem.BankId, FRequestItem.SubLedgerId, FRequestItem.RevenueId);
				}
				
				string SaveSql1 = "";
				string SaveSql2 = "";
				string SaveSql3 = "";
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					SaveSql1 = JsonUtilities.GetJsonPathValueStr(Entries.ToString(), "$.SaveSQL1", out ErrorMsg);
					if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(SaveSql1)) ErrorMsg = "Cannot find the first Save entry";
					if (string.IsNullOrEmpty(ErrorMsg))
					{
						SaveSql2 = JsonUtilities.GetJsonPathValueStr(Entries.ToString(), "$.SaveSQL2", out ErrorMsg);
						if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(SaveSql2)) ErrorMsg = "Cannot find the second Save entry";
					}
					if (string.IsNullOrEmpty(ErrorMsg))
					{
						SaveSql3 = JsonUtilities.GetJsonPathValueStr(Entries.ToString(), "$.SaveSQL3", out ErrorMsg);
						if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(SaveSql3)) ErrorMsg = "Cannot find the third Save entry";
					}
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					SaveSql1 = SaveSql1.Replace("@", "'");
					ErrorMsg = FirebirdUtilities.DBExecute(FbConnectionString, SaveSql1);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg = FirebirdUtilities.DBExecute(FbConnectionString, SaveSql2);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg = FirebirdUtilities.DBExecute(FbConnectionString, SaveSql3);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Entries.Length = 0;
					Entries.Append(JsonUtilities.AddPropertyNameAndValue("MasterEventId", FRequestItem.MasterEventId, false));

					ErrorMsg =
						AddDirectReceipt.CommonMethods.EAReceiptScreenDirectAdd(
							FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue, FRequestItem, ref Entries);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					//Result = LogResponse(Method, "");				
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
