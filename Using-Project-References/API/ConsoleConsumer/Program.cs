/*
 * Created by SharpDevelop.
 * User: jbennett
 * Date: 10/10/2018
 * Time: 3:05 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.ServiceModel;
using System.Xml;
using CommonUtilities;

namespace ConsoleConsumer
{
	class Program
	{
		public static void Main(string[] args)
		{
			// if there are no command line args display the usage details
			if (args.Length == 0)
			{
				Console.WriteLine("usage: RPOfficaAdminConsumer <options> SourceFile.json");
				Console.WriteLine("Options:");
				Console.WriteLine("/D\tSend debug info to the console");
				Console.WriteLine();
				Console.WriteLine("Options must be separated with a space from each other.");
				Console.WriteLine();
				Console.WriteLine("The SourceFile must be the last arguement on the command line.");
				Console.WriteLine("The SourceFile must contain valid JSON.");
				Console.WriteLine();
				return;
			}

			string SourceFile = args[args.Length - 1];
			if (!File.Exists(SourceFile))
			{
				Console.WriteLine("FileName: {0} does not exist", SourceFile);
			}
			
			Console.WriteLine("Processing the file: " + SourceFile);
			bool DebugFlag = false;
			
			string Str = "";
			for (int Index = 0; Index < args.Length - 1; Index++)
			{
				Str = args[Index];
				while (Str.StartsWith("/")) Str = Str.Substring(1);
				Console.WriteLine("arg: " + Str);
				if (CommonUtilities.StringUtilities.SameText(Str, "D")) DebugFlag = true;
			}
			
			string FileContent = File.ReadAllText(SourceFile);

			if (DebugFlag)
			{
				Console.WriteLine(SourceFile);
				Console.WriteLine();
				Console.Write(FileContent);
				Console.WriteLine(); Console.WriteLine();
			}
			
			string ConfigUrl = System.Configuration.ConfigurationManager.AppSettings["ServiceUrl"];
			string Port = System.Configuration.ConfigurationManager.AppSettings["Port"];
			
			Console.WriteLine("Using HTTP Binding");
			string Address = "http://{0}:{1}/EAOfficeAPI";
			Address = string.Format(Address, ConfigUrl, Port);
			Console.WriteLine("Service Location: " + Address);
			
			EndpointAddress endPointAddr = new EndpointAddress(Address);
			BasicHttpBinding binding = new BasicHttpBinding();
			XmlDictionaryReaderQuotas quotas = new XmlDictionaryReaderQuotas();
			quotas.MaxStringContentLength = 2147483647;
			binding.ReaderQuotas = quotas;
			binding.TransferMode = TransferMode.Buffered;
			binding.MaxReceivedMessageSize = 2147483647;
			binding.MaxBufferSize = 2147483647;
			binding.MaxBufferPoolSize = 524288;
			EAOfficeAPIClient client = new EAOfficeAPIClient(binding, endPointAddr);
			
			try 
			{
				Str = client.Execute(FileContent);
				Console.WriteLine(Str);
			}
			catch (System.ServiceModel.EndpointNotFoundException notFound)
			{
				Console.WriteLine("End-Point Not Found Exception");
				Console.WriteLine(notFound.Message);
				//Console.WriteLine(notFound.StackTrace);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				Console.WriteLine(ex.StackTrace);
			}
			finally
			{
				client.Close();
			}
			
			
			client.Close();			
		}
	}
}