/*
 * Created by SharpDevelop.
 * User: johne
 * Date: 29/12/2025
 * Time: 1:50 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using System.Linq;
//using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Diagnostics.Contracts;

namespace IPXOvernightProcessor.Actions
{
    public class ConfigOptions
    {
        public string TargetAppName { get; set; }
        public string ShowDebugMessages { get; set; }
        public string QueryText { get; set; }
        public int HasExitedTries { get; set; }
        public int SleepTimeMilliseconds { get; set; }
        public int ClientToProcess { get; set; }
        public string ShowCommandLinesOnly { get; set; }
        
        public ConfigOptions()
        {
        	TargetAppName = "";
            ShowDebugMessages = "N";
            QueryText = "";
            HasExitedTries = 300;
            SleepTimeMilliseconds = 1000;
            ClientToProcess = 0;
            ShowCommandLinesOnly = "Y";
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

        public static ConfigOptions LoadConfigOptions()
        {
            string ConfigFileName = "IPXOvernightProcessor.json";
            return LoadConfigOptions(ConfigFileName);
        }

        public static ConfigOptions LoadConfigOptions(string ConfigFileName)
        {
            ConfigFileName = LocalAppDir() + ConfigFileName;
            Console.WriteLine("ConfigFileName: " + ConfigFileName);
            if (!File.Exists(ConfigFileName))
            {
                Console.WriteLine(string.Format("Cannot find the configuration file: {0}", ConfigFileName));
                Console.WriteLine("Continuing without an configuration values");
                return new ConfigOptions();
            }
            else
            {
            	string ConfigContent = File.ReadAllText(ConfigFileName);
            	return JsonConvert.DeserializeObject<ConfigOptions>(ConfigContent);
            }
        }

        public static string ValidateConfigOptions(ConfigOptions options)
        {
            string Result = "";

            if (string.IsNullOrEmpty(Result))
            {
                if (!File.Exists(options.TargetAppName))
                {
                    Result = "The file: " + options.TargetAppName + " does not exist.";
                }
            }

            if (string.IsNullOrEmpty(Result))
            {
                if (string.IsNullOrEmpty(options.QueryText))
                {
                    Result = "No query text has been set.";
                }
            }

            return Result;
        }
    }
}