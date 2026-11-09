/*
 * Created by SharpDevelop.
 * User: John
 * Date: 28/05/2020
 * Time: 7:09 PM
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
	/// Description of Delete.
	/// </summary>
	public class Delete : ActionBase
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

			Delete inst = new Delete();
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
						CommonMethods.EAInvoicesToAddGroup(
							FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue, 
							FRequestItem.InvoiceMasterId, FRequestItem.SelectedCreditor, FRequestItem.SelectedProperty, ref Entries);
				}
				
				string QueryText = "";
				string Path = "";
				string Source = Entries.ToString();				
				string PostMasterDeleteProperty = "";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Path = "$.PostMasterDeleteProperty";
					PostMasterDeleteProperty = JsonUtilities.GetJsonPathValueStr(Source, Path, out ErrorMsg);
					if (string.IsNullOrEmpty(ErrorMsg))
					{
						InvoiceActionTaken ActionTaken = InvoiceActionTaken.Unknown;
						if (PostMasterDeleteProperty == "2") ActionTaken = InvoiceActionTaken.Delete;
						if (ActionTaken != InvoiceActionTaken.Unknown)
						{
							ErrorMsg = 
								CommonMethods.EAInvoiceMasterAdd(
									FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue, ActionTaken, "Invoice", FRequestItem);
							WindowsEventLoggingUtilities.WriteToInfoEvent("After EAInvoiceMasterAdd ErrorMsg: " + ErrorMsg, Method, 0);
						}
					}
				}
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Path = "$.DeleteSQL1";
					QueryText = JsonUtilities.GetJsonPathValueStr(Source, Path, out ErrorMsg);
                    WindowsEventLoggingUtilities.WriteToInfoEvent("DeleteSql1 QueryText:" + Environment.NewLine + QueryText, Method, 0);
                    if (string.IsNullOrEmpty(ErrorMsg) && !string.IsNullOrEmpty(QueryText))
                    {
                        WindowsEventLoggingUtilities.WriteToInfoEvent("DeleteSql1:" + Environment.NewLine + QueryText, Method, 0);
						/*
						Params.Clear();
						Params.Add(new FbParameterItem("CompanyClientId", CommonItem.CompanyIdValue));
						Params.Add(new FbParameterItem("MasterInvoiceId", FRequestItem.InvoiceMasterId));
						Params.Add(new FbParameterItem("LoginId", CommonItem.ClientIdValue));
						ErrorMsg = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
						
						if (string.IsNullOrEmpty(ErrorMsg))
						{
							ErrorMsg = FirebirdUtilities.DBExecute(FbConnectionString, QueryText, Params, 0);
						}
						*/
					}
                    ErrorMsg = FirebirdUtilities.DBExecute(FbConnectionString, QueryText);
                }

                if (string.IsNullOrEmpty(ErrorMsg))
				{
					Path = "$.DeleteSQL2";
					QueryText = JsonUtilities.GetJsonPathValueStr(Source, Path, out ErrorMsg);
                    WindowsEventLoggingUtilities.WriteToInfoEvent("DeleteSql2 QueryText:" + Environment.NewLine + QueryText, Method, 0);
                    if (string.IsNullOrEmpty(ErrorMsg) && !string.IsNullOrEmpty(QueryText))
					{
						WindowsEventLoggingUtilities.WriteToInfoEvent("DeleteSql2:" + Environment.NewLine + QueryText, Method, 0);
						ErrorMsg = FirebirdUtilities.DBExecute(FbConnectionString, QueryText);
					}
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Path = "$.DeleteSQL3";
					QueryText = JsonUtilities.GetJsonPathValueStr(Source, Path, out ErrorMsg);
                    WindowsEventLoggingUtilities.WriteToInfoEvent("DeleteSql3 QueryText:" + Environment.NewLine + QueryText, Method, 0);
                    if (string.IsNullOrEmpty(ErrorMsg) && !string.IsNullOrEmpty(QueryText))
					{
						WindowsEventLoggingUtilities.WriteToInfoEvent("DeleteSql3:" + Environment.NewLine + QueryText, Method, 0);
						ErrorMsg = FirebirdUtilities.DBExecute(FbConnectionString, QueryText);
					}
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Path = "$.DeleteSQL4";
					QueryText = JsonUtilities.GetJsonPathValueStr(Source, Path, out ErrorMsg);
                    WindowsEventLoggingUtilities.WriteToInfoEvent("DeleteSql4 QueryText:" + Environment.NewLine + QueryText, Method, 0);
                    if (string.IsNullOrEmpty(ErrorMsg) && !string.IsNullOrEmpty(QueryText))
					{
						WindowsEventLoggingUtilities.WriteToInfoEvent("DeleteSQL4:" + Environment.NewLine + QueryText, Method, 0);
						ErrorMsg = FirebirdUtilities.DBExecute(FbConnectionString, QueryText);
					}
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Path = "$.DeleteSQL5";
					QueryText = JsonUtilities.GetJsonPathValueStr(Source, Path, out ErrorMsg);
                    WindowsEventLoggingUtilities.WriteToInfoEvent("DeleteSql5 QueryText:" + Environment.NewLine + QueryText, Method, 0);
                    if (string.IsNullOrEmpty(ErrorMsg) && !string.IsNullOrEmpty(QueryText))
					{
						WindowsEventLoggingUtilities.WriteToInfoEvent("DeleteSQL5:" + Environment.NewLine + QueryText, Method, 0);
						ErrorMsg = FirebirdUtilities.DBExecute(FbConnectionString, QueryText);
					}
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