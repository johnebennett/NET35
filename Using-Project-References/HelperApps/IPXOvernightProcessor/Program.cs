/*
 * Created by SharpDevelop.
 * User: johne
 * Date: 29/12/2025
 * Time: 1:42 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Reflection;
using CommonUtilities;
using EAOfficeAPI;
using IPXOvernightProcessor.Actions;
using System.IO;

namespace IPXOvernightProcessor
{
	class Program
	{
		private static string m_ConnectionStr = "";
		//private static Int64 m_EVA_ID = 0;
		private static ConfigOptions config = new ConfigOptions();

		public static void Main(string[] args)
		{
			string ErrorMsg = "";
			//Console.WriteLine("Before calling ApiSettings");
			using (new ConsoleSilencer())
			{
				APISettingsItem ApiSettings = APISettingsFile.GetAPISettings(APIAppType.Service, out ErrorMsg);
			}
			//Console.WriteLine("After calling ApiSettings");

			if (string.IsNullOrEmpty(ErrorMsg))
			{
				using (new ConsoleSilencer())
				{
					config = ConfigOptions.LoadConfigOptions();
				}

				//Console.WriteLine("After setting config values");


				/*
				Console.WriteLine("TargetAppName: " + config.TargetAppName);

				if (!File.Exists(config.TargetAppName))
				{
					CommonMethods.WriteToOutput("The file: " + config.TargetAppName + " does not exist.");
					return;
				}
				*/

				ErrorMsg = ConfigOptions.ValidateConfigOptions(config);
				if (!string.IsNullOrEmpty(ErrorMsg))
				{
					CommonMethods.WriteToOutput(ErrorMsg);
					return;
				}

				CommonMethods.WriteToOutput("The file exists", config.ShowDebugMessages);

				CommonMethods.WriteToOutput("Before getting the Connection String", config.ShowDebugMessages);
				m_ConnectionStr = CommonMethods.ConnectionString(out ErrorMsg);
				CommonMethods.WriteToOutput("Connection String: " + m_ConnectionStr, config.ShowDebugMessages);
				CommonMethods.WriteToOutput("After getting the Connection String", config.ShowDebugMessages);
				bool testConnection = false;
				using (new ConsoleSilencer())
				{
					testConnection = CommonUtilities.FirebirdUtilities.TestDBConnection(m_ConnectionStr, out ErrorMsg);
				}
				
				if (!testConnection)
				{
					Console.WriteLine(ErrorMsg);
				}
				else
				{
					Console.WriteLine("Connection Successful");					
					new IPXOvernightProcessor.Actions.Processor().Execute(m_ConnectionStr, config);
				};
			}
			else
			{
				Console.WriteLine(ErrorMsg);
			}
		}
	}
}