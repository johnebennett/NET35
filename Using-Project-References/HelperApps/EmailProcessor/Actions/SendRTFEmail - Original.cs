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
	public class SendRTFEmail
	{
		private string FModuleName = "EmailAction";
		private string FRunId = "";
		private int FRunIdValue = 0;
		private JObject DataObject = null;
		private string FRTFFileName = "";
		private string FHtmlFileName = "";
		private SendEmail_Item FEmailItem = new SendEmail_Item();

		public SendRTFEmail()
		{
		}
		
		public static string Execute(JObject SourceDataObject,
		                             string RTFFileName,
		                             out string ErrorMsg)
		{
			string Method = "Execute";
			Console.WriteLine("Inside - " + Method);
			string Result = "";
			ErrorMsg = "";

			SendRTFEmail inst = new SendRTFEmail();
			inst.DataObject = SourceDataObject;
			if (inst.DataObject == null)
			{
				ErrorMsg = "The provided DataObject is NULL";
				return "";
			}
			inst.FRTFFileName = RTFFileName;
			if (string.IsNullOrEmpty(inst.FRTFFileName))
			{
				ErrorMsg = "The provided eMail body filename is missing or empty.";
				return "";
			}
			
			if (!File.Exists(inst.FRTFFileName))
			{
				ErrorMsg = "The provided eMail body filename does not exist.";
				return "";
			}
			//if (!inst.Setup(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg)) return "";
			
			//inst.LogOnInsideMethod(Method);
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
				//inst.LogOnLeavingMethod(Method);
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
			
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " + Method, FModuleName, 0);
			//LogOnInsideMethod(Method);
			try 
			{
				FRunId = (string)DataObject["runId"];
				FRunIdValue = StringUtilities.StrToIntDef(FRunId, -1);
				
				if (string.IsNullOrEmpty(Result))
				{
					jObj = (DataObject["common"] as JObject);				
					Result = FEmailItem.Common.Load(jObj);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					FEmailItem.Subject = (string)DataObject["emailSubject"];
					//FEmailItem.Body = (string)DataObject["emailBody"];
					//FEmailItem.Body = EmailBody;
					FEmailItem.SenderAddress = (string)DataObject["senderAddress"];
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
				//Result = LogOnException(Method, Ex, "");
				Result = Ex.Message + Environment.NewLine + Ex.InnerException;				
			}
			finally
			{
				//LogOnLeavingMethod(Method);
			}
			return Result;
		}
		
		private string Validate()
		{
			string Method = "Validate";
			string Result = "";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " +Method, FModuleName, 0);
			//LogOnInsideMethod(Method);
			try 
			{
				if (string.IsNullOrEmpty(Result) && (FRunIdValue < 0)) Result = "Invalid RunId provided";
				if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(FRTFFileName)) Result = "Invalid or missing RTF FileName provided.";
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = FEmailItem.Validate(ValidateBody: false);
				}				

				if (!string.IsNullOrEmpty(Result))
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent("Validation Failed: " + Result, FModuleName, 0);
					Result =
						JsonUtilities.StartJson() +
						JsonUtilities.AddPropertyNameAndValue("Error", "Validation Failed") +
						JsonUtilities.AddPropertyNameAndValue("Result", Result, false) +
						JsonUtilities.EndJson();
				}
			}
			catch (Exception Ex)
			{
				//Result = LogOnException(Method, Ex, "");
				Result = Ex.Message + Environment.NewLine + Ex.InnerException;				
			}
			finally
			{
				//LogOnLeavingMethod(Method);
			}
			
			return Result;
		}
		
		private string Output()
		{
			string Method = "Output";
			string Result = "";
			string ErrorMsg = "";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " +Method, FModuleName, 0);
			//LogOnInsideMethod(Method);
			try 
			{
				Result = BuildOutput(out ErrorMsg);
				if (!string.IsNullOrEmpty(ErrorMsg))
				{
					//Result = LogOnProcessingError(Method, ErrorMsg, "");
					//Result = LogOnProcessingError(Method, ErrorMsg, "");
				}
			}
			catch (Exception Ex)
			{
				//Result = LogOnException(Method, Ex, "");
				Result = Ex.Message + Environment.NewLine + Ex.InnerException;				
			}
			finally
			{
				//LogOnLeavingMethod(Method);
			}
			WindowsEventLoggingUtilities.WriteToInfoEvent("Leaving: " +Method, FModuleName, 0);
			
			return Result;
		}
		
		private string BuildOutput(out string ErrorMsg)
		{
			ErrorMsg = "";
			string Result = "";
			string Method = "BuildOutput";
			Console.WriteLine("Inside - " + Method);
			
			List<string> ErrorList = new List<string>();
			
			//LogOnInsideMethod(Method);
			try
			{
				WindowsEventLoggingUtilities.WriteToInfoEvent("Inside BuildOutput", FModuleName, 0);
				
				StringBuilder entries = new StringBuilder();
				entries.AppendLine(JsonUtilities.StartJson());
				
				RichTextBox richText = new RichTextBox();
				richText.Visible = false;
				richText.LoadFile(FRTFFileName);
				richText.SaveFile(FRTFFileName);
				
				/* Convert the RTF to HTML */
				/*
				TMSWordHelpers msWordHelpers = new TMSWordHelpers();
				Console.WriteLine("Word Version:" + msWordHelpers.WordVersion());
				int WordResult = msWordHelpers.RTFDocToHTMLFiltered(FRTFFileName, FRTFFileName + ".html");
				if (WordResult < 0) Result = msWordHelpers.ErrorDescription(WordResult);
				else
				{
					Console.WriteLine("Check the file: " + FRTFFileName + ".html");
					FEmailItem.Body = File.ReadAllText(FRTFFileName + ".html");
				}
				*/
				
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
				//Result = LogOnException(Method, Ex, "");
				Result = Ex.Message + Environment.NewLine + Ex.InnerException;				
			}
			finally
			{
				//LogOnLeavingMethod(Method);
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
			string result;
			try
			{
				using (MailMessage mailMessage = new MailMessage())
				{
					mailMessage.From = new MailAddress(FEmailItem.SenderAddress);
					if (FEmailItem.Recipients.Count > 0)
					{
						foreach (SendEmail_Recipient Item in FEmailItem.Recipients)
						{
							mailMessage.To.Add(Item.Address);
							//mailMessage.To.Add("johnebennett_au@yahoo.com.au");
						}
					}					

					mailMessage.Subject = FEmailItem.Subject;
					//mailMessage.IsBodyHtml = true;
					//mailMessage.BodyEncoding = System.Text.Encoding.  
					mailMessage.Body = FEmailItem.Body;

					if (FEmailItem.CCs.Count > 0)
					{
						foreach (SendEmail_Recipient Item in FEmailItem.CCs)
						{
							mailMessage.CC.Add(Item.Address);
							//mailMessage.CC.Add("jebemf_au@yahoo.com.au");
						}
					}

					if (FEmailItem.BCCs.Count > 0)
					{
						foreach (SendEmail_Recipient Item in FEmailItem.BCCs)
						{
							mailMessage.Bcc.Add(Item.Address);
							//mailMessage.Bcc.Add("johnebennett_au@yahoo.com.au");
						}
					}
					
					string FileName = "";
					foreach (SendEmail_BodyImage Item in FEmailItem.BodyImages)
					{
						Attachment attachment = new Attachment(Item.FileName);
						attachment.ContentId = Item.Identifier;
						mailMessage.Attachments.Add(attachment);
					}
					
					foreach (SendEmail_AttachmentFile Item in FEmailItem.Attachments)
					{
						//FileName = Path.Combine(Item.Path, Item.FileName);
						FileName = Item.FileName;
						mailMessage.Attachments.Add(new Attachment(FileName));
					}
					
					//SmtpClient smtpClient = new SmtpClient(FEmailItem.Common.Host);
					SmtpClient smtpClient = new SmtpClient();
					smtpClient.Host = FEmailItem.Common.Host;
					smtpClient.Port = CommonUtilities.StringUtilities.StrToIntDef(FEmailItem.Common.Port, 0);
					smtpClient.Credentials = new System.Net.NetworkCredential(FEmailItem.Common.EmailUserName, FEmailItem.Common.EmailPassword);
					string str = Environment.CurrentDirectory + Path.DirectorySeparatorChar.ToString();
					if (FEmailItem.Common.Timeout > 0)
					{
						smtpClient.Timeout = FEmailItem.Common.Timeout * 1000;
						WindowsEventLoggingUtilities.WriteToInfoEvent("Setting Smtp Timeout to " + FEmailItem.Common.Timeout.ToString(), FModuleName, 0);
					}
					Console.WriteLine("Before smtpClient.Send");
					smtpClient.Send(mailMessage);
					Console.WriteLine("After smtpClient.Send");
				}
				result = 
					JsonUtilities.AddPropertyNameAndValue("status", "SUCCESS") +
					JsonUtilities.AddPropertyNameAndValue("message", "", false);
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
	}
}
