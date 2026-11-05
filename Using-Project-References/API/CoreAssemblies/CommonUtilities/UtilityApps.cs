/*
 * Created by SharpDevelop.
 * User: John
 * Date: 25/11/2019
 * Time: 11:43 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Diagnostics;
using System.Reflection;

namespace CommonUtilities
{
	/// <summary>
	/// Description of UtilityApps.
	/// </summary>
	public class UtilityApps
	{
		public UtilityApps()
		{
		}
		private const string FModuleName = "UtilityApps";
		public string ModuleName { get { return FModuleName; } }
		
		public static string HtmlToRtf(string SourceFileName,
		                               string DestinationFileName)
		{
			string Result = "";
			string SepChar = Path.DirectorySeparatorChar.ToString();
			string WorkingDir = Directory.GetCurrentDirectory();
            WorkingDir = FolderHelpers.GetAssemblyDirectoryName(Assembly.GetExecutingAssembly());
			if (!WorkingDir.EndsWith(SepChar)) WorkingDir += SepChar;
			try
			{
				string Args = "";
				if (!string.IsNullOrEmpty(SourceFileName))
					Args = Args + "/srcFileName:" + SourceFileName + " ";
				if (!string.IsNullOrEmpty(DestinationFileName))
					Args = Args + "/destFileName:" + DestinationFileName + " ";
				Process runApp = new Process();
				runApp.StartInfo.FileName = WorkingDir + "HtmlToRTF";
				runApp.StartInfo.Arguments = Args;
				//runApp.StartInfo.Verb = "runas";
				//runApp.StartInfo.UseShellExecute = true;
				runApp.Start();
				int tries = 1000;
				for (int Index = 0; Index < tries; Index++)
				{
					//if (!runApp.HasExited) System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
					if (!runApp.HasExited) System.Threading.Thread.Sleep(1000);
				}
				
						
				if (!runApp.HasExited)
				{
					Result = "Application did not finish in the alloted time.";
				}
			}
			catch (Exception Ex)
			{
				Result = 
					Ex.Message + Environment.NewLine +
					Ex.InnerException + Environment.NewLine +
					Ex.StackTrace;
			}
			finally
			{
				Directory.SetCurrentDirectory(WorkingDir);
			}
			return Result;
		}
		
		public static string HtmlToRtf(string SourceTableName,
		                               string SourceSelectFieldName,
		                               string SourceUniqueFieldName,
		                               int SourceTableId,
		                               string DestinationTableName,
		                               string DestinationSelectFieldName,
		                               string DestinationUniqueFieldName,
		                               int DestinationTableId)
		{
			return
				HtmlToRtf(SourceTableName,
				          SourceSelectFieldName,
				          SourceUniqueFieldName,
				          SourceTableId.ToString(),
				          DestinationTableName,
		                  DestinationSelectFieldName,
		                  DestinationUniqueFieldName,
		                  DestinationTableId.ToString());
		}
		
		public static string HtmlToRtf(string SourceTableName,
		                               string SourceSelectFieldName,
		                               string SourceUniqueFieldName,
		                               string SourceTableId,
		                               string DestinationTableName,
		                               string DestinationSelectFieldName,
		                               string DestinationUniqueFieldName,
		                               string DestinationTableId)
		{
			string Result = "";
			string SepChar = Path.DirectorySeparatorChar.ToString();
			string WorkingDir = Directory.GetCurrentDirectory();
            WorkingDir = FolderHelpers.GetAssemblyDirectoryName(Assembly.GetExecutingAssembly());
            if (!WorkingDir.EndsWith(SepChar)) WorkingDir += SepChar;
			try
			{
				string Args = "";
				if (!string.IsNullOrEmpty(SourceTableName))
					Args = Args + "/srcTableName:" + SourceTableName + " ";
				if (!string.IsNullOrEmpty(SourceSelectFieldName))
					Args = Args + "/srcSelectFieldName:" + SourceSelectFieldName + " ";
				if (!string.IsNullOrEmpty(SourceUniqueFieldName))
					Args = Args + "/srcFieldName:" + SourceUniqueFieldName + " ";
				if (!string.IsNullOrEmpty(SourceTableId))
					Args = Args + "/srcTableId:" + SourceTableId + " ";

				if (!string.IsNullOrEmpty(DestinationTableName))
					Args = Args + "/destTableName:" + DestinationTableName + " ";
				if (!string.IsNullOrEmpty(DestinationSelectFieldName))
					Args = Args + "/destSelectFieldName:" + DestinationSelectFieldName + " ";
				if (!string.IsNullOrEmpty(DestinationUniqueFieldName))
					Args = Args + "/destFieldName:" + DestinationUniqueFieldName + " ";
				if (!string.IsNullOrEmpty(SourceTableId))
					Args = Args + "/destTableId:" + DestinationTableId + " ";
				WindowsEventLoggingUtilities.WriteToInfoEvent("WorkingDir: " + WorkingDir, FModuleName, 0);
				WindowsEventLoggingUtilities.WriteToInfoEvent("Args: " + Args, FModuleName, 0);
				Process runApp = new Process();
				runApp.StartInfo.FileName = WorkingDir + "HtmlToRTF";
				runApp.StartInfo.Arguments = Args;
				//runApp.StartInfo.Verb = "runas";
				//runApp.StartInfo.UseShellExecute = true;
				runApp.Start();
				int tries = 1000;
				for (int Index = 0; Index < tries; Index++)
				{
					//if (!runApp.HasExited) System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
					if (!runApp.HasExited) System.Threading.Thread.Sleep(1000);
				}
				
						
				if (!runApp.HasExited)
				{
					Result = "Application did not finish in the alloted time.";
				}
			}
			catch (Exception Ex)
			{
				Result = 
					Ex.Message + Environment.NewLine +
					Ex.InnerException + Environment.NewLine +
					Ex.StackTrace;
			}
			finally
			{
				Directory.SetCurrentDirectory(WorkingDir);
			}
			return Result;
		}		
	}
}
