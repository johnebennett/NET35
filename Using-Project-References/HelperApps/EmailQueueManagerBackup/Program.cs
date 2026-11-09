/*
 * Created by SharpDevelop.
 * User: johne
 * Date: 6/02/2024
 * Time: 8:40 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Data;
using System.Threading;
using System.Timers;
using EmailQueueManager.Actions;
using CommonUtilities;
using EAOfficeAPI;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using Microsoft.VisualBasic.Devices;

namespace EmailQueueManager
{
	class Program
	{
		private static Semaphore processingSemaphore = new Semaphore(5, 5); // Allow 20 concurrent instances		
		private static string m_ConnectionStr = "";
		private static Int64 m_EVA_ID = 0;
		
		public static void Main(string[] args)
		{
			string ErrorMsg = "";
			APISettingsItem ApiSettings = APISettingsFile.GetAPISettings(APIAppType.Service, out ErrorMsg);
			if (string.IsNullOrEmpty(ErrorMsg))
			{
				Console.WriteLine("Before getting the Connection String");
				m_ConnectionStr = CommonMethods.ConnectionString(out ErrorMsg);
				Console.WriteLine("After getting the Connection String");
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					System.Timers.Timer timer = new System.Timers.Timer(5000); // Set the timer interval to 1 minute (60000 milliseconds)
			        timer.Elapsed += TimerElapsed;	
			        timer.Start();		
			
			        Console.WriteLine("Monitoring for new records. Press Enter to exit.");	
			        Console.ReadLine();					
				}
				else
				{
					Console.WriteLine(ErrorMsg);
				}
			}
			else
			{
				Console.WriteLine(ErrorMsg);
			}
		}

	    private static void TimerElapsed(object sender, ElapsedEventArgs e)
	    {	
	    	/*
	    	Console.WriteLine("Checking available memory...");
	    	if (!EnoughMemoryAvailable())
	    	{
	    		Console.WriteLine("Not enough memory available - skipping...");
	    		return;
	    	}
	    	*/
	    	
	        Console.WriteLine("Checking for new records...");	
	
	        string ErrorMsg = "";
	        try 
	        {
		        if (string.IsNullOrEmpty(ErrorMsg) && !string.IsNullOrEmpty(m_ConnectionStr))
		        {
			        string QueryText =
			        	"SELECT FIRST 1 EVA_ID FROM EAEventRun WHERE EMAIL_STATUS = 'NEW' ORDER BY EVA_ID";
			        Console.WriteLine("QueryText:" + Environment.NewLine + QueryText);
			        DataTable tbl = FirebirdUtilities.GetFirstDataTable(m_ConnectionStr, QueryText);
			        if (tbl != null)
			        {
			        	Console.WriteLine("Table is not null");
			        	Object Obj = null;
			        	if (tbl.Rows.Count > 0) 
			        	{
			        		Console.WriteLine("Table has records");
			        	}
			        	foreach (DataRow Row in tbl.Rows)
			        	{
			        		Obj = Row["EVA_ID"];
			        		m_EVA_ID = (Obj == DBNull.Value) ? 0 : Convert.ToInt64(Obj);
			        		Console.WriteLine("Processing record with EVA_ID: " + m_EVA_ID);	                        
		                    // Wait for a semaphore slot before launching a new processing application	
		                    processingSemaphore.WaitOne();	                        
		
		                    // Launch the processing application in a separate thread to not block the timer event	
		                    ThreadPool.QueueUserWorkItem(ProcessData, m_EVA_ID);	
			        	}
			        }
		        }
	        }
	        catch (Exception Ex)
	        {
	        	Console.WriteLine(Ex.Message + Environment.NewLine + Ex.StackTrace);
	        }
	    }	
	
	    private static void ProcessData(object state)	
	    {	
	        try	
	        {	
	            Int64 id = (Int64)state;	
	            LaunchProcessingApplication(id);	
	        }	
	        finally	
	        {	
	            // Release the semaphore slot when processing is complete	
	            processingSemaphore.Release();	
	        }	
	    }	
	    
	    /*
	    private static bool EnoughMemoryAvailable()
	    {
	    	bool Result = true;
	    	
	    	ComputerInfo info = new ComputerInfo();
	    	ulong totalMemory = info.TotalPhysicalMemory;
	    	ulong availableMemory = info.AvailablePhysicalMemory;
	    	float usedMemoryPercentage = ((float)(totalMemory - availableMemory) / totalMemory) * 100;
	    	
	    	if (usedMemoryPercentage < 90.0)
	    	{
	    		Console.WriteLine("Memory: " + usedMemoryPercentage);
	    		Result = false;
	    	}
	    	
	    	return Result;
	    }
	    */
	
	    private static void LaunchProcessingApplication(Int64 Id)	
	    {	
	    	string Result = "";
	    	string ModuleName = "Launch Email Processor Html";
	        // Replace "your_processing_app.exe" with the actual name of your processing application	
	        //string processingAppPath = "your_processing_app.exe";	        
	
	        // You can customize the command line arguments as needed	
	        //string arguments = @"--id={id}";	
	        //System.Diagnostics.Process.Start(processingAppPath, arguments);	

	        string WorkingDir = "";
			WindowsEventLoggingUtilities.WriteToInfoEvent(
	        	"Before calling GetCurrentDirectory", ModuleName, 0);

			//WorkingDir = Directory.GetCurrentDirectory();
            WorkingDir = 
            	CommonUtilities.FolderHelpers.GetAssemblyDirectoryName(
            		Assembly.GetExecutingAssembly());

            string SepChar = Path.DirectorySeparatorChar.ToString();
			if (!WorkingDir.EndsWith(SepChar)) WorkingDir += SepChar;
			Console.WriteLine("WorkingDir: " + WorkingDir);
			string WorkingFileName = WorkingDir + "EmailProcessorHtml.exe";
			//WindowsEventLoggingUtilities.WriteToInfoEvent("After calling GetCurrentDirectory", ModuleName, 0);
			//WindowsEventLoggingUtilities.WriteToInfoEvent("WorkingDir " + WorkingDir, ModuleName, 0);
			try
			{				
				if (!File.Exists(WorkingFileName))
				{
					Console.WriteLine("File: " + WorkingFileName + " does not exist");					
				}
				else
				{
					Result = CommonMethods.SetStartedStatus(m_ConnectionStr, m_EVA_ID);
					CommonMethods.WriteToOutput("After SetStartedStatus: " + Result);
					Process runApp = new Process();
	                //runApp.StartInfo.FileName = WorkingDir + "EmailProcessorHtml";
	                runApp.StartInfo.FileName = WorkingFileName;
	                runApp.StartInfo.Arguments = Convert.ToString(Id);
					runApp.Start();
				}
			}
			catch (Exception Ex)
			{
				//Result = LogOnException(Method, Ex, "");
				Console.WriteLine(Ex.Message + Environment.NewLine + Ex.StackTrace);
			}
	    }
	}
}