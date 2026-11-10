/*
 * Created by SharpDevelop.
 * User: johne
 * Date: 30/10/2023
 * Time: 4:29 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using CommonUtilities;
using MailKitEmailProcessor.Actions;

namespace MailKitEmailProcessor
{

	class Program
	{
	    static void Main(string[] args)
	    {
			if (args.Length != 1)
			{
				Console.WriteLine("There must be only one command line parameter");
				Console.WriteLine("which can either be the RunId to process or");
				Console.WriteLine("the filename to process.");
				return;
			}
		
			string Result = "";
			string arg = args[(args.Length - 1)];
			int RunId = StringUtilities.StrToIntDef(arg, 0);
			if (RunId > 0)
			{
				Console.WriteLine("Processing the RunId: " + RunId);
				//Console.WriteLine("An invalid RunId value has been provided.");
				// Process the runId
				try 
				{
					Result = new Actions.RunIdProcessor().Execute(RunId);
				}
				catch (Exception Ex)
				{
					CommonMethods.WriteToOutput(
						RunId.ToString(),
						Ex.Message + Environment.NewLine + Ex.StackTrace);
				}
				//Console.WriteLine(Result);
				//return;
			}
			else if (arg.StartsWith(@"/FromFile:"))
			{
				arg = arg.Replace(@"/FromFile:", "");
				Result = new Actions.FileNameProcessor().Execute(arg);
				//return;
			}
			else
			{
				Console.WriteLine("Empty or invalid command line provided");
			}
			
			if (!string.IsNullOrEmpty(Result))
			{
				Console.WriteLine(Result);
			}
			
			return;
	    }
	}
}