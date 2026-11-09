/*
 * Created by SharpDevelop.
 * User: John
 * Date: 17/10/2019
 * Time: 11:54 AM
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
using Newtonsoft.Json.Linq;
*/

using CommonUtilities;
using EAOfficeAPI;

namespace Actions.Reconcilliation.PerformEOM
{
	/// <summary>
	/// Description of PerformEOM.
	/// </summary>
	public class PerformEOM : ActionBase
	{
		private string FModuleName = "Reconcilliation";
		//private GetReconcilliationReportRequestItem FRequestItem;
		
		private string FDocsLocation = "";
		private string FTestRPOLocation = "";

		public static string Execute(int MasterId,
									 APISettingsItem SettingsItem,
									 string Request,
									 out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			PerformEOM inst = new PerformEOM();
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
				//FRequestItem = new GetReconcilliationReportRequestItem();
				//Result = FRequestItem.Load((ActionBase)this);
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
				//Result = FRequestItem.Validate();
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
				ErrorMsg = UsefulDBMethods.Helpers.RunEAMainOfficeProcess(FbConnectionString, CommonItem.CompanyIdValue);				
				
				// Check to see if the EOM is already in progress - eg: maybe by someone else
				ErrorMsg = IsEOMInProgress();
				
				
				StringBuilder Entries = new StringBuilder();
				int NewEOMId = 0;
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					NewEOMId = 
						UsefulDBMethods.GeneratorHelpers.GetNextGeneratorId(
							FbConnectionString, "I_EndOfMonth_ID_Gen", CommonItem.CompanyIdValue, CommonItem.ClientIdValue, out ErrorMsg);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg = InsertEndOfMonthRecord(NewEOMId);
				}
				
				int EOM_Id = 0;
				string EOM_Proceed = "";
				string EOM_Status = "";
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg = GetEndOfMonthEventValues(NewEOMId, out EOM_Proceed, out EOM_Status, out EOM_Id);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					if (StringUtilities.SameText(EOM_Proceed, "Y"))
					{
						ErrorMsg = PerformEOMProcess(EOM_Id, NewEOMId);
						if (string.IsNullOrEmpty(ErrorMsg))
						{
							Result = LogResponse(Method, "");
						}
						else
						{
							Result = LogOnProcessingError(Method, ErrorMsg, "");
						}
					}
					else
					{
						Result = BuildEOMInvalidList();
					}
				}
				
				/*
				if (!string.IsNullOrEmpty(ErrorMsg))
				{
					Result = LogOnProcessingError(Method, ErrorMsg, "");
				}
				else
				{
					Result =
						JsonUtilities.StartJson() +
						JsonUtilities.AddPropertyNameAndValue("FileName", @"/Docs/" + "", false) +
						JsonUtilities.EndJson();
				}
				*/
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
		
		private string IsEOMInProgress()
		{
			string Result = "";
			string Method = "IsEOMInProgress";
			try 
			{
				string QueryText = 
					"SELECT COUNT(*) AS Total FROM ENDOFMONTHEVENT WHERE I_ENDOFMONTH_COMPANYCLIENTID = @CompanyId  and B_ENDOFMONTHINPROGRESS='Y'";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CommonItem.CompanyIdValue));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
	
