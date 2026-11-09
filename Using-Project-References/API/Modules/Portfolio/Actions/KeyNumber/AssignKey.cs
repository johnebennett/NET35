/*
 * Created by SharpDevelop.
 * User: John
 * Date: 10/06/2020
 * Time: 9:39 AM
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
	/// Description of AssignKey.
	/// </summary>
	public class AssignKey : ActionBase
	{
		private string FModuleName = "Portfolio";
		private UpdateKeysRequestItem FRequestItem;
		
		public static string Execute(int MasterId,
		                             APISettingsItem SettingsItem,
		                             string Request,
		                             out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			AssignKey inst = new AssignKey();
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
				FRequestItem = new UpdateKeysRequestItem();
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
				string Value = "";
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Path = "$.ValidKeyNumber";
					Value = JsonUtilities.GetJsonPathValueStr(Source, Path, out ErrorMsg);
					if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(Value)) ErrorMsg = "Cannot find the validation item";
					if (string.IsNullOrEmpty(ErrorMsg) && (Value == "N")) ErrorMsg = "The key number provided has been used";
				}
				
				string SaveSql1 = "";
				string SaveSql2 = "";
                string SaveSql3 = "";
                string SaveSql4 = "";
                if (string.IsNullOrEmpty(ErrorMsg))
				{
					Path = "$.SaveSQL1";
					SaveSql1 = JsonUtilities.GetJsonPathValueStr(Source, Path, out ErrorMsg);
					if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(SaveSql1)) ErrorMsg = "Cannot find the first query item";
					if (string.IsNullOrEmpty(ErrorMsg))
					{
						Path = "$.SaveSQL2";
						SaveSql2 = JsonUtilities.GetJsonPathValueStr(Source, Path, out ErrorMsg);
						if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(SaveSql2)) ErrorMsg = "Cannot find the second query item";
					}
                    if (string.IsNullOrEmpty(ErrorMsg))
                    {
                        Path = "$.SaveSQL3";
                        SaveSql3 = JsonUtilities.GetJsonPathValueStr(Source, Path, out ErrorMsg);
                        if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(SaveSql2)) ErrorMsg = "Cannot find the third query item";
                    }
                    if (string.IsNullOrEmpty(ErrorMsg))
                    {
                        Path = "$.SaveSQL4";
                        SaveSql4 = JsonUtilities.GetJsonPathValueStr(Source, Path, out ErrorMsg);
                        if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(SaveSql2)) ErrorMsg = "Cannot find the fourth query item";
                    }
                }

                List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					SaveSql1 = SaveSql1.Replace("__", "'");
					Params.Add(new FbParameterItem("Comment", FRequestItem.Comment));
                    UsefulDBMethods.Helpers.WriteParamsToInfoEvent(SaveSql1, Params, Method, 0);
                    ErrorMsg = FirebirdUtilities.DBExecute(FbConnectionString, SaveSql1, Params, 0);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					SaveSql2 = SaveSql2.Replace("__", "'");
                    WindowsEventLoggingUtilities.WriteToInfoEvent("SaveSql2: " + Environment.NewLine + SaveSql2, FModuleName, MasterId);
                    ErrorMsg = FirebirdUtilities.DBExecute(FbConnectionString, SaveSql2);
				}

                if (string.IsNullOrEmpty(ErrorMsg) && !string.IsNullOrEmpty(SaveSql3))
                {
                    SaveSql3 = SaveSql3.Replace("__", "'");
                    WindowsEventLoggingUtilities.WriteToInfoEvent("SaveSql3: " + Environment.NewLine + SaveSql3, FModuleName, MasterId);
                    ErrorMsg = FirebirdUtilities.DBExecute(FbConnectionString, SaveSql3);
                }


                if (string.IsNullOrEmpty(ErrorMsg) && !string.IsNullOrEmpty(SaveSql4))
                {
                    SaveSql4 = SaveSql4.Replace("__", "'");
                    WindowsEventLoggingUtilities.WriteToInfoEvent("SaveSql4: " + Environment.NewLine + SaveSql4, FModuleName, MasterId);
                    ErrorMsg = FirebirdUtilities.DBExecute(FbConnectionString, SaveSql4);
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