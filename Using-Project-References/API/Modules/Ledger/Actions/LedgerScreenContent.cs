/*
 * Created by SharpDevelop.
 * User: John
 * Date: 05/12/2019
 * Time: 12:30 PM
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
	/// Description of LedgerScreenContent.
	/// </summary>
	public class LedgerScreenContent : ActionBase
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

			LedgerScreenContent inst = new LedgerScreenContent();
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
				
				StringBuilder entries = new StringBuilder();
					
				ErrorMsg = BuildOutputEntries(ref entries);				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Result =
						JsonUtilities.StartJson() +
						entries.ToString() +
						//JsonUtilities.AddPropertyNameAndValue("FileName", @"/Docs/" + OutputFileName, false) +
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
		
		private string BuildOutputEntries(ref StringBuilder Entries)
		{
			string Result = "";
			
			Result = BuildPageItems(ref Entries);
			
			/*
			if (string.IsNullOrEmpty(Result))
			{
				Entries.Append(",");
				Result = BuildGridEntries(ref Entries);
			}
			*/
			
			return Result;
		}
		
		private string BuildPageItems(ref StringBuilder Entries)
		{
			string Method = "BuildOutputEntries";
			string Result = "";			
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, FModuleName, MasterId);
			
			LogOnInsideMethod(Method);
			try
			{
				Result =
					CommonMethods.BuildLedgerScreenGroupEntries(
						FbConnectionString,	CommonItem.CompanyIdValue,
						FRequestItem.BranchId, FRequestItem.ManagerId, FRequestItem.LedgerId, FRequestItem.EventId,
						FRequestItem.ReportId, FRequestItem.ActiveStatus, FRequestItem.RunNumber, CommonItem.ClientIdValue,
						FRequestItem.ScreenOrBulk, FRequestItem.GSTRegistered, FRequestItem.FindText, 
						FRequestItem.DateFrom, FRequestItem.DateTo, FRequestItem.DateShow, FRequestItem.OnlyShowPDFEntreis, ref Entries);
				/*
 
				string QueryText = 
					" SELECT OUTPUTLINE FROM EALEDGERSCREENGROUP(@CompanyId, @BranchId, @ManagerId, @LedgerId, @EventId, @ReportId," +
					" @ActiveStatus, @RunNumber, @AddedById, @ScreenOrBulk, @GSTRegistered, @FindText, @DateFrom, @DateTo, @DateShow)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CommonItem.CompanyIdValue));
				Params.Add(new FbParameterItem("BranchId", FRequestItem.BranchId));
				Params.Add(new FbParameterItem("ManagerId", FRequestItem.ManagerId));
				Params.Add(new FbParameterItem("LedgerId", FRequestItem.LedgerId));
				Params.Add(new FbParameterItem("EventId", FRequestItem.EventIdValue));
				Params.Add(new FbParameterItem("ReportId", FRequestItem.ReportId));
				Params.Add(new FbParameterItem("ActiveStatus", FRequestItem.ActiveStatus));
				Params.Add(new FbParameterItem("RunNumber", FRequestItem.RunNumber));
				Params.Add(new FbParameterItem("AddedById", FRequestItem.AddedById));
				Params.Add(new FbParameterItem("ScreenOrBulk", FRequestItem.ScreenOrBulk));
				Params.Add(new FbParameterItem("GSTRegistered", FRequestItem.GSTRegistered));
				Params.Add(new FbParameterItem("FindText", FRequestItem.FindText));
				Params.Add(new FbParameterItem("DateFrom", FRequestItem.DateFrom));
				Params.Add(new FbParameterItem("DateTo", FRequestItem.DateTo));
				Params.Add(new FbParameterItem("DateShow", FRequestItem.DateShow));
				//WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText:" + Environment.NewLine + QueryText, FModuleName, MasterId);
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);

				DataTable dt = null;
				if (string.IsNullOrEmpty(Result))
				{
					try 
					{
						//dt = FirebirdUtilities.GetFirstDataTable(FbConnectionString, QueryText, Params, 0);
						dt = 
							UsefulDBMethods.Helpers.GetFirstDataTableEx(
								FbConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
					}
					catch (Exception Ex)
					{
						Result = LogOnException(Method, Ex, "");
						return Result;
					}
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					foreach (DataRow dataRow in dt.Rows)
					{
						Entries.AppendLine((dataRow[0] == DBNull.Value) ? "" : Convert.ToString(dataRow[0]));
					}
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
			
			return Result;
		}
		
		private string BuildGridEntries(ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "BuildGridEntries";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, FModuleName, MasterId);
			
			LogOnInsideMethod(Method);
			try
			{
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
