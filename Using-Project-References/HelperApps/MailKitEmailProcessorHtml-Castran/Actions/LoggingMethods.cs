using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using CommonUtilities;

namespace MailKitEmailProcessor.Actions
{
	/// <summary>
	/// LoggingMethods
	/// </summary>
	public class LoggingMethods
    {
		private static string GetWorkingDir()
        {
			string WorkingDir = CommonMethods.GetExecutingAssemblyPath();
			string SepChar = Path.DirectorySeparatorChar.ToString();
			if (!WorkingDir.EndsWith(SepChar)) WorkingDir += SepChar;

			return WorkingDir;
        }

		private static string GetTimeStr(bool AppendTrailingDash)
        {
            string TimeStr = DateTime.Now.ToString("HH:mm:ss");
			if (AppendTrailingDash) TimeStr += " - ";
			return TimeStr;
        }

		private static Int64 GetProcessId()
        {
            Int64 ProcessId = Process.GetCurrentProcess().Id;
			return ProcessId;
        }
		public static void WriteToOutput(string LineText)
        {
            //string TimeStr = DateTime.Now.ToString("HH:mm:ss") + " - ";
			string TimeStr = GetTimeStr(true);
            
            Console.WriteLine(TimeStr + LineText);
        }

		public static void WriteToConsole(string LineText, bool ShowProcessId = true)
        {
			string ProcessIdStr = "ProcessId: " + GetProcessId().ToString();
            string TimeStr = GetTimeStr(true);

			if (ShowProcessId)
            {
                Console.WriteLine(TimeStr + ProcessIdStr);
            }

			Console.WriteLine(TimeStr + LineText);
        }

		public static void WriteToLog(string LineText,
		                              Int64 RunId,
		                              bool ShowProcessId,
									  Int64 CompanyClientId,
									  bool WriteToLogFile,
									  bool WriteToConsole,
									  Int64 EVAC_ID = 0,
									  Int64 CurrentEmailNumber = 0,
									  Int64 TotalEmailCount = 0)
        {
			List<string> LineItems = new List<string>();
			Int64 ProcessId = 0;
			if (ShowProcessId)
            {
                ProcessId = GetProcessId();
				LineItems.Add("ProcessId: " + ProcessId.ToString());
            }

			if (CompanyClientId > 0)
            {
                LineItems.Add("CompanyClient ID: " + CompanyClientId.ToString());
            }

			if (EVAC_ID > 0)
            {
                LineItems.Add("EVAC_ID: " + EVAC_ID.ToString());
            }

			string Str = "";
			if (TotalEmailCount > 0)
            {
                Str = "Total Emails: " + TotalEmailCount.ToString();
            }

			if (CurrentEmailNumber > 0)
            {
                Str = "Email: " + CurrentEmailNumber.ToString() + " of " + Str;
            }

			if (!string.IsNullOrEmpty(Str))
            {
                LineItems.Add(Str);
            }

			if (WriteToLogFile)
            {
				string FileName = GetWorkingDir() + "EmailProcessor_" + RunId.ToString() + ".txt";
				using (StreamWriter writer = new StreamWriter(FileName, true))
				{
					if (LineItems.Count > 0)
					{
						foreach (string LineItem in LineItems)
						{
							writer.WriteLine(LineItem);
							if (WriteToConsole)
								Console.WriteLine(LineItem);
						}
					}
					writer.WriteLine(LineText);
					if (WriteToConsole)
						Console.WriteLine(LineText);
				}
            }

			if (WriteToConsole)
            {
				if (LineItems.Count > 0)
				{
					foreach (string LineItem in LineItems)
					{
						Console.WriteLine(LineItem);
					}
				}
				Console.WriteLine(LineText);
            }			
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
        
        public static void WriteParamsToOutput(Int64 UniqueFileId, string QueryText, List<FbParameterItem> Params)
        {
        	WriteParamsToOutput(Convert.ToString(UniqueFileId), QueryText, Params);
        }

        public static void WriteToOutputLog(int RunId, string LineText)
        {
			string FileName = "EmailProcessor_" + RunId.ToString() + ".txt";
			string WorkingDir = CommonMethods.GetExecutingAssemblyPath();
			FileName = WorkingDir + FileName;
			using (StreamWriter writer = new StreamWriter(FileName, true))
			{
				writer.WriteLine(LineText);
				Console.WriteLine(LineText);
			}
        }
        
        public static void WriteToOutputLog(Int64 RunId, string LineText)
        {
        	WriteToOutputLog(Convert.ToInt32(RunId), LineText);
        }
        
        public static void WriteParamsToOutputLog(Int64 RunId, string QueryText, List<FbParameterItem> Params)
        {
        	StringBuilder sb = new StringBuilder();
        	sb.AppendLine("QueryText:");
        	sb.AppendLine(QueryText);
        	
        	foreach (FbParameterItem Item in Params)
        	{
        		sb.AppendLine(Item.ParameterName + ": " + Convert.ToString(Item.ParameterValue));
        	}
        	
        	WriteToOutputLog(RunId, sb.ToString());
        }
        public static void WriteToOutput(string UniqueFileId, string LineText)
        {
            string WorkingDir = GetWorkingDir();
            string OutputFileName = WorkingDir + UniqueFileId + ".txt";
            Console.WriteLine("Output FileName: " + OutputFileName);
            string TimeStr = DateTime.Now.ToString("HH:mm:ss") + " - ";
            
            Console.WriteLine(TimeStr + LineText);
            File.AppendAllText(OutputFileName, TimeStr + LineText + Environment.NewLine);
        }
        
		public static void WriteToErrorOutput(Int64 RunId, string LineText)
        {
            string WorkingDir = GetWorkingDir();
            string TimeStr = DateTime.Now.ToString("HH:mm:ss") + " - ";

			string FileName = "EmailProcessor_";
			if (RunId > 0)
            {
                FileName = FileName + RunId.ToString() + "_Errors";
            }
			else
            {
                FileName = FileName + "Errors";
            }

            FileName = FileName + ".txt";
            FileName = WorkingDir + FileName;
			File.AppendAllText(FileName, TimeStr + LineText);
        }
    } 
}