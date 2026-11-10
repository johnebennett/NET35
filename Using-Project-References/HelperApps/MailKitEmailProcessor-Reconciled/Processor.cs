/*
 * Created by SharpDevelop.
 * User: johne
 * Date: 30/10/2023
 * Time: 6:55 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using MailKitEmailProcessor.Actions;
using MimeKit;
using MailKit.Net.Smtp;
using MimeKit.Utils;
using System.Threading;  

namespace MailKitEmailProcessor
{
	/// <summary>
	/// Description of Processor.
	/// </summary>
	public class Processor
	{
		public Processor()
		{
		}
		
		public string ProcessEmailRun(Actions.EmailItems EmailItems)
		{
			string Result = "";	    	
	    	
	        // Email configuration
	        /*
	        string smtpServer = "smtp.office365.com";
	        int smtpPort = 587; // Office 365 SMTP port
	        string username = "your_email@example.com";
	        string password = "your_password";
	        bool useSsl = true; // Use SSL/TLS
	        bool useTls = true; // Use TLS
	        */

	        /*
	        smtpServer = "smtp.office365.com";
	        smtpPort = 587; // Office 365 SMTP port
	        username = "your_email@example.com";
	        password = "your_password";
	        useSsl = true; // Use SSL/TLS
	        useTls = true; // Use TLS
	        */
	       /*
	        smtpServer = "smtp-relay.brevo.com";
	        smtpPort = 587; // Office 365 SMTP port
	        username = "goatpropertymanager@gmail.com";
	        password = "qDNvW9fKGUpJQrF6";
	        useSsl = true; // Use SSL/TLS
	        useTls = false; // Use TLS
	        */
	        
	        // Create a new MimeMessage
	        var message = new MimeMessage();
	        /*
	        message.From.Add(new MailboxAddress("Sender Name", "your_email@example.com"));
	        message.To.Add(new MailboxAddress("Recipient Name", "recipient_email@example.com"));
	        */
	       
	       /*
	        message.From.Add(new MailboxAddress("Sender Name", "bennettcentral@gmail.com"));
	        message.To.Add(new MailboxAddress("Recipient Name", "bennettcentral@gmail.com"));
	        message.Subject = "Subject of the Email";
	        */
	       
			//message.From.Add(new MailboxAddress(EmailItems.SenderName, EmailItems.FromAddress));
			foreach (EmailAddressItem Item in EmailItems.Sender)
			{
				message.From.Add(new MailboxAddress(Item.Name, Item.Address));
			}
			//message.From.Add(new MailboxAddress((EmailItems.Sender as EmailAddressItem).Name, (EmailItems.Sender as EmailAddressItem).Address));
	       
	       
			/*
			foreach (string Str in EmailItems.ToAddress.Split(new string[]{";"}, StringSplitOptions.RemoveEmptyEntries))
			{
				message.To.Add(new MailboxAddress(EmailItems.RecipientName, Str));
			}
			*/
			
			foreach (EmailAddressItem Item in EmailItems.Recipients)
			{
				message.To.Add(new MailboxAddress(Item.Name, Item.Address));
			}
	       
	       //message.To.Add(new MailboxAddress(EmailItems.RecipientName, EmailItems.ToAddress));
	       	message.Subject = EmailItems.Subject;
	
	        // Create the text part of the email
	        var textPart = new TextPart("plain")
	        {
	            Text = "Hello, this is the text part of the email."
	        };
	
	        // Create the HTML part of the email (with embedded image)
	        var htmlPart = new TextPart("html")
	        {
	            Text = "<html><body><p>Hello, this is the HTML part of the email.</p><img src='cid:logo'></body></html>"
	        };
	
	        // Create a multipart/mixed container to hold the text and HTML parts
	        var multipart = new Multipart("mixed");
	        //multipart.Add(textPart);
	        multipart.Add(htmlPart);
	
	        // Load and attach an image
	        var logo = new MimePart("image", "png")
	        {
	            Content = new MimeContent(File.OpenRead("logo.png"), ContentEncoding.Default),
	            ContentDisposition = new ContentDisposition(ContentDisposition.Inline),
	            ContentTransferEncoding = ContentEncoding.Base64,
	            FileName = Path.GetFileName("logo.png")
	        };
	        logo.ContentId = MimeUtils.GenerateMessageId();
	
	        // Add the embedded image to the HTML part
	        htmlPart.Text = htmlPart.Text.Replace("cid:logo", "cid:" + logo.ContentId);
	        multipart.Add(logo);
	
	        // Set the message body to the multipart container
	        message.Body = multipart;
	        //multipart.Add(MimeEntity);
	
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
	
	                client.Connect(
	               		EmailItems.SmtpServer, EmailItems.SmtpPort, MailKit.Security.SecureSocketOptions.StartTls, CancellationToken.None);
	               client.Authenticate(EmailItems.EmailUserName, EmailItems.EmailPassword);

	                // Send the email
	                client.Send(message);
	                client.Disconnect(true);
	            }
	            
	            Result = "Email sent successfully!";	
	            //Console.WriteLine("Email sent successfully!");
	        }
	        catch (Exception ex)
	        {
	            //Console.WriteLine(@"Error: {ex.Message}");
	            //Console.WriteLine("Error:" + ex.Message);
	            Result = "Error: " + ex.Message;
	        }

	        return Result;
		}
	}
}
