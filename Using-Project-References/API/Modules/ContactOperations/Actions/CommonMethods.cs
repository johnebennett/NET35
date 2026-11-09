/*
 * Created by SharpDevelop.
 * User: John
 * Date: 15/03/2019
 * Time: 10:53 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Diagnostics;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CommonUtilities;
using EAOfficeAPI;
using System.Web;
using System.IO;
using System.Reflection;

namespace Actions.ContactOperations
{
	/// <summary>
	/// Class that contains all of the form data passed from the 
	/// Contact\Email page
	/// </summary>
	/// 
	public class ContactEmailFormData
	{
		public string RunId { get; set; }
		public int RunIdValue { get; set; }
		public string Task { get; set; }
		public int TaskValue { get; set; }
		public string WhoTo1 { get; set; }
		public int WhoTo1Value { get; set; }
		public string WhoTo2 { get; set; }
		public int WhoTo2Value { get; set; }
		public string DistinctClientsOnly { get; set; }
		public string AddClientsAsExcluded { get; set; }
		public string EmailOption { get; set; }
		public int EmailOptionValue { get; set; }
		public string EmailTemplateId { get; set; }
		public int EmailTemplateIdValue { get; set; }
		
		public string Subject { get; set; }
		public string EmailBody { get; set; }

		public string FromId { get; set; }
		public int FromIdValue { get; set; }
		public string CCId { get; set; }
		public int CCIdValue { get; set; }
		public string BccId { get; set; }
		public int BccIdValue { get; set; }
		public string PrintToId { get; set; }
		public int PrintToIdValue { get; set; }
		public string ReportEventId { get; set; }
		public int ReportEventIdValue { get; set; }
		public string TestEmail { get; set; }
		public string EmailSignatureId { get; set; }
		public int EmailSignatureIdValue { get; set; }
		
		private string FModuleName = "ContactEmailFormData";
		
		public string Load(JObject Source)
		{
			string Result = "";
			try 
			{
				RunId = (string)Source["runId"];
				RunIdValue = StringUtilities.StrToIntDef(RunId, -1);
				Task = (string)Source["task"];
				TaskValue = StringUtilities.StrToIntDef(Task, -1);
				WhoTo1 = (string)Source["whoTo1"];
				WhoTo1Value = StringUtilities.StrToIntDef(WhoTo1, -1);
				WhoTo2 = (string)Source["whoTo2"];
				WhoTo2Value = StringUtilities.StrToIntDef(WhoTo2, -1);
				DistinctClientsOnly = (string)Source["distinct"];
				AddClientsAsExcluded = (string)Source["excluded"];
				EmailOption = (string)Source["emailOption"];
				EmailOptionValue = StringUtilities.StrToIntDef(EmailOption, -1);
				EmailTemplateId = (string)Source["templateId"];
				EmailTemplateIdValue = StringUtilities.StrToIntDef(EmailTemplateId, -1);				
				Subject = (string)Source["subject"];
				EmailBody = (string)Source["emailBody"];		
				FromId = (string)Source["from"];
				FromIdValue = StringUtilities.StrToIntDef(FromId, -1);
				CCId = (string)Source["cc"];
				CCIdValue = StringUtilities.StrToIntDef(CCId, -1);
				BccId = (string)Source["bcc"];
				BccIdValue = StringUtilities.StrToIntDef(BccId, -1);
				PrintToId = (string)Source["print"];
				PrintToIdValue = StringUtilities.StrToIntDef(PrintToId, -1);
				ReportEventId = (string)Source["reportEventId"];
				ReportEventIdValue = StringUtilities.StrToIntDef(ReportEventId, -1);
				TestEmail = (string)Source["testEmail"];
				EmailSignatureId = (string)Source["emailSignatureId"];
				EmailSignatureIdValue = StringUtilities.StrToIntDef(EmailSignatureId, -1);
			}
			catch (Exception Ex)
			{
				Result = Ex.Message + Environment.NewLine + Ex.StackTrace;
			}
			
			return Result;
		}
		
		public string Validate()
		{
			string Result = "";
			string Method = "Validate";
			try 
			{
				if (string.IsNullOrEmpty(Result) && (RunIdValue < 0)) Result = "Invalid RunId provided";
				if (string.IsNullOrEmpty(Result) && (TaskValue < 0)) Result = "Invalid Task Value provided";
				if (string.IsNullOrEmpty(Result) && (WhoTo1Value < 0)) Result = "Invalid WhotTo1 provided";
				if (string.IsNullOrEmpty(Result) && (WhoTo2Value < 0)) Result = "Invalid WhotTo2 provided";
				if (string.IsNullOrEmpty(Result) && (WhoTo2Value < 0)) Result = "Invalid WhotTo2 provided";

				if (string.IsNullOrEmpty(Result) && (EmailOptionValue < 0)) Result = "Invalid EmailOption Value provided";
				if (string.IsNullOrEmpty(Result) && (EmailTemplateIdValue < 0)) Result = "Invalid EmailTemplateId Value provided";
				if (string.IsNullOrEmpty(Result) && (FromIdValue < 0)) Result = "Invalid FromId Value provided";
				if (string.IsNullOrEmpty(Result) && (CCIdValue < 0)) Result = "Invalid CCId Value provided";
				if (string.IsNullOrEmpty(Result) && (BccIdValue < 0)) Result = "Invalid BccId Value provided";
				if (string.IsNullOrEmpty(Result) && (PrintToIdValue < 0)) Result = "Invalid PrintToId Value provided";
				if (string.IsNullOrEmpty(Result) && (EmailSignatureIdValue < 0)) Result = "Invalid Email Signature Id Value provided";

				if (string.IsNullOrEmpty(Result) &&
				    !((StringUtilities.SameText(DistinctClientsOnly, "Y")) || (StringUtilities.SameText(DistinctClientsOnly, "N")))
				   ) Result = "Invalid DistinctClientsOnly Value provided";
				if (string.IsNullOrEmpty(Result) && 
				    !(StringUtilities.SameText(AddClientsAsExcluded, "Y") || StringUtilities.SameText(AddClientsAsExcluded, "N"))
				   ) Result = "Invalid DistinctClientsOnly Value provided";
				if (string.IsNullOrEmpty(Result) && 
				    !(StringUtilities.SameText(TestEmail, "Y") || StringUtilities.SameText(TestEmail, "N"))
				   ) Result = "Invalid Test Email Value provided";
				
				if (!string.IsNullOrEmpty(Result))
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent("Validation Failed: " + Result, FModuleName, 0);
					Result =
						JsonUtilities.StartJson() +
						JsonUtilities.AddPropertyNameAndValue("Error", "Validation Failed", false) +
						JsonUtilities.EndJson();
				}
			}
			catch (Exception Ex)
			{
				Result = new ActionBase().LogOnException(FModuleName + "." + Method, Ex, "");
			}
			return Result;
		}
	}
	
	/// <summary>
	/// Description of CommonMethods.
	/// </summary>
	public class CommonMethods
	{
		private string FModuleName = "ContactOperations";
		
		public CommonMethods()
		{
		}

        public static string GetModuleName() { return "ContactOperations."; }

        public static string CreateXEmailTableForRunId(string ConnectionString,
		                                               int RunId)
		{
			string Result = "";
			string Method = "CreateXEmailTableForRunId";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, "ContactOperations", 0);
			WindowsEventLoggingUtilities.WriteToInfoEvent("RunId " + RunId.ToString(), "ContactOperations", 0);
			
			CommonMethods inst = new CommonMethods();
			string ModuleName = inst.FModuleName;
			
			try 
			{
				string QueryText = "EXECUTE PROCEDURE CREATE_WORKING_XEMAIL_CLIENTS(@RunId)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("RunId", RunId));
				WindowsEventLoggingUtilities.WriteToInfoEvent("Before creating XEMAIL Table", "ContactOperations", 0);
				Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
				WindowsEventLoggingUtilities.WriteToInfoEvent("After creating XEMAIL Table", "ContactOperations", 0);
				WindowsEventLoggingUtilities.WriteToInfoEvent("Result: " + Result, "ContactOperations", 0);
				if (!string.IsNullOrEmpty(Result))
				{
					Result = new ActionBase().LogOnErrorSelectingData(Method, Result, QueryText);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					// check to see if the table exists
					// Table should be named XEMAIL_RunId_
					QueryText = "SELECT * FROM XEMAIL" + Convert.ToString(RunId);
					DataTable dt = FirebirdUtilities.GetFirstDataTable(ConnectionString, QueryText);
					if (dt == null) Result = "No table returned.";
				}
			}
			catch (Exception Ex)
			{
				Result =  new ActionBase().LogOnException(Method, Ex, "");
			}
			
			return Result;
		}
		
		public static string UpdateEventRunRecord(string ConnectionString,
                                                  int RunId,
                                                  int TaskId,
                                                  int WhoTo1Id,
                                                  int WhoTo2Id,
                                                  string DistinctClientsOnly,
                                                  string AddClientsAsExcluded,
                                                  int EmailOption,
                                                  int EmailTemplateId,
                                                  string Subject,
                                                  string EmailBody,
                                                  int FromId,
                                                  int CCId,
                                                  int BccId,
                                                  int PrintToId,
                                                  string TestEmail,
                                                  int EmailSignatureId)
		{
			string Result = "";
			string Msg =
				"Inside UpdateEventRunRecord" + Environment.NewLine +
				"TemplateId: " + EmailTemplateId.ToString() + Environment.NewLine +
				"Subject: " + Subject + Environment.NewLine +
				"EmailBody: " + EmailBody;
			WindowsEventLoggingUtilities.WriteToInfoEvent(Msg, "ContactOperations - UpdateEventRunRecord", 0);
			
			if (string.IsNullOrEmpty(Result))
			{
				string QueryText =
					"UPDATE EAEventRun SET " +
					"I_EVA_TaskType = @TaskType," +
					"EVA_WHO1 = @WhoTo1," +
					"EVA_WHO2 = @WhoTo2," +
					"B_EVA_DistinctClients = @DistinctClientsOnly," +
					"B_EVA_AddAsExcluded = @AddClientsAsExcluded," +
					"I_EVA_EmailOption = @EmailOption," +
					"I_EVA_EmailTemplateId = @EmailTemplateId," +
					"EVA_SubjectLine = @EmailSubject," +
					"EVA_EmailBody = @EmailBody," +
					"EVA_FromEmail = @FromId," +
					"EVA_CCTo = @CCId," +
					"EVA_BCCTo = @BccId," +
					"EVA_ExclusionTo = @PrintToId," +
					"B_EVA_TestEmail = @TestEmail," +
					"I_EVA_SignatureId = @EmailSignatureId" +
					" WHERE EVA_ID = @RunId";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("@TaskType", TaskId));
			
				Params.Add(new FbParameterItem("@WhoTo1", WhoTo1Id));
				Params.Add(new FbParameterItem("@WhoTo2", WhoTo2Id));
				Params.Add(new FbParameterItem("@DistinctClientsOnly", DistinctClientsOnly));
				Params.Add(new FbParameterItem("@AddClientsAsExcluded", AddClientsAsExcluded));
				Params.Add(new FbParameterItem("@EmailOption", EmailOption));
				Params.Add(new FbParameterItem("@EmailTemplateId", EmailTemplateId));
				Params.Add(new FbParameterItem("@EmailSubject", Subject));
				
				/*
				byte[] EmailBodyArray = new byte[0];
				if (ContentIsValidHtml(EmailBody))
				{
					EmailBodyArray = Encoding.UTF8.GetBytes(HtmlToRTF(EmailBody));
				}
				else
				{
					EmailBodyArray = Encoding.UTF8.GetBytes(HtmlToRTF(EmailBody));
				}
				*/
				byte[] EmailBodyArray = Encoding.UTF8.GetBytes(EmailBody);
				//byte[] EmailBodyArray = Encoding.UTF8.GetBytes(HtmlToRTF(EmailBody));
				
				Params.Add(new FbParameterItem("@EmailBody", EmailBodyArray));
				Params.Add(new FbParameterItem("@FromId", FromId));
				Params.Add(new FbParameterItem("@CCId", CCId));
				Params.Add(new FbParameterItem("@BccId", BccId));
				Params.Add(new FbParameterItem("@PrintToId", PrintToId));
				Params.Add(new FbParameterItem("@RunId", RunId));
			
				Params.Add(new FbParameterItem("@TestEmail", TestEmail));
				Params.Add(new FbParameterItem("@EmailSignatureId", EmailSignatureId));
				UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, "ContactOperations", 0);

				WindowsEventLoggingUtilities.WriteToInfoEvent("Before calling DBExecute", "UpdateEventRunRecord", 0);
				Result = CommonUtilities.FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
				WindowsEventLoggingUtilities.WriteToInfoEvent("After calling DBExecute", "UpdateEventRunRecord", 0);
				WindowsEventLoggingUtilities.WriteToInfoEvent("Result: " + Result, "UpdateEventRunRecord", 0);
			}
			
			if (string.IsNullOrEmpty(Result) && (EmailTemplateId == 0))
			{
				// The content of the email body should be HTML.
				// Convert it to RTF and update the EVA_EmailBody field with the converted text
				Result =
					CommonUtilities.UtilityApps.HtmlToRtf(
						"EAEventRun", "EVA_EmailBody", "EVA_ID", RunId,
						"EAEventRun", "EVA_EmailBody", "EVA_ID", RunId);
			}

			return Result;
		}
		
		public static string BuildEmailAttachmentList(string ConnectionString,
		                                              Int64 RunId,
		                                              ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "BuildEmailAttachmentList";
			List<string> RowList = new List<string>();
			List<string> StrList = new List<string>();
			string ColName = "";
			string ColValue = "";
			string Str = "";
			string QueryText = 
                "SELECT EAEmailAttachments.*, character_length(EAEmailAttachments.EA_Base64) AS FileLength " +
                "FROM EAEmailAttachments WHERE EA_EventRunId = @RunId";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("@RunId", RunId));
			
			//WindowsEventLoggingUtilities.WriteToErrorEvent("QueryText: " + QueryText, Method, 0);
			UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
			
            Entries.Append(CommonUtilities.JsonUtilities.StartJsonArray("Items"));
			//DataTable dt = FirebirdUtilities.GetFirstDataTable(ConnectionString, QueryText, Params, 0);
			DataTable dt = 
				UsefulDBMethods.Helpers.GetFirstDataTableEx(
					ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
			/*
			FirebirdUtilities.GetFirstDataTable(ConnectionString, QueryText, Params, 0);
			if (dt == null)
			{
				Result = "No data table returned";
				WindowsEventLoggingUtilities.WriteToErrorEvent(Result, Method, 0);
			}
			/*
			else if (dt.Rows.Count == 0)
			{
				Result = "No record returned for the query";
				WindowsEventLoggingUtilities.WriteToErrorEvent(Result, Method, 0);
			}
			else if (dt.Rows.Count > 0)			
			*/
			if (string.IsNullOrEmpty(Result))
			{
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
			}			
            Entries.Append(CommonUtilities.JsonUtilities.EndJsonArray());
            
			return Result;
		}		                                              
		
		public static string BuildXEmailRecipientList(string ConnectionString,
		                                       int RunId,
		                                       ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "BuildXEmailRecipientList";
			List<string> RowList = new List<string>();
			List<string> StrList = new List<string>();
			string ColName = "";
			string ColValue = "";
			string Str = "";
			string QueryText = "SELECT * FROM XEMAIL" + RunId;
			WindowsEventLoggingUtilities.WriteToErrorEvent("QueryText: " + QueryText, Method, 0);
			
            Entries.Append(CommonUtilities.JsonUtilities.StartJsonArray("Items"));
			DataTable dt = FirebirdUtilities.GetFirstDataTable(ConnectionString, QueryText);
			if (dt == null)
			{
				Result = "No data table returned";
				WindowsEventLoggingUtilities.WriteToErrorEvent(Result, Method, 0);
			}
            /*
			else if (dt.Rows.Count == 0)
			{
				Result = "No record returned for the query";
				WindowsEventLoggingUtilities.WriteToErrorEvent(Result, Method, 0);
			}
			*/
			else if (dt.Rows.Count > 0)
			{
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
			}
            Entries.Append(CommonUtilities.JsonUtilities.EndJsonArray());
			
			return Result;
		}
		
		public static string ExecuteEventRunProcess(string ConnectionString,
		                                            int EventRunId,
		                                            int CompanyClientId)
		{
			string Result = "";
			string Method = "ExecuteEventRunProcess";

			string QueryText = "EXECUTE PROCEDURE EventRunProcess(@RunId, @CompanyClientId)";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("@RunId", EventRunId));
			Params.Add(new FbParameterItem("@CompanyClientId", CompanyClientId));
			Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
			
			if (string.IsNullOrEmpty(Result))
			{
				WindowsEventLoggingUtilities.WriteToInfoEvent("Before EXECUTE PROCEDURE EVENTRUNPROCESS", Method, 0);
				Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
				WindowsEventLoggingUtilities.WriteToInfoEvent("After EXECUTE PROCEDURE EVENTRUNPROCESS", Method, 0);
				WindowsEventLoggingUtilities.WriteToInfoEvent("Result: " + Result, Method, 0);
			}
			
			if (!string.IsNullOrEmpty(Result))
			{
				Result = new ActionBase().LogOnProcessingError(Method, Result, "");
			}

			return Result;
		}
		
		public static string HtmlToRTF(string HtmlText)
		{
			string Result = "";
			System.Windows.Forms.WebBrowser wb = new System.Windows.Forms.WebBrowser();
			System.Windows.Forms.RichTextBox richText = new System.Windows.Forms.RichTextBox();
			wb.Navigate("about:blank");
			
			wb.Document.Write(HtmlText);
			wb.Document.ExecCommand("SelectAll", false, null);
			wb.Document.ExecCommand("Copy", false, null);
			
			richText.SelectAll();
			richText.Paste();
			Result = richText.SelectedRtf;
			return Result;
		}
		
		public static bool ContentIsValidHtml(string Source)
		{
			bool Result = true;
			
			if (Result &&
			    Source.StartsWith("{\\rtf", StringComparison.CurrentCultureIgnoreCase))
				//Result = "The content looks like an RTF file";			
				Result = false;
			else if (Result && 
			    Source.IndexOf("<html", StringComparison.CurrentCultureIgnoreCase) < 0)
				//Result = "Invalid - no html element";
				Result = false;
			else if (Result &&
			    Source.IndexOf("</html>", StringComparison.CurrentCultureIgnoreCase) < 0)
				//Result = "Invalid = no ending html element";
				Result = false;
			else if (Result &&
			    Source.IndexOf("<body", StringComparison.CurrentCultureIgnoreCase) < 0)
				//Result = "Invalid - no body element";
				Result = false;
			else if (Result &&
			    Source.IndexOf("</body>", StringComparison.CurrentCultureIgnoreCase) < 0)
				//Result = "Invalid - no ending body element";
				Result = false;
			
			return Result;
		}
		
		public static string LoadBulkEmail_SetupPageEntries_RequestItem(ActionBase AActionBase,
		                                                                ref BulkEmail_SetupPageEntries_RequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadBulkEmail_SetupPageEntries_RequestItem";
			try 
			{
				Object Obj = AActionBase.GetCollectionValue("Data", "RunId");
				TempItem.RunIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = AActionBase.GetCollectionValue("Data", "Source");
				TempItem.Source = (Obj == null) ? "" : Convert.ToString(Obj); 
			}
			catch (Exception Ex)
			{
				Result =
					Method + Environment.NewLine +
					Ex.Message + Environment.NewLine +
					Ex.StackTrace;
			}
			return Result;
		}
		
		public static string BuildLedgerGridEntries(ActionBase ActionBase,
		                                            int RunId,
		                                            ref StringBuilder Entries)
		{
			string Method = "BuildLedgerGridEntries";
			string Result = "";			
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
			
			ActionBase.LogOnInsideMethod(Method);
			try
			{
	            //Entries.Append(",");
	            Entries.Append(CommonUtilities.JsonUtilities.StartJsonArray("gridEntries"));

				string QueryText = "SELECT ELB_SQLFOREMAIL FROM EALEDGERBULKEVENT where I_ELB_ID= @RunId";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("RunId", RunId));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);

				object Obj = null;
				DataTable tbl = null;
				string SqlToUse = "";
				if (string.IsNullOrEmpty(Result))
				{
					tbl = 
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ActionBase.FbConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTablesAndRows, out Result);
					if (string.IsNullOrEmpty(Result))
					{
						Obj = tbl.Rows[0]["ELB_SqlForEmail"];
						SqlToUse = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					}
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					SqlToUse = SqlToUse.Replace("\"", "'");
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ActionBase.FbConnectionString, SqlToUse, null, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
					if (string.IsNullOrEmpty(Result))
					{
						List<string> RowList = new List<string>();
						List<string> StrList = new List<string>();
						string ColName = "";
						string ColValue = "";
						string Str = "";
						if (tbl.Rows.Count > 0)
						{
							RowList.Clear();				
							Str = "";
							int RowCount = tbl.Rows.Count;
							foreach(DataRow ThisRow in tbl.Rows)
							{
								StrList.Clear();
								foreach(DataColumn ThisCol in tbl.Columns)
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
							
							Entries.AppendLine(string.Join(",", RowList.ToArray()));
						}			
					}
				}
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
	            Entries.Append(CommonUtilities.JsonUtilities.EndJsonArray());
				ActionBase.LogOnLeavingMethod(Method);
			}
			
			return Result;
		}
		
		public static string LoadBulkEmail_RemoveRecipient_RequestItem(ActionBase AActionBase,
		                                                               ref BulkEmail_RemoveRecipient_RequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadBulkEmail_SetupPageEntries_RequestItem";
			try 
			{
				Object Obj = AActionBase.GetCollectionValue("Data", "RunId");
				TempItem.RunIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = AActionBase.GetCollectionValue("Data", "Source");
				TempItem.Source = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = AActionBase.GetCollectionValue("Data", "RecordId");
				TempItem.RecordIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
			}
			catch (Exception Ex)
			{
				Result =
					Method + Environment.NewLine +
					Ex.Message + Environment.NewLine +
					Ex.StackTrace;
			}
			return Result;
		}
		
		public static string BulkEmail_GetStatementRunId(ActionBase ActionBase,
		                                                 int RunId,
		                                                 string Source,
		                                                 out int StatementRunId)
		{
			string Result = "";
			string Method = "BulkEmail_GetStatementRunId";
			StatementRunId = -1;
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				switch (Source.ToLower())
				{
					case "ledger":
					case "debts":
					case "addinvoice":
						Result = BulkEmail_GetStatementRunId_Ledger(ActionBase, RunId, out StatementRunId); break;
					default: Result = "Unknown Source: " + Source; break;
				}
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
			}
			
			return Result;
		}
		
		public static string BulkEmail_GetStatementRunId_Ledger(ActionBase ActionBase,
		                                                        int RunId,
		                                                        out int StatementRunId)
		{
			string Result = "";
			string Method = "BulkEmail_GetStatementRunId_Ledger";
			StatementRunId = -1;
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string QueryText = "SELECT I_ELB_StatementRunId FROM EALEDGERBULKEVENT WHERE I_ELB_ID = @RunId";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("RunId", RunId));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, ActionBase.MasterId);
				DataTable tbl = null;
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ActionBase.FbConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTablesAndRows, out Result);					
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					DataRow row = tbl.Rows[0];
					object Obj = row["I_ELB_StatementRunId"];
					StatementRunId = (Obj == DBNull.Value) ? -1 : Convert.ToInt32(Obj);
				}
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
			}
			
			return Result;
		}
		
		public static string BulkEmail_GetEventRunId(ActionBase ActionBase,
		                                             int RunId,
		                                             string Source,
		                                             out int EventRunId)
		{
			string Result = "";
			string Method = "BulkEmail_GetEventRunId";
			EventRunId = -1;
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				switch (Source.ToLower())
				{
					case "ledger":
					case "debts":
					case "addinvoice":
						Result = BulkEmail_GetEventRunId_Ledger(ActionBase, RunId, out EventRunId); break;
					default: Result = "Unknown Source: " + Source; break;
				}
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
			}
			
			return Result;
		}
		
		public static string BulkEmail_GetEventRunId_Ledger(ActionBase ActionBase,
		                                                    int RunId,
		                                                    out int EventRunId)
		{
			string Result = "";
			string Method = "BulkEmail_GetEventRunId_Ledger";
			EventRunId = -1;
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string QueryText = "SELECT I_ELB_EVENTRUNID FROM EALEDGERBULKEVENT WHERE I_ELB_ID = @RunId";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("RunId", RunId));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, ActionBase.MasterId);
				DataTable tbl = null;
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ActionBase.FbConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTablesAndRows, out Result);					
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					DataRow row = tbl.Rows[0];
					object Obj = row["I_ELB_EVENTRUNID"];
					EventRunId = (Obj == DBNull.Value) ? -1 : Convert.ToInt32(Obj);
				}
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
			}
			
			return Result;
		}
		
		//public static string LoadBulkEmail_ProcessRunRequestItem(JObject DataObject,
		public static string LoadBulkEmail_ProcessRunRequestItem(ActionBase ActionBase,
		                                                         ref BulkEmail_ProcessRunRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadBulkEmail_SetupPageEntries_RequestItem";
			try 
			{
				object Obj = ActionBase.GetCollectionValue("data", "runId");
				TempItem.RunIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("data", "eventRunId");
				TempItem.EventRunIdValue = (Obj == null) ? "" : Convert.ToString(Obj);

				Obj = ActionBase.GetCollectionValue("data", "emailOption");
				TempItem.EmailOptionValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("data", "templateId");
				TempItem.TemplateIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("data", "reportId");
				TempItem.ReportIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("data", "subject");
				TempItem.Subject = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("data", "emailBody");
				TempItem.EmailBody = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("data", "from");
				TempItem.FromValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("data", "cc");
				TempItem.CCValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("data", "bcc");
				TempItem.BCCValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("data", "print");
				TempItem.ExclusionsValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("data", "emailSignatureId");
				TempItem.SignatureIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("data", "eventRunSource");
				TempItem.EventRunSource = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("data", "source");
				TempItem.Source = (Obj == null) ? "" : Convert.ToString(Obj);
			}
			catch (Exception Ex)
			{
				Result =
					Method + Environment.NewLine +
					Ex.Message + Environment.NewLine +
					Ex.StackTrace;
			}
			return Result;
		}
		
		public static string RemoveAnEmailAttachment(ActionBase ActionBase,
		                                             int RecordId)
		{
			string Result = "";
			string Method = "RemoveAnEmailAttachment";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
			
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string QueryText = "DELETE FROM EAEmailAttachments WHERE EA_ID = @RecordId";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("@RecordId", RecordId));
				UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, ActionBase.MasterId);
				
				WindowsEventLoggingUtilities.WriteToInfoEvent("Before calling DBExecute", Method, ActionBase.MasterId);
				Result = FirebirdUtilities.DBExecute(ActionBase.FbConnectionString, QueryText, Params, 0);
				WindowsEventLoggingUtilities.WriteToInfoEvent("After calling DBExecute", Method, ActionBase.MasterId);
				WindowsEventLoggingUtilities.WriteToInfoEvent("Result: " + Result, Method, ActionBase.MasterId);
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
			}
			
			return Result;
		}
		
		public static string BulkEmail_GetEventRunSource(ActionBase ActionBase,
		                                                 int RunId,
		                                                 string Source,
		                                                 out string EventRunSource)
		{
			string Result = "";
			string Method = "BulkEmail_GetEventRunSource";
			EventRunSource = "";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				int EventRunId = 0;
				Result = BulkEmail_GetEventRunId(ActionBase, RunId, Source, out EventRunId);
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
			}
			
			return Result;
		}
		
		public static string BulkEmail_GetEventRunSource(ActionBase ActionBase,
		                                                 int EventRunId,
		                                                 out string EventRunSource)
		{
			string Result = "";
			string Method = "BulkEmail_GetEventRunSource";
			EventRunSource = "";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string QueryText = "SELECT EVA_SourceScreen FROM EAEventRun WHERE EVA_ID = @EventRunId";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("EventRunId", EventRunId));
				UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, "ContactOperations", ActionBase.MasterId);
				
				DataTable tbl =
					UsefulDBMethods.Helpers.GetFirstDataTableEx(
						ActionBase.FbConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTablesAndRows, out Result);
				if (string.IsNullOrEmpty(Result))
				{
					object Obj = tbl.Rows[0]["EVA_SourceScreen"];
					EventRunSource = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
				}
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
			}
			
			return Result;
		}
		
		/*
		public static string BulkEmail_GetStatementRunId(ActionBase ActionBase,
		                                                 int RunId,
		                                                 out int StatementRunId)
		{
			string Result = "";
			string Method = "BulkEmail_GetStatementRunId";
			StatementRunId = 0;
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string QueryText = "SELECT I_ELB_StatementRunId FROM EALEDGERBULKEVENT where I_ELB_ID = @RunId";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("RunId", RunId));
				UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, "ContactOperations", ActionBase.MasterId);
				
				DataTable tbl =
					UsefulDBMethods.Helpers.GetFirstDataTableEx(
						ActionBase.FbConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTablesAndRows, out Result);
				if (string.IsNullOrEmpty(Result))
				{
					object Obj = tbl.Rows[0]["EVA_SourceScreen"];
					StatementRunId = (Obj == DBNull.Value) ? -1 : Convert.ToInt32(Obj);
				}
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
			}
			
			return Result;
		}
		*/
		
		public static string PopulateAddInvoiceAttatchment(string ConnectionString,
		                                                   Int64 CompanyId,
		                                                   int ClientId,
		                                                   int RunId)
		{
			string Result = "";
			string Method = "PopulateAddInvoiceAttatchment";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			ActionBase ActionBase = new ActionBase();
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				// only add the attachment if there aren't any there yet
				string QueryText = "SELECT COUNT(EA_ID) FROM EAEmailAttachments WHERE EA_EventRunId = @RunId";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("RunId", RunId));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				
				DataTable tbl = null;
				object Obj = null;
				int ItemCount = 0;
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
					if (string.IsNullOrEmpty(Result) && (tbl.Rows.Count == 0)) Result = "No rows to process";
					if (string.IsNullOrEmpty(Result))
					{
						Obj = tbl.Rows[0][0];
						ItemCount = (Obj == DBNull.Value) ? -1 : Convert.ToInt32(Obj);
						if (ItemCount < 0) Result = "Error getting attachment entry count";
					}
				}
				
				if (string.IsNullOrEmpty(Result) && (ItemCount > 0)) return "";					
				
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = "SELECT ELB_SQLFORINVOICEBLOB FROM EALedgerBulkEvent WHERE I_ELB_ID = @RunId";
					// No need to change anything with the Params object because we are only using the same content
					//Params.Clear();
					//Params.Add(new FbParameterItem("RunId", RunId));
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
					
					if (string.IsNullOrEmpty(Result))
					{
						tbl =
							UsefulDBMethods.Helpers.GetFirstDataTableEx(
								ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
					}
						
					if (string.IsNullOrEmpty(Result) && (tbl.Rows.Count == 0)) Result = "No rows to process";
					if (string.IsNullOrEmpty(Result))
					{
						Obj = tbl.Rows[0][0];
						QueryText = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
						if (string.IsNullOrEmpty(QueryText)) Result = "Cannot find the Blob query";						
					}
				}				
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, null, Method, 0);
				}
				
				string FileExtn = "";
				string FileName = "";
				byte[] BlobBytes = new byte[0];
				string Base64String = "";
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, QueryText, UsefulDBMethods.FirebirdQueryCheckTypes.HasTablesAndRows, out Result);
					WindowsEventLoggingUtilities.WriteToInfoEvent("RowCount: " + tbl.Rows.Count.ToString(), Method, 0);
					if (string.IsNullOrEmpty(Result))
					{
						Obj = tbl.Rows[0]["IVS_SendFileName"];
						FileName = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
						if (string.IsNullOrEmpty(FileName)) Result = "Empty or invalid FileName";
						WindowsEventLoggingUtilities.WriteToInfoEvent("FileName: " + FileName, Method, 0);
						WindowsEventLoggingUtilities.WriteToInfoEvent("Result: " + Result, Method, 0);
					}
					
					if (string.IsNullOrEmpty(Result))
					{
						Obj = tbl.Rows[0]["IVS_FileExtension"];
						FileExtn = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
						if (string.IsNullOrEmpty(FileExtn)) Result = "Empty or invalid File Extension";
						WindowsEventLoggingUtilities.WriteToInfoEvent("FileExtn: " + FileExtn, Method, 0);
						WindowsEventLoggingUtilities.WriteToInfoEvent("Result: " + Result, Method, 0);
					}
					
					if (string.IsNullOrEmpty(Result))
					{
						Obj = tbl.Rows[0]["IVS_BlobAttachment"];
						if (Obj != DBNull.Value) BlobBytes = (byte[])Obj;
						Base64String = System.Text.Encoding.Default.GetString(BlobBytes);
						if (string.IsNullOrEmpty(Base64String)) Result = "No Blob content to process";
						WindowsEventLoggingUtilities.WriteToInfoEvent("After the byte array", Method, 0);
						WindowsEventLoggingUtilities.WriteToInfoEvent("Result: " + Result, Method, 0);
					}

					WindowsEventLoggingUtilities.WriteToInfoEvent("FileName: " + FileName, Method, 0);
					WindowsEventLoggingUtilities.WriteToInfoEvent("FileExtn: " + FileExtn, Method, 0);
					
					if (string.IsNullOrEmpty(Result))
					{
						string OutputFolder = global::UsefulDBMethods.CompanyFolderSettings.DocsLocation(ConnectionString, CompanyId, out Result);
						if (string.IsNullOrEmpty(Result))
						{
							if (!OutputFolder.EndsWith(Path.DirectorySeparatorChar.ToString())) OutputFolder += Path.DirectorySeparatorChar;
							FileName = OutputFolder + FileName;
						}
					}

					if (string.IsNullOrEmpty(Result))
					{
						Result =
							CommonMethods.AddEmailAttachment(
								ConnectionString, CompanyId, ClientId, 0, RunId, FileName, Base64String);
					}
					
					/*
					if (string.IsNullOrEmpty(Result))
					{
						string OutputFolder = global::UsefulDBMethods.CompanyFolderSettings.DocsLocation(ConnectionString, CompanyId, out Result);
						if (string.IsNullOrEmpty(Result))
						{
							if (!OutputFolder.EndsWith(Path.DirectorySeparatorChar.ToString())) OutputFolder += Path.DirectorySeparatorChar;
							FileName = OutputFolder + FileName;
							Result = global::UsefulDBMethods.Reporting.SystemReportUtilities.WriteOutputFile(ConnectionString, QueryText, FileName);
						}
						
						if (string.IsNullOrEmpty(Result))
						{
							
						}
					}
					*/
				}
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
			}
			
			return Result;
		}
		
		public static string AddEmailAttachment(string ConnectionString,
		                                        Int64 CompanyId,
		                                        int ClientId,
		                                        int NewId,
		                                        int RunId,
		                                        string FileName,
		                                        string Base64FileContents)
		{
			string Result = "";
			string Method = "AddEmailAttachment";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			ActionBase ActionBase = new ActionBase();
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				if (NewId == 0)
				{
					NewId =
						UsefulDBMethods.GeneratorHelpers.GetNextGeneratorId(
							ConnectionString, "EAEMAILATTACHMENT_GEN", (int)CompanyId, ClientId, out Result);
				}
				
				string QueryText = 
					"INSERT INTO EAEmailAttachments(EA_ID, EA_CompanyClientId, EA_EventRunId, EA_AttachmentName, EA_FileExtension, EA_Source, EA_Base64) " +
					"VALUES (@Id, @CompanyClientId, @EventRunId, @AttachmentName, @FileExtension, @Source, @Base64Content)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("@Id", NewId));
				
				Params.Add(new FbParameterItem("@CompanyClientId", CompanyId));
				Params.Add(new FbParameterItem("@EventRunId", RunId));
				
				string AttachmentName = Path.GetFileNameWithoutExtension(FileName);
				Params.Add(new FbParameterItem("@AttachmentName", AttachmentName));
				
				string FileExtension = Path.GetExtension(FileName);
				Params.Add(new FbParameterItem("@FileExtension", FileExtension));
				
				Params.Add(new FbParameterItem("@Source", "Contact-Email"));
				
				var byteArray = Convert.FromBase64String(Base64FileContents);
				Params.Add(new FbParameterItem("@Base64Content", byteArray));
	
				WindowsEventLoggingUtilities.WriteToInfoEvent("Before calling DBExecute", Method, 0);
				Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
				WindowsEventLoggingUtilities.WriteToInfoEvent("After calling DBExecute", Method, 0);
				WindowsEventLoggingUtilities.WriteToInfoEvent("Result: " + Result, Method, 0);
			}			
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
			}
			return Result;
		}


        public static string GetEventRunIdForBulkRunId(string ConnectionString,
                                                       Int64 BulkRunId,
                                                       out Int64 EventRunId)
        {
            string Result = "";
            string Method = "GetEventRunIdForBulkRunId";
            EventRunId = 0;
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                string QueryText = "select I_ELB_EVENTRUNID from ealedgerbulkevent where I_ELB_ID = @BulkRunId";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                Params.Add(new FbParameterItem("BulkRunId", BulkRunId));
                UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);

                DataTable tbl =
                    UsefulDBMethods.Helpers.GetFirstDataTableEx(
                        ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
                if (string.IsNullOrEmpty(Result))
                {
                    object Obj = tbl.Rows[0]["I_ELB_EVENTRUNID"];
                    EventRunId = (Obj == DBNull.Value) ? 0 : Convert.ToInt64(Obj);
                }
            }
            catch (Exception Ex)
            {
                Result = ActionBase.LogOnException(Method, Ex, "");
            }
            finally
            {
                ActionBase.LogOnLeavingMethod(Method);
            }

            return Result;
        }

        public static string LoadConvertRTFTemplatesRequestItem(ActionBase ActionBase,
                                                                ref ConvertRTFTemplatesRequestItem TempItem)
        {
            string Result = "";
            string Method = GetModuleName() + "LoadAllPageDataRequestItem";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                Object Obj = null;
                Obj = ActionBase.GetCollectionValue("Data", "CompanyId");
                TempItem.CompanyIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
            }
            catch (Exception Ex)
            {
                Result = ActionBase.LogOnException(Method, Ex, "");
            }
            finally
            {
                ActionBase.LogOnLeavingMethod(Method);
            }

            return Result;
        }

        public static string ConvertRTFTemplates(string ConnectionString,
                                                 int CompanyId,
                                                 int ClientId,
                                                 ConvertRTFTemplatesRequestItem Request)
        {
            string Result = "";
            string Method = GetModuleName() + "ConvertRTFTemplates";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                string ClientIdStr = "-1";
                string FileNamePrefix = CompanyId.ToString() + "_" + ClientIdStr;
                string QueryText = "";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    QueryText =
                        " SELECT I_AEM_ID" +
                        " FROM EAEMAILTEMPLATES" +
                        " WHERE I_AEM_CompanyClientId = @CompanyId" +
                        "   AND ((AEM_EM_Message IS NOT NULL) AND (AEM_EM_Message <> ''))" +
                        "   AND ((AEM_BlobHtml = '') OR (AEM_BlobHtml IS NULL))" +
                        " ORDER BY I_AEM_ID DESC";
                    Params.Add(new FbParameterItem("CompanyId", CompanyId));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
                }

                string DocsLocation = "";
                string WorkingDir = "";
                string ConsoleFileName = "";
                string SepChar = Path.DirectorySeparatorChar.ToString();
                if (string.IsNullOrEmpty(Result))
                {
                    DocsLocation = global::UsefulDBMethods.CompanyFolderSettings.DocsLocation(ConnectionString, CompanyId, out Result);
                    if (string.IsNullOrEmpty(Result))
                    {
                        if (!DocsLocation.EndsWith(SepChar))
                        {
                            DocsLocation += SepChar;
                        }
                        //WorkingDir = Directory.GetCurrentDirectory();
                        WorkingDir = CommonUtilities.FolderHelpers.GetAssemblyDirectoryName(Assembly.GetExecutingAssembly());
                        if (!WorkingDir.EndsWith(SepChar)) WorkingDir += SepChar;
                        ConsoleFileName = CommonUtilities.MiscUtilities.DateTimeToYYYYMMDDTHHNNSS(DateTime.Now);
                        ConsoleFileName = WorkingDir + "ConvertRTFTemplates_" + ConsoleFileName + ".txt";
                    }
                }

                if (string.IsNullOrEmpty(Result))
                {
                    string Msg =
                        "DocsLocation: " + DocsLocation + Environment.NewLine +
                        "WorkingDir: " + WorkingDir + Environment.NewLine +
                        "ConsoleFileName: " + ConsoleFileName + Environment.NewLine;
                    WindowsEventLoggingUtilities.WriteToInfoEvent(Msg, Method, 0);
                }

                int CurrentId = 0;
                object Obj = null;
                if (string.IsNullOrEmpty(Result))
                {
                    DataTable dt = FirebirdUtilities.GetFirstDataTable(ConnectionString, QueryText, Params, 0);
                    foreach (DataRow Row in dt.Rows)
                    {
                        Obj = Row["I_AEM_ID"];
                        CurrentId = (Obj == DBNull.Value) ? 0 : Convert.ToInt32(Obj);
                        WriteOutputLine(ConsoleFileName, "Processing TemplateId: " + CurrentId);
                        Result = 
                            UpdateHtmlTemplate(
                                ConnectionString, CurrentId, DocsLocation, FileNamePrefix, ClientIdStr, 
                                WorkingDir, CompanyId, ConsoleFileName);
                        if (!string.IsNullOrEmpty(Result))
                        {
                            WriteOutputLine(ConsoleFileName, "Error: " + Result);
                            break;
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                Result = ActionBase.LogOnException(Method, Ex, "");
            }
            finally
            {
                ActionBase.LogOnLeavingMethod(Method);
            }

            return Result;
        }

        public static string UpdateHtmlTemplate(string ConnectionString,
                                                int TemplateId,
                                                string DocsLocation,
                                                string FileNamePrefix,
                                                string ClientIdStr,
                                                string WorkingDir,
                                                int CompanyId,
                                                string ConsoleFileName)
        {
            string Result = "";
            string Method = GetModuleName() + "UpdateHtmlTemplate";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                string Args = "";
                List<string> ArgsList = new List<string>();
                string FileContent = "";
                if (string.IsNullOrEmpty(Result))
                {
                    ArgsList.Add("cmd=GetEmailTemplateTextAsHtml");
                    ArgsList.Add("EmailTemplateId=" + TemplateId);
                    ArgsList.Add("CCID=" + CompanyId);
                    ArgsList.Add("ClientId=" + ClientIdStr);
                    Args = string.Join(" ", ArgsList.ToArray());
                    Process runApp = new Process();
                    runApp.StartInfo.FileName = WorkingDir + "EAOfficeUtilities";
                    runApp.StartInfo.Arguments = Args;
                    WriteOutputLine(ConsoleFileName, "Command: " + runApp.StartInfo.FileName + " " + Args);
                    runApp.Start();
                    const int tries = 60;
                    for (int Index = 0; Index < tries; Index++)
                    {
                        if (!runApp.HasExited) System.Threading.Thread.Sleep(1000);
                    }
                    if (runApp.HasExited)
                    {
                        string OutputFileNameOnly = DocsLocation + FileNamePrefix + "_" + TemplateId.ToString();
                        string HtmlFileName = OutputFileNameOnly + ".html";
                        string RTFFilename = OutputFileNameOnly + ".rtf";
                        WriteOutputLine(ConsoleFileName, OutputFileNameOnly);
                        if (!File.Exists(RTFFilename))
                        {
                            WriteOutputLine(ConsoleFileName, "Source file does not exist");
                        }
                        if (!File.Exists(HtmlFileName))
                        {
                            WriteOutputLine(ConsoleFileName, "Destination file does not exist");
                        }
                        if (File.Exists(HtmlFileName))
                        {
                            FileContent = File.ReadAllText(HtmlFileName);
                            FileContent = ProcessHtmlFileOutput(FileContent, ConsoleFileName, out Result);
                            WriteOutputLine(ConsoleFileName, FileContent);
                            if (!string.IsNullOrEmpty(FileContent))
                            {
                                Result = UpdateEmailTemplateRecord(ConnectionString, CompanyId, TemplateId, FileContent);
                            }
                        }
                    }
                }
                //string HtmlFileName = FileNamePrefix + "_" + TemplateId.ToString() + ".html";

            }
            catch (Exception Ex)
            {
                Result = ActionBase.LogOnException(Method, Ex, "");
            }
            finally
            {
                ActionBase.LogOnLeavingMethod(Method);
            }

            return Result;
        }

        public static string UpdateEmailTemplateRecord(string ConnectionString,
                                                       int CompanyId,
                                                       int TemplateId,
                                                       string FileContent)
        {
            string Result = "";
            string Method = GetModuleName() + "UpdateHtmlTemplate";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                string QueryText = "";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    byte[] EmailBodyArray = Encoding.ASCII.GetBytes(FileContent);
                    QueryText =
                        "UPDATE EAEmailTemplates SET AEM_BlobHtml = @BlobHtml WHERE I_AEM_CompanyClientId = @CompanyId AND I_AEM_ID = @TemplateId";
                    Params.Add(new FbParameterItem("CompanyId", CompanyId));
                    Params.Add(new FbParameterItem("TemplateId", TemplateId));
                    Params.Add(new FbParameterItem("BlobHtml", EmailBodyArray));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
                }
            }
            catch (Exception Ex)
            {
                Result = ActionBase.LogOnException(Method, Ex, "");
            }
            finally
            {
                ActionBase.LogOnLeavingMethod(Method);
            }

            return Result;
        }

        public static string ProcessHtmlFileOutput(string Content,
                                                   string ConsoleFileName,
                                                   out string ErrorMsg)
        {
            string Result = "";
            ErrorMsg = "";
            string Method = GetModuleName() + "ProcessHtmlFileOutput";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                Result = Content;
                int Posn = 0;
                if (string.IsNullOrEmpty(ErrorMsg))
                {
                    Posn = Result.IndexOf("<body>");
                    WriteOutputLine(ConsoleFileName, "Posn: " + Posn);
                    // strip out all the text before and including the <body> element
                    if (Posn > 0)
                    {
                        Result = Result.Substring(Posn + 6);
                        //WriteOutputLine(ConsoleFileName, Content);
                    }
                }

                // strip out all the text after and incuding the </body> element
                Posn = Result.IndexOf("</body>");
                if (Posn > 0)
                {
                    WriteOutputLine(ConsoleFileName, "Posn: " + Posn);
                    Result = Result.Substring(0, Posn);
                }
            }
            catch (Exception Ex)
            {
                ErrorMsg = ActionBase.LogOnException(Method, Ex, "");
            }
            finally
            {
                ActionBase.LogOnLeavingMethod(Method);
            }

            return Result;
        }

        public static void WriteOutputLine(string FileName,
                                             string Content)
        {
            using (StreamWriter writer = new StreamWriter(FileName, true))
            {
                writer.WriteLine(Content);
                //Console.WriteLine(LineText);
            }
        }

        public static string LoadUpdateHtmlTemplateRequestItem(ActionBase ActionBase,
                                                               ref UpdateHtmlTemplateRequestItem TempItem)
        {
            string Result = "";
            string Method = "LoadRunIdRequestItem";
            object Obj = null;
            try
            {
                Obj = ActionBase.GetCollectionValue("Data", "templateId");
                TempItem.TemplateIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
                Obj = ActionBase.GetCollectionValue("Data", "emailBody");
                TempItem.EmailBody = (Obj == null) ? "" : Convert.ToString(Obj);
            }
            catch (Exception Ex)
            {
                Result =
                    Method + Environment.NewLine +
                    Ex.Message + Environment.NewLine +
                    Ex.StackTrace;
            }
            return Result;
        }

        public static string UpdateHtmlTemplate(string ConnectionString,
                                                int CompanyId,
                                                int ClientId,
                                                UpdateHtmlTemplateRequestItem Request)
        {
            string Result = "";
            string Method = GetModuleName() + "UpdateHtmlTemplate";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            ActionBase ActionBase = new ActionBase();

            ActionBase.LogOnInsideMethod(Method);
            try
            {
                string QueryText = "";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(Result))
                {
                    byte[] EmailBodyArray = Encoding.ASCII.GetBytes(Request.EmailBody);
                    QueryText =
                        "UPDATE EAEmailTemplates SET AEM_BlobHtml = @BlobHtml WHERE I_AEM_CompanyClientId = @CompanyId AND I_AEM_ID = @TemplateId";
                    Params.Add(new FbParameterItem("CompanyId", CompanyId));
                    Params.Add(new FbParameterItem("TemplateId", Request.TemplateId));
                    Params.Add(new FbParameterItem("BlobHtml", EmailBodyArray));
                    Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
                }

                string TestRPOLocation = "";
                if (string.IsNullOrEmpty(Result))
                {
                    TestRPOLocation = global::UsefulDBMethods.CompanyFolderSettings.TestRPOLocation(ConnectionString, CompanyId, out Result);
                }

                if (string.IsNullOrEmpty(Result))
                {
                    string CommandLine = "GOATIMPORT /HTMLTOTEXT:" + Request.TemplateId;
                    Result =
                        UsefulDBMethods.Helpers.ProcessTestRPOCommandLine(
                            CommandLine, TestRPOLocation, 1000);
                }
            }
            catch (Exception Ex)
            {
                Result = ActionBase.LogOnException(Method, Ex, "");
            }
            finally
            {
                ActionBase.LogOnLeavingMethod(Method);
            }

            return Result;
        }

        public static string UpdateSignatureIdForEventRunId(string ConnectionString,
                                                            Int64 EventRunId,
                                                            int SignatureId)
        {
            string Result = "";
            string Method = "UpdateSignatureIdForEventRunId";
            string QueryText = "UPDATE EAEventRun SET I_EVA_SignatureId = @SignatureId WHERE EVA_ID = @EventRunId";
            List<FbParameterItem> Params = new List<FbParameterItem>();
            Params.Add(new FbParameterItem("SignatureId", SignatureId));
            Params.Add(new FbParameterItem("EventRunId", EventRunId));
            Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);

            try
            {
                if (string.IsNullOrEmpty(Result))
                {
                    Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
                }
            }
            catch (Exception Ex)
            {
                Result =
                    Method + Environment.NewLine +
                    Ex.Message + Environment.NewLine +
                    Ex.StackTrace;
            }

            return Result;
        }

        public static string GetOptionValue(string Path,
                                            out string ErrorMsg)
        {
            string Result = "";
            ErrorMsg = "";
            string Method = GetModuleName() + "GetOptionValue";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);

            ActionBase ActionBase = new ActionBase();
            try
            {
                string DirSepChar = System.IO.Path.DirectorySeparatorChar.ToString();
                //string OptionsFileName = Directory.GetCurrentDirectory();
                string OptionsFileName = CommonUtilities.FolderHelpers.GetAssemblyDirectoryName(Assembly.GetExecutingAssembly());
                if (!OptionsFileName.EndsWith(DirSepChar)) OptionsFileName += DirSepChar;
                OptionsFileName += "Options.json";
                WindowsEventLoggingUtilities.WriteToInfoEvent("OptionsFileName: " + OptionsFileName, Method, 0);
                string FileContent = File.ReadAllText(OptionsFileName);
                if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(FileContent)) ErrorMsg = "Cannot determine the Group entries";

                JObject JSONSource = null;
                if (string.IsNullOrEmpty(ErrorMsg))
                {
                    JSONSource = JObject.Parse(FileContent);
                    if (JSONSource == null) ErrorMsg = "Cannot parse the Group source";
                    FileContent = "";
                }

                if (string.IsNullOrEmpty(ErrorMsg))
                {
                    Result = JsonUtilities.GetJsonPathValueStr(JSONSource, Path, out ErrorMsg);
                    if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(Result)) ErrorMsg = "Cannot find the path value";
                }
            }
            catch (Exception Ex)
            {
                ErrorMsg = ActionBase.LogOnException(Method, Ex, "");
            }
            finally
            {
                ActionBase.LogOnLeavingMethod(Method);
            }

            return Result;
        }
		
		public static string SetNewEmailStatus(string ConnectionString,
		                                       string RunId)
		{
			string Result = "";
			string Method = "SetNewEmailStatus";
			string QueryText = "UPDATE EAEventRun SET EMAIL_STATUS = 'NEW' WHERE EVA_ID = @RunId";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("RunId", RunId));
			UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
			Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
			WindowsEventLoggingUtilities.WriteToInfoEvent("Result: " + Result, Method, 0);
			if (!string.IsNullOrEmpty(Result))
			{
				Result = new ActionBase().LogOnErrorSelectingData(Method, Result, QueryText);
			}
				
			return Result;
		}
    }
}
