/*
 * Created by SharpDevelop.
 * User: John
 * Date: 28/12/2018
 * Time: 5:17 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CommonUtilities;
using EAOfficeAPI;

namespace Actions.Reconcilliation
{
	/// <summary>
	/// Description of BankAccountList.
	/// </summary>
	public class BankAccountList : ActionBase
	{
		private string FModuleName = "Reconcilliation";
		
		public static string Execute(int MasterId,
		                             RequestCommonItem CommonItem,
		                             JObject DataObject,
		                             APISettingsItem SettingsItem,
		                             out string ErrorMsg)
		{
			string Method = "DefaultMethods - Execute";
			string Result = "";
			ErrorMsg = "";

			BankAccountList inst = new BankAccountList();
			if (!inst.Setup(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg))
				return "";
			
			inst.LogOnInsideMethod(Method);
			try
			{
				Result = inst.ExecuteCommon(out ErrorMsg);
			}
			finally
			{
				inst.LogOnLeavingMethod(Method);
			}
			
			return Result;
		}
		
		private string ExecuteCommon(out string ErrorMsg)
		{
			ErrorMsg = "";
			string Method = "ExecuteCommon";
			LogOnInsideMethod(Method);
			
			string Result = "";
			try 
			{
				WindowsEventLoggingUtilities.WriteToInfoEvent("Inside ExecuteCommon - ActionName: " + this.CommonItem.Action, FModuleName, MasterId);
				if (CommonUtilities.StringUtilities.SameText(this.CommonItem.Action, "GetBankAccountList"))
				    Result = GetBankAccountList(out ErrorMsg);
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
		
		private string GetBankAccountList(out string ErrorMsg)
		{
			ErrorMsg = "";
			string Result = "";

			const string Method = "ExecuteCommon";
			string ConnectionString = CommonDBMethods.Common.ConnectionString(APISettings);
			LogOnInsideMethod(Method);
			try 
			{
				WindowsEventLoggingUtilities.WriteToInfoEvent("Inside GetBankAccountList", FModuleName, MasterId);
				StringBuilder sb = new StringBuilder();
				if (!string.IsNullOrEmpty(ConnectionString)) {
					WindowsEventLoggingUtilities.WriteToInfoEvent("ConnectionString: " + ConnectionString, FModuleName, MasterId);
					string FieldList = 
						"Bank_Description, BSB, Account_Number, Bank_Name, Status, S_CashBook_Balances AS ZR10_CashBook_Balances," +
						"S_Ledger_Balances AS ZR10_Ledger_Balances, S_Bank_Balances AS ZR10_Bank_Balances," +
						"B_General, B_IBD, B_Active, B_Residential, B_Commercial, B_Strata, B_Sales, B_Management, B_Bond," +
						"I_Bank_ID";
					string QueryText = "SELECT " + FieldList + " FROM BankAccountStatus({0})";
					QueryText = string.Format(QueryText, StringUtilities.StrToIntDef(CommonItem.CompanyId, 0));
					DataSet dataSetFromQuery = FirebirdUtilities.GetDataSetFromQuery(ConnectionString, QueryText, out ErrorMsg);
					if (!string.IsNullOrEmpty(ErrorMsg)) {
						ErrorMsg = 
							"Error Executing Query" + Environment.NewLine +
							ErrorMsg + Environment.NewLine +
							"QueryText:" + Environment.NewLine + QueryText;
						WindowsEventLoggingUtilities.WriteToInfoEvent(ErrorMsg, "EAOfficeAPI", MasterId);
						Result = LogOnErrorSelectingData(Method, ErrorMsg, QueryText);
						return Result;
					}
					else if (dataSetFromQuery.Tables.Count == 0) {
						ErrorMsg = "No tables in the dataset to process";
						Result = LogOnErrorSelectingData(Method, ErrorMsg, QueryText);
						return Result;
					}
					Result = BuildResult(dataSetFromQuery.Tables[0], out ErrorMsg);
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
		
		private string BuildResult(System.Data.DataTable Source,
		                           out string ErrorMsg)
		{
			ErrorMsg = "";
			string Result = "";
			const string Method = "BuildResult";
			
			LogOnInsideMethod(Method);
			try 
			{
				string Str = "";
				string ColName = "";
				string ColStartsWith = "";
				string ColValue = "";
				int ColCount = Source.Columns.Count;
				int RowCount = Source.Rows.Count;
				DataRow CurrentRow = null;
				List<string> RowList = new List<string>();
				List<string> StrList = new List<string>();
				string QuotedStr = "\"{0}\"";

	            StringBuilder sb = new StringBuilder();
				sb.Append(CommonUtilities.JsonUtilities.StartJson());
				
	            sb.Append(CommonUtilities.JsonUtilities.StartJsonArray("fieldNames"));
	            StrList.Clear();
				foreach (DataColumn column in Source.Columns)
				{
					StrList.Add(string.Format(QuotedStr, column.ColumnName));
				}
				sb.Append(string.Join(",", StrList.ToArray()));
	            sb.Append(CommonUtilities.JsonUtilities.EndJsonArray());
	            
	            // Row entries
	            sb.Append(",");
	            
	            sb.Append(CommonUtilities.JsonUtilities.StartJsonArray("rows"));
				RowList.Clear();
				for (int RowIndex = 0; RowIndex < RowCount; RowIndex++)
				{
					CurrentRow = Source.Rows[RowIndex];
					StrList.Clear();
	            	for (int ColIndex = 0; ColIndex < Source.Columns.Count; ColIndex++)
	            	{
	            		ColStartsWith = "";
	            		ColName = Source.Columns[ColIndex].ColumnName;
	            		if ((ColName.StartsWith("I_")) ||
	            		    (ColName.StartsWith("B_")) ||
	            		    (ColName.StartsWith("D_")) ||
	            		    (ColName.StartsWith("M_")))
	            		{
	            			ColStartsWith = ColName;
	            			ColName = ColName.Substring(2);
	            		}
	            		else if ((ColName.StartsWith("ZZ")) ||
	            		         (ColName.StartsWith("ZL")) ||
	            		         (ColName.StartsWith("ZR")) ||
	            		         (ColName.StartsWith("ZC")))
	            		{
	            			ColStartsWith = ColName;
	            			ColName = ColName.Substring(ColName.IndexOf("_") + 1);
	            		}
						
	            		ColValue = ((CurrentRow[ColIndex] == DBNull.Value) ? "" : Convert.ToString(CurrentRow[ColIndex]));	            		
	            		if (ColStartsWith.StartsWith("B_"))
	            		{
	            			string str = ""; 
	            			if (CommonUtilities.StringUtilities.SameText(ColValue, "N") || 
	            			    CommonUtilities.StringUtilities.SameText(ColValue, "NO"))
	            				str = "false";
	            			else str = "true";
	            				
	            			string text = "\"" + ColName + "\": " + str;
	            			StrList.Add(text);
	            		}
	            		else
	            		{
		            		StrList.Add(CommonUtilities.JsonUtilities.AddPropertyNameAndValue(ColName, ColValue, false));
	            		}
	            	}
	            	Str =
	            		CommonUtilities.JsonUtilities.StartJsonObject("") +
	            		string.Join(",", StrList.ToArray()) +
	            		CommonUtilities.JsonUtilities.EndJsonObject();
	            	RowList.Add(Str);	            	
				}
				sb.Append(string.Join(",", RowList.ToArray()));
	            sb.AppendLine(CommonUtilities.JsonUtilities.EndJsonArray());
				
				sb.Append(CommonUtilities.JsonUtilities.EndJson());
				Result = sb.ToString();
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