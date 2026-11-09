/*
 * Created by SharpDevelop.
 * User: johne
 * Date: 15/07/2024
 * Time: 11:09 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace RunNextNBackup
{
	class Program
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
}