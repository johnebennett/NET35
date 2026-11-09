/*
 * Created by SharpDevelop.
 * User: John
 * Date: 09/06/2020
 * Time: 2:58 PM
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
namespace Actions.Portfolio.Management
{
	/// <summary>
	/// Description of ChangeActiveStatus.
	/// </summary>
	public class ChangeActiveStatus : ActionBase
	{
		private string FModuleName = "Portfolio";
		private ManagementPageRequestItem FRequestItem;
		
		public static string Execute(int MasterId,
		                             APISettingsItem SettingsItem,
		                             string Request,
		                             out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			ChangeActiveStatus inst = new ChangeActiveStatus();
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
				FRequestItem = new ManagementPageRequestItem();
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
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg =
						CommonMethods.SaveManagementEntries(
							FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue, FRequestItem);
				}
				
				StringBuilder Entries = new StringBuilder();				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg =
						Actions.Portfolio.CommonMethods.EAResComSalesGroup(
							FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue, 
							FRequestItem.PortfolioId, FRequestItem.NotesId, FRequestItem.Option1Id, FRequestItem.Option2Id, ref Entries);
				}
				
				string InActiveMessage = "";
				string InActiveSql = "";
				string Source = Entries.ToString();
				string Path = "";
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Path = "$.InActiveMessage";
					InActiveMessage = JsonUtilities.GetJsonPathValueStr(Source, Path, out ErrorMsg);
					//if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(InActiveMessage)) ErrorMsg = "Cannot find the Message entry";
					if (string.IsNullOrEmpty(ErrorMsg))
					{
						Path = "$.InActiveSQL";
						InActiveSql = JsonUtilities.GetJsonPathValueStr(Source, Path, out ErrorMsg);
						//if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(InActiveSql)) ErrorMsg = "Cannot find the Query entry";						
					}
				}
				
				if (string.IsNullOrEmpty(ErrorMsg) && !string.IsNullOrEmpty(InActiveMessage)) ErrorMsg = InActiveMessage;
				if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(InActiveSql)) ErrorMsg = "Cannot find the Query entry";
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					InActiveSql = InActiveSql.Replace("__", "'");
					ErrorMsg = FirebirdUtilities.DBExecute(FbConnectionString, InActiveSql);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Entries.Length = 0;
					ErrorMsg =
						Actions.Portfolio.CommonMethods.EAResComSalesGroup(
							FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue, 
							FRequestItem.PortfolioId, FRequestItem.NotesId, FRequestItem.Option1Id, FRequestItem.Option2Id, ref Entries);
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