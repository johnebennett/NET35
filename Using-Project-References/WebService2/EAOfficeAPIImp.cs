/*
 * Created by SharpDevelop.
 * User: jbennett
 * Date: 10/10/2018
 * Time: 11:15 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using CommonUtilities;

namespace EAOfficeAPI
{
	/// <summary>
	/// Implementation of the IEAOfficeAPI Interface
	/// </summary>
	public class EAOfficeAPI : IEAOfficeAPI
	{
		private bool FAPISettingsLoaded = false;
		private bool FLogging = false;
		private DateTime FLoggingFailureDate = DateTime.MinValue;
		//private bool FDBLoggingEnabled = false;
		//private long FMasterSQLiteLoggingID = -1;
		private bool FEventViewerLogging = false;
		private string FSessionInd = "";

		public string HelloWorld()
		{
			return "Hello World";
		}
		
		public string Execute(string Request)
		{
			// Internally work with JSON exclusively
			string Result = "";
			string InternalRequest = "";
			
			try 
			{
				if (FAPISettingsLoaded == false)
				{				
					LoadAPISettingsFromFile();
				}
			}
			finally
			{
				FAPISettingsLoaded = false;			
			}
			
			//FLogging = true;
			
			//fMasterSQLiteLoggingID = WriteToLog(Request, "Execute", "Request");  // Always log - dont exclude according to debug
			
			InternalRequest = Request;				
			Result = InternalRequest;			
			
			try 
			{
				Result = RequestProcessor.Execute(InternalRequest);				
				WriteToLog(Result, "Execute", "Interim Result");
			}
			catch (Exception ex) 
			{				
				Result = "";
				
				WriteToErrorLog(ex.ToString(), "Execute", "Command Processor Error");  // Always log - dont exclude according to debug
			}			
			
			return Result;
		}
		
		private void LoadAPISettingsFromFile()
		{
			APISettingsItem Item = null;
			string ErrorMsg = "";
			
			try
			{
				Item = APISettingsFile.GetAPISettings(APIAppType.Service, out ErrorMsg);
				
				FEventViewerLogging = Item.EventViewerLogging;
				FLogging = Item.DebugLogging;
				FSessionInd = Item.APISessionName;
				
				if (!String.IsNullOrEmpty(ErrorMsg))
				{			
					WriteToErrorLog(ErrorMsg, "EAOfficeAPI", "LoadAPISettingsFromFile");	
				}
				
				FAPISettingsLoaded = true;

			}
			catch (Exception e)
			{
				ErrorMsg = e.Message;
				
				WriteToErrorLog(e.ToString(), "EAOfficeAPI", "LoadAPISettingsFromFile");	
			}
		}
		private void WriteLoggingErrorToEventLog(string Message, 
		                                         string Category, 
		                                         string SubCategory)
		{			
			string Msg = Message;
			
			if (string.IsNullOrEmpty(Category))
			{
				Msg = Msg + Environment.NewLine + "Category: " + Category;
			}

			if (string.IsNullOrEmpty(SubCategory))
			{
				Msg = Msg + Environment.NewLine + "SubCategory: " + SubCategory;
			}								
			
			string lResult = CommonUtilities.WindowsEventLoggingUtilities.WriteToErrorEvent(Msg, "EAOfficeAPI " + FSessionInd, 0);			
		}
		
		private void WriteLoggingFailureErrorToEventLog(string Message)
		{
			// Dont continue if have already logged error in last day
			if (FLoggingFailureDate > (DateTime.Now.AddDays(-1)))
				return;
			
			WriteLoggingErrorToEventLog(Message, "", "");						
		}
		
		private void WriteInfoToEventLog(string aMessage)
		{
			string lResult = CommonUtilities.WindowsEventLoggingUtilities.WriteToInfoEvent(aMessage, "EAOfficeAPI_INFO " + FSessionInd, 0);
		}
		
		/*
		private Int64 WriteToLoggingDB(string aMessage, string aCategory, string aSubCategory, bool aIsError)
		{
			Int64 lResult = 0;
			
			// If is an error and not logging then write as error to EventViewer
			// Dont continue if logging disabled			
			if ((FLogging == false) && (aIsError == true))
			{
				WriteLoggingErrorToEventLog(aMessage, aCategory, aSubCategory);
				return lResult;
			}
			
			string lErrorMsg = "";
			
			string lLoggingPath = CommonUtilities.WindowsEventLoggingUtilities.CommonUtilities.GetLoggingDirBaseForLocalAssembly();
			try 
			{	
				// Include for debugging purposes
				//WriteInfoToEventLog("WriteToLoggingDB - Message " + Environment.NewLine + aMessage + Environment.NewLine + Environment.NewLine +
				//                     "Logging Path is: " + Environment.NewLine + lLoggingPath + Environment.NewLine);
				
				//string ConfigFilePath = new Uri(Assembly.GetExecutingAssembly().GetName().CodeBase).LocalPath;
				//ConfigFilePath = System.IO.Path.GetDirectoryName(ConfigFilePath);
				
				//WriteInfoToEventLog("WriteToLoggingDB - Message " + Environment.NewLine + aMessage + Environment.NewLine + Environment.NewLine +
				//                    "Assembly Execution Path is: " + Environment.NewLine + ConfigFilePath + Environment.NewLine);
				
				// Initialise the local logging database
				lErrorMsg = SqlLiteLoggingDBHelpers.InitialiseDBDefault(FLoggingModule, lLoggingPath);
				
				if (String.IsNullOrEmpty(lErrorMsg))
				{
					if (aIsError == false)
						lResult = SqlLiteLoggingDBHelpers.WriteInformationToLog(FLoggingModule, lLoggingPath, fMasterSQLiteLoggingID, aCategory, aSubCategory, 
						                                                                        "", aMessage, out lErrorMsg);
					else
							
						lResult = SqlLiteLoggingDBHelpers.WriteErrorToLog(FLoggingModule, lLoggingPath, fMasterSQLiteLoggingID, aCategory, aSubCategory, "", aMessage, out lErrorMsg);
				}
			} 
			catch (Exception ex) 
			{
				// Catch any exception and send to Windows Error Event if no logging failure today 	
				lErrorMsg = "Error in EAOfficeAPI WriteToLog -" + Environment.NewLine + ex.ToString();
			}
			
			if (String.IsNullOrEmpty(lErrorMsg) == false)
			{
				WriteLoggingFailureErrorToEventLog(lErrorMsg);
				FLoggingFailureDate = DateTime.Now;				
			}
			
			return lResult;
		}
		*/
		
		private long WriteToLog(string Message, string Category, string SubCategory)
		{
			long lResult = -1;
			
			if (FLogging == false)
				return lResult;
			
			/*
			if (FDBLoggingEnabled)
			{
				lResult = WriteToLoggingDB(Message, Category, SubCategory, false);
			}
			else
			{	
			*/
				
				if (FEventViewerLogging == true) 
				{
					// Limit the message length if saving to Event Viewer
					string ActualMessage = 
						CommonUtilities.WindowsEventLoggingUtilities.GetMessageSubStringForEventViewer(
							Message, CommonUtilities.WindowsEventLoggingUtilities.EventViewerLogDefaultByteLimit);
					
					WriteInfoToEventLog(
						String.Format(
							"{0}" + Environment.NewLine + "Category: {1}" + Environment.NewLine + "SubCategory: {2}",
							ActualMessage, Category, SubCategory));
				}
			/*
			}
			*/
			
			return lResult;
		}
		
		private long WriteToErrorLog(string Message, 
		                             string Category, 
		                             string SubCategory)
		{
			long Result = -1;
			
			/*
			if (FDBLoggingEnabled)
			{
				lResult = WriteToLoggingDB(aMessage, aCategory, aSubCategory, true);
			}
			else
			{
			*/
				string ActualMessage = 
					CommonUtilities.WindowsEventLoggingUtilities.GetMessageSubStringForEventViewer(
						Message, CommonUtilities.WindowsEventLoggingUtilities.EventViewerLogDefaultByteLimit);				                                                                                                       
				                                                                                                       
				WriteLoggingErrorToEventLog(ActualMessage, Category, SubCategory);
			/*
			}			
			*/
			
			return Result;
		}
		
	}
}
