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
		private AccountDetails m_AccountDetails;
		private NameAndAddressItem m_Sender;
		private List<NameAndAddressItem> m_Recipients;
		private List<NameAndAddressItem> m_CCRecipients;
		private List<NameAndAddressItem> m_BCCRecipients;
		private string m_Subject = "";
		private string m_EmailBody = "";
		private List<EmailBodyImage> m_EmailBodyImages;
		private List<EmailAttachment> m_EmailAttachments;
		
		private string m_EmailBodyContent = "";
		private string m_EmailBodyFileName = "";
		private string m_UniqueFileId = "";

		public FileNameProcessor()
		{
			m_AccountDetails = new AccountDetails();
			m_Sender = new NameAndAddressItem();
			m_Recipients = new List<NameAndAddressItem>();
			m_CCRecipients = new List<NameAndAddressItem>();
			m_BCCRecipients = new List<NameAndAddressItem>();
			m_Subject = "";
			m_EmailBody = "";
			m_EmailBodyImages = new List<EmailBodyImage>();
			m_EmailAttachments = new List<EmailAttachment>();
			
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
			string Str = "";
			
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
				m_AccountDetails.Host = CommonMethods.GetJsonPathValueAsString(JsonSource, Path, "SmtpServer", out Result);
			}
			
			if (string.IsNullOrEmpty(Result)) 
			{
				Path = "$.SmtpPort";
				//Str = JsonUtilities.GetJsonPathValueStr(JsonSource, Path, out Result);
				//Str = GetJsonPathValueAsString(JsonSource, Path, "SmtpPort", out Result);
				m_AccountDetails.Port = CommonMethods.GetJsonPathValueAsInt32(JsonSource, Path, "SmtpPort", out Result);
			}
			
			if (string.IsNullOrEmpty(Result))
			{
				Path = "$.SmtpUserName";
				m_AccountDetails.UserName = CommonMethods.GetJsonPathValueAsString(JsonSource, Path, "Email User Name", out Result);
			}
			
			if (string.IsNullOrEmpty(Result))
			{
				Path = "$.SmtpPassword";
				m_AccountDetails.Password = CommonMethods.GetJsonPathValueAsString(JsonSource, Path, "Email Password", out Result);
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
				m_Subject = CommonMethods.GetJsonPathValueAsString(JsonSource, Path, "Subject", out Result);
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
				m_Sender.Name = CommonMethods.GetJsonPathValueAsString(JsonSource, Path, "Sender Name", out Result);
			}
			
			if (string.IsNullOrEmpty(Result))
			{
				Path = "$.FromAddress";
				m_Sender.Address = CommonMethods.GetJsonPathValueAsString(JsonSource, Path, "From Address", out Result);
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
						m_EmailAttachments.Add(attachment);
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
						m_Recipients.Add(Recipient);
					}
				}
			}

			if (string.IsNullOrEmpty(Result))
			{
				Path = "$.CCRecipients";
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
						m_CCRecipients.Add(Recipient);
					}
				}
			}
			
			if (string.IsNullOrEmpty(Result))
			{
				Path = "$.BCCRecipients";
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
						m_BCCRecipients.Add(Recipient);
					}
				}
			}

			if (string.IsNullOrEmpty(Result) && (!string.IsNullOrEmpty(m_EmailBodyContent)))
			{
				m_EmailBody = m_EmailBodyContent;
			}
			
			if (string.IsNullOrEmpty(Result) && (!string.IsNullOrEmpty(m_EmailBodyFileName)))
			{
				Result = CommonMethods.ValidateFileName(m_EmailBodyFileName);
				if (string.IsNullOrEmpty(Result))
				{
					m_EmailBody = File.ReadAllText(m_EmailBodyFileName);
				}
			}

			CommonMethods.WriteToOutput(m_UniqueFileId, "Body Before Processing");
			CommonMethods.WriteToOutput(m_UniqueFileId, m_EmailBody);
			
			if (string.IsNullOrEmpty(Result) && (!string.IsNullOrEmpty(m_EmailBody)))
			{
				Result = CommonMethods.ProcessEmailBody(ref m_EmailBody, ref m_EmailBodyImages);
			}			

			CommonMethods.WriteToOutput(m_UniqueFileId, "Body After Processing");
			CommonMethods.WriteToOutput(m_UniqueFileId, m_EmailBody);
			
			CommonMethods.DisplayEmailItems(
				m_AccountDetails, m_Sender, m_Recipients, m_Subject, m_EmailBody,
				m_EmailBodyImages, m_EmailAttachments, m_EmailBodyContent,
				m_EmailBodyFileName);
			
			if (string.IsNullOrEmpty(Result))
			{
				SendEmail EmailSender = new SendEmail();
				Result =
					EmailSender.Send(
							m_AccountDetails, m_Sender, 
							m_Recipients, m_CCRecipients, m_BCCRecipients,
							m_Subject, m_EmailBody, m_EmailBodyImages, 
							m_EmailAttachments);
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
