/*
 * Created by SharpDevelop.
 * User: John
 * Date: 01/10/2019
 * Time: 4:25 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using EAOfficeAPI;
using System.Text;
using System.Data;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using CommonUtilities;
using System.Diagnostics;
using System.IO;

namespace Actions.Reconcilliation
{
	/// <summary>
	/// Description of CommonMethods.
	/// </summary>
	public class CommonMethods
	{
		public CommonMethods()
		{
		}
		
		public static string LoadGatherGridEntriesRequestItem(ActionBase AActionBase,
		                                                      ref GatherGridEntries.GatherGridEntriesRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadGatherGridEntriesRequestItem";
			try 
			{
				Object Obj = AActionBase.GetCollectionValue("Data", "bankAccountType");
				TempItem.BankAccountTypeValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = AActionBase.GetCollectionValue("Data", "useDefaults");
				TempItem.UseDefaults = (Obj == null) ? "Y" : Convert.ToString(Obj);
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
		
		public static string LoadGatherReportsGridEntriesRequestItem(ActionBase AActionBase,
		                                                             ref GatherReportsGridEntries.GatherReportsGridEntriesRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadGatherReportsGridEntriesRequestItem";
			try 
			{
				Object Obj = AActionBase.GetCollectionValue("Data", "bankId");
				TempItem.BankIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = AActionBase.GetCollectionValue("Data", "comboId");
				TempItem.ComboIdValue= (Obj == null) ? "" : Convert.ToString(Obj); 
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
		
		public static string LoadGetReconcilliationReportRequestItem(ActionBase AActionBase,
		                                                             ref GetReconcilliationReport.GetReconcilliationReportRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadGetReconcilliationReportRequestItem";
			try 
			{
				Object Obj = AActionBase.GetCollectionValue("Data", "bankId");
				TempItem.BankIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = AActionBase.GetCollectionValue("Data", "reportId");
				TempItem.ReportIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = AActionBase.GetCollectionValue("Data", "comboId");
				TempItem.ComboIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = AActionBase.GetCollectionValue("Data", "buttonId");
				TempItem.ButtonIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
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
		
		public static string LoadGatherReconcileGridEntriesRequestItem(ActionBase AActionBase,
		                                                             ref GatherReconcileGridEntries.GatherReconcileGridEntriesRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadGatherReconcileGridEntriesRequestItem";
			try 
			{
				Object Obj = AActionBase.GetCollectionValue("Data", "bankId");
				TempItem.BankIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = AActionBase.GetCollectionValue("Data", "bankType");
				TempItem.BankTypeValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = AActionBase.GetCollectionValue("Data", "pageNo");
				TempItem.PageNoValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = AActionBase.GetCollectionValue("Data", "presentedPageNo");
				TempItem.PresentedPageNoValue = (Obj == null) ? "" : Convert.ToString(Obj); 
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
		
		public static string LoadGatherSystemReportsGridEntriesRequestItem(ActionBase AActionBase,
		                                                                   ref GatherSystemReportsGridEntries.GatherSystemReportsGridEntriesRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadGatherSystemReportsGridEntriesRequestItem";
			try 
			{
				Object Obj = AActionBase.GetCollectionValue("Data", "bankId");
				TempItem.BankIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
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
		
		public static string LoadGetSystemReportRequestItem(ActionBase AActionBase,
		                                                    ref GetSystemReport.GetSystemReportRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadGetSystemReportRequestItem";
			try 
			{
				Object Obj = AActionBase.GetCollectionValue("Data", "bankId");
				TempItem.BankIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = AActionBase.GetCollectionValue("Data", "reportId");
				TempItem.ReportIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = AActionBase.GetCollectionValue("Data", "buttonId");
				TempItem.ButtonIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
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
		
		public static string GetEndOfMonthReportsGroupEntries(string ConnectionString, 
		                                                      Int64 CompanyId,
		                                                      Int64 BankId,
		                                                      int CategoryId,
		                                                      ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "GetEndOfMonthReportsGroupEntries";
			
			string QueryText = "SELECT OutputLine FROM EndOfMonthReportsGroup(@CompanyId, @BankId, @CategoryOnlyId)";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("@CompanyId", CompanyId));
			Params.Add(new FbParameterItem("@BankId", BankId));
			Params.Add(new FbParameterItem("@CategoryOnlyId", CategoryId));
			Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
			DataTable tbl = null;
			if (string.IsNullOrEmpty(Result))
			{
				tbl =
					UsefulDBMethods.Helpers.GetFirstDataTableEx(
						ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
			}
				
			if (string.IsNullOrEmpty(Result))
			{
				foreach (DataRow Row in tbl.Rows)
				{
					Object Obj = Row["OUTPUTLINE"];
					Entries.AppendLine((Obj == DBNull.Value) ? "" : Convert.ToString(Obj));
				}
			}
			
			return Result;
		}
		
		public static string GetEndOfMonthPrintGroupEntries(string ConnectionString,
		                                                    Int64 CompanyId,
		                                                    int BankId,
		                                                    ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "GetEndOfMonthPrintGroupEntries";
			
			string QueryText = "SELECT OutputLine FROM EndOfMonthPrintGroup(@CompanyId, @BankId)";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("@CompanyId", CompanyId));
			Params.Add(new FbParameterItem("@BankId", BankId));
			Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
			DataTable tbl = null;
			if (string.IsNullOrEmpty(Result))
			{
				tbl =
					UsefulDBMethods.Helpers.GetFirstDataTableEx(
						ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
			}
				
			if (string.IsNullOrEmpty(Result))
			{
				foreach (DataRow Row in tbl.Rows)
				{
					Object Obj = Row["OUTPUTLINE"];
					Entries.AppendLine((Obj == DBNull.Value) ? "" : Convert.ToString(Obj));
				}
			}
			
			return Result;
		}
		
		public static string ProcessTestRPOCommandLine(string CommandLine,
		                                               string TestRPOLocation,
		                                               int Tries)
		{
			string Result = "";
			string Method = "ProcessTestRPOCommandLine";
			
			if (string.IsNullOrEmpty(CommandLine))
			{
				Result = "Invalid or empty Command Line provided in " + Method;
				return Result;
			}
			
			if (string.IsNullOrEmpty(TestRPOLocation))
			{
				Result = "Invalid or empty location in " + Method;
				return Result;
			}
			
			if (Tries <= 0) Tries = 1200; // 20 minutes
			
			string CmdBase = TestRPOLocation;
			if (string.IsNullOrEmpty(Result))
			{
				if (!CmdBase.EndsWith(@"\")) CmdBase += @"\";
				CmdBase += "{0}";
				string AppName = CommandLine.Substring(0, CommandLine.IndexOf(" /"));
				AppName = string.Format(CmdBase, AppName);
				
				string ArgsBase = CommandLine.Substring(CommandLine.IndexOf("/"));
				WindowsEventLoggingUtilities.WriteToInfoEvent("AppName: " + AppName, Method, 0);
				WindowsEventLoggingUtilities.WriteToInfoEvent("Args: " + ArgsBase, Method, 0);
				
				Process runApp = new Process();
				runApp.StartInfo.FileName = AppName;
				runApp.StartInfo.Arguments = ArgsBase;
				runApp.Start();
				for (int Index = 0; Index < Tries; Index++)
				{
					if (!runApp.HasExited) System.Threading.Thread.Sleep(1000);
				}
				if (!runApp.HasExited)
				{
					Result = "Application did not finish in time.";
				}
				
				WindowsEventLoggingUtilities.WriteToInfoEvent("After calling: " + AppName + " Result: " + Result, Method, 0);
			}
			return Result;
		}
		
		public static string LoadSavePageNumberRequestItem(ActionBase AActionBase,
		                                                   ref SavePageNumber.SavePageNumberRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadSavePageNumberRequestItem";
			try 
			{
				Object Obj = AActionBase.GetCollectionValue("Data", "bankId");
				TempItem.BankIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = AActionBase.GetCollectionValue("Data", "pageNumber");
				TempItem.PageNumberValue = (Obj == null) ? "" : Convert.ToString(Obj); 
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
		
		public static string LoadPresentUnpresentRequestItem(ActionBase AActionBase,
		                                                     ref PresentUnpresent.PresentUnpresentRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadPresentUnpresentRequestItem";
			try 
			{
				Object Obj = AActionBase.GetCollectionValue("Data", "bankId");
				TempItem.BankIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = AActionBase.GetCollectionValue("Data", "bankType");
				TempItem.BankTypeValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = AActionBase.GetCollectionValue("Data", "recordId");
				TempItem.RecordIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = AActionBase.GetCollectionValue("Data", "pageNumber");
				TempItem.PageNumberValue = (Obj == null) ? "" : Convert.ToString(Obj); 
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
		
		public static string LoadGenerateBankStmtRptRequestItem(ActionBase AActionBase,
		                                                        ref GenerateBankStmtRpt.GenerateBankStmtRptRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadGenerateBankStmtRptRequestItem";
			try 
			{
				Object Obj = AActionBase.GetCollectionValue("Data", "bankId");
				TempItem.BankIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
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
		
		public static string InsertEASystemReportRecord(string ConnectionString,
		                                                Int64 CompanyId,
		                                                Int64 ClientId,
		                                                Int64 NewId,
		                                                int PassedId,
		                                                string ReportSource)
		{
			string Result = "";
			string Method = "InsertEASystemReportRecord";
			try 
			{
				string QueryText =
					" INSERT INTO EASYSTEMREPORT (I_SYS_ID, I_SYS_COMPANYCLIENTID, SYS_SOURCE, I_PASSED_ID, I_SYS_ADDEDBYID )" +
					" VALUES(@NewId, @CompanyId, @Source, @PassedId, @ClientId)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("@NewId", NewId));
				Params.Add(new FbParameterItem("@CompanyId", CompanyId));
				Params.Add(new FbParameterItem("@Source", ReportSource));
				Params.Add(new FbParameterItem("@PassedId", PassedId));
				Params.Add(new FbParameterItem("@ClientId", ClientId));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
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
		
		public static string PerformStep1(string ConnectionString,
		                                  Int64 NewId,
		                                  Int64 CompanyId,
		                                  Int64 ClientId)
		{
			string Result = "";
			string Method = "PerformStep1";
			
			try 
			{
				string QueryText = "SELECT First 1 SYS_Step1 FROM EASystemReport WHERE I_SYS_ID = @NewId";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("@NewId", NewId));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				
				DataTable tbl = null;
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
				}
				
				string Step1 = "";
				if (string.IsNullOrEmpty(Result))
				{
					Object Obj = tbl.Rows[0]["SYS_Step1"];
					Step1 = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					Step1 = Step1.Trim();
					WindowsEventLoggingUtilities.WriteToInfoEvent("Step1: " + Step1, Method, 0);
				}
				
				if ((string.IsNullOrEmpty(Result)) && (!string.IsNullOrEmpty(Step1)))
				{
					//if (Step1.ToUpper().StartsWith("TESTRPO"))
                    if (!Step1.ToUpper().StartsWith("SELECT"))
                    {
                            Result = PerformTestRPO(ConnectionString, CompanyId, Step1);
					}
					else if (Step1.ToUpper().StartsWith("SELECT"))
					{
						// Don't know what to do here
					}
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
		
		public static string GetSys_FileCreated(string ConnectionString,
		                                        Int64 NewId, 
		                                        out string ErrorMsg)
		{
			string Result = "";
			ErrorMsg = "";
			string Method = "GetSys_FileCreated";
			
			try 
			{
				string QueryText = "SELECT First 1 SYS_FileCreated FROM EASystemReport WHERE I_SYS_ID = @NewId";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("@NewId", NewId));
				ErrorMsg = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				
				DataTable tbl = null;
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out ErrorMsg);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Object Obj = tbl.Rows[0]["SYS_FileCreated"];
					Result = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					Result = Result.Trim();
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
		
		public static string PerformTestRPO(string ConnectionString,
		                                    Int64 CompanyId,
		                                    string CommandLine)
		{
			string Result = "";
			string Method = "PerformTestRPO";
			string CmdBase = global::UsefulDBMethods.CompanyFolderSettings.TestRPOLocation(ConnectionString, CompanyId, out Result);
			if (string.IsNullOrEmpty(Result))
			{
				if (!CmdBase.EndsWith(@"\")) CmdBase += @"\";
				CmdBase += "{0}";
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
					Result = "Application did not finish in time.";
				}
				//runApp.WaitForExit(10000);
				
				WindowsEventLoggingUtilities.WriteToInfoEvent("After calling: " + AppName + " Result: " + Result, Method, 0);
			}
			return Result;
		}
		
		public static string GetSqlToUse(string ConnectionString,
		                                 Int64 CompanyId,
		                                 int NewId,
		                                 out string SqlToUse,
		                                 out string FileName,
		                                 out string FileNameOnly,
		                                 out string Extension)
		{
			string Result = "";
			string Method = "GetSqlToUse";
			SqlToUse = "";
			FileName = "";
			FileNameOnly = "";
			Extension = "";
			
			string QueryText =
				" SELECT First 1 SYS_SqlBase64, SYS_FileName, SYS_Extension FROM EASystemReport" +
				" WHERE I_SYS_ID = @NewId";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("@NewId", NewId));
			Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
			
			DataTable tbl = null;
			if (string.IsNullOrEmpty(Result))
			{
				tbl =
					UsefulDBMethods.Helpers.GetFirstDataTableEx(
						ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
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
					string FilePath = global::UsefulDBMethods.CompanyFolderSettings.DocsLocation(ConnectionString, CompanyId, out Result);
					if (!FilePath.EndsWith(@"\")) FilePath += @"\";
					FileName = FilePath + FileName;
				}
				
				WindowsEventLoggingUtilities.WriteToInfoEvent("SqlToUse: " + SqlToUse + " FileName: " + FileName, Method, 0);
			}
			
			return Result;
		}
		
		public static string GetCreatedSystemReportFileName(string ConnectionString,
		                                                    string SqlToUse, 
		                                                    out string ErrorMsg)
		{
			string Result = "";
			string Method = "GetCreatedSystemReportFileName";
			ErrorMsg = "";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Original SqlToUse: " + SqlToUse, Method, 0);
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " + Method, Method, 0);
			
			try 
			{
                SqlToUse = SqlToUse.Replace("\"", "'");
                WindowsEventLoggingUtilities.WriteToInfoEvent("New SqlToUse: " + SqlToUse, Method, 0);
                DataTable tbl =
					UsefulDBMethods.Helpers.GetFirstDataTableEx(
						ConnectionString, SqlToUse, null, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out ErrorMsg);
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					object Obj = tbl.Rows[0][0];
					Result = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
				}
			}
			catch (Exception Ex)
			{
				
				ErrorMsg =
					Method + Environment.NewLine +
					Ex.Message + Environment.NewLine +
					Ex.StackTrace;
			}
			
			return Result;
			
		}
		
		public static string WriteOutputFile(string ConnectionString,
		                                     string SqlToUse,
		                                     string FileName,
		                                     string Extension)
		{
			string Result = "";
			string Method = "WriteOutputFile";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " + Method, Method, 0);
			
			DataTable tbl =
				UsefulDBMethods.Helpers.GetFirstDataTableEx(
					ConnectionString, SqlToUse, null, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
			if ((string.IsNullOrEmpty(Result)) && (tbl.Rows.Count > 0))
			{
				Object Obj = tbl.Rows[0][0];
				byte[] toBytes = (byte[])Obj;
				string Base64String = System.Text.Encoding.Default.GetString(toBytes);
				File.WriteAllBytes(FileName + Extension, Convert.FromBase64String(Base64String));
				WindowsEventLoggingUtilities.WriteToInfoEvent("After writing the file: " + FileName, Method, 0);
			}
			
			if (!File.Exists(FileName + Extension)) Result = "The file: " + FileName + " does NOT exist.";
			
			return Result;
		}
		
		public static string EABankEntriesGroup(string ConnectionString,
		                                        Int64 CompanyId, 
		                                        int BankId, 
		                                        int BankType, 
		                                        int PageNo,
		                                        int PresentedPageNo,
		                                        ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "EABankEntriesGroup";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
			
			ActionBase ActionBase = new ActionBase();					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				string QueryText = 
					" SELECT OUTPUTLINE" +
					" FROM EABankEntriesGroup(@CompanyId, @BankId, @BankType, @PageNo, @PresentedPageNo)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("@CompanyId", CompanyId));
				Params.Add(new FbParameterItem("@BankId", BankId));
				Params.Add(new FbParameterItem("@BankType", BankType));
				Params.Add(new FbParameterItem("@PageNo", PageNo));
				Params.Add(new FbParameterItem("@PresentedPageNo", PresentedPageNo));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
	
				DataTable tbl = null;
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					foreach (DataRow Row in tbl.Rows)
					{
						Object Obj = Row["OUTPUTLINE"];
						Entries.AppendLine((Obj == DBNull.Value) ? "" : Convert.ToString(Obj));
					}
				}
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
			}
			
			return Result;
		}
		
		public static string LoadGenerateReconcileRptRequestItem(ActionBase AActionBase,
		                                                             ref GenerateReconcileRptRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadGenerateReconcileRptRequestItem";
			try 
			{
				Object Obj = AActionBase.GetCollectionValue("Data", "bankId");
				TempItem.BankIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = AActionBase.GetCollectionValue("Data", "bankType");
				TempItem.BankTypeValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = AActionBase.GetCollectionValue("Data", "pageNo");
				TempItem.PageNoValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = AActionBase.GetCollectionValue("Data", "presentedPageNo");
				TempItem.PresentedPageNoValue = (Obj == null) ? "" : Convert.ToString(Obj); 

				Obj = AActionBase.GetCollectionValue("Data", "RecordId");
				TempItem.RecordIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = AActionBase.GetCollectionValue("Data", "ButtonId");
				TempItem.ButtonIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
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
	}
}
