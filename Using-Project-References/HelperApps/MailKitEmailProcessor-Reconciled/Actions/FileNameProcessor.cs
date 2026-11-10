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
		/*
		private AccountDetails m_AccountDetails;
		private NameAndAddressItem m_Sender;
		private List<NameAndAddressItem> m_Recipients;
		private List<NameAndAddressItem> m_CCRecipients;
		private List<NameAndAddressItem> m_BCCRecipients;
		private string m_Subject = "";
		private string m_EmailBody = "";
		private List<EmailBodyImage> m_EmailBodyImages;
		private List<EmailAttachment> m_EmailAttachments;
		*/
		
		private SendEmailParam SendEmailParam;
		
		private string m_EmailBodyContent = "";
		private string m_EmailBodyFileName = "";
		private string m_UniqueFileId = "";

		public FileNameProcessor()
		{
			/*
			m_AccountDetails = new AccountDetails();
			m_Sender = new NameAndAddressItem();
			m_Recipients = new List<NameAndAddressItem>();
			m_CCRecipients = new List<NameAndAddressItem>();
			m_BCCRecipients = new List<NameAndAddressItem>();
			m_Subject = "";
			m_EmailBody = "";
			m_EmailBodyImages = new List<EmailBodyImage>();
			m_EmailAttachments = new List<EmailAttachment>();
			*/
			
			SendEmailParam = new SendEmailParam();
			
			m_UniqueFileId = Guid.NewGuid().ToString();
			Console.WriteLine("UniqueFileId: " + m_UniqueFileId);
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
			//string Str = "";
			
			if (string.IsNullOrEmpty(Result))
			{
				try 
				{
					string FileContents = File.ReadAllText(FileName);
					/*
					Console.WriteLine("File Contents:");
					Console.WriteLine(FileContents);
					*/
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
				
				/*
				Console.WriteLine("Processing EmailBodyContent");
				Console.WriteLine("Result: " + Result);
				Console.WriteLine("Email Body Content: " + EmailBodyContent);
				*/
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
				if (string.IsNullOrEmpty(Result)) Console.WriteLine(Path + " returned OK");
				foreach (JObject Item in Token.Children())
				{
					EmailAttachment attachment = new EmailAttachment();
					attachment.FileName = JsonUtilities.GetJsonPathValueStr(Item, "$.FileName", out Result);
					if (string.IsNullOrEmpty(Result))
					{
						Console.WriteLine(attachment.FileName);
						SendEmailParam.EmailAttachments.Add(attachment);
					}
					else
					{
						Console.WriteLine(Result);
					}
				}
			}
			
			if (string.IsNullOrEmpty(Result))
			{
				Path = "$.Recipients";
				Token =  JsonUtilities.GetJsonPathValue(JsonSource, Path, out Result);
				if (string.IsNullOrEmpty(Result)) Console.WriteLine(Path + " returned OK");
				foreach (JObject Item in  Token.Children())
				{
					NameAndAddressItem Recipient = new NameAndAddressItem();
					Recipient.Name = JsonUtilities.GetJsonPathValueStr(Item, "$.Name", out Result);
					if (string.IsNullOrEmpty(Result))
					{
						Recipient.Address = JsonUtilities.GetJsonPathValueStr(Item, "$.Address", out Result);
						Console.WriteLine("Recipient-Name: " + Recipient.Name);
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
				if (string.IsNullOrEmpty(Result)) Console.WriteLine(Path + " returned OK");
				if (Token != null)
				{
					foreach (JObject Item in  Token.Children())
					{
						NameAndAddressItem Recipient = new NameAndAddressItem();
						Recipient.Name = JsonUtilities.GetJsonPathValueStr(Item, "$.Name", out Result);
						if (string.IsNullOrEmpty(Result))
						{
							Recipient.Address = JsonUtilities.GetJsonPathValueStr(Item, "$.Address", out Result);
							Console.WriteLine("Recipient-Name: " + Recipient.Name);
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
				if (string.IsNullOrEmpty(Result)) Console.WriteLine(Path + " returned OK");
				if (Token != null) 
				{
					foreach (JObject Item in  Token.Children())
					{
						NameAndAddressItem Recipient = new NameAndAddressItem();
						Recipient.Name = JsonUtilities.GetJsonPathValueStr(Item, "$.Name", out Result);
						if (string.IsNullOrEmpty(Result))
						{
							Recipient.Address = JsonUtilities.GetJsonPathValueStr(Item, "$.Address", out Result);
							Console.WriteLine("Recipient-Name: " + Recipient.Name);
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

			CommonMethods.WriteToOutput(m_UniqueFileId, "Body Before Processing");
			CommonMethods.WriteToOutput(m_UniqueFileId, SendEmailParam.EmailBody);
			
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

			CommonMethods.WriteToOutput(m_UniqueFileId, "Body After Processing");
			CommonMethods.WriteToOutput(m_UniqueFileId, SendEmailParam.EmailBody);
			
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
				/*
				Result =
					EmailSender.Send(
							SendEmailParam.AccountDetails, SendEmailParam.Sender, 
							SendEmailParam.Recipients, SendEmailParam.CCRecipients, SendEmailParam.BCCRecipients,
							SendEmailParam.Subject, SendEmailParam.EmailBody, SendEmailParam.EmailBodyImages, 
							SendEmailParam.EmailAttachments);
				*/
				Result = EmailSender.Send(SendEmailParam, 0);
			}
			
			return Result;
		}
		
		/*
		public void Display()
		{
			StringBuilder sb = new StringBuilder();
			sb.Clear();
			sb.AppendLine("Account Details");
			sb.AppendLine("===============");
			sb.Append(m_AccountDetails.Display());
			sb.AppendLine("Sender Details");
			sb.AppendLine("==============");
			sb.Append(m_Sender.Display());
			sb.AppendLine("Recipient Details");
			sb.AppendLine("=================");
			
			foreach(NameAndAddressItem Item in m_Recipients)
			{
				sb.Append(Item.Display());
			}
			
			sb.AppendLine("Subject: " + m_Subject);
			
			if (m_EmailAttachments.Count == 0)
			{
				sb.AppendLine("NO ATTACHMENTS");
			}
			else
			{
				sb.AppendLine("Attachments");
				sb.AppendLine("===========");
			}
			
			foreach(EmailAttachment Item in m_EmailAttachments)
			{
				sb.Append(Item.Display());
			}
			
			sb.AppendLine("EmailBodyContent");
			sb.AppendLine("================");
			sb.AppendLine(m_EmailBodyContent);
			sb.AppendLine("Email Body FileName");
			sb.AppendLine("===================");
			sb.AppendLine(m_EmailBodyFileName);
			
			sb.AppendLine("Email Body");
			sb.AppendLine("==========");
			sb.AppendLine(m_EmailBody);
			
			if (m_EmailBodyImages.Count == 0)
			{
				sb.AppendLine("NO IMAGES");
			}
			else
			{
				sb.AppendLine("Images");
				sb.AppendLine("======");
			}
			
			foreach (EmailBodyImage Item in m_EmailBodyImages)
			{
				sb.Append(Item.Display());
			}
			
		private List<EmailBodyImage> m_EmailBodyImages;
		private List<EmailAttachment> m_EmailAttachments;
		
		private string m_EmailBodyContent = "";
		private string m_EmailBodyFileName = "";
		private string m_UniqueFileId = "";
		Console.WriteLine(sb.ToString());
		}
		*/
	}
}
