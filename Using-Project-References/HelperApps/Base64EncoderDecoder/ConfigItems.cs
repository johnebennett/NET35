/*
 * Created by SharpDevelop.
 * User: johne
 * Date: 27/03/2025
 * Time: 10:55 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using Newtonsoft.Json;

namespace Base64EncoderDecoder
{
	/// <summary>
	/// Description of ConfigItems.
	/// </summary>
	public class ConfigItems
	{
		
		public string WriteToOutput { get; set; }

		public ConfigItems()
		{
			WriteToOutput = "N";
		}

        private static string LocalAppDir()
        {
            string Result = "";

            Result = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            if (!Result.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal))
            {
            	Result = Result + Path.DirectorySeparatorChar.ToString();
            }

            return Result;
        }
		
		public static ConfigItems Load(out string ErrorMsg)
		{
			ErrorMsg = "";
			string ConfigFileName = LocalAppDir() + "Base64EncoderDecoder.json";
			
            Console.WriteLine("ConfigFileName: " + ConfigFileName);
            if (!File.Exists(ConfigFileName))
            {
                Console.WriteLine(string.Format("Cannot find the configuration file: {0}", ConfigFileName));
                Console.WriteLine("Continuing without configuration values");
                ErrorMsg = 
                	string.Format("Cannot find the configuration file: {0}", ConfigFileName) +
                	Environment.NewLine +
                	"Continuing without configuration values";
                return new ConfigItems();
            }
            else
            {
            	string ConfigContent = File.ReadAllText(ConfigFileName);
            	return JsonConvert.DeserializeObject<ConfigItems>(ConfigContent);
            }			
		}
	}
}
