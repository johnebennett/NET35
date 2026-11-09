using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using Newtonsoft.Json.Linq;

namespace RunNextNBackup
{
public class CommonMethods
{
	public static void WriteToOutput(string LineText)
	{
		WriteToOutput(LineText, AlsoWriteToConsole: true);
	}

	public static void WriteToOutput(string LineText, bool AlsoWriteToConsole)
	{
		string executingDirectory = GetExecutingDirectory(IncludePathSeparator: true);
		string text = DateTime.Today.ToString("yyyyMMdd");
		string text2 = DateTime.Now.ToString("HH:mm:ss") + " - ";
		string text3 = "RunNextNBackup_" + text + ".log";
		text3 = executingDirectory + text3;
		using (StreamWriter streamWriter = new StreamWriter(text3, append: true))
		{
			streamWriter.WriteLine(text + " " + text2 + LineText);
		}
		if (AlsoWriteToConsole)
		{
			Console.WriteLine(LineText);
		}
	}

	public static string DateTimeToYYYYMMDD(DateTime Source)
	{
		string text = Source.Year.ToString();
		string text2 = Source.Month.ToString();
		if (text2.Length == 1)
		{
			text2 = "0" + text2;
		}
		text += text2;
		text2 = Source.Day.ToString();
		if (text2.Length == 1)
		{
			text2 = "0" + text2;
		}
		return text + text2;
	}

	public static string GetExecutingDirectory(bool IncludePathSeparator)
	{
		string localPath = new Uri(Assembly.GetExecutingAssembly().GetName().CodeBase).LocalPath;
		localPath = Path.GetDirectoryName(localPath);
		if (IncludePathSeparator)
		{
			localPath = AddTrailingPathSeparator(localPath);
		}
		return localPath;
	}

	public static string AddTrailingPathSeparator(string Source)
	{
		string text = Source;
		string text2 = Path.DirectorySeparatorChar.ToString();
		if (!text.EndsWith(text2))
		{
			text += text2;
		}
		return text;
	}

	public static JObject LoadJSONConfigSource(string FileName, out string ErrorMsg)
	{
		JObject val = null;
		ErrorMsg = "";
		string text = File.ReadAllText(FileName);
		if (string.IsNullOrEmpty(text))
		{
			ErrorMsg = "Cannot read the contents of the config file";
			WriteToOutput(ErrorMsg);
			return null;
		}
		if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(ErrorMsg))
		{
			WriteToOutput("Before parse");
			val = JObject.Parse(text);
			WriteToOutput("After parse");
			if (val == null)
			{
				ErrorMsg = "Cannot parse the config source";
				WriteToOutput(ErrorMsg);
				return null;
			}
		}
		return val;
	}

	public static string RunCommandLineOperationEx(string Command, string Args, int Tries)
	{
		return RunCommandLineOperationEx(Command, Args, "", Tries);
	}

	public static string RunCommandLineOperationEx(string Command, string Args, string OutputFileName, int Tries)
	{
		string text = "";
		string text2 = "RunCommandLineOperation";
		if (string.IsNullOrEmpty(Command))
		{
			return "Invalid or empty Command Line provided in " + text2;
		}
		if (Tries <= 0)
		{
			Tries = 1200;
		}
		if (string.IsNullOrEmpty(text))
		{
			Process process = new Process();
			process.StartInfo.FileName = Command;
			process.StartInfo.Arguments = Args;
			if (!string.IsNullOrEmpty(OutputFileName))
			{
				process.StartInfo.UseShellExecute = false;
				process.StartInfo.RedirectStandardOutput = true;
			}
			WriteToOutput("AppName: " + process.StartInfo.FileName);
			WriteToOutput("Args: " + process.StartInfo.Arguments);
			process.Start();
			string text3 = "";
			for (int i = 0; i < Tries; i = checked(i + 1))
			{
				if (!process.HasExited)
				{
					Thread.Sleep(1000);
				}
			}
			if (!process.HasExited)
			{
				text = "Application did not finish in time.";
			}
			else if (!string.IsNullOrEmpty(OutputFileName))
			{
				text3 = process.StandardOutput.ReadToEnd();
			}
			WriteToOutput("After calling: " + Command + " Result: " + text);
			if (!string.IsNullOrEmpty(OutputFileName))
			{
				Console.WriteLine(text3);
				File.WriteAllText(OutputFileName, text3);
			}
		}
		return text;
	}
}
}