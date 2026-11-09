/*
 * Created by SharpDevelop.
 * User: John
 * Date: 28/12/2019
 * Time: 10:04 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Data;
using System.Text;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CommonUtilities;
using EAOfficeAPI;

namespace Actions.LookupLists
{
	/// <summary>
	/// Description of TransactionLookup.
	/// </summary>
	public class TransactionLookup : ActionBase
	{
		private string FModuleName = "TransactionLookup";
		private TransactionLookupRequestItem FRequestItem;
		
		public static string Execute(int MasterId,
		                             APISettingsItem SettingsItem,
		                             string Request,
		                             out string ErrorMsg)
		{
			string Method = "LedgerLookup - Execute";
			string Result = "";
			ErrorMsg = "";

			TransactionLookup inst = new TransactionLookup();
			if (!inst.Setup(MasterId, SettingsItem, Request, out ErrorMsg))
				return "";
			
			inst.LogOnInsideMethod(Method);
			try 
			{
				Result = inst.Init();
				if (!string.IsNullOrEmpty(Result))
					return "";
				
				Result = inst.Validate();
				if (!string.IsNullOrEmpty(Result))
					return Result;
				
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
			
			LogOnInsideMethod(Method);
			try 
			{
				FRequestItem = new TransactionLookupRequestItem();
				Result = FRequestItem.Load((ActionBase)this);
			}
			catch (Exception ex) 
			{
				Result = LogOnException(Method, ex, "");
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
			
			LogOnInsideMethod(Method);
			try 
			{
				Result = FRequestItem.Validate();
			}
			catch (Exception ex)
			{
				Result = LogOnException(Method, ex, "");
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
			
			LogOnInsideMethod(Method);
			try
			{
				string QueryText = 
					" SELECT BusinessId AS ID, BUType AS Category, StreetName AS Street, ClientRef AS Client, AccountOwner AS Owner" +
					" FROM EAGetFindTBA(@CompanyId, @TBA, @SearchText, @CreditorAccountId)" + Environment.NewLine;
				List<string> WhereClause = new List<string>();
				if (FRequestItem.SearchType == 1) WhereClause.Add("BUType containing '@SearchText'");
				if (FRequestItem.SearchStreet == 1) WhereClause.Add("StreetName containing '@SearchText'");
				if (FRequestItem.SearchClient == 1) WhereClause.Add("ClientRef containing '@SearchText'");
				if (FRequestItem.SearchOwner == 1) WhereClause.Add("AccountOwner containing '@SearchText'");
				
				if (WhereClause.Count > 0)
				{
					QueryText = QueryText + string.Join(" OR ", WhereClause.ToArray());
				}
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CommonItem.CompanyIdValue));
				Params.Add(new FbParameterItem("TBA", FRequestItem.TBA));
				Params.Add(new FbParameterItem("SearchText", FRequestItem.SearchText));
				Params.Add(new FbParameterItem("CreditorAccountId", FRequestItem.CreditorAccountId));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
				DataTable tbl = null;
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							FbConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out Result);
				}
				
				if (string.IsNullOrEmpty(Result))
				{
					StringBuilder Entries = new StringBuilder();
					Result = CommonUtilities.JsonUtilities.BuildJsonArrayFromDataTable(tbl, "gridEntries", ref Entries);
				}
			}
			catch (Exception ex)
			{
				Result = LogOnException(Method, ex, "");
			}
			finally
			{
				LogOnLeavingMethod(Method);
			}
			return Result;
		}
		
		private string BuildResltFromDataSet(DataSet Source)
		{
			string Result = "";
			const string Method = "BuildResltFromDataSet";
			
			LogOnInsideMethod(Method);
			try 
			{
				DataTable tbl = Source.Tables[0];
				string Str = "";
				string ColValue = "";
				int ColCount = tbl.Columns.Count;
				int RowCount = tbl.Rows.Count;
				DataRow CurrentRow = null;
				List<string> RowList = new List<string>();
				List<string> StrList = new List<string>();
				string QuotedStr = "\"{0}\"";

	            StringBuilder sb = new StringBuilder();
				sb.Append(CommonUtilities.JsonUtilities.StartJson());
				
	            sb.Append(CommonUtilities.JsonUtilities.StartJsonArray("fieldNames"));
	            StrList.Clear();
				foreach (DataColumn column in tbl.Columns)
				{
					StrList.Add(string.Format(QuotedStr, column.ColumnName));
				}
				sb.Append(string.Join(",", StrList.ToArray()));
	            sb.Append(CommonUtilities.JsonUtilities.EndJsonArray());
	            
	            sb.Append(",");
	            
	            sb.Append(CommonUtilities.JsonUtilities.StartJsonArray("rows"));
				RowList.Clear();
				for (int RowIndex = 0; RowIndex < RowCount; RowIndex++)
				{
					CurrentRow = tbl.Rows[RowIndex];
					StrList.Clear();
	            	for (int Index = 0; Index < ColCount; Index++)
	            	{
	            		ColValue = ((CurrentRow[Index] == DBNull.Value) ? "" : Convert.ToString(CurrentRow[Index]));
	            		ColValue = CommonUtilities.JsonUtilities.CleanForJSON(ColValue);	            		
	            		ColValue = string.Format(QuotedStr, ColValue);
	            		StrList.Add(ColValue);
	            	}
	            	Str =
	            		CommonUtilities.JsonUtilities.StartJsonArray("") +
	            		string.Join(",", StrList.ToArray()) +
	            		CommonUtilities.JsonUtilities.EndJsonArray();
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
