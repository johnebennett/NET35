/*
 * Created by SharpDevelop.
 * User: John
 * Date: 23/05/2020
 * Time: 5:32 PM
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
namespace Actions.Invoices.AddInvoice
{
	/// <summary>
	/// Description of AddToList.
	/// </summary>
	public class AddToList : ActionBase
	{
		private string FModuleName = "Invoices";
		private SetupPageEntriesRequestItem FRequestItem;
		
		public static string Execute(int MasterId,
		                             APISettingsItem SettingsItem,
		                             string Request,
		                             out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			AddToList inst = new AddToList();
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
				//UsefulDBMethods.Helpers.RunEAMainOfficeProcess(inst.FbConnectionString, inst.CommonItem.CompanyIdValue);
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
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " +Method, FModuleName, MasterId);
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
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					if (FRequestItem.InvoiceMasterId == 0) ErrorMsg = "Invalid Master Id provided";
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg = 
						CommonMethods.EAInvoiceMasterAdd(
							FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue, InvoiceActionTaken.Update, "Invoice", FRequestItem);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg =
						CommonMethods.EAInvoicesToAddGroup(
							FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue, 
							FRequestItem.InvoiceMasterId, FRequestItem.SelectedCreditor, FRequestItem.SelectedProperty, ref Entries);
				}
				
				string Str = 
						JsonUtilities.StartJson() +
						Entries.ToString() +
						JsonUtilities.EndJson();
				
				string AddToListSql = "";
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					AddToListSql = JsonUtilities.GetJsonPathValueStr(Str, "$.AddtoListSQL", out ErrorMsg);
					if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(AddToListSql)) ErrorMsg = "Cannot find the Add To List entry";					
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					AddToListSql = AddToListSql.Replace("__", "'");
					AddToListSql = AddToListSql.Replace("\"", "'");
					WindowsEventLoggingUtilities.WriteToInfoEvent("AddToListSql:" + Environment.NewLine + AddToListSql, Method, 0);
					ErrorMsg = FirebirdUtilities.DBExecute(FbConnectionString, AddToListSql);
					WindowsEventLoggingUtilities.WriteToInfoEvent("Result:" + Environment.NewLine + ErrorMsg, Method, 0);
				}

                AddToListSql = "";
                if (string.IsNullOrEmpty(ErrorMsg))
                {
                    AddToListSql = JsonUtilities.GetJsonPathValueStr(Str, "$.AddtoListSQL1", out ErrorMsg);
                }

                if (string.IsNullOrEmpty(ErrorMsg) && !string.IsNullOrEmpty(AddToListSql))
                {
                    ErrorMsg = FirebirdUtilities.DBExecute(FbConnectionString, AddToListSql);
                }

                if (string.IsNullOrEmpty(ErrorMsg))
				{
					Entries.Length = 0;
					ErrorMsg =
						CommonMethods.EAInvoicesToAddGroup(
							FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue, 
							FRequestItem.InvoiceMasterId, FRequestItem.SelectedCreditor, FRequestItem.SelectedProperty, ref Entries);
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