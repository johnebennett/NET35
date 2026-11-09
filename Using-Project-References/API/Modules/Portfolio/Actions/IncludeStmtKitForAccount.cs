/*
 * Created by SharpDevelop.
 * User: John
 * Date: 19/02/2020
 * Time: 7:41 PM
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
	/// Description of IncludeStmtKitForAccount.
	/// </summary>
	public class IncludeStmtKitForAccount : ActionBase
	{
		private string FModuleName = "Portfolio";
		private  IncludeStmtKitForAccountRequestItem FRequestItem;
		
		public static string Execute(int MasterId,
									 APISettingsItem SettingsItem,
									 string Request,
									 out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			IncludeStmtKitForAccount inst = new IncludeStmtKitForAccount();
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
				UsefulDBMethods.Helpers.RunEAMainOfficeProcess(inst.FbConnectionString, inst.CommonItem.ClientIdValue);
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
				FRequestItem = new IncludeStmtKitForAccountRequestItem();
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
			string QueryText = 
				"UPDATE EALEDGER SET B_ALE_INCLUDESTATEMENTKIT= @Include WHERE I_ALE_COMPANYCLIENTID  = @CompanyId AND I_ALE_ACCOUNTSID = @ACCOUNTID";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("Include", FRequestItem.IsChecked));
			Params.Add(new FbParameterItem("CompanyId", CommonItem.CompanyIdValue));
			Params.Add(new FbParameterItem("ACCOUNTID", FRequestItem.AccountId));
			Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
			
			if (string.IsNullOrEmpty(Result))
			{
				Result = FirebirdUtilities.DBExecute(FbConnectionString, QueryText, Params, 0);
			}
			/*
			string ReportSource = GetReportSource(out ErrorMsg);
			if ((string.IsNullOrEmpty(ErrorMsg)) && (string.IsNullOrEmpty(ReportSource)))
			{
				ErrorMsg = "Missing or Empty ReportSource";
			}
			
			if (string.IsNullOrEmpty(ErrorMsg))
			{			
				Int64 NewId =
					global::UsefulDBMethods.Reporting.SystemReportUtilities.Execute(
						FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue, FRequestItem.PassedId, ReportSource, 
						out ErrorMsg, out OutputFileName);
			}
			*/
			
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