/*
 * Created by SharpDevelop.
 * User: John
 * Date: 10/12/2019
 * Time: 7:46 AM
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

using CommonUtilities;
using EAOfficeAPI;

namespace Actions.ContactOperations
{
	/// <summary>
	/// Description of BulkEmail_SetupPageEntries.
	/// </summary>
	public class BulkEmail_SetupPageEntries : ActionBase
	{
		private string FModuleName = "ContactOperations";
		private BulkEmail_SetupPageEntries_RequestItem FRequestItem;
		
		public static string Execute(int MasterId,
									 APISettingsItem SettingsItem,
									 string Request,
									 out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			BulkEmail_SetupPageEntries inst = new BulkEmail_SetupPageEntries();
			//if (!inst.Setup(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg))
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
				FRequestItem = new BulkEmail_SetupPageEntries_RequestItem();
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
				
				StringBuilder entries = new StringBuilder();
				//entries.AppendLine(JsonUtilities.StartJson());
					
				ErrorMsg = BuildComboEntries(ref entries);				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					//if (entries.Length > 0) entries.AppendLine(",");
					ErrorMsg = BuildGridEntries(ref entries);
				}
				
				// create an eventrun record				
				/*
				int EventRunId = 0;
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					EventRunId =
						UsefulDBMethods.GeneratorHelpers.GetNextGeneratorId(
							FbConnectionString, "EVA_ID_GEN", CommonItem.CompanyIdValue, CommonItem.ClientIdValue, out ErrorMsg);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					entries.AppendLine(",");
					entries.Append(JsonUtilities.AddPropertyNameAndValue("EventRunId", EventRunId, false));
					
					string TodayStr = MiscUtilities.DateTimeToYYYYMMDD(DateTime.Now);
					ErrorMsg =
						UsefulDBMethods.EventHelpers.ExecuteEventRunCreate(
							FbConnectionString, EventRunId, CommonItem.CompanyIdValue, 0, 0, 0, CommonItem.ClientIdValue, 0, "", "", "", 0, "N", "N",
							0, "", "", "", "", "Bulk_Email", //"Contact-EmailStatements", 
							"N", // forMyself
							0, // ForMyselfPdfOrXml
							0, FRequestItem.RunId, 0, 0, TodayStr, TodayStr, "", 0);
				}
				*/
				
				//entries.AppendLine(JsonUtilities.EndJson());
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Result =
						JsonUtilities.StartJson() +
						entries.ToString() +
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
			
			return Result;
		}
		
		private string BuildComboEntries(ref StringBuilder Entries)
		{
			string Method = "BuildComboEntries";
			string Result = "";			
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, FModuleName, MasterId);
			
			LogOnInsideMethod(Method);
			try
			{
				string QueryText = "SELECT * FROM Init_BulkEmail_Items(@CompanyId, @ClientId, @Source, @RunId)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("@CompanyId", CommonItem.CompanyIdValue));
				Params.Add(new FbParameterItem("ClientId", CommonItem.ClientIdValue));
				Params.Add(new FbParameterItem("Source", FRequestItem.Source));
				Params.Add(new FbParameterItem("RunId", FRequestItem.RunId));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);

				DataTable dt = null;
				try 
				{
					dt = 
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							FbConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTablesAndRows, out Result);
					if (string.IsNullOrEmpty(Result))
					{
						foreach (DataRow dataRow in dt.Rows)
						{
							Entries.AppendLine((dataRow[0] == DBNull.Value) ? "" : Convert.ToString(dataRow[0]));
						}
					}
				}
				catch (Exception Ex)
				{
					Result = LogOnException(Method, Ex, "");
					return Result;
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
		
		private string BuildGridEntries(ref StringBuilder Entries)
		{
			string Method = "BuildGridEntries";
			string Result = "";			
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, FModuleName, MasterId);
			
			LogOnInsideMethod(Method);
			try
			{
				switch (FRequestItem.Source.ToLower())
				{
					case "ledger": Result = BuildLedgerEntries(ref Entries); break;
					case "debts": Result = BuildDebtsEntries(ref Entries); break;
					case "addinvoice": Result = BuildAddInvoiceEntries(ref Entries); break;
					default: Result = "Unknown Source: " + FRequestItem.Source; break;
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
		
		private string BuildLedgerEntries(ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "BuildLedgerEntries";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, FModuleName, MasterId);
			
			LogOnInsideMethod(Method);
			try 
			{
				//Result = BuildLedgerGridEntries(ref Entries);
				Entries.Append(",");
				Result = CommonMethods.BuildLedgerGridEntries(this, FRequestItem.RunId, ref Entries);
				if (string.IsNullOrEmpty(Result))
				{
					Result = BuildLedgerReportEntries(ref Entries);
				}				
			}
			catch (Exception Ex)
			{
				Result = LogOnException(Method, Ex, "");
			}
			finally
			{
				WindowsEventLoggingUtilities.WriteToInfoEvent("Result: " + Result, FModuleName, MasterId);
				LogOnLeavingMethod(Method);
			}
			
			return Result;
		}
		
		/*
		private string BuildLedgerGridEntries(ref StringBuilder Entries)
		{
			string Method = "BuildLedgerGridEntries";
			string Result = "";			
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, FModuleName, MasterId);
			
			LogOnInsideMethod(Method);
			try
			{
	            Entries.Append(",");
	            Entries.Append(CommonUtilities.JsonUtilities.StartJsonArray("gridEntries"));

				string QueryText = "SELECT ELB_SQLFOREMAIL FROM EALEDGERBULKEVENT where I_ELB_ID= @RunId";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("RunId", FRequestItem.RunId));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);

				object Obj = null;
				DataTable tbl = null;
				string SqlToUse = "";
				if (string.IsNullOrEmpty(Result))
				{
					tbl = 
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							FbConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTablesAndRows, out Result);
					if (string.IsNullOrEmpty(Result))
					{
						Obj = tbl.Rows[0]["ELB_SqlForEmail"];
						SqlToUse = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					}
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					SqlToUse = SqlToUse.Replace("\"", "'");
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							FbConnectionString, SqlToUse, null, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
					if (string.IsNullOrEmpty(Result))
					{
						List<string> RowList = new List<string>();
						List<string> StrList = new List<string>();
						string ColName = "";
						string ColValue = "";
						string Str = "";
						if (tbl.Rows.Count > 0)
						{
							RowList.Clear();				
							Str = "";
							int RowCount = tbl.Rows.Count;
							foreach(DataRow ThisRow in tbl.Rows)
							{
								StrList.Clear();
								foreach(DataColumn ThisCol in tbl.Columns)
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
							
							Entries.AppendLine(string.Join(",", RowList.ToArray()));
						}			
					}
				}
			}
			catch (Exception Ex)
			{
				Result = LogOnException(Method, Ex, "");
			}
			finally
			{
	            Entries.Append(CommonUtilities.JsonUtilities.EndJsonArray());
				LogOnLeavingMethod(Method);
			}
			
			return Result;
		}
		*/
		
		private string BuildLedgerReportEntries(ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "BuildLedgerReportEntries";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, FModuleName, MasterId);
					
			LogOnInsideMethod(Method);
			try 
			{
	            Entries.Append(",");
	            Entries.Append(CommonUtilities.JsonUtilities.StartJsonArray("reports"));

				string QueryText = "SELECT ELB_SQLTOGETREPORT FROM EALEDGERBULKEVENT where I_ELB_ID= @RunId";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("RunId", FRequestItem.RunId));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);

				object Obj = null;
				DataTable tbl = null;
				string SqlToUse = "";
				if (string.IsNullOrEmpty(Result))
				{
					tbl = 
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							FbConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTablesAndRows, out Result);
					if (string.IsNullOrEmpty(Result))
					{
						Obj = tbl.Rows[0]["ELB_SQLTOGETREPORT"];
						SqlToUse = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					}
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					SqlToUse = SqlToUse.Replace("\"", "'");
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							FbConnectionString, SqlToUse, null, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
					if (string.IsNullOrEmpty(Result))
					{
						List<string> RowList = new List<string>();
						List<string> StrList = new List<string>();
						string ColName = "";
						string ColValue = "";
						string Str = "";
						if (tbl.Rows.Count > 0)
						{
							RowList.Clear();				
							Str = "";
							foreach(DataRow ThisRow in tbl.Rows)
							{
								StrList.Clear();
								foreach(DataColumn ThisCol in tbl.Columns)
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
							
							Entries.AppendLine(string.Join(",", RowList.ToArray()));
						}			
					}
				}
			}
			catch (Exception Ex)
			{
				Result = LogOnException(Method, Ex, "");
			}
			finally
			{
	            Entries.Append(CommonUtilities.JsonUtilities.EndJsonArray());
				LogOnLeavingMethod(Method);
			}
			
			return Result;
		}
		
		private string BuildDebtsEntries(ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "BuildDebtsEntries";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, FModuleName, MasterId);
			
			LogOnInsideMethod(Method);
			try 
			{
				//Result = BuildLedgerGridEntries(ref Entries);
				Entries.Append(",");
				Result = CommonMethods.BuildLedgerGridEntries(this, FRequestItem.RunId, ref Entries);
				//Result = CommonMethods.BuildDebtsGridEntries(this, FRequestItem.RunId, ref Entries);
				if (string.IsNullOrEmpty(Result))
				{
					//Result = BuildDebtsReportEntries(ref Entries);
					Result = BuildLedgerReportEntries(ref Entries);
				}				
			}
			catch (Exception Ex)
			{
				Result = LogOnException(Method, Ex, "");
			}
			finally
			{
				WindowsEventLoggingUtilities.WriteToInfoEvent("Result: " + Result, FModuleName, MasterId);
				LogOnLeavingMethod(Method);
			}
			
			return Result;
		}
		
		private string BuildAddInvoiceEntries(ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "BuildAddInvoiceEntries";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, FModuleName, MasterId);
					
			LogOnInsideMethod(Method);
			try 
			{
				//Result = BuildLedgerGridEntries(ref Entries);
				Entries.Append(",");
				Result = CommonMethods.BuildLedgerGridEntries(this, FRequestItem.RunId, ref Entries);
				//Result = CommonMethods.BuildDebtsGridEntries(this, FRequestItem.RunId, ref Entries);
				if (string.IsNullOrEmpty(Result))
				{
					//Result = BuildDebtsReportEntries(ref Entries);
					Result = BuildLedgerReportEntries(ref Entries);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					//WindowsEventLoggingUtilities.WriteToInfoEvent("Before PopulateAddInvoiceAttatchment RunId: " + FRequestItem.RunId.ToString(), Method, 0);
					Result =
						CommonMethods.PopulateAddInvoiceAttatchment(FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue, FRequestItem.RunId);
					//WindowsEventLoggingUtilities.WriteToInfoEvent("After PopulateAddInvoiceAttatchment RunId: " + FRequestItem.RunId.ToString(), Method, 0);
				}

                //select ealedgerbulkevent.I_ELB_EVENTRUNID from ealedgerbulkevent where ealedgerbulkevent.I_ELB_ID = 503
                Int64 EventRunId = 0;
                if (string.IsNullOrEmpty(Result))
                {
                    Result = CommonMethods.GetEventRunIdForBulkRunId(FbConnectionString, FRequestItem.RunId, out EventRunId);
                }

                if (string.IsNullOrEmpty(Result))
				{
					Entries.Append(",");
					Result =
						CommonMethods.BuildEmailAttachmentList(FbConnectionString, EventRunId, ref Entries);
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
