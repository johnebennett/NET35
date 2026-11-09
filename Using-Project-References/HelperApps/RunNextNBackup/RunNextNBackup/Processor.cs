using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace RunNextNBackup;

public class Processor
{
	private const string m_BackupFilesArgs = "*.nbk";

	private string m_CurrentDir = "";

	private string m_NBackupDir = "";

	private string m_ExecutingDir = "";

	private string m_ConfigFileName = "";

	private string m_ZipFileName = "";

	private string m_GStatFileName = "";

	private string m_DBLocation = "";

	private string m_DBName = "";

	private string m_NBackupArgs = "-u sysdba -p !bcdeaoffice! -b {0} localhost:{1}{2}";

	private int m_DeleteFilesTries = -1;

	private int m_NBackupCmdTries = -1;

	private int m_GStatAndZipTries = -1;

	private int m_BackupNumber = -1;

	public void Execute(string DeleteBackupFiles)
	{
		string text = "";
		CommonMethods.WriteToOutput("Inside Processor.Execute");
		CommonMethods.WriteToOutput("DeleteBackupFiles: " + DeleteBackupFiles);
		if (string.IsNullOrEmpty(text))
		{
			text = LoadConfigValues();
		}
		if (DeleteBackupFiles == "Y")
		{
			text = DeleteExistingBackupFiles();
		}
		if (string.IsNullOrEmpty(text))
		{
			CommonMethods.WriteToOutput("ConfigFileName: " + m_ConfigFileName);
			CommonMethods.WriteToOutput("NBackupDir: " + m_NBackupDir);
			CommonMethods.WriteToOutput("ExecutingDir: " + m_ExecutingDir);
			CommonMethods.WriteToOutput("CurrentDir: " + m_CurrentDir);
			CommonMethods.WriteToOutput("ZipFileName: " + m_ZipFileName);
			CommonMethods.WriteToOutput("GStatFileName: " + m_GStatFileName);
			CommonMethods.WriteToOutput("NBackupArgs: " + m_NBackupArgs);
			CommonMethods.WriteToOutput("DeleteFilesTries: " + m_DeleteFilesTries);
			CommonMethods.WriteToOutput("NBackupCmdTries: " + m_NBackupCmdTries);
			CommonMethods.WriteToOutput("GStatAndZipTries: " + m_GStatAndZipTries);
			CommonMethods.WriteToOutput("HighestBackupNumber: " + m_BackupNumber);
		}
		checked
		{
			if (string.IsNullOrEmpty(text))
			{
				CommonMethods.WriteToOutput("Before running NBackup Command");
				if (m_BackupNumber > -1)
				{
					m_BackupNumber++;
				}
				m_NBackupArgs = string.Format(m_NBackupArgs, m_BackupNumber, m_DBLocation, m_DBName);
				text = CommonMethods.RunCommandLineOperationEx("nbackup", m_NBackupArgs, m_NBackupCmdTries);
			}
			if (string.IsNullOrEmpty(text))
			{
				text = RunGStatCommand(m_DBLocation, m_DBName, m_GStatFileName, m_GStatAndZipTries);
			}
			if (string.IsNullOrEmpty(text))
			{
				text = ZipGStatResultsFile(m_DBLocation, m_ZipFileName, m_GStatFileName, m_GStatAndZipTries);
			}
			if (string.IsNullOrEmpty(text) && File.Exists(m_DBLocation + m_GStatFileName))
			{
				File.Delete(m_DBLocation + m_GStatFileName);
			}
			if (!string.IsNullOrEmpty(text))
			{
				CommonMethods.WriteToOutput(text);
				Console.WriteLine(text);
			}
			Console.WriteLine("================================================================================");
			Console.WriteLine();
			if (!string.IsNullOrEmpty(text))
			{
				CommonMethods.WriteToOutput(text);
			}
		}
	}

