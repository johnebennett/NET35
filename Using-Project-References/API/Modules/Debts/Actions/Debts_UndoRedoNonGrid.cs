/*
 * Created by SharpDevelop.
 * User: John
 * Date: 25/03/2020
 * Time: 5:25 PM
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
	/// Description of Debts_UndoRedoNonGrid.
	/// </summary>
	public class Debts_UndoRedoNonGrid : ActionBase
	{
		private string FModuleName = "Debts";
		private Debts_UndoRedoNonGridRequestItem FRequestItem;		
		
		public static string Execute(int MasterId,
									 APISettingsItem SettingsItem,
									 string Request,
									 out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			Debts_UndoRedoNonGrid inst = new Debts_UndoRedoNonGrid();
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
				FRequestItem = new Debts_UndoRedoNonGridRequestItem();
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
				string QueryText = GetIncludeExcludeScript(out ErrorMsg);
				if ((string.IsNullOrEmpty(ErrorMsg)) && (string.IsNullOrEmpty(QueryText)))
				{
					ErrorMsg = "Missing or Empty Status";
				}
				
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Params.Add(new FbParameterItem("CompanyId", CommonItem.CompanyIdValue));
					Params.Add(new FbParameterItem("MasterExclusionId", FRequestItem.MasterExclusionId));
					
					switch (FRequestItem.Source)
					{
						case 01: Params.Add(new FbParameterItem("AccountId", FRequestItem.LedgerId)); break;
						case 02: Params.Add(new FbParameterItem("BusinessId", FRequestItem.TransactionsId)); break;
						default: break;						
					}
					ErrorMsg = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg = FirebirdUtilities.DBExecute(FbConnectionString, QueryText, Params, 0);
				}
				
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
		
		private string GetIncludeExcludeScript(out string ErrorMsg)
		{
			string Result = "";
			string Method = "GetIncludeExcludeScript";
			ErrorMsg = "";
			
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " +Method, FModuleName, MasterId);
			LogOnInsideMethod(Method);
			try 
			{
				switch (FRequestItem.Source)
				{
					case 01: 
						Result = "EXECUTE PROCEDURE EADEBTEXCLUSIONADD (@CompanyId, @MasterExclusionId, 0, @AccountId, 'Y', 'N', 'N')";
						break;
					case 02: 
						Result = "EXECUTE PROCEDURE EADEBTEXCLUSIONADD (@CompanyId, @MasterExclusionId, @BusinessId, 0, 'Y', 'N', 'N')";
						break;
					default:
						ErrorMsg = "Invalid Source value provided";
						break;
				}
			}
			catch (Exception Ex)
			{
				ErrorMsg = LogOnException(Method, Ex, "");
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
