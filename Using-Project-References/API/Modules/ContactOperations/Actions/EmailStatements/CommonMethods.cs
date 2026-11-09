/*
 * Created by SharpDevelop.
 * User: John
 * Date: 21/08/2019
 * Time: 3:06 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Data;
using System.Runtime.Hosting;
using System.Text;
using System.Collections.Generic;
using System.Collections.Specialized;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CommonUtilities;
using EAOfficeAPI;

namespace Actions.ContactOperations.EmailStatements
{
	/// <summary>
	/// Description of CommonMethods.
	/// </summary>
	public class CommonMethods
	{
		public CommonMethods()
		{
		}
		
		public static string LoadRunIdRequestItem(ActionBase AActionBase, 
		                                          ref ContactOperations. EmailStatements.RunIdRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadRunIdRequestItem";
			object Obj = null;
			try 
			{
				Obj = AActionBase.GetCollectionValue("Data", "runId");
				TempItem.RunIdValue = (Obj == null) ? "" : Convert.ToString(Obj);
			}
			catch (Exception Ex)
			{
				Result =
					Method + Environment.NewLine +
					Ex.Message + Environment.NewLine +
					Ex.StackTrace;
			}
			return Result;
		}
		
		public static string LoadProcessRunRequestItem(JObject DataObject,
		                                               ref EmailStatements.ProcessRunRequestItem TempItem)
		{
			string Result = "";
			string Method = "LoadStatementCancelPreviewRequestItem";
			try 
			{
				TempItem.RunIdValue = (string)DataObject["runId"];
				TempItem.EventRunIdValue = (string)DataObject["eventRunId"];

				TempItem.EmailOptionValue = (string)DataObject["emailOption"];
				TempItem.TemplateIdValue = (string)DataObject["templateId"];
				TempItem.ReportIdValue = (string)DataObject["reportId"];
				TempItem.Subject = (string)DataObject["subject"];
				TempItem.EmailBody = (string)DataObject["emailBody"];
				TempItem.FromValue = (string)DataObject["from"];
				TempItem.CCValue = (string)DataObject["cc"];
				TempItem.BCCValue = (string)DataObject["bcc"];
				TempItem.ExclusionsValue = (string)DataObject["print"];
				TempItem.SignatureIdValue = (string)DataObject["emailSignatureId"];
				TempItem.EventRunSource = (string)DataObject["eventRunSource"];
			}
			catch (Exception Ex)
			{
				Result =
					Method + Environment.NewLine +
					Ex.Message + Environment.NewLine +
					Ex.StackTrace;
			}
			return Result;
		}
		
		public static string UpdateEmailBodyForEventRunId(string ConnectionString,
		                                                  Int64 EventRunId,
		                                                  string EmailBody)
		{
			string Result = "";
			string Method = "UpdateEmailBodyForEventRunId";
			string QueryText = "UPDATE EAEventRun SET EVA_EmailBody = @EmailBody WHERE EVA_ID = @EventRunId";
			List<FbParameterItem> Params = new List<FbParameterItem>();
			byte[] EmailBodyArray = Encoding.ASCII.GetBytes(EmailBody);
			Params.Add(new FbParameterItem("EmailBody", EmailBodyArray));
			Params.Add(new FbParameterItem("EventRunId", EventRunId));
			Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, Method, 0);
			
			try 
			{
				if (string.IsNullOrEmpty(Result))
				{
					Result = FirebirdUtilities.DBExecute(ConnectionString, QueryText, Params, 0);
				}				
			}
			catch (Exception Ex)
			{
				Result =
					Method + Environment.NewLine +
					Ex.Message + Environment.NewLine +
					Ex.StackTrace;
			}
			
			return Result;
		}
	}
}
