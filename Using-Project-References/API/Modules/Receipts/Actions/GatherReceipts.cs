/*
 * Created by SharpDevelop.
 * User: John
 * Date: 21/04/2020
 * Time: 12:28 PM
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
	/// Description of GatherReceipts.
	/// </summary>
	public class GatherReceipts : ActionBase
	{
		private string FModuleName = "Receipts";
		private GatherReceiptsRequestItem FRequestItem;		
		
		public static string Execute(int MasterId,
									 APISettingsItem SettingsItem,
									 string Request,
									 out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			GatherReceipts inst = new GatherReceipts();
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
				FRequestItem = new GatherReceiptsRequestItem();
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
				int MasterEventId = FRequestItem.MasterEventId;
				if ((string.IsNullOrEmpty(ErrorMsg)) && (MasterEventId == 0))
				{
					MasterEventId = 
						UsefulDBMethods.GeneratorHelpers.GetNextGeneratorId(
							FbConnectionString, "I_EARC_RECEIPTEVENTID_GEN", CommonItem.CompanyIdValue, CommonItem.ClientIdValue, out ErrorMsg);
					if (string.IsNullOrEmpty(ErrorMsg))
					{
						FRequestItem.MasterEventId = MasterEventId;
						ErrorMsg = CommonMethods.DeleteUserRun(FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue);
					}
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					string TodayStr = DateTime.Now.ToString("dd/MM/yyyy");
					ErrorMsg = 
						CommonMethods.EAReceiptScreenEventAdd(
							FbConnectionString, CommonItem.CompanyIdValue, FRequestItem.MasterEventId, 
							0, 0, 0, 0, 0, 0, 0,TodayStr, TodayStr, TodayStr, TodayStr, CommonItem.ClientIdValue);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Entries.Append(JsonUtilities.AddPropertyNameAndValue("MasterEventId", FRequestItem.MasterEventId));
					ErrorMsg = 
						CommonMethods.EAReceiptScreenGroup2(
							FbConnectionString, CommonItem.CompanyIdValue, FRequestItem.MasterEventId, 
							CommonItem.ClientIdValue, FRequestItem.PastEventId, FRequestItem.ReceiptOptionsId, ref Entries);
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
