/*
 * Created by SharpDevelop.
 * User: John
 * Date: 27/03/2019
 * Time: 12:52 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CommonUtilities;
using EAOfficeAPI;

namespace Actions.ContactOperations
{
	/// <summary>
	/// Update the EAEventRun record for the provided EventRunId
	/// </summary>
	public class AddEmailDocumentAttachment : ActionBase
	{
		private string FModuleName = "ContactOperations";
		
		private string FRunId = "";
		private int FRunIdValue = 0;
		private string FDocumentId = "";
		private int FDocumentIdValue = 0;

		public static string Execute(int MasterId,
		                             RequestCommonItem CommonItem,
		                             JObject DataObject,
		                             APISettingsItem SettingsItem,
		                             out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			AddEmailDocumentAttachment inst = new AddEmailDocumentAttachment();
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
				FRunId = (string)DataObject["runId"];
				FRunIdValue = StringUtilities.StrToIntDef(FRunId, -1);
				FDocumentId = (string)DataObject["documentId"];
				FDocumentIdValue = StringUtilities.StrToIntDef(FDocumentId, -1);
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
				if (string.IsNullOrEmpty(Result) && (FRunIdValue < 0)) Result = "Invalid RunId provided";
				if (string.IsNullOrEmpty(Result) && (FDocumentIdValue < 0)) Result = "Invalid DocumentId provided";
				

				if (!string.IsNullOrEmpty(Result))
				{
					WindowsEventLoggingUtilities.WriteToInfoEvent("Validation Failed: " + Result, FModuleName, MasterId);
					Result =
						JsonUtilities.StartJson() +
						JsonUtilities.AddPropertyNameAndValue("Error", "Validation Failed", false) +
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
			
			List<string> ErrorList = new List<string>();
			
			LogOnInsideMethod(Method);
			try
			{
				WindowsEventLoggingUtilities.WriteToInfoEvent("Inside BuildOutput", FModuleName, MasterId);
				
				StringBuilder entries = new StringBuilder();
				entries.AppendLine(JsonUtilities.StartJson());
					
				/*
				ErrorMsg = BuildOutputEntries(ref entries);				
				if (!string.IsNullOrEmpty(ErrorMsg)) return "";
				
				// create an eventrun record				
				EventRunId =
					UsefulDBMethods.EventHelpers.AddEmailEvent(
						this.FbConnectionString, CommonItem.CompanyIdValue, FReportEventIdValue, "Contact-Email", 
						CommonItem.ClientIdValue, out ErrorMsg);
				if (!string.IsNullOrEmpty(ErrorMsg)) return "";				
				
				// pass the eventrunId in the results set
				entries.Append(",");
				entries.AppendLine(JsonUtilities.AddPropertyNameAndValue("EventRunId", EventRunId, false));
				
				// create the xemail_eventrunId_ table
				ErrorMsg = CommonMethods.CreateXEmailTableForRunId(this.FbConnectionString, EventRunId);
				if (!string.IsNullOrEmpty(ErrorMsg)) return "";
				*/
				
				/*
				 * Convert FileName to base64
				 * Get next EAEMAILATTACHMENT_GEN value
				 * insert record for the above gen value
				 * get list of records for the provided runId
				*/
				
				//int NextId = 
				/*
				Byte[] bytes = null;
				string fileContents = "";
				
				foreach (string FileItem in FFileList)
				{
					Result = "";
					bytes = File.ReadAllBytes(FileItem);
					fileContents = Convert.ToBase64String(bytes);
				*/
				int NextId = 
						UsefulDBMethods.GeneratorHelpers.GetNextGeneratorId(
							FbConnectionString, "EAEMAILATTACHMENT_GEN", CommonItem.CompanyIdValue, CommonItem.ClientIdValue, out Result);
					if (!string.IsNullOrEmpty(Result))
					{
						Result = LogOnProcessingError(Method, Result, JsonUtilities.AddPropertyNameAndValue("attachment", NextId.ToString(), false));
					}
					
					if (string.IsNullOrEmpty(Result))
					{
						Result = AddEmailAttachment(FbConnectionString, NextId, FRunIdValue);
						if (!string.IsNullOrEmpty(Result))
						{
							Result = LogOnProcessingError(Method, Result, JsonUtilities.AddPropertyNameAndValue("attachment", NextId.ToString(), false));
						}
					}
					
					if (!string.IsNullOrEmpty(Result))
					{
						ErrorList.Add(Result);
					}
				//}
				
				if (ErrorList.Count != 0)
				{
					entries.AppendLine(JsonUtilities.StartJsonArray("Errors"));
					entries.Append(string.Join(",", ErrorList.ToArray()));
					entries.AppendLine(JsonUtilities.EndJsonArray());
				}
				else
				{
					//StringBuilder entries = new StringBuilder();
					Result = CommonMethods.BuildEmailAttachmentList(FbConnectionString, FRunIdValue, ref entries);
				}

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
		
		private string AddEmailAttachment(string ConnectionString, 
		                                  int NewId,
		                                  int RunId)
		{
			string Result = "";
			
			string QueryText = 
				"INSERT INTO EAEmailAttachments(EA_ID, EA_CompanyClientId, EA_EventRunId, EA_DOCUMENTID, EA_Source) " +
				"VALUES (@Id, @CompanyClientId, @EventRunId, @DocumentId, @Source)";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("@Id", NewId));
			
			Params.Add(new FbParameterItem("@CompanyClientId", CommonItem.CompanyIdValue));
			Params.Add(new FbParameterItem("@EventRunId", RunId));
			
			Params.Add(new FbParameterItem("@DocumentId", FDocumentIdValue));
			Params.Add(new FbParameterItem("@Source", "Contact-Email"));
			
			WindowsEventLoggingUtilities.WriteToInfoEvent("Before calling DBExecute", FModuleName, MasterId);
			Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
			WindowsEventLoggingUtilities.WriteToInfoEvent("After calling DBExecute", FModuleName, MasterId);
			WindowsEventLoggingUtilities.WriteToInfoEvent("Result: " + Result, FModuleName, MasterId);
			
			return Result;
		}
	}
}
