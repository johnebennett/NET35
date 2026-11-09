/*
 * Created by SharpDevelop.
 * User: John
 * Date: 04/08/2020
 * Time: 10:23 AM
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

using CommonUtilities;
using EAOfficeAPI;
namespace Actions.PortfolioAddButton
{
	/// <summary>
	/// Description of RelationGetDisplayValues.
	/// </summary>
	public class RelationGetDisplayValues : ActionBase
	{
		private string FModuleName = "PortfolioAddButton";
		private string FRelationIdValue = "";
		private int FRelationId = 0;
		
		public static string Execute(int MasterId,
		                             APISettingsItem SettingsItem,
		                             string Request,
		                             out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			RelationGetDisplayValues inst = new RelationGetDisplayValues();
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
				this.FRelationIdValue = (string)GetCollectionValue("Data", "RelationId");
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
                int Dummy = 0;
                bool IsValid = int.TryParse(FRelationIdValue, out Dummy);
                FRelationId = (IsValid) ? Dummy : 0;
                if (!IsValid) Result = "RelationId is Not Provided or Invalid";
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
				string DisplayType = "";
				int DisplayId = 0;
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					ErrorMsg =
						UsefulDBMethods.Universal.RelationGetDisplayType(
							FbConnectionString, CommonItem.CompanyIdValue, FRelationId, out DisplayType, out DisplayId);
				}
				
				if (string.IsNullOrEmpty(ErrorMsg))
				{
					Entries.Append(JsonUtilities.AddPropertyNameAndValue("DisplayType", DisplayType));
					Entries.Append(JsonUtilities.AddPropertyNameAndValue("DisplayId", DisplayId, false));
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
				Result = LogOnException(Method, Ex, "");
			}
			finally
			{
				LogOnLeavingMethod(Method);
				WindowsEventLoggingUtilities.WriteToInfoEvent("Leaving: " + Method, FModuleName, MasterId);
			}
			return Result;
		}
	}
}