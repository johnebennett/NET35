/*
 * Created by SharpDevelop.
 * User: John
 * Date: 12/01/2019
 * Time: 10:59 AM
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

namespace Actions.Portfolio
{
	/// <summary>
	/// Description of PortfolioSelectEntryItems.
	/// </summary>
	public class PortfolioSelectEntryItems : ActionBase
	{
		private string FModuleName = "Portfolio";
		
		private int DefaultPortfolioId = 0;
		private int DefaultManagerId =  0;
		private int DefaultBranchId = 0;

		public static string Execute(int MasterId,
		                             RequestCommonItem CommonItem,
		                             JObject DataObject,
		                             APISettingsItem SettingsItem,
		                             out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			PortfolioSelectEntryItems inst = new PortfolioSelectEntryItems();
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
				//Result = "{\"message\": \"hello\"}";
				Result = BuildOutput(out ErrorMsg);
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
				
				/*
				PopulateDefaultValues(out ErrorMsg);
				if (!string.IsNullOrEmpty(ErrorMsg)) return "";
				*/
				
				StringBuilder entries = new StringBuilder();
				entries.AppendLine(JsonUtilities.StartJson());
				
				/*
				ErrorMsg = BuildCommonComboBoxEntries(ref entries);
				if (!string.IsNullOrEmpty(ErrorMsg)) return "";
				
				ErrorMsg = 
					CommonMethods.BuildFilterComboBoxEntries(
						FbConnectionString, CommonItem.CompanyIdValue, MasterId, DefaultPortfolioId, ref entries);
				if (!string.IsNullOrEmpty(ErrorMsg)) return "";				
				
				entries.Append(",");
				ErrorMsg = 
					CommonMethods.BuildListReportEntries(
						FbConnectionString, CommonItem.CompanyIdValue, MasterId, DefaultPortfolioId, ref entries);
				if (!string.IsNullOrEmpty(ErrorMsg)) return "";
				*/
					
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
				string QueryText = "SELECT * FROM Portfolio_Select_Items({0}, {1})";
				QueryText = string.Format(QueryText, CommonItem.CompanyIdValue, CommonItem.ClientIdValue);
				WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText:" + Environment.NewLine + QueryText, FModuleName, MasterId);
				DataTable dt = null;
				try 
				{
					dt = FirebirdUtilities.GetFirstDataTable(FbConnectionString, QueryText);					
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
		
		private void PopulateDefaultValues(out string ErrorMsg)
		{
			string Method = "PopulateDefaultValues";
			ErrorMsg = "";			
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, FModuleName, MasterId);
			
			LogOnInsideMethod(Method);
			try
			{
				string QueryText =
					"SELECT I_ALogin_PortfolioBranchId, I_ALogin_PortfolioManagerId, I_ALogin_PortfolioPortfolioId " +
					"FROM EALogin " +
					"WHERE I_ALogin_CompanyId = {0} AND I_ALogin_ClientId = {1}";
				QueryText = string.Format(QueryText, CommonItem.CompanyIdValue, CommonItem.ClientIdValue);
				WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText:" + Environment.NewLine + QueryText, FModuleName, MasterId);
				DataTable dt = null;
				try 
				{
					dt = FirebirdUtilities.GetFirstDataTable(FbConnectionString, QueryText);					
					if (dt == null)
					{
						ErrorMsg = "No tables in the dataset to process";
						return;
					}
				}					
				catch (Exception Ex)
				{
					ErrorMsg = LogOnException(Method, Ex, "");
					return;
				}
				
				if (dt.Rows.Count > 0) 
				{
					object obj = dt.Rows[0]["I_ALogin_PortfolioBranchId"];
					DefaultBranchId = (obj == DBNull.Value) ? 0 : Convert.ToInt32(obj);
					
					obj = dt.Rows[0]["I_ALogin_PortfolioManagerId"];
					DefaultManagerId = (obj == DBNull.Value) ? 0 : Convert.ToInt32(obj);
					
					obj = dt.Rows[0]["I_ALogin_PortfolioPortfolioId"];
					DefaultPortfolioId = (obj == DBNull.Value) ? 0 : Convert.ToInt32(obj);
				}
			}
			catch (Exception Ex)
			{
				ErrorMsg = LogOnException(Method, Ex, "");
			}
			finally
			{
				LogOnLeavingMethod(Method);
			}
		}

		private string BuildCommonComboBoxEntries(ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "BuildCommonComboBoxEntries";
			
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside BuildCommonComboBoxEntries", FModuleName, MasterId);
			int[] ComboBoxIds = new int[] {1, 2, 3, 5}; // offices, managers, portfolioTypes, businessActiveTypes
			string QueryTextBase = "SELECT * FROM CommonComboEntriesOnly({0}, {1}, {2}, 0)";
			string QueryText = "";
			DataSet ds = null;
				
			foreach(int ComboBoxId in ComboBoxIds)
			{
				QueryText =
					string.Format(QueryTextBase, CommonItem.CompanyIdValue, CommonItem.ClientIdValue, ComboBoxId);
				WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText:" + Environment.NewLine + QueryText, FModuleName, MasterId);
				ds = FirebirdUtilities.GetDataSetFromQuery(this.FbConnectionString, QueryText, out Result);
				if (!string.IsNullOrEmpty(Result))
				{
						Result = 
							"Error Executing Query" + Environment.NewLine +
							Result + Environment.NewLine +
							"QueryText:" + Environment.NewLine + QueryText;
						WindowsEventLoggingUtilities.WriteToInfoEvent(Result, FModuleName, MasterId);
						LogOnErrorSelectingData(Method, Result, QueryText);
						return Result;
				}
				Result = BuildCommonComboBoxEntryItem(ds, ref Entries);
				if (!string.IsNullOrEmpty(Result)) return Result;
			}
			//Entries.AppendLine(JsonUtilities.EndJson());

			return Result;
		}
		
		private string BuildCommonComboBoxEntryItem(DataSet Source,
		                                            //bool IsLastEntry,
		                                            ref StringBuilder Entries)
		{
			string Result = "";
			
			try
			{
				if (Source.Tables.Count == 0) return "No tables in the dataset to process";			
				
				foreach (DataRow dataRow in Source.Tables[0].Rows)
				{
					Entries.AppendLine((dataRow[0] == DBNull.Value) ? "" : Convert.ToString(dataRow[0]));
				}
				//if (!IsLastEntry) 
				Entries.Append(",");
			}
			finally
			{
				//Entries.AppendLine(JsonUtilities.EndJsonArray());
			}

			return Result;
		}
	}
}
