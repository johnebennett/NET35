/*
 * Created by SharpDevelop.
 * User: John
 * Date: 19/08/2019
 * Time: 6:41 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Collections.Specialized;
//using Actions.ContactOperations.EmailStatements;

/*
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
*/

using CommonUtilities;
using EAOfficeAPI;

//namespace Actions.EmailStatements.ContactOperations
namespace Actions.ContactOperations.EmailStatements
{
	/// <summary>
	/// Description of SetupPageEntries.
	/// </summary>
	public class SetupPageEntries : ActionBase
	{
		private string FModuleName = "ContactOperations.EmailStatements";
		private EmailStatements.RunIdRequestItem FRequestItem;
		
		/*
		public static string Execute(int MasterId,
									 RequestCommonItem CommonItem,
									 JObject DataObject,
									 APISettingsItem SettingsItem,
									 out string ErrorMsg)
		*/
		public static string Execute(int MasterId,
									 APISettingsItem SettingsItem,
									 string Request,
									 out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			ContactOperations.EmailStatements.SetupPageEntries inst = new SetupPageEntries();
			//if (!inst.Setup(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg))
			WindowsEventLoggingUtilities.WriteToInfoEvent("Request:" + Environment.NewLine + Request, Method, MasterId);
			if (!inst.Setup(MasterId, SettingsItem, Request, out ErrorMsg))
				return ErrorMsg;
			
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
				FRequestItem = new RunIdRequestItem();
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
				StringBuilder Entries = new StringBuilder();
				BuildOutputEntries(ref Entries, out ErrorMsg);
				int EventRunId = 0;
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					// create an eventrun record				
					EventRunId =
						UsefulDBMethods.GeneratorHelpers.GetNextGeneratorId(
							FbConnectionString, "EVA_ID_GEN", CommonItem.CompanyIdValue, CommonItem.ClientIdValue, out ErrorMsg);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Entries.Append(",");
					Entries.Append(JsonUtilities.AddPropertyNameAndValue("EventRunId", EventRunId, false));
					
					string TodayStr = MiscUtilities.DateTimeToYYYYMMDD(DateTime.Now);
					ErrorMsg =
						UsefulDBMethods.EventHelpers.ExecuteEventRunCreate(
							FbConnectionString, EventRunId, CommonItem.CompanyIdValue, 0, 0, 0, CommonItem.ClientIdValue, 0, "", "", "", 0, "N", "N",
							0, "", "", "", "", "", //"Contact-EmailStatements", 
							"N", // forMyself
							0, // ForMyselfPdfOrXml
							0, FRequestItem.RunId, 0, 0, TodayStr, TodayStr, "", 0);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					// get the entries for the recipients for the grid display
					if (Entries.Length > 0) Entries.Append(",");
					ErrorMsg = GetEventRunShowClientsAll(EventRunId, ref Entries);
				}

				if (!string.IsNullOrEmpty(ErrorMsg))
				{
					Result = LogOnProcessingError(Method, ErrorMsg, "");
				}
				else
				{
					/*
					Result += ",";
					Result += JsonUtilities.AddPropertyNameAndValue("EventRunId", EventRunId, false);
					Result =
						JsonUtilities.StartJson() +
						Result +
						JsonUtilities.EndJson();
					*/
					Result =
						JsonUtilities.StartJson() +
						Entries.ToString() +
						JsonUtilities.EndJson();
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
		
		/*		
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
					
				ErrorMsg = BuildOutputEntries(out ErrorMsg);				
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
		*/
		
		private void BuildOutputEntries(ref StringBuilder Entries,
		                                out string ErrorMsg)
		{
			string Method = "BuildOutputEntries";
			//string Result = "";			
			ErrorMsg = "";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, FModuleName, MasterId);
			
			LogOnInsideMethod(Method);
			try
			{
				string QueryText = "SELECT * FROM Init_Contact_EmailStmts(@CompanyId, @ClientId, @RunId)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("@CompanyId", CommonItem.CompanyIdValue));
				Params.Add(new FbParameterItem("@ClientId", CommonItem.ClientIdValue));
				Params.Add(new FbParameterItem("@RunId", FRequestItem.RunId));
				ErrorMsg = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
	
				DataTable tbl = null;
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					tbl =
						UsefulDBMethods.Helpers.GetFirstDataTableEx(
							FbConnectionString, QueryText, Params, 0, UsefulDBMethods.FirebirdQueryCheckTypes.HasTables, out ErrorMsg);				
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{				
					foreach (DataRow dataRow in tbl.Rows)
					{
						Entries.AppendLine((dataRow[0] == DBNull.Value) ? "" : Convert.ToString(dataRow[0]));
					}
					//Result = Entries.ToString();
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
		
			//return Result;
		}
		
		private String GetEventRunShowClientsAll(Int64 EventRunId, 
		                                         ref StringBuilder Entries)
		{
			string Result = "";
			string Method = "GetEventRunShowClientsAll";
			
			LogOnInsideMethod(Method);
			try 
			{
				string QueryText = 
					" SELECT" +
					"   Z20_Client AS ZL20_Client," +
					"   Z20_Property AS ZL20_Property," +
					"   ZR10_Payment AS M_Payment," +
					"   I_Client_Id," +
					"   I_Id" +
					" FROM EventRunShowClientsAll(@EventRunId)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("@EventRunId", EventRunId));
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
					Result = JsonUtilities.BuildJsonArrayFromDataTable(tbl, "gridEntries", ref Entries);
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
