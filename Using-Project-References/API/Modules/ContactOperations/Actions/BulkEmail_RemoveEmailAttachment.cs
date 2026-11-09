/*
 * Created by SharpDevelop.
 * User: John
 * Date: 18/12/2019
 * Time: 12:26 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Diagnostics;

using Newtonsoft.Json.Linq;
using CommonUtilities;
using EAOfficeAPI;

namespace Actions.ContactOperations
{
	/// <summary>
	/// Description of BulkEmail_RemoveEmailAttachment.
	/// </summary>
	public class BulkEmail_RemoveEmailAttachment : ActionBase
	{
		private string FModuleName = "ContactOperations";
		private string FRunId = "";
		private int FRunIdValue = 0;
		private int FEventRunIdValue = 0;
		private string FSource = "";
		private string FRecordId = "";
		private int FRecordIdValue = 0;
		
		public static string Execute(int MasterId,
									 APISettingsItem SettingsItem,
									 string Request,
									 out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			BulkEmail_RemoveEmailAttachment inst = new BulkEmail_RemoveEmailAttachment();
			if (!inst.Setup(MasterId, SettingsItem, Request, out ErrorMsg))
			{
				WindowsEventLoggingUtilities.WriteToInfoEvent("ErrorMsg: " + ErrorMsg, Method, MasterId);
				return "";
			}
			
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
				FRunId = (string)DataObject["runId"];
				FRunIdValue = StringUtilities.StrToIntDef(FRunId, -1);
				FSource = (string)DataObject["source"];
				FRecordId = (string)DataObject["recordId"];
				FRecordIdValue = StringUtilities.StrToIntDef(FRecordId, -1);
				
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
				if (string.IsNullOrEmpty(Result) && (FRunIdValue < 0)) Result = "Invalid RunId provided";
				if (string.IsNullOrEmpty(Result) && (FRecordIdValue < 0)) Result = "Invalid RecordId provided";				
				// NO VALIDATION on the FSource value
				
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
				
				ErrorMsg = CommonMethods.BulkEmail_GetEventRunId(this, FRunIdValue, FSource, out FEventRunIdValue);
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg = CommonMethods.RemoveAnEmailAttachment(this, FRecordIdValue);
					if (!string.IsNullOrEmpty(ErrorMsg))
					{
						ErrorList.Add(ErrorMsg);
					}
				}
				
				if (ErrorList.Count != 0)
				{
					entries.AppendLine(JsonUtilities.StartJsonArray("Errors"));
					entries.Append(string.Join(",", ErrorList.ToArray()));
					entries.AppendLine(JsonUtilities.EndJsonArray());
				}
				else
				{
					//StringBuilder entries = new StringBuilder();
					Result = CommonMethods.BuildEmailAttachmentList(FbConnectionString, FEventRunIdValue, ref entries);
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
		                                  int RunId,
		                                  string FileName, 
		                                  string Base64FileContents)
		{
			string Result = "";
			
			string QueryText = 
				"INSERT INTO EAEmailAttachments(EA_ID, EA_CompanyClientId, EA_EventRunId, EA_AttachmentName, EA_FileExtension, EA_Source, EA_Base64) " +
				"VALUES (@Id, @CompanyClientId, @EventRunId, @AttachmentName, @FileExtension, @Source, @Base64Content)";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			Params.Add(new FbParameterItem("@Id", NewId));
			
			Params.Add(new FbParameterItem("@CompanyClientId", CommonItem.CompanyIdValue));
			Params.Add(new FbParameterItem("@EventRunId", RunId));
			
			string AttachmentName = Path.GetFileNameWithoutExtension(FileName);
			Params.Add(new FbParameterItem("@AttachmentName", AttachmentName));
			
			string FileExtension = Path.GetExtension(FileName);
			Params.Add(new FbParameterItem("@FileExtension", FileExtension));
			
			Params.Add(new FbParameterItem("@Source", "Contact-Email"));
			
			var byteArray = Convert.FromBase64String(Base64FileContents);
			Params.Add(new FbParameterItem("@Base64Content", byteArray));

			WindowsEventLoggingUtilities.WriteToInfoEvent("Before calling DBExecute", FModuleName, MasterId);
			Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
			WindowsEventLoggingUtilities.WriteToInfoEvent("After calling DBExecute", FModuleName, MasterId);
			WindowsEventLoggingUtilities.WriteToInfoEvent("Result: " + Result, FModuleName, MasterId);
			
			return Result;
		}
	}
}
