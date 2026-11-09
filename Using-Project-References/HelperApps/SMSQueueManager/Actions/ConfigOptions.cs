using System;
using System.Collections.Generic;
using System.Linq;
//using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace SMSQueueManager.Actions
{
    public class ConfigOptions
    {
        public string CompanyIdIn { get; set; }
        public string CompanyIdNotIn { get; set;}
        public string DelayBetweenSearches { get; set; }
        public string MaxNumberOfInstances { get; set; }
        public string TargetAppName { get; set; }
        public string BuildWhereClause { get; set; }
        public string SelectQuery { get; set; }
        public string ActivitySelector { get; set; }
        public string ActivityCount { get; set; }
        public string ShowDebugMessages { get; set; }
        
        public ConfigOptions()
        {
        	CompanyIdIn = "";
        	CompanyIdNotIn = "";
        	DelayBetweenSearches = "";
        	MaxNumberOfInstances = "";
        	TargetAppName = "";
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
            string ConfigFileName = LocalAppDir() + "SMSQueueManager.json";
            Console.WriteLine("ConfigFileName: " + ConfigFileName);
            if (!File.Exists(ConfigFileName))
            {
                Console.WriteLine(string.Format("Cannot find the configuration file: {0}", ConfigFileName));
                Console.WriteLine("Continuing without any configuration values");
                return new ConfigOptions();
            }
            else
            {
            	string ConfigContent = File.ReadAllText(ConfigFileName);
            	return JsonConvert.DeserializeObject<ConfigOptions>(ConfigContent);
            }
        }
    }
}