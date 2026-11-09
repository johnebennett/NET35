/*
 * Created by SharpDevelop.
 * User: John
 * Date: 09/06/2020
 * Time: 8:50 PM
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
namespace Actions.Portfolio.KeyNumber
{
	/// <summary>
	/// Description of DisplayPdfReport.
	/// </summary>
	public class DisplayPdfReport : ActionBase
	{
		private string FModuleName = "Portfolio";
		private DisplayPdfReportRequestItem FRequestItem;
		
		public static string Execute(int MasterId,
		                             APISettingsItem SettingsItem,
		                             string Request,
		                             out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			DisplayPdfReport inst = new DisplayPdfReport();
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
				FRequestItem = new DisplayPdfReportRequestItem();
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
				string ReportSource = "";
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ReportSource = GetReportSource(out ErrorMsg);
				}
				
				string OutputFileName = "";
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Int64 NewId =
						global::UsefulDBMethods.Reporting.SystemReportUtilities.Execute(
							FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue,
							FRequestItem.ReportId, ReportSource, out ErrorMsg, out OutputFileName);
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
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, FModuleName, MasterId);
					
			LogOnInsideMethod(Method);
			try 
			{
				StringBuilder Entries = new StringBuilder();
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg =
						CommonMethods.EATransactionKeys(
							FbConnectionString, CommonItem.CompanyIdValue,  
							FRequestItem.PortfolioId, FRequestItem.KeyNumber, FRequestItem.KeyNumber2, FRequestItem.KeyNumber3,
                            ref Entries);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					string Source =
						JsonUtilities.StartJson() +
						Entries.ToString() +
						JsonUtilities.EndJson();				
					string Path = "$.B" + FRequestItem.ReportId + "Value";
					Result = JsonUtilities.GetJsonPathValueStr(Source, Path, out ErrorMsg);
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