/*
 * Created by SharpDevelop.
 * User: John
 * Date: 21/02/2020
 * Time: 7:29 PM
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

namespace Actions.PaymentOperations
{
	/// <summary>
	/// Description of DisplayPmtPdfReport.
	/// </summary>
	public class DisplayPmtPdfReport : ActionBase
	{
		private string FModuleName = "PaymentOperations";		
		private DisplayPmtPdfReportRequestItem FRequestItem;
		
		public static string Execute(int MasterId,
		                             APISettingsItem SettingsItem,
		                             string Request,
		                             out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			DisplayPmtPdfReport inst = new DisplayPmtPdfReport();
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
				FRequestItem = new DisplayPmtPdfReportRequestItem();
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
				StatementRunItem RunItem = new StatementRunItem();
				ErrorMsg = RunItem.Load((EAOfficeAPI.ActionBase)this, FbConnectionString, CommonItem.CompanyIdValue, FRequestItem.RunId);

				string ReportSource = "";
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ReportSource = GetReportSource(RunItem, out ErrorMsg);
				}
				
				string OutputFileName = "";
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Int64 NewId =
						CommonUtilities.SystemReportUtilities.Execute(
							FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue,
							FRequestItem.PassedId, ReportSource, out ErrorMsg, out OutputFileName);
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
		
		private string GetReportSource(StatementRunItem RunItem,
		                               out string ErrorMsg)
		{
			string Result = "";
			ErrorMsg = "";
			string Method = "GetReportSource";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, FModuleName, MasterId);
					
			LogOnInsideMethod(Method);
			try 
			{
				StringBuilder Entries = new StringBuilder();
				
				ErrorMsg = 
					CommonMethods.BuildPaymentSearchGridResults(this,
						FbConnectionString, CommonItem.CompanyIdValue,
						RunItem, "STATEMENT-REPORT", "STATEMENTS-MANAGEMENTMANAGER", ref Entries);				
				if (!string.IsNullOrEmpty(ErrorMsg)) return Result;
				
				string Str =
					JsonUtilities.StartJson() +
					Entries.ToString() +
					JsonUtilities.EndJson();
				
				WindowsEventLoggingUtilities.WriteToInfoEvent("JsonText: " + Environment.NewLine + Str, FModuleName, MasterId);
					
				JObject JObj = null;
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
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					string JPath = @"$.Items[?(@.I_SUMMARY_ID == '" + FRequestItem.PassedId + "')]";
					WindowsEventLoggingUtilities.WriteToInfoEvent("JPath: " + JPath, FModuleName, MasterId);
					JToken Record = JObj.SelectToken(JPath);
					if (Record == null)
						WindowsEventLoggingUtilities.WriteToInfoEvent("Record IS NULL", FModuleName, MasterId);
					else
					{
						WindowsEventLoggingUtilities.WriteToInfoEvent("Record: " + Record.ToString(), FModuleName, MasterId);
						Result = (string)Record["ZL3_B" + FRequestItem.BtnId];				
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
