/*
 * Created by SharpDevelop.
 * User: John
 * Date: 06/03/2019
 * Time: 3:48 PM
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

namespace Actions.ContactOperations
{
	/// <summary>
	/// Description of InitEmailItems.
	/// </summary>
	public class InitEmailItems : ActionBase
	{
		private string FModuleName = "ContactOperations";
		
		private string FPortfolioType = "";
		private int FPortfolioTypeValue = 0;
		private string FReportEventId = "";
		private int FReportEventIdValue = 0;
		private string FTransactionId = "";
		private int FTransactionIdValue = 0;
		
		public static string Execute(int MasterId,
		                             RequestCommonItem CommonItem,
		                             JObject DataObject,
		                             APISettingsItem SettingsItem,
		                             out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			InitEmailItems inst = new InitEmailItems();
			if (!inst.Setup(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg))
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
				FPortfolioType = (string)DataObject["portfolioTypeId"];
				FPortfolioTypeValue = StringUtilities.StrToIntDef(FPortfolioType, -1);
				FReportEventId = (string)DataObject["reportEventId"];
				FReportEventIdValue = StringUtilities.StrToIntDef(FReportEventId, -1);
				FTransactionId = (string)DataObject["transactionId"];
				FTransactionIdValue = StringUtilities.StrToIntDef(FTransactionId, -1);
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
			int EventRunId = 0;
			
			LogOnInsideMethod(Method);
			try
			{
				WindowsEventLoggingUtilities.WriteToInfoEvent("Inside BuildOutput", FModuleName, MasterId);
				
				StringBuilder entries = new StringBuilder();
				entries.AppendLine(JsonUtilities.StartJson());
					
				ErrorMsg = BuildOutputEntries(ref entries);				
				if (!string.IsNullOrEmpty(ErrorMsg)) return "";
				
				// create an eventrun record				
				EventRunId =
					UsefulDBMethods.EventHelpers.AddEmailEvent(
						this.FbConnectionString, CommonItem.CompanyIdValue, FReportEventIdValue, "Contact-Email", 
						CommonItem.ClientIdValue, out ErrorMsg);
				if (!string.IsNullOrEmpty(ErrorMsg)) return "";
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					if (FPortfolioTypeValue > 200)
					{
						string QueryText = "UPDATE EAEVENTRUN set EAEVENTRUN.I_EVA_TASKID = @TaskId where eva_id = @EventRunId";
						List<FbParameterItem> Params = new List<FbParameterItem>();
						Params.Add(new FbParameterItem("TaskId", FPortfolioTypeValue));
						Params.Add(new FbParameterItem("EventRunId", EventRunId));
						ErrorMsg = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
						
						if (string.IsNullOrEmpty(ErrorMsg))
						{
							ErrorMsg = FirebirdUtilities.DBExecute(FbConnectionString, QueryText, Params, 0);
						}
					}					
				}
				
				if (!string.IsNullOrEmpty(ErrorMsg)) return "";
				
				// pass the eventrunId in the results set
				entries.Append(",");
				entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("EventRunId", EventRunId, false));
				
				// create the xemail_eventrunId_ table
				ErrorMsg = CommonMethods.CreateXEmailTableForRunId(this.FbConnectionString, EventRunId);
				if (!string.IsNullOrEmpty(ErrorMsg)) return "";
				
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
		
		private string BuildOutputEntries(ref StringBuilder Entries)
		{
			string Method = "BuildOutputEntries";
			string Result = "";			
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, FModuleName, MasterId);
			
			LogOnInsideMethod(Method);
			try
			{
				string QueryText = "SELECT * FROM Init_Email_Items(@CompanyId, @ClientId, @PortfolioType, @TransactionId)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("@CompanyId", CommonItem.CompanyIdValue));
				Params.Add(new FbParameterItem("@ClientId", CommonItem.ClientIdValue));
				Params.Add(new FbParameterItem("@PortfolioType", FPortfolioTypeValue));
				Params.Add(new FbParameterItem("@TransactionId", FTransactionIdValue));
				//WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText:" + Environment.NewLine + QueryText, FModuleName, MasterId);
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);

				DataTable dt = null;
				try 
				{
					dt = FirebirdUtilities.GetFirstDataTable(FbConnectionString, QueryText, Params, 0);
					if (dt == null)
					{
						Result = "No tables in the dataset to process";
						return Result;
					}
				}
				catch (Exception Ex)
				{
					Result = LogOnException(Method, Ex, "");
					return Result;
				}
				
				foreach (DataRow dataRow in dt.Rows)
				{
					Entries.AppendLine((dataRow[0] == DBNull.Value) ? "" : Convert.ToString(dataRow[0]));
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
