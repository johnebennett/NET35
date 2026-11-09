/*
 * Created by SharpDevelop.
 * User: John
 * Date: 17/01/2020
 * Time: 11:13 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text;
using System.Data;
using System.Collections.Generic;

using Actions.Invoices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CommonUtilities;
using EAOfficeAPI;

namespace Actions.Invoices
{
	/// <summary>
	/// Description of InvoicesGetDisplayTypeReport.
	/// </summary>
	public class InvoicesGetDisplayTypeReport : ActionBase
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

			InvoicesGetDisplayTypeReport inst = new InvoicesGetDisplayTypeReport();
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
				Result = BuildOutput(out ErrorMsg);
				if (!string.IsNullOrEmpty(ErrorMsg))
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
		
		private string BuildOutput(out string ErrorMsg)
		{
			string Result = "";
			string Method = "BuildOutput";
			ErrorMsg = "";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, FModuleName, MasterId);
					
			LogOnInsideMethod(Method);
			try 
			{
				string ReportSource = "";
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ReportSource = GetReportSource(out ErrorMsg);
				}
				
				string OutputFileName = "";
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Int64 NewId =
						CommonUtilities.SystemReportUtilities.Execute(
							FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue,
							CommonItem.ClientIdValue, ReportSource, out ErrorMsg, out OutputFileName);
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
			
			return Result;
		}
		
		private string GetReportSource(out string ErrorMsg)
		{
			string Method = "GetReportSource";
			string Result = "";			
			ErrorMsg = "";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, FModuleName, MasterId);
			
			LogOnInsideMethod(Method);
			try
			{
				StringBuilder Entries = new StringBuilder();
				ErrorMsg = 
					CommonMethods.EAMainInvoiceScreenGroup(
						FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue,
						FRequestItem.BankId, FRequestItem.BranchId, FRequestItem.ManagerId, FRequestItem.DisplayType,
						FRequestItem.ExpenseId, FRequestItem.AssignedToId, FRequestItem.DateFrom, FRequestItem.DateTo,
						FRequestItem.CreditorId, FRequestItem.TransactionId, FRequestItem.AccountId, FRequestItem.LedgerType,
						ref Entries);				
				WindowsEventLoggingUtilities.WriteToInfoEvent("Entries.Length: " + Entries.Length, Method, 0);
				
				if (!string.IsNullOrEmpty(ErrorMsg))
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent("ErrorMsg: " + Environment.NewLine + ErrorMsg, FModuleName, MasterId);
					
					return Result;
				}
				
				WindowsEventLoggingUtilities.WriteToInfoEvent("(1)", FModuleName, MasterId);
				string Str = "";
				JObject JObj = null;
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent("(2)", FModuleName, MasterId);
					Str = Entries.ToString();
					WindowsEventLoggingUtilities.WriteToInfoEvent("(3)", FModuleName, MasterId);
					Str = JsonUtilities.StartJson() + Str + JsonUtilities.EndJson();
					WindowsEventLoggingUtilities.WriteToInfoEvent("(4)", FModuleName, MasterId);
					WindowsEventLoggingUtilities.WriteToInfoEvent("Here: " + Environment.NewLine + Str.Substring(0,1000), FModuleName, MasterId);
					LoggingEntry(Method, Str, "");
					WindowsEventLoggingUtilities.WriteToInfoEvent("(5)", FModuleName, MasterId);

					/*
					Str =
						JsonUtilities.StartJson() +
						Entries.ToString() +
						JsonUtilities.EndJson();				
				
					WindowsEventLoggingUtilities.WriteToInfoEvent("JsonText: " + Environment.NewLine + Str, FModuleName, MasterId);
					*/
					
					try 
					{
						//if (Entries.Length > 0)
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
					/*
					JObject jobj = JsonConvert.DeserializeObject<JObject>(Str);
					if (jobj == null)
					{
						ErrorMsg = "Error deserializing the Request";
					}
					else
					{
						JObject SystemRep = (jobj["SystemReport"] as JObject);
						if (SystemRep == null)
						{
							ErrorMsg = "Missing SystemRep in Json Content.";
						}
						else 
						{
							Result = (string)SystemRep;
						}
					}
					*/
					string JPath = @"$.SystemReport";
					WindowsEventLoggingUtilities.WriteToInfoEvent("JPath: " + JPath, FModuleName, MasterId);
					JToken Record = JObj.SelectToken(JPath);
					if (Record == null)
						WindowsEventLoggingUtilities.WriteToInfoEvent("Record IS NULL", FModuleName, MasterId);
					else
					{
						WindowsEventLoggingUtilities.WriteToInfoEvent("Record: " + Record.ToString(), FModuleName, MasterId);
						Result = Record.ToString();
						WindowsEventLoggingUtilities.WriteToInfoEvent("Result: " + Result, FModuleName, MasterId);
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
	}
}
