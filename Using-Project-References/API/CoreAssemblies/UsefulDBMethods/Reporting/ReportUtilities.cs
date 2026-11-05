/*
 * Created by SharpDevelop.
 * User: John
 * Date: 14/08/2019
 * Time: 10:02 PM
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
	/// Utilities relating to reports and report generation
	/// </summary>
	public class ReportUtilities
	{
		private const string FModuleName = "ReportUtilities";
		public string ModuleName { get { return FModuleName; } }
		public ReportUtilities()
		{
		}
		
		public DataTable EAReportGetDefault(string ConnectionString,
		                                 int CompanyClientId,
		                                 string ReportSource,
		                                 int RunId,
		                                 out string ErrorMsg)
		{
			ErrorMsg = "";
			DataTable Result = null;
			
			string QueryText = "SELECT ReportName, ReportId, DefaultValue FROM EAReportGetDefault(@CompanyClientId, @ReportSource, @RunId)";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			
			try 
			{
				Params.Add(new FbParameterItem("CompanyClientId", CompanyClientId));
				Params.Add(new FbParameterItem("ReportSource", ReportSource));
				Params.Add(new FbParameterItem("RunId", RunId));
				ErrorMsg = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, ModuleName, 0);
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Result = 
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out ErrorMsg);
				}
			}
			catch (Exception Ex)
			{
				ErrorMsg =
					Ex.Message + Environment.NewLine +
					Ex.InnerException + Environment.NewLine +
					Ex.StackTrace;
			}
			
			
			return Result;
		}
		
		public string EAReportGetDefaultJsonList(string ConnectionString,
		                                         int CompanyClientId,
		                                         string ReportSource,
		                                         int RunId,
		                                         string JsonArrayName,
		                                         out string ErrorMsg)
		{
			ErrorMsg = "";
			string Result = "";
			
			try 
			{
				DataTable tbl = EAReportGetDefault(ConnectionString, CompanyClientId, ReportSource, RunId, out ErrorMsg);
				
				List<EAReportGetDefaultItem> ReportItems = new List<EAReportGetDefaultItem>();
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					object Obj = null;
					foreach (DataRow Row in tbl.Rows)
					{
						EAReportGetDefaultItem Item = new EAReportGetDefaultItem();
						Obj = Row["ReportName"];
						Item.ReportName = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
						Obj = Row["ReportId"];
						Item.ReportId = (Obj == DBNull.Value) ? 0 : Convert.ToInt32(Obj);
						Obj = Row["DefaultValue"];
						Item.Default = (Obj == DBNull.Value) ? "0" : Convert.ToString(Obj);
						ReportItems.Add(Item);
					}
				}
				
				List<string> OutputItems = new List<string>();
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					foreach (EAReportGetDefaultItem Item in ReportItems)
					{
						OutputItems.Add(Item.AsJsonObject());
					}
					
					Result = 
						JsonUtilities.StartJsonArray(JsonArrayName) +
						string.Join(",", OutputItems.ToArray()) +
						JsonUtilities.EndJsonArray();
				}				
				
			}
			catch (Exception Ex)
			{
				ErrorMsg =
					Ex.Message + Environment.NewLine +
					Ex.InnerException + Environment.NewLine +
					Ex.StackTrace;
			}
			
			
			return Result;
		}
		
		public StringBuilder BuildReportFileList(string ConnectionString,
		                                         int ReportId,
		                                         Int64 CompanyClientId,
		                                         Int64 ClientId,
		                                         NameValueCollection CommonCollection,
		                                         NameValueCollection DataCollection,
		                                         string OutputArrayName,
		                                         out string ErrorMsg)
		{
			StringBuilder Result = new StringBuilder();
			
			int EventRunId = 0;
			string ReportSql = 
				BuildReportSql(ConnectionString, ReportId, CompanyClientId, ClientId, CommonCollection,  DataCollection, out EventRunId, out ErrorMsg);
			if (string.IsNullOrEmpty(ErrorMsg))
			{
				ExecuteEventRunCreate(ConnectionString, EventRunId, ReportId, ReportSql, CompanyClientId, ClientId, out ErrorMsg);
			}
			
			string AppLocation = "";
			string DocsLocation = "";
			if (string.IsNullOrEmpty(ErrorMsg))
			{
				GetCompanySettingsItems(ConnectionString, CompanyClientId, out AppLocation, out DocsLocation, out ErrorMsg);
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					if  (!AppLocation.EndsWith(@"\")) AppLocation += @"\";
					AppLocation += "{0}";
				}
			}
			
			string CommandLine = "";
			if (string.IsNullOrEmpty(ErrorMsg))
			{
				CommandLine = 
					UsefulDBMethods.EventHelpers.GetEventCommandLineForRunId(
						ConnectionString, EventRunId, out ErrorMsg);
			}
			
			if (string.IsNullOrEmpty(ErrorMsg))
			{
				BuildReportFileListEntries(ConnectionString, EventRunId, CommandLine, AppLocation, DocsLocation, OutputArrayName, ref Result, out ErrorMsg);
			}
			
			if (!string.IsNullOrEmpty(ErrorMsg))
			{
				Result.Length = 0;
			}
			
			return Result;
		}

        public StringBuilder BuildReportFileList_ExcludingBase64(string ConnectionString,
                                                                 int ReportId,
                                                                 Int64 CompanyClientId,
                                                                 Int64 ClientId,
                                                                 NameValueCollection CommonCollection,
                                                                 NameValueCollection DataCollection,
                                                                 string OutputArrayName,
                                                                 out string ErrorMsg)
        {
            StringBuilder Result = new StringBuilder();

            int EventRunId = 0;
            string ReportSql =
                BuildReportSql(ConnectionString, ReportId, CompanyClientId, ClientId, CommonCollection, DataCollection, out EventRunId, out ErrorMsg);
            if (string.IsNullOrEmpty(ErrorMsg))
            {
                ExecuteEventRunCreate(ConnectionString, EventRunId, ReportId, ReportSql, CompanyClientId, ClientId, out ErrorMsg);
            }

            string AppLocation = "";
            string DocsLocation = "";
            if (string.IsNullOrEmpty(ErrorMsg))
            {
                GetCompanySettingsItems(ConnectionString, CompanyClientId, out AppLocation, out DocsLocation, out ErrorMsg);

                if (string.IsNullOrEmpty(ErrorMsg))
                {
                    if (!AppLocation.EndsWith(@"\")) AppLocation += @"\";
                    AppLocation += "{0}";
                }
            }

            string CommandLine = "";
            if (string.IsNullOrEmpty(ErrorMsg))
            {
                CommandLine =
                    UsefulDBMethods.EventHelpers.GetEventCommandLineForRunId(
                        ConnectionString, EventRunId, out ErrorMsg);
            }

            if (string.IsNullOrEmpty(ErrorMsg))
            {
                BuildReportFileListEntries_ExcludingBase64(ConnectionString, EventRunId, CommandLine, AppLocation, DocsLocation, OutputArrayName, ref Result, out ErrorMsg);
            }

            if (!string.IsNullOrEmpty(ErrorMsg))
            {
                Result.Length = 0;
            }

            return Result;
        }

        private string BuildReportSql(string ConnectionString,
		                              int ReportId,
		                              Int64 CompanyClientId,
		                              Int64 ClientId,
		                              NameValueCollection CommonCollection,
		                              NameValueCollection DataCollection,
		                              out int EventRunId,
		                              out string ErrorMsg)
		{
			string Result = "";
			ErrorMsg = "";
			EventRunId = 0;
			
			List<GeneralReportParameters> ParameterList = BuildReportParameterList(ConnectionString, ReportId, out ErrorMsg);
			if (string.IsNullOrEmpty(ErrorMsg))
			{
				ProcessReportParameterList(ConnectionString, CompanyClientId, ClientId, CommonCollection, DataCollection, ParameterList, out EventRunId, out ErrorMsg);
			}
			if (string.IsNullOrEmpty(ErrorMsg))
			{
				Result = GetEventRunReportSqlStatement(ConnectionString, ReportId, EventRunId, out ErrorMsg);
			}
			
			return Result;
		}
		
		private List<GeneralReportParameters> BuildReportParameterList(string ConnectionString,
		                                                                      int ReportId,
		                                                                      out string ErrorMsg)
		{
			ErrorMsg = "";
			List<GeneralReportParameters> Result = new List<GeneralReportParameters>();
			
			string QueryText  = "SELECT * FROM EAXFILESWHATFIELDISNEEDED(@REPORTID)";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("ReportId", ReportId));
			ErrorMsg = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, ModuleName, 0);
			if (string.IsNullOrEmpty(ErrorMsg))
			{
				DataTable tbl = 
					UsefulDBMethods.Helpers.GetFirstDataTableEx(
						ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out ErrorMsg);
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					string IndexStr = "";
					string ColumnName = "";
					string ColumnValue = "";
					object Obj = DBNull.Value;
					
					if ((tbl.Rows != null) && (tbl.Rows.Count > 0))
					{
						for (int Index = 1; Index <= 15; Index++)
						{
							IndexStr = "";
							if (Index < 10) IndexStr = "0";
							IndexStr += Index.ToString();
							ColumnName = "Parameter" + IndexStr;
							DataRow FirstRow = tbl.Rows[0];
							Obj = FirstRow[ColumnName];
							ColumnValue = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
							if (!string.IsNullOrEmpty(ColumnValue))
							{
								GeneralReportParameters Item = new GeneralReportParameters();
								Item.Parameter = ColumnValue;
								Obj = FirstRow[ColumnName + "Source"];
								Item.Source = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
								Obj = FirstRow[ColumnName + "Field"];
								Item.Field = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
								Result.Add(Item);
							}
						}
					}
				}
			}
			
			return Result;
		}
		
		private void ProcessReportParameterList(string ConnectionString,
		                                        Int64 CompanyClientId,
		                                        Int64 ClientId,
		                                        NameValueCollection CommonCollection,
		                                        NameValueCollection DataCollection,
		                                        List<GeneralReportParameters> ReportParameterList,
		                                        out int EventRunId,
		                                        out string ErrorMsg)
		{
			string QueryText = "";
			ErrorMsg = "";

			EventRunId =
				UsefulDBMethods.GeneratorHelpers.GetNextGeneratorId(
					ConnectionString, "EVA_ID_GEN", (int)CompanyClientId, (int)ClientId, out ErrorMsg);
			List<FbParameterItem> Params = new List<FbParameterItem>();
			string Str = "";
			object Obj = DBNull.Value;
			if (string.IsNullOrEmpty(ErrorMsg))
			{
				QueryText = "INSERT INTO EAEventRunReportData(EVA_ID) VALUES (@EvaId)";
				Params.Clear();
				Params.Add(new FbParameterItem("EvaId", EventRunId));
				ErrorMsg = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, ModuleName, 0);
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
				}
			}
			
			if (string.IsNullOrEmpty(ErrorMsg))
			{
				string QueryBase = "UPDATE EAEventRunReportData SET {0} = @FieldValue WHERE EVA_ID = @EvaId";
				foreach (var Item in ReportParameterList)
				{
					if (string.IsNullOrEmpty(ErrorMsg))
					{
						Obj = GetCollectionValue(Item.Source, Item.Field, CommonCollection, DataCollection);
						if (Obj == null)
						{
							ErrorMsg = "Cannot find the value for: " + Item.Source + "." + Item.Field;
							WriteCollectionInfo(CommonCollection, DataCollection);
						}
						
						if (string.IsNullOrEmpty(ErrorMsg))
						{
							Str = (Obj == null) ? "GetCollectionValue == NULL" : Convert.ToString(Obj);
		
							QueryText = string.Format(QueryBase, Item.Parameter);
							
							Params.Clear();
							Params.Add(new FbParameterItem("FieldValue", Str));
							Params.Add(new FbParameterItem("EvaId", EventRunId));
							ErrorMsg = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, ModuleName, 0);
							if (string.IsNullOrEmpty(ErrorMsg))
							{
								ErrorMsg = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
							}
						}
					}					
				}
			}
		}
		
		private string GetEventRunReportSqlStatement(string ConnectionString, 
		                                             int ReportId,
		                                             int EventRunId,
		                                             out string ErrorMsg)
		{
			string SqlStatement = "";
			ErrorMsg = "";
			string QueryText = "";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			
			if (string.IsNullOrEmpty(ErrorMsg))
			{
				QueryText = "EXECUTE PROCEDURE EAXFILESREPORTSQL(@REPORTID, @EVENTRUNID)";
				Params.Clear();
				Params.Add(new FbParameterItem("ReportId", ReportId));
				Params.Add(new FbParameterItem("EventRunId", EventRunId));
				ErrorMsg = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, ModuleName, 0);					
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
				}
			}
			
			if (string.IsNullOrEmpty(ErrorMsg))
			{
				QueryText = "SELECT EVA_SQLSTATEMENT FROM EAEVENTRUNREPORTDATA WHERE EVA_ID = @EVENTRUNID";
				Params.Clear();
				Params.Add(new FbParameterItem("EventRunId", EventRunId));
				ErrorMsg = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, ModuleName, 0);
			}
			
			if (string.IsNullOrEmpty(ErrorMsg))
			{
				DataTable tbl = 
					UsefulDBMethods.Helpers.GetFirstDataTableEx(
						ConnectionString, QueryText, Params, 0,
						UsefulDBMethods.FirebirdQueryCheckTypes.HasTablesAndRows, out ErrorMsg);
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					object Obj = tbl.Rows[0]["EVA_SqlStatement"];
					SqlStatement = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					if (string.IsNullOrEmpty(SqlStatement)) ErrorMsg = "No Sql Statement returned for the Report Id: " + ReportId.ToString();
				}
			}
			
			return SqlStatement;
		}
		
		private void ExecuteEventRunCreate(string ConnectionString,
		                                   int EventRunId, 
		                                   int ReportId, 
		                                   string ReportSql,
		                                   Int64 CompanyClientId,
		                                   Int64 ClientId,
		                                   out string ErrorMsg)
		{
			ErrorMsg = "";
			string TodayStr = MiscUtilities.DateTimeToYYYYMMDD(DateTime.Now);
			int PdfType = 0;
			ErrorMsg = 
				UsefulDBMethods.EventHelpers.ExecuteEventRunCreate(
					ConnectionString, EventRunId, (int)CompanyClientId, 0, 0, 0, (int)ClientId, 0,
					ReportSql, "", "", 0, "", "", 0, "", "", "", "", "Payment", "Y", PdfType, ReportId, 0, 0, 0, TodayStr, TodayStr, "", 0);
		}
		
		public void GetCompanySettingsItems(string ConnectionString, 
		                                    Int64 CompanyClientId, 
		                                    out string WordLocation, 
		                                    out string DocsLocation, 
		                                    out string ErrorMsg)
		{
			ErrorMsg = "";
			WordLocation = "";
			DocsLocation = "";
			try 
			{
				string QueryText = "SELECT CO_EAWordLocation, CO_DocsLocation FROM EACompanySettings WHERE I_Settings_CompanyId = @CompanyClientId";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyClientId", CompanyClientId));
				ErrorMsg = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, ModuleName, 0);
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					DataTable tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, QueryText, Params, 0, 
							UsefulDBMethods.FirebirdQueryCheckTypes.HasTablesAndRows, out ErrorMsg);
					if (string.IsNullOrEmpty(ErrorMsg))
					{
						object Obj = tbl.Rows[0]["CO_EAWordLocation"];
						WordLocation = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
						Obj = tbl.Rows[0]["CO_DocsLocation"];
						DocsLocation = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					}
				}				
			}
			catch (Exception Ex)
			{
				ErrorMsg =
					Ex.Message + Environment.NewLine +
					Ex.InnerException + Environment.NewLine +
					Ex.StackTrace;
			}
		}
		
		public void BuildReportFileListEntries(string ConnectionString,
		                                       int EventRunId,
		                                       string CommandLine,
		                                       string AppLocation,
		                                       string DocsLocation,
		                                       string ArrayName,
		                                       ref StringBuilder Entries,
		                                       out string ErrorMsg)
		{
			ErrorMsg = "";
			string AppName = "";
			string ArgsBase = "";
			
			if (string.IsNullOrEmpty(ErrorMsg))
			{
				AppName = CommandLine.Substring(0, CommandLine.IndexOf(" /"));
				AppName = string.Format(AppLocation, AppName);
				ArgsBase = CommandLine.Substring(CommandLine.IndexOf("/"));
				WindowsEventLoggingUtilities.WriteToInfoEvent("AppName: " + AppName, ModuleName, 0);
				WindowsEventLoggingUtilities.WriteToInfoEvent("Args: " + ArgsBase, ModuleName, 0);
				
				Process runApp = new Process();
				runApp.StartInfo.FileName = AppName;
				runApp.StartInfo.Arguments = ArgsBase;
				runApp.Start();
				//runApp.WaitForExit(60000);
				runApp.WaitForExit(300000);
				
				WindowsEventLoggingUtilities.WriteToInfoEvent("After calling: " + AppName, ModuleName, 0);
				
				Entries.Append(JsonUtilities.StartJsonArray(ArrayName));
				EAEventRunBase64Entries(ConnectionString, EventRunId, DocsLocation, ref Entries, out ErrorMsg);
		        Entries.Append(CommonUtilities.JsonUtilities.EndJsonArray());
			}
		}

        public void BuildReportFileListEntries_ExcludingBase64(string ConnectionString,
                                                               int EventRunId,
                                                               string CommandLine,
                                                               string AppLocation,
                                                               string DocsLocation,
                                                               string ArrayName,
                                                               ref StringBuilder Entries,
                                                               out string ErrorMsg)
        {
            ErrorMsg = "";
            string AppName = "";
            string ArgsBase = "";

            if (string.IsNullOrEmpty(ErrorMsg))
            {
                AppName = CommandLine.Substring(0, CommandLine.IndexOf(" /"));
                AppName = string.Format(AppLocation, AppName);
                ArgsBase = CommandLine.Substring(CommandLine.IndexOf("/"));
                WindowsEventLoggingUtilities.WriteToInfoEvent("AppName: " + AppName, ModuleName, 0);
                WindowsEventLoggingUtilities.WriteToInfoEvent("Args: " + ArgsBase, ModuleName, 0);

                Process runApp = new Process();
                runApp.StartInfo.FileName = AppName;
                runApp.StartInfo.Arguments = ArgsBase;
                runApp.Start();
                //runApp.WaitForExit(60000);
                runApp.WaitForExit(300000);

                WindowsEventLoggingUtilities.WriteToInfoEvent("After calling: " + AppName, ModuleName, 0);

                Entries.Append(JsonUtilities.StartJsonArray(ArrayName));
                EAEventRunEntries(ConnectionString, EventRunId, DocsLocation, ref Entries, out ErrorMsg);
                Entries.Append(CommonUtilities.JsonUtilities.EndJsonArray());
            }
        }

        public void EAEventRunBase64Entries(string ConnectionString,
		                                    int EventRunId,
		                                    string DocsLocation,
		                                    ref StringBuilder Entries,
		                                    out string ErrorMsg)
		{
			ErrorMsg = "";			
			try 
			{
				string QueryText =
					" SELECT EC_ID, EC_Base64, EC_EventRunId, EC_FileExtension, EC_DisplayName" +
					" FROM EAEventRunBase64 WHERE EC_EventRunId = @EventRunId ORDER BY EC_ID";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("EventRunId", EventRunId));
				ErrorMsg = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, ModuleName, 0);
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					DataTable tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out ErrorMsg);
					if (string.IsNullOrEmpty(ErrorMsg))
					{
						object Obj = null;			
						string FileExt = "";
						string ECId = "";
						string RunId = "";
						string DisplayName= "";
						string FileNameOnly = "";
						string OutputLocation = "";
						string OutputFileName = "";
						string Base64String = "";
						string Str = "";
		        
		        		List<string> RowList = new List<string>();
		        		foreach(DataRow ARow in tbl.Rows)
		        		{
				        	Obj = ARow["EC_DisplayName"];
				        	DisplayName = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
				        	Obj = ARow["EC_FileExtension"];
				        	FileExt = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
				        	Obj = ARow["EC_ID"];
				        	ECId = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
				        	Obj = ARow["EC_EventRunId"];
				        	RunId = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
				        	FileNameOnly = "{0}-{1}{2}";
				        	FileNameOnly = string.Format(FileNameOnly, RunId, ECId, FileExt);
				        	OutputLocation = @"\Docs\" + FileNameOnly;
				        	OutputFileName = DocsLocation + FileNameOnly;
				        	WindowsEventLoggingUtilities.WriteToInfoEvent("OutputFileName" + Environment.NewLine + OutputFileName, ModuleName, 0);
				        	Obj = ARow["EC_Base64"];
				        	byte[] toBytes = (byte[])Obj;
				        	Base64String = System.Text.Encoding.Default.GetString(toBytes);
				        	File.WriteAllBytes(OutputFileName, Convert.FromBase64String(Base64String));	        	
				        	Str =
				        		JsonUtilities.StartJsonObject("") +
				        		JsonUtilities.AddPropertyNameAndValue("displayName", DisplayName) +
				        		JsonUtilities.AddPropertyNameAndValue("file", OutputFileName) +
				        		JsonUtilities.AddPropertyNameAndValue("href", OutputLocation, false) +
				        		JsonUtilities.EndJsonObject();
				        	RowList.Add(Str);
				        }
		        		Entries.Append(string.Join(",", RowList.ToArray()));
					}
				}
			}
			catch (Exception Ex)
			{
				ErrorMsg =
					Ex.Message + Environment.NewLine +
					Ex.InnerException + Environment.NewLine +
					Ex.StackTrace;
			}
		}

        public void EAEventRunEntries(string ConnectionString,
                                      int EventRunId,
                                      string DocsLocation,
                                      ref StringBuilder Entries,
                                      out string ErrorMsg)
        {
            ErrorMsg = "";
            try
            {
                string QueryText =
                    " SELECT EC_ID, EC_EventRunId, EC_FileExtension, EC_DisplayName" +
                    " FROM EAEventRunBase64 WHERE EC_EventRunId = @EventRunId ORDER BY EC_ID";
                List<FbParameterItem> Params = new List<FbParameterItem>();
                Params.Add(new FbParameterItem("EventRunId", EventRunId));
                ErrorMsg = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, ModuleName, 0);

                if (string.IsNullOrEmpty(ErrorMsg))
                {
                    DataTable tbl =
                        UsefulDBMethods.Helpers.GetFirstDataTableEx(
                            ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out ErrorMsg);
                    if (string.IsNullOrEmpty(ErrorMsg))
                    {
                        object Obj = null;
                        string FileExt = "";
                        string ECId = "";
                        string RunId = "";
                        string DisplayName = "";
                        string FileNameOnly = "";
                        string OutputLocation = "";
                        string OutputFileName = "";
                        string Str = "";

                        List<string> RowList = new List<string>();
                        foreach (DataRow ARow in tbl.Rows)
                        {
                            Obj = ARow["EC_DisplayName"];
                            DisplayName = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                            Obj = ARow["EC_FileExtension"];
                            FileExt = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                            Obj = ARow["EC_ID"];
                            ECId = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                            Obj = ARow["EC_EventRunId"];
                            RunId = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
                            FileNameOnly = "{0}-{1}{2}";
                            FileNameOnly = string.Format(FileNameOnly, RunId, ECId, FileExt);
                            OutputLocation = @"\Docs\" + FileNameOnly;
                            OutputFileName = DocsLocation + FileNameOnly;
                            WindowsEventLoggingUtilities.WriteToInfoEvent("OutputFileName" + Environment.NewLine + OutputFileName, ModuleName, 0);
                            /*
                                Obj = ARow["EC_Base64"];
                                byte[] toBytes = (byte[])Obj;
                                Base64String = System.Text.Encoding.Default.GetString(toBytes);
                                File.WriteAllBytes(OutputFileName, Convert.FromBase64String(Base64String));
                            */
                            Str =
                                JsonUtilities.StartJsonObject("") +
                                JsonUtilities.AddPropertyNameAndValue("displayName", DisplayName) +
                                JsonUtilities.AddPropertyNameAndValue("file", OutputFileName) +
                                JsonUtilities.AddPropertyNameAndValue("href", OutputLocation, false) +
                                JsonUtilities.EndJsonObject();
                            RowList.Add(Str);
                        }
                        Entries.Append(string.Join(",", RowList.ToArray()));
                    }
                }
            }
            catch (Exception Ex)
            {
                ErrorMsg =
                    Ex.Message + Environment.NewLine +
                    Ex.InnerException + Environment.NewLine +
                    Ex.StackTrace;
            }
        }

        private object GetCollectionValue(string Source, string Key, NameValueCollection CommonCollection, NameValueCollection DataCollection)
		{
			NameValueCollection Coll;
			if (StringUtilities.SameText(Source, "Common")) Coll = CommonCollection; else Coll = DataCollection;
			return Coll[Key];
		}

		private void WriteCollectionInfo(NameValueCollection CommonCollection, NameValueCollection DataCollection)
		{
			string Method = "WriteCollectionInfo";
			List<string> StrList = new List<string>();
			StrList.Add("CommonCollection.Count: " + CommonCollection.Count.ToString());
			if (CommonCollection.Count > 0) 
			{
				foreach (string Key in CommonCollection.Keys) 
				{
					string Value = Key + ":" + CommonCollection.Get(Key);
					StrList.Add(Value);
				}
			}
			
			StrList.Add("DataCollection.Count: " + DataCollection.Count.ToString());
			if (DataCollection.Count > 0) 
			{
				foreach (string Key in DataCollection.Keys) 
				{
					string Value = Key + ":" + DataCollection.Get(Key);
					StrList.Add(Value);
				}
			}
			WindowsEventLoggingUtilities.WriteToInfoEvent(string.Join(Environment.NewLine, StrList.ToArray()), Method, 0);
		}
	}
}
