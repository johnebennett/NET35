using System;

namespace RunNextNBackup;

internal class Program
{
	public static void Main(string[] args)
	{
		Console.WriteLine("Run NextNBackup");
		string text = "N";
		if (args.Length == 1)
		{
			text = args[checked(args.Length - 1)].ToUpper();
		}
		if (!string.IsNullOrEmpty(text) && text != "Y")
		{
			text = "N";
		}
		new Processor().Execute(text);
	}
}
