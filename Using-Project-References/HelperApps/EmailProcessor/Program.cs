/*
 * Created by SharpDevelop.
 * User: jbennett
 * Date: 10/10/2018
 * Time: 2:39 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
/*
using System.ServiceModel;
using System.ServiceModel.Description;
using System.IO;
using System.Configuration;
*/
using EAOfficeAPI;
using CommonUtilities;

namespace EmailProcessor	
{
	class Program
	{
		//public static void Main(string[] args)
		public static void Main(string[] args)
		{
            // if there are not exactly one command line args just return
			if (args.Length != 1)
			{
				Console.WriteLine("There must be only one command line parameter");
				Console.WriteLine("which must be the RunId to process.");
				return;
			}

			string RunId = args[args.Length - 1];
			int RunIdValue = CommonUtilities.StringUtilities.StrToIntDef(RunId, -1);
			if (RunIdValue < 0)
			{
				Console.WriteLine("An invalid RunId value has been provided.");
				return;
			}
			
			Console.WriteLine("Processing the RunId: " + RunIdValue);
			
			try 
			{
				string Str = new EmailProcessor().ProcessEmailRun(RunIdValue);
				Console.WriteLine(Str);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				Console.WriteLine(ex.StackTrace);
			}			
		}
	}
}