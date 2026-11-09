/*
 * Created by SharpDevelop.
 * User: DELL 990
 * Date: 04/06/2019
 * Time: 5:33 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Net.Mail;
using CommonUtilities;

namespace EmailProcessor.Actions
{
	public class SmtpExceptionItem
	{
		public string ShortDescription { get; set; }
		public string LongDescription { get; set; }
		public SmtpStatusCode StatusCode { get; set; }
		public int SmtpStatusCodeValue { get; set; }
	}
	
	/// <summary>
	/// Description of SmtpClientExceptions.
	/// </summary>
	public class SmtpClientExceptions
	{
		public SmtpClientExceptions()
		{
		}
		
		public static List<SmtpExceptionItem> EmailExceptionResult(Exception Source)
		{
			List<SmtpExceptionItem> Result = new List<SmtpExceptionItem>();
			
			if (Source is SmtpException)
			{
				Type StatusCode = typeof(SmtpStatusCode);
				SmtpException Ex = new SmtpException((Source as SmtpException).StatusCode);
				SmtpExceptionItem Item = new SmtpExceptionItem();
				Item.StatusCode = Ex.StatusCode;
				switch (Item.StatusCode)
				{
						case SmtpStatusCode.BadCommandSequence:
							Item.ShortDescription = Enum.GetName(StatusCode, SmtpStatusCode.BadCommandSequence);
							Item.LongDescription = "The commands were sent in the incorrect sequence.";
							Item.SmtpStatusCodeValue = 503;
							break;
						case SmtpStatusCode.CannotVerifyUserWillAttemptDelivery:
							Item.ShortDescription = Enum.GetName(StatusCode, SmtpStatusCode.CannotVerifyUserWillAttemptDelivery);
							Item.LongDescription = 
								"The specified user is not local, but the receiving SMTP service accepted the message " +
								"and attempted to deliver it." + Environment.NewLine +
								"This status code is defined in RFC 1123, which is available at https://www.ietf.org.";
							Item.SmtpStatusCodeValue = 252;
							break;
						case SmtpStatusCode.ClientNotPermitted:
							Item.ShortDescription = Enum.GetName(StatusCode, SmtpStatusCode.ClientNotPermitted);
							Item.LongDescription = "The client was not authenticated or is not allowed to send mail using the specified SMTP host.";
							Item.SmtpStatusCodeValue = 454;
							break;
						case SmtpStatusCode.CommandNotImplemented:
							Item.ShortDescription = Enum.GetName(StatusCode, SmtpStatusCode.CommandNotImplemented);
							Item.LongDescription = "The SMTP service does not implement the specified command.";
							Item.SmtpStatusCodeValue = 502;
							break;
						case SmtpStatusCode.CommandParameterNotImplemented:
							Item.ShortDescription = Enum.GetName(StatusCode, SmtpStatusCode.CommandParameterNotImplemented);
							Item.LongDescription = "The SMTP service does not implement the specified command parameter.";
							Item.SmtpStatusCodeValue = 504;
							break;
						case SmtpStatusCode.CommandUnrecognized:
							Item.ShortDescription = Enum.GetName(StatusCode, SmtpStatusCode.CommandUnrecognized);
							Item.LongDescription = "The SMTP service does not recognize the specified command.";
							Item.SmtpStatusCodeValue = 500;
							break;
						case SmtpStatusCode.ExceededStorageAllocation:
							Item.ShortDescription = Enum.GetName(StatusCode, SmtpStatusCode.ExceededStorageAllocation);
							Item.LongDescription = "The message is too large to be stored in the destination mailbox.";
							Item.SmtpStatusCodeValue = 552;
							break;
						case SmtpStatusCode.GeneralFailure:
							Item.ShortDescription = Enum.GetName(StatusCode, SmtpStatusCode.GeneralFailure);
							Item.LongDescription = 
								"The transaction could not occur. You receive this error when the specified SMTP host cannot be found.";
							Item.SmtpStatusCodeValue = -1;
							break;
						case SmtpStatusCode.HelpMessage:
							Item.ShortDescription = Enum.GetName(StatusCode, SmtpStatusCode.HelpMessage);
							Item.LongDescription = "A Help message was returned by the service.";
							Item.SmtpStatusCodeValue = 214;
							break;
						case SmtpStatusCode.InsufficientStorage:
							Item.ShortDescription = Enum.GetName(StatusCode, SmtpStatusCode.InsufficientStorage);
							Item.LongDescription = "The SMTP service does not have sufficient storage to complete the request.";
							Item.SmtpStatusCodeValue = 452;
							break;
						case SmtpStatusCode.LocalErrorInProcessing:
							Item.ShortDescription = Enum.GetName(StatusCode, SmtpStatusCode.LocalErrorInProcessing);
							Item.LongDescription = 
								"The SMTP service cannot complete the request." + Environment.NewLine +
								"This error can occur if the client's IP address cannot be resolved (that is, a reverse lookup failed)." + Environment.NewLine +
								"You can also receive this error if the client domain has been identified as an open relay or " + Environment.NewLine +
								"source for unsolicited email (spam). For details, see RFC 2505, which is available at https://www.ietf.org.";
							Item.SmtpStatusCodeValue = 451;
							break;
						case SmtpStatusCode.MailboxBusy:
							Item.ShortDescription = Enum.GetName(StatusCode, SmtpStatusCode.MailboxBusy);
							Item.LongDescription = "The destination mailbox is in use.";
							Item.SmtpStatusCodeValue = 450;
							break;
						case SmtpStatusCode.MailboxNameNotAllowed:
							Item.ShortDescription = Enum.GetName(StatusCode, SmtpStatusCode.MailboxNameNotAllowed);
							Item.LongDescription = "The syntax used to specify the destination mailbox is incorrect.";
							Item.SmtpStatusCodeValue = 553;
							break;
						case SmtpStatusCode.MailboxUnavailable:
							Item.ShortDescription = Enum.GetName(StatusCode, SmtpStatusCode.MailboxUnavailable);
							Item.LongDescription = "The destination mailbox was not found or could not be accessed.";
							Item.SmtpStatusCodeValue = 550;
							break;
						case SmtpStatusCode.MustIssueStartTlsFirst:
							Item.ShortDescription = Enum.GetName(StatusCode, SmtpStatusCode.MustIssueStartTlsFirst);
							Item.LongDescription = 
								"The SMTP server is configured to accept only TLS connections, and the SMTP client is" + Environment.NewLine +
								"attempting to connect by using a non-TLS connection. The solution is for the user to set" + Environment.NewLine +
								"EnableSsl=true on the SMTP Client.";
							Item.SmtpStatusCodeValue = 530;
							break;
						case SmtpStatusCode.Ok:
							Item.ShortDescription = Enum.GetName(StatusCode, SmtpStatusCode.Ok);
							Item.LongDescription = "The email was successfully sent to the SMTP service.";
							Item.SmtpStatusCodeValue = 250;
							break;
						case SmtpStatusCode.ServiceClosingTransmissionChannel:
							Item.ShortDescription = Enum.GetName(StatusCode, SmtpStatusCode.ServiceClosingTransmissionChannel);
							Item.LongDescription = "The SMTP service is closing the transmission channel.";
							Item.SmtpStatusCodeValue = 221;
							break;
						case SmtpStatusCode.ServiceNotAvailable:
							Item.ShortDescription = Enum.GetName(StatusCode, SmtpStatusCode.ServiceNotAvailable);
							Item.LongDescription = "The SMTP service is not available; the server is closing the transmission channel.";
							Item.SmtpStatusCodeValue = 421;
							break;
						case SmtpStatusCode.ServiceReady:
							Item.ShortDescription = Enum.GetName(StatusCode, SmtpStatusCode.ServiceReady);
							Item.LongDescription = "The SMTP service is ready.";
							Item.SmtpStatusCodeValue = 220;
							break;
						case SmtpStatusCode.StartMailInput:
							Item.ShortDescription = Enum.GetName(StatusCode, SmtpStatusCode.StartMailInput);
							Item.LongDescription = "The SMTP service is ready to receive the email content.";
							Item.SmtpStatusCodeValue = 354;
							break;
						case SmtpStatusCode.SyntaxError:
							Item.ShortDescription = Enum.GetName(StatusCode, SmtpStatusCode.SyntaxError);
							Item.LongDescription = "The syntax used to specify a command or parameter is incorrect.";
							Item.SmtpStatusCodeValue = 501;
							break;
						case SmtpStatusCode.SystemStatus:
							Item.ShortDescription = Enum.GetName(StatusCode, SmtpStatusCode.SystemStatus);
							Item.LongDescription = "A system status or system Help reply.";
							Item.SmtpStatusCodeValue = 211;
							break;
						case SmtpStatusCode.TransactionFailed:
							Item.ShortDescription = Enum.GetName(StatusCode, SmtpStatusCode.TransactionFailed);
							Item.LongDescription = "The transaction failed.";
							Item.SmtpStatusCodeValue = 554;
							break;
						case SmtpStatusCode.UserNotLocalTryAlternatePath:
							Item.ShortDescription = Enum.GetName(StatusCode, SmtpStatusCode.UserNotLocalTryAlternatePath);
							Item.LongDescription = 
								"The user mailbox is not located on the receiving server. You should resend using the supplied address information.";
							Item.SmtpStatusCodeValue = 551;
							break;
						case SmtpStatusCode.UserNotLocalWillForward:
							Item.ShortDescription = Enum.GetName(StatusCode, SmtpStatusCode.UserNotLocalWillForward);
							Item.LongDescription = 
								"The user mailbox is not located on the receiving server; the server forwards the email.";
							Item.SmtpStatusCodeValue = 251;
							break;
						default:
							Item.ShortDescription = "Unknown";
							Item.LongDescription = "Unknown condition";
							Item.SmtpStatusCodeValue = -999;
							break;
				}
			}
			return Result;
		}		
		
		public static string EmailExceptionResultJson(Exception Source)			
		{
			List<SmtpExceptionItem> Items = EmailExceptionResult(Source);
			List<string> JsonItems = new List<string>();
			List<string> JsonItem = new List<string>();
			string Str = "";
			
			foreach (SmtpExceptionItem Item in Items)
			{
				JsonItem.Clear();
				JsonItem.Add(JsonUtilities.AddPropertyNameAndValue("ShortDescription", Item.ShortDescription));
				JsonItem.Add(JsonUtilities.AddPropertyNameAndValue("LongDescription", Item.LongDescription));
				JsonItem.Add(JsonUtilities.AddPropertyNameAndValue("SmtpStatusCodeValue", Item.SmtpStatusCodeValue, false));
				Str =
					JsonUtilities.StartJsonObject("") +
					string.Join(",", JsonItem.ToArray()) +
					JsonUtilities.EndJsonObject();
				JsonItems.Add(Str);
			}
			
			return string.Join(",", JsonItems.ToArray());
		}
	}
}
