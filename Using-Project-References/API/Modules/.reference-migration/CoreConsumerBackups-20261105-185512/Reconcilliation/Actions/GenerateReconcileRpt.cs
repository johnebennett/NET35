/*
 * Created by SharpDevelop.
 * User: John
 * Date: 01/04/2020
 * Time: 1:42 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CommonUtilities;
using EAOfficeAPI;

namespace Actions.Reconcilliation
{
	/// <summary>
	/// Description of GenerateReconcileRpt.
	/// </summary>
	public class GenerateReconcileRpt : ActionBase
	{
		private string FModuleName = "Reconcilliation";
		private GenerateReconcileRptRequestItem FRequestItem;

		public static string Execute(int MasterId,
									 APISettingsItem SettingsItem,
									 string Request,
									 out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			GenerateReconcileRpt inst = new GenerateReconcileRpt();
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
				FRequestItem = new GenerateReconcileRptRequestItem();
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
				string ReportSource = GetReportSource(out ErrorMsg);
				
				string OutputFileName = "";
				if ((string.IsNullOrEmpty(ErrorMsg)) && (!string.IsNullOrEmpty(ReportSource)))
				{
					Int64 NewId =
						SystemReportUtilities.Execute(
							FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue, FRequestItem.RecordId, ReportSource, 
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
			ErrorMsg = "";
			string Method = "GetReportSource";
			LogOnInsideMethod(Method);
			
			try
			{
				StringBuilder Entries = new StringBuilder();
				
				ErrorMsg =
					CommonMethods.EABankEntriesGroup(
						FbConnectionString, CommonItem.CompanyIdValue, FRequestItem.BankId, FRequestItem.BankType, 
						FRequestItem.PageNo, FRequestItem.PresentedPageNo, ref Entries);
				
				string Str = "";
				JObject JObj = null;
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Str =
						JsonUtilities.StartJson() +
						Entries.ToString() +
						JsonUtilities.EndJson();
			
					WindowsEventLoggingUtilities.WriteToInfoEvent("JsonText: " + Environment.NewLine + Str, FModuleName, MasterId);
					
					if (Entries.Length > 0)
					{
						JObj = JObject.Parse(Str);
					}
				}
			
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					string JPath = @"$.gridEntries[?(@.ID == '" + FRequestItem.RecordId + "')]";
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
			}
			catch (Exception Ex)
			{
				ErrorMsg = LogOnException(Method, Ex, "");
			}
			finally
			{
				LogOnLeavingMethod(Method);
			}
			
			return Result;
		}
	}
}
