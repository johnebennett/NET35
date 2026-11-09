/*
 * Created by SharpDevelop.
 * User: John
 * Date: 20/03/2019
 * Time: 4:53 PM
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

namespace Actions.ContactOperations
{
	/// <summary>
	/// Description of RemoveRecordFromList.
	/// </summary>
	public class RemoveEmailRecipient : ActionBase
	{
		private string FModuleName = "ContactOperations";
		
		private string FRunId = "";
		private int FRunIdValue = 0;
		private string FRecordId = "";
		private int FRecordIdValue = 0;
		private string FClientId = "";
		private int FClientIdValue = 0;

		public static string Execute(int MasterId,
		                             RequestCommonItem CommonItem,
		                             JObject DataObject,
		                             APISettingsItem SettingsItem,
		                             out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			RemoveEmailRecipient inst = new RemoveEmailRecipient();
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
				FRecordId = (string)DataObject["recordId"];
				FRecordIdValue = StringUtilities.StrToIntDef(FRecordId, -1);
				FClientId = (string)DataObject["clientId"];
				FClientIdValue = StringUtilities.StrToIntDef(FClientId, -1);
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
				if (string.IsNullOrEmpty(Result) && (FRunIdValue <= 0)) Result = "Invalid RunId Value provided";
				if (string.IsNullOrEmpty(Result) && (FRecordIdValue <= 0)) Result = "Invalid Record ID Value provided";
				if (string.IsNullOrEmpty(Result) && (FClientIdValue < 0)) Result = "Invalid Client ID Value provided";
				
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
			
			LogOnInsideMethod(Method);
			try
			{
				WindowsEventLoggingUtilities.WriteToInfoEvent("Inside BuildOutput", FModuleName, MasterId);
				
				string QueryText = "DELETE FROM XEMAIL" + FRunId + " WHERE ID = @Id";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("@Id", FRecordId));
				WindowsEventLoggingUtilities.WriteToInfoEvent("Before calling DBExecute", FModuleName, MasterId);
				Result = FirebirdUtilities.DBExecute(FbConnectionString, QueryText, Params, 0);
				WindowsEventLoggingUtilities.WriteToInfoEvent("After calling DBExecute", FModuleName, MasterId);
				WindowsEventLoggingUtilities.WriteToInfoEvent("Result: " + Result, FModuleName, MasterId);
				
				if (!string.IsNullOrEmpty(Result))
				{
					Result = LogOnProcessingError(Method, Result, JsonUtilities.AddPropertyNameAndValue("Error", Result, false));;
				}
				else
				{
					StringBuilder Entries = new StringBuilder();
					Entries.AppendLine(JsonUtilities.StartJson());
					Result = CommonMethods.BuildXEmailRecipientList(FbConnectionString, FRunIdValue, ref Entries);
					Entries.AppendLine(JsonUtilities.EndJson());

					if (!string.IsNullOrEmpty(Result))
					{
						WindowsEventLoggingUtilities.WriteToInfoEvent("After BuildXEmailRecipientList Result: " + Result, FModuleName, MasterId);
						Result = LogOnProcessingError(Method, Result, "");
					}
					else
					{
						Result = Entries.ToString();
						WindowsEventLoggingUtilities.WriteToInfoEvent("After BuildXEmailRecipientList Entries: " + Environment.NewLine + Entries.ToString(), FModuleName, MasterId);
					}
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
