/*
 * Created by SharpDevelop.
 * User: John
 * Date: 24/04/2020
 * Time: 7:29 PM
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
namespace Actions.Receipts.CreateOptions
{
	/// <summary>
	/// Description of ValidateImportFile.
	/// </summary>
	public class ValidateImportFile : ActionBase
	{
		private string FModuleName = "Receipts";
		private ImportBankingFileDlgRequestItem FRequestItem;		
		
		public static string Execute(int MasterId,
									 APISettingsItem SettingsItem,
									 string Request,
									 out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			ValidateImportFile inst = new ValidateImportFile();
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
				FRequestItem = new ImportBankingFileDlgRequestItem();
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
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " +Method, FModuleName, MasterId);
			LogOnInsideMethod(Method);
			try 
			{
				StringBuilder Entries = new StringBuilder();
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg = 
						CommonMethods.EABankImportFileScreenGroup(
							FbConnectionString, CommonItem.CompanyIdValue, FRequestItem.MasterEventId, 
							CommonItem.ClientIdValue,
							FRequestItem.BankImportTypeId, FRequestItem.FileName, FRequestItem.BankNameId,
							FRequestItem.StartingReceiptNumber, FRequestItem.ReceiptSetId, FRequestItem.PageNumber,
							FRequestItem.BankStmtDate, ref Entries);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					string Path = "$.whycannotimportfile";
					string Value = JsonUtilities.GetJsonPathValueStr(Entries.ToString(), Path, out ErrorMsg);
					WindowsEventLoggingUtilities.WriteToInfoEvent("Source:" + Entries.ToString(), Method, 0);
					WindowsEventLoggingUtilities.WriteToInfoEvent("Path:" + Path, Method, 0);
					WindowsEventLoggingUtilities.WriteToInfoEvent("Value:" + Value, Method, 0);
					if (string.IsNullOrEmpty(ErrorMsg) && !string.IsNullOrEmpty(Value)) ErrorMsg = Value;
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Result = LogResponse(Method, "");
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
