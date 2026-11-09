/*
 * Created by SharpDevelop.
 * User: johne
 * Date: 09/11/2023
 * Time: 12:00 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using MimeKit.Utils;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Collections.Generic;
using CommonUtilities;
using System.Text;

namespace MailKitEmailProcessor.Actions
{
	/// <summary>
	/// Description of FileNameProcessor.
	/// </summary>
	public class FileNameProcessor
	{
		
		private SendEmailParam SendEmailParam;
		
		private string m_EmailBodyContent = "";
		private string m_EmailBodyFileName = "";
		private string m_UniqueFileId = "";

		public FileNameProcessor()
		{
			SendEmailParam = new SendEmailParam();
			
			m_UniqueFileId = Guid.NewGuid().ToString();
			LoggingMethods.WriteToConsole("UniqueFileId: " + m_UniqueFileId);
		}
		
		public string Execute(string FileName)
		{
			string Result = "";
			
			string UniqueId = MimeUtils.GenerateMessageId();
			
			if (string.IsNullOrEmpty(Result))
			{
				Result = CommonMethods.ValidateFileName(FileName);
			}
			
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
				Path = "$.SmtpServer";
				//SmtpServer = JsonUtilities.GetJsonPathValueStr(JsonSource, Path, out Result);
				SendEmailParam.AccountDetails.Host = CommonMethods.GetJsonPathValueAsString(JsonSource, Path, "SmtpServer", out Result);
			}
			
			if (string.IsNullOrEmpty(Result)) 
			{
				Path = "$.SmtpPort";
				//Str = JsonUtilities.GetJsonPathValueStr(JsonSource, Path, out Result);
				//Str = GetJsonPathValueAsString(JsonSource, Path, "SmtpPort", out Result);
				SendEmailParam.AccountDetails.Port = CommonMethods.GetJsonPathValueAsInt32(JsonSource, Path, "SmtpPort", out Result);
			}
			
			if (string.IsNullOrEmpty(Result))
			{
				Path = "$.SmtpUserName";
				SendEmailParam.AccountDetails.UserName = CommonMethods.GetJsonPathValueAsString(JsonSource, Path, "Email User Name", out Result);
			}
			
			if (string.IsNullOrEmpty(Result))
			{
				Path = "$.SmtpPassword";
				SendEmailParam.AccountDetails.Password = CommonMethods.GetJsonPathValueAsString(JsonSource, Path, "Email Password", out Result);
			}
			
			/*
			if (string.IsNullOrEmpty(Result))
			{
				Path = "$.UseSSL";
				UseSSL = GetJsonPathValueAsBoolean(JsonSource, Path, "Use SSL", out Result);
			}
			
			if (string.IsNullOrEmpty(Result))
			{
				Path = "$.UseTLS";
				UseTLS = GetJsonPathValueAsBoolean(JsonSource, Path, "Use TLS", out Result);
			}
			*/
			
			if (string.IsNullOrEmpty(Result))
			{
				Path = "$.Subject";
				SendEmailParam.Subject = CommonMethods.GetJsonPathValueAsString(JsonSource, Path, "Subject", out Result);
			}
			
			if (string.IsNullOrEmpty(Result))
			{
				Path = "$.EmailBody.Content";
				m_EmailBodyContent = CommonMethods.GetJsonPathValueAsString(JsonSource, Path, "Email Body Content", out Result);
			}
			
			if (string.IsNullOrEmpty(Result))
			{
				Path = "$.EmailBody.FileName";
				m_EmailBodyFileName = CommonMethods.GetJsonPathValueAsString(JsonSource, Path, "Email Body FileName", out Result);
			}
			
			if (string.IsNullOrEmpty(Result))
			{
				Path = "$.SenderName";
				SendEmailParam.Sender.Name = CommonMethods.GetJsonPathValueAsString(JsonSource, Path, "Sender Name", out Result);
			}
			
			if (string.IsNullOrEmpty(Result))
			{
				Path = "$.FromAddress";
				SendEmailParam.Sender.Address = CommonMethods.GetJsonPathValueAsString(JsonSource, Path, "From Address", out Result);
			}
			
			/*
			if (string.IsNullOrEmpty(Result) && !string.IsNullOrEmpty(SenderName) && !string.IsNullOrEmpty(FromAddress))
			{
				Sender.Add(new EmailAddressItem(SenderName, FromAddress));
			}
			*/
			
			JToken Token = null;			
			if (string.IsNullOrEmpty(Result))
			{
				Path = "$.Attachments";
				Token =  JsonUtilities.GetJsonPathValue(JsonSource, Path, out Result);
				if (string.IsNullOrEmpty(Result)) LoggingMethods.WriteToConsole(Path + " returned OK");
				foreach (JObject Item in Token.Children())
				{
					EmailAttachment attachment = new EmailAttachment();
					attachment.FileName = JsonUtilities.GetJsonPathValueStr(Item, "$.FileName", out Result);
					if (string.IsNullOrEmpty(Result))
					{
						LoggingMethods.WriteToConsole(attachment.FileName);
						SendEmailParam.EmailAttachments.Add(attachment);
					}
					else
					{
						LoggingMethods.WriteToConsole(Result);
					}
				}
			}
			
			if (string.IsNullOrEmpty(Result))
			{
				Path = "$.Recipients";
				Token =  JsonUtilities.GetJsonPathValue(JsonSource, Path, out Result);
				if (string.IsNullOrEmpty(Result)) LoggingMethods.WriteToConsole(Path + " returned OK");
				foreach (JObject Item in  Token.Children())
				{
					NameAndAddressItem Recipient = new NameAndAddressItem();
					Recipient.Name = JsonUtilities.GetJsonPathValueStr(Item, "$.Name", out Result);
					if (string.IsNullOrEmpty(Result))
					{
						Recipient.Address = JsonUtilities.GetJsonPathValueStr(Item, "$.Address", out Result);
						LoggingMethods.WriteToConsole("Recipient-Name: " + Recipient.Name);
					}
					if (string.IsNullOrEmpty(Result))
					{
						//m_Recipients.Add(new  EmailAddressItem(RecipientName, ToAddress));
						SendEmailParam.Recipients.Add(Recipient);
					}
				}
			}

			if (string.IsNullOrEmpty(Result))
			{
				Path = "$.CCRecipients";
				Token =  JsonUtilities.GetJsonPathValue(JsonSource, Path, out Result);
				if (string.IsNullOrEmpty(Result)) LoggingMethods.WriteToConsole(Path + " returned OK");
				if (Token != null)
				{
					foreach (JObject Item in  Token.Children())
					{
						NameAndAddressItem Recipient = new NameAndAddressItem();
						Recipient.Name = JsonUtilities.GetJsonPathValueStr(Item, "$.Name", out Result);
						if (string.IsNullOrEmpty(Result))
						{
							Recipient.Address = JsonUtilities.GetJsonPathValueStr(Item, "$.Address", out Result);
							LoggingMethods.WriteToConsole("Recipient-Name: " + Recipient.Name);
						}
						if (string.IsNullOrEmpty(Result))
						{
							//m_Recipients.Add(new  EmailAddressItem(RecipientName, ToAddress));
							SendEmailParam.CCRecipients.Add(Recipient);
						}
					}
				}
			}
			
			if (string.IsNullOrEmpty(Result))
			{
				Path = "$.BCCRecipients";
				Token =  JsonUtilities.GetJsonPathValue(JsonSource, Path, out Result);
				if (string.IsNullOrEmpty(Result)) LoggingMethods.WriteToConsole(Path + " returned OK");
				if (Token != null) 
				{
					foreach (JObject Item in  Token.Children())
					{
						NameAndAddressItem Recipient = new NameAndAddressItem();
						Recipient.Name = JsonUtilities.GetJsonPathValueStr(Item, "$.Name", out Result);
						if (string.IsNullOrEmpty(Result))
						{
							Recipient.Address = JsonUtilities.GetJsonPathValueStr(Item, "$.Address", out Result);
							LoggingMethods.WriteToConsole("Recipient-Name: " + Recipient.Name);
						}
						if (string.IsNullOrEmpty(Result))
						{
							//m_Recipients.Add(new  EmailAddressItem(RecipientName, ToAddress));
							SendEmailParam.BCCRecipients.Add(Recipient);
						}
					}
				}
			}

			if (string.IsNullOrEmpty(Result) && (!string.IsNullOrEmpty(m_EmailBodyContent)))
			{
				SendEmailParam.EmailBody = m_EmailBodyContent;
			}
			
			if (string.IsNullOrEmpty(Result) && (!string.IsNullOrEmpty(m_EmailBodyFileName)))
			{
				Result = CommonMethods.ValidateFileName(m_EmailBodyFileName);
				if (string.IsNullOrEmpty(Result))
				{
					SendEmailParam.EmailBody = File.ReadAllText(m_EmailBodyFileName);
				}
			}

			LoggingMethods.WriteToOutput(m_UniqueFileId, "Body Before Processing");
			LoggingMethods.WriteToOutput(m_UniqueFileId, SendEmailParam.EmailBody);
			
			string StrEmailBody = SendEmailParam.EmailBody;
			List<EmailBodyImage> bodyImages = new List<EmailBodyImage>();
			
			if (string.IsNullOrEmpty(Result) && (!string.IsNullOrEmpty(SendEmailParam.EmailBody)))
			{
				//Result = CommonMethods.ProcessEmailBody(ref SendEmailParam.EmailBody, ref SendEmailParam.EmailBodyImages);
				Result = 
					CommonMethods.ProcessEmailBody(ref StrEmailBody, ref bodyImages);
				LoggingMethods.WriteToConsole("After ProcessEmailBody Result:");
				LoggingMethods.WriteToConsole(Result);
				if (string.IsNullOrEmpty(Result))
				{
					SendEmailParam.EmailBodyImages = bodyImages;
					SendEmailParam.EmailBody = StrEmailBody;
				}
			}			

			LoggingMethods.WriteToOutput(m_UniqueFileId, "Body After Processing");
			LoggingMethods.WriteToOutput(m_UniqueFileId, SendEmailParam.EmailBody);
			
			/*
			CommonMethods.DisplayEmailItems(
				SendEmailParam.AccountDetails, SendEmailParam.Sender, SendEmailParam.Recipients, 
				SendEmailParam.Subject, SendEmailParam.EmailBody,
				SendEmailParam.EmailBodyImages, SendEmailParam.EmailAttachments, m_EmailBodyContent,
				m_EmailBodyFileName);
			*/
			CommonMethods.DisplayEmailItems(SendEmailParam, m_EmailBodyContent, m_EmailBodyFileName);
			
			if (string.IsNullOrEmpty(Result))
			{
				SendEmail EmailSender = new SendEmail();
				Result = EmailSender.Send(SendEmailParam, 0);
			}
			
			return Result;
		}
	}
}
