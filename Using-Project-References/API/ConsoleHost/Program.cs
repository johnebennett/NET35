/*
 * Created by SharpDevelop.
 * User: jbennett
 * Date: 10/10/2018
 * Time: 2:39 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.IO;
using System.Configuration;
using EAOfficeAPI;
using System.Threading;

namespace ConsoleHost
{
	class Program
	{
		public static void Main(string[] args)
		{
            var PortNumber = ConfigurationManager.AppSettings["PortNumber"];
            var ServerAddress = ConfigurationManager.AppSettings["ServerAddress"];
            string UrlString = "";
            if (string.IsNullOrEmpty(PortNumber))
            {
                UrlString = "http://{0}/EAOfficeAPI";
                UrlString = string.Format(UrlString, ServerAddress);
            }
            else
            {
                UrlString = "http://{0}:{1}/EAOfficeAPI";
                UrlString = string.Format(UrlString, ServerAddress, PortNumber);
            }

            Console.WriteLine("URL: " + UrlString);
            Console.WriteLine();

            Uri[] addressBase = { new Uri(UrlString) };
            ServiceHost selfHost = new ServiceHost(typeof(EAOfficeAPI.EAOfficeAPI), addressBase);            

            try
            {
                ServiceMetadataBehavior mBehave = new ServiceMetadataBehavior();
                selfHost.Description.Behaviors.Add(mBehave);
                
                BasicHttpBinding binding = new BasicHttpBinding();
                /*
                TimeSpan timeSpan = new TimeSpan();
                timeSpan = binding.CloseTimeout;
                ConsoleHost.Program inst = new Program();
                inst.WriteTimeSpan("CloseTimeout", timeSpan);
                inst.WriteTimeSpan("OpenTimeout", binding.OpenTimeout);
                inst.WriteTimeSpan("ReceiveTimeout", binding.ReceiveTimeout);
                inst.WriteTimeSpan("SendTimeout", binding.SendTimeout);
                Console.WriteLine("Resetting Timeouts");
                binding.CloseTimeout = new TimeSpan(0, 25, 0);
                binding.OpenTimeout = new TimeSpan(0, 25, 0);
                binding.ReceiveTimeout = new TimeSpan(0, 25, 0);
                binding.SendTimeout = new TimeSpan(0, 25, 0);
                inst.WriteTimeSpan("CloseTimeout", binding.CloseTimeout);
                inst.WriteTimeSpan("OpenTimeout", binding.OpenTimeout);
                inst.WriteTimeSpan("ReceiveTimeout", binding.ReceiveTimeout);
                inst.WriteTimeSpan("SendTimeout", binding.SendTimeout);
                */
                selfHost.AddServiceEndpoint(typeof(EAOfficeAPI.IEAOfficeAPI), binding, UrlString);
                System.ServiceModel.Channels.Binding mexBinding = MetadataExchangeBindings.CreateMexHttpBinding();
                /*
                inst.WriteTimeSpan("mex CloseTimeout", mexBinding.CloseTimeout);
                inst.WriteTimeSpan("mex OpenTimeout", mexBinding.OpenTimeout);
                inst.WriteTimeSpan("mex ReceiveTimeout", mexBinding.ReceiveTimeout);
                inst.WriteTimeSpan("mex SendTimeout", mexBinding.SendTimeout);
                Console.WriteLine("Resetting Timeouts");
                mexBinding.CloseTimeout = new TimeSpan(0, 25, 0);
                mexBinding.OpenTimeout = new TimeSpan(0, 25, 0);
                mexBinding.ReceiveTimeout = new TimeSpan(0, 25, 0);
                mexBinding.SendTimeout = new TimeSpan(0, 25, 0);
                inst.WriteTimeSpan("mex CloseTimeout", mexBinding.CloseTimeout);
                inst.WriteTimeSpan("mex OpenTimeout", mexBinding.OpenTimeout);
                inst.WriteTimeSpan("mex ReceiveTimeout", mexBinding.ReceiveTimeout);
                inst.WriteTimeSpan("mex SendTimeout", mexBinding.SendTimeout);
                */

                //selfHost.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexHttpBinding(), "mex");
                selfHost.AddServiceEndpoint(typeof(IMetadataExchange), mexBinding, "mex");

                selfHost.Open();
                Console.WriteLine("The service is ready.");
                //Console.WriteLine("Press <ENTER> to terminate the service.");
                //Console.WriteLine();
                //Console.ReadLine();
                Thread.Sleep(Timeout.Infinite);

                selfHost.Close();
            }
            catch (CommunicationException ce)
            {
                Console.WriteLine("An exception occurred: {0}", ce.Message);
                selfHost.Abort();
            }
		}
		
		private void WriteTimeSpan(string TimeSpanType, TimeSpan ValueType)
		{
			Console.WriteLine(TimeSpanType + ": " + ValueType);
		}
	}
}