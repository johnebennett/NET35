/*
 * Created by SharpDevelop.
 * User: John
 * Date: 04/10/2019
 * Time: 12:38 PM
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

namespace Actions.Reconcilliation.GetReconcilliationReport
{
	/// <summary>
	/// Description of GetReconcilliationReport.
	/// </summary>
	public class GetReconcilliationReport : ActionBase
	{
		private string FModuleName = "Reconcilliation";
		private GetReconcilliationReportRequestItem FRequestItem;

		public static string Execute(int MasterId,
									 APISettingsItem SettingsItem,
									 string Request,
									 out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			GetReconcilliationReport inst = new GetReconcilliationReport();
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
				FRequestItem = new GetReconcilliationReportRequestItem();
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
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " +Method, FModuleName, MasterId);
			LogOnInsideMethod(Method);
			try 
			{
				int NewId = 
					UsefulDBMethods.GeneratorHelpers.GetNextGeneratorId(
						FbConnectionString, "I_SYS_ID_GEN", CommonItem.CompanyIdValue, CommonItem.ClientIdValue, out Result);
				string ReportSource = "";
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ReportSource = GetReportSource(out ErrorMsg);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg = 
						CommonMethods.InsertEASystemReportRecord(
							FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue, NewId, FRequestItem.ReportId, ReportSource);
				}

				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg =
						CommonMethods.PerformStep1(FbConnectionString, NewId, CommonItem.CompanyIdValue, CommonItem.ClientIdValue);
				}
				
				string FileCreatedSql = "";
				string SqlToUse = "";
				string OutputFileName = "";
				string OutputFileNameOnly = "";
				string Extension = "";				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					FileCreatedSql =
						CommonMethods.GetSys_FileCreated(FbConnectionString, NewId, out ErrorMsg);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					if (!string.IsNullOrEmpty(FileCreatedSql))
					{
						OutputFileName =
							CommonMethods.GetCreatedSystemReportFileName(
								FbConnectionString, FileCreatedSql, out ErrorMsg);
						if (string.IsNullOrEmpty(ErrorMsg))
						{
							if (!File.Exists(OutputFileName))
							{
								ErrorMsg = "The file: " + OutputFileName + " does not exist.";
							}
						}
						
						if (string.IsNullOrEmpty(ErrorMsg))
						{
							string[] Delimiters = new string[] {Path.DirectorySeparatorChar.ToString()};
							string[] PathParts = OutputFileName.Split(Delimiters, StringSplitOptions.None);
							OutputFileNameOnly = PathParts[PathParts.Length - 1];
						}
					}
					else
					{				
						ErrorMsg =
							CommonMethods.GetSqlToUse(
								FbConnectionString, CommonItem.CompanyIdValue, NewId, out SqlToUse, out OutputFileName, out OutputFileNameOnly, out Extension);
						if (string.IsNullOrEmpty(ErrorMsg))
						{
							ErrorMsg = WriteOutputFile(SqlToUse, OutputFileName + Extension);
							OutputFileNameOnly = OutputFileNameOnly + Extension;
						}
					}

                    WindowsEventLoggingUtilities.WriteToInfoEvent("OutputFileName: " + OutputFileNameOnly, Method, 0);
				}
				
				if (!string.IsNullOrEmpty(ErrorMsg))
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
		
		private string GetReportSource(out string ErrorMsg)
		{
			string Result = "";
			ErrorMsg = "";
			string Method = "GetReportSource";
			LogOnInsideMethod(Method);
			
			StringBuilder Entries = new StringBuilder();
			
			ErrorMsg =
				CommonMethods.GetEndOfMonthReportsGroupEntries(
					FbConnectionString, CommonItem.CompanyIdValue, FRequestItem.BankId, FRequestItem.ComboId, ref Entries);
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
		
		private string GetSqlToUse(out string SqlToUse, out string FileName, out string FileNameOnly)
		{
			string Result = "";
			//string Method = "GetSqlToUse";
			SqlToUse = "";
			FileName = "";
			FileNameOnly = "";
			
			string QueryText =
				" SELECT First 1 SqlToUse, FileName FROM EndOfMonthReports(@CompanyId)" +
				" WHERE BankId = @BankId AND UseId = @ReportId";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("@CompanyId", CommonItem.CompanyIdValue));
			Params.Add(new FbParameterItem("@BankId", FRequestItem.BankId));
			Params.Add(new FbParameterItem("ReportId", FRequestItem.ReportId));
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
				Object Obj = tbl.Rows[0]["SqlToUse"];
				SqlToUse = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
				Obj = tbl.Rows[0]["FileName"];
				FileName = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
				FileNameOnly = FileName;
				
				
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
		
		private string WriteOutputFile(string SqlToUse, string FileName)
		{
			string Result = "";
			string Method = "WriteOutputFile";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " + Method, FModuleName, MasterId);
			
			DataTable tbl =
				UsefulDBMethods.Helpers.GetFirstDataTableEx(
					FbConnectionString, SqlToUse, null, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
			if ((string.IsNullOrEmpty(Result)) && (tbl.Rows.Count > 0))
			{
				//Object Obj = tbl.Rows[0]["eatake_base64"];
				Object Obj = tbl.Rows[0][0];
				byte[] toBytes = (byte[])Obj;
				string Base64String = System.Text.Encoding.Default.GetString(toBytes);
				File.WriteAllBytes(FileName, Convert.FromBase64String(Base64String));
				WindowsEventLoggingUtilities.WriteToInfoEvent("After writing the file: " + FileName, FModuleName, MasterId);
			}
			
			if (!File.Exists(FileName)) Result = "The file: " + FileName + " does NOT exist.";
			
			return Result;
		}
	}
}
