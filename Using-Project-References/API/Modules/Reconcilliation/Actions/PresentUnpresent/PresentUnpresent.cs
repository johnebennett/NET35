/*
 * Created by SharpDevelop.
 * User: John
 * Date: 24/10/2019
 * Time: 7:06 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;

/*
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
*/

using CommonUtilities;
using EAOfficeAPI;

namespace Actions.Reconcilliation.PresentUnpresent
{
	/// <summary>
	/// Description of PresentUnpresent.
	/// </summary>
	public class PresentUnpresent : ActionBase
	{
		private string FModuleName = "Reconcilliation";
		private PresentUnpresentRequestItem FRequestItem;
		
		public static string Execute(int MasterId,
									 APISettingsItem SettingsItem,
									 string Request,
									 out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			PresentUnpresent inst = new PresentUnpresent();
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
				FRequestItem = new PresentUnpresentRequestItem();
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
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " +Method, FModuleName, MasterId);
			LogOnInsideMethod(Method);
			try 
			{
				// GetReturnStirng
				// Generate NewId for GEN_ID(I_EBKP_ID_GEN,1)
				// Insert EABANKPRESENT record
				int NewId = 0;
				string ReturnString = GetReturnString(out Result);
				WindowsEventLoggingUtilities.WriteToInfoEvent("ReturnString: " + ReturnString, Method, 0);
				if (string.IsNullOrEmpty(Result))
				{
					NewId = 
						UsefulDBMethods.GeneratorHelpers.GetNextGeneratorId(
							FbConnectionString, "I_EBKP_ID_GEN", CommonItem.CompanyIdValue, CommonItem.ClientIdValue, out Result);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = InsertPresentUnpresentRecord(NewId, ReturnString, FRequestItem.PageNumber);
				}
				
				if (!string.IsNullOrEmpty(Result))
				{
					Result = LogOnProcessingError(Method, Result, "");
				}
				else
				{
					Result = LogResponse(Method, "");
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
		
		private string GetReturnString(out string ErrorMsg)
		{
			string Result = "";
			ErrorMsg = "";
			string Method = "GetReturnString";
			
			try 
			{
				string QueryText =
					" SELECT ReturnString FROM EABankEntries(@CompanyId, @BankType, @BankId, 0)" +
					" WHERE ID = @RecordId";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CommonItem.CompanyIdValue));
				Params.Add(new FbParameterItem("BankType", FRequestItem.BankType));
				Params.Add(new FbParameterItem("BankId", FRequestItem.BankId));
				Params.Add(new FbParameterItem("RecordId", FRequestItem.RecordId));
				ErrorMsg = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
				
				DataTable tbl = null;
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							FbConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTablesAndRows, out ErrorMsg);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					DataRow Row = tbl.Rows[0];
					Object Obj = Row["ReturnString"];
					Result = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					if (string.IsNullOrEmpty(Result))
						ErrorMsg = "No ReturnString for the selected record!";
				}
			}
			catch (Exception Ex)
			{
				ErrorMsg =
					Method + Environment.NewLine +
					Ex.Message + Environment.NewLine +
					Ex.StackTrace;
			}
			
			return Result;
		}
		
		private string InsertPresentUnpresentRecord(Int64 NewId,
		                                            string ReturnString,
		                                            int PageNumber)
		{
			string Result = "";
			string Method = "InsertPresentRecord";
			LogOnInsideMethod(Method);
			try 
			{
				string QueryText =
					" INSERT INTO EABankPresent(I_EBKP_ID, EBKP_RETURNSTRING, I_EBKP_ADDEDBYID, I_EBKP_PAGENO)" +
                    " VALUES(@NewId, @ReturnString, @WhoById, @PageNumber)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("NewId", NewId));
				Params.Add(new FbParameterItem("ReturnString", ReturnString));
				Params.Add(new FbParameterItem("WhoById", CommonItem.ClientIdValue));
				Params.Add(new FbParameterItem("PageNumber", PageNumber));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
				
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(FbConnectionString, QueryText, Params, 0);
				}
				
			}
			catch (Exception Ex)
			{
				Result =
					Method + Environment.NewLine +
					Ex.Message + Environment.NewLine +
					Ex.StackTrace;
			}

			return Result;
		}
	}
}
