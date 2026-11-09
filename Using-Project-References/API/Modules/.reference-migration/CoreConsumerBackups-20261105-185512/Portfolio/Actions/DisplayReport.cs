/*
 * Created by SharpDevelop.
 * User: John
 * Date: 31/10/2019
 * Time: 2:57 PM
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
using UsefulDBMethods;

namespace Actions.Portfolio
{
	/// <summary>
	/// Description of DisplayReport.
	/// </summary>
	public class DisplayReport : ActionBase
	{
		private string FModuleName = "Portfolio";
		private  DisplayReportRequestItem FRequestItem;
		
		public static string Execute(int MasterId,
									 APISettingsItem SettingsItem,
									 string Request,
									 out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			DisplayReport inst = new DisplayReport();
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
				FRequestItem = new DisplayReportRequestItem();
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
				//Result = "{\"message\": \"hello\"}";
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
			string ReportSource = GetReportSource(out ErrorMsg);
			if ((string.IsNullOrEmpty(ErrorMsg)) && (string.IsNullOrEmpty(ReportSource)))
			{
				ErrorMsg = "Missing or Empty ReportSource";
			}
			
			if (string.IsNullOrEmpty(ErrorMsg))
			{			
				Int64 NewId =
					SystemReportUtilities.Execute(
						FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue, FRequestItem.PassedId, ReportSource, 
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
			string Result = "";
			ErrorMsg = "";
			
			switch (FRequestItem.DlgType)
			{
				case "Contacts":
					Result = GetContactsReportSource(out ErrorMsg);
					break;
				case "Tasks":
					Result = GetTasksReportSource(out ErrorMsg);
					break;
				case "Financials":
					Result = GetFinancialsReportSource(out ErrorMsg);
					break;
				case "Attachments":
					Result = GetAttachmentsReportSource(out ErrorMsg);
					break;
				case "OCReport":
					Result = GetOCReportSource(out ErrorMsg);
					break;
				case "OCCertReport":
					Result = GetOCCertReportSource(out ErrorMsg);
					break;
			}
			
			return Result;
		}
		
		private string GetContactsReportSource(out string ErrorMsg)
		{
			string Result = "";
			string Method = "GetContactsReportSource";
			StringBuilder Entries = new StringBuilder();
			ErrorMsg = 
				Actions.ComboBoxEntries.CommonMethods.BuildOutput_PortfolioContacts(
					FbConnectionString, CommonItem.CompanyIdValue, FRequestItem.PortfolioId, FRequestItem.CategoryOnlyId, ref Entries);
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
				string JPath = @"$.gridEntries[?(@.I_ID == '" + FRequestItem.PassedIdValue + "')]";
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
			
			return Result;
		}
		
		private string GetTasksReportSource(out string ErrorMsg)
		{
			string Result = "";
			string Method = "GetTasksReportSource";
			StringBuilder Entries = new StringBuilder();
			ErrorMsg = 
				Actions.ComboBoxEntries.CommonMethods.BuildOutput_PortfolioTasks(
					FbConnectionString, CommonItem.CompanyIdValue, FRequestItem.PortfolioId, FRequestItem.CategoryOnlyId, ref Entries);
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
				string JPath = @"$.gridEntries[?(@.I_ID == '" + FRequestItem.PassedIdValue + "')]";
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
			
			return Result;
		}
		
		private string GetFinancialsReportSource(out string ErrorMsg)
		{
			string Result = "";
			string Method = "GetFinancialsReportSource";
			StringBuilder Entries = new StringBuilder();
			ErrorMsg = 
				Actions.ComboBoxEntries.CommonMethods.BuildOutput_PortfolioFinancials(
					FbConnectionString, CommonItem.CompanyIdValue, FRequestItem.PortfolioId, FRequestItem.CategoryOnlyId, ref Entries);
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
				string JPath = @"$.gridEntries[?(@.I_ID == '" + FRequestItem.PassedIdValue + "')]";
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
			
			return Result;
		}
		
		private string GetAttachmentsReportSource(out string ErrorMsg)
		{
			string Result = "";
			string Method = "GetAttachmentsReportSource";
			StringBuilder Entries = new StringBuilder();
			ErrorMsg = 
				Actions.ComboBoxEntries.CommonMethods.BuildOutput_PortfolioStorage(
					FbConnectionString, CommonItem.CompanyIdValue, FRequestItem.PortfolioId, FRequestItem.CategoryOnlyId, ref Entries);
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
				string JPath = @"$.gridEntries[?(@.I_ID == '" + FRequestItem.PassedIdValue + "')]";
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
			
			return Result;
		}
		
		private string GetOCReportSource(out string ErrorMsg)
		{
			string Result = "";
			string Method = "GetOCReportSource";
			int AccountId = StringUtilities.StrToIntDef(Convert.ToString(this.GetCollectionValue("Data", "AccountId")), -1);
			int SubAccountId = FRequestItem.CategoryOnlyId;
			string DateFrom = Convert.ToString(this.GetCollectionValue("Data", "DateFrom"));
			string DateTo = Convert.ToString(this.GetCollectionValue("Data", "DateTo"));
			StringBuilder Entries = new StringBuilder();
			ErrorMsg = 
				Actions.ComboBoxEntries.CommonMethods.BuildOutput_PortfolioOwnersCorpReports(
					FbConnectionString, CommonItem.CompanyIdValue, AccountId, SubAccountId, DateFrom, DateTo, ref Entries);
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
				string JPath = @"$.gridEntries[?(@.Value == '" + FRequestItem.PassedIdValue + "')]";
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
			
			return Result;
		}
		
		private string GetOCCertReportSource(out string ErrorMsg)
		{
			string Result = 
				CommonMethods.GetOCCertReportSource(
					FbConnectionString, CommonItem.CompanyIdValue, FRequestItem.PassedId, FRequestItem.CategoryOnlyId, CommonItem.ClientIdValue, out ErrorMsg);
			/*
			string Result = "";
			string Method = "GetOCCertReportSource";
			StringBuilder Entries = new StringBuilder();
			ErrorMsg = 
				CommonMethods.EAOCCertificateGroup(
					FbConnectionString, CommonItem.CompanyIdValue, FRequestItem.PassedId, FRequestItem.CategoryOnlyId, CommonItem.ClientIdValue, ref Entries);
			string Str = "";
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
						JObject jobj = JsonConvert.DeserializeObject<JObject>(Str);
						if (jobj == null)
						{
							Result = "Error deserializing the Request";
						}
						else
						{
							Result = (string)jobj["certificate_reportsource"];
							if (string.IsNullOrEmpty(Result)) ErrorMsg = "Error getting report source";
						}
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
			*/
			
			return Result;
		}
	}
}