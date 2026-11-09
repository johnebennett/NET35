/*
 * Created by SharpDevelop.
 * User: John
 * Date: 29/11/2019
 * Time: 7:13 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text;
using System.Data;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CommonUtilities;
using EAOfficeAPI;

namespace Actions.Ledger
{
	/// <summary>
	/// Description of CommonMethods.
	/// </summary>
	public class CommonMethods
	{
		public CommonMethods()
		{
		}
		
		public static string LoadSetupPageEntriesRequestItem(ActionBase AActionBase, 
		                                                     ref SetupPageEntriesRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadDisplayReportRequestItem";
			try 
			{
				Object Obj = AActionBase.GetCollectionValue("Data", "OfficeId");
				TempItem.BranchIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = AActionBase.GetCollectionValue("Data", "ManagerId");
				TempItem.ManagerIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = AActionBase.GetCollectionValue("Data", "LedgerId");
				TempItem.LedgerIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = AActionBase.GetCollectionValue("Data", "EventId");
				TempItem.EventIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = AActionBase.GetCollectionValue("Data", "ReportId");
				TempItem.ReportIdValue = (Obj == null) ? "" : Convert.ToString(Obj); 

				Obj = AActionBase.GetCollectionValue("Data", "ActiveStatusId");
				TempItem.ActiveStatusValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = AActionBase.GetCollectionValue("Data", "RunNumber");
				TempItem.RunNumberValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = AActionBase.GetCollectionValue("Data", "ScreenOrBulk");
				TempItem.ScreenOrBulkValue = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = AActionBase.GetCollectionValue("Data", "GSTRegistered");
				TempItem.GSTRegisteredValue = (Obj == null) ? "" : Convert.ToString(Obj); 

				Obj = AActionBase.GetCollectionValue("Data", "FindText");
				TempItem.FindText = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = AActionBase.GetCollectionValue("Data", "DateFrom");
				TempItem.DateFrom = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = AActionBase.GetCollectionValue("Data", "DateTo");
				TempItem.DateTo = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = AActionBase.GetCollectionValue("Data", "DateShow");
				TempItem.DateShow = (Obj == null) ? "" : Convert.ToString(Obj); 
				Obj = AActionBase.GetCollectionValue("Data", "OnlyShowPDFEntries");
				TempItem.OnlyShowPDFEntreis = (Obj == null) ? "N" : Convert.ToString(Obj); 
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
		
		public static string GetLedgerScreenSqlToUse(string ConnectionString,
		                                             Int64 CompanyId,
		                                             int BranchId,
		                                             int ManagerId,
		                                             int ActiveStatus,
		                                             int LedgerId,
		                                             string FindText,
		                                             int RunNumber,
		                                             int GSTRegistered,
		                                             out string ErrorMsg)
		{
			string Result = "";
			ErrorMsg = "";
			string Method = "GetLedgerScreenSqlToUse";
			
			string QueryText =
				"SELECT SCREENSQL FROM EALEDGERSCREEN(" +
				" @CompanyId, @BranchId, @ManagerId, @ActiveStatus, @LedgerId, @FindText, @RunNumber, @GSTRegistered)";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("CompanyId", CompanyId));
			Params.Add(new FbParameterItem("BranchId", BranchId));
			Params.Add(new FbParameterItem("ManagerId", ManagerId));
			Params.Add(new FbParameterItem("ActiveStatus", ActiveStatus));
			Params.Add(new FbParameterItem("LedgerId", LedgerId));
			Params.Add(new FbParameterItem("FindText", FindText));
			Params.Add(new FbParameterItem("RunNumber", RunNumber));
			Params.Add(new FbParameterItem("GSTRegistered", GSTRegistered));
			ErrorMsg = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
			
			if (string.IsNullOrEmpty(ErrorMsg))
			{
				try 
				{
					DataTable tbl = 
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, QueryText, Params, 0, 
							UsefulDBMethods.FirebirdQueryCheckTypes.HasTablesAndRows, out ErrorMsg);
					if (string.IsNullOrEmpty(ErrorMsg))
					{
						object Obj = tbl.Rows[0]["ScreenSql"];
						Result = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					}
				}
				catch (Exception Ex)
				{
				Result = Method + Environment.NewLine + Ex.Message + Environment.NewLine + Ex.InnerException;
				}
			}
			
			return Result;			
		}
		
		public static string BuildLedgerScreenGridEntries(string ConnectionString,
		                                                  string SqlToUse,
		                                                  string ArrayName,
		                                                  ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "GetLedgerScreenSqlToUse";
			
			try 
			{
				DataTable tbl =
					UsefulDBMethods.Helpers.GetFirstDataTableEx(
						ConnectionString, SqlToUse, null, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
				if (string.IsNullOrEmpty(Result))
				{
					Result = CommonUtilities.JsonUtilities.BuildJsonArrayFromDataTable(tbl, ArrayName, ref Entries);
				}
			}
			catch (Exception Ex)
			{
				Result = Method + Environment.NewLine + Ex.Message + Environment.NewLine + Ex.InnerException;
			}
			
			return Result;
		}
		
		public static string BuildLedgerScreenGroupEntries(string ConnectionString,
		                                                   Int64 CompanyId,
		                                                   int BranchId,
		                                                   int ManagerId,
		                                                   int LedgerId,
		                                                   int EventId,
		                                                   int ReportId,
		                                                   int ActiveStatus,
		                                                   int RunNumber,
		                                                   int AddedById,
		                                                   int ScreenOrBulk,
		                                                   int GSTRegistered,
		                                                   string FindText,
		                                                   string DateFrom,
		                                                   string DateTo,
		                                                   string DateShow,
		                                                   string OnlyShowPDFEntries,
		                                                   ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "BuildLedgerScreenGroupEntries";
			
			try
			{
			
				string QueryText = 
					" SELECT OUTPUTLINE FROM EALEDGERSCREENGROUP(@CompanyId, @BranchId, @ManagerId, @LedgerId, @EventId, @ReportId," +
					" @ActiveStatus, @RunNumber, @AddedById, @ScreenOrBulk, @GSTRegistered, @FindText, @DateFrom, @DateTo, @DateShow, @OnlyShowPDFEntries)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				Params.Add(new FbParameterItem("BranchId", BranchId));
				Params.Add(new FbParameterItem("ManagerId", ManagerId));
				Params.Add(new FbParameterItem("LedgerId", LedgerId));
				Params.Add(new FbParameterItem("EventId", EventId));
				Params.Add(new FbParameterItem("ReportId", ReportId));
				Params.Add(new FbParameterItem("ActiveStatus", ActiveStatus));
				Params.Add(new FbParameterItem("RunNumber", RunNumber));
				Params.Add(new FbParameterItem("AddedById", AddedById));
				Params.Add(new FbParameterItem("ScreenOrBulk", ScreenOrBulk));
				Params.Add(new FbParameterItem("GSTRegistered", GSTRegistered));
				Params.Add(new FbParameterItem("FindText", FindText));
				Params.Add(new FbParameterItem("DateFrom", DateFrom));
				Params.Add(new FbParameterItem("DateTo", DateTo));
				Params.Add(new FbParameterItem("DateShow", DateShow));
				Params.Add(new FbParameterItem("OnlyShowPDFEntries", OnlyShowPDFEntries));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);

				DataTable dt = null;
				if (string.IsNullOrEmpty(Result))
				{
					try 
					{
						dt = 
							UsefulDBMethods.Helpers.GetFirstDataTableEx(
								ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
					}
					catch (Exception Ex)
					{
						Result = Ex.Message + Environment.NewLine + Ex.InnerException;
					}
				}
				
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
				Result = Method + Environment.NewLine + Ex.Message + Environment.NewLine + Ex.InnerException;
			}
			
			return Result;
		}
		
		public static string EALedgerScreenBulk(string ConnectionString,
		                                        int RunId,
		                                        Int64 CompanyId,
		                                        int BranchId,
		                                        int ManagerId,
		                                        int LedgerId,
		                                        int EventId,
		                                        //int ReportId,
		                                        int ActiveStatus,
		                                        int RunNumber,
		                                        int AddedById,
		                                        //int ScreenOrBulk,
		                                        int GSTRegistered,
		                                        string FindText,
		                                        string DateFrom,
		                                        string DateTo)
		{
			string Result = "";
			string Method = "EALedgerScreenBulk";
			
			try
			{
			
				string QueryText = 
					" EXECUTE PROCEDURE EALEDGERSCREENBULK (" +
					"   @COMPANYID, @ELBID, @BRANCHID, @MANAGERID, @RUNNUMBER, @GSTREGISTERED, @LEDGERTYPE," +
					"   @DATEFROM, @DATETO, @ADDEDBYID, @EVENTID, @ACCOUNTID, @EXCLUSIONID, @ReportId, @ActiveStatus, @FindText)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				Params.Add(new FbParameterItem("ELBID", RunId));
				Params.Add(new FbParameterItem("BranchId", BranchId));
				Params.Add(new FbParameterItem("ManagerId", ManagerId));
				Params.Add(new FbParameterItem("RunNumber", RunNumber));
				Params.Add(new FbParameterItem("GSTRegistered", GSTRegistered));
				Params.Add(new FbParameterItem("LEDGERTYPE", LedgerId));

				Params.Add(new FbParameterItem("DateFrom", DateFrom));
				Params.Add(new FbParameterItem("DateTo", DateTo));
				Params.Add(new FbParameterItem("AddedById", AddedById));
				Params.Add(new FbParameterItem("EventId", EventId));

				Params.Add(new FbParameterItem("AccountId", 0));
				Params.Add(new FbParameterItem("ExclusionId", 0));
				Params.Add(new FbParameterItem("ReportId", 0));
				Params.Add(new FbParameterItem("ActiveStatus", ActiveStatus));
				Params.Add(new FbParameterItem("FindText", FindText));
				/*
				Params.Add(new FbParameterItem("ScreenOrBulk", ScreenOrBulk));
				Params.Add(new FbParameterItem("DateShow", DateShow));
				*/
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
				}
			}
			catch (Exception Ex)
			{
				Result = Method + Environment.NewLine + Ex.Message + Environment.NewLine + Ex.InnerException;
			}
			
			return Result;
		}
		
		public static string EALedgerScreenCreate(string ConnectionString,
		                                          Int64 CompanyId,
		                                          Int64 RunId)
		{
			string Result = "";
			string Method = "EALedgerScreenCreate";
			
			try
			{
			
				string QueryText = "EXECUTE PROCEDURE EALEDGERSCREENCREATE (@COMPANYID, @RunID)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				Params.Add(new FbParameterItem("RunId", RunId));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
				}
			}
			catch (Exception Ex)
			{
				Result = Method + Environment.NewLine + Ex.Message + Environment.NewLine + Ex.InnerException;
			}
			return Result;			
		}
	}
}
