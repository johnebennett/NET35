/*
 * Created by SharpDevelop.
 * User: johne
 * Date: 30/10/2023
 * Time: 6:59 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.IO;
using CommonUtilities;

namespace MailKitEmailProcessor.Actions
{
	public class EmailAddressItem
	{
		public EmailAddressItem() { }
		
		public EmailAddressItem(string Name, string Address)
		{
			this.Name = Name;
			this.Address = Address;
		}
		
		public string Name { get; set; }
		public string Address { get; set; }
	}
	
	public class BodyImageItem
	{
		public string Identifier { get; set; }
		public string FileName { get; set; }
	}
	
	public class AttachmentFileItem
	{
		public string FileName { get; set; }
	}

	/// <summary>
	/// Description of EmailItems.
	/// </summary>
	public class EmailItems
	{
		public EmailItems()
		{
			this.RunId = 0;
			this.SmtpServer = "server";
			this.SmtpPort = 999;
			this.EmailUserName = "Hello";
			this.EmailPassword = "There";
			this.UseSSL = false;
			this.UseTLS = false;
			this.FromAddress = "From";
			this.ToAddress= "To";
			this.Subject = "Subject";
			this.SenderName = "SenderName";
			this.RecipientName = "RecipientName";
			this.EmailBodyContent = "No Content";
			this.EmailBodyFileName = "No FileName";
		}
		
		public int RunId { get; set; }
		public string SmtpServer { get; set; }
		public int SmtpPort { get; set; }
		public string EmailUserName { get; set; }
		public string EmailPassword { get; set; }
		public bool UseSSL { get; set; }
		public bool UseTLS { get; set; }
		
		public string Subject { get; set; }

		public string SenderName { get; set; }
		public string ToAddress { get; set; }

		public string RecipientName { get; set; }
		public string FromAddress { get; set; }
		
		public List<EmailAddressItem> Recipients = new List<EmailAddressItem>();
		public List<EmailAddressItem> Sender = new List<EmailAddressItem>();
		public List<AttachmentFileItem> Attachments = new List<AttachmentFileItem>();
		
		public string EmailBodyContent { get; set; }
		public string EmailBodyFileName { get; set; }
		
		public string LoadItems(string[] args)
		{
			string Result = "";
			
			if (args.Length == 0)
			{
				Result =
					"There must be at least one command line parameter";
			}
			
			Console.WriteLine("Command Line Args Count: " + args.Length);
			
			if (string.IsNullOrEmpty(Result) && (args.Length == 1))
			{
				// The arg could either be a RunId or the name of the config file to use
				// Assume the arg is the RunId but first check if it is a number
				string Arg = args[0];
				Console.WriteLine("Arg: " + Arg);
				int TempId = 0;
				bool ParseResult = false;
				ParseResult = int.TryParse(Arg, out TempId);
				if (!ParseResult) { Result = LoadFromConfigFile(Arg); return Result;}
				else RunId = TempId;
			}

			if (string.IsNullOrEmpty(Result) && (args.Length > 1))
			{
				this.RunId = 0;
				
				int MaxArgs = args.Length;
				string Item = "";

	            for (int Index = 1; Index <= MaxArgs; Index++)
	            {
	                Item = args[args.Length - Index].ToUpper();
	                Item = args[args.Length - Index];
	                CommonMethods.WriteToOutput("Processing Item: " + Item);
	                if (Item.StartsWith(@"/SmtpServer:"))
	                {
	                	Item = Item.Replace(@"/SmtpServer:", "");
	                	this.SmtpServer = Item;
	                }
	                else if (Item.StartsWith(@"/SmtpPort:"))
	                {
	                	Item = Item.Replace(@"/SmtpPort:", "");
	                	CommonMethods.WriteToOutput("SmtpPort value: " + Item);
	                	this.SmtpPort = Convert.ToInt32(Item);
	                }
	                else if (Item.StartsWith(@"/EmailUserName:"))
	                {
	                	Item = Item.Replace(@"/EmailUserName:", "");
	                	this.EmailUserName = Item;
	                }
	                else if (Item.StartsWith(@"/EmailPassword:") || Item.StartsWith(@"/EmailPwd:"))
	                {
	                	Item = Item.Replace(@"/EmailPassword:", "");
	                	Item = Item.Replace(@"/EmailPwd:", "");
	                	this.EmailPassword = Item;
	                }
	                else if (Item.StartsWith(@"/UseSSL:"))
	                {
	                	Item = Item.Replace(@"/UseSSL:", "");
	                	this.UseSSL = Convert.ToBoolean(Item);
	                }
	                else if (Item.StartsWith(@"/UseTLS:"))
	                {
	                	Item = Item.Replace(@"/UseTLS:", "");
	                	this.UseTLS = Convert.ToBoolean(Item);
	                }
	                else if (Item.StartsWith(@"/FromAddress:"))
	                {
	                	Item = Item.Replace(@"/FromAddress:", "");
	                	this.FromAddress = Item;
	                }
	                else if (Item.StartsWith(@"/ToAddress:"))
	                {
	                	Item = Item.Replace(@"/ToAddress:", "");
	                	this.ToAddress = Item;
	                }
	                else if (Item.StartsWith(@"/Subject:"))
	                {
	                	Item = Item.Replace(@"/Subject:", "");
	                	this.Subject = Item;
	                }
	                else if (Item.StartsWith(@"/SenderName:"))
	                {
	                	Item = Item.Replace(@"/SenderName:", "");
	                	this.SenderName = Item;
	                }
	                else if (Item.StartsWith(@"/RecipientName:"))
	                {
	                	Item = Item.Replace(@"/RecipientName:", "");
	                	this.RecipientName = Item;
	                }
	                
	                if (!string.IsNullOrEmpty(RecipientName) && !string.IsNullOrEmpty(ToAddress))
	                {
	                	Console.WriteLine("Processing Recipient(s)");
	                	Console.WriteLine(RecipientName);
	                	Console.WriteLine(ToAddress);
	                	Recipients.Add(new EmailAddressItem(RecipientName, ToAddress));
	                	RecipientName = "";
	                	ToAddress = "";
	                }
	                
	                if (!string.IsNullOrEmpty(SenderName) && !string.IsNullOrEmpty(FromAddress))
	                {
	                	Sender.Add(new EmailAddressItem(SenderName, FromAddress));
	                	SenderName = "";
	                	FromAddress = "";
	                }		
				}
			}		
			
			return Result;
		}
		
		public string LoadFromConfigFile(string Arg)
		{
			string Result = "";
			string FileName = Arg;
			FileName = FileName.Replace(@"/FromFile:", "");
			Console.WriteLine("Inside LoadFromConfigFile");
			Console.WriteLine("FileName: " + FileName);
			
			if (!File.Exists(FileName))
			{
				Result = "The FromFile: " + FileName + " does not exist";
			}
			
			JObject JsonSource = null;
			string Path = "";
			
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
				SmtpServer = GetJsonPathValueAsString(JsonSource, Path, "SmtpServer", out Result);
			}
			
			if (string.IsNullOrEmpty(Result)) 
			{
				Path = "$.SmtpPort";
				//Str = JsonUtilities.GetJsonPathValueStr(JsonSource, Path, out Result);
				//Str = GetJsonPathValueAsString(JsonSource, Path, "SmtpPort", out Result);
				SmtpPort = GetJsonPathValueAsInt32(JsonSource, Path, "SmtpPort", out Result);
			}
			
			if (string.IsNullOrEmpty(Result))
			{
				Path = "$.SmtpUserName";
				EmailUserName = GetJsonPathValueAsString(JsonSource, Path, "Email User Name", out Result);
			}
			
			if (string.IsNullOrEmpty(Result))
			{
				Path = "$.SmtpPassword";
				EmailPassword = GetJsonPathValueAsString(JsonSource, Path, "Email Password", out Result);
			}
			
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
			
			if (string.IsNullOrEmpty(Result))
			{
				Path = "$.Subject";
				Subject = GetJsonPathValueAsString(JsonSource, Path, "Subject", out Result);
			}
			
			if (string.IsNullOrEmpty(Result))
			{
				Path = "$.EmailBody.Content";
				EmailBodyContent = GetJsonPathValueAsString(JsonSource, Path, "Email Body Content", out Result);
				
				/*
				Console.WriteLine("Processing EmailBodyContent");
				Console.WriteLine("Result: " + Result);
				Console.WriteLine("Email Body Content: " + EmailBodyContent);
				*/
			}
			
			if (string.IsNullOrEmpty(Result))
			{
				Path = "$.EmailBody.FileName";
				EmailBodyFileName = GetJsonPathValueAsString(JsonSource, Path, "Email Body FileName", out Result);
			}
			
			if (string.IsNullOrEmpty(Result))
			{
				Path = "$.SenderName";
				SenderName = GetJsonPathValueAsString(JsonSource, Path, "Sender Name", out Result);
			}
			
			if (string.IsNullOrEmpty(Result))
			{
				Path = "$.FromAddress";
				FromAddress = GetJsonPathValueAsString(JsonSource, Path, "From Address", out Result);
			}
			
			if (string.IsNullOrEmpty(Result) && !string.IsNullOrEmpty(SenderName) && !string.IsNullOrEmpty(FromAddress))
			{
				Sender.Add(new EmailAddressItem(SenderName, FromAddress));
			}
			
			JToken Token = null;
			if (string.IsNullOrEmpty(Result))
			{
				Path = "$.Recipients";
				//Token =  JsonUtilities.GetJsonPathValue(JsonSource, Path, out Result);				
				Token =  JsonUtilities.GetJsonPathValue(JsonSource, Path, out Result);
				if (string.IsNullOrEmpty(Result)) Console.WriteLine(Path + " returned OK");
				//foreach (var Item in  Token.Children())
				foreach (JObject Item in  Token.Children())
				{
					RecipientName = JsonUtilities.GetJsonPathValueStr(Item, "$.Name", out Result);
					if (string.IsNullOrEmpty(Result))
					{
						ToAddress = JsonUtilities.GetJsonPathValueStr(Item, "$.Address", out Result);
						Console.WriteLine("Recipient-Name: " + RecipientName);
					}
					if (string.IsNullOrEmpty(Result))
					{
						Recipients.Add(new EmailAddressItem(RecipientName, ToAddress));
					}
				}
			}
			
			return Result;
		}
		
		private string GetJsonPathValueAsString(JObject Source, string Path, string FieldName, out string Res)
		{
			string Result = JsonUtilities.GetJsonPathValueStr(Source, Path, out Res);
			if (!string.IsNullOrEmpty(Res) || string.IsNullOrEmpty(Result))
			{
				Res = "Cannot provide a value for: " + FieldName;
			}
			return Result;
		}
		
		private int GetJsonPathValueAsInt32(JObject Source, string Path, string FieldName, out string Res)
		{
			int Result = 0;
			string Value = GetJsonPathValueAsString(Source, Path, FieldName, out Res);
			
			if (string.IsNullOrEmpty(Res))
			{
				bool IsValid = Int32.TryParse(Value, out Result);
				if (!IsValid) Res = "Error converting Value to number";
			}
			
			return Result;			
		}
		
		private bool GetJsonPathValueAsBoolean(JObject Source, string Path, string FieldName, out string Res)
		{
			bool Result = false;
			
			string Value = GetJsonPathValueAsString(Source, Path, FieldName, out Res);
			if (string.IsNullOrEmpty(Res))
			{
				if ((Value.ToUpper() != "TRUE") && (Value.ToUpper() != "FALSE"))
				{
					Res = "The value for: " + FieldName + " is not valid (" + Value + ")";
				}
			}
			
			if (string.IsNullOrEmpty(Res))
			{
				if (Value.ToUpper() == "TRUE") Result = true; else Result = false;				
			}
			
			return Result;
		}
		
		public string Validate()
		{
			string Result = "";
			return Result;
		}

        public void Display()
        {
        	Console.WriteLine("Display");
        	Console.WriteLine("-------");
        	Console.WriteLine("RunId: " + RunId);
        	Console.WriteLine("SmtpServer: " + SmtpServer);
        	Console.WriteLine("SmtpPort: " + SmtpPort);
        	Console.WriteLine("EmailUserName: " + EmailUserName);
        	Console.WriteLine("EmailPassword: " + EmailPassword);
        	Console.WriteLine("UseSSL: " + UseSSL);
        	Console.WriteLine("UseTLS: " + UseTLS);
			
        	Console.WriteLine("Subject: " + Subject);
        	
        	/*
        	Console.WriteLine("Sender Name: " + SenderName);
        	Console.WriteLine("FromAddress: " + FromAddress);
        	Console.WriteLine("Recipient Name: " + RecipientName);
        	Console.WriteLine("ToAddress: " + ToAddress);        	
        	*/
        	
        	foreach (EmailAddressItem Item in Recipients)
        	{
        		Console.WriteLine("Recipient Name: " + Item.Name);
        		Console.WriteLine("Address: " + Item.Address);
        	}
        	
        	foreach (EmailAddressItem Item in Sender)
        	{
        		Console.WriteLine("Sender Name: " + Item.Name);
        		Console.WriteLine("Address: " + Item.Address);
        	}
        	
        	Console.WriteLine("EmailBodyContent: " + EmailBodyContent);
        	Console.WriteLine("EmailBodyFileName: " + EmailBodyFileName);
        }	
	}
}
