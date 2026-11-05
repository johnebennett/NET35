/*
 * Created by SharpDevelop.
 * User: John
 * Date: 23/10/2018
 * Time: 7:50 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Reflection;

namespace CommonUtilities
{
	public partial class WindowsEventLoggingUtilities
	{
		public const string EAOfficeSourceName = "EAOffice";

		public const double EventViewerLogDefaultByteLimit = 30000.0;

		private static string WriteToEventLog(string aMessage, string aSource, EventLogEntryType aType, int aEventID)
		{
			string Result = "";
			
			if (string.IsNullOrEmpty(Result))
			{
				string Level = EventLogEntryTypeToString(aType);
				Result = LogEventViewerRecord(Level, aSource, aEventID, aMessage);
			}
			
			if (!string.IsNullOrEmpty(Result))
			//if (string.IsNullOrEmpty(Result))
			{
				WriteToErrorEvent(Result, "Error from WriteToEventLog", 0);
				/*
				try
				{
					if (aMessage.Length > 32766) aMessage = aMessage.Substring(1, 32000);
					string sLog = "Application";
					if (!EventLog.SourceExists(aSource))
					{
						EventLog.CreateEventSource(aSource, sLog);
					}
					EventLog.WriteEntry(aSource, Result + Environment.NewLine + aMessage, aType, aEventID);
				}
				catch (Exception ex)
				{
					Result = ex.Message;
					Console.WriteLine(ex.Message);
					Console.WriteLine("Method: WriteToEventLog");
					Console.WriteLine("Source: " + aSource);
				}
				*/
			}
			return Result;
		}

		public static string WriteToErrorEvent(string aMessage, string aSource, int aEventID)
		{
			//return WindowsEventLoggingUtilities.WriteToEventLog(aMessage, aSource, EventLogEntryType.Error, aEventID);
			string Result = "";
			
			if (string.IsNullOrEmpty(Result))
			{
				if (string.IsNullOrEmpty(aSource))
				{
					aSource = "The provided Source is empty";
				}
				try
				{
					if (aMessage.Length > 32766) aMessage = aMessage.Substring(1, 32000);
					string sLog = "Application";
					if (!EventLog.SourceExists(aSource))
					{
						EventLog.CreateEventSource(aSource, sLog);
					}
					EventLog.WriteEntry(aSource, Result + Environment.NewLine + aMessage, EventLogEntryType.Error, aEventID);
				}
				catch (Exception ex)
				{
					Result = "Method: WriteToErrorEvent" + Environment.NewLine +ex.Message;
				}
			}
			return Result;
		}

		public static string WriteToInfoEvent(string aMessage, string aSource, int aEventID)
		{
			return WindowsEventLoggingUtilities.WriteToEventLog(aMessage, aSource, EventLogEntryType.Information, aEventID);
		}

		public static string WriteToWarningEvent(string aMessage, string aSource, int aEventID)
		{
			return WindowsEventLoggingUtilities.WriteToEventLog(aMessage, aSource, EventLogEntryType.Warning, aEventID);
		}

		public static string GetMessageSubStringForEventViewer(string aMessage, double aByteLimit)
		{
			double lLimit = aByteLimit / 2.0;
			double lLengthLimit = Math.Truncate(lLimit);
			string lActualMessage = aMessage;
			if ((double)lActualMessage.Length > lLengthLimit)
			{
				lActualMessage = lActualMessage.Substring(1, (int)lLengthLimit) + Environment.NewLine + "*** Text Truncated for EventViewer  ***";
			}
			return lActualMessage;
		}
		
		private static string EventLogEntryTypeToString(EventLogEntryType Type)
		{
			string Result = "Unknown";
			switch (Type)
			{
				case EventLogEntryType.Error: Result = "Error"; break;
				case EventLogEntryType.FailureAudit: Result = "Failure Audit"; break;
				case EventLogEntryType.Information: Result = "Information"; break;
				case EventLogEntryType.SuccessAudit: Result = "Success Audit"; break;
				case EventLogEntryType.Warning: Result = "Warning"; break;
				default: Result = "Unknown"; break;
			}
			return Result;
		}
		
		
	}
}
