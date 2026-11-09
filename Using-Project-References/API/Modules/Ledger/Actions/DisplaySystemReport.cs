/*
 * Created by SharpDevelop.
 * User: John
 * Date: 30/11/2019
 * Time: 10:41 PM
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

namespace Actions.Ledger
{
	/// <summary>
	/// Description of DisplaySystemReport.
	/// </summary>
	public class DisplaySystemReport : ActionBase
	{
		private string FModuleName = "Ledger";
		private SetupPageEntriesRequestItem FRequestItem;
		private int FPassedId = 0;
		private int FButtonId = 0;
		
		public static string Execute(int MasterId,
		                             APISettingsItem SettingsItem,
		                             string Request,
		                             out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			DisplaySystemReport inst = new DisplaySystemReport();
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
				if (string.IsNullOrEmpty(Result))
				{
					object Obj = GetCollectionValue("Data", "buttonId");
					FButtonId = (Obj == null) ? -1 : Convert.ToInt32(Obj);
					Obj = GetCollectionValue("Data", "passedId");
					FPassedId = (Obj == null) ? -1 : Convert.ToInt32(Obj);
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
	                string BaseFormat = "{0} is {1}";
	                string Key = "";
	                
	                if (string.IsNullOrEmpty(Result))
	                {
	                    Key = "ButtonId";
	                    if (FButtonId < 0) Result = string.Format(BaseFormat, Key, "NotProvided or Invalid");
	                }
	                
	                if (string.IsNullOrEmpty(Result))
	                {
	                    Key = "PassedId";
	                    if (FPassedId < 0) Result = string.Format(BaseFormat, Key, "NotProvided or Invalid");
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
			ErrorMsg = "";
			string Method = "BuildOutput";
			string Result = "";
			string OutputFileName = "";
			string ReportSource = GetReportSource(out ErrorMsg);
			if ((string.IsNullOrEmpty(ErrorMsg)) && (string.IsNullOrEmpty(ReportSource)))
			{
				ErrorMsg = "Missing or Empty ReportSource";
			}
			
			if (string.IsNullOrEmpty(ErrorMsg))
			{			
				Int64 NewId =
					global::UsefulDBMethods.Reporting.SystemReportUtilities.Execute(
						FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue, FPassedId, ReportSource, 
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
			return Result;
		}
		
		private string GetReportSource(out string ErrorMsg)
		{
			string Method = "BuildOutputEntries";
			string Result = "";			
			ErrorMsg = "";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, FModuleName, MasterId);
			
			LogOnInsideMethod(Method);
			try
			{
				StringBuilder Entries = new StringBuilder();
				ErrorMsg = 
					CommonMethods.BuildLedgerScreenGroupEntries(
						FbConnectionString, CommonItem.CompanyIdValue, 
						FRequestItem.BranchId, FRequestItem.ManagerId, FRequestItem.LedgerId, FRequestItem.EventId,
						FRequestItem.ReportId, FRequestItem.ActiveStatus, FRequestItem.RunNumber, CommonItem.ClientIdValue,
						FRequestItem.ScreenOrBulk, FRequestItem.GSTRegistered, FRequestItem.FindText, 
						FRequestItem.DateFrom, FRequestItem.DateTo, FRequestItem.DateShow, FRequestItem.OnlyShowPDFEntreis, ref Entries);

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
					string JPath = @"$.grid[?(@.I_ID == '" + FPassedId + "')]";
					WindowsEventLoggingUtilities.WriteToInfoEvent("JPath: " + JPath, FModuleName, MasterId);
					JToken Record = JObj.SelectToken(JPath);
					if (Record == null)
						WindowsEventLoggingUtilities.WriteToInfoEvent("Record IS NULL", FModuleName, MasterId);
					else
					{
						WindowsEventLoggingUtilities.WriteToInfoEvent("Record: " + Record.ToString(), FModuleName, MasterId);
						Result = (string)Record["ZL20_B" + FButtonId];				
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
