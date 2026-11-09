/*
 * Created by SharpDevelop.
 * User: John
 * Date: 20/02/2020
 * Time: 11:58 AM
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
using UsefulDBMethods;

namespace Actions.Portfolio
{
	/// <summary>
	/// Description of IncludeStmtKitForAccountReport.
	/// </summary>
	public class IncludeStmtKitForAccountReport : ActionBase
	{
		private string FModuleName = "Portfolio";
		private  IncludeStmtKitForAccountReportRequestItem FRequestItem;
		
		public static string Execute(int MasterId,
									 APISettingsItem SettingsItem,
									 string Request,
									 out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			IncludeStmtKitForAccountReport inst = new IncludeStmtKitForAccountReport();
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
				FRequestItem = new IncludeStmtKitForAccountReportRequestItem();
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
					FbConnectionString, "I_ESK_ID_GEN", CommonItem.CompanyIdValue, CommonItem.ClientIdValue, out Result);
			
			if (string.IsNullOrEmpty(Result))
			{
				string QueryText = 
					"INSERT INTO EASTATEMENTKITOPTION(I_ESK_ID, I_ESK_COMPANYCLIENTID, I_ESK_ACCOUNTID, I_ESK_OPTION, I_ESK_ADDEDBYID) " +
					"VALUES(@NewId, @CompanyId, @AccountId, @OptionId, @ClientId)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("NewId", NewId));
				Params.Add(new FbParameterItem("CompanyId", CommonItem.CompanyIdValue));
				Params.Add(new FbParameterItem("ACCOUNTID", FRequestItem.AccountId));
				Params.Add(new FbParameterItem("OptionId", FRequestItem.ReportId));
				Params.Add(new FbParameterItem("ClientId", CommonItem.ClientIdValue));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(FbConnectionString, QueryText, Params, 0);
				}
			}
			
			if (string.IsNullOrEmpty(Result))
			{
				Result =
					JsonUtilities.StartJson() +
					JsonUtilities.AddPropertyNameAndValue("Status", "Success", false) +
					JsonUtilities.EndJson();
			}
			else
			{
				Result = LogOnProcessingError(Method, Result, "");
			}
			
			return Result;
		}
	}
}