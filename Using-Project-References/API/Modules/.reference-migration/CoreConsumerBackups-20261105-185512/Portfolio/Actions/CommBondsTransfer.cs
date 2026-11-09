/*
 * Created by SharpDevelop.
 * User: John
 * Date: 12/03/2020
 * Time: 7:00 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.ComponentModel;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Serialization;
using UsefulDBMethods;

namespace Actions.Portfolio
{
	/// <summary>
	/// Description of CommBondsTransfer.
	/// </summary>
	public class CommBondsTransfer : ActionBase
	{
		private string FModuleName = "Portfolio";
		private  CommBondsTransferRequestItem FRequestItem;
		
		public static string Execute(int MasterId,
									 APISettingsItem SettingsItem,
									 string Request,
									 out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			CommBondsTransfer inst = new CommBondsTransfer();
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
				FRequestItem = new CommBondsTransferRequestItem();
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
			LogOnInsideMethod(Method);
			try 
			{
				Result = BuildOutput(out ErrorMsg);
				if (!string.IsNullOrEmpty(ErrorMsg))
					WindowsEventLoggingUtilities.WriteToErrorEvent(ErrorMsg, Method, MasterId);
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
		
		private string BuildOutput(out string ErrorMsg)
		{
			ErrorMsg = "";
			string Method = "BuildOutput";
			string Result = "";
			int NewId = 
				UsefulDBMethods.GeneratorHelpers.GetNextGeneratorId(
					FbConnectionString, "I_EPM_ID_gen", CommonItem.CompanyIdValue, CommonItem.ClientIdValue, out ErrorMsg);
			if (string.IsNullOrEmpty(ErrorMsg))
			{
				ErrorMsg =
					CommonMethods.Portfolio_CommBondsOnTransferCmd(
						FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue,
						FRequestItem.PortfolioId, FRequestItem.SubLedgerId, FRequestItem.Details, FRequestItem.LodgedAmount,
						FRequestItem.LodgedOn, FRequestItem.ExpiryDate, FRequestItem.DoesNotExpire, FRequestItem.BondHeld,
						FRequestItem.BSB, FRequestItem.AccountNumber, FRequestItem.ShortBankName, NewId);
			}
			
			int StatementRunId = 0;
			if (string.IsNullOrEmpty(ErrorMsg))
			{
				StatementRunId = GetStatementRunId(NewId, out ErrorMsg);
			}
			
			int EventRunId = 0;
			if (string.IsNullOrEmpty(ErrorMsg))
			{
				EventRunId =
					UsefulDBMethods.GeneratorHelpers.GetNextGeneratorId(
						FbConnectionString, "EVA_ID_GEN", (int)CommonItem.CompanyIdValue, (int)CommonItem.ClientIdValue, out ErrorMsg);
			}
			
			if (string.IsNullOrEmpty(ErrorMsg))
			{
				ErrorMsg = ExecuteStatementPayFileProcedure(StatementRunId, EventRunId);
			}
			
			string commandLine = "";
			if (string.IsNullOrEmpty(ErrorMsg))
			{
				commandLine = UsefulDBMethods.EventHelpers.GetEventCommandLineForRunId(FbConnectionString, EventRunId, out ErrorMsg);
			}
			
			string TestRPOLocation = "";
			if (string.IsNullOrEmpty(ErrorMsg))
			{
				TestRPOLocation = CommonUtilities.FolderHelpers.TestRPOLocation(FbConnectionString, CommonItem.CompanyIdValue, out ErrorMsg);
			}
			
			string DocsLocation = "";
			if (string.IsNullOrEmpty(ErrorMsg))
			{
				DocsLocation = CommonUtilities.FolderHelpers.DocsLocation(FbConnectionString, CommonItem.CompanyIdValue, out ErrorMsg);
			}

			StringBuilder Entries = new StringBuilder();			
			if (string.IsNullOrEmpty(ErrorMsg))
			{
				// run the command line etc
				//ErrorMsg = CommonMethods.ProcessTestRPOCommandLine(commandLine, TestRPOLocation, 0);
				ErrorMsg = UsefulDBMethods.Helpers.ProcessTestRPOCommandLine(commandLine, TestRPOLocation, 0);
				// extract the base64 field and put the file in the docs location
				// open / download the output file
				try
				{
					Entries.Append(JsonUtilities.StartJsonArray("OutputFileList"));
					ReportUtilities repUtils = new ReportUtilities();
					repUtils.EAEventRunBase64Entries(FbConnectionString, EventRunId, DocsLocation, ref Entries, out ErrorMsg);
				}
				finally
				{
					Entries.Append(JsonUtilities.EndJsonArray());
					Entries.Append(",");
				}
			}
			
			if (string.IsNullOrEmpty(ErrorMsg))
			{
				Entries.Append(JsonUtilities.AddPropertyNameAndValue("PortfolioId", FRequestItem.PortfolioId));
				ErrorMsg =
					Actions.ComboBoxEntries.CommonMethods.BuildOutput_PortfolioCommBondsList(
						FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue, FRequestItem.PortfolioId, FRequestItem.SubLedgerId, ref Entries);
			}
			
			if (string.IsNullOrEmpty(ErrorMsg))
			{
				Result =
					JsonUtilities.StartJson() +
					Entries.ToString() +
					JsonUtilities.EndJson();
			}
			else
			{
				Result = LogOnProcessingError(Method, ErrorMsg, "");
			}
			return Result;
		}
		
		private int GetStatementRunId(int NewRunId, out string ErrorMsg)
		{
			int Result = -99;
			string Method = "GetStatementRunId";
			ErrorMsg = "";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, FModuleName, MasterId);
					
			LogOnInsideMethod(Method);
			try 
			{
				ErrorMsg = UsefulDBMethods.Helpers.RunEAMainOfficeProcess(FbConnectionString, CommonItem.CompanyIdValue);
				string QueryText = "";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					QueryText = "SELECT I_EPM_StatementRunId FROM EAPaymentMaster2 WHERE I_EPM_Id = @NewRunId";
	
					Params.Add(new FbParameterItem("NewRunId", NewRunId));
					//Params.Add(new FbParameterItem("CompanyId", CommonItem.CompanyIdValue));
					ErrorMsg = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
				}
				
				DataTable tbl = null;
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							FbConnectionString, QueryText, Params, 0, FirebirdQueryCheckTypes.HasTables, out ErrorMsg);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					object obj = null;
					string StrValue = "";
					WindowsEventLoggingUtilities.WriteToInfoEvent("Table RowCount: " + tbl.Rows.Count, FModuleName, MasterId);
					foreach (DataRow row in tbl.Rows)
					{
						/*
						foreach(DataColumn col in tbl.Columns)
						{
							ColName = col.ColumnName;
							ColType = col.DataType.ToString();
							ColValue = Convert.ToString(row[ColName]);
							WindowsEventLoggingUtilities.WriteToInfoEvent(ColName + " "  + ColType + ": " + ColValue, FModuleName, MasterId);
						}
						*/
						obj = row[0];
						WindowsEventLoggingUtilities.WriteToInfoEvent("StatementRunId Object: " + obj, FModuleName, MasterId);
						StrValue = (obj == DBNull.Value) ? "IS NULL" : Convert.ToString(obj);
						WindowsEventLoggingUtilities.WriteToInfoEvent("StatementRunId: " + StrValue, FModuleName, MasterId);
						Int32 IntValue = (obj == DBNull.Value) ? -1 : Convert.ToInt32(obj);
						Result = (int)IntValue;
					}
				}
				
				if ((string.IsNullOrEmpty(ErrorMsg)) && (Result <= 0))
				{
					ErrorMsg = "Result: " + Result + Environment.NewLine + "Could not determine the StatementRunId value";
					WindowsEventLoggingUtilities.WriteToInfoEvent("ErrorMsg:" + Environment.NewLine + ErrorMsg, FModuleName, MasterId);
				}
				
			}
			catch (Exception Ex)
			{
				ErrorMsg = LogOnException(Method, Ex, "");
			}
			finally
			{
				LogOnLeavingMethod(Method);
			}
			
			return Result;
		}
		
		private string ExecuteStatementPayFileProcedure(int StatementRunId,
		                                                int EventRunId)
		{
			string Result = "";
			string Method = "ExecuteStatementPayFileProcedure";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, FModuleName, MasterId);
					
			LogOnInsideMethod(Method);
			try 
			{
				string QueryText =
					"execute procedure EASTATEMENTPAYFILE(@COMPANYID, @RUNID, @WHOBYID, @EVENTRUNID)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("COMPANYID", CommonItem.CompanyIdValue));
				Params.Add(new FbParameterItem("RUNID", StatementRunId));
				Params.Add(new FbParameterItem("WHOBYID", CommonItem.ClientIdValue));
				Params.Add(new FbParameterItem("EVENTRUNID", EventRunId));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(FbConnectionString, QueryText, Params, 0);
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