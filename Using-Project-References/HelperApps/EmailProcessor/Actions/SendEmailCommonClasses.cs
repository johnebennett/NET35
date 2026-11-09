/*
 * Created by SharpDevelop.
 * User: DELL 990
 * Date: 20/05/2019
 * Time: 5:51 PM
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

namespace EmailProcessor.Actions
{
	/// <summary>
	/// Class that represents an email recipient
	/// </summary>
	public class SendEmail_Recipient
	{
		public string Address { get; set; }
	}
	
	/// <summary>
	/// Class that represents an email attachment file
	/// </summary>
	public class SendEmail_AttachmentFile
	{
		//public string Path { get; set; }
		public string FileName { get; set; }
	}
	
	public class SendEmail_BodyImage
	{
		public string Identifier { get; set; }
		public string FileName { get; set; }
	}
	
	/// <summary>
	/// The properties that make up the common parts of 
	/// an email request
	/// </summary>
	public class SendEmail_Common
	{
		public string Host { get; set; }
		public string Port { get; set; }
		public string EmailUserName { get; set; }
		public string EmailPassword { get; set; }
		
		private string TimeoutValue = "";
		public int Timeout { get; set; }
		
		public string Load(JObject Source)
		{
			string Result = "";
			
			if (Source == null) Result = "Invalid Source for Load method";
			
			if (string.IsNullOrEmpty(Result))
			{
				try 
				{
					Host = (string)Source["host"];
					Port = (string)Source["port"];
					EmailUserName = (string)Source["emailUserName"];
					EmailPassword = (string)Source["emailPassword"];
					TimeoutValue = (string)Source["timeout"];
					Timeout = CommonUtilities.StringUtilities.StrToIntDef(TimeoutValue, -1);
				}
				catch (Exception ex)
				{
					Result = ex.Message + Environment.NewLine + ex.StackTrace;
				}
			}
			
			return Result;
		}
		
		public string Validate()
		{
			string Result = "";
			
			if (string.IsNullOrEmpty(Host)) Result = "Invalid Host provided";			
			if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(Port)) Result = "Invalid Port provided";
			if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(EmailUserName)) Result = "Invalid Email UserName provided";
			if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(EmailPassword)) Result = "Invalid Email Password provided";
			if (string.IsNullOrEmpty(Result) && (Timeout < 0)) Result = "Invalid or missing Timeout value provided";
			
			return Result;
		}
	}
	
	/// <summary>
	/// Class that represents an email item and 
	/// all of it constituent properties
	/// </summary>
	public class SendEmail_Item
	{
		public SendEmail_Common Common = new SendEmail_Common();
		
		public string Subject { get; set; }
		public string Body { get; set; }
		
		// New property
		public int TemplateId { get; set; }
		
		public string SenderAddress { get; set; }
		
		public List<SendEmail_Recipient> Recipients = new List<SendEmail_Recipient>();
		public string LoadRecipients(JArray Source)
		{
			string Result = "";
			SendEmail_Recipient recipient = null;
			
			if (Source == null) Result = "Invalid Source provided for LoadRecipients";
			
			if (string.IsNullOrEmpty(Result))
			{
				if (Source.Count > 0)
				{
					foreach (JObject Item in Source)
					{
						recipient = new SendEmail_Recipient();
						recipient.Address = (string)Item["address"];
						Recipients.Add(recipient);
					}
				}
			}
			
			return Result;
		}
		
		public List<SendEmail_Recipient> CCs = new List<SendEmail_Recipient>();
		public string LoadCCs(JArray Source)
		{
			string Result = "";
			SendEmail_Recipient recipient = null;
			
			if (Source == null) Result = "Invalid Source provided for LoadCCs";
			
			if (string.IsNullOrEmpty(Result))
			{
				if (Source.Count > 0)
				{
					foreach (JObject Item in Source)
					{
						recipient = new SendEmail_Recipient();
						recipient.Address = (string)Item["address"];
						CCs.Add(recipient);
					}
				}
			}
			
			return Result;
		}
		
		public List<SendEmail_Recipient> BCCs = new List<SendEmail_Recipient>();
		public string LoadBCCs(JArray Source)
		{
			string Result = "";
			SendEmail_Recipient recipient = null;
			
			if (Source == null) Result = "Invalid Source provided for LoadBCCs";
			
			if (string.IsNullOrEmpty(Result))
			{
				if (Source.Count > 0)
				{
					foreach (JObject Item in Source)
					{
						recipient = new SendEmail_Recipient();
						recipient.Address = (string)Item["address"];
						BCCs.Add(recipient);
					}
				}
			}
			
			return Result;
		}
		
		
		public List<SendEmail_AttachmentFile> Attachments = new List<SendEmail_AttachmentFile>();
		public string LoadAttachments(JArray Source)
		{
			string Result = "";
			SendEmail_AttachmentFile attachment = null;
			
			if (Source == null) Result = "Invalid Source provided for LoadAttachments";
			
			if (string.IsNullOrEmpty(Result))
			{
				if (Source.Count > 0)
				{
					foreach (JObject Item in Source)
					{
						attachment = new SendEmail_AttachmentFile();
						//attachment.Path = (string)Item["path"];
						attachment.FileName = (string)Item["fileName"];
						Attachments.Add(attachment);
					}
				}
			}
			
			return Result;
		}
		
		public List<SendEmail_AttachmentFile> Images = new List<SendEmail_AttachmentFile>();
		public string LoadImages(JArray Source)
		{
			string Result = "";
			SendEmail_AttachmentFile image = null;
			
			if (Source == null) Result = "Invalid Source provided for LoadImages";
			
			if (string.IsNullOrEmpty(Result))
			{
				if (Source.Count > 0)
				{
					foreach (JObject Item in Source)
					{
						image = new SendEmail_AttachmentFile();
						//image.Path = (string)Item["path"];
						image.FileName = (string)Item["fileName"];
						Attachments.Add(image);
					}
				}
			}
			
			return Result;
		}
		
		public List<SendEmail_BodyImage> BodyImages = new List<SendEmail_BodyImage>();
		public string LoadBodyImages(JArray Source)
		{
			string Result = "";
			SendEmail_BodyImage image = null;
			
			if (Source == null) Result = "Invalid Source provided for LoadImages";
			
			if (string.IsNullOrEmpty(Result))
			{
				if (Source.Count > 0)
				{
					foreach (JObject Item in Source)
					{
						image = new SendEmail_BodyImage();
						image.Identifier = (string)Item["identifier"];
						image.FileName = (string)Item["fileName"];
						BodyImages.Add(image);
					}
				}
			}
			
			return Result;
		}
		
		public string Validate(bool ValidateBody = true)
		{
			string Result = Common.Validate();
			
			if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(Subject)) Result = "Invalid Email Subject provided";
			if (string.IsNullOrEmpty(Result) && ValidateBody && string.IsNullOrEmpty(Body))
			{
				Result = "Invalid Email Body provided";
			}
			
			if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(SenderAddress)) Result = "Invalid Sender Address provided";
			if (string.IsNullOrEmpty(Result) && (Recipients.Count == 0)) Result = "At least 1 email recipient should be provided";
			
			string FileName = "";
			if (string.IsNullOrEmpty(Result) && (Attachments.Count > 0))
			{
				// check to see that the entries all exists
				foreach (SendEmail_AttachmentFile Item in Attachments)
				{
					//FileName = Path.Combine(Item.Path, Item.FileName);
					FileName = Item.FileName;
					if (!File.Exists(FileName))
					{
						Result = "Cannot find the attachment file: " + FileName;
						break;
					}
				}
			}
			
			if (string.IsNullOrEmpty(Result) && (Images.Count > 0))
			{
				// check to see that the entries all exists
				foreach (SendEmail_AttachmentFile Item in Images)
				{
					//FileName = Path.Combine(Item.Path, Item.FileName);
					FileName = Item.FileName;
					if (!File.Exists(FileName))
					{
						Result = "Cannot find the attachment file: " + FileName;
						break;
					}
				}
			}
			
			if (string.IsNullOrEmpty(Result) && (BodyImages.Count > 0))
			{
				// check to see that the entries all exists
				foreach (SendEmail_BodyImage Item in BodyImages)
				{
					FileName = Item.FileName;
					if (!File.Exists(FileName))
					{
						Result = "Cannot find the attachment file: " + FileName;
						break;
					}
					
					if (string.IsNullOrEmpty(Item.Identifier))
					{
						Result = "Each body image item must have an Identifier";
						break;
					}
				}
			}
			
			// All the other properties are optional
			
			return Result;
		}
		
		public List<string> OutputProperties()
		{
			List<string> StrList = new List<string>();
			StrList.Add("SMTP Host: " + Common.Host);
			StrList.Add("SMTP Port: " + Common.Port.ToString());
			StrList.Add("Email UserName: " + Common.EmailUserName);
			StrList.Add("Email Password: " + Common.EmailPassword);
			StrList.Add("Sender Email Address: " + SenderAddress);
			StrList.Add("Email Subject: " + Subject);
			StrList.Add("Email Body:" + Environment.NewLine + Body);

			StrList.Add("Recipient List:");
			foreach(SendEmail_Recipient Item in Recipients)
			{
				StrList.Add(Item.Address);
			}
			
			StrList.Add("CC List:");
			foreach (SendEmail_Recipient Item in CCs)
			{
				StrList.Add(Item.Address);
			}
			
			StrList.Add("Bcc List:");
			foreach (SendEmail_Recipient Item in BCCs)
			{
				StrList.Add(Item.Address);
			}
			
			StrList.Add("Body Images:");
			foreach (SendEmail_BodyImage Item in BodyImages)
			{
				StrList.Add("Identifier: " + Item.Identifier);
				StrList.Add("FileName: " + Item.FileName);
			}
			
			StrList.Add("Attachments");
			foreach (SendEmail_AttachmentFile Item in Attachments)
			{
				//StrList.Add("Attachment Path : " + Item.Path);
				StrList.Add("Attachment FileName : " + Item.FileName);
			}
			return StrList;
		}
	}
}
