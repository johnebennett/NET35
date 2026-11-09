/*
 * Created by SharpDevelop.
 * User: John
 * Date: 26/03/2020
 * Time: 2:17 PM
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
	/// Description of Debts_ClearSelections.
	/// </summary>
	public class Debts_ClearSelections : ActionBase
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

			Debts_ClearSelections inst = new Debts_ClearSelections();
			//if (!inst.Setup(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg))
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
				string QueryText = 
					"DELETE FROM EADEBTEXCLUSIONLIST WHERE I_DEEX_MASTERID = @MasterExclusionId AND I_DEEX_COMPANYCLIENTID = @CompanyId";				
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CommonItem.CompanyIdValue));
				Params.Add(new FbParameterItem("MasterExclusionId", FRequestItem.MasterExclusionId));
				ErrorMsg = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg = FirebirdUtilities.DBExecute(FbConnectionString, QueryText, Params, 0);
				}
				
				StringBuilder Entries = new StringBuilder();
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Entries.Append(JsonUtilities.AddPropertyNameAndValue("MasterExclusionId", FRequestItem.MasterExclusionId));
					
					FRequestItem.LedgerId = 0;
					FRequestItem.TransactionsId = 0;
					ErrorMsg =
						CommonMethods.Debts_GatherDebts(
							FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue,
							FRequestItem.MasterExclusionId, FRequestItem.OfficeId, FRequestItem.ManagerId, FRequestItem.LedgerId,
							FRequestItem.LedgerTypeId, FRequestItem.TransactionsId, FRequestItem.RangeFrom, FRequestItem.RangeTo,
							FRequestItem.ChargeOptionsId, FRequestItem.BankTypesId, FRequestItem.PrintGroupId, FRequestItem.ReportsId,
							FRequestItem.TransactionTypeId, FRequestItem.PastEventsId, ref Entries);
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
