/*
 * Created by SharpDevelop.
 * User: John
 * Date: 25/09/2019
 * Time: 11:24 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CommonUtilities;
using EAOfficeAPI;

namespace Actions.ComboBoxEntries
{
	/// <summary>
	/// Description of CommonMethods.
	/// </summary>
	public class CommonMethods
	{
		//private string FModuleName = "ComboBoxEntries";
		public CommonMethods()
		{
		}
		
		public static string LoadCommonFilteredLookupsItem(JObject DataObject,
		                                                   ref CommonFilteredLookupsItem TempItem)
		{
			string Result = "";
			string Method = "LoadCommonFilteredLookupsItem";
			try 
			{
				TempItem.PortfolioIdValue = (string)DataObject["PortfolioId"];
				TempItem.CategoryOnlyIdValue = (string)DataObject["CategoryOnlyId"];
				TempItem.DlgType = (string)DataObject["DlgType"];
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
		
		public static string BuildOutput_PortfolioContacts(string ConnectionString,
		                                                   Int64 CompanyId,
		                                                   int PortfolioId,
		                                                   int CategoryOnlyId,
		                                                   ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "BuildOutput_PortfolioContacts";
			string QueryText = 
				" SELECT OUTPUTLINE" +
				" FROM EATRANSACTIONPHONESGROUP(@CompanyId, @PortfolioId, @CategoryOnlyId)";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("@CompanyId", CompanyId));
			Params.Add(new FbParameterItem("@PortfolioId", PortfolioId));
			Params.Add(new FbParameterItem("CategoryOnlyId", CategoryOnlyId));
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
					//Entries.AppendLine((Row[0] == DBNull.Value) ? "" : Convert.ToString(Row[0]));
					Entries.AppendLine((Obj == DBNull.Value) ? "" : Convert.ToString(Obj));
				}
			}
			
			return Result;
		}
		
		public static string BuildOutput_PortfolioStorage(string ConnectionString,
		                                                  Int64 CompanyId,
		                                                  int PortfolioId,
		                                                  int CategoryOnlyId,
		                                                  ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "BuildOutput_PortfolioStorage";
			string QueryText = 
				" SELECT OUTPUTLINE" +
				" FROM EAGETBUSINESSSTORAGEGROUP(@CompanyId, @PortfolioId, @CategoryOnlyId)";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("@CompanyId", CompanyId));
			Params.Add(new FbParameterItem("@PortfolioId", PortfolioId));
			Params.Add(new FbParameterItem("CategoryOnlyId", CategoryOnlyId));
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
					//Entries.AppendLine((Row[0] == DBNull.Value) ? "" : Convert.ToString(Row[0]));
					Entries.AppendLine((Obj == DBNull.Value) ? "" : Convert.ToString(Obj));
				}
			}
			
			return Result;
		}
		
		public static string BuildOutput_PortfolioTasks(string ConnectionString,
		                                                Int64 CompanyId,
		                                                int PortfolioId,
		                                                int CategoryOnlyId,
		                                                ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "BuildOutput_PortfolioTasks";
			string QueryText = 
				" SELECT OUTPUTLINE" +
				" FROM EATransactionTaskGroup(@CompanyId, @PortfolioId, @CategoryOnlyId)";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("@CompanyId", CompanyId));
			Params.Add(new FbParameterItem("@PortfolioId", PortfolioId));
			Params.Add(new FbParameterItem("CategoryOnlyId", CategoryOnlyId));
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
					//Entries.AppendLine((Row[0] == DBNull.Value) ? "" : Convert.ToString(Row[0]));
					Entries.AppendLine((Obj == DBNull.Value) ? "" : Convert.ToString(Obj));
				}
			}
			
			return Result;
		}
		
		public static string BuildOutput_PortfolioNotes(string ConnectionString,
		                                                Int64 CompanyId,
		                                                Int64 ClientId,
		                                                int PortfolioId,
		                                                int CategoryOnlyId,
		                                                ref StringBuilder Entries)
		{
			string Result = "";			
			
			StringBuilder EntryList = new StringBuilder();
			
			Result = BuildOutput_PortfolioNotes_ComboEntries(ConnectionString, CompanyId, PortfolioId, CategoryOnlyId,  ref EntryList);
			if (string.IsNullOrEmpty(Result))
			{
				Entries.Append(EntryList.ToString());
				Entries.Append(",");
				EntryList.Length = 0;
				Result = BuildOutput_PortfolioNotes_NoteEntry(ConnectionString, CompanyId, ClientId, PortfolioId, CategoryOnlyId, ref EntryList);
			}
			
			if (string.IsNullOrEmpty(Result))
			{
				Entries.Append(EntryList.ToString());
			}
			
			return Result;
		}
		
		private static string BuildOutput_PortfolioNotes_ComboEntries(string ConnectionString,
		                                                              Int64 CompanyId,
		                                                              int PortfolioId,
		                                                              int CategoryOnlyId,
		                                                              ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "BuildOutput_PortfolioNotes_ComboEntries";
			string QueryText = 
				" SELECT OUTPUTLINE" +
				" FROM EAGETBUSINESSNOTES(@CompanyId, @PortfolioId, @CategoryOnlyId)";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("@CompanyId", CompanyId));
			Params.Add(new FbParameterItem("@PortfolioId", PortfolioId));
			Params.Add(new FbParameterItem("CategoryOnlyId", CategoryOnlyId));
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
		
		private static string BuildOutput_PortfolioNotes_NoteEntry_OLD(string ConnectionString,
		                                                           Int64 CompanyId,
		                                                           Int64 ClientId,
		                                                           int PortfolioId,
		                                                           int CategoryOnlyId,
		                                                           ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "BuildOutput_PortfolioNotes_NoteEntry";

			string HtmlContent = "";
			string SqlToUse = NoteEntry_SqlToUse(ConnectionString, CompanyId, PortfolioId, CategoryOnlyId, out Result);
			WindowsEventLoggingUtilities.WriteToInfoEvent("SqlToUse: " + SqlToUse, Method, 0);
			if ((string.IsNullOrEmpty(Result)) && (!string.IsNullOrEmpty(SqlToUse)))
			{
				string FilePath = CommonUtilities.FolderHelpers.DocsLocation(ConnectionString, CompanyId, out Result);
				if (!FilePath.EndsWith(@"\")) FilePath += @"\";
				string BaseFileName = CompanyId + "_" + ClientId + "_BusinessNote";
				string RTFFileName = BaseFileName + ".rtf";
				string HtmlFileName = BaseFileName + ".html";
				WindowsEventLoggingUtilities.WriteToInfoEvent("FilePath: " + FilePath, Method, 0);
				WindowsEventLoggingUtilities.WriteToInfoEvent("RTFFileName: " + RTFFileName, Method, 0);
				WindowsEventLoggingUtilities.WriteToInfoEvent("HtmlFileName: " + HtmlFileName, Method, 0);
				
				if (File.Exists(FilePath + RTFFileName)) File.Delete(FilePath + RTFFileName);
				if (File.Exists(FilePath + HtmlFileName)) File.Delete(FilePath + HtmlFileName);
	
				if (string.IsNullOrEmpty(Result))
				{
					Result = NoteEntry_WriteSqlResultToFile(ConnectionString, SqlToUse, FilePath + RTFFileName);
					WindowsEventLoggingUtilities.WriteToInfoEvent("After NoteEntry_WriteSqlResultToFile - Result: " + Result, Method, 0);
				}
				
				if ((string.IsNullOrEmpty(Result)) && (File.Exists(FilePath + RTFFileName)))
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent("Before calling NoteEntry_ConvertRTFFile", Method, 0);
					Result = NoteEntry_ConvertRTFFile(FilePath, RTFFileName, CompanyId);
					WindowsEventLoggingUtilities.WriteToInfoEvent("After calling NoteEntry_ConvertRTFFile", Method, 0);
				
					if ((string.IsNullOrEmpty(Result)) && (File.Exists(FilePath + HtmlFileName)))
					{
						WindowsEventLoggingUtilities.WriteToInfoEvent("The HtmlFile Exists", Method, 0);
						HtmlContent = File.ReadAllText(FilePath + HtmlFileName);
					}			
				}
			}
			
			Entries.Append(JsonUtilities.AddPropertyNameAndValue("noteContent", HtmlContent, false));
			
			return Result;
		}
		
		
		private static string BuildOutput_PortfolioNotes_NoteEntry(string ConnectionString,
		                                                           Int64 CompanyId,
		                                                           Int64 ClientId,
		                                                           int PortfolioId,
		                                                           int CategoryOnlyId,
		                                                           ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "BuildOutput_PortfolioNotes_NoteEntry";

			string HtmlContent = "";
			string SqlToUse = NoteEntry_SqlToUse(ConnectionString, CompanyId, PortfolioId, CategoryOnlyId, out Result);
			WindowsEventLoggingUtilities.WriteToInfoEvent("SqlToUse: " + SqlToUse, Method, 0);
			if ((string.IsNullOrEmpty(Result)) && (!string.IsNullOrEmpty(SqlToUse)))
			{
				DataTable tbl =
					UsefulDBMethods.Helpers.GetFirstDataTableEx(
						ConnectionString, SqlToUse, null, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
				if ((string.IsNullOrEmpty(Result)) && (tbl.Rows.Count > 0))
				{
					Object Obj = tbl.Rows[0]["ANote_NotepadText"];
					if (Obj != DBNull.Value)
					{
						byte[] byteArray = (byte[])Obj;
						HtmlContent = Encoding.UTF8.GetString(byteArray);
					}
				}
				/*
				string FilePath = CommonUtilities.FolderHelpers.DocsLocation(ConnectionString, CompanyId, out Result);
				if (!FilePath.EndsWith(@"\")) FilePath += @"\";
				string BaseFileName = CompanyId + "_" + ClientId + "_BusinessNote";
				string RTFFileName = BaseFileName + ".rtf";
				string HtmlFileName = BaseFileName + ".html";
				WindowsEventLoggingUtilities.WriteToInfoEvent("FilePath: " + FilePath, Method, 0);
				WindowsEventLoggingUtilities.WriteToInfoEvent("RTFFileName: " + RTFFileName, Method, 0);
				WindowsEventLoggingUtilities.WriteToInfoEvent("HtmlFileName: " + HtmlFileName, Method, 0);
				
				if (File.Exists(FilePath + RTFFileName)) File.Delete(FilePath + RTFFileName);
				if (File.Exists(FilePath + HtmlFileName)) File.Delete(FilePath + HtmlFileName);
	
				if (string.IsNullOrEmpty(Result))
				{
					Result = NoteEntry_WriteSqlResultToFile(ConnectionString, SqlToUse, FilePath + RTFFileName);
					WindowsEventLoggingUtilities.WriteToInfoEvent("After NoteEntry_WriteSqlResultToFile - Result: " + Result, Method, 0);
				}
				
				if ((string.IsNullOrEmpty(Result)) && (File.Exists(FilePath + RTFFileName)))
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent("Before calling NoteEntry_ConvertRTFFile", Method, 0);
					Result = NoteEntry_ConvertRTFFile(FilePath, RTFFileName, CompanyId);
					WindowsEventLoggingUtilities.WriteToInfoEvent("After calling NoteEntry_ConvertRTFFile", Method, 0);
				
					if ((string.IsNullOrEmpty(Result)) && (File.Exists(FilePath + HtmlFileName)))
					{
						WindowsEventLoggingUtilities.WriteToInfoEvent("The HtmlFile Exists", Method, 0);
						HtmlContent = File.ReadAllText(FilePath + HtmlFileName);
					}			
				}
				*/
			}
			
			Entries.Append(JsonUtilities.AddPropertyNameAndValue("noteContent", HtmlContent, false));
			
			return Result;
		}

		public static string BuildOutput_PortfolioFinancials(string ConnectionString,
		                                                          Int64 CompanyId,
		                                                          int PortfolioId,
		                                                          int CategoryOnlyId,
		                                                          ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "BuildOutput_PortfolioFinancials";
			string QueryText = 
				" SELECT OUTPUTLINE" +
				" FROM EATRANSACTIONFINANCEGROUP(@CompanyId, @PortfolioId, @CategoryOnlyId)";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("@CompanyId", CompanyId));
			Params.Add(new FbParameterItem("@PortfolioId", PortfolioId));
			Params.Add(new FbParameterItem("CategoryOnlyId", CategoryOnlyId));
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
					//Entries.AppendLine((Row[0] == DBNull.Value) ? "" : Convert.ToString(Row[0]));
					Entries.AppendLine((Obj == DBNull.Value) ? "" : Convert.ToString(Obj));
				}
			}
			
			return Result;
		}
		
		private static string NoteEntry_SqlToUse(string ConnectionString,
		                                         Int64 CompanyId,
		                                         int PortfolioId,
		                                         int CategoryOnlyId,
		                                         out string ErrorMsg)
		{
			string Result = "";
			string Method = "NoteEntry_SqlToUse";
			string QueryText = 
				" SELECT FIRST 1 SqlToUse FROM EAGETBUSINESSNOTES(@CompanyId, @PortfolioId, @CategoryOnlyId)" +
				" WHERE OutputLine containing 'Value' and OutputLine containing @CategoryOnlyId";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("@CompanyId", CompanyId));
			Params.Add(new FbParameterItem("@PortfolioId", PortfolioId));
			Params.Add(new FbParameterItem("@CategoryOnlyId", CategoryOnlyId));
			ErrorMsg = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);

			DataTable tbl = null;
			if (string.IsNullOrEmpty(ErrorMsg))
			{
				tbl =
					UsefulDBMethods.Helpers.GetFirstDataTableEx(
						ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out ErrorMsg);
			}
			
			if ((string.IsNullOrEmpty(ErrorMsg)) && (tbl.Rows.Count > 0))
			{
				Object Obj = tbl.Rows[0]["SqlToUse"];
				Result = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
				WindowsEventLoggingUtilities.WriteToInfoEvent("SqlToUse: " + Result, Method, 0);
			}
			
			return Result;
		}
		
		private static string NoteEntry_WriteSqlResultToFile(string ConnectionString,
		                                                     string SqlToUse,
		                                                     string FileName)
		{
			string Result = "";
			string Method = "NoteEntry_WriteSqlResultToFile";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " + Method, Method, 0);
			
			DataTable tbl =
				UsefulDBMethods.Helpers.GetFirstDataTableEx(
					ConnectionString, SqlToUse, null, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
			if ((string.IsNullOrEmpty(Result)) && (tbl.Rows.Count > 0))
			{
				Object Obj = tbl.Rows[0]["ANote_Notepad"];
				if (Obj != DBNull.Value)
				{
					byte[] byteArray = (byte[])Obj;
					File.WriteAllBytes(FileName, byteArray);				
					WindowsEventLoggingUtilities.WriteToInfoEvent("After writing the file: " + FileName, Method, 0);
				}
			}
			
			return Result;
		}
		
		private static string NoteEntry_ConvertRTFFile(string FilePath,
		                                               string RTFFileName,
		                                               Int64 CompanyClientId)
		{
			// Call the EAOfficeUtilities app to convert the RTF file to HTML
			string Result = "";
			string Method = "NoteEntry_ConvertRTFFile";
			string SepChar = Path.DirectorySeparatorChar.ToString();
			string WorkingDir = Directory.GetCurrentDirectory();
			if (!WorkingDir.EndsWith(SepChar)) WorkingDir += SepChar;
			try
			{
				Process runApp = new Process();
				runApp.StartInfo.FileName = WorkingDir + "EAOfficeUtilities";
				runApp.StartInfo.Verb = "runas";
				List<string> ArgsList = new List<string>();
				ArgsList.Add("cmd=ConvertRTFFile");
				ArgsList.Add("FilePath=" + FilePath);
				ArgsList.Add("FileName=" + RTFFileName);
				ArgsList.Add("CCID=" + Convert.ToString(CompanyClientId));
				string Args = string.Join(" ", ArgsList.ToArray());
				WindowsEventLoggingUtilities.WriteToInfoEvent("Args: " + Args, Method, 0);
				runApp.StartInfo.Arguments = Args;
				runApp.Start();
				const int tries = 60;
				for (int Index = 0; Index < tries; Index++)
				{
					if (!runApp.HasExited) System.Threading.Thread.Sleep(1000);
				}
				if (!runApp.HasExited)
				{
					Result = "The File was not created successfully";
				}
			}
			catch (Exception Ex)
			{
				Result =
					Method + Environment.NewLine +
					Ex.Message + Environment.NewLine +
					Ex.StackTrace;
			}
			finally
			{
				Directory.SetCurrentDirectory(WorkingDir);
			}
			
			return Result;
		}
		
		public static string BuildOutput_PortfolioOwnersCorpReports(string ConnectionString,
		                                                            Int64 CompanyId,
		                                                            int AccountId,
		                                                            int SubAccountId,
		                                                            string DateFrom,
		                                                            string DateTo,
		                                                            ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "BuildOutput_PortfolioOwnersCorpReports";
			string QueryText = 
				" SELECT OUTPUTLINE" +
				" FROM OCViewReportListGroup(@CompanyId, @AccountId, @SubAccountId, @DateFrom, @DateTo)";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("@CompanyId", CompanyId));
			Params.Add(new FbParameterItem("AccountId", AccountId));
			Params.Add(new FbParameterItem("SubAccountId", SubAccountId));
			Params.Add(new FbParameterItem("DateFrom", DateFrom));
			Params.Add(new FbParameterItem("DateTo", DateTo));
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
					//Entries.AppendLine((Row[0] == DBNull.Value) ? "" : Convert.ToString(Row[0]));
					Entries.AppendLine((Obj == DBNull.Value) ? "" : Convert.ToString(Obj));
				}
			}
			
			return Result;
		}
		
		public static string LoadOCViewReportListItem(JObject DataObject,
		                                              ref OCViewReportListItem TempItem)
		{
			string Result = "";
			string Method = "LoadOCViewReportListItem";
			try 
			{
				TempItem.AccountIdValue = (string)DataObject["AccountId"];
				TempItem.SubAccountIdValue = (string)DataObject["SubAccountId"];
				TempItem.DateFrom = (string)DataObject["DateFrom"];
				TempItem.DateTo = (string)DataObject["DateTo"];
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
		
		public static string LoadMgmtPdfReportListRequestItem(ActionBase ActionBase,
		                                                      ref MgmtPdfReportListRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadMgmtPdfReportListRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				Object Obj = ActionBase.GetCollectionValue("Data", "TransactionId");
				TempItem.TransactionIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "AccountId");
				TempItem.AccountIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
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
		
		public static string BuildOutput_PortfolioMgmtPdfReports(string ConnectionString,
		                                                         Int64 CompanyId,
		                                                         int TransactionId,
		                                                         int AccountId,
		                                                         int CategoryOnlyId,
		                                                         ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "BuildOutput_PortfolioMgmtPdfReports";
			string QueryText = 
				" SELECT OUTPUTLINE" +
				" FROM EATransactionFinanceGroup4(@CompanyId, @TransactionId, @AccountId, @CategoryOnlyId)";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("@CompanyId", CompanyId));
			Params.Add(new FbParameterItem("@TransactionId", TransactionId));
			Params.Add(new FbParameterItem("@AccountId", AccountId));
			Params.Add(new FbParameterItem("CategoryOnlyId", CategoryOnlyId));
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
					//Entries.AppendLine((Row[0] == DBNull.Value) ? "" : Convert.ToString(Row[0]));
					Entries.AppendLine((Obj == DBNull.Value) ? "" : Convert.ToString(Obj));
				}
			}
			
			return Result;
		}
		
		public static string BuildOutput_PortfolioStmtKitReports(string ConnectionString,
		                                                         Int64 CompanyId,
		                                                         int AccountId,
		                                                         int CategoryOnlyId,
		                                                         int NewId,
		                                                         ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "BuildOutput_PortfolioStmtKitReports";
			string QueryText = 
				" SELECT OUTPUTLINE" +
				" FROM EAStatementKitGroup(@CompanyId, @AccountId, @CategoryOnlyId, @ESKPID)";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("@CompanyId", CompanyId));
			Params.Add(new FbParameterItem("@AccountId", AccountId));
			Params.Add(new FbParameterItem("CategoryOnlyId", CategoryOnlyId));
			Params.Add(new FbParameterItem("ESKPID", NewId));
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
					//Entries.AppendLine((Row[0] == DBNull.Value) ? "" : Convert.ToString(Row[0]));
					Entries.AppendLine((Obj == DBNull.Value) ? "" : Convert.ToString(Obj));
				}
			}
			
			return Result;
		}
		
		public static string LoadStmtKitListRequestItem(ActionBase ActionBase,
		                                                ref StmtKitListRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadMgmtPdfReportListRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				Object Obj = ActionBase.GetCollectionValue("Data", "AccountId");
				TempItem.AccountIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "CategoryOnlyId");
				TempItem.CategoryOnlyIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "NewId");
				TempItem.NewIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
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
		
		public static string EAStatementKitViewPrepare(string ConnectionString, 
		                                               Int64 CompanyId,
		                                               int AccountId,
		                                               int AddedById,
		                                               int NewId)
		{
			string Result = "";
			string Method = "EAStatementKitViewPrepare";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			try 
			{
				string QueryText = 
					" EXECUTE PROCEDURE EAStatementKitViewPrepare(@CompanyId, @AccountId, @AddedById, @ESKPID)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("@CompanyId", CompanyId));
				Params.Add(new FbParameterItem("@AccountId", AccountId));
				Params.Add(new FbParameterItem("AddedById", AddedById));
				Params.Add(new FbParameterItem("ESKPID", NewId));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
				}
	
				if (!string.IsNullOrEmpty(Result))
				{
					Result =
						JsonUtilities.AddPropertyNameAndValue("Result", Result) +
						JsonUtilities.AddPropertyNameAndValue("Method", Method, false);
				}
			}
			catch (Exception Ex)
			{
				Result =
					Method + Environment.NewLine +
					Ex.Message + Environment.NewLine +
					Ex.StackTrace;
			}
			finally
			{
				WindowsEventLoggingUtilities.WriteToInfoEvent("Leaving " + Method, Method, 0);
			}
			
			return Result;
		}
		
		public static string BuildOutput_PortfolioResBondsList(string ConnectionString,
		                                                       Int64 CompanyId,
		                                                       Int64 ClientId,
		                                                       int PortfolioId,
		                                                       //int CategoryOnlyId,
		                                                       ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "BuildOutput_PortfolioResBondsList";
			string QueryText = 
				" SELECT OUTPUTLINE" +
				" FROM EABondResidentialDisplayGroup(@CompanyId, @PortfolioId, @RequestedById)";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("CompanyId", CompanyId));
			Params.Add(new FbParameterItem("PortfolioId", PortfolioId));
			Params.Add(new FbParameterItem("RequestedById", ClientId));
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
					//Entries.AppendLine((Row[0] == DBNull.Value) ? "" : Convert.ToString(Row[0]));
					Entries.AppendLine((Obj == DBNull.Value) ? "" : Convert.ToString(Obj));
				}
			}
			
			return Result;
		}
		
		public static string LoadResBondsListRequestItem(ActionBase ActionBase,
		                                                 ref ResBondsListRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadResBondsListRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				Object Obj = ActionBase.GetCollectionValue("Data", "PortfolioId");
				TempItem.PortfolioIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "CategoryOnlyId");
				TempItem.CategoryOnlyIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
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
		
		public static string EAStatementKitViewPrepare(string ConnectionString,
		                                               Int64 CompanyId,
		                                               Int64 ClientId,
		                                               int AccountId,
		                                               int NewId)
		{
			string Result = "";
			string Method = "EAStatementKitViewPrepare";
			string QueryText = "EXECUTE PROCEDURE EAStatementKitViewPrepare(@CompanyId, @Accountid, @addedbyid,@GenId)";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			try 
			{
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				Params.Add(new FbParameterItem("AccountId", AccountId));
				Params.Add(new FbParameterItem("addedbyid", ClientId));
				Params.Add(new FbParameterItem("GenId", NewId));
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
		
		public static string LoadCommBondsListRequestItem(ActionBase ActionBase,
		                                                  ref CommBondsListRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadResBondsListRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				Object Obj = ActionBase.GetCollectionValue("Data", "PortfolioId");
				TempItem.PortfolioIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = ActionBase.GetCollectionValue("Data", "SubAccountId");
				TempItem.SubAccountIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
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
		
		public static string BuildOutput_PortfolioCommBondsList(string ConnectionString,
		                                                        Int64 CompanyId,
		                                                        Int64 ClientId,
		                                                        int PortfolioId,
		                                                        int SubAccountId,
		                                                        ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "BuildOutput_PortfolioCommBondsList";
			string QueryText = 
				" SELECT OUTPUTLINE" +
				" FROM EABondCommercialDisplayGroup(@CompanyId, @PortfolioId, @SubAccountId, @RequestedById)";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("CompanyId", CompanyId));
			Params.Add(new FbParameterItem("PortfolioId", PortfolioId));
			Params.Add(new FbParameterItem("SubAccountId", SubAccountId));
			Params.Add(new FbParameterItem("RequestedById", ClientId));
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
					//Entries.AppendLine((Row[0] == DBNull.Value) ? "" : Convert.ToString(Row[0]));
					Entries.AppendLine((Obj == DBNull.Value) ? "" : Convert.ToString(Obj));
				}
			}
			
			return Result;
		}		
	}
}
