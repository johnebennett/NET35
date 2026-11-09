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
        
        /*
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
        
        public static void WriteParamsToOutputLog(Int64 RunId,
                                                  string QueryText,
                                                  List<FbParameterItem> Params)
        {
        	WriteParamsToOutputLog(Convert.ToInt32(RunId), QueryText, Params);
        }
        
        public static void WriteToOutput(string UniqueFileId, string LineText)
        {
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
		*/
		
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
	        
	        string SmtpServer = "";
	        int SmtpPort = 0;
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
				/*
	        	Console.WriteLine("Images Count: " + Images.Count);
	        	WriteToOutput("Images Count: " + Images.Count);
				*/
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

            ActionBase ActionBase = new ActionBase();
            try
            {
                Regex reg = new Regex("{{(.|\\n)*?}}");

                string Str = "";
                MatchCollection matches = reg.Matches(EmailBody);
                for (int Index = 0; Index < matches.Count; Index++)
                {
                    Str = matches[Index].Value;
                    Str = Str.Replace("{{", "").Replace("}}", "");
                    Result.Add(Str);
                }
            }
            //catch (Exception Ex)
            catch (Exception)
            {
            }

            return Result;
        }
	
		public static string CopySharedFiles(string ConnectionString,
			                                 Int64 CompanyClientId,
			                                 Int64 RunId,
			                                 Int64 EVAC_ID,
			                                 Int64 EventRunClientId,
			                                 ConfigSettings ConfigSettings)
		{
			string Result = "";
			
			//int RunIdInt = Convert.ToInt32(RunId);
			//WriteToOutputLog(RunIdInt, "Inside CopySharedFiles");
			LoggingMethods.WriteToOutputLog(RunId, "Inside CopySharedFiles");
			string QueryText =
				" SELECT ReportId, DocumentId, StorageId" +
				" FROM EAEventClientAttachment(@CompanyClientId, @RunID, @EVAC_ID)" +
				" WHERE ETemplateId = 0" +
				"   AND ((ReportId > 0) OR (DocumentId > 0) or (StorageId > 0))";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("CompanyClientId", CompanyClientId));
			Params.Add(new FbParameterItem("RunId", RunId));
			Params.Add(new FbParameterItem("EVAC_ID", EVAC_ID));
			//WriteParamsToOutputLog(RunId, QueryText, Params);
			LoggingMethods.WriteParamsToOutputLog(RunId, QueryText, Params);
			DataTable dt = null;
			Int32 ReportId = 0;
			Int32 DocumentId = 0;
			Int32 StorageId = 0;
			Int32 Value = 0;
			Object Obj = null;
			string SrcFileName = "";
			try
			{
				dt =
					UsefulDBMethods.Helpers.GetFirstDataTableEx(
						ConnectionString, QueryText, Params, 0, 
						UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
				if (string.IsNullOrEmpty(Result))
				{
					foreach (DataRow Row in dt.Rows)
					{
						Value = 0;
						if (Value == 0)
						{
							Obj = Row["ReportId"];
							Value = (Obj == DBNull.Value) ? 0 : Convert.ToInt32(Obj);
							if (Value > 0)
							{
								ReportId = Value;
								SrcFileName = "EARep" + Convert.ToString(ReportId) + ".rtm";								
							}
						}
						if (Value == 0)
						{
							Obj = Row["DocumentId"];
							Value = (Obj == DBNull.Value) ? 0 : Convert.ToInt32(Obj);
							if (Value > 0)
							{
								DocumentId = Value;
								SrcFileName = "Doc" + Convert.ToString(DocumentId) + ".doc";
							}
						}
						if (Value == 0)
						{
							Obj = Row["StorageId"];
							Value = (Obj == DBNull.Value) ? 0 : Convert.ToInt32(Obj);
							if (Value > 0)
							{
								//SrcFileName = "" + Convert.ToString(StorageId) + "";
								StorageId = Value;
								SrcFileName = "";
							}
						}
						
						if (!string.IsNullOrEmpty(SrcFileName))
						{
							string RemoteFileName = ConfigSettings.ShareCreateDocsLocation + SrcFileName;
							string LocalFileName = ConfigSettings.LocalCreateDocsLocation + SrcFileName;
							FileInfo RemoteFI = new FileInfo(RemoteFileName);
							FileInfo LocalFI = new FileInfo(LocalFileName);							
							
							if ((!LocalFI.Exists) || (RemoteFI.LastWriteTime > LocalFI.LastWriteTime))
							{
								string Msg = "Copying the file: {0} to {1}";
								LoggingMethods.WriteToOutputLog(RunId, string.Format(Msg, RemoteFileName, LocalFileName));
								File.Copy(RemoteFileName, LocalFileName, true);
							}
						}
					}
				}
            }
            catch (Exception ex)
            {
				LoggingMethods.WriteToConsole(ex.Message);				
                LoggingMethods.WriteToOutputLog(RunId, ex.Message);
                Result = ex.Message;
            }
			return Result;
		}
        
        public static string CopyEmailTemplateToEmailServer(Int64 TemplateId)
        {
        	string Result = "";
        	return Result;
        }
        
        public static string LoadAppSettings()
        {
        	string Result = "";
        	return Result;
        }
        
        /// <summary>
        /// SyncCreateDocsItems
        /// Need to copy the contents of the signature, logo and images folder
        /// from the source (remote machine) to the same folders in the local machine
        /// These folder contents are used for report generation and are changeable 
        /// by clients on the web server.
        /// </summary>
        /// <param name="RunId">
        /// The RunId to use for output entries to the log file for the email processor
        /// </param>
        /// <param name="CompanyClientId">
        /// The companyId contents that need to be copied
        /// </param>
        /// <param name="config">
        /// An object that contains the config entries from the JSON file
        /// This is used to process the folder locations
        /// </param>
        /// <returns></returns>
        public static string SyncCreateDocsItems(Int64 RunId,
                                                 Int64 CompanyClientId,
                                                 ConfigSettings config)
        {
        	string Result = "";
        	StringBuilder ErrorOutput = new StringBuilder();
        	List<string> FolderList = new List<string>();
        	FolderList.Add("signature");
        	FolderList.Add("logo");
        	FolderList.Add("images");
        	
        	
			LoggingMethods.WriteToOutputLog(RunId, "Inside SyncCreateDocsItems");
			
			string RemoteFolderBase = config.ShareCreateDocsLocation;
			string LocalFolderBase = config.LocalCreateDocsLocation;
			string CurrentRemoteFolder = "";
			string CurrentLocalFolder = "";
			string PathSep = Path.DirectorySeparatorChar.ToString();
			//string[] RemoteFileList;
			string CurrentLocalFileName = "";
			string FileNameOnly = "";
			
			foreach (string folder in FolderList)
			{
				LoggingMethods.WriteToOutputLog(RunId, "Processing folder: " + folder);
				// for each of the folders simply copy the contents to the corresponding
				// folder in the email server machine
				// The folder structure will be of the format:
				// ShareCreateDocsLocation\{folder}\{CompanyClientId}\*.*
				
				CurrentRemoteFolder = Path.Combine(RemoteFolderBase, folder, CompanyClientId.ToString());
				CurrentLocalFolder = Path.Combine(LocalFolderBase, folder, CompanyClientId.ToString());
				
				string[] RemoteFileList = Directory.GetFiles(CurrentRemoteFolder, "*.*");
				foreach(string FileName in RemoteFileList)
				{
					LoggingMethods.WriteToOutputLog(RunId, "Processing the file: " + FileName);
					FileNameOnly = Path.GetFileName(FileName);
					CurrentLocalFileName = CurrentLocalFolder + PathSep + FileNameOnly;
					LoggingMethods.WriteToOutputLog(RunId, "Copying to: " + CurrentLocalFileName);
					try 
					{
						File.Copy(FileName, CurrentLocalFileName, true);
					}
					catch (Exception Ex)
					{
						Result = Ex.Message;
						ErrorOutput.AppendLine(Result);
						LoggingMethods.WriteToOutputLog(RunId, Result);
					}
				}
			}
			
			if (string.IsNullOrEmpty(Result))
			{
				if (ErrorOutput.Length > 0)
					Result = ErrorOutput.ToString();
			}
        	
        	return Result;
        }
	}
}
