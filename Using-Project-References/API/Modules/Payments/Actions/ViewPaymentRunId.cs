/*
 * Created by SharpDevelop.
 * User: John
 * Date: 25/06/2019
 * Time: 6:34 PM
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
	/// Description of ViewPaymentRunId.
	/// </summary>
	public class ViewPaymentRunId : ActionBase
	{
		private string FModuleName = "PaymentOperations";
		
		private string FDisplayTypeValue = "";
		private int FDisplayType = 0;
		private string FPaymentRunIdValue = "";
		private int FPaymentRunId = 0;
		
		public static string Execute(int MasterId,
		                             RequestCommonItem CommonItem,
		                             JObject DataObject,
		                             APISettingsItem SettingsItem,
		                             out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			ViewPaymentRunId inst = new ViewPaymentRunId();
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
				FDisplayTypeValue = (string)DataObject["displayType"];
				FDisplayType = StringUtilities.StrToIntDef(FDisplayTypeValue, -1);
				FPaymentRunIdValue = (string)DataObject["paymentRunId"];
				FPaymentRunId = StringUtilities.StrToIntDef(FPaymentRunIdValue, -1);
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
				string BaseFormat = "{0} is {1}";
				string NotProvided = "Not Provided";
				string Invalid = "Invalid";				
				string Key = "";
				
				if (string.IsNullOrEmpty(Result))
				{
					Key = "DisplayType";
					if (string.IsNullOrEmpty(FDisplayTypeValue)) Result = string.Format(BaseFormat, Key, NotProvided);
					else if (FDisplayType < 0)                   Result = string.Format(BaseFormat, Key, Invalid);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					Key = "PaymentRunId";
					if (string.IsNullOrEmpty(FPaymentRunIdValue)) Result = string.Format(BaseFormat, Key, NotProvided);
					else if (FPaymentRunId < 0)                   Result = string.Format(BaseFormat, Key, Invalid);
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
				string QueryText =
					" SELECT i_summary_id as I_Summary_ID," +
					"        ZC10_Date," +
					"        (select OUTPUTVALUE from formatcurrencyforlist(ZR10_Payment)) as ZR10_Payment," +
					"        ZL20_Client," +
					"      	 ZL20_Property," +
					"        ZL20_Sub_Ledger," +
					"        ZC8_Direct," +
					"        I_Reference," +
					"        ZL20_BANK_NAME as ZL20_Payment_Details," +
					"        ZL30_EMAIL_TO as ZL30_eMailed_To," +
					"        ZL30_EMAIL_COMMENTS," +
					"    	 ZL20_LEDGER_TYPE as ZC15_Ledger_Type," +
					"        ZC8_ARCHIVED," + 
					"        I_LEDGER_ID," +
					"        I_SUB_LEDGER_ID," +
					"        I_Client_ID," +
					"        ZL20_FROM_BANK," +
					"        I_Bank_ID," +
					"        I_Invoice_ID," +
					"	     ZC8_CONVERSION," +
					"	     ZC8_UNASSIGNED" +
					" FROM" +
					" EAPAYMENTLIST (@DisplayType, @RunId, @CompanyClientId, 0, 0, 0, 0, 0, 0, 0)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("DisplayType", FDisplayType));
				Params.Add(new FbParameterItem("StatementRunId", FPaymentRunId));
				Params.Add(new FbParameterItem("CompanyClientId", CommonItem.CompanyIdValue));
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
				
				string Str = "";
				string ColName = "";
				string ColValue = "";
				List<string> RowList = new List<string>();
				List<string> StrList = new List<string>();
				object Obj = null;
				Entries.AppendLine(JsonUtilities.StartJsonArray("Items"));
				foreach (DataRow ThisRow in dt.Rows)
				{
					StrList.Clear();
					foreach (DataColumn ThisCol in dt.Columns)
					{
						ColName = ThisCol.ColumnName;
						Obj = ThisRow[ColName];
						ColValue = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
						StrList.Add(JsonUtilities.AddPropertyNameAndValue(ColName, ColValue, false));
					}
					Str =
						JsonUtilities.StartJsonObject("") +
						string.Join(",", StrList.ToArray()) +
						JsonUtilities.EndJsonObject();
					RowList.Add(Str);
				}
				Entries.Append(string.Join(",", RowList.ToArray()));
				Entries.AppendLine(JsonUtilities.EndJsonArray());
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
