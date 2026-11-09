/*
 * Created by SharpDevelop.
 * User: John
 * Date: 02/05/2019
 * Time: 10:05 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Data;
using CommonUtilities;
using System.Text;
using System.Diagnostics;
using System.Linq;
using System.IO;
using EAOfficeAPI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using EmailProcessor.Actions;

namespace EmailProcessor
{
	/// <summary>
	/// Description of Processor.
	/// </summary>
	public class EmailProcessor
	{
		/*
		public Processor()
		{
		}
		*/
		
		private int FRunId = 0;
		private EAOfficeAPI.APISettingsItem FSettingsItem = null;
		private RequestCommonItem FCommonItem = new RequestCommonItem();
		private string FConnectionString = "";		
		private string FModuleName = "Email Processor";
		private string FDocsLocation = @"e:\EAOffice\Development\Docs\";
		//private string FEAOWordLocation = @"E:\temp\eaWord\";
		//private string FTestRPOLocation = @"E:\temp\report\";
		private string FRTFFileName = "";
		private string FEAOWordLocation = @"E:\CreateDocs\";
		private string FTestRPOLocation = @"E:\CreateDocs\";
		private int FMasterId = 0;
		private string FWorkingDir = "";
		
		private Int64 FCompanyClientId = 0;

		public string ProcessEmailRun(int RunId)
		{
			this.FRunId = RunId;
			string Result = "";
			
			FSettingsItem = EAOfficeAPI.APISettingsFile.GetAPISettings(EAOfficeAPI.APIAppType.Service, out Result);
			FSettingsItem.DebugLogging = true;
            FCommonItem.Plugin = "EmailProcessor";
			
			if (string.IsNullOrEmpty(Result))
			{
				FConnectionString = CommonDBMethods.Common.ConnectionString(FSettingsItem);
			}

			FWorkingDir = Directory.GetCurrentDirectory();
			if (!FWorkingDir.EndsWith(Path.DirectorySeparatorChar.ToString())) FWorkingDir += Path.DirectorySeparatorChar;
			Console.WriteLine("WorkingDir: " + FWorkingDir);

			if (string.IsNullOrEmpty(Result))
			{
				Result = ExecuteRun(RunId);
			}
			
			return Result;
		}
		
		
		private string ExecuteRun(int RunId)
		{
			string Result = "";
			string Method = "ExecuteRun";
			string ErrorMsg = "";
			//int MasterId = 0;
			
			FCompanyClientId = UsefulDBMethods.EventHelpers.GetCustomerClientIdForEventRunId(FConnectionString, RunId, out Result);
			Result = BuildFolderItems(FCompanyClientId);
			// Log that we have landed here - use the MasterId going forward to identify this processes log entries
			string Content = 
				JsonUtilities.StartJson() +
				JsonUtilities.StartJsonObject("common") +
				JsonUtilities.AddPropertyNameAndValue("plugin", "EmailProcessor") +
				JsonUtilities.AddPropertyNameAndValue("companyClientId", FCompanyClientId.ToString()) +
				JsonUtilities.AddPropertyNameAndValue("Action", "ExecuteRun", false) +
				JsonUtilities.EndJsonObject() +
				"," +
				JsonUtilities.StartJsonObject("data") +
				JsonUtilities.AddPropertyNameAndValue("runId", RunId.ToString()) +
				JsonUtilities.AddPropertyNameAndValue("docsLocation", FDocsLocation) +
				JsonUtilities.AddPropertyNameAndValue("testRPOLocation", FTestRPOLocation) +
				JsonUtilities.AddPropertyNameAndValue("eaWordLocation", FEAOWordLocation, false) +
				JsonUtilities.EndJsonObject() +
				JsonUtilities.EndJson();
			ProgressLogging.AddRecord(
				true, 0, FCommonItem.Plugin, FCommonItem.Command, "ExecuteRun", //CommonItem.Action, 
				Content, JsonContentEntryType.Request, (int)FCompanyClientId, 0, out FMasterId, out Result);

			FCommonItem.Debug = "true";
			/*
			APISettingsItem SettingsItem = new APISettingsItem();
			SettingsItem.DebugLogging = true;
			*/

			WindowsEventLoggingUtilities.WriteToInfoEvent("Before calling BuildCommonAttachments", "Test", 0);
			List<string> commonAttachments = new List<string>();
			Actions.SendEmail_Common EmailSettings = new Actions.SendEmail_Common();
			ErrorMsg = PopulateEmailSettings(FCompanyClientId, ref EmailSettings);
			/*
			EmailSettings.EmailUserName = "rdenardo01@gmail.com";
			EmailSettings.EmailPassword = "wWt4nd2s8IVN1Tf6";
			EmailSettings.Host = "smtp-relay.SendInBlue.com";
			EmailSettings.Port = "587";
			EmailSettings.Timeout = 5000;
			*/
			List<Actions.SendEmail_Recipient> Recipients = new List<Actions.SendEmail_Recipient>();
			List<Actions.SendEmail_Recipient> CCList = new List<Actions.SendEmail_Recipient>();
			List<Actions.SendEmail_Recipient> BCCList = new List<Actions.SendEmail_Recipient>();
			List<Actions.SendEmail_Recipient> ExclusionList = new List<Actions.SendEmail_Recipient>();
			string SubjectLine = "";
			string EmailBody = "";
			string SenderAddress = "";
			string StrValue = "";
			Int64 TemplateId = 0;
			Int64 StatementSummaryId = 0;
			if (string.IsNullOrEmpty(Result))
			{
				Result = BuildCommonAttachments(FCompanyClientId, RunId, ref commonAttachments, ref StatementSummaryId);
			}
			
			if (string.IsNullOrEmpty(Result))
			{
				Result = BuildCommonEmailSettings(RunId, out SubjectLine, out EmailBody, out TemplateId);
			}

			// Gather the records from EAEventRunClients for the runId
			DataTable dt = null;
			if (string.IsNullOrEmpty(Result))
			{
				//string QueryText = "SELECT EVAC_ID, I_EVAC_ClientId, I_EVAC_CompanyClientId FROM EAEventRunClients WHERE I_EVAC_EventRunId = @RunId";
				string QueryText = 
					" SELECT DISTINCT EVAC_ID, I_EVAC_ClientId, B_EVAC_SENTEVENT, EVAC_StatementSummaryId," +
					" '' AS EVAC_CCEMAIL, '' AS EVAC_BCCEMAIL, '' AS EVAC_EXCLUSIONEMAIL, EVAC_FROMEMAIL, EVAC_TOEMAIL" +
					" FROM EAEventRunClients WHERE I_EVAC_EventRunId = @RunId AND B_EVAC_SentEvent = 'N'" +
					" ORDER BY EVAC_ID";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("RunId", RunId));
				WriteParamsToOutput(QueryText, Params);
				
				string Msg = "Processing QueryText:" + Environment.NewLine + QueryText + Environment.NewLine + "For RunId: " + RunId.ToString();
				WindowsEventLoggingUtilities.WriteToInfoEvent(Msg, "EmailProcessor-ExecuteRun", 0);
				dt = FirebirdUtilities.GetFirstDataTable(FConnectionString, QueryText, Params, 0);
				if (dt == null)
				{
					Result = "No data table returned";
					WindowsEventLoggingUtilities.WriteToErrorEvent(Result, Method, 0);
				}			
			}
			
			StringBuilder EmailAttachments = new StringBuilder();
			StringBuilder EmailRequest = new StringBuilder();
			//sbOutput.Append(JsonUtilities.StartJson());
			//sbOutput.Append(JsonUtilities.StartJsonArray("TemplateAttachments"));
			Int64 EVAC_ID = 0;
			Int64 ClientId = 0;
			string SentEvent = "";
			//string EntrySource = "";
			Object Obj = null;
			List<string> ClientAttachments = null;
			if (string.IsNullOrEmpty(Result))
			{
				WriteToOutput("Processing: " + dt.Rows.Count.ToString() + " records");
				foreach (DataRow Row in dt.Rows)
				{
					Obj = Row["EVAC_StatementSummaryId"];
					StatementSummaryId = (Obj == DBNull.Value) ? 0 : Convert.ToInt64(Obj);
					Obj = Row["B_EVAC_SENTEVENT"];
					SentEvent = (Obj == DBNull.Value) ? "N" : Convert.ToString(Obj);
					if (StringUtilities.SameText(SentEvent, "N"))
					{
						EmailAttachments.Length = 0; // clear for each entry
						EmailAttachments.Append(JsonUtilities.StartJson());
						EmailAttachments.Append(JsonUtilities.StartJsonArray("TemplateAttachments"));
						ClientAttachments = new List<string>(commonAttachments);
						Obj = Row["EVAC_ID"];
						EVAC_ID = (Obj == DBNull.Value) ? 0L : Convert.ToInt64(Obj);
						Obj = Row["I_EVAC_ClientId"];
						ClientId = (Obj == DBNull.Value) ? 0L : Convert.ToInt64(Obj);

						if ((EVAC_ID > 0) && (ClientId > 0))
						{
							WriteToOutput("EVAC_ID: " + EVAC_ID);
							//Result = BuildClientReportAttachments(RunId, EVAC_ID, ClientId, ref ClientAttachments);
							Result = BuildAllBlobContents(FCompanyClientId, RunId, EVAC_ID, ClientId);
							if (string.IsNullOrEmpty(Result))
							{
								Result = 
									BuildAllBlobAttachments(
										FCompanyClientId, RunId, EVAC_ID, ClientId, ref ClientAttachments, ref StatementSummaryId);
							}
							//Result = BuildClientWordDocumentAttachments(RunId, EVAC_ID, ClientId, ref ClientAttachments);
							//if (string.IsNullOrEmpty(Result)) Result = "Stop Here";
							if (string.IsNullOrEmpty(Result))
							{
								EmailAttachments.Append(string.Join(",", ClientAttachments));
								EmailAttachments.Append(JsonUtilities.EndJsonArray());
								EmailAttachments.Append(JsonUtilities.EndJson());
								using (TextWriter tw = new StreamWriter(FDocsLocation + Convert.ToString(ClientId) + "_Attachments.json"))
								{
									tw.Write(EmailAttachments.ToString());
								}
								Recipients.Clear();
                                //WriteToOutput("Cleared Recipients");
								CCList.Clear();
								BCCList.Clear();
								ExclusionList.Clear();
								/*
								Result = 
									BuildEmailAddressItems(
										RunId, FCompanyClientId, ClientId, StatementSummaryId, 
										out SenderAddress, ref Recipients, ref CCList, ref BCCList, ref ExclusionList);
								*/

								Obj = Row["EVAC_FROMEMAIL"];
								SenderAddress = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);

								if (string.IsNullOrEmpty(SenderAddress))
								{
									Result = "No Sender address available";
									WriteToOutput(Result);
								}

								if (string.IsNullOrEmpty(Result))
								{
									Obj = Row["EVAC_TOEMAIL"];
									StrValue = (Obj != DBNull.Value) ? Convert.ToString(Obj) : "";
									if (!string.IsNullOrEmpty(StrValue))
									{
										SendEmail_Recipient recipient = new SendEmail_Recipient();
										recipient.Address = StrValue;
										Recipients.Add(recipient);
										WriteToOutput("==============================");
										WriteToOutput("Processing ToEmail: " + StrValue);
										WriteToOutput("==============================");
									}
									else
									{
										Result = "No Recipient Email to send to.";
										WriteToOutput(Result);
									}
								}

								if (string.IsNullOrEmpty(Result))
								{
									Obj = Row["EVAC_CCEMAIL"];
									StrValue = (Obj != DBNull.Value) ? Convert.ToString(Obj) : "";
									if (!string.IsNullOrEmpty(StrValue))
									{
										SendEmail_Recipient cc_recipient = new SendEmail_Recipient
										{
											Address = StrValue
										};

										CCList.Add(cc_recipient);
									}

									Obj = Row["EVAC_BCCEMAIL"];
									StrValue = (Obj != DBNull.Value) ? Convert.ToString(Obj) : "";
									if (!string.IsNullOrEmpty(StrValue))
									{
										SendEmail_Recipient bcc_recipient = new SendEmail_Recipient
										{
											Address = StrValue
										};
										BCCList.Add(bcc_recipient);
									}

									Obj = Row["EVAC_EXCLUSIONEMAIL"];
									StrValue = (Obj != DBNull.Value) ? Convert.ToString(Obj) : "";
									if (!string.IsNullOrEmpty(StrValue))
									{
										SendEmail_Recipient exclusions = new SendEmail_Recipient
										{
											Address = StrValue
										};
										BCCList.Add(exclusions);
									}
									WriteToOutput("==============================");
									WriteToOutput("After processing address parts");
									WriteToOutput("==============================");
								}

                                /*
                                WriteToOutput("============================");
                                WriteToOutput("After BuildEmailAddressItems");
                                WriteToOutput("Recipients Count: " + Recipients.Count);
                                foreach (Actions.SendEmail_Recipient Recipient in Recipients)
                                {
                                    WriteToOutput("Recipient: " + Recipient.Address);
                                }
                                WriteToOutput("============================");
                                WriteToOutput("");
                                */                              
                            }

                            if (string.IsNullOrEmpty(Result))
							{
								/*
								Result =
									UpdateClientEmailAddressItems(FCompanyClientId, EVAC_ID, Recipients);
								*/
                                /*
                                WriteToOutput("****************************");
                                WriteToOutput("After UpdateClientEmailAddressItems");
                                WriteToOutput("Recipients Count: " + Recipients.Count);
                                foreach (Actions.SendEmail_Recipient Recipient in Recipients)
                                {
                                    WriteToOutput("Recipient: " + Recipient.Address);
                                }
                                WriteToOutput("****************************");
                                WriteToOutput("");
                                */                              
                            }


                            //EmailBody = "xxxx";
                            //if ((string.IsNullOrEmpty(Result)))// && (TemplateId > 0))
                            if ((string.IsNullOrEmpty(Result)) && (TemplateId > 0))
							{
								WriteToOutput("TemplateId: " + TemplateId);
								Result = BuildClientEmailBody(FCompanyClientId, RunId, EVAC_ID, ClientId, TemplateId, out EmailBody);
								using (TextWriter tw = 
								       new StreamWriter(FDocsLocation + Convert.ToString(RunId) + "_" + Convert.ToString(ClientId) + "_EmailBody.txt"))
								{
									tw.Write(EmailBody);
								}
								WriteToOutput("EmailBody:");
								WriteToOutput(EmailBody);
							}
	
							if (string.IsNullOrEmpty(Result))
							{
								EmailRequest.Length = 0;
								Result = 
									BuildJsonRequest(
										//RunId, EmailSettings, SenderAddress, SubjectLine, EmailBody, 
										RunId, EmailSettings, SenderAddress, EmailBody, EmailBody, 
										Recipients, CCList, BCCList, ClientAttachments, StatementSummaryId, TemplateId, ref EmailRequest);
								if (string.IsNullOrEmpty(Result))
								{
									using (TextWriter tw = new StreamWriter(FDocsLocation + Convert.ToString(RunId) + "_" + Convert.ToString(ClientId) + "_Request.json"))
									{
										tw.Write(EmailRequest.ToString());
									}
								}
							}
							
							if (string.IsNullOrEmpty(Result))
							{
								JObject DataObject = null;
								JObject jobj = JsonConvert.DeserializeObject<JObject>(EmailRequest.ToString());
								if (jobj == null)
								{
									Result = "Error deserializing the Email Request";
								}
								else
								{
									DataObject = (jobj["data"] as JObject);
									if (DataObject == null)
									{
										Result = "Missing data section in Request.";
									}
								}
								
								if (string.IsNullOrEmpty(Result))
								{
									string HtmlFileName = FDocsLocation + "0Test.html";
									//Result = Actions.SendEmail.Execute(DataObject, EmailBody, out ErrorMsg);
									//Result = Actions.SendRTFEmail.Execute(DataObject, FRTFFileName, out ErrorMsg);
									Result = 
										Actions.SendRTFEmail.Execute(
											DataObject, FCommonItem, FSettingsItem, FMasterId, FRTFFileName);
									//Result = ""; ErrorMsg = "";
									//if (string.IsNullOrEmpty(ErrorMsg) && (string.IsNullOrEmpty(Result)))
									WriteToOutput("After SendRTFEmail: Result: " + Result);
									//WriteToOutput("After SendRTFEmail: ErrorMsg: " + ErrorMsg);
									//if (string.IsNullOrEmpty(ErrorMsg)) 
									if (string.IsNullOrEmpty(Result)) 
										UpdateSentValues(EVAC_ID, SenderAddress, Recipients, CCList, BCCList, ExclusionList);
									else
									{
										//string Str = ErrorMsg;
										//if (string.IsNullOrEmpty(Str)) Str = Result;
										//UpdateSentComment(EVAC_ID, Str);
										//UpdateSentComment(EVAC_ID, ErrorMsg);
										UpdateSentComment(EVAC_ID, Result);
									}
									//Result = Actions.SendHTMLEmail.Execute(DataObject, HtmlFileName, out ErrorMsg);
								}
								if (!string.IsNullOrEmpty(ErrorMsg)) WriteToOutput("Error:" + ErrorMsg);
								if (!string.IsNullOrEmpty(Result)) WriteToOutput("Result:" + Result);
							}						
						}
					}
				}
			}
			//sbOutput.Append(JsonUtilities.EndJsonArray());
			//sbOutput.Append(JsonUtilities.EndJson());			
			if (string.IsNullOrEmpty(Result)) Result = EmailAttachments.ToString();
			
			return Result;
		}
		
		private string PopulateEmailSettings(Int64 CompanyClientId,
		                                     ref Actions.SendEmail_Common EmailSettings)
		{
			string Result = "";
			/*
			EmailSettings.EmailUserName = "rdenardo01@gmail.com";
			EmailSettings.EmailPassword = "wWt4nd2s8IVN1Tf6";
			EmailSettings.Host = "smtp-relay.SendInBlue.com";
			EmailSettings.Port = "587";
			EmailSettings.Timeout = 5000;
			*/
			string QueryText = 
				" SELECT CO_EMAILHOST, I_CO_EMAILPORT, CO_EMAILUSERNAME, CO_EMAILPASSWORD" +
				" FROM EACompanySettings" +
				" WHERE I_SETTINGS_COMPANYID = @CompanyClientId";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("CompanyClientId", CompanyClientId));
            UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, "PopulateEmailSettings", 0);

            DataTable dt = FirebirdUtilities.GetFirstDataTable(FConnectionString, QueryText, Params, 0);
			if ((dt != null) && (dt.Rows.Count > 0))
			{
				Object Obj = null;
				foreach (DataRow Row in dt.Rows)
				{
					Obj = Row["CO_EMAILHOST"];
					EmailSettings.Host = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					Obj = Row["I_CO_EMAILPORT"];
					EmailSettings.Port = (Obj == DBNull.Value) ? "0" : Convert.ToString(Obj);
					Obj = Row["CO_EMAILUSERNAME"];
					EmailSettings.EmailUserName = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					Obj = Row["CO_EMAILPASSWORD"];
					EmailSettings.EmailPassword = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
				}
			}
			else
			{
				Result = "No record returned for the CompanyClientId: " + Convert.ToString(CompanyClientId);
			}
			return Result;
		}
		
		private string UpdateClientEmailAddressItems(Int64 CompanyClientId, 
		                                             Int64 EVAC_ID, 
		                                             List<Actions.SendEmail_Recipient> Recipients)
		{
			string Result = "";
			string Method = "UpdateClientEmailAddressItems";
			List<string> Entries = new List<string>();
			foreach (Actions.SendEmail_Recipient Recipient in Recipients)
			{
				Entries.Add(Recipient.Address);
			}
			string ToEmail = string.Join(";", Entries);
			
			string QueryText =
				" UPDATE EAEventRunClients" +
				"    SET EVAC_ToEmail = @ToEmail" +
				" WHERE EVAC_ID = @EVAC_ID";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("EVAC_ID", EVAC_ID));
				Params.Add(new FbParameterItem("ToEmail", ToEmail));
            UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
            Result = FirebirdUtilities.DBExecute(FConnectionString, QueryText, Params, 0);
			if (!string.IsNullOrEmpty(Result))
			{
				Entries.Clear();
				Entries.Add(
					JsonUtilities.StartJsonObject("") +
					JsonUtilities.AddPropertyNameAndValue("EVAC_ID", EVAC_ID.ToString(), false) +
					JsonUtilities.EndJsonObject());
				Entries.Add(
					JsonUtilities.StartJsonObject("") +
					JsonUtilities.AddPropertyNameAndValue("ToEmail", ToEmail, false) +
					JsonUtilities.EndJsonObject());
				
				Result =
					//JsonUtilities.StartJson() +
						JsonUtilities.StartJsonObject("error") +
							JsonUtilities.AddPropertyNameAndValue("message", Result) +
							JsonUtilities.AddPropertyNameAndValue("query", QueryText) +
							JsonUtilities.StartJsonArray("values") +
							string.Join(",", Entries) +
							JsonUtilities.EndJsonArray() +
						JsonUtilities.EndJsonObject();
					//JsonUtilities.EndJson();
				LoggingEntry(Method, "", Result);
					
/*
				"Error executing query:" + Environment.NewLine + QueryText + Environment.NewLine +
					"Using the values:" + Environment.NewLine +
					"EVAC_ID: " + Convert.ToString(EVAC_ID) + Environment.NewLine +
					"ToEmail: " + ToEmail + Environment.NewLine +
					"ErrorMessage: " + Result;
*/
					WindowsEventLoggingUtilities.WriteToErrorEvent(Result, Method, 0);
			}
			
			return Result;
		}

		private string BuildCommonAttachments(Int64 CompanyClientId,
		                                      Int64 RunId,
		                                      ref List<string> CommonAttachments,
		                                      ref Int64 StatementSummaryId)
		{
			string Result = "";
			string Method = "BuildCommonAttachments";
			WriteToOutput("Inside: " + Method);
			
			// Gather the records from EAEmailAttachments for the runId
			/*
			string QueryText = 
				" SELECT ea_AttachmentName, ea_FileExtension, ea_Base64" +
				" FROM eaEmailAttachments" +
				" WHERE ea_EventRunId = @RunId AND ea_DocumentId = 0";
			*/
			
			string QueryText =
				" SELECT FileExtension, FileName, BlobFileName, StatementSummaryId, Entry_Source" +
				" FROM EAEventClientAttachment(@CompanyClientId, @RunId, 0)";

			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("CompanyClientId", CompanyClientId));
			Params.Add(new FbParameterItem("RunId", RunId));
            WriteParamsToOutput(QueryText, Params);
			
			string Msg = "Processing QueryText:" + Environment.NewLine + QueryText + Environment.NewLine + "For RunId: " + RunId.ToString();
			WindowsEventLoggingUtilities.WriteToInfoEvent(Msg, "EmailProcessor-ExecuteRun", 0);
			WriteToOutput(Msg);
			DataTable dt = FirebirdUtilities.GetFirstDataTable(FConnectionString, QueryText, Params, 0);
			if (dt == null)
			{
				Result = "No data table returned in " + Method;
				WindowsEventLoggingUtilities.WriteToInfoEvent(Result, Method, 0);
				Result = "";
			}
			else if (dt.Rows.Count > 0)
			{
				WriteToOutput("DocsLocation:" + FDocsLocation);
				Object Obj = null;
				string Str = "";
				string FileName = "";
				string FileExt = "";
				string EntrySource = "";
				string Base64String = "";
				const string FileNameBase = "{0}-{1}{2}";
				string WorkingFileName = "";
				Int64 TempStatementSummaryId = 0;
				foreach (DataRow Row in dt.Rows)
				{
					Obj = Row["FileName"];
					FileName = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					
					if (!string.IsNullOrEmpty(FileName))
					{
						// If the filename starts with a drive letter and : then extract the FileName only from this value
						FileName = Path.GetFileNameWithoutExtension(FileName);
						
						Obj = Row["FileExtension"];
						FileExt = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
						if (!string.IsNullOrEmpty(FileExt) && !FileExt.StartsWith(".")) FileExt = "." + FileExt;
						WorkingFileName = string.Format(FileNameBase, Convert.ToString(RunId), FileName, FileExt);
			        	WorkingFileName = FDocsLocation + WorkingFileName;
						Str =
							JsonUtilities.StartJsonObject("") +
							JsonUtilities.AddPropertyNameAndValue("fileName", WorkingFileName, false) +
							JsonUtilities.EndJsonObject();
						CommonAttachments.Add(Str);
						Obj = Row["BlobFileName"];
			        	byte[] toBytes = (byte[])Obj;
			        	Obj = Row["Entry_Source"];
			        	EntrySource = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);			        	
			        	if (CommonUtilities.StringUtilities.SameText(EntrySource, "EMailAttachments"))
			        	{
			        		File.WriteAllBytes(WorkingFileName, toBytes);
			        	}
			        	else
			        	{
			        		Base64String = System.Text.Encoding.Default.GetString(toBytes);
			        		File.WriteAllBytes(WorkingFileName, Convert.FromBase64String(Base64String));
			        	}
			        	
			        	if (StatementSummaryId == 0)
			        	{
			        		Obj = Row["StatementSummaryId"];
			        		TempStatementSummaryId = (Obj == DBNull.Value) ? 0 : Convert.ToInt64(Obj);
			        		if ((StatementSummaryId == 0) && (TempStatementSummaryId > 0)) StatementSummaryId = TempStatementSummaryId;
			        	}			        	
					}
				}
			}
			return Result;
		}
		
		/*
		private string BuildCommonAttachments(Int64 RunId,
		                                      ref List<string> CommonAttachments)
		{
			string Result = "";
			string Method = "BuildCommonAttachments";
			WriteToOutput("Inside: " + Method);
			
			// Gather the records from EAEmailAttachments for the runId
			string QueryText = 
				" SELECT ea_AttachmentName, ea_FileExtension, ea_Base64" +
				" FROM eaEmailAttachments" +
				" WHERE ea_EventRunId = @RunId AND ea_DocumentId = 0";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("RunId", RunId));
			
			string Msg = "Processing QueryText:" + Environment.NewLine + QueryText + Environment.NewLine + "For RunId: " + RunId.ToString();
			WindowsEventLoggingUtilities.WriteToInfoEvent(Msg, "EmailProcessor-ExecuteRun", 0);
			WriteToOutput(Msg);
			DataTable dt = FirebirdUtilities.GetFirstDataTable(FConnectionString, QueryText, Params, 0);
			if (dt == null)
			{
				Result = "No data table returned in " + Method;
				WindowsEventLoggingUtilities.WriteToInfoEvent(Result, Method, 0);
				Result = "";
			}
			else if (dt.Rows.Count > 0)
			{
				WriteToOutput("DocsLocation:" + FDocsLocation);
				Object Obj = null;
				string Str = "";
				string FileName = "";
				string FileExt = "";
				const string FileNameBase = "{0}-{1}{2}";
				string WorkingFileName = "";
				foreach (DataRow Row in dt.Rows)
				{
					Obj = Row["ea_AttachmentName"];
					FileName = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					if (!string.IsNullOrEmpty(FileName))
					{
						Obj = Row["ea_FileExtension"];
						FileExt = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
						if (!string.IsNullOrEmpty(FileExt) && !FileExt.StartsWith(".")) FileExt = "." + FileExt;
						WorkingFileName = string.Format(FileNameBase, Convert.ToString(RunId), FileName, FileExt);
			        	WorkingFileName = FDocsLocation + WorkingFileName;
						Str =
							JsonUtilities.StartJsonObject("") +
							JsonUtilities.AddPropertyNameAndValue("fileName", WorkingFileName, false) +
							JsonUtilities.EndJsonObject();
						CommonAttachments.Add(Str);
						Obj = Row["ea_Base64"];
			        	byte[] toBytes = (byte[])Obj;
			        	File.WriteAllBytes(WorkingFileName, toBytes);
					}
				}
			}
			return Result;
		}
		*/
		
		private string BuildAllBlobContents(Int64 CompanyClientId,
		                                    Int64 RunId,
		                                    Int64 EVAC_ID,
		                                    Int64 EventRunClientId)
		{
			string Result = "";
			
			string Method = "BuildAllBlobContents";
			string Source = this.FModuleName + " - " + Method + " for EventRunClientId: " + EventRunClientId.ToString();
			WriteToOutput(Source);
			
  		    string AppName = "";
  		    string ArgsBase = "";
  		    string[] delimiters = new string[]{"/", " "};
  		    List<string> ArgsList = new List<string>();

			//string ErrorMsg = "";
			string EventId = "";
			
			//string CmdBase = FTestRPOLocation + "{0}";
			//string CmdBase = FEAOWordLocation + "{0}";
			string CmdBase = "";
			
			WriteToOutput("RunId: " + Convert.ToString(RunId));
			WriteToOutput("EventRunClientId: " + Convert.ToString(EventRunClientId));			
			
			string AppDir = System.AppDomain.CurrentDomain.BaseDirectory;
			//string WorkingDir = FEAOWordLocation;
			string WorkingDir = "";

			List<string> CommandLines =
				GetRunCommandLines(FConnectionString, CompanyClientId, RunId, EVAC_ID, EventRunClientId);
				//GetWordDocumentCommandLines(FConnectionString, RunId, EventRunClientId);
			if (CommandLines.Count > 0) WriteToOutput("CommandLines.Count: " + CommandLines.Count);
			int tries = 60;
			foreach (string Item in CommandLines)
			{
				WriteToOutput("Command Line Item:" + Item);
				AppName = Item.Substring(0, Item.IndexOf(" /"));
				if (CommonUtilities.StringUtilities.SameText(AppName, "EAOWord"))
				{
					CmdBase = FEAOWordLocation;
					WorkingDir = FEAOWordLocation;
				}
				else
				{				
					CmdBase = FTestRPOLocation;
					WorkingDir = FTestRPOLocation;
				}
				CmdBase += "{0}";
				AppName = string.Format(CmdBase, AppName);
				
				//string ArgsBase = "/eventid:{0} /emailtemplateid:{1}";
				ArgsBase = Item.Substring(Item.IndexOf("/"));
				WindowsEventLoggingUtilities.WriteToInfoEvent("AppName: " + AppName, Source, 0);
				WindowsEventLoggingUtilities.WriteToInfoEvent("Args: " + ArgsBase, Source, 0);
				WriteToOutput("AppName: " + AppName);
				WriteToOutput("Args: " + ArgsBase);

				EventId = GetArgValue(ArgsBase, "EventId", out Result);				
				//WordDocumentId = GetArgValue(ArgsBase, "DocumentId", out Result);
				//WriteToOutput("EventId: " + EventId + " DocumentId: " + WordDocumentId);
				WriteToOutput("EventId: " + EventId);
				if (!string.IsNullOrEmpty(Result)) WriteToOutput("Error: " + Result);
				
				if (string.IsNullOrEmpty(Result))
				{
					Directory.SetCurrentDirectory(WorkingDir);
					try
					{
						Process runApp = new Process();
						runApp.StartInfo.FileName = AppName;
						runApp.StartInfo.Arguments = ArgsBase;
						runApp.Start();
						for (int Index = 0; Index < tries; Index++)
						{
							//if (!runApp.HasExited) System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
							if (!runApp.HasExited) System.Threading.Thread.Sleep(1000);
						}
						//runApp.WaitForExit(10000);
						
						WriteToOutput("Finished Creating Document");
						WriteToOutput("");
						
						if (runApp.HasExited)
						{
							
						}
					}
					finally
					{
						Directory.SetCurrentDirectory(AppDir);
					}
					
					/*
					if (string.IsNullOrEmpty(Result))
						Result = BuildWordDocumentFiles(Convert.ToInt64(EventId), ref Attachments);
					*/
				}
				//Result = ProcessTemplateOutputFile(StringUtilities.StrToIntDef(EventId, 0), ref ReportList);
				
				//ReportList.Append(JsonUtilities.EndJsonObject());
			}			
			
			return Result;
		}
		
		private List<string> GetRunCommandLines(string ConnectionString,
		                                        Int64 CompanyClientId,
		                                        Int64 RunId,
		                                        Int64 EVAC_ID,
		                                        Int64 EventRunClientId)
		{
			List<string> Result = new List<string>();
			
			string QueryText =
				" SELECT * FROM EAEVENTCLIENTCREATEFILE(@CompanyClientId, @RunId, @EVAC_ID)";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("CompanyClientId", CompanyClientId));
			Params.Add(new FbParameterItem("RunId", RunId));
			Params.Add(new FbParameterItem("EVAC_ID", EVAC_ID));
			
			string ErrorMsg = "";
			Int64 ReturnedClientId = 0;
			DataTable dt = FirebirdUtilities.GetFirstDataTable(FConnectionString, QueryText, Params, 0);
			if (dt == null) 
			{
				ErrorMsg = string.Concat(new string[] {
					"Error Executing Query",
					Environment.NewLine,
					"QueryText:",
					Environment.NewLine,
					QueryText
				});
				WriteToOutput(ErrorMsg);
			}
			else if (dt.Rows.Count == 0) 
			{
				ErrorMsg = "No rows in the table to process";
				WriteToOutput(ErrorMsg);
			}
			else 
			{
				if (dt.Rows.Count > 0) 
				{
					foreach (DataRow Row in dt.Rows) 
					{
						object obj = Row["CommandLine"];
						string CommandLine = (obj == DBNull.Value) ? "" : Convert.ToString(obj);
						obj = Row["ClientId"];
						ReturnedClientId = (obj == DBNull.Value) ? 0 : Convert.ToInt64(obj);
						if (!string.IsNullOrEmpty(CommandLine) && (ReturnedClientId == EventRunClientId))
						{
							Result.Add(CommandLine);
						}
					}
				}
			}
					
			return Result;
		}
		
		private string BuildAllBlobAttachments(Int64 CompanyClientId,
		                                       Int64 RunId,
		                                       Int64 EVAC_ID, 
		                                       Int64 ClientId, 
		                                       ref List<string> Attachments, 
		                                       ref Int64 StatementSummaryId)
		{
			string Result = "";
			
			string Method = "BuildAllBlobAttachments";
			string Source = this.FModuleName + " - " + Method;
			WriteToOutput(Source);
			
			WriteToOutput("RunId: " + Convert.ToString(RunId));
			
			string AppDir = System.AppDomain.CurrentDomain.BaseDirectory;
			string WorkingDir = FEAOWordLocation;
			
			string QueryText =
				" SELECT * FROM EAEventClientAttachment(@CompanyClientId, @RunID, @EVAC_ID)" +
				" WHERE ETemplateId = 0" +
				"   AND ((ReportId > 0) OR (DocumentId > 0) or (StorageId > 0))";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("CompanyClientId", CompanyClientId));
			Params.Add(new FbParameterItem("RunId", RunId));
			Params.Add(new FbParameterItem("EVAC_ID", EVAC_ID));
            //UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
            WriteToOutput("QueryText");
            WriteToOutput(QueryText);
            WriteParamsToOutput(QueryText, Params);
            WriteToOutput("After WriteParams");

            try
            {
                string ErrorMsg = "";
                DataTable dt = FirebirdUtilities.GetFirstDataTable(FConnectionString, QueryText, Params, 0);
                if (dt == null)
                {
                    ErrorMsg = string.Concat(new string[] {
                    "Error Executing Query",
                    Environment.NewLine,
                    "QueryText:",
                    Environment.NewLine,
                    QueryText
                });
                    WriteToOutput(ErrorMsg);
                }
                else if (dt.Rows.Count == 0)
                {
                    ErrorMsg = "No rows in the table to process";
                    WriteToOutput(ErrorMsg);
                }
                else
                {
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow Row in dt.Rows)
                        {
                            object obj = Row["FileName"];
                            string FileName = (obj == DBNull.Value) ? "" : Convert.ToString(obj);
                            obj = Row["FileExtension"];
                            string FileExt = (obj == DBNull.Value) ? "" : Convert.ToString(obj);
                            obj = Row["StatementSummaryId"];
                            Int64 tempStatementSummaryId = (obj == DBNull.Value) ? 0 : Convert.ToInt64(obj);
                            if ((StatementSummaryId == 0) && (tempStatementSummaryId > 0)) StatementSummaryId = tempStatementSummaryId;
                            /*
                            obj = Row["ec_DisplayName"];
                            string DisplayName = (obj == DBNull.Value) ? "" : Convert.ToString(obj);
                            */
                            string FileNameOnly = "{0}-{1}{2}";
                            if (!string.IsNullOrEmpty(FileName))
                            {
                                string DisplayName = FileName.Replace(" ", "_");
                                //FileNameOnly = "{0}{1}";
                                FileNameOnly = string.Format(FileNameOnly, EVAC_ID, DisplayName, FileExt);
                            }
                            else
                            {
                                //FileNameOnly = "{0}-{1}{2}";
                                FileNameOnly = string.Format(FileNameOnly, EVAC_ID, "Document", FileExt);
                            }

                            WriteToOutput("FileNameOnly: " + FileNameOnly);

                            string OutputFileName = FDocsLocation + FileNameOnly;
                            WriteToOutput("OutputFileName: " + OutputFileName);
                            //WindowsEventLoggingUtilities.WriteToInfoEvent("OutputFileName" + Environment.NewLine + OutputFileName, FModuleName, MasterId);
                            obj = Row["BlobFileName"];
                            byte[] toBytes = (byte[])obj;
                            string Base64String = System.Text.Encoding.Default.GetString(toBytes);
                            File.WriteAllBytes(OutputFileName, Convert.FromBase64String(Base64String));
                            string Str =
                                JsonUtilities.StartJsonObject("") +
                                JsonUtilities.AddPropertyNameAndValue("fileName", OutputFileName, false) +
                                JsonUtilities.EndJsonObject();
                            WriteToOutput("OutputJson" + Environment.NewLine + Str);
                            Attachments.Add(Str);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteToOutput(ex.Message);
                Result = ex.Message;
            }


			/*
			if (string.IsNullOrEmpty(Result))
						Result = BuildWordDocumentFiles(Convert.ToInt64(EventId), ref Attachments);
			}
			*/
				//Result = ProcessTemplateOutputFile(StringUtilities.StrToIntDef(EventId, 0), ref ReportList);
				
				//ReportList.Append(JsonUtilities.EndJsonObject());
			
			return Result;
		}
		
		private string BuildClientWordDocumentAttachments(Int64 RunId,
		                                                  Int64 EVAC_ID,
		                                                  Int64 EventRunClientId,
		                                                  ref List<string> Attachments) // ec_closeaccountsummaryid
		{
			string Result = "";
			
			string Method = "BuildClientWordDocumentAttachments";
			string Source = this.FModuleName + " - " + Method;
			WriteToOutput(Source);
			
  		    string AppName = "";
  		    string ArgsBase = "";
  		    string[] delimiters = new string[]{"/", " "};
  		    List<string> ArgsList = new List<string>();

			//string ErrorMsg = "";
			string EventId = "";
			string WordDocumentId = "";
			
			//string CmdBase = FTestRPOLocation + "{0}";
			string CmdBase = FEAOWordLocation + "{0}";
			
			WriteToOutput("RunId: " + Convert.ToString(RunId));
			WriteToOutput("EventRunClientId: " + Convert.ToString(EventRunClientId));			
			
			string AppDir = System.AppDomain.CurrentDomain.BaseDirectory;
			string WorkingDir = FEAOWordLocation;

			List<string> CommandLines =
				GetWordDocumentCommandLines(FConnectionString, RunId, EventRunClientId);
			if (CommandLines.Count > 0) WriteToOutput("CommandLines.Count: " + CommandLines.Count);
			int tries = 60;
			foreach (string Item in CommandLines)
			{
				WriteToOutput("Command Line Item:" + Item);
				AppName = Item.Substring(0, Item.IndexOf(" /"));
				AppName = string.Format(CmdBase, AppName);
				
				//string ArgsBase = "/eventid:{0} /emailtemplateid:{1}";
				ArgsBase = Item.Substring(Item.IndexOf("/"));
				WindowsEventLoggingUtilities.WriteToInfoEvent("AppName: " + AppName, Source, 0);
				WindowsEventLoggingUtilities.WriteToInfoEvent("Args: " + ArgsBase, Source, 0);
				WriteToOutput("AppName: " + AppName);
				WriteToOutput("Args: " + ArgsBase);

				EventId = GetArgValue(ArgsBase, "EventId", out Result);				
				WordDocumentId = GetArgValue(ArgsBase, "DocumentId", out Result);
				WriteToOutput("EventId: " + EventId + " DocumentId: " + WordDocumentId);
				if (!string.IsNullOrEmpty(Result)) WriteToOutput("Error: " + Result);
				
				if (string.IsNullOrEmpty(Result))
				{
					Directory.SetCurrentDirectory(WorkingDir);
					try
					{
						Process runApp = new Process();
						runApp.StartInfo.FileName = AppName;
						runApp.StartInfo.Arguments = ArgsBase;
						runApp.Start();
						for (int Index = 0; Index < tries; Index++)
						{
							//if (!runApp.HasExited) System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
							if (!runApp.HasExited) System.Threading.Thread.Sleep(1000);
						}
						//runApp.WaitForExit(10000);
						
						WriteToOutput("Finished Creating Document");
						
						if (runApp.HasExited)
						{
							
						}
					}
					finally
					{
						Directory.SetCurrentDirectory(AppDir);
					}
					
					if (string.IsNullOrEmpty(Result))
						Result = BuildWordDocumentFiles(Convert.ToInt64(EventId), ref Attachments);
				}
				//Result = ProcessTemplateOutputFile(StringUtilities.StrToIntDef(EventId, 0), ref ReportList);
				
				//ReportList.Append(JsonUtilities.EndJsonObject());
			}			
			
			return Result;
		}
		
		private string BuildWordDocumentFiles(Int64 EventId,
		                                      ref List<string> Attachments)
		{
			string Result = "";

			string FileNameOnly = "";
			string OutputFileName = "";
			string DisplayName = "";
			string FileExt = "";
			object Obj = null;
			string Base64String = "";
			string Str = "";

			// Get the ec_Base64 from the EAEventRunBase64 table for the EC_ID (EventId)
			string QueryText =
				"SELECT ec_Base64, ec_FileExtension, EC_DisplayName FROM eaeventrunbase64 WHERE EC_ID = @EventId";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("EventId", EventId));
			DataTable dt = FirebirdUtilities.GetFirstDataTable(FConnectionString, QueryText, Params, 0);
			if ((dt != null) && (dt.Rows.Count > 0))
			{
				foreach (DataRow Row in dt.Rows)
				{
					Obj = Row["ec_FileExtension"];					
					FileExt = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					Obj = Row["ec_DisplayName"];
					DisplayName = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					if (!string.IsNullOrEmpty(DisplayName))
					{
						DisplayName = DisplayName.Replace(" ", "_");
			        	FileNameOnly = "{0}{1}";
		        		FileNameOnly = string.Format(FileNameOnly, DisplayName, FileExt);
					}
					else
					{
			        	FileNameOnly = "{0}-{1}{2}";
		        		FileNameOnly = string.Format(FileNameOnly, EventId, "Document", FileExt);
					}
		        	OutputFileName = FDocsLocation + FileNameOnly;
		        	//WindowsEventLoggingUtilities.WriteToInfoEvent("OutputFileName" + Environment.NewLine + OutputFileName, FModuleName, MasterId);
		        	Obj = Row["EC_Base64"];
		        	byte[] toBytes = (byte[])Obj;
		        	Base64String = System.Text.Encoding.Default.GetString(toBytes);
		        	File.WriteAllBytes(OutputFileName, Convert.FromBase64String(Base64String));
					Str =
						JsonUtilities.StartJsonObject("") +
						JsonUtilities.AddPropertyNameAndValue("fileName", OutputFileName, false) +
						JsonUtilities.EndJsonObject();
		        	Attachments.Add(Str);
				}
			}
			
			return Result;
		}
		
		private List<string> GetWordDocumentCommandLines(string ConnectionString,
		                                                 Int64 RunId,
		                                                 Int64 EventRunClientId)
		{
			List<string> Result = new List<string>();
			
			string QueryText =
				" SELECT EC_CommandLine, ec_CloseAccountSummaryId FROM EAEventCreateFile" +
                " WHERE EC_EventRunId = @RunId" +
				"  AND EC_ClientId = @EventRunClientId" +
				"  AND EC_WordDocumentId > 0";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("RunId", RunId));
			Params.Add(new FbParameterItem("EventRunClientId", EventRunClientId));
			
			string ErrorMsg = "";
			DataTable dt = FirebirdUtilities.GetFirstDataTable(FConnectionString, QueryText, Params, 0);
			if (dt == null) 
			{
				ErrorMsg = string.Concat(new string[] {
					"Error Executing Query",
					Environment.NewLine,
					"QueryText:",
					Environment.NewLine,
					QueryText
				});
				WriteToOutput(ErrorMsg);
			}
			else if (dt.Rows.Count == 0) 
			{
				ErrorMsg = "No rows in the table to process";
				WriteToOutput(ErrorMsg);
			}
			else 
			{
				if (dt.Rows.Count > 0) 
				{
					foreach (DataRow Row in dt.Rows) 
					{
						object obj = Row["EC_CommandLine"];
						string CommandLine = (obj == DBNull.Value) ? "" : Convert.ToString(obj);
						if (!string.IsNullOrEmpty(CommandLine))
						{
							Result.Add(CommandLine);
						}
					}
				}
			}
					
			return Result;
		}

		
		private string BuildClientAttachments(Int64 RunId,
		                                      Int64 EVAC_ID,
		                                      Int64 EventRunClientId,
		                                      ref List<string> Attachments)
		{
			string Result = "";
			
			string Method = "BuildClientAttachments";
			string Source = this.FModuleName + " - " + Method;
			
  		    string AppName = "";
  		    string ArgsBase = "";
  		    string[] delimiters = new string[]{"/", " "};
  		    List<string> ArgsList = new List<string>();

			string ErrorMsg = "";
			string EventId = "";
			string EmailTemplateId = "";
			
			string CmdBase = FTestRPOLocation + "{0}";
			
			WriteToOutput("EventRunClientId: " + Convert.ToString(EventRunClientId));

			List<string> CommandLines =
				UsefulDBMethods.EventHelpers.GetEventCommandLine(FConnectionString, Convert.ToInt32(EventRunClientId), false, out Result);
			if (CommandLines.Count > 0) WriteToOutput("CommandLines.Count: " + CommandLines.Count);
			int tries = 60;
			foreach (string Item in CommandLines)
			{
				WriteToOutput("Command Line Item:" + Item);
				AppName = Item.Substring(0, Item.IndexOf(" /"));
				AppName = string.Format(CmdBase, AppName);
				
				//string ArgsBase = "/eventid:{0} /emailtemplateid:{1}";
				ArgsBase = Item.Substring(Item.IndexOf("/"));
				WindowsEventLoggingUtilities.WriteToInfoEvent("AppName: " + AppName, Source, 0);
				WindowsEventLoggingUtilities.WriteToInfoEvent("Args: " + ArgsBase, Source, 0);
				WriteToOutput("AppName: " + AppName);
				WriteToOutput("Args: " + ArgsBase);

				EventId = GetArgValue(ArgsBase, "EventId", out ErrorMsg);
				EmailTemplateId = GetArgValue(ArgsBase, "EmailTemplateId", out ErrorMsg);
				WriteToOutput("EventId: " + EventId + " EmailTemplateId: " + EmailTemplateId);
				
				Process runApp = new Process();
				runApp.StartInfo.FileName = AppName;
				runApp.StartInfo.Arguments = ArgsBase;
				runApp.Start();
				for (int Index = 0; Index < tries; Index++)
				{
					//if (!runApp.HasExited) System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
					if (!runApp.HasExited) System.Threading.Thread.Sleep(1000);
				}
				//runApp.WaitForExit(10000);
				
				WriteToOutput("Done");
				WriteToOutput("");
				
				// Get the ec_Base64 from the EAEventRunBase64 table for the EC_ID (EventId)
				//Result = ProcessTemplateOutputFile(StringUtilities.StrToIntDef(EventId, 0), ref ReportList);
				
				//ReportList.Append(JsonUtilities.EndJsonObject());
			}			
			
			return Result;
		}
		
		private string ExecuteRunCommands(Int64 EventRunClientId,
		                                 ref StringBuilder ReportList)
		{
			string Result = "";
			
			string Method = "ExcuteRunCommands";
			string Source = this.FModuleName + " - " + Method;
			
			string CmdBase = @"E:\temp\report\{0}";
  		    string AppName = "";
  		    string ArgsBase = "";
  		    string[] delimiters = new string[]{"/", " "};
  		    List<string> ArgsList = new List<string>();

			List<string> CommandLines = 
				UsefulDBMethods.EventHelpers.GetEventCommandLineForEventRunClientId(
					FConnectionString, (int)EventRunClientId, out Result);
			if (CommandLines.Count > 0) 
				WriteToOutput("CommandLines.Count: " + CommandLines.Count);
			string ErrorMsg = "";
			string EventId = "";
			string EmailTemplateId = "";
			foreach (string Item in CommandLines)
			{
				ReportList.Append(JsonUtilities.StartJsonObject(""));
				ReportList.Append(JsonUtilities.AddPropertyNameAndValue("evac_id", EventRunClientId.ToString()));
				ReportList.Append(JsonUtilities.AddPropertyNameAndValue("commandLine", Item));

				WriteToOutput("Command Line Item:" + Item);
				AppName = Item.Substring(0, Item.IndexOf(" /"));
				AppName = string.Format(CmdBase, AppName);
				
				//string ArgsBase = "/eventid:{0} /emailtemplateid:{1}";
				ArgsBase = Item.Substring(Item.IndexOf("/"));
				WindowsEventLoggingUtilities.WriteToInfoEvent("AppName: " + AppName, Source, 0);
				WindowsEventLoggingUtilities.WriteToInfoEvent("Args: " + ArgsBase, Source, 0);
				WriteToOutput("AppName: " + AppName);
				WriteToOutput("Args: " + ArgsBase);
				ReportList.Append(JsonUtilities.AddPropertyNameAndValue("args", ArgsBase));

				EventId = GetArgValue(ArgsBase, "EventId", out ErrorMsg);
				EmailTemplateId = GetArgValue(ArgsBase, "EmailTemplateId", out ErrorMsg);
				WriteToOutput("EventId: " + EventId + " EmailTemplateId: " + EmailTemplateId);
				ReportList.Append(JsonUtilities.AddPropertyNameAndValue("eventId", EventId));
				ReportList.Append(JsonUtilities.AddPropertyNameAndValue("emailTemplateId", EmailTemplateId));
				
				Process runApp = new Process();
				runApp.StartInfo.FileName = AppName;
				runApp.StartInfo.Arguments = ArgsBase;
				runApp.Start();
				runApp.WaitForExit(10000);
				
				WriteToOutput("Done");
				WriteToOutput("");
				
				// Get the ec_Base64 from the EAEventRunBase64 table for the EC_ID (EventId)
				Result = ProcessTemplateOutputFile(StringUtilities.StrToIntDef(EventId, 0), ref ReportList);
				
				ReportList.Append(JsonUtilities.EndJsonObject());
			}
			
			return Result;
		}
		
		private string GetArgValue(string Source,
		                           string ArgName,
		                           out string ErrorMsg)
		{
			string Result = "";
			ErrorMsg = "";
  		    string[] delimiters = new string[]{"/", " "};
  		    string[] ArgsDelimiters = new string[]{":", " "};
  		    string[] ArgsPair = null;
  		    //WriteToOutput("Source: " + Source);
			
  		    List<string> ArgsList = Source.Trim().Split(delimiters, StringSplitOptions.RemoveEmptyEntries).ToList();
  		    //WriteToOutput("Processing:" + Source + " For:" + ArgName);
			foreach (string Args in ArgsList)
			{
				ArgsPair = Args.Trim().Split(ArgsDelimiters, StringSplitOptions.RemoveEmptyEntries).ToArray();
				if (StringUtilities.SameText(ArgsPair[0], ArgName))
				{
					Result = ArgsPair[1];
				}
			}
			
			//WriteToOutput("For: " + ArgName + " returning: " + Result);
			
			return Result;
		}
		
		private string ProcessTemplateOutputFile(int EventId,
		                                         ref StringBuilder ReportList)
		{
			string Result = "";
			
			string QueryText =
				"SELECT EC_ID, EC_FileExtension, EC_Base64 FROM EAEventRunBase64 WHERE EC_ID = @EventId";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("EventId", EventId));
			DataTable dt = FirebirdUtilities.GetFirstDataTable(FConnectionString, QueryText, Params, 0);
						
			Object Obj = null;
			string FileNameOnly = "";
			string FileExtn = "";
			string DateTimeStringT = DateTime.UtcNow.ToString("yyyymmddTHHmmss");
			string OutputFileName = "";
			string OutputLocation = "";
			string Base64String = "";
			Int64 EC_ID = 0;
			
			try 
			{
				foreach (DataRow ARow in dt.Rows) // there should be a maximum of 1 rows so this is a little overkill but...
				{
					Obj = ARow["EC_ID"];
					EC_ID = (Obj == DBNull.Value)  ? 0 : Convert.ToInt64(Obj);
					ReportList.Append(JsonUtilities.AddPropertyNameAndValue("ec_id", Convert.ToString(EC_ID)));
					if (EC_ID > 0)
					{
						Obj = ARow["EC_FileExtension"];
						FileExtn = (Obj == DBNull.Value) ? "Unknown" : Convert.ToString(Obj);
			        	FileNameOnly = "{0}-{1}{2}";
			        	FileNameOnly = string.Format(FileNameOnly, Convert.ToString(EC_ID), DateTimeStringT, FileExtn);
			        	OutputLocation = @"\Docs\" + FileNameOnly;
			        	OutputFileName = @"e:\EAOffice\Development" + OutputLocation;
						Obj = ARow["EC_Base64"];
			        	byte[] toBytes = (byte[])Obj;
			        	Base64String = System.Text.Encoding.Default.GetString(toBytes);
			        	File.WriteAllBytes(OutputFileName, Convert.FromBase64String(Base64String));	        	
			        	ReportList.Append(JsonUtilities.AddPropertyNameAndValue("file", OutputFileName));
			        	ReportList.Append(JsonUtilities.AddPropertyNameAndValue("href", OutputLocation, false));
					}
				}
			}
			catch (Exception Ex)
			{
				Result = Ex.Message + Environment.NewLine + Ex.StackTrace;
			}
			
			return Result;
		}
		
		private string BuildEmailAddressItems(Int64 RunId, 
		                                      Int64 CompanyClientId, 
		                                      Int64 ClientId,
		                                      Int64 StatementSummaryId,
                                              out string SenderAddress, 		                                      
		                                      ref List<Actions.SendEmail_Recipient> Recipients, 
		                                      ref List<Actions.SendEmail_Recipient> CCList, 
		                                      ref List<Actions.SendEmail_Recipient> BCCList, 
		                                      ref List<Actions.SendEmail_Recipient> ExclusionList)
		{
			SenderAddress = "";
			string Result = "";
			
			string QueryText =
				" SELECT FromAddress, EMAILADDRESSES, CCADDRESS, BCCADDRESS, EXCLUSIONTO" +
				" FROM EventRunShowClients(@RunId, @CompanyClientId, @ClientId, @StatementSummaryId)";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("RunId", RunId));
			Params.Add(new FbParameterItem("CompanyClientId", CompanyClientId));
			Params.Add(new FbParameterItem("ClientId", ClientId));
			Params.Add(new FbParameterItem("StatementSummaryId", StatementSummaryId));
            /*
            string Msg =
                QueryText + Environment.NewLine +
                "RunId: " + RunId.ToString() + Environment.NewLine +
                "CompanyClientId: " + CompanyClientId.ToString() + Environment.NewLine +
                "ClientId: " + ClientId.ToString() + Environment.NewLine +
                "StatementSummaryId: " + StatementSummaryId.ToString();
            WriteToOutput(Msg);
            */
			
			Object Obj = null;
			string ColumnValue = "";
			Actions.SendEmail_Recipient EmailItem = null;
			DataTable tbl = FirebirdUtilities.GetFirstDataTable(FConnectionString, QueryText, Params, 0);
			foreach (DataRow Row in tbl.Rows)
			{
				Obj = Row["FromAddress"];
				SenderAddress = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);

				Obj = Row["EmailAddresses"];
				ColumnValue = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
				if (string.IsNullOrEmpty(ColumnValue))
				{
					Obj = Row["ExclusionTo"];
					ColumnValue = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
				}
                //WriteToOutput("ColumnValue: " + ColumnValue);
				foreach (string Item in ColumnValue.Split(new string[]{";"}, StringSplitOptions.RemoveEmptyEntries))
				{
					EmailItem = new Actions.SendEmail_Recipient();
					EmailItem.Address = Item;
					Recipients.Add(EmailItem);
				}
				
				Obj = Row["CCAddress"];
				ColumnValue = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
				foreach (string Item in ColumnValue.Split(new string[]{";"}, StringSplitOptions.RemoveEmptyEntries))
				{
					EmailItem = new Actions.SendEmail_Recipient();
					EmailItem.Address = Item;
					CCList.Add(EmailItem);
				}
				
				Obj = Row["BCCAddress"];
				ColumnValue = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
				foreach (string Item in ColumnValue.Split(new string[]{";"}, StringSplitOptions.RemoveEmptyEntries))
				{
					EmailItem = new Actions.SendEmail_Recipient();
					EmailItem.Address = Item;
					BCCList.Add(EmailItem);
				}
			}
			
			return Result;
		}
		
		private string	BuildCommonEmailSettings(Int64 RunId, 
		                                         out string SubjectLine, 
		                                         out string EmailBody,
		                                         out Int64 TemplateId)
		{
			string Result = "";
			SubjectLine = "";
			EmailBody = "";
			TemplateId = 0;
			
			string QueryText =
				" SELECT EVA_SubjectLine, EVA_EmailBody, I_EVA_EmailTemplateId" + 
				" FROM EAEventRun WHERE EVA_ID = @RunId";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("RunId", RunId));
			
			Object Obj = null;
			DataTable tbl = FirebirdUtilities.GetFirstDataTable(FConnectionString, QueryText, Params, 0);
			// There should only be one row returned but...
			if ((tbl != null) && (tbl.Rows.Count > 0))
			{
				foreach (DataRow Row in tbl.Rows)
				{
					Obj = Row["EVA_SubjectLine"];
					SubjectLine = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					Obj = Row["I_EVA_EmailTemplateId"];
					TemplateId = (Obj == DBNull.Value) ? 0 : Convert.ToInt64(Obj);
//					if (TemplateId > 0)
//					{
//						Obj = Row["EVA_EmailBody"];
//					}
					Obj = Row["EVA_EmailBody"];
				}
			}
			
			return Result;
		}		
		
		private string BuildClientEmailBody(Int64 CompanyClientId,
		                                    Int64 RunId,
		                                    Int64 EVAC_ID,
		                                    Int64 ClientId,
		                                    Int64 TemplateId,
		                                    out string EmailBody)
		{
			string Result = "";
			EmailBody = "";
			FRTFFileName = FDocsLocation + RunId.ToString() + "_" + ClientId.ToString() + ".rtf";
			
			string Method = "BuildClientEmailBody";
			string Source = this.FModuleName + " - " + Method;
			WriteToOutput(Source);
			
			string ErrorMsg = "";
			string QueryText = "";
			DataTable dt = null;
			
			if (TemplateId > 0)
			{
				QueryText =
					" SELECT FIRST 1 BlobFileName AS EmailBody FROM EAEventClientAttachment(@CompanyClientId, @RunId, @EVAC_ID)" +
					" WHERE ETemplateId = @TemplateId";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyClientId", CompanyClientId));
				Params.Add(new FbParameterItem("RunId", RunId));
				Params.Add(new FbParameterItem("EVAC_ID", EVAC_ID));
				Params.Add(new FbParameterItem("TemplateId", TemplateId));
                WriteParamsToOutput(QueryText, Params);
				dt = FirebirdUtilities.GetFirstDataTable(FConnectionString, QueryText, Params, 0);
			}
			else
			{
				QueryText = "SELECT FIRST 1 EVA_EmailBody AS EmailBody FROM EAEventRun WHERE EVA_ID = @RunId";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("RunId", RunId));
				dt = FirebirdUtilities.GetFirstDataTable(FConnectionString, QueryText, Params, 0);
			}
			if (dt == null) 
			{
				ErrorMsg = 
					"Error Executing Query" + Environment.NewLine +
					"QueryText:" + Environment.NewLine +
					QueryText;
				WriteToOutput(ErrorMsg);
			}
			else if (dt.Rows.Count == 0) 
			{
				ErrorMsg = "No rows in the table to process";
				WriteToOutput(ErrorMsg);
			}
			else 
			{
				if (dt.Rows.Count > 0) 
				{
					foreach (DataRow Row in dt.Rows) 
					{
						byte[] toBytes = new byte[0];
						object Obj = Row["EmailBody"];
						if (Obj != DBNull.Value) toBytes = (byte[])Obj;
			        	//byte[] toBytes = (byte[])Obj;
			        	File.WriteAllBytes(FRTFFileName, toBytes);
			        	EmailBody = System.Text.Encoding.Default.GetString(toBytes);
					}
				}
			}
			
			return Result;
		}

		/*
		private string BuildClientEmailBody(Int64 RunId,
		                                    Int64 ClientId,
		                                    Int64 TemplateId,
		                                    out string EmailBody)
		{
			string Result = "";
			EmailBody = "";
			
			string Method = "BuildClientEmailBody";
			string Source = this.FModuleName + " - " + Method;
			WriteToOutput(Source);
			
  		    string AppName = "";
  		    string ArgsBase = "";
  		    string[] delimiters = new string[]{"/", " "};
  		    List<string> ArgsList = new List<string>();

			//string ErrorMsg = "";
			string EventId = "";
			string ItemTemplateId = "";
			
			//string CmdBase = FTestRPOLocation + "{0}";
			string CmdBase = FEAOWordLocation + "{0}";
			
			WriteToOutput("RunId: " + Convert.ToString(RunId));
			WriteToOutput("EventRunClientId: " + Convert.ToString(ClientId));
			
			string AppDir = System.AppDomain.CurrentDomain.BaseDirectory;
			string WorkingDir = FEAOWordLocation;

			List<string> CommandLines = GetEmailTemplateCommandLines(FConnectionString, RunId, ClientId);
			int tries = 60;
			foreach (string Item in CommandLines)
			{
				WriteToOutput("Command Line Item:" + Item);
				AppName = Item.Substring(0, Item.IndexOf(" /"));
				AppName = string.Format(CmdBase, AppName);
				
				//string ArgsBase = "/eventid:{0} /emailtemplateid:{1}";
				ArgsBase = Item.Substring(Item.IndexOf("/"));
				WindowsEventLoggingUtilities.WriteToInfoEvent("AppName: " + AppName, Source, 0);
				WindowsEventLoggingUtilities.WriteToInfoEvent("Args: " + ArgsBase, Source, 0);
				WriteToOutput("AppName: " + AppName);
				WriteToOutput("Args: " + ArgsBase);

				EventId = GetArgValue(ArgsBase, "EventId", out Result);				
				ItemTemplateId = GetArgValue(ArgsBase, "EmailTemplateId", out Result);
				WriteToOutput("EventId: " + EventId + " Template: " + ItemTemplateId);
				if (!string.IsNullOrEmpty(Result)) WriteToOutput("Error: " + Result);
				
				if (string.IsNullOrEmpty(Result))
				{
					Directory.SetCurrentDirectory(WorkingDir);
					try
					{
						Process runApp = new Process();
						runApp.StartInfo.FileName = AppName;
						runApp.StartInfo.Arguments = ArgsBase;
						runApp.Start();
						for (int Index = 0; Index < tries; Index++)
						{
							//if (!runApp.HasExited) System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
							if (!runApp.HasExited) System.Threading.Thread.Sleep(1000);
						}
						//runApp.WaitForExit(10000);
						
						WriteToOutput("Finished Creating Document");
						
						if (runApp.HasExited)
						{
							
						}
					}
					finally
					{
						Directory.SetCurrentDirectory(AppDir);
					}
					
					if (string.IsNullOrEmpty(Result))
						Result = BuildTemplateEmailBody(RunId, ClientId, ItemTemplateId, out EmailBody);
				}
			}			
			return Result;
		}
		*/
		
		private List<string> GetEmailTemplateCommandLines(string ConnectionString,
		                                                  Int64 RunId,
		                                                  Int64 EventRunClientId)
		{
			List<string> Result = new List<string>();
			
			string QueryText =
				" SELECT EC_CommandLine FROM EAEventCreateFile" +
                " WHERE EC_EventRunId = @RunId" +
				"  AND EC_ClientId = @EventRunClientId" +
				"  AND EC_EmailTemplateId > 0";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("RunId", RunId));
			Params.Add(new FbParameterItem("EventRunClientId", EventRunClientId));
			
			string ErrorMsg = "";
			DataTable dt = FirebirdUtilities.GetFirstDataTable(FConnectionString, QueryText, Params, 0);
			if (dt == null) 
			{
				ErrorMsg = 
					"Error Executing Query" + Environment.NewLine +
					"QueryText:" + Environment.NewLine +
					QueryText;
				WriteToOutput(ErrorMsg);
			}
			else if (dt.Rows.Count == 0) 
			{
				ErrorMsg = "No rows in the table to process";
				WriteToOutput(ErrorMsg);
			}
			else 
			{
				if (dt.Rows.Count > 0) 
				{
					foreach (DataRow Row in dt.Rows) 
					{
						object obj = Row["EC_CommandLine"];
						string CommandLine = (obj == DBNull.Value) ? "" : Convert.ToString(obj);
						if (!string.IsNullOrEmpty(CommandLine))
						{
							Result.Add(CommandLine);
						}
					}
				}
			}
					
			return Result;
		}
		
		private string BuildTemplateEmailBody(Int64 RunId, 
		                                      Int64 ClientId, 
		                                      string TemplateId, 
		                                      out string EmailBody)
		{
			string Result = "";
			EmailBody = "";
			FRTFFileName = FDocsLocation + RunId.ToString() + "_" + ClientId.ToString() + ".rtf";
			
			string QueryText =
				" SELECT EC_Blob FROM EAEventCreateFile" +
				" WHERE EC_EventRunId = @RunId" +
				"  AND EC_ClientId = @ClientId" +
				"  AND EC_EmailTemplateId = @TemplateId";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("RunId", RunId));
			Params.Add(new FbParameterItem("ClientId", ClientId));
			Params.Add(new FbParameterItem("TemplateId", TemplateId));
			
			string ErrorMsg = "";
			DataTable dt = FirebirdUtilities.GetFirstDataTable(FConnectionString, QueryText, Params, 0);
			if (dt == null) 
			{
				ErrorMsg = 
					"Error Executing Query" + Environment.NewLine +
					"QueryText:" + Environment.NewLine +
					QueryText;
				WriteToOutput(ErrorMsg);
			}
			else if (dt.Rows.Count == 0) 
			{
				ErrorMsg = "No rows in the table to process";
				WriteToOutput(ErrorMsg);
			}
			else 
			{
				if (dt.Rows.Count > 0) 
				{
					foreach (DataRow Row in dt.Rows) 
					{
						object Obj = Row["EC_Blob"];
			        	byte[] toBytes = (byte[])Obj;
			        	//Base64String = System.Text.Encoding.Default.GetString(toBytes);
			        	//File.WriteAllBytes(OutputFileName, Convert.FromBase64String(Base64String));
			        	File.WriteAllBytes(FRTFFileName, toBytes);
			        	EmailBody = System.Text.Encoding.Default.GetString(toBytes);
					}
				}
			}
			
			return Result;
		}
		
		private string BuildJsonRequest(Int64 RunId, 
		                                Actions.SendEmail_Common EmailSettings, 
		                                string SenderAddress, 
		                                string SubjectLine, 
		                                string EmailBody, 
		                                List<Actions.SendEmail_Recipient> Recipients, 
		                                List<Actions.SendEmail_Recipient> CCList, 
		                                List<Actions.SendEmail_Recipient> BCCList, 
		                                List<string> EmailAttachments,
		                                Int64 StatementSummaryId,
		                                Int64 TemplateId,
		                                ref StringBuilder Request)
		{
			string Result = "";
			string Str = "";
			List<string> StrList = new List<string>();
			
			Request.Append(JsonUtilities.StartJson());			
			Request.Append(JsonUtilities.StartJsonObject("data"));
			
			// The RunId
			Request.Append(JsonUtilities.AddPropertyNameAndValue("runId", RunId));
			Request.Append(JsonUtilities.AddPropertyNameAndValue("docsLocation", FDocsLocation));
			Request.Append(JsonUtilities.AddPropertyNameAndValue("testRPOLocation", FTestRPOLocation));
			Request.Append(JsonUtilities.AddPropertyNameAndValue("eaWordLocation", FEAOWordLocation));
			
			// The common entries
			Request.Append(JsonUtilities.StartJsonObject("common"));
			Request.Append(JsonUtilities.AddPropertyNameAndValue("host", EmailSettings.Host));
			Request.Append(JsonUtilities.AddPropertyNameAndValue("port", EmailSettings.Port));
			Request.Append(JsonUtilities.AddPropertyNameAndValue("emailUserName", EmailSettings.EmailUserName));
			Request.Append(JsonUtilities.AddPropertyNameAndValue("emailPassword", EmailSettings.EmailPassword));
			Request.Append(JsonUtilities.AddPropertyNameAndValue("timeout", EmailSettings.Timeout, false));
			Request.Append(JsonUtilities.EndJsonObject());
			Request.Append(",");
			
			// The SenderAddress
			//SenderAddress = "johnebennett_au@yahoo.com.au";

			Request.Append(JsonUtilities.AddPropertyNameAndValue("senderAddress", SenderAddress));
			
			// The Subject
			Request.Append(JsonUtilities.AddPropertyNameAndValue("emailSubject", SubjectLine));
			
			Request.Append(JsonUtilities.AddPropertyNameAndValue("templateId", TemplateId));
			// The Body
			// replace any crlf's with /r/n
			/*
			if (EmailBody.IndexOf("\r\n") > 0)
			{
				WriteToOutput("we have CRLFs");
				Str = EmailBody.Replace("\r\n", "<br>");
			}
			else if (EmailBody.IndexOf("\r") > 0)
			{
				WriteToOutput("we have CRs");
				Str = EmailBody.Replace("\r", "<br>");
			}
			else if (EmailBody.IndexOf("\n") > 0)
			{
				WriteToOutput("we have NLs");
				Str = EmailBody.Replace("\n", "<br>");
			}
			*/
			Request.Append(JsonUtilities.AddPropertyNameAndValue("emailBody", ""));
			
			// The Recipients
			Request.Append(JsonUtilities.StartJsonArray("recipients"));
			StrList.Clear();
			foreach (Actions.SendEmail_Recipient Item in Recipients)
			{
				Str =
					JsonUtilities.StartJsonObject("") +
					JsonUtilities.AddPropertyNameAndValue("address", Item.Address, false) +
					//JsonUtilities.AddPropertyNameAndValue("address", "Rdenardo01@gmail.com", false) +
					//JsonUtilities.AddPropertyNameAndValue("address", "johnebennett_au@yahoo.com.au", false) +
					JsonUtilities.EndJsonObject();
				StrList.Add(Str);
			}
			Request.Append(string.Join(",", StrList));
			Request.Append(JsonUtilities.EndJsonArray());
			Request.Append(",");
			
			// The CCList
			Request.Append(JsonUtilities.StartJsonArray("cc"));
			StrList.Clear();
			foreach (Actions.SendEmail_Recipient Item in CCList)
			{
				Str =
					JsonUtilities.StartJsonObject("") +
					JsonUtilities.AddPropertyNameAndValue("address", Item.Address, false) +
					//JsonUtilities.AddPropertyNameAndValue("address", "johnebennett_au@yahoo.com.au", false) +
					JsonUtilities.EndJsonObject();
				StrList.Add(Str);
			}
			Request.Append(string.Join(",", StrList));
			Request.Append(JsonUtilities.EndJsonArray());
			Request.Append(",");

			// The BCCList
			Request.Append(JsonUtilities.StartJsonArray("bcc"));
			StrList.Clear();
			foreach (Actions.SendEmail_Recipient Item in BCCList)
			{
				Str =
					JsonUtilities.StartJsonObject("") +
					JsonUtilities.AddPropertyNameAndValue("address", Item.Address, false) +
					//JsonUtilities.AddPropertyNameAndValue("address", "johnebennett_au@yahoo.com.au", false) +
					JsonUtilities.EndJsonObject();
				StrList.Add(Str);
			}
			Request.Append(string.Join(",", StrList));
			Request.Append(JsonUtilities.EndJsonArray());
			Request.Append(",");

			// The Attachments
			Request.Append(JsonUtilities.StartJsonArray("attachments"));
			/*
			StrList.Clear();
			foreach (string Item in EmailAttachments)
			{
				Str =
					JsonUtilities.StartJsonObject("") +
					JsonUtilities.AddPropertyNameAndValue("path", "") +
					JsonUtilities.AddPropertyNameAndValue("fileName", Item, false) +
					JsonUtilities.EndJsonObject();
				StrList.Add(Str);
			}
			Request.Append(string.Join(",", StrList));
			*/
			Request.Append(string.Join(",", EmailAttachments));
			Request.Append(JsonUtilities.EndJsonArray());
			Request.Append(",");
			
			// The Images
			Request.Append(JsonUtilities.StartJsonArray("images"));
			Request.Append(JsonUtilities.EndJsonArray());
			Request.Append(",");
			
			// The Body Images
			Request.Append(JsonUtilities.StartJsonArray("bodyImages"));
			Request.Append(JsonUtilities.EndJsonArray());
			Request.Append(",");
			
			// The StatementSummaryId
			Request.Append(JsonUtilities.AddPropertyNameAndValue("StatementSummaryId", Convert.ToString(StatementSummaryId), false));				
			
			Request.Append(JsonUtilities.EndJsonObject());			
			Request.Append(JsonUtilities.EndJson());
			
			return Result;
		}
		
		//private void UpdateSentValues(Int64 EVAC_ID)
		private void UpdateSentValues(Int64 EVAC_ID,
		                              string SenderAddress,
		                              List<Actions.SendEmail_Recipient> Recipients,
		                              List<Actions.SendEmail_Recipient> CCList,
		                              List<Actions.SendEmail_Recipient> BCCList,
		                              List<Actions.SendEmail_Recipient> ExclusionList)
		{
			string MethodName = "UpdateSentValues";
			WriteToOutput("Inside " + MethodName);
			string CurrentTime = DateTime.Now.ToString("HH:mm:ss");
			DateTime CurrentDate = DateTime.Now;
			
			string Result = "";
			try
			{
				WriteToOutput("Processing SenderAddress");
				/*
				if (string.IsNullOrEmpty(SenderAddress))
				{
					WriteToOutput("********************************************************************************");
					WriteToOutput("SenderAddress is EMPTY");
					WriteToOutput("********************************************************************************");
				}
				else SenderAddress = SenderAddress.Substring(0, 120);
				*/
				SenderAddress = DelphiSubstring(SenderAddress, 0, 120);
				
				WriteToOutput("Processing Recipients");
				string StrRecipients = BuildAddressList(Recipients); //string.Join(";", Recipients);
				/*
				if (string.IsNullOrEmpty(StrRecipients))
				{
					WriteToOutput("********************************************************************************");
					WriteToOutput("Recipients is EMPTY");
					WriteToOutput("********************************************************************************");
				}
				else StrRecipients = StrRecipients.Substring(0, 2000);
				*/
				StrRecipients = DelphiSubstring(StrRecipients, 0, 2000);				

				WriteToOutput("Processing CCList");
				string StrCC = BuildAddressList(CCList);//string.Join(";", CCList);
				/*
				if (string.IsNullOrEmpty(StrCC))
				{
					WriteToOutput("********************************************************************************");
					WriteToOutput("CC List is EMPTY");
					WriteToOutput("********************************************************************************");
				}
				else StrCC = StrCC.Substring(0, 120);
				*/
				StrCC = DelphiSubstring(StrCC, 0, 120);
				
				WriteToOutput("Processing BCC List");
				string StrBCC = BuildAddressList(BCCList);//string.Join(";", BCCList);
				/*
				if (string.IsNullOrEmpty(StrBCC))
				{
					WriteToOutput("********************************************************************************");
					WriteToOutput("BCC List is EMPTY");
					WriteToOutput("********************************************************************************");
				}
				else StrBCC = StrBCC.Substring(0, 120);
				*/
				StrBCC = DelphiSubstring(StrBCC, 0, 120);
				
				WriteToOutput("Processing Exclusions");
				string StrExclusions = BuildAddressList(ExclusionList);//string.Join(";", ExclusionList);
				/*
				if (string.IsNullOrEmpty(StrExclusions))
				{
					WriteToOutput("********************************************************************************");
					WriteToOutput("Exclusions List is EMPTY");
					WriteToOutput("********************************************************************************");
				}
				else StrExclusions = StrExclusions.Substring(0, 120);
				*/
				StrExclusions = DelphiSubstring(StrExclusions, 0, 120);
				
				string QueryText = 
					" UPDATE EAEventRunClients" +
					"    SET B_EVAC_SentEvent = @SentEvent," +
					"        D_EVAC_SENTDATE = @SentDate," +
					"        EVAC_SENTTIME = @SentTime," +
					"        EVAC_FromEmail = @FromEmail," +
					"        EVAC_ToEmail = @ToEmail," +
					"        EVAC_CCEmail = @CCEmail," +
					"        EVAC_BCCEmail = @BCCEmail," +
					"        EVAC_ExclusionEmail = @Exclusions" +
					" WHERE EVAC_ID = @EVAC_ID";
					List<FbParameterItem> Params = new List<FbParameterItem>();
					Params.Add(new FbParameterItem("EVAC_ID", EVAC_ID));
					Params.Add(new FbParameterItem("SentEvent", "Y"));
					Params.Add(new FbParameterItem("SentDate", CurrentDate));
					Params.Add(new FbParameterItem("SentTime", CurrentTime));
	
					Params.Add(new FbParameterItem("FromEmail", SenderAddress));
					Params.Add(new FbParameterItem("ToEmail", StrRecipients));
					Params.Add(new FbParameterItem("CCEmail", StrCC));
					Params.Add(new FbParameterItem("BCCEmail", StrBCC));
					Params.Add(new FbParameterItem("Exclusions", StrExclusions));
	
				WriteParamsToOutput(QueryText, Params);
				Result = FirebirdUtilities.DBExecute(FConnectionString, QueryText, Params, 0);
				if (!string.IsNullOrEmpty(Result))
				{
					string Msg =
						"Error executing query:" + Environment.NewLine + QueryText + Environment.NewLine +
						"Using the values:" + Environment.NewLine +
						"EVAC_ID: " + Convert.ToString(EVAC_ID) + Environment.NewLine +
						"SentEvent: \"Y\"" + Environment.NewLine +
						"SentDate DD/MM/YYYY: " + CurrentDate.ToString("dd/MM/yyyy") + Environment.NewLine +
						"SentTime: " + CurrentTime;
						
					//WindowsEventLoggingUtilities.WriteToErrorEvent(Msg, MethodName, 0);
					WriteToOutput(Msg);
					WriteToOutput("DBExecute Result:" + Environment.NewLine + Result);
				}
			}
			catch (Exception Ex)
			{
				Result = Ex.Message + Environment.NewLine + Ex.StackTrace;
				WriteToOutput(Result);				
			}
		}
		
		private void UpdateSentComment(Int64 EVAC_ID, 
		                               string ErrorMsg)
		{
			string MethodName = "UpdateSentComment";
			WriteToOutput("Inside " + MethodName);
			WriteToOutput("ErrorMsg " + ErrorMsg);
			string CurrentTime = DateTime.Now.ToString("HH:mm:ss");
			DateTime CurrentDate = DateTime.Now;
			string QueryText = 
				" UPDATE EAEventRunClients" +
				"    SET EVAC_SentComments = @SentComment," +
				"        D_EVAC_SENTDATE = @SentDate," +
				"        EVAC_SENTTIME = @SentTime" +
				" WHERE EVAC_ID = @EVAC_ID";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("EVAC_ID", EVAC_ID));
				Params.Add(new FbParameterItem("SentComment", ErrorMsg));
				Params.Add(new FbParameterItem("SentDate", CurrentDate));
				Params.Add(new FbParameterItem("SentTime", CurrentTime));
			WriteParamsToOutput(QueryText, Params);
			
			string Result = "";
			if (string.IsNullOrEmpty(Result))
			{
				Result = FirebirdUtilities.DBExecute(FConnectionString, QueryText, Params, 0);
			
				if (!string.IsNullOrEmpty(Result))
				{
					string Msg =
						"Error executing query:" + Environment.NewLine + QueryText + Environment.NewLine + 
						Result + Environment.NewLine +
						"Using the values:" + Environment.NewLine +
						"EVAC_ID: " + Convert.ToString(EVAC_ID) + Environment.NewLine +
						"EVAC_SentComments: " + ErrorMsg + Environment.NewLine +
						"SentDate DD/MM/YYYY: " + CurrentDate.ToString("dd/MM/yyyy") + Environment.NewLine +
						"SentTime: " + CurrentTime;
						
					WindowsEventLoggingUtilities.WriteToErrorEvent(Msg, MethodName, 0);
					WriteToOutput(Result);
				}
			}
		}
		
		private string LoggingEntry(string Method,
		                            string Msg,
		                            string DataAsJson)
		{
			string Result = "";
			string ErrorMsg = "";
			int TempId = 0;
			bool res = false;
			
			try
			{
				res =
					ProgressLogging.BuildContentAndLog(
						true, FMasterId, 
						GeneralResponseStatus.LoggingEntry, 
						"", Msg, "", "", Method, 
						JsonContentEntryType.Logging_Entry, 
						DataAsJson,
						FCommonItem.CompanyIdValue,
						FCommonItem.ClientIdValue,
						out TempId, out Result, out ErrorMsg);
				//if (!string.IsNullOrEmpty(ErrorMsg)) Result = ErrorMsg;
				if (res) Result = "";
			}
			catch (Exception Ex)
			{
				//Result = LogOnException(Method, Ex, "");
				Result = Ex.Message + Environment.NewLine + Ex.InnerException;
			}

			return Result;
		}
		
		private void WriteToOutput(string LineText)
		{
			string FileName = "EmailProcessor_" + FRunId.ToString() + ".txt";
			FileName = FWorkingDir + FileName;			
			using (StreamWriter writer = new StreamWriter(FileName, true))
			{
				writer.WriteLine(LineText);
				Console.WriteLine(LineText);
			}
		}
		
		private void WriteParamsToOutput(string QueryText,
		                                 List<FbParameterItem> Params)
		{
			/*
            WriteToOutput("QueryText:" + Environment.NewLine + QueryText);
			foreach (FbParameterItem Item in Params)
			{
				WriteToOutput(Item.ParameterName + ": " + Convert.ToString(Item.ParameterValue));
			}
			*/
            string Msg = QueryText;
            if (Params != null)
            {
                foreach (FbParameterItem Param in Params)
                {
                    Msg += Environment.NewLine;
                    Msg = Msg + Param.ParameterName + ": " + Convert.ToString(Param.ParameterValue);
                }
            }

            WriteToOutput(Msg);
        }

        /// <summary>
        /// The standard C# Substring does not allow for the StartIndex to
        /// be greater than the length of the Source string. It also does not
        /// allow for the StartIndex + Count to be greater than the Source
        /// string length.
        /// Remember that C# strings are 0 indexed. An empty string has a length of 0
        /// </summary>
        /// <param name="Source">The string to use as the source to get the Substring</param>
        /// <param name="StartIndex">The starting index in the Source string to use</param>
        /// <param name="Count">The number of characters that should be in the returned string after processing</param>
        /// <returns></returns>
        private string DelphiSubstring(string Source, int StartIndex, int Count)
		{
			string Result = Source;
			
			/*
			WriteToOutput("Inside DelphiSubstring");
			WriteToOutput("Source: " + Source);
			WriteToOutput("StartIndex: " + StartIndex.ToString());
			WriteToOutput("Count: " + Count.ToString());
			*/
			
			if (string.IsNullOrEmpty(Source))
			{
				//WriteToOutput("Source is empty - Result: " + Result);
				return Result;
			}
			
			int StrLength = Source.Length;
			int MinRequiredStrLength = StartIndex + Count + 1;
			
			//WriteToOutput("StrLength: " + StrLength.ToString());
			//WriteToOutput("MinRequiredStrLength: " + MinRequiredStrLength.ToString());
			
			if (StrLength >= MinRequiredStrLength)
			{
				//WriteToOutput("StrLength is >= MinRequiredStrLength");
				Result = Source.Substring(StartIndex, Count);
				return Result;
			}
			
			// The length of the Source string is less than the required size
			if (StrLength <= (StartIndex + 1))
			{
				//WriteToOutput("StrLength <= StartIndex + 1");
				return "";
			}
			
			// The length of the SourceString < MinRequiredStrLength so adjust Count to accommodate
			/*
			Count = StrLength - StartIndex - 1;
			WriteToOutput("After adjusting Count to: " + Count.ToString());
			return Source.Substring(StartIndex, Count);
			*/
			// Just return the remainder of the string from the StartIndex
			return Source.Substring(StartIndex);
		}
		
		private string BuildAddressList(List<Actions.SendEmail_Recipient> Source)
		{
			string Result = "";
			List<string> Addresses = new List<string>();
			foreach(Actions.SendEmail_Recipient Item in Source)
			{
				Addresses.Add(Item.Address);
			}
			Result = string.Join(";", Addresses.ToArray());
			return Result;
		}
		
		private string BuildFolderItems(Int64 CompanyId)
		{
			string Result = "";
			try 
			{
				string QueryText = 
					" SELECT CO_DocsLocation, CO_TestRPOLocation, CO_EAWordLocation" +
					" FROM EACompanySettings WHERE I_Settings_CompanyId = @CompanyId";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				OutputParamsInfo(QueryText, Params);
				
				DataTable dt = FirebirdUtilities.GetFirstDataTable(FConnectionString, QueryText, Params, 0);
				if ((dt == null) || (dt.Rows.Count == 0))
				{
					Result = "No data table returned";
					WriteToOutput(Result);
				}
				else
				{
					string Sep = Path.DirectorySeparatorChar.ToString();
					DataRow dr = dt.Rows[0];
					object Obj = dr["CO_DocsLocation"];
					FDocsLocation = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					if (!string.IsNullOrEmpty(FDocsLocation) && (!FDocsLocation.EndsWith(Sep))) FDocsLocation += Sep;
					Obj = dr["CO_TestRPOLocation"];
					FTestRPOLocation = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					if (!string.IsNullOrEmpty(FTestRPOLocation) && (!FTestRPOLocation.EndsWith(Sep))) FTestRPOLocation += Sep;
					Obj = dr["CO_EAWordLocation"];
					FEAOWordLocation = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					if (!string.IsNullOrEmpty(FEAOWordLocation) && (!FEAOWordLocation.EndsWith(Sep))) FEAOWordLocation += Sep;
				}
			} 
			catch (Exception Ex)
			{
				Result = Ex.Message + Environment.NewLine + Ex.StackTrace;
			}
			return Result;
		}
		
		private void OutputParamsInfo(string QueryText,
		                              List<FbParameterItem> Params)
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine(QueryText);
			foreach (FbParameterItem Item in Params)
			{
				sb.AppendLine(Item.ParameterName + ": " + Convert.ToString(Item.ParameterValue));
			}
			WriteToOutput(sb.ToString());
		}
	}
}
