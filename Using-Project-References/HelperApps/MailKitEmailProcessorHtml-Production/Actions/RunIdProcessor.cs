/*
 * Created by SharpDevelop.
 * User: johne
 * Date: 12/11/2023
 * Time: 6:15 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CommonUtilities;
using System.Data;
//using EAOfficeAPI;
using System.Text;
using System.Diagnostics;

namespace MailKitEmailProcessor.Actions
{
	/// <summary>
	/// Description of RunIdProcessor.
	/// </summary>
	public class RunIdProcessor
	{
		/*
		private AccountDetails m_AccountDetails;
		private NameAndAddressItem m_Sender;
		private List<NameAndAddressItem> m_Recipients;
		private List<NameAndAddressItem> m_CCRecipients;
		private List<NameAndAddressItem> m_BCCRecipients;
		private List<NameAndAddressItem> m_ExclusionList;
		private string m_Subject = "";
		private string m_EmailBody = "";
		private List<EmailBodyImage> m_EmailBodyImages;
		private List<EmailAttachment> m_EmailAttachments;
		*/
		
		private SendEmailParam SendEmailParam;
		
		private string m_EmailBodyContent = "";
		private string m_EmailBodyFileName = "";
		private string m_UniqueFileId = "";
		private int m_RunId = 0;

		private EAOfficeAPI.APISettingsItem m_SettingsItem = null;
		private EAOfficeAPI.RequestCommonItem m_CommonItem = new EAOfficeAPI.RequestCommonItem();
		private string m_ConnectionString = "";		
		private string m_ModuleName = "Email Processor";
		private string m_DocsLocation = @"e:\EAOffice\Development\Docs\";
		private string m_RTFFileName = "";
		private string m_EAOWordLocation = @"E:\CreateDocs\";
		private string m_TestRPOLocation = @"E:\CreateDocs\";
		private int m_MasterId = 0;
		private string m_WorkingDir = "";
        private int m_RetryCount = 300;
		private string ShareDir = "";
		private ConfigSettings m_ConfigSettings;
		
		private Int64 m_CompanyClientId = 0;
		private string m_CompanyActiveStatus = "";

		
		public RunIdProcessor()
		{
			/*
			m_AccountDetails = new AccountDetails();
			m_Sender = new NameAndAddressItem();
			m_Recipients = new List<NameAndAddressItem>();
			m_CCRecipients = new List<NameAndAddressItem>();
			m_BCCRecipients = new List<NameAndAddressItem>();
			m_ExclusionList = new List<NameAndAddressItem>();
			m_Subject = "";
			m_EmailBody = "";
			m_EmailBodyImages = new List<EmailBodyImage>();
			m_EmailAttachments = new List<EmailAttachment>();
			*/
			
			SendEmailParam = new SendEmailParam();
			
			m_UniqueFileId = "";
		}

		public string Execute(int RunId)
		{
			string Result = "";
			
			m_UniqueFileId = Convert.ToString(RunId);
			m_RunId = RunId;
			
            m_SettingsItem = EAOfficeAPI.APISettingsFile.GetAPISettings(EAOfficeAPI.APIAppType.Service, out Result);
			m_SettingsItem.DebugLogging = true;
            m_CommonItem.Plugin = "MailKit_RunId_EmailProcessor";
			
			if (string.IsNullOrEmpty(Result))
			{
				m_ConnectionString = CommonDBMethods.Common.ConnectionString(m_SettingsItem);
			}
			
			WriteToOutput("Connection String:");
			WriteToOutput(m_ConnectionString);
			m_WorkingDir =
				new Uri(
	            	System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).LocalPath;
            m_WorkingDir = Path.GetDirectoryName(m_WorkingDir);
            string SepChar = Path.DirectorySeparatorChar.ToString();
            if (!m_WorkingDir.EndsWith(SepChar)) m_WorkingDir += SepChar;
			Console.WriteLine("WorkingDir: " + m_WorkingDir);
			
			if (string.IsNullOrEmpty(Result))
			{
				// Load the same settings and path mappings as the production executable.
                try
                {
                    m_ConfigSettings = ConfigSettings.LoadFromJson(m_WorkingDir + "MailKitEmailProcessorSettings.json");
                    m_DocsLocation = m_ConfigSettings.LocalDocsLocation;
                    m_EAOWordLocation = m_ConfigSettings.LocalCreateDocsLocation;
                    m_TestRPOLocation = m_ConfigSettings.LocalCreateDocsLocation;
                    m_RetryCount = m_ConfigSettings.RetryCount;
                    ShareDir = m_ConfigSettings.ShareDir;
                }
                catch (Exception Ex)
                {
                    Result = "Error loading ConfigSettings: " + Ex.Message;
                    CommonMethods.WriteToOutputLog(m_RunId, Result);
                }
			}
			
			if (string.IsNullOrEmpty(Result))
			{
				Result = ProcessRun();
			}
			else
			{
				WriteToOutput(Result);
			}

			return Result;
		}
		
		private string ProcessRun()
		{
			string Result = "";

			string Method = "ExecuteRun";
			string ErrorMsg = "";
            string FileName = "";
            string Msg = "";
			//int MasterId = 0;
			
			m_CompanyClientId = 
				UsefulDBMethods.EventHelpers.GetCustomerClientIdForEventRunId(
						m_ConnectionString, m_RunId, out Result);
			
			if (string.IsNullOrEmpty(Result))
			{
				Result = BuildFolderItems(m_CompanyClientId);
			}
			
			// Log that we have landed here - use the MasterId going forward to identify this processes log entries
			string Content = 
				JsonUtilities.StartJson() +
				JsonUtilities.StartJsonObject("common") +
				JsonUtilities.AddPropertyNameAndValue("plugin", "EmailProcessor") +
				JsonUtilities.AddPropertyNameAndValue("companyClientId", m_CompanyClientId.ToString()) +
				JsonUtilities.AddPropertyNameAndValue("Action", "ExecuteRun", false) +
				JsonUtilities.EndJsonObject() +
				"," +
				JsonUtilities.StartJsonObject("data") +
				JsonUtilities.AddPropertyNameAndValue("runId", m_RunId.ToString()) +
				JsonUtilities.AddPropertyNameAndValue("docsLocation", m_DocsLocation) +
				JsonUtilities.AddPropertyNameAndValue("testRPOLocation", m_TestRPOLocation) +
				JsonUtilities.AddPropertyNameAndValue("eaWordLocation", m_EAOWordLocation, false) +
				JsonUtilities.EndJsonObject() +
				JsonUtilities.EndJson();
			ProgressLogging.AddRecord(
				true, 0, m_CommonItem.Plugin, m_CommonItem.Command, "ExecuteRun", //CommonItem.Action, 
				Content, JsonContentEntryType.Request, (int)m_CompanyClientId, 0, out m_MasterId, out Result);

			m_CommonItem.Debug = "true";

			/*
			WindowsEventLoggingUtilities.WriteToInfoEvent("Before calling BuildCommonAttachments", "Test", 0);
			*/
			/*
			Actions.SendEmail_Common EmailSettings = new Actions.SendEmail_Common();
			*/
            string m_CompanyActiveStatus = 
                Actions.CommonMethods.GetCompanyActiveStatus(m_ConnectionString, (int)m_CompanyClientId, out Result);
            //m_CompanyActiveStatus = CompanyActiveStatus;
            WriteToOutput("CompanyActiveStatus: " + m_CompanyActiveStatus);

            if (string.IsNullOrEmpty(Result))
            {
	            AccountDetails AccountDetails = new AccountDetails();	            
				Result = PopulateEmailSettings(m_CompanyClientId, ref AccountDetails);
				if (string.IsNullOrEmpty(Result))
				{
					Result = SendEmailParam.CopyAccountDetailsFrom(AccountDetails);
				}
            }
            /*
			List<Actions.SendEmail_Recipient> Recipients = new List<Actions.SendEmail_Recipient>();
			List<Actions.SendEmail_Recipient> CCList = new List<Actions.SendEmail_Recipient>();
			List<Actions.SendEmail_Recipient> BCCList = new List<Actions.SendEmail_Recipient>();
			List<Actions.SendEmail_Recipient> ExclusionList = new List<Actions.SendEmail_Recipient>();
			*/
            string TempSubjectLine = "";
			string SenderAddress = "";
			Int64 TemplateId = 0;
			Int64 StatementSummaryId = 0;
			List<EmailAttachment> CommonAttachments = new List<EmailAttachment>();
			if (string.IsNullOrEmpty(Result))
			{
                Result = BuildCommonAttachments(m_CompanyClientId, m_RunId, ref CommonAttachments, ref StatementSummaryId);
                WriteToOutput("After BuildCommonAttachments");
                /* Moved to the start of the individual emails below
                WriteToOutput("Before copy EmailAttachments.Count = " + SendEmailParam.EmailAttachments.Count);
                if (string.IsNullOrEmpty(Result))
                {
                	Result = SendEmailParam.CopyEmailAttachmentsFrom(CommonAttachments);
                }
                WriteToOutput("After copy EmailAttachments.Count = " + SendEmailParam.EmailAttachments.Count);
                */
			}
			
			if (string.IsNullOrEmpty(Result))
			{
				CommonMethods.WriteToOutputLog(m_RunId, "Before BuildCommonEmailSettings");
				string SubjectLine = "";
				string EmailBody = "";
				Result = BuildCommonEmailSettings(m_RunId, out SubjectLine, out EmailBody, out TemplateId);
				if (string.IsNullOrEmpty(Result))
				{
					SendEmailParam.Subject = SubjectLine;
					SendEmailParam.EmailBody = EmailBody;
				}
				else
				{
					CommonMethods.WriteToOutputLog(m_RunId, Result);
				}
            }

            if (!string.IsNullOrEmpty(Result))
            {
                CommonMethods.WriteToOutputLog(m_RunId, Result);
            }
            else
            {
                CommonMethods.WriteToOutputLog(m_RunId, "After BuildCommonEmailSettings");
            }
            if (m_ConfigSettings != null)
            {
                CommonMethods.WriteToOutputLog(m_RunId, "Config Settings IS OK");
            }
            CommonMethods.WriteToOutputLog(m_RunId, "TemplateId: " + TemplateId);
            CommonMethods.WriteToOutputLog(m_RunId, "RunningOnEmailServer: " + m_ConfigSettings.RunningOnEmailServer);
            if (string.IsNullOrEmpty(Result) && TemplateId > 0 && m_ConfigSettings.RunningOnEmailServer == "Y")
            {
                CommonMethods.WriteToOutputLog(m_RunId, "Before CopyEmailTemplateToEmailServer");
                Result = CommonMethods.CopyEmailTemplateToEmailServer(TemplateId);
            }
            if (string.IsNullOrEmpty(Result))
            {
                Result = CommonMethods.SyncCreateDocsItems(m_RunId, m_CompanyClientId, m_ConfigSettings);
            }
            CommonMethods.WriteToOutputLog(m_RunId, "After CopyEmailTemplateToEmailServer");

            // Gather the records from EAEventRunClients for the runId
            DataTable dt = null;
			if (string.IsNullOrEmpty(Result))
			{
				string QueryText = 
					" SELECT EVAC_ID, I_EVAC_ClientId, B_EVAC_SENTEVENT, EVAC_StatementSummaryId" +
					" FROM EAEventRunClients WHERE I_EVAC_EventRunId = @RunId AND B_EVAC_SentEvent = 'N'";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("RunId", m_RunId));
				WriteParamsToOutput(QueryText, Params);
				
				Msg = "Processing QueryText:" + Environment.NewLine + QueryText + Environment.NewLine + "For RunId: " + m_RunId.ToString();
				WindowsEventLoggingUtilities.WriteToInfoEvent(Msg, "EmailProcessor-ExecuteRun", 0);
				dt = FirebirdUtilities.GetFirstDataTable(m_ConnectionString, QueryText, Params, 0);
				if (dt == null)
				{
					Result = "No data table returned";
					WindowsEventLoggingUtilities.WriteToErrorEvent(Result, Method, 0);
					WriteToOutput(Result);
				}			
			}
			
			StringBuilder EmailAttachmentsList = new StringBuilder();
			StringBuilder EmailRequestList = new StringBuilder();
			List<string> commonAttachments = new List<string>();
			Int64 EVAC_ID = 0;
			Int64 ClientId = 0;
			string SentEvent = "";
			Object Obj = null;
			List<EmailAttachment> Attachments;// = null;
			if (string.IsNullOrEmpty(Result))
			{
				WriteToOutput("Processing: " + dt.Rows.Count.ToString() + " records");
				#region ForEach email
				foreach (DataRow Row in dt.Rows)
				{
					Obj = Row["EVAC_StatementSummaryId"];
					StatementSummaryId = (Obj == DBNull.Value) ? 0 : Convert.ToInt64(Obj);
					Obj = Row["B_EVAC_SENTEVENT"];
					SentEvent = (Obj == DBNull.Value) ? "N" : Convert.ToString(Obj);
					if (StringUtilities.SameText(SentEvent, "N"))
					{
						EmailAttachmentsList.Length = 0; // clear for each entry
						EmailAttachmentsList.Append(JsonUtilities.StartJson());
						EmailAttachmentsList.Append(JsonUtilities.StartJsonArray("TemplateAttachments"));
						Attachments = new List<EmailAttachment>();
						Attachments.Clear();
						SendEmailParam.EmailAttachments.Clear();
						SendEmailParam.Recipients.Clear();
						SendEmailParam.BCCRecipients.Clear();
						SendEmailParam.CCRecipients.Clear();
						SendEmailParam.ExclusionList.Clear();

		                /*
		                 * Copied from above.
		                 * Because we needed to clear the attachments at the start of a new
		                 * email, we need to restore the common attachments before we create the 
		                 * email specific attachments
		                 * */
						//WriteToOutput("Before copy EmailAttachments.Count = " + SendEmailParam.EmailAttachments.Count);
						//WriteToOutput("Common Attachment.Count = " + CommonAttachments.Count);
						//WriteToOutput("Result Value: " + Result);
		                if (string.IsNullOrEmpty(Result))
		                {
		                	WriteToOutput("Before CopyEmailAttachmentsFrom");
		                	Result = SendEmailParam.CopyEmailAttachmentsFrom(CommonAttachments);
		                	WriteToOutput("After CopyEmailAttachmentsFrom");
		                }
		                //WriteToOutput("After copy EmailAttachments.Count = " + SendEmailParam.EmailAttachments.Count);
		
		                EmailAttachment NewAttachment;
						Obj = Row["EVAC_ID"];
						EVAC_ID = (Obj == DBNull.Value) ? 0L : Convert.ToInt64(Obj);
						Obj = Row["I_EVAC_ClientId"];
						ClientId = (Obj == DBNull.Value) ? 0L : Convert.ToInt64(Obj);

						if ((EVAC_ID > 0) && (ClientId > 0))
						{
							WriteToOutput("EVAC_ID: " + EVAC_ID);
                            Result = BuildAllBlobContents(m_CompanyClientId, m_RunId, EVAC_ID, ClientId);
                            WriteToOutput("BuildAllBlobContent Result: " + Result);
							if (string.IsNullOrEmpty(Result))
							{
								Result = 
									BuildAllBlobAttachments(
										m_CompanyClientId, m_RunId, EVAC_ID, ClientId, 
										ref Attachments, ref StatementSummaryId);
								WriteToOutput("BuildAllBlobAttachments Count: " + Attachments.Count);
								if (string.IsNullOrEmpty(Result))
								{
									//SendEmailParam.CopyEmailAttachmentsFrom(Attachments);
									foreach (EmailAttachment Item in Attachments)
									{
										NewAttachment = new EmailAttachment();
										NewAttachment.FileName = Item.FileName;
										SendEmailParam.EmailAttachments.Add(NewAttachment);
									}
									
								}
							}
							//if (string.IsNullOrEmpty(Result)) Result = "Stop Here";
							if (string.IsNullOrEmpty(Result))
							{
                                /* xxx */
                                WriteToOutput("Before AttachmentsList");
                                List<string> AttachmentsList = new List<string>();
                                foreach (EmailAttachment Item in Attachments)
                                {
                                	AttachmentsList.Add(Item.FileName);
                                }
                                EmailAttachmentsList.Append(string.Join(",", AttachmentsList.ToArray()));
								EmailAttachmentsList.Append(JsonUtilities.EndJsonArray());
								EmailAttachmentsList.Append(JsonUtilities.EndJson());
								using (TextWriter tw = new StreamWriter(m_DocsLocation + Convert.ToString(ClientId) + "_Attachments.json"))
								{
									tw.Write(EmailAttachmentsList.ToString());
								}
								WriteToOutput("After AttachmentsList");
								List<NameAndAddressItem> Recipients = new List<NameAndAddressItem>();
								List<NameAndAddressItem> CCRecipients = new List<NameAndAddressItem>();
								List<NameAndAddressItem> BCCRecipients = new List<NameAndAddressItem>();
								List<NameAndAddressItem> ExclusionList = new List<NameAndAddressItem>();
								SenderDetailsItem Sender = new SenderDetailsItem();
                                //WriteToOutput("Cleared Recipients");
                                WriteToOutput("Before BuildEmailAddressItems");
								Result = 
									BuildEmailAddressItems(
										m_RunId, m_CompanyClientId, ClientId, StatementSummaryId, 
										ref Sender, ref Recipients, ref CCRecipients, 
										ref BCCRecipients, ref ExclusionList);
                                WriteToOutput("After BuildEmailAddressItems");
                                WriteToOutput("Result: " + Result);
								WriteToOutput("Sender.ReplyToAddress: " + Sender.ReplyToAddress);
								if (string.IsNullOrEmpty(Result))
								{
									Result = SendEmailParam.CopySenderFrom(Sender);
									WriteToOutput("SendEmailParam.Sender.Address: " + SendEmailParam.Sender.Address);
									WriteToOutput("SendEmailParam.Sender.ReplyTo: " + SendEmailParam.Sender.ReplyToAddress);
									
									if (string.IsNullOrEmpty(Result))
									{
										WriteToOutput("Before copying Recipients");
										WriteToOutput("Existing Recipients Count: " + SendEmailParam.Recipients.Count);
										WriteToOutput("New Recipients Count: " + Recipients.Count);
										Result = SendEmailParam.CopyRecipientsFrom(Recipients);
										WriteToOutput("After copying Recipients");
										WriteToOutput("Existing Recipients Count: " + SendEmailParam.Recipients.Count);
									}
									
									if (string.IsNullOrEmpty(Result))
									{
										Result = SendEmailParam.CopyCCRecipientsFrom(CCRecipients);
									}
									
									if (string.IsNullOrEmpty(Result))
									{
										WriteToOutput("Before copying BCC entries");
										WriteToOutput("Count: " + SendEmailParam.BCCRecipients.Count);
										Result = SendEmailParam.CopyBCCRecipientsFrom(BCCRecipients);
										WriteToOutput("After copying BCC entries");
										WriteToOutput("Count: " + SendEmailParam.BCCRecipients.Count);
									}
									
									if (string.IsNullOrEmpty(Result))
									{
										Result = SendEmailParam.CopyExclusionsFrom(ExclusionList);
									}
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
								Result =
									UpdateClientEmailAddressItems(m_CompanyClientId, EVAC_ID, SendEmailParam.Recipients);
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


                            FileName = m_DocsLocation + Convert.ToString(m_RunId) + "_" + Convert.ToString(ClientId) + "_UnadulteratedEmailBody.json";
                            using (TextWriter tw = new StreamWriter(FileName))
                            {
                                tw.Write(SendEmailParam.EmailBody);
                            }

                            List<string> MergeFields = CommonMethods.ParseEmailBody(SendEmailParam.EmailBody);
                            WriteToOutput("Merge Fields");
                            foreach (string str in MergeFields)
                            {
                                WriteToOutput(str);
                            }

                            //EmailBody = "xxxx";
                            //if ((string.IsNullOrEmpty(Result)))// && (TemplateId > 0))
                            if ((string.IsNullOrEmpty(Result)) && (TemplateId > 0))
							{
                            	string EmailBody = "";
                                TempSubjectLine = "";
								WriteToOutput("TemplateId: " + TemplateId);
								Result = 
                                    BuildClientEmailBody(
                                        m_CompanyClientId, m_RunId, EVAC_ID, ClientId, TemplateId, 
                                        out EmailBody, out TempSubjectLine);
								WriteToOutput("Result: " + Result);
								if (string.IsNullOrEmpty(Result))
								{
									SendEmailParam.EmailBody = EmailBody;
								}
                                FileName = m_DocsLocation + Convert.ToString(m_RunId) + "_" + Convert.ToString(ClientId) + "_EmailBody.txt";
                                using (TextWriter tw = new StreamWriter(FileName))
								{
									tw.Write(EmailBody);
								}
								WriteToOutput("EmailBody in File: " + FileName);
                                //WriteToOutput(EmailBody);
                                WriteToOutput("Global SubjectLine: " + SendEmailParam.Subject + " Attachment Subject Line: " + TempSubjectLine);
                                if (!string.IsNullOrEmpty(TempSubjectLine)) SendEmailParam.Subject = TempSubjectLine;
							}

                            if (string.IsNullOrEmpty(Result))
                            {
                                BuildOriginalJsonRequest(
                                        m_RunId, SendEmailParam.AccountDetails, SendEmailParam.Sender, 
                                        SendEmailParam.Subject, SendEmailParam.EmailBody,
                                        SendEmailParam.Recipients, SendEmailParam.CCRecipients, 
                                        SendEmailParam.BCCRecipients, SendEmailParam.EmailAttachments,
                                        StatementSummaryId, TemplateId, ClientId);
                            }

                            StringBuilder EmailRequest = new StringBuilder();
                            if (string.IsNullOrEmpty(Result))
							{
								EmailRequest.Length = 0;
								Result = 
									BuildJsonRequest(
										m_RunId, SendEmailParam.AccountDetails, SendEmailParam.Sender, 
										SendEmailParam.Subject, SendEmailParam.EmailBody,
										SendEmailParam.Recipients, SendEmailParam.CCRecipients, 
										SendEmailParam.BCCRecipients, SendEmailParam.EmailAttachments,
										StatementSummaryId, TemplateId, ref EmailRequest);
								if (string.IsNullOrEmpty(Result))
								{
                                    FileName = 
                                    	m_DocsLocation + Convert.ToString(m_RunId) + "_" + 
                                    	Convert.ToString(ClientId) + "_Request.json";
                                    using (TextWriter tw = new StreamWriter(FileName))
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
									string StrEmailBody = SendEmailParam.EmailBody;
									List<EmailBodyImage> bodyImages = new List<EmailBodyImage>();
									
									if (string.IsNullOrEmpty(Result) && (!string.IsNullOrEmpty(SendEmailParam.EmailBody)))
									{
										//Result = CommonMethods.ProcessEmailBody(ref SendEmailParam.EmailBody, ref SendEmailParam.EmailBodyImages);
										Result = 
											CommonMethods.ProcessEmailBody(ref StrEmailBody, ref bodyImages);
										Console.WriteLine("After ProcessEmailBody Result:");
										Console.WriteLine(Result);
										if (string.IsNullOrEmpty(Result))
										{
											SendEmailParam.EmailBodyImages = bodyImages;
											SendEmailParam.EmailBody = StrEmailBody;
										}
									}			
						
									WriteToOutput("Body After Processing");
									WriteToOutput(SendEmailParam.EmailBody);
								}
								
								if (string.IsNullOrEmpty(Result))
								{
									string HtmlFileName = m_DocsLocation + "0Test.html";
									//Result = Actions.SendEmail.Execute(DataObject, EmailBody, out ErrorMsg);
									//Result = Actions.SendRTFEmail.Execute(DataObject, m_RTFFileName, out ErrorMsg);
									
									/************************************************************************
									Result = 
										Actions.SendRTFEmail.Execute(
											DataObject, FCommonItem, FSettingsItem, m_MasterId, m_RTFFileName);
									*************************************************************************/
									//SendEmailParam.Display();									
									string Str = SendEmailParam.Display();
									WriteToOutput(Str);
									
									WriteToOutput("Attachment Count Before Email Send: " + SendEmailParam.EmailAttachments.Count);
									
									/* Send the email */
									SendEmail EmailSender = new SendEmail();
									if (m_ConfigSettings.SendEmail == "Y")
                                    {
                                        Result = EmailSender.Send(SendEmailParam, m_RunId);
                                    }
				
									/*
									WriteToOutput("Recipients");
									foreach (NameAndAddressItem Item in SendEmailParam.Recipients)
									{
										WriteToOutput(Item.Name);
										WriteToOutput(Item.Address);
									}
									*/
									
									//Result = ""; ErrorMsg = "";
									//if (string.IsNullOrEmpty(ErrorMsg) && (string.IsNullOrEmpty(Result)))
									WriteToOutput("After SendEmail: Result: " + Result);
									//WriteToOutput("After SendRTFEmail: ErrorMsg: " + ErrorMsg);
									//if (string.IsNullOrEmpty(ErrorMsg)) 
									if (string.IsNullOrEmpty(Result))
                                    {
                                      if (m_ConfigSettings.UpdateSentValues == "Y")
                                        UpdateSentValues(
											EVAC_ID, SendEmailParam.Sender, SendEmailParam.Recipients, 
											SendEmailParam.CCRecipients, SendEmailParam.BCCRecipients, 
											SendEmailParam.ExclusionList);
                                    }
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
				#endregion
			}
			//sbOutput.Append(JsonUtilities.EndJsonArray());
			//sbOutput.Append(JsonUtilities.EndJson());			
			if (string.IsNullOrEmpty(Result))
			{
				//Result = EmailAttachments.ToString();
				StringBuilder sb = new StringBuilder();
				foreach(EmailAttachment Item in SendEmailParam.EmailAttachments)
				{
					sb.AppendLine(Item.FileName);
				}
				Result = sb.ToString();
			}

			return Result;
		}
		
		private string PopulateSettingsEntries()
		{
			string Result = "";
			
			string FileName = m_WorkingDir + "Settings.json";
			
			JObject JsonSource = null;
			string Path = "";
			
			if (string.IsNullOrEmpty(Result))
			{
				try 
				{
					string FileContents = File.ReadAllText(FileName);
					JsonSource = JObject.Parse(FileContents);
					Console.WriteLine("File Read OK");					
				}
				catch (Exception Ex)
				{
					Result = Ex.Message;
				}
			}
			
			if (string.IsNullOrEmpty(Result)) 
			{
				Path = "$.DocsLocation";
				//m_DocsLocation = CommonMethods.GetJsonPathValueAsString(JsonSource, Path, "Docs Location", out Result);
			}
			
			if (string.IsNullOrEmpty(Result)) 
			{
				Path = "$.WordLocation";
				//m_EAOWordLocation = CommonMethods.GetJsonPathValueAsString(JsonSource, Path, "Word Location", out Result);
			}
			
			if (string.IsNullOrEmpty(Result)) 
			{
				Path = "$.TestRPOLocation";
				//m_TestRPOLocation = CommonMethods.GetJsonPathValueAsString(JsonSource, Path, "Test RPO Location", out Result);
			}
			
			if (string.IsNullOrEmpty(Result)) 
			{
				Path = "$.RetryCount";
				m_RetryCount = CommonMethods.GetJsonPathValueAsInt32(JsonSource, Path, "Retry Count", out Result);
			}

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
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, m_ModuleName, 0);
				//OutputParamsInfo(QueryText, Params);
				
				DataTable dt = FirebirdUtilities.GetFirstDataTable(m_ConnectionString, QueryText, Params, 0);
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
					m_DocsLocation = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					if (!string.IsNullOrEmpty(m_DocsLocation) && (!m_DocsLocation.EndsWith(Sep))) m_DocsLocation += Sep;
					Obj = dr["CO_TestRPOLocation"];
					m_TestRPOLocation = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					if (!string.IsNullOrEmpty(m_TestRPOLocation) && (!m_TestRPOLocation.EndsWith(Sep))) m_TestRPOLocation += Sep;
					Obj = dr["CO_EAWordLocation"];
					m_EAOWordLocation = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					if (!string.IsNullOrEmpty(m_EAOWordLocation) && (!m_EAOWordLocation.EndsWith(Sep))) m_EAOWordLocation += Sep;
				}
			} 
			catch (Exception Ex)
			{
				Result = Ex.Message + Environment.NewLine + Ex.StackTrace;
			}
			return Result;
		}
		
		public void WriteToOutput(string LineText)
		{
			string FileName = "EmailProcessor_" + m_RunId.ToString() + ".txt";
			FileName = m_WorkingDir + FileName;			
			using (StreamWriter writer = new StreamWriter(FileName, true))
			{
				writer.WriteLine(LineText);
				Console.WriteLine(LineText);
			}
		}
		
		private string PopulateEmailSettings(Int64 CompanyClientId,
		                                     ref AccountDetails AccountDetails)
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
				" SELECT CO_EMAILHOST, I_CO_EMAILPORT, CO_EMAILUSERNAME, CO_EMAILPASSWORD, BYPASSAUTHCHECK" +
				" FROM EACompanySettings" +
				" WHERE I_SETTINGS_COMPANYID = @CompanyClientId";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("CompanyClientId", CompanyClientId));
            UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, "PopulateEmailSettings", 0);
            WriteParamsToOutput(QueryText, Params);

            DataTable dt = FirebirdUtilities.GetFirstDataTable(m_ConnectionString, QueryText, Params, 0);
			if ((dt != null) && (dt.Rows.Count > 0))
			{
				Object Obj = null;
				foreach (DataRow Row in dt.Rows)
				{
					Obj = Row["CO_EMAILHOST"];
					AccountDetails.Host = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					Obj = Row["I_CO_EMAILPORT"];
					AccountDetails.Port = (Obj == DBNull.Value) ? 0 : Convert.ToInt32(Obj);
					Obj = Row["CO_EMAILUSERNAME"];
					AccountDetails.UserName = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					Obj = Row["CO_EMAILPASSWORD"];
					AccountDetails.Password = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					Obj = Row["BYPASSAUTHCHECK"];
					AccountDetails.BypassAuthCheck = (Obj != DBNull.Value) && Convert.ToBoolean(Obj);
				}
			}
			else
			{
				Result = "No record returned for the CompanyClientId: " + Convert.ToString(CompanyClientId);
			}
			return Result;
		}

		private string BuildCommonAttachments(Int64 CompanyClientId,
		                                      Int64 RunId,
		                                      ref List<EmailAttachment> CommonAttachments,
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
			DataTable dt = FirebirdUtilities.GetFirstDataTable(m_ConnectionString, QueryText, Params, 0);
			if (dt == null)
			{
				Result = "No data table returned in " + Method;
				WindowsEventLoggingUtilities.WriteToInfoEvent(Result, Method, 0);
				Result = "";
			}
			else if (dt.Rows.Count > 0)
			{
				WriteToOutput("DocsLocation:" + m_DocsLocation);
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
			        	WorkingFileName = m_DocsLocation + WorkingFileName;
						Str =
							JsonUtilities.StartJsonObject("") +
							JsonUtilities.AddPropertyNameAndValue("fileName", WorkingFileName, false) +
							JsonUtilities.EndJsonObject();
						/****************************************************************************
						CommonAttachments.Add(Str);
						****************************************************************************/
						EmailAttachment newAttachment = new EmailAttachment();
						newAttachment.FileName = WorkingFileName;
						CommonAttachments.Add(newAttachment);
						WriteToOutput("Added attachment: " + WorkingFileName);
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
				Result = FirebirdUtilities.DBExecute(m_ConnectionString, QueryText, Params, 0);
			
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
						true, m_MasterId, 
						GeneralResponseStatus.LoggingEntry, 
						"", Msg, "", "", Method, 
						JsonContentEntryType.Logging_Entry, 
						DataAsJson,
						/*********************************************************
						FCommonItem.CompanyIdValue,
						FCommonItem.ClientIdValue,
						*********************************************************/
						(int)m_CompanyClientId,
						(int)m_CompanyClientId,
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
		
		private string BuildAddressList(List<NameAndAddressItem> Source)
		{
			string Result = "";
			List<string> Addresses = new List<string>();
			foreach(NameAndAddressItem Item in Source)
			{
				Addresses.Add(Item.Address);
			}
			Result = string.Join(";", Addresses.ToArray());
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

/*
		private string BuildCommonAttachments(Int64 CompanyClientId,
		                                      Int64 RunId,
		                                      ref List<string> CommonAttachments,
		                                      ref Int64 StatementSummaryId)
		{
			string Result = "";
			string Method = "BuildCommonAttachments";
			WriteToOutput("Inside: " + Method);
			
			// Gather the records from EAEmailAttachments for the runId
//			string QueryText = 
//				" SELECT ea_AttachmentName, ea_FileExtension, ea_Base64" +
//				" FROM eaEmailAttachments" +
//				" WHERE ea_EventRunId = @RunId AND ea_DocumentId = 0";
			
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
			DataTable dt = FirebirdUtilities.GetFirstDataTable(m_ConnectionString, QueryText, Params, 0);
			if (dt == null)
			{
				Result = "No data table returned in " + Method;
				WindowsEventLoggingUtilities.WriteToInfoEvent(Result, Method, 0);
				Result = "";
			}
			else if (dt.Rows.Count > 0)
			{
				WriteToOutput("DocsLocation:" + m_DocsLocation);
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
			        	WorkingFileName = m_DocsLocation + WorkingFileName;
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
		*/
		
		private string	BuildCommonEmailSettings(Int64 RunId, 
		                                         out string SubjectLine, 
		                                         out string EmailBody,
		                                         out Int64 TemplateId)
		{
			string Result = "";
			SubjectLine = "";
			EmailBody = "";
			TemplateId = 0;
			int runId = Convert.ToInt32(RunId);
			CommonMethods.WriteToOutputLog(runId, "Inside BuildCommonEmailSettings");
			
			string QueryText =
				" SELECT EVA_SubjectLine, EVA_EmailBody, I_EVA_EmailTemplateId" + 
				" FROM EAEventRun WHERE EVA_ID = @RunId";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("RunId", RunId));
			WriteParamsToOutput(QueryText, Params);
			
			Object Obj = null;
			DataTable tbl = FirebirdUtilities.GetFirstDataTable(m_ConnectionString, QueryText, Params, 0);
			// There should only be one row returned but...
			if ((tbl != null) && (tbl.Rows.Count > 0))
			{
				foreach (DataRow Row in tbl.Rows)
				{
					Obj = Row["EVA_SubjectLine"];
					SubjectLine = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					Obj = Row["I_EVA_EmailTemplateId"];
					TemplateId = (Obj == DBNull.Value) ? 0 : Convert.ToInt64(Obj);
                    byte[] toBytes = new byte[0];
                    Obj = Row["EVA_EmailBody"];
                    if (Obj != DBNull.Value) toBytes = (byte[])Obj;
                    EmailBody = System.Text.Encoding.Default.GetString(toBytes);
                }
            }
			
			return Result;
		}		
		
		private string BuildAllBlobContents(Int64 CompanyClientId,
		                                    Int64 RunId,
		                                    Int64 EVAC_ID,
		                                    Int64 EventRunClientId)
		{
			string Result = "";
			
			string Method = "BuildAllBlobContents";
			string Source = this.m_ModuleName + " - " + Method + " for EventRunClientId: " + EventRunClientId.ToString();
			WriteToOutput(Source);
			
  		    string AppName = "";
  		    string ArgsBase = "";
  		    string[] delimiters = new string[]{"/", " "};
  		    List<string> ArgsList = new List<string>();

			//string ErrorMsg = "";
			string EventId = "";
			
			//string CmdBase = m_TestRPOLocation + "{0}";
			//string CmdBase = m_EAOWordLocation + "{0}";
			string CmdBase = "";
			
			WriteToOutput("RunId: " + Convert.ToString(RunId));
			WriteToOutput("EventRunClientId: " + Convert.ToString(EventRunClientId));			
			
			string AppDir = System.AppDomain.CurrentDomain.BaseDirectory;
			//string WorkingDir = m_EAOWordLocation;
			string WorkingDir = "";

            if (m_ConfigSettings.RunningOnEmailServer == "Y")
            {
                CommonMethods.CopySharedFiles(m_ConnectionString, CompanyClientId, RunId, EVAC_ID, EventRunClientId, m_ConfigSettings);
            }
			List<string> CommandLines =
				GetRunCommandLines(m_ConnectionString, CompanyClientId, RunId, EVAC_ID, EventRunClientId);
				//GetWordDocumentCommandLines(m_ConnectionString, RunId, EventRunClientId);
			if (CommandLines.Count > 0) WriteToOutput("CommandLines.Count: " + CommandLines.Count);
			//int tries = 180;
			foreach (string Item in CommandLines)
			{
				WriteToOutput("Command Line Item:" + Item);
				AppName = Item.Substring(0, Item.IndexOf(" /"));
				if (CommonUtilities.StringUtilities.SameText(AppName, "EAOWord"))
				{
					CmdBase = m_EAOWordLocation;
					WorkingDir = m_EAOWordLocation;
				}
				else
				{				
					CmdBase = m_TestRPOLocation;
					WorkingDir = m_TestRPOLocation;
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
						//WriteToOutput("Retry Count: " + m_RetryCount);
							
						Process runApp = new Process();
						runApp.StartInfo.FileName = AppName;
						runApp.StartInfo.Arguments = ArgsBase;
						runApp.Start();
						for (int Index = 0; Index < m_RetryCount; Index++)
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
		
		private string BuildAllBlobAttachments(Int64 CompanyClientId,
		                                       Int64 RunId,
		                                       Int64 EVAC_ID, 
		                                       Int64 ClientId, 
		                                       ref List<EmailAttachment> Attachments, 
		                                       ref Int64 StatementSummaryId)
		{
			string Result = "";
			
			string Method = "BuildAllBlobAttachments";
			string Source = this.m_ModuleName + " - " + Method;
			WriteToOutput(Source);
			
			WriteToOutput("RunId: " + Convert.ToString(RunId));
			
			string AppDir = System.AppDomain.CurrentDomain.BaseDirectory;
			string WorkingDir = m_EAOWordLocation;
			
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
                string[] FileNameParts = null;
                WriteToOutput("Before GetFirstDataTable");
                //DataTable dt = FirebirdUtilities.GetFirstDataTable(m_ConnectionString, QueryText, Params, 0);
                DataTable dt =
                    UsefulDBMethods.Helpers.GetFirstDataTableEx(
                        m_ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out ErrorMsg);
                WriteToOutput("After GetFirstDataTable");
                WriteToOutput("ErrorMsg: " + ErrorMsg);
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
                        	WriteToOutput("Inside foreach");
                            object obj = Row["FileName"];
                            string FileName = (obj == DBNull.Value) ? "" : Convert.ToString(obj);
                            //WriteToOutput("Full Filename String: " + FileName);
                            FileNameParts = FileName.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                            //WriteToOutput("FileNameParts.Length: " + FileNameParts.Length.ToString());
                            FileName = FileNameParts[FileNameParts.Length - 1];
                            //WriteToOutput("After FileNameParts");
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
                            WriteToOutput("FileNameOnly: " + FileNameOnly);
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

                            string OutputFileName = m_DocsLocation + FileNameOnly;
                            WriteToOutput("OutputFileName: " + OutputFileName);
                            //WindowsEventLoggingUtilities.WriteToInfoEvent("OutputFileName" + Environment.NewLine + OutputFileName, m_ModuleName, MasterId);
                            obj = Row["BlobFileName"];
                            byte[] toBytes = (byte[])obj;
                            string Base64String = System.Text.Encoding.Default.GetString(toBytes);
                            File.WriteAllBytes(OutputFileName, Convert.FromBase64String(Base64String));
                            /*
                            string Str =
                                JsonUtilities.StartJsonObject("") +
                                JsonUtilities.AddPropertyNameAndValue("fileName", OutputFileName, false) +
                                JsonUtilities.EndJsonObject();
                            */
                            /* Test this for the word attachments */ 
                            string Str = OutputFileName;
                            WriteToOutput("OutputJson" + Environment.NewLine + Str);
                            //Attachments.Add(Str);
                            EmailAttachment NewAttachment = new EmailAttachment();
                            NewAttachment.FileName = Str;
                            Attachments.Add(NewAttachment);
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
		
		private string BuildEmailAddressItems(Int64 RunId, 
		                                      Int64 CompanyClientId, 
		                                      Int64 ClientId,
		                                      Int64 StatementSummaryId,
                                              ref SenderDetailsItem Sender,                                      
		                                      ref List<NameAndAddressItem> Recipients, 
		                                      ref List<NameAndAddressItem> CCList, 
		                                      ref List<NameAndAddressItem> BCCList, 
		                                      ref List<NameAndAddressItem> ExclusionList)
		{
			string Result = "";
			
			string QueryText =
				" SELECT Client, FromAddress, ReplyToAddress, EMAILADDRESSES, CCADDRESS, BCCADDRESS, EXCLUSIONTO" +
				" FROM EventRunShowClients(@RunId, @CompanyClientId, @ClientId, @StatementSummaryId)";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("RunId", RunId));
			Params.Add(new FbParameterItem("CompanyClientId", CompanyClientId));
			Params.Add(new FbParameterItem("ClientId", ClientId));
			Params.Add(new FbParameterItem("StatementSummaryId", StatementSummaryId));
            UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, "BuildEmailAddressItems", (int)RunId);
            WriteParamsToOutput(QueryText, Params);
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
			NameAndAddressItem EmailItem = null;
			DataTable tbl = FirebirdUtilities.GetFirstDataTable(m_ConnectionString, QueryText, Params, 0);
			foreach (DataRow Row in tbl.Rows)
			{
				/*
				Obj = Row["Client"];
				Sender.Name = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
				*/

				Obj = Row["FromAddress"];
				Sender.Address = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
				Sender.Name = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
				
				WriteToOutput("From Address: " + Sender.Address);
				Console.WriteLine("Sender.Name: " + Sender.Name);
				Console.WriteLine("Sender.Address: " + Sender.Address);
                Obj = Row["ReplyToAddress"];
                ColumnValue = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                WriteToOutput("Column Value: " + ColumnValue);
                Sender.ReplyToAddress = ColumnValue;
                if (string.IsNullOrEmpty(Sender.ReplyToAddress))
                {
                    Sender.ReplyToAddress = Sender.Address;
                }
                WriteToOutput("ReplyToAddress: " + Sender.ReplyToAddress);

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
					EmailItem = new Actions.NameAndAddressItem();
					EmailItem.Address = Item;
					WriteToOutput("Adding Recipient Email Address - " + Item);
					/*
					if (CommonUtilities.StringUtilities.SameText(Item, "rdenardo01@gmail.com"))
					{
						EmailItem.Address = "johnebennett_au@yahoo.com.au";
					}
					*/
					
					Recipients.Add(EmailItem);
				}
				
				Obj = Row["CCAddress"];
				ColumnValue = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
				foreach (string Item in ColumnValue.Split(new string[]{";"}, StringSplitOptions.RemoveEmptyEntries))
				{
					EmailItem = new NameAndAddressItem();
					EmailItem.Address = Item;
					WriteToOutput("Adding CC Email Address - " + Item);
					/*
					if (CommonUtilities.StringUtilities.SameText(Item, "ddenardo01@gmail.com"))
					{
						EmailItem.Address = "bennettcentral@gmail.com";
					}
					*/
					CCList.Add(EmailItem);
				}
				
				Obj = Row["BCCAddress"];
				ColumnValue = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
				
				if (string.IsNullOrEmpty(ColumnValue))
					WriteToOutput("No BCC entries to process");
				else
					WriteToOutput("BCC - " + ColumnValue);
				
				foreach (string Item in ColumnValue.Split(new string[]{";"}, StringSplitOptions.RemoveEmptyEntries))
				{
					EmailItem = new NameAndAddressItem();
					EmailItem.Address = Item;
					WriteToOutput("Adding BCC Email Address - " + Item);
					BCCList.Add(EmailItem);
				}
			}
			
			return Result;
		}
		
		private string UpdateClientEmailAddressItems(Int64 CompanyClientId, 
		                                             Int64 EVAC_ID, 
		                                             List<NameAndAddressItem> Recipients)
		{
			string Result = "";
			string Method = "UpdateClientEmailAddressItems";
			List<string> Entries = new List<string>();
			foreach (NameAndAddressItem Recipient in Recipients)
			{
				Entries.Add(Recipient.Address);
			}
			string ToEmail = string.Join(";", Entries.ToArray());
			
			string QueryText =
				" UPDATE EAEventRunClients" +
				"    SET EVAC_ToEmail = @ToEmail" +
				" WHERE EVAC_ID = @EVAC_ID";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("EVAC_ID", EVAC_ID));
				Params.Add(new FbParameterItem("ToEmail", ToEmail));
            UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
            WriteParamsToOutput(QueryText, Params);
            Result = FirebirdUtilities.DBExecute(m_ConnectionString, QueryText, Params, 0);
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
							string.Join(",", Entries.ToArray()) +
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
		
		private string BuildClientEmailBody(Int64 CompanyClientId,
		                                    Int64 RunId,
		                                    Int64 EVAC_ID,
		                                    Int64 ClientId,
		                                    Int64 TemplateId,
		                                    out string EmailBody,
                                            out string SubjectLine)
		{
			string Result = "";
			EmailBody = "";
            SubjectLine = "";
			m_RTFFileName = m_DocsLocation + RunId.ToString() + "_" + ClientId.ToString() + ".rtf";
			
			string Method = "BuildClientEmailBody";
			string Source = this.m_ModuleName + " - " + Method;
			WriteToOutput(Source);
			
			string ErrorMsg = "";
			string QueryText = "";
			DataTable dt = null;
			
			if (TemplateId > 0)
			{
				QueryText =
					" SELECT FIRST 1 BlobFileName AS EmailBody, SubjectLine FROM EAEventClientAttachment(@CompanyClientId, @RunId, @EVAC_ID)" +
					" WHERE ETemplateId = @TemplateId";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyClientId", CompanyClientId));
				Params.Add(new FbParameterItem("RunId", RunId));
				Params.Add(new FbParameterItem("EVAC_ID", EVAC_ID));
				Params.Add(new FbParameterItem("TemplateId", TemplateId));
                WriteParamsToOutput(QueryText, Params);
                //dt = FirebirdUtilities.GetFirstDataTable(m_ConnectionString, QueryText, Params, 0);
                dt = 
                    UsefulDBMethods.Helpers.GetFirstDataTableEx(
                        m_ConnectionString, QueryText, Params, 0, 
                        UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out ErrorMsg);
            }
            else
			{
				QueryText = "SELECT FIRST 1 EVA_EmailBody AS EmailBody, '' AS SubjectLine FROM EAEventRun WHERE EVA_ID = @RunId";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("RunId", RunId));
				dt = FirebirdUtilities.GetFirstDataTable(m_ConnectionString, QueryText, Params, 0);
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
			        	File.WriteAllBytes(m_RTFFileName, toBytes);
			        	EmailBody = System.Text.Encoding.Default.GetString(toBytes);
                        Obj = Row["SubjectLine"];
                        SubjectLine = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					}
				}
			}
			
			return Result;
		}

        private void BuildOriginalJsonRequest(Int64 RunId,
                                              AccountDetails EmailSettings,
                                              SenderDetailsItem Sender,
                                              string SubjectLine,
                                              string EmailBody,
                                              List<NameAndAddressItem> Recipients,
                                              List<NameAndAddressItem> CCList,
                                              List<NameAndAddressItem> BCCList,
                                              List<EmailAttachment> EmailAttachments,
                                              Int64 StatementSummaryId,
                                              Int64 TemplateId,
                                              Int64 ClientId)
        {
            string Result = "";
            string Str = "";
            List<string> StrList = new List<string>();
            StringBuilder Request = new StringBuilder();

            Request.Append(JsonUtilities.StartJson());
            Request.Append(JsonUtilities.StartJsonObject("data"));

            // The RunId
            Request.Append(JsonUtilities.AddPropertyNameAndValue("runId", RunId));
            Request.Append(JsonUtilities.AddPropertyNameAndValue("docsLocation", m_DocsLocation));
            Request.Append(JsonUtilities.AddPropertyNameAndValue("testRPOLocation", m_TestRPOLocation));
            Request.Append(JsonUtilities.AddPropertyNameAndValue("eaWordLocation", m_EAOWordLocation));

            // The common entries
            Request.Append(JsonUtilities.StartJsonObject("common"));
            Request.Append(JsonUtilities.AddPropertyNameAndValue("host", EmailSettings.Host));
            Request.Append(JsonUtilities.AddPropertyNameAndValue("port", EmailSettings.Port));
            Request.Append(JsonUtilities.AddPropertyNameAndValue("emailUserName", EmailSettings.UserName));
            Request.Append(JsonUtilities.AddPropertyNameAndValue("emailPassword", EmailSettings.Password));
            //Request.Append(JsonUtilities.AddPropertyNameAndValue("timeout", EmailSettings.Timeout));
            Request.Append(JsonUtilities.AddPropertyNameAndValue("timeout", "9999999"));
            Request.Append(JsonUtilities.AddPropertyNameAndValue("CompanyActiveStatus", m_CompanyActiveStatus, false));
            Request.Append(JsonUtilities.EndJsonObject());
            Request.Append(",");

            // The SenderAddress
            Request.Append(JsonUtilities.AddPropertyNameAndValue("senderAddress", Sender.Address));
            Request.Append(JsonUtilities.AddPropertyNameAndValue("ReplyToAddress", Sender.ReplyToAddress));

            // The Subject
            Request.Append(JsonUtilities.AddPropertyNameAndValue("emailSubject", SubjectLine));

            Request.Append(JsonUtilities.AddPropertyNameAndValue("templateId", TemplateId));
            Request.Append(JsonUtilities.AddPropertyNameAndValue("emailBody", EmailBody));

            // The Recipients
            Request.Append(JsonUtilities.StartJsonArray("recipients"));
            StrList.Clear();
            foreach (NameAndAddressItem Item in Recipients)
            {
                Str =
                    JsonUtilities.StartJsonObject("") +
                    JsonUtilities.AddPropertyNameAndValue("address", Item.Address, false) +
                    JsonUtilities.EndJsonObject();
                StrList.Add(Str);
            }
            Request.Append(string.Join(",", StrList.ToArray()));
            Request.Append(JsonUtilities.EndJsonArray());
            Request.Append(",");

            // The CCList
            Request.Append(JsonUtilities.StartJsonArray("cc"));
            StrList.Clear();
            foreach (NameAndAddressItem Item in CCList)
            {
                Str =
                    JsonUtilities.StartJsonObject("") +
                    JsonUtilities.AddPropertyNameAndValue("address", Item.Address, false) +
                    JsonUtilities.EndJsonObject();
                StrList.Add(Str);
            }
            Request.Append(string.Join(",", StrList.ToArray()));
            Request.Append(JsonUtilities.EndJsonArray());
            Request.Append(",");

            // The BCCList
            Request.Append(JsonUtilities.StartJsonArray("bcc"));
            StrList.Clear();
            foreach (NameAndAddressItem Item in BCCList)
            {
                Str =
                    JsonUtilities.StartJsonObject("") +
                    JsonUtilities.AddPropertyNameAndValue("address", Item.Address, false) +
                    JsonUtilities.EndJsonObject();
                StrList.Add(Str);
            }
            Request.Append(string.Join(",", StrList.ToArray()));
            Request.Append(JsonUtilities.EndJsonArray());
            Request.Append(",");

            // The Attachments
            Request.Append(JsonUtilities.StartJsonArray("attachments"));
            StrList.Clear();
            foreach (EmailAttachment Item in EmailAttachments)
            {
            	Str =
                    JsonUtilities.StartJsonObject("") +
                    JsonUtilities.AddPropertyNameAndValue("filename", Item.FileName, false) +
                    JsonUtilities.EndJsonObject();
                StrList.Add(Str);
            }
            Request.Append(string.Join(",", StrList.ToArray()));
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

            string FileName = m_DocsLocation + Convert.ToString(RunId) + "_" + Convert.ToString(ClientId) + "_UnadulteratedRequest.json";
            using (TextWriter tw = new StreamWriter(FileName))
            {
                tw.Write(Request.ToString());
            }
        }

        private string BuildJsonRequest(Int64 RunId, 
		                                AccountDetails EmailSettings, 
		                                SenderDetailsItem Sender, 
		                                string SubjectLine, 
		                                string EmailBody, 
		                                List<NameAndAddressItem> Recipients, 
		                                List<NameAndAddressItem> CCList, 
		                                List<NameAndAddressItem> BCCList, 
		                                List<EmailAttachment> EmailAttachments,
		                                Int64 StatementSummaryId,
		                                Int64 TemplateId,
		                                ref StringBuilder Request)
		{
			string Result = "";
			string Str = "";
            string Addr = "";
			List<string> StrList = new List<string>();
			
			Request.Append(JsonUtilities.StartJson());			
			Request.Append(JsonUtilities.StartJsonObject("data"));
			
			// The RunId
			Request.Append(JsonUtilities.AddPropertyNameAndValue("runId", RunId));
			Request.Append(JsonUtilities.AddPropertyNameAndValue("docsLocation", m_DocsLocation));
			Request.Append(JsonUtilities.AddPropertyNameAndValue("testRPOLocation", m_TestRPOLocation));
			Request.Append(JsonUtilities.AddPropertyNameAndValue("eaWordLocation", m_EAOWordLocation));
			
			// The common entries
			Request.Append(JsonUtilities.StartJsonObject("common"));
			Request.Append(JsonUtilities.AddPropertyNameAndValue("host", EmailSettings.Host));
			Request.Append(JsonUtilities.AddPropertyNameAndValue("port", EmailSettings.Port));
			Request.Append(JsonUtilities.AddPropertyNameAndValue("emailUserName", EmailSettings.UserName));
			Request.Append(JsonUtilities.AddPropertyNameAndValue("emailPassword", EmailSettings.Password));
            //Request.Append(JsonUtilities.AddPropertyNameAndValue("timeout", EmailSettings.Timeout));
            Request.Append(JsonUtilities.AddPropertyNameAndValue("timeout", "99999"));
            Request.Append(JsonUtilities.AddPropertyNameAndValue("CompanyActiveStatus", m_CompanyActiveStatus, false));
            Request.Append(JsonUtilities.EndJsonObject());
			Request.Append(",");
			
			// The SenderAddress
			//SenderAddress = "johnebennett_au@yahoo.com.au";

			Request.Append(JsonUtilities.AddPropertyNameAndValue("senderAddress", Sender.Address));
            Request.Append(JsonUtilities.AddPropertyNameAndValue("ReplyToAddress", Sender.ReplyToAddress));
			
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
			//Request.Append(JsonUtilities.AddPropertyNameAndValue("emailBody", ""));
            Request.Append(JsonUtilities.AddPropertyNameAndValue("emailBody", EmailBody));

            // The Recipients
            Request.Append(JsonUtilities.StartJsonArray("recipients"));
			StrList.Clear();
			WriteToOutput("Processing Recipients - " + Recipients.Count);
			WriteToOutput("---------------------");
			foreach (NameAndAddressItem Item in Recipients)
			{
                //if (m_CompanyActiveStatus == "Y") Addr = Item.Address; else Addr = "Rdenardo01@gmail.com";
                Addr = Item.Address;
				WriteToOutput("CC Item - " + Addr);
                Str =
                    JsonUtilities.StartJsonObject("") +
                    //JsonUtilities.AddPropertyNameAndValue("address", Item.Address, false) +
                    //JsonUtilities.AddPropertyNameAndValue("address", "__Rdenardo01@gmail.com", false) +
                    //JsonUtilities.AddPropertyNameAndValue("address", "johnebennett_au@yahoo.com.au", false) +
                    JsonUtilities.AddPropertyNameAndValue("address", Addr, false) +
					JsonUtilities.EndJsonObject();
				StrList.Add(Str);
			}
			Request.Append(string.Join(",", StrList.ToArray()));
			Request.Append(JsonUtilities.EndJsonArray());
			Request.Append(",");
			
			// The CCList
			Request.Append(JsonUtilities.StartJsonArray("cc"));
			StrList.Clear();
			WriteToOutput("Processing CCs - " + CCList.Count);
			WriteToOutput("---------------------");
			foreach (NameAndAddressItem Item in CCList)
			{
				Addr = Item.Address;
				WriteToOutput("CC Item - " + Addr);
                Str =
					JsonUtilities.StartJsonObject("") +
					//JsonUtilities.AddPropertyNameAndValue("address", Item.Address, false) +
					//JsonUtilities.AddPropertyNameAndValue("address", "__johnebennett_au@yahoo.com.au", false) +
                    JsonUtilities.AddPropertyNameAndValue("address", Addr, false) +
                    JsonUtilities.EndJsonObject();
				StrList.Add(Str);
			}
			Request.Append(string.Join(",", StrList.ToArray()));
			Request.Append(JsonUtilities.EndJsonArray());
			Request.Append(",");

			// The BCCList
			Request.Append(JsonUtilities.StartJsonArray("bcc"));
			StrList.Clear();
			WriteToOutput("Processing BCCs - " + BCCList.Count);
			WriteToOutput("---------------------");
			foreach (NameAndAddressItem Item in BCCList)
			{
                Addr = Item.Address; 
				WriteToOutput("BCC Item - " + Addr);
                Str =
					JsonUtilities.StartJsonObject("") +
                    //JsonUtilities.AddPropertyNameAndValue("address", Item.Address, false) +
                    //JsonUtilities.AddPropertyNameAndValue("address", "__johnebennett_au@yahoo.com.au", false) +
                    JsonUtilities.AddPropertyNameAndValue("address", Addr, false) +
                    JsonUtilities.EndJsonObject();
				StrList.Add(Str);
			}
			Request.Append(string.Join(",", StrList.ToArray()));
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
			StrList.Clear();
			WriteToOutput("Processing Attachments - " + EmailAttachments.Count);
			WriteToOutput("---------------------");
            foreach (EmailAttachment Item in EmailAttachments)
            {
            	WriteToOutput("Attachment - " + Item.FileName);
            	Str =
                    JsonUtilities.StartJsonObject("") +
                    JsonUtilities.AddPropertyNameAndValue("filename", Item.FileName, false) +
                    JsonUtilities.EndJsonObject();
                StrList.Add(Str);
            }
            Request.Append(string.Join(",", StrList.ToArray()));
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
		private void UpdateSentValues(Int64 EVAC_ID,
		                              SenderDetailsItem Sender,
		                              List<NameAndAddressItem> Recipients,
		                              List<NameAndAddressItem> CCList,
		                              List<NameAndAddressItem> BCCList,
		                              List<NameAndAddressItem> ExclusionList)
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
				Sender.Address = DelphiSubstring(Sender.Address, 0, 120);
				
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
                    "        EVAC_ReplyToEmail = @ReplyToEmail," +
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
	
					Params.Add(new FbParameterItem("FromEmail", Sender.Address));
                    Params.Add(new FbParameterItem("ReplyToEmail", Sender.ReplyToAddress));
					Params.Add(new FbParameterItem("ToEmail", StrRecipients));
					Params.Add(new FbParameterItem("CCEmail", StrCC));
					Params.Add(new FbParameterItem("BCCEmail", StrBCC));
					Params.Add(new FbParameterItem("Exclusions", StrExclusions));
	
				WriteParamsToOutput(QueryText, Params);
				Result = FirebirdUtilities.DBExecute(m_ConnectionString, QueryText, Params, 0);
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
			WriteParamsToOutput(QueryText, Params);
			
			string ErrorMsg = "";
			Int64 ReturnedClientId = 0;
			DataTable dt = FirebirdUtilities.GetFirstDataTable(m_ConnectionString, QueryText, Params, 0);
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
				ArgsPair = Args.Trim().Split(ArgsDelimiters, StringSplitOptions.RemoveEmptyEntries);//.ToArray();
				if (StringUtilities.SameText(ArgsPair[0], ArgName))
				{
					Result = ArgsPair[1];
				}
			}
			
			//WriteToOutput("For: " + ArgName + " returning: " + Result);
			
			return Result;
		}
	}
}
