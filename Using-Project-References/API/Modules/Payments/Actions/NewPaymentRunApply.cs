/*
 * Created by SharpDevelop.
 * User: John
 * Date: 23/07/2019
 * Time: 03:24 PM
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

namespace Actions.PaymentOperations
{
	/// <summary>
	/// Description of NewPaymentRunApply.
	/// </summary>
	public class NewPaymentRunApply : ActionBase
	{
		private string FModuleName = "PaymentOperations";
		private NewPaymentRunApplyRequestItem FRequestItem;
		
		public static string Execute(int MasterId,
		                             RequestCommonItem CommonItem,
		                             JObject DataObject,
		                             APISettingsItem SettingsItem,
		                             out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			NewPaymentRunApply inst = new NewPaymentRunApply();
			if (!inst.Setup(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg))
				return "";
			
			inst.FRequestItem = new NewPaymentRunApplyRequestItem();				
			
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
				Result = FRequestItem.Load((ActionBase)this, DataObject);
				FRequestItem.WhoBy = CommonItem.ClientId;
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
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " +Method, FModuleName, MasterId);
			LogOnInsideMethod(Method);
			try 
			{
				Result = BuildOutput(out ErrorMsg);
				if (!string.IsNullOrEmpty(ErrorMsg))
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
		
		private string BuildOutput(out string ErrorMsg)
		{
			ErrorMsg = "";
			string Result = "";
			string Method = "BuildOutput";
			
			LogOnInsideMethod(Method);
			try
			{
				WindowsEventLoggingUtilities.WriteToInfoEvent("Inside BuildOutput", FModuleName, MasterId);
				
				ErrorMsg = InsertStatementItems();
				if (!string.IsNullOrEmpty(ErrorMsg)) return "";
				
				StringBuilder entries = new StringBuilder();
				entries.AppendLine(JsonUtilities.StartJson());
				ErrorMsg = BuildOutputEntries(ref entries);
				entries.AppendLine(JsonUtilities.EndJson());
				Result = entries.ToString();
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
		
		private string InsertStatementItems()
		{
			string Method = "InsertStatementItems";
			string Result = "";			
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, FModuleName, MasterId);
			
			LogOnInsideMethod(Method);
			try
			{
				string QueryText = 
					" EXECUTE PROCEDURE EASTATEMENTCREATELIST(@COMPANYCLIENTID, @RUNID, @BANKID, @ACCOUNTTYPEID, @BRANCHID, @MANAGERID, @ORDERID," +
					"   @MIDMONTH, @ACCOUNTIDS, @RUNNUMBERID, @ALLISSELECTED, @PREVIEW, @MULTIDIRECTRUN, @DIRECTENTRYUSERNUMBER, @BPAY, @WHOBY," +
					"   @STATEMENTDATE, @BPAYBANKUSERID, @BPAYMINIMUM, @PAYMENTEVENTID)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyClientId", CommonItem.CompanyIdValue));
				Params.Add(new FbParameterItem("RunId", FRequestItem.RunId));
				Params.Add(new FbParameterItem("BankId", FRequestItem.BankId));
				Params.Add(new FbParameterItem("AccountTypeId", FRequestItem.AccountTypeId));
				Params.Add(new FbParameterItem("BranchId", FRequestItem.BranchId));
				Params.Add(new FbParameterItem("ManagerId", FRequestItem.ManagerId));
				Params.Add(new FbParameterItem("OrderId", FRequestItem.OrderId));
				Params.Add(new FbParameterItem("MidMonth", FRequestItem.MidMonth));
				Params.Add(new FbParameterItem("AccountIds", FRequestItem.AccountId));
				Params.Add(new FbParameterItem("RunNumberId", FRequestItem.RunNumberId));
				Params.Add(new FbParameterItem("AllIsSelected", FRequestItem.AllIsSelected));
				Params.Add(new FbParameterItem("Preview", FRequestItem.Preview));
				Params.Add(new FbParameterItem("MultiDirectRun", FRequestItem.MultiDirectRun));
				Params.Add(new FbParameterItem("DirectEntryUserNumber", FRequestItem.DirectEntryUserNumber));
				Params.Add(new FbParameterItem("BPay", FRequestItem.BPay));
				Params.Add(new FbParameterItem("WhoBy", FRequestItem.WhoBy));
				Params.Add(new FbParameterItem("StatementDate", FRequestItem.StatementDate));
				Params.Add(new FbParameterItem("BPayBankUserId", FRequestItem.BPayBankUserId));
				Params.Add(new FbParameterItem("BPayMinimum", FRequestItem.BPayMinimumValue));
				Params.Add(new FbParameterItem("PaymentEventId", FRequestItem.PaymentEventId));
				UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
				Result = FirebirdUtilities.DBExecute(FbConnectionString, QueryText, Params, 0);
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
		
		private string BuildOutputEntries(ref StringBuilder Entries)
		{
			string Method = "BuildOutputEntries";
			string Result = "";			
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, FModuleName, MasterId);
			
			LogOnInsideMethod(Method);
			try
			{
				string QueryText = StatementRunGetSQL(CommonItem.CompanyIdValue, FRequestItem.RunId, out Result);
				if (!string.IsNullOrEmpty(Result)) return Result;
				if (string.IsNullOrEmpty(QueryText))
				{
					Result = "Empty or invalid QueryText returned";
					return Result;
				}
				
				WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText " + QueryText, FModuleName, MasterId);
				DataTable dt = FirebirdUtilities.GetFirstDataTable(FbConnectionString, QueryText);
				
				try 
				{
					if ((dt != null) && (dt.Rows != null) && (dt.Rows.Count > 0))
					{
						CommonMethods.BuildJsonArrayFromDataTable(dt, "rows", ref Entries);
					}
				}					
				catch (Exception Ex)
				{
					Result = LogOnException(Method, Ex, "");
					return Result;
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
		
		private string StatementRunGetSQL(Int64 CompanyClientId, 
		                                  Int64 RunId,
		                                  out string ErrorMsg)
		{
			string Result = "";
			string Method = "StatementRunGetSQL";
			ErrorMsg = "";
			
			LogOnInsideMethod(Method);
			try
			{
				string QueryText = "SELECT OUTPUTSQL FROM EASTATEMENTRUNGETSQL(@COMPANYCLIENTID, @RUNID)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyClientId", CommonItem.CompanyIdValue));
				Params.Add(new FbParameterItem("RunId", FRequestItem.RunId));
				UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
				DataTable dt = 
					UsefulDBMethods.Helpers.GetFirstDataTableEx(
						FbConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out ErrorMsg);
				if (!string.IsNullOrEmpty(ErrorMsg)) return Result;
				
				object Obj = null;
				string Str = "";
				foreach (DataRow Row in dt.Rows)
				{
					Obj = Row["OutputSql"];
					Str = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
					if (!string.IsNullOrEmpty(Str)) Result = Str;
				}
			}
			catch (Exception Ex)
			{
				ErrorMsg = 
					Method + Environment.NewLine +
					Ex.Message + Environment.NewLine +
					Ex.StackTrace;					
			}
			finally
			{
				LogOnLeavingMethod(Method);
			}
			
			return Result;
		}
	}
}
