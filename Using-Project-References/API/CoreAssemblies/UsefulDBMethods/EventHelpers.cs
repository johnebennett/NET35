/*
 * Created by SharpDevelop.
 * User: John
 * Date: 22/01/2019
 * Time: 3:32 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

using System.Data;

using CommonUtilities;

using Newtonsoft.Json.Linq;

namespace UsefulDBMethods
{
	/// <summary>
	/// Helper methods for Event table and stored
	/// procedure objects 
	/// </summary>
	public class EventHelpers
	{
		public EventHelpers()			
		{
			ModuleName = "UsefulDBMethods";
		}
		
		public string ModuleName { get; set; }
		
		public static int GetNextFilterEventGeneratorId(string ConnectionString,
  		                                                int CompanyId,
  		                                                int ClientId,
  		                                                out string ErrorMsg)
		{
			ErrorMsg = "";
			EventHelpers inst = new EventHelpers();
			
			int Result = GeneratorHelpers.GetNextGeneratorId(ConnectionString, "EA_FILTEREVENTID_GEN", CompanyId, ClientId, out ErrorMsg);
			if (!string.IsNullOrEmpty(ErrorMsg))
			{
				WindowsEventLoggingUtilities.WriteToErrorEvent("Error getting next generator: " + ErrorMsg, inst.ModuleName, 0);
				return 0;
			}
			
			return Result;
		}

		public static int SetupFilterEventRecord(string ConnectionString,
		                                         int CompanyId,
		                                         int ClientId,
		                                         int FilterId,
		                                         out string ErrorMsg)
		{
			ErrorMsg = "";
			int Result = GetNextFilterEventGeneratorId(ConnectionString, CompanyId, ClientId, out ErrorMsg);
			if (!string.IsNullOrEmpty(ErrorMsg)) return 0;
			
			SetupFilterEventRecord(ConnectionString, Result, CompanyId, ClientId, FilterId, out ErrorMsg);
			
			return Result;
		}

		public static void SetupFilterEventRecord(string ConnectionString,
		                                          int FilterEventId,
		                                          int CompanyId,
		                                          int ClientId,
		                                          int FilterId,
		                                          out string ErrorMsg)
		{
			ErrorMsg = "";
			EventHelpers inst = new EventHelpers();
			
			/*
			Result = GeneratorHelpers.GetNextGeneratorId(ConnectionString, "EA_FILTEREVENTID_GEN", CompanyId, ClientId, out ErrorMsg);
			if (!string.IsNullOrEmpty(ErrorMsg))
			{
				WindowsEventLoggingUtilities.WriteToErrorEvent("Error getting next generator: " + ErrorMsg, inst.ModuleName, 0);
				return 0;
			}
			*/
			
			// do the insert into the FilterEvent table
			/*
			string QueryText =
				"INSERT INTO EAFilterEvent (EA_FilterEventId, EA_CompanyClientId, EA_RequestedByClientId, ea_FilterListId, ea_RequestedByDate) " +
				"VALUES({0}, {1}, {2}, {3}, '{4}')";
			*/
			string QueryText =
				"INSERT INTO EAFilterEvent (EA_FilterEventId, EA_CompanyClientId, EA_RequestedByClientId, ea_FilterListId, ea_RequestedByDate) " +
				"VALUES(@FilterEventId, @CompanyClientId, @RequestedById, @FilterListId, @RequestedDate)";
			
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("@FilterEventId", FilterEventId));
			Params.Add(new FbParameterItem("@CompanyClientId", CompanyId));
			Params.Add(new FbParameterItem("@RequestedById", ClientId));
			Params.Add(new FbParameterItem("@FilterListId", FilterId));
			//Params.Add(new FbParameterItem("@RequestedDate", FirebirdUtilities.DateTimeToDbDateString(DateTime.Now)));			
			Params.Add(new FbParameterItem("@RequestedDate", DateTime.Now));			
			
			WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText" + Environment.NewLine + QueryText, inst.ModuleName, 0);
			/*
			QueryText =
				string.Format(
					QueryText, Result, CompanyId, ClientId, FilterId, FirebirdUtilities.DateTimeToDbDateString(DateTime.Now));
			ErrorMsg = FirebirdUtilities.DBExecute(ConnectionString, QueryText);
			*/
			ErrorMsg = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
			if (!string.IsNullOrEmpty(ErrorMsg))
			{
				WindowsEventLoggingUtilities.WriteToErrorEvent("Error executing query: " + ErrorMsg, inst.ModuleName, 0);
				return;
			}
		}
		
		/*
		private static DateTime YYYYMMDDToDateTime(string Source)
		{
			if (Source.Length != 8) return DateTime.MinValue;
			
			int DayInt = StringUtilities.StrToIntDef(Source.Substring(6, 2), 0);
			int MonthInt = StringUtilities.StrToIntDef(Source.Substring(4, 2), 0);
			int YearInt = StringUtilities.StrToIntDef(Source.Substring(0, 4), 0);
			return new DateTime(YearInt, MonthInt, DayInt);
		}
		*/
		
		public static string UpdateFilterEventRecordItems(int FilterEventId,
		                                                  string AskLater,
		                                                  string AskFor,
		                                                  string FromValue,
		                                                  string FromDate,
		                                                  string ToValue,
		                                                  string ToDate,
		                                                  string ConnectionString)
		{
			string Result = "";
			string QueryText = "";
			string MethodName = "EventHelpers.UpdateFilterEventRecordItems";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			DateTime FromDateTime = DateTime.MinValue;
			DateTime ToDateTime = DateTime.MinValue;

			List<string> SetList = new List<string>();
			
			//EventHelpers inst = new EventHelpers();
			
			//Result = GeneratorHelpers.GetNextGeneratorId(ConnectionString, "EA_FILTEREVENTID_GEN", CompanyId, ClientId, out ErrorMsg);
			if (!StringUtilities.SameText(AskLater, "1")) return Result;
			
			QueryText = 
				" UPDATE EAFilterEvent" +
				"    SET ";

				switch (AskFor)
			{
				case "DATERANGE":
						QueryText = QueryText + "ea_FilterDate1 = @FromDate,";
						QueryText = QueryText + "ea_FilterDate2 = @ToDate";
						Params.Add(new FbParameterItem("@FromDate", MiscUtilities.YYYYMMDDToDateTime(FromDate)));
						Params.Add(new FbParameterItem("@ToDate", MiscUtilities.YYYYMMDDToDateTime(ToDate)));
						break;
				case "NUMBERRANGE":
						QueryText = QueryText + "ea_FilterInt1 = @FromValue,"; // + FromValue);
						QueryText = QueryText + "ea_FilterInt2 = @ToValue";
						Params.Add(new FbParameterItem("@FromValue", FromValue));
						Params.Add(new FbParameterItem("@ToValue", ToValue));
						break;
				case "STRINGRANGE":
				case "BOOLRANGE":
						QueryText = QueryText + "ea_String1 = @FromValue";
						Params.Add(new FbParameterItem("@FromValue", FromValue));
						break;
			}
			
			QueryText = QueryText + " WHERE ea_FilterEventId = @FilterEventId";
			Params.Add(new FbParameterItem("@FilterEventId", FilterEventId));
			//QueryText = string.Format(QueryText, string.Join(",", SetList.ToArray()), FilterEventId);
			WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText" + Environment.NewLine + QueryText, MethodName, 0);
			//Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText);			
			Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
			if (!string.IsNullOrEmpty(Result))
			{
				WindowsEventLoggingUtilities.WriteToErrorEvent("Error executing query: " + Result, MethodName, 0);
			}
			
			return Result;
		}
		
		public static int AddReportEvent(string ConnectionString,
		                                 int CompanyClientId,
		                                 int OfficeId,
		                                 int ManagerId,
		                                 int TransactionTypeId,
		                                 bool ForMyself,
		                                 int ForMyselfPdfOrXls,
		                                 int ReportId,
		                                 string SqlText,
		                                 string SourceScreen,
		                                 int AddedById,
		                                 out string ErrorMsg)
		{
			ErrorMsg = "";
			EventHelpers inst = new EventHelpers();

			// Get the next Id from the generator
			int Result = GeneratorHelpers.GetNextGeneratorId(ConnectionString, "EVA_ID_GEN", CompanyClientId, AddedById, out ErrorMsg);
			if (!string.IsNullOrEmpty(ErrorMsg))
			{
				WindowsEventLoggingUtilities.WriteToErrorEvent("Error getting next generator: " + ErrorMsg, inst.ModuleName, 0);
				return 0;
			}
			WindowsEventLoggingUtilities.WriteToErrorEvent("Next generator: " + Result, inst.ModuleName, 0);
			
			// call the stored procedure to do all the necessary setup			
			string ForMyselfValue = (ForMyself == true) ? "Y" : "N";
			//string TodayStr = FirebirdUtilities.DateTimeToDbDateString(DateTime.Now);
			string TodayStr = MiscUtilities.DateTimeToYYYYMMDD(DateTime.Now);
			
			WindowsEventLoggingUtilities.WriteToErrorEvent("Before calling EventRunCreate", inst.ModuleName, 0);
			ErrorMsg =
				ExecuteEventRunCreate(
					ConnectionString,
					Result, 				// RunId,
					CompanyClientId, 		// CompanyClientId,
					OfficeId, 				// OfficeId,
					ManagerId, 				// ManagerId,
					TransactionTypeId,		// TransactionTypeId, // portfolioType
					AddedById, 				// AddedById,
					0, 						// TaskType - not required
					SqlText, 				// SqlText,
					"", "", 0, "", "", 0,	// WhoTo1, WhoTo2, EmailOption, DistinctClients, AddAsExcluded, EmailTemplateId, 
					"", "", "", "", 		// SubjectLine, CCTo, BCCTo, ExclusionTo,
					SourceScreen, 			// SourceSceen,
					ForMyselfValue,			// ForMyself,
					ForMyselfPdfOrXls, 		// ForMyselfPdfOrXls,
					ReportId,				// ReportId
					0,						// StatementRunId					
					0,						// LedgerId
					0,						// SubAccountLedgerId
					TodayStr,				// DateFrom
					TodayStr,				// DateTo
					"N",					// RandomQuestion1
					0						// TransactionId
					);
			if (!string.IsNullOrEmpty(ErrorMsg))
			{
				WindowsEventLoggingUtilities.WriteToErrorEvent("Error calling EventRunCreate: " + Environment.NewLine + ErrorMsg, inst.ModuleName, 0);
				return 0;
			}
					
			
			return Result;
		}
		
		public static string ExecuteEventRunCreate(string ConnectionString,
		                                           int RunId,
		                                           int CompanyClientId,
		                                           int OfficeId,
		                                           int ManagerId,
		                                           int TransactionTypeId, // portfolioType
		                                           int AddedById,
		                                           int TaskType,
		                                           string SqlText,
		                                           string WhoTo1,
		                                           string WhoTo2,
		                                           int EmailOption,
		                                           string DistinctClients,
		                                           string AddAsExcluded,
		                                           int EmailTemplateId,
		                                           string SubjectLine,
		                                           string CCTo,
		                                           string BCCTo,
		                                           string ExclusionTo,
		                                           string SourceSceen,
		                                           string ForMyself,
		                                           int ForMyselfPdfOrXls,
		                                           int ReportId,
		                                           int StatementRunId,
		                                           int LedgerId,
		                                           int SubAccountLedgerId,
		                                           string DateFrom,
		                                           string DateTo,
		                                           string RandomQuestion1,
		                                           int TransactionId)
		{
			string Result = "";
			
			EventHelpers inst = new EventHelpers();
			List<FbParameterItem> Params = new List<FbParameterItem>();
			string QueryText =
				"EXECUTE PROCEDURE EventRunCreate(" +
				" @RunId," + // RunId
				" @CompanyClientId," + // CompanyClientId
				" @BranchId," + // OfficeId
				" @ManagerId," + // ManagerId
				" @TransactionTypeId," + // TransactionTypeId // portfolioType
				" @AddedById," + // AddedById
				" @TaskType," + // TaskType
				" @Sql1," + // SqlText
				" @WhoTo1," + // WhoTo1
				" @WhoTo2," + // WhoTo2
				" @EmailOption," + // EmailOption
				" @DistinctClients," + // DistinctClients
				" @AddAsExcluded," + // AddAsExcluded
				" @EmailTemplateId," + // EmailTemplateId
				" @SubjectLine," + // SubjectLine
				" @CCTo," + // CCTo
				" @BCCTo," + // BCCTo
				" @ExclusionTo," + // ExclusionTo
				" @SourceScreen," + // SourceSceen
				" @ForMyself," + // ForMyself
				" @ForMyselfPdfOrXls," + // ForMyselfPdfOrXls
				" @ReportId," + // ReportId
				" @StatementRunId," + // StatementRunId
				" @LedgerId," + // LedgerId
				" @SubAccountLedgerId," + // SubAccountLedgerId
				" @DateFrom," + // DateFrom
				" @DateTo," + // DateTo
				" @RandomQuestion1," + // RandomQuestion1
				" @TransactionId" + // TrasactionId
				")";
			
			Params.Add(new FbParameterItem("@RunId", RunId));
			Params.Add(new FbParameterItem("@CompanyClientId", CompanyClientId));
			Params.Add(new FbParameterItem("@BranchId", OfficeId));
			Params.Add(new FbParameterItem("@ManagerId", ManagerId));
			Params.Add(new FbParameterItem("@TransactionTypeId", TransactionId));
			Params.Add(new FbParameterItem("@AddedById", AddedById));
			Params.Add(new FbParameterItem("@TaskType", TaskType));
			Params.Add(new FbParameterItem("@Sql1", SqlText));
			Params.Add(new FbParameterItem("@WhoTo1", WhoTo1));
			Params.Add(new FbParameterItem("@WhoTo2", WhoTo2));
			Params.Add(new FbParameterItem("@EmailOption", EmailOption));
			Params.Add(new FbParameterItem("@DistinctClients", DistinctClients));
			Params.Add(new FbParameterItem("@AddAsExcluded", AddAsExcluded));
			Params.Add(new FbParameterItem("@EmailTemplateId", EmailTemplateId));
			Params.Add(new FbParameterItem("@SubjectLine", SubjectLine));
			Params.Add(new FbParameterItem("@CCTo", CCTo));
			Params.Add(new FbParameterItem("@BCCTo", BCCTo));
			Params.Add(new FbParameterItem("@ExclusionTo", ExclusionTo));
			Params.Add(new FbParameterItem("@SourceScreen", SourceSceen));
			Params.Add(new FbParameterItem("@ForMyself", ForMyself));
			Params.Add(new FbParameterItem("@ForMyselfPdfOrXls", ForMyselfPdfOrXls));
			Params.Add(new FbParameterItem("@ReportId", ReportId));
			Params.Add(new FbParameterItem("@StatementRunId", StatementRunId));
			Params.Add(new FbParameterItem("@LedgerId", LedgerId));
			Params.Add(new FbParameterItem("@SubAccountLedgerId", SubAccountLedgerId));
			Params.Add(new FbParameterItem("@DateFrom", MiscUtilities.YYYYMMDDToDateTime(DateFrom)));
			Params.Add(new FbParameterItem("@DateTo", MiscUtilities.YYYYMMDDToDateTime(DateTo)));
			Params.Add(new FbParameterItem("@RandomQuestion1", RandomQuestion1));
			Params.Add(new FbParameterItem("@TransactionId", TransactionId));
			
			WindowsEventLoggingUtilities.WriteToInfoEvent("Before calling DBExecute", "ExecuteEventRunCreate", 0);
			Result = CommonUtilities.FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
			WindowsEventLoggingUtilities.WriteToInfoEvent("After calling DBExecute", "ExecuteEventRunCreate", 0);
			
			/*
			using (FbConnection conn = new FbConnection(ConnectionString))
			{
				conn.Open();
				using (FbTransaction trans = conn.BeginTransaction())
				{
					using (FbCommand cmd = new FbCommand())
					{
						cmd.Connection = conn;
						cmd.Transaction = trans;
						cmd.CommandText =
							"EXECUTE PROCEDURE EventRunCreate(" +
							" @RunId," + // RunId
							" @CompanyClientId," + // CompanyClientId
							" @BranchId," + // OfficeId
							" @ManagerId," + // ManagerId
							" @TransactionTypeId," + // TransactionTypeId // portfolioType
							" @AddedById," + // AddedById
							" @TaskType," + // TaskType
							" @Sql1," + // SqlText
							" @WhoTo1," + // WhoTo1
							" @WhoTo2," + // WhoTo2
							" @EmailOption," + // EmailOption
							" @DistinctClients," + // DistinctClients
							" @AddAsExcluded," + // AddAsExcluded
							" @EmailTemplateId," + // EmailTemplateId
							" @SubjectLine," + // SubjectLine
							" @CCTo," + // CCTo
							" @BCCTo," + // BCCTo
							" @ExclusionTo," + // ExclusionTo
							" @SourceScreen," + // SourceSceen
							" @ForMyself," + // ForMyself
							" @ForMyselfPdfOrXls," + // ForMyselfPdfOrXls
							" @ReportId," + // ReportId
							" @StatementRunId," + // StatementRunId
							" @LedgerId," + // LedgerId
							" @SubAccountLedgerId," + // SubAccountLedgerId
							" @DateFrom," + // DateFrom
							" @DateTo," + // DateTo
							" @RandomQuestion1," + // RandomQuestion1
							" @TransactionId" + // TrasactionId
							")";
						cmd.Parameters.AddWithValue("@RunId", RunId);
						cmd.Parameters.AddWithValue("@CompanyClientId", CompanyClientId);
						cmd.Parameters.AddWithValue("@BranchId", OfficeId);
						cmd.Parameters.AddWithValue("@ManagerId", ManagerId);
						cmd.Parameters.AddWithValue("@TransactionTypeId", TransactionId);
						cmd.Parameters.AddWithValue("@AddedById", AddedById);
						cmd.Parameters.AddWithValue("@TaskType", TaskType);
						cmd.Parameters.AddWithValue("@Sql1", SqlText);
						cmd.Parameters.AddWithValue("@WhoTo1", WhoTo1);
						cmd.Parameters.AddWithValue("@WhoTo2", WhoTo2);
						cmd.Parameters.AddWithValue("@EmailOption", EmailOption);
						cmd.Parameters.AddWithValue("@DistinctClients", DistinctClients);
						cmd.Parameters.AddWithValue("@AddAsExcluded", AddAsExcluded);
						cmd.Parameters.AddWithValue("@EmailTemplateId", EmailTemplateId);
						cmd.Parameters.AddWithValue("@SubjectLine", SubjectLine);
						cmd.Parameters.AddWithValue("@CCTo", CCTo);
						cmd.Parameters.AddWithValue("@BCCTo", BCCTo);
						cmd.Parameters.AddWithValue("@ExclusionTo", ExclusionTo);
						cmd.Parameters.AddWithValue("@SourceScreen", SourceSceen);
						cmd.Parameters.AddWithValue("@ForMyself", ForMyself);
						cmd.Parameters.AddWithValue("@ForMyselfPdfOrXls", ForMyselfPdfOrXls);
						cmd.Parameters.AddWithValue("@ReportId", ReportId);
						cmd.Parameters.AddWithValue("@StatementRunId", StatementRunId);
						cmd.Parameters.AddWithValue("@LedgerId", LedgerId);
						cmd.Parameters.AddWithValue("@SubAccountLedgerId", SubAccountLedgerId);
						cmd.Parameters.AddWithValue("@DateFrom", DateFrom);
						cmd.Parameters.AddWithValue("@DateTo", DateTo);
						cmd.Parameters.AddWithValue("@RandomQuestion1", RandomQuestion1);
						cmd.Parameters.AddWithValue("@TransactionId", TransactionId);
						
						try 
						{
							cmd.ExecuteNonQuery();							
							FirebirdUtilities.FirebirdUtilities.DBExecute(
						}
						catch (Exception Ex)
						{
							Result = Ex.Message + Environment.NewLine + Ex.StackTrace;
							WindowsEventLoggingUtilities.WriteToErrorEvent("Error executing query: " + Result, inst.ModuleName, 0);
						}
					}
				}
			}
			*/
			
			/*
			string QueryText =
				"EXECUTE PROCEDURE EventRunCreate(" +
				" {0}," + // RunId
				" {1}," + // CompanyClientId
				" {2}," + // OfficeId
				" {3}," + // ManagerId
				" {4}," + // TransactionTypeId // portfolioType
				" {5}," + // AddedById
				" {6}," + // TaskType
				" '{7}'," + // SqlText
				" '{8}'," + // WhoTo1
				" '{9}'," + // WhoTo2
				" {10}," + // EmailOption
				" '{11}'," + // DistinctClients
				" '{12}'," + // AddAsExcluded
				" {13}," + // EmailTemplateId
				" '{14}'," + // SubjectLine
				" '{15}'," + // CCTo
				" '{16}'," + // BCCTo
				" '{17}'," + // ExclusionTo
				" '{18}'," + // SourceSceen
				" '{19}'," + // ForMyself
				" {20}," + // ForMyselfPdfOrXls
				" {21}," + // ReportId
				" {22}," + // StatementRunId

				" {23}," + // LedgerId
				" {24}," + // SubAccountLedgerId
				" '{25}'," + // DateFrom
				" '{26}'," + // DateTo
				" '{27}'," + // RandomQuestion1
				" {28}" + // TrasactionId
				")";
			QueryText =
				string.Format(QueryText,
					RunId,
					CompanyClientId,
					OfficeId,
					ManagerId,
					TransactionTypeId, // portfolioType
					AddedById,
					TaskType,
					SqlText,
					WhoTo1,
					WhoTo2,
					EmailOption,
					DistinctClients,
					AddAsExcluded,
					EmailTemplateId,
					SubjectLine,
					CCTo,
					BCCTo,
					ExclusionTo,
					SourceSceen,
					ForMyself,
					ForMyselfPdfOrXls,
					ReportId,
				    StatementRunId,
				    LedgerId,
				    SubAccountLedgerId,
				    DateFrom,
				    DateTo,
				    RandomQuestion1,
				    TransactionId);
			WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText" + Environment.NewLine + QueryText, inst.ModuleName, 0);
			Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText);
			if (!string.IsNullOrEmpty(Result))
			{
				WindowsEventLoggingUtilities.WriteToErrorEvent("Error executing query: " + Result, inst.ModuleName, 0);
				return Result;
			}
			*/
			
			return Result;
		}
		
		public static string GetEventCommandLineForRunId(string ConnectionString,
		                                                 int RunId,
		                                                 out string ErrorMsg)
		{
			ErrorMsg = "";
			
			EventHelpers inst = new EventHelpers();
			
			string 
				QueryText = 
					" SELECT FIRST 1 EC_CommandLine" +
					" FROM EAEventCreateFile" +
					" WHERE EC_EventRunId = {0}";
			QueryText = string.Format(QueryText, RunId);
			WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText" + Environment.NewLine + QueryText, inst.ModuleName, 0);
			DataTable dt = FirebirdUtilities.GetFirstDataTable(ConnectionString, QueryText);
			if (dt == null) {
				ErrorMsg = 
					"Error Executing Query" + Environment.NewLine +
					"QueryText:" + Environment.NewLine + QueryText;
				WindowsEventLoggingUtilities.WriteToInfoEvent(ErrorMsg, inst.ModuleName, 0);
				//Result = LogOnErrorSelectingData(Method, ErrorMsg, QueryText);
				return "";
			}
			else if (dt.Rows.Count == 0) {
				ErrorMsg = "No rows in the table to process";
				//Result = LogOnErrorSelectingData(Method, ErrorMsg, QueryText);
				return "";
			}
			
			object Obj = dt.Rows[0]["EC_CommandLine"];
			string Result = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
			return Result;
		}
		
		public static List<string> GetEventCommandLineForEventRunClientId(string ConnectionString,
		                                                                   int EventRunClientId,
		                                                                   out string ErrorMsg)
		{
			ErrorMsg = "";
			
			EventHelpers inst = new EventHelpers();
			List<string> Result = new List<string>();
			
			string 
				QueryText = 
					" SELECT FIRST 1 EC_CommandLine" +
					" FROM EAEventCreateFile" +
					" WHERE EC_EventRunClientId = @EventRunClientId";
			//QueryText = string.Format(QueryText, RunId);
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("EventRunClientId", EventRunClientId));
			string Msg = 
				"QueryText:" + Environment.NewLine + QueryText + Environment.NewLine +
				"For EventRunClientId: " + EventRunClientId.ToString();
			WindowsEventLoggingUtilities.WriteToInfoEvent(Msg, inst.ModuleName, 0);
			DataTable dt = FirebirdUtilities.GetFirstDataTable(ConnectionString, QueryText, Params, 0);
			if (dt == null) {
				ErrorMsg = 
					"Error Executing Query" + Environment.NewLine +
					"QueryText:" + Environment.NewLine + QueryText;
				WindowsEventLoggingUtilities.WriteToInfoEvent(ErrorMsg, inst.ModuleName, 0);
				//Result = LogOnErrorSelectingData(Method, ErrorMsg, QueryText);
				return Result;
			}
			else if (dt.Rows.Count == 0) {
				ErrorMsg = "No rows in the table to process";
				//Result = LogOnErrorSelectingData(Method, ErrorMsg, QueryText);
				return Result;
			}
			else if (dt.Rows.Count > 0)
			{
				object Obj = null;
				string Item = "";
				foreach (DataRow Row in dt.Rows)
				{
					Obj = dt.Rows[0]["EC_CommandLine"];
					Item = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					if (!string.IsNullOrEmpty(Item)) Result.Add(Item);
				}
			}
			
			return Result;
		}
		
		public static List<string> GetEventCommandLine(string ConnectionString,
		                                               int EventRunClientId,
		                                               bool TemplateEntriesOnly,
		                                               out string ErrorMsg)
		{
			ErrorMsg = "";
			
			EventHelpers inst = new EventHelpers();
			List<string> Result = new List<string>();
			
			string 
				QueryText = 
					" SELECT EC_CommandLine" +
					" FROM EAEventCreateFile" +
					" WHERE EC_EventRunClientId = @EventRunClientId";
			if (TemplateEntriesOnly)
			{
				QueryText += " AND EC_EmailTemmplateId > 0";
			}
			else
			{
				QueryText += " AND EC_EmailTemplateId = 0";
			}
			
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("EventRunClientId", EventRunClientId));
			//QueryText = string.Format(QueryText, RunId);
			string Msg = 
				"QueryText:" + Environment.NewLine + QueryText + Environment.NewLine +
				"For EventRunClientId: " + EventRunClientId.ToString();
			WindowsEventLoggingUtilities.WriteToInfoEvent(Msg, inst.ModuleName, 0);
			DataTable dt = FirebirdUtilities.GetFirstDataTable(ConnectionString, QueryText, Params, 0);
			if (dt == null) {
				ErrorMsg = 
					"Error Executing Query" + Environment.NewLine +
					"QueryText:" + Environment.NewLine + QueryText;
				WindowsEventLoggingUtilities.WriteToInfoEvent(ErrorMsg, inst.ModuleName, 0);
				//Result = LogOnErrorSelectingData(Method, ErrorMsg, QueryText);
				return Result;
			}
			else if (dt.Rows.Count == 0) {
				ErrorMsg = "No rows in the table to process";
				//Result = LogOnErrorSelectingData(Method, ErrorMsg, QueryText);
				return Result;
			}
			else if (dt.Rows.Count > 0)
			{
				object Obj = null;
				string Item = "";
				foreach (DataRow Row in dt.Rows)
				{
					Obj = dt.Rows[0]["EC_CommandLine"];
					Item = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					if (!string.IsNullOrEmpty(Item)) Result.Add(Item);
				}
			}
			
			return Result;
		}
		
		public static string GetEventFileLocationForRunId(string ConnectionString,
		                                                  int RunId,
		                                                  out string ErrorMsg)
		{
			ErrorMsg = "";
			
			EventHelpers inst = new EventHelpers();
			
			string 
				QueryText = 
					" SELECT FIRST 1 EC_FileLocation" +
					" FROM EAEventCreateFile" +
					" WHERE EC_EventRunId = {0}";
			QueryText = string.Format(QueryText, RunId);
			WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText" + Environment.NewLine + QueryText, inst.ModuleName, 0);
			DataTable dt = FirebirdUtilities.GetFirstDataTable(ConnectionString, QueryText);
			if (dt == null) {
				ErrorMsg = 
					"Error Executing Query" + Environment.NewLine +
					"QueryText:" + Environment.NewLine + QueryText;
				WindowsEventLoggingUtilities.WriteToInfoEvent(ErrorMsg, inst.ModuleName, 0);
				//Result = LogOnErrorSelectingData(Method, ErrorMsg, QueryText);
				return "";
			}
			else if (dt.Rows.Count == 0) {
				ErrorMsg = "No rows in the table to process";
				//Result = LogOnErrorSelectingData(Method, ErrorMsg, QueryText);
				return "";
			}
			
			object Obj = dt.Rows[0]["EC_FileLocation"];
			string Result = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
			return Result;
		}
		
		public static string CallFilterEventSqlCreate(int FilterEventId,
		                                              string ConnectionString)
		{
			string QueryText = "EXECUTE PROCEDURE FilterEventSqlCreate({0})";
			QueryText = string.Format(QueryText, FilterEventId);
			EventHelpers inst = new EventHelpers();
			WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText" + Environment.NewLine + QueryText, inst.ModuleName, 0);
			string Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText);			
			if (!string.IsNullOrEmpty(Result))
			{
				WindowsEventLoggingUtilities.WriteToErrorEvent("Error executing query: " + Result, inst.ModuleName, 0);
			}
			
			return Result;
		}
		
		public static int AddEmailEvent(string ConnectionString,
		                                int CompanyClientId,
		                                //int OfficeId,
		                                //int ManagerId,
		                                //int TransactionTypeId,
		                                //bool ForMyself,
		                                //int ForMyselfPdfOrXls,
		                                //int ReportId,
		                                int FilterEventId,
		                                string SourceScreen,
		                                int AddedById,
		                                out string ErrorMsg)
		{
			ErrorMsg = "";
			EventHelpers inst = new EventHelpers();

			string SqlText = "";
			if (FilterEventId >= 0)
			{
				SqlText = GetFilterEventSql(ConnectionString, FilterEventId, out ErrorMsg);
			}
			if (!string.IsNullOrEmpty(ErrorMsg)) return 0;
			
		    // Get the next Id from the generator
			int Result = GeneratorHelpers.GetNextGeneratorId(ConnectionString, "EVA_ID_GEN", CompanyClientId, AddedById, out ErrorMsg);
			if (!string.IsNullOrEmpty(ErrorMsg))
			{
				WindowsEventLoggingUtilities.WriteToErrorEvent("Error getting next generator: " + ErrorMsg, inst.ModuleName, 0);
				return 0;
			}
			WindowsEventLoggingUtilities.WriteToErrorEvent("Next generator: " + Result, inst.ModuleName, 0);
			
			// call the stored procedure to do all the necessary setup			
			//string ForMyselfValue = (ForMyself == true) ? "Y" : "N";
			//string TodayStr = FirebirdUtilities.DateTimeToDbDateString(DateTime.Now);
			string TodayStr = MiscUtilities.DateTimeToYYYYMMDD(DateTime.Now);
			
			WindowsEventLoggingUtilities.WriteToErrorEvent("Before calling EventRunCreate", inst.ModuleName, 0);
			ErrorMsg =
				ExecuteEventRunCreate(
					ConnectionString,
					Result, 				// RunId,
					CompanyClientId, 		// CompanyClientId,
					//OfficeId, 			// OfficeId,
					//ManagerId, 			// ManagerId,
					//TransactionTypeId,	// TransactionTypeId, // portfolioType
					0, 0, 0,				// OfficeId, ManagerId,portfolioType
					AddedById, 				// AddedById,
					0, 						// TaskType - not required
					SqlText, 				// SqlText,
					"", "", 0, "", "", 0,	// WhoTo1, WhoTo2, EmailOption, DistinctClients, AddAsExcluded, EmailTemplateId, 
					"", "", "", "", 		// SubjectLine, CCTo, BCCTo, ExclusionTo,
					SourceScreen, 			// SourceSceen,
					//ForMyselfValue,		// ForMyself,
					//ForMyselfPdfOrXls,	// ForMyselfPdfOrXls,
					"N", 0, 				// ForMyself, ForMyselfPdfOrXls
					//ReportId,				// ReportId
					0, 0, 0, 0,				// ReportId, StatementRunId, LedgerId, SubAccountLedgerId,
					TodayStr, TodayStr,		// DateFrom, DateTo
					"N", 0					// RandomQuestion1, TransactionId
					);
			if (!string.IsNullOrEmpty(ErrorMsg))
			{
				WindowsEventLoggingUtilities.WriteToErrorEvent("Error calling EventRunCreate: " + Environment.NewLine + ErrorMsg, inst.ModuleName, 0);
				return 0;
			}					
			
			return Result;
		}
		
		public static string GetFilterEventSql(string ConnectionString,
		                                       int FilterEventId,
		                                       out string ErrorMsg)
		{
			ErrorMsg = "";			
			string Result = "";
			EventHelpers inst = new EventHelpers();
			
			List<FbParameterItem> Params = new List<FbParameterItem>();
			
			string QueryText = "SELECT EA_FilterEventSql FROM EAFilterEvent WHERE EA_FilterEventId = @FilterEventId";
			Params.Add(new FbParameterItem("@FilterEventId", FilterEventId));
			WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText:" + Environment.NewLine + QueryText, inst.ModuleName, 0);
			
			DataTable dt = FirebirdUtilities.GetFirstDataTable(ConnectionString, QueryText, Params, 0);
			if (dt == null)
			{
				ErrorMsg = "No data table returned";
				WindowsEventLoggingUtilities.WriteToErrorEvent(ErrorMsg, inst.ModuleName, 0);
			}
			else if (dt.Rows.Count == 0)
			{
				ErrorMsg = "No record returned for the FilterEventId: " + Convert.ToString(FilterEventId);
				WindowsEventLoggingUtilities.WriteToErrorEvent(ErrorMsg, inst.ModuleName, 0);
			}
			else if (dt.Rows.Count > 0)
			{
				foreach (DataRow dataRow in dt.Rows) 
				{
					Result = (dataRow[0] == DBNull.Value) ? "" : Convert.ToString(dataRow[0]);
				}
				WindowsEventLoggingUtilities.WriteToInfoEvent("FilterEventSql:" + Environment.NewLine + Result, inst.ModuleName, 0);
			}
			
			return Result;
		}
		
		public static string CreateXEmailTableForRunId(string ConnectionString,
		                                               int EventRunId)
		{
			string Result = "";
			string Method = "CreateXEmailTableForRunId";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, "ContactOperations", 0);
			WindowsEventLoggingUtilities.WriteToInfoEvent("RunId " + EventRunId.ToString(), "ContactOperations", 0);
			
			//CommonMethods inst = new CommonMethods();
			//string ModuleName = inst.FModuleName;
			
			try 
			{
				string QueryText = "EXECUTE PROCEDURE CREATE_WORKING_XEMAIL_CLIENTS(@RunId)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("RunId", EventRunId));
				WindowsEventLoggingUtilities.WriteToInfoEvent("Before creating XEMAIL Table", "ContactOperations", 0);
				Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
				WindowsEventLoggingUtilities.WriteToInfoEvent("After creating XEMAIL Table", "ContactOperations", 0);
				WindowsEventLoggingUtilities.WriteToInfoEvent("Result: " + Result, "ContactOperations", 0);
				/*
				if (!string.IsNullOrEmpty(Result))
				{
					Result = new ActionBase().LogOnErrorSelectingData(Method, Result, QueryText);
				}
				*/
				
				if (string.IsNullOrEmpty(Result))
				{
					// check to see if the table exists
					// Table should be named XEMAIL_RunId_
					QueryText = "SELECT * FROM XEMAIL" + Convert.ToString(EventRunId);
					DataTable dt = FirebirdUtilities.GetFirstDataTable(ConnectionString, QueryText);
					if (dt == null) Result = "No table returned.";
				}
			}
			catch (Exception Ex)
			{
				Result =  
					Ex.Message + Environment.NewLine +
					Ex.InnerException + Environment.NewLine +
					Ex.StackTrace + Environment.NewLine +
					Ex.Source;
			}
			
			return Result;
		}
		
		public static Int64 GetCustomerClientIdForEventRunId(string ConnectionString,
		                                                     Int64 RunId,
		                                                     out string ErrorMsg)
		{
			Int64 Result = 0;
			ErrorMsg = "";
			EventHelpers inst = new EventHelpers();
			
			List<FbParameterItem> Params = new List<FbParameterItem>();
			
			string QueryText = "SELECT I_EVA_CompanyClientId FROM EAEventRun WHERE EVA_Id = @RunId";
			Params.Add(new FbParameterItem("@RunId", RunId));
			WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText:" + Environment.NewLine + QueryText, inst.ModuleName, 0);
			WindowsEventLoggingUtilities.WriteToInfoEvent("RunId:" + Convert.ToString(RunId), inst.ModuleName, 0);
			
			DataTable dt = FirebirdUtilities.GetFirstDataTable(ConnectionString, QueryText, Params, 0);
			if (dt == null)
			{
				ErrorMsg = "No data table returned";
				WindowsEventLoggingUtilities.WriteToErrorEvent(ErrorMsg, inst.ModuleName, 0);
			}
			else if (dt.Rows.Count == 0)
			{
				ErrorMsg = "No record returned for the RunId: " + Convert.ToString(RunId);
				WindowsEventLoggingUtilities.WriteToErrorEvent(ErrorMsg, inst.ModuleName, 0);
			}
			else if (dt.Rows.Count > 0)
			{
				foreach (DataRow dataRow in dt.Rows) 
				{
					Result = (dataRow[0] == DBNull.Value) ? 0 : Convert.ToInt64(dataRow[0]);
				}
				WindowsEventLoggingUtilities.WriteToInfoEvent("CompanyClientId:" + Environment.NewLine + Convert.ToString(Result), inst.ModuleName, 0);
			}
			
			return Result;
		}
		
		public static Int64 GetNextStatementRunId(string ConnectionString,
		                                          Int64 CompanyClientId,
		                                          Int64 ClientId,
		                                          out string ErrorMsg)
		{
			Int64 Result = 0;
			string Method = "GetNextStatementRunId";
			ErrorMsg = "";
			EventHelpers inst = new EventHelpers();

			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, inst.ModuleName, 0);
			
			try
			{
				int nextGeneratorId = 
					GeneratorHelpers.GetNextGeneratorId(
						ConnectionString, "I_ASTATE_ID_GEN", (int)CompanyClientId, (int)ClientId, out ErrorMsg);
				string QueryText = 
					" EXECUTE PROCEDURE EAGetNextStatementRunId(@CompanyClientId, @GeneratorId)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyClientId", CompanyClientId));
				Params.Add(new FbParameterItem("GeneratorId", nextGeneratorId));
				UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, inst.ModuleName, 0);
				ErrorMsg = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					QueryText =
						"select FIRST 1 I_ASTR_CLOSEACCOUNTRUNID AS RunId from EASTATEMENTRUN where I_ASTATE_COMPANYCLIENTID=@COMPANYCLIENTID AND I_ASTATE_ID=@ISTATEID";
					Params.Clear();
					Params.Add(new FbParameterItem("CompanyClientId", CompanyClientId));
					Params.Add(new FbParameterItem("IStateId", nextGeneratorId));
											
					DataTable dt =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, QueryText, Params, 0, FirebirdQueryCheckTypes.HasRows, out ErrorMsg);
					if (string.IsNullOrEmpty(ErrorMsg))
					{
						DataRow dataRow = dt.Rows[0];
						object Obj = dataRow["RunId"];
						if (Obj != DBNull.Value) Result = Convert.ToInt64(Obj);
						WindowsEventLoggingUtilities.WriteToInfoEvent("RunId: " + Convert.ToString(Result), inst.ModuleName, 0);
					}				
				}
			}
			catch (Exception Ex)
			{
				ErrorMsg = Ex.Message + Environment.NewLine + Ex.InnerException;
			}
			
			return Result;
		}

        public static string SetEndOfMonthInProgress(string ConnectionString,
                                                    Int64 CompanyClientId,
                                                    string NewValue)
        {
            string Result = "";
            string Method = "SetEndOfMonthInProgress";
            EventHelpers inst = new EventHelpers();

            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, inst.ModuleName, 0);

            try
            {
                string QueryText =
                    " UPDATE EACompany set B_CO_EndOfMonthInProgress = @Value" +
                    " WHERE EACompany.I_CO_CompanyClientId = @CompanyClientId";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                Params.Add(new FbParameterItem("CompanyClientId", CompanyClientId));
                Params.Add(new FbParameterItem("Value", NewValue));
                UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, inst.ModuleName, 0);
                Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
            }
            catch (Exception Ex)
            {
                Result = Ex.Message + Environment.NewLine + Ex.InnerException;
            }

            return Result;
        }
	}
}