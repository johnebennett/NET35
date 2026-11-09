/*
 * Created by SharpDevelop.
 * User: John
 * Date: 25/07/2019
 * Time: 10:03 AM
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

namespace Actions.PaymentOperations
{
	/// <summary>
	/// Description of NewPaymentPreviewStartAgain.
	/// </summary>
	public class NewPaymentPreviewStartAgain : ActionBase
	{
		private string FModuleName = "PaymentOperations";
		private NewPaymentPreviewStartAgainRequestItem FRequestItem;
		
		public static string Execute(int MasterId,
		                             RequestCommonItem CommonItem,
		                             JObject DataObject,
		                             APISettingsItem SettingsItem,
		                             out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			NewPaymentPreviewStartAgain inst = new NewPaymentPreviewStartAgain();
			if (!inst.Setup(MasterId, CommonItem, DataObject, SettingsItem, out ErrorMsg))
				return "";
			inst.FRequestItem = new NewPaymentPreviewStartAgainRequestItem();
			
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
				Result = FRequestItem.Load((ActionBase)this, DataObject);
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
				/*
				 * call the stored procedure to clear the list for the runId
				 * call the BuildOutputEntries method
				*/
				ErrorMsg = UsefulDBMethods.Helpers.RunEAMainOfficeProcess(FbConnectionString, CommonItem.CompanyIdValue);
				if (!string.IsNullOrEmpty(ErrorMsg)) return "";
				
				ErrorMsg = StatementClearList();
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					StringBuilder Entries = new StringBuilder();
					Entries.AppendLine(JsonUtilities.StartJson());
					ErrorMsg = BuildOutputEntries(ref Entries);
					Entries.AppendLine(JsonUtilities.EndJson());

					if (!string.IsNullOrEmpty(ErrorMsg)) return "";
					
					Result = Entries.ToString();
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
		
		private string BuildOutputEntries(ref StringBuilder Entries)
		{
			string Method = "BuildOutputEntries";
			string Result = "";			
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, FModuleName, MasterId);
			
			LogOnInsideMethod(Method);
			try
			{
				// remove this
				Result = 
					CommonMethods.BuildNewPaymentPreviewGridResults(
						(ActionBase)this, FbConnectionString, CommonItem.CompanyIdValue, FRequestItem.RunId, "rows", ref Entries);
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
		
		private string StatementClearList()
		{
			string Result = "";
			string Method = "StatementClearList";
			
			LogOnInsideMethod(Method);
			try 
			{
				string QueryText = "EXECUTE PROCEDURE EASTATEMENTCLEARLIST(@CompanyId,@RunId)";
				List<FbParameterItem> Params = new List<FbParameterItem>();
				Params.Add(new FbParameterItem("CompanyId", CommonItem.CompanyIdValue));
				Params.Add(new FbParameterItem("RunId", FRequestItem.RunId));
				Result = UsefulDBMethods.Helpers.WriteParamsToInfoEvent(QueryText, Params, FModuleName, MasterId);
				if (!string.IsNullOrEmpty(Result)) return Result;
				
				Result = FirebirdUtilities.DBExecute(FbConnectionString, QueryText, Params, 0);
			}					
			catch (Exception Ex)
			{
				Result = LogOnException(Method, Ex, "");
				return Result;
			}
			finally
			{
				LogOnLeavingMethod(Method);
			}
			
			return Result;
		}
	}
}
