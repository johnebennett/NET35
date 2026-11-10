/*
 * Created by SharpDevelop.
 * User: johne
 * Date: 31/10/2023
 * Time: 3:27 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
//using System.IO;
using System.Collections.Generic;
using System.Text;

namespace MailKitEmailProcessor.Actions
{
	/// <summary>
	/// Description of CommonClasses.
	/// </summary>
	public class AccountDetails 
	{
		
		public string Host { get; set; }
		public int Port { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public bool BypassAuthCheck { get; set; }

		public AccountDetails()
		{
			this.Host = "";
			this.Port = 0;
			this.UserName = "";
			this.Password = "";
			this.BypassAuthCheck = false;
		}
		
		public AccountDetails(string Host,
		                      int Port,
		                      string UserName,
		                      string Password,
		                      bool BypassAuthCheck = false)
		{
			this.Host = Host;
			this.Port = Port;
			this.UserName = UserName;
			this.Password = Password;
			this.BypassAuthCheck = BypassAuthCheck;
		}
		
		public string Display()
		{
			return
				"Host: " + Host + Environment.NewLine +
				"Port: " + Port + Environment.NewLine +
				"UserName: " + UserName + Environment.NewLine +
				"Password: " + Password + Environment.NewLine;
		}
		
		public string Validate()
		{
			string Result = "";
			
			if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(Host))
			{
				Result = "Empty or invalid Host provided";
			}
			
			if (string.IsNullOrEmpty(Result) && (Port == 0))
			{
				Result = "No Port number provided";
			}
			
			if (!BypassAuthCheck && string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(UserName))
			{
				Result = "Empty or invalid UserName provided";
			}
			
			if (!BypassAuthCheck && string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(Password))
			{
				Result = "Empty or invalid Password provided";
			}
			
			return Result;
		}
		
		public string CopyFrom(AccountDetails Source)
		{
			string Result = "";
			
			try 
			{
				Host = Source.Host;
				Port = Source.Port;
				UserName = Source.UserName;
				Password = Source.Password;
				BypassAuthCheck = Source.BypassAuthCheck;
			}
			catch (Exception Ex)
			{
				Result = Ex.Message + Environment.NewLine + Ex.StackTrace;
			}			
			
			return Result;
		}
	}
	
	public class NameAndAddressItem
	{
		public string Name { get; set; }
		public string Address { get; set; }
		
		public NameAndAddressItem()
		{
			this.Name = "";
			this.Address = "";			
		}
		
		public NameAndAddressItem(string Name, string Address)
		{
			this.Name = Name;
			this.Address = Address;
		}
		
		public string Validate()
		{
			string Result = "";
			
			if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(Name))
			{
				Result = "Empty or invalid Name provided";
			}
			
			if (string.IsNullOrEmpty(Result))
			{
				Result = CommonMethods.ValidateEmailAddress(Address);
			}
			
			return Result;
		}
		
		public string Display()
		{
			return
				"Name: " + Name + Environment.NewLine +
				"Address: " + Address + Environment.NewLine;
		}
		
		public string CopyFrom(NameAndAddressItem Source)
		{
			string Result = "";
			
			try 
			{
				Name = Source.Name;
				Address = Source.Address;
			}
			catch (Exception Ex)
			{
				Result = Ex.Message + Environment.NewLine + Ex.StackTrace;
			}
			
			return Result;
		}
	}
	
	public class EmailAttachment
	{
		public string FileName { get; set; }
		
		public EmailAttachment()
		{
			this.FileName = "";
		}
		
		public EmailAttachment(string FileName)
		{
			this.FileName = FileName;
		}
		
		public string Validate()
		{
			string Result = "";
			
			if (string.IsNullOrEmpty(Result))
			{
				Result = CommonMethods.ValidateFileName(FileName);
			}
			
			return Result;
		}
		
		public string Display()
		{
			return 
				"FileName: " + FileName + Environment.NewLine;
		}
		
		public string CopyFrom(EmailAttachment Source)
		{
			string Result = "";
			
			try 
			{
				FileName = Source.FileName;
			}
			catch (Exception Ex)
			{
				Result = Ex.Message + Environment.NewLine + Ex.StackTrace;
			}
			
			return Result;
		}
	}
	
	public class EmailBodyImage
	{
		public string Identifier { get; set; }
		public string FileName { get; set; }
		
		public EmailBodyImage()
		{
			this.Identifier = "";
			this.FileName = "";
		}
		
		public EmailBodyImage(string Identifier, string FileName)
		{
			this.Identifier = Identifier;
			this.FileName = FileName;
		}
		
		public string Validate()
		{
			string Result = "";
			
			if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(Identifier))
			{
				Result = "Empty or invalid Identifier provided";				
			}
			
			if (string.IsNullOrEmpty(Result))
			{
				Result = CommonMethods.ValidateFileName(FileName);
			}
			
			return Result;
		}
		
		public string Display()
		{
			return 
				"Identifier: " + Identifier + Environment.NewLine +
				"FileName: " + FileName + Environment.NewLine;
		}
		
		public string CopyFrom(EmailBodyImage Source)
		{
			string Result = "";
			
			try 
			{
				Identifier = Source.Identifier;
				FileName = Source.FileName;
			}
			catch (Exception Ex)
			{
				Result = Ex.Message + Environment.NewLine + Ex.StackTrace;
			}
			
			return Result;
		}
	}
	
	public class SendEmailParam
	{
		public AccountDetails AccountDetails { get ; set; }
		public SenderDetailsItem Sender { get; set; }
		public List<NameAndAddressItem> Recipients { get; set; }
		public List<NameAndAddressItem> CCRecipients { get; set; }
		public List<NameAndAddressItem> BCCRecipients { get; set; }
		public List<NameAndAddressItem> ExclusionList { get; set; }
		public string Subject { get; set; }
		public string EmailBody { get; set; }
		public List<EmailBodyImage> EmailBodyImages { get; set; }
		public List<EmailAttachment> EmailAttachments { get; set; }
		
		public SendEmailParam ()
		{
			AccountDetails = new AccountDetails();
			Sender = new SenderDetailsItem();
			Recipients = new List<NameAndAddressItem>();
			CCRecipients = new List<NameAndAddressItem>();
			BCCRecipients = new List<NameAndAddressItem>();
			ExclusionList = new List<NameAndAddressItem>();
			Subject = "";
			EmailBody = "";
			EmailBodyImages = new List<EmailBodyImage>();
			EmailAttachments = new List<EmailAttachment>();
		}
		
		public string Validate()
		{
			StringBuilder Result = new StringBuilder();
			string Str = AccountDetails.Validate();
			if (!string.IsNullOrEmpty(Str))
			{
				Result.AppendLine("Account Details");
				Result.AppendLine("===============");
				Result.AppendLine(Str);
			}
			
			if (string.IsNullOrEmpty(Subject))
			{
				Result.AppendLine("No Subject provided");
			}
			
			Str = Sender.Validate();
			if (!string.IsNullOrEmpty(Str))
			{
				Result.AppendLine("Sender");
				Result.AppendLine("======");
				Result.AppendLine(Str);
			}
			
			foreach (NameAndAddressItem Item in Recipients)
			{
				Str = Item.Validate();
				if (!string.IsNullOrEmpty(Str))
				{
					Result.AppendLine(Str);
				}
			}
			
			foreach (NameAndAddressItem Item in CCRecipients)
			{
				Str = Item.Validate();
				if (!string.IsNullOrEmpty(Str))
				{
					Result.AppendLine(Str);
				}
			}
			
			foreach (NameAndAddressItem Item in BCCRecipients)
			{
				Str = Item.Validate();
				if (!string.IsNullOrEmpty(Str))
				{
					Result.AppendLine(Str);
				}
			}
			
			foreach (NameAndAddressItem Item in ExclusionList)
			{
				Str = Item.Validate();
				if (!string.IsNullOrEmpty(Str))
				{
					Result.AppendLine(Str);
				}
			}
			
			foreach (EmailBodyImage Item in EmailBodyImages)
			{
				Str = Item.Validate();
				if (!string.IsNullOrEmpty(Str))
				{
					Result.AppendLine(Str);
				}
			}
			
			foreach (EmailAttachment Item in EmailAttachments)
			{
				Str = Item.Validate();
				if (!string.IsNullOrEmpty(Str))
				{
					Result.AppendLine(Str);
				}
			}
			
			if (string.IsNullOrEmpty(EmailBody))
			{
				Result.AppendLine("No Email Body provided");
			}
			
			return Result.ToString();
		}
		
		public string Display()
		{
			StringBuilder Result = new StringBuilder();
			
			Result.AppendLine("Account Details");
			Result.AppendLine("===============");
			Result.AppendLine(AccountDetails.Display());
			Result.AppendLine("");
			
			Result.AppendLine("Subject");
			Result.AppendLine("=======");
			Result.AppendLine(Subject);
			Result.AppendLine("");
			
			Result.AppendLine("Sender");
			Result.AppendLine("======");
			Result.AppendLine(Sender.Display());
			Result.AppendLine("");
			
			Result.AppendLine("Recipients");
			Result.AppendLine("==========");
			foreach (NameAndAddressItem Item in Recipients)
			{
				Result.AppendLine(Item.Display());
			}
			Result.AppendLine("");
			
			Result.AppendLine("CC Recipients - " + CCRecipients.Count);
			Result.AppendLine("=============");
			foreach (NameAndAddressItem Item in CCRecipients)
			{
				Result.AppendLine(Item.Display());
			}
			Result.AppendLine("");
			
			Result.AppendLine("BCC Recipients - " + BCCRecipients.Count);
			Result.AppendLine("==============");
			foreach (NameAndAddressItem Item in BCCRecipients)
			{
				Result.AppendLine(Item.Display());
			}
			Result.AppendLine("");
			
			Result.AppendLine("Exclusion List - " + ExclusionList.Count);
			Result.AppendLine("==============");
			foreach (NameAndAddressItem Item in ExclusionList)
			{
				Result.AppendLine(Item.Display());
			}
			Result.AppendLine("");
			
			Result.AppendLine("Body Images");
			Result.AppendLine("===========");
			foreach (EmailBodyImage Item in EmailBodyImages)
			{
				Result.AppendLine(Item.Display());
			}
			Result.AppendLine("");
			
			Result.AppendLine("Attachments");
			Result.AppendLine("===========");
			foreach (EmailAttachment Item in EmailAttachments)
			{
				Result.AppendLine(Item.Display());
			}
			Result.AppendLine("");
			
			Result.AppendLine("Email Body");
			Result.AppendLine("==========");
			Result.AppendLine(EmailBody);			
			
			Console.WriteLine(Result.ToString());

			return Result.ToString();
		}
		
		public string CopyAccountDetailsFrom(AccountDetails Source)
		{
			string Result = "";
			
			try 
			{
				AccountDetails.Host = Source.Host;
				AccountDetails.Port = Source.Port;
				AccountDetails.UserName = Source.UserName;
				AccountDetails.Password = Source.Password;
				AccountDetails.BypassAuthCheck = Source.BypassAuthCheck;
			}
			catch (Exception Ex)
			{
				Result = Ex.Message + Environment.NewLine + Ex.StackTrace;
			}
			
			return Result;
		}
		
		public string CopySenderFrom(SenderDetailsItem Source)
		{
			string Result = "";
			
			try 
			{
				Sender.Name = Source.Name;
				Sender.Address = Source.Address;
				if (string.IsNullOrEmpty(Sender.Name))
				{
					Sender.Name = Sender.Address;
				}
				Sender.ReplyToAddress = Source.ReplyToAddress;
				if (string.IsNullOrEmpty(Sender.ReplyToAddress))
				{
					Sender.ReplyToAddress = Sender.Address;
				}
			}
			catch (Exception Ex)
			{
				Result = Ex.Message + Environment.NewLine + Ex.StackTrace;
			}
			
			return Result;
		}
		
		public string CopyRecipientsFrom(List<NameAndAddressItem> Source)
		{
			string Result = "";
			
			try 
			{
				Recipients.Clear();
				NameAndAddressItem Recipient;
				foreach(NameAndAddressItem Item in Source)
				{
					Recipient = new NameAndAddressItem();
					Recipient.Name = Item.Name;
					Recipient.Address = Item.Address;
					if (string.IsNullOrEmpty(Recipient.Name))
					{
						Recipient.Name = Recipient.Address;
					}
					Recipients.Add(Recipient);
				}
			}
			catch (Exception Ex)
			{
				Result = Ex.Message + Environment.NewLine + Ex.StackTrace;
			}
			
			return Result;
		}
		
		public string CopyCCRecipientsFrom(List<NameAndAddressItem> Source)
		{
			string Result = "";
			
			try 
			{
				CCRecipients.Clear();
				NameAndAddressItem Recipient;
				foreach(NameAndAddressItem Item in Source)
				{
					Recipient = new NameAndAddressItem();
					Recipient.Name = Item.Name;
					Recipient.Address = Item.Address;
					if (string.IsNullOrEmpty(Recipient.Name))
					{
						Recipient.Name = Recipient.Address;
					}
					CCRecipients.Add(Recipient);
				}
			}
			catch (Exception Ex)
			{
				Result = Ex.Message + Environment.NewLine + Ex.StackTrace;
			}
			
			return Result;
		}
		
		public string CopyBCCRecipientsFrom(List<NameAndAddressItem> Source)
		{
			string Result = "";
			
			try 
			{
				BCCRecipients.Clear();
				NameAndAddressItem Recipient;
				foreach(NameAndAddressItem Item in Source)
				{
					Recipient = new NameAndAddressItem();
					Recipient.Name = Item.Name;
					Recipient.Address = Item.Address;
					if (string.IsNullOrEmpty(Recipient.Name))
					{
						Recipient.Name = Recipient.Address;
					}
					BCCRecipients.Add(Recipient);
				}
			}
			catch (Exception Ex)
			{
				Result = Ex.Message + Environment.NewLine + Ex.StackTrace;
			}
			
			return Result;
		}
		
		public string CopyExclusionsFrom(List<NameAndAddressItem> Source)
		{
			string Result = "";
			
			try 
			{
				ExclusionList.Clear();
				NameAndAddressItem Recipient;
				foreach(NameAndAddressItem Item in Source)
				{
					Recipient = new NameAndAddressItem();
					Recipient.Name = Item.Name;
					Recipient.Address = Item.Address;
					if (string.IsNullOrEmpty(Recipient.Name))
					{
						Recipient.Name = Recipient.Address;
					}
					ExclusionList.Add(Recipient);
				}
			}
			catch (Exception Ex)
			{
				Result = Ex.Message + Environment.NewLine + Ex.StackTrace;
			}
			
			return Result;
		}
		
		public string CopyEmailBodyImagesFrom(List<EmailBodyImage> Source)
		{
			string Result = "";
			
			try 
			{
				EmailBodyImages.Clear();
				EmailBodyImage NewImage;
				foreach(EmailBodyImage Item in Source)
				{
					NewImage = new EmailBodyImage();
					NewImage.Identifier = Item.Identifier;
					NewImage.FileName = Item.FileName;
					EmailBodyImages.Add(NewImage);
				}
			}
			catch (Exception Ex)
			{
				Result = Ex.Message + Environment.NewLine + Ex.StackTrace;
			}
			
			return Result;
		}
		
		public string CopyEmailAttachmentsFrom(List<EmailAttachment> Source)
		{
			string Result = "";
			
			try 
			{
				EmailAttachments.Clear();
				EmailAttachment NewAttachment;
				foreach(EmailAttachment Item in Source)
				{
					NewAttachment = new EmailAttachment();
					NewAttachment.FileName = Item.FileName;
					EmailAttachments.Add(NewAttachment);
				}
			}
			catch (Exception Ex)
			{
				Result = Ex.Message + Environment.NewLine + Ex.StackTrace;
			}
			
			return Result;
		}
 
	}
	// Recovered from the production executable.
	public class SenderDetailsItem
	{
		public string Name { get; set; }

		public string Address { get; set; }

		public string ReplyToAddress { get; set; }

		public SenderDetailsItem()
		{
			Name = "";
			Address = "";
			ReplyToAddress = "";
		}

		public SenderDetailsItem(string Name, string Address, string ReplyToAddress)
		{
			this.Name = Name;
			this.Address = Address;
			this.ReplyToAddress = ReplyToAddress;
		}

		public string Validate()
		{
			string text = "";
			if (string.IsNullOrEmpty(text) && string.IsNullOrEmpty(Name))
			{
				text = "Empty or invalid Name provided";
			}
			if (string.IsNullOrEmpty(text))
			{
				text = CommonMethods.ValidateEmailAddress(Address);
			}
			if (string.IsNullOrEmpty(text))
			{
				text = CommonMethods.ValidateEmailAddress(ReplyToAddress);
			}
			return text;
		}

		public string Display()
		{
			return "Name: " + Name + Environment.NewLine + "Address: " + Address + Environment.NewLine + "ReplyTo: " + ReplyToAddress + Environment.NewLine;
		}

		public string CopyFrom(SenderDetailsItem Source)
		{
			string result = "";
			try
			{
				Name = Source.Name;
				Address = Source.Address;
				ReplyToAddress = Source.ReplyToAddress;
			}
			catch (Exception ex)
			{
				result = ex.Message + Environment.NewLine + ex.StackTrace;
			}
			return result;
		}
	}

}
