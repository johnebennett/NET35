
/*
 * Created by SharpDevelop.
 * User: John
 * Date: 31/10/2019
 * Time: 4:07 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using System.Diagnostics;
using System.IO;

using CommonUtilities;

namespace UsefulDBMethods.Reporting
{
	/// <summary>
	/// Description of SystemReportUtilities.
	/// </summary>
	public class SystemReportUtilities
	{
		private const string FModuleName = "SystemReportUtilities";
		public string ModuleName { get { return FModuleName; } }
		
		private Int64 FReportId = 0;
		public Int64 SystemReportId { get { return FReportId; } }
		public string ConnectionString { get; set; }
		public Int64 CompanyId { get; set; }
		public Int64 ClientId { get; set; }
		public Int64 PassedId { get; set; }
		public string ReportSource { get; set; }
		private string FErrorMsg = "";
		public string ErrorMsg { get { return FErrorMsg;} }
		
		public SystemReportUtilities(string ConnectionString,
		                             Int64 CompanyId,
		                             Int64 ClientId,
		                             Int64 PassedId,
		                             string ReportSource)
		{
			this.FErrorMsg = "";
			this.ConnectionString = ConnectionString;
			this.CompanyId = CompanyId;
			this.ClientId = ClientId;
			this.PassedId = PassedId;
			this.ReportSource = ReportSource;
			FReportId = GetNewSystemReportId();
		}
		
		private Int64 GetNewSystemReportId()
		{
			Int64 Result = 
				UsefulDBMethods.GeneratorHelpers.GetNextGeneratorId(
					ConnectionString, "I_SYS_ID_GEN", (int)CompanyId, (int)ClientId, out FErrorMsg);
			return Result;
		}
		
		public static Int64 Execute(string ConnectionString,
		                            Int64 CompanyId,
		                            Int64 ClientId,
		                            Int64 PassedId,
		                            string ReportSource,
		                            out string ErrorMsg,
		                            out string OutputFileName)
		{
			ErrorMsg = "";
			OutputFileName = "";
			string Method = "Execute";
			SystemReportUtilities inst = new SystemReportUtilities(ConnectionString, CompanyId, ClientId, PassedId, ReportSource);
			try
			{
				WindowsEventLoggingUtilities.WriteToInfoEvent("ReportSource: " + ReportSource, "SystemReportUtilities", 0);
				if (string.IsNullOrEmpty(inst.ErrorMsg))
				{
					inst.InsertEASystemReportRecord();
				}
				
				if (string.IsNullOrEmpty(inst.ErrorMsg))
				{
					inst.PerformStep1();
				}
				
				string FileCreatedSql = "";
				string SqlToUse = "";
				string OutputFileNameOnly = "";
				string Extension = "";				
				if (string.IsNullOrEmpty(inst.ErrorMsg))
				{
					FileCreatedSql = inst.GetSys_FileCreated();
				}
				
				if (string.IsNullOrEmpty(inst.ErrorMsg))
				{
					if (!string.IsNullOrEmpty(FileCreatedSql))
					{
						OutputFileName = inst.GetCreatedSystemReportFileName(FileCreatedSql);
						if (string.IsNullOrEmpty(ErrorMsg))
						{
							if (!File.Exists(OutputFileName))
							{
								inst.FErrorMsg = "The file: " + OutputFileName + " does not exist.";
							}
						}
						
						if (string.IsNullOrEmpty(inst.ErrorMsg))
						{
							string[] Delimiters = new string[] {Path.DirectorySeparatorChar.ToString()};
							string[] PathParts = OutputFileName.Split(Delimiters, StringSplitOptions.None);
							OutputFileNameOnly = PathParts[PathParts.Length - 1];
							OutputFileName = OutputFileNameOnly;
						}
					}
					else
					{				
						inst.GetSqlToUse(out SqlToUse, out OutputFileName, out OutputFileNameOnly, out Extension);
						if (string.IsNullOrEmpty(ErrorMsg))
						{
							inst.FErrorMsg = WriteOutputFile(ConnectionString, SqlToUse, OutputFileName, Extension);
							OutputFileNameOnly = OutputFileNameOnly + Extension;
							OutputFileName = OutputFileNameOnly;
						}
					}
				}
				
				/*
				if (string.IsNullOrEmpty(inst.ErrorMsg))
				{
					Result = LogOnProcessingError(Method, ErrorMsg, "");
				}
				else
				{
					Result =
						JsonUtilities.StartJson() +
						JsonUtilities.AddPropertyNameAndValue("FileName", @"/Docs/" + OutputFileNameOnly, false) +
						JsonUtilities.EndJson();
				}
				*/
			}
			catch (Exception Ex)
			{
				inst.FErrorMsg =
					Method + Environment.NewLine +
					Ex.Message + Environment.NewLine +
					Ex.StackTrace;					
			}
			
			ErrorMsg = inst.ErrorMsg;			
			return inst.SystemReportId;			
		}
		
		private void InsertEASystemReportRecord()
		{
			string Method = "InsertEASystemReportRecord";
			try 
			{
				string QueryText =
					" INSERT INTO EASYSTEMREPORT (I_SYS_ID, I_SYS_COMPANYCLIENTID, SYS_SOURCE, I_PASSED_ID, I_SYS_ADDEDBYID )" +
					" VALUES(@NewId, @CompanyId, @Source, @PassedId, @ClientId)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("@NewId", FReportId));
				Params.Add(new FbParameterItem("@CompanyId", CompanyId));
				Params.Add(new FbParameterItem("@Source", ReportSource));
				Params.Add(new FbParameterItem("@PassedId", PassedId));
				Params.Add(new FbParameterItem("@ClientId", ClientId));
				FErrorMsg = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				
				if (string.IsNullOrEmpty(FErrorMsg))
				{
					FErrorMsg = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
				}
			}
			catch (Exception Ex)
			{
				FErrorMsg =
					Method + Environment.NewLine +
					Ex.Message + Environment.NewLine +
					Ex.StackTrace;
			}
		}
		
		private void PerformStep1()
		{
			string Method = "PerformStep1";
			
			try 
			{
				string QueryText = "SELECT First 1 SYS_Step1 FROM EASystemReport WHERE I_SYS_ID = @NewId";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("@NewId", FReportId));
				FErrorMsg = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				
				DataTable tbl = null;
				if (string.IsNullOrEmpty(FErrorMsg))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out FErrorMsg);
				}
				
				string Step1 = "";
				if (string.IsNullOrEmpty(FErrorMsg))
				{
					Object Obj = tbl.Rows[0]["SYS_Step1"];
					Step1 = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					Step1 = Step1.Trim();
					WindowsEventLoggingUtilities.WriteToInfoEvent("Step1: " + Step1, Method, 0);
                    if (!string.IsNullOrEmpty(Step1)) PerformTestRPO(Step1);
                }

                /*
                if ((string.IsNullOrEmpty(FErrorMsg)) && (!string.IsNullOrEmpty(Step1)))
                {
                    //if (Step1.ToUpper().StartsWith("TESTRPO"))
                    if (Step1.ToUpper().StartsWith("SELECT"))
                    { }
                    else
                    {
                        PerformTestRPO(Step1);
					}
				}
			    */
            }
			catch (Exception Ex)
			{
				FErrorMsg =
					Method + Environment.NewLine +
					Ex.Message + Environment.NewLine +
					Ex.StackTrace;
			}
		}
		
		private void PerformTestRPO(string CommandLine)
		{
			string Method = "PerformTestRPO";
			string CurrentDirectory = Directory.GetCurrentDirectory();
			string WorkingDirectory = UsefulDBMethods.CompanyFolderSettings.TestRPOLocation(ConnectionString, CompanyId, out FErrorMsg);
			string CmdBase = "";
			//string AppName = "";
			
			if (string.IsNullOrEmpty(FErrorMsg))
			{
				if (!WorkingDirectory.EndsWith(Path.DirectorySeparatorChar.ToString())) WorkingDirectory += Path.DirectorySeparatorChar.ToString();
				/*
				if (!CmdBase.EndsWith(@"\")) CmdBase += @"\";
				string CmdBase = UsefulDBMethods.CompanyFolderSettings.TestRPOLocation(ConnectionString, CompanyId, out FErrorMsg);
				if (!CmdBase.EndsWith(@"\")) CmdBase += @"\";
				CmdBase += "{0}";
				*/
				WindowsEventLoggingUtilities.WriteToInfoEvent("CurrentDirectory: " + CurrentDirectory, "SystemReportUtilities", 0);
				WindowsEventLoggingUtilities.WriteToInfoEvent("WorkingDirectory: " + WorkingDirectory, "SystemReportUtilities", 0);
				CmdBase = WorkingDirectory + "{0}";
				
				Directory.SetCurrentDirectory(WorkingDirectory);
				try
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent("New CurrentDirectory: " + Directory.GetCurrentDirectory(), "SystemReportUtilities", 0);
					string AppName = CommandLine.Substring(0, CommandLine.IndexOf(" /"));
					AppName = string.Format(CmdBase, AppName);
					
					string ArgsBase = CommandLine.Substring(CommandLine.IndexOf("/"));
					WindowsEventLoggingUtilities.WriteToInfoEvent("AppName: " + AppName, Method, 0);
					WindowsEventLoggingUtilities.WriteToInfoEvent("Args: " + ArgsBase, Method, 0);
					
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
						FErrorMsg = "Application did not finish in time.";
					}
					
					WindowsEventLoggingUtilities.WriteToInfoEvent("After calling: " + AppName + " Result: " + FErrorMsg, Method, 0);
				}
				finally
				{
					Directory.SetCurrentDirectory(CurrentDirectory);					
				}
				
			}
		}
		
		private string GetSys_FileCreated()
		{
			string Result = "";
			//ErrorMsg = "";
			string Method = "GetSys_FileCreated";
			
			try 
			{
				string QueryText = "SELECT First 1 SYS_FileCreated FROM EASystemReport WHERE I_SYS_ID = @NewId";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("@NewId", FReportId));
				FErrorMsg = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				
				DataTable tbl = null;
				if (string.IsNullOrEmpty(FErrorMsg))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out FErrorMsg);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Object Obj = tbl.Rows[0]["SYS_FileCreated"];
					Result = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					Result = Result.Trim();
					Result = Result.Replace("\"", "'");
					WindowsEventLoggingUtilities.WriteToInfoEvent("Sys_FileCreated: " + Result, Method, 0);
				}
			}
			catch (Exception Ex)
			{
				Result =
					Method + Environment.NewLine +
					Ex.Message + Environment.NewLine +
					Ex.StackTrace;
			}

			return Result;
		}
		
		private string GetCreatedSystemReportFileName(string SqlToUse)
		{
			string Result = "";
			string Method = "GetCreatedSystemReportFileName";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " + Method, Method, 0);
			
			try 
			{
				DataTable tbl =
					UsefulDBMethods.Helpers.GetFirstDataTableEx(
						ConnectionString, SqlToUse, null, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out FErrorMsg);
				if (string.IsNullOrEmpty(FErrorMsg))
				{
					object Obj = tbl.Rows[0][0];
					Result = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
				}
			}
			catch (Exception Ex)
			{
				
				FErrorMsg =
					Method + Environment.NewLine +
					Ex.Message + Environment.NewLine +
					Ex.StackTrace;
			}
			
			return Result;
		}
		
		private void GetSqlToUse(out string SqlToUse,
		                         out string FileName,
		                         out string FileNameOnly,
		                         out string Extension)
		{
			string Method = "GetSqlToUse";
			SqlToUse = "";
			FileName = "";
			FileNameOnly = "";
			Extension = "";
			
			string QueryText =
				" SELECT First 1 SYS_SqlBase64, SYS_FileName, SYS_Extension FROM EASystemReport" +
				" WHERE I_SYS_ID = @NewId";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("@NewId", FReportId));
			FErrorMsg = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
			
			DataTable tbl = null;
			if (string.IsNullOrEmpty(FErrorMsg))
			{
				tbl =
					UsefulDBMethods.Helpers.GetFirstDataTableEx(
						ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out FErrorMsg);
			}
			
			if ((string.IsNullOrEmpty(FErrorMsg)) && (tbl.Rows.Count > 0))
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
					string FilePath = UsefulDBMethods.CompanyFolderSettings.DocsLocation(ConnectionString, CompanyId, out FErrorMsg);
					if (!FilePath.EndsWith(@"\")) FilePath += @"\";
					FileName = FilePath + FileName;
				}
				
				WindowsEventLoggingUtilities.WriteToInfoEvent("SqlToUse: " + SqlToUse + " FileName: " + FileName, Method, 0);
			}
		}
		
		public static string WriteOutputFile(string ConnectionString,
		                                     string SqlToUse,
		                                     string FileName,
		                                     string Extension)
		{
			string Method = "WriteOutputFile";
			string Result = "";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " + Method, Method, 0);
			WindowsEventLoggingUtilities.WriteToInfoEvent("SqlToUse:" + Environment.NewLine + SqlToUse, Method, 0);
			
			DataTable tbl =
				UsefulDBMethods.Helpers.GetFirstDataTableEx(
					ConnectionString, SqlToUse, null, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
			if ((string.IsNullOrEmpty(Result)) && (tbl.Rows.Count > 0))
			{
				Object Obj = tbl.Rows[0][0];
				byte[] toBytes = (byte[])Obj;
				string Base64String = System.Text.Encoding.Default.GetString(toBytes);
				File.WriteAllBytes(FileName + Extension, Convert.FromBase64String(Base64String));
				WindowsEventLoggingUtilities.WriteToInfoEvent("After writing the file: " + FileName + Extension, Method, 0);
			}
			
			if (!File.Exists(FileName + Extension)) Result = "The file: " + FileName + " does NOT exist.";
			return Result;
		}
		
		public static string WriteOutputFile(string ConnectionString,
		                                     string SqlToUse,
		                                     string FileName)
		{
			string Method = "WriteOutputFile";
			string Result = "";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " + Method, Method, 0);
			WindowsEventLoggingUtilities.WriteToInfoEvent("SqlToUse:" + Environment.NewLine + SqlToUse, Method, 0);
			
			DataTable tbl =
				UsefulDBMethods.Helpers.GetFirstDataTableEx(
					ConnectionString, SqlToUse, null, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
			if ((string.IsNullOrEmpty(Result)) && (tbl.Rows.Count > 0))
			{
				Object Obj = tbl.Rows[0][0];
				byte[] toBytes = (byte[])Obj;
				string Base64String = System.Text.Encoding.Default.GetString(toBytes);
				File.WriteAllBytes(FileName, Convert.FromBase64String(Base64String));
				WindowsEventLoggingUtilities.WriteToInfoEvent("After writing the file: " + FileName, Method, 0);
			}
			
			if (!File.Exists(FileName)) Result = "The file: " + FileName + " does NOT exist.";
			return Result;
		}
		
		public static string WriteBinaryOutputFile(string ConnectionString,
		                                           string SqlToUse,
		                                           string FileNameIncludingExtn)
		{
			string Method = "WriteBinaryOutputFile";
			string Result = "";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " + Method, Method, 0);
			WindowsEventLoggingUtilities.WriteToInfoEvent("SqlToUse:" + Environment.NewLine + SqlToUse, Method, 0);
			
			DataTable tbl =
				UsefulDBMethods.Helpers.GetFirstDataTableEx(
					ConnectionString, SqlToUse, null, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
			if (string.IsNullOrEmpty(Result) && (tbl.Rows.Count == 0)) Result = "No entry to process";
			if (string.IsNullOrEmpty(Result))
			{
				Object Obj = tbl.Rows[0][0];
				byte[] toBytes = (byte[])Obj;
				using (BinaryWriter writer = new BinaryWriter(File.Open(FileNameIncludingExtn, FileMode.Create)))
				{
					writer.Write(toBytes);
					//File.Write( AllBytes(FileName + Extension, Convert.FromBase64String(Base64String));
					WindowsEventLoggingUtilities.WriteToInfoEvent("After writing the file: " + FileNameIncludingExtn, Method, 0);
				}
			}
			
			if (!File.Exists(FileNameIncludingExtn)) Result = "The file: " + FileNameIncludingExtn + " does NOT exist.";
			return Result;
		}
		
		public static string WriteBinaryOutputFile(string ConnectionString,
		                                           string SqlToUse,
		                                           string FileName,
		                                           string Extension)
		{
			return WriteBinaryOutputFile(ConnectionString, SqlToUse, FileName + Extension);
		}
	}
}
