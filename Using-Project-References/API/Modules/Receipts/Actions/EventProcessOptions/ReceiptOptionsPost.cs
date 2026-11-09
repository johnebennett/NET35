/*
 * Created by SharpDevelop.
 * User: John
 * Date: 29/04/2020
 * Time: 4:51 PM
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
namespace Actions.Receipts.EventProcessOptions
{
	/// <summary>
	/// Description of ReceiptOptionsPost.
	/// </summary>
	public class ReceiptOptionsPost : ActionBase
	{
		private string FModuleName = "Receipts";
		private EventProcessOptions_OnChangeRequestItem FRequestItem;		
		
		public static string Execute(int MasterId,
									 APISettingsItem SettingsItem,
									 string Request,
									 out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			ReceiptOptionsPost inst = new ReceiptOptionsPost();
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
				FRequestItem = new EventProcessOptions_OnChangeRequestItem();
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
                ErrorMsg = UsefulDBMethods.EventHelpers.SetEndOfMonthInProgress(FbConnectionString, CommonItem.CompanyIdValue, "R");

                StringBuilder Entries = new StringBuilder();
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg =
						CommonMethods.EAReceiptScreenGroup2(
							FbConnectionString, CommonItem.CompanyIdValue, FRequestItem.MasterEventId, CommonItem.ClientIdValue, 
							FRequestItem.PastEventId, FRequestItem.ItemId, ref Entries);
				}
				
				//string ShowCancelButton = "";
				string PostSql = "";
				string PostSql2 = "";
				string Path = "";
				if (string.IsNullOrEmpty(ErrorMsg))					
				{
					/*
					Path = "$.ShowCancelButton";
					ShowCancelButton = JsonUtilities.GetJsonPathValueStr(Entries.ToString(), Path, out ErrorMsg);
					if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(ShowCancelButton))
						ErrorMsg = "Cannot find the Show Cancel Button value";
					if (string.IsNullOrEmpty(ErrorMsg) && (ShowCancelButton == "N"))
						ErrorMsg = "This item cannot be removed";
					*/
					if (string.IsNullOrEmpty(ErrorMsg))
					{
						Path = "$.PostSQL";
						PostSql = JsonUtilities.GetJsonPathValueStr(Entries.ToString(), Path, out ErrorMsg);
						WindowsEventLoggingUtilities.WriteToInfoEvent("PostSQL: " + PostSql, Method, 0);
					if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(PostSql))
						ErrorMsg = "Cannot find the Post SQL value";
					}
					if (string.IsNullOrEmpty(ErrorMsg))
					{
						ErrorMsg = FirebirdUtilities.DBExecute(FbConnectionString, PostSql);
						WindowsEventLoggingUtilities.WriteToInfoEvent("After PostSQL: " + ErrorMsg, Method, 0);
					}
					if (string.IsNullOrEmpty(ErrorMsg))
					{
						Path = "$.PostSQL2";
						PostSql2 = JsonUtilities.GetJsonPathValueStr(Entries.ToString(), Path, out ErrorMsg);
						WindowsEventLoggingUtilities.WriteToInfoEvent("PostSQ2L: " + PostSql2, Method, 0);
					if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(PostSql2))
						ErrorMsg = "Cannot find the second Post SQL value";
					}
					if (string.IsNullOrEmpty(ErrorMsg))
					{
						ErrorMsg = FirebirdUtilities.DBExecute(FbConnectionString, PostSql2);
						WindowsEventLoggingUtilities.WriteToInfoEvent("After PostSQL2: " + ErrorMsg, Method, 0);
					}
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Entries.Length = 0;
					FRequestItem.ItemId = 0; // the original entry has been removed so we can't use it here
					ErrorMsg =
						CommonMethods.EAReceiptScreenGroup2(
							FbConnectionString, CommonItem.CompanyIdValue, FRequestItem.MasterEventId, CommonItem.ClientIdValue, 
							FRequestItem.PastEventId, FRequestItem.ItemId, ref Entries);
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
                UsefulDBMethods.EventHelpers.SetEndOfMonthInProgress(FbConnectionString, CommonItem.CompanyIdValue, "N");
                LogOnLeavingMethod(Method);
			}
			WindowsEventLoggingUtilities.WriteToInfoEvent("Leaving: " +Method, FModuleName, MasterId);
			return Result;
		}
	}
}
