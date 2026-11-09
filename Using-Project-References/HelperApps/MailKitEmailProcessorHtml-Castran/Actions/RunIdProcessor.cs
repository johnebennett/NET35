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
		private SendEmailParam SendEmailParam;
		
		private string m_UniqueFileId = "";
		private Int64 m_RunId = 0;

		private EAOfficeAPI.APISettingsItem m_SettingsItem = null;
		private EAOfficeAPI.RequestCommonItem m_CommonItem = new EAOfficeAPI.RequestCommonItem();
		private string m_ConnectionString = "";		
		private string m_ModuleName = "Email Processor";

		private string m_DocsLocation = @"e:\EAOffice\Development\Docs\";
		private string m_EAOWordLocation = @"E:\CreateDocs\";
		private string m_TestRPOLocation = @"E:\CreateDocs\";
        private int m_RetryCount = 300;
        private string ShareDir = "";
        private ConfigSettings m_ConfigSettings;

        private int m_MasterId = 0;
		private string m_WorkingDir = "";
		private string m_RTFFileName = "";
		
		private Int64 m_CompanyClientId = 0;
		private string m_CompanyActiveStatus = "";

		//private AppLogger appLogger;// = new Actions.AppLogger();
		
		public RunIdProcessor()
		{
			SendEmailParam = new SendEmailParam();
			
			m_UniqueFileId = "";
		}

		public string Execute(Int64 RunId)
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
			
			LoggingMethods.WriteToConsole("Connection String:");
			LoggingMethods.WriteToConsole(m_ConnectionString);
			
			m_WorkingDir =
				new Uri(
	            	System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).LocalPath;
            m_WorkingDir = Path.GetDirectoryName(m_WorkingDir);
            string SepChar = Path.DirectorySeparatorChar.ToString();
            if (!m_WorkingDir.EndsWith(SepChar)) m_WorkingDir += SepChar;
			LoggingMethods.WriteToConsole("WorkingDir: " + m_WorkingDir);
			
			if (string.IsNullOrEmpty(Result))
			{
				//Result = PopulateSettingsEntries();
				try 
				{
					m_ConfigSettings = 
						ConfigSettings.LoadFromJson(
							m_WorkingDir + "MailKitEmailProcessorSettings.json");

					m_DocsLocation = m_ConfigSettings.LocalDocsLocation;
					m_EAOWordLocation = m_ConfigSettings.LocalCreateDocsLocation;
					m_TestRPOLocation = m_ConfigSettings.LocalCreateDocsLocation;
					m_RetryCount = m_ConfigSettings.RetryCount;
					ShareDir = m_ConfigSettings.ShareDir;
				}
				catch (Exception Ex)
				{
					Result = "Error loading ConfigSettings: " + Ex.Message;
					LoggingMethods.WriteToOutputLog(m_RunId, Result);
				}
			}
			
			if (string.IsNullOrEmpty(Result))
			{
				Result = ProcessRun();
			}
			else
			{
				LoggingMethods.WriteToConsole(Result);
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
			EAOfficeAPI.ProgressLogging.AddRecord(
				true, 0, m_CommonItem.Plugin, m_CommonItem.Command, "ExecuteRun", //CommonItem.Action, 
				Content, EAOfficeAPI.JsonContentEntryType.Request, (int)m_CompanyClientId, 0, out m_MasterId, out Result);

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
            LoggingMethods.WriteToConsole("CompanyActiveStatus: " + m_CompanyActiveStatus);

            if (string.IsNullOrEmpty(Result))
            {
	            AccountDetails AccountDetails = new AccountDetails();	            
				Result = PopulateEmailSettings(m_CompanyClientId, ref AccountDetails);
				if (string.IsNullOrEmpty(Result))
				{
					Result = SendEmailParam.CopyAccountDetailsFrom(AccountDetails);
				}
            }
            string TempSubjectLine = "";
			Int64 TemplateId = 0;
			Int64 StatementSummaryId = 0;
			List<EmailAttachment> CommonAttachments = new List<EmailAttachment>();
			if (string.IsNullOrEmpty(Result))
			{
                Result = BuildCommonAttachments(m_CompanyClientId, m_RunId, ref CommonAttachments, ref StatementSummaryId);
                LoggingMethods.WriteToConsole("After BuildCommonAttachments");
			}
			
			if (string.IsNullOrEmpty(Result))
			{
				LoggingMethods.WriteToOutputLog(m_RunId, "Before BuildCommonEmailSettings");
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
					LoggingMethods.WriteToOutputLog(m_RunId, Result);
				}
            }
			
			if (!string.IsNullOrEmpty(Result))
			{
				LoggingMethods.WriteToOutputLog(m_RunId, Result);
			}
			else
			{
				LoggingMethods.WriteToOutputLog(m_RunId, "After BuildCommonEmailSettings");
			}
			
			if (m_ConfigSettings != null)
				LoggingMethods.WriteToOutputLog(m_RunId, "Config Settings IS OK");
			LoggingMethods.WriteToOutputLog(m_RunId, "TemplateId: " + TemplateId);
			LoggingMethods.WriteToOutputLog(m_RunId, "RunningOnEmailServer: " + m_ConfigSettings.RunningOnEmailServer);
			
			if (string.IsNullOrEmpty(Result) && (TemplateId > 0) && (m_ConfigSettings.RunningOnEmailServer == "Y"))
			{
				LoggingMethods.WriteToOutputLog(m_RunId, "Before CopyEmailTemplateToEmailServer");
				Result = CommonMethods.CopyEmailTemplateToEmailServer(TemplateId);
			}
			
			if (string.IsNullOrEmpty(Result) && (m_ConfigSettings.RunningOnEmailServer == "Y"))
			{
				Result = CommonMethods.SyncCreateDocsItems(m_RunId, m_CompanyClientId, m_ConfigSettings);
			}

			LoggingMethods.WriteToOutputLog(m_RunId, "After CopyEmailTemplateToEmailServer");
            // Gather the records from EAEventRunClients for the runId
            DataTable dt = null;
			if (string.IsNullOrEmpty(Result))
			{
				string QueryText = 
					" SELECT EVAC_ID, I_EVAC_ClientId, B_EVAC_SENTEVENT, EVAC_StatementSummaryId" +
					" FROM EAEventRunClients WHERE I_EVAC_EventRunId = @RunId AND B_EVAC_SentEvent = 'N'";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("RunId", m_RunId));
				//WriteParamsToOutput(QueryText, Params);
				LoggingMethods.WriteParamsToOutput(m_RunId, QueryText, Params);
				
				Msg = "Processing QueryText:" + Environment.NewLine + QueryText + Environment.NewLine + "For RunId: " + m_RunId.ToString();
				WindowsEventLoggingUtilities.WriteToInfoEvent(Msg, "EmailProcessor-ExecuteRun", 0);
				dt = FirebirdUtilities.GetFirstDataTable(m_ConnectionString, QueryText, Params, 0);
				if (dt == null)
				{
					Result = "No data table returned";
					WindowsEventLoggingUtilities.WriteToErrorEvent(Result, Method, 0);
					LoggingMethods.WriteToConsole(Result);
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
				LoggingMethods.WriteToConsole("Processing: " + dt.Rows.Count.ToString() + " records");
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
						/*
		                if (string.IsNullOrEmpty(Result))
		                {
		                	LoggingMethods.WriteToConsole("Before CopyEmailAttachmentsFrom");
		                	Result = SendEmailParam.CopyEmailAttachmentsFrom(CommonAttachments);
		                	LoggingMethods.WriteToConsole("After CopyEmailAttachmentsFrom");
		                }
						*/
		                //WriteToOutput("After copy EmailAttachments.Count = " + SendEmailParam.EmailAttachments.Count);
		
		                EmailAttachment NewAttachment;
						Obj = Row["EVAC_ID"];
						EVAC_ID = (Obj == DBNull.Value) ? 0L : Convert.ToInt64(Obj);
						Obj = Row["I_EVAC_ClientId"];
						ClientId = (Obj == DBNull.Value) ? 0L : Convert.ToInt64(Obj);

						if ((EVAC_ID > 0) && (ClientId > 0))
						{
							LoggingMethods.WriteToConsole("EVAC_ID: " + EVAC_ID);
                            Result = BuildAllBlobContents(m_CompanyClientId, m_RunId, EVAC_ID, ClientId);
                            LoggingMethods.WriteToConsole("BuildAllBlobContent Result: " + Result);
							if (string.IsNullOrEmpty(Result))
							{
								Result = 
									BuildAllBlobAttachments(
										m_CompanyClientId, m_RunId, EVAC_ID, ClientId, 
										ref Attachments, ref StatementSummaryId);
								LoggingMethods.WriteToConsole("BuildAllBlobAttachments Count: " + Attachments.Count);
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

							// After the non-common attachments are added now add any common attachments
							foreach (EmailAttachment Item in CommonAttachments)
                            {
								NewAttachment = new EmailAttachment();
								NewAttachment.FileName = Item.FileName;
								SendEmailParam.EmailAttachments.Add(NewAttachment);
                            }

							//if (string.IsNullOrEmpty(Result)) Result = "Stop Here";
							if (string.IsNullOrEmpty(Result))
							{
                                /* xxx */
                                LoggingMethods.WriteToConsole("Before AttachmentsList");
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
								LoggingMethods.WriteToConsole("After AttachmentsList");
								List<NameAndAddressItem> Recipients = new List<NameAndAddressItem>();
								List<NameAndAddressItem> CCRecipients = new List<NameAndAddressItem>();
								List<NameAndAddressItem> BCCRecipients = new List<NameAndAddressItem>();
								List<NameAndAddressItem> ExclusionList = new List<NameAndAddressItem>();
								SenderDetailsItem Sender = new SenderDetailsItem();
                                //WriteToOutput("Cleared Recipients");
                                LoggingMethods.WriteToConsole("Before BuildEmailAddressItems");
								Result = 
									BuildEmailAddressItems(
										m_RunId, m_CompanyClientId, ClientId, StatementSummaryId, 
										ref Sender, ref Recipients, ref CCRecipients, 
										ref BCCRecipients, ref ExclusionList);
                                LoggingMethods.WriteToConsole("After BuildEmailAddressItems");
                                LoggingMethods.WriteToConsole("Result: " + Result);
                                LoggingMethods.WriteToConsole("Sender.ReplyToAddress: " + Sender.ReplyToAddress);
								if (string.IsNullOrEmpty(Result))
								{
									//Result = SendEmailParam.CopySenderFrom(Sender);
									Result = SendEmailParam.CopySenderFrom(Sender);
									LoggingMethods.WriteToConsole("SendEmailParam.Sender.Address: " + SendEmailParam.Sender.Address);
									LoggingMethods.WriteToConsole("SendEmailParam.Sender.ReplyTo: " + SendEmailParam.Sender.ReplyToAddress);
									
									if (string.IsNullOrEmpty(Result))
									{
 										LoggingMethods.WriteToConsole("Before copying Recipients");
										LoggingMethods.WriteToConsole("Existing Recipients Count: " + SendEmailParam.Recipients.Count);
										LoggingMethods.WriteToConsole("New Recipients Count: " + Recipients.Count);
										Result = SendEmailParam.CopyRecipientsFrom(Recipients);
										LoggingMethods.WriteToConsole("After copying Recipients");
										LoggingMethods.WriteToConsole("Existing Recipients Count: " + SendEmailParam.Recipients.Count);
									}
									
									if (string.IsNullOrEmpty(Result))
									{
										Result = SendEmailParam.CopyCCRecipientsFrom(CCRecipients);
									}
									
									if (string.IsNullOrEmpty(Result))
									{
										LoggingMethods.WriteToConsole("Before copying BCC entries");
										LoggingMethods.WriteToConsole("Count: " + SendEmailParam.BCCRecipients.Count);
										Result = SendEmailParam.CopyBCCRecipientsFrom(BCCRecipients);
										LoggingMethods.WriteToConsole("After copying BCC entries");
										LoggingMethods.WriteToConsole("Count: " + SendEmailParam.BCCRecipients.Count);
									}
									
									if (string.IsNullOrEmpty(Result))
									{
										Result = SendEmailParam.CopyExclusionsFrom(ExclusionList);
									}
								}
                            }

                            if (string.IsNullOrEmpty(Result))
							{
								Result =
									UpdateClientEmailAddressItems(m_CompanyClientId, EVAC_ID, SendEmailParam.Recipients);
                            }


                            FileName = m_DocsLocation + Convert.ToString(m_RunId) + "_" + Convert.ToString(ClientId) + "_UnadulteratedEmailBody.json";
                            using (TextWriter tw = new StreamWriter(FileName))
                            {
                                tw.Write(SendEmailParam.EmailBody);
                            }

                            List<string> MergeFields = CommonMethods.ParseEmailBody(SendEmailParam.EmailBody);
                            LoggingMethods.WriteToConsole("Merge Fields");
                            foreach (string str in MergeFields)
                            {
                                LoggingMethods.WriteToConsole(str);
                            }

                            //EmailBody = "xxxx";
                            //if ((string.IsNullOrEmpty(Result)))// && (TemplateId > 0))
                            if ((string.IsNullOrEmpty(Result)) && (TemplateId > 0))
							{
                            	string EmailBody = "";
                                TempSubjectLine = "";
								LoggingMethods.WriteToConsole("TemplateId: " + TemplateId);
								Result = 
                                    BuildClientEmailBody(
                                        m_CompanyClientId, m_RunId, EVAC_ID, ClientId, TemplateId, 
                                        out EmailBody, out TempSubjectLine);
								LoggingMethods.WriteToConsole("Result: " + Result);
								if (string.IsNullOrEmpty(Result))
								{
									SendEmailParam.EmailBody = EmailBody;
								}
                                FileName = m_DocsLocation + Convert.ToString(m_RunId) + "_" + Convert.ToString(ClientId) + "_EmailBody.txt";
                                using (TextWriter tw = new StreamWriter(FileName))
								{
									tw.Write(EmailBody);
								}
								LoggingMethods.WriteToConsole("EmailBody in File: " + FileName);
                                //WriteToOutput(EmailBody);
                                LoggingMethods.WriteToConsole("Global SubjectLine: " + SendEmailParam.Subject + " Attachment Subject Line: " + TempSubjectLine);
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
										
										LoggingMethods.WriteToConsole("After ProcessEmailBody Result:");
										LoggingMethods.WriteToConsole(Result);

										//appLogger.WriteLine("Images Count: " + bodyImages.Count, true, true);
										if (string.IsNullOrEmpty(Result))
										{
											SendEmailParam.EmailBodyImages = bodyImages;
											SendEmailParam.EmailBody = StrEmailBody;
										}
									}			
						
									LoggingMethods.WriteToConsole("Body After Processing");
									LoggingMethods.WriteToConsole(SendEmailParam.EmailBody);
								}
								
								if (string.IsNullOrEmpty(Result))
								{
									string HtmlFileName = m_DocsLocation + "0Test.html";
									
									/************************************************************************
									Result = 
										Actions.SendRTFEmail.Execute(
											DataObject, FCommonItem, FSettingsItem, m_MasterId, m_RTFFileName);
									*************************************************************************/
									//SendEmailParam.Display();									
									string Str = SendEmailParam.Display();
									LoggingMethods.WriteToConsole(Str);
									
									LoggingMethods.WriteToConsole("Attachment Count Before Email Send: " + SendEmailParam.EmailAttachments.Count);
									
									/* Send the email */
									SendEmail EmailSender = new SendEmail();
									if (m_ConfigSettings.SendEmail == "Y")
									{
										Result = EmailSender.Send(SendEmailParam, m_RunId);
									}
				
									LoggingMethods.WriteToConsole("After SendEmail: Result: " + Result);
									if (string.IsNullOrEmpty(Result))
									{
										if (m_ConfigSettings.UpdateSentValues == "Y")
										{
											UpdateSentValues(
												EVAC_ID, SendEmailParam.Sender, SendEmailParam.Recipients, 
												SendEmailParam.CCRecipients, SendEmailParam.BCCRecipients, 
												SendEmailParam.ExclusionList);
										}
									}
									else
									{
										UpdateSentComment(EVAC_ID, Result);
									}
									//Result = Actions.SendHTMLEmail.Execute(DataObject, HtmlFileName, out ErrorMsg);
								}
								if (!string.IsNullOrEmpty(ErrorMsg)) LoggingMethods.WriteToConsole("Error:" + ErrorMsg);
								if (!string.IsNullOrEmpty(Result)) LoggingMethods.WriteToConsole("Result:" + Result);
							}						
						}
					}
				}
				#endregion
			}

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
					LoggingMethods.WriteToConsole("File Read OK");
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
				
				DataTable dt = FirebirdUtilities.GetFirstDataTable(m_ConnectionString, QueryText, Params, 0);
				if ((dt == null) || (dt.Rows.Count == 0))
				{
					Result = "No data table returned";
					LoggingMethods.WriteToConsole(Result);
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
		
		/*
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
		*/
		
		private string PopulateEmailSettings(Int64 CompanyClientId,
		                                     ref AccountDetails AccountDetails)
		{
			string Result = "";
			string QueryText = 
				" SELECT CO_EMAILHOST, I_CO_EMAILPORT, CO_EMAILUSERNAME, CO_EMAILPASSWORD, BYPASSAUTHCHECK" +
				" FROM EACompanySettings" +
				" WHERE I_SETTINGS_COMPANYID = @CompanyClientId";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("CompanyClientId", CompanyClientId));
            UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, "PopulateEmailSettings", 0);
            LoggingMethods.WriteParamsToOutput(m_RunId, QueryText, Params);

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
			LoggingMethods.WriteToConsole("Inside: " + Method);
			
			// Gather the records from EAEmailAttachments for the runId
			
			string QueryText =
				" SELECT FileExtension, FileName, BlobFileName, StatementSummaryId, Entry_Source" +
				" FROM EAEventClientAttachment(@CompanyClientId, @RunId, 0)";

			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("CompanyClientId", CompanyClientId));
			Params.Add(new FbParameterItem("RunId", RunId));
            LoggingMethods.WriteParamsToOutput(m_RunId, QueryText, Params);
			
			string Msg = "Processing QueryText:" + Environment.NewLine + QueryText + Environment.NewLine + "For RunId: " + RunId.ToString();
			WindowsEventLoggingUtilities.WriteToInfoEvent(Msg, "EmailProcessor-ExecuteRun", 0);
			LoggingMethods.WriteToConsole(Msg);
			DataTable dt = FirebirdUtilities.GetFirstDataTable(m_ConnectionString, QueryText, Params, 0);
			if (dt == null)
			{
				Result = "No data table returned in " + Method;
				WindowsEventLoggingUtilities.WriteToInfoEvent(Result, Method, 0);
				Result = "";
			}
			else if (dt.Rows.Count > 0)
			{
				LoggingMethods.WriteToConsole("DocsLocation:" + m_DocsLocation);
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
						LoggingMethods.WriteToConsole("Added attachment: " + WorkingFileName);
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
			LoggingMethods.WriteToConsole("Inside " + MethodName);
			LoggingMethods.WriteToConsole("ErrorMsg " + ErrorMsg);
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
			LoggingMethods.WriteParamsToOutput(m_RunId, QueryText, Params);
			
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
					LoggingMethods.WriteToConsole(Result);
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
					EAOfficeAPI.ProgressLogging.BuildContentAndLog(
						true, m_MasterId, 
						EAOfficeAPI.GeneralResponseStatus.LoggingEntry, 
						"", Msg, "", "", Method, 
						EAOfficeAPI.JsonContentEntryType.Logging_Entry, 
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
		
		/*
		private void WriteParamsToOutput(string QueryText,
		                                 List<FbParameterItem> Params)
		{
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
		*/

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
			
			if (string.IsNullOrEmpty(Source))
			{
				return Result;
			}
			
			int StrLength = Source.Length;
			int MinRequiredStrLength = StartIndex + Count + 1;
			
			if (StrLength >= MinRequiredStrLength)
			{
				Result = Source.Substring(StartIndex, Count);
				return Result;
			}
			
			// The length of the Source string is less than the required size
			if (StrLength <= (StartIndex + 1))
			{
				return "";
			}
			
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
			LoggingMethods.WriteToConsole(sb.ToString());
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
			
			LoggingMethods.WriteToOutputLog(RunId, "Inside BuildCommonEmailSettings");
			
			string QueryText =
				" SELECT EVA_SubjectLine, EVA_EmailBody, I_EVA_EmailTemplateId" + 
				" FROM EAEventRun WHERE EVA_ID = @RunId";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("RunId", RunId));
			LoggingMethods.WriteParamsToOutput(RunId, QueryText, Params);
			
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
			LoggingMethods.WriteToConsole(Source);
			
  		    string AppName = "";
  		    string ArgsBase = "";
  		    string[] delimiters = new string[]{"/", " "};
  		    List<string> ArgsList = new List<string>();

			string EventId = "";
			
			string CmdBase = "";
			
			LoggingMethods.WriteToConsole("RunId: " + Convert.ToString(RunId));
			LoggingMethods.WriteToConsole("EventRunClientId: " + Convert.ToString(EventRunClientId));			
			
			string AppDir = System.AppDomain.CurrentDomain.BaseDirectory;
			//string WorkingDir = m_EAOWordLocation;
			string WorkingDir = "";
			
			if (m_ConfigSettings.RunningOnEmailServer == "Y")
			{
				CommonMethods.CopySharedFiles(
					m_ConnectionString, CompanyClientId, RunId, EVAC_ID, EventRunClientId, m_ConfigSettings);
			}

			List<string> CommandLines =
				GetRunCommandLines(m_ConnectionString, CompanyClientId, RunId, EVAC_ID, EventRunClientId);
				//GetWordDocumentCommandLines(m_ConnectionString, RunId, EventRunClientId);
			if (CommandLines.Count > 0) LoggingMethods.WriteToConsole("CommandLines.Count: " + CommandLines.Count);
			//int tries = 180;
			foreach (string Item in CommandLines)
			{
				LoggingMethods.WriteToConsole("Command Line Item:" + Item);
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
				LoggingMethods.WriteToConsole("AppName: " + AppName);
				LoggingMethods.WriteToConsole("Args: " + ArgsBase);

				EventId = GetArgValue(ArgsBase, "EventId", out Result);				
				LoggingMethods.WriteToConsole("EventId: " + EventId);
				if (!string.IsNullOrEmpty(Result)) LoggingMethods.WriteToConsole("Error: " + Result);
				
				if (string.IsNullOrEmpty(Result))
				{
					Directory.SetCurrentDirectory(WorkingDir);
					try
					{
						Process runApp = new Process();
						runApp.StartInfo.FileName = AppName;
						runApp.StartInfo.Arguments = ArgsBase;
						runApp.Start();
						for (int Index = 0; Index < m_RetryCount; Index++)
						{
							if (!runApp.HasExited) System.Threading.Thread.Sleep(1000);
						}
						
						LoggingMethods.WriteToConsole("Finished Creating Document");
						LoggingMethods.WriteToConsole("");
						
						if (runApp.HasExited)
						{
							
						}
					}
					finally
					{
						Directory.SetCurrentDirectory(AppDir);
					}
					
				}
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
			LoggingMethods.WriteToConsole(Source);
			
			LoggingMethods.WriteToConsole("RunId: " + Convert.ToString(RunId));
			
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
            LoggingMethods.WriteToConsole("QueryText");
            LoggingMethods.WriteToConsole(QueryText);
            LoggingMethods.WriteParamsToOutput(RunId, QueryText, Params);
            LoggingMethods.WriteToConsole("After WriteParams");

            try
            {
                string ErrorMsg = "";
                string[] FileNameParts = null;
                LoggingMethods.WriteToConsole("Before GetFirstDataTable");
                DataTable dt =
                    UsefulDBMethods.Helpers.GetFirstDataTableEx(
                        m_ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out ErrorMsg);
                LoggingMethods.WriteToConsole("After GetFirstDataTable");
                LoggingMethods.WriteToConsole("ErrorMsg: " + ErrorMsg);
                if (dt == null)
                {
                    ErrorMsg = string.Concat(new string[] {
                        "Error Executing Query",
                        Environment.NewLine,
                        "QueryText:",
                        Environment.NewLine,
                        QueryText
                    });
                    LoggingMethods.WriteToConsole(ErrorMsg);
                }
                else if (dt.Rows.Count == 0)
                {
                    ErrorMsg = "No rows in the table to process";
                    LoggingMethods.WriteToConsole(ErrorMsg);
                }
                else
                {
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow Row in dt.Rows)
                        {
                        	LoggingMethods.WriteToConsole("Inside foreach");
                            object obj = Row["FileName"];
                            string FileName = (obj == DBNull.Value) ? "" : Convert.ToString(obj);
                            FileNameParts = FileName.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                            FileName = FileNameParts[FileNameParts.Length - 1];
                            obj = Row["FileExtension"];
                            string FileExt = (obj == DBNull.Value) ? "" : Convert.ToString(obj);
                            obj = Row["StatementSummaryId"];
                            Int64 tempStatementSummaryId = (obj == DBNull.Value) ? 0 : Convert.ToInt64(obj);
                            if ((StatementSummaryId == 0) && (tempStatementSummaryId > 0)) StatementSummaryId = tempStatementSummaryId;
                            string FileNameOnly = "{0}-{1}{2}";
                            LoggingMethods.WriteToConsole("FileNameOnly: " + FileNameOnly);
                            if (!string.IsNullOrEmpty(FileName))
                            {
                                string DisplayName = FileName.Replace(" ", "_");
                                FileNameOnly = string.Format(FileNameOnly, EVAC_ID, DisplayName, FileExt);
                            }
                            else
                            {
                                //FileNameOnly = "{0}-{1}{2}";
                                FileNameOnly = string.Format(FileNameOnly, EVAC_ID, "Document", FileExt);
                            }

                            LoggingMethods.WriteToConsole("FileNameOnly: " + FileNameOnly);

                            string OutputFileName = m_DocsLocation + FileNameOnly;
                            LoggingMethods.WriteToConsole("OutputFileName: " + OutputFileName);
                            obj = Row["BlobFileName"];
                            byte[] toBytes = (byte[])obj;
                            string Base64String = System.Text.Encoding.Default.GetString(toBytes);
                            File.WriteAllBytes(OutputFileName, Convert.FromBase64String(Base64String));

                            /* Test this for the word attachments */ 
                            string Str = OutputFileName;
                            LoggingMethods.WriteToConsole("OutputJson" + Environment.NewLine + Str);
                            EmailAttachment NewAttachment = new EmailAttachment();
                            NewAttachment.FileName = Str;
                            Attachments.Add(NewAttachment);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingMethods.WriteToConsole(ex.Message);
                Result = ex.Message;
            }
			
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
            LoggingMethods.WriteParamsToOutput(RunId, QueryText, Params);
			
			Object Obj = null;
			string ColumnValue = "";
			NameAndAddressItem EmailItem = null;
			DataTable tbl = FirebirdUtilities.GetFirstDataTable(m_ConnectionString, QueryText, Params, 0);
			foreach (DataRow Row in tbl.Rows)
			{
				Obj = Row["FromAddress"];
				Sender.Address = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
				Sender.Name = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
				LoggingMethods.WriteToConsole("From Address: " + Sender.Address);
				
				LoggingMethods.WriteToConsole("Sender.Name: " + Sender.Name);
				LoggingMethods.WriteToConsole("Sender.Address: " + Sender.Address);
				
				Obj = Row["ReplyToAddress"];
				ColumnValue = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
				LoggingMethods.WriteToConsole("Column Value: " + ColumnValue);
				Sender.ReplyToAddress = ColumnValue;
					if (string.IsNullOrEmpty(Sender.ReplyToAddress))
						Sender.ReplyToAddress = Sender.Address;
				LoggingMethods.WriteToConsole("ReplyToAddress: " + Sender.ReplyToAddress);
				

				Obj = Row["EmailAddresses"];
				ColumnValue = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
				if (string.IsNullOrEmpty(ColumnValue))
				{
					Obj = Row["ExclusionTo"];
					ColumnValue = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
				}
				foreach (string Item in ColumnValue.Split(new string[]{";"}, StringSplitOptions.RemoveEmptyEntries))
				{
					EmailItem = new Actions.NameAndAddressItem();
					EmailItem.Address = Item;
					LoggingMethods.WriteToConsole("Adding Recipient Email Address - " + Item);
					
					Recipients.Add(EmailItem);
				}
				
				Obj = Row["CCAddress"];
				ColumnValue = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
				foreach (string Item in ColumnValue.Split(new string[]{";"}, StringSplitOptions.RemoveEmptyEntries))
				{
					EmailItem = new NameAndAddressItem();
					EmailItem.Address = Item;
					LoggingMethods.WriteToConsole("Adding CC Email Address - " + Item);
					CCList.Add(EmailItem);
				}
				
				Obj = Row["BCCAddress"];
				ColumnValue = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
				
				if (string.IsNullOrEmpty(ColumnValue))
					LoggingMethods.WriteToConsole("No BCC entries to process");
				else
					LoggingMethods.WriteToConsole("BCC - " + ColumnValue);
				
				foreach (string Item in ColumnValue.Split(new string[]{";"}, StringSplitOptions.RemoveEmptyEntries))
				{
					EmailItem = new NameAndAddressItem();
					EmailItem.Address = Item;
					LoggingMethods.WriteToConsole("Adding BCC Email Address - " + Item);
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
            LoggingMethods.WriteParamsToOutput(m_RunId, QueryText, Params);
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
						JsonUtilities.StartJsonObject("error") +
							JsonUtilities.AddPropertyNameAndValue("message", Result) +
							JsonUtilities.AddPropertyNameAndValue("query", QueryText) +
							JsonUtilities.StartJsonArray("values") +
							string.Join(",", Entries.ToArray()) +
							JsonUtilities.EndJsonArray() +
						JsonUtilities.EndJsonObject();
				LoggingEntry(Method, "", Result);
					
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
			LoggingMethods.WriteToConsole(Source);
			
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
                LoggingMethods.WriteParamsToOutput(RunId, QueryText, Params);
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
				LoggingMethods.WriteToConsole(ErrorMsg);
			}
			else if (dt.Rows.Count == 0) 
			{
				ErrorMsg = "No rows in the table to process";
				LoggingMethods.WriteToConsole(ErrorMsg);
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
            Request.Append(JsonUtilities.AddPropertyNameAndValue("timeout", "99999"));
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
			LoggingMethods.WriteToConsole("Processing Recipients - " + Recipients.Count);
			LoggingMethods.WriteToConsole("---------------------");
			foreach (NameAndAddressItem Item in Recipients)
			{
                //if (m_CompanyActiveStatus == "Y") Addr = Item.Address; else Addr = "Rdenardo01@gmail.com";
                Addr = Item.Address;
				LoggingMethods.WriteToConsole("CC Item - " + Addr);
                Str =
                    JsonUtilities.StartJsonObject("") +
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
			LoggingMethods.WriteToConsole("Processing CCs - " + CCList.Count);
			LoggingMethods.WriteToConsole("---------------------");
			foreach (NameAndAddressItem Item in CCList)
			{
				Addr = Item.Address;
				LoggingMethods.WriteToConsole("CC Item - " + Addr);
                Str =
					JsonUtilities.StartJsonObject("") +
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
			LoggingMethods.WriteToConsole("Processing BCCs - " + BCCList.Count);
			LoggingMethods.WriteToConsole("---------------------");
			foreach (NameAndAddressItem Item in BCCList)
			{
                Addr = Item.Address; 
				LoggingMethods.WriteToConsole("BCC Item - " + Addr);
                Str =
					JsonUtilities.StartJsonObject("") +
                    JsonUtilities.AddPropertyNameAndValue("address", Addr, false) +
                    JsonUtilities.EndJsonObject();
				StrList.Add(Str);
			}
			Request.Append(string.Join(",", StrList.ToArray()));
			Request.Append(JsonUtilities.EndJsonArray());
			Request.Append(",");

			// The Attachments
			Request.Append(JsonUtilities.StartJsonArray("attachments"));
			StrList.Clear();
			LoggingMethods.WriteToConsole("Processing Attachments - " + EmailAttachments.Count);
			LoggingMethods.WriteToConsole("---------------------");
            foreach (EmailAttachment Item in EmailAttachments)
            {
            	LoggingMethods.WriteToConsole("Attachment - " + Item.FileName);
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
			LoggingMethods.WriteToConsole("Inside " + MethodName);
			string CurrentTime = DateTime.Now.ToString("HH:mm:ss");
			DateTime CurrentDate = DateTime.Now;
			
			string Result = "";
			try
			{
				LoggingMethods.WriteToConsole("Processing SenderAddress");
				Sender.Address = DelphiSubstring(Sender.Address, 0, 120);
				
				LoggingMethods.WriteToConsole("Processing Recipients");
				string StrRecipients = BuildAddressList(Recipients); //string.Join(";", Recipients);
				StrRecipients = DelphiSubstring(StrRecipients, 0, 2000);				

				LoggingMethods.WriteToConsole("Processing CCList");
				string StrCC = BuildAddressList(CCList);//string.Join(";", CCList);
				StrCC = DelphiSubstring(StrCC, 0, 120);
				
				LoggingMethods.WriteToConsole("Processing BCC List");
				string StrBCC = BuildAddressList(BCCList);//string.Join(";", BCCList);

				StrBCC = DelphiSubstring(StrBCC, 0, 120);
				
				LoggingMethods.WriteToConsole("Processing Exclusions");
				string StrExclusions = BuildAddressList(ExclusionList);//string.Join(";", ExclusionList);
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
	
				LoggingMethods.WriteParamsToOutput(m_RunId, QueryText, Params);
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
					LoggingMethods.WriteToConsole(Msg);
					LoggingMethods.WriteToConsole("DBExecute Result:" + Environment.NewLine + Result);
				}
			}
			catch (Exception Ex)
			{
				Result = Ex.Message + Environment.NewLine + Ex.StackTrace;
				LoggingMethods.WriteToConsole(Result);				
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
			LoggingMethods.WriteParamsToOutput(RunId, QueryText, Params);
			
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
				LoggingMethods.WriteToConsole(ErrorMsg);
			}
			else if (dt.Rows.Count == 0) 
			{
				ErrorMsg = "No rows in the table to process";
				LoggingMethods.WriteToConsole(ErrorMsg);
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
