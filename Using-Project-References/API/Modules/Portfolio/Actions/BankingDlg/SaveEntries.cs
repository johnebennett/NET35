/*
 * Created by SharpDevelop.
 * User: John
 * Date: 11/06/2020
 * Time: 6:40 AM
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
namespace Actions.Portfolio.BankingDlg
{
	/// <summary>
	/// Description of SaveEntries.
	/// </summary>
	public class SaveEntries : ActionBase
	{
		private string FModuleName = "Portfolio";
		private SaveEntriesRequestItem FRequestItem;
		
		public static string Execute(int MasterId,
		                             APISettingsItem SettingsItem,
		                             string Request,
		                             out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			SaveEntries inst = new SaveEntries();
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
				FRequestItem = new SaveEntriesRequestItem();
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
				StringBuilder Entries = new StringBuilder();
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg =
						CommonMethods.EATransactionBankingInfo(
							FbConnectionString, CommonItem.CompanyIdValue, FRequestItem.PortfolioId, ref Entries);
				}
				
				string Source = 
						JsonUtilities.StartJson() +
						Entries.ToString() +
						JsonUtilities.EndJson();
				string Path = "";
				string UpdateSql1 = "";
				string UpdateSql2 = "";
                string UpdateSql3 = "";
                string UpdateSql4 = "";
                string UpdateSql5 = "";
                if (string.IsNullOrEmpty(ErrorMsg))
				{
					Path = "$.UpdateSQL1";
					UpdateSql1 = JsonUtilities.GetJsonPathValueStr(Source, Path, out ErrorMsg);
					if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(UpdateSql1)) ErrorMsg = "Cannot find the first query item";
					if (string.IsNullOrEmpty(ErrorMsg))
					{
						Path = "$.UpdateSQL2";
						UpdateSql2 = JsonUtilities.GetJsonPathValueStr(Source, Path, out ErrorMsg);
						if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(UpdateSql2)) ErrorMsg = "Cannot find the second query item";
					}
                    if (string.IsNullOrEmpty(ErrorMsg))
                    {
                        Path = "$.UpdateSQL3";
                        UpdateSql3 = JsonUtilities.GetJsonPathValueStr(Source, Path, out ErrorMsg);
                        if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(UpdateSql3)) ErrorMsg = "Cannot find the third query item";
                    }
                    if (string.IsNullOrEmpty(ErrorMsg))
                    {
                        Path = "$.UpdateSQL4";
                        UpdateSql4 = JsonUtilities.GetJsonPathValueStr(Source, Path, out ErrorMsg);
                        if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(UpdateSql4)) ErrorMsg = "Cannot find the fourth query item";
                    }
                    if (string.IsNullOrEmpty(ErrorMsg))
                    {
                        Path = "$.UpdateSQL5";
                        UpdateSql5 = JsonUtilities.GetJsonPathValueStr(Source, Path, out ErrorMsg);
                        if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(UpdateSql5)) ErrorMsg = "Cannot find the fifth query item";
                    }
                }

                List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Params.Clear();
					Params.Add(new FbParameterItem("coma_abn", FRequestItem.TenantABN));
					ErrorMsg = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(UpdateSql1, Params, Method, 0);
					if (string.IsNullOrEmpty(ErrorMsg))
					{
						ErrorMsg = FirebirdUtilities.DBExecute(FbConnectionString, UpdateSql1, Params, 0);
					}
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Params.Clear();
					Params.Add(new FbParameterItem("BSB1", FRequestItem.BSB1));
					Params.Add(new FbParameterItem("BSB2", FRequestItem.BSB2));
					Params.Add(new FbParameterItem("BSB3", FRequestItem.BSB3));
					Params.Add(new FbParameterItem("BSB4", FRequestItem.BSB4));
					Params.Add(new FbParameterItem("AccountNumber1", FRequestItem.Number1));
					Params.Add(new FbParameterItem("AccountNumber2", FRequestItem.Number2));
					Params.Add(new FbParameterItem("AccountNumber3", FRequestItem.Number3));
					Params.Add(new FbParameterItem("AccountNumber4", FRequestItem.Number4));
					Params.Add(new FbParameterItem("AgentPer1", FRequestItem.Percentage1));
					Params.Add(new FbParameterItem("AgentPer2", FRequestItem.Percentage2));
					Params.Add(new FbParameterItem("AgentPer3", FRequestItem.Percentage3));
					Params.Add(new FbParameterItem("AgentPer4", FRequestItem.Percentage4));
					Params.Add(new FbParameterItem("AccountDetails1", FRequestItem.Details1));
					Params.Add(new FbParameterItem("AccountDetails2", FRequestItem.Details2));
					Params.Add(new FbParameterItem("AccountDetails3", FRequestItem.Details3));
					Params.Add(new FbParameterItem("AccountDetails4", FRequestItem.Details4));
                    Params.Add(new FbParameterItem("DEBITRUNCODE", FRequestItem.RunNumber));
                    ErrorMsg = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(UpdateSql2, Params, Method, 0);
					if (string.IsNullOrEmpty(ErrorMsg))
					{
						ErrorMsg = FirebirdUtilities.DBExecute(FbConnectionString, UpdateSql2, Params, 0);
					}
				}

                if (string.IsNullOrEmpty(ErrorMsg))
                {
                    Params.Clear();
                    Params.Add(new FbParameterItem("debtprintrunnumber", FRequestItem.PrintRunNumber));
                    ErrorMsg = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(UpdateSql3, Params, Method, 0);
                    if (string.IsNullOrEmpty(ErrorMsg))
                    {
                        ErrorMsg = FirebirdUtilities.DBExecute(FbConnectionString, UpdateSql3, Params, 0);
                    }
                }

                if (string.IsNullOrEmpty(ErrorMsg))
                {
                    Params.Clear();
                    Params.Add(new FbParameterItem("TenancyOrder", FRequestItem.TenancyOrder));
                    ErrorMsg = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(UpdateSql4, Params, Method, 0);
                    if (string.IsNullOrEmpty(ErrorMsg))
                    {
                        ErrorMsg = FirebirdUtilities.DBExecute(FbConnectionString, UpdateSql4, Params, 0);
                    }
                }

                if (string.IsNullOrEmpty(ErrorMsg))
                {
                    Params.Clear();
                    Params.Add(new FbParameterItem("DirectDebitdefault", FRequestItem.DefaultDebit));
                    ErrorMsg = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(UpdateSql5, Params, Method, 0);
                    if (string.IsNullOrEmpty(ErrorMsg))
                    {
                        ErrorMsg = FirebirdUtilities.DBExecute(FbConnectionString, UpdateSql5, Params, 0);
                    }
                }

                if (string.IsNullOrEmpty(ErrorMsg))
				{
					UsefulDBMethods.Helpers.RunEAMainOfficeProcess(FbConnectionString, CommonItem.CompanyIdValue);
					UsefulDBMethods.Helpers.RunEAMainOfficeProcess(FbConnectionString, CommonItem.CompanyIdValue);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Entries.Length = 0;
					ErrorMsg =
						CommonMethods.EATransactionBankingInfo(
							FbConnectionString, CommonItem.CompanyIdValue, FRequestItem.PortfolioId, ref Entries);
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
	}
}