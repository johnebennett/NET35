/*
 * Created by SharpDevelop.
 * User: John
 * Date: 08/06/2020
 * Time: 3:28 PM
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
	/// Description of ResBondsApplyChanges.
	/// </summary>
	public class ResBondsApplyChanges : ActionBase
	{
		private string FModuleName = "Portfolio";
		private  ResBondsApplyChangesRequestItem FRequestItem;
		
		public static string Execute(int MasterId,
									 APISettingsItem SettingsItem,
									 string Request,
									 out string ErrorMsg)
		{
			string Method = "Execute";
			string Result = "";
			ErrorMsg = "";

			ResBondsApplyChanges inst = new ResBondsApplyChanges();
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
				FRequestItem = new ResBondsApplyChangesRequestItem();
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
			StringBuilder Entries = new StringBuilder();
			if (string.IsNullOrEmpty(ErrorMsg))
			{
				ErrorMsg =
					CommonMethods.Portfolio_ResBondsApplyChanges(
						FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue,
						FRequestItem.PortfolioId, FRequestItem.CategoryOnlyId, FRequestItem.LodgedAmount,
						FRequestItem.LodgementNumber, FRequestItem.LodgedOn, FRequestItem.BondNumber,
						FRequestItem.Guarantee);
			}
			
			if (string.IsNullOrEmpty(ErrorMsg))
			{
				Entries.Append(JsonUtilities.AddPropertyNameAndValue("PortfolioId", FRequestItem.PortfolioId));
				ErrorMsg =
					Actions.ComboBoxEntries.CommonMethods.BuildOutput_PortfolioResBondsList(
						FbConnectionString, CommonItem.CompanyIdValue, CommonItem.ClientIdValue, FRequestItem.PortfolioId, ref Entries);
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
			return Result;
		}
	}
}