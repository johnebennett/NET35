/*
 * Created by SharpDevelop.
 * User: John
 * Date: 22/06/2020
 * Time: 11:43 AM
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
namespace Actions.PortfolioAddButton.SubLedgerDlg
{
	/// <summary>
	/// Description of SaveCmd.
	/// </summary>
	public class SaveCmd : ActionBase
	{
		private string FModuleName = "PortfolioAddButton";
		private SaveCmdRequestItem FRequestItem;
		
		public static string Execute(int MasterId,
		                             APISettingsItem SettingsItem,
		                             string Request,
		                             out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			SaveCmd inst = new SaveCmd();
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
				FRequestItem = new SaveCmdRequestItem();
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
				if (string.IsNullOrEmpty(Result))
				{
					Result = AdditionalValidation();
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
		
		private string AdditionalValidation()
		{
			string Method = "AdditionalValidate";
			string Result = "";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " +Method, FModuleName, MasterId);
			LogOnInsideMethod(Method);
			try 
			{
				/*
				ChangeStatementDETAILS
				if Yes 
					Show StatementName, Add1 to 4
					Must have Statement Name to be valid
				ChangeBSBDetails
				if Yes	Show Change BSB,AccountNumber
					BSB and Account Details same as Direct Debit
				*/
				if (string.IsNullOrEmpty(Result))
				{
					if (FRequestItem.CanChangeStmt == "Y")
					{
						if (string.IsNullOrEmpty(FRequestItem.StatementName)) Result = "The Statement Name cannot be empty.";
					}
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					if (FRequestItem.CanChangeBSB == "Y")
					{
						if ((string.IsNullOrEmpty(FRequestItem.BSB)) || (FRequestItem.BSB == "-")) Result = "The BSB value is not valid.";
						if (string.IsNullOrEmpty(Result))
						{
							if (string.IsNullOrEmpty(FRequestItem.AccountNumber)) Result = "The Account Number provided is not valid.";
						}
					}
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
				ErrorMsg = CommonMethods.SaveCmd(FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue, FRequestItem);

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
				WindowsEventLoggingUtilities.WriteToInfoEvent("Leaving: " + Method, FModuleName, MasterId);
			}
			return Result;
		}
	}
}