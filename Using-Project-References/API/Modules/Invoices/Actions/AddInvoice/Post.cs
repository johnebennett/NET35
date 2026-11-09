/*
 * Created by SharpDevelop.
 * User: John
 * Date: 28/05/2020
 * Time: 7:08 PM
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
	/// Description of Post.
	/// </summary>
	public class Post : ActionBase
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

			Post inst = new Post();
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
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " +Method, FModuleName, MasterId);
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
				
				string QueryText = "";
				string Path = "";
				string Source = Entries.ToString();
				string PrePostSql = "";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Path = "$.PrePostSQL";
					PrePostSql = JsonUtilities.GetJsonPathValueStr(Source, Path, out ErrorMsg);
					if (string.IsNullOrEmpty(ErrorMsg) && !string.IsNullOrEmpty(PrePostSql))
					{
						PrePostSql = PrePostSql.Replace("\"", "'");
						ErrorMsg = FirebirdUtilities.DBExecute(FbConnectionString, PrePostSql);
					}
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Path = "$.PostSQL1";
					QueryText = JsonUtilities.GetJsonPathValueStr(Source, Path, out ErrorMsg);
					if (string.IsNullOrEmpty(ErrorMsg) && !string.IsNullOrEmpty(QueryText))
					{
						WindowsEventLoggingUtilities.WriteToInfoEvent("PostSql1:" + Environment.NewLine + QueryText, Method, 0);
						Params.Clear();
						Params.Add(new FbParameterItem("CompanyClientId", CommonItem.CompanyIdValue));
						Params.Add(new FbParameterItem("MasterInvoiceId", FRequestItem.InvoiceMasterId));
						Params.Add(new FbParameterItem("LoginId", CommonItem.ClientIdValue));
						ErrorMsg = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
						
						if (string.IsNullOrEmpty(ErrorMsg))
						{
							ErrorMsg = FirebirdUtilities.DBExecute(FbConnectionString, QueryText, Params, 0);
						}
					}
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Path = "$.PostSQL2";
					QueryText = JsonUtilities.GetJsonPathValueStr(Source, Path, out ErrorMsg);
					if (string.IsNullOrEmpty(ErrorMsg) && !string.IsNullOrEmpty(QueryText))
					{
						WindowsEventLoggingUtilities.WriteToInfoEvent("PostSql2:" + Environment.NewLine + QueryText, Method, 0);
						ErrorMsg = FirebirdUtilities.DBExecute(FbConnectionString, QueryText);
					}
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Path = "$.PostSQL3";
					QueryText = JsonUtilities.GetJsonPathValueStr(Source, Path, out ErrorMsg);
					if (string.IsNullOrEmpty(ErrorMsg) && !string.IsNullOrEmpty(QueryText))
					{
						WindowsEventLoggingUtilities.WriteToInfoEvent("PostSql3:" + Environment.NewLine + QueryText, Method, 0);
						ErrorMsg = FirebirdUtilities.DBExecute(FbConnectionString, QueryText);
					}
				}
				
				UsefulDBMethods.Helpers.RunEAMainOfficeProcess(FbConnectionString, CommonItem.CompanyIdValue);
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Entries.Length = 0;
					ErrorMsg =
						CommonMethods.EAInvoicesToAddGroup(
							FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue, 
							FRequestItem.InvoiceMasterId, FRequestItem.SelectedCreditor, FRequestItem.SelectedProperty, ref Entries);
				}
				
				string BulkId = "";
				Source = Entries.ToString();
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Result =
						JsonUtilities.StartJson() +
						Entries.ToString() +
						JsonUtilities.EndJson();
					LogResponse(Method, Result);
					
					Path = "$.LedgerBulkID";
					BulkId = JsonUtilities.GetJsonPathValueStr(Source, Path, out ErrorMsg);
					if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(BulkId)) ErrorMsg = "Cannot find the Bulk ID value";
				}
				
				int DebtEventId = -99;
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg = CommonMethods.GetDebtEventId(FbConnectionString, FRequestItem.InvoiceMasterId, out DebtEventId);
					WindowsEventLoggingUtilities.WriteToInfoEvent("DebtEventId: " + DebtEventId.ToString(), Method, 0);
					if (string.IsNullOrEmpty(ErrorMsg) && (DebtEventId < 0)) ErrorMsg = "Invalid value for Debt Event Id";
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					//Result = LogResponse(Method, "");
					Result =
						JsonUtilities.StartJson() +
						JsonUtilities.AddPropertyNameAndValue("Status", "SUCCESS") +
						JsonUtilities.AddPropertyNameAndValue("DebtEventId", DebtEventId) +
						JsonUtilities.AddPropertyNameAndValue("BulkId", BulkId, false) +
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