/*
 * Created by SharpDevelop.
 * User: John
 * Date: 04/08/2019
 * Time: 12:03 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Collections.Specialized;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CommonUtilities;
using EAOfficeAPI;
using System.Reflection;

namespace Actions.PaymentOperations
{
	/// <summary>
	/// Description of ReverseRunEntry.
	/// </summary>
	public class ReverseRunEntry : ActionBase
	{
		private string FModuleName = "PaymentOperations";
		private ReverseRunEntryRequestItem FRequestItem;
		/*
		private string FRequest = "";
		private NameValueCollection FCommonItems = new NameValueCollection();
		private NameValueCollection FDataItems = new NameValueCollection();
		*/
		
		/*
		public static string Execute(int MasterId,
									 RequestCommonItem CommonItem,
									 JObject DataObject,
									 APISettingsItem SettingsItem,
									 string Request,
									 out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			ReverseRunEntry inst = new ReverseRunEntry();
			if (!inst.Setup(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg))
				return "";
			
			inst.FRequest = Request;
			
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
		*/
		
		public static string Execute(int MasterId,
									 APISettingsItem SettingsItem,
									 string Request,
									 out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			ReverseRunEntry inst = new ReverseRunEntry();
			if (!inst.Setup(MasterId, SettingsItem, Request, out ErrorMsg))
			{
				WindowsEventLoggingUtilities.WriteToErrorEvent(ErrorMsg, Method, MasterId);
				return "";
			}
			
			//inst.FRequest = Request;
			
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
				//Result = CommonMethods.MakeCollections(FRequest, ref FCommonItems, ref FDataItems);
				
				if (string.IsNullOrEmpty(Result))
				{
					FRequestItem = new ReverseRunEntryRequestItem();
					Result = FRequestItem.Load((ActionBase)this, DataObject);
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

				string QueryText = "EXECUTE PROCEDURE EAStatementReverseWhat(@CompanyClientId, @RunId, @SummaryId, @WhoById)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyClientId", CommonItem.CompanyIdValue));
				object Value = GetCollectionValue("Data", "runId");
				Params.Add(new FbParameterItem("RunId", Value));
				Value = GetCollectionValue("Data", "summaryId");
				Params.Add(new FbParameterItem("SummaryId", Value));

				Params.Add(new FbParameterItem("WhoById", CommonItem.ClientIdValue));
				
				ErrorMsg = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg = FirebirdUtilities.DBExecute(FbConnectionString, QueryText, Params, 0);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Result = CheckCancelledResult(CommonItem.CompanyIdValue, FRequestItem.RunId, FRequestItem.SummaryId, out ErrorMsg);
					if (!string.IsNullOrEmpty(Result)) return Result;
				}				
				
				StatementRunItem RunItem = new StatementRunItem();
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg = RunItem.Load((EAOfficeAPI.ActionBase)this, FbConnectionString, CommonItem.CompanyIdValue, FRequestItem.RunId);
				
					if (!string.IsNullOrEmpty(ErrorMsg))
					{
						Result = LogOnProcessingError(Method, ErrorMsg, "");
					}
				}
				
				//if (string.IsNullOrEmpty(ErrorMsg) && (StringUtilities.SameText(RunItem.StatementPreview, "Y")))
				if (string.IsNullOrEmpty(ErrorMsg) && (FRequestItem.SummaryId > 0))
				{
					StringBuilder Entries = new StringBuilder();
					Entries.AppendLine(JsonUtilities.StartJson());
					
					ErrorMsg = 
						CommonMethods.BuildPaymentSearchGridResults(								
							(ActionBase)this, FbConnectionString, CommonItem.CompanyIdValue, RunItem, 
							"STATEMENT-REPORT", "STATEMENTS-MANAGEMENTMANAGER", ref Entries);
					
					Entries.AppendLine(JsonUtilities.EndJson());
					if (string.IsNullOrEmpty(ErrorMsg)) Result = Entries.ToString(); else Result = LogOnProcessingError(Method, ErrorMsg, "");
					
				}
				else Result = LogResponse(Method, "");						
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
		
		private string CheckCancelledResult(Int64 CompanyClientId,
		                                    Int64 RunId,
		                                    Int64 SummaryId,
		                                    out string ErrorMsg)
		{
			ErrorMsg = "";
			string Result = "";
			string Method = "CheckCancelledResult";
			
			LogOnInsideMethod(Method);
			try
			{
				/*
				select * from EASTATEMENTCANCELS,eastatementrun
				where I_ASTATE_COMPANYCLIENTID = I_ASC_COMPANYCLIENTID and  I_ASC_COMPANYCLIENTID=1011 and I_ASC_RUNID=19777
				  and I_ASC_RUNID=  I_ASTR_CLOSEACCOUNTRUNID and B_ASTR_STATEMENTPREVIEW='N'
				 */  

				string QueryText =
					" SELECT B_ASC_CANCELLED,ASC_NOTCANCELLEDWHY" +
					" FROM EAStatementCancels, EAStatementRun" +
					" WHERE I_AState_CompanyClientId = I_ASC_CompanyClientId" +
					"   and I_ASTR_CLOSEACCOUNTRUNID = I_ASC_RunId" +
					"   and I_ASC_COMPANYCLIENTID=@CompanyClientId" +
					"   and I_ASC_RUNID=@RunId";
				if (SummaryId > 0) QueryText += " and I_ASC_SUMMARYID=@summaryid";
				QueryText += " and B_ASC_CANCELLED = 'N'";
				QueryText += " and B_ASTR_StatementPreview = 'N'";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyClientId", CompanyClientId));
				Params.Add(new FbParameterItem("RunId", RunId));
				if (SummaryId > 0) Params.Add(new FbParameterItem("SummaryId", SummaryId));
				ErrorMsg = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					DataTable tbl = 
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							FbConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.None, out ErrorMsg);
					if (string.IsNullOrEmpty(ErrorMsg) && (tbl.Rows.Count > 0))
					{
						StringBuilder Entries = new StringBuilder();
						Entries.AppendLine(JsonUtilities.StartJson());
						
						ErrorMsg = CommonMethods.BuildJsonArrayFromDataTable(tbl, "whyNotCancelled", ref Entries);

						Entries.AppendLine(JsonUtilities.EndJson());
						Result = Entries.ToString();
					}
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
