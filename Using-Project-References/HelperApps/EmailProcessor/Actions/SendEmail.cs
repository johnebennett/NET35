/*
 * Created by SharpDevelop.
 * User: John
 * Date: 13/05/2019
 * Time: 4:15 PM
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
	/*
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
		public string Host { get; set; }
		public string Port { get; set; }
		public string EmailUserName { get; set; }
		public string EmailPassword { get; set; }
		public int Timeout { get; set; }
		
		public SendEmail_Common Common = new SendEmail_Common();
		
		public string Subject { get; set; }
		public string Body { get; set; }
		
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
		
		public string Validate()
		{
			string Result = Common.Validate();
			
			if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(Subject)) Result = "Invalid Email Subject provided";
			if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(Body)) Result = "Invalid Email Body provided";
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
	*/
	
	/// <summary>
	/// Description of Email.
	/// </summary>
	public class SendEmail //: ActionBase
	{
		private string FModuleName = "EmailAction";
		private string FRunId = "";
		private int FRunIdValue = 0;
		private JObject DataObject = null;
		private string EmailBody = "";
		private SendEmail_Item FEmailItem = new SendEmail_Item();

	/*
		public static string Execute(int MasterId,
		                             //RequestCommonItem CommonItem,
		                             JObject DataObject,
		                             APISettingsItem SettingsItem,
		                             out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			SendEmail inst = new SendEmail();
			//if (!inst.Setup(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg)) return "";
			
			inst.LogOnInsideMethod(Method);
			try
			{
				Result = inst.Init();				
				if (!string.IsNullOrEmpty(Result))
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent(Result, Method, MasterId);
					return "";
				}
				
				Result = inst.Validate();
				if (!string.IsNullOrEmpty(Result))
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent(Result, Method, MasterId);
					return "";
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
			
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " +Method, FModuleName, MasterId);
			LogOnInsideMethod(Method);
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
					FEmailItem.Body = (string)DataObject["emailBody"];
					FEmailItem.SenderAddress = (string)DataObject["senderAddress"];
				}
				
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
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " +Method, FModuleName, MasterId);
			LogOnInsideMethod(Method);
			try 
			{
				if (string.IsNullOrEmpty(Result) && (FRunIdValue < 0)) Result = "Invalid RunId provided";
				if (string.IsNullOrEmpty(Result))
				{
					Result = FEmailItem.Validate();
				}
				

				if (!string.IsNullOrEmpty(Result))
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent("Validation Failed: " + Result, FModuleName, MasterId);
					Result =
						JsonUtilities.StartJson() +
						JsonUtilities.AddPropertyNameAndValue("Error", "Validation Failed", false) +
						JsonUtilities.EndJson();
				}
			}
			catch (Exception Ex)
			{
				Result = LogOnException(Method, Ex, "");
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
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " +Method, FModuleName, MasterId);
			LogOnInsideMethod(Method);
			try 
			{
				Result = BuildOutput(out ErrorMsg);
				if (!string.IsNullOrEmpty(ErrorMsg))
				{
					Result = LogOnProcessingError(Method, ErrorMsg, "");
				}
			}
			catch (Exception Ex)
			{
				Result = LogOnException(Method, Ex, "");
			}
			finally
			{
				LogOnLeavingMethod(Method);
			}
			WindowsEventLoggingUtilities.WriteToInfoEvent("Leaving: " +Method, FModuleName, MasterId);
			
			return Result;
		}
		
		private string BuildOutput(out string ErrorMsg)
		{
			ErrorMsg = "";
			string Result = "";
			string Method = "BuildOutput";
			
			List<string> ErrorList = new List<string>();
			
			LogOnInsideMethod(Method);
			try
			{
				WindowsEventLoggingUtilities.WriteToInfoEvent("Inside BuildOutput", FModuleName, MasterId);
				
				StringBuilder entries = new StringBuilder();
				entries.AppendLine(JsonUtilities.StartJson());
				
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
			}
			finally
			{
				LogOnLeavingMethod(Method);
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
					JsonUtilities.AddPropertyNameAndValue("Path", Item.Path) +
					JsonUtilities.AddPropertyNameAndValue("FileName", Item.FileName, false) +
					JsonUtilities.EndJsonObject();
				StrList.Add(Str);
			}
			Source.Append(string.Join(",", StrList.ToArray()));
			Source.AppendLine(JsonUtilities.EndJsonArray());
		}
		*/
		public static string Execute(Int64 RunId,
		                             string Subject,
		                             string Body,
		                             string SenderEmail,
		                             List<SendEmail_Recipient> Recipients,
		                             List<SendEmail_Recipient> CCList,
		                             List<SendEmail_Recipient> BCCList,
		                             List<SendEmail_AttachmentFile> Attachments,
		                             List<SendEmail_AttachmentFile> Images,
		                             List<SendEmail_AttachmentFile> BodyImages,
		                             out string ErrorMsg)
		{
			string Method = "Execute";
			Console.WriteLine("Inside - " + Method);
			string Result = "";
			ErrorMsg = "";

			SendEmail inst = new SendEmail();
			return Result;
		}
		public static string Execute(JObject SourceDataObject,
		                             string EmailBody,
		                             out string ErrorMsg)
		{
			string Method = "Execute";
			Console.WriteLine("Inside - " + Method);
			string Result = "";
			ErrorMsg = "";

			SendEmail inst = new SendEmail();
			inst.DataObject = SourceDataObject;
			if (inst.DataObject == null)
			{
				ErrorMsg = "The provided DataObject is NULL";
				return "";
			}
			inst.EmailBody = EmailBody;
			if (string.IsNullOrEmpty(inst.EmailBody))
			{
				ErrorMsg = "The provided eMail body is missing or empty.";
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
					FEmailItem.Body = EmailBody;
					FEmailItem.SenderAddress = (string)DataObject["senderAddress"];
				}
				
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
				if (string.IsNullOrEmpty(Result))
				{
					Result = FEmailItem.Validate();
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
				
				//TMSWordHelpers msWordHelpers = new TMSWordHelpers();
				//Console.WriteLine("Word Version:" + msWordHelpers.WordVersion());
				
				
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
					entries.AppendLine(JsonUtilities.StartJsonArray("SendEmailResult"));
					Result = SendEmailItem(ref entries);
					entries.Append(Result);
					entries.AppendLine(JsonUtilities.EndJsonArray());
				}

				entries.AppendLine(JsonUtilities.EndJson());
				Result = entries.ToString();
			}
			catch (Exception Ex)
			{
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
					mailMessage.IsBodyHtml = true;
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
				/*
				string text2 = XmlUtilities.StartXml();
				text2 += XmlUtilities.StartRootTag();
				text2 += XmlUtilities.WriteResultTag(1, 0);
				text2 += XmlUtilities.WriteXmlTag("Message", "");
				text2 += XmlUtilities.EndRootTag();
				*/
				result = 
					JsonUtilities.AddPropertyNameAndValue("status", "SUCCESS") +
					JsonUtilities.AddPropertyNameAndValue("message", "", false);
				//result = LogResponse("SendEmailItem", result);
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
				/*
				StrList.Add("SMTP Host: " + FEmailItem.Common.Host);
				StrList.Add("SMTP Port: " + FEmailItem.Common.Port.ToString());
				StrList.Add("Email UserName: " + FEmailItem.Common.EmailUserName);
				StrList.Add("Email Password: " + FEmailItem.Common.EmailPassword);
				StrList.Add("Sender Email Address: " + FEmailItem.SenderAddress);
				StrList.Add("Email Subject: " + FEmailItem.Subject);
				StrList.Add("Email Body:" + Environment.NewLine + FEmailItem.Body);

				StrList.Add("Recipient List:");
				foreach(SendEmail_Recipient Item in FEmailItem.Recipients)
				{
					StrList.Add(Item.Address);
				}
				
				StrList.Add("CC List:");
				foreach (SendEmail_Recipient Item in FEmailItem.CCs)
				{
					StrList.Add(Item.Address);
				}
				
				StrList.Add("Bcc List:");
				foreach (SendEmail_Recipient Item in FEmailItem.BCCs)
				{
					StrList.Add(Item.Address);
				}
				
				StrList.Add("Body Images:");
				foreach (SendEmail_BodyImage Item in FEmailItem.BodyImages)
				{
					StrList.Add("Identifier: " + Item.Identifier);
					StrList.Add("Path: " + Item.Path);
					StrList.Add("FileName: " + Item.FileName);
				}
				
				StrList.Add("Attachments");
				foreach (SendEmail_AttachmentFile Item in FEmailItem.Attachments)
				{
					StrList.Add("Attachment Path : " + Item.Path);
					StrList.Add("Attachment FileName : " + Item.FileName);
				}
				*/
				
				if (ex.Data != null)
				{
					StrList.Add("Start - Outputing Dictionary Values");
					foreach (System.Collections.DictionaryEntry dictionaryEntry in ex.Data)
					{
						StrList.Add(string.Format("Key: {0} with Value: {1}", dictionaryEntry.Key, dictionaryEntry.Value));
					}
					StrList.Add("End - Outputing Dictionary Values");
				}
				/*
				string text2 = XmlUtilities.StartXml();
				text2 += XmlUtilities.StartRootTag();
				text2 += XmlUtilities.WriteResultTag(0, 0);
				text2 += XmlUtilities.WriteXmlTag("Message", text3);
				text2 += XmlUtilities.EndRootTag();
				*/
				WindowsEventLoggingUtilities.WriteToInfoEvent("Information:" + Environment.NewLine + string.Join("", StrList.ToArray()), FModuleName, 0);
				result = 
					JsonUtilities.AddPropertyNameAndValue("status", "FAILURE") +
					JsonUtilities.AddPropertyNameAndValue("message", result, false);
				//result = LogResponse("SendEmailItem", result);
			}
			return result;
		}
	}

		/// <summary>
	/// Description of SendEmail.
	/// </summary>
	public class SendSingleEmail
	{
		public SendSingleEmail()
		{
		}
	}
}
