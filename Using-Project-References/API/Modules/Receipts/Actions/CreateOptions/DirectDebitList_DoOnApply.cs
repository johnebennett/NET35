/*
 * Created by SharpDevelop.
 * User: John
 * Date: 27/04/2020
 * Time: 6:38 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Diagnostics;

using Newtonsoft.Json.Linq;
using CommonUtilities;
using EAOfficeAPI;
namespace Actions.Receipts.CreateOptions
{
	/// <summary>
	/// Description of DirectDebitList_DoOnApply.
	/// </summary>
	public class DirectDebitList_DoOnApply : ActionBase
	{
		private string FModuleName = "Receipts";
		private CreateDirectDebitListRequestItem FRequestItem;		
		
		public static string Execute(int MasterId,
									 APISettingsItem SettingsItem,
									 string Request,
									 out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			DirectDebitList_DoOnApply inst = new DirectDebitList_DoOnApply();
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
				FRequestItem = new CreateDirectDebitListRequestItem();
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
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " + Method, FModuleName, MasterId);
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
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " + Method, FModuleName, MasterId);
			LogOnInsideMethod(Method);
			try 
			{
				StringBuilder Entries = new StringBuilder();				
				ErrorMsg = 
					CommonMethods.EADirectDebitOKToCreate(
						FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue, 
						FRequestItem.MasterEventId, FRequestItem.DirectDebitId, ref Entries);
				
				string Status = "";
				//string DirectDebitSql = "";
				string UpdateSql = "";
				string SaveSql1 = "";
				string SaveSql2 = "";
				string Path = "";
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					if (string.IsNullOrEmpty(ErrorMsg))
					{
						Path = "$.Status";
						Status = JsonUtilities.GetJsonPathValueStr(Entries.ToString(), Path, out ErrorMsg);
						if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(Status))
							ErrorMsg = "Cannot find the Status value";
						if (string.IsNullOrEmpty(ErrorMsg) && (Status != "OK")) ErrorMsg = Status;
						WindowsEventLoggingUtilities.WriteToInfoEvent("Status: " + Status, Method, 0);
					}
					
					/*
					if (string.IsNullOrEmpty(ErrorMsg))
					{
						Path = "$.DirectDebitSql";
						DirectDebitSql = JsonUtilities.GetJsonPathValueStr(Entries.ToString(), Path, out ErrorMsg);
						if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(DirectDebitSql))
							ErrorMsg = "Cannot find the DirectDebitSql value";
						WindowsEventLoggingUtilities.WriteToInfoEvent("DirectDebitSql: " + DirectDebitSql, Method, 0);
						if (string.IsNullOrEmpty(ErrorMsg)) ErrorMsg = ProcessDirectDebitSql(DirectDebitSql);
					}
					*/
					
					if (string.IsNullOrEmpty(ErrorMsg))
					{
						Path = "$.UpdateSQL";
						UpdateSql = JsonUtilities.GetJsonPathValueStr(Entries.ToString(), Path, out ErrorMsg);
						WindowsEventLoggingUtilities.WriteToInfoEvent("UpdateSQL: " + UpdateSql, Method, 0);
						if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(UpdateSql))
							ErrorMsg = "Cannot find the UpdateSql value";
						if (string.IsNullOrEmpty(ErrorMsg)) ErrorMsg = ProcessUpdateSql(UpdateSql);
					}

                    string ShowNextReceiptNumber = "";
                    if (string.IsNullOrEmpty(ErrorMsg))
                    {
                        Path = "$.ShowNextReceiptNumber";
                        ShowNextReceiptNumber = JsonUtilities.GetJsonPathValueStr(Entries.ToString(), Path, out ErrorMsg);
                        WindowsEventLoggingUtilities.WriteToInfoEvent("ShowNextReceiptNumber: " + ShowNextReceiptNumber, Method, 0);
                        if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(ShowNextReceiptNumber))
                            ErrorMsg = "Cannot find the ShowNextReceptNumber value";
                        if (string.IsNullOrEmpty(ErrorMsg) && (ShowNextReceiptNumber == "Y"))
                        {
                            Path = "$.UpdateSQL2";
                            UpdateSql = JsonUtilities.GetJsonPathValueStr(Entries.ToString(), Path, out ErrorMsg);
                            WindowsEventLoggingUtilities.WriteToInfoEvent("UpdateSQL2: " + UpdateSql, Method, 0);
                            if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(UpdateSql))
                                ErrorMsg = "Cannot find the second UpdateSql value";
                            if (string.IsNullOrEmpty(ErrorMsg)) ErrorMsg = ProcessUpdateSql2(UpdateSql);
                        }
                    }

                    if (string.IsNullOrEmpty(ErrorMsg))
					{
						Path = "$.SaveSQL1";
						SaveSql1 = JsonUtilities.GetJsonPathValueStr(Entries.ToString(), Path, out ErrorMsg);
						WindowsEventLoggingUtilities.WriteToInfoEvent("SaveSql1: " + SaveSql1, Method, 0);
						if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(SaveSql1))
							ErrorMsg = "Cannot find the SaveSQL1 value";
						if (string.IsNullOrEmpty(ErrorMsg)) ErrorMsg = ProcessSaveSql1(SaveSql1);
					}
					
					if (string.IsNullOrEmpty(ErrorMsg))
					{
						Path = "$.SaveSQL2";
						SaveSql2 = JsonUtilities.GetJsonPathValueStr(Entries.ToString(), Path, out ErrorMsg);
						WindowsEventLoggingUtilities.WriteToInfoEvent("SaveSql2: " + SaveSql2, Method, 0);
						if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(SaveSql2))
							ErrorMsg = "Cannot find the SaveSql2 value";
						if (string.IsNullOrEmpty(ErrorMsg)) ErrorMsg = ProcessSaveSql2(SaveSql2);
					}
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Result = LogResponse(Method, "");
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
		
		/*
		private string ProcessDirectDebitSql(string DirectDebitSql)
		{
			string Result = "";
			string Method = "ProcessDirectDebitSql";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, FModuleName, MasterId);
					
			LogOnInsideMethod(Method);
			try 
			{
				string QueryText = DirectDebitSql.Replace("@", "'");
				
				if (string.IsNullOrEmpty(Result))
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent("Before ProcessDirectDebitSql", Method, 0);
					Result = FirebirdUtilities.DBExecute(FbConnectionString, QueryText);
					WindowsEventLoggingUtilities.WriteToInfoEvent("After ProcessDirectDebitSql", Method, 0);
					WindowsEventLoggingUtilities.WriteToInfoEvent("ProcessDirectDebitSql Result: " + Result, Method, 0);
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
		*/

        private string ProcessUpdateSql(string UpdateSql)
		{
			string Result = "";
			string Method = "ProcessUpdateSql";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, FModuleName, MasterId);
					
			LogOnInsideMethod(Method);
			try 
			{
				Result = 
					CommonMethods.DirectDebitList_ProcessUpdateSql(
						UpdateSql, FbConnectionString, FRequestItem, CommonItem.ClientIdValue);
				/*
				string QueryText = UpdateSql.Replace("@", "'");
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, null, Method, 0);
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("ManagerId", FRequestItem.ManagerId));
				Params.Add(new FbParameterItem("BranchId", FRequestItem.BranchId));
				Params.Add(new FbParameterItem("LoginId", CommonItem.ClientIdValue));
				Params.Add(new FbParameterItem("BankId", FRequestItem.BankNameId));
				Params.Add(new FbParameterItem("MultiBankFile", FRequestItem.MultiBankFile));
				Params.Add(new FbParameterItem("TransactionTypeId", FRequestItem.TransactionType));
				Params.Add(new FbParameterItem("DebitRunCode1", FRequestItem.DebitRunCode1));
				Params.Add(new FbParameterItem("DebitRunCode2", FRequestItem.DebitRunCode2));
				Params.Add(new FbParameterItem("DebitRunCode3", FRequestItem.DebitRunCode3));
				Params.Add(new FbParameterItem("DebitCollection", FRequestItem.DebitTypeId));
				Params.Add(new FbParameterItem("ClearanceDays", FRequestItem.ClearanceDays));
				Params.Add(new FbParameterItem("DueFrom", FRequestItem.DueFrom));
				Params.Add(new FbParameterItem("DueTo", FRequestItem.DueTo));
				Params.Add(new FbParameterItem("ExcludeVacating", FRequestItem.ExcludeVacating));
				Params.Add(new FbParameterItem("VacateFrom", FRequestItem.VacatingFrom));
				Params.Add(new FbParameterItem("VacateTo", FRequestItem.VacatingTo));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(FbConnectionString, QueryText, Params, 0);
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

        private string ProcessUpdateSql2(string UpdateSql)
        {
            string Result = "";
            string Method = "ProcessUpdateSql2";
            WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, FModuleName, MasterId);

            LogOnInsideMethod(Method);
            try
            {
                Result =
                    CommonMethods.DirectDebitList_ProcessUpdateSql2(
                        UpdateSql, FbConnectionString, FRequestItem, CommonItem.ClientIdValue);
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

        private string ProcessSaveSql1(string SaveSql1)
		{
			string Result = "";
			string Method = "ProcessSaveSql1";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, FModuleName, MasterId);
					
			LogOnInsideMethod(Method);
			try 
			{
				string QueryText = SaveSql1;
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(FbConnectionString, QueryText);
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
		
		private string ProcessSaveSql2(string SaveSql2)
		{
			string Result = "";
			string Method = "ProcessSaveSql2";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, FModuleName, MasterId);
					
			LogOnInsideMethod(Method);
			try 
			{
				string QueryText = SaveSql2;
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(FbConnectionString, QueryText);
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
