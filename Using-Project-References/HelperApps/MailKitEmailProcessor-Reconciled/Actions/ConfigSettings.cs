using System;
using System.IO;
using Newtonsoft.Json;

namespace MailKitEmailProcessor.Actions
{
	public class ConfigSettings
	{
		public string ShareCreateDocsLocation { get; set; }

		public string ShareWordLocation { get; set; }

		public string ShareTestRPOLocation { get; set; }

		public int RetryCount { get; set; }

		public string ShareDir { get; set; }

		public string LocalCreateDocsLocation { get; set; }

		public string LocalDocsLocation { get; set; }

		public string LocalWordLocation { get; set; }

		public string LocalTestRPOLocation { get; set; }

		public string SendEmail { get; set; }

		public string UpdateSentValues { get; set; }

		public string RunningOnEmailServer { get; set; }

		public static ConfigSettings LoadFromJson(string filePath)
		{
			if (!File.Exists(filePath))
			{
				CommonMethods.WriteToErrorOutput("The specified configuration file does not exist: " + filePath);
				throw new FileNotFoundException("The specified configuration file does not exist.", filePath);
			}
			string value = File.ReadAllText(filePath);
			try
			{
				return JsonConvert.DeserializeObject<ConfigSettings>(value);
			}
			catch (Exception ex)
			{
				CommonMethods.WriteToErrorOutput(ex.Message);
				return new ConfigSettings();
			}
		}
	}
}
