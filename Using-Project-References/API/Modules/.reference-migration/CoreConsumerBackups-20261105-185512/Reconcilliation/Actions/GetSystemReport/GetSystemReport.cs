/*
 * Created by SharpDevelop.
 * User: John
 * Date: 08/10/2019
 * Time: 11:59 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;

/*
using Newtonsoft.Json;
*/
using Newtonsoft.Json.Linq;

using CommonUtilities;
using EAOfficeAPI;

namespace Actions.Reconcilliation.GetSystemReport
{
	/// <summary>
	/// Description of GetSystemReport.
	/// </summary>
	public class GetSystemReport : ActionBase
	{
		private string FModuleName = "Reconcilliation";
		private GetSystemReportRequestItem FRequestItem;

		public static string Execute(int MasterId,
									 APISettingsItem SettingsItem,
									 string Request,
									 out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			GetSystemReport inst = new GetSystemReport();
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
				FRequestItem = new GetSystemReportRequestItem();
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

        //private string Output()
        //{
        //	string Method = "Output";
        //	string Result = "";
        //	string ErrorMsg = "";
        //	WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " +Method, FModuleName, MasterId);
        //	LogOnInsideMethod(Method);
        //	try 
        //	{
        //		int NewId = GetNextGeneratorId(out ErrorMsg);
        //		string ReportSource = "";
        //		if (string.IsNullOrEmpty(ErrorMsg))
        //		{
        //			ReportSource = GetReportSource(out ErrorMsg);
        //		}

        //		if (string.IsNullOrEmpty(ErrorMsg))
        //		{
        //			//ErrorMsg = InsertEASystemReportRecord(NewId, ReportSource);
        //			ErrorMsg =
        //				CommonMethods.InsertEASystemReportRecord(
        //					FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue, NewId, FRequestItem.ReportId, ReportSource);
        //		}

        //		if (string.IsNullOrEmpty(ErrorMsg))
        //		{
        //			//ErrorMsg =  PerformStep1(NewId);
        //			ErrorMsg = CommonMethods.PerformStep1(FbConnectionString, NewId, CommonItem.CompanyIdValue, CommonItem.ClientIdValue);
        //		}				

        //		string SqlToUse = "";
        //		string OutputFileName = "";
        //		string OutputFileNameOnly = "";
        //		string Extension = "";
        //		if (string.IsNullOrEmpty(ErrorMsg))
        //		{
        //			//ErrorMsg = GetSqlToUse(NewId, out SqlToUse, out OutputFileName, out OutputFileNameOnly, out Extension);
        //			ErrorMsg =
        //				CommonMethods.GetSqlToUse(
        //					FbConnectionString, CommonItem.CompanyIdValue, NewId, out SqlToUse, out OutputFileName, out OutputFileNameOnly, out Extension);
        //		}

        //		if (string.IsNullOrEmpty(ErrorMsg))
        //		{
        //			ErrorMsg = WriteOutputFile(SqlToUse, OutputFileName, Extension);
        //		}

        //		if (!string.IsNullOrEmpty(ErrorMsg))
        //		{
        //			Result = LogOnProcessingError(Method, ErrorMsg, "");
        //		}
        //		else
        //		{
        //			Result =
        //				JsonUtilities.StartJson() +
        //				JsonUtilities.AddPropertyNameAndValue("FileName", @"/Docs/" + OutputFileNameOnly + Extension, false) +
        //				JsonUtilities.EndJson();
        //		}

        //		/*
        //		StringBuilder Entries = new StringBuilder();
        //		string SqlToUse = "";
        //		string OutputFileName = "";
        //		string OutputFileNameOnly = "";
        //		ErrorMsg = GetSqlToUse(out SqlToUse, out OutputFileName, out OutputFileNameOnly);

        //		if (string.IsNullOrEmpty(ErrorMsg))
        //		{
        //			ErrorMsg = WriteOutputFile(SqlToUse, OutputFileName);
        //		}
        //		*/

        //		if (!string.IsNullOrEmpty(ErrorMsg))
        //		{
        //			Result = LogOnProcessingError(Method, ErrorMsg, "");
        //		}
        //		else
        //		{
        //			Result =
        //				JsonUtilities.StartJson() +
        //				JsonUtilities.AddPropertyNameAndValue("FileName", @"/Docs/" + OutputFileNameOnly + Extension, false) +
        //				JsonUtilities.EndJson();
        //		}
        //	}
        //	catch (Exception Ex)
        //	{
        //		Result = LogOnException(Method, Ex, "");
        //	}
        //	finally
        //	{
        //		LogOnLeavingMethod(Method);
        //	}
        //	WindowsEventLoggingUtilities.WriteToInfoEvent("Leaving: " +Method, FModuleName, MasterId);
        //	return Result;
        //}

        private string Output()
        {
          string Method = "Output";
          string Result = "";
          string ErrorMsg = "";
          WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " +Method, FModuleName, MasterId);
          LogOnInsideMethod(Method);
          try 
          {
              string ReportSource = "";
              if (string.IsNullOrEmpty(ErrorMsg))
              {
                  ReportSource = GetReportSource(out ErrorMsg);
              }

              string OutputFileName = "";
              if (string.IsNullOrEmpty(ErrorMsg))
              {
                    long UniqueId = (long)DateTime.Now.Subtract(DateTime.MinValue.AddYears(1969)).TotalMilliseconds;
                    Int64 NewId =
                        CommonUtilities.SystemReportUtilities.Execute(
                            FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue,
                            UniqueId, ReportSource, out ErrorMsg, out OutputFileName);
              }

              if (string.IsNullOrEmpty(ErrorMsg))
              {
                    Result =
                        JsonUtilities.StartJson() +
                        JsonUtilities.AddPropertyNameAndValue("FileName", @"/Docs/" + OutputFileName, false) +
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

        private int GetNextGeneratorId(out string ErrorMsg)
		{
			string Method = "GetNextGeneratorId";
			LogOnInsideMethod(Method);
			int Result =
				UsefulDBMethods.GeneratorHelpers.GetNextGeneratorId(
					FbConnectionString, "I_SYS_ID_GEN", CommonItem.CompanyIdValue, CommonItem.ClientIdValue, out ErrorMsg);
			LogOnLeavingMethod(Method);
			return Result;
		}
		
		private string GetReportSource(out string ErrorMsg)
		{
			string Result = "";
			ErrorMsg = "";
			string Method = "GetReportSource";
			LogOnInsideMethod(Method);
			
			StringBuilder Entries = new StringBuilder();
			
			ErrorMsg = CommonMethods.GetEndOfMonthPrintGroupEntries(FbConnectionString, CommonItem.CompanyIdValue, FRequestItem.BankId, ref Entries);
			if (!string.IsNullOrEmpty(ErrorMsg)) return Result;
			
			string Str =
				JsonUtilities.StartJson() +
				Entries.ToString() +
				JsonUtilities.EndJson();
			
			WindowsEventLoggingUtilities.WriteToInfoEvent("JsonText: " + Environment.NewLine + Str, FModuleName, MasterId);
				
			JObject JObj = null;
			try 
			{
				if (Entries.Length > 0)
				{
					JObj = JObject.Parse(Str);
				}				
			}
			catch (Exception Ex)
			{
				ErrorMsg =
					Method + Environment.NewLine +
					Ex.Message + Environment.NewLine +
					Ex.StackTrace;					
			}
			
			if (string.IsNullOrEmpty(ErrorMsg))
			{
				string JPath = @"$.gridEntries[?(@.I_ID == '" + FRequestItem.ReportIdValue + "')]";
				WindowsEventLoggingUtilities.WriteToInfoEvent("JPath: " + JPath, FModuleName, MasterId);
				JToken Record = JObj.SelectToken(JPath);
				if (Record == null)
					WindowsEventLoggingUtilities.WriteToInfoEvent("Record IS NULL", FModuleName, MasterId);
				else
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent("Record: " + Record.ToString(), FModuleName, MasterId);
					Result = (string)Record["ZL20_B" + FRequestItem.ButtonIdValue];				
					WindowsEventLoggingUtilities.WriteToInfoEvent("Result: " + Result, FModuleName, MasterId);
				}
			}				
			
			return Result;
		}
		
		private string InsertEASystemReportRecord(int NewId,
		                                          string ReportSource)
		{
			string Result = "";
			
			string QueryText =
				" INSERT INTO EASYSTEMREPORT (I_SYS_ID, I_SYS_COMPANYCLIENTID, SYS_SOURCE, I_PASSED_ID, I_SYS_ADDEDBYID )" +
				" VALUES(@NewId, @CompanyId, @Source, @BankId, @ClientId)";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("@NewId", NewId));
			Params.Add(new FbParameterItem("@CompanyId", CommonItem.CompanyIdValue));
			Params.Add(new FbParameterItem("@Source", ReportSource));
			Params.Add(new FbParameterItem("@BankId", FRequestItem.BankId));
			Params.Add(new FbParameterItem("@ClientId", CommonItem.ClientIdValue));
			Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
			
			if (string.IsNullOrEmpty(Result))
			{
				Result = FirebirdUtilities.DBExecute(FbConnectionString, QueryText, Params, 0);
			}
			
			return Result;
		}
		
		private string PerformStep1(int Id)
		{
			string Result = "";
			
			string QueryText = "SELECT First 1 SYS_Step1 FROM EASystemReport WHERE I_SYS_ID = @NewId";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("@NewId", Id));
			Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
			
			DataTable tbl = null;
			if (string.IsNullOrEmpty(Result))
			{
				tbl =
					UsefulDBMethods.Helpers.GetFirstDataTableEx(
						FbConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
			}
			
			string Step1 = "";
			if (string.IsNullOrEmpty(Result))
			{
				Object Obj = tbl.Rows[0]["SYS_Step1"];
				Step1 = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
				Step1 = Step1.Trim();
				WindowsEventLoggingUtilities.WriteToInfoEvent("Step1: " + Step1, FModuleName, MasterId);
			}
			
			if ((string.IsNullOrEmpty(Result)) && (!string.IsNullOrEmpty(Step1)))
			{
				if (Step1.ToUpper().StartsWith("TESTRPO"))
				{
					Result = PerformTestRPO(Step1);
				}
				else if (Step1.ToUpper().StartsWith("SELECT"))
				{
					// Don't know what to do here
				}
			}
			return Result;
		}
		
		private string PerformTestRPO(string CommandLine)
		{
			string Result = "";
			string CmdBase = CommonUtilities.FolderHelpers.TestRPOLocation(FbConnectionString, CommonItem.CompanyIdValue, out Result);
			if (string.IsNullOrEmpty(Result))
			{
				if (!CmdBase.EndsWith(@"\")) CmdBase += @"\";
				CmdBase += "{0}";
				string AppName = CommandLine.Substring(0, CommandLine.IndexOf(" /"));
				AppName = string.Format(CmdBase, AppName);
				
				string ArgsBase = CommandLine.Substring(CommandLine.IndexOf("/"));
				WindowsEventLoggingUtilities.WriteToInfoEvent("AppName: " + AppName, this.FModuleName, MasterId);
				WindowsEventLoggingUtilities.WriteToInfoEvent("Args: " + ArgsBase, this.FModuleName, MasterId);
				
				Process runApp = new Process();
				runApp.StartInfo.FileName = AppName;
				runApp.StartInfo.Arguments = ArgsBase;
				runApp.Start();
				const int tries = 1000;
				for (int Index = 0; Index < tries; Index++)
				{
					if (!runApp.HasExited) System.Threading.Thread.Sleep(1000);
				}
				if (!runApp.HasExited)
				{
					Result = "Application did not finish in time.";
				}
				//runApp.WaitForExit(10000);
				
				WindowsEventLoggingUtilities.WriteToInfoEvent("After calling: " + AppName + " Result: " + Result, FModuleName, MasterId);
			}
			return Result;
		}                               
		
		private string GetSqlToUse(int NewId,
		                           out string SqlToUse, 
		                           out string FileName, 
		                           out string FileNameOnly,
		                           out string Extension)
		{
			string Result = "";
			//string Method = "GetSqlToUse";
			SqlToUse = "";
			FileName = "";
			FileNameOnly = "";
			Extension = "";
			
			string QueryText =
				" SELECT First 1 SYS_SqlBase64, SYS_FileName, SYS_Extension FROM EASystemReport" +
				" WHERE I_SYS_ID = @NewId";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("@NewId", NewId));
			Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
			
			DataTable tbl = null;
			if (string.IsNullOrEmpty(Result))
			{
				tbl =
					UsefulDBMethods.Helpers.GetFirstDataTableEx(
						FbConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
			}
			
			if ((string.IsNullOrEmpty(Result)) && (tbl.Rows.Count > 0))
			{
				Object Obj = tbl.Rows[0]["SYS_SqlBase64"];
				SqlToUse = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
				Obj = tbl.Rows[0]["SYS_FileName"];
				FileName = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
				FileNameOnly = FileName;
				Obj = tbl.Rows[0]["SYS_Extension"];
				Extension = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
				if (!Extension.StartsWith(".")) Extension = "." + Extension;
				
				if (!string.IsNullOrEmpty(FileName))
				{
					string FilePath = CommonUtilities.FolderHelpers.DocsLocation(FbConnectionString, CommonItem.CompanyIdValue, out Result);
					if (!FilePath.EndsWith(@"\")) FilePath += @"\";
					FileName = FilePath + FileName;
				}
				
				WindowsEventLoggingUtilities.WriteToInfoEvent("SqlToUse: " + SqlToUse + " FileName: " + FileName, FModuleName, MasterId);
			}
			
			return Result;
		}
		
		private string WriteOutputFile(string SqlToUse, string FileName, string Extension)
		{
			string Result = "";
			string Method = "WriteOutputFile";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " + Method, FModuleName, MasterId);
			
			DataTable tbl =
				UsefulDBMethods.Helpers.GetFirstDataTableEx(
					FbConnectionString, SqlToUse, null, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
			if ((string.IsNullOrEmpty(Result)) && (tbl.Rows.Count > 0))
			{
				Object Obj = tbl.Rows[0][0];
				byte[] toBytes = (byte[])Obj;
				string Base64String = System.Text.Encoding.Default.GetString(toBytes);
				File.WriteAllBytes(FileName + Extension, Convert.FromBase64String(Base64String));
				WindowsEventLoggingUtilities.WriteToInfoEvent("After writing the file: " + FileName, FModuleName, MasterId);
			}
			
			if (!File.Exists(FileName + Extension)) Result = "The file: " + FileName + " does NOT exist.";
			
			return Result;
		}
	}
}
