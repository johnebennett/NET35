/*
 * Created by SharpDevelop.
 * User: John
 * Date: 27/03/2020
 * Time: 12:32 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.ComponentModel;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CommonUtilities;
using EAOfficeAPI;
using Newtonsoft.Json.Serialization;
using UsefulDBMethods;

namespace Actions.Portfolio
{
	/// <summary>
	/// Description of AddPortfolioComment.
	/// </summary>
	public class AddPortfolioComment : ActionBase
	{
		private string FModuleName = "Portfolio";
		private  AddPortfolioCommentRequestItem FRequestItem;
		
		public static string Execute(int MasterId,
									 APISettingsItem SettingsItem,
									 string Request,
									 out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			AddPortfolioComment inst = new AddPortfolioComment();
			if (!inst.Setup(MasterId, SettingsItem, Request, out ErrorMsg))
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
				UsefulDBMethods.Helpers.RunEAMainOfficeProcess(inst.FbConnectionString, inst.CommonItem.CompanyIdValue);
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
				FRequestItem = new AddPortfolioCommentRequestItem();
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
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside: " +Method, FModuleName, MasterId);
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
			LogOnInsideMethod(Method);
			try 
			{
				Result = BuildOutput(out ErrorMsg);
				if (!string.IsNullOrEmpty(ErrorMsg))
					WindowsEventLoggingUtilities.WriteToErrorEvent(ErrorMsg, Method, MasterId);
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
		
		private string BuildOutput(out string ErrorMsg)
		{
			ErrorMsg = "";
			string Method = "BuildOutput";
			string Result = "";
			
			LogOnInsideMethod(Method);
			try
			{
				StringBuilder Entries = new StringBuilder();
				ErrorMsg =
					CommonMethods.Portfolio_EANotesAddGroup(
						FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue, FRequestItem.PortfolioId,
						FRequestItem.CategoryOnlyId, FRequestItem.NoteToAdd, FRequestItem.DefaultArrears, ref Entries);
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					if ((FRequestItem.AddNote == "Y") && (FRequestItem.NoteToAdd != ""))
					{
						// get the update script from the Entries set
						// execute the script
						ErrorMsg = ExecuteUpdateScript(Entries);
						// if successful add the section to the Entries for processing back on the page
						string Str = ErrorMsg;
						if (string.IsNullOrEmpty(Str)) Str = "Successful";
						Entries.Append(",");
						Entries.Append(JsonUtilities.AddPropertyNameAndValue("AddNoteResult", Str, false));
					}
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Result =
						JsonUtilities.StartJson() +
						Entries.ToString() +
						JsonUtilities.EndJson();
				}
				else
				{
					Result = LogOnProcessingError(Method, ErrorMsg, "");
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
			
			return Result;
		}
		
		private string ExecuteUpdateScript(StringBuilder Entries)
		{
			string Result = "";
			string Method = "ExecuteUpdateScript";
			WindowsEventLoggingUtilities.WriteToInfoEvent("Inside " + Method, FModuleName, MasterId);
					
			LogOnInsideMethod(Method);
			try 
			{
				string Str =
					JsonUtilities.StartJson() +
					Entries.ToString() +
					JsonUtilities.EndJson();
				JObject JObj = null;
				if (Entries.Length > 0)
				{
					JObj = JObject.Parse(Str);
				}
				
				string QueryText = "";
				if (string.IsNullOrEmpty(Result) && (JObj != null))
				{
					QueryText = (string)JObj["updatesqlstatement"];
					WindowsEventLoggingUtilities.WriteToInfoEvent("QueryText: " + Environment.NewLine + QueryText, FModuleName, MasterId);
					if (string.IsNullOrEmpty(QueryText)) Result = "Cannot determine the QueryText in " + Method;
				}
				
				List<FbParameterItem> Params = new List<FbParameterItem>();
				if (string.IsNullOrEmpty(Result) && !string.IsNullOrEmpty(QueryText))
				{
					// replace the parameter in the udpate script
					//Params.Add(new FbParameterItem("NoteText", FRequestItem.NoteToAdd));
					QueryText = QueryText.Replace("__", "'");
					Result = FirebirdUtilities.DBExecute(FbConnectionString, QueryText, null, 0);
					Str = (Result == "") ? "No Issue" : Result;
					WindowsEventLoggingUtilities.WriteToInfoEvent("Execution Result: " + Environment.NewLine + Str, FModuleName, MasterId);
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