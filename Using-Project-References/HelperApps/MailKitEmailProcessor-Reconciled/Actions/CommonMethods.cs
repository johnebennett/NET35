/*
 * Created by SharpDevelop.
 * User: johne
 * Date: 30/10/2023
 * Time: 6:58 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using MimeKit;
using MimeKit.Utils;
using MailKit.Net.Smtp;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using Newtonsoft.Json.Linq;
using CommonUtilities;
using System.Data;
using EAOfficeAPI;
using System.Text.RegularExpressions;

namespace MailKitEmailProcessor.Actions
{
	/// <summary>
	/// Description of CommonMethods.
	/// </summary>
	public class CommonMethods
	{
		public CommonMethods()
		{
		}
		
        public static void WriteToOutput(string LineText)
        {
            //string WorkingDir = Directory.GetCurrentDirectory();
            string WorkingDir = GetExecutingAssemblyPath();
            string SepChar = Path.DirectorySeparatorChar.ToString();
            if (!WorkingDir.EndsWith(SepChar)) WorkingDir += SepChar;
            string DateStr = DateTime.Today.ToString("yyyy-MM-dd");
            string TimeStr = DateTime.Now.ToString("HH:mm:ss") + " - ";
            
            Console.WriteLine(TimeStr + LineText);
        }
        
        public static void WriteToOutputLog(int RunId, string LineText)
        {
			string FileName = "EmailProcessor_" + RunId.ToString() + ".txt";
			string WorkingDir = GetExecutingAssemblyPath();
			FileName = WorkingDir + FileName;
			using (StreamWriter writer = new StreamWriter(FileName, true))
			{
				writer.WriteLine(LineText);
				Console.WriteLine(LineText);
			}
        }
        
        public static string GetExecutingAssemblyPath()
        {
        	string 
        		Result =
        			new Uri(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).LocalPath;
        	Result = Path.GetDirectoryName(Result);
        	string SepChar = Path.DirectorySeparatorChar.ToString();
        	if (!Result.EndsWith(SepChar)) Result += SepChar;
        	return Result;
        }
        
        public static void WriteParamsToOutput(string UniqueFileId, 
                                               string QueryText,
                                               List<FbParameterItem> Params)
        {
        	StringBuilder sb = new StringBuilder();
        	sb.AppendLine("QueryText:");
        	sb.AppendLine(QueryText);
        	
        	foreach (FbParameterItem Item in Params)
        	{
        		sb.AppendLine(Item.ParameterName + ": " + Convert.ToString(Item.ParameterValue));
        	}
        	
        	WriteToOutput(UniqueFileId, sb.ToString());
        }
        
        public static void WriteParamsToOutput(int UniqueFileId, string QueryText, List<FbParameterItem> Params)
        {
        	WriteParamsToOutput(Convert.ToString(UniqueFileId), QueryText, Params);
        }
		
        public static void WriteToOutput(string UniqueFileId, string LineText)
        {
            //string WorkingDir = Directory.GetCurrentDirectory();
            /*
            string WorkingDir =
				new Uri(
	            	System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).LocalPath;
            WorkingDir = Path.GetDirectoryName(WorkingDir);
            string SepChar = Path.DirectorySeparatorChar.ToString();
            if (!WorkingDir.EndsWith(SepChar)) WorkingDir += SepChar;
            */
            string WorkingDir = GetExecutingAssemblyPath();
            string OutputFileName = WorkingDir + UniqueFileId + ".txt";
            Console.WriteLine("Output FileName: " + OutputFileName);
            string TimeStr = DateTime.Now.ToString("HH:mm:ss") + " - ";
            
            Console.WriteLine(TimeStr + LineText);
            File.AppendAllText(OutputFileName, TimeStr + LineText + Environment.NewLine);
        }
        
		public static void WriteToErrorOutput(string LineText)
        {
            string DateStr = DateTime.Today.ToString("yyyy-MM-dd");
            string TimeStr = DateTime.Now.ToString("HH:mm:ss") + " - ";
            string WorkingDir = Directory.GetCurrentDirectory();
            string SepChar = Path.DirectorySeparatorChar.ToString();
            if (!WorkingDir.EndsWith(SepChar)) WorkingDir += SepChar;

            string FileName = "EmailProcessor_Errors_" + DateStr + ".txt";
            FileName = WorkingDir + FileName;
            using (StreamWriter writer = new StreamWriter(FileName, true))
            {
                writer.WriteLine(TimeStr + LineText);
            }
        }
		
		/***********************************************************************/
		public static string BuildEmail() 
		{
			string Result = "";
        	// Load HTML content with embedded images and attachments
        	string htmlContent = File.ReadAllText("email_with_images_and_attachments.html"); // Replace with your HTML content file

	        // Create a new MimeMessage
	        var message = new MimeMessage();
	        message.From.Add(new MailboxAddress("Sender Name", "your_email@example.com"));
	        message.To.Add(new MailboxAddress("Recipient Name", "recipient_email@example.com"));
	        message.Subject = "Subject of the Email";

	        // Create a BodyBuilder to build the email body
	        var builder = new BodyBuilder();

	        // Create the text part of the email (same as before)
	        builder.TextBody = "Hello, this is the text part of the email.";
	
	        // Create the HTML part of the email
	        List<MimePart> imageMimeParts = new List<MimePart>();
	        builder.HtmlBody = ModifyHtmlWithEmbeddedImages(htmlContent, out imageMimeParts);

	        // Add the image MimeParts to the BodyBuilder
	        foreach (var imagePart in imageMimeParts)
	        {
	            builder.Attachments.Add(imagePart);
	        }

	        // Add attachments to the BodyBuilder
	        AddAttachments(builder);
	
	        // Set the BodyBuilder as the body of the MimeMessage
	        message.Body = builder.ToMessageBody();
	        
	        //xxx
	        string SmtpServer = "";
	        int SmtpPort = 0;
	        //bool UseSSL = true;
	        //bool UseTls = true;
	        string EmailUserName = "";
	        string EmailPassword = "";
		    CancellationTokenSource source = new CancellationTokenSource();
	        CancellationToken token = source.Token;

	        try
	        {
	            using (var client = new SmtpClient())
	            {
	            	client.Connect(SmtpServer, SmtpPort, MailKit.Security.SecureSocketOptions.StartTls, CancellationToken.None);
	                client.Authenticate(EmailUserName, EmailPassword);
	
	                // Send the email
	                client.Send(message);
	                client.Disconnect(true);
	            }
	
	            Console.WriteLine("Email sent successfully!");
	        }
	        catch (Exception ex)
	        {
	            Result = "Error: " + ex.Message;
	        }
	        
	        return Result;
	    }
	
	    static string ModifyHtmlWithEmbeddedImages(string htmlContent, out List<MimePart> imageMimeParts)
	    {
	        var doc = new HtmlDocument();
	        //doc.LoadHtml(htmlContent);
	        doc.Load(htmlContent);
	
	        imageMimeParts = new List<MimePart>();
	
	        // Find all <img> tags in the HTML
	        //var imgTags = doc.DocumentNode.Descendants("img"). ToList();
	        IEnumerable<HtmlNode> imgTags = doc.DocumentNode.Descendants("img");
	        
	
	        //foreach (var imgTag in imgTags)
	        foreach (var imgTag in doc.DocumentNode.Descendants("img"))
	        {
	            var src = imgTag.GetAttributeValue("src", null);
	            if (!string.IsNullOrEmpty(src))
	            {
	                // Load the image from the src URL or file path (modify this logic based on your needs)
	                byte[] imageBytes;
	                if (src.StartsWith("http"))
	                {
	                    // Download image from a URL
	                    using (var webClient = new System.Net.WebClient())
	                    {
	                        imageBytes = webClient.DownloadData(src);
	                    }
	                }
	                else
	                {
	                    // Load image from a local file
	                    imageBytes = File.ReadAllBytes(src);
	                }
	
	                // Create a MimePart for the image
	                var imagePart = new MimePart("image", "jpeg")
	                {
	                    Content = new MimeContent(new MemoryStream(imageBytes), ContentEncoding.Default),
	                    ContentDisposition = new ContentDisposition(ContentDisposition.Inline),
	                    ContentTransferEncoding = ContentEncoding.Base64,
	                };
	
	                // Generate a unique Content-ID for the image
	                imagePart.ContentId = MimeUtils.GenerateMessageId();
	
	                // Replace the src attribute with the Content-ID
	                imgTag.SetAttributeValue("src", "cid:" + imagePart.ContentId);
	
	                // Add the MimePart to the list
	                imageMimeParts.Add(imagePart);
	            }
	        }
	
	        return doc.DocumentNode.OuterHtml;
	    }
	
	    static void AddAttachments(BodyBuilder builder)
	    {
	        // Add attachments here
	        var attachment1 = CreateAttachment("file.txt");
	        var attachment2 = CreateAttachment("document.pdf");
	
	        builder.Attachments.Add(attachment1);
	        builder.Attachments.Add(attachment2);
	    }

	    static MimePart CreateAttachment(string filePath)
	    {
	        var attachment = new MimePart("application", "octet-stream")
	        {
	            Content = new MimeContent(File.OpenRead(filePath), ContentEncoding.Default),
	            ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
	            ContentTransferEncoding = ContentEncoding.Base64,
	            FileName = Path.GetFileName(filePath),
	        };
	
	        return attachment;
	    }
	    
	    public static string ValidateEmailAddress(string EmailAddress)
	    {
	    	string Result = "";
	    	
	    	if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(EmailAddress))
	    	{
				Result = "Empty or invalid Email Address provided";
	    	}
	    	
	    	if (string.IsNullOrEmpty(Result))
	    	{
	    		// validate the email address format for compliance
	    	}
	    	
	    	return Result;
	    }
	    
	    public static string ValidateFileName(string FileName)
	    {
	    	string Result = "";
	    	
	    	if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(FileName))
	    	{
				Result = "Empty or invalid FileName provided";				
	    	}
	    	
	    	if (string.IsNullOrEmpty(Result))
	    	{
	    		if (!File.Exists(FileName))
	    		{
					Result = "The provided file: " + FileName + " does not exist";
	    		}
	    	}
	    	
	    	return Result;
	    }
	    
	    public static string ProcessEmailBody(ref string EmailBody, ref List<EmailBodyImage> Images)
	    {
	    	string Result = "";
	    	
	    	Console.WriteLine("Inside ProcessEmailBody");
	    	Console.WriteLine("Image Count: " + Images.Count);
	    	
	    	try 
	    	{
		    	var doc = new HtmlDocument();
		    	doc.LoadHtml(EmailBody);
		    	
		    	// Find all <img> tags in the HTML
		    	var imgTags = doc.DocumentNode.Descendants("img");
	
		        foreach (var imgTag in imgTags)
		        {
		            var src = imgTag.GetAttributeValue("src", "");
		            if (!string.IsNullOrEmpty(src))
		            {
		            	EmailBodyImage newImage = new EmailBodyImage();
		            	newImage.FileName = src;
		            	newImage.Identifier = MimeUtils.GenerateMessageId();
		                // Replace the src attribute with the Content-ID
		                imgTag.SetAttributeValue("src", "cid:" + newImage.Identifier);
		                Images.Add(newImage);
		            }
		        }
	
	        	EmailBody = doc.DocumentNode.OuterHtml;
	        	Console.WriteLine("Images Count: " + Images.Count);
	        	WriteToOutput("Images Count: " + Images.Count);
	    	}
	    	catch (Exception Ex)
	    	{
	    		Result = Ex.Message + Environment.NewLine + Ex.StackTrace;
	    	}

	        return Result;
	    }
	    
	    public static void DisplayEmailItems(SendEmailParam Source,
	                                         string EmailBodyContent,
	                                         string EmailBodyFileName)
	    {
	    	DisplayEmailItems(
	    		Source.AccountDetails, Source.Sender, 
	    		Source.Recipients, Source.CCRecipients, Source.BCCRecipients, Source.ExclusionList, 
	    		Source.Subject, Source.EmailBody,
	    		Source.EmailBodyImages, Source.EmailAttachments,
	    		EmailBodyContent, EmailBodyFileName);
	    }
	    	
	    
	    public static void DisplayEmailItems(AccountDetails AccountDetails,
                                             SenderDetailsItem Sender,
                                             List<NameAndAddressItem> Recipients,
                                             List<NameAndAddressItem> CCRecipients,
                                             List<NameAndAddressItem> BCCRecipients,
                                             List<NameAndAddressItem> ExclusionList,
                                             string Subject,
                                             string EmailBody,
                                             List<EmailBodyImage> EmailBodyImages,
                                             List<EmailAttachment> EmailAttachments,
                                             string EmailBodyContent,
                                             string EmailBodyFileName)
	    {
			StringBuilder sb = new StringBuilder();
			sb.Clear();
			sb.AppendLine("Account Details");
			sb.AppendLine("===============");
			sb.Append(AccountDetails.Display());
			
			sb.AppendLine("Sender Details");
			sb.AppendLine("==============");
			sb.Append(Sender.Display());
			
			sb.AppendLine("Recipient Details");
			sb.AppendLine("=================");			
			foreach(NameAndAddressItem Item in Recipients)
			{
				sb.Append(Item.Display());
			}
			
			if (CCRecipients.Count == 0)
			{
				sb.AppendLine("No CC Recipients");
			}
			else
			{
				sb.AppendLine("CC Recipient Details");
				sb.AppendLine("====================");			
				foreach(NameAndAddressItem Item in CCRecipients)
				{
					sb.Append(Item.Display());
				}
			}
			
			if (BCCRecipients.Count == 0)
			{
				sb.AppendLine("No BCC Recipients");
			}
			else
			{
				sb.AppendLine("BCC Recipient Details");
				sb.AppendLine("=====================");			
				foreach(NameAndAddressItem Item in CCRecipients)
				{
					sb.Append(Item.Display());
				}
			}
			
			if (ExclusionList.Count == 0)
			{
				sb.AppendLine("No Exclusions");
			}
			else
			{
				sb.AppendLine("Exclusion List Details");
				sb.AppendLine("======================");			
				foreach(NameAndAddressItem Item in ExclusionList)
				{
					sb.Append(Item.Display());
				}
			}
			
			sb.AppendLine("Subject: " + Subject);
			
			if (EmailAttachments.Count == 0)
			{
				sb.AppendLine("NO ATTACHMENTS");
			}
			else
			{
				sb.AppendLine("Attachments");
				sb.AppendLine("===========");
			}
			
			foreach(EmailAttachment Item in EmailAttachments)
			{
				sb.Append(Item.Display());
			}
			
			sb.AppendLine("EmailBodyContent");
			sb.AppendLine("================");
			sb.AppendLine(EmailBodyContent);
			sb.AppendLine("Email Body FileName");
			sb.AppendLine("===================");
			sb.AppendLine(EmailBodyFileName);
			
			sb.AppendLine("Email Body");
			sb.AppendLine("==========");
			sb.AppendLine(EmailBody);
			
			if (EmailBodyImages.Count == 0)
			{
				sb.AppendLine("NO IMAGES");
			}
			else
			{
				sb.AppendLine("Images");
				sb.AppendLine("======");
			}
			
			foreach (EmailBodyImage Item in EmailBodyImages)
			{
				sb.Append(Item.Display());
			}
			
			Console.WriteLine(sb.ToString());
	    }
	    
	    public static string GetImageExtnForUrl(string FileName, out string ErrorMsg)
	    {
	    	ErrorMsg = "";
	    	string Result = "";
	    	
	    	string[] parts = FileName.Split('.');
	    	if (string.IsNullOrEmpty(ErrorMsg) && (parts.Length == 0))
	    	{
	    		ErrorMsg =
	    			"Invalid FileName in GetImageExtnForUrl" + Environment.NewLine +
	    			"No extension can be found";
	    	}
	    	
	    	Result = parts[parts.Length - 1];
	    	
	    	return Result;
	    }
		
		public static string GetJsonPathValueAsString(JObject Source, string Path, string FieldName, out string Res)
		{
			string Result = JsonUtilities.GetJsonPathValueStr(Source, Path, out Res);
			if (!string.IsNullOrEmpty(Res) || string.IsNullOrEmpty(Result))
			{
				Res = "Cannot provide a value for: " + FieldName;
			}
			return Result;
		}
		
		public static int GetJsonPathValueAsInt32(JObject Source, string Path, string FieldName, out string Res)
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
		
		public static bool GetJsonPathValueAsBoolean(JObject Source, string Path, string FieldName, out string Res)
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

        public static string GetCompanyActiveStatus(string ConnectionString,
                                                    int CompanyId,
                                                    out string ErrorMsg)
        {
            string Result = "";
            ErrorMsg = "";
            string Method = "GetCompanyActiveStatus";

            ActionBase ActionBase = new ActionBase();
            try
            {
                string QueryText = "";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                if (string.IsNullOrEmpty(ErrorMsg))
                {
                    QueryText =
                        "SELECT B_CO_ACTIVECLIENT FROM EACOMPANY WHERE I_CO_COMPANYCLIENTID = @CompanyId";
                    Params.Add(new FbParameterItem("CompanyId", CompanyId));
                    ErrorMsg = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
                }

                DataTable tbl = null;
                object Obj = null;
                if (string.IsNullOrEmpty(ErrorMsg))
                {
                    tbl =
                        UsefulDBMethods.Helpers.GetFirstDataTableEx(
                            ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTablesAndRows, out ErrorMsg);

                    if (string.IsNullOrEmpty(ErrorMsg))
                    {
                        Obj = tbl.Rows[0]["B_CO_ACTIVECLIENT"];
                        Result = (Obj == DBNull.Value) ? "N" : Convert.ToString(Obj);
                    }
                }
            }
            catch (Exception Ex)
            {
                Result = ActionBase.LogOnException(Method, Ex, "");
            }

            return Result;
        }

        public static List<string> ParseEmailBody(string EmailBody)
        {
            List<string> Result = new List<string>();
            //string Method = "ParseEmailBody";

            ActionBase ActionBase = new ActionBase();
            try
            {
                //int StartPos = 0;
                //int EndPos = 0;
                Regex reg = new Regex("{{(.|\\n)*?}}");

                string Str = "";
                MatchCollection matches = reg.Matches(EmailBody);
                for (int Index = 0; Index < matches.Count; Index++)
                {
                    Str = matches[Index].Value;
                    Str = Str.Replace("{{", "").Replace("}}", "");
                    Result.Add(Str);
                }

                /*
                while (EmailBody.Length > 0)
                {
                    StartPos = EmailBody.IndexOf("{{");
                    if (StartPos > 0)
                    {
                        EmailBody = EmailBody.Substring(StartPos + 2);
                        EndPos = EmailBody.IndexOf("}}");
                        if (EndPos > 0)
                        {
                            Result.Add(EmailBody.Substring(0, EndPos));
                            EmailBody = EmailBody.Substring(EndPos + 2);
                        }
                    }
                    else EmailBody = "";
                }
            */
            }
            catch (Exception)
            {
                //Result = ActionBase.LogOnException(Method, Ex, "");
            }

            return Result;
        }	
		// Recovered production file synchronization and logging helpers.
		public static string CopySharedFiles(string ConnectionString, long CompanyClientId, long RunId, long EVAC_ID, long EventRunClientId, ConfigSettings ConfigSettings)
		{
			string ErrorMsg = "";
			int runId = Convert.ToInt32(RunId);
			WriteToOutputLog(runId, "Inside CopySharedFiles");
			string queryText = " SELECT ReportId, DocumentId, StorageId FROM EAEventClientAttachment(@CompanyClientId, @RunID, @EVAC_ID) WHERE ETemplateId = 0   AND ((ReportId > 0) OR (DocumentId > 0) or (StorageId > 0))";
			List<FbParameterItem> list = new List<FbParameterItem>();
			list.Add(new FbParameterItem("CompanyClientId", CompanyClientId));
			list.Add(new FbParameterItem("RunId", RunId));
			list.Add(new FbParameterItem("EVAC_ID", EVAC_ID));
			WriteParamsToOutputLog(RunId, queryText, list);
			DataTable dataTable = null;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			object obj = null;
			string text = "";
			try
			{
				dataTable = UsefulDBMethods.Helpers.GetFirstDataTableEx(ConnectionString, queryText, list, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out ErrorMsg);
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					foreach (DataRow row in dataTable.Rows)
					{
						num4 = 0;
						if (num4 == 0)
						{
							obj = row["ReportId"];
							num4 = ((obj != DBNull.Value) ? Convert.ToInt32(obj) : 0);
							if (num4 > 0)
							{
								num = num4;
								text = "EARep" + Convert.ToString(num) + ".rtm";
							}
						}
						if (num4 == 0)
						{
							obj = row["DocumentId"];
							num4 = ((obj != DBNull.Value) ? Convert.ToInt32(obj) : 0);
							if (num4 > 0)
							{
								num2 = num4;
								text = "Doc" + Convert.ToString(num2) + ".doc";
							}
						}
						if (num4 == 0)
						{
							obj = row["StorageId"];
							num4 = ((obj != DBNull.Value) ? Convert.ToInt32(obj) : 0);
							if (num4 > 0)
							{
								num3 = num4;
								text = "";
							}
						}
						if (!string.IsNullOrEmpty(text))
						{
							string text2 = ConfigSettings.ShareCreateDocsLocation + text;
							string text3 = ConfigSettings.LocalCreateDocsLocation + text;
							FileInfo fileInfo = new FileInfo(text2);
							FileInfo fileInfo2 = new FileInfo(text3);
							if (!fileInfo2.Exists || fileInfo.LastWriteTime > fileInfo2.LastWriteTime)
							{
								string format = "Copying the file: {0} to {1}";
								WriteToOutputLog(runId, string.Format(format, text2, text3));
								File.Copy(text2, text3, true);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				WriteToOutput(ex.Message);
				WriteToOutputLog(runId, ex.Message);
				ErrorMsg = ex.Message;
			}
			return ErrorMsg;
		}

		public static string CopyEmailTemplateToEmailServer(long TemplateId)
		{
			return "";
		}

		public static string LoadAppSettings()
		{
			return "";
		}

		public static string SyncCreateDocsItems(int RunId, long CompanyClientId, ConfigSettings config)
		{
			string text = "";
			StringBuilder stringBuilder = new StringBuilder();
			List<string> list = new List<string>();
			list.Add("signature");
			list.Add("logo");
			list.Add("images");
			WriteToOutputLog(RunId, "Inside SyncCreateDocsItems");
			string shareCreateDocsLocation = config.ShareCreateDocsLocation;
			string localCreateDocsLocation = config.LocalCreateDocsLocation;
			string text2 = "";
			string text3 = "";
			string text4 = Path.DirectorySeparatorChar.ToString();
			string text5 = "";
			string text6 = "";
			foreach (string item in list)
			{
				WriteToOutputLog(RunId, "Processing folder: " + item);
				text2 = Path.Combine(shareCreateDocsLocation, item, CompanyClientId.ToString());
				text3 = Path.Combine(localCreateDocsLocation, item, CompanyClientId.ToString());
				string[] files = Directory.GetFiles(text2, "*.*");
				string[] array = files;
				foreach (string text7 in array)
				{
					WriteToOutputLog(RunId, "Processing the file: " + text7);
					text6 = Path.GetFileName(text7);
					text5 = text3 + text4 + text6;
					WriteToOutputLog(RunId, "Copying to: " + text5);
					try
					{
						File.Copy(text7, text5, true);
					}
					catch (Exception ex)
					{
						text = ex.Message;
						stringBuilder.AppendLine(text);
						WriteToOutputLog(RunId, text);
					}
				}
			}
			if (string.IsNullOrEmpty(text) && stringBuilder.Length > 0)
			{
				text = stringBuilder.ToString();
			}
			return text;
		}
		public static void WriteToOutputLog(long RunId, string LineText)
		{
			WriteToOutputLog(Convert.ToInt32(RunId), LineText);
		}


		public static void WriteParamsToOutputLog(long RunId, string QueryText, List<FbParameterItem> Params)
		{
			WriteParamsToOutputLog(Convert.ToInt32(RunId), QueryText, Params);
		}

		public static void WriteParamsToOutputLog(int RunId, string QueryText, List<FbParameterItem> Params)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("QueryText:");
			stringBuilder.AppendLine(QueryText);
			foreach (FbParameterItem Param in Params)
			{
				stringBuilder.AppendLine(Param.ParameterName + ": " + Convert.ToString(Param.ParameterValue));
			}
			WriteToOutputLog(RunId, stringBuilder.ToString());
		}

}
}
