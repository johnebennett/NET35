/*
 * Created by SharpDevelop.
 * User: John
 * Date: 20/04/2019
 * Time: 5:12 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Reflection;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CommonUtilities;
using EAOfficeAPI;

namespace Actions.ContactOperations
{
	/// <summary>
	/// Update the EAEventRun record for the provided EventRunId
	/// </summary>
	public class ProcessRun : ActionBase
	{
		private string FModuleName = "ContactOperations";
		private ContactEmailFormData FFormData = new ContactEmailFormData();

		public static string Execute(int MasterId,
		                             RequestCommonItem CommonItem,
		                             JObject DataObject,
		                             APISettingsItem SettingsItem,
		                             out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			ProcessRun inst = new ProcessRun();
			if (!inst.Setup(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg))
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
				Result = FFormData.Load(DataObject);
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
				Result = FFormData.Validate();
				
				if (!string.IsNullOrEmpty(Result))
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent("Validation Failed: " + Result, FModuleName, MasterId);
					Result =
						JsonUtilities.StartJson() +
						JsonUtilities.AddPropertyNameAndValue("Error", "Validation Failed", false) +
						JsonUtilities.EndJson();
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
			string Result = "";
			string Method = "BuildOutput";
			
			LogOnInsideMethod(Method);
			try
			{
				WindowsEventLoggingUtilities.WriteToInfoEvent("Inside BuildOutput", FModuleName, MasterId);
				
				StringBuilder entries = new StringBuilder();
				entries.AppendLine(JsonUtilities.StartJson());
					
				ErrorMsg = BuildOutputEntries(ref entries);				
				if (!string.IsNullOrEmpty(ErrorMsg)) return "";
				
				// create an eventrun record				
				/*
				EventRunId =
					UsefulDBMethods.EventHelpers.AddEmailEvent(
						this.FbConnectionString, CommonItem.CompanyIdValue, FReportEventIdValue, "Contact-Email", 
						CommonItem.ClientIdValue, out ErrorMsg);
				if (!string.IsNullOrEmpty(ErrorMsg)) return "";				
				
				// pass the eventrunId in the results set
				entries.Append(",");
				entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("EventRunId", EventRunId, false));
				
				// create the xemail_eventrunId_ table
				ErrorMsg = CommonMethods.CreateXEmailTableForRunId(this.FbConnectionString, EventRunId);
				if (!string.IsNullOrEmpty(ErrorMsg)) return "";
				*/
				
				entries.AppendLine(JsonUtilities.EndJson());
				Result = entries.ToString();
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
		
		private string BuildOutputEntries(ref StringBuilder Entries)
		{
			string Method = "BuildOutputEntries";
			string Result = "";			
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, FModuleName, MasterId);
			
			LogOnInsideMethod(Method);
			try
			{
				Result = 
					CommonMethods.UpdateEventRunRecord(
						this.FbConnectionString,
						FFormData.RunIdValue, FFormData.TaskValue, FFormData.WhoTo1Value, FFormData.WhoTo2Value,
						FFormData.DistinctClientsOnly, FFormData.AddClientsAsExcluded, FFormData.EmailOptionValue,
						FFormData.EmailTemplateIdValue, FFormData.Subject, FFormData.EmailBody,
						FFormData.FromIdValue, FFormData.CCIdValue, FFormData.BccIdValue, FFormData.PrintToIdValue, 
						FFormData.TestEmail, FFormData.EmailSignatureIdValue);

                if (string.IsNullOrEmpty(Result))
                {
                    Result =
                        EmailStatements.CommonMethods.UpdateEmailBodyForEventRunId(
                            FbConnectionString, FFormData.RunIdValue, FFormData.EmailBody);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result =
                        CommonMethods.UpdateSignatureIdForEventRunId(
                        FbConnectionString, FFormData.RunIdValue, FFormData.EmailSignatureIdValue);
                }

                if (string.IsNullOrEmpty(Result))
				{
					Result = 
						CommonMethods.ExecuteEventRunProcess(
							FbConnectionString, FFormData.RunIdValue, CommonItem.CompanyIdValue);
					WindowsEventLoggingUtilities.WriteToInfoEvent("After calling ExecuteEventRunProcess", FModuleName, MasterId);
				}
				
				// Call the EmailProcessor to handle the sending of the emails
				/*
				string WorkingDir = "";
				WindowsEventLoggingUtilities.WriteToInfoEvent("Before calling GetCurrentDirectory", FModuleName, MasterId);
                WorkingDir = CommonUtilities.FolderHelpers.GetAssemblyDirectoryName(Assembly.GetExecutingAssembly());
                string SepChar = Path.DirectorySeparatorChar.ToString();
				if (!WorkingDir.EndsWith(SepChar)) WorkingDir += SepChar;
				WindowsEventLoggingUtilities.WriteToInfoEvent("After calling GetCurrentDirectory", FModuleName, MasterId);
				WindowsEventLoggingUtilities.WriteToInfoEvent("WorkingDir " + WorkingDir, FModuleName, MasterId);
				try
				{
					Process runApp = new Process();
                    runApp.StartInfo.FileName = WorkingDir + "EmailProcessorHtml";
                    runApp.StartInfo.Arguments = FFormData.RunId;
					runApp.Start();
				}
				*/
				try 
				{
					Result = CommonMethods.SetNewEmailStatus(FbConnectionString, FFormData.RunId);
				}
				catch (Exception Ex)
				{
					Result = LogOnException(Method, Ex, "");
				}
				finally
				{
					//Directory.SetCurrentDirectory(WorkingDir);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					StringBuilder DataAsJson = new StringBuilder();
					DataAsJson.Append(JsonUtilities.StartJson());
					DataAsJson.Append(JsonUtilities.StartJsonObject("result"));
					DataAsJson.Append(JsonUtilities.AddPropertyNameAndValue("RunId", FFormData.RunId));
					DataAsJson.Append(JsonUtilities.AddPropertyNameAndValue("Status", "Success"));
					DataAsJson.Append(JsonUtilities.EndJsonObject());
					DataAsJson.Append(JsonUtilities.EndJson());
					Result = LogResponse(Method, DataAsJson.ToString());
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
		
		private string ExecuteEventEmailSql(int RunId)
		{
			string Result = "";
			string Method = "ExecuteEventEmailSql";

			string QueryText = "EXECUTE PROCEDURE EAEVENTEMAILSQL(@RunId)";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("@RunId", RunId));
			WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText:" + Environment.NewLine + QueryText, FModuleName, MasterId);
			
			WindowsEventLoggingUtilities.WriteToInfoEvent("Before EXECUTE PROCEDURE EAEVENTEMAILSQL", FModuleName, MasterId);
			Result = FirebirdUtilities.DBExecute(FbConnectionString, QueryText, Params, 0);
			WindowsEventLoggingUtilities.WriteToInfoEvent("After EXECUTE PROCEDURE EAEVENTEMAILSQL", FModuleName, MasterId);
			WindowsEventLoggingUtilities.WriteToInfoEvent("Result: " + Result, FModuleName, MasterId);
			if (!string.IsNullOrEmpty(Result))
			{
				Result = new ActionBase().LogOnProcessingError(Method, Result, "");
			}

			return Result;
		}
		
		private string GetRecipientsSql(int RunId,
		                                out string ErrorMsg)
		{
			ErrorMsg = "";
			string Result = "";
			string QueryText = "SELECT ClientListSql FROM EAEVENTEMAILSQL(@RunId)";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("@RunId", RunId));
			WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText:" + Environment.NewLine + QueryText, FModuleName, MasterId);
			
			DataTable dt = FirebirdUtilities.GetFirstDataTable(FbConnectionString, QueryText, Params, 0);
			if (dt == null)
			{
				ErrorMsg = "No data table returned";
				WindowsEventLoggingUtilities.WriteToErrorEvent(ErrorMsg, FModuleName, MasterId);
			}
			else if (dt.Rows.Count == 0)
			{
				ErrorMsg = "No record returned for the RunId: " + Convert.ToString(RunId);
				WindowsEventLoggingUtilities.WriteToErrorEvent(ErrorMsg, FModuleName, MasterId);
			}
			else if (dt.Rows.Count > 0)
			{
				foreach (DataRow dataRow in dt.Rows) 
				{
					Result = (dataRow[0] == DBNull.Value) ? "" : Convert.ToString(dataRow[0]);
				}
				WindowsEventLoggingUtilities.WriteToInfoEvent("ClientListSql:" + Environment.NewLine + Result, FModuleName, MasterId);
			}
			
			return Result;
		}
		
		private string BuildClientListOutputEntries(int RunId,
		                                            ref StringBuilder Entries)
		{
			string Result = "";
			List<string> RowList = new List<string>();
			List<string> StrList = new List<string>();
			string ColName = "";
			string ColValue = "";
			string Str = "";
			string QueryText = "SELECT * FROM XEMAIL" + RunId;
			WindowsEventLoggingUtilities.WriteToErrorEvent("QueryText: " + QueryText, FModuleName, MasterId);
			
			DataTable dt = FirebirdUtilities.GetFirstDataTable(FbConnectionString, QueryText);
			if (dt == null)
			{
				Result = "No data table returned";
				WindowsEventLoggingUtilities.WriteToErrorEvent(Result, FModuleName, MasterId);
			}
			else if (dt.Rows.Count == 0)
			{
				Result = "No record returned for the query";
				WindowsEventLoggingUtilities.WriteToErrorEvent(Result, FModuleName, MasterId);
			}
			else if (dt.Rows.Count > 0)
			{
	            Entries.Append(CommonUtilities.JsonUtilities.StartJsonArray("Items"));
				RowList.Clear();				
				object Obj = null;
				Str = "";
				foreach(DataRow ThisRow in dt.Rows)
				{
					StrList.Clear();
					foreach(DataColumn ThisCol in dt.Columns)
					{
						ColName = ThisCol.ColumnName;
						Obj = ThisRow[ColName];
						ColValue = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
						StrList.Add(JsonUtilities.AddPropertyNameAndValue(ColName, ColValue, false));
					}
					Str =
						JsonUtilities.StartJsonObject("") +
						string.Join(",", StrList.ToArray()) +
						JsonUtilities.EndJsonObject();
					RowList.Add(Str);
				}
				
				Entries.Append(string.Join(",", RowList.ToArray()));
	            Entries.Append(CommonUtilities.JsonUtilities.EndJsonArray());
			}
			
			return Result;
		}
	}
}
