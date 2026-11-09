/*
 * Created by SharpDevelop.
 * User: John
 * Date: 23/03/2020
 * Time: 8:33 PM
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
	/// Description of Debts_ShowReportItem.
	/// </summary>
	public class Debts_ShowReportItem : ActionBase
	{
		private string FModuleName = "Debts";
		private Debts_ShowReportItemRequestItem FRequestItem;		
		
		public static string Execute(int MasterId,
									 APISettingsItem SettingsItem,
									 string Request,
									 out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			Debts_ShowReportItem inst = new Debts_ShowReportItem();
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
				FRequestItem = new Debts_ShowReportItemRequestItem();
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
			string OutputFileName = "";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " +Method, FModuleName, MasterId);
			LogOnInsideMethod(Method);
			try 
			{
				string ReportSource = GetReportSource(out ErrorMsg);
				if ((string.IsNullOrEmpty(ErrorMsg)) && (string.IsNullOrEmpty(ReportSource)))
				{
					ErrorMsg = "Missing or Empty ReportSource";
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{			
					Int64 NewId =
						global::UsefulDBMethods.Reporting.SystemReportUtilities.Execute(
							FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue, FRequestItem.PortfolioId, ReportSource, 
							out ErrorMsg, out OutputFileName);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Result =
						JsonUtilities.StartJson() +
						JsonUtilities.AddPropertyNameAndValue("FileName", @"/Docs/" + OutputFileName, false) +
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
		
		private string GetReportSource(out string ErrorMsg)
		{
			string Result = "";
			string Method = "GetReportSource";
			StringBuilder Entries = new StringBuilder();
			ErrorMsg = 
				CommonMethods.Debts_GatherDebts(
					FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue,
					FRequestItem.MasterExclusionId, FRequestItem.OfficeId, FRequestItem.ManagerId, FRequestItem.LedgerId,
					FRequestItem.LedgerTypeId, FRequestItem.TransactionsId, FRequestItem.RangeFrom, FRequestItem.RangeTo,
					FRequestItem.ChargeOptionsId, FRequestItem.BankTypesId, FRequestItem.PrintGroupId, FRequestItem.ReportsId,
					FRequestItem.TransactionTypeId, FRequestItem.PastEventsId, ref Entries);
				
			string Str = "";
			JObject JObj = null;
			if (string.IsNullOrEmpty(ErrorMsg))
			{
				Str =
					JsonUtilities.StartJson() +
					Entries.ToString() +
					JsonUtilities.EndJson();
			
				WindowsEventLoggingUtilities.WriteToInfoEvent("JsonText: " + Environment.NewLine + Str, FModuleName, MasterId);
				
				try 
				{
					if (Entries.Length > 0)
					{
						JObj = JObject.Parse(Str);
					}				
				}
				catch (Exception Ex)
				{
					ErrorMsg =
						Method + Environment.NewLine +
						Ex.Message + Environment.NewLine +
						Ex.StackTrace;					
				}
			}
			
			if (string.IsNullOrEmpty(ErrorMsg))
			{
				string JPath = @"$.gridEntries[?(@.I_TRANSACTION_ID == '" + FRequestItem.PortfolioId + "')]";
				WindowsEventLoggingUtilities.WriteToInfoEvent("JPath: " + JPath, FModuleName, MasterId);
				JToken Record = JObj.SelectToken(JPath);
				if (Record == null)
					WindowsEventLoggingUtilities.WriteToInfoEvent("Record IS NULL", FModuleName, MasterId);
				else
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent("Record: " + Record.ToString(), FModuleName, MasterId);
					Result = (string)Record["ZL20_B" + FRequestItem.ButtonIdValue];				
					WindowsEventLoggingUtilities.WriteToInfoEvent("Result: " + Result, FModuleName, MasterId);
				}
			}				
			
			return Result;
		}
	}
}
