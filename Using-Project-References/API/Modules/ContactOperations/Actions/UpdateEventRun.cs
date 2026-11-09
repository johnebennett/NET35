/*
 * Created by SharpDevelop.
 * User: John
 * Date: 15/03/2019
 * Time: 4:20 PM
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

namespace Actions.ContactOperations
{
	/// <summary>
	/// Update the EAEventRun record for the provided EventRunId
	/// </summary>
	public class UpdateEventRun : ActionBase
	{
		private string FModuleName = "ContactOperations";
		
		/*
		private string FRunId = "";
		private int FRunIdValue = 0;
		private string FTask = "";
		private int FTaskValue = 0;
		private string FWhoTo1 = "";
		private int FWhoTo1Value = 0;
		private string FWhoTo2 = "";
		private int FWhoTo2Value = 0;
		private string FDistinctClientsOnly = "";
		private string FAddClientsAsExcluded = "";
		private string FEmailOption = "";
		private int FEmailOptionValue = 0;
		private string FEmailTemplateId = "";
		private int FEmailTemplateIdValue = 0;
		
		private string FSubject = "";
		private string FEmailBody = "";

		private string FFromId = "";
		private int FFromIdValue = 0;
		private string FCCId = "";
		private int FCCIdValue = 0;
		private string FBccId = "";
		private int FBccIdValue = 0;
		private string FPrintToId = "";
		private int FPrintToIdValue = 0;
		private string FReportEventId = "";
		private int FReportEventIdValue = 0;
		private string FTestEmail = "";
		private string FEmailSignatureId = "";
		private int FEmailSignatureIdValue = 0;
		*/
		
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

			UpdateEventRun inst = new UpdateEventRun();
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
				/*
				FRunId = (string)DataObject["runId"];
				FRunIdValue = StringUtilities.StrToIntDef(FRunId, -1);
				FTask = (string)DataObject["task"];
				FTaskValue = StringUtilities.StrToIntDef(FTask, -1);
				FWhoTo1 = (string)DataObject["whoTo1"];
				FWhoTo1Value = StringUtilities.StrToIntDef(FWhoTo1, -1);
				FWhoTo2 = (string)DataObject["whoTo2"];
				FWhoTo2Value = StringUtilities.StrToIntDef(FWhoTo2, -1);
				FDistinctClientsOnly = (string)DataObject["distinct"];
				FAddClientsAsExcluded = (string)DataObject["excluded"];
				FEmailOption = (string)DataObject["emailOption"];
				FEmailOptionValue = StringUtilities.StrToIntDef(FEmailOption, -1);
				FEmailTemplateId = (string)DataObject["templateId"];
				FEmailTemplateIdValue = StringUtilities.StrToIntDef(FEmailTemplateId, -1);				
				FSubject = (string)DataObject["subject"];
				FEmailBody = (string)DataObject["emailBody"];		
				FFromId = (string)DataObject["from"];
				FFromIdValue = StringUtilities.StrToIntDef(FFromId, -1);
				FCCId = (string)DataObject["cc"];
				FCCIdValue = StringUtilities.StrToIntDef(FCCId, -1);
				FBccId = (string)DataObject["bcc"];
				FBccIdValue = StringUtilities.StrToIntDef(FBccId, -1);
				FPrintToId = (string)DataObject["print"];
				FPrintToIdValue = StringUtilities.StrToIntDef(FPrintToId, -1);
				FReportEventId = (string)DataObject["reportEventId"];
				FReportEventIdValue = StringUtilities.StrToIntDef(FReportEventId, -1);
				FTestEmail = (string)DataObject["testEmail"];
				FEmailSignatureId = (string)DataObject["emailSignatureId"];
				FEmailSignatureIdValue = StringUtilities.StrToIntDef(FEmailSignatureId, -1);
				*/
				
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
				/*
				if (string.IsNullOrEmpty(Result) && (FRunIdValue < 0)) Result = "Invalid RunId provided";
				if (string.IsNullOrEmpty(Result) && (FTaskValue < 0)) Result = "Invalid Task Value provided";
				if (string.IsNullOrEmpty(Result) && (FWhoTo1Value < 0)) Result = "Invalid WhotTo1 provided";
				if (string.IsNullOrEmpty(Result) && (FWhoTo2Value < 0)) Result = "Invalid WhotTo2 provided";
				if (string.IsNullOrEmpty(Result) && (FWhoTo2Value < 0)) Result = "Invalid WhotTo2 provided";

				if (string.IsNullOrEmpty(Result) && (FEmailOptionValue < 0)) Result = "Invalid EmailOption Value provided";
				if (string.IsNullOrEmpty(Result) && (FEmailTemplateIdValue < 0)) Result = "Invalid EmailTemplateId Value provided";
				if (string.IsNullOrEmpty(Result) && (FFromIdValue < 0)) Result = "Invalid FromId Value provided";
				if (string.IsNullOrEmpty(Result) && (FCCIdValue < 0)) Result = "Invalid CCId Value provided";
				if (string.IsNullOrEmpty(Result) && (FBccIdValue < 0)) Result = "Invalid BccId Value provided";
				if (string.IsNullOrEmpty(Result) && (FPrintToIdValue < 0)) Result = "Invalid PrintToId Value provided";
				if (string.IsNullOrEmpty(Result) && (FEmailSignatureIdValue < 0)) Result = "Invalid Email Signature Id Value provided";

				if (string.IsNullOrEmpty(Result) &&
				    !((StringUtilities.SameText(FDistinctClientsOnly, "Y")) || (StringUtilities.SameText(FDistinctClientsOnly, "N")))
				   ) Result = "Invalid DistinctClientsOnly Value provided";
				if (string.IsNullOrEmpty(Result) && 
				    !(StringUtilities.SameText(FAddClientsAsExcluded, "Y") || StringUtilities.SameText(FAddClientsAsExcluded, "N"))
				   ) Result = "Invalid DistinctClientsOnly Value provided";
				if (string.IsNullOrEmpty(Result) && 
				    !(StringUtilities.SameText(FTestEmail, "Y") || StringUtilities.SameText(FTestEmail, "N"))
				   ) Result = "Invalid Test Email Value provided";
				*/
				
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
				/*
				Result =
					CommonMethods.UpdateEventRunRecord(
						this.FbConnectionString,
						FRunIdValue, FTaskValue, FWhoTo1Value, FWhoTo2Value,
						FDistinctClientsOnly, FAddClientsAsExcluded, FEmailOptionValue,
						FEmailTemplateIdValue, FSubject, FEmailBody,
						FFromIdValue, FCCIdValue, FBccIdValue, FPrintToIdValue, FTestEmail, FEmailSignatureIdValue);
				*/
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
					//RecipientsSql = GetRecipientsSql(FRunIdValue, out Result);
					Result = ExecuteEventEmailSql(FFormData.RunIdValue);
				}
				
				//if (string.IsNullOrEmpty(Result) && !string.IsNullOrEmpty(RecipientsSql))
				if (string.IsNullOrEmpty(Result))
				{
					//Result = BuildClientListOutputEntries(RecipientsSql, ref Entries);
					//Result = BuildClientListOutputEntries(FRunIdValue, ref Entries);
					Result = CommonMethods.BuildXEmailRecipientList(FbConnectionString, FFormData.RunIdValue, ref Entries);
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
