/*
 * Created by SharpDevelop.
 * User: John
 * Date: 03/12/2019
 * Time: 3:49 PM
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
	/// Description of GatherGridEntries.
	/// </summary>
	public class GatherGridEntries : ActionBase
	{
		private string FModuleName = "Ledger";
		private SetupPageEntriesRequestItem FRequestItem;
		
		public static string Execute(int MasterId,
		                             APISettingsItem SettingsItem,
		                             string Request,
		                             out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			GatherGridEntries inst = new GatherGridEntries();
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
				FRequestItem = new SetupPageEntriesRequestItem();
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
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " +Method, FModuleName, MasterId);
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
				
				// Call EALEDGERSCREEN to get the sql that is used to populate the grid
				string SqlToUse = GetSqlToUse(out ErrorMsg);
				WindowsEventLoggingUtilities.WriteToInfoEvent("SqlToUse:" + Environment.NewLine + SqlToUse, Method, 0);
				// Execute the sql to get the grid entries
				StringBuilder entries = new StringBuilder();
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg = BuildOutputEntries(SqlToUse, ref entries);
				}				
					
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
		
		private string GetSqlToUse(out string ErrorMsg)
		{
			string Result = "";
			ErrorMsg = "";
			//string Method = "GetSqlToUse";
			
			Result = 
				CommonMethods.GetLedgerScreenSqlToUse(
					FbConnectionString, CommonItem.CompanyIdValue,
					FRequestItem.BranchId, FRequestItem.ManagerId, FRequestItem.ActiveStatus,
					FRequestItem.LedgerId, FRequestItem.FindText, FRequestItem.RunNumber, FRequestItem.GSTRegistered, out ErrorMsg);
			
			/*
			string QueryText =
				"SELECT SCREENSQL FROM EALEDGERSCREEN(" +
				" @CompanyId, @BranchId, @ManagerId, @ActiveStatus, @LedgerId, @FindText, @RunNumber, @GSTRegistered)";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("CompanyId", CommonItem.CompanyIdValue));
			Params.Add(new FbParameterItem("BranchId", FRequestItem.BranchId));
			Params.Add(new FbParameterItem("ManagerId", FRequestItem.ManagerId));
			Params.Add(new FbParameterItem("ActiveStatus", FRequestItem.ActiveStatus));
			Params.Add(new FbParameterItem("LedgerId", FRequestItem.LedgerId));
			Params.Add(new FbParameterItem("FindText", FRequestItem.FindText));
			Params.Add(new FbParameterItem("RunNumber", FRequestItem.RunNumber));
			Params.Add(new FbParameterItem("GSTRegistered", FRequestItem.GSTRegistered));
			ErrorMsg = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
			
			if (string.IsNullOrEmpty(ErrorMsg))
			{
				try 
				{
					DataTable tbl = 
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							FbConnectionString, QueryText, Params, 0, 
							UsefulDBMethods.FirebirdQueryCheckTypes.HasTablesAndRows, out ErrorMsg);
					if (string.IsNullOrEmpty(ErrorMsg))
					{
						object Obj = tbl.Rows[0]["ScreenSql"];
						Result = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					}
				}
				catch (Exception Ex)
				{
					ErrorMsg = LogOnException(Method, Ex, "");
				}
			}
			*/
			return Result;			
		}
		
		private string BuildOutputEntries(string SqlToUse, ref StringBuilder Entries)
		{
			string Method = "BuildOutputEntries";
			string Result = "";			
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, FModuleName, MasterId);			
			
			LogOnInsideMethod(Method);
			try
			{
				/*
				string QueryText = SqlToUse;
				DataTable tbl = null;
				try 
				{
					tbl = 
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							FbConnectionString, QueryText, null, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
				}
				catch (Exception Ex)
				{
					Result = LogOnException(Method, Ex, "");
					return Result;
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = CommonUtilities.JsonUtilities.BuildJsonArrayFromDataTable(tbl, "gridEntries", ref Entries);
				}
				*/
				Result = CommonMethods.BuildLedgerScreenGridEntries(FbConnectionString, SqlToUse, "gridEntries", ref Entries);
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
