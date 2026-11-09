/*
 * Created by SharpDevelop.
 * User: John
 * Date: 10/06/2020
 * Time: 9:53 AM
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
namespace Actions.Portfolio.KeyNumber
{
	/// <summary>
	/// Description of RemoveKey.
	/// </summary>
	public class RemoveKey : ActionBase
	{
		private string FModuleName = "Portfolio";
		//private UpdateKeysRequestItem FRequestItem;
		private SetupPageEntriesRequestItem FRequestItem;
		
		public static string Execute(int MasterId,
		                             APISettingsItem SettingsItem,
		                             string Request,
		                             out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			RemoveKey inst = new RemoveKey();
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
				//FRequestItem = new UpdateKeysRequestItem();
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
						CommonMethods.EATransactionKeys(
							FbConnectionString, CommonItem.CompanyIdValue, FRequestItem.PortfolioId, 
                            FRequestItem.KeyNumber, FRequestItem.KeyNumber2, FRequestItem.KeyNumber3, ref Entries);
				}
				
				string Source = 
						JsonUtilities.StartJson() +
						Entries.ToString() +
						JsonUtilities.EndJson();
				string Path = "";
				string RemoveSql1 = "";
				string RemoveSql2 = "";
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Path = "$.RemoveSQL1";
					RemoveSql1 = JsonUtilities.GetJsonPathValueStr(Source, Path, out ErrorMsg);
					if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(RemoveSql1)) ErrorMsg = "Cannot find the first query item";
					if (string.IsNullOrEmpty(ErrorMsg))
					{
						Path = "$.RemoveSQL2";
						RemoveSql2 = JsonUtilities.GetJsonPathValueStr(Source, Path, out ErrorMsg);
						if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(RemoveSql2)) ErrorMsg = "Cannot find the second query item";
					}
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					RemoveSql1 = RemoveSql1.Replace("__", "'");
					ErrorMsg = FirebirdUtilities.DBExecute(FbConnectionString, RemoveSql1);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					RemoveSql2 = RemoveSql2.Replace("__", "'");
					ErrorMsg = FirebirdUtilities.DBExecute(FbConnectionString, RemoveSql2);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Entries.Length = 0;
					ErrorMsg =
						CommonMethods.EATransactionKeys(
							FbConnectionString, CommonItem.CompanyIdValue, FRequestItem.PortfolioId, 
                            FRequestItem.KeyNumber, FRequestItem.KeyNumber2, FRequestItem.KeyNumber3, ref Entries);
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