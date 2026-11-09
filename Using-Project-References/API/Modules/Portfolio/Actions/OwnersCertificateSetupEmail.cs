/*
 * Created by SharpDevelop.
 * User: John
 * Date: 04/08/2020
 * Time: 3:06 PM
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
using UsefulDBMethods;
using EAOfficeAPI;
namespace Actions.Portfolio
{
	/// <summary>
	/// Description of OwnersCertificateSetupEmail.
	/// </summary>
	public class OwnersCertificateSetupEmail : ActionBase
	{
		private string FModuleName = "Portfolio";
		private  OwnersCertificateDialogContentRequestItem FRequestItem;
		
		public static string Execute(int MasterId,
									 APISettingsItem SettingsItem,
									 string Request,
									 out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			OwnersCertificateSetupEmail inst = new OwnersCertificateSetupEmail();
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
				FRequestItem = new OwnersCertificateDialogContentRequestItem();
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
			
			LogOnInsideMethod(Method);
			try
			{			
				WindowsEventLoggingUtilities.WriteToInfoEvent("Inside BuildOutput", FModuleName, MasterId);
				
				StringBuilder Entries = new StringBuilder();
				
				// We shouldn't get to this point without a RecordId so fail if we do
				if (FRequestItem.RecordId == 0)
				{
					ErrorMsg = "Invalid or missing RecordId - terminating";
				}
				
				StringBuilder TempEntries = new StringBuilder();
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg =
						CommonMethods.EAOCCertificateGroup(
							FbConnectionString, CommonItem.CompanyIdValue, FRequestItem.RecordId, FRequestItem.CategoryOnlyId, 
							CommonItem.ClientIdValue, ref TempEntries);
				}
				
				// Don't need the note text for this process like we use in the OwnerCertificateDialogContent
				
				string Value = "";
				JObject JSONSource = null;
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Value =
						JsonUtilities.StartJson() +
						TempEntries.ToString() +
						JsonUtilities.EndJson();
					if (string.IsNullOrEmpty(Result) && string.IsNullOrEmpty(Value)) Result = "Cannot determine the Group entries";
					
					if (string.IsNullOrEmpty(Result))
					{
						JSONSource = JObject.Parse(Value);
						if (JSONSource == null) Result = "Cannot parse the Group source";
						Value = "";
					}
				}
				
				string QueryText = "";
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.EmailSQL1", out ErrorMsg);
					if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(QueryText)) ErrorMsg = "Cannot find the first email query";
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg = FirebirdUtilities.DBExecute(FbConnectionString, QueryText);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					QueryText = JsonUtilities.GetJsonPathValueStr(JSONSource, "$.EmailSQL12", out ErrorMsg);
					if (string.IsNullOrEmpty(ErrorMsg) && string.IsNullOrEmpty(QueryText)) ErrorMsg = "Cannot find the second email query";
				}
				
				DataTable tbl = null;
				object Obj = null;
				int StatementRunId = 0;
				int LedgerBulkId = 0;
				int EventRunId = 0;
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							FbConnectionString, QueryText, null, 0, FirebirdQueryCheckTypes.HasTables, out Result);
					if (string.IsNullOrEmpty(Result))
					{
						if ((Entries.Length > 0) && (tbl.Rows.Count > 0)) Entries.Append(",");
						foreach (DataRow Row in tbl.Rows)
						{
							Obj = Row["I_OC_STATEMENTRUNID"];
							StatementRunId = (Obj == DBNull.Value) ? 0: Convert.ToInt32(Obj);
							Obj = Row["I_OC_LEDGERBULKID"];
							LedgerBulkId = (Obj == DBNull.Value) ? 0: Convert.ToInt32(Obj);
							Obj = Row["I_OC_EVENTRUNID"];
							EventRunId = (Obj == DBNull.Value) ? 0: Convert.ToInt32(Obj);
						}
					}
				}				
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Entries.Append(JsonUtilities.AddPropertyNameAndValue("StatementRunId", StatementRunId));
					Entries.Append(JsonUtilities.AddPropertyNameAndValue("LedgerBulkId", LedgerBulkId));
					Entries.Append(JsonUtilities.AddPropertyNameAndValue("EventRunId", EventRunId, false));
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
			
			return Result;
		}
	}
}