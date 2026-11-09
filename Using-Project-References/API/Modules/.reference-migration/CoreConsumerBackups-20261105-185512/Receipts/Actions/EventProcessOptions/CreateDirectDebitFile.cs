/*
 * Created by SharpDevelop.
 * User: John
 * Date: 30/04/2020
 * Time: 12:28 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Diagnostics;

using Newtonsoft.Json.Linq;
using CommonUtilities;
using EAOfficeAPI;
namespace Actions.Receipts.EventProcessOptions
{
	/// <summary>
	/// Description of CreateDirectDebitFile.
	/// </summary>
	public class CreateDirectDebitFile : ActionBase
	{
		private string FModuleName = "Receipts";
		private EventProcessOptions_OnChangeRequestItem FRequestItem;		
		
		public static string Execute(int MasterId,
									 APISettingsItem SettingsItem,
									 string Request,
									 out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			CreateDirectDebitFile inst = new CreateDirectDebitFile();
			if (!inst.Setup(MasterId, SettingsItem, Request, out ErrorMsg))
				return "";
			
			inst.LogOnInsideMethod(Method);
			try
			{
				Result = inst.Init();				
				if (!string.IsNullOrEmpty(Result))
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent(Result, Method, MasterId);
					return "";
				}
				
				Result = inst.Validate();
				if (!string.IsNullOrEmpty(Result))
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent(Result, Method, MasterId);
					return "";
				}

				Result = inst.Output();
				UsefulDBMethods.Helpers.RunEAMainOfficeProcess(inst.FbConnectionString, inst.CommonItem.CompanyIdValue);
			}
			finally
			{
				inst.LogOnLeavingMethod(Method);
			}
			
			return Result;
		}
		
		private string Init()
		{
			string Method = "Init";
			string Result = "";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " +Method, FModuleName, MasterId);
			LogOnInsideMethod(Method);
			try 
			{
				FRequestItem = new EventProcessOptions_OnChangeRequestItem();
				Result = FRequestItem.Load((ActionBase)this);
			}
			catch (Exception Ex)
			{
				Result = LogOnException(Method, Ex, "");
			}
			finally
			{
				LogOnLeavingMethod(Method);
			}
			return Result;
		}
		
		private string Validate()
		{
			string Method = "Validate";
			string Result = "";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " + Method, FModuleName, MasterId);
			LogOnInsideMethod(Method);
			try 
			{
				Result = FRequestItem.Validate();
			}
			catch (Exception Ex)
			{
				Result = LogOnException(Method, Ex, "");
			}
			finally
			{
				LogOnLeavingMethod(Method);
			}
			return Result;
		}
		
		private string Output()
		{
			string Method = "Output";
			string Result = "";
			string ErrorMsg = "";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " + Method, FModuleName, MasterId);
			LogOnInsideMethod(Method);
			try 
			{
				StringBuilder Entries = new StringBuilder();
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg =
						CommonMethods.EAReceiptScreenGroup2(
							FbConnectionString, CommonItem.CompanyIdValue, FRequestItem.MasterEventId, CommonItem.ClientIdValue, 
							FRequestItem.PastEventId, FRequestItem.ItemId, ref Entries);
				}
				
				StringBuilder Results = new StringBuilder();
				
				string ShowButton = "";
				string SqlToCreateFile = "";
				string SqlToCreateFile2 = "";
				string Path = "";
				if (string.IsNullOrEmpty(ErrorMsg))					
				{
					Path = "$.showdirectdebitcreatefile";
					ShowButton = JsonUtilities.GetJsonPathValueStr(Entries.ToString(), Path, out ErrorMsg);
					if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(ShowButton))
						ErrorMsg = "Cannot find the Show Cancel Button value";
					if (string.IsNullOrEmpty(ErrorMsg) && (ShowButton == "N"))
						ErrorMsg = "This item cannot be processed";
					if (string.IsNullOrEmpty(ErrorMsg))
					{
						Path = "$.sqltocreatefile";
						SqlToCreateFile = JsonUtilities.GetJsonPathValueStr(Entries.ToString(), Path, out ErrorMsg);
						if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(SqlToCreateFile))
							ErrorMsg = "Cannot find the SQL Create File value";
						else WindowsEventLoggingUtilities.WriteToInfoEvent("SqlToCreateFile: " + SqlToCreateFile, Method, 0);
					}
					
					if (string.IsNullOrEmpty(ErrorMsg))
						ErrorMsg = ProcessSqlToCreateFile(SqlToCreateFile, ref Results);

					if (string.IsNullOrEmpty(ErrorMsg))
					{
						Path = "$.sqltocreatefile2";
						SqlToCreateFile2 = JsonUtilities.GetJsonPathValueStr(Entries.ToString(), Path, out ErrorMsg);
						
						if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(SqlToCreateFile2))
							ErrorMsg = "Cannot find the second SQL Create File value";
						else WindowsEventLoggingUtilities.WriteToInfoEvent("SqlToCreateFile2: " + SqlToCreateFile2, Method, 0);
					}
					
					if (string.IsNullOrEmpty(ErrorMsg))
						/****** ErrorMsg = FirebirdUtilities.DBExecute(FbConnectionString, SqlToCreateFile2); ******/
						ErrorMsg = ProcessSqlToCreateFile2(SqlToCreateFile2);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					if (Results.Length > 0) Results.Append(",");
					FRequestItem.PastEventId = 0;
					ErrorMsg =
						CommonMethods.EAReceiptScreenGroup2(
							FbConnectionString, CommonItem.CompanyIdValue, FRequestItem.MasterEventId, CommonItem.ClientIdValue, 
							FRequestItem.PastEventId, FRequestItem.ItemId, ref Results);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Result =
						JsonUtilities.StartJson() +
						Results.ToString() +
						JsonUtilities.EndJson();
				}
				else
				{
					Result = LogOnProcessingError(Method, ErrorMsg, "");
				}
			}
			catch (Exception Ex)
			{
				Result = LogOnException(Method, Ex, "");
			}
			finally
			{
				LogOnLeavingMethod(Method);
			}
			WindowsEventLoggingUtilities.WriteToInfoEvent("Leaving: " +Method, FModuleName, MasterId);
			return Result;
		}
		
		private string ProcessSqlToCreateFile(string SqlToCreateFile, ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "ProcessSqlToCreateFile";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, FModuleName, MasterId);
					
			LogOnInsideMethod(Method);
			try 
			{
				string QueryText = SqlToCreateFile.Replace("@", "'");;
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, null, Method, 0);
				
				DataTable tbl = null;
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							FbConnectionString, QueryText, null, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
					if (string.IsNullOrEmpty(Result) && (tbl == null)) Result = "Error calling query in " + Method;
				}
				
				string CommandLine = "";
				string EventRunIdValue = "";
				int EventRunId = 0;
				object Obj = null;
				if (string.IsNullOrEmpty(Result))
				{
					foreach(DataRow Row in tbl.Rows)
					{
						Obj = Row["EC_COMMANDLINE"];
						CommandLine = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
						Obj = Row["EC_EventRunId"];
						EventRunIdValue = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					}
					if (string.IsNullOrEmpty(CommandLine)) Result = "Cannot find the CommandLine value";
					else WindowsEventLoggingUtilities.WriteToInfoEvent("CommandLine: " + CommandLine, Method, 0);
					EventRunId = StringUtilities.StrToIntDef(EventRunIdValue, -1);
					if (string.IsNullOrEmpty(Result) && (EventRunId < 0)) Result = "Invalid EventRunId value";
				}
				
				string TestRPOLocation = "";
				string DocsLocation = "";
				if (string.IsNullOrEmpty(Result))
				{
					TestRPOLocation = CommonUtilities.FolderHelpers.TestRPOLocation(FbConnectionString, CommonItem.CompanyIdValue, out Result);
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(TestRPOLocation)) Result = "Cannot determine the TestRPOLocation";
					if (string.IsNullOrEmpty(Result))
					{
						DocsLocation = CommonUtilities.FolderHelpers.DocsLocation(FbConnectionString, CommonItem.CompanyIdValue, out Result);
					}
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(DocsLocation)) Result = "Cannot determine the DocsLocation";
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					// run the command line etc
					Result = UsefulDBMethods.Helpers.ProcessTestRPOCommandLine(CommandLine, TestRPOLocation, 0);
					// extract the base64 field and put the file in the docs location
					// open / download the output file
					try
					{
						Entries.Append(JsonUtilities.StartJsonArray("OutputFileList"));
						ReportUtilities repUtils = new ReportUtilities();
						repUtils.EAEventRunBase64Entries(FbConnectionString, EventRunId, DocsLocation, ref Entries, out Result);
					}
					finally
					{
						Entries.Append(JsonUtilities.EndJsonArray());
					}
				}
			}
			catch (Exception Ex)
			{
				Result = LogOnException(Method, Ex, "");
			}
			finally
			{
				LogOnLeavingMethod(Method);
			}
			
			return Result;
		}
		
		private string ProcessSqlToCreateFile2(string SqlToCreateFile2)
		{
			string Result = "";
			string Method = "ProcessSqlToCreateFile2";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, FModuleName, MasterId);
					
			LogOnInsideMethod(Method);
			try 
			{
				string QueryText = SqlToCreateFile2.Replace("@", "'");
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(FbConnectionString, QueryText);
				}

			}
			catch (Exception Ex)
			{
				Result = LogOnException(Method, Ex, "");
			}
			finally
			{
				LogOnLeavingMethod(Method);
			}
			
			return Result;
		}
	}
}
