/*
 * Created by SharpDevelop.
 * User: John
 * Date: 11/06/2020
 * Time: 6:23 PM
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
namespace Actions.Portfolio.AgentNoDlg
{
	/// <summary>
	/// Description of SaveEntries.
	/// </summary>
	public class SaveEntries : ActionBase
	{
		private string FModuleName = "Portfolio";
		private SetupPageEntriesRequestItem FRequestItem;
		
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
				StringBuilder Entries = new StringBuilder();
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg =
						CommonMethods.EATransactionAgentNo(
							FbConnectionString, CommonItem.CompanyIdValue, FRequestItem.PortfolioId,
							FRequestItem.AgentNumber1, FRequestItem.AgentNumber2, FRequestItem.AgentNumber3,
							FRequestItem.ValidateAgentNo, ref Entries);
				}
				
				string Source = 
						JsonUtilities.StartJson() +
						Entries.ToString() +
						JsonUtilities.EndJson();
				string Path = "";
				string UpdateSql = "";
				string Value = "";
				bool OKToSave = true;
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					int Index = 1;
					while (OKToSave && string.IsNullOrEmpty(ErrorMsg) && (Index <= 3))
					{
						Path = "$.ValidAgentNumber" + Index.ToString();
						Value = JsonUtilities.GetJsonPathValueStr(Source, Path, out ErrorMsg);
						if (Value == "N") OKToSave = false;
						Index++;
					}
				}
				
				if (string.IsNullOrEmpty(ErrorMsg) && !OKToSave) ErrorMsg = "One or more values are invalid";
				
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Path = "$.UpdateSQL";
					UpdateSql = JsonUtilities.GetJsonPathValueStr(Source, Path, out ErrorMsg);
					if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(UpdateSql)) ErrorMsg = "Cannot find the first query item";
				}
				
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Params.Clear();
					Params.Add(new FbParameterItem("agentnumber1", FRequestItem.AgentNumber1));
					Params.Add(new FbParameterItem("agentnumber2", FRequestItem.AgentNumber2));
					Params.Add(new FbParameterItem("agentnumber3", FRequestItem.AgentNumber3));
					ErrorMsg = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(UpdateSql, Params, Method, 0);
					if (string.IsNullOrEmpty(ErrorMsg))
					{
						ErrorMsg = FirebirdUtilities.DBExecute(FbConnectionString, UpdateSql, Params, 0);
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
						CommonMethods.EATransactionAgentNo(
							FbConnectionString, CommonItem.CompanyIdValue, FRequestItem.PortfolioId,
							FRequestItem.AgentNumber1, FRequestItem.AgentNumber2, FRequestItem.AgentNumber3,
							FRequestItem.ValidateAgentNo, ref Entries);
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