/*
 * Created by SharpDevelop.
 * User: John
 * Date: 11/06/2020
 * Time: 4:00 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text;
using System.Data;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System.Diagnostics;

using CommonUtilities;
using EAOfficeAPI;
namespace Actions.Portfolio.AgentNoDlg
{
	/// <summary>
	/// Description of CommonMethods.
	/// </summary>
	public class CommonMethods
	{
		public CommonMethods()
		{
		}
		
		public static string LoadSetupPageEntriesRequestItem(ActionBase ActionBase, 
		                                                     ref SetupPageEntriesRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadSetupPageEntriesRequestItem";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, ActionBase.MasterId);
					
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				Object Obj = ActionBase.GetCollectionValue("Data", "PortfolioId");
				TempItem.PortfolioIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "AgentNumber1");
				TempItem.AgentNumber1 = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "AgentNumber2");
				TempItem.AgentNumber2 = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "AgentNumber3");
				TempItem.AgentNumber3 = (Obj == null) ? "" : Convert.ToString(Obj);
				Obj = ActionBase.GetCollectionValue("Data", "ValidateAgentNo");
				TempItem.ValidateAgentNo = (Obj == null) ? "" : Convert.ToString(Obj);
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
			}
			
			return Result;
		}
		
		public static string EATransactionAgentNo(string ConnectionString,
		                                          Int64 CompanyId,
		                                          int TransactionId,
		                                          string AgentNumber1,
		                                          string AgentNumber2,
		                                          string AgentNumber3,
		                                          string ValidateAgentNo,
		                                          ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "EATransactionAgentNo";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, Method, 0);
					
			ActionBase ActionBase = new ActionBase();
			ActionBase.LogOnInsideMethod(Method);
			try 
			{
				if (Entries.Length > 0) Entries.Append(",");
				Entries.Append(JsonUtilities.AddPropertyNameAndValue("PortfolioId", TransactionId, false));
				
				string QueryText = 
					"SELECT OutputLine FROM EATransactionAgentNo(@CompanyId, @TransactionId, @AgentNumber1, @AgentNumber2, @AgentNumber3, @ValidateAgentNo)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CompanyId));
				Params.Add(new FbParameterItem("TransactionId", TransactionId));
				Params.Add(new FbParameterItem("AgentNumber1", AgentNumber1));
				Params.Add(new FbParameterItem("AgentNumber2", AgentNumber2));
				Params.Add(new FbParameterItem("AgentNumber3", AgentNumber3));
				Params.Add(new FbParameterItem("ValidateAgentNo", ValidateAgentNo));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
				
				object Obj = null;
				DataTable tbl = null;
				if (string.IsNullOrEmpty(Result))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							ConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTablesAndRows, out Result);
					if (string.IsNullOrEmpty(Result))
					{
						if (Entries.Length > 0) Entries.Append(",");
						foreach (DataRow Row in tbl.Rows)
						{
							Obj = Row["OUTPUTLINE"];
							Entries.AppendLine((Obj == DBNull.Value) ? "" : Convert.ToString(Obj));
						}
					}
				}
			}
			catch (Exception Ex)
			{
				Result = ActionBase.LogOnException(Method, Ex, "");
			}
			finally
			{
				ActionBase.LogOnLeavingMethod(Method);
				WindowsEventLoggingUtilities.WriteToInfoEvent("After EATransactionAgentNo ErrorMsg: " + Result, Method, 0);
			}
			
			return Result;
		}
	}
}
