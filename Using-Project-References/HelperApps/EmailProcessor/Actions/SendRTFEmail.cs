/*
 * Created by SharpDevelop.
 * User: DELL 990
 * Date: 20/05/2019
 * Time: 5:57 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Newtonsoft.Json.Linq;
using CommonUtilities;
using System.Text;
using System.IO;
using System.Collections.Generic;
using EAOfficeAPI;
using System.Net.Mail;
using System.Windows.Forms;

namespace EmailProcessor.Actions
{
	/// <summary>
	/// Description of SendRTFEmail.
	/// </summary>
	public class SendRTFEmail : ActionBase
	{
		private string FModuleName = "EmailAction";
		private string FRunId = "";
		private int FRunIdValue = 0;
		//private JObject DataObject = null;
		private string FRTFFileName = "";
		private string FHtmlFileName = "";
		private SendEmail_Item FEmailItem = new SendEmail_Item();
		private string FWorkingDir = "";
		private string FDocsLocation = "";
		private string FEAOWordLocation = "";
		private string FTestRPOLocation = "";

		public SendRTFEmail()
		{
		}
		
		public static string Execute(JObject SourceDataObject,
		                             RequestCommonItem CommonItem,
		                             APISettingsItem SettingsItem,
		                             int MasterId,
		                             string RTFFileName)
		{
			string Method = "Execute";
			Console.WriteLine("Inside - " + Method);
			string Result = "";			

			SendRTFEmail inst = new SendRTFEmail();
			/*
			RequestCommonItem CommonItem = new RequestCommonItem();
			APISettingsItem SettingsItem = new APISettingsItem();
			CommonItem.Plugin = inst.GetType().Name;
			CommonItem.Debug = "true";
			SettingsItem.DebugLogging = true;
			*/
			if (!inst.Setup(MasterId, CommonItem, SourceDataObject, SettingsItem, out Result)) return Result;
			//inst.DataObject = SourceDataObject;
			if (inst.DataObject == null)
			{
				Result = "The provided DataObject is NULL";
				//return "";
			}
			
			string TemplateId = (string)inst.DataObject["templateId"];
			if (StringUtilities.StrToIntDef(TemplateId, -1) > 0)
			{			
				inst.FRTFFileName = RTFFileName;
				if (string.IsNullOrEmpty(inst.FRTFFileName))
				{
					Result = "The provided eMail body filename is missing or empty.";
					//return "";
				}
				
				if (!File.Exists(inst.FRTFFileName))
				{
					Result = "The provided eMail body filename does not exist.";
					//return "";
				}
			}
			//if (!inst.Setup(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg)) return "";
			
			if (!string.IsNullOrEmpty(Result)) return Result;
			
			Result = inst.LogOnInsideMethod(Method);
			WindowsEventLoggingUtilities.WriteToErrorEvent("LogOnInsideMethod Result: " + Result, inst.FModuleName, 0);
			try
			{
				Result = inst.Init();				
				if (!string.IsNullOrEmpty(Result))
				{
					//WindowsEventLoggingUtilities.WriteToInfoEvent(Result, Method, MasterId);
					//return "";
					return Result;
				}
				
				Result = inst.Validate();
				if (!string.IsNullOrEmpty(Result))
				{
					//WindowsEventLoggingUtilities.WriteToInfoEvent(Result, Method, MasterId);
					//return "";
					return Result;
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
			
			JObject jObj = null;
			JArray jArray = null;
			//int WordResultValue = 0;
			string Str = "";
			
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " + Method, FModuleName, 0);
			LogOnInsideMethod(Method);
			try 
			{
				FRunId = (string)DataObject["runId"];
				FRunIdValue = StringUtilities.StrToIntDef(FRunId, -1);
				
				FDocsLocation = (string)DataObject["docsLocation"];
				FTestRPOLocation = (string)DataObject["testRPOLocation"];
				FEAOWordLocation = (string)DataObject["eaWordLocation"];
				WriteToOutput("DocsLocation: " + FDocsLocation);
				WriteToOutput("TestRPOLocation: " + FTestRPOLocation);
				WriteToOutput("EAWordLocation: " + FEAOWordLocation);
				
				if (string.IsNullOrEmpty(Result))
				{
					jObj = (DataObject["common"] as JObject);				
					Result = FEmailItem.Common.Load(jObj);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					FEmailItem.Subject = (string)DataObject["emailSubject"];
					//FEmailItem.Body = EmailBody;
					FEmailItem.SenderAddress = (string)DataObject["senderAddress"];
					Str = (string)DataObject["templateId"];
					FEmailItem.TemplateId = StringUtilities.StrToIntDef(Str, 0);
					FWorkingDir = Directory.GetCurrentDirectory();
					if (!FWorkingDir.EndsWith(Path.DirectorySeparatorChar.ToString())) FWorkingDir += Path.DirectorySeparatorChar;
				}
				
				/*
				if (string.IsNullOrEmpty(Result))
				{
					this.FHtmlFileName = this.FRTFFileName + ".html";
					if (File.Exists(FHtmlFileName)) File.Delete(FHtmlFileName);
					
					TMSWordHelpers msWord = new TMSWordHelpers();
					//FEmailItem.Body =
					WordResultValue =
						msWord.RTFDocToHTMLFiltered(this.FRTFFileName, this.FHtmlFileName);
					if (WordResultValue < 0)
					{
						Result = msWord.ErrorDescription(WordResultValue);
					}
					else if (!File.Exists(this.FHtmlFileName))
					{
						Result = "Error creating the HTML version of the email body";
					}
				}
				*/
				
				if (string.IsNullOrEmpty(Result))
				{
					jArray = (JArray)DataObject["recipients"];
					Result = FEmailItem.LoadRecipients(jArray);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					jArray = (JArray)DataObject["cc"];
					Result = FEmailItem.LoadCCs(jArray);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					jArray = (JArray)DataObject["bcc"];
					Result = FEmailItem.LoadBCCs(jArray);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					jArray = (JArray)DataObject["attachments"];
					Result = FEmailItem.LoadAttachments(jArray);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					jArray = (JArray)DataObject["images"];
					Result = FEmailItem.LoadImages(jArray);
				}
				
			}
			catch (Exception Ex)
			{
				Result = LogOnException(Method, Ex, "");
				//Result = Ex.Message + Environment.NewLine + Ex.InnerException;				
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
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " +Method, FModuleName, 0);
			LogOnInsideMethod(Method);
			try 
			{
				if (string.IsNullOrEmpty(Result) && (FRunIdValue < 0)) Result = "Invalid RunId provided";
				//if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(FRTFFileName)) Result = "Invalid or missing RTF FileName provided.";
				if (string.IsNullOrEmpty(Result) && (FEmailItem.TemplateId > 0) && string.IsNullOrEmpty(FRTFFileName)) Result = "Invalid or missing RTF FileName provided.";
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = FEmailItem.Validate(ValidateBody: false);
				}				

				if (!string.IsNullOrEmpty(Result))
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent("Validation Failed: " + Result, FModuleName, 0);
					WriteToOutput("Validation Failed: " + Environment.NewLine + Result);
					Result =
						JsonUtilities.StartJson() +
						JsonUtilities.AddPropertyNameAndValue("Error", "Validation Failed") +
						JsonUtilities.AddPropertyNameAndValue("Result", Result, false) +
						JsonUtilities.EndJson();
				}
			}
			catch (Exception Ex)
			{
				Result = LogOnException(Method, Ex, "");
				//Result = Ex.Message + Environment.NewLine + Ex.InnerException;				
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
			//WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " +Method, FModuleName, 0);
			LogOnInsideMethod(Method);
			try 
			{
				//Result = BuildOutput(out ErrorMsg);
				BuildOutput(out ErrorMsg);
				if (!string.IsNullOrEmpty(ErrorMsg))
				{
					Result = LogOnProcessingError(Method, ErrorMsg, "");
				}
			}
			catch (Exception Ex)
			{
				Result = LogOnException(Method, Ex, "");
				//Result = Ex.Message + Environment.NewLine + Ex.InnerException;				
			}
			finally
			{
				LogOnLeavingMethod(Method);
			}
			//WindowsEventLoggingUtilities.WriteToInfoEvent("Leaving: " +Method, FModuleName, 0);
			
			return Result;
		}
		
/*
		private string BuildOutput(out string ErrorMsg)
		{
			ErrorMsg = "";
			string Result = "";
			string Method = "BuildOutput";
			Console.WriteLine("Inside - " + Method);
			
			List<string> ErrorList = new List<string>();
			
			LogOnInsideMethod(Method);
			try
			{
				WindowsEventLoggingUtilities.WriteToInfoEvent("Inside BuildOutput", FModuleName, 0);
				
				StringBuilder entries = new StringBuilder();
				entries.AppendLine(JsonUtilities.StartJson());
				
				RichTextBox richText = new RichTextBox();
				richText.Visible = false;
				richText.LoadFile(FRTFFileName);
				richText.SaveFile(FRTFFileName);
				
//				 Convert the RTF to HTML
//				
//				TMSWordHelpers msWordHelpers = new TMSWordHelpers();
//				Console.WriteLine("Word Version:" + msWordHelpers.WordVersion());
//				int WordResult = msWordHelpers.RTFDocToHTMLFiltered(FRTFFileName, FRTFFileName + ".html");
//				if (WordResult < 0) Result = msWordHelpers.ErrorDescription(WordResult);
//				else
//				{
//					Console.WriteLine("Check the file: " + FRTFFileName + ".html");
//					FEmailItem.Body = File.ReadAllText(FRTFFileName + ".html");
//				}
//								
				RTFToHtml helper = new RTFToHtml();
				helper.settings.SourceFile = FRTFFileName;
				helper.settings.SpecialCharsRepresentation = "Tabulator=&gt;";
				Console.WriteLine("Before Html conversion");
				helper.Execute();
				Console.WriteLine("After Html conversion");
				FHtmlFileName = Path.GetDirectoryName(FRTFFileName);
				if (!FHtmlFileName.EndsWith("\\")) FHtmlFileName += "\\";
				FHtmlFileName += Path.GetFileNameWithoutExtension(FRTFFileName);
				FHtmlFileName += ".html";
				if (!File.Exists(FHtmlFileName)) 
					ErrorList.Add("The file: " + FHtmlFileName + " does not exist");
				else
				{
					FEmailItem.Body = File.ReadAllText(FHtmlFileName);
					FEmailItem.Body = FEmailItem.Body.Replace("\r\n", "");
				}
				
				if (string.IsNullOrEmpty(FEmailItem.Body))
					ErrorList.Add("There is no body text for the email");
				
				if (ErrorList.Count != 0)
				{
					entries.AppendLine(JsonUtilities.StartJsonArray("Errors"));
					entries.Append(string.Join(",", ErrorList.ToArray()));
					entries.AppendLine(JsonUtilities.EndJsonArray());
				}
				else
				{
					entries.AppendLine(JsonUtilities.StartJsonObject("Output"));
					GenerateOutputValues(ref entries);
					entries.AppendLine(JsonUtilities.EndJsonObject());
					entries.Append(",");
					//entries.AppendLine(JsonUtilities.StartJsonObject("SendEmailResult"));
					entries.AppendLine(JsonUtilities.StartJsonArray("SendEmailResult"));
					Result = SendEmailItem(ref entries);
					entries.Append(Result);
					//entries.AppendLine(JsonUtilities.EndJsonObject());
					entries.AppendLine(JsonUtilities.EndJsonArray());
				}

				entries.AppendLine(JsonUtilities.EndJson());
				Result = entries.ToString();
			}
			catch (Exception Ex)
			{
				Result = LogOnException(Method, Ex, "");
				//Result = Ex.Message + Environment.NewLine + Ex.InnerException;				
			}
			finally
			{
				LogOnLeavingMethod(Method);
			}
			
			return Result;
		}
*/		
		
		private void BuildOutput(out string ErrorMsg)
		{
			ErrorMsg = "";
			//string Result = "";
			string Method = "BuildOutput";
			Console.WriteLine("Inside - " + Method);
			WriteToOutput("Inside - " + Method);
			
			List<string> ErrorList = new List<string>();
			
			LogOnInsideMethod(Method);
			try
			{
				WindowsEventLoggingUtilities.WriteToInfoEvent("Inside BuildOutput", FModuleName, 0);
				
				StringBuilder entries = new StringBuilder();
				entries.AppendLine(JsonUtilities.StartJson());
				
				WriteToOutput("Before new RichTextBox - 1");
				RichTextBox richText = new RichTextBox();
				WriteToOutput("FEmailItem.TemplateId: " + FEmailItem.TemplateId.ToString());
				if (FEmailItem.TemplateId > 0)
				{
					try
					{
						WriteToOutput("***** Before richText Processing *****");
						richText.Visible = false;
						richText.LoadFile(FRTFFileName);
						richText.SaveFile(FRTFFileName);
						WriteToOutput("***** After richText Processing *****");						
					}
					catch
					{
						WriteToOutput("***** Inside Catch for richText Processing *****");
					}
				}
				WriteToOutput("After new RichTextBox - 1");
				
				// Convert the RTF to HTML //
//				TMSWordHelpers msWordHelpers = new TMSWordHelpers();
//				Console.WriteLine("Word Version:" + msWordHelpers.WordVersion());
//				int WordResult = msWordHelpers.RTFDocToHTMLFiltered(FRTFFileName, FRTFFileName + ".html");
//				if (WordResult < 0) Result = msWordHelpers.ErrorDescription(WordResult);
//				else
//				{
//					Console.WriteLine("Check the file: " + FRTFFileName + ".html");
//					FEmailItem.Body = File.ReadAllText(FRTFFileName + ".html");
//				}
				
				WriteToOutput("Before FEmailItem.TemplateId > 0");
				if (FEmailItem.TemplateId > 0)
				{
					RTFToHtml helper = new RTFToHtml();
					helper.settings.SourceFile = FRTFFileName;
					helper.settings.SpecialCharsRepresentation = "Tabulator=&gt;";
					Console.WriteLine("Before Html conversion");
					WriteToOutput("Before Html conversion");
					helper.Execute();
					Console.WriteLine("After Html conversion");
					WriteToOutput("After Html conversion");
					FHtmlFileName = Path.GetDirectoryName(FRTFFileName);
					if (!FHtmlFileName.EndsWith("\\")) FHtmlFileName += "\\";
					FHtmlFileName += Path.GetFileNameWithoutExtension(FRTFFileName);
					FHtmlFileName += ".html";
					if (!File.Exists(FHtmlFileName))
					{ 
						//ErrorList.Add("The file: " + FHtmlFileName + " does not exist");
						WriteToOutput("The file: " + FHtmlFileName + " does not exist");
						FEmailItem.Body = "The file: " + FHtmlFileName + " does not exist";
						FEmailItem.Body = File.ReadAllText(FRTFFileName);
					}
					else
					{
						FEmailItem.Body = File.ReadAllText(FHtmlFileName);
						FEmailItem.Body = FEmailItem.Body.Replace("\r\n", "");
						if (string.IsNullOrEmpty(FEmailItem.Body))
						{
							ErrorList.Add("There is no body text for the email");
							WriteToOutput("There is no body text for the email");						
						}
					}
				}
				else
				{
					// get the email body from the EAEventRun run table
					// the content will already be HTML and will not 
					// need to be processed any further
					FEmailItem.Body = CommonMethods.GetEventRunEmailBody(FbConnectionString, FRunIdValue, out ErrorMsg);
					if (!string.IsNullOrEmpty(ErrorMsg)) ErrorList.Add(ErrorMsg);
				}
				WriteToOutput("After FEmailItem.TemplateId > 0");
				
				Console.WriteLine("EMailBody: " + FEmailItem.Body);
				WriteToOutput("EMailBody: " + FEmailItem.Body);
				
				if (ErrorList.Count == 0)
				{
					WriteToOutput("Before ProcessEmailBodyImages");
					ErrorMsg = ProcessEmailBodyImages();
					WriteToOutput("After ProcessEmailBodyImages");
				}
				
				if (ErrorList.Count != 0)
				{
					entries.AppendLine(JsonUtilities.StartJsonArray("Errors"));
					entries.Append(string.Join(",", ErrorList.ToArray()));
					entries.AppendLine(JsonUtilities.EndJsonArray());
				}
				else
				{
					entries.AppendLine(JsonUtilities.StartJsonObject("Output"));
					WriteToOutput("Before GenerateOutputValues");
					GenerateOutputValues(ref entries);
					WriteToOutput("After GenerateOutputValues");
					entries.AppendLine(JsonUtilities.EndJsonObject());
					entries.Append(",");
					//entries.AppendLine(JsonUtilities.StartJsonObject("SendEmailResult"));
					entries.AppendLine(JsonUtilities.StartJsonArray("SendEmailResult"));
					WriteToOutput("Before SendEmailItem");
					ErrorMsg = SendEmailItem(ref entries);
					WriteToOutput("After SendEmailItem");
					//entries.Append(Result);
					entries.Append(ErrorMsg);
					//entries.AppendLine(JsonUtilities.EndJsonObject());
					entries.AppendLine(JsonUtilities.EndJsonArray());
				}

				entries.AppendLine(JsonUtilities.EndJson());
				//Result = entries.ToString();
				//LogResponse(Method, Result);
				//Result = entries.ToString();
				
				LogResponse(Method, ErrorMsg);
			}
			catch (Exception Ex)
			{
				//Result = LogOnException(Method, Ex, "");
				ErrorMsg = LogOnException(Method, Ex, "");
				//Result = Ex.Message + Environment.NewLine + Ex.InnerException;				
			}
			finally
			{
				LogOnLeavingMethod(Method);
			}
			
			//return Result;
		}
		
		private string ProcessEmailBodyImages()
		{
			string Result = "";
			
			/*
			 Get the list of img src entries from the email body
			 Generate CID entries for the images and replace the src content with the CID conent
			 Add the CID entry to the attachments list  
			*/
			WriteToOutput("Inside ProcessEmailBodyImages");
			Result = BuildBodyImageList(FEmailItem.Body, ref FEmailItem.BodyImages);
			WriteToOutput("BodyImage Count: " + FEmailItem.BodyImages.Count.ToString());
			return Result;
		}
		
		private string BuildBodyImageList(string EmailBody,
		                                  ref List<SendEmail_BodyImage> BodyImages)
		{
			string Result = "";
			int CurrentImgPos = EmailBody.IndexOf("<img ");
			int CurrentSrcPos = -1;
			int EndSrcPos = 0;
			string AttrChar = "";
			string SrcFileName = "";
			int TempPos = 0;
			SendEmail_BodyImage Item = null;
			while (CurrentImgPos != -1)
			{
				WriteToOutput("CurrentImgPos: " + CurrentImgPos.ToString());
				// look for the scr attribute
				CurrentSrcPos = EmailBody.IndexOf("src=", CurrentImgPos);
				if (CurrentSrcPos != -1)
				{
					WriteToOutput("CurrentSrcPos: " + CurrentSrcPos.ToString());
					CurrentImgPos = CurrentSrcPos;
					// get the next char after the = sign, this will tell us whether we are using single or double quotes
					TempPos = EmailBody.IndexOf("=", CurrentSrcPos);
					WriteToOutput("TempPos: " + TempPos.ToString());
					//AttrChar = EmailBody.Substring(CurrentImgPos + 4, 1);
					AttrChar = EmailBody.Substring(TempPos + 1, 1);
					if (!string.IsNullOrEmpty(AttrChar)) TempPos += 1;
					WriteToOutput("AttrChar: " + AttrChar);
					CurrentSrcPos += 1;
					//EndSrcPos = EmailBody.IndexOf(AttrChar, CurrentImgPos + 1);					
					EndSrcPos = EmailBody.IndexOf(AttrChar, TempPos + 1);
					//SrcFileName = EmailBody.Substring(CurrentSrcPos, (EndSrcPos - CurrentSrcPos) - 1);					
					SrcFileName = EmailBody.Substring(TempPos + 1, (EndSrcPos - TempPos) - 1);
					Item = new SendEmail_BodyImage();
					Item.Identifier = SrcFileName;
					Item.FileName = FDocsLocation + SrcFileName;
					BodyImages.Add(Item);
					Console.WriteLine("Item.Identifier: " + Item.Identifier);
					Console.WriteLine("Item.FileName: " + Item.FileName);
					WriteToOutput("Item.Identifier: " + Item.Identifier);
					WriteToOutput("Item.FileName: " + Item.FileName);
					CurrentSrcPos = -1;
				}
				CurrentImgPos = EmailBody.IndexOf("<img ", CurrentImgPos);
			}
			
			return Result;
		}

		private void GenerateOutputValues(ref StringBuilder Source)
		{
			List<string> StrList = new List<string>();
			string Str = "";
			
			Source.AppendLine(JsonUtilities.AddPropertyNameAndValue("Host", FEmailItem.Common.Host));
			Source.AppendLine(JsonUtilities.AddPropertyNameAndValue("EmailSubject", FEmailItem.Subject));
			Source.AppendLine(JsonUtilities.AddPropertyNameAndValue("RecipientCount", FEmailItem.Recipients.Count.ToString()));
			Source.AppendLine(JsonUtilities.AddPropertyNameAndValue("CCCount", FEmailItem.CCs.Count.ToString()));
			Source.AppendLine(JsonUtilities.AddPropertyNameAndValue("BCCCount", FEmailItem.BCCs.Count.ToString()));
			Source.AppendLine(JsonUtilities.AddPropertyNameAndValue("AttachmentCount", FEmailItem.Attachments.Count.ToString()));
			Source.AppendLine(JsonUtilities.StartJsonArray("Attachments"));
			foreach (SendEmail_AttachmentFile Item in FEmailItem.Attachments)
			{
				Str = 
					JsonUtilities.StartJsonObject("") +
					//JsonUtilities.AddPropertyNameAndValue("Path", Item.Path) +
					JsonUtilities.AddPropertyNameAndValue("FileName", Item.FileName, false) +
					JsonUtilities.EndJsonObject();
				StrList.Add(Str);
			}
			Source.Append(string.Join(",", StrList.ToArray()));
			Source.AppendLine(JsonUtilities.EndJsonArray());
		}
		
		private string SendEmailItem(ref StringBuilder Source)
		{
			string result = "";
			string Method = "SendEmailItem";
			WriteToOutput("***** Inside " + Method + " *****");
			try
			{
				using (MailMessage mailMessage = new MailMessage())
				{
					mailMessage.From = new MailAddress(FEmailItem.SenderAddress);
					WriteToOutput("Sender Address: " + FEmailItem.SenderAddress);
					if (FEmailItem.Recipients.Count > 0)
					{
						foreach (SendEmail_Recipient Item in FEmailItem.Recipients)
						{
							mailMessage.To.Add(Item.Address);
							WriteToOutput("Recipient: " + Item.Address);
							//mailMessage.To.Add("johnebennett_au@yahoo.com.au");
						}
					}
					else
					{
						result = "There are no recipients to process";
						return result;
					}
					WriteToOutput("***** 1 *****");

					mailMessage.Subject = FEmailItem.Subject;
					//mailMessage.IsBodyHtml = true;
					//mailMessage.BodyEncoding = System.Text.Encoding.  
					mailMessage.Body = FEmailItem.Body;
					WriteToOutput("SMS Body: " + FEmailItem.Body);

					if (FEmailItem.CCs.Count > 0)
					{
						foreach (SendEmail_Recipient Item in FEmailItem.CCs)
						{
							mailMessage.CC.Add(Item.Address);
							//mailMessage.CC.Add("jebemf_au@yahoo.com.au");
						}
					}
					WriteToOutput("***** 2 *****");

					if (FEmailItem.BCCs.Count > 0)
					{
						foreach (SendEmail_Recipient Item in FEmailItem.BCCs)
						{
							mailMessage.Bcc.Add(Item.Address);
							//mailMessage.Bcc.Add("johnebennett_au@yahoo.com.au");
						}
					}
					WriteToOutput("***** 3 *****");
					
					string FileName = "";
					foreach (SendEmail_BodyImage Item in FEmailItem.BodyImages)
					{
						Attachment attachment = new Attachment(Item.FileName);
						attachment.ContentId = Item.Identifier;
						//attachment.
						mailMessage.Attachments.Add(attachment);
					}
					WriteToOutput("***** 4 *****");
					
					foreach (SendEmail_AttachmentFile Item in FEmailItem.Attachments)
					{
						//FileName = Path.Combine(Item.Path, Item.FileName);
						FileName = Item.FileName;
						mailMessage.Attachments.Add(new Attachment(FileName));
					}
					WriteToOutput("***** 5 *****");
					
					//SmtpClient smtpClient = new SmtpClient(FEmailItem.Common.Host);
					SmtpClient smtpClient = new SmtpClient();
					smtpClient.Host = FEmailItem.Common.Host;
					smtpClient.Port = CommonUtilities.StringUtilities.StrToIntDef(FEmailItem.Common.Port, 0);
					smtpClient.Credentials = new System.Net.NetworkCredential(FEmailItem.Common.EmailUserName, FEmailItem.Common.EmailPassword);
                    smtpClient.EnableSsl = true;
					WriteToOutput("***** 6 *****");
                    string str = Environment.CurrentDirectory + Path.DirectorySeparatorChar.ToString();
					if (FEmailItem.Common.Timeout > 0)
					{
						smtpClient.Timeout = FEmailItem.Common.Timeout * 1000;
						WindowsEventLoggingUtilities.WriteToInfoEvent("Setting Smtp Timeout to " + FEmailItem.Common.Timeout.ToString(), FModuleName, 0);
					}
					WriteToOutput("***** 7 *****");
					Console.WriteLine("Before smtpClient.Send");
					LogToInfoEvent("Before smtpClient.Send");
					//WindowsEventLoggingUtilities.WriteToInfoEvent("Before smtpClient.Send", FModuleName, 0);
					WriteToOutput("***** Before smtpClient.Send *****");
					smtpClient.Send(mailMessage);
					LogToInfoEvent("After smtpClient.Send");
					Console.WriteLine("After smtpClient.Send");
					WriteToOutput("***** After smtpClient.Send *****");
					//WindowsEventLoggingUtilities.WriteToInfoEvent("After smtpClient.Send", FModuleName, 0);
				}
				result = 
					JsonUtilities.AddPropertyNameAndValue("status", "SUCCESS") +
					JsonUtilities.AddPropertyNameAndValue("message", "", false);
				LoggingEntry(Method, "SendEmail Result", result);
				// if the email is successfully sent return nothing. Only return a result if an error okkurs
				result = "";
			}
			catch (Exception ex)
			{
				result = "Error: " + ex.Message + ", ";
				
				if (ex is SmtpException)
				{
					result = result + Environment.NewLine + (ex as SmtpException).StatusCode.ToString() + ",";
				}
				if (ex.InnerException != null)
				{
					result = result + Environment.NewLine + "Inner: " + ex.InnerException.Message + ", ";
				}				
				
				result = result + Environment.NewLine + "Source: " + ex.Source + ", ";
				result = result + Environment.NewLine + "Stack: " + ex.StackTrace;				
				WindowsEventLoggingUtilities.WriteToErrorEvent("SendEmail Exception:" + Environment.NewLine + result, FModuleName, 0);
				
				List<string> StrList = new List<string>();
				StrList.Add("SendEmail Exception:" + Environment.NewLine + result);
				StrList.Add("Error String: " + ex.ToString());
				StrList.AddRange(FEmailItem.OutputProperties());
				
				if (ex.Data != null)
				{
					StrList.Add("Start - Outputing Dictionary Values");
					foreach (System.Collections.DictionaryEntry dictionaryEntry in ex.Data)
					{
						StrList.Add(string.Format("Key: {0} with Value: {1}", dictionaryEntry.Key, dictionaryEntry.Value));
					}
					StrList.Add("End - Outputing Dictionary Values");
				}

				WindowsEventLoggingUtilities.WriteToInfoEvent("Information:" + Environment.NewLine + string.Join("", StrList.ToArray()), FModuleName, 0);
				result = 
					JsonUtilities.AddPropertyNameAndValue("status", "FAILURE") +
					JsonUtilities.AddPropertyNameAndValue("message", result, false);
			}
			return result;
		}
		
		private void WriteToOutput(string LineText)
		{
			string FileName = "EmailProcessor_" + FRunId.ToString() + ".txt";
			FileName = FWorkingDir + FileName;			
			using (StreamWriter writer = new StreamWriter(FileName, true))
			{
				writer.WriteLine("SendRTFEmail");
				writer.WriteLine(LineText);
				Console.WriteLine(LineText);
			}
		}
	}
}
