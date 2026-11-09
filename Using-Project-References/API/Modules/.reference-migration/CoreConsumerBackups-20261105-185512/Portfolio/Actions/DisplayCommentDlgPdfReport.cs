/*
 * Created by SharpDevelop.
 * User: John
 * Date: 27/03/2020
 * Time: 5:08 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.ComponentModel;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Serialization;
using UsefulDBMethods;

namespace Actions.Portfolio
{
	/// <summary>
	/// Description of DisplayCommentDlgPdfReport.
	/// </summary>
	public class DisplayCommentDlgPdfReport : ActionBase
	{
		private string FModuleName = "Portfolio";
		private  AddPortfolioCommentRequestItem FRequestItem;
		
		public static string Execute(int MasterId,
									 APISettingsItem SettingsItem,
									 string Request,
									 out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			DisplayCommentDlgPdfReport inst = new DisplayCommentDlgPdfReport();
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
				FRequestItem = new AddPortfolioCommentRequestItem();
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
			LogOnInsideMethod(Method);
			try 
			{
				Result = BuildOutput(out ErrorMsg);
				if (!string.IsNullOrEmpty(ErrorMsg))
					WindowsEventLoggingUtilities.WriteToErrorEvent(ErrorMsg, Method, MasterId);
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
		
		private string BuildOutput(out string ErrorMsg)
		{
			ErrorMsg = "";
			string Method = "BuildOutput";
			string Result = "";
			string OutputFileName = "";
			
			LogOnInsideMethod(Method);
			try
			{
				string ReportSource = GetReportSource(out ErrorMsg);
				if ((string.IsNullOrEmpty(ErrorMsg)) && (string.IsNullOrEmpty(ReportSource)))
				{
					ErrorMsg = "Missing or Empty ReportSource";
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{			
					Int64 NewId =
						SystemReportUtilities.Execute(
							FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue, FRequestItem.PortfolioId, ReportSource, 
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
				ErrorMsg = LogOnException(Method, Ex, "");
			}
			finally
			{
				LogOnLeavingMethod(Method);
			}
			
			return Result;
		}
		private string GetReportSource(out string ErrorMsg)
		{
			string Result = "";
			string Method = "GetReportSource";
			StringBuilder Entries = new StringBuilder();
			
			LogOnInsideMethod(Method);
			try
			{
				ErrorMsg =
					CommonMethods.Portfolio_EANotesAddGroup(
						FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue, FRequestItem.PortfolioId,
						FRequestItem.CategoryOnlyId, FRequestItem.NoteToAdd, FRequestItem.DefaultArrears, ref Entries);
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
					Result = (string)JObj["ZL20_B1"];
					WindowsEventLoggingUtilities.WriteToInfoEvent("Result: " + Result, FModuleName, MasterId);
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