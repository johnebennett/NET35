/*
 * Created by SharpDevelop.
 * User: John
 * Date: 17/12/2019
 * Time: 9:43 PM
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
using System.Reflection;

using CommonUtilities;
using EAOfficeAPI;

namespace Actions.ContactOperations
{
	/// <summary>
	/// Description of BulkEmail_ProcessRun.
	/// </summary>
	public class BulkEmail_ProcessRun : ActionBase
	{
		private string FModuleName = "ContactOperations";
		private BulkEmail_ProcessRunRequestItem FRequestItem;
		
		public static string Execute(int MasterId,
									 APISettingsItem SettingsItem,
									 string Request,
									 out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			BulkEmail_ProcessRun inst = new BulkEmail_ProcessRun();
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
				FRequestItem = new BulkEmail_ProcessRunRequestItem();
				Result = FRequestItem.Load(this);
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
				Result = BuildOutput(out ErrorMsg);
				if (!string.IsNullOrEmpty(ErrorMsg))
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
		
		private string BuildOutput(out string ErrorMsg)
		{
			ErrorMsg = "";
			string Result = "";
			string Method = "BuildOutput";
			
			LogOnInsideMethod(Method);
			try
			{
				WindowsEventLoggingUtilities.WriteToInfoEvent("Inside BuildOutput", FModuleName, MasterId);
				
				int TempInt = 0;
				string EventRunSource = "";
				ErrorMsg = CommonMethods.BulkEmail_GetEventRunId(this, FRequestItem.RunId, FRequestItem.Source, out TempInt);
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					FRequestItem.EventRunId = TempInt;
					FRequestItem.EventRunIdValue = TempInt.ToString();
					//ErrorMsg = CommonMethods.BulkEmail_GetEventRunSource(this, FRequestItem.RunId, FRequestItem.Source, out EventRunSource);					
					ErrorMsg = CommonMethods.BulkEmail_GetEventRunSource(this, FRequestItem.EventRunId, out EventRunSource);
				}
				
				int StatementRunId = 0;
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					FRequestItem.EventRunSource = EventRunSource;
					ErrorMsg = CommonMethods.BulkEmail_GetStatementRunId(this, FRequestItem.RunId, FRequestItem.Source, out StatementRunId);
				}
				
				string TodayStr = MiscUtilities.DateTimeToYYYYMMDD(DateTime.Now);				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent("EventRunId: " + FRequestItem.EventRunId.ToString(), FModuleName, MasterId);
					WindowsEventLoggingUtilities.WriteToInfoEvent("EventRunIdValue: " + FRequestItem.EventRunIdValue, FModuleName, MasterId);
					WindowsEventLoggingUtilities.WriteToInfoEvent("EventRunSource: " + FRequestItem.EventRunSource, FModuleName, MasterId);
					WindowsEventLoggingUtilities.WriteToInfoEvent("StatementRunId: " + StatementRunId.ToString(), FModuleName, MasterId);
					ErrorMsg =
						UsefulDBMethods.EventHelpers.ExecuteEventRunCreate(
							FbConnectionString, FRequestItem.EventRunId, CommonItem.CompanyIdValue, 0, 0, 0, CommonItem.ClientIdValue, 0, "", "", "",
							FRequestItem.EmailOption, "N", "N", FRequestItem.TemplateId, FRequestItem.Subject, FRequestItem.CCValue, FRequestItem.BCCValue,
							FRequestItem.ExclusionsValue, FRequestItem.EventRunSource, "N", 0, FRequestItem.ReportId, StatementRunId, 0, 0, TodayStr, TodayStr, "", 0);
					WindowsEventLoggingUtilities.WriteToInfoEvent("After ExecuteEventRunCreate", FModuleName, MasterId);
				}

                if (string.IsNullOrEmpty(ErrorMsg))
                {
                    string QueryText =
                        "UPDATE EAEventRun SET " +
                        "EVA_FromEmail = @FromId" +
                        " WHERE EVA_ID = @RunId";
                    List<FbParameterItem> Params = new List<FbParameterItem>();
                    Params.Add(new FbParameterItem("@FromId", FRequestItem.From));
                    Params.Add(new FbParameterItem("@RunId", FRequestItem.EventRunId));

                    UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, 0);
                    WindowsEventLoggingUtilities.WriteToInfoEvent("Before calling DBExecute", FModuleName, 0);
                    ErrorMsg = CommonUtilities.FirebirdUtilities.DBExecute(FbConnectionString, QueryText, Params, 0);
                    WindowsEventLoggingUtilities.WriteToInfoEvent("After calling DBExecute", FModuleName, 0);
                    WindowsEventLoggingUtilities.WriteToInfoEvent("Result: " + ErrorMsg, FModuleName, 0);
                }

                // When the TemplateId == 0 we assume that the email body is in HTML format already
                //string UseHtmlEmail = "";
                //if (string.IsNullOrEmpty(ErrorMsg))
                //{
                //    UseHtmlEmail = CommonMethods.GetOptionValue("$.options.BulkEmail.UseHtmlEmail", out ErrorMsg);
                //    WindowsEventLoggingUtilities.WriteToInfoEvent("UseHtmlEmail value: " + UseHtmlEmail, Method, 0);
                //}

                //if (string.IsNullOrEmpty(ErrorMsg)) 
                //{
                //if (UseHtmlEmail == "Y")
                {
                        WindowsEventLoggingUtilities.WriteToInfoEvent("Before UpdateEmailBodyForEventRunId", FModuleName, MasterId);
                        ErrorMsg =
                            EmailStatements.CommonMethods.UpdateEmailBodyForEventRunId(
                                FbConnectionString, FRequestItem.EventRunId, FRequestItem.EmailBody);
                        WindowsEventLoggingUtilities.WriteToInfoEvent("After UpdateEmailBodyForEventRunId", FModuleName, MasterId);
                    }
                    //else
                    //{
                    //    if (FRequestItem.TemplateId == 0)
                    //    {
                    //        WindowsEventLoggingUtilities.WriteToInfoEvent("Before UpdateEmailBodyForEventRunId", FModuleName, MasterId);
                    //        ErrorMsg =
                    //            EmailStatements.CommonMethods.UpdateEmailBodyForEventRunId(
                    //                FbConnectionString, FRequestItem.EventRunId, FRequestItem.EmailBody);
                    //        WindowsEventLoggingUtilities.WriteToInfoEvent("After UpdateEmailBodyForEventRunId", FModuleName, MasterId);
                    //        if (string.IsNullOrEmpty(ErrorMsg))
                    //        {
                    //            WindowsEventLoggingUtilities.WriteToInfoEvent("Before HtmlToRtf", FModuleName, MasterId);
                    //            ErrorMsg =
                    //                CommonUtilities.UtilityApps.HtmlToRtf(
                    //                        "EAEventRun", "EVA_EmailBody", "EVA_ID", FRequestItem.EventRunId,
                    //                        "EAEventRun", "EVA_EmailBody", "EVA_ID", FRequestItem.EventRunId);
                    //            WindowsEventLoggingUtilities.WriteToInfoEvent("After HtmlToRtf", FModuleName, MasterId);
                    //        }
                    //    }
                    //}
                //}

                if (string.IsNullOrEmpty(ErrorMsg))
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent("Before EALedgerScreenEmailPost", FModuleName, MasterId);
					//ErrorMsg = EALedgerScreenEmailPost(FbConnectionString, CommonItem.CompanyIdValue, FRequestItem.RunId);
					ErrorMsg = EALedgerScreenEmailPost(FbConnectionString, CommonItem.CompanyIdValue, FRequestItem.EventRunId);
					WindowsEventLoggingUtilities.WriteToInfoEvent("After EALedgerScreenEmailPost", FModuleName, MasterId);
				}

                if (string.IsNullOrEmpty(ErrorMsg))
                {
                    ErrorMsg =
                        CommonMethods.UpdateSignatureIdForEventRunId(
                        FbConnectionString, FRequestItem.EventRunId, FRequestItem.SignatureId);
                }

                if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg = 
						Actions.ContactOperations.CommonMethods.ExecuteEventRunProcess(
							FbConnectionString, FRequestItem.EventRunId, CommonItem.CompanyIdValue);
					WindowsEventLoggingUtilities.WriteToInfoEvent("After calling ExecuteEventRunProcess", FModuleName, MasterId);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					// Call the EmailProcessor to handle the sending of the emails
					string WorkingDir = "";
                    WorkingDir = CommonUtilities.FolderHelpers.GetAssemblyDirectoryName(Assembly.GetExecutingAssembly());
                    /*
					string SepChar = Path.DirectorySeparatorChar.ToString();
					if (!WorkingDir.EndsWith(SepChar)) WorkingDir += SepChar;
					WindowsEventLoggingUtilities.WriteToInfoEvent("After calling GetCurrentDirectory", FModuleName, MasterId);
					WindowsEventLoggingUtilities.WriteToInfoEvent("WorkingDir " + WorkingDir, FModuleName, MasterId);
					try
					{
						Process runApp = new Process();
                        //if (UseHtmlEmail == "Y")
                            runApp.StartInfo.FileName = WorkingDir + "EmailProcessorHtml";
                        //else
                            //runApp.StartInfo.FileName = WorkingDir + "EmailProcessor";
                        runApp.StartInfo.Arguments = FRequestItem.EventRunId.ToString();
						runApp.Start();
					}
					*/
					try
					{
						Result =
							CommonMethods.SetNewEmailStatus(
								FbConnectionString, FRequestItem.EventRunId.ToString());
					}
					catch (Exception Ex)
					{
						Result = LogOnException(Method, Ex, "");
					}
					finally
					{
						//Directory.SetCurrentDirectory(WorkingDir);
					}
					
					if (string.IsNullOrEmpty(Result))
					{
						StringBuilder DataAsJson = new StringBuilder();
						DataAsJson.Append(JsonUtilities.StartJson());
						DataAsJson.Append(JsonUtilities.StartJsonObject("result"));
						DataAsJson.Append(JsonUtilities.AddPropertyNameAndValue("RunId", FRequestItem.RunIdValue));
						DataAsJson.Append(JsonUtilities.AddPropertyNameAndValue("EventRunId", FRequestItem.EventRunId));
						DataAsJson.Append(JsonUtilities.AddPropertyNameAndValue("Status", "Success"));
						DataAsJson.Append(JsonUtilities.EndJsonObject());
						DataAsJson.Append(JsonUtilities.EndJson());
						Result = LogResponse(Method, DataAsJson.ToString());
					}
				}
				
				StringBuilder entries = new StringBuilder();				
				entries.AppendLine(JsonUtilities.StartJson());
					
				//ErrorMsg = BuildOutputEntries(ref entries);				
				if (!string.IsNullOrEmpty(ErrorMsg)) return "";
				
				entries.AppendLine(JsonUtilities.EndJson());
				Result = entries.ToString();
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
		
		private string EALedgerScreenEmailPost(string ConnectionString, 
		                                       Int64 CompanyId,
		                                       int RunId)
		{
			string Result = "";
			string Method = "EALedgerScreenEmailPost";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, FModuleName, MasterId);
					
			LogOnInsideMethod(Method);
			try 
			{
				string QueryText = "EXECUTE PROCEDURE EALEDGERSCREENEMAILPOST(@CompanyId, @RunId)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				Params.Add(new FbParameterItem("RunId", RunId));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
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
			
		
		/*
		private string BuildOutputEntries(ref StringBuilder Entries)
		{
			string Method = "BuildOutputEntries";
			string Result = "";			
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, FModuleName, MasterId);
			
			LogOnInsideMethod(Method);
			try
			{
				string QueryText = "";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				//Params.Add(new FbParameterItem("DisplayType", FSearchGridRequestItem.DisplayType));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
				
				DataTable tbl = null;
				if (string.IsNullOrEmpty(Result))
				{
					tbl = 
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							FbConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.None, out Result);
					if (string.IsNullOrEmpty(Result) && (tbl.Rows.Count > 0))
					{
						//StringBuilder Entries = new StringBuilder();
						Entries.AppendLine(JsonUtilities.StartJson());
						
						Result = JsonUtilities.BuildJsonArrayFromDataTable(tbl, "whyNotCancelled", ref Entries);

						Entries.AppendLine(JsonUtilities.EndJson());
						Result = Entries.ToString();
					}
				}

				
				string Str = "";
				string ColName = "";
				string ColValue = "";
				List<string> RowList = new List<string>();
				List<string> StrList = new List<string>();
				object Obj = null;
				Entries.AppendLine(JsonUtilities.StartJsonArray("Items"));
				foreach (DataRow ThisRow in dt.Rows)
				{
					StrList.Clear();
					foreach (DataColumn ThisCol in dt.Columns)
					{
						ColName = ThisCol.ColumnName;
						Obj = ThisRow[ColName];
						ColValue = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
						StrList.Add(JsonUtilities.AddPropertyNameAndValue(ColName, ColValue, false));
					}
					Str =
						JsonUtilities.StartJsonObject("") +
						string.Join(",", StrList.ToArray()) +
						JsonUtilities.EndJsonObject();
					RowList.Add(Str);
				}
				Entries.Append(string.Join(",", RowList.ToArray()));
				Entries.AppendLine(JsonUtilities.EndJsonArray());
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
		*/
	}
}