	private string LoadConfigValues()
	{
		string ErrorMsg = "";
		JObject source = null;
		if (string.IsNullOrEmpty(ErrorMsg))
		{
			m_CurrentDir = Directory.GetCurrentDirectory();
			m_ExecutingDir = new Uri(Assembly.GetExecutingAssembly().GetName().CodeBase).LocalPath;
			m_ExecutingDir = CommonMethods.AddTrailingPathSeparator(Path.GetDirectoryName(m_ExecutingDir));
			m_ConfigFileName = m_ExecutingDir + "App.Config";
			source = CommonMethods.LoadJSONConfigSource(m_ConfigFileName, out ErrorMsg);
		}
		if (string.IsNullOrEmpty(ErrorMsg))
		{
			m_NBackupDir = JsonUtilities.GetJsonPathValueStr(source, "$.NBackupDir", out ErrorMsg);
			m_NBackupDir = CommonMethods.AddTrailingPathSeparator(m_NBackupDir);
		}
		if (string.IsNullOrEmpty(ErrorMsg))
		{
			m_DBLocation = JsonUtilities.GetJsonPathValueStr(source, "$.DBLocation", out ErrorMsg);
			m_DBLocation = CommonMethods.AddTrailingPathSeparator(m_DBLocation);
		}
		if (string.IsNullOrEmpty(ErrorMsg))
		{
			m_DBName = JsonUtilities.GetJsonPathValueStr(source, "$.DBName", out ErrorMsg);
		}
		bool flag = false;
		if (string.IsNullOrEmpty(ErrorMsg))
		{
			flag = int.TryParse(JsonUtilities.GetJsonPathValueStr(source, "$.DeleteFilesTries", out ErrorMsg), out m_DeleteFilesTries);
			if (string.IsNullOrEmpty(ErrorMsg) && !flag)
			{
				ErrorMsg = "Error converting the value for DeleteFilesTries";
				CommonMethods.WriteToOutput(ErrorMsg);
			}
		}
		if (string.IsNullOrEmpty(ErrorMsg))
		{
			flag = int.TryParse(JsonUtilities.GetJsonPathValueStr(source, "$.NBackupCmdTries", out ErrorMsg), out m_NBackupCmdTries);
			if (string.IsNullOrEmpty(ErrorMsg) && !flag)
			{
				ErrorMsg = "Error converting the value for NBackupCmdTries";
				CommonMethods.WriteToOutput(ErrorMsg);
			}
		}
		if (string.IsNullOrEmpty(ErrorMsg))
		{
			flag = int.TryParse(JsonUtilities.GetJsonPathValueStr(source, "$.GStatAndZipTries", out ErrorMsg), out m_GStatAndZipTries);
			if (string.IsNullOrEmpty(ErrorMsg) && !flag)
			{
				ErrorMsg = "Error converting the value for GStatAndZipTries";
				CommonMethods.WriteToOutput(ErrorMsg);
			}
		}
		if (string.IsNullOrEmpty(ErrorMsg))
		{
			m_BackupNumber = GetNextBackupNumber(m_NBackupDir, "*.nbk", out ErrorMsg);
		}
		if (string.IsNullOrEmpty(ErrorMsg))
		{
			DateTime now = DateTime.Now;
			m_ZipFileName = m_DBLocation + now.ToString("yyyyMMdd") + ".zip";
			m_GStatFileName = now.ToString("yyyyMMdd_HHmm") + "_GSTAT.txt";
		}
		return ErrorMsg;
	}

	private int GetNextBackupNumber(string NBackupDir, string FilePattern, out string ErrorMsg)
	{
		int num = -1;
		ErrorMsg = "";
		CommonMethods.WriteToOutput("Inside GetNextBackupNumber");
		string[] array = NBackupFileList(NBackupDir, FilePattern, out ErrorMsg);
		string text = "";
		int result = -1;
		bool flag = false;
		string[] array2 = array;
		foreach (string text2 in array2)
		{
			CommonMethods.WriteToOutput("Procesing File: " + text2);
			text = text2.Split('-')[1];
			if (int.TryParse(text, out result) && result > num)
			{
				num = result;
			}
		}
		return num;
	}

	private string[] NBackupFileList(string NBackupDir, string FilePattern, out string ErrorMsg)
	{
		ErrorMsg = "";
		string[] result = new string[0];
		CommonMethods.WriteToOutput("Inside NBackupFileList");
		try
		{
			result = Directory.GetFiles(NBackupDir, FilePattern);
		}
		catch (Exception ex)
		{
			ErrorMsg = ex.Message;
		}
		return result;
	}

	private string DeleteExistingBackupFiles()
	{
		string ErrorMsg = "";
		string[] array = NBackupFileList(m_NBackupDir, "*.nbk", out ErrorMsg);
		if (string.IsNullOrEmpty(ErrorMsg))
		{
			try
			{
				string[] array2 = array;
				foreach (string path in array2)
				{
					if (File.Exists(path))
					{
						File.Delete(path);
					}
				}
			}
			catch (Exception ex)
			{
				ErrorMsg = ex.Message;
			}
		}
		if (string.IsNullOrEmpty(ErrorMsg))
		{
			m_BackupNumber = -1;
		}
		return ErrorMsg;
	}

	private string RunGStatCommand(string DBLocation, string DBName, string GStatFileName, int GStatAndZipTries)
	{
		string result = "";
		string currentDirectory = Directory.GetCurrentDirectory();
		string format = "-h -user SYSDBA -password !bcdeaoffice! {0}";
		string command = DBLocation + "gstat";
		try
		{
			Directory.SetCurrentDirectory(DBLocation);
			format = string.Format(format, DBLocation + DBName);
			result = CommonMethods.RunCommandLineOperationEx(command, format, GStatFileName, GStatAndZipTries);
		}
		finally
		{
			Directory.SetCurrentDirectory(currentDirectory);
		}
		return result;
	}

	private string ZipGStatResultsFile(string DBLocation, string ZipFileName, string GStatFileName, int GStatAndZipTries)
	{
		string result = "";
		string currentDirectory = Directory.GetCurrentDirectory();
		string format = "a -r {0} {1}";
		string command = DBLocation + "rpzip";
		try
		{
			Directory.SetCurrentDirectory(DBLocation);
			format = string.Format(format, ZipFileName, GStatFileName);
			result = CommonMethods.RunCommandLineOperationEx(command, format, GStatAndZipTries);
		}
		finally
		{
			Directory.SetCurrentDirectory(currentDirectory);
		}
		return result;
	}
}
