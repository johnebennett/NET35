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
//using System.Threading.Tasks;
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
		private static ConfigOptions config = new ConfigOptions();
		
		public static void Main(string[] args)
		{
			string ErrorMsg = "";
			APISettingsItem ApiSettings = APISettingsFile.GetAPISettings(APIAppType.Service, out ErrorMsg);
			if (string.IsNullOrEmpty(ErrorMsg))
			{
				config = ConfigOptions.LoadConfigOptions();
				CommonMethods.WriteToOutput("Before getting the Connection String", config.ShowDebugMessages);
				m_ConnectionStr = CommonMethods.ConnectionString(out ErrorMsg);
				Console.WriteLine("Connection String: " + m_ConnectionStr);
				Console.WriteLine("After getting the Connection String", config.ShowDebugMessages);
				if (!CommonUtilities.FirebirdUtilities.TestDBConnection(m_ConnectionStr, out ErrorMsg))
				{
					Console.WriteLine(ErrorMsg);
				}
				else
               	{
                    Console.WriteLine("Connection Successful");
                };
				
				// default to 5000 milliseconds between calls to search for records
				int Delay = CommonUtilities.StringUtilities.StrToIntDef(config.DelayBetweenSearches, 5000);
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					System.Timers.Timer timer = 
						//new System.Timers.Timer(5000); // Set the timer interval to 1 minute (60000 milliseconds)
						new System.Timers.Timer(Delay); // Set the timer interval to 1 minute (60000 milliseconds)
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
	    	
	    	CommonMethods.WriteToOutput("Checking the number of running instances...", config.ShowDebugMessages);
	    	int InstanceCount = CountRunningInstances(config);
	    	int  MaxInstanceCount = CommonUtilities.StringUtilities.StrToIntDef(config.MaxNumberOfInstances, 5);
	    	if (InstanceCount >= MaxInstanceCount)
	    	{
	    		Console.WriteLine(
	    			string.Format("There are already {0} instances running...Skipping...", config.MaxNumberOfInstances));
	    		return;
	    	}
	    	
	        Console.WriteLine("Checking for new records...");
	
	        string ErrorMsg = "";
	        try 
	        {
		        if (string.IsNullOrEmpty(ErrorMsg) && !string.IsNullOrEmpty(m_ConnectionStr))
		        {
		        	string WhereCondition = "";
		        	if (config.BuildWhereClause == "Y")
		        	{
		        		/*
			        	if (!string.IsNullOrEmpty(config.IncludeCompanyId))
			        	{
			        		WhereCondition = "I_EVA_CompanyClientId = " + config.IncludeCompanyId + " AND";
			        	}
			        	else if (!string.IsNullOrEmpty(config.ExcludeCompanyId))
			        	{
			        		WhereCondition = "I_EVA_CompanyClientId <> " + config.ExcludeCompanyId + " AND";
			        	}
			        	*/
			        	
			        	if (!string.IsNullOrEmpty(config.CompanyIdIn))
			        	{
			        		if (!string.IsNullOrEmpty(WhereCondition))
			        			WhereCondition += " AND ";
			        		WhereCondition = 
			        			WhereCondition + " I_EVA_CompanyClientId IN (" + config.CompanyIdIn + ")";
			        	}
			        	
			        	if (!string.IsNullOrEmpty(config.CompanyIdNotIn))
			        	{
			        		if (!string.IsNullOrEmpty(WhereCondition))
			        			WhereCondition += " AND ";
			        		WhereCondition =
			        			WhereCondition + "I_EVA_CompanyClientId NOT IN (" + config.CompanyIdNotIn + ")";
			        	}			        	
		        	}
		        	
		        	if (!string.IsNullOrEmpty(config.ActivitySelector) && 
		        	    !string.IsNullOrEmpty(config.ActivityCount))
		        	{
		        		if (!string.IsNullOrEmpty(WhereCondition))
		        			WhereCondition += " AND ";
		        		WhereCondition =
		        			WhereCondition + " I_EVA_GOATActivityCount " +
		        			config.ActivitySelector + " " + config.ActivityCount;
		        	}
		        	
		        	if (!string.IsNullOrEmpty(WhereCondition))
		        		WhereCondition = " AND " + WhereCondition;
		        	
		        	CommonMethods.WriteToOutput("Where Clause: " + WhereCondition, config.ShowDebugMessages);
		        	
		        	string QueryText = string.Format(config.SelectQuery, WhereCondition);
			        Console.WriteLine("QueryText:" + Environment.NewLine + QueryText);
			        DataTable tbl = FirebirdUtilities.GetFirstDataTable(m_ConnectionStr, QueryText);
			        if (tbl != null)
			        {
			        	//Console.WriteLine("Table is not null");
			        	Object Obj = null;
			        	/*
			        	if (tbl.Rows.Count > 0) 
			        	{
			        		Console.WriteLine("Table has records");
			        	}
			        	*/
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
			        else
			        {
			        	Console.WriteLine("Table is NULL");
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
			//string WorkingFileName = WorkingDir + "EmailProcessorHtml.exe";
			string WorkingFileName = WorkingDir + config.TargetAppName;
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
	    
	    public static int CountRunningInstances(ConfigOptions config)
	    {
	    	int Result = 0;
	    	
	    	string WorkingDir = CommonMethods.GetExecutingAssemblyPath();
	    	string AppName = System.IO.Path.GetFileNameWithoutExtension(config.TargetAppName);
	    	Result = Process.GetProcessesByName(AppName).Length;
	    	Console.WriteLine(string.Format("There are {0} instances running", Result));
	    	
	    	return Result;
	    }
	}
}