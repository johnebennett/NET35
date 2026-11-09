/*
 * Created by SharpDevelop.
 * User: John
 * Date: 01/02/2019
 * Time: 10:28 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

using CommonUtilities;
using EAOfficeAPI;

namespace Actions.LogsOperations
{
	/// <summary>
	/// Description of GenerateReportFromRunId.
	/// </summary>
	public class GenerateReportFromRunId : ActionBase
	{
		public GenerateReportFromRunId()
		{
		}

		private string FModuleName = "LogsOperations";
		
		private int RunId = 0;
		
		public static string Execute(int MasterId,
			RequestCommonItem CommonItem,
			JObject DataObject,
			APISettingsItem SettingsItem,
			out string ErrorMsg)
		{
			string Method = "LogsLookup - Execute";
			string Result = "";
			ErrorMsg = "";

			GenerateReportFromRunId inst = new GenerateReportFromRunId();
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
			
			LogOnInsideMethod(Method);
			try 
			{
				RunId = StringUtilities.StrToIntDef((string)DataObject["runId"], 0);
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
			string ErrorMsg = "";
			
			LogOnInsideMethod(Method);
			try
			{
				string ConnectionString = CommonDBMethods.Common.ConnectionString(APISettings);
				if (!string.IsNullOrEmpty(ConnectionString)) 
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent("ConnectionString: " + ConnectionString, FModuleName, MasterId);
					string QueryText = "SELECT EC_ID, EC_Base64, EC_EventRunId, EC_FileExtension FROM EAEventRunBase64 WHERE EC_EventRunId = {0} ORDER BY EC_ID";
					QueryText = string.Format(QueryText, RunId);
					WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText: " + Environment.NewLine + QueryText, FModuleName, MasterId);
					DataTable tbl = FirebirdUtilities.GetFirstDataTable(FbConnectionString, QueryText);
					if (tbl == null) {
						ErrorMsg = 
							"Error Executing Query" + Environment.NewLine +
							ErrorMsg + Environment.NewLine +
							"QueryText:" + Environment.NewLine + QueryText;
						WindowsEventLoggingUtilities.WriteToInfoEvent(ErrorMsg, FModuleName, MasterId);
						Result = LogOnErrorSelectingData(Method, ErrorMsg, QueryText);
						return Result;
					}
					else if (tbl.Rows.Count == 0) {
						ErrorMsg = "No rows returned in the table to process";
						Result = LogOnErrorSelectingData(Method, ErrorMsg, QueryText);
						return Result;
					}
					
					// We now have the query to call for the Portfolio Search Results
					Result = ProcessTableContents(tbl);
				}
				//SELECT * FROM ea_LedgerList("")
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
		
		private string ProcessTableContents(DataTable Source)
		{
			string Base64String = "";
			string FileExt = "";
			string ECId = "";
			string RunId = "";
			string FileNameOnly = "";
			string OutputFileName = "";
			string OutputLocation = "";
			string Str = "";
			
			object Obj = null;
			
			StringBuilder sb = new StringBuilder();
			sb.Append(CommonUtilities.JsonUtilities.StartJson());
				
	        sb.Append(CommonUtilities.JsonUtilities.StartJsonArray("items"));
	        
	        List<string> RowList = new List<string>();
	        foreach(DataRow ARow in Source.Rows)
	        {
	        	Obj = ARow["EC_FileExtension"];
	        	FileExt = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
	        	Obj = ARow["EC_ID"];
	        	ECId = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
	        	Obj = ARow["EC_EventRunId"];
	        	RunId = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
	        	FileNameOnly = "{0}-{1}{2}";
	        	FileNameOnly = string.Format(FileNameOnly, RunId, ECId, FileExt);
	        	OutputLocation = @"\Docs\" + FileNameOnly;
	        	OutputFileName = @"e:\EAOffice\Development" + OutputLocation;
	        	WindowsEventLoggingUtilities.WriteToInfoEvent("OutputFileName" + Environment.NewLine + OutputFileName, FModuleName, MasterId);
	        	Obj = ARow["EC_Base64"];
	        	/*
	        	Base64String = (Obj == DBNull.Value) ? "" : Convert.ToString(Obj);
	        	Base64String = Base64String.Replace("\n", "").Replace("\r", "");
	        	while ((Base64String.Length % 4) > 0) Base64String += "=";
	        	WindowsEventLoggingUtilities.WriteToInfoEvent("Base64String" + Environment.NewLine + Base64String + "____", FModuleName, MasterId);	        	
	        	File.WriteAllBytes(OutputFileName, Convert.FromBase64String(Base64String));
	        	*/
	        	//byte[] toBytes = (byte[])ARow["EC_Base64"];
	        	byte[] toBytes = (byte[])Obj;
	        	Base64String = System.Text.Encoding.Default.GetString(toBytes);
	        	File.WriteAllBytes(OutputFileName, Convert.FromBase64String(Base64String));	        	
	        	Str =
	        		CommonUtilities.JsonUtilities.StartJsonObject("") +
	        		CommonUtilities.JsonUtilities.AddPropertyNameAndValue("file", OutputFileName) +
	        		CommonUtilities.JsonUtilities.AddPropertyNameAndValue("href", OutputLocation, false) +
	        		CommonUtilities.JsonUtilities.EndJsonObject();
	        	RowList.Add(Str);
	        }
	        
			sb.Append(string.Join(",", RowList.ToArray()));
	        sb.AppendLine(CommonUtilities.JsonUtilities.EndJsonArray());
			sb.Append(CommonUtilities.JsonUtilities.EndJson());
			return sb.ToString();
		}
	}
}
		