/*
 * Created by SharpDevelop.
 * User: John
 * Date: 22/04/2020
 * Time: 6:30 PM
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

namespace Actions.Receipts
{
	/// <summary>
	/// Description of CreateOptionsMaster.
	/// </summary>
	public class CreateOptionsMaster : ActionBase
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

			CreateOptionsMaster inst = new CreateOptionsMaster();
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
			string Method = "CreateOptionsMaster.Output";
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
					case "CreateBankingDlg":
						Result = CreateOptions.CreateBankingDlg.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
						break;
					case "AddBanking":
						Result = CreateOptions.AddBanking.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
						break;
					case "ImportBankingFileDlg":
						Result = CreateOptions.ImportBankingFileDlg.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
						break;
					case "ValidateImportFile":
						Result = CreateOptions.ValidateImportFile.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
						break;
					case "ImportBankingFile":
						Result = CreateOptions.ImportBankingFile.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
						break;
					case "CreateDirectDebitList":
						Result = CreateOptions.CreateDirectDebitList.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
						break;
					case "DirectDebitList_ChangeDebitType":
						Result = CreateOptions.DirectDebitList_ChangeDebitType.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
						break;
					case "DirectDebitList_DoOnCancel":
						Result = CreateOptions.DirectDebitList_DoOnCancel.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
						break;
					case "DirectDebitList_DoOnApply":
						Result = CreateOptions.DirectDebitList_DoOnApply.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
						break;
                    case "DirectDebitList_ChangeBankAccount":
                        Result = CreateOptions.DirectDebitList_ChangeBankAccount.Execute(this.MasterId, this.APISettings, this.JsonSource, out ErrorMsg);
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
