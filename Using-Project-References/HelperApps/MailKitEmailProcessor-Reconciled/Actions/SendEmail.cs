/*
 * Created by SharpDevelop.
 * User: johne
 * Date: 08/11/2023
 * Time: 6:46 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using MailKitEmailProcessor.Actions;
using MimeKit;
using MailKit.Net.Smtp;
using MimeKit.Utils;
using System.Threading;
//using System.Net;

namespace MailKitEmailProcessor.Actions
{
	/// <summary>
	/// Description of SendEmail.
	/// </summary>
	public class SendEmail
	{
		public string Send(SendEmailParam Source, Int32 RunId)
		{
			return 
				Send(Source.AccountDetails, Source.Sender, Source.Recipients,
				     Source.CCRecipients, Source.BCCRecipients, Source.Subject,
				     Source.EmailBody, Source.EmailBodyImages, Source.EmailAttachments, RunId);
		}
		
		public string Send(AccountDetails AccountDetails,		                   
		                   SenderDetailsItem Sender,
		                   List<NameAndAddressItem> Recipients,
		                   List<NameAndAddressItem> CCRecipients,
		                   List<NameAndAddressItem> BCCRecipients,
		                   string Subject,
		                   string EmailBody,
		                   List<EmailBodyImage> EmailBodyImages,
		                   List<EmailAttachment> EmailAttachments,
		                   Int32 RunId)
		{
			string Result = "";
			
			string str = "Inside SendEmail.Send";
			
			Console.WriteLine(str);
            CommonMethods.WriteToOutputLog(RunId, str);

	        // Create a new MimeMessage
	        var message = new MimeMessage();
	        
	        message.From.Add(new MailboxAddress(Sender.Name, Sender.Address));
	        message.ReplyTo.Add(new MailboxAddress(Sender.Name, Sender.ReplyToAddress));

	        /*
	        foreach (NameAndAddressItem Item in Recipients)
			{
				message.To.Add(new MailboxAddress(Item.Name, Item.Address));
			}
			*/
	        
	        message.Subject = Subject;

	        // Create the HTML part of the email (with embedded image(s))
	        var htmlPart = new TextPart("html");
	        htmlPart.Text = EmailBody;

	        // Create a multipart/mixed container to hold the text and HTML parts
	        var multipart = new Multipart("mixed");
	        //multipart.Add(textPart);
	        multipart.Add(htmlPart);

	        // Load and attach body images
	        List<MimePart> imageMimeParts = new List<MimePart>();
	        foreach (EmailBodyImage Img in EmailBodyImages)
	        {
	        	string Extn = "";
	        	byte[] imageBytes;
	        	if (Img.FileName.ToUpper().StartsWith("HTTP://") || Img.FileName.ToUpper().StartsWith("HTTPS://"))
	        	{
					System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
	        		WebClient wc = new WebClient();
	        		wc.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3");

	        		imageBytes = wc.DownloadData(Img.FileName);
	        		// Download the image from the URL
	        		using (var webClient = new System.Net.WebClient())
	        		{
	        			imageBytes = webClient.DownloadData(Img.FileName);
	        		}
	        		Extn = CommonMethods.GetImageExtnForUrl(Img.FileName, out Result);
	        	}
	        	else
	        	{
	        		// Load image from a local file
	        		imageBytes = File.ReadAllBytes(Img.FileName);
		        	Extn = new FileInfo(Img.FileName).Extension;
	        	}
	        	var bodyImage = new MimePart("image", Extn);
	        	bodyImage.Content = new MimeContent(new MemoryStream(imageBytes), ContentEncoding.Default);
	        	bodyImage.ContentDisposition = new ContentDisposition(ContentDisposition.Inline);
	        	bodyImage.ContentTransferEncoding = ContentEncoding.Base64;
	        	//bodyImage.FileName = Path.GetFileName(Img.FileName);
	        	//anImage.ContentId = MimeUtils.GenerateMessageId();
	        	bodyImage.ContentId = Img.Identifier;
	        	multipart.Add(bodyImage);
	        }
	        
	        foreach (EmailAttachment Item in EmailAttachments)
	        {
	        	AttachFile(multipart, Item.FileName);
	        }
	        

        	message.Body = multipart;
	        
	        
	        try 
	        {
	            using (var client = new SmtpClient())
	            {
	            	//client.Connect(smtpServer, smtpPort, useSsl, useTls);
	                //var cancellationToken = SmtpClient
	                CancellationTokenSource source = new CancellationTokenSource();
	                CancellationToken token = source.Token;
	                //CancellationToken cancellationToken = source.Token.
	                
	                /*
	                client.Connect(smtpServer, smtpPort, MailKit.Security.SecureSocketOptions.StartTls, CancellationToken.None);
	                client.Authenticate(username, password);
	                */
	               
	                
	                Console.WriteLine("Before Connect Time: " + DateTime.Now.ToString("HH:mm:ss"));
	                client.Connect(
	               		AccountDetails.Host, AccountDetails.Port, 
	               		MailKit.Security.SecureSocketOptions.StartTls, 
	               		CancellationToken.None);
	                Console.WriteLine("After Connect Time: " + DateTime.Now.ToString("HH:mm:ss"));
	                if (!AccountDetails.BypassAuthCheck)
	                {
	                    client.Authenticate(AccountDetails.UserName, AccountDetails.Password);
	                    Console.WriteLine("After Authenticate Time: " + DateTime.Now.ToString("HH:mm:ss"));
	                }

	                message.To.Clear();
		            CommonMethods.WriteToOutputLog(RunId, "Recipient count inside Send - " + Recipients.Count);
	                foreach (NameAndAddressItem Item in Recipients)
	                {
	                	message.To.Add(new MailboxAddress(Item.Name, Item.Address));
	                	/*
	                	message.To.Add(new MailboxAddress
	                	               ("John Bennett Yahoo", "johnebennett_au@yahoo.com.au"));
	                	*/
	                }
		            CommonMethods.WriteToOutputLog(RunId, "message.To count inside Send - " + message.To.Count);
				        
	                message.Cc.Clear();
		            CommonMethods.WriteToOutputLog(RunId, "CCRecipients count inside Send - " + CCRecipients.Count);
	                foreach (NameAndAddressItem Item in CCRecipients)
				    {
				    	message.Cc.Add(new MailboxAddress(Item.Name, Item.Address));
				    	/*
				    	message.Cc.Add(new MailboxAddress(
				    		"John Bennett GMail", "bennettcentral@gmail.com"));
				    	*/
				    }
		            CommonMethods.WriteToOutputLog(RunId, "message.CC count inside Send - " + message.Cc.Count);
				        
		            message.Bcc.Clear();
		            CommonMethods.WriteToOutputLog(RunId, "BCCRecipients count inside Send - " + BCCRecipients.Count);
				    foreach (NameAndAddressItem Item in BCCRecipients)
				    {
				    	message.Bcc.Add(new MailboxAddress(Item.Name, Item.Address));
				    	/*
				    	message.Bcc.Add(new MailboxAddress(
				    		"John Bennett", "hiddenitemfound@gmail.com"));
				    	*/
				    }
		            CommonMethods.WriteToOutputLog(RunId, "message.BCC count inside Send - " + message.Bcc.Count);

	                // Send the email
	                Console.WriteLine("Before Send - " + DateTime.Now.ToString("HH:mm:ss"));
	                Result = client.Send(message);
	                Console.WriteLine("After Send - " + DateTime.Now.ToString("HH:mm:ss"));
	               	
	               	client.Disconnect(true);
	            }
	            
	            CommonMethods.WriteToOutputLog(RunId, Result);
	            //if (CommonUtilities.StringUtilities.SameText(Result, "Message sent successfully!"))
	            	Result = "";
	            //Console.WriteLine("Email sent successfully!");
	        }
	        catch (Exception ex)
	        {
	            //Console.WriteLine(@"Error: {ex.Message}");
	            //Console.WriteLine("Error:" + ex.Message);
	            Result = "Error: " + ex.Message;
	            Console.WriteLine(Result);
	            CommonMethods.WriteToOutputLog(RunId, Result);
	        }

	        return Result;
		}
		
		private string AttachFile(Multipart multipart, string FileName)
		{
			string Result = "";
			
			if (File.Exists(FileName))
			{
				var attachment = new MimePart("application", "octet-stream")
				{
					Content = new MimeContent(File.OpenRead(FileName), ContentEncoding.Default),
					ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
					ContentTransferEncoding = ContentEncoding.Base64,
					FileName = Path.GetFileName(FileName)
				};

            	multipart.Add(attachment);
            }
			else
			{
				Result = "File not found: {" + FileName + "}";
			}
			
			return Result;
		}
	}
}