				DataTable tbl = null;
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							FbConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTablesAndRows, out Result);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					DataRow Row = tbl.Rows[0];
					Object Obj = Row["Total"];
					int Total = (Obj == DBNull.Value) ? 0 : Convert.ToInt32(Obj);
					if (Total > 0)
					{
						Result = LogOnProcessingError(Method, "EOM is already in progress", "");
					}
				}
			}
			catch (Exception Ex)
			{
				Result = LogOnException(Method, Ex, "");
			}
			return Result;
		}
		
		private string InsertEndOfMonthRecord(int NewEOMId)
		{
			string Result = "";
			string Method = "InsertEndOfMonthRecord";
			try 
			{
				string QueryText = 
					" INSERT INTO EndOfMonthEvent(I_ENDOFMONTH_ID, I_ENDOFMONTH_COMPANYCLIENTID, I_ENDOFMONTH_ADDEDBYID)" +
					" VALUES(@NewEOMId, @CompanyClientId, @AddedById)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("NewEOMId", NewEOMId));
				Params.Add(new FbParameterItem("CompanyClientId", CommonItem.CompanyIdValue));
				Params.Add(new FbParameterItem("AddedById", CommonItem.ClientIdValue));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(FbConnectionString, QueryText, Params, 0);
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
		
		private string GetEndOfMonthEventValues(int NewEOMId, 
		                                        out string EOM_Proceed, 
		                                        out string EOM_Status, 
		                                        out int EOM_Id)
		{
			string Result = "";
			string Method = "GetEndOfMonthEventValues";
			EOM_Proceed = "";
			EOM_Status = "";
			EOM_Id = -1;
			try
			{
				string QueryText = 
					" SELECT B_ENDOFMONTH_PROCEED, ENDOFMONTHSTATUS, I_ENDOFMONTH_EOMID" +
					" FROM ENDOFMONTHEVENT WHERE I_ENDOFMONTH_ID = @NewEOMId";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("@NewEOMId", NewEOMId));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
	
				DataTable tbl = null;
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							FbConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTablesAndRows, out Result);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					DataRow Row = tbl.Rows[0];
					Object Obj = Row["B_ENDOFMONTH_PROCEED"];
					EOM_Proceed = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					Obj = Row["ENDOFMONTHSTATUS"];
					EOM_Status = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					Obj = Row["I_ENDOFMONTH_EOMID"];
					EOM_Id = (Obj == DBNull.Value) ? -1 : Convert.ToInt32(Obj);
				}
			}
			catch (Exception Ex)
			{
				Result = LogOnException(Method, Ex, "");
			}

			return Result;
		}
		
		private string PerformEOMProcess(int EOM_Id,
		                                 int NewEOMId)
		{
			string Result = "";
			/*
			 * Process the command lines for each required CreateFile as required
			 * Check that all the command lines have executed and completed as requried
			*/
			/* 
			 * Due to an issue where some clients files are not getting created fully we 
			 * run the ProcessEOMCreateFiles 3 times to give them every chance of being created
			 * correctly
			*/

			for (int Index = 1; Index <= 3; Index++)
			{
				if (string.IsNullOrEmpty(Result))
				{
					Result = ProcessEOMCreateFiles(EOM_Id);
				}
			}
			
			if (string.IsNullOrEmpty(Result))
			{
				Result = CheckEOMCreateFiles(EOM_Id);
			}
			
			// if all is OK here then do the EOM Rollover
			if (string.IsNullOrEmpty(Result))
			{
				Result = PerformEOMRollover(NewEOMId);
			}
			
			Int64 NewBOMId = 0;
			if (string.IsNullOrEmpty(Result))
			{
				Result = GetBOMEventValues(NewEOMId, out NewBOMId);
			}
			
			if (string.IsNullOrEmpty(Result))
			{
				Result = ProcessBOMCreateFiles(NewBOMId);
			}
			
			if (string.IsNullOrEmpty(Result))
			{
				Result = CheckBOMCreateFiles(NewBOMId);
			}
			
			if (string.IsNullOrEmpty(Result))
			{
				Result = UpdateEOMInProgressStatus();
			}
			
			return Result;
		}
		
		private string ProcessEOMCreateFiles(int EOM_Id)
		{
			string Result = "";
			string Method = "ProcessEOMCreateFiles";
			try 
			{
				if (string.IsNullOrEmpty(Result))
					FDocsLocation =
						CommonUtilities.FolderHelpers.DocsLocation(FbConnectionString, CommonItem.CompanyIdValue, out Result);
				
				if (string.IsNullOrEmpty(Result))
					FTestRPOLocation =
						CommonUtilities.FolderHelpers.TestRPOLocation(FbConnectionString, CommonItem.CompanyIdValue, out Result);
				WindowsEventLoggingUtilities.WriteToInfoEvent("TestRPOLocation: " + FTestRPOLocation, Method, 0);
				
				string QueryText ="";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					QueryText =
						" SELECT EC_CommandLine FROM EAEventCreateFile" +
						" WHERE ec_EventRunId = @EOMID AND EC_FileLocation = ''";
					Params.Add(new FbParameterItem("EOMID", EOM_Id));
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
				}
	
				DataTable tbl = null;
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							FbConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
				}				
				
				if (string.IsNullOrEmpty(Result))
				{
					string CommandLine = "";
					object Obj = null;
					foreach(DataRow Row in tbl.Rows)
					{
						if (string.IsNullOrEmpty(Result))
						{
							Obj = Row["EC_CommandLine"];
							CommandLine = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
							if (!string.IsNullOrEmpty(CommandLine))
							{
								Result = CommonMethods.ProcessTestRPOCommandLine(CommandLine, FTestRPOLocation, 1200);
							}
						}
					}
				}
			}
			catch (Exception Ex)
			{
				Result = LogOnException(Method, Ex, "");
			}
			
			return Result;
		}
		
		private string CheckEOMCreateFiles(Int64 EOM_Id)
		{
			string Result = "";
			string Method = "CheckEOMCreateFiles";
			try 
			{
				string QueryText =
					" SELECT" +
					"   (select count(*) from eaeventrun, eaeventcreatefile a" +
					"   where eva_sourcescreen = 'ENDOFMONTH'" +
					"     and eva_id = a.ec_eventrunid and eva_id = @EOM_Id) AS FirstTotal," +
					"   (select count(*) from eaeventrun,eaeventcreatefile a,eaeventrunbase64 b" +
					"   where eva_sourcescreen = 'ENDOFMONTH'" +
					"     and eva_id = a.ec_eventrunid" +
					"     and a.ec_id = b.ec_id and eva_id = @EOM_Id) AS SecondTotal" +
					" FROM EACompany WHERE I_CO_CompanyClientId = @CompanyId";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("EOM_Id", EOM_Id));
				Params.Add(new FbParameterItem("CompanyId", CommonItem.CompanyIdValue));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
	
				DataTable tbl = null;
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							FbConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					object Obj = null;
					int Total = 0;
					foreach (DataRow Row in tbl.Rows)
					{
						Obj = Row["FirstTotal"];
						Total = Total + ((Obj == DBNull.Value) ? 0 : Convert.ToInt32(Obj));
						Obj = Row["SecondTotal"];
						Total = Total - ((Obj == DBNull.Value) ? 0 : Convert.ToInt32(Obj));
					}
					
					if (Total != 0) Result = "The total in " + Method + " should be 0 but returned " + Convert.ToString(Total);
				}
				
			}
			catch (Exception Ex)
			{
				Result = LogOnException(Method, Ex, "");
			}
			
			return Result;
		}
		
		private string PerformEOMRollover(Int64 NewEOMId)
		{
			string Result = "";
			string Method = "PerformEOMRollover";
			try 
			{
				string QueryText = "EXECUTE PROCEDURE EndOfMonthRoll(@NewId)";				
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("NewId", NewEOMId));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(FbConnectionString, QueryText, Params, 0);
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
		
		private string GetBOMEventValues(Int64 NewEOMId, out Int64 NewBOMId)
		{
			string Result = "";
			string Method = "GetBOMEventValues";
			NewBOMId = 0;
			try
			{
				string QueryText = "SELECT I_ENDOFMONTH_BOMID FROM ENDOFMONTHEVENT WHERE I_ENDOFMONTH_ID = @NewEOMId";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("@NewEOMId", NewEOMId));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
	
				DataTable tbl = null;
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							FbConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTablesAndRows, out Result);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					DataRow Row = tbl.Rows[0];
					Object Obj = Row["I_ENDOFMONTH_BOMID"];
					NewBOMId = (Obj == DBNull.Value) ? -1 : Convert.ToInt32(Obj);
				}
				
				if (NewBOMId == 0)
				{
					Result = "Error gathering the BOM ID from " + Method;
				}
			}
			catch (Exception Ex)
			{
				Result = LogOnException(Method, Ex, "");
			}

			return Result;
		}
		
		private string ProcessBOMCreateFiles(Int64 NewBOMId)
		{
			string Result = "";
			string Method = "ProcessBOMCreateFiles";
			try 
			{
				string DocsLocation = "";
				string TestRPOLocation = "";
				if (string.IsNullOrEmpty(Result))
					DocsLocation =
						CommonUtilities.FolderHelpers.DocsLocation(FbConnectionString, CommonItem.CompanyIdValue, out Result);
				
				if (string.IsNullOrEmpty(Result))
					TestRPOLocation =
						CommonUtilities.FolderHelpers.TestRPOLocation(FbConnectionString, CommonItem.CompanyIdValue, out Result);
				
				string QueryText ="";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result))
				{
					QueryText =
						" SELECT EC_CommandLine FROM EAEventCreateFile" +
						" WHERE ec_EventRunId = @NewBOMId AND EC_FileLocation = ''";
					Params.Add(new FbParameterItem("NewBOMId", NewBOMId));
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
				}
	
				DataTable tbl = null;
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							FbConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
				}
				
				
				if (string.IsNullOrEmpty(Result))
				{
					string CommandLine = "";
					object Obj = null;
					foreach(DataRow Row in tbl.Rows)
					{
						if (string.IsNullOrEmpty(Result))
						{
							Obj = Row["EC_CommandLine"];
							CommandLine = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
							if (!string.IsNullOrEmpty(CommandLine))
							{
								Result = CommonMethods.ProcessTestRPOCommandLine(CommandLine, TestRPOLocation, 1200);
							}
						}
					}
				}
			}
			catch (Exception Ex)
			{
				Result = LogOnException(Method, Ex, "");
			}
			
			return Result;
		}
		
		private string CheckBOMCreateFiles(Int64 NewBOMId)
		{
			string Result = "";
			string Method = "CheckEOMCreateFiles";
			try 
			{
				string QueryText =
					" SELECT" +
					"   (select count(*) from eaeventrun, eaeventcreatefile a" +
					"   where eva_sourcescreen = 'ENDOFMONTH'" +
					"     and eva_id = a.ec_eventrunid and eva_id = @NewBOMId) AS FirstTotal," +
					"   (select count(*) from eaeventrun,eaeventcreatefile a,eaeventrunbase64 b" +
					"   where eva_sourcescreen = 'ENDOFMONTH'" +
					"     and eva_id = a.ec_eventrunid" +
					"     and a.ec_id = b.ec_id and eva_id = @NewBOMId) AS SecondTotal" +
					" FROM EACompany WHERE I_CO_CompanyClientId = @CompanyId";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("NewBOMId", NewBOMId));
				Params.Add(new FbParameterItem("CompanyId", CommonItem.CompanyIdValue));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
	
				DataTable tbl = null;
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							FbConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					object Obj = null;
					int Total = 0;
					foreach (DataRow Row in tbl.Rows)
					{
						Obj = Row["FirstTotal"];
						Total = Total + ((Obj == DBNull.Value) ? 0 : Convert.ToInt32(Obj));
						Obj = Row["SecondTotal"];
						Total = Total - ((Obj == DBNull.Value) ? 0 : Convert.ToInt32(Obj));
					}
					
					if (Total != 0) Result = "The total in " + Method + " should be 0 but returned " + Convert.ToString(Total);
				}
				
			}
			catch (Exception Ex)
			{
				Result = LogOnException(Method, Ex, "");
			}
			
			return Result;
		}
		
		private string UpdateEOMInProgressStatus()
		{
			string Result = "";
			string Method = "UpdateEOMInProgressStatus";
			
			try 
			{
				string QueryText = "UPDATE EACompany SET B_CO_ENDOFMONTHINPROGRESS = 'N' WHERE I_CO_COMPANYCLIENTID = @CompanyId";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CommonItem.CompanyIdValue));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(FbConnectionString, QueryText, Params, 0);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					QueryText = 
						" UPDATE EndOfMonthEvent SET B_ENDOFMONTHINPROGRESS = 'N'" +
						" WHERE I_ENDOFMONTH_COMPANYCLIENTID = @CompanyId AND B_ENDOFMONTHINPROGRESS = 'Y'";
					Params.Clear();
					Params.Add(new FbParameterItem("CompanyId", CommonItem.CompanyIdValue));
					Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(FbConnectionString, QueryText, Params, 0);
				}
			}
			catch (Exception Ex)
			{
				Result = LogOnException(Method, Ex, "");
			}
			
			return Result;
		}
		
		/*
		private string ProcessTestRPOCommandLine(string CommandLine,
		                                         string TestRPOLocation)
		{
			string Result = "";
			string CmdBase = TestRPOLocation;
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
				const int tries = 1200; // 20 minutes
				for (int Index = 0; Index < tries; Index++)
				{
					if (!runApp.HasExited) System.Threading.Thread.Sleep(1000);
				}
				if (!runApp.HasExited)
				{
					Result = "Application did not finish in time.";
				}
				
				WindowsEventLoggingUtilities.WriteToInfoEvent("After calling: " + AppName + " Result: " + Result, FModuleName, MasterId);
			}
			return Result;
		}
		*/
		
		private string BuildEOMInvalidList()
		{			
			string Result = "";
			string Method = "BuildEOMInvalidList";
			
			try 
			{
				string QueryText =
					" SELECT" +
					"   BANKID AS ZL5_Bank_ID," +
					"   BANKNAME as ZL30_Bank_Account," +
					"   CASHBOK as ZL10_CashBook," +
					"   BFOK   as ZL5_Holdover," +
					"   DEPOK  as ZL5_Deposits," +
					"   PAYOK  as ZL10_Payments," +
					"   JNLOK  as ZL5_Journals," +
					"   RESOK  as ZL15_Residential," +
					"   COMOK  as ZL15_Commercial," +
					"   STROK  as ZL5_Strata," +
					"   BONOK  as ZL5_Bond," +
					"   SALOK  as ZL5_Sales," +
					"   CREOK  as ZL5_Creditor," +
					"   MANOK  as ZL15_Management," +
					"   PADOK  as ZL5_Paid" +
					" FROM ENDOFMONTHVALIDATE (@CompanyId) where HAVEALOOK='Y'";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CommonItem.CompanyIdValue));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
	
				DataTable tbl = null;
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							FbConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
				}
				
				StringBuilder sb = new StringBuilder();
				if (string.IsNullOrEmpty(Result))
				{
					Result = CommonUtilities.JsonUtilities.BuildJsonArrayFromDataTable(tbl, "EOMInvalidList", ref sb);
				}
				
				if (!string.IsNullOrEmpty(Result))
				{
					Result = LogOnProcessingError(Method, Result, "");
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = 
						JsonUtilities.StartJson() +
						sb.ToString() +
						JsonUtilities.EndJson();
				}
			}
			catch (Exception Ex)
			{
				Result = LogOnException(Method, Ex, "");
			}
			
			return Result;
		}
	}
}
