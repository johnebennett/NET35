/*
 * Created by SharpDevelop.
 * User: John
 * Date: XX/XX/2019
 * Time: XX:XX XM
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
	/// Description of InitNewPaymentPreviewEntries.
	/// </summary>
	public class InitNewPaymentPreviewEntries : ActionBase
	{
		private string FModuleName = "PaymentOperations";
		
		public static string Execute(int MasterId,
		                             RequestCommonItem CommonItem,
		                             JObject DataObject,
		                             APISettingsItem SettingsItem,
		                             out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			InitNewPaymentPreviewEntries inst = new InitNewPaymentPreviewEntries();
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
				
				StringBuilder entries = new StringBuilder();
				entries.AppendLine(JsonUtilities.StartJson());
					
				ErrorMsg = BuildOutputEntries(ref entries);				
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
				string QueryText = "SELECT * FROM Init_NewPaymentRun_Items(@CompanyId, @ClientId)";				
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CommonItem.CompanyIdValue));
				Params.Add(new FbParameterItem("ClientId", CommonItem.ClientIdValue));
				WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText:" + Environment.NewLine + QueryText, FModuleName, MasterId);
				
				foreach (FbParameterItem Param in Params)
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent(
						Param.ParameterName + ": " + Convert.ToString(Param.ParameterValue), FModuleName, MasterId);
				}

				DataTable dt = null;
				try 
				{
					dt = FirebirdUtilities.GetFirstDataTable(FbConnectionString, QueryText, Params, 0);
					if (dt == null)
					{
						Result = "No tables in the dataset to process";
						Result = LoggingEntry(Method, Result, "");
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
				
				Int64 NewRunId = GetNextStatementRunId(out Result);
				if (string.IsNullOrEmpty(Result))
				{
					// add the new RunId in the results set
					Entries.Append(",");
					Entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("NewRunId", NewRunId, false));
				}
				else
				{
					WindowsEventLoggingUtilities.WriteToErrorEvent("Error getting the new StatementRunId:" + Environment.NewLine + Result, FModuleName, MasterId);
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
		
		private Int64 GetNextStatementRunId(out string ErrorMsg)
		{
			Int64 Result = 0;
			string Method = "GetNextStatementRunId";
			ErrorMsg = "";

			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, FModuleName, MasterId);
			
			LogOnInsideMethod(Method);
			try
			{
				Result = 
					UsefulDBMethods.EventHelpers.GetNextStatementRunId(
						FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue, out ErrorMsg);
			}
			catch (Exception Ex)
			{
				ErrorMsg = LogOnException(Method, Ex, "");
			}
			finally
			{
				LogOnLeavingMethod(Method);
			}
			
			return Result;
		}
	}
}
